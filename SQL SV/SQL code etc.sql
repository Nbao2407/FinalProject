ALTER PROCEDURE sp_ThemChiTietHoaDon
    @MaHoaDon INT,
    @MaVatLieu INT,
    @SoLuong INT
AS
BEGIN
    DECLARE @DonGia DECIMAL(18,2);
    DECLARE @ThanhTien DECIMAL(18,2);

    SELECT @DonGia = DonGiaBan FROM QLVatLieu WHERE MaVatLieu = @MaVatLieu;

    SET @ThanhTien = dbo.fn_LamTronTien(@SoLuong * @DonGia, 0);

    INSERT INTO ChiTietHoaDon (MaHoaDon, MaVatLieu, SoLuong, DonGia)
    VALUES (@MaHoaDon, @MaVatLieu, @SoLuong, @DonGia);

    UPDATE QLHoaDon
    SET TongTien = dbo.fn_LamTronTien(TongTien + @ThanhTien, 0)
    WHERE MaHoaDon = @MaHoaDon;
END;
Go
CREATE FUNCTION dbo.fn_LamTronTien (@Gia DECIMAL(18,2), @SoLamTron INT = 2)
RETURNS DECIMAL(18,2)
AS
BEGIN
    RETURN ROUND(@Gia, @SoLamTron)
END;
GO

go
CREATE TRIGGER trg_CapNhatSoLuongNhap
ON ChiTietDonNhap
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE QLVatLieu
    SET SoLuong = v.SoLuong + i.SoLuong
    FROM QLVatLieu v
    INNER JOIN inserted i ON v.MaVatLieu = i.MaVatLieu;
END;
GO
CREATE TRIGGER trg_KiemTraSoLuongBan
ON ChiTietHoaDon
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM QLVatLieu v
        INNER JOIN inserted i ON v.MaVatLieu = i.MaVatLieu
        WHERE v.SoLuong < i.SoLuong
    )
    BEGIN
        RAISERROR (N'Số lượng vật liệu trong kho không đủ!', 16, 1);
        ROLLBACK TRANSACTION;
    END
    ELSE
    BEGIN
        UPDATE QLVatLieu
        SET SoLuong =v.SoLuong - i.SoLuong
        FROM QLVatLieu v
        INNER JOIN inserted i ON v.MaVatLieu = i.MaVatLieu;
    END;
END;
GO
CREATE PROCEDURE sp_ThemVatLieu
    @Ten NVARCHAR(100),
    @Loai INT,
    @DonGiaNhap DECIMAL(18,2),
    @DonGiaBan DECIMAL(18,2),
    @DonViTinh NVARCHAR(50),
    @MaKho INT
AS
BEGIN
    INSERT INTO QLVatLieu (Ten, Loai, DonGiaNhap, DonGiaBan, DonViTinh, SoLuong, MaKho, TrangThai)
    VALUES (@Ten, @Loai, @DonGiaNhap, @DonGiaBan, @DonViTinh, 0, @MaKho, N'Hoạt động');
END;
GO
CREATE PROCEDURE sp_TaoHoaDon
    @MaTKLap INT,
    @MaKhachHang INT = NULL, -- Cho phép khách vãng lai
    @HinhThucThanhToan NVARCHAR(50),
    @MaHoaDon INT OUTPUT
AS
BEGIN
    INSERT INTO QLHoaDon (NgayLap, MaTKLap, MaKhachHang, TongTien, TrangThai, HinhThucThanhToan)
    VALUES (GETDATE(), @MaTKLap, @MaKhachHang, 0, N'Chờ thanh toán', @HinhThucThanhToan);

    SET @MaHoaDon = SCOPE_IDENTITY();
END;
GO
CREATE PROCEDURE sp_ThemChiTietHoaDon
    @MaHoaDon INT,
    @MaVatLieu INT,
    @SoLuong INT
AS
BEGIN
    DECLARE @DonGia DECIMAL(18,2);

    -- Lấy giá bán của vật liệu
    SELECT @DonGia = DonGiaBan FROM QLVatLieu WHERE MaVatLieu = @MaVatLieu;

    -- Thêm chi tiết hóa đơn
    INSERT INTO ChiTietHoaDon (MaHoaDon, MaVatLieu, SoLuong, DonGia)
    VALUES (@MaHoaDon, @MaVatLieu, @SoLuong, @DonGia);

    -- Cập nhật tổng tiền hóa đơn
    UPDATE QLHoaDon
    SET TongTien = TongTien + (@SoLuong * @DonGia)
    WHERE MaHoaDon = @MaHoaDon;
END;
GO
INSERT INTO QLTK (TenDangNhap, MatKhau, Email, ChucVu, TrangThai)
VALUES (N'admin', N'123456', N'admin@example.com', N'Quản lý', N'Hoạt động'),
       (N'nhanvien1', N'abc123', N'nhanvien1@example.com', N'Nhân viên', N'Hoạt động');
GO
INSERT INTO Kho (TenKho)
VALUES (N'Kho Chính'), (N'Kho Phụ');
GO
INSERT INTO NCC (TenNCC, DiaChi, SDT, Email)
VALUES (N'Công ty A', N'123 Đường ABC, TP.HCM', N'0909123456', N'ncc_a@example.com'),
       (N'Công ty B', N'456 Đường XYZ, Hà Nội', N'0912345678', N'ncc_b@example.com');
GO
	   INSERT INTO QLLoaiVatLieu (TenLoai)
