-- Hệ thống Quản lý Vật liệu
-- Script SQL bao gồm các stored procedure, trigger và cài đặt dữ liệu ban đầu

-- Stored Procedure: Thêm chi tiết hóa đơn
ALTER PROCEDURE sp_ThemChiTietHoaDon
    @MaHoaDon INT,        -- Mã hóa đơn
    @MaVatLieu INT,       -- Mã vật liệu
    @SoLuong INT          -- Số lượng
AS
BEGIN
    -- Khai báo biến để lưu giá và tổng tiền
    DECLARE @DonGia DECIMAL(18,2);
    DECLARE @ThanhTien DECIMAL(18,2);

    -- Lấy giá bán của vật liệu
    SELECT @DonGia = DonGiaBan FROM QLVatLieu WHERE MaVatLieu = @MaVatLieu;

    -- Tính tổng tiền và làm tròn
    SET @ThanhTien = dbo.fn_LamTronTien(@SoLuong * @DonGia, 0);

    -- Thêm chi tiết hóa đơn
    INSERT INTO ChiTietHoaDon (MaHoaDon, MaVatLieu, SoLuong, DonGia)
    VALUES (@MaHoaDon, @MaVatLieu, @SoLuong, @DonGia);

    -- Cập nhật tổng tiền hóa đơn và làm tròn
    UPDATE QLHoaDon
    SET TongTien = dbo.fn_LamTronTien(TongTien + @ThanhTien, 0)
    WHERE MaHoaDon = @MaHoaDon;
END;
GO

-- Hàm làm tròn tiền tệ
CREATE FUNCTION dbo.fn_LamTronTien (
    @Gia DECIMAL(18,2),      -- Số tiền cần làm tròn
    @SoLamTron INT = 2       -- Số chữ số thập phân (mặc định 2)
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    RETURN ROUND(@Gia, @SoLamTron)
END;
GO

-- Trigger cập nhật số lượng khi nhập hàng
CREATE TRIGGER trg_CapNhatSoLuongNhap
ON ChiTietDonNhap
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Cập nhật số lượng vật liệu trong kho
    UPDATE QLVatLieu
    SET SoLuong = v.SoLuong + i.SoLuong
    FROM QLVatLieu v
    INNER JOIN inserted i ON v.MaVatLieu = i.MaVatLieu;
END;
GO

-- Trigger kiểm tra và cập nhật số lượng khi bán hàng
CREATE TRIGGER trg_KiemTraSoLuongBan
ON ChiTietHoaDon
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra xem số lượng yêu cầu có vượt quá số lượng trong kho không
    IF EXISTS (
        SELECT 1
        FROM QLVatLieu v
        INNER JOIN inserted i ON v.MaVatLieu = i.MaVatLieu
        WHERE v.SoLuong < i.SoLuong
    )
    BEGIN
        -- Báo lỗi nếu không đủ hàng trong kho
        RAISERROR (N'Số lượng vật liệu trong kho không đủ!', 16, 1);
        ROLLBACK TRANSACTION;
    END
    ELSE
    BEGIN
        -- Cập nhật số lượng kho sau khi bán hàng thành công
        UPDATE QLVatLieu
        SET SoLuong = v.SoLuong - i.SoLuong
        FROM QLVatLieu v
        INNER JOIN inserted i ON v.MaVatLieu = i.MaVatLieu;
    END;
END;
GO
ALTER TABLE QLKH
    ADD TrangThai NVARCHAR(20) CHECK (TrangThai IN (N'Hoạt động', N'Vô hiệu hóa')) DEFAULT N'Hoạt động';
	Go
	UPDATE QLKH
SET TrangThai = N'Hoạt động'
WHERE TrangThai IS NULL;
GO
GO
-- Stored Procedure: Thêm vật liệu mới
CREATE PROCEDURE sp_ThemVatLieu
    @Ten NVARCHAR(100),       -- Tên vật liệu
    @Loai INT,                -- Loại vật liệu
    @DonGiaNhap DECIMAL(18,2),-- Giá nhập
    @DonGiaBan DECIMAL(18,2), -- Giá bán
    @DonViTinh NVARCHAR(50),  -- Đơn vị tính
    @MaKho INT               -- Mã kho
AS
BEGIN
    -- Thêm vật liệu mới với số lượng ban đầu là 0 và trạng thái hoạt động
    INSERT INTO QLVatLieu (Ten, Loai, DonGiaNhap, DonGiaBan, DonViTinh, SoLuong, MaKho, TrangThai)
    VALUES (@Ten, @Loai, @DonGiaNhap, @DonGiaBan, @DonViTinh, 0, @MaKho, N'Hoạt động');
END;
GO
-- Stored Procedure: Vô hiệu hóa khách hàng (disable) với kiểm tra hóa đơn chờ thanh toán
Create PROCEDURE sp_DisableKhachHang
    @MaKhachHang INT,
    @NguoiCapNhat INT
AS
BEGIN
    BEGIN TRY
        -- Kiểm tra khách hàng có tồn tại
        IF NOT EXISTS (SELECT 1 FROM QLKH WHERE MaKhachHang = @MaKhachHang)
            THROW 50003, N'Khách hàng không tồn tại!', 1;

        -- Kiểm tra xem khách hàng có hóa đơn "Chờ thanh toán" hay không
        IF EXISTS (
            SELECT 1 
            FROM QLHoaDon 
            WHERE MaKhachHang = @MaKhachHang 
            AND TrangThai = N'Chờ thanh toán'
        )
            THROW 50004, N'Không thể vô hiệu hóa khách hàng vì còn hóa đơn "Chờ thanh toán"!', 1;

        -- Thêm cột TrangThai nếu chưa có
        IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                      WHERE TABLE_NAME = 'QLKH' AND COLUMN_NAME = 'TrangThai')
        BEGIN
            ALTER TABLE QLKH
            ADD TrangThai NVARCHAR(20) CHECK (TrangThai IN (N'Hoạt động', N'Ngừng sử dụng')) DEFAULT N'Hoạt động';
        END

        -- Cập nhật trạng thái thành 'Vô hiệu hóa'
        UPDATE QLKH
        SET TrangThai = N'Ngừng sử dụng',
            NguoiTao = @NguoiCapNhat,
            NgayTao = GETDATE()
        WHERE MaKhachHang = @MaKhachHang;

        -- Ghi log hoạt động
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
        VALUES (GETDATE(), @NguoiCapNhat, 'QLKH', @MaKhachHang, N'Ngừng sử dụng', N'Hoạt động', N'Ngừng sử dụng', N'Khách hàng bị vô hiệu hóa');

        SELECT N'Vô hiệu hóa khách hàng thành công!' AS Message;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Stored Procedure: Xóa khách hàng với kiểm tra hóa đơn chờ thanh toán
