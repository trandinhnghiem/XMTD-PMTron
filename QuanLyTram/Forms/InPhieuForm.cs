using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyTram.Forms
{
    public class InPhieuForm : Form
    {
        public InPhieuForm()
        {
            Text = "PHIẾU GIAO NHẬN BÊ TÔNG";
            // Không phóng to full màn hình nữa, để AutoSize và mở vừa nội dung
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(255, 255, 230); // nền vàng nhạt

            Font = new Font("Segoe UI", 10, FontStyle.Regular);

            // Panel chính chia làm 2 bên
            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                AutoSize = true
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 280F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Controls.Add(mainLayout);

            // ====== BÊN TRÁI ======
            var leftPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                AutoSize = true
            };
            mainLayout.Controls.Add(leftPanel, 0, 0);

            // Group: Chọn phiếu
            var grpChonPhieu = new GroupBox { Text = "CHỌN PHIẾU", Width = 260, Height = 220 };
            var dtNgay = new DateTimePicker
                {
                    Location = new Point(20, 30),
                    Width = 150,
                    Format = DateTimePickerFormat.Custom,
                    CustomFormat = "dd/MM/yyyy" // Ngày/Tháng/Năm
                };

            var btnTim = new Button { Text = "Tìm", Location = new Point(180, 30) };
            var listPhieu = new ListBox { Location = new Point(20, 70), Width = 220, Height = 120 };
            grpChonPhieu.Controls.AddRange(new Control[] { dtNgay, btnTim, listPhieu });
            leftPanel.Controls.Add(grpChonPhieu);

            // Group: Tùy chọn in
            var grpIn = new GroupBox { Text = "TÙY CHỌN IN", Width = 260, Height = 160 };
            var rad1 = new RadioButton { Text = "Mẫu 1 (In chi tiết)", Location = new Point(20, 30), ForeColor = Color.Red };
            var rad2 = new RadioButton { Text = "Mẫu 2 (In chi tiết)", Location = new Point(20, 55), ForeColor = Color.Blue };
            var rad3 = new RadioButton { Text = "Mẫu 3 (In chi tiết)", Location = new Point(20, 80), ForeColor = Color.Magenta };
            var rad4 = new RadioButton { Text = "Mẫu 4 (In tổng)", Location = new Point(20, 105), ForeColor = Color.Purple };
            var btnSave = new Button { Text = "Lưu", Location = new Point(20, 130), Width = 80 };
            var btnPrint = new Button { Text = "In", Location = new Point(120, 130), Width = 80 };
            grpIn.Controls.AddRange(new Control[] { rad1, rad2, rad3, rad4, btnSave, btnPrint });
            leftPanel.Controls.Add(grpIn);

            // Group: Thời gian trộn
            var grpTime = new GroupBox { Text = "THỜI GIAN TRỘN", Width = 260, Height = 100 };
            var lblStart = new Label { Text = "Thời gian bắt đầu:", Location = new Point(20, 30), AutoSize = true };
            var txtStart = new TextBox { Location = new Point(140, 27), Width = 100 };
            var lblEnd = new Label { Text = "Thời gian kết thúc:", Location = new Point(20, 60), AutoSize = true };
            var txtEnd = new TextBox { Location = new Point(140, 57), Width = 100 };
            grpTime.Controls.AddRange(new Control[] { lblStart, txtStart, lblEnd, txtEnd });
            leftPanel.Controls.Add(grpTime);

            // ====== BÊN PHẢI ======
            var rightPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                AutoSize = true
            };
            rightPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            rightPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            rightPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            mainLayout.Controls.Add(rightPanel, 1, 0);

            // Group: Thông tin phiếu xuất
            var grpThongTin = new GroupBox { Text = "THÔNG TIN PHIẾU XUẤT", Dock = DockStyle.Fill, AutoSize = true };
            var lblKH = new Label { Text = "Khách hàng:", Location = new Point(20, 30), AutoSize = true };
            var cbKH = new ComboBox { Location = new Point(120, 27), Width = 200 };
            var lblDiaDiem = new Label { Text = "Địa điểm XD:", Location = new Point(20, 60), AutoSize = true };
            var cbDiaDiem = new ComboBox { Location = new Point(120, 57), Width = 200 };
            var lblMaPhieu = new Label { Text = "Mã phiếu:", Location = new Point(400, 30), AutoSize = true };
            var txtMaPhieu = new TextBox { Location = new Point(480, 27), Width = 150 };
            var lblSoPhieu = new Label { Text = "Số phiếu:", Location = new Point(400, 60), AutoSize = true };
            var txtSoPhieu = new TextBox { Location = new Point(480, 57), Width = 150 };
            grpThongTin.Controls.AddRange(new Control[] { lblKH, cbKH, lblDiaDiem, cbDiaDiem, lblMaPhieu, txtMaPhieu, lblSoPhieu, txtSoPhieu });
            rightPanel.Controls.Add(grpThongTin, 0, 0);

            // Group: Tổng khối lượng (fix full ô)
            var grpKhoiLuong = new GroupBox { Text = "TỔNG KHỐI LƯỢNG", Dock = DockStyle.Fill };
            var gridKhoiLuong = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill // fill hết groupbox
            };
            gridKhoiLuong.ColumnCount = 2;
            gridKhoiLuong.Columns[0].Name = "VẬT LIỆU";
            gridKhoiLuong.Columns[1].Name = "ĐƠN VỊ";
            gridKhoiLuong.Rows.Add("XI MĂNG", "Kg");
            gridKhoiLuong.Rows.Add("CÁT", "Kg");
            gridKhoiLuong.Rows.Add("ĐÁ", "Kg");
            gridKhoiLuong.Rows.Add("NƯỚC", "Lit");
            gridKhoiLuong.Rows.Add("PHỤ GIA", "Kg");
            grpKhoiLuong.Controls.Add(gridKhoiLuong);
            rightPanel.Controls.Add(grpKhoiLuong, 0, 1);

            // Group: Thông tin chi tiết
            var grpChiTiet = new GroupBox { Text = "THÔNG TIN CHI TIẾT", Dock = DockStyle.Fill };
            var gridChiTiet = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            gridChiTiet.ColumnCount = 5;
            gridChiTiet.Columns[0].Name = "VẬT LIỆU";
            gridChiTiet.Columns[1].Name = "TP1";
            gridChiTiet.Columns[2].Name = "TP2";
            gridChiTiet.Columns[3].Name = "TP3";
            gridChiTiet.Columns[4].Name = "TP4";
            grpChiTiet.Controls.Add(gridChiTiet);
            rightPanel.Controls.Add(grpChiTiet, 0, 2);
        }
    }
}
