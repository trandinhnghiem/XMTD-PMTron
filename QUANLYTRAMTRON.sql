-- Tạo database QUANLYTRAMTRON
CREATE DATABASE QUANLYTRAMTRON
GO

USE QUANLYTRAMTRON
GO

-- Bảng NGUOIDUNG (dùng cho đăng nhập)
CREATE TABLE NGUOIDUNG (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    USERNAME NVARCHAR(50) UNIQUE NOT NULL,
    PASSWORD NVARCHAR(50) NOT NULL,
    HOTEN NVARCHAR(100),
    DIACHI NVARCHAR(200),
    CAPDO NVARCHAR(20) CHECK (CAPDO IN (N'Quản lý', N'Vận hành')),
    EMAIL NVARCHAR(100),
    DANGNHAPCUOI DATETIME,
	QUYEN NVARCHAR(MAX) NULL
)
GO

-- Bảng KHACHHANG
CREATE TABLE KHACHHANG (
    MAKHACH INT IDENTITY(1,1) PRIMARY KEY,
    TENKHACH NVARCHAR(100) NOT NULL,
    DIACHI NVARCHAR(200),
    DIENTHOAI NVARCHAR(20)
)
GO

-- Bảng CONGTRINH
CREATE TABLE CONGTRINH (
    MACONGTRINH INT IDENTITY(1,1) PRIMARY KEY,
    DIADIEM NVARCHAR(200) NOT NULL,
    HANGMUC NVARCHAR(200),
    THIETBIBOM NVARCHAR(200)
)
GO

-- Bảng XE
CREATE TABLE XE (
    MAXE INT IDENTITY(1,1) PRIMARY KEY,
    BIENSO NVARCHAR(20) NOT NULL,
    LAIXE NVARCHAR(100) NOT NULL
)
GO

-- Bảng PHUGIA
CREATE TABLE PHUGIA (
    MAPHUGIA INT IDENTITY(1,1) PRIMARY KEY,
    TENPHUGIA NVARCHAR(100) NOT NULL,
    GHICHU NVARCHAR(500)
)
GO

-- Bảng KINHDOANH
CREATE TABLE KINHDOANH (
    MAKINHDOANH INT IDENTITY(1,1) PRIMARY KEY,
    TENKINHDOANH NVARCHAR(200) NOT NULL
)
GO

-- Bảng TRAM
CREATE TABLE TRAM (
    MATRAM INT IDENTITY(1,1) PRIMARY KEY,
    TENTRAM NVARCHAR(200) NOT NULL,
    CHUTRAM NVARCHAR(200),
    DIADIEM NVARCHAR(200),
    CONGSUAT NVARCHAR(50),
    DIENTHOAI NVARCHAR(20),
    TRANGTHAI NVARCHAR(20) DEFAULT 'Online',
    DUONGDANRUNTIME NVARCHAR(500),
    LOAICAPPHOI NVARCHAR(50)
)
GO

-- Bảng DONHANG (đã bổ sung MAXE và DIACHI)
CREATE TABLE DONHANG (
    MADONHANG INT IDENTITY(1,1) PRIMARY KEY,
    NGAYDAT DATETIME NOT NULL DEFAULT GETDATE(),
    KYHIEUDON NVARCHAR(50),
    SOPHIEU NVARCHAR(50),
    KHOILUONG DECIMAL(10,2),
    TICHLUY DECIMAL(10,2),
    MATRAM INT FOREIGN KEY REFERENCES TRAM(MATRAM),
    MAKHACH INT FOREIGN KEY REFERENCES KHACHHANG(MAKHACH),
    MAKINHDOANH INT FOREIGN KEY REFERENCES KINHDOANH(MAKINHDOANH),
    MACONGTRINH INT FOREIGN KEY REFERENCES CONGTRINH(MACONGTRINH),
    MAXE INT FOREIGN KEY REFERENCES XE(MAXE),   -- ✅ Bổ sung
    NHANVIENKD NVARCHAR(100),
    DIACHI NVARCHAR(200),                      -- ✅ Bổ sung
    HOATDONG BIT DEFAULT 1
)
GO

-- Bảng CAPPHOI
CREATE TABLE CAPPHOI (
    MACAPPHOI INT IDENTITY(1,1) PRIMARY KEY,
    STT INT,
    MACBETONG NVARCHAR(50) NOT NULL,
    CUONGDO NVARCHAR(50),
    COTLIEUMAX NVARCHAR(50),
    DOSUT NVARCHAR(20),
    TONGSLVATTU DECIMAL(10,2),
    GHICHU NVARCHAR(500)
)
GO