Alter PROCEDURE sp_XoaKhachHang
    @MaKhachHang INT,
    @NguoiCapNhat INT
AS
BEGIN
    BEGIN TRY
        -- Kiểm tra khách hàng có tồn tại
        IF NOT EXISTS (SELECT 1 FROM QLKH WHERE MaKhachHang = @MaKhachHang)
            THROW 50003, N'Khách hàng không tồn tại!', 1;

        -- Kiểm tra xem khách hàng có hóa đơn "Chờ thanh toán" hay không
        IF EXISTS (
            SELECT 1 
            FROM QLHoaDon 
            WHERE MaKhachHang = @MaKhachHang 
            AND TrangThai = N'Chờ thanh toán'
        )
            THROW 50005, N'Không thể xóa khách hàng vì còn hóa đơn "Chờ thanh toán"!', 1;

        -- Xóa thông tin khách hàng nhưng giữ lại MaKhachHang trong lịch sử giao dịch
        DELETE FROM QLKH
        WHERE MaKhachHang = @MaKhachHang;

        -- Ghi log hoạt động
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
        VALUES (GETDATE(), @NguoiCapNhat, 'QLKH', @MaKhachHang, N'Xóa', N'Có dữ liệu', N'Đã xóa', N'Xóa khách hàng nhưng giữ lịch sử giao dịch');

        SELECT N'Xóa khách hàng thành công! Lịch sử giao dịch vẫn được giữ lại.' AS Message;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Ví dụ kiểm tra:
-- Thêm một hóa đơn "Chờ thanh toán" để thử nghiệm
INSERT INTO QLHoaDon (NgayLap, MaTKLap, MaKhachHang, TongTien, TrangThai)
VALUES (GETDATE(), 1, 1, 500000, N'Chờ thanh toán');
GO

-- Thử vô hiệu hóa khách hàng
EXEC sp_DisableKhachHang 
    @MaKhachHang = 1, 
    @NguoiCapNhat = 1;
-- Kết quả: Sẽ báo lỗi "Không thể vô hiệu hóa khách hàng vì còn hóa đơn 'Chờ thanh toán'!"

-- Thử xóa khách hàng
EXEC sp_XoaKhachHang 
    @MaKhachHang = 1, 
    @NguoiCapNhat = 1;
-- Kết quả: Sẽ báo lỗi "Không thể xóa khách hàng vì còn hóa đơn 'Chờ thanh toán'!"

-- Cập nhật hóa đơn thành "Đã thanh toán" để thử lại
UPDATE QLHoaDon
SET TrangThai = N'Đã thanh toán'
WHERE MaKhachHang = 1;

-- Thử lại vô hiệu hóa/xóa sau khi hóa đơn đã được xử lý
EXEC sp_DisableKhachHang 
    @MaKhachHang = 1, 
    @NguoiCapNhat = 1;
-- Kết quả: Thành công

EXEC sp_XoaKhachHang 
    @MaKhachHang = 1, 
    @NguoiCapNhat = 1;
-- Kết quả: Thành công
-- Cài đặt dữ liệu ban đầu

-- Chèn tài khoản người dùng
INSERT INTO QLTK (TenDangNhap, MatKhau, Email, ChucVu, TrangThai)
VALUES 
    (N'admin', N'123456', N'admin@example.com', N'Quản lý', N'Hoạt động'),
    (N'nhanvien1', N'abc123', N'nhanvien1@example.com', N'Nhân viên', N'Hoạt động');
GO

-- Chèn thông tin kho
INSERT INTO Kho (TenKho)
VALUES 
    (N'Kho Chính'), 
    (N'Kho Phụ');
GO

-- Chèn nhà cung cấp
INSERT INTO NCC (TenNCC, DiaChi, SDT, Email)
VALUES 
    (N'Công ty A', N'123 Đường ABC, TP.HCM', N'0909123456', N'ncc_a@example.com'),
    (N'Công ty B', N'456 Đường XYZ, Hà Nội', N'0912345678', N'ncc_b@example.com');
GO

-- Chèn loại vật liệu
INSERT INTO QLLoaiVatLieu (TenLoai)
VALUES 
    (N'Xi măng'), 
    (N'Cát'), 
    (N'Gạch'), 
    (N'Sắt thép');
GO

-- Chèn khách hàng
INSERT INTO QLKH (Ten, NgaySinh, GioiTinh, SDT, Email)
VALUES 
    (N'Nguyễn Văn A', '1990-01-01', N'Nam', '0987654321', N'vana@example.com'),
    (N'Trần Thị B', '1995-05-10', N'Nữ', '0976543210', N'thib@example.com');
GO
-- Stored Procedure cập nhật mật khẩu với mã hóa
CREATE PROCEDURE sp_CapNhatMatKhau
    @MaTK INT,
    @MatKhauMoi NVARCHAR(255)
