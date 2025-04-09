-- Tạo cơ sở dữ liệu Quản Lý Vật Tư (QLVT)
CREATE DATABASE QLVT
GO
USE QLVT
GO

-- 1. Bảng Quản lý tài khoản (Quản lý đăng nhập và thông tin người dùng)
CREATE TABLE QLTK (
    MaTK INT PRIMARY KEY IDENTITY,
    TenDangNhap NVARCHAR(50) UNIQUE NOT NULL,
    MatKhau NVARCHAR(255) NOT NULL, -- Nên lưu hash mật khẩu
    Email NVARCHAR(100) UNIQUE NOT NULL,
    ChucVu NVARCHAR(50) NOT NULL,
    TrangThai NVARCHAR(20) CHECK (TrangThai IN (N'Hoạt động', N'Bị khóa')) DEFAULT N'Hoạt động',
    SDT NVARCHAR(15), -- Số điện thoại
    GHICHU NVARCHAR(255), -- Ghi chú
    DiaChi NVARCHAR(255), -- Địa chỉ
    NgayTao DATETIME DEFAULT GETDATE() -- Ngày tạo tài khoản
);

-- 2. Bảng Kho (Quản lý thông tin kho hàng)
CREATE TABLE Kho (
    MaKho INT PRIMARY KEY IDENTITY,
    TenKho NVARCHAR(100) NOT NULL
);

-- 3. Bảng Nhà cung cấp (Quản lý thông tin nhà cung cấp)
CREATE TABLE NCC (
    MaNCC INT PRIMARY KEY IDENTITY,
    TenNCC NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(255),
    SDT NVARCHAR(15) CHECK (SDT LIKE '[0-9]%' AND LEN(SDT) BETWEEN 10 AND 15),
    Email NVARCHAR(100) UNIQUE,
    NguoiTao Nvarchar(256),
    NgayTao DATETIME DEFAULT GETDATE(),
    TrangThai NVARCHAR(20) CHECK (TrangThai IN (N'Hoạt động', N'Bị khóa')) DEFAULT N'Hoạt động'
);
Select * from NCC

-- 4. Bảng Quản lý khách hàng (Lưu trữ thông tin khách hàng)
CREATE TABLE QLKH (
    MaKhachHang INT PRIMARY KEY IDENTITY,
    Ten NVARCHAR(100) NOT NULL,
    NgaySinh DATE,
    GioiTinh NVARCHAR(10) CHECK (GioiTinh IN (N'Nam', N'Nữ')),
    SDT NVARCHAR(15) CHECK (SDT LIKE '[0-9]%' AND LEN(SDT) BETWEEN 10 AND 15),
    Email NVARCHAR(100) UNIQUE,
    DiaChi NvarChar(100),
    TenNguoiTao NvarChar(50),
    NguoiTao INT REFERENCES QLTK(MaTK),
    NgayTao DATETIME DEFAULT GETDATE()
);

-- 5. Bảng Quản lý loại vật liệu (Phân loại vật liệu)
CREATE TABLE QLLoaiVatLieu (
    MaLoaiVatLieu INT PRIMARY KEY IDENTITY,
    TenLoai NVARCHAR(100) NOT NULL,
    TrangThai NVARCHAR(20) CHECK (TrangThai IN (N'Hoạt động', N'Ngừng sử dụng')) DEFAULT N'Hoạt động'
);

-- 6. Bảng Quản lý vật liệu (Thông tin chi tiết về từng loại vật liệu)
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
    HinhAnh VARBINARY(MAX),
    NgayTao DATETIME DEFAULT GETDATE(),
    NguoiTao INT REFERENCES QLTK(MaTK),
    NgayCapNhat DATETIME DEFAULT GETDATE(),
    NguoiCapNhat INT REFERENCES QLTK(MaTK)
);

-- 7. Bảng Nhật ký hoạt động (Ghi lại các thay đổi trong hệ thống)
CREATE TABLE AuditLog (
    MaLog INT PRIMARY KEY IDENTITY,
    ThoiGian DATETIME DEFAULT GETDATE(),
    MaTK INT REFERENCES QLTK(MaTK),
    TenBang NVARCHAR(50) NOT NULL,
    MaBanGhi INT NOT NULL,
    HanhDong NVARCHAR(20),
    GiaTriCu NVARCHAR(MAX),
    GiaTriMoi NVARCHAR(MAX),
    GhiChu NVARCHAR(255)
);

-- 8. Bảng Quản lý đơn nhập (Theo dõi các đợt nhập hàng)
CREATE TABLE QLDonNhap (
    MaDonNhap INT PRIMARY KEY IDENTITY,
    NgayNhap DATE DEFAULT GETDATE(),
    MaNCC INT NOT NULL FOREIGN KEY REFERENCES NCC(MaNCC),
    MaTK INT NOT NULL FOREIGN KEY REFERENCES QLTK(MaTK),
    TrangThai NVARCHAR(50) CHECK (TrangThai IN (N'Đang xử lý', N'Hoàn thành', N'Đã hủy')) DEFAULT N'Đang xử lý',
    GhiChu NVARCHAR(255),
    NgayCapNhat DATETIME,
    NguoiCapNhat INT REFERENCES QLTK(MaTK)
);

-- 9. Bảng Chi tiết đơn nhập (Chi tiết từng mặt hàng trong đơn nhập)
CREATE TABLE ChiTietDonNhap (
    MaCTDN INT PRIMARY KEY IDENTITY,
    MaDonNhap INT NOT NULL FOREIGN KEY REFERENCES QLDonNhap(MaDonNhap),
    MaVatLieu INT NOT NULL FOREIGN KEY REFERENCES QLVatLieu(MaVatLieu),
    SoLuong INT CHECK (SoLuong > 0),
    GiaNhap DECIMAL(18,2) CHECK (GiaNhap >= 0)
);

