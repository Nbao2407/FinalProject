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
CREATE TABLE AuditLog (
    MaLog INT PRIMARY KEY IDENTITY,
    ThoiGian DATETIME DEFAULT GETDATE(),
    MaTK INT REFERENCES QLTK(MaTK),
    TenBang NVARCHAR(50) NOT NULL,
    MaBanGhi INT NOT NULL,
    HanhDong NVARCHAR(20) ,
    GiaTriCu NVARCHAR(MAX),
    GiaTriMoi NVARCHAR(MAX),
    GhiChu NVARCHAR(255)
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

Alter table NCC ADD
	NguoiTao Nvarchar(256),
	NgayTao DATETIME DEFAULT GETDATE()
	Go
ALTER TABLE NCC
ADD TrangThai NVARCHAR(20) CHECK (TrangThai IN (N'Hoạt động', N'Bị khóa')) DEFAULT N'Hoạt động';
	Go
ALTER TABLE QLVatLieu ADD 
    NgayTao DATETIME DEFAULT GETDATE(),
    NguoiTao INT REFERENCES QLTK(MaTK),
    NgayCapNhat DATETIME,
    NguoiCapNhat INT REFERENCES QLTK(MaTK);
	GO
ALTER TABLE QLDonNhap ADD
    NgayCapNhat DATETIME,
    NguoiCapNhat INT REFERENCES QLTK(MaTK);
	Go
ALTER TABLE QLHoaDon ADD
    NgayCapNhat DATETIME,
    NguoiCapNhat INT REFERENCES QLTK(MaTK);
	ALTER TABLE QLVatLieu ADD CONSTRAINT DF_QLVatLieu_NgayCapNhat DEFAULT GETDATE() FOR NgayCapNhat;

	 

	 select * from QLDonNhap
	 SELECT * FROM QLVatLieu


	 INSERT INTO NCC (TenNCC, DiaChi, SDT, Email, NguoiTao, TrangThai)
VALUES 
    (N'Công ty C', N'789 Đường DEF, Đà Nẵng', N'0935123456', N'ncc_c@example.com', N'admin', N'Hoạt động'),
    (N'Công ty D', N'101 Đường GHI, Cần Thơ', N'0945123456', N'ncc_d@example.com', N'admin', N'Hoạt động');
GO
INSERT INTO QLLoaiVatLieu (TenLoai)
VALUES 
    (N'Thạch cao'),
    (N'Sơn');
GO

EXEC sp_ThemVatLieu N'Thạch cao xây dựng', 5, 30000, 45000, N'Bao', 1;
EXEC sp_ThemVatLieu N'Sơn trắng Dulux', 6, 500000, 600000, N'Thùng', 2;
EXEC sp_ThemVatLieu N'Sắt phi 12', 4, 15000, 20000, N'Cây', 1;
EXEC sp_ThemVatLieu N'Gạch ống', 3, 1200, 1800, N'Viên', 2;
GO



INSERT INTO QLDonNhap (NgayNhap, MaNCC, MaTK, TrangThai, GhiChu)
VALUES 
    (GETDATE(), 3, 2, N'Hoàn thành', N'Nhập hàng tháng 3 - Lô 2'),
    ('2025-03-20', 1, 1, N'Đang xử lý', N'Nhập hàng dự trữ');

INSERT INTO ChiTietDonNhap (MaDonNhap, MaVatLieu, SoLuong, GiaNhap)
VALUES 
    (1, 3, 200, 30000),  
    (8, 7, 50, 500000), 
    (9, 6, 1000, 15000)
GO

DECLARE @MaHD1 INT, @MaHD2 INT;

EXEC sp_TaoHoaDon 2, 2, N'Chuyển khoản', @MaHD1 OUTPUT;
EXEC sp_ThemChiTietHoaDon @MaHD1, 4, 50;  -- Thạch cao
EXEC sp_ThemChiTietHoaDon @MaHD1, 5, 10;  -- Sơn trắng

EXEC sp_TaoHoaDon 3, NULL, N'Tiền mặt', @MaHD2 OUTPUT;
EXEC sp_ThemChiTietHoaDon @MaHD2, 6, 200; -- Sắt phi 12
EXEC sp_ThemChiTietHoaDon @MaHD2, 7, 500; -- Gạch ống
GO
SELECT 
    l.TenLoai, 
    SUM(ct.SoLuong * ct.DonGia) AS DoanhThu
FROM ChiTietHoaDon ct
JOIN QLVatLieu v ON ct.MaVatLieu = v.MaVatLieu
JOIN QLLoaiVatLieu l ON v.Loai = l.MaLoaiVatLieu
GROUP BY l.TenLoai
ORDER BY DoanhThu DESC;
-- Thêm hóa đơn vào ngày Mar 17, 2025
DECLARE @MaHD1 INT;
INSERT INTO QLHoaDon (NgayLap, MaTKLap, MaKhachHang, TongTien, TrangThai, HinhThucThanhToan)
VALUES ('2025-03-17', 1, 1, 0, N'Đã thanh toán', N'Tiền mặt');
SET @MaHD1 = SCOPE_IDENTITY();

EXEC sp_ThemChiTietHoaDon @MaHD1, 1, 20; -- Xi măng Hà Tiên: 20 bao
EXEC sp_ThemChiTietHoaDon @MaHD1, 2, 10; -- Cát vàng: 10 khối
EXEC sp_ThemChiTietHoaDon @MaHD1, 3, 200; -- Gạch đỏ: 200 viên

-- Thêm hóa đơn vào ngày Mar 20, 2025
DECLARE @MaHD2 INT;
INSERT INTO QLHoaDon (NgayLap, MaTKLap, MaKhachHang, TongTien, TrangThai, HinhThucThanhToan)
VALUES ('2025-03-20', 2, 2, 0, N'Đã thanh toán', N'Chuyển khoản');
SET @MaHD2 = SCOPE_IDENTITY();

EXEC sp_ThemChiTietHoaDon @MaHD2, 4, 30; -- Thạch cao: 30 bao
EXEC sp_ThemChiTietHoaDon @MaHD2, 5, 5;  -- Sơn trắng: 5 thùng

-- Thêm hóa đơn vào ngày Mar 23, 2025
DECLARE @MaHD3 INT;
INSERT INTO QLHoaDon (NgayLap, MaTKLap, MaKhachHang, TongTien, TrangThai, HinhThucThanhToan)
VALUES ('2025-03-23', 3, NULL, 0, N'Đã thanh toán', N'Tiền mặt');
SET @MaHD3 = SCOPE_IDENTITY();

EXEC sp_ThemChiTietHoaDon @MaHD3, 6, 100; -- Sắt phi 12: 100 cây
EXEC sp_ThemChiTietHoaDon @MaHD3, 7, 300; -- Gạch ống: 300 viên
GO
-- Number of Orders (Số lượng hóa đơn từ Mar 17, 2025 đến Mar 24, 2025)
SELECT COUNT(*) AS NumberOfOrders
FROM QLHoaDon
WHERE NgayLap BETWEEN '2025-03-17' AND '2025-03-24'
AND TrangThai = N'Đã thanh toán';

-- Total Revenue (Tổng doanh thu)
SELECT ISNULL(SUM(TongTien), 0) AS TotalRevenue
FROM QLHoaDon
WHERE NgayLap BETWEEN '2025-03-17' AND '2025-03-24'
AND TrangThai = N'Đã thanh toán';

-- Total Profit (Tổng lợi nhuận)
SELECT ISNULL(SUM((ct.DonGia - vl.DonGiaNhap) * ct.SoLuong), 0) AS TotalProfit
FROM ChiTietHoaDon ct
JOIN QLHoaDon hd ON ct.MaHoaDon = hd.MaHoaDon
JOIN QLVatLieu vl ON ct.MaVatLieu = vl.MaVatLieu
WHERE hd.NgayLap BETWEEN '2025-03-17' AND '2025-03-24'
AND hd.TrangThai = N'Đã thanh toán';

-- 5 Best Selling Products (5 sản phẩm bán chạy nhất)
SELECT TOP 5 
    vl.Ten AS ProductName,
    SUM(ct.SoLuong) AS TotalQuantitySold
FROM ChiTietHoaDon ct
JOIN QLHoaDon hd ON ct.MaHoaDon = hd.MaHoaDon
JOIN QLVatLieu vl ON ct.MaVatLieu = vl.MaVatLieu
WHERE hd.NgayLap BETWEEN '2025-03-17' AND '2025-03-24'
AND hd.TrangThai = N'Đã thanh toán'
GROUP BY vl.Ten
ORDER BY TotalQuantitySold DESC;