AS
BEGIN
    -- Mã hóa mật khẩu bằng SHA-256
    DECLARE @MatKhauHash NVARCHAR(255);
    SET @MatKhauHash = CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', @MatKhauMoi), 2);

    -- Cập nhật mật khẩu đã mã hóa
    UPDATE QLTK
    SET MatKhau = @MatKhauHash
    WHERE MaTK = @MaTK;

    -- Ghi log hành động
    INSERT INTO AuditLog (MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
    SELECT 
        @MaTK,
        N'QLTK',
        @MaTK,
        N'Cập nhật mật khẩu',
        MatKhau,
        @MatKhauHash,
        N'Đổi mật khẩu thành công'
    FROM QLTK
    WHERE MaTK = @MaTK;
END;
GO

-- Stored Procedure kiểm tra đăng nhập
CREATE PROCEDURE sp_KiemTraDangNhap
    @TenDangNhap NVARCHAR(50),
    @MatKhau NVARCHAR(255)
AS
BEGIN
    DECLARE @MatKhauHash NVARCHAR(255);
    SET @MatKhauHash = CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', @MatKhau), 2);

    SELECT 
        MaTK,
        TenDangNhap,
        Email,
        ChucVu,
        TrangThai
    FROM QLTK
    WHERE TenDangNhap = @TenDangNhap 
    AND MatKhau = @MatKhauHash 
    AND TrangThai = N'Hoạt động';
END;
GO
-- Stored Procedure tạo token reset mật khẩu
CREATE PROCEDURE sp_TaoTokenResetMatKhau
    @Email NVARCHAR(100),
    @Token NVARCHAR(255) OUTPUT
AS
BEGIN
    DECLARE @MaTK INT;
    DECLARE @NgayHetHan DATETIME;

    -- Kiểm tra email tồn tại
    SELECT @MaTK = MaTK
    FROM QLTK
    WHERE Email = @Email;

    IF @MaTK IS NULL
    BEGIN
        RAISERROR (N'Email không tồn tại trong hệ thống!', 16, 1);
        RETURN;
    END

    -- Tạo token ngẫu nhiên (trong thực tế, nên dùng GUID hoặc cách tạo token an toàn hơn)
    SET @Token = NEWID();
    SET @NgayHetHan = DATEADD(HOUR, 24, GETDATE()); -- Token hết hạn sau 24 giờ

    -- Thêm token vào bảng
    INSERT INTO PasswordResetToken (MaTK, Token, NgayHetHan)
    VALUES (@MaTK, @Token, @NgayHetHan);

    -- Ghi log
    INSERT INTO AuditLog (MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
    VALUES (@MaTK, N'PasswordResetToken', @MaTK, N'Tạo token reset', N'Yêu cầu reset mật khẩu');
END;
GO

-- Stored Procedure xác nhận và reset mật khẩu
CREATE PROCEDURE sp_ResetMatKhau
    @Token NVARCHAR(255),
    @MatKhauMoi NVARCHAR(255)
AS
BEGIN
    DECLARE @MaTK INT;
    DECLARE @MatKhauHash NVARCHAR(255);

    -- Kiểm tra token hợp lệ
    SELECT @MaTK = MaTK
    FROM PasswordResetToken
    WHERE Token = @Token
    AND NgayHetHan > GETDATE()
    AND DaSuDung = 0;

    IF @MaTK IS NULL
    BEGIN
        RAISERROR (N'Token không hợp lệ hoặc đã hết hạn!', 16, 1);
        RETURN;
    END

    -- Mã hóa mật khẩu mới
    SET @MatKhauHash = CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', @MatKhauMoi), 2);

    -- Cập nhật mật khẩu
    UPDATE QLTK
    SET MatKhau = @MatKhauHash
    WHERE MaTK = @MaTK;

    -- Đánh dấu token đã sử dụng
    UPDATE PasswordResetToken
    SET DaSuDung = 1
    WHERE Token = @Token;

    -- Ghi log
    INSERT INTO AuditLog (MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
    VALUES (@MaTK, N'QLTK', @MaTK, N'Reset mật khẩu', N'Reset mật khẩu thành công');
END;
GO
-- Stored Procedure lấy vai trò người dùng
CREATE PROCEDURE sp_LayVaiTroNguoiDung
    @MaTK INT
AS
BEGIN
    SELECT 
        MaTK,
        TenDangNhap,
        ChucVu,
        TrangThai
    FROM QLTK
    WHERE MaTK = @MaTK;
END;
GO
ALTER PROCEDURE [dbo].[sp_KiemTraDangNhap]
    @TenDangNhap NVARCHAR(50) = NULL,
    @Email NVARCHAR(255) = NULL,
    @MatKhau NVARCHAR(255)
AS
BEGIN
    DECLARE @MatKhauHash NVARCHAR(255);

    SET @MatKhauHash = CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', @MatKhau), 2);

    SELECT 
        MaTK,
        TenDangNhap,
        Email,
        ChucVu,
        TrangThai
    FROM QLTK
    WHERE (TenDangNhap = @TenDangNhap OR Email = @Email)
    AND MatKhau = @MatKhauHash 
    AND TrangThai = N'Hoạt động';
END;
Go
ALTER TRIGGER trg_KiemTraSoLuongBan
ON ChiTietHoaDon
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra xem số lượng yêu cầu có vượt quá số lượng trong kho không
    IF EXISTS (
        SELECT 1
        FROM QLVatLieu v
        INNER JOIN inserted i ON v.MaVatLieu = i.MaVatLieu
        WHERE v.SoLuong < i.SoLuong
    )
    BEGIN
        -- Báo lỗi nếu không đủ hàng trong kho
        RAISERROR (N'Số lượng vật liệu trong kho không đủ!', 16, 1);
        ROLLBACK TRANSACTION;
    END
    ELSE
    BEGIN
        -- Cập nhật số lượng kho sau khi bán hàng thành công
        UPDATE QLVatLieu
        SET SoLuong = v.SoLuong - i.SoLuong
        FROM QLVatLieu v
        INNER JOIN inserted i ON v.MaVatLieu = i.MaVatLieu;
    END;
END;
GO
Create PROCEDURE sp_HuyHoaDon
    @MaHoaDon INT,
    @NguoiCapNhat INT,
    @LyDoHuy NVARCHAR(255)
AS
BEGIN
    BEGIN TRY
        -- Kiểm tra hóa đơn có tồn tại không
        IF NOT EXISTS (SELECT 1 FROM QLHoaDon WHERE MaHoaDon = @MaHoaDon)
            THROW 50001, N'Hóa đơn không tồn tại!', 1;

        -- Kiểm tra trạng thái hóa đơn
        IF EXISTS (SELECT 1 FROM QLHoaDon WHERE MaHoaDon = @MaHoaDon AND TrangThai = N'Đã hủy')
            THROW 50002, N'Hóa đơn đã bị hủy trước đó!', 1;

        IF EXISTS (SELECT 1 FROM QLHoaDon WHERE MaHoaDon = @MaHoaDon AND TrangThai = N'Đã thanh toán')
            THROW 50003, N'Không thể hủy hóa đơn đã thanh toán!', 1;

        -- Cập nhật số lượng vật liệu trong kho (hoàn lại số lượng đã bán)
        UPDATE QLVatLieu
        SET SoLuong = v.SoLuong + cthd.SoLuong
        FROM QLVatLieu v
        INNER JOIN ChiTietHoaDon cthd ON v.MaVatLieu = cthd.MaVatLieu
        WHERE cthd.MaHoaDon = @MaHoaDon;

        -- Cập nhật trạng thái hóa đơn thành "Đã hủy"
        UPDATE QLHoaDon
        SET TrangThai = N'Đã hủy',
            NgayCapNhat = GETDATE(),
            NguoiCapNhat = @NguoiCapNhat
        WHERE MaHoaDon = @MaHoaDon;

        -- Ghi log hoạt động với lý do hủy
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
        VALUES (GETDATE(), @NguoiCapNhat, 'QLHoaDon', @MaHoaDon, N'Hủy', N'Chờ thanh toán', N'Đã hủy', @LyDoHuy);

        SELECT N'Hủy hóa đơn thành công!' AS Message;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO
CREATE PROCEDURE sp_CapNhatHoaDon
    @MaHoaDon INT,
    @NgayLap DATE,
    @MaTKLap INT,
    @MaKhachHang INT,
    @TongTien DECIMAL(18,2),
    @TrangThai NVARCHAR(50),
    @HinhThucThanhToan NVARCHAR(50),
    @NguoiCapNhat INT
AS
BEGIN
    UPDATE QLHoaDon
    SET NgayLap = @NgayLap,
        MaTKLap = @MaTKLap,
        MaKhachHang = @MaKhachHang,
        TongTien = @TongTien,
        TrangThai = @TrangThai,
        HinhThucThanhToan = @HinhThucThanhToan,
        NgayCapNhat = GETDATE(),
        NguoiCapNhat = @NguoiCapNhat
    WHERE MaHoaDon = @MaHoaDon;

    -- Ghi log
    INSERT INTO AuditLog (MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
    VALUES (@NguoiCapNhat, N'QLHoaDon', @MaHoaDon, N'Cập nhật', N'Cập nhật thông tin hóa đơn');
END;
GO
CREATE PROCEDURE sp_XoaChiTietHoaDon
    @MaHoaDon INT,
    @MaVatLieu INT
AS
BEGIN
    BEGIN TRY
        -- Kiểm tra chi tiết hóa đơn có tồn tại không
        IF NOT EXISTS (SELECT 1 FROM ChiTietHoaDon WHERE MaHoaDon = @MaHoaDon AND MaVatLieu = @MaVatLieu)
            THROW 50001, N'Chi tiết hóa đơn không tồn tại!', 1;

        -- Lấy số lượng để hoàn lại kho
        DECLARE @SoLuong INT;
        SELECT @SoLuong = SoLuong FROM ChiTietHoaDon WHERE MaHoaDon = @MaHoaDon AND MaVatLieu = @MaVatLieu;

        -- Hoàn lại số lượng vào kho
        UPDATE QLVatLieu
        SET SoLuong = SoLuong + @SoLuong
        WHERE MaVatLieu = @MaVatLieu;

        -- Xóa chi tiết hóa đơn
        DELETE FROM ChiTietHoaDon
        WHERE MaHoaDon = @MaHoaDon AND MaVatLieu = @MaVatLieu;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO
CREATE PROCEDURE sp_CapNhatChiTietHoaDon
    @MaHoaDon INT,
    @MaVatLieu INT,
    @SoLuong INT
AS
BEGIN
    BEGIN TRY
        -- Kiểm tra chi tiết hóa đơn có tồn tại không
        IF NOT EXISTS (SELECT 1 FROM ChiTietHoaDon WHERE MaHoaDon = @MaHoaDon AND MaVatLieu = @MaVatLieu)
            THROW 50001, N'Chi tiết hóa đơn không tồn tại!', 1;

        -- Lấy số lượng hiện tại
        DECLARE @SoLuongCu INT;
        SELECT @SoLuongCu = SoLuong FROM ChiTietHoaDon WHERE MaHoaDon = @MaHoaDon AND MaVatLieu = @MaVatLieu;

        -- Cập nhật số lượng trong kho
        UPDATE QLVatLieu
        SET SoLuong = SoLuong + (@SoLuongCu - @SoLuong)
        WHERE MaVatLieu = @MaVatLieu;

        -- Cập nhật chi tiết hóa đơn
        UPDATE ChiTietHoaDon
        SET SoLuong = @SoLuong
        WHERE MaHoaDon = @MaHoaDon AND MaVatLieu = @MaVatLieu;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO
CREATE PROCEDURE sp_XoaHoaDonTam
    @MaHoaDon INT,
    @NguoiCapNhat INT
AS
BEGIN
    BEGIN TRY
        -- Bắt đầu transaction để đảm bảo tính toàn vẹn dữ liệu
        BEGIN TRANSACTION;

        -- Kiểm tra hóa đơn có tồn tại không
        IF NOT EXISTS (SELECT 1 FROM QLHoaDon WHERE MaHoaDon = @MaHoaDon)
            THROW 50001, N'Hóa đơn không tồn tại!', 1;

        -- Kiểm tra trạng thái hóa đơn (chỉ cho phép xóa hóa đơn tạm - Chờ thanh toán)
        IF NOT EXISTS (SELECT 1 FROM QLHoaDon WHERE MaHoaDon = @MaHoaDon AND TrangThai = N'Chờ thanh toán')
            THROW 50002, N'Chỉ có thể xóa hóa đơn đang ở trạng thái "Chờ thanh toán"!', 1;

        -- Hoàn lại số lượng vật liệu trong kho nếu có chi tiết hóa đơn
        IF EXISTS (SELECT 1 FROM ChiTietHoaDon WHERE MaHoaDon = @MaHoaDon)
        BEGIN
            UPDATE QLVatLieu
            SET SoLuong = v.SoLuong + cthd.SoLuong
            FROM QLVatLieu v
            INNER JOIN ChiTietHoaDon cthd ON v.MaVatLieu = cthd.MaVatLieu
            WHERE cthd.MaHoaDon = @MaHoaDon;

            -- Xóa chi tiết hóa đơn
            DELETE FROM ChiTietHoaDon
            WHERE MaHoaDon = @MaHoaDon;
        END

        -- Xóa hóa đơn
        DELETE FROM QLHoaDon
        WHERE MaHoaDon = @MaHoaDon;

        -- Ghi log hoạt động
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
        VALUES (GETDATE(), @NguoiCapNhat, N'QLHoaDon', @MaHoaDon, N'Xóa tạm', N'Chờ thanh toán', N'Đã xóa', N'Xóa hóa đơn tạm khi đóng form');

        -- Commit transaction nếu thành công
        COMMIT TRANSACTION;

        SELECT N'Xóa hóa đơn tạm thành công!' AS Message;
    END TRY
    BEGIN CATCH
        -- Rollback nếu có lỗi
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Ném lỗi ra ngoài để xử lý
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO
CREATE PROCEDURE sp_TimKiemKhachHang
    @MaKhachHang INT = NULL,        -- Mã khách hàng (có thể NULL)
    @Ten NVARCHAR(100) = NULL       -- Tên khách hàng (có thể NULL)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        kh.MaKhachHang,
        kh.Ten,
        kh.NgaySinh,
        kh.GioiTinh,
        kh.SDT,
        kh.Email,
        kh.DiaChi,
        kh.TenNguoiTao,
        tk.TenDangNhap AS NguoiTao,
        kh.NgayTao,
        kh.TrangThai
    FROM QLKH kh
    LEFT JOIN QLTK tk ON kh.NguoiTao = tk.MaTK
    WHERE 
        -- Tìm theo MaKhachHang nếu được cung cấp
        (@MaKhachHang IS NULL OR kh.MaKhachHang = @MaKhachHang)
        AND 
        -- Tìm theo Ten nếu được cung cấp (tìm kiếm gần đúng)
        (@Ten IS NULL OR kh.Ten LIKE '%' + @Ten + '%')
        -- Chỉ lấy khách hàng có trạng thái hợp lệ
        AND kh.TrangThai IN (N'Hoạt động')
    ORDER BY kh.NgayTao DESC;
END;
GO
ALTER PROCEDURE sp_ThemVatLieu
    @Ten NVARCHAR(100),
    @Loai INT,
    @DonGiaNhap DECIMAL(18,2),
    @DonGiaBan DECIMAL(18,2),
    @DonViTinh NVARCHAR(50),
    @MaKho INT,
    @HinhAnh VARBINARY(MAX) = NULL, -- Hình ảnh có thể null
    @GhiChu NVARCHAR(255) = NULL    -- Ghi chú có thể null
AS
BEGIN
    INSERT INTO QLVatLieu (Ten, Loai, DonGiaNhap, DonGiaBan, DonViTinh, SoLuong, MaKho, TrangThai, HinhAnh, GhiChu)
    VALUES (@Ten, @Loai, @DonGiaNhap, @DonGiaBan, @DonViTinh, 0, @MaKho, N'Hoạt động', @HinhAnh, @GhiChu);
END;
GO
Alter PROCEDURE sp_CapNhatVatLieu
    @MaVatLieu INT,
    @Ten NVARCHAR(100),
    @Loai INT,
    @DonGiaNhap DECIMAL(18,2),
    @DonGiaBan DECIMAL(18,2),
    @DonViTinh NVARCHAR(50),
    @HinhAnh VARBINARY(MAX) = NULL,
    @GhiChu NVARCHAR(255) = NULL,
    @NguoiCapNhat INT
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM QLVatLieu WHERE MaVatLieu = @MaVatLieu)
            THROW 50001, N'Vật tư không tồn tại!', 1;

        UPDATE QLVatLieu
        SET Ten = @Ten,
            Loai = @Loai,
            DonGiaNhap = @DonGiaNhap,
            DonGiaBan = @DonGiaBan,
            DonViTinh = @DonViTinh,
            HinhAnh = @HinhAnh,
            GhiChu = @GhiChu,
            NgayCapNhat = GETDATE(),
            NguoiCapNhat = @NguoiCapNhat
        WHERE MaVatLieu = @MaVatLieu;

        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @NguoiCapNhat, N'QLVatLieu', @MaVatLieu, N'Cập nhật', N'Cập nhật thông tin vật tư');
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO
CREATE PROCEDURE sp_XoaVatLieu
    @MaVatLieu INT,
    @NguoiCapNhat INT
AS
BEGIN
    BEGIN TRY
        -- Kiểm tra vật tư có tồn tại không
        IF NOT EXISTS (SELECT 1 FROM QLVatLieu WHERE MaVatLieu = @MaVatLieu)
            THROW 50001, N'Vật tư không tồn tại!', 1;

        -- Kiểm tra vật tư có trong đơn nhập hoặc hóa đơn không
        IF EXISTS (SELECT 1 FROM ChiTietDonNhap WHERE MaVatLieu = @MaVatLieu)
            OR EXISTS (SELECT 1 FROM ChiTietHoaDon WHERE MaVatLieu = @MaVatLieu)
            THROW 50002, N'Không thể xóa vật tư vì đã có trong đơn nhập hoặc hóa đơn!', 1;

        -- Xóa vật tư
        DELETE FROM QLVatLieu
        WHERE MaVatLieu = @MaVatLieu;

        -- Ghi log
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @NguoiCapNhat, N'QLVatLieu', @MaVatLieu, N'Xóa', N'Xóa vật tư');
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO
CREATE PROCEDURE sp_LayDanhSachDonViTinh
AS
BEGIN
    SELECT DISTINCT DonViTinh
    FROM QLVatLieu
    WHERE DonViTinh IS NOT NULL
    ORDER BY DonViTinh;
END;
GO
CREATE PROCEDURE sp_LayDanhSachLoaiVatLieu
AS
BEGIN
    SELECT MaLoaiVatLieu, TenLoai
    FROM QLLoaiVatLieu
    WHERE TrangThai = N'Hoạt động';
END;
GO
EXEC sp_TimKiemKH @Keyword = 'a'
Go
ALTER PROCEDURE [dbo].[sp_TimKiemKH]
    @Keyword NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Search NVARCHAR(100) = '%' + @Keyword + '%';
    SELECT 
        kh.MaKhachHang,
        kh.Ten,
        kh.GioiTinh, 
        kh.NgaySinh, 
        kh.SDT, 
        kh.Email,
		kh.TrangThai,
        CASE 
            WHEN kh.Ten LIKE @Keyword + '%' THEN 3  -- Tên khớp đầu tiên
            WHEN kh.Ten LIKE '%' + @Keyword + '%' THEN 2 -- Tên chứa từ khóa
            WHEN RIGHT(kh.SDT, 3) = @Keyword THEN 2  -- Khớp với 3 số cuối SĐT
            ELSE 1
        END AS MatchScore
    FROM QLKH kh  
    WHERE 
        kh.Ten COLLATE Latin1_General_CI_AI LIKE @Search
        OR kh.Email COLLATE Latin1_General_CI_AI LIKE @Search
		OR LEFT(kh.Email, CHARINDEX('@', kh.Email) - 1) LIKE @Search
        OR RIGHT(kh.SDT, 3) = @Keyword
        OR CAST(kh.MaKhachHang AS NVARCHAR) LIKE @Search
    ORDER BY MatchScore DESC, kh.Ten;
END;
Go
-- Stored Procedure: Thêm Nhà Cung Cấp
Alter PROCEDURE sp_ThemNCC
    @TenNCC NVARCHAR(100),
    @DiaChi NVARCHAR(255),
    @SDT NVARCHAR(15),
    @Email NVARCHAR(100),
    @NguoiTao int
AS
BEGIN
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM NCC WHERE Email = @Email)
            THROW 50001, N'Email đã tồn tại!', 1;

        IF @SDT NOT LIKE '[0-9]%' OR LEN(@SDT) NOT BETWEEN 10 AND 15
            THROW 50002, N'Số điện thoại không hợp lệ!', 1;

        -- Thêm NCC
        INSERT INTO NCC (TenNCC, DiaChi, SDT, Email, NguoiTao, TrangThai)
        VALUES (@TenNCC, @DiaChi, @SDT, @Email, @NguoiTao, N'Hoạt động');

        SELECT N'Thêm nhà cung cấp thành công!' AS Message;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Stored Procedure: Sửa Nhà Cung Cấp
