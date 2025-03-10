CREATE DATABASE QLVT
GO
USE QLVT
GO
-- Bảng Quản lý tài khoản
CREATE TABLE QLTK (
    MaTK INT PRIMARY KEY IDENTITY,
    TenDangNhap NVARCHAR(50) UNIQUE NOT NULL,
    MatKhau NVARCHAR(255) NOT NULL, -- Nên lưu hash mật khẩu
    Email NVARCHAR(100) UNIQUE NOT NULL,
    ChucVu NVARCHAR(50) NOT NULL,
    TrangThai NVARCHAR(20) CHECK (TrangThai IN (N'Hoạt động', N'Bị khóa')) DEFAULT N'Hoạt động'
);

-- Bảng Kho
CREATE TABLE Kho (
    MaKho INT PRIMARY KEY IDENTITY,
    TenKho NVARCHAR(100) NOT NULL
);

-- Bảng Nhà cung cấp (NCC)
CREATE TABLE NCC (
    MaNCC INT PRIMARY KEY IDENTITY,
    TenNCC NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(255),
    SDT NVARCHAR(15) CHECK (SDT LIKE '[0-9]%' AND LEN(SDT) BETWEEN 10 AND 15),
    Email NVARCHAR(100) UNIQUE
);

-- Bảng Quản lý khách hàng (QLKH)
CREATE TABLE QLKH (
    MaKhachHang INT PRIMARY KEY IDENTITY,
    Ten NVARCHAR(100) NOT NULL,
    NgaySinh DATE,
    GioiTinh NVARCHAR(10) CHECK (GioiTinh IN (N'Nam', N'Nữ')),
    SDT NVARCHAR(15) CHECK (SDT LIKE '[0-9]%' AND LEN(SDT) BETWEEN 10 AND 15),
    Email NVARCHAR(100) UNIQUE
);

-- Bảng Quản lý loại vật liệu
CREATE TABLE QLLoaiVatLieu (
    MaLoaiVatLieu INT PRIMARY KEY IDENTITY,
    TenLoai NVARCHAR(100) NOT NULL,
    TrangThai NVARCHAR(20) CHECK (TrangThai IN (N'Hoạt động', N'Ngừng sử dụng')) DEFAULT N'Hoạt động',

);

-- Bảng Quản lý vật liệu
CREATE TABLE QLVatLieu (
    MaVatLieu INT PRIMARY KEY IDENTITY,
    Ten NVARCHAR(100) NOT NULL,
    Loai INT NOT NULL FOREIGN KEY REFERENCES QLLoaiVatLieu(MaLoaiVatLieu),
    DonGiaNhap DECIMAL(18,2) CHECK (DonGiaNhap >= 0),
    DonGiaBan DECIMAL(18,2) CHECK (DonGiaBan >= 0),
    DonViTinh NVARCHAR(50) NOT NULL,
    SoLuong INT CHECK (SoLuong >= 0) DEFAULT 0,
    MaKho INT NOT NULL FOREIGN KEY REFERENCES Kho(MaKho),
    TrangThai NVARCHAR(20) CHECK (TrangThai IN (N'Hoạt động', N'Ngừng sử dụng')) DEFAULT N'Hoạt động',
    GhiChu NVARCHAR(255),
    HinhAnh VARBINARY(MAX)
);

-- Bảng Quản lý đơn nhập
CREATE TABLE QLDonNhap (
    MaDonNhap INT PRIMARY KEY IDENTITY,
    NgayNhap DATE DEFAULT GETDATE(),
    MaNCC INT NOT NULL FOREIGN KEY REFERENCES NCC(MaNCC),
    MaTK INT NOT NULL FOREIGN KEY REFERENCES QLTK(MaTK),
    TrangThai NVARCHAR(50) CHECK (TrangThai IN (N'Đang xử lý', N'Hoàn thành', N'Đã hủy')) DEFAULT N'Đang xử lý',
    GhiChu NVARCHAR(255)
);

-- Bảng Chi tiết đơn nhập
CREATE TABLE ChiTietDonNhap (
    MaCTDN INT PRIMARY KEY IDENTITY,
    MaDonNhap INT NOT NULL FOREIGN KEY REFERENCES QLDonNhap(MaDonNhap),
    MaVatLieu INT NOT NULL FOREIGN KEY REFERENCES QLVatLieu(MaVatLieu),
    SoLuong INT CHECK (SoLuong > 0),
    GiaNhap DECIMAL(18,2) CHECK (GiaNhap >= 0)
);

