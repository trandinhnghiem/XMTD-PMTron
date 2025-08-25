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
        Text = "THỜI GIAN THỐNG KÊ",
        Dock = DockStyle.Fill,
        Font = new Font("Segoe UI", 10, FontStyle.Bold),
        ForeColor = Color.Red
    };

    var pnlFilter = new TableLayoutPanel
    {
        Dock = DockStyle.Fill,
        ColumnCount = 4,  // giảm từ 6 cột xuống 4 cột
        RowCount = 3,
        Padding = new Padding(5)
    };
    pnlFilter.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));    // label
    pnlFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));// control
    pnlFilter.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));    // label
    pnlFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));// control

    dtpFrom = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, Font = new Font("Segoe UI", 10, FontStyle.Regular), CalendarForeColor = Color.Black };
    dtpTo = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, Font = new Font("Segoe UI", 10, FontStyle.Regular), CalendarForeColor = Color.Black };

    cbSoTram = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10, FontStyle.Regular), ForeColor = Color.Black };
    cbSoTram.Items.AddRange(new object[] { "Tất cả", "Trạm 1", "Trạm 2", "Trạm 3" });
    cbSoTram.SelectedIndex = 0;

    cbNhapXuat = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10, FontStyle.Regular), ForeColor = Color.Black };
    cbNhapXuat.Items.AddRange(new object[] { "Tất cả", "Nhập", "Xuất" });
    cbNhapXuat.SelectedIndex = 0;

    cbVatLieu = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10, FontStyle.Regular), ForeColor = Color.Black };
    cbVatLieu.Items.AddRange(new object[] { "Tất cả", "Xi măng", "Cát", "Đá", "Phụ gia" });
    cbVatLieu.SelectedIndex = 0;

    // Hàng 1: Từ ngày - Số trạm
    pnlFilter.Controls.Add(new Label { Text = "Từ ngày:", Anchor = AnchorStyles.Left, AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black }, 0, 0);
    pnlFilter.Controls.Add(dtpFrom, 1, 0);
    pnlFilter.Controls.Add(new Label { Text = "Số trạm:", Anchor = AnchorStyles.Left, AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black }, 2, 0);
    pnlFilter.Controls.Add(cbSoTram, 3, 0);

    // Hàng 2: Đến ngày - Tên vật liệu
    pnlFilter.Controls.Add(new Label { Text = "Đến ngày:", Anchor = AnchorStyles.Left, AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black }, 0, 1);
    pnlFilter.Controls.Add(dtpTo, 1, 1);
    pnlFilter.Controls.Add(new Label { Text = "Tên vật liệu:", Anchor = AnchorStyles.Left, AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black }, 2, 1);
    pnlFilter.Controls.Add(cbVatLieu, 3, 1);

// Hàng 3: Nhập xuất + nút Xem, Excel
pnlFilter.Controls.Add(new Label { Text = "Nhập xuất:", Anchor = AnchorStyles.Left, AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black }, 0, 2);
pnlFilter.Controls.Add(cbNhapXuat, 1, 2);

// tạo panel chứa 2 nút
var pnlButtons = new FlowLayoutPanel
{
    Dock = DockStyle.Fill,
    FlowDirection = FlowDirection.LeftToRight,
    AutoSize = true
};

btnXem = new Button
{
    Text = "XEM",
    Font = new Font("Segoe UI", 10, FontStyle.Bold),
    BackColor = Color.LightGreen,
    AutoSize = true
};

btnExcel = new Button
{
    Text = "Excel",
    Font = new Font("Segoe UI", 10, FontStyle.Bold),
    BackColor = Color.LightBlue,
    AutoSize = true
};

pnlButtons.Controls.Add(btnXem);
pnlButtons.Controls.Add(btnExcel);

// thêm panel nút vào cột 3, hàng 2
pnlFilter.Controls.Add(pnlButtons, 3, 2);


    gbFilter.Controls.Add(pnlFilter);

    // ========== Khối lượng tồn ==========
    var gbTon = new GroupBox
    {
        Text = "KHỐI LƯỢNG TỒN CÁC THÀNH PHẦN",
        Dock = DockStyle.Fill,
        Font = new Font("Segoe UI", 10, FontStyle.Bold),
        ForeColor = Color.Red
    };

    txtKhoiLuongTon = new TextBox
    {
        Dock = DockStyle.Fill,
        Multiline = true,
        BackColor = Color.White,
        ReadOnly = true,
        Font = new Font("Segoe UI", 10, FontStyle.Regular),
        ForeColor = Color.Black,
        Text = "Khối lượng tồn các thành phần..."
    };

    gbTon.Controls.Add(txtKhoiLuongTon);

    // ========== Bảng dữ liệu ==========
    var gbData = new GroupBox
    {
        Text = "LỊCH SỬ NHẬP XUẤT THỐNG KÊ",
        Dock = DockStyle.Fill,
        Font = new Font("Segoe UI", 10, FontStyle.Bold),
        ForeColor = Color.Red
    };

    dgv = new DataGridView
    {
        Dock = DockStyle.Fill,
        BackgroundColor = Color.White,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        AllowUserToAddRows = false,
        Font = new Font("Segoe UI", 10, FontStyle.Regular),
        ForeColor = Color.Black
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
