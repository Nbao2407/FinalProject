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