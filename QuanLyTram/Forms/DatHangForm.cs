using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyTram.Forms
{
    public class DatHangForm : Form
    {
        // Khai báo control
        private Button btnThemMoi, btnCapNhat, btnXoa, btnLuu;
        private DateTimePicker dtpNgay;
        private TextBox txtMaDon, txtKyHieu, txtSoPhieu, txtDatHang, txtTichLuy;
        private ComboBox cbTramTron, cbKhachHang, cbDiaDiem, cbKinhDoanh;
        private CheckBox chkHoatDong;
        private DataGridView dgvDonHang;

        public DatHangForm()
        {
            this.Text = "QUẢN LÝ ĐƠN ĐẶT HÀNG";
            this.Size = new Size(1250, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Beige;

            // ===== Tạo style chung cho button =====
            Font btnFont = new Font("Segoe UI", 10, FontStyle.Bold);

            // Nút Thêm mới
            btnThemMoi = new Button()
            {
                Text = " THÊM MỚI",
                Width = 150,
                Height = 50,
                Location = new Point(20, 20),
                BackColor = Color.LightGreen,
                Font = btnFont,
                FlatStyle = FlatStyle.Standard,
                Image = SystemIcons.Application.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText
            };

            // Nút Cập nhật
            btnCapNhat = new Button()
            {
                Text = " CẬP NHẬT",
                Width = 150,
                Height = 50,
                Location = new Point(190, 20),
                BackColor = Color.Khaki,
                Font = btnFont,
                FlatStyle = FlatStyle.Standard,
                Image = SystemIcons.Information.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText
            };

            // Nút Xóa
            btnXoa = new Button()
            {
                Text = " XÓA",
                Width = 150,
                Height = 50,
                Location = new Point(360, 20),
                BackColor = Color.LightCoral,
                Font = btnFont,
                FlatStyle = FlatStyle.Standard,
                Image = SystemIcons.Error.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText
            };

            this.Controls.Add(btnThemMoi);
            this.Controls.Add(btnCapNhat);
            this.Controls.Add(btnXoa);

            // GroupBox Thông tin đặt hàng
            GroupBox groupInfo = new GroupBox()
            {
                Text = "THÔNG TIN ĐẶT HÀNG",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(900, 220),
                Location = new Point(20, 90)
            };

            // Ngày hệ thống
            Label lblNgay = new Label() { Text = "Ngày hệ thống:", Location = new Point(20, 40), AutoSize = true };
            dtpNgay = new DateTimePicker()
            {
                Location = new Point(150, 35),
                Width = 200,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy"
            };

            // Mã đơn
            Label lblMaDon = new Label() { Text = "Mã đơn hàng:", Location = new Point(20, 70), AutoSize = true };
            txtMaDon = new TextBox() { Location = new Point(150, 65), Width = 200 };

            // Ký hiệu
            Label lblKyHieu = new Label() { Text = "Ký hiệu đơn:", Location = new Point(20, 100), AutoSize = true };
            txtKyHieu = new TextBox() { Location = new Point(150, 95), Width = 200 };

            // Số phiếu
            Label lblSoPhieu = new Label() { Text = "Số phiếu:", Location = new Point(20, 130), AutoSize = true };
            txtSoPhieu = new TextBox() { Location = new Point(150, 125), Width = 200, Text = "0" };

            // Trạm trộn
            Label lblTramTron = new Label() { Text = "Trạm trộn:", Location = new Point(400, 40), AutoSize = true };
            cbTramTron = new ComboBox() { Location = new Point(500, 35), Width = 350 };
            cbTramTron.Items.Add("1 - CÔNG TY CỔ PHẦN BÊ TÔNG TÂY ĐÔ - T90 Băng Tải - I");

            // Khách hàng
            Label lblKhach = new Label() { Text = "Khách hàng:", Location = new Point(400, 70), AutoSize = true };
            cbKhachHang = new ComboBox() { Location = new Point(500, 65), Width = 350 };
            cbKhachHang.Items.Add("ANH DƯƠNG");

            // Địa điểm
            Label lblDiaDiem = new Label() { Text = "Địa điểm:", Location = new Point(400, 100), AutoSize = true };
            cbDiaDiem = new ComboBox() { Location = new Point(500, 95), Width = 350 };
            cbDiaDiem.Items.Add("Đ. NGUYỄN HUỆ - TPVT");

            // Kinh doanh
            Label lblKD = new Label() { Text = "Kinh doanh:", Location = new Point(400, 130), AutoSize = true };
            cbKinhDoanh = new ComboBox() { Location = new Point(500, 125), Width = 350 };
            cbKinhDoanh.Items.Add("ĐIỀN");

            // Đặt hàng m3
            Label lblDatHang = new Label() { Text = "Đặt hàng (m3):", Location = new Point(20, 160), AutoSize = true };
            txtDatHang = new TextBox() { Location = new Point(150, 155), Width = 200 };

            // Tích lũy
            Label lblTichLuy = new Label() { Text = "Tích lũy (m3):", Location = new Point(400, 160), AutoSize = true };
            txtTichLuy = new TextBox() { Location = new Point(500, 155), Width = 200 };

            // Checkbox
            chkHoatDong = new CheckBox() { Text = "Hoạt động", Location = new Point(750, 155), AutoSize = true };

            // Nút Lưu
            btnLuu = new Button()
            {
                Text = " LƯU",
                Width = 120,
                Height = 40,
                Location = new Point(760, 190),
                BackColor = Color.LightSkyBlue,
                Font = btnFont,
                FlatStyle = FlatStyle.Standard,
                Image = SystemIcons.Shield.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText
            };

            groupInfo.Controls.AddRange(new Control[] {
                lblNgay, dtpNgay,
                lblMaDon, txtMaDon,
                lblKyHieu, txtKyHieu,
                lblSoPhieu, txtSoPhieu,
                lblTramTron, cbTramTron,
                lblKhach, cbKhachHang,
                lblDiaDiem, cbDiaDiem,
                lblKD, cbKinhDoanh,
                lblDatHang, txtDatHang,
                lblTichLuy, txtTichLuy,
                chkHoatDong, btnLuu
            });

            this.Controls.Add(groupInfo);

            // DataGridView Dữ liệu đơn hàng
            dgvDonHang = new DataGridView()
            {
                Location = new Point(20, 330),
                Size = new Size(1180, 300),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(dgvDonHang);

            // Thêm dữ liệu mẫu
            DataTable dt = new DataTable();
            dt.Columns.Add("Mã");
            dt.Columns.Add("Ký hiệu đơn");
            dt.Columns.Add("Khách hàng");
            dt.Columns.Add("M3 đặt hàng");
            dt.Columns.Add("Ngày tháng", typeof(DateTime));
            dt.Columns.Add("Số phiếu");
            dt.Columns.Add("Tích lũy");
            dt.Columns.Add("Địa điểm CT");

            dt.Rows.Add("1", "A01", "CTY TNHH TV TK XD", "24", new DateTime(2025, 7, 27), "0", "0", "KHO BẠC NHÀ NƯỚC");
            dt.Rows.Add("2", "A02", "CTY TRƯỜNG SƠN 145", "20", new DateTime(2025, 7, 5), "0", "0", "QL61C, H.CHÂU THÀNH");

            dgvDonHang.DataSource = dt;

            // Format lại cột ngày tháng thành dd/MM/yyyy
            dgvDonHang.Columns["Ngày tháng"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }


    }
}