-- Bảng VATTU
CREATE TABLE VATTU (
    MAVATTU INT IDENTITY(1,1) PRIMARY KEY,
    TENVATTU NVARCHAR(100) NOT NULL,
    DONVITINH NVARCHAR(50),
    HESOQUYDOI DECIMAL(10,2),
    TONKHO DECIMAL(10,2) DEFAULT 0
)
GO

-- Bảng KHO
CREATE TABLE KHO (
    MAKHO INT IDENTITY(1,1) PRIMARY KEY,
    NGAYGIAODICH DATETIME NOT NULL DEFAULT GETDATE(),
    LOAIGIAODICH NVARCHAR(20) CHECK (LOAIGIAODICH IN (N'Nhập', N'Xuất', N'Tịnh kho')),
    MAVATTU INT FOREIGN KEY REFERENCES VATTU(MAVATTU),
    SOPHIEU NVARCHAR(50),
    SOHOPDONG NVARCHAR(50),
    SOLUONG DECIMAL(10,2),
    SOLUONGKG DECIMAL(10,2),
    PHUONGTIEN NVARCHAR(100),
    LAIXE NVARCHAR(100),
    DONVIVANCHUYEN NVARCHAR(100),
    NHACUNGCAP NVARCHAR(200),
    TONKHO DECIMAL(10,2),
    GHICHU NVARCHAR(500),
    MATRAM INT FOREIGN KEY REFERENCES TRAM(MATRAM)
)
GO

-- Bảng PHIEUXUAT
CREATE TABLE PHIEUXUAT (
    MAPHIEUXUAT INT IDENTITY(1,1) PRIMARY KEY,
    SOPHIEU NVARCHAR(50) NOT NULL,
    NGAYXUAT DATETIME NOT NULL DEFAULT GETDATE(),
    MADONHANG INT FOREIGN KEY REFERENCES DONHANG(MADONHANG),
    MAXE INT FOREIGN KEY REFERENCES XE(MAXE),
    MAKHACH INT FOREIGN KEY REFERENCES KHACHHANG(MAKHACH),
    MACONGTRINH INT FOREIGN KEY REFERENCES CONGTRINH(MACONGTRINH),
    MACBETONG NVARCHAR(50),
    KHOILUONG DECIMAL(10,2),
    THOIGIANTRON DATETIME,
    GHICHU NVARCHAR(500),
    MATRAM INT FOREIGN KEY REFERENCES TRAM(MATRAM),
    THOIGIANBATDAU DATETIME,
    THOIGIANKETTHUC DATETIME,
    THIETBIBOM NVARCHAR(100),
    SUDUNGBOM BIT DEFAULT 0
)
GO

-- Bảng CHITIETPHIEUXUAT
CREATE TABLE CHITIETPHIEUXUAT (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    MAPHIEUXUAT INT FOREIGN KEY REFERENCES PHIEUXUAT(MAPHIEUXUAT),
    MAVATTU INT FOREIGN KEY REFERENCES VATTU(MAVATTU),
    SOLUONG DECIMAL(10,2),
    GHICHU NVARCHAR(500)
)
GO

-- Bảng CAIDATCHUNG
CREATE TABLE CAIDATCHUNG (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    THIETLAP NVARCHAR(100) NOT NULL,
    GIATRI NVARCHAR(500),
    GHICHU NVARCHAR(500)
)
GO

-- Bảng EMAILCONFIG
CREATE TABLE EMAILCONFIG (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    MATRAM INT FOREIGN KEY REFERENCES TRAM(MATRAM),
    CHEDOGUI NVARCHAR(20) CHECK (CHEDOGUI IN ('Không gửi', 'Hàng giờ', 'Hàng ngày')),
    GUITUNGAY DATETIME,
    MAUBAOCAO NVARCHAR(20) CHECK (MAUBAOCAO IN ('Tổng', 'Chi tiết')),
    GHICHU NVARCHAR(500)
)
GO

-- Bảng DONGBO
CREATE TABLE DONGBO (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    MATRAM INT FOREIGN KEY REFERENCES TRAM(MATRAM),
    KIEUDONGBO NVARCHAR(50) CHECK (KIEUDONGBO IN ('Từ Máy Trạm → Máy Chủ', 'Từ Máy Chủ → Máy Trạm', 'Không đồng bộ')),
    LANCUOIDONGBO DATETIME,
    TRANGTHAI NVARCHAR(20),
    GHICHU NVARCHAR(500)
)
GO

