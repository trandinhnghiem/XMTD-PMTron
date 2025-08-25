using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyTron.Forms
{
    public class ThongKeForm : Form
    {
        public ThongKeForm()
        {
            Text = "THỐNG KÊ ĐƠN HÀNG";
            Width = 1220;
            Height = 750;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10, FontStyle.Regular);
            BackColor = Color.White;

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(10)
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 230));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 80));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 200));
            Controls.Add(mainLayout);

            // ================== BỘ LỌC ==================
            var groupFilter = new GroupBox
            {
                Text = "BỘ LỌC THỐNG KÊ",
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.Red,
                BackColor = Color.FromArgb(240, 248, 255)
            };
            mainLayout.Controls.Add(groupFilter, 0, 0);

            // Layout chính trong groupFilter
            var filterMainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 2
            };
            filterMainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
            filterMainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 30));

            // ================== CHỈNH TỈ LỆ CHIỀU NGANG CÁC CỘT ==================
            filterMainLayout.ColumnStyles.Clear();
            filterMainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20)); // Khoảng thời gian rộng hơn
            filterMainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12)); // Dạng thống kê hẹp lại
            filterMainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68)); // Thống kê theo

            groupFilter.Controls.Add(filterMainLayout);

            // ================== GroupBox con ==================

// ------------------ 1. KHOẢNG THỜI GIAN ------------------
var gbTime = new GroupBox
{
    Text = "Khoảng thời gian",
    Dock = DockStyle.Fill,
    Font = new Font("Segoe UI", 10, FontStyle.Bold),
    ForeColor = Color.DarkBlue
};

var timeLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2 };
timeLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
timeLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
timeLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
timeLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
gbTime.Controls.Add(timeLayout);

timeLayout.Controls.Add(new Label 
{ 
    Text = "Từ ngày:", AutoSize = true, Anchor = AnchorStyles.Left, 
    Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black 
}, 0, 0);
timeLayout.Controls.Add(new DateTimePicker 
{ 
    Format = DateTimePickerFormat.Short, Anchor = AnchorStyles.Left, Width = 120, 
    Font = new Font("Segoe UI", 10, FontStyle.Regular) 
}, 1, 0);

timeLayout.Controls.Add(new Label 
{ 
    Text = "Đến ngày:", AutoSize = true, Anchor = AnchorStyles.Left,
    Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black
}, 0, 1);
timeLayout.Controls.Add(new DateTimePicker 
{ 
    Format = DateTimePickerFormat.Short, Anchor = AnchorStyles.Left, Width = 120, 
    Font = new Font("Segoe UI", 10, FontStyle.Regular)
}, 1, 1);

filterMainLayout.Controls.Add(gbTime, 0, 0);

// ------------------ 2. DẠNG THỐNG KÊ ------------------
var gbType = new GroupBox
{
    Text = "Dạng thống kê",
    Dock = DockStyle.Fill,
    Font = new Font("Segoe UI", 10, FontStyle.Bold),
    ForeColor = Color.DarkBlue
};

var typeLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
for (int i = 0; i < 3; i++)
    typeLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3f));
gbType.Controls.Add(typeLayout);

typeLayout.Controls.Add(new RadioButton { Text = "Số chuyến", Checked = true, AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 0);
typeLayout.Controls.Add(new RadioButton { Text = "Chi tiết", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 1);
typeLayout.Controls.Add(new RadioButton { Text = "Tổng", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 2);

filterMainLayout.Controls.Add(gbType, 1, 0);

// ------------------ 3. THỐNG KÊ THEO ------------------
var gbFilter = new GroupBox
{
    Text = "Thống kê theo",
    Dock = DockStyle.Fill,
    Font = new Font("Segoe UI", 10, FontStyle.Bold),
    ForeColor = Color.DarkBlue
};

var filterLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 3 };
filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // label trái
filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); // combobox trái
filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // label phải
filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); // combobox phải
for (int i = 0; i < 3; i++)
    filterLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3f));
gbFilter.Controls.Add(filterLayout);

// --- Hàng 1 ---
filterLayout.Controls.Add(new Label { Text = "Hạng mục:", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black }, 0, 0);
filterLayout.Controls.Add(new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10, FontStyle.Regular) }, 1, 0);
filterLayout.Controls.Add(new Label { Text = "Khách hàng:", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black }, 2, 0);
filterLayout.Controls.Add(new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10, FontStyle.Regular) }, 3, 0);

// --- Hàng 2 ---
filterLayout.Controls.Add(new Label { Text = "Kinh doanh:", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black }, 0, 1);
filterLayout.Controls.Add(new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10, FontStyle.Regular) }, 1, 1);
filterLayout.Controls.Add(new Label { Text = "Mác bê tông:", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black }, 2, 1);
filterLayout.Controls.Add(new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10, FontStyle.Regular) }, 3, 1);