-- 10. Bảng Quản lý hóa đơn (Theo dõi các giao dịch bán hàng)
CREATE TABLE QLHoaDon (
    MaHoaDon INT PRIMARY KEY IDENTITY,
    NgayLap DATE DEFAULT GETDATE(),
    MaTKLap INT NOT NULL FOREIGN KEY REFERENCES QLTK(MaTK),
    MaKhachHang INT NULL FOREIGN KEY REFERENCES QLKH(MaKhachHang),
    TongTien DECIMAL(18,2) CHECK (TongTien >= 0),
    TrangThai NVARCHAR(50) CHECK (TrangThai IN (N'Chờ thanh toán', N'Đã thanh toán', N'Đã hủy')) DEFAULT N'Chờ thanh toán',
    HinhThucThanhToan NVARCHAR(50) CHECK (HinhThucThanhToan IN (N'Tiền mặt', N'Chuyển khoản', N'Thẻ tín dụng')) DEFAULT N'Tiền mặt',
    NgayCapNhat DATETIME,
    NguoiCapNhat INT REFERENCES QLTK(MaTK)
);

-- 11. Bảng Chi tiết hóa đơn (Chi tiết từng mặt hàng trong hóa đơn)
CREATE TABLE ChiTietHoaDon (
    MaCTHD INT PRIMARY KEY IDENTITY,
    MaHoaDon INT NOT NULL FOREIGN KEY REFERENCES QLHoaDon(MaHoaDon),
    MaVatLieu INT NOT NULL FOREIGN KEY REFERENCES QLVatLieu(MaVatLieu),
    SoLuong INT CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) CHECK (DonGia >= 0)
);
-- 12 Bảng lưu token reset mật khẩu
CREATE TABLE PasswordResetToken (
    MaToken INT PRIMARY KEY IDENTITY,
    MaTK INT NOT NULL FOREIGN KEY REFERENCES QLTK(MaTK),
    Token NVARCHAR(255) NOT NULL,
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayHetHan DATETIME NOT NULL,
    DaSuDung BIT DEFAULT 0 -- 0: chưa dùng, 1: đã dùng
);
GO
alter table QLKH add 
    TrangThai NVARCHAR(20) CHECK (TrangThai IN (N'Hoạt động', N'Ngừng sử dụng')) DEFAULT N'Hoạt động'

-- Thêm dữ liệu mẫu cho các bảng
-- Thêm dữ liệu tài khoản
UPDATE QLTK
SET 
    SDT = '09123425678',
    GHICHU = N'Tài khoản quản trị hệ thống',
    DiaChi = N'123 Đường Láng, Đống Đa, Hà Nội',
    NgayTao = '2025-01-01 10:00:00'
WHERE MaTK = 1;

UPDATE QLTK
SET 
    SDT = '0912345678',
    GHICHU = N'Nhân viên mới',
    DiaChi = N'45 Nguyễn Huệ, Quận 1, TP.HCM',
    NgayTao = '2025-02-15 09:30:00'
WHERE MaTK = 2;

-- Thêm dữ liệu nhà cung cấp
INSERT INTO NCC (TenNCC, DiaChi, SDT, Email, NguoiTao, TrangThai)
VALUES 
    (N'Công ty C', N'789 Đường DEF, Đà Nẵng', N'0935123456', N'ncc_c@example.com', N'admin', N'Hoạt động'),
    (N'Công ty D', N'101 Đường GHI, Cần Thơ', N'0945123456', N'ncc_d@example.com', N'admin', N'Hoạt động');


-- Thêm dữ liệu loại vật liệu
INSERT INTO QLLoaiVatLieu (TenLoai)
VALUES 
	(N'Đá');

-- Thêm dữ liệu khách hàng và cập nhật người tạo
UPDATE QLKH
SET NguoiTao = 1,
    NgayTao = GETDATE()
WHERE NguoiTao IS NULL;

UPDATE QLKH
SET Trangthai =  N'Ngừng sử dụng'
Where MaKhachHang=MaKhachHang


WHERE kh.TenNguoiTao IS NULL;
DELETE FROM ChiTietHoaDon
WHERE MaHoaDon IN (
    SELECT MaHoaDon FROM QLHoaDon
    WHERE NgayLap = '2025-04-07'
);

DELETE FROM QLHoaDon
WHERE NgayLap = '2025-04-07';

DELETE FROM ChiTietDonNhap
WHERE MaCTDN IN (
    SELECT MaCTDN FROM QLDonNhap
    WHERE NgayNhap = '2025-04-07'
);

DELETE FROM QLDonNhap
WHERE NgayNhap = '2025-04-07';

SELECT maTK, tenDangNhap, email, sdt, ChucVu, trangThai, ngayTao , GHICHU ,DiaChi FROM QLTK WHERE maTk = 1
Select * from QLTK

BACKUP DATABASE QLVT 
TO DISK = 'I:\Pro213\SQL SV\Backup\QLVT.bak'
WITH FORMAT, INIT, 
NAME = 'Backup QLVT';

UPDATE dbo.QLTK
SET TrangThai =  N'Bị khóa'
WHERE MaTK= 2;

SELECT * 
FROM INFORMATION_SCHEMA.CHECK_CONSTRAINTS 
WHERE CONSTRAINT_NAME = 'CK_QLTK_TrangThai_39508EEE';

SELect MaNCC,TenNCC from NCC Where TrangThai =N'Hoạt động'

SELECT MaNCC, TenNCC, DiaChi, SDT, Email, NgayTao, NguoiTao FROM NCC