ALTER PROCEDURE sp_CapNhatNCC
    @MaNCC INT,
    @TenNCC NVARCHAR(100),
    @DiaChi NVARCHAR(255),
    @SDT NVARCHAR(15),
    @Email NVARCHAR(100),
    @NguoiCapNhat NVARCHAR(256)
AS
BEGIN
    BEGIN TRY
        -- Debug giá trị SDT
        PRINT 'SDT nhận được: [' + @SDT + ']';
        PRINT 'Độ dài SDT: ' + CAST(LEN(@SDT) AS NVARCHAR(10));
        PRINT 'Ký tự đầu tiên: ' + LEFT(@SDT, 1);

        IF NOT EXISTS (SELECT 1 FROM NCC WHERE MaNCC = @MaNCC)
            THROW 50003, N'Nhà cung cấp không tồn tại!', 1;

        IF EXISTS (SELECT 1 FROM NCC WHERE Email = @Email AND MaNCC != @MaNCC)
            THROW 50001, N'Email đã tồn tại!', 1;

        IF @SDT NOT LIKE '[0-9]%' OR LEN(@SDT) NOT BETWEEN 10 AND 15
            THROW 50002, N'Số điện thoại không hợp lệ! Phải bắt đầu bằng số và có độ dài từ 10 đến 15 ký tự.', 1;

        UPDATE NCC
        SET TenNCC = @TenNCC,
            DiaChi = @DiaChi,
            SDT = @SDT,
            Email = @Email,
            NgayTao = GETDATE(),
            NguoiTao = @NguoiCapNhat
        WHERE MaNCC = @MaNCC;

        SELECT N'Cập nhật nhà cung cấp thành công!' AS Message;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO
GO

Alter PROCEDURE sp_XoaNCC
    @MaNCC INT,
    @NguoiCapNhat int
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM NCC WHERE MaNCC = @MaNCC)
            THROW 50003, N'Nhà cung cấp không tồn tại!', 1;

        IF EXISTS (SELECT 1 FROM QLDonNhap WHERE MaNCC = @MaNCC)
            THROW 50004, N'Không thể xóa nhà cung cấp vì đã có đơn nhập liên quan!', 1;

        -- Xóa NCC
        DELETE FROM NCC
        WHERE MaNCC = @MaNCC;

        -- Ghi log
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), NULL, N'NCC', @MaNCC, N'Xóa', N'Xóa nhà cung cấp bởi ' + @NguoiCapNhat);

        SELECT N'Xóa nhà cung cấp thành công!' AS Message;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO
CREATE PROCEDURE sp_CapNhatThongTinTK
    @MaTK INT,
    @TenDangNhap NVARCHAR(50),
    @Email NVARCHAR(100),
    @SDT NVARCHAR(15),
    @DiaChi NVARCHAR(255),
    @Avatar VARBINARY(MAX) = NULL, 
    @NguoiCapNhat INT
AS
BEGIN
    BEGIN TRY
        -- Kiểm tra tài khoản có tồn tại không
        IF NOT EXISTS (SELECT 1 FROM QLTK WHERE MaTK = @MaTK)
            THROW 50001, N'Tài khoản không tồn tại!', 1;

        -- Kiểm tra email hoặc tên đăng nhập đã tồn tại ở tài khoản khác chưa
        IF EXISTS (SELECT 1 FROM QLTK WHERE (Email = @Email OR TenDangNhap = @TenDangNhap) AND MaTK != @MaTK)
            THROW 50002, N'Tên đăng nhập hoặc email đã tồn tại!', 1;

        -- Kiểm tra định dạng SDT
        IF @SDT IS NOT NULL AND (@SDT NOT LIKE '[0-9]%' OR LEN(@SDT) NOT BETWEEN 10 AND 15)
            THROW 50003, N'Số điện thoại không hợp lệ!', 1;

        -- Cập nhật thông tin tài khoản
        UPDATE QLTK
        SET TenDangNhap = @TenDangNhap,
            Email = @Email,
            SDT = @SDT,
            DiaChi = @DiaChi,
            Avatar = ISNULL(@Avatar, Avatar), 
            NgayTao = GETDATE() 
        WHERE MaTK = @MaTK;

        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @NguoiCapNhat, N'QLTK', @MaTK, N'Cập nhật', N'Cập nhật thông tin tài khoản, bao gồm avatar');

        SELECT N'Cập nhật thông tin tài khoản thành công!' AS Message;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO
