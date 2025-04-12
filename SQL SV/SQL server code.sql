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
    WHERE NgayNhap = '2025-04-08'
);

DELETE FROM QLDonNhap
WHERE NgayNhap = '2025-04-08';

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

ALTER TABLE QLDonNhap
 NguoiNhap INT not null,
    CONSTRAINT FK_QLDonNhap_QLTK_NguoiNhap FOREIGN KEY (NguoiNhap) REFERENCES QLTK(MaTK);


ALTER PROCEDURE sp_NhapHang
    @NgayNhap DATE,                  -- Ngày nhập hàng
    @MaNCC INT,                      -- Mã nhà cung cấp
    @MaTK INT,                       -- Mã tài khoản người tạo đơn
    @GhiChu NVARCHAR(255) = NULL,    -- Ghi chú (không bắt buộc)
    @ChiTietNhap NVARCHAR(MAX),      -- Chuỗi JSON chứa danh sách chi tiết nhập hàng
    @NguoiCapNhat INT,               -- Mã tài khoản người cập nhật
    @NguoiNhap INT                   -- Mã tài khoản người nhập hàng (thêm mới)
AS
BEGIN
    BEGIN TRY
        -- Bắt đầu transaction để đảm bảo tính toàn vẹn dữ liệu
        BEGIN TRANSACTION;

        -- Kiểm tra nhà cung cấp có tồn tại và hoạt động không
        IF NOT EXISTS (SELECT 1 FROM NCC WHERE MaNCC = @MaNCC AND TrangThai = N'Hoạt động')
            THROW 50001, N'Nhà cung cấp không tồn tại hoặc không hoạt động!', 1;

        -- Kiểm tra tài khoản người tạo có tồn tại và hoạt động không
        IF NOT EXISTS (SELECT 1 FROM QLTK WHERE MaTK = @MaTK AND TrangThai = N'Hoạt động')
            THROW 50002, N'Tài khoản người tạo không tồn tại hoặc bị khóa!', 1;

        -- Kiểm tra tài khoản người nhập có tồn tại và hoạt động không
        IF NOT EXISTS (SELECT 1 FROM QLTK WHERE MaTK = @NguoiNhap AND TrangThai = N'Hoạt động')
            THROW 50006, N'Tài khoản người nhập không tồn tại hoặc bị khóa!', 1;

        -- Thêm đơn nhập hàng mới, bao gồm cột NguoiNhap
        INSERT INTO QLDonNhap (NgayNhap, MaNCC, MaTK, TrangThai, GhiChu, NgayCapNhat, NguoiCapNhat, NguoiNhap)
        VALUES (@NgayNhap, @MaNCC, @MaTK, N'Hoàn thành', @GhiChu, GETDATE(), @NguoiCapNhat, @NguoiNhap);

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
        VALUES (GETDATE(), @NguoiCapNhat, N'QLDonNhap', @MaDonNhap, N'Thêm', NULL, NULL, N'Thêm đơn nhập hàng mới bởi người nhập: ' + CAST(@NguoiNhap AS NVARCHAR(10)));

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
CREATE TABLE QLDonXuat (
    MaDonXuat INT PRIMARY KEY IDENTITY,
    NgayXuat DATE DEFAULT GETDATE(),
    MaTK INT NOT NULL FOREIGN KEY REFERENCES QLTK(MaTK), -- Người lập đơn
    MaHoaDon INT NULL FOREIGN KEY REFERENCES QLHoaDon(MaHoaDon), -- Liên kết với hóa đơn (nếu xuất để bán)
    MaKhachHang INT NULL FOREIGN KEY REFERENCES QLKH(MaKhachHang), -- Khách hàng (nếu xuất để bán)
    TrangThai NVARCHAR(50) CHECK (TrangThai IN (N'Đang xử lý', N'Hoàn thành', N'Đã hủy')) DEFAULT N'Đang xử lý',
    GhiChu NVARCHAR(255),
    NgayCapNhat DATETIME DEFAULT GETDATE(),
    NguoiCapNhat INT REFERENCES QLTK(MaTK)
);
GO

CREATE TABLE ChiTietDonXuat (
    MaCTDX INT PRIMARY KEY IDENTITY,
    MaDonXuat INT NOT NULL FOREIGN KEY REFERENCES QLDonXuat(MaDonXuat),
    MaVatLieu INT NOT NULL FOREIGN KEY REFERENCES QLVatLieu(MaVatLieu),
    SoLuong INT CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) CHECK (DonGia >= 0) 
);
GO
EXEC sp_ThemDonXuat
    @NgayXuat = '2025-04-11',
    @MaTK = 1,
    @MaHoaDon = NULL,
    @MaKhachHang = 1,
    @GhiChu = N'Xuất kho để bán',
    @ChiTietXuat = '[{"MaVatLieu": 1, "SoLuong": 50, "DonGia": 100000}]',
    @NguoiCapNhat = 1;
	SELECT MaDonXuat, NgayXuat, TrangThai, GhiChu 
                                      FROM QLDonXuat
									  -- Thêm cột LoaiXuat vào bảng QLDonXuat
ALTER TABLE QLDonXuat
ADD LoaiXuat NVARCHAR(50) NULL; -- Tạm thời cho phép NULL để cập nhật dữ liệu cũ
GO

-- Cập nhật giá trị mặc định cho các bản ghi hiện có (GIẢ SỬ LÀ 'Bán hàng')
UPDATE QLDonXuat
SET LoaiXuat = N'Xuất hàng'
WHERE LoaiXuat IS NULL;
GO

-- Thêm ràng buộc NOT NULL và CHECK sau khi đã có dữ liệu
ALTER TABLE QLDonXuat
ALTER COLUMN LoaiXuat NVARCHAR(50) NOT NULL;
GO

ALTER TABLE QLDonXuat
ADD CONSTRAINT CK_QLDonXuat_LoaiXuat CHECK (LoaiXuat IN (
    N'Xuất hàng',
    N'Chuyển kho'
));
GO


ALTER TABLE QLDonXuat
ADD CONSTRAINT CK_QLDonXuat_KhachHangTheoLoai CHECK (
    (LoaiXuat != N'Xuất hàng' AND MaKhachHang IS NULL)
    OR
    (LoaiXuat = N'Xuất hàng' AND MaKhachHang IS NOT NULL)
);
GO
SELECT
                    dx.MaDonXuat, dx.NgayXuat, dx.LoaiXuat, 
                    dx.MaHoaDon,  kh.Ten AS TenKhachHang, dx.TrangThai,dx.GhiChu
                FROM QLDonXuat dx
                LEFT JOIN QLKH kh ON dx.MaKhachHang = kh.MaKhachHang