-- Bảng LICHSUDANGNHAP
CREATE TABLE LICHSUDANGNHAP (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    IDNGUOIDUNG INT FOREIGN KEY REFERENCES NGUOIDUNG(ID),
    THOIGIANDANGNHAP DATETIME NOT NULL DEFAULT GETDATE(),
    IPADDRESS NVARCHAR(50),
    TRANGTHAI NVARCHAR(20)
)
GO

-- Bảng CUA_VATTU
CREATE TABLE CUA_VATTU (
    MACUA INT IDENTITY(1,1) PRIMARY KEY,
    STT INT,
    TENTRAM NVARCHAR(200),
    LOAIVATTU NVARCHAR(100),
    TENCUA NVARCHAR(100) NOT NULL,
    HESOQUYDOI DECIMAL(10,2),
    DONVITINH NVARCHAR(50)
)
GO

-- Bảng THIETLAPIN
CREATE TABLE THIETLAPIN (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    TENMAUIN NVARCHAR(50) NOT NULL,
    LOAIIN NVARCHAR(20) CHECK (LOAIIN IN (N'Chi tiết', N'Tổng hợp')),
    GHICHU NVARCHAR(500)
)
GO

-- Bảng THONGKE
CREATE TABLE THONGKE (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    MADONHANG INT FOREIGN KEY REFERENCES DONHANG(MADONHANG),
    MAKHACH INT FOREIGN KEY REFERENCES KHACHHANG(MAKHACH),
    MACONGTRINH INT FOREIGN KEY REFERENCES CONGTRINH(MACONGTRINH),
    MAKINHDOANH INT FOREIGN KEY REFERENCES KINHDOANH(MAKINHDOANH),
    MAXE INT FOREIGN KEY REFERENCES XE(MAXE),
    MACBETONG NVARCHAR(50),
    KYHIEUDON NVARCHAR(50),
    SOLUONG DECIMAL(10,2),
    NGAYTHONGKE DATETIME DEFAULT GETDATE(),
    LOAITHONGKE NVARCHAR(20) CHECK (LOAITHONGKE IN ('Chi tiết', 'Tổng hợp', 'Số chuyến'))
)
GO

-- Bảng CHITIET_VATTU_THONGKE
CREATE TABLE CHITIET_VATTU_THONGKE (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    IDTHONGKE INT FOREIGN KEY REFERENCES THONGKE(ID),
    MAVATTU INT FOREIGN KEY REFERENCES VATTU(MAVATTU),
    SOLUONG DECIMAL(10,2)
)
GO

-- Thêm dữ liệu mẫu cho bảng NGUOIDUNG
INSERT INTO NGUOIDUNG (USERNAME, PASSWORD, HOTEN, CAPDO, QUYEN) 
VALUES 
(N'admin', N'123456', N'Administrator', N'Quản lý', N'Danh mục, Cấp phối, Thống kê, Cài đặt, Kho, Đặt hàng, In phiếu'),
(N'user1', N'123456', N'Người dùng 1', N'Vận hành', N'Danh mục, Cấp phối'),
(N'user2', N'123456', N'Người dùng 2', N'Vận hành', N'Thống kê, Kho'),
(N'quanly', N'123456', N'Quản lý hệ thống', N'Quản lý', N'Danh mục, Cấp phối, Thống kê, Cài đặt'),
(N'kho', N'123456', N'Nhân viên kho', N'Vận hành', N'Kho')
GO

-- Thêm dữ liệu mẫu cho bảng TRAM
INSERT INTO TRAM (TENTRAM, CHUTRAM, DIADIEM, CONGSUAT, DIENTHOAI, TRANGTHAI) 
VALUES 
(N'CÔNG TY CỔ PHẦN BÊ TÔNG TÂY ĐÔ', N'CÔNG TY CỔ PHẦN BÊ TÔNG TÂY ĐÔ', N'T 90 Băng Tải - Hậu Giang', N'90m3', N'123456789', N'Online'),
(N'TRẠM THỬ NGHIỆM', N'TRẠM THỬ NGHIỆM', N'Địa điểm thử nghiệm', N'82m3', N'987654321', N'Online'),
(N'82m3 - T 82 Xe kíp - M', N'TRẠM 82', N'TP.HCM', N'82m3', N'0912345678', N'Online'),
(N'150m3 - T 150 - Ô Môn 1', N'TRẠM 150', N'Cần Thơ', N'150m3', N'0913456789', N'Online'),
(N'150m3 - T 150 - Ô Môn 2', N'TRẠM 150', N'Cần Thơ', N'150m3', N'0914567890', N'Online')
GO

