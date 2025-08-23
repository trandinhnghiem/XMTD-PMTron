using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyTram.Forms
{
    public class Kho_NhapXuatForm : Form
    {
        private Button btnThemMoi, btnCapNhat, btnLuu, btnInPhieu;
        private ComboBox cbTram, cbNhapXuat, cbDonVi, cbPhuongTien, cbDonViVC, cbNhaCungCap;
        private TextBox txtVatLieu, txtSoPhieu, txtSoHoaDon, txtSLNhapXuat, txtQuyDoi, txtLaiXe, txtSoLuongTon;
        private DateTimePicker dtpNgay;
        private DataGridView dgvKho;

        public Kho_NhapXuatForm()
        {
            this.Text = "NHẬP XUẤT KHO";
            this.Size = new Size(1250, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            Font btnFont = new Font("Segoe UI", 10, FontStyle.Bold);

            // ================== Thanh nút chức năng ==================
            btnThemMoi = new Button()
            {
                Text = " THÊM MỚI",
                Width = 150,
                Height = 50,
                Location = new Point(20, 20),
                BackColor = Color.LightGreen,
                Font = btnFont,
                Image = SystemIcons.Application.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText
            };

            btnCapNhat = new Button()
            {
                Text = " CẬP NHẬT",
                Width = 150,
                Height = 50,
                Location = new Point(190, 20),
                BackColor = Color.Khaki,
                Font = btnFont,
                Image = SystemIcons.Information.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText
            };

            btnLuu = new Button()
            {
                Text = " LƯU",
                Width = 150,
                Height = 50,
                Location = new Point(360, 20),
                BackColor = Color.LightSkyBlue,
                Font = btnFont,
                Image = SystemIcons.Shield.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText
            };

            btnInPhieu = new Button()
            {
                Text = " IN PHIẾU",
                Width = 150,
                Height = 50,
                Location = new Point(530, 20),
                BackColor = Color.LightGray,
                Enabled = false,
                Font = btnFont,
                Image = SystemIcons.Application.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText
            };

            this.Controls.AddRange(new Control[] { btnThemMoi, btnCapNhat, btnLuu, btnInPhieu });

            // ================== GroupBox Thông tin nhập xuất ==================
            GroupBox groupInfo = new GroupBox()
            {
                Text = "Thông tin nhập xuất",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(1200, 200),
                Location = new Point(20, 90)
            };

            int  txtW = 220;

            // Trạm
            Label lblTram = new Label() { Text = "Trạm:", Location = new Point(20, 35), AutoSize = true };
            cbTram = new ComboBox() { Location = new Point(150, 30), Width = txtW };
            cbTram.Items.Add("1 - CÔNG TY CỔ PHẦN BÊ TÔNG TÂY ĐÔ - T90 Băng Tải - I");

            // Nhập xuất
            Label lblNhapXuat = new Label() { Text = "Nhập/Xuất:", Location = new Point(20, 70), AutoSize = true };
            cbNhapXuat = new ComboBox() { Location = new Point(150, 65), Width = txtW };
            cbNhapXuat.Items.AddRange(new string[] { "Nhập vật liệu", "Xuất vật liệu" });

            // Tên VL
            Label lblVatLieu = new Label() { Text = "Tên vật liệu:", Location = new Point(20, 105), AutoSize = true };
            txtVatLieu = new TextBox() { Location = new Point(150, 100), Width = txtW };

            // Đơn vị
            Label lblDonVi = new Label() { Text = "Đơn vị:", Location = new Point(20, 140), AutoSize = true };
            cbDonVi = new ComboBox() { Location = new Point(150, 135), Width = 100 };
            cbDonVi.Items.AddRange(new string[] { "Kg", "Tấn", "m3" });
            Label lblQuyDoi = new Label() { Text = "Quy đổi:", Location = new Point(260, 140), AutoSize = true };
            txtQuyDoi = new TextBox() { Location = new Point(320, 135), Width = 50 };

            // Số phiếu
            Label lblSoPhieu = new Label() { Text = "Số phiếu:", Location = new Point(400, 35), AutoSize = true };
            txtSoPhieu = new TextBox() { Location = new Point(530, 30), Width = txtW };

            // Số hóa đơn
            Label lblSoHoaDon = new Label() { Text = "Số hóa đơn:", Location = new Point(400, 70), AutoSize = true };
            txtSoHoaDon = new TextBox() { Location = new Point(530, 65), Width = txtW };

            // Ngày
            Label lblNgay = new Label() { Text = "Ngày:", Location = new Point(400, 105), AutoSize = true };
            dtpNgay = new DateTimePicker()
            {
                Location = new Point(530, 100),
                Width = txtW,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy"
            };

            // SL Nhập/Xuất
            Label lblSLNX = new Label() { Text = "SL nhập xuất:", Location = new Point(400, 140), AutoSize = true };
            txtSLNhapXuat = new TextBox() { Location = new Point(530, 135), Width = txtW };

            // Quy đổi về KG
            Label lblQuyDoiKg = new Label() { Text = "Quy đổi về Kg:", Location = new Point(770, 35), AutoSize = true };
            TextBox txtQuyDoiKg = new TextBox() { Location = new Point(900, 30), Width = txtW };

            // Phương tiện
            Label lblPhuongTien = new Label() { Text = "Tên phương tiện:", Location = new Point(770, 70), AutoSize = true };
            cbPhuongTien = new ComboBox() { Location = new Point(900, 65), Width = txtW };
            cbPhuongTien.Items.Add("AG-257.29");

            // Lái xe
            Label lblLaiXe = new Label() { Text = "Tên lái xe:", Location = new Point(770, 105), AutoSize = true };
            txtLaiXe = new TextBox() { Location = new Point(900, 100), Width = txtW };

            // Đơn vị VC
            Label lblDonViVC = new Label() { Text = "Đơn vị vận chuyển:", Location = new Point(770, 140), AutoSize = true };
            cbDonViVC = new ComboBox() { Location = new Point(900, 135), Width = txtW };
            cbDonViVC.Items.Add("CTY AB");

            // Nhà cung cấp
            Label lblNCC = new Label() { Text = "Nhà cung cấp:", Location = new Point(770, 175), AutoSize = true };
            cbNhaCungCap = new ComboBox() { Location = new Point(900, 170), Width = txtW };
            cbNhaCungCap.Items.Add("CTY DC");

            // Số lượng tồn
            Label lblTon = new Label() { Text = "Số lượng tồn:", Location = new Point(400, 175), AutoSize = true };
            txtSoLuongTon = new TextBox() { Location = new Point(530, 170), Width = txtW };

            groupInfo.Controls.AddRange(new Control[] {
                lblTram, cbTram,
                lblNhapXuat, cbNhapXuat,
                lblVatLieu, txtVatLieu,
                lblDonVi, cbDonVi, lblQuyDoi, txtQuyDoi,
                lblSoPhieu, txtSoPhieu,
                lblSoHoaDon, txtSoHoaDon,
                lblNgay, dtpNgay,
                lblSLNX, txtSLNhapXuat,
                lblQuyDoiKg, txtQuyDoiKg,
                lblPhuongTien, cbPhuongTien,
                lblLaiXe, txtLaiXe,
                lblDonViVC, cbDonViVC,
                lblNCC, cbNhaCungCap,
                lblTon, txtSoLuongTon
            });

            this.Controls.Add(groupInfo);

            // ================== DataGridView Danh sách ==================
            dgvKho = new DataGridView()
            {
                Location = new Point(20, 310),
                Size = new Size(1200, 350),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            this.Controls.Add(dgvKho);

            // Cột
            dgvKho.Columns.Add("STT", "STT");
            dgvKho.Columns.Add("SoPhieu", "Số phiếu");
            dgvKho.Columns.Add("SoHoaDon", "Số hóa đơn");
            dgvKho.Columns.Add("NhapXuat", "Nhập/Xuất");
            dgvKho.Columns.Add("ThoiGian", "Thời gian");
            dgvKho.Columns.Add("TenVatLieu", "Tên vật liệu");
            dgvKho.Columns.Add("BienXe", "Biển xe");
            dgvKho.Columns.Add("LaiXe", "Lái xe");
            dgvKho.Columns.Add("DonViVC", "Đơn vị VC");
            dgvKho.Columns.Add("NhaCungCap", "Nhà cung cấp");
            dgvKho.Columns.Add("NhanVien", "Tên nhân viên");

            // Data mẫu
            dgvKho.Rows.Add("1", "PX01", "HD001", "Nhập", "18/08/2025", "Xi măng PCB40", "XM-257.29", "Nguyễn Văn An", "CTY Xi Măng Hà Tiên", "CTY Xây Dựng Hòa Bình", "Trần Quốc Khánh");
            dgvKho.Rows.Add("2", "PX02", "HD002", "Xuất", "19/08/2025", "Cát vàng rửa", "CT-128.44", "Trần Minh Tuấn", "CTY VLXD Minh Tâm", "CTY Xây Dựng Hoàng Long", "Nguyễn Hữu Phúc");
            dgvKho.Rows.Add("3", "PX03", "HD003", "Nhập", "20/08/2025", "Đá 1x2 xanh", "DA-349.15", "Lê Văn Hùng", "CTY VLXD Hòa Bình", "CTY Xây Dựng Sông Hậu", "Phạm Văn Toàn");
            dgvKho.Rows.Add("4", "PX04", "HD004", "Xuất", "20/08/2025", "Phụ gia siêu dẻo", "PG-482.60", "Nguyễn Thanh Bình", "CTY Hóa Chất Miền Nam", "CTY Bê Tông 586", "Võ Thành Công");
            dgvKho.Rows.Add("5", "PX05", "HD005", "Nhập", "21/08/2025", "Thép D16 Việt Nhật", "TP-673.40", "Phạm Văn Lộc", "CTY Thép Việt Nhật", "CTY Xây Dựng Phúc An", "Đỗ Minh Quang");
            dgvKho.Rows.Add("6", "PX06", "HD006", "Xuất", "21/08/2025", "Xi măng Nghi Sơn", "XM-112.90", "Nguyễn Hoàng Nam", "CTY Xi Măng Nghi Sơn", "CTY Xây Dựng Hưng Thịnh", "Trần Văn Phát");
            dgvKho.Rows.Add("7", "PX07", "HD007", "Nhập", "22/08/2025", "Đá 0x4 trắng", "DA-214.75", "Nguyễn Văn Phước", "CTY VLXD Thành Công", "CTY Xây Dựng Hoàng Anh", "Lê Quốc Đạt");
            dgvKho.Rows.Add("8", "PX08", "HD008", "Xuất", "22/08/2025", "Phụ gia chống thấm Sika", "PG-552.10", "Trần Minh Hào", "CTY Hóa Chất An Phát", "CTY Xây Dựng Phương Nam", "Nguyễn Văn Giang");
            dgvKho.Rows.Add("9", "PX09", "HD009", "Nhập", "23/08/2025", "Thép hộp 50x50", "TP-762.33", "Phan Văn Kiệt", "CTY Thép Hòa Phát", "CTY Cơ Khí Đại Nam", "Hoàng Văn Tâm");
            dgvKho.Rows.Add("10", "PX10", "HD010", "Xuất", "23/08/2025", "Cát xây tô", "CT-441.82", "Ngô Văn Long", "CTY VLXD Sông Đà", "CTY Xây Dựng An Phú", "Trần Văn Hiếu");

        }
    }
}