CREATE PROCEDURE sp_ThemTaiKhoan
    @TenDangNhap NVARCHAR(50),
    @MatKhau NVARCHAR(255),
    @Email NVARCHAR(100),
    @SDT NVARCHAR(15),
    @ChucVu NVARCHAR(50),
    @NguoiTao INT -- MaTK của người tạo
AS
BEGIN
    BEGIN TRY
        DECLARE @ChucVuNguoiTao NVARCHAR(50);
        
        -- Lấy chức vụ của người tạo
        SELECT @ChucVuNguoiTao = ChucVu 
        FROM QLTK 
        WHERE MaTK = @NguoiTao;

        -- Kiểm tra quyền
        IF @ChucVuNguoiTao != N'Quản lý' AND @ChucVuNguoiTao != N'Admin'
            THROW 50001, N'Không có quyền tạo tài khoản!', 1;

        IF @ChucVuNguoiTao = N'Quản lý' AND @ChucVu != N'Nhân viên'
            THROW 50002, N'Quản lý chỉ có thể tạo tài khoản nhân viên!', 1;

        -- Mã hóa mật khẩu
        DECLARE @MatKhauHash NVARCHAR(255) = CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', @MatKhau), 2);

        -- Thêm tài khoản
        INSERT INTO QLTK (TenDangNhap, MatKhau, Email, SDT, ChucVu, TrangThai, NguoiTao, NgayTao)
        VALUES (@TenDangNhap, @MatKhauHash, @Email, @SDT, @ChucVu, N'Hoạt động', @NguoiTao, GETDATE());

        -- Ghi log
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @NguoiTao, N'QLTK', SCOPE_IDENTITY(), N'Thêm', N'Tạo tài khoản mới');
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO
Alter PROCEDURE sp_XoaTaiKhoan
    @MaTK INT,              -- Mã tài khoản cần xóa
    @NguoiCapNhat INT       -- Người thực hiện xóa (thường là người dùng hệ thống)