-- Thêm dữ liệu mẫu cho bảng KHACHHANG
INSERT INTO KHACHHANG (TENKHACH, DIACHI) 
VALUES 
(N'ANH DƯƠNG', N''),
(N'CTY TNHH TM-XD VẠN AN PHÁT CT', N''),
(N'ANH GIÀU', N''),
(N'CTY HÙNG THỊNH', N''),
(N'CTY 585', N''),
(N'CTY THIÊN MINH', N''),
(N'CTY BẢO LONG', N''),
(N'CTY TNHH CT MINH THÀNH', N''),
(N'ANH TÂM', N''),
(N'NGUYỄN VĂN CẢNH', N''),
(N'Công ty Xây dựng An Phát', N'Quận 1, TP.HCM'),
(N'Công ty Hoà Bình', N'Quận 7, TP.HCM'),
(N'Công ty Nam Thành', N'Bình Thạnh, TP.HCM')
GO

-- Thêm dữ liệu mẫu cho bảng CONGTRINH
INSERT INTO CONGTRINH (DIADIEM, HANGMUC, THIETBIBOM) 
VALUES 
(N'Công trình A', N'Xây dựng hạ tầng', N'Bơm nước 3HP'),
(N'Công trình B', N'Lắp đặt điện', N'Bơm công nghiệp 5HP'),
(N'Công trình C', N'Thi công PCCC', N'Bơm cứu hỏa'),
(N'Công trình D', N'Nhà xưởng sản xuất', N'Máy nén khí 15kW'),
(N'Công trình E', N'Chung cư cao tầng', N'Thang máy tải khách'),
(N'Công trình F', N'Cầu đường', N'Máy phát điện 100kVA'),
(N'Công trình G', N'Kho bãi logistics', N'Hệ thống lạnh công nghiệp'),
(N'Công trình H', N'Khu đô thị', N'Máy bơm nước thải 7.5HP'),
(N'KHO BẠC NHÀ NƯỚC HẬU GIANG', N'Kho bạc', N'Máy bơm công nghiệp'),
(N'QL61C, H.CHÂU THÀNH', N'Cầu đường', N'Máy xúc'),
(N'KCN TÂN PHÚ THẠNH', N'Khu công nghiệp', N'Máy trộn bê tông'),
(N'QL1A, P.7, TP.VĨNH LONG', N'Quốc lộ', N'Máy đầm'),
(N'CẦU CÁI RĂNG - CẦN THƠ', N'Cầu', N'Máy khoan'),
(N'KDC HƯNG PHÚ 1, CẦN THƠ', N'Khu dân cư', N'Máy bơm bê tông'),
(N'KĐT MỚI NAM CẦN THƠ', N'Khu đô thị', N'Máy trộn bê tông'),
(N'KCN TRÀ NÓC 2, CẦN THƠ', N'Khu công nghiệp', N'Máy nâng')
GO

-- Thêm dữ liệu mẫu cho bảng XE
INSERT INTO XE (BIENSO, LAIXE) 
VALUES 
(N'51C-34567', N'Nguyễn Văn An'),
(N'51D-67890', N'Trần Quốc Bảo'),
(N'60A-24680', N'Lê Minh Cường'),
(N'65H-11223', N'Phạm Thị Dung'),
(N'66B-33445', N'Huỳnh Tấn Đạt'),
(N'67C-55667', N'Ngô Hoàng Phúc'),
(N'68D-77889', N'Đỗ Thị Hồng'),
(N'69E-99001', N'Bùi Văn Hùng'),
(N'51D-12345', N'Nguyễn Văn An'),
(N'51D-67890', N'Trần Quốc Bảo'),
(N'51C-54321', N'Lê Minh Cường')
GO

-- Thêm dữ liệu mẫu cho bảng PHUGIA
INSERT INTO PHUGIA (TENPHUGIA, GHICHU) 
VALUES 
(N'Phụ gia siêu dẻo', N'Tăng độ chảy, giảm nước khi trộn bê tông'),
(N'Phụ gia chậm đông kết', N'Kéo dài thời gian ninh kết, phù hợp công trình lớn'),
(N'Phụ gia chống thấm', N'Tăng khả năng chống thấm cho bê tông và vữa'),
(N'Phụ gia khoáng hoạt tính', N'Cải thiện cường độ và độ bền lâu dài'),
(N'Phụ gia cuốn khí', N'Tăng khả năng chống nứt và chống băng giá'),
(N'Phụ gia tăng nhanh đông kết', N'Rút ngắn thời gian ninh kết, thi công nhanh'),
(N'Phụ gia giảm co ngót', N'Giảm hiện tượng nứt do co ngót'),
(N'Phụ gia khoáng mịn (Silica Fume)', N'Tăng độ đặc chắc, chống xâm thực hóa chất')
GO