-- Bảng Quản lý hóa đơn
CREATE TABLE QLHoaDon (
    MaHoaDon INT PRIMARY KEY IDENTITY,
    NgayLap DATE DEFAULT GETDATE(),
    MaTKLap INT NOT NULL FOREIGN KEY REFERENCES QLTK(MaTK),
    MaKhachHang INT NULL FOREIGN KEY REFERENCES QLKH(MaKhachHang), -- Thêm khách hàng vào hóa đơn
    TongTien DECIMAL(18,2) CHECK (TongTien >= 0),
    TrangThai NVARCHAR(50) CHECK (TrangThai IN (N'Chờ thanh toán', N'Đã thanh toán', N'Đã hủy')) DEFAULT N'Chờ thanh toán',
    HinhThucThanhToan NVARCHAR(50) CHECK (HinhThucThanhToan IN (N'Tiền mặt', N'Chuyển khoản', N'Thẻ tín dụng')) DEFAULT N'Tiền mặt'
);

-- Bảng Chi tiết hóa đơn
CREATE TABLE ChiTietHoaDon (
    MaCTHD INT PRIMARY KEY IDENTITY,
    MaHoaDon INT NOT NULL FOREIGN KEY REFERENCES QLHoaDon(MaHoaDon),
    MaVatLieu INT NOT NULL FOREIGN KEY REFERENCES QLVatLieu(MaVatLieu),
    SoLuong INT CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) CHECK (DonGia >= 0)
);
GO

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
create PROCEDURE [dbo].[sp_TimKiemVatLieu]
    @Keyword NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT vl.MaVatLieu, vl.Ten, lv.TenLoai, vl.DonGiaBan, vl.DonGiaNhap, vl.DonViTinh, vl.SoLuong
    FROM QLVatLieu vl  
    JOIN QLLoaiVatLieu lv ON vl.Loai = lv.MaLoaiVatLieu
    WHERE CAST(vl.MaVatLieu AS NVARCHAR) LIKE '%' + @Keyword + '%'  
       OR vl.Ten LIKE '%' + @Keyword + '%'
       OR lv.TenLoai LIKE '%' + @Keyword + '%';   
END;
Go
ALTER PROCEDURE sp_TimKiemLoaiVatLieu
    @Keyword NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        MaLoaiVatLieu, 
        TenLoai, 
        TrangThai,
        -- Tính độ khớp để sắp xếp
        CASE 
            WHEN TenLoai LIKE @Keyword + '%' THEN 3 -- Khớp đầu tiên
            WHEN TenLoai LIKE '%' + @Keyword + '%' THEN 2 -- Khớp giữa
            ELSE 1
        END AS MatchScore
    FROM QLLoaiVatLieu
    WHERE 
        TenLoai COLLATE Latin1_General_CI_AI LIKE '%' + @Keyword + '%'
        OR CAST(MaLoaiVatLieu AS NVARCHAR) LIKE @Keyword + '%'
    ORDER BY MatchScore DESC, TenLoai;
END;
ALTER PROCEDURE [dbo].[sp_TimKiemVatLieu]
    @Keyword NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    -- Chuẩn hóa keyword để tìm kiếm không dấu và không phân biệt hoa thường
    DECLARE @Search NVARCHAR(100) = '%' + @Keyword + '%';

    SELECT 
        vl.MaVatLieu, 
        vl.Ten, 
        lv.TenLoai, 
        vl.DonGiaBan, 
        vl.DonGiaNhap, 
        vl.DonViTinh, 
        vl.SoLuong,
        -- Xếp hạng mức độ khớp để ưu tiên hiển thị
        CASE 
            WHEN vl.Ten LIKE @Keyword + '%' THEN 3  -- Khớp đầu tiên
            WHEN vl.Ten LIKE '%' + @Keyword + '%' THEN 2 -- Khớp giữa
            ELSE 1
        END AS MatchScore
    FROM QLVatLieu vl  
    JOIN QLLoaiVatLieu lv ON vl.Loai = lv.MaLoaiVatLieu
    WHERE 
        vl.Ten COLLATE Latin1_General_CI_AI LIKE @Search
        OR lv.TenLoai COLLATE Latin1_General_CI_AI LIKE @Search
        OR CAST(vl.MaVatLieu AS NVARCHAR) LIKE @Search
    ORDER BY MatchScore DESC, vl.Ten;
END;

SELECT * FROM QLTK;
SELECT * FROM Kho;
SELECT * FROM NCC;
SELECT * FROM QLKH;
SELECT * FROM QLLoaiVatLieu;
SELECT * FROM QLVatLieu;
SELECT * FROM QLDonNhap;
SELECT * FROM ChiTietDonNhap;
SELECT * FROM QLHoaDon;
SELECT * FROM ChiTietHoaDon;
