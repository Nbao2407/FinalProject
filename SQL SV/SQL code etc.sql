-- Hệ thống Quản lý Vật liệu
-- Script SQL bao gồm các stored procedure, trigger và cài đặt dữ liệu ban đầu

--=============================================================================
-- HÀM VÀ FUNCTION TIỆN ÍCH
--=============================================================================

-- Hàm làm tròn tiền tệ (Đảm bảo hàm này tồn tại trước khi SP dùng nó)
-- Kiểm tra xem hàm đã tồn tại chưa
IF OBJECT_ID('dbo.fn_LamTronTien', 'FN') IS NULL
BEGIN
    -- Nếu chưa tồn tại, tạo hàm
    EXEC('
    CREATE FUNCTION dbo.fn_LamTronTien (
        @Gia DECIMAL(18,2),      -- Số tiền cần làm tròn
        @SoLamTron INT = 0       -- Số chữ số thập phân (MẶC ĐỊNH LÀ 0 CHO TIỀN VIỆT)
    )
    RETURNS DECIMAL(18,0) -- Trả về số nguyên nếu làm tròn đến 0
    AS
    BEGIN
        -- Làm tròn đến số nguyên gần nhất
        RETURN ROUND(@Gia, @SoLamTron)
    END;
    ')
END
GO

--=============================================================================
-- STORED PROCEDURES CHO HÓA ĐƠN BÁN HÀNG (QLHOADON, CHITIETHOADON)
--=============================================================================

-- Stored Procedure: Thêm chi tiết hóa đơn bán hàng
-- Lưu ý: Cần đảm bảo Trigger trg_KiemTraSoLuongBan tồn tại và xử lý việc trừ kho
ALTER PROCEDURE sp_ThemChiTietHoaDon
    @MaHoaDon INT,        -- Mã hóa đơn
    @MaVatLieu INT,       -- Mã vật liệu
    @SoLuong INT          -- Số lượng
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @DonGiaBan DECIMAL(18,2);
    DECLARE @ThanhTien DECIMAL(18,2);
    DECLARE @CurrentTongTien DECIMAL(18,2);
    DECLARE @ErrorMsg NVARCHAR(MAX);

    BEGIN TRY
        -- Kiểm tra hóa đơn tồn tại
        IF NOT EXISTS (SELECT 1 FROM QLHoaDon WHERE MaHoaDon = @MaHoaDon)
        BEGIN
            SET @ErrorMsg = N'Hóa đơn không tồn tại.';
            THROW 50001, @ErrorMsg, 1;
            RETURN;
        END

        -- Kiểm tra vật liệu tồn tại và lấy giá bán
        SELECT @DonGiaBan = DonGiaBan FROM QLVatLieu WHERE MaVatLieu = @MaVatLieu AND TrangThai = N'Hoạt động';
        IF @DonGiaBan IS NULL
        BEGIN
            SET @ErrorMsg = N'Vật liệu không tồn tại hoặc không hoạt động.';
            THROW 50002, @ErrorMsg, 1;
            RETURN;
        END

        -- Kiểm tra số lượng > 0
        IF @SoLuong <= 0
        BEGIN
            SET @ErrorMsg = N'Số lượng phải lớn hơn 0.';
            THROW 50003, @ErrorMsg, 1;
            RETURN;
        END

        -- Tính thành tiền (chưa làm tròn)
        SET @ThanhTien = @SoLuong * @DonGiaBan;

        -- Bắt đầu transaction để đảm bảo tính toàn vẹn khi thêm chi tiết và cập nhật tổng tiền
        BEGIN TRANSACTION;

        -- Thêm chi tiết hóa đơn (Trigger trg_KiemTraSoLuongBan sẽ kiểm tra và trừ kho)
        -- Cần đảm bảo ChiTietHoaDon có Primary Key (MaHoaDon, MaVatLieu) hoặc xử lý trùng lặp
        IF EXISTS (SELECT 1 FROM ChiTietHoaDon WHERE MaHoaDon = @MaHoaDon AND MaVatLieu = @MaVatLieu)
        BEGIN
            -- Nếu đã tồn tại, cập nhật số lượng (cần logic kiểm tra kho lại hoặc trigger xử lý update)
            -- Tạm thời báo lỗi nếu muốn mỗi vật liệu chỉ có 1 dòng
            SET @ErrorMsg = N'Vật liệu này đã có trong hóa đơn. Vui lòng xóa hoặc sửa chi tiết cũ.';
            THROW 50004, @ErrorMsg, 1;
            -- Hoặc logic cập nhật:
            -- UPDATE ChiTietHoaDon SET SoLuong = SoLuong + @SoLuong WHERE MaHoaDon = @MaHoaDon AND MaVatLieu = @MaVatLieu;
            -- Lưu ý: Trigger trg_KiemTraSoLuongBan hiện chỉ xử lý INSERT, cần sửa nếu cho phép UPDATE
        END
        ELSE
        BEGIN
            INSERT INTO ChiTietHoaDon (MaHoaDon, MaVatLieu, SoLuong, DonGia)
            VALUES (@MaHoaDon, @MaVatLieu, @SoLuong, @DonGiaBan);
        END

        -- Lấy tổng tiền hiện tại của hóa đơn
        SELECT @CurrentTongTien = ISNULL(TongTien, 0) FROM QLHoaDon WHERE MaHoaDon = @MaHoaDon;

        -- Cập nhật tổng tiền hóa đơn (Sử dụng hàm làm tròn)
        UPDATE QLHoaDon
        SET TongTien = dbo.fn_LamTronTien(@CurrentTongTien + @ThanhTien, 0) -- Làm tròn đến 0 cho VNĐ
        WHERE MaHoaDon = @MaHoaDon;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW; -- Ném lại lỗi gốc
    END CATCH
END;
GO

-- Stored Procedure: Cập nhật hóa đơn bán hàng (Header)
-- Lưu ý: SP này chỉ cập nhật header, không cập nhật tổng tiền tự động từ chi tiết
ALTER PROCEDURE sp_CapNhatHoaDon
    @MaHoaDon INT,
    @NgayLap DATE, -- Nên là DATE thay vì DATETIME nếu chỉ cần ngày
    @MaTKLap INT,
    @MaKhachHang INT,
    -- @TongTien DECIMAL(18,2), -- Bỏ: Tổng tiền nên được tính từ SP khác hoặc Trigger
    @TrangThai NVARCHAR(50),
    @HinhThucThanhToan NVARCHAR(50),
    @NguoiCapNhat INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(MAX);

    BEGIN TRY
        -- Kiểm tra hóa đơn tồn tại
        IF NOT EXISTS (SELECT 1 FROM QLHoaDon WHERE MaHoaDon = @MaHoaDon)
        BEGIN
            SET @ErrorMsg = N'Hóa đơn không tồn tại.';
            THROW 50001, @ErrorMsg, 1;
            RETURN;
        END

        -- Kiểm tra Khách hàng tồn tại
        IF NOT EXISTS (SELECT 1 FROM QLKH WHERE MaKhachHang = @MaKhachHang AND TrangThai = N'Hoạt động')
        BEGIN
            SET @ErrorMsg = N'Khách hàng không tồn tại hoặc không hoạt động.';
            THROW 50002, @ErrorMsg, 1;
            RETURN;
        END

         -- Kiểm tra Tài khoản lập tồn tại
        IF NOT EXISTS (SELECT 1 FROM QLTK WHERE MaTK = @MaTKLap AND TrangThai = N'Hoạt động')
        BEGIN
            SET @ErrorMsg = N'Tài khoản lập hóa đơn không tồn tại hoặc không hoạt động.';
            THROW 50003, @ErrorMsg, 1;
            RETURN;
        END

        -- Kiểm tra trạng thái hợp lệ (ví dụ)
        IF @TrangThai NOT IN (N'Chờ thanh toán', N'Đã thanh toán', N'Đã hủy') -- Các trạng thái hợp lệ
        BEGIN
             SET @ErrorMsg = N'Trạng thái hóa đơn không hợp lệ.';
             THROW 50004, @ErrorMsg, 1;
             RETURN;
        END

        UPDATE QLHoaDon
        SET NgayLap = @NgayLap,
            MaTKLap = @MaTKLap,
            MaKhachHang = @MaKhachHang,
            -- TongTien = @TongTien, -- Không cập nhật tổng tiền ở đây
            TrangThai = @TrangThai,
            HinhThucThanhToan = @HinhThucThanhToan,
            NgayCapNhat = GETDATE(),
            NguoiCapNhat = @NguoiCapNhat
        WHERE MaHoaDon = @MaHoaDon;

        -- Ghi log
        INSERT INTO AuditLog (MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (@NguoiCapNhat, N'QLHoaDon', @MaHoaDon, N'Cập nhật', N'Cập nhật thông tin hóa đơn');

        -- SELECT N'Cập nhật hóa đơn thành công!' AS Message; -- Không cần thiết nếu chỉ gọi từ code

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Stored Procedure: Cập nhật chi tiết hóa đơn bán hàng
-- Lưu ý: Cần tính lại chênh lệch số lượng để cập nhật kho và tổng tiền
ALTER PROCEDURE sp_CapNhatChiTietHoaDon
    @MaHoaDon INT,
    @MaVatLieu INT,
    @SoLuongMoi INT,
    @NguoiCapNhat INT -- Thêm người cập nhật để ghi log và kiểm tra quyền nếu cần
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @SoLuongCu INT;
    DECLARE @DonGiaBan DECIMAL(18,2);
    DECLARE @SoLuongThayDoi INT;
    DECLARE @TienThayDoi DECIMAL(18,2);
    DECLARE @CurrentTongTien DECIMAL(18,2);
    DECLARE @SoLuongTon INT;
    DECLARE @ErrorMsg NVARCHAR(MAX);

    BEGIN TRY
        -- Kiểm tra chi tiết hóa đơn có tồn tại không
        SELECT @SoLuongCu = SoLuong, @DonGiaBan = DonGia
        FROM ChiTietHoaDon
        WHERE MaHoaDon = @MaHoaDon AND MaVatLieu = @MaVatLieu;

        IF @SoLuongCu IS NULL OR @DonGiaBan IS NULL
        BEGIN
            SET @ErrorMsg = N'Chi tiết hóa đơn không tồn tại!';
            THROW 50001, @ErrorMsg, 1;
            RETURN;
        END

        -- Kiểm tra hóa đơn có ở trạng thái cho phép sửa không (ví dụ: chỉ "Chờ thanh toán")
        IF NOT EXISTS (SELECT 1 FROM QLHoaDon WHERE MaHoaDon = @MaHoaDon AND TrangThai = N'Chờ thanh toán')
        BEGIN
             SET @ErrorMsg = N'Chỉ có thể sửa chi tiết của hóa đơn đang "Chờ thanh toán"!' ;
             THROW 50002, @ErrorMsg, 1;
             RETURN;
        END

        -- Kiểm tra số lượng mới > 0
        IF @SoLuongMoi <= 0
        BEGIN
            SET @ErrorMsg = N'Số lượng mới phải lớn hơn 0.';
            THROW 50003, @ErrorMsg, 1;
            RETURN;
        END

        -- Tính toán sự thay đổi số lượng
        SET @SoLuongThayDoi = @SoLuongMoi - @SoLuongCu; -- >0 nếu tăng, <0 nếu giảm

        -- Kiểm tra tồn kho nếu số lượng tăng lên
        IF @SoLuongThayDoi > 0
        BEGIN
            SELECT @SoLuongTon = SoLuong FROM QLVatLieu WHERE MaVatLieu = @MaVatLieu;
            IF @SoLuongTon < @SoLuongThayDoi
            BEGIN
                SET @ErrorMsg = N'Số lượng vật liệu trong kho không đủ để tăng thêm!';
                THROW 50004, @ErrorMsg, 1;
                RETURN;
            END
        END

        -- Tính toán sự thay đổi thành tiền
        SET @TienThayDoi = @SoLuongThayDoi * @DonGiaBan;

        -- Bắt đầu Transaction
        BEGIN TRANSACTION;

        -- Cập nhật số lượng trong kho (tăng lại nếu giảm SL bán, giảm đi nếu tăng SL bán)
        UPDATE QLVatLieu
        SET SoLuong = SoLuong - @SoLuongThayDoi -- Trừ đi lượng thay đổi (nếu thay đổi là âm thì thành cộng)
        WHERE MaVatLieu = @MaVatLieu;

        -- Cập nhật chi tiết hóa đơn
        UPDATE ChiTietHoaDon
        SET SoLuong = @SoLuongMoi
        WHERE MaHoaDon = @MaHoaDon AND MaVatLieu = @MaVatLieu;

        -- Cập nhật tổng tiền hóa đơn
        SELECT @CurrentTongTien = ISNULL(TongTien, 0) FROM QLHoaDon WHERE MaHoaDon = @MaHoaDon;
        UPDATE QLHoaDon
        SET TongTien = dbo.fn_LamTronTien(@CurrentTongTien + @TienThayDoi, 0)
        WHERE MaHoaDon = @MaHoaDon;

        -- Ghi log (tùy chọn)
        INSERT INTO AuditLog (MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
        VALUES (@NguoiCapNhat, N'ChiTietHoaDon', @MaHoaDon, N'Cập nhật SL', CAST(@SoLuongCu AS VARCHAR), CAST(@SoLuongMoi AS VARCHAR), N'Cập nhật SL vật tư ' + CAST(@MaVatLieu AS VARCHAR));


        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Stored Procedure: Xóa chi tiết hóa đơn bán hàng
-- Lưu ý: Cần hoàn lại kho và cập nhật tổng tiền hóa đơn
ALTER PROCEDURE sp_XoaChiTietHoaDon
    @MaHoaDon INT,
    @MaVatLieu INT,
    @NguoiCapNhat INT -- Thêm người cập nhật
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @SoLuong INT;
    DECLARE @DonGia DECIMAL(18,2);
    DECLARE @ThanhTien DECIMAL(18,2);
    DECLARE @CurrentTongTien DECIMAL(18,2);
    DECLARE @ErrorMsg NVARCHAR(MAX);

    BEGIN TRY
        -- Kiểm tra chi tiết hóa đơn có tồn tại không và lấy thông tin
        SELECT @SoLuong = SoLuong, @DonGia = DonGia
        FROM ChiTietHoaDon
        WHERE MaHoaDon = @MaHoaDon AND MaVatLieu = @MaVatLieu;

        IF @SoLuong IS NULL OR @DonGia IS NULL
        BEGIN
             SET @ErrorMsg = N'Chi tiết hóa đơn không tồn tại!';
             THROW 50001, @ErrorMsg, 1;
             RETURN;
        END

        -- Kiểm tra hóa đơn có ở trạng thái cho phép sửa/xóa không
        IF NOT EXISTS (SELECT 1 FROM QLHoaDon WHERE MaHoaDon = @MaHoaDon AND TrangThai = N'Chờ thanh toán')
        BEGIN
             SET @ErrorMsg = N'Chỉ có thể xóa chi tiết của hóa đơn đang "Chờ thanh toán"!' ;
             THROW 50002, @ErrorMsg, 1;
             RETURN;
        END

        -- Tính thành tiền của chi tiết bị xóa
        SET @ThanhTien = @SoLuong * @DonGia;

        -- Bắt đầu Transaction
        BEGIN TRANSACTION;

        -- Hoàn lại số lượng vào kho
        UPDATE QLVatLieu
        SET SoLuong = SoLuong + @SoLuong
        WHERE MaVatLieu = @MaVatLieu;

        -- Xóa chi tiết hóa đơn
        DELETE FROM ChiTietHoaDon
        WHERE MaHoaDon = @MaHoaDon AND MaVatLieu = @MaVatLieu;

        -- Cập nhật tổng tiền hóa đơn (trừ đi thành tiền của chi tiết bị xóa)
        SELECT @CurrentTongTien = ISNULL(TongTien, 0) FROM QLHoaDon WHERE MaHoaDon = @MaHoaDon;
        UPDATE QLHoaDon
        SET TongTien = dbo.fn_LamTronTien(@CurrentTongTien - @ThanhTien, 0)
        WHERE MaHoaDon = @MaHoaDon;

        -- Ghi log (tùy chọn)
        INSERT INTO AuditLog (MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (@NguoiCapNhat, N'ChiTietHoaDon', @MaHoaDon, N'Xóa', N'Xóa vật tư ' + CAST(@MaVatLieu AS VARCHAR) + N' khỏi hóa đơn');

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Stored Procedure: Hủy hóa đơn bán hàng
-- Lưu ý: Hoàn lại kho cho TẤT CẢ chi tiết
ALTER PROCEDURE sp_HuyHoaDon
    @MaHoaDon INT,
    @NguoiCapNhat INT,
    @LyDoHuy NVARCHAR(255) = NULL -- Thêm lý do hủy (tùy chọn)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentTrangThai NVARCHAR(50);
    DECLARE @ErrorMsg NVARCHAR(MAX);

    BEGIN TRY
        -- Lấy trạng thái hiện tại
        SELECT @CurrentTrangThai = TrangThai FROM QLHoaDon WHERE MaHoaDon = @MaHoaDon;

        -- Kiểm tra hóa đơn có tồn tại không
        IF @CurrentTrangThai IS NULL
        BEGIN
            SET @ErrorMsg = N'Hóa đơn không tồn tại!';
            THROW 50001, @ErrorMsg, 1;
            RETURN;
        END

        -- Kiểm tra trạng thái hóa đơn có cho phép hủy không
        IF @CurrentTrangThai = N'Đã hủy'
        BEGIN
            SET @ErrorMsg = N'Hóa đơn đã bị hủy trước đó!';
            THROW 50002, @ErrorMsg, 1;
            RETURN;
        END
        IF @CurrentTrangThai = N'Đã thanh toán'
        BEGIN
            SET @ErrorMsg = N'Không thể hủy hóa đơn đã thanh toán!';
            THROW 50003, @ErrorMsg, 1;
            RETURN;
        END
        -- Chỉ cho phép hủy nếu đang ở trạng thái 'Chờ thanh toán' (hoặc trạng thái khác tùy quy trình)
        IF @CurrentTrangThai != N'Chờ thanh toán'
        BEGIN
             SET @ErrorMsg = N'Chỉ có thể hủy hóa đơn đang ở trạng thái "Chờ thanh toán".';
             THROW 50004, @ErrorMsg, 1;
             RETURN;
        END

        -- Bắt đầu Transaction
        BEGIN TRANSACTION;

        -- Cập nhật số lượng vật liệu trong kho (hoàn lại số lượng đã bán)
        -- Chỉ hoàn lại nếu có chi tiết
        IF EXISTS (SELECT 1 FROM ChiTietHoaDon WHERE MaHoaDon = @MaHoaDon)
        BEGIN
            UPDATE QLVatLieu
            SET SoLuong = ISNULL(v.SoLuong, 0) + cthd.SoLuong -- Dùng ISNULL cho chắc chắn
            FROM QLVatLieu v
            INNER JOIN ChiTietHoaDon cthd ON v.MaVatLieu = cthd.MaVatLieu
            WHERE cthd.MaHoaDon = @MaHoaDon;
        END

        -- Cập nhật trạng thái hóa đơn thành "Đã hủy"
        UPDATE QLHoaDon
        SET TrangThai = N'Đã hủy',
            NgayCapNhat = GETDATE(),
            NguoiCapNhat = @NguoiCapNhat,
            GhiChu = ISNULL(GhiChu + N'; ', '') + N'Lý do hủy: ' + ISNULL(@LyDoHuy, N'Không rõ') -- Thêm lý do vào ghi chú
        WHERE MaHoaDon = @MaHoaDon;

        -- Ghi log hoạt động với lý do hủy
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
        VALUES (GETDATE(), @NguoiCapNhat, N'QLHoaDon', @MaHoaDon, N'Hủy', @CurrentTrangThai, N'Đã hủy', @LyDoHuy);

        COMMIT TRANSACTION;
        -- SELECT N'Hủy hóa đơn thành công!' AS Message; -- Không cần thiết

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Stored Procedure: Xóa hóa đơn tạm (Trạng thái 'Chờ thanh toán')
-- Lưu ý: Hoàn lại kho
ALTER PROCEDURE sp_XoaHoaDonTam
    @MaHoaDon INT,
    @NguoiCapNhat INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(MAX);
    DECLARE @CurrentTrangThai NVARCHAR(50);

    BEGIN TRY
        -- Lấy trạng thái hiện tại
        SELECT @CurrentTrangThai = TrangThai FROM QLHoaDon WHERE MaHoaDon = @MaHoaDon;

        -- Kiểm tra hóa đơn có tồn tại không
        IF @CurrentTrangThai IS NULL
        BEGIN
            SET @ErrorMsg = N'Hóa đơn không tồn tại!';
            THROW 50001, @ErrorMsg, 1;
            RETURN;
        END

        -- Kiểm tra trạng thái hóa đơn (chỉ cho phép xóa hóa đơn tạm - Chờ thanh toán)
        IF @CurrentTrangThai != N'Chờ thanh toán'
        BEGIN
            SET @ErrorMsg = N'Chỉ có thể xóa hóa đơn đang ở trạng thái "Chờ thanh toán"!';
            THROW 50002, @ErrorMsg, 1;
            RETURN;
        END

        -- Bắt đầu transaction
        BEGIN TRANSACTION;

        -- Hoàn lại số lượng vật liệu trong kho nếu có chi tiết hóa đơn
        IF EXISTS (SELECT 1 FROM ChiTietHoaDon WHERE MaHoaDon = @MaHoaDon)
        BEGIN
            UPDATE QLVatLieu
            SET SoLuong = ISNULL(v.SoLuong, 0) + cthd.SoLuong
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
        VALUES (GETDATE(), @NguoiCapNhat, N'QLHoaDon', @MaHoaDon, N'Xóa tạm', N'Chờ thanh toán', N'Đã xóa', N'Xóa hóa đơn tạm');

        -- Commit transaction nếu thành công
        COMMIT TRANSACTION;
        -- SELECT N'Xóa hóa đơn tạm thành công!' AS Message; -- Không cần

    END TRY
    BEGIN CATCH
        -- Rollback nếu có lỗi
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW; -- Ném lỗi ra ngoài
    END CATCH
END;
GO


--=============================================================================
-- TRIGGERS
--=============================================================================

-- Trigger cập nhật số lượng khi nhập hàng (ĐƠN NHẬP)
-- Cần đảm bảo trigger này xử lý đúng, có thể cần sửa nếu SP Nhập hàng đã xử lý tồn kho
-- Nếu SP sp_NhapHang và sp_CapNhatDonNhapChiTiet đã cập nhật kho thì KHÔNG cần trigger này
-- Kiểm tra xem trigger có tồn tại không
IF OBJECT_ID ('trg_CapNhatSoLuongNhap', 'TR') IS NOT NULL
BEGIN
    -- Nếu tồn tại, có thể xóa hoặc vô hiệu hóa nếu logic đã chuyển vào SP
    PRINT 'Trigger trg_CapNhatSoLuongNhap exists. Consider disabling or dropping if SP handles inventory.';
    -- DISABLE TRIGGER trg_CapNhatSoLuongNhap ON ChiTietDonNhap;
    -- DROP TRIGGER trg_CapNhatSoLuongNhap;
END
ELSE
BEGIN
    -- Nếu chưa tồn tại và BẠN MUỐN dùng trigger để cập nhật kho khi nhập
    EXEC('
    CREATE TRIGGER trg_CapNhatSoLuongNhap
    ON ChiTietDonNhap
    AFTER INSERT -- Chỉ xử lý khi INSERT mới
    AS
    BEGIN
        SET NOCOUNT ON;

        -- Cập nhật số lượng vật liệu trong kho
        UPDATE QLVatLieu
        SET SoLuong = ISNULL(v.SoLuong, 0) + i.SoLuong, -- Dùng ISNULL
            DonGiaNhap = i.GiaNhap, -- Cập nhật giá nhập mới nhất khi nhập
            NgayCapNhat = GETDATE() -- Cập nhật ngày
            -- NguoiCapNhat = (SELECT TOP 1 NguoiCapNhat FROM QLDonNhap WHERE MaDonNhap = i.MaDonNhap) -- Lấy người cập nhật từ đơn nhập nếu cần
        FROM QLVatLieu v
        INNER JOIN inserted i ON v.MaVatLieu = i.MaVatLieu;
    END;
    ')
    PRINT 'Trigger trg_CapNhatSoLuongNhap created.';
END
GO

-- Trigger kiểm tra và cập nhật số lượng khi bán hàng (HÓA ĐƠN)
-- Trigger này vẫn cần thiết nếu SP thêm chi tiết không tự kiểm tra và trừ kho
-- Đảm bảo nó xử lý đúng các trường hợp INSERT, UPDATE, DELETE nếu cần
IF OBJECT_ID ('trg_KiemTraSoLuongBan', 'TR') IS NOT NULL
    ALTER TRIGGER trg_KiemTraSoLuongBan
    ON ChiTietHoaDon
    AFTER INSERT -- Chỉ xử lý INSERT, các SP cập nhật/xóa đã xử lý kho
    AS
    BEGIN
        SET NOCOUNT ON;
        DECLARE @ErrorMsg NVARCHAR(MAX);

        -- Kiểm tra xem số lượng yêu cầu có vượt quá số lượng trong kho không
        IF EXISTS (
            SELECT 1
            FROM QLVatLieu v
            INNER JOIN inserted i ON v.MaVatLieu = i.MaVatLieu
            WHERE ISNULL(v.SoLuong, 0) < i.SoLuong -- Dùng ISNULL
        )
        BEGIN
            -- Lấy thông tin vật liệu không đủ để báo lỗi rõ ràng hơn
            SELECT TOP 1 @ErrorMsg = N'Số lượng vật liệu "' + ISNULL(v.Ten, CAST(i.MaVatLieu AS VARCHAR)) + N'" trong kho (' + CAST(ISNULL(v.SoLuong,0) AS VARCHAR) + N') không đủ cho yêu cầu (' + CAST(i.SoLuong AS VARCHAR) + N')!'
            FROM QLVatLieu v
            INNER JOIN inserted i ON v.MaVatLieu = i.MaVatLieu
            WHERE ISNULL(v.SoLuong, 0) < i.SoLuong;

            RAISERROR (@ErrorMsg, 16, 1);
            ROLLBACK TRANSACTION;
            RETURN; -- Thêm return để chắc chắn dừng lại
        END
        ELSE
        BEGIN
            -- Cập nhật số lượng kho sau khi bán hàng thành công (chỉ cho INSERT)
            UPDATE QLVatLieu
            SET SoLuong = ISNULL(v.SoLuong, 0) - i.SoLuong
            FROM QLVatLieu v
            INNER JOIN inserted i ON v.MaVatLieu = i.MaVatLieu;
        END;
    END;
GO
PRINT 'Trigger trg_KiemTraSoLuongBan updated/checked.';
GO

--=============================================================================
-- CẬP NHẬT BẢNG (Thêm cột nếu thiếu)
--=============================================================================

-- Thêm cột TrangThai vào QLKH nếu chưa có
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'QLKH' AND COLUMN_NAME = 'TrangThai')
BEGIN
    ALTER TABLE QLKH
    ADD TrangThai NVARCHAR(20) CHECK (TrangThai IN (N'Hoạt động', N'Ngừng sử dụng')) DEFAULT N'Hoạt động'; -- Sửa tên trạng thái
    PRINT 'Added TrangThai column to QLKH.';

    -- Cập nhật giá trị mặc định cho các dòng đã có
    UPDATE QLKH SET TrangThai = N'Hoạt động' WHERE TrangThai IS NULL;
    PRINT 'Updated existing NULL TrangThai in QLKH to Hoạt động.';
END
ELSE
BEGIN
    -- Kiểm tra và sửa đổi CHECK constraint nếu cần (đổi 'Vô hiệu hóa' thành 'Ngừng sử dụng')
    -- Cần kiểm tra tên constraint trước khi xóa và thêm lại
    PRINT 'TrangThai column already exists in QLKH. Check constraint if needed.';
END
GO

--=============================================================================
-- STORED PROCEDURES CHO KHÁCH HÀNG (QLKH)
--=============================================================================

-- Stored Procedure: Vô hiệu hóa khách hàng (sử dụng tên trạng thái mới)
ALTER PROCEDURE sp_DisableKhachHang
    @MaKhachHang INT,
    @NguoiCapNhat INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(MAX);

    BEGIN TRY
        -- Kiểm tra khách hàng có tồn tại
        IF NOT EXISTS (SELECT 1 FROM QLKH WHERE MaKhachHang = @MaKhachHang)
        BEGIN
            SET @ErrorMsg = N'Khách hàng không tồn tại!';
            THROW 50003, @ErrorMsg, 1;
        END

        -- Kiểm tra xem khách hàng có hóa đơn "Chờ thanh toán" hay không
        IF EXISTS (SELECT 1 FROM QLHoaDon WHERE MaKhachHang = @MaKhachHang AND TrangThai = N'Chờ thanh toán')
        BEGIN
            SET @ErrorMsg = N'Không thể vô hiệu hóa khách hàng vì còn hóa đơn "Chờ thanh toán"!';
            THROW 50004, @ErrorMsg, 1;
        END

        -- Cập nhật trạng thái thành 'Ngừng sử dụng'
        UPDATE QLKH
        SET TrangThai = N'Ngừng sử dụng', -- SỬ DỤNG TRẠNG THÁI MỚI
            NguoiTao = @NguoiCapNhat, -- Nên cập nhật NguoiCapNhat nếu có cột đó
            NgayTao = GETDATE()       -- Nên cập nhật NgayCapNhat nếu có cột đó
        WHERE MaKhachHang = @MaKhachHang AND TrangThai = N'Hoạt động'; -- Chỉ cập nhật nếu đang hoạt động

        -- Ghi log hoạt động
        IF @@ROWCOUNT > 0 -- Chỉ ghi log nếu thực sự có cập nhật
        BEGIN
            INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
            VALUES (GETDATE(), @NguoiCapNhat, 'QLKH', @MaKhachHang, N'Ngừng sử dụng', N'Hoạt động', N'Ngừng sử dụng', N'Khách hàng bị vô hiệu hóa');
            -- SELECT N'Vô hiệu hóa khách hàng thành công!' AS Message; -- Không cần thiết
        END
        -- ELSE
        -- BEGIN
            -- SELECT N'Khách hàng không ở trạng thái hoạt động hoặc đã bị vô hiệu hóa trước đó.' AS Message;
        -- END

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Stored Procedure: Xóa khách hàng (giữ nguyên logic kiểm tra)
ALTER PROCEDURE sp_XoaKhachHang
    @MaKhachHang INT,
    @NguoiCapNhat INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(MAX);

    BEGIN TRY
        -- Kiểm tra khách hàng có tồn tại
        IF NOT EXISTS (SELECT 1 FROM QLKH WHERE MaKhachHang = @MaKhachHang)
        BEGIN
            SET @ErrorMsg = N'Khách hàng không tồn tại!';
            THROW 50003, @ErrorMsg, 1;
        END

        -- Kiểm tra xem khách hàng có hóa đơn "Chờ thanh toán" hay không
        IF EXISTS (SELECT 1 FROM QLHoaDon WHERE MaKhachHang = @MaKhachHang AND TrangThai = N'Chờ thanh toán')
        BEGIN
            SET @ErrorMsg = N'Không thể xóa khách hàng vì còn hóa đơn "Chờ thanh toán"!';
            THROW 50005, @ErrorMsg, 1;
        END

        -- Có thể thêm kiểm tra các ràng buộc khóa ngoại khác nếu cần

        -- Xóa thông tin khách hàng
        DELETE FROM QLKH
        WHERE MaKhachHang = @MaKhachHang;

        -- Ghi log hoạt động
        IF @@ROWCOUNT > 0 -- Chỉ ghi log nếu xóa thành công
        BEGIN
            INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
            VALUES (GETDATE(), @NguoiCapNhat, 'QLKH', @MaKhachHang, N'Xóa', N'Có dữ liệu', N'Đã xóa', N'Xóa khách hàng khỏi hệ thống');
            -- SELECT N'Xóa khách hàng thành công!' AS Message; -- Không cần thiết
        END

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Stored Procedure: Tìm kiếm khách hàng (Thêm TrangThai vào SELECT)
ALTER PROCEDURE sp_TimKiemKhachHang -- Đổi tên thành sp_TimKiemKhachHang để nhất quán
    @Keyword NVARCHAR(100) -- Giữ một keyword chung
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Search NVARCHAR(100) = '%' + @Keyword + '%';
    DECLARE @KeywordNum INT = TRY_CAST(@Keyword AS INT); -- Thử chuyển keyword sang số

    SELECT
        kh.MaKhachHang,
        kh.Ten,
        kh.GioiTinh,
        kh.NgaySinh,
        kh.SDT,
        kh.Email,
        kh.DiaChi,
        -- kh.TenNguoiTao, -- Bỏ nếu không có cột này
        tk.TenDangNhap AS NguoiTao, -- Lấy tên người tạo
        kh.NgayTao,
        kh.TrangThai -- << THÊM TRẠNG THÁI VÀO SELECT
    FROM QLKH kh
    LEFT JOIN QLTK tk ON kh.NguoiTao = tk.MaTK
    WHERE
        kh.TrangThai = N'Hoạt động' -- Chỉ tìm khách hàng hoạt động
        AND (
            kh.Ten COLLATE Latin1_General_CI_AI LIKE @Search
            OR kh.Email COLLATE Latin1_General_CI_AI LIKE @Search
            OR kh.SDT LIKE @Search -- Tìm SĐT gần đúng
            OR (@KeywordNum IS NOT NULL AND kh.MaKhachHang = @KeywordNum) -- Tìm chính xác theo Mã nếu keyword là số
           )
    ORDER BY
        -- Ưu tiên khớp mã, rồi đến tên, rồi đến ngày tạo
        CASE
            WHEN @KeywordNum IS NOT NULL AND kh.MaKhachHang = @KeywordNum THEN 0
            WHEN kh.Ten COLLATE Latin1_General_CI_AI LIKE @Keyword + '%' THEN 1 -- Khớp đầu tên
            WHEN kh.Ten COLLATE Latin1_General_CI_AI LIKE @Search THEN 2      -- Khớp chứa trong tên
            ELSE 3
        END,
        kh.Ten,
        kh.NgayTao DESC;
END;
GO


--=============================================================================
-- STORED PROCEDURES CHO VẬT LIỆU (QLVatLieu, QLLoaiVatLieu)
--=============================================================================

-- Stored Procedure: Thêm vật liệu mới (Đã có, xem lại và bổ sung nếu cần)
-- Đảm bảo các cột như HinhAnh, GhiChu đã tồn tại trong bảng QLVatLieu
ALTER PROCEDURE sp_ThemVatLieu
    @Ten NVARCHAR(100),
    @Loai INT,                -- Mã loại vật liệu (tham chiếu QLLoaiVatLieu)
    @DonGiaNhap DECIMAL(18,2),
    @DonGiaBan DECIMAL(18,2),
    @DonViTinh NVARCHAR(50),
    @MaKho INT,               -- Mã kho (tham chiếu Kho)
    @HinhAnh VARBINARY(MAX) = NULL,
    @GhiChu NVARCHAR(255) = NULL,
    @NguoiTao INT -- Thêm người tạo
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(MAX);

    BEGIN TRY
        -- Kiểm tra Loại vật liệu tồn tại
        IF NOT EXISTS (SELECT 1 FROM QLLoaiVatLieu WHERE MaLoaiVatLieu = @Loai AND TrangThai = N'Hoạt động')
        BEGIN
             SET @ErrorMsg = N'Loại vật liệu không tồn tại hoặc không hoạt động.';
             THROW 50011, @ErrorMsg, 1;
        END
        -- Kiểm tra Kho tồn tại
        IF NOT EXISTS (SELECT 1 FROM Kho WHERE MaKho = @MaKho) -- Giả sử có bảng Kho và trạng thái
        BEGIN
             SET @ErrorMsg = N'Kho không tồn tại.';
             THROW 50012, @ErrorMsg, 1;
        END
         -- Kiểm tra giá không âm
        IF @DonGiaNhap < 0 OR @DonGiaBan < 0
        BEGIN
             SET @ErrorMsg = N'Đơn giá không được âm.';
             THROW 50013, @ErrorMsg, 1;
        END

        INSERT INTO QLVatLieu (Ten, Loai, DonGiaNhap, DonGiaBan, DonViTinh, SoLuong, MaKho, TrangThai, HinhAnh, GhiChu, NgayTao, NguoiTao)
        VALUES (@Ten, @Loai, @DonGiaNhap, @DonGiaBan, @DonViTinh, 0, @MaKho, N'Hoạt động', @HinhAnh, @GhiChu, GETDATE(), @NguoiTao);

         -- Ghi log
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @NguoiTao, N'QLVatLieu', SCOPE_IDENTITY(), N'Thêm', N'Thêm vật liệu mới');

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Stored Procedure: Cập nhật vật liệu (Đã có, xem lại và bổ sung nếu cần)
ALTER PROCEDURE sp_CapNhatVatLieu
    @MaVatLieu INT,
    @Ten NVARCHAR(100),
    @Loai INT,
    @DonGiaNhap DECIMAL(18,2),
    @DonGiaBan DECIMAL(18,2),
    @DonViTinh NVARCHAR(50),
    @MaKho INT, -- Thêm MaKho nếu cho phép sửa kho
    @TrangThai NVARCHAR(50), -- Thêm trạng thái nếu cho sửa
    @HinhAnh VARBINARY(MAX) = NULL,
    @GhiChu NVARCHAR(255) = NULL,
    @NguoiCapNhat INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(MAX);
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM QLVatLieu WHERE MaVatLieu = @MaVatLieu)
        BEGIN
             SET @ErrorMsg = N'Vật tư không tồn tại!';
             THROW 50001, @ErrorMsg, 1;
        END
         -- Kiểm tra các giá trị đầu vào khác (Loai, Kho, TrangThai, Giá) nếu cần
        IF @DonGiaNhap < 0 OR @DonGiaBan < 0
        BEGIN
             SET @ErrorMsg = N'Đơn giá không được âm.';
             THROW 50013, @ErrorMsg, 1;
        END
         IF @TrangThai NOT IN (N'Hoạt động', N'Ngừng kinh doanh') -- Ví dụ trạng thái
         BEGIN
              SET @ErrorMsg = N'Trạng thái vật liệu không hợp lệ.';
              THROW 50014, @ErrorMsg, 1;
         END


        UPDATE QLVatLieu
        SET Ten = @Ten,
            Loai = @Loai,
            DonGiaNhap = @DonGiaNhap,
            DonGiaBan = @DonGiaBan,
            DonViTinh = @DonViTinh,
            MaKho = @MaKho,           -- Cập nhật kho
            TrangThai = @TrangThai,   -- Cập nhật trạng thái
            HinhAnh = ISNULL(@HinhAnh, HinhAnh), -- Chỉ cập nhật nếu có ảnh mới
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

-- Stored Procedure: Xóa vật liệu (Đã có, logic kiểm tra ràng buộc ổn)
ALTER PROCEDURE sp_XoaVatLieu
    @MaVatLieu INT,
    @NguoiCapNhat INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(MAX);
    BEGIN TRY
        -- Kiểm tra vật tư có tồn tại không
        IF NOT EXISTS (SELECT 1 FROM QLVatLieu WHERE MaVatLieu = @MaVatLieu)
        BEGIN
             SET @ErrorMsg = N'Vật tư không tồn tại!';
             THROW 50001, @ErrorMsg, 1;
        END

        -- Kiểm tra vật tư có trong đơn nhập hoặc hóa đơn không
        -- Nên kiểm tra cả đơn hàng chưa hoàn thành/chờ thanh toán
        IF EXISTS (SELECT 1 FROM ChiTietDonNhap ctdn JOIN QLDonNhap dn ON ctdn.MaDonNhap = dn.MaDonNhap WHERE ctdn.MaVatLieu = @MaVatLieu AND dn.TrangThai != N'Đã hủy') -- Ví dụ kiểm tra đơn nhập chưa hủy
            OR EXISTS (SELECT 1 FROM ChiTietHoaDon cthd JOIN QLHoaDon hd ON cthd.MaHoaDon = hd.MaHoaDon WHERE cthd.MaVatLieu = @MaVatLieu AND hd.TrangThai != N'Đã hủy') -- Ví dụ kiểm tra hóa đơn chưa hủy
        BEGIN
             SET @ErrorMsg = N'Không thể xóa vật tư vì đã có trong đơn nhập hoặc hóa đơn chưa hoàn tất/hủy!';
             THROW 50002, @ErrorMsg, 1;
        END

        -- Thay vì xóa cứng, nên cập nhật trạng thái thành 'Ngừng kinh doanh' hoặc tương tự
        UPDATE QLVatLieu
        SET TrangThai = N'Ngừng kinh doanh', -- Ví dụ
            NgayCapNhat = GETDATE(),
            NguoiCapNhat = @NguoiCapNhat
        WHERE MaVatLieu = @MaVatLieu;

        -- Ghi log là "Ngừng kinh doanh" thay vì "Xóa"
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
        SELECT GETDATE(), @NguoiCapNhat, N'QLVatLieu', @MaVatLieu, N'Ngừng kinh doanh', TrangThai, N'Ngừng kinh doanh', N'Ngừng kinh doanh vật tư'
        FROM QLVatLieu WHERE MaVatLieu = @MaVatLieu; -- Lấy trạng thái cũ để ghi log

        -- Nếu vẫn muốn xóa cứng (không khuyến khích nếu đã có giao dịch lịch sử)
        -- DELETE FROM QLVatLieu
        -- WHERE MaVatLieu = @MaVatLieu;
        -- INSERT INTO AuditLog ... (Hành động là 'Xóa')

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Stored Procedure: Lấy danh sách đơn vị tính (Đã có, OK)
ALTER PROCEDURE sp_LayDanhSachDonViTinh
AS
BEGIN
    SET NOCOUNT ON;
    SELECT DISTINCT DonViTinh
    FROM QLVatLieu
    WHERE DonViTinh IS NOT NULL AND DonViTinh != '' -- Loại bỏ rỗng
    ORDER BY DonViTinh;
END;
GO

-- Stored Procedure: Lấy danh sách loại vật liệu (Đã có, OK)
-- Đảm bảo bảng QLLoaiVatLieu có cột TrangThai
ALTER PROCEDURE sp_LayDanhSachLoaiVatLieu
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MaLoaiVatLieu, TenLoai
    FROM QLLoaiVatLieu
    WHERE TrangThai = N'Hoạt động'; -- Chỉ lấy loại đang hoạt động
END;
GO

-- Stored Procedure: Tìm kiếm Vật liệu theo Tên hoặc ID (Đã có, OK)
ALTER PROCEDURE sp_TimKiemVatLieuNameID
    @Keyword NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Search NVARCHAR(100) = '%' + @Keyword + '%';
    DECLARE @KeywordNum INT = TRY_CAST(@Keyword AS INT);

    SELECT
        vl.MaVatLieu,
        vl.Ten,
        vl.DonViTinh,
        vl.DonGiaNhap,
        vl.DonGiaBan,
        vl.SoLuong,
        vl.TrangThai, -- Thêm trạng thái
        lv.TenLoai,   -- Thêm tên loại
        k.TenKho      -- Thêm tên kho
    FROM QLVatLieu vl
    LEFT JOIN QLLoaiVatLieu lv ON vl.Loai = lv.MaLoaiVatLieu -- Join để lấy tên loại
    LEFT JOIN Kho k ON vl.MaKho = k.MaKho                 -- Join để lấy tên kho
    WHERE
        vl.TrangThai = N'Hoạt động' -- Chỉ tìm vật liệu hoạt động
        AND (
            vl.Ten COLLATE Latin1_General_CI_AI LIKE @Search
            OR (@KeywordNum IS NOT NULL AND vl.MaVatLieu = @KeywordNum)
           )
    ORDER BY vl.Ten;
END;
GO


--=============================================================================
-- STORED PROCEDURES CHO NHÀ CUNG CẤP (NCC)
--=============================================================================

-- Stored Procedure: Thêm Nhà Cung Cấp (Sửa lại để trả về ID nếu cần)
ALTER PROCEDURE sp_ThemNCC
    @TenNCC NVARCHAR(100),
    @DiaChi NVARCHAR(255) = NULL, -- Cho phép NULL
    @SDT NVARCHAR(15) = NULL,     -- Cho phép NULL
    @Email NVARCHAR(100) = NULL,   -- Cho phép NULL
    @NguoiTao INT,
    @NewMaNCC INT OUTPUT       -- Thêm tham số OUTPUT để trả về ID mới
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(MAX);
    BEGIN TRY
        -- Kiểm tra trùng lặp (Email hoặc SDT nếu chúng là duy nhất và không null)
        IF @Email IS NOT NULL AND EXISTS (SELECT 1 FROM NCC WHERE Email = @Email)
        BEGIN
            SET @ErrorMsg = N'Email nhà cung cấp đã tồn tại!';
            THROW 50001, @ErrorMsg, 1;
        END
        IF @SDT IS NOT NULL AND EXISTS (SELECT 1 FROM NCC WHERE SDT = @SDT)
        BEGIN
            SET @ErrorMsg = N'Số điện thoại nhà cung cấp đã tồn tại!';
            THROW 50002, @ErrorMsg, 1; -- Mã lỗi khác
        END

        -- Kiểm tra định dạng SDT nếu được cung cấp
        IF @SDT IS NOT NULL AND (@SDT NOT LIKE '[0-9]%' OR LEN(@SDT) NOT BETWEEN 9 AND 15) -- Sửa độ dài min nếu cần
        BEGIN
             SET @ErrorMsg = N'Số điện thoại không hợp lệ!';
             THROW 50003, @ErrorMsg, 1;
        END

        -- Thêm NCC
        INSERT INTO NCC (TenNCC, DiaChi, SDT, Email, NguoiTao, TrangThai, NgayTao) -- Thêm NgayTao
        VALUES (@TenNCC, @DiaChi, @SDT, @Email, @NguoiTao, N'Hoạt động', GETDATE());

        -- Lấy ID vừa tạo
        SET @NewMaNCC = SCOPE_IDENTITY();

        -- Ghi log
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @NguoiTao, N'NCC', @NewMaNCC, N'Thêm', N'Thêm nhà cung cấp mới');

        -- SELECT N'Thêm nhà cung cấp thành công!' AS Message, @NewMaNCC AS MaNCC; -- Trả về ID qua OUTPUT

    END TRY
    BEGIN CATCH
        SET @NewMaNCC = 0; -- Đặt ID về 0 nếu lỗi
        THROW;
    END CATCH
END;
GO

-- Stored Procedure: Sửa Nhà Cung Cấp (Sửa @NguoiCapNhat thành INT)
ALTER PROCEDURE sp_CapNhatNCC
    @MaNCC INT,
    @TenNCC NVARCHAR(100),
    @DiaChi NVARCHAR(255) = NULL,
    @SDT NVARCHAR(15) = NULL,
    @Email NVARCHAR(100) = NULL,
    @TrangThai NVARCHAR(50), -- Thêm trạng thái
    @NguoiCapNhat INT -- Đổi thành INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(MAX);
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM NCC WHERE MaNCC = @MaNCC)
        BEGIN
             SET @ErrorMsg = N'Nhà cung cấp không tồn tại!';
             THROW 50003, @ErrorMsg, 1;
        END

        IF @Email IS NOT NULL AND EXISTS (SELECT 1 FROM NCC WHERE Email = @Email AND MaNCC != @MaNCC)
        BEGIN
             SET @ErrorMsg = N'Email đã tồn tại ở nhà cung cấp khác!';
             THROW 50001, @ErrorMsg, 1;
        END
        IF @SDT IS NOT NULL AND EXISTS (SELECT 1 FROM NCC WHERE SDT = @SDT AND MaNCC != @MaNCC)
        BEGIN
             SET @ErrorMsg = N'Số điện thoại đã tồn tại ở nhà cung cấp khác!';
             THROW 50002, @ErrorMsg, 1;
        END

        IF @SDT IS NOT NULL AND (@SDT NOT LIKE '[0-9]%' OR LEN(@SDT) NOT BETWEEN 9 AND 15)
        BEGIN
             SET @ErrorMsg = N'Số điện thoại không hợp lệ!';
             THROW 50004, @ErrorMsg, 1; -- Mã lỗi khác
        END
         IF @TrangThai NOT IN (N'Hoạt động', N'Ngừng hợp tác') -- Ví dụ trạng thái NCC
         BEGIN
              SET @ErrorMsg = N'Trạng thái nhà cung cấp không hợp lệ.';
              THROW 50005, @ErrorMsg, 1;
         END

        UPDATE NCC
        SET TenNCC = @TenNCC,
            DiaChi = @DiaChi,
            SDT = @SDT,
            Email = @Email,
            TrangThai = @TrangThai, -- Cập nhật trạng thái
            NgayTao = GETDATE(), -- Nên là NgayCapNhat
            NguoiTao = @NguoiCapNhat -- Nên là NguoiCapNhat
        WHERE MaNCC = @MaNCC;

         -- Ghi log
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @NguoiCapNhat, N'NCC', @MaNCC, N'Cập nhật', N'Cập nhật thông tin nhà cung cấp');

        -- SELECT N'Cập nhật nhà cung cấp thành công!' AS Message; -- Không cần

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Stored Procedure: Xóa Nhà Cung Cấp (Thay bằng cập nhật trạng thái)
ALTER PROCEDURE sp_XoaNCC
    @MaNCC INT,
    @NguoiCapNhat INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(MAX);
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM NCC WHERE MaNCC = @MaNCC)
        BEGIN
             SET @ErrorMsg = N'Nhà cung cấp không tồn tại!';
             THROW 50003, @ErrorMsg, 1;
        END

        -- Kiểm tra đơn nhập chưa hoàn thành/hủy
        IF EXISTS (SELECT 1 FROM QLDonNhap WHERE MaNCC = @MaNCC AND TrangThai NOT IN (N'Đã hủy', N'Hoàn thành')) -- Ví dụ
        BEGIN
             SET @ErrorMsg = N'Không thể ngừng hợp tác với nhà cung cấp vì còn đơn nhập đang xử lý!';
             THROW 50004, @ErrorMsg, 1;
        END

        -- Cập nhật trạng thái thành 'Ngừng hợp tác'
        UPDATE NCC
        SET TrangThai = N'Ngừng hợp tác', -- Ví dụ trạng thái
            NgayTao = GETDATE(),       -- Nên là NgayCapNhat
            NguoiTao = @NguoiCapNhat   -- Nên là NguoiCapNhat
        WHERE MaNCC = @MaNCC AND TrangThai = N'Hoạt động'; -- Chỉ cập nhật nếu đang hoạt động

        -- Ghi log
        IF @@ROWCOUNT > 0
        BEGIN
            INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
            VALUES (GETDATE(), @NguoiCapNhat, N'NCC', @MaNCC, N'Ngừng hợp tác', N'Hoạt động', N'Ngừng hợp tác', N'Ngừng hợp tác với nhà cung cấp');
            -- SELECT N'Ngừng hợp tác với nhà cung cấp thành công!' AS Message;
        END
        -- ELSE
        -- BEGIN
            -- SELECT N'Nhà cung cấp không hoạt động hoặc đã ngừng hợp tác trước đó.' AS Message;
        -- END

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