-- Thêm dữ liệu mẫu cho bảng KINHDOANH
INSERT INTO KINHDOANH (TENKINHDOANH) 
VALUES 
(N'Công ty TNHH Thương mại An Phát'),
(N'Công ty CP Xây dựng Minh Tâm'),
(N'Doanh nghiệp Tư nhân Hoàng Gia'),
(N'Công ty TNHH SX - TM Đại Thành'),
(N'Công ty CP Vận tải Đông Dương'),
(N'Công ty TNHH Cơ khí Tân Tiến'),
(N'Công ty CP VLXD Sài Gòn'),
(N'Doanh nghiệp Tư nhân Hòa Bình')
GO

-- Thêm dữ liệu mẫu cho bảng CAPPHOI
INSERT INTO CAPPHOI (STT, MACBETONG, CUONGDO, COTLIEUMAX, DOSUT) 
VALUES 
(1, N'C30R28-10±2', N'25', N'20', N'10±2'),
(2, N'C20R28-12±2', N'20', N'16', N'12±2'),
(3, N'C25R28-14±2', N'22', N'18', N'14±2'),
(4, N'C35R28-10±2', N'28', N'22', N'10±2'),
(5, N'C40R28-16±2', N'30', N'24', N'16±2'),
(6, N'C25R14-12±2', N'21', N'17', N'12±2'),
(7, N'C30R14-10±2', N'24', N'19', N'10±2'),
(8, N'C20R14-14±2', N'18', N'15', N'14±2')
GO

-- Thêm dữ liệu mẫu cho bảng VATTU
INSERT INTO VATTU (TENVATTU, DONVITINH, HESOQUYDOI) 
VALUES 
(N'Xi măng', N'Bao', N'50'),
(N'Cát', N'm3', N'1500'),
(N'Đá', N'm3', N'1600'),
(N'Nước', N'L', N'1'),
(N'Phụ gia siêu dẻo', N'Kg', N'1'),
(N'Phụ gia chậm đông kết', N'Kg', N'1'),
(N'Thép', N'Thanh', N'12'),
(N'Gạch', N'Viên', N'2.5')
GO

-- Thêm dữ liệu mẫu cho bảng DONHANG
INSERT INTO DONHANG (NGAYDAT, KYHIEUDON, SOPHIEU, KHOILUONG, TICHLUY, MAKHACH, DIACHI) 
VALUES 
('2025-07-27', N'A01', N'0', 24, 0, 2, N'KHO BẠC NHÀ NƯỚC HẬU GIANG'),
('2025-07-05', N'A02', N'0', 20, 0, 3, N'QL61C, H.CHÂU THÀNH'),
('2025-07-10', N'A03', N'0', 15, 0, 4, N'KCN TÂN PHÚ THẠNH'),
('2025-07-15', N'A04', N'0', 30, 0, 5, N'QL1A, P.7, TP.VĨNH LONG'),
('2025-07-20', N'A05', N'0', 18, 0, 6, N'CẦU CÁI RĂNG - CẦN THƠ'),
('2025-07-22', N'A06', N'0', 25, 0, 7, N'KDC HƯNG PHÚ 1, CẦN THƠ'),
('2025-07-24', N'A07', N'0', 40, 0, 8, N'KĐT MỚI NAM CẦN THƠ'),
('2025-07-30', N'A08', N'0', 12, 0, 9, N'KCN TRÀ NÓC 2, CẦN THƠ')
GO


