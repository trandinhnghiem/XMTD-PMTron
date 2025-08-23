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
            Width = 1200;
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
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 160));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 200));
            Controls.Add(mainLayout);

            // ================== BỘ LỌC ==================
            var groupFilter = new GroupBox
            {
                Text = "Bộ lọc thống kê",
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(240, 248, 255)
            };
            mainLayout.Controls.Add(groupFilter, 0, 0);

            var filterLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 3 };
            for (int i = 0; i < 4; i++) filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            for (int i = 0; i < 3; i++) filterLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            groupFilter.Controls.Add(filterLayout);

            void AddLabel(string text, int col, int row)
            {
                filterLayout.Controls.Add(new Label
                {
                    Text = text,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill
                }, col, row);
            }
            void AddControl(Control c, int col, int row) => filterLayout.Controls.Add(c, col, row);

            AddLabel("Từ ngày:", 0, 0);
            AddControl(new DateTimePicker { Format = DateTimePickerFormat.Short, Dock = DockStyle.Fill }, 1, 0);
            AddLabel("Đến ngày:", 2, 0);
            AddControl(new DateTimePicker { Format = DateTimePickerFormat.Short, Dock = DockStyle.Fill }, 3, 0);

            AddLabel("Hạng mục:", 0, 1);
            AddControl(new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill }, 1, 1);
            AddLabel("Mác bê tông:", 2, 1);
            AddControl(new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill }, 3, 1);

            AddLabel("Khách hàng:", 0, 2);
            AddControl(new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill }, 1, 2);
            AddLabel("Xe trộn:", 2, 2);

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
            AddControl(btnXem, 3, 2);

            // ================== CHI TIẾT ==================
            var detailGroup = new GroupBox
            {
                Text = "Thống kê chi tiết",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            mainLayout.Controls.Add(detailGroup, 0, 1);

            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ColumnHeadersDefaultCellStyle = { BackColor = Color.SteelBlue, ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold) },
                AlternatingRowsDefaultCellStyle = { BackColor = Color.FromArgb(240, 248, 255) }
            };
            detailGroup.Controls.Add(dgv);

            // Thêm cột và dữ liệu mẫu
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
            var totalGroup = new GroupBox { Text = "Thống kê tổng", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
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
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersDefaultCellStyle = { BackColor = Color.SteelBlue, ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold) },
                AlternatingRowsDefaultCellStyle = { BackColor = Color.FromArgb(240, 248, 255) }
            };
            materialGrid.Columns.Add("vatlieu", "Vật liệu");
            materialGrid.Columns.Add("cat", "Cát (KG)");
            materialGrid.Columns.Add("da", "Đá (KG)");
            materialGrid.Columns.Add("ximang", "Xi măng (KG)");
            materialGrid.Columns.Add("nuoc", "Nước (KG)");
            materialGrid.Columns.Add("phugia", "Phụ gia (KG)");

            // Dữ liệu mẫu vật liệu
            materialGrid.Rows.Add("Đơn hàng DH001", "500", "400", "300", "150", "20");
            materialGrid.Rows.Add("Đơn hàng DH002", "300", "250", "200", "100", "15");
            materialGrid.Rows.Add("Đơn hàng DH003", "400", "350", "250", "120", "18");
            materialGrid.Rows.Add("Đơn hàng DH004", "600", "500", "420", "200", "25");
            materialGrid.Rows.Add("Đơn hàng DH005", "250", "200", "150", "80",  "10");
            materialGrid.Rows.Add("Đơn hàng DH006", "700", "600", "500", "250", "30");
            materialGrid.Rows.Add("Đơn hàng DH007", "450", "380", "300", "160", "22");
            materialGrid.Rows.Add("Đơn hàng DH008", "520", "420", "340", "170", "24");
            materialGrid.Rows.Add("Đơn hàng DH009", "350", "290", "230", "110", "16");
            materialGrid.Rows.Add("Đơn hàng DH010", "800", "700", "600", "300", "35");


            totalLayout.Controls.Add(materialGrid, 0, 0);

            // Tổng
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

            // Xuất file
            var exportPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(5), WrapContents = false };
            void AddExportButton(string text) => exportPanel.Controls.Add(new Button { Text = text, Width = 100, Height = 40, BackColor = Color.SteelBlue, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold) });
            AddExportButton("In");
            AddExportButton("Xuất XLSX");
            AddExportButton("Xuất CSV");
            totalLayout.Controls.Add(exportPanel, 2, 0);
        }
    }
}