--=============================================================================
-- STORED PROCEDURES CHO TÀI KHOẢN (QLTK)
--=============================================================================
-- Bảng PasswordResetToken (Nếu chưa có)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'PasswordResetToken')
BEGIN
CREATE TABLE [dbo].[PasswordResetToken](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MaTK] [int] NOT NULL,
	[Token] [nvarchar](255) NOT NULL,
	[NgayTao] [datetime] NOT NULL DEFAULT (getdate()),
	[NgayHetHan] [datetime] NOT NULL,
	[DaSuDung] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_PasswordResetToken] PRIMARY KEY CLUSTERED ([Id] ASC),
 CONSTRAINT [UQ_PasswordResetToken_Token] UNIQUE NONCLUSTERED ([Token] ASC),
 CONSTRAINT [FK_PasswordResetToken_QLTK] FOREIGN KEY([MaTK]) REFERENCES [dbo].[QLTK] ([MaTK]) ON DELETE CASCADE
)
END
GO
-- Bảng AuditLog (Nếu chưa có)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'AuditLog')
BEGIN
CREATE TABLE [dbo].[AuditLog](
	[AuditLogID] [int] IDENTITY(1,1) NOT NULL,
	[ThoiGian] [datetime] NOT NULL DEFAULT (getdate()),
	[MaTK] [int] NULL, -- User ID thực hiện hành động (có thể NULL nếu là hệ thống)
	[TenBang] [nvarchar](100) NULL, -- Tên bảng bị ảnh hưởng
	[MaBanGhi] [int] NULL, -- Khóa chính của bản ghi bị ảnh hưởng
	[HanhDong] [nvarchar](50) NULL, -- Ví dụ: Thêm, Sửa, Xóa, Đăng nhập, Đăng xuất
	[GiaTriCu] [nvarchar](max) NULL, -- Dữ liệu cũ (có thể là JSON hoặc XML)
	[GiaTriMoi] [nvarchar](max) NULL, -- Dữ liệu mới (có thể là JSON hoặc XML)
	[GhiChu] [nvarchar](max) NULL, -- Mô tả thêm
 CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED ([AuditLogID] ASC)
)
-- Thêm FK nếu cần
-- ALTER TABLE [dbo].[AuditLog] WITH CHECK ADD CONSTRAINT [FK_AuditLog_QLTK] FOREIGN KEY([MaTK]) REFERENCES [dbo].[QLTK] ([MaTK])
-- ALTER TABLE [dbo].[AuditLog] CHECK CONSTRAINT [FK_AuditLog_QLTK]
END
GO

