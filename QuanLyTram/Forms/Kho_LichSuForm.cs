using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyTram.Forms
{
    public class Kho_LichSuForm : Form
    {
        private DateTimePicker dtpFrom, dtpTo;
        private ComboBox cbSoTram, cbNhapXuat, cbVatLieu;
        private Button btnXem, btnExcel;
        private TextBox txtKhoiLuongTon;
        private DataGridView dgv;

        private DataTable _table;

        public Kho_LichSuForm()
        {
            this.Text = "LỊCH SỬ NHẬP XUẤT";
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.Beige;

            InitializeLayout();
            InitializeData();
        }

        private void InitializeLayout()
        {
            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 140));   // bộ lọc
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));   // khối lượng tồn
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));    // bảng dữ liệu

            // ========== Bộ lọc (GroupBox Thời gian thống kê) ==========
            var gbFilter = new GroupBox
            {
                Text = "Thời gian thống kê",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            var pnlFilter = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 6,
                RowCount = 3,
                Padding = new Padding(5)
            };
            pnlFilter.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));   // label
            pnlFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));// control
            pnlFilter.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            pnlFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            pnlFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            pnlFilter.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            dtpFrom = new DateTimePicker { Dock = DockStyle.Fill, Value = DateTime.Today };
            dtpTo = new DateTimePicker { Dock = DockStyle.Fill, Value = DateTime.Today };

            cbSoTram = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            cbSoTram.Items.AddRange(new object[] { "Tất cả", "Trạm 1", "Trạm 2", "Trạm 3" });
            cbSoTram.SelectedIndex = 0;

            cbNhapXuat = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            cbNhapXuat.Items.AddRange(new object[] { "Tất cả", "Nhập", "Xuất" });
            cbNhapXuat.SelectedIndex = 0;

            cbVatLieu = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            cbVatLieu.Items.AddRange(new object[] { "Tất cả", "Xi măng", "Cát", "Đá", "Phụ gia" });
            cbVatLieu.SelectedIndex = 0;

            btnXem = new Button
            {
                Text = "XEM",
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnExcel = new Button
            {
                Text = "XLSX",
                BackColor = Color.Green,
                ForeColor = Color.White,
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            // Hàng 1: Từ ngày - Số trạm
            pnlFilter.Controls.Add(new Label { Text = "Từ ngày:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 0);
            pnlFilter.Controls.Add(dtpFrom, 1, 0);
            pnlFilter.Controls.Add(new Label { Text = "Số trạm:", Anchor = AnchorStyles.Left, AutoSize = true }, 2, 0);
            pnlFilter.Controls.Add(cbSoTram, 3, 0);

            // Hàng 2: Đến ngày - Tên vật liệu
            pnlFilter.Controls.Add(new Label { Text = "Đến ngày:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 1);
            pnlFilter.Controls.Add(dtpTo, 1, 1);
            pnlFilter.Controls.Add(new Label { Text = "Tên vật liệu:", Anchor = AnchorStyles.Left, AutoSize = true }, 2, 1);
            pnlFilter.Controls.Add(cbVatLieu, 3, 1);

            // Hàng 3: Nhập xuất - Nút
            pnlFilter.Controls.Add(new Label { Text = "Nhập xuất:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 2);
            pnlFilter.Controls.Add(cbNhapXuat, 1, 2);
            pnlFilter.Controls.Add(btnXem, 4, 2);
            pnlFilter.Controls.Add(btnExcel, 5, 2);

            gbFilter.Controls.Add(pnlFilter);


            // ========== Khối lượng tồn ==========
            var gbTon = new GroupBox
            {
                Text = "Khối lượng tồn các thành phần",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            txtKhoiLuongTon = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                BackColor = Color.White,
                ReadOnly = true,
                Font = new Font("Segoe UI", 10),
                Text = "Khối lượng tồn các thành phần..."
            };

            gbTon.Controls.Add(txtKhoiLuongTon);

            // ========== Bảng dữ liệu ==========
            var gbData = new GroupBox
            {
                Text = "Lịch sử nhập xuất thống kê",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false
            };

            gbData.Controls.Add(dgv);

            mainLayout.Controls.Add(gbFilter, 0, 0);
            mainLayout.Controls.Add(gbTon, 0, 1);
            mainLayout.Controls.Add(gbData, 0, 2);

            this.Controls.Add(mainLayout);
        }

        private void InitializeData()
        {
            _table = new DataTable();
            _table.Columns.Add("Ngày", typeof(DateTime));
            _table.Columns.Add("Trạm", typeof(string));
            _table.Columns.Add("Vật liệu", typeof(string));
            _table.Columns.Add("Loại", typeof(string));
            _table.Columns.Add("Số lượng", typeof(double));
            _table.Columns.Add("Ghi chú", typeof(string));

            // fake data
            _table.Rows.Add(DateTime.Today, "Trạm 1", "Xi măng", "Nhập", 50, "Đơn hàng 001");
            _table.Rows.Add(DateTime.Today, "Trạm 2", "Cát", "Xuất", 30, "Công trình A");

            dgv.DataSource = _table;
        }
    }
}