AS
BEGIN
    BEGIN TRY
        -- Bắt đầu transaction để đảm bảo tính toàn vẹn dữ liệu
        BEGIN TRANSACTION;

        -- Kiểm tra tài khoản có tồn tại không
        IF NOT EXISTS (SELECT 1 FROM QLTK WHERE MaTK = @MaTK)
            THROW 50001, N'Tài khoản không tồn tại!', 1;

        -- Kiểm tra tài khoản có đang được tham chiếu trong các bảng khác không
        IF EXISTS (SELECT 1 FROM QLHoaDon WHERE MaTKLap = @MaTK)
            OR EXISTS (SELECT 1 FROM QLDonNhap WHERE MaTK = @MaTK)
            OR EXISTS (SELECT 1 FROM QLKH WHERE NguoiTao = @MaTK)
            OR EXISTS (SELECT 1 FROM QLVatLieu WHERE NguoiTao = @MaTK OR NguoiCapNhat = @MaTK)
            OR EXISTS (SELECT 1 FROM PasswordResetToken WHERE MaTK = @MaTK)
        BEGIN
            THROW 50002, N'Không thể xóa tài khoản vì đã có giao dịch hoặc dữ liệu liên quan!', 1;
        END

        -- Xóa token reset mật khẩu liên quan (nếu có)
        DELETE FROM PasswordResetToken
        WHERE MaTK = @MaTK;

        -- Xóa tài khoản
        DELETE FROM QLTK
        WHERE MaTK = @MaTK;

        -- Ghi log hoạt động
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
        VALUES (GETDATE(), @NguoiCapNhat, N'QLTK', @MaTK, N'Xóa', N'Có dữ liệu', N'Đã xóa', N'Xóa tài khoản khỏi hệ thống');

        -- Commit transaction nếu thành công
        COMMIT TRANSACTION;

        SELECT N'Xóa tài khoản thành công!' AS Message;
    END TRY
    BEGIN CATCH
        -- Rollback nếu có lỗi
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Ném lỗi ra ngoài để xử lý
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO
CREATE PROCEDURE sp_SuaTaiKhoan
    @MaTK INT,
    @TenDangNhap NVARCHAR(50),
    @Email NVARCHAR(100),
    @SDT NVARCHAR(15),
    @ChucVu NVARCHAR(50),
    @TrangThai NVARCHAR(20),
    @GhiChu NVARCHAR(255),
    @NguoiCapNhat INT
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM QLTK WHERE MaTK = @MaTK)
            THROW 50001, N'Tài khoản không tồn tại!', 1;

        UPDATE QLTK
        SET TenDangNhap = @TenDangNhap,
            Email = @Email,
            SDT = @SDT,
            ChucVu = @ChucVu,
            TrangThai = @TrangThai,
            GHICHU = @GhiChu
        WHERE MaTK = @MaTK;

        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @NguoiCapNhat, N'QLTK', @MaTK, N'Sửa', N'Cập nhật thông tin tài khoản');
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO
CREATE PROCEDURE sp_ThemTaiKhoan
    @TenDangNhap NVARCHAR(50),
    @MatKhau NVARCHAR(255),
    @Email NVARCHAR(100),
    @SDT NVARCHAR(15),
    @ChucVu NVARCHAR(50),
    @NguoiTao INT 
AS
BEGIN
    BEGIN TRY
        DECLARE @ChucVuNguoiTao NVARCHAR(50);
        
        -- Lấy chức vụ của người tạo
        SELECT @ChucVuNguoiTao = ChucVu 
        FROM QLTK 
        WHERE MaTK = @NguoiTao;

        -- Kiểm tra quyền
        IF @ChucVuNguoiTao != N'Quản lý' AND @ChucVuNguoiTao != N'Admin'
            THROW 50001, N'Không có quyền tạo tài khoản!', 1;

        IF @ChucVuNguoiTao = N'Quản lý' AND @ChucVu != N'Nhân viên'
            THROW 50002, N'Quản lý chỉ có thể tạo tài khoản nhân viên!', 1;

        -- Mã hóa mật khẩu
        DECLARE @MatKhauHash NVARCHAR(255) = CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', @MatKhau), 2);

        -- Thêm tài khoản
        INSERT INTO QLTK (TenDangNhap, MatKhau, Email, SDT, ChucVu, TrangThai, NguoiTao, NgayTao)
        VALUES (@TenDangNhap, @MatKhauHash, @Email, @SDT, @ChucVu, N'Hoạt động', @NguoiTao, GETDATE());

        -- Ghi log
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @NguoiTao, N'QLTK', SCOPE_IDENTITY(), N'Thêm', N'Tạo tài khoản mới');
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO
ALTER PROCEDURE [dbo].[sp_ThemTaiKhoan]
    @TenDangNhap NVARCHAR(50),
    @MatKhau NVARCHAR(255),
    @Email NVARCHAR(100),
    @SDT NVARCHAR(15),
    @ChucVu NVARCHAR(50),
	@GHICHU NVARCHAR(255),
    @NguoiTao INT 