-- SP Cập nhật mật khẩu (Đã có, OK)
ALTER PROCEDURE sp_CapNhatMatKhau
    @MaTK INT,
    @MatKhauMoi NVARCHAR(255),
    @NguoiCapNhat INT -- Thêm người thực hiện cập nhật (có thể là chính user hoặc admin)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @MatKhauHash NVARCHAR(255);
    DECLARE @MatKhauCu NVARCHAR(255); -- Lấy mật khẩu cũ để ghi log

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM QLTK WHERE MaTK = @MaTK)
            THROW 50001, N'Tài khoản không tồn tại!', 1;

        -- Lấy mật khẩu cũ
        SELECT @MatKhauCu = MatKhau FROM QLTK WHERE MaTK = @MaTK;

        -- Mã hóa mật khẩu mới
        SET @MatKhauHash = CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', @MatKhauMoi), 2);

        -- Cập nhật mật khẩu
        UPDATE QLTK
        SET MatKhau = @MatKhauHash
        WHERE MaTK = @MaTK;

        -- Ghi log hành động
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
        VALUES (GETDATE(), @NguoiCapNhat, N'QLTK', @MaTK, N'Cập nhật mật khẩu', @MatKhauCu, @MatKhauHash, N'Đổi mật khẩu');

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- SP Kiểm tra đăng nhập (Đã có, OK)
ALTER PROCEDURE [dbo].[sp_KiemTraDangNhap]
    @TenDangNhapOrEmail NVARCHAR(255), -- Nhận tên đăng nhập hoặc email
    @MatKhau NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @MatKhauHash NVARCHAR(255);
    SET @MatKhauHash = CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', @MatKhau), 2);

    SELECT
        MaTK,
        TenDangNhap,
        Email,
        ChucVu,
        TrangThai,
        Avatar -- Trả về Avatar nếu cần hiển thị sau đăng nhập
        -- Thêm các cột khác nếu cần thiết cho session người dùng
    FROM QLTK
    WHERE (TenDangNhap = @TenDangNhapOrEmail OR Email = @TenDangNhapOrEmail) -- Kiểm tra cả hai
    AND MatKhau = @MatKhauHash
    AND TrangThai = N'Hoạt động'; -- Chỉ cho đăng nhập nếu hoạt động
END;
GO

-- SP Tạo token reset mật khẩu (Đã có, OK)
ALTER PROCEDURE sp_TaoTokenResetMatKhau
    @Email NVARCHAR(100),
    @Token NVARCHAR(255) OUTPUT -- Trả token ra ngoài để gửi email
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @MaTK INT;
    DECLARE @NgayHetHan DATETIME;
    DECLARE @ErrorMsg NVARCHAR(MAX);

    -- Kiểm tra email tồn tại và tài khoản hoạt động
    SELECT @MaTK = MaTK
    FROM QLTK
    WHERE Email = @Email AND TrangThai = N'Hoạt động'; -- Thêm kiểm tra trạng thái

    IF @MaTK IS NULL
    BEGIN
         SET @ErrorMsg = N'Email không tồn tại hoặc tài khoản không hoạt động!';
         THROW 50010, @ErrorMsg, 1;
         RETURN;
    END

    -- Tạo token GUID
    SET @Token = NEWID();
    SET @NgayHetHan = DATEADD(HOUR, 1, GETDATE()); -- Token hết hạn sau 1 giờ (ngắn hơn an toàn hơn)

    -- Vô hiệu hóa các token cũ chưa sử dụng của người dùng này
    UPDATE PasswordResetToken SET DaSuDung = 1 WHERE MaTK = @MaTK AND DaSuDung = 0;

    -- Thêm token mới vào bảng
    INSERT INTO PasswordResetToken (MaTK, Token, NgayHetHan, DaSuDung)
    VALUES (@MaTK, @Token, @NgayHetHan, 0);

    -- Ghi log
    INSERT INTO AuditLog (MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
    VALUES (@MaTK, N'PasswordResetToken', @MaTK, N'Tạo token reset', N'Yêu cầu reset mật khẩu');
END;
GO

-- SP Xác nhận và reset mật khẩu (Đã có, OK)
ALTER PROCEDURE sp_ResetMatKhau
    @Token NVARCHAR(255),
    @MatKhauMoi NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @MaTK INT;
    DECLARE @MatKhauHash NVARCHAR(255);
    DECLARE @ErrorMsg NVARCHAR(MAX);

    -- Kiểm tra token hợp lệ
    SELECT @MaTK = MaTK
    FROM PasswordResetToken
    WHERE Token = @Token
    AND NgayHetHan > GETDATE()
    AND DaSuDung = 0;

    IF @MaTK IS NULL
    BEGIN
         SET @ErrorMsg = N'Token không hợp lệ, đã hết hạn hoặc đã được sử dụng!';
         THROW 50011, @ErrorMsg, 1;
         RETURN;
    END

    -- Mã hóa mật khẩu mới
    SET @MatKhauHash = CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', @MatKhauMoi), 2);

    -- Bắt đầu Transaction
    BEGIN TRANSACTION;
    BEGIN TRY
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
        VALUES (@MaTK, N'QLTK', @MaTK, N'Reset mật khẩu', N'Reset mật khẩu thành công qua token');

        COMMIT TRANSACTION;
        -- SELECT N'Reset mật khẩu thành công!' AS Message; -- Không cần thiết

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- SP Lấy vai trò người dùng (Đã có, OK)
ALTER PROCEDURE sp_LayVaiTroNguoiDung
    @MaTK INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        MaTK,
        TenDangNhap,
        Email, -- Thêm email
        ChucVu,
        TrangThai,
        Avatar -- Thêm avatar
    FROM QLTK
    WHERE MaTK = @MaTK;
END;
GO

-- SP Cập nhật thông tin tài khoản (Đã có, OK)
ALTER PROCEDURE sp_CapNhatThongTinTK
    @MaTK INT,
    @TenDangNhap NVARCHAR(50),
    @Email NVARCHAR(100),
    @SDT NVARCHAR(15) = NULL, -- Cho phép null
    @DiaChi NVARCHAR(255) = NULL, -- Cho phép null
    @Avatar VARBINARY(MAX) = NULL,
    @NguoiCapNhat INT -- ID của người thực hiện thay đổi (có thể là chính user đó hoặc admin)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(MAX);
    BEGIN TRY
        -- Kiểm tra tài khoản có tồn tại không
        IF NOT EXISTS (SELECT 1 FROM QLTK WHERE MaTK = @MaTK)
        BEGIN
             SET @ErrorMsg = N'Tài khoản không tồn tại!';
             THROW 50001, @ErrorMsg, 1;
        END

        -- Kiểm tra email hoặc tên đăng nhập đã tồn tại ở tài khoản khác chưa
        IF EXISTS (SELECT 1 FROM QLTK WHERE (Email = @Email OR TenDangNhap = @TenDangNhap) AND MaTK != @MaTK)
        BEGIN
             SET @ErrorMsg = N'Tên đăng nhập hoặc email đã được sử dụng bởi tài khoản khác!';
             THROW 50002, @ErrorMsg, 1;
        END

        -- Kiểm tra định dạng SDT nếu được cung cấp
        IF @SDT IS NOT NULL AND (@SDT NOT LIKE '[0-9]%' OR LEN(@SDT) NOT BETWEEN 9 AND 15)
        BEGIN
             SET @ErrorMsg = N'Số điện thoại không hợp lệ!';
             THROW 50003, @ErrorMsg, 1;
        END

        -- Cập nhật thông tin tài khoản
        UPDATE QLTK
        SET TenDangNhap = @TenDangNhap,
            Email = @Email,
            SDT = @SDT,       -- Cập nhật SDT
            DiaChi = @DiaChi, -- Cập nhật Địa chỉ
            Avatar = ISNULL(@Avatar, Avatar), -- Chỉ cập nhật Avatar nếu có giá trị mới được truyền vào
            NgayTao = GETDATE() -- Nên là NgayCapNhat nếu có cột đó
            -- Giữ nguyên MatKhau, ChucVu, TrangThai, NguoiTao trừ khi có SP riêng để sửa chúng
        WHERE MaTK = @MaTK;

        -- Ghi log
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @NguoiCapNhat, N'QLTK', @MaTK, N'Cập nhật thông tin', N'Cập nhật thông tin cá nhân');

        -- SELECT N'Cập nhật thông tin tài khoản thành công!' AS Message; -- Không cần

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- SP Thêm tài khoản (Đã có, sửa lại dùng tên cột đúng trong INSERT)
ALTER PROCEDURE [dbo].[sp_ThemTaiKhoan]
    @TenDangNhap NVARCHAR(50),
    @MatKhau NVARCHAR(255),
    @Email NVARCHAR(100),
    @SDT NVARCHAR(15) = NULL, -- Cho phép null
    @ChucVu NVARCHAR(50),
    @GHICHU NVARCHAR(255) = NULL, -- Cho phép null
    @DiaChi NVARCHAR(255) = NULL, -- Thêm địa chỉ
    @NguoiTao INT -- MaTK của người tạo
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ChucVuNguoiTao NVARCHAR(50);
    DECLARE @ErrorMsg NVARCHAR(MAX);

    BEGIN TRY
        -- Kiểm tra quyền của người tạo
        SELECT @ChucVuNguoiTao = ChucVu FROM QLTK WHERE MaTK = @NguoiTao AND TrangThai = N'Hoạt động';
        IF @ChucVuNguoiTao IS NULL
            THROW 50000, N'Người tạo không tồn tại hoặc không hoạt động!', 1;
        IF @ChucVuNguoiTao NOT IN (N'Quản lý', N'Admin') -- Chỉ Admin hoặc Quản lý được tạo
            THROW 50001, N'Bạn không có quyền tạo tài khoản!', 1;
        IF @ChucVuNguoiTao = N'Quản lý' AND @ChucVu NOT IN (N'Nhân viên', N'Quản lý') -- QL chỉ tạo NV hoặc QL khác? Tùy logic
             THROW 50002, N'Quản lý chỉ có thể tạo tài khoản nhân viên (hoặc quản lý khác tùy cấu hình)!', 1;
        IF @ChucVuNguoiTao = N'Admin' AND @ChucVu NOT IN (N'Nhân viên', N'Quản lý', N'Admin') -- Admin tạo được mọi loại
             THROW 50006, N'Chức vụ tạo mới không hợp lệ!', 1;


        -- Kiểm tra Tên đăng nhập và Email đã tồn tại chưa
         IF EXISTS (SELECT 1 FROM QLTK WHERE TenDangNhap = @TenDangNhap)
             THROW 50003, N'Tên đăng nhập đã tồn tại!', 1;
         IF EXISTS (SELECT 1 FROM QLTK WHERE Email = @Email)
             THROW 50004, N'Email đã tồn tại!', 1;

         -- Kiểm tra định dạng SDT nếu có
         IF @SDT IS NOT NULL AND (@SDT NOT LIKE '[0-9]%' OR LEN(@SDT) NOT BETWEEN 9 AND 15)
              THROW 50005, N'Số điện thoại không hợp lệ!', 1;


        -- Mã hóa mật khẩu
        DECLARE @MatKhauHash NVARCHAR(255) = CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', @MatKhau), 2);

        -- Thêm tài khoản (đảm bảo tên cột trong VALUES khớp với tên cột trong INSERT INTO)
        INSERT INTO QLTK (
            TenDangNhap, MatKhau, Email, SDT, ChucVu, TrangThai, NguoiTao, GHICHU, DiaChi, NgayTao
            -- Thiếu cột DiaChi trong VALUES ở script gốc của bạn
        )
        VALUES (
            @TenDangNhap, @MatKhauHash, @Email, @SDT, @ChucVu, N'Hoạt động', @NguoiTao, @GHICHU, @DiaChi, GETDATE()
            -- Thêm @DiaChi vào đây
        );

        -- Ghi log
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @NguoiTao, N'QLTK', SCOPE_IDENTITY(), N'Thêm', N'Tạo tài khoản mới: ' + @TenDangNhap);

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- SP Xóa tài khoản (Đã có, OK - logic kiểm tra ràng buộc tốt)
ALTER PROCEDURE sp_XoaTaiKhoan
    @MaTK INT,              -- Mã tài khoản cần xóa
    @NguoiCapNhat INT       -- Người thực hiện xóa
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(MAX);

    BEGIN TRY
        -- Kiểm tra tài khoản có tồn tại không
        IF NOT EXISTS (SELECT 1 FROM QLTK WHERE MaTK = @MaTK)
        BEGIN
             SET @ErrorMsg = N'Tài khoản không tồn tại!';
             THROW 50001, @ErrorMsg, 1;
        END

        -- Không cho phép xóa chính tài khoản đang thực hiện thao tác
        IF @MaTK = @NguoiCapNhat
        BEGIN
             SET @ErrorMsg = N'Bạn không thể xóa chính tài khoản của mình!';
             THROW 50003, @ErrorMsg, 1;
        END

        -- Kiểm tra tài khoản có đang được tham chiếu trong các bảng khác không
        -- (Thêm các kiểm tra khác nếu cần)
        IF EXISTS (SELECT 1 FROM QLHoaDon WHERE MaTKLap = @MaTK OR NguoiCapNhat = @MaTK) -- Kiểm tra cả người cập nhật HD
            OR EXISTS (SELECT 1 FROM QLDonNhap WHERE MaTK = @MaTK OR NguoiCapNhat = @MaTK) -- Kiểm tra cả người cập nhật ĐN
            OR EXISTS (SELECT 1 FROM QLKH WHERE NguoiTao = @MaTK) -- Không nên có NguoiCapNhat ở KH
            OR EXISTS (SELECT 1 FROM QLVatLieu WHERE NguoiTao = @MaTK OR NguoiCapNhat = @MaTK)
            OR EXISTS (SELECT 1 FROM NCC WHERE NguoiTao = @MaTK) -- Không nên có NguoiCapNhat ở NCC
            OR EXISTS (SELECT 1 FROM AuditLog WHERE MaTK = @MaTK) -- Có thể giữ lại log
        BEGIN
            SET @ErrorMsg = N'Không thể xóa tài khoản vì đã có giao dịch hoặc dữ liệu liên quan!';
            THROW 50002, @ErrorMsg, 1;
        END

        -- Bắt đầu transaction
        BEGIN TRANSACTION;

        -- Xóa token reset mật khẩu liên quan (nếu có)
        DELETE FROM PasswordResetToken WHERE MaTK = @MaTK;

        -- Thay vì xóa cứng, nên cập nhật trạng thái thành 'Vô hiệu hóa' hoặc 'Đã xóa'
        UPDATE QLTK
        SET TrangThai = N'Đã xóa', -- Ví dụ trạng thái
            TenDangNhap = TenDangNhap + '_deleted_' + CAST(@MaTK AS VARCHAR), -- Đổi tên đăng nhập để tránh trùng lặp sau này
            Email = Email + '_deleted_' + CAST(@MaTK AS VARCHAR) -- Đổi email
            -- Có thể xóa các thông tin nhạy cảm khác nếu cần
        WHERE MaTK = @MaTK;

        -- Ghi log là "Vô hiệu hóa" hoặc "Đánh dấu xóa"
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
        SELECT GETDATE(), @NguoiCapNhat, N'QLTK', @MaTK, N'Đánh dấu xóa', TrangThai, N'Đã xóa', N'Đánh dấu xóa tài khoản'
        FROM QLTK WHERE MaTK = @MaTK; -- Lấy trạng thái cũ

        -- Nếu vẫn muốn xóa cứng (không khuyến khích)
        -- DELETE FROM QLTK WHERE MaTK = @MaTK;
        -- INSERT INTO AuditLog ... (Hành động là 'Xóa')

        COMMIT TRANSACTION;
        -- SELECT N'Xóa (đánh dấu) tài khoản thành công!' AS Message;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- SP Sửa tài khoản (Thêm kiểm tra và sửa lỗi logic)
