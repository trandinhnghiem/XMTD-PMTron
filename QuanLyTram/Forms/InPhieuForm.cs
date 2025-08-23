using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyTram.Forms
{
    public class InPhieuForm : Form
    {
        public InPhieuForm()
        {
            // Form
            Text = "PHIẾU GIAO NHẬN BÊ TÔNG";
            Size = new Size(1260, 740);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.LightYellow;
            Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);

            // --- Panel Trái ---
            Panel panelLeft = new Panel()
            {
                Location = new Point(10, 10),
                Size = new Size(250, 660),
                BackColor = Color.Transparent
            };

            // Chọn phiếu
            GroupBox grpChonPhieu = new GroupBox()
            {
                Text = "CHỌN PHIẾU",
                Location = new Point(0, 0),
                Size = new Size(240, 300),
            };
            Label lblNgay = new Label() { Text = "Ngày:", Location = new Point(10, 25), AutoSize = true };
            DateTimePicker dtpNgay = new DateTimePicker() { Location = new Point(60, 20), Width = 150, Format = DateTimePickerFormat.Short };
            Button btnTim = new Button() { Text = "Tìm", Location = new Point(60, 50), Width = 150 };
            ListBox lstPhieu = new ListBox() { Location = new Point(10, 85), Size = new Size(210, 200) };

            // 👉 Thêm dữ liệu mẫu phiếu
            lstPhieu.Items.AddRange(new object[]
            {
                "PX001 - KH A",
                "PX002 - KH B",
                "PX003 - KH C",
                "PX004 - KH D"
            });

            grpChonPhieu.Controls.AddRange(new Control[] { lblNgay, dtpNgay, btnTim, lstPhieu });

            // Tùy chọn in
            GroupBox grpIn = new GroupBox()
            {
                Text = "TÙY CHỌN IN",
                Location = new Point(0, 300),
                Size = new Size(240, 140)
            };

            RadioButton radMau1 = new RadioButton() { Text = "Mẫu 1 (In chi tiết)", Location = new Point(10, 20), AutoSize = true, ForeColor = Color.Blue, Checked = true };
            RadioButton radMau2 = new RadioButton() { Text = "Mẫu 2 (In chi tiết)", Location = new Point(10, 40), AutoSize = true, ForeColor = Color.Red };
            RadioButton radMau3 = new RadioButton() { Text = "Mẫu 3 (In chi tiết)", Location = new Point(10, 60), AutoSize = true, ForeColor = Color.Orange };
            RadioButton radMau4 = new RadioButton() { Text = "Mẫu 4 (In tổng)", Location = new Point(10, 80), AutoSize = true, ForeColor = Color.Purple };

            Button btnSave = new Button()
            {
                Text = "💾 Lưu",
                Location = new Point(150, 20),
                Size = new Size(80, 35),
                ForeColor = Color.SeaGreen,
                Font = new Font("Segoe UI Emoji", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            Button btnPrint = new Button()
            {
                Text = "🖨 In",
                Location = new Point(150, 65),
                Size = new Size(80, 35),
                ForeColor = Color.RoyalBlue,
                Font = new Font("Segoe UI Emoji", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };

            grpIn.Controls.AddRange(new Control[] { radMau1, radMau2, radMau3, radMau4, btnSave, btnPrint });

            // Thời gian trộn
            GroupBox grpThoiGian = new GroupBox()
            {
                Text = "THỜI GIAN TRỘN",
                Location = new Point(0, 450),
                Size = new Size(240, 120)
            };
            Label lblBD = new Label() { Text = "Thời gian bắt đầu", Location = new Point(10, 25), AutoSize = true };
            TextBox txtBD = new TextBox() { Location = new Point(130, 20), Width = 90, Text = "08:30" };
            Label lblKT = new Label() { Text = "Thời gian kết thúc", Location = new Point(10, 60), AutoSize = true };
            TextBox txtKT = new TextBox() { Location = new Point(130, 55), Width = 90, Text = "09:15" };
            grpThoiGian.Controls.AddRange(new Control[] { lblBD, txtBD, lblKT, txtKT });

            panelLeft.Controls.AddRange(new Control[] { grpChonPhieu, grpIn, grpThoiGian });
            Controls.Add(panelLeft);

            // --- Panel Phải ---
            Panel panelRight = new Panel()
            {
                Location = new Point(270, 10),
                Size = new Size(980, 660),
                BackColor = Color.Transparent
            };

            // Thông tin phiếu xuất
            GroupBox grpThongTin = new GroupBox()
            {
                Text = "THÔNG TIN PHIẾU XUẤT",
                Location = new Point(0, 0),
                Size = new Size(960, 140)
            };

            Label lblKH = new Label() { Text = "Khách hàng:", Location = new Point(10, 25), AutoSize = true };
            ComboBox cboKH = new ComboBox() { Location = new Point(100, 20), Width = 220 };
            cboKH.Items.AddRange(new object[] { "Công ty Xây dựng An Phát", "Công ty Hoà Bình", "Công ty Nam Thành" });
            cboKH.SelectedIndex = 0;

            Label lblMaPhieu = new Label() { Text = "Mã phiếu:", Location = new Point(340, 25), AutoSize = true };
            TextBox txtMaPhieu = new TextBox() { Location = new Point(410, 20), Width = 180, Text = "PX001" };

            Label lblDD = new Label() { Text = "Địa điểm XD:", Location = new Point(10, 60), AutoSize = true };
            ComboBox cboDD = new ComboBox() { Location = new Point(100, 55), Width = 220 };
            cboDD.Items.AddRange(new object[] { "Quận 1", "Quận 2", "Quận 7", "Bình Thạnh" });
            cboDD.SelectedIndex = 1;

            Label lblSoPhieu = new Label() { Text = "Số phiếu:", Location = new Point(340, 60), AutoSize = true };
            TextBox txtSoPhieu = new TextBox() { Location = new Point(410, 55), Width = 180, Text = "S001" };

            Label lblHM = new Label() { Text = "Hạng mục:", Location = new Point(10, 95), AutoSize = true };
            ComboBox cboHM = new ComboBox() { Location = new Point(100, 90), Width = 220 };
            cboHM.Items.AddRange(new object[] { "Móng", "Cột", "Dầm", "Sàn" });
            cboHM.SelectedIndex = 2;

            Label lblNgayTron = new Label() { Text = "Ngày trộn:", Location = new Point(340, 95), AutoSize = true };
            DateTimePicker dtpTron = new DateTimePicker() { Location = new Point(410, 90), Width = 180, Format = DateTimePickerFormat.Short, Value = DateTime.Today };

            Label lblTB = new Label() { Text = "Thiết bị bơm:", Location = new Point(620, 25), AutoSize = true };
            ComboBox cboTB = new ComboBox() { Location = new Point(710, 20), Width = 220 };
            cboTB.Items.AddRange(new object[] { "Bơm cần 36m", "Bơm tĩnh", "Bơm tự hành" });
            cboTB.SelectedIndex = 0;

            Label lblMacBT = new Label() { Text = "Mác bê tông:", Location = new Point(620, 60), AutoSize = true };
            TextBox txtMacBT = new TextBox() { Location = new Point(710, 55), Width = 220, Text = "M300" };

            CheckBox chkBom = new CheckBox() { Text = "Sử dụng bơm", Location = new Point(710, 95), AutoSize = true, Checked = true };

            grpThongTin.Controls.AddRange(new Control[] {
                lblKH, cboKH, lblMaPhieu, txtMaPhieu,
                lblDD, cboDD, lblSoPhieu, txtSoPhieu,
                lblHM, cboHM, lblNgayTron, dtpTron,
                lblTB, cboTB, lblMacBT, txtMacBT, chkBom
            });

            // ================== TỔNG KHỐI LƯỢNG ==================
            GroupBox grpKhoiLuong = new GroupBox()
            {
                Text = "TỔNG KHỐI LƯỢNG",
                Location = new Point(0, 150),
                Size = new Size(480, 280)
            };

            // DataGridView
            DataGridView dgvKhoiLuong = new DataGridView()
            {
                ColumnCount = 2,
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToResizeColumns = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.None
            };

            dgvKhoiLuong.Columns[0].Name = "VẬT LIỆU";
            dgvKhoiLuong.Columns[1].Name = "SỐ LƯỢNG";

            dgvKhoiLuong.Rows.Add("XI MĂNG", "500 Kg");
            dgvKhoiLuong.Rows.Add("CÁT", "1200 Kg");
            dgvKhoiLuong.Rows.Add("ĐÁ", "1500 Kg");
            dgvKhoiLuong.Rows.Add("NƯỚC", "250 L");
            dgvKhoiLuong.Rows.Add("PHỤ GIA", "20 Kg");

            dgvKhoiLuong.AllowUserToAddRows = false;

            grpKhoiLuong.Controls.Add(dgvKhoiLuong);
            Controls.Add(grpKhoiLuong);

            // Thông số
            GroupBox grpThongSo = new GroupBox()
            {
                Text = "THÔNG SỐ",
                Location = new Point(490, 150),
                Size = new Size(470, 280)
            };
            TableLayoutPanel tblThongSo = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1
            };

            DataGridView dgv1 = CreateGrid(4, 2);
            dgv1.Rows[0].Cells[0].Value = "Xe";
            dgv1.Rows[0].Cells[1].Value = "Biển số";
            dgv1.Rows[1].Cells[0].Value = "Xe 1";
            dgv1.Rows[1].Cells[1].Value = "51D-12345";

            DataGridView dgv2 = CreateGrid(5, 2);
            dgv2.Rows[0].Cells[0].Value = "STT";
            dgv2.Rows[0].Cells[1].Value = "Khối lượng";
            dgv2.Rows[1].Cells[0].Value = "1";
            dgv2.Rows[1].Cells[1].Value = "7 m³";

            DataGridView dgv3 = CreateGrid(5, 4);
            dgv3.Rows[0].Cells[0].Value = "STT";
            dgv3.Rows[0].Cells[1].Value = "Tên";
            dgv3.Rows[0].Cells[2].Value = "SL";
            dgv3.Rows[0].Cells[3].Value = "Ghi chú";
            dgv3.Rows[1].Cells[0].Value = "1";
            dgv3.Rows[1].Cells[1].Value = "Xe bồn";
            dgv3.Rows[1].Cells[2].Value = "2";
            dgv3.Rows[1].Cells[3].Value = "Chở đủ tải";

            tblThongSo.Controls.Add(dgv1, 0, 0);
            tblThongSo.Controls.Add(dgv2, 0, 1);
            tblThongSo.Controls.Add(dgv3, 0, 2);
            grpThongSo.Controls.Add(tblThongSo);

            // Thông tin chi tiết
            GroupBox grpChiTiet = new GroupBox()
            {
                Text = "THÔNG TIN CHI TIẾT",
                Location = new Point(0, 430),
                Size = new Size(960, 270),
                Padding = new Padding(5)
            };
            Panel pnlChiTiet = new Panel() { Dock = DockStyle.Fill };
            DataGridView dgvChiTiet = new DataGridView()
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.Gainsboro,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToResizeColumns = false,
                AllowUserToResizeRows = false
            };

            dgvChiTiet.ColumnCount = 5;
            dgvChiTiet.Columns[0].Name = "STT";
            dgvChiTiet.Columns[1].Name = "Xe";
            dgvChiTiet.Columns[2].Name = "Khối lượng (m³)";
            dgvChiTiet.Columns[3].Name = "Thời gian xuất";
            dgvChiTiet.Columns[4].Name = "Ghi chú";

            dgvChiTiet.Rows.Add("1", "51D-12345", "7.0", "08:45", "OK");
            dgvChiTiet.Rows.Add("2", "51D-67890", "6.5", "09:10", "Trễ 5p");
            dgvChiTiet.Rows.Add("3", "51C-54321", "7.5", "09:40", "OK");

            pnlChiTiet.Controls.Add(dgvChiTiet);
            grpChiTiet.Controls.Add(pnlChiTiet);

            panelRight.Controls.AddRange(new Control[] { grpThongTin, grpKhoiLuong, grpThongSo, grpChiTiet });

            // Add 2 panel vào form
            Controls.Add(panelRight);
            Controls.Add(panelLeft);
        }

        private DataGridView CreateGrid(int cols, int rows)
        {
            DataGridView dgv = new DataGridView()
            {
                Dock = DockStyle.Fill,
                ColumnCount = cols,
                RowCount = rows,
                BackgroundColor = Color.WhiteSmoke,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToResizeColumns = false,
                AllowUserToResizeRows = false
            };
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgv.EnableHeadersVisualStyles = false;
            return dgv;
        }
    }
}
