USE [QLVT]
GO
/****** Object:  StoredProcedure [dbo].[sp_TimKiemKH]    Script Date: 3/21/2025 12:53:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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