ALTER PROCEDURE sp_SuaTaiKhoan
    @MaTK INT,
    @TenDangNhap NVARCHAR(50),
    @Email NVARCHAR(100),
    @SDT NVARCHAR(15) = NULL,
    @ChucVu NVARCHAR(50),
    @TrangThai NVARCHAR(20),
    @GHICHU NVARCHAR(255) = NULL, -- Sửa tên tham số GHICHU
    @DiaChi NVARCHAR(255) = NULL, -- Thêm địa chỉ
    @NguoiCapNhat INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(MAX);
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM QLTK WHERE MaTK = @MaTK)
        BEGIN
             SET @ErrorMsg = N'Tài khoản không tồn tại!';
             THROW 50001, @ErrorMsg, 1;
        END

         -- Kiểm tra email hoặc tên đăng nhập đã tồn tại ở tài khoản khác chưa
        IF EXISTS (SELECT 1 FROM QLTK WHERE (Email = @Email OR TenDangNhap = @TenDangNhap) AND MaTK != @MaTK)
        BEGIN
             SET @ErrorMsg = N'Tên đăng nhập hoặc email đã được sử dụng bởi tài khoản khác!';
             THROW 50002, @ErrorMsg, 1;
        END

         -- Kiểm tra định dạng SDT nếu có
         IF @SDT IS NOT NULL AND (@SDT NOT LIKE '[0-9]%' OR LEN(@SDT) NOT BETWEEN 9 AND 15)
              THROW 50003, N'Số điện thoại không hợp lệ!', 1;

         -- Kiểm tra Chức vụ và Trạng thái hợp lệ
         IF @ChucVu NOT IN (N'Admin', N'Quản lý', N'Nhân viên') -- Ví dụ các chức vụ
             THROW 50004, N'Chức vụ không hợp lệ!', 1;
         IF @TrangThai NOT IN (N'Hoạt động', N'Vô hiệu hóa', N'Đã xóa') -- Ví dụ các trạng thái
              THROW 50005, N'Trạng thái không hợp lệ!', 1;

         -- Không cho phép sửa tài khoản của chính mình thành trạng thái không hoạt động
         IF @MaTK = @NguoiCapNhat AND @TrangThai != N'Hoạt động'
              THROW 50006, N'Bạn không thể tự vô hiệu hóa tài khoản của mình!', 1;


        UPDATE QLTK
        SET TenDangNhap = @TenDangNhap,
            Email = @Email,
            SDT = @SDT,
            ChucVu = @ChucVu,
            TrangThai = @TrangThai,
            GHICHU = @GHICHU, -- Dùng tên tham số đúng
            DiaChi = @DiaChi, -- Thêm cập nhật địa chỉ
            NgayTao = GETDATE() -- Nên là NgayCapNhat
            -- MatKhau không được sửa ở đây
        WHERE MaTK = @MaTK;

        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @NguoiCapNhat, N'QLTK', @MaTK, N'Sửa', N'Cập nhật thông tin tài khoản');
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO


--=============================================================================
-- STORED PROCEDURES CHO ĐƠN NHẬP HÀNG (QLDONNHAP, CHITIETDONNHAP)
--=============================================================================

-- SP Thêm Đơn Nhập Hàng (Đã có sp_NhapHang, sử dụng JSON và TVP không cần thiết cùng lúc)
-- Giữ lại sp_NhapHang dùng JSON vì nó đã có sẵn và hoạt động
ALTER PROCEDURE [dbo].[sp_NhapHang]
    @NgayNhap DATE,
    @MaNCC INT,
    @MaTK INT,                       -- Mã tài khoản người tạo đơn
    @GhiChu NVARCHAR(255) = NULL,
    @ChiTietNhap NVARCHAR(MAX),      -- JSON string
    @NguoiThucHien INT               -- Thống nhất tên là NguoiThucHien hoặc NguoiCapNhat
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(MAX);
    DECLARE @MaDonNhap INT;

    -- Bắt đầu Transaction
    BEGIN TRANSACTION;
    BEGIN TRY
        -- Kiểm tra NCC tồn tại và hoạt động
        IF NOT EXISTS (SELECT 1 FROM NCC WHERE MaNCC = @MaNCC AND TrangThai = N'Hoạt động')
        BEGIN
             SET @ErrorMsg = N'Nhà cung cấp không tồn tại hoặc không hoạt động!';
             THROW 50001, @ErrorMsg, 1;
        END

        -- Kiểm tra Tài khoản tạo tồn tại và hoạt động
        IF NOT EXISTS (SELECT 1 FROM QLTK WHERE MaTK = @MaTK AND TrangThai = N'Hoạt động')
        BEGIN
             SET @ErrorMsg = N'Tài khoản người tạo không tồn tại hoặc bị khóa!';
             THROW 50002, @ErrorMsg, 1;
        END

        -- Thêm Header Đơn Nhập
        INSERT INTO QLDonNhap (NgayNhap, MaNCC, MaTK, TrangThai, GhiChu, NgayCapNhat, NguoiCapNhat)
        VALUES (@NgayNhap, @MaNCC, @MaTK, N'Hoàn thành', @GhiChu, GETDATE(), @NguoiThucHien); -- Mặc định là Hoàn thành? Hay Chờ xử lý?

        SET @MaDonNhap = SCOPE_IDENTITY();

        -- Parse JSON và kiểm tra chi tiết
        IF @ChiTietNhap IS NULL OR @ChiTietNhap = '' OR NOT ISJSON(@ChiTietNhap) > 0
        BEGIN
             SET @ErrorMsg = N'Dữ liệu chi tiết nhập hàng không hợp lệ (không phải JSON hoặc rỗng)!';
             THROW 50003, @ErrorMsg, 1;
        END

        -- Tạo bảng tạm để kiểm tra trước khi INSERT và UPDATE
        DECLARE @TempChiTiet TABLE (MaVatLieu INT PRIMARY KEY, SoLuong INT, GiaNhap DECIMAL(18,2));

        INSERT INTO @TempChiTiet (MaVatLieu, SoLuong, GiaNhap)
        SELECT
            JSON_VALUE(value, '$.MaVatLieu'),
            JSON_VALUE(value, '$.SoLuong'),
            JSON_VALUE(value, '$.GiaNhap')
        FROM OPENJSON(@ChiTietNhap);

        -- Kiểm tra chi tiết có trống không sau khi parse
        IF NOT EXISTS (SELECT 1 FROM @TempChiTiet)
        BEGIN
             SET @ErrorMsg = N'Danh sách chi tiết nhập hàng trống sau khi phân tích JSON!';
             THROW 50006, @ErrorMsg, 1;
        END

        -- Kiểm tra vật liệu tồn tại, hoạt động, số lượng, giá nhập
        IF EXISTS (
            SELECT 1 FROM @TempChiTiet t
            LEFT JOIN QLVatLieu v ON t.MaVatLieu = v.MaVatLieu
            WHERE v.MaVatLieu IS NULL OR v.TrangThai != N'Hoạt động'
           )
        BEGIN
             SET @ErrorMsg = N'Có vật liệu không tồn tại hoặc không hoạt động trong danh sách chi tiết!';
             THROW 50004, @ErrorMsg, 1;
        END

        IF EXISTS (SELECT 1 FROM @TempChiTiet WHERE SoLuong <= 0 OR GiaNhap < 0)
        BEGIN
            SET @ErrorMsg = N'Số lượng hoặc giá nhập trong danh sách chi tiết không hợp lệ!';
            THROW 50005, @ErrorMsg, 1;
        END


        -- Thêm chi tiết vào bảng ChiTietDonNhap
        INSERT INTO ChiTietDonNhap (MaDonNhap, MaVatLieu, SoLuong, GiaNhap)
        SELECT @MaDonNhap, MaVatLieu, SoLuong, GiaNhap
        FROM @TempChiTiet;

        -- Cập nhật tồn kho và giá nhập mới nhất trong QLVatLieu
        UPDATE QLVatLieu
        SET SoLuong = ISNULL(v.SoLuong, 0) + t.SoLuong,
            DonGiaNhap = t.GiaNhap, -- Cập nhật giá nhập mới nhất
            NgayCapNhat = GETDATE(),
            NguoiCapNhat = @NguoiThucHien
        FROM QLVatLieu v
        INNER JOIN @TempChiTiet t ON v.MaVatLieu = t.MaVatLieu;

        -- Ghi log
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @NguoiThucHien, N'QLDonNhap', @MaDonNhap, N'Thêm', N'Thêm đơn nhập hàng mới');

        -- Commit Transaction
        COMMIT TRANSACTION;

        -- Trả về thông báo và ID đơn nhập mới
        SELECT N'Nhập hàng thành công!' AS Message, @MaDonNhap AS MaDonNhap;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW; -- Ném lại lỗi để C# bắt
    END CATCH
END;
GO

-- SP Cập nhật Đơn Nhập Hàng (Đã có sp_CapNhatDonNhapChiTiet dùng TVP, giữ lại SP này)
-- Lưu ý: SP này dùng TVP, cần tạo TYPE dbo.ChiTietDonNhapType trước
-- SP này nên được gọi từ BUS sau khi đã tính toán xong inventoryChanges
ALTER PROCEDURE [dbo].[sp_CapNhatDonNhapChiTiet]
    @MaDonNhap INT,
    @NgayNhap DATE,
    -- @MaNCC INT, -- Không nên cho sửa NCC
    @MaTK INT, -- Người nhập mới (nếu cho sửa)
    @TrangThai NVARCHAR(50),
    @GhiChu NVARCHAR(255) = NULL,
    @UserId INT, -- Người thực hiện cập nhật
    @ChiTietCapNhat dbo.ChiTietDonNhapType READONLY -- TVP chi tiết mới
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(MAX);
    DECLARE @CurrentTrangThai NVARCHAR(50);
    DECLARE @OriginalChiTiet TABLE (MaVatLieu INT PRIMARY KEY, SoLuong INT); -- Bảng tạm lưu chi tiết gốc

    -- Lấy trạng thái hiện tại và kiểm tra tồn tại
    SELECT @CurrentTrangThai = TrangThai FROM QLDonNhap WHERE MaDonNhap = @MaDonNhap;
    IF @CurrentTrangThai IS NULL
        THROW 50001, N'Đơn nhập hàng không tồn tại.', 1;

    -- Kiểm tra logic chuyển đổi trạng thái (ví dụ: không sửa đơn đã hủy)
    IF @CurrentTrangThai = N'Đã hủy' AND @TrangThai != N'Đã hủy'
         THROW 50002, N'Không thể thay đổi trạng thái của đơn hàng đã hủy.', 1;
    IF @TrangThai NOT IN (N'Hoàn thành', N'Đang xử lý', N'Đã hủy') -- Các trạng thái hợp lệ
          THROW 50006, N'Trạng thái cập nhật không hợp lệ.', 1;


    -- Lấy chi tiết gốc
    INSERT INTO @OriginalChiTiet (MaVatLieu, SoLuong)
    SELECT MaVatLieu, SoLuong FROM ChiTietDonNhap WHERE MaDonNhap = @MaDonNhap;

    -- Bắt đầu Transaction
    BEGIN TRANSACTION;
    BEGIN TRY
        -- 1. Cập nhật Header
        UPDATE QLDonNhap
        SET NgayNhap = @NgayNhap, TrangThai = @TrangThai, GhiChu = @GhiChu,
            MaTK = @MaTK, NgayCapNhat = GETDATE(), NguoiCapNhat = @UserId
        WHERE MaDonNhap = @MaDonNhap;

        -- 2. Xóa chi tiết không còn trong TVP
        DELETE ctdn FROM ChiTietDonNhap ctdn LEFT JOIN @ChiTietCapNhat tvp ON ctdn.MaVatLieu = tvp.MaVatLieu AND ctdn.MaDonNhap = @MaDonNhap
        WHERE ctdn.MaDonNhap = @MaDonNhap AND tvp.MaVatLieu IS NULL;

        -- 3. Cập nhật chi tiết hiện có
        UPDATE ctdn SET ctdn.SoLuong = tvp.SoLuong, ctdn.GiaNhap = tvp.GiaNhap
        FROM ChiTietDonNhap ctdn INNER JOIN @ChiTietCapNhat tvp ON ctdn.MaVatLieu = tvp.MaVatLieu AND ctdn.MaDonNhap = @MaDonNhap
        WHERE ctdn.SoLuong != tvp.SoLuong OR ctdn.GiaNhap != tvp.GiaNhap;

        -- 4. Thêm chi tiết mới
        INSERT INTO ChiTietDonNhap (MaDonNhap, MaVatLieu, SoLuong, GiaNhap)
        SELECT @MaDonNhap, tvp.MaVatLieu, tvp.SoLuong, tvp.GiaNhap
        FROM @ChiTietCapNhat tvp LEFT JOIN ChiTietDonNhap ctdn ON tvp.MaVatLieu = ctdn.MaVatLieu AND ctdn.MaDonNhap = @MaDonNhap
        WHERE ctdn.MaVatLieu IS NULL;

        -- 5. Cập nhật tồn kho (logic tính chênh lệch như trong SP gốc)
        DECLARE @InventoryChanges TABLE (MaVatLieu INT PRIMARY KEY, ChangeAmount INT);
        INSERT INTO @InventoryChanges (MaVatLieu, ChangeAmount)
        SELECT tvp.MaVatLieu, tvp.SoLuong - ISNULL(orig.SoLuong, 0)
        FROM @ChiTietCapNhat tvp LEFT JOIN @OriginalChiTiet orig ON tvp.MaVatLieu = orig.MaVatLieu;
        INSERT INTO @InventoryChanges (MaVatLieu, ChangeAmount)
        SELECT orig.MaVatLieu, -orig.SoLuong
        FROM @OriginalChiTiet orig LEFT JOIN @ChiTietCapNhat tvp ON orig.MaVatLieu = tvp.MaVatLieu
        WHERE tvp.MaVatLieu IS NULL;

        UPDATE vl SET vl.SoLuong = ISNULL(vl.SoLuong, 0) + ic.ChangeAmount, vl.NgayCapNhat = GETDATE(), vl.NguoiCapNhat = @UserId
        FROM QLVatLieu vl INNER JOIN @InventoryChanges ic ON vl.MaVatLieu = ic.MaVatLieu WHERE ic.ChangeAmount != 0;

        -- (Tùy chọn) Kiểm tra tồn kho âm
        -- IF EXISTS (...) THROW ...;

        -- Ghi log
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @UserId, N'QLDonNhap', @MaDonNhap, N'Cập nhật', N'Cập nhật đơn nhập hàng và chi tiết');

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO


-- SP Tìm kiếm Đơn Nhập Hàng (Đã có, giữ lại SP này)
ALTER PROCEDURE [dbo].[sp_TimKiemQLDonNhap]
    @Keyword NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Search NVARCHAR(100) = '%' + @Keyword + '%';
    DECLARE @KeywordNum INT = TRY_CAST(@Keyword AS INT);

    SELECT
        dn.MaDonNhap,
        dn.NgayNhap,
        ncc.TenNCC AS TenNhaCungCap, -- Trả về TenNhaCungCap
        dn.TrangThai,
        dn.GhiChu,
        tk.TenDangNhap AS NguoiTao, -- Thêm người tạo
        tkUpdate.TenDangNhap AS NguoiCapNhatTen, -- Thêm người cập nhật
        dn.NgayCapNhat -- Thêm ngày cập nhật
    FROM QLDonNhap dn
    LEFT JOIN NCC ncc ON dn.MaNCC = ncc.MaNCC
    LEFT JOIN QLTK tk ON dn.MaTK = tk.MaTK
    LEFT JOIN QLTK tkUpdate ON dn.NguoiCapNhat = tkUpdate.MaTK
    WHERE
        (@KeywordNum IS NOT NULL AND dn.MaDonNhap = @KeywordNum)
        OR ncc.TenNCC COLLATE Latin1_General_CI_AI LIKE @Search
        OR tk.TenDangNhap COLLATE Latin1_General_CI_AI LIKE @Search -- Tìm theo người tạo
        OR dn.TrangThai COLLATE Latin1_General_CI_AI LIKE @Search  -- Tìm theo trạng thái
        OR dn.GhiChu COLLATE Latin1_General_CI_AI LIKE @Search     -- Tìm theo ghi chú
    ORDER BY
        -- Ưu tiên khớp mã, rồi đến tên NCC, rồi đến ngày nhập
         CASE
            WHEN @KeywordNum IS NOT NULL AND dn.MaDonNhap = @KeywordNum THEN 0
            WHEN ncc.TenNCC COLLATE Latin1_General_CI_AI LIKE @Keyword + '%' THEN 1
            WHEN ncc.TenNCC COLLATE Latin1_General_CI_AI LIKE @Search THEN 2
            ELSE 3
        END,
        dn.NgayNhap DESC,
        dn.MaDonNhap DESC;
END;
GO


ALTER PROCEDURE [dbo].[sp_CapNhatDonNhap]
    @MaDonNhap INT,
    @NgayNhap DATE,
    @MaNCC INT,
    @MaTK INT,
    @GhiChu NVARCHAR(255) = NULL,
    @ChiTietNhap NVARCHAR(MAX), -- JSON string
    @NguoiCapNhat INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(MAX);
    DECLARE @CurrentTrangThai NVARCHAR(50);

    -- Bảng tạm lưu chi tiết mới từ JSON
    DECLARE @TempChiTiet TABLE (MaVatLieu INT PRIMARY KEY, SoLuong INT, GiaNhap DECIMAL(18,2));
    -- Bảng tạm lưu chi tiết gốc
    DECLARE @OriginalChiTiet TABLE (MaVatLieu INT PRIMARY KEY, SoLuong INT, GiaNhap DECIMAL(18,2));
    -- Bảng tạm lưu thay đổi tồn kho
    DECLARE @InventoryChanges TABLE (MaVatLieu INT PRIMARY KEY, ChangeAmount INT);

    BEGIN TRY
        --=====================================================================
        -- KIỂM TRA ĐẦU VÀO VÀ TRẠNG THÁI
        --=====================================================================
        SELECT @CurrentTrangThai = TrangThai FROM QLDonNhap WHERE MaDonNhap = @MaDonNhap;
        IF @CurrentTrangThai IS NULL THROW 50001, N'Đơn nhập không tồn tại!', 1;
        -- Cho phép cập nhật nếu là 'Hoàn thành' hoặc 'Đang xử lý'? Tùy nghiệp vụ. Bỏ kiểm tra này nếu muốn linh hoạt hơn.
        -- IF @CurrentTrangThai != N'Đang xử lý' THROW 50002, N'Chỉ có thể cập nhật đơn nhập ở trạng thái "Đang xử lý"!', 1;
        IF NOT EXISTS (SELECT 1 FROM NCC WHERE MaNCC = @MaNCC AND TrangThai = N'Hoạt động') THROW 50003, N'Nhà cung cấp không tồn tại hoặc không hoạt động!', 1;
        IF NOT EXISTS (SELECT 1 FROM QLTK WHERE MaTK = @MaTK AND TrangThai = N'Hoạt động') THROW 50004, N'Tài khoản không tồn tại hoặc bị khóa!', 1;
        IF @ChiTietNhap IS NULL OR @ChiTietNhap = '' OR NOT ISJSON(@ChiTietNhap) > 0 THROW 50005, N'Dữ liệu chi tiết nhập hàng không hợp lệ!', 1;

        --=====================================================================
        -- PARSE JSON VÀ KIỂM TRA CHI TIẾT MỚI
        --=====================================================================
        INSERT INTO @TempChiTiet (MaVatLieu, SoLuong, GiaNhap)
        SELECT JSON_VALUE(value, '$.MaVatLieu'), JSON_VALUE(value, '$.SoLuong'), JSON_VALUE(value, '$.GiaNhap')
        FROM OPENJSON(@ChiTietNhap);

        IF NOT EXISTS (SELECT 1 FROM @TempChiTiet) THROW 50006, N'Danh sách chi tiết nhập hàng trống!', 1;
        IF EXISTS (SELECT 1 FROM @TempChiTiet t LEFT JOIN QLVatLieu v ON t.MaVatLieu = v.MaVatLieu WHERE v.MaVatLieu IS NULL OR v.TrangThai != N'Hoạt động')
            THROW 50007, N'Có vật liệu không tồn tại hoặc không hoạt động!', 1;
        IF EXISTS (SELECT 1 FROM @TempChiTiet WHERE SoLuong <= 0 OR GiaNhap < 0)
            THROW 50008, N'Số lượng hoặc giá nhập không hợp lệ!', 1;

        --=====================================================================
        -- LẤY CHI TIẾT GỐC VÀ BẮT ĐẦU TRANSACTION
        --=====================================================================
        INSERT INTO @OriginalChiTiet (MaVatLieu, SoLuong, GiaNhap)
        SELECT MaVatLieu, SoLuong, GiaNhap FROM ChiTietDonNhap WHERE MaDonNhap = @MaDonNhap;

        BEGIN TRANSACTION;

        --=====================================================================
        -- XÓA CHI TIẾT CŨ VÀ THÊM CHI TIẾT MỚI
        --=====================================================================
        DELETE FROM ChiTietDonNhap WHERE MaDonNhap = @MaDonNhap;

        INSERT INTO ChiTietDonNhap (MaDonNhap, MaVatLieu, SoLuong, GiaNhap)
        SELECT @MaDonNhap, MaVatLieu, SoLuong, GiaNhap FROM @TempChiTiet;

        --=====================================================================
        -- CẬP NHẬT HEADER
        --=====================================================================
        UPDATE QLDonNhap
        SET NgayNhap = @NgayNhap, MaNCC = @MaNCC, MaTK = @MaTK, GhiChu = @GhiChu,
            NgayCapNhat = GETDATE(), NguoiCapNhat = @NguoiCapNhat
            -- Cập nhật tổng tiền nếu cần
            -- TongTien = (SELECT SUM(ThanhTien) FROM ChiTietDonNhap WHERE MaDonNhap = @MaDonNhap)
        WHERE MaDonNhap = @MaDonNhap;

        --=====================================================================
        -- TÍNH TOÁN VÀ CẬP NHẬT TỒN KHO
        --=====================================================================
        INSERT INTO @InventoryChanges (MaVatLieu, ChangeAmount)
        SELECT ISNULL(t.MaVatLieu, o.MaVatLieu), ISNULL(t.SoLuong, 0) - ISNULL(o.SoLuong, 0)
        FROM @TempChiTiet t
        FULL OUTER JOIN @OriginalChiTiet o ON t.MaVatLieu = o.MaVatLieu;

        UPDATE vl
        SET vl.SoLuong = ISNULL(vl.SoLuong, 0) + ic.ChangeAmount,
            vl.NgayCapNhat = GETDATE(),
            vl.NguoiCapNhat = @NguoiCapNhat,
            -- Cập nhật giá nhập mới nhất từ chi tiết vừa sửa
            vl.DonGiaNhap = ISNULL((SELECT TOP 1 GiaNhap FROM @TempChiTiet t WHERE t.MaVatLieu = vl.MaVatLieu), vl.DonGiaNhap)
        FROM QLVatLieu vl
        INNER JOIN @InventoryChanges ic ON vl.MaVatLieu = ic.MaVatLieu
        WHERE ic.ChangeAmount != 0;

        -- (Tùy chọn) Kiểm tra tồn kho âm
        IF EXISTS (SELECT 1 FROM QLVatLieu WHERE MaVatLieu IN (SELECT MaVatLieu FROM @InventoryChanges) AND SoLuong < 0)
        BEGIN
            SET @ErrorMsg = N'Cập nhật làm số lượng tồn kho của một hoặc nhiều vật liệu bị âm.';
            THROW 50009, @ErrorMsg, 1;
        END;

        --=====================================================================
        -- GHI LOG VÀ COMMIT
        --=====================================================================
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @NguoiCapNhat, N'QLDonNhap', @MaDonNhap, N'Cập nhật', N'Cập nhật thông tin đơn nhập');

        COMMIT TRANSACTION;

        SELECT N'Cập nhật đơn nhập thành công!' AS Message, @MaDonNhap AS MaDonNhap; -- Trả về ID nếu cần

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
ALTER TRIGGER trg_CapNhatSoLuongXuat
ON ChiTietDonXuat
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(MAX);

    -- Kiểm tra tồn kho tại đúng KHO NGUỒN
    IF EXISTS (
        SELECT 1
        FROM QLVatLieu v
        INNER JOIN inserted i ON v.MaVatLieu = i.MaVatLieu AND v.MaKho = i.MaKhoNguon -- << THÊM ĐIỀU KIỆN KHO
        WHERE ISNULL(v.SoLuong, 0) < i.SoLuong -- Dùng ISNULL cho chắc
    )
    BEGIN
        SELECT TOP 1 @ErrorMsg = N'Số lượng VL "' + ISNULL(v.Ten, CAST(i.MaVatLieu AS VARCHAR))
             + N'" tại Kho ID ' + CAST(i.MaKhoNguon AS VARCHAR)
             + N' (' + CAST(ISNULL(v.SoLuong,0) AS VARCHAR) + N') không đủ cho yêu cầu (' + CAST(i.SoLuong AS VARCHAR) + N')!'
        FROM QLVatLieu v
        INNER JOIN inserted i ON v.MaVatLieu = i.MaVatLieu AND v.MaKho = i.MaKhoNguon
        WHERE ISNULL(v.SoLuong, 0) < i.SoLuong;

        RAISERROR (@ErrorMsg, 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
    ELSE
    BEGIN
        -- Cập nhật kho tại đúng KHO NGUỒN
        UPDATE QLVatLieu
        SET SoLuong = ISNULL(v.SoLuong, 0) - i.SoLuong, -- Dùng ISNULL
            NgayCapNhat = GETDATE()
            -- NguoiCapNhat = (SELECT TOP 1 NguoiCapNhat FROM QLDonXuat WHERE MaDonXuat = i.MaDonXuat) -- Lấy người cập nhật nếu cần
        FROM QLVatLieu v
        INNER JOIN inserted i ON v.MaVatLieu = i.MaVatLieu AND v.MaKho = i.MaKhoNguon;
        -- Ghi log hoạt động
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
        SELECT 
            GETDATE(),
            (SELECT TOP 1 MaTK FROM QLDonXuat WHERE MaDonXuat = i.MaDonXuat),
            N'QLVatLieu',
            i.MaVatLieu,
            N'Giảm tồn kho',
            CAST((v.SoLuong + i.SoLuong) AS NVARCHAR(MAX)),
            CAST(v.SoLuong AS NVARCHAR(MAX)),
            N'Giảm tồn kho do xuất kho qua đơn xuất ' + CAST(i.MaDonXuat AS NVARCHAR(10))
        FROM QLVatLieu v
        INNER JOIN inserted i ON v.MaVatLieu = i.MaVatLieu;
    END;
END;
Go


GO

ALTER PROCEDURE sp_ThemDonXuat
    @NgayXuat DATE,
    @MaTK INT,
    @MaHoaDon INT = NULL,
    @MaKhachHang INT = NULL,
    @GhiChu NVARCHAR(255) = NULL,
    @ChiTietXuat NVARCHAR(MAX),
    @NguoiCapNhat INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM QLTK WHERE MaTK = @MaTK AND TrangThai = N'Hoạt động')
            THROW 50001, N'Tài khoản không tồn tại hoặc bị khóa!', 1;

        IF @MaHoaDon IS NOT NULL AND NOT EXISTS (SELECT 1 FROM QLHoaDon WHERE MaHoaDon = @MaHoaDon)
            THROW 50002, N'Hóa đơn không tồn tại!', 1;

        IF @MaKhachHang IS NOT NULL AND NOT EXISTS (SELECT 1 FROM QLKH WHERE MaKhachHang = @MaKhachHang AND TrangThai = N'Hoạt động')
            THROW 50003, N'Khách hàng không tồn tại hoặc không hoạt động!', 1;

        INSERT INTO QLDonXuat (NgayXuat, MaTK, MaHoaDon, MaKhachHang, TrangThai, GhiChu, NgayCapNhat, NguoiCapNhat)
        VALUES (@NgayXuat, @MaTK, @MaHoaDon, @MaKhachHang, N'Hoàn thành', @GhiChu, GETDATE(), @NguoiCapNhat);

        DECLARE @MaDonXuat INT = SCOPE_IDENTITY();

        DECLARE @TempChiTiet TABLE (
            MaVatLieu INT,
            SoLuong INT,
            DonGia DECIMAL(18,2)
        );

        INSERT INTO @TempChiTiet (MaVatLieu, SoLuong, DonGia)
        SELECT 
            JSON_VALUE(value, '$.MaVatLieu') AS MaVatLieu,
            JSON_VALUE(value, '$.SoLuong') AS SoLuong,
            JSON_VALUE(value, '$.DonGia') AS DonGia
        FROM OPENJSON(@ChiTietXuat);

        IF NOT EXISTS (SELECT 1 FROM @TempChiTiet)
            THROW 50004, N'Danh sách chi tiết xuất kho trống!', 1;

        IF EXISTS (
            SELECT 1 
            FROM @TempChiTiet t
            LEFT JOIN QLVatLieu v ON t.MaVatLieu = v.MaVatLieu
            WHERE v.MaVatLieu IS NULL OR v.TrangThai != N'Hoạt động'
        )
            THROW 50005, N'Có vật liệu không tồn tại hoặc không hoạt động!', 1;

        IF EXISTS (SELECT 1 FROM @TempChiTiet WHERE SoLuong <= 0 OR DonGia < 0)
            THROW 50006, N'Số lượng hoặc đơn giá không hợp lệ!', 1;

        INSERT INTO ChiTietDonXuat (MaDonXuat, MaVatLieu, SoLuong, DonGia)
        SELECT @MaDonXuat, MaVatLieu, SoLuong, DonGia
        FROM @TempChiTiet;

        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @NguoiCapNhat, N'QLDonXuat', @MaDonXuat, N'Thêm', N'Thêm đơn xuất kho');

        COMMIT TRANSACTION;

        SELECT N'Thêm đơn xuất kho thành công!' AS Message, @MaDonXuat AS MaDonXuat;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH;
END;
GO
ALTER PROCEDURE sp_ThemChiTietHoaDon
    @MaHoaDon INT,
    @MaVatLieu INT,
    @SoLuong INT,
    @NguoiCapNhat INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Kiểm tra hóa đơn tồn tại
        IF NOT EXISTS (SELECT 1 FROM QLHoaDon WHERE MaHoaDon = @MaHoaDon)
            THROW 50001, N'Hóa đơn không tồn tại!', 1;

        -- Kiểm tra vật liệu tồn tại và đang hoạt động
        IF NOT EXISTS (SELECT 1 FROM QLVatLieu WHERE MaVatLieu = @MaVatLieu AND TrangThai = N'Hoạt động')
            THROW 50002, N'Vật liệu không tồn tại hoặc không hoạt động!', 1;

        -- Kiểm tra số lượng hợp lệ
        IF @SoLuong <= 0
            THROW 50003, N'Số lượng phải lớn hơn 0!', 1;

        DECLARE @DonGia DECIMAL(18,2);
        DECLARE @ThanhTien DECIMAL(18,2);

        -- Lấy giá bán của vật liệu
        SELECT @DonGia = DonGiaBan FROM QLVatLieu WHERE MaVatLieu = @MaVatLieu;

        -- Kiểm tra giá bán hợp lệ
        IF @DonGia IS NULL
            THROW 50004, N'Giá bán của vật liệu không hợp lệ!', 1;

        -- Tính tổng tiền
        SET @ThanhTien = dbo.fn_LamTronTien(@SoLuong * @DonGia, 0);

        -- Thêm chi tiết hóa đơn
        INSERT INTO ChiTietHoaDon (MaHoaDon, MaVatLieu, SoLuong, DonGia)
        VALUES (@MaHoaDon, @MaVatLieu, @SoLuong, @DonGia);

        -- Cập nhật tổng tiền hóa đơn
        UPDATE QLHoaDon
        SET TongTien = dbo.fn_LamTronTien(ISNULL(TongTien, 0) + @ThanhTien, 0)
        WHERE MaHoaDon = @MaHoaDon;

        -- Ghi log hoạt động tài chính
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @NguoiCapNhat, N'ChiTietHoaDon', @MaHoaDon, N'Thêm chi tiết', N'Thêm chi tiết hóa đơn tài chính');

        COMMIT TRANSACTION;

        SELECT N'Thêm chi tiết hóa đơn thành công!' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH;
END;
GO

CREATE PROCEDURE sp_HuyDonXuat
    @MaDonXuat INT,
    @NguoiCapNhat INT,
    @LyDoHuy NVARCHAR(255)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Kiểm tra đơn xuất tồn tại
        IF NOT EXISTS (SELECT 1 FROM QLDonXuat WHERE MaDonXuat = @MaDonXuat)
            THROW 50001, N'Đơn xuất không tồn tại!', 1;

        -- Kiểm tra trạng thái
        IF EXISTS (SELECT 1 FROM QLDonXuat WHERE MaDonXuat = @MaDonXuat AND TrangThai = N'Đã hủy')
            THROW 50002, N'Đơn xuất đã bị hủy trước đó!', 1;

        IF EXISTS (SELECT 1 FROM QLDonXuat WHERE MaDonXuat = @MaDonXuat AND TrangThai = N'Hoàn thành')
            THROW 50003, N'Không thể hủy đơn xuất đã hoàn thành!', 1;

        -- Hoàn lại số lượng vật liệu trong kho
        UPDATE QLVatLieu
        SET SoLuong = v.SoLuong + ctdx.SoLuong,
            NgayCapNhat = GETDATE(),
            NguoiCapNhat = @NguoiCapNhat
        FROM QLVatLieu v
        INNER JOIN ChiTietDonXuat ctdx ON v.MaVatLieu = ctdx.MaVatLieu
        WHERE ctdx.MaDonXuat = @MaDonXuat;

        -- Cập nhật trạng thái đơn xuất
        UPDATE QLDonXuat
        SET TrangThai = N'Đã hủy',
            NgayCapNhat = GETDATE(),
            NguoiCapNhat = @NguoiCapNhat
        WHERE MaDonXuat = @MaDonXuat;

        -- Ghi log hoạt động
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
        VALUES (GETDATE(), @NguoiCapNhat, N'QLDonXuat', @MaDonXuat, N'Hủy', N'Đang xử lý', N'Đã hủy', @LyDoHuy);

        COMMIT TRANSACTION;

        SELECT N'Hủy đơn xuất kho thành công!' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO
DISABLE TRIGGER trg_KiemTraSoLuongBan ON ChiTietHoaDon;
GO
CREATE TRIGGER trg_CanhBaoSoLuongHoaDon
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
        -- Ghi log cảnh báo, không rollback
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        SELECT 
            GETDATE(),
            (SELECT TOP 1 MaTKLap FROM QLHoaDon WHERE MaHoaDon = i.MaHoaDon),
            N'ChiTietHoaDon',
            i.MaHoaDon,
            N'Cảnh báo',
            N'Số lượng hóa đơn vượt quá tồn kho thực tế cho vật liệu ' + CAST(i.MaVatLieu AS NVARCHAR(10))
        FROM inserted i;
    END;
END;
GO
ALTER TABLE QLDonXuat
ALTER COLUMN TrangThai NVARCHAR(50) CHECK (TrangThai IN (N'Chờ xử lý', N'Hoàn thành', N'Đã hủy', N'Chờ hóa đơn'));
go
Alter PROCEDURE sp_TimKiemVatLieuTheoKho
    @MaKho INT,
    @Keyword NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    -- Chuẩn hóa từ khóa để tìm kiếm với ký tự đại diện
    DECLARE @Search NVARCHAR(100) = '%' + @Keyword + '%';

    SELECT 
        vl.MaVatLieu, 
        vl.Ten, 
        vl.DonGiaBan, 
        vl.DonGiaNhap, 
        vl.DonViTinh, 
        vl.SoLuong,
        k.TenKho,
        CASE 
            WHEN vl.Ten LIKE @Keyword + '%' THEN 3  -- Khớp ở đầu tên vật liệu
            WHEN vl.Ten LIKE '%' + @Keyword + '%' THEN 2 -- Khớp trong tên vật liệu
            ELSE 1 -- Khớp với loại vật liệu hoặc mã vật liệu
        END AS MatchScore
    FROM QLVatLieu vl  
    JOIN Kho k ON vl.MaKho = k.MaKho
    WHERE 
        vl.MaKho = @MaKho
        AND vl.TrangThai = N'Hoạt động'
        AND (
            vl.Ten COLLATE Latin1_General_CI_AI LIKE @Search
            OR CAST(vl.MaVatLieu AS NVARCHAR) LIKE @Search
        )
    ORDER BY MatchScore DESC, vl.Ten;