-- Thêm dữ liệu mẫu cho bảng KHO với tiền tố N cho Unicode
INSERT INTO KHO (NGAYGIAODICH, LOAIGIAODICH, MAVATTU, SOPHIEU, SOHOPDONG, SOLUONG, SOLUONGKG, PHUONGTIEN, LAIXE, DONVIVANCHUYEN, NHACUNGCAP, TONKHO, MATRAM) 
VALUES 
('2025-07-01', N'Nhập', 1, N'PN001', N'HD001', 100, 5000, N'Xe tải 5 tấn', N'Nguyễn Văn A', N'Công ty vận tải ABC', N'Công ty Xi măng Đồng Nai', 1500, 1),
('2025-07-02', N'Nhập', 2, N'PN002', N'HD002', 50, 75000, N'Xe ben 10 tấn', N'Trần Văn B', N'Công ty vận tải XYZ', N'Công ty Cát Sông Đà', 2500, 1),
('2025-07-03', N'Xuất', 1, N'PX001', NULL, 30, 1500, N'Xe mixer', N'Lê Văn C', N'Công ty vận tải ABC', NULL, 1470, 1),
('2025-07-04', N'Xuất', 2, N'PX002', NULL, 20, 30000, N'Xe ben 8 tấn', N'Phạm Văn D', N'Công ty vận tải XYZ', NULL, 2480, 1),
('2025-07-05', N'Nhập', 3, N'PN003', N'HD003', 80, 80, N'Xe tải 3 tấn', N'Ngô Văn E', N'Công ty vận tải DEF', N'Công ty Đá Hòa Phú', 80, 1),
('2025-07-06', N'Tịnh kho', 1, N'TK001', NULL, 0, 0, NULL, NULL, NULL, NULL, 1470, 1),
('2025-07-07', N'Nhập', 4, N'PN004', N'HD004', 10, 10, N'Xe tải 1.5 tấn', N'Văn Văn F', N'Công ty vận tải GHI', N'Công ty Nước sạch Sài Gòn', 10, 1),
('2025-07-08', N'Xuất', 3, N'PX003', NULL, 5, 5, N'Xe tải 1 tấn', N'Hoàng Văn G', N'Công ty vận chuyển JKL', NULL, 75, 1),
('2025-07-09', N'Nhập', 5, N'PN005', N'HD005', 20, 20, N'Xe tải 2 tấn', N'Đỗ Văn H', N'Công ty vận tải MNO', N'Công ty Phụ gia Hòa Bình', 20, 1),
('2025-07-10', N'Xuất', 4, N'PX004', NULL, 2, 2, N'Xe máy', N'Nguyễn Thị I', N'Tự vận chuyển', NULL, 8, 1),
('2025-07-11', N'Nhập', 1, N'PN006', N'HD006', 150, 7500, N'Xe tải 8 tấn', N'Trần Thị K', N'Công ty vận tải PQR', N'Công ty Xi măng Đồng Nai', 1508, 2),
('2025-07-12', N'Nhập', 2, N'PN007', N'HD007', 60, 90000, N'Xe ben 15 tấn', N'Lê Văn L', N'Công ty vận tải STU', N'Công ty Cát Sông Đà', 2560, 2),
('2025-07-13', N'Xuất', 1, N'PX005', NULL, 40, 2000, N'Xe mixer', N'Phạm Thị M', N'Công ty vận tải PQR', NULL, 1508, 2),
('2025-07-14', N'Xuất', 2, N'PX006', NULL, 30, 45000, N'Xe ben 10 tấn', N'Ngô Văn N', N'Công ty vận tải STU', NULL, 2530, 2),
('2025-07-15', N'Nhập', 3, N'PN008', N'HD008', 100, 100, N'Xe tải 5 tấn', N'Văn Văn O', N'Công ty vận tải VWX', N'Công ty Đá Hòa Phú', 100, 2),
('2025-07-16', N'Tịnh kho', 1, N'TK002', NULL, 0, 0, NULL, NULL, NULL, NULL, 1508, 2),
('2025-07-17', N'Nhập', 4, N'PN009', N'HD009', 15, 15, N'Xe tải 2 tấn', N'Hoàng Văn P', N'Công ty vận tải YZA', N'Công ty Nước sạch Sài Gòn', 15, 2),
('2025-07-18', N'Xuất', 3, N'PX007', NULL, 8, 8, N'Xe tải 1.5 tấn', N'Đỗ Thị Q', N'Công ty vận chuyển BCD', NULL, 92, 2),
('2025-07-19', N'Nhập', 5, N'PN010', N'HD010', 25, 25, N'Xe tải 3 tấn', N'Nguyễn Văn R', N'Công ty vận tải EFG', N'Công ty Phụ gia Hòa Bình', 25, 2),
('2025-07-20', N'Xuất', 4, N'PX008', NULL, 3, 3, N'Xe máy', N'Trần Văn S', N'Tự vận chuyển', NULL, 12, 2)
GO


-- Thêm dữ liệu mẫu cho bảng CUA_VATTU
INSERT INTO CUA_VATTU (STT, TENTRAM, LOAIVATTU, TENCUA, HESOQUYDOI, DONVITINH) 
VALUES 
(1, N'Trạm A', N'Xi măng', N'Cửa số 1', N'1.00', N'Bao'),
(2, N'Trạm B', N'Cát', N'Cửa số 2', N'0.80', N'Khối')
GO

-- Thêm dữ liệu mẫu cho bảng CAIDATCHUNG
INSERT INTO CAIDATCHUNG (THIETLAP, GIATRI) 
VALUES 
(N'SoTram', N'2'),
(N'LoaiCapPhoi', N'1'),
(N'TenCongTy', N'CÔNG TY CỔ PHẦN BÊ TÔNG TÂY ĐÔ')
GO