// --- Hàng 3 ---
filterLayout.Controls.Add(new Label { Text = "Xe trộn:", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black }, 0, 2);
filterLayout.Controls.Add(new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10, FontStyle.Regular) }, 1, 2);
filterLayout.Controls.Add(new Label { Text = "Ký hiệu đơn:", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black }, 2, 2);
filterLayout.Controls.Add(new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10, FontStyle.Regular) }, 3, 2);

filterMainLayout.Controls.Add(gbFilter, 2, 0);

// ------------------ HÀNG 2: chọn trạm + nút xem ------------------
var stationLayout = new TableLayoutPanel
{
    Dock = DockStyle.Fill,
    ColumnCount = 3
};
stationLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
stationLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
stationLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));

filterMainLayout.Controls.Add(stationLayout, 0, 1);
filterMainLayout.SetColumnSpan(stationLayout, 3);

var cbTram = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10, FontStyle.Regular) };
cbTram.Items.Add("1 - CÔNG TY CỔ PHẦN BÊ TÔNG TÂY ĐÔ - T 90 Băng Tải - Hậu Giang - 90m3");
stationLayout.Controls.Add(cbTram, 0, 0);

stationLayout.Controls.Add(new CheckBox { Text = "Tất cả các trạm", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black }, 1, 0);

            var btnXem = new Button
            {
                Text = "XEM",
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat
            };
            btnXem.FlatAppearance.BorderSize = 0;
            stationLayout.Controls.Add(btnXem, 2, 0);

            // ================== CHI TIẾT ==================
            var detailGroup = new GroupBox
            {
                Text = "THỐNG KÊ CHI TIẾT",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.Red
            };
            mainLayout.Controls.Add(detailGroup, 0, 1);

            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);
            detailGroup.Controls.Add(dgv);

            dgv.Columns.Add("madon", "Mã đơn");
            dgv.Columns.Add("khachhang", "Khách hàng");
            dgv.Columns.Add("ngaylap", "Ngày lập");
            dgv.Columns.Add("hangmuc", "Hạng mục");
            dgv.Columns.Add("macbetong", "Mác bê tông");
            dgv.Columns.Add("xetroi", "Xe trộn");
            dgv.Columns.Add("soluong", "Số lượng (m3)");

            dgv.Rows.Add("DH001", "Công ty ABC", "18/08/2025", "Đổ sàn", "M250", "Xe 01", "12");
            dgv.Rows.Add("DH002", "Công ty XYZ", "17/08/2025", "Đổ cột", "M300", "Xe 03", "8");
            dgv.Rows.Add("DH003", "Công ty 123", "16/08/2025", "Đổ dầm", "M200", "Xe 02", "10");

            // ================== TỔNG ==================
            var totalGroup = new GroupBox
            {
                Text = "THỐNG KÊ TỔNG",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.Red
            };
            mainLayout.Controls.Add(totalGroup, 0, 2);

            var totalLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, Padding = new Padding(5) };
            totalLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            totalLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            totalLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            totalGroup.Controls.Add(totalLayout);

            var materialGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            materialGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            materialGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            materialGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            materialGrid.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            materialGrid.DefaultCellStyle.ForeColor = Color.Black;
            materialGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);

            materialGrid.Columns.Add("vatlieu", "Vật liệu");
            materialGrid.Columns.Add("cat", "Cát (KG)");
            materialGrid.Columns.Add("da", "Đá (KG)");
            materialGrid.Columns.Add("ximang", "Xi măng (KG)");
            materialGrid.Columns.Add("nuoc", "Nước (KG)");
            materialGrid.Columns.Add("phugia", "Phụ gia (KG)");

            materialGrid.Rows.Add("Đơn hàng DH001", "500", "400", "300", "150", "20");
            materialGrid.Rows.Add("Đơn hàng DH002", "300", "250", "200", "100", "15");
            materialGrid.Rows.Add("Đơn hàng DH003", "400", "350", "250", "120", "18");

            totalLayout.Controls.Add(materialGrid, 0, 0);

            var totalBox = new GroupBox { Text = "TỔNG", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 11, FontStyle.Bold) };
            var lblTong = new Label
            {
                Text = "30 m3\n2000 kg",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };
            totalBox.Controls.Add(lblTong);
            totalLayout.Controls.Add(totalBox, 1, 0);

            var exportPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(5), WrapContents = false };
            void AddExportButton(string text)
            {
                exportPanel.Controls.Add(new Button
                {
                    Text = text,
                    Width = 100,
                    Height = 40,
                    BackColor = Color.SteelBlue,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                });
            }
            AddExportButton("In");
            AddExportButton("Xuất XLSX");
            AddExportButton("Xuất CSV");
            totalLayout.Controls.Add(exportPanel, 2, 0);
        }
    }
}
