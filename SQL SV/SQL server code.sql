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

	 




