END;
GO
SELECT vl.MaVatLieu, vl.Ten, lv.TenLoai, vl.DonGiaNhap, vl.DonGiaBan, vl.DonViTinh, vl.SoLuong
FROM QLVatLieu vl
JOIN QLLoaiVatLieu lv ON vl.Loai = lv.MaLoaiVatLieu
Select * from QLVatLieu
Alter PROCEDURE sp_LayDonXuatTheoID
    @MaDonXuat INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra Đơn xuất có tồn tại
    IF NOT EXISTS (SELECT 1 FROM QLDonXuat WHERE MaDonXuat = @MaDonXuat)
    BEGIN
        SELECT N'Đơn xuất không tồn tại!' AS Message;
        RETURN;
    END

    -- Lấy thông tin header của Đơn xuất
  SELECT 
        dx.MaDonXuat,
        dx.NgayXuat,
        dx.TrangThai,
        dx.GhiChu,
        dx.LoaiXuat,
        tk.TenDangNhap AS NguoiTao,
        kh.Ten AS TenKhachHang,
        hd.MaHoaDon,
        hd.NgayLap AS NgayLapHoaDon,
        hd.TongTien AS TongTienHoaDon,
        hd.TrangThai AS TrangThaiHoaDon
    FROM QLDonXuat dx
    LEFT JOIN QLTK tk ON dx.MaTK = tk.MaTK
    LEFT JOIN QLKH kh ON dx.MaKhachHang = kh.MaKhachHang
    LEFT JOIN QLHoaDon hd ON dx.MaHoaDon = hd.MaHoaDon
    WHERE dx.MaDonXuat = @MaDonXuat;
    -- Lấy chi tiết Đơn xuất
    SELECT 
        ctdx.MaCTDX,
        ctdx.MaVatLieu,
        vl.Ten AS TenVatLieu,
        ctdx.SoLuong,
        ctdx.DonGia,
        vl.DonViTinh,
		ctdx.MaKhoNguon
    FROM ChiTietDonXuat ctdx
    INNER JOIN QLVatLieu vl ON ctdx.MaVatLieu = vl.MaVatLieu
    WHERE ctdx.MaDonXuat = @MaDonXuat;
END;
GO
EXEC sp_LayDonXuatTheoID @MaDonXuat = 1;
Proposed Solution (Skeleton):

sql

Sao chép
ALTER PROCEDURE sp_DuyetHoaDon
    @MaHoaDon INT,
    @NguoiDuyet INT, 
    @GhiChu NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(4000);
    DECLARE @CurrentTrangThai NVARCHAR(50);
    DECLARE @ChucVu NVARCHAR(50);

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Check if invoice exists
        SELECT @CurrentTrangThai = TrangThai
        FROM QLHoaDon
        WHERE MaHoaDon = @MaHoaDon;

        IF @CurrentTrangThai IS NULL
            THROW 50001, N'Hóa đơn không tồn tại!', 1;

        -- Check invoice status
        IF @CurrentTrangThai != N'Chờ thanh toán'
            THROW 50002, N'Chỉ có thể duyệt hóa đơn ở trạng thái "Chờ thanh toán"!', 1;

        -- Check user permission
        SELECT @ChucVu = ChucVu
        FROM QLTK
        WHERE MaTK = @NguoiDuyet AND TrangThai = N'Hoạt động';

        IF @ChucVu NOT IN (N'Admin', N'Quản lý')
            THROW 50003, N'Bạn không có quyền duyệt hóa đơn!', 1;

        -- Update invoice status
        UPDATE QLHoaDon
        SET TrangThai = N'Đã thanh toán',
            NgayCapNhat = GETDATE(),
            NguoiCapNhat = @NguoiDuyet,
            GhiChu = ISNULL(GhiChu + N'; ', N'') + ISNULL(@GhiChu, N'Đã duyệt')
        WHERE MaHoaDon = @MaHoaDon;

        -- Log action
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
        VALUES (GETDATE(), @NguoiDuyet, N'QLHoaDon', @MaHoaDon, N'Duyệt', @CurrentTrangThai, N'Đã thanh toán', @GhiChu);

        COMMIT TRANSACTION;
        SELECT N'Duyệt hóa đơn thành công!' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO
ALTER PROCEDURE sp_DuyetDonXuat
    @MaDonXuat INT,
    @NguoiDuyet INT,
    @GhiChu NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(4000);
    DECLARE @CurrentTrangThai NVARCHAR(50);
    DECLARE @ChucVu NVARCHAR(50);
    DECLARE @MaHoaDon INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Check if export order exists
        SELECT @CurrentTrangThai = TrangThai, @MaHoaDon = MaHoaDon
        FROM QLDonXuat
        WHERE MaDonXuat = @MaDonXuat;

        IF @CurrentTrangThai IS NULL
            THROW 50001, N'Đơn xuất không tồn tại!', 1;

        -- Check status
        IF @CurrentTrangThai NOT IN (N'Chờ duyệt')
            THROW 50002, N'Không thể duyệt đơn xuất ở trạng thái này', 1;

        -- Check user permission
        SELECT @ChucVu = ChucVu
        FROM QLTK
        WHERE MaTK = @NguoiDuyet AND TrangThai = N'Hoạt động';

        IF @ChucVu NOT IN (N'Admin', N'Quản lý')
            THROW 50003, N'Bạn không có quyền duyệt đơn xuất!', 1;

        IF @MaHoaDon IS NOT NULL
            AND NOT EXISTS (SELECT 1 FROM QLHoaDon WHERE MaHoaDon = @MaHoaDon AND TrangThai = N'Đã thanh toán')
            THROW 50004, N'Hóa đơn liên kết chưa được thanh toán!', 1;

        UPDATE QLDonXuat
        SET TrangThai = N'Hoàn thành',
            NgayCapNhat = GETDATE(),
            NguoiCapNhat = @NguoiDuyet,
            GhiChu = ISNULL(GhiChu + N'; ', N'') + ISNULL(@GhiChu, N'Đã duyệt')
        WHERE MaDonXuat = @MaDonXuat;

        -- Log action
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
        VALUES (GETDATE(), @NguoiDuyet, N'QLDonXuat', @MaDonXuat, N'Duyệt', @CurrentTrangThai, N'Hoàn thành', @GhiChu);

        COMMIT TRANSACTION;
        SELECT N'Duyệt đơn xuất thành công!' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO
Create PROCEDURE [dbo].[sp_DuyetDonXuat]
    @MaDonXuat INT,
    @NguoiDuyet INT,
    @GhiChu NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(4000);
    DECLARE @CurrentTrangThai NVARCHAR(50);
    DECLARE @MaHoaDon INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Check if export order exists
        SELECT @CurrentTrangThai = TrangThai, @MaHoaDon = MaHoaDon
        FROM QLDonXuat
        WHERE MaDonXuat = @MaDonXuat;

        IF @CurrentTrangThai IS NULL
            THROW 50001, N'Đơn xuất không tồn tại!', 1;

        -- Check status
        IF @CurrentTrangThai != N'Chờ duyệt'
            THROW 50002, N'Chỉ có thể duyệt đơn xuất ở trạng thái "Chờ duyệt"!', 1;

        -- Check user permission
        IF NOT EXISTS (SELECT 1 FROM QLTK WHERE MaTK = @NguoiDuyet AND TrangThai = N'Hoạt động' AND ChucVu IN (N'Admin', N'Quản lý'))
            THROW 50003, N'Bạn không có quyền duyệt đơn xuất!', 1;

        -- Check linked invoice (if applicable)
        IF @MaHoaDon IS NOT NULL
            AND NOT EXISTS (SELECT 1 FROM QLHoaDon WHERE MaHoaDon = @MaHoaDon AND TrangThai = N'Đã thanh toán')
            THROW 50004, N'Hóa đơn liên kết chưa được thanh toán!', 1;

        -- Check inventory availability
        IF EXISTS (
            SELECT 1
            FROM ChiTietDonXuat ctdx
            INNER JOIN QLVatLieu v ON ctdx.MaVatLieu = v.MaVatLieu AND ctdx.MaKhoNguon = v.MaKho
            WHERE ctdx.MaDonXuat = @MaDonXuat AND v.SoLuong < ctdx.SoLuong
        )
            THROW 50005, N'Không đủ số lượng vật liệu trong kho để duyệt đơn xuất!', 1;

        -- Update inventory
        UPDATE QLVatLieu
        SET SoLuong = v.SoLuong - ctdx.SoLuong,
            NgayCapNhat = GETDATE(),
            NguoiCapNhat = @NguoiDuyet
        FROM QLVatLieu v
        INNER JOIN ChiTietDonXuat ctdx ON v.MaVatLieu = ctdx.MaVatLieu AND v.MaKho = ctdx.MaKhoNguon
        WHERE ctdx.MaDonXuat = @MaDonXuat;

        -- Update export order status
        UPDATE QLDonXuat
        SET TrangThai = N'Hoàn thành',
            NgayCapNhat = GETDATE(),
            NguoiCapNhat = @NguoiDuyet,
            GhiChu = ISNULL(GhiChu + N'; ', N'') + ISNULL(@GhiChu, N'Đã duyệt')
        WHERE MaDonXuat = @MaDonXuat;

        -- Log action
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
        VALUES (GETDATE(), @NguoiDuyet, N'QLDonXuat', @MaDonXuat, N'Duyệt', @CurrentTrangThai, N'Hoàn thành', ISNULL(@GhiChu, N'Đã duyệt'));

        COMMIT TRANSACTION;
        SELECT N'Duyệt đơn xuất thành công!' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SET @ErrorMsg = ERROR_MESSAGE();
        THROW 50000, @ErrorMsg, 1;
    END CATCH;
END;
GO
Create PROCEDURE [dbo].[sp_DuyetDonNhap]
    @MaDonNhap INT,
    @NguoiDuyet INT,
    @GhiChu NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(4000);
    DECLARE @CurrentTrangThai NVARCHAR(50);
    DECLARE @MaNCC INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Check if import order exists
        SELECT @CurrentTrangThai = TrangThai, @MaNCC = MaNCC
        FROM QLDonNhap
        WHERE MaDonNhap = @MaDonNhap;

        IF @CurrentTrangThai IS NULL
            THROW 50001, N'Đơn nhập không tồn tại!', 1;

        -- Check status
        IF @CurrentTrangThai != N'Chờ duyệt'
            THROW 50002, N'Chỉ có thể duyệt đơn nhập ở trạng thái "Chờ duyệt"!', 1;

        -- Check user permission
        IF NOT EXISTS (SELECT 1 FROM QLTK WHERE MaTK = @NguoiDuyet AND TrangThai = N'Hoạt động' AND ChucVu IN (N'Admin', N'Quản lý'))
            THROW 50003, N'Bạn không có quyền duyệt đơn nhập!', 1;

        -- Check supplier status
        IF NOT EXISTS (SELECT 1 FROM NCC WHERE MaNCC = @MaNCC AND TrangThai != N'Bị khoá')
            THROW 50004, N'Nhà cung cấp không hoạt động!', 1;

        -- Check material status
        IF EXISTS (
            SELECT 1
            FROM ChiTietDonNhap ctdn
            LEFT JOIN QLVatLieu v ON ctdn.MaVatLieu = v.MaVatLieu
            WHERE ctdn.MaDonNhap = @MaDonNhap AND (v.MaVatLieu IS NULL OR v.TrangThai != N'Hoạt động')
        )
            THROW 50005, N'Có vật liệu không tồn tại hoặc không hoạt động!', 1;

        -- Update inventory
        UPDATE QLVatLieu
        SET SoLuong = ISNULL(v.SoLuong, 0) + ctdn.SoLuong,
            NgayCapNhat = GETDATE(),
            NguoiCapNhat = @NguoiDuyet
        FROM QLVatLieu v
        INNER JOIN ChiTietDonNhap ctdn ON v.MaVatLieu = ctdn.MaVatLieu
        WHERE ctdn.MaDonNhap = @MaDonNhap;

        -- Update import order status
        UPDATE QLDonNhap
        SET TrangThai = N'Hoàn thành',
            NgayCapNhat = GETDATE(),
            NguoiCapNhat = @NguoiDuyet,
            GhiChu = ISNULL(GhiChu + N'; ', N'') + ISNULL(@GhiChu, N'Đã duyệt')
        WHERE MaDonNhap = @MaDonNhap;

        -- Log action
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
        VALUES (GETDATE(), @NguoiDuyet, N'QLDonNhap', @MaDonNhap, N'Duyệt', @CurrentTrangThai, N'Hoàn thành', ISNULL(@GhiChu, N'Đã duyệt'));

        COMMIT TRANSACTION;
        SELECT N'Duyệt đơn nhập thành công!' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SET @ErrorMsg = ERROR_MESSAGE();
        THROW 50000, @ErrorMsg, 1;
    END CATCH;
END;
GO
Create PROCEDURE [dbo].[sp_XoaHoaDon]
    @MaHoaDon INT,
    @NguoiXoa INT,
    @LyDoXoa NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(4000);
    DECLARE @CurrentTrangThai NVARCHAR(50);

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Check if invoice exists
        SELECT @CurrentTrangThai = TrangThai
        FROM QLHoaDon
        WHERE MaHoaDon = @MaHoaDon;

        IF @CurrentTrangThai IS NULL
            THROW 50001, N'Hóa đơn không tồn tại!', 1;

        -- Check status
        IF @CurrentTrangThai != N'Chờ duyệt'
            THROW 50002, N'Chỉ có thể xóa hóa đơn ở trạng thái "Chờ duyệt"!', 1;

        -- Check user permission
        IF NOT EXISTS (SELECT 1 FROM QLTK WHERE MaTK = @NguoiXoa AND TrangThai = N'Hoạt động' AND ChucVu IN (N'Admin', N'Quản lý'))
            THROW 50003, N'Bạn không có quyền xóa hóa đơn!', 1;

   
        UPDATE QLHoaDon
        SET TrangThai = N'Đã hủy',
            NgayCapNhat = GETDATE(),
            NguoiCapNhat = @NguoiXoa
        WHERE MaHoaDon = @MaHoaDon;

        -- Log action
        INSERT INTO AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
        VALUES (GETDATE(), @NguoiXoa, N'QLHoaDon', @MaHoaDon, N'Xóa', @CurrentTrangThai, N'Đã hủy', ISNULL(@LyDoXoa, N'Xóa hóa đơn'));

        COMMIT TRANSACTION;
        SELECT N'Xóa hóa đơn thành công!' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SET @ErrorMsg = ERROR_MESSAGE();
        THROW 50000, @ErrorMsg, 1;
    END CATCH;
END;
GO
INSERT INTO dbo.TonKhoVatLieu (MaVatLieu, MaKho, SoLuongTon)
SELECT
    vl.MaVatLieu,
    vl.MaKho, -- Lấy kho mặc định từ QLVatLieu
    ISNULL(vl.SoLuong, 0) -- Lấy số lượng từ QLVatLieu (nếu null thì coi là 0)
FROM
    dbo.QLVatLieu vl
WHERE
    -- Chỉ insert nếu cặp (MaVatLieu, MaKho) chưa tồn tại trong TonKhoVatLieu
    NOT EXISTS (
        SELECT 1
        FROM dbo.TonKhoVatLieu tkv
        WHERE tkv.MaVatLieu = vl.MaVatLieu AND tkv.MaKho = vl.MaKho
    )
    AND vl.MaKho IS NOT NULL -- Đảm bảo vật liệu có gán kho mặc định
    AND vl.SoLuong IS NOT NULL AND vl.SoLuong > 0; -- Chỉ insert nếu có số lượng > 0
	ALTER PROCEDURE [dbo].[sp_DuyetDonXuat]
    @MaDonXuat INT,
    @NguoiDuyet INT,
    @GhiChu NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMsg NVARCHAR(4000);
    DECLARE @CurrentTrangThai NVARCHAR(50);
    DECLARE @LoaiXuat NVARCHAR(50); -- Thêm biến để lưu loại xuất
    DECLARE @MaHoaDon INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- === 1. KIỂM TRA ĐƠN XUẤT VÀ QUYỀN ===

        -- Lấy thông tin trạng thái, loại xuất, mã hóa đơn (nếu có)
        SELECT @CurrentTrangThai = TrangThai, @LoaiXuat = LoaiXuat, @MaHoaDon = MaHoaDon
        FROM dbo.QLDonXuat -- Sử dụng dbo. schema
        WHERE MaDonXuat = @MaDonXuat;

        IF @CurrentTrangThai IS NULL
            THROW 50001, N'Đơn xuất không tồn tại!', 1;

        -- Chỉ duyệt đơn ở trạng thái 'Chờ duyệt' (hoặc trạng thái chờ khác nếu có)
        IF @CurrentTrangThai != N'Chờ duyệt' -- Đảm bảo trạng thái chờ là 'Chờ duyệt'
            THROW 50002, N'Chỉ có thể duyệt đơn xuất ở trạng thái "Chờ duyệt"!', 1;

        -- Kiểm tra quyền người duyệt
        IF NOT EXISTS (SELECT 1 FROM dbo.QLTK WHERE MaTK = @NguoiDuyet AND TrangThai = N'Hoạt động' AND ChucVu IN (N'Admin', N'Quản lý'))
            THROW 50003, N'Bạn không có quyền duyệt đơn xuất!', 1;

        -- Kiểm tra hóa đơn liên kết (nếu là 'Xuất hàng' và có mã hóa đơn)
        IF @LoaiXuat = N'Xuất hàng' AND @MaHoaDon IS NOT NULL
            AND NOT EXISTS (SELECT 1 FROM dbo.QLHoaDon WHERE MaHoaDon = @MaHoaDon AND TrangThai = N'Đã thanh toán') -- Hoặc trạng thái khác tùy quy trình
            THROW 50004, N'Hóa đơn liên kết chưa được thanh toán (hoặc không hợp lệ)!', 1;

        -- === 2. KIỂM TRA TỒN KHO TRONG TonKhoVatLieu ===

        -- Kiểm tra xem có chi tiết nào không đủ số lượng tồn tại KHO NGUỒN không
        IF EXISTS (
            SELECT 1
            FROM dbo.ChiTietDonXuat ctdx
            LEFT JOIN dbo.TonKhoVatLieu tkv ON ctdx.MaVatLieu = tkv.MaVatLieu AND ctdx.MaKhoNguon = tkv.MaKho
            WHERE ctdx.MaDonXuat = @MaDonXuat
              AND ISNULL(tkv.SoLuongTon, 0) < ctdx.SoLuong -- Nếu tồn kho < số lượng cần xuất
        )
        BEGIN
            -- Lấy thông tin chi tiết để báo lỗi rõ hơn (tùy chọn)
            DECLARE @ErrorDetail NVARCHAR(500);
            SELECT TOP 1 @ErrorDetail = N'Vật liệu ID ' + CAST(ctdx.MaVatLieu AS NVARCHAR(10))
                                     + N' tại Kho ID ' + CAST(ctdx.MaKhoNguon AS NVARCHAR(10))
                                     + N' không đủ số lượng (Cần: ' + CAST(ctdx.SoLuong AS NVARCHAR(20))
                                     + N', Tồn: ' + CAST(ISNULL(tkv.SoLuongTon, 0) AS NVARCHAR(20)) + N')'
            FROM dbo.ChiTietDonXuat ctdx
            LEFT JOIN dbo.TonKhoVatLieu tkv ON ctdx.MaVatLieu = tkv.MaVatLieu AND ctdx.MaKhoNguon = tkv.MaKho
            WHERE ctdx.MaDonXuat = @MaDonXuat AND ISNULL(tkv.SoLuongTon, 0) < ctdx.SoLuong;

            SET @ErrorMsg = N'Không đủ số lượng vật liệu trong kho để duyệt đơn xuất! Chi tiết: ' + ISNULL(@ErrorDetail, N'Không xác định');
            THROW 50005, @ErrorMsg, 1;
        END;

        -- === 3. CẬP NHẬT TỒN KHO TRONG TonKhoVatLieu ===

        -- 3.1. Trừ tồn kho tại Kho Nguồn (Áp dụng cho cả Xuất hàng và Chuyển kho)
        UPDATE tkv
        SET tkv.SoLuongTon = tkv.SoLuongTon - ctdx.SoLuong
        -- SELECT tkv.MaVatLieu, tkv.MaKho, tkv.SoLuongTon AS TonCu, ctdx.SoLuong AS SLXuat, tkv.SoLuongTon - ctdx.SoLuong AS TonMoi -- Dùng để debug
        FROM dbo.TonKhoVatLieu tkv
        INNER JOIN dbo.ChiTietDonXuat ctdx ON tkv.MaVatLieu = ctdx.MaVatLieu AND tkv.MaKho = ctdx.MaKhoNguon
        WHERE ctdx.MaDonXuat = @MaDonXuat;

        -- 3.2. Cộng tồn kho tại Kho Đích (Chỉ áp dụng cho Chuyển kho)
        IF @LoaiXuat = N'Chuyển kho'
        BEGIN
            MERGE dbo.TonKhoVatLieu AS Target
            USING (SELECT MaVatLieu, MaKhoDich, SoLuong FROM dbo.ChiTietDonXuat WHERE MaDonXuat = @MaDonXuat AND MaKhoDich IS NOT NULL) AS Source -- Lấy chi tiết của đơn này
            ON (Target.MaVatLieu = Source.MaVatLieu AND Target.MaKho = Source.MaKhoDich) -- Join theo vật liệu và kho đích
            WHEN MATCHED THEN
                -- Nếu vật liệu đã có ở kho đích -> Cộng thêm số lượng
                UPDATE SET Target.SoLuongTon = Target.SoLuongTon + Source.SoLuong
            WHEN NOT MATCHED BY TARGET THEN
                -- Nếu vật liệu chưa có ở kho đích -> Thêm mới bản ghi tồn kho
                INSERT (MaVatLieu, MaKho, SoLuongTon)
                VALUES (Source.MaVatLieu, Source.MaKhoDich, Source.SoLuong);
        END;

        -- === 4. CẬP NHẬT TRẠNG THÁI ĐƠN XUẤT ===
        UPDATE dbo.QLDonXuat
        SET TrangThai = N'Hoàn thành', -- Trạng thái mới sau khi duyệt
            NgayCapNhat = GETDATE(),
            NguoiCapNhat = @NguoiDuyet,
            GhiChu = ISNULL(GhiChu + N'; ', N'') + ISNULL(@GhiChu, N'Đã duyệt') -- Nối ghi chú duyệt vào ghi chú cũ
        WHERE MaDonXuat = @MaDonXuat;

        -- === 5. GHI AUDIT LOG ===
        INSERT INTO dbo.AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GiaTriCu, GiaTriMoi, GhiChu)
        VALUES (GETDATE(), @NguoiDuyet, N'QLDonXuat', @MaDonXuat, N'Duyệt', @CurrentTrangThai, N'Hoàn thành', ISNULL(@GhiChu, N'Đã duyệt'));

        COMMIT TRANSACTION;
        SELECT N'Duyệt đơn xuất thành công!' AS Message; -- Thông báo thành công

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION; -- Hoàn tác nếu có lỗi

        -- Ném lại lỗi để lớp gọi xử lý
        -- SET @ErrorMsg = ERROR_MESSAGE(); -- Lấy thông báo lỗi gốc
        -- THROW 50000, @ErrorMsg, 1; -- Ném lỗi với mã tùy chỉnh nếu muốn
        THROW; -- Ném lại lỗi gốc để giữ nguyên thông tin lỗi

    END CATCH;