-- Thêm dữ liệu mẫu cho bảng THIETLAPIN
INSERT INTO THIETLAPIN (TENMAUIN, LOAIIN) 
VALUES 
(N'Mẫu 1', N'Chi tiết'),
(N'Mẫu 2', N'Chi tiết'),
(N'Mẫu 3', N'Chi tiết'),
(N'Mẫu 4', N'Tổng hợp')
GO


-- Thêm dữ liệu mẫu cho bảng PHIEUXUAT
INSERT INTO PHIEUXUAT (SOPHIEU, NGAYXUAT, MADONHANG, MAXE, MAKHACH, MACONGTRINH, MACBETONG, KHOILUONG, THOIGIANTRON, GHICHU, MATRAM, THOIGIANBATDAU, THOIGIANKETTHUC, THIETBIBOM, SUDUNGBOM) 
VALUES 
(N'PX001', '2025-07-01 08:30:00', 1, 1, 2, 9, N'C30R28-10±2', 7.5, '2025-07-01 08:45:00', N'Phiếu xuất đầu tiên', 1, '2025-07-01 08:30:00', '2025-07-01 09:15:00', N'Bơm nước 3HP', 1),
(N'PX002', '2025-07-02 09:15:00', 2, 2, 3, 10, N'C20R28-12±2', 6.0, '2025-07-02 09:30:00', N'Phiếu xuất thứ hai', 1, '2025-07-02 09:15:00', '2025-07-02 10:00:00', N'Bơm công nghiệp 5HP', 1),
(N'PX003', '2025-07-03 10:20:00', 3, 3, 4, 11, N'C25R28-14±2', 8.0, '2025-07-03 10:35:00', N'Phiếu xuất thứ ba', 2, '2025-07-03 10:20:00', '2025-07-03 11:10:00', N'Máy trộn bê tông', 0),
(N'PX004', '2025-07-04 14:10:00', 4, 4, 5, 12, N'C35R28-10±2', 7.0, '2025-07-04 14:25:00', N'Phiếu xuất thứ tư', 2, '2025-07-04 14:10:00', '2025-07-04 14:55:00', N'Máy đầm', 0),
(N'PX005', '2025-07-05 15:30:00', 5, 5, 6, 13, N'C40R28-16±2', 7.5, '2025-07-05 15:45:00', N'Phiếu xuất thứ năm', 3, '2025-07-05 15:30:00', '2025-07-05 16:15:00', N'Máy bơm bê tông', 1),
(N'PX006', '2025-07-06 08:45:00', 6, 6, 7, 14, N'C25R14-12±2', 6.5, '2025-07-06 09:00:00', N'Phiếu xuất thứ sáu', 3, '2025-07-06 08:45:00', '2025-07-06 09:30:00', N'Máy bơm bê tông', 1),
(N'PX007', '2025-07-07 09:30:00', 7, 7, 8, 15, N'C30R14-10±2', 8.0, '2025-07-07 09:45:00', N'Phiếu xuất thứ bảy', 4, '2025-07-07 09:30:00', '2025-07-07 10:15:00', N'Máy trộn bê tông', 0),
(N'PX008', '2025-07-08 10:15:00', 8, 8, 9, 16, N'C20R14-14±2', 7.0, '2025-07-08 10:30:00', N'Phiếu xuất thứ tám', 4, '2025-07-08 10:15:00', '2025-07-08 11:00:00', N'Máy nâng', 0),
(N'PX009', '2025-07-09 13:20:00', 1, 9, 2, 9, N'C30R28-10±2', 7.5, '2025-07-09 13:35:00', N'Phiếu xuất thứ chín', 5, '2025-07-09 13:20:00', '2025-07-09 14:05:00', N'Bơm nước 3HP', 1),
(N'PX010', '2025-07-10 14:45:00', 2, 10, 3, 10, N'C20R28-12±2', 6.0, '2025-07-10 15:00:00', N'Phiếu xuất thứ mười', 5, '2025-07-10 14:45:00', '2025-07-10 15:30:00', N'Bơm công nghiệp 5HP', 1)
GO

-- Thêm dữ liệu mẫu cho bảng CHITIETPHIEUXUAT
-- Chi tiết cho phiếu PX001
INSERT INTO CHITIETPHIEUXUAT (MAPHIEUXUAT, MAVATTU, SOLUONG, GHICHU) 
VALUES 
(1, 1, 450, N'Xi măng PCB 40'),
(1, 2, 1.2, N'Cát vàng'),
(1, 3, 1.8, N'Đá 1-2'),
(1, 4, 180, N'Nước sạch'),
(1, 5, 7.5, N'Phụ gia siêu dẻo')
GO