VALUES (N'Xi măng'), (N'Cát'), (N'Gạch'), (N'Sắt thép');
GO
INSERT INTO QLKH (Ten, NgaySinh, GioiTinh, SDT, Email)
VALUES (N'Nguyễn Văn A', '1990-01-01', N'Nam', '0987654321', N'vana@example.com'),
       (N'Trần Thị B', '1995-05-10', N'Nữ', '0976543210', N'thib@example.com');

go
EXEC sp_ThemVatLieu N'Xi măng Hà Tiên', 1, 50000, 70000, N'Bao', 1;
EXEC sp_ThemVatLieu N'Cát vàng', 2, 200000, 250000, N'Khối', 2;
EXEC sp_ThemVatLieu N'Gạch đỏ', 3, 1000, 1500, N'Viên', 1;
GO
INSERT INTO QLDonNhap (NgayNhap, MaNCC, MaTK, TrangThai, GhiChu)
VALUES (GETDATE(), 1, 1, N'Đang xử lý', N'Nhập hàng tháng 3');

INSERT INTO ChiTietDonNhap (MaDonNhap, MaVatLieu, SoLuong, GiaNhap)
VALUES (1, 1, 100, 50000),
       (1, 2, 50, 200000),
       (1, 3, 500, 1000);
GO
DECLARE @MaHD INT;
EXEC sp_TaoHoaDon 1, 1, N'Tiền mặt', @MaHD OUTPUT;
EXEC sp_ThemChiTietHoaDon @MaHD, 1, 10;
EXEC sp_ThemChiTietHoaDon @MaHD, 2, 5;
EXEC sp_ThemChiTietHoaDon @MaHD, 3, 100;
go
CREATE PROCEDURE sp_LayDanhSachHoaDon
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        hd.MaHoaDon, 
        hd.NgayLap, 
        tk.TenDangNhap AS N'Người Lập', 
        kh.Ten AS N'Khách Hàng', 
        hd.TongTien, 
        hd.TrangThai, 
        hd.HinhThucThanhToan
    FROM QLHoaDon hd
    JOIN QLTK tk ON hd.MaTKLap = tk.MaTK
    LEFT JOIN QLKH kh ON hd.MaKhachHang = kh.MaKhachHang;
END;
go
CREATE TRIGGER trg_CapNhatNgayCapNhat_QLVatLieu
ON QLVatLieu
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE QLVatLieu
    SET NgayCapNhat = GETDATE(),
        NguoiCapNhat = SYSTEM_USER
    FROM QLVatLieu v
    INNER JOIN inserted i ON v.MaVatLieu = i.MaVatLieu;
END;
go
CREATE TRIGGER trg_AuditLog_QLVatLieu
ON QLVatLieu
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO AuditLog (MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
    SELECT 
        COALESCE(i.NguoiCapNhat, 1), -- Sử dụng MaTK mặc định nếu không có
        'QLVatLieu',
        i.MaVatLieu,
        'UPDATE',
        CONCAT('SL:', d.SoLuong, ', GiaNhap:', d.DonGiaNhap, ', GiaBan:', d.DonGiaBan),
        CONCAT('SL:', i.SoLuong, ', GiaNhap:', i.DonGiaNhap, ', GiaBan:', i.DonGiaBan),
        'Cập nhật thông tin vật liệu'
    FROM deleted d
    INNER JOIN inserted i ON d.MaVatLieu = i.MaVatLieu
    WHERE d.SoLuong != i.SoLuong OR d.DonGiaNhap != i.DonGiaNhap OR d.DonGiaBan != i.DonGiaBan;
END;
Go
-- Stored Procedure tìm kiếm QLTK (Quản lý tài khoản)
CREATE PROCEDURE sp_TimKiemQLTK
    @MaTK INT = NULL,
    @TenDangNhap NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        MaTK,
        TenDangNhap,
        Email,
        ChucVu,
        TrangThai
    FROM QLTK
    WHERE (@MaTK IS NULL OR MaTK = @MaTK)
    AND (@TenDangNhap IS NULL OR TenDangNhap LIKE '%' + @TenDangNhap + '%')
    ORDER BY MaTK;
END;
GO

-- Stored Procedure tìm kiếm QLDonNhap (Quản lý đơn nhập)
CREATE PROCEDURE sp_TimKiemQLDonNhap
    @MaDonNhap INT = NULL,
    @TenNCC NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        dn.MaDonNhap,
        dn.NgayNhap,
        ncc.TenNCC,
        tk.TenDangNhap AS NguoiLap,
        dn.TrangThai,
        dn.GhiChu
    FROM QLDonNhap dn
    JOIN NCC ncc ON dn.MaNCC = ncc.MaNCC
    JOIN QLTK tk ON dn.MaTK = tk.MaTK
    WHERE (@MaDonNhap IS NULL OR dn.MaDonNhap = @MaDonNhap)
    AND (@TenNCC IS NULL OR ncc.TenNCC LIKE '%' + @TenNCC + '%')
    ORDER BY dn.MaDonNhap DESC;
END;
GO

-- Stored Procedure tìm kiếm NCC (Nhà cung cấp)
CREATE PROCEDURE sp_TimKiemNCC
    @MaNCC INT = NULL,
    @TenNCC NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        MaNCC,
        TenNCC,
        DiaChi,
        SDT,
        Email,
        NguoiTao,
        NgayTao
    FROM NCC
    WHERE (@MaNCC IS NULL OR MaNCC = @MaNCC)
    AND (@TenNCC IS NULL OR TenNCC LIKE '%' + @TenNCC + '%')
    ORDER BY MaNCC;
END;
GO