END;
GO
ALTER PROCEDURE dbo.sp_NhapHang -- Đảm bảo tên schema dbo.
    @NgayNhap DATE,
    @MaNCC INT,
    @MaTK INT,                 -- Mã tài khoản người tạo đơn
    @MaKhoNhap INT,            -- *** THÊM MỚI: Mã kho nhập hàng vào ***
    @GhiChu NVARCHAR(255) = NULL,
    @ChiTietNhap NVARCHAR(MAX), -- JSON chi tiết
    @NguoiCapNhat INT,         -- Mã tài khoản người cập nhật (có thể trùng MaTK)
    @NguoiNhap INT             -- Mã tài khoản người nhập hàng (có thể trùng MaTK)
AS
BEGIN
    SET NOCOUNT ON; -- Thêm dòng này để tránh thông báo số dòng ảnh hưởng

    BEGIN TRY
        BEGIN TRANSACTION;

        -- === VALIDATION ===
        IF NOT EXISTS (SELECT 1 FROM dbo.NCC WHERE MaNCC = @MaNCC AND TrangThai = N'Hoạt động')
            THROW 50001, N'Nhà cung cấp không tồn tại hoặc không hoạt động!', 1;

        IF NOT EXISTS (SELECT 1 FROM dbo.QLTK WHERE MaTK = @MaTK AND TrangThai = N'Hoạt động')
            THROW 50002, N'Tài khoản người tạo không tồn tại hoặc bị khóa!', 1;

        IF NOT EXISTS (SELECT 1 FROM dbo.QLTK WHERE MaTK = @NguoiNhap AND TrangThai = N'Hoạt động')
            THROW 50006, N'Tài khoản người nhập không tồn tại hoặc bị khóa!', 1;

        -- *** THÊM MỚI: Kiểm tra kho nhập có tồn tại không ***
        IF NOT EXISTS (SELECT 1 FROM dbo.Kho WHERE MaKho = @MaKhoNhap)
             THROW 50007, N'Kho nhập hàng không tồn tại!', 1;

        -- === INSERT ĐƠN NHẬP ===
        INSERT INTO dbo.QLDonNhap (NgayNhap, MaNCC, MaTK, TrangThai, GhiChu, NgayCapNhat, NguoiCapNhat, NguoiNhap)
        VALUES (@NgayNhap, @MaNCC, @MaTK, N'Hoàn thành', @GhiChu, GETDATE(), @NguoiCapNhat, @NguoiNhap);

        DECLARE @MaDonNhap INT = SCOPE_IDENTITY();

        -- === PARSE JSON CHI TIẾT ===
        DECLARE @TempChiTiet TABLE (
            MaVatLieu INT,
            SoLuong DECIMAL(18, 2), -- Nên dùng DECIMAL cho số lượng
            GiaNhap DECIMAL(18,2)
        );
        INSERT INTO @TempChiTiet (MaVatLieu, SoLuong, GiaNhap)
        SELECT
            JSON_VALUE(value, '$.MaVatLieu'),
            TRY_CONVERT(DECIMAL(18, 2), JSON_VALUE(value, '$.SoLuong')), -- Dùng TRY_CONVERT
            TRY_CONVERT(DECIMAL(18, 2), JSON_VALUE(value, '$.GiaNhap'))  -- Dùng TRY_CONVERT
        FROM OPENJSON(@ChiTietNhap);

        -- === VALIDATE CHI TIẾT ===
        IF NOT EXISTS (SELECT 1 FROM @TempChiTiet)
            THROW 50003, N'Danh sách chi tiết nhập hàng trống!', 1;

        IF EXISTS (
            SELECT 1
            FROM @TempChiTiet t
            LEFT JOIN dbo.QLVatLieu v ON t.MaVatLieu = v.MaVatLieu
            WHERE v.MaVatLieu IS NULL OR v.TrangThai != N'Hoạt động'
        )
            THROW 50004, N'Có vật liệu không tồn tại hoặc không hoạt động!', 1;

        IF EXISTS (SELECT 1 FROM @TempChiTiet WHERE SoLuong IS NULL OR SoLuong <= 0 OR GiaNhap IS NULL OR GiaNhap < 0)
            THROW 50005, N'Số lượng hoặc giá nhập không hợp lệ!', 1;

        -- === INSERT CHI TIẾT ĐƠN NHẬP ===
        INSERT INTO dbo.ChiTietDonNhap (MaDonNhap, MaVatLieu, SoLuong, GiaNhap)
        SELECT @MaDonNhap, MaVatLieu, SoLuong, GiaNhap
        FROM @TempChiTiet;

        -- === CẬP NHẬT TỒN KHO (Bảng TonKhoVatLieu) ===
        -- Sử dụng MERGE để vừa INSERT (nếu vật liệu chưa có ở kho này) vừa UPDATE (nếu đã có)
        MERGE dbo.TonKhoVatLieu AS Target
        USING @TempChiTiet AS Source
        ON (Target.MaVatLieu = Source.MaVatLieu AND Target.MaKho = @MaKhoNhap) -- Điều kiện join với kho nhập
        WHEN MATCHED THEN
            -- Nếu vật liệu đã có trong kho nhập -> Cộng thêm số lượng
            UPDATE SET Target.SoLuongTon = Target.SoLuongTon + Source.SoLuong
        WHEN NOT MATCHED BY TARGET THEN
            -- Nếu vật liệu chưa có trong kho nhập -> Thêm mới bản ghi tồn kho
            INSERT (MaVatLieu, MaKho, SoLuongTon)
            VALUES (Source.MaVatLieu, @MaKhoNhap, Source.SoLuong);

        -- === CẬP NHẬT GIÁ NHẬP MỚI NHẤT VÀO QLVatLieu (Tùy chọn) ===
        -- Vẫn có thể cập nhật giá nhập mới nhất vào bảng QLVatLieu nếu muốn
        UPDATE dbo.QLVatLieu
        SET DonGiaNhap = t.GiaNhap, -- Cập nhật giá nhập mới nhất
            NgayCapNhat = GETDATE(),
            NguoiCapNhat = @NguoiCapNhat
        FROM dbo.QLVatLieu v
        INNER JOIN @TempChiTiet t ON v.MaVatLieu = t.MaVatLieu;
        -- *** LƯU Ý: Đã bỏ việc cập nhật QLVatLieu.SoLuong ở đây ***

        -- === GHI AUDIT LOG ===
        INSERT INTO dbo.AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu) -- Bỏ GiaTriCu/Moi nếu không cần
        VALUES (GETDATE(), @NguoiCapNhat, N'QLDonNhap', @MaDonNhap, N'Thêm', N'Thêm đơn nhập hàng mới vào kho ' + CAST(@MaKhoNhap AS NVARCHAR(10)) + N' bởi người nhập: ' + CAST(@NguoiNhap AS NVARCHAR(10)));

        COMMIT TRANSACTION;

        SELECT N'Nhập hàng thành công!' AS Message, @MaDonNhap AS MaDonNhap;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN; -- Thêm return để chắc chắn dừng
    END CATCH
END;
GO
Alter PROCEDURE [dbo].[sp_DuyetDonNhap]
    @MaDonNhap INT,         -- Mã đơn nhập cần duyệt
    @MaTK_NguoiDuyet INT    -- Mã tài khoản người thực hiện duyệt
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TrangThaiHienTai NVARCHAR(50);
    DECLARE @ApprovedStatus NVARCHAR(50) = N'Hoàn thành'; -- Hoặc N'Đã duyệt' tùy bạn chọn

    BEGIN TRY
        -- === VALIDATION ===
        -- Kiểm tra người duyệt có tồn tại và hoạt động không
        IF NOT EXISTS (SELECT 1 FROM dbo.QLTK WHERE MaTK = @MaTK_NguoiDuyet AND TrangThai = N'Hoạt động')
            THROW 50101, N'Tài khoản người duyệt không tồn tại hoặc bị khóa!', 1;

        -- Lấy trạng thái hiện tại của đơn nhập và kiểm tra đơn có tồn tại không
        SELECT @TrangThaiHienTai = TrangThai
        FROM dbo.QLDonNhap
        WHERE MaDonNhap = @MaDonNhap;

        IF @TrangThaiHienTai IS NULL
            THROW 50102, N'Đơn nhập hàng không tồn tại!', 1;

        -- Kiểm tra xem đơn có đang ở trạng thái chờ duyệt không
        IF @TrangThaiHienTai != N'Chờ duyệt' -- *** Đảm bảo trạng thái chờ duyệt là 'Chờ duyệt' ***
            THROW 50103, N'Chỉ có thể duyệt đơn hàng đang ở trạng thái "Chờ duyệt"!', 1;

        BEGIN TRANSACTION;

        -- === CẬP NHẬT TRẠNG THÁI ĐƠN NHẬP ===
        UPDATE dbo.QLDonNhap
        SET TrangThai = @ApprovedStatus,         -- Cập nhật trạng thái mới
            NguoiCapNhat = @MaTK_NguoiDuyet,   -- Ghi nhận người duyệt là người cập nhật cuối
            NgayCapNhat = GETDATE()             -- Ghi nhận thời gian duyệt
        WHERE MaDonNhap = @MaDonNhap;

        -- === LẤY CHI TIẾT ĐƠN ĐỂ CẬP NHẬT TỒN KHO ===
        -- *** Yêu cầu: Bảng ChiTietDonNhap phải có cột MaKho ***
        DECLARE @TempChiTietDuyet TABLE (
            MaVatLieu INT,
            MaKho INT,
            SoLuong DECIMAL(18, 2)
        );

        INSERT INTO @TempChiTietDuyet (MaVatLieu, MaKho, SoLuong)
        SELECT MaVatLieu, MaKho, SoLuong -- Lấy MaKho từ ChiTietDonNhap
        FROM dbo.ChiTietDonNhap
        WHERE MaDonNhap = @MaDonNhap;

        -- Kiểm tra lại nếu không lấy được chi tiết (dù hiếm khi xảy ra nếu đơn đã tồn tại)
        IF NOT EXISTS (SELECT 1 FROM @TempChiTietDuyet)
             THROW 50104, N'Không tìm thấy chi tiết cho đơn nhập hàng này!', 1;
         -- Kiểm tra MaKho có null không trong chi tiết (nếu cột MaKho trong ChiTietDonNhap cho phép NULL)
         IF EXISTS (SELECT 1 FROM @TempChiTietDuyet WHERE MaKho IS NULL)
             THROW 50105, N'Có chi tiết đơn nhập bị thiếu thông tin Kho!', 1;


        -- === CẬP NHẬT TỒN KHO (Bảng TonKhoVatLieu) ===
        MERGE dbo.TonKhoVatLieu AS Target
        USING @TempChiTietDuyet AS Source
        ON (Target.MaVatLieu = Source.MaVatLieu AND Target.MaKho = Source.MaKho) -- Join bằng MaKho từ chi tiết
        WHEN MATCHED THEN
            -- Nếu vật liệu đã có trong kho cụ thể -> Cộng thêm số lượng
            UPDATE SET Target.SoLuongTon = Target.SoLuongTon + Source.SoLuong
        WHEN NOT MATCHED BY TARGET THEN
            -- Nếu vật liệu chưa có trong kho cụ thể -> Thêm mới bản ghi tồn kho
            INSERT (MaVatLieu, MaKho, SoLuongTon)
            VALUES (Source.MaVatLieu, Source.MaKho, Source.SoLuong);

        -- === GHI AUDIT LOG CHO VIỆC DUYỆT ===
        INSERT INTO dbo.AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @MaTK_NguoiDuyet, N'QLDonNhap', @MaDonNhap, N'Duyệt', N'Đơn nhập hàng (ID: ' + CAST(@MaDonNhap AS NVARCHAR(10)) + N') đã được duyệt.');

        COMMIT TRANSACTION;

        SELECT N'Duyệt đơn nhập hàng thành công!' AS Message, @MaDonNhap AS MaDonNhap;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Ghi lại lỗi chi tiết hơn nếu cần
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        -- Có thể ghi log lỗi vào bảng khác ở đây
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN;
    END CATCH
END;
SQL

UPDATE dbo.ChiTietDonNhap
SET MaKho = 1 -- Thay 1 bằng MaKho đúng
WHERE MaDonNhap = 57 AND MaKho IS NULL;
CREATE PROCEDURE [dbo].[sp_DuyetNhaCungCap]
    @MaNCC INT,             -- Mã Nhà Cung Cấp cần duyệt
    @MaTK_NguoiDuyet INT    -- Mã tài khoản người thực hiện duyệt
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TrangThaiHienTai NVARCHAR(50);
    DECLARE @ActiveStatus NVARCHAR(50) = N'Hoạt động'; -- Trạng thái mới sau khi duyệt
    DECLARE @PendingStatus NVARCHAR(50) = N'Chờ duyệt'; -- Trạng thái cần duyệt

    BEGIN TRY
        -- === VALIDATION ===
        -- Kiểm tra người duyệt có tồn tại và hoạt động không
        IF NOT EXISTS (SELECT 1 FROM dbo.QLTK WHERE MaTK = @MaTK_NguoiDuyet AND TrangThai = N'Hoạt động')
            THROW 50201, N'Tài khoản người duyệt không tồn tại hoặc bị khóa!', 1;

        -- Lấy trạng thái hiện tại của NCC và kiểm tra NCC có tồn tại không
        SELECT @TrangThaiHienTai = TrangThai
        FROM dbo.NCC -- Giả sử bảng NCC của bạn tên là NCC
        WHERE MaNCC = @MaNCC;

        IF @TrangThaiHienTai IS NULL
            THROW 50202, N'Nhà cung cấp không tồn tại!', 1;

        -- Kiểm tra xem NCC có đang ở trạng thái chờ duyệt không
        IF @TrangThaiHienTai != @PendingStatus
            THROW 50203, N'Chỉ có thể duyệt Nhà Cung Cấp đang ở trạng thái "Chờ duyệt"!', 1;

        -- TODO: Thêm kiểm tra quyền hạn của người duyệt nếu cần
        -- Ví dụ: Kiểm tra xem @MaTK_NguoiDuyet có thuộc nhóm Admin/Quản lý không

        BEGIN TRANSACTION;

        -- === CẬP NHẬT TRẠNG THÁI NHÀ CUNG CẤP ===
        -- Giả sử bảng NCC có cột NguoiCapNhat và NgayCapNhat
        UPDATE dbo.NCC
        SET
            TrangThai = @ActiveStatus,       
            NguoiCapNhat = @MaTK_NguoiDuyet,  
            NgayCapNhat = GETDATE()           
        WHERE MaNCC = @MaNCC;

        -- Kiểm tra xem việc UPDATE có thành công không (tùy chọn)
        IF @@ROWCOUNT = 0
        BEGIN
             -- Trường hợp hiếm gặp: NCC tồn tại nhưng không update được
             THROW 50204, N'Không thể cập nhật trạng thái Nhà Cung Cấp.', 1;
        END

        -- === GHI AUDIT LOG CHO VIỆC DUYỆT NCC ===
        INSERT INTO dbo.AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @MaTK_NguoiDuyet, N'NCC', @MaNCC, N'Duyệt', N'Nhà cung cấp (ID: ' + CAST(@MaNCC AS NVARCHAR(10)) + N') đã được duyệt.');

        COMMIT TRANSACTION;

        SELECT N'Duyệt Nhà Cung Cấp thành công!' AS Message, @MaNCC AS MaNCC_DaDuyet;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Ghi lại lỗi chi tiết hơn nếu cần
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        -- Có thể ghi log lỗi vào bảng khác ở đây
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN;
    END CATCH
END;
CREATE PROCEDURE [dbo].[sp_TuChoiNhaCungCap]
    @MaNCC INT,             -- Mã Nhà Cung Cấp cần từ chối
    @MaTK_NguoiTuChoi INT  -- Mã tài khoản người thực hiện từ chối
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TrangThaiHienTai NVARCHAR(50);
    DECLARE @RejectedStatus NVARCHAR(50) = N'Từ chối'; -- Trạng thái mới sau khi từ chối
    DECLARE @PendingStatus NVARCHAR(50) = N'Chờ duyệt'; -- Trạng thái cần xử lý

    BEGIN TRY
        -- === VALIDATION ===
        -- Kiểm tra người từ chối có tồn tại và hoạt động không
        IF NOT EXISTS (SELECT 1 FROM dbo.QLTK WHERE MaTK = @MaTK_NguoiTuChoi AND TrangThai = N'Hoạt động')
            THROW 50301, N'Tài khoản người từ chối không tồn tại hoặc bị khóa!', 1;

        -- Lấy trạng thái hiện tại của NCC và kiểm tra NCC có tồn tại không
        SELECT @TrangThaiHienTai = TrangThai
        FROM dbo.NCC -- Giả sử bảng NCC của bạn tên là NCC
        WHERE MaNCC = @MaNCC;

        IF @TrangThaiHienTai IS NULL
            THROW 50302, N'Nhà cung cấp không tồn tại!', 1;

        -- Kiểm tra xem NCC có đang ở trạng thái chờ duyệt không
        IF @TrangThaiHienTai != @PendingStatus
            THROW 50303, N'Chỉ có thể từ chối Nhà Cung Cấp đang ở trạng thái "Chờ duyệt"!', 1;

        -- TODO: Thêm kiểm tra quyền hạn của người từ chối nếu cần

        BEGIN TRANSACTION;

        -- === CẬP NHẬT TRẠNG THÁI NHÀ CUNG CẤP ===
        -- Giả sử bảng NCC có cột NguoiCapNhat, NgayCapNhat và GhiChu (để lưu lý do)
        UPDATE dbo.NCC
        SET
            TrangThai = @RejectedStatus,        -- Cập nhật trạng thái mới
            NguoiCapNhat = @MaTK_NguoiTuChoi,  -- Ghi nhận người từ chối
            NgayCapNhat = GETDATE()       -- Ghi nhận thời gian
        WHERE MaNCC = @MaNCC;

        IF @@ROWCOUNT = 0
        BEGIN
             THROW 50304, N'Không thể cập nhật trạng thái Nhà Cung Cấp.', 1;
        END

        -- === GHI AUDIT LOG CHO VIỆC TỪ CHỐI NCC ===
        INSERT INTO dbo.AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @MaTK_NguoiTuChoi, N'NCC', @MaNCC, N'Từ chối', N'Nhà cung cấp (ID: ' + CAST(@MaNCC AS NVARCHAR(10)) + N') đã bị từ chối.' );
        COMMIT TRANSACTION;

        SELECT N'Từ chối Nhà Cung Cấp thành công!' AS Message, @MaNCC AS MaNCC_DaTuChoi;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN;
    END CATCH