-- Chi tiết cho phiếu PX002
INSERT INTO CHITIETPHIEUXUAT (MAPHIEUXUAT, MAVATTU, SOLUONG, GHICHU) 
VALUES 
(2, 1, 360, N'Xi măng PCB 40'),
(2, 2, 1.0, N'Cát vàng'),
(2, 3, 1.5, N'Đá 1-2'),
(2, 4, 150, N'Nước sạch'),
(2, 6, 6.0, N'Phụ gia chậm đông kết')
GO

-- Chi tiết cho phiếu PX003
INSERT INTO CHITIETPHIEUXUAT (MAPHIEUXUAT, MAVATTU, SOLUONG, GHICHU) 
VALUES 
(3, 1, 480, N'Xi măng PCB 40'),
(3, 2, 1.3, N'Cát vàng'),
(3, 3, 1.9, N'Đá 1-2'),
(3, 4, 190, N'Nước sạch'),
(3, 5, 8.0, N'Phụ gia siêu dẻo')
GO

-- Chi tiết cho phiếu PX004
INSERT INTO CHITIETPHIEUXUAT (MAPHIEUXUAT, MAVATTU, SOLUONG, GHICHU) 
VALUES 
(4, 1, 420, N'Xi măng PCB 40'),
(4, 2, 1.1, N'Cát vàng'),
(4, 3, 1.7, N'Đá 1-2'),
(4, 4, 170, N'Nước sạch'),
(4, 7, 7.0, N'Phụ gia chống thấm')
GO

-- Chi tiết cho phiếu PX005
INSERT INTO CHITIETPHIEUXUAT (MAPHIEUXUAT, MAVATTU, SOLUONG, GHICHU) 
VALUES 
(5, 1, 450, N'Xi măng PCB 40'),
(5, 2, 1.2, N'Cát vàng'),
(5, 3, 1.8, N'Đá 1-2'),
(5, 4, 180, N'Nước sạch'),
(5, 8, 7.5, N'Phụ gia khoáng hoạt tính')
GO

-- Chi tiết cho phiếu PX006
INSERT INTO CHITIETPHIEUXUAT (MAPHIEUXUAT, MAVATTU, SOLUONG, GHICHU) 
VALUES 
(6, 1, 390, N'Xi măng PCB 40'),
(6, 2, 1.0, N'Cát vàng'),
(6, 3, 1.6, N'Đá 1-2'),
(6, 4, 160, N'Nước sạch'),
(6, 5, 6.5, N'Phụ gia cuốn khí')
GO

-- Chi tiết cho phiếu PX007
INSERT INTO CHITIETPHIEUXUAT (MAPHIEUXUAT, MAVATTU, SOLUONG, GHICHU) 
VALUES 
(7, 1, 480, N'Xi măng PCB 40'),
(7, 2, 1.3, N'Cát vàng'),
(7, 3, 1.9, N'Đá 1-2'),
(7, 4, 190, N'Nước sạch'),
(7, 5, 8.0, N'Phụ gia tăng nhanh đông kết')
GO

-- Chi tiết cho phiếu PX008
INSERT INTO CHITIETPHIEUXUAT (MAPHIEUXUAT, MAVATTU, SOLUONG, GHICHU) 
VALUES 
(8, 1, 420, N'Xi măng PCB 40'),
(8, 2, 1.1, N'Cát vàng'),
(8, 3, 1.7, N'Đá 1-2'),
(8, 4, 170, N'Nước sạch'),
(8, 5, 7.0, N'Phụ gia giảm co ngót')
GO

-- Chi tiết cho phiếu PX009
INSERT INTO CHITIETPHIEUXUAT (MAPHIEUXUAT, MAVATTU, SOLUONG, GHICHU) 
VALUES 
(9, 1, 450, N'Xi măng PCB 40'),
(9, 2, 1.2, N'Cát vàng'),
(9, 3, 1.8, N'Đá 1-2'),
(9, 4, 180, N'Nước sạch'),
(9, 5, 7.5, N'Phụ gia siêu dẻo')
GO

-- Chi tiết cho phiếu PX010
INSERT INTO CHITIETPHIEUXUAT (MAPHIEUXUAT, MAVATTU, SOLUONG, GHICHU) 
VALUES 
(10, 1, 360, N'Xi măng PCB 40'),
(10, 2, 1.0, N'Cát vàng'),
(10, 3, 1.5, N'Đá 1-2'),
(10, 4, 150, N'Nước sạch'),
(10, 6, 6.0, N'Phụ gia chậm đông kết')
GO