AS
BEGIN
    BEGIN TRY
        DECLARE @ChucVuNguoiTao NVARCHAR(50);
        
        SELECT @ChucVuNguoiTao = ChucVu 
        FROM QLTK 
        WHERE MaTK = @NguoiTao;

        IF @ChucVuNguoiTao != N'Quản lý' AND @ChucVuNguoiTao != N'Admin'
            THROW 50001, N'Không có quyền tạo tài khoản!', 1;

        IF @ChucVuNguoiTao = N'Quản lý' AND @ChucVu != N'Nhân viên'
            THROW 50002, N'Quản lý chỉ có thể tạo tài khoản nhân viên!', 1;

        DECLARE @MatKhauHash NVARCHAR(255) = CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', @MatKhau), 2);

        INSERT INTO QLTK (TenDangNhap, MatKhau, Email, SDT, ChucVu, TrangThai,NguoiTao, GHICHU, NgayTao)
        VALUES (@TenDangNhap, @MatKhauHash, @Email, @SDT, @ChucVu, N'Hoạt động', @NguoiTao,@GHICHU, GETDATE());

        -- Ghi log
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @NguoiTao, N'QLTK', SCOPE_IDENTITY(), N'Thêm', N'Tạo tài khoản mới');
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
go
	CREATE PROCEDURE sp_NhapHang
    @NgayNhap DATE,                  -- Ngày nhập hàng
    @MaNCC INT,                      -- Mã nhà cung cấp
    @MaTK INT,                       -- Mã tài khoản người tạo đơn
    @GhiChu NVARCHAR(255) = NULL,    -- Ghi chú (không bắt buộc)
    @ChiTietNhap NVARCHAR(MAX),      -- Chuỗi JSON chứa danh sách chi tiết nhập hàng
    @NguoiCapNhat INT                -- Mã tài khoản người thực hiện
AS
BEGIN
    BEGIN TRY
        -- Bắt đầu transaction để đảm bảo tính toàn vẹn dữ liệu
        BEGIN TRANSACTION;

        -- Kiểm tra nhà cung cấp có tồn tại và hoạt động không
        IF NOT EXISTS (SELECT 1 FROM NCC WHERE MaNCC = @MaNCC AND TrangThai = N'Hoạt động')
            THROW 50001, N'Nhà cung cấp không tồn tại hoặc không hoạt động!', 1;

        -- Kiểm tra tài khoản có tồn tại và hoạt động không
        IF NOT EXISTS (SELECT 1 FROM QLTK WHERE MaTK = @MaTK AND TrangThai = N'Hoạt động')
            THROW 50002, N'Tài khoản không tồn tại hoặc bị khóa!', 1;

        -- Thêm đơn nhập hàng mới
        INSERT INTO QLDonNhap (NgayNhap, MaNCC, MaTK, TrangThai, GhiChu, NgayCapNhat, NguoiCapNhat)
        VALUES (@NgayNhap, @MaNCC, @MaTK, N'Hoàn thành', @GhiChu, GETDATE(), @NguoiCapNhat);

        -- Lấy mã đơn nhập vừa tạo
        DECLARE @MaDonNhap INT = SCOPE_IDENTITY();

        -- Tạo bảng tạm để lưu chi tiết nhập hàng từ JSON
        DECLARE @TempChiTiet TABLE (
            MaVatLieu INT,
            SoLuong INT,
            GiaNhap DECIMAL(18,2)
        );

        -- Parse JSON chi tiết nhập hàng vào bảng tạm
        INSERT INTO @TempChiTiet (MaVatLieu, SoLuong, GiaNhap)
        SELECT 
            JSON_VALUE(value, '$.MaVatLieu') AS MaVatLieu,
            JSON_VALUE(value, '$.SoLuong') AS SoLuong,
            JSON_VALUE(value, '$.GiaNhap') AS GiaNhap
        FROM OPENJSON(@ChiTietNhap);

        -- Kiểm tra dữ liệu chi tiết nhập hàng
        IF NOT EXISTS (SELECT 1 FROM @TempChiTiet)
            THROW 50003, N'Danh sách chi tiết nhập hàng trống!', 1;

        -- Kiểm tra các vật liệu có tồn tại và hợp lệ không
        IF EXISTS (
            SELECT 1 
            FROM @TempChiTiet t
            LEFT JOIN QLVatLieu v ON t.MaVatLieu = v.MaVatLieu
            WHERE v.MaVatLieu IS NULL OR v.TrangThai != N'Hoạt động'
        )
            THROW 50004, N'Có vật liệu không tồn tại hoặc không hoạt động!', 1;

        -- Kiểm tra số lượng và giá nhập
        IF EXISTS (SELECT 1 FROM @TempChiTiet WHERE SoLuong <= 0 OR GiaNhap < 0)
            THROW 50005, N'Số lượng hoặc giá nhập không hợp lệ!', 1;

        -- Thêm chi tiết đơn nhập vào bảng ChiTietDonNhap
        INSERT INTO ChiTietDonNhap (MaDonNhap, MaVatLieu, SoLuong, GiaNhap)
        SELECT @MaDonNhap, MaVatLieu, SoLuong, GiaNhap
        FROM @TempChiTiet;

        -- Cập nhật số lượng tồn kho trong bảng QLVatLieu
        UPDATE QLVatLieu
        SET SoLuong = v.SoLuong + t.SoLuong,
            DonGiaNhap = t.GiaNhap, -- Cập nhật giá nhập mới nhất
            NgayCapNhat = GETDATE(),
            NguoiCapNhat = @NguoiCapNhat
        FROM QLVatLieu v
        INNER JOIN @TempChiTiet t ON v.MaVatLieu = t.MaVatLieu;

        -- Ghi log hoạt động
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
        VALUES (GETDATE(), @NguoiCapNhat, N'QLDonNhap', @MaDonNhap, N'Thêm', NULL, NULL, N'Thêm đơn nhập hàng mới');

        -- Commit transaction nếu thành công
        COMMIT TRANSACTION;

        SELECT N'Nhập hàng thành công!' AS Message, @MaDonNhap AS MaDonNhap;
    END TRY
    BEGIN CATCH
        -- Rollback nếu có lỗi
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Ném lỗi ra ngoài để xử lý
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO
Alter PROCEDURE sp_TimKiemVatLieuNameID
    @Keyword NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Search NVARCHAR(100) = '%' + @Keyword + '%';

    SELECT 
        MaVatLieu,
        Ten,
        DonViTinh,
		DonGiaNhap,
        DonGiaBan,
        SoLuong
    FROM QLVatLieu
    WHERE 
        (Ten LIKE @Search OR CAST(MaVatLieu AS NVARCHAR) LIKE @Search)
        AND TrangThai = N'Hoạt động'
    ORDER BY Ten;
END;
GO

exec sp_NhapHang @NgayNhap='2025-04-07',@MaNCC=2,@MaTK=2,@GhiChu=N'hello',@ChiTietNhap=N'[{"MaDonNhap":0,"NgayNhap":"0001-01-01T00:00:00","NCC":null,"TrangThai":null,"MaVatLieu":6,"SoLuong":3,"GiaNhap":15800.50}]',@NguoiCapNhat=2
SELECT name, create_date, modify_date 
FROM sys.triggers 
WHERE parent_id = OBJECT_ID('ChiTietDonNhap');
DROP TRIGGER trg_CapNhatSoLuongNhap;