END;
Go
CREATE PROCEDURE [dbo].[sp_ToggleKhoaNhaCungCap]
    @MaNCC INT,                 -- Mã Nhà Cung Cấp cần thay đổi trạng thái
    @MaTK_NguoiThucHien INT     -- Mã tài khoản người thực hiện hành động
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TrangThaiHienTai NVARCHAR(50);
    DECLARE @TrangThaiMoi NVARCHAR(50);
    DECLARE @HanhDong NVARCHAR(50); -- Để ghi log: 'Khóa' hoặc 'Mở khóa'

    -- Định nghĩa các trạng thái
    DECLARE @ActiveStatus NVARCHAR(50) = N'Hoạt động';
    DECLARE @LockedStatus NVARCHAR(50) = N'Bị khóa';

    BEGIN TRY
        -- === VALIDATION ===
        -- Kiểm tra người thực hiện
        IF NOT EXISTS (SELECT 1 FROM dbo.QLTK WHERE MaTK = @MaTK_NguoiThucHien AND TrangThai = N'Hoạt động')
            THROW 50501, N'Tài khoản người thực hiện không tồn tại hoặc bị khóa!', 1;

        -- Lấy trạng thái hiện tại và kiểm tra NCC tồn tại
        SELECT @TrangThaiHienTai = TrangThai
        FROM dbo.NCC -- Giả sử bảng NCC của bạn tên là NCC
        WHERE MaNCC = @MaNCC;

        IF @TrangThaiHienTai IS NULL
            THROW 50502, N'Nhà cung cấp không tồn tại!', 1;

        -- Kiểm tra xem trạng thái hiện tại có phải là 'Hoạt động' hoặc 'Khóa' không
        IF @TrangThaiHienTai NOT IN (@ActiveStatus, @LockedStatus)
            THROW 50503, N'Chỉ có thể Khóa/Mở khóa Nhà Cung Cấp đang ở trạng thái "Hoạt động" hoặc "Khóa"!', 1;

        -- TODO: Thêm kiểm tra quyền hạn nếu cần

        -- === XÁC ĐỊNH HÀNH ĐỘNG VÀ KIỂM TRA NGHIỆP VỤ (NẾU LÀ KHÓA) ===
        IF @TrangThaiHienTai = @ActiveStatus
        BEGIN
            -- Hành động là KHÓA
            SET @TrangThaiMoi = @LockedStatus;
            SET @HanhDong = N'Bị khóa';

            -- *** KIỂM TRA NGHIỆP VỤ: Chỉ kiểm tra đơn hàng mở KHI KHÓA ***
            DECLARE @CoDonHangMo BIT = 0;
            -- Giả sử bạn có SP hoặc Function kiểm tra, hoặc thực hiện query trực tiếp
            -- Ví dụ query trực tiếp:
            IF EXISTS (
                SELECT TOP 1 1
                FROM dbo.QLDonNhap
                WHERE MaNCC = @MaNCC
                  AND TrangThai NOT IN (N'Hoàn thành', N'Đã hủy', N'Từ chối') -- Các trạng thái đã kết thúc
            )
            BEGIN
                SET @CoDonHangMo = 1;
            END

            IF @CoDonHangMo = 1
            BEGIN
                 THROW 50505, N'Không thể khóa Nhà Cung Cấp này vì vẫn còn đơn nhập hàng đang xử lý.', 1;
            END
        END
        ELSE -- Nếu trạng thái hiện tại là @LockedStatus
        BEGIN
            -- Hành động là MỞ KHÓA
            SET @TrangThaiMoi = @ActiveStatus;
            SET @HanhDong = N'Mở khóa';
            -- Không cần kiểm tra đơn hàng mở khi mở khóa
        END

        BEGIN TRANSACTION;

        -- === CẬP NHẬT TRẠNG THÁI NHÀ CUNG CẤP ===
        UPDATE dbo.NCC
        SET
            TrangThai = @TrangThaiMoi,          -- Cập nhật trạng thái mới
            NguoiCapNhat = @MaTK_NguoiThucHien, -- Ghi nhận người thực hiện
            NgayCapNhat = GETDATE()             -- Ghi nhận thời gian
        WHERE MaNCC = @MaNCC;

        IF @@ROWCOUNT = 0
        BEGIN
             THROW 50504, N'Không thể cập nhật trạng thái Nhà Cung Cấp.', 1;
        END

        -- === GHI AUDIT LOG ===
        INSERT INTO dbo.AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @MaTK_NguoiThucHien, N'NCC', @MaNCC, @HanhDong, N'Nhà cung cấp (ID: ' + CAST(@MaNCC AS NVARCHAR(10)) + N') đã được ' + LOWER(@HanhDong) + N'.');

        COMMIT TRANSACTION;

        SELECT N'Thực hiện ' + LOWER(@HanhDong) + N' Nhà Cung Cấp thành công!' AS Message, @MaNCC AS MaNCC_ThucHien;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN;
    END CATCH
END;
CREATE PROCEDURE [dbo].[sp_UpdateDonXuat]
    @MaDonXuat INT,           
    @NgayXuat DATE,            
    @GhiChu NVARCHAR(255) = NULL,
    @MaKhachhang INT = NULL,        
    @MaKhoNguon INT,            
    @MaKhoDich INT = NULL,       
    @LoaiPhieu VARCHAR(55),  
    @NguoiCapNhat INT,      
    @ChiTietMoi NVARCHAR(MAX)  
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TrangThaiHienTai NVARCHAR(50);
    DECLARE @ErrMsg NVARCHAR(MAX);

    DECLARE @ClosedStatus1 NVARCHAR(50) = N'Hoàn thành';
    DECLARE @ClosedStatus2 NVARCHAR(50) = N'Đã hủy';

    BEGIN TRY
      IF NOT EXISTS (SELECT 1 FROM dbo.QLTK WHERE MaTK = @NguoiCapNhat AND TrangThai = N'Hoạt động')
            THROW 50601, N'Tài khoản người cập nhật không tồn tại hoặc bị khóa!', 1;
        SELECT @TrangThaiHienTai = TrangThai FROM dbo.QLDonXuat WHERE MaDonXuat = @MaDonXuat;
        IF @TrangThaiHienTai IS NULL THROW 50602, N'Đơn xuất hàng không tồn tại!', 1;
        IF @TrangThaiHienTai IN (@ClosedStatus1, @ClosedStatus2) THROW 50603, N'Không thể chỉnh sửa đơn xuất hàng đã hoàn thành hoặc đã hủy!', 1;
        IF @LoaiPhieu = 'PX' AND (@MaKhachhang IS NULL OR NOT EXISTS (SELECT 1 FROM QLKH WHERE MaKhachHang = @MaKhachhang)) THROW 50606, N'Khách hàng không hợp lệ cho phiếu xuất bán!', 1;


        IF @ChiTietMoi IS NULL OR LTRIM(RTRIM(@ChiTietMoi)) = '' OR @ChiTietMoi = '[]'
             THROW 50607, N'Chi tiết đơn xuất không được để trống!', 1;


   DECLARE @TempChiTietMoi TABLE (
            MaVatLieu INT,
            MaKhoNguon INT,         
            MaKhoDich INT NULL,     
            SoLuong DECIMAL(18, 2),
            DonGia DECIMAL(18, 2)
        );

     INSERT INTO @TempChiTietMoi (MaVatLieu, MaKhoNguon, MaKhoDich, SoLuong, DonGia)
        SELECT
            TRY_CONVERT(INT, JSON_VALUE(value, '$.MaVatLieu')),
            TRY_CONVERT(INT, JSON_VALUE(value, '$.MaKhoNguon')),
            TRY_CONVERT(INT, JSON_VALUE(value, '$.MaKhoDich')), 
            TRY_CONVERT(DECIMAL(18, 2), JSON_VALUE(value, '$.SoLuong')),
            TRY_CONVERT(DECIMAL(18, 2), JSON_VALUE(value, '$.DonGia'))
        FROM OPENJSON(@ChiTietMoi);


        IF EXISTS (SELECT 1 FROM @TempChiTietMoi WHERE MaVatLieu IS NULL OR MaKhoNguon IS NULL OR SoLuong IS NULL OR DonGia IS NULL)
            THROW 50608, N'Dữ liệu chi tiết trong JSON không hợp lệ hoặc bị thiếu (MaVatLieu, MaKhoNguon, SoLuong, DonGia)!', 1;

        IF EXISTS (SELECT 1 FROM @TempChiTietMoi WHERE SoLuong <= 0 OR DonGia < 0)
            THROW 50609, N'Số lượng phải lớn hơn 0 và Đơn giá không được âm!', 1;

        IF EXISTS ( SELECT 1 FROM @TempChiTietMoi t LEFT JOIN dbo.QLVatLieu vl ON t.MaVatLieu = vl.MaVatLieu WHERE vl.MaVatLieu IS NULL )
            THROW 50610, N'Có vật liệu trong chi tiết không tồn tại trong hệ thống!', 1;

        -- *** THÊM: Kiểm tra Kho Nguồn và Kho Đích trong chi tiết có tồn tại không ***
        IF EXISTS ( SELECT 1 FROM @TempChiTietMoi t LEFT JOIN dbo.Kho k ON t.MaKhoNguon = k.MaKho WHERE k.MaKho IS NULL )
            THROW 50612, N'Có chi tiết chứa Mã Kho Nguồn không tồn tại!', 1;
        IF EXISTS ( SELECT 1 FROM @TempChiTietMoi t LEFT JOIN dbo.Kho k ON t.MaKhoDich = k.MaKho WHERE t.MaKhoDich IS NOT NULL AND k.MaKho IS NULL )
            THROW 50613, N'Có chi tiết chứa Mã Kho Đích không tồn tại!', 1;
        IF @LoaiPhieu = N'Chuyển kho' AND EXISTS (SELECT 1 FROM @TempChiTietMoi WHERE MaKhoDich IS NULL OR MaKhoNguon = MaKhoDich)
             THROW 50614, N'Phiếu chuyển kho yêu cầu Kho Đích khác Kho Nguồn cho mọi chi tiết!', 1;
        IF @LoaiPhieu = N'Xuất hàng' AND EXISTS (SELECT 1 FROM @TempChiTietMoi WHERE MaKhoDich IS NOT NULL)
             THROW 50615, N'Phiếu xuất bán không được có Kho Đích trong chi tiết!', 1;

        -- 4. *** KIỂM TRA TỒN KHO (Dựa trên MaKhoNguon của từng chi tiết) ***
        DECLARE @ChiTietGoc TABLE (MaVatLieu INT, MaKhoNguon INT, SoLuongGoc DECIMAL(18,2));
        -- Lấy chi tiết gốc, bao gồm cả MaKhoNguon gốc
        INSERT INTO @ChiTietGoc (MaVatLieu, MaKhoNguon, SoLuongGoc)
        SELECT MaVatLieu, MaKhoNguon, SoLuong FROM dbo.ChiTietDonXuat WHERE MaDonXuat = @MaDonXuat;

        DECLARE @TonKhoCheck TABLE (MaVatLieu INT, MaKhoNguon INT, SoLuongCan DECIMAL(18,2), TonKhoKhaDung DECIMAL(18,2));
        INSERT INTO @TonKhoCheck (MaVatLieu, MaKhoNguon, SoLuongCan, TonKhoKhaDung)
        SELECT
            tm.MaVatLieu,
            tm.MaKhoNguon, -- Kho nguồn từ chi tiết mới
            tm.SoLuong,    -- Số lượng cần mới
            -- Tồn khả dụng = Tồn hiện tại ở kho nguồn MỚI + Lượng đã xuất trong đơn gốc TẠI KHO NGUỒN GỐC CỦA DÒNG ĐÓ
            ISNULL(tk.SoLuongTon, 0) + ISNULL(ctg.SoLuongGoc, 0)
        FROM @TempChiTietMoi tm
        LEFT JOIN @ChiTietGoc ctg ON tm.MaVatLieu = ctg.MaVatLieu -- Join chi tiết gốc để lấy số lượng cũ
        LEFT JOIN dbo.TonKhoVatLieu tk ON tm.MaVatLieu = tk.MaVatLieu AND tk.MaKho = tm.MaKhoNguon; -- Join tồn kho tại kho nguồn MỚI

        -- Kiểm tra xem có dòng nào số lượng cần > tồn kho khả dụng không
        IF EXISTS (SELECT 1 FROM @TonKhoCheck WHERE SoLuongCan > TonKhoKhaDung)
        BEGIN
            SET @ErrMsg = N'Không đủ tồn kho tại kho nguồn:';
            SELECT @ErrMsg = @ErrMsg + CHAR(13) + CHAR(10) +
                             N'- VL ID ' + CAST(MaVatLieu AS VARCHAR(10)) +
                             N' tại Kho ID ' + CAST(MaKhoNguon AS VARCHAR(10)) +
                             N': Cần ' + CAST(SoLuongCan AS VARCHAR(20)) +
                             N', Khả dụng ' + CAST(TonKhoKhaDung AS VARCHAR(20))
            FROM @TonKhoCheck WHERE SoLuongCan > TonKhoKhaDung;
            THROW 50611, @ErrMsg, 1;
        END;


        -- === THỰC THI CẬP NHẬT ===
        BEGIN TRANSACTION;

        -- 1. Cập nhật Header Đơn Xuất (Dùng MaKhoNguon/Dich từ tham số header)
        UPDATE dbo.QLDonXuat
        SET NgayXuat = @NgayXuat,
            GhiChu = @GhiChu,
            MaKhachHang = @MaKhachhang,
            LoaiXuat = @LoaiPhieu,
            NguoiCapNhat = @NguoiCapNhat,
            NgayCapNhat = GETDATE()
        WHERE MaDonXuat = @MaDonXuat;

        DELETE FROM dbo.ChiTietDonXuat WHERE MaDonXuat = @MaDonXuat;

        INSERT INTO dbo.ChiTietDonXuat (MaDonXuat, MaVatLieu, MaKhoNguon, MaKhoDich, SoLuong, DonGia)
        SELECT @MaDonXuat, MaVatLieu, MaKhoNguon, MaKhoDich, SoLuong, DonGia
        FROM @TempChiTietMoi;

        INSERT INTO dbo.AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @NguoiCapNhat, N'QLDonXuat', @MaDonXuat, N'Sửa', N'Cập nhật đơn xuất hàng (ID: ' + CAST(@MaDonXuat AS NVARCHAR(10)) + N').');

        COMMIT TRANSACTION;

        SELECT N'Cập nhật đơn xuất hàng thành công!' AS Message, @MaDonXuat AS MaDonXuat_Updated;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

       SET @ErrMsg = ERROR_MESSAGE(); -- Đúng tên biến

        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        RAISERROR (@ErrMsg, @ErrorSeverity, @ErrorState); 
        RETURN;
    END CATCH
END;
CREATE PROCEDURE [dbo].[sp_TuChoiDonXuat]
    @MaDonXuat INT,             -- Mã Đơn Xuất cần từ chối
    @MaTK_NguoiTuChoi INT     -- Mã tài khoản người thực hiện từ chối
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TrangThaiHienTai NVARCHAR(50);
    DECLARE @MaHoaDon INT = NULL;
    DECLARE @RejectedStatus NVARCHAR(50) = N'Từ chối';   -- Trạng thái mới
    DECLARE @PendingStatus NVARCHAR(50) = N'Chờ duyệt'; -- Trạng thái yêu cầu

    BEGIN TRY
        -- === VALIDATION ===
        -- 1. Kiểm tra người từ chối
        IF NOT EXISTS (SELECT 1 FROM dbo.QLTK WHERE MaTK = @MaTK_NguoiTuChoi AND TrangThai = N'Hoạt động')
            THROW 50701, N'Tài khoản người từ chối không tồn tại hoặc bị khóa!', 1;

        -- 2. Lấy trạng thái hiện tại và Mã Hóa Đơn (nếu có) của Đơn Xuất
        SELECT
            @TrangThaiHienTai = TrangThai,
            @MaHoaDon = MaHoaDon -- Giả sử QLDonXuat có cột MaHoaDon liên kết đến QLHoaDon
        FROM dbo.QLDonXuat
        WHERE MaDonXuat = @MaDonXuat;

        IF @TrangThaiHienTai IS NULL
            THROW 50702, N'Đơn xuất hàng không tồn tại!', 1;

        -- 3. Kiểm tra trạng thái hiện tại có phải là "Chờ duyệt" không
        IF @TrangThaiHienTai != @PendingStatus
            THROW 50703, N'Chỉ có thể từ chối Đơn Xuất Hàng đang ở trạng thái "Chờ duyệt"!', 1;

        -- 4. *** KIỂM TRA HÓA ĐƠN ***
        -- Kiểm tra xem đơn xuất này đã được liên kết với hóa đơn nào chưa
        -- (Giả sử cột MaHoaDon trong QLDonXuat sẽ có giá trị nếu đã tạo hóa đơn)
        IF @MaHoaDon IS NOT NULL
            THROW 50704, N'Không thể từ chối Đơn Xuất Hàng này vì đã có Hóa Đơn liên kết. Vui lòng hủy Hóa Đơn trước.', 1;


        BEGIN TRANSACTION;

        UPDATE dbo.QLDonXuat
        SET
            TrangThai = @RejectedStatus,        -- Cập nhật trạng thái mới
            NguoiCapNhat = @MaTK_NguoiTuChoi,  -- Ghi nhận người từ chối
            NgayCapNhat = GETDATE(),           -- Ghi nhận thời gian
            GhiChu = ISNULL(GhiChu + N'; ', N'') + N'Bị từ chối bởi TK '
                     + CAST(@MaTK_NguoiTuChoi AS NVARCHAR(10)) 
        WHERE MaDonXuat = @MaDonXuat;
        IF @@ROWCOUNT = 0
        BEGIN
             THROW 50705, N'Không thể cập nhật trạng thái Đơn Xuất Hàng.', 1;
        END


        -- === GHI AUDIT LOG ===
        INSERT INTO dbo.AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @MaTK_NguoiTuChoi, N'QLDonXuat', @MaDonXuat, N'Từ chối', N'Đơn xuất hàng (ID: ' + CAST(@MaDonXuat AS NVARCHAR(10)) + N') đã bị từ chối.');

        COMMIT TRANSACTION;

        SELECT N'Từ chối Đơn Xuất Hàng thành công!' AS Message, @MaDonXuat AS MaDonXuat_DaTuChoi;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN;
    END CATCH
END;
CREATE PROCEDURE [dbo].[sp_TuChoiHoaDon]
    @MaHoaDon INT,             -- Mã Hóa Đơn cần từ chối/hủy
    @MaTK_NguoiThucHien INT  -- Mã tài khoản người thực hiện
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TrangThaiHienTai NVARCHAR(50);
    DECLARE @RejectedStatus NVARCHAR(50) = N'Đã hủy'; -- Hoặc N'Bị từ chối'
    DECLARE @FinalStatus1 NVARCHAR(50) = N'Đã thanh toán';
    DECLARE @FinalStatus2 NVARCHAR(50) = N'Đã hủy'; -- Chính nó
    DECLARE @FinalStatus3 NVARCHAR(50) = N'Bị từ chối'; -- Chính nó

    BEGIN TRY
        -- === VALIDATION ===
        -- 1. Kiểm tra người thực hiện
        IF NOT EXISTS (SELECT 1 FROM dbo.QLTK WHERE MaTK = @MaTK_NguoiThucHien AND TrangThai = N'Hoạt động')
            THROW 50801, N'Tài khoản người thực hiện không tồn tại hoặc bị khóa!', 1;

        -- 2. Lấy trạng thái hiện tại và kiểm tra Hóa Đơn tồn tại
        SELECT @TrangThaiHienTai = TrangThai -- Giả sử có cột TrangThai
        FROM dbo.QLHoaDon -- Giả sử bảng Hóa Đơn là QLHoaDon
        WHERE MaHoaDon = @MaHoaDon;

        IF @TrangThaiHienTai IS NULL
            THROW 50802, N'Hóa đơn không tồn tại!', 1;

        -- 3. Kiểm tra xem trạng thái hiện tại có cho phép từ chối/hủy không
        IF @TrangThaiHienTai IN (@FinalStatus1, @FinalStatus2, @FinalStatus3)
            THROW 50803, N'Không thể Từ chối/Hủy Hóa Đơn ở trạng thái này!', 1;

        -- TODO: Thêm kiểm tra quyền hạn nếu cần

        BEGIN TRANSACTION;

        -- === CẬP NHẬT TRẠNG THÁI HÓA ĐƠN ===
        -- Giả sử bảng QLHoaDon có cột NguoiCapNhat, NgayCapNhat, GhiChu
        UPDATE dbo.QLHoaDon
        SET
            TrangThai = @RejectedStatus,        -- Cập nhật trạng thái mới
            NguoiCapNhat = @MaTK_NguoiThucHien,  -- Ghi nhận người thực hiện
            NgayCapNhat = GETDATE()        -- Ghi nhận thời gian
        WHERE MaHoaDon = @MaHoaDon;

        IF @@ROWCOUNT = 0
        BEGIN
             THROW 50804, N'Không thể cập nhật trạng thái Hóa Đơn.', 1;
        END



        -- === GHI AUDIT LOG ===
        INSERT INTO dbo.AuditLog (ThoiGian, MaTK, TenBang, MaBanGhi, HanhDong, GhiChu)
        VALUES (GETDATE(), @MaTK_NguoiThucHien, N'QLHoaDon', @MaHoaDon, N'Từ chối/Hủy', N'Hóa đơn (ID: ' + CAST(@MaHoaDon AS NVARCHAR(10)) + N') đã bị từ chối/hủy.');

        COMMIT TRANSACTION;

        SELECT N'Từ chối/Hủy Hóa Đơn thành công!' AS Message, @MaHoaDon AS MaHoaDon_DaXuLy;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN;
    END CATCH
END;
