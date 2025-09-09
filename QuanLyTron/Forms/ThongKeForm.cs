using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using QuanLyTron.DAL;
using System.Drawing.Printing;

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
            
            DateTimePicker dtpFrom = new DateTimePicker { Format = DateTimePickerFormat.Short, Anchor = AnchorStyles.Left, Width = 120, Font = new Font("Segoe UI", 10, FontStyle.Regular) };
            DateTimePicker dtpTo = new DateTimePicker { Format = DateTimePickerFormat.Short, Anchor = AnchorStyles.Left, Width = 120, Font = new Font("Segoe UI", 10, FontStyle.Regular) };
            
            timeLayout.Controls.Add(new Label { Text = "Từ ngày:", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black }, 0, 0);
            timeLayout.Controls.Add(dtpFrom, 1, 0);
            timeLayout.Controls.Add(new Label { Text = "Đến ngày:", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black }, 0, 1);
            timeLayout.Controls.Add(dtpTo, 1, 1);
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
            
            RadioButton radSoChuyen = new RadioButton { Text = "Số chuyến", Checked = true, AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            RadioButton radChiTiet = new RadioButton { Text = "Chi tiết", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            RadioButton radTong = new RadioButton { Text = "Tổng", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            
            typeLayout.Controls.Add(radSoChuyen, 0, 0);
            typeLayout.Controls.Add(radChiTiet, 0, 1);
            typeLayout.Controls.Add(radTong, 0, 2);
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
            
            // Tạo ComboBox
            ComboBox cbHangMuc = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10, FontStyle.Regular) };
            ComboBox cbKhachHang = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10, FontStyle.Regular) };
            ComboBox cbKinhDoanh = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10, FontStyle.Regular) };
            ComboBox cbMacBeTong = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10, FontStyle.Regular) };
            ComboBox cbXeTron = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10, FontStyle.Regular) };
            ComboBox cbKyHieuDon = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10, FontStyle.Regular) };
            
            // Load dữ liệu cho ComboBox
            LoadComboBoxData(cbHangMuc, cbKhachHang, cbKinhDoanh, cbMacBeTong, cbXeTron, cbKyHieuDon);
            
            // --- Hàng 1 ---
            filterLayout.Controls.Add(new Label { Text = "Hạng mục:", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black }, 0, 0);
            filterLayout.Controls.Add(cbHangMuc, 1, 0);
            filterLayout.Controls.Add(new Label { Text = "Khách hàng:", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black }, 2, 0);
            filterLayout.Controls.Add(cbKhachHang, 3, 0);
            // --- Hàng 2 ---
            filterLayout.Controls.Add(new Label { Text = "Kinh doanh:", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black }, 0, 1);
            filterLayout.Controls.Add(cbKinhDoanh, 1, 1);
            filterLayout.Controls.Add(new Label { Text = "Mác bê tông:", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black }, 2, 1);
            filterLayout.Controls.Add(cbMacBeTong, 3, 1);
            // --- Hàng 3 ---
            filterLayout.Controls.Add(new Label { Text = "Xe trộn:", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black }, 0, 2);
            filterLayout.Controls.Add(cbXeTron, 1, 2);
            filterLayout.Controls.Add(new Label { Text = "Ký hiệu đơn:", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black }, 2, 2);
            filterLayout.Controls.Add(cbKyHieuDon, 3, 2);
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
            
            ComboBox cbTram = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10, FontStyle.Regular) };
            CheckBox chkTatCa = new CheckBox { Text = "Tất cả các trạm", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Black };
            
            // Load dữ liệu trạm
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT MATRAM, TENTRAM FROM TRAM", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cbTram.Items.Add(new { Value = reader["MATRAM"], Display = reader["TENTRAM"].ToString() });
                        }
                    }
                }
            }
            cbTram.DisplayMember = "Display";
            cbTram.ValueMember = "Value";
            if (cbTram.Items.Count > 0) cbTram.SelectedIndex = 0;
            
            Button btnXem = new Button
            {
                Text = "XEM",
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat
            };
            btnXem.FlatAppearance.BorderSize = 0;
            
            stationLayout.Controls.Add(cbTram, 0, 0);
            stationLayout.Controls.Add(chkTatCa, 1, 0);
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
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 8, FontStyle.Regular);
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);
            detailGroup.Controls.Add(dgv);
            
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
            materialGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            materialGrid.DefaultCellStyle.Font = new Font("Segoe UI", 8, FontStyle.Regular);
            materialGrid.DefaultCellStyle.ForeColor = Color.Black;
            materialGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);
            materialGrid.Columns.Add("vatlieu", "Vật liệu");
            materialGrid.Columns.Add("cat", "Cát (KG)");
            materialGrid.Columns.Add("da", "Đá (KG)");
            materialGrid.Columns.Add("ximang", "Xi măng (KG)");
            materialGrid.Columns.Add("nuoc", "Nước (KG)");
            materialGrid.Columns.Add("phugia", "Phụ gia (KG)");
            totalLayout.Controls.Add(materialGrid, 0, 0);
            
            var totalBox = new GroupBox { Text = "TỔNG", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 11, FontStyle.Bold) };
            var lblTong = new Label
            {
                Text = "0 m3\n0 kg",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };
            totalBox.Controls.Add(lblTong);
            totalLayout.Controls.Add(totalBox, 1, 0);
            
            var exportPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(5), WrapContents = false };
            Button btnIn = new Button
            {
                Text = "In",
                Width = 100,
                Height = 40,
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            
            Button btnXuatExcel = new Button
            {
                Text = "Xuất XLSX",
                Width = 100,
                Height = 40,
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            
            Button btnXuatCSV = new Button
            {
                Text = "Xuất CSV",
                Width = 100,
                Height = 40,
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            
            exportPanel.Controls.Add(btnIn);
            exportPanel.Controls.Add(btnXuatExcel);
            exportPanel.Controls.Add(btnXuatCSV);
            totalLayout.Controls.Add(exportPanel, 2, 0);
            
            // Đăng ký sự kiện
            btnXem.Click += (s, e) => {
                LoadStatisticsData(dgv, materialGrid, lblTong, dtpFrom.Value, dtpTo.Value, cbTram, chkTatCa.Checked, 
                                    cbHangMuc, cbKhachHang, cbKinhDoanh, cbMacBeTong, cbXeTron, cbKyHieuDon,
                                    radSoChuyen.Checked, radChiTiet.Checked, radTong.Checked);
            };
            
            // Xử lý sự kiện in và xuất
            btnIn.Click += (s, e) => {
                PrintData(dgv, materialGrid, lblTong.Text, detailGroup.Text);
            };
            
            btnXuatExcel.Click += (s, e) => {
                ExportToExcel(dgv, materialGrid, lblTong.Text, detailGroup.Text);
            };
            
            btnXuatCSV.Click += (s, e) => {
                ExportToCSV(dgv, materialGrid, lblTong.Text, detailGroup.Text);
            };
            
            // Xử lý sự kiện thay đổi RadioButton
            radSoChuyen.CheckedChanged += (s, e) => {
                if (radSoChuyen.Checked)
                {
                    detailGroup.Text = "THỐNG KÊ SỐ CHUYẾN";
                }
            };
            
            radChiTiet.CheckedChanged += (s, e) => {
                if (radChiTiet.Checked)
                {
                    detailGroup.Text = "THỐNG KÊ CHI TIẾT";
                }
            };
            
            radTong.CheckedChanged += (s, e) => {
                if (radTong.Checked)
                {
                    detailGroup.Text = "THỐNG KÊ TỔNG HỢP";
                }
            };
        }
        
        private void LoadComboBoxData(ComboBox cbHangMuc, ComboBox cbKhachHang, ComboBox cbKinhDoanh, 
                                       ComboBox cbMacBeTong, ComboBox cbXeTron, ComboBox cbKyHieuDon)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    
                    // Load hạng mục
                    cbHangMuc.Items.Add("Tất cả");
                    using (var cmd = new SqlCommand("SELECT DISTINCT HANGMUC FROM CONGTRINH", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cbHangMuc.Items.Add(reader["HANGMUC"].ToString());
                            }
                        }
                    }
                    if (cbHangMuc.Items.Count > 0) cbHangMuc.SelectedIndex = 0;
                    
                    // Load khách hàng
                    cbKhachHang.Items.Add("Tất cả");
                    using (var cmd = new SqlCommand("SELECT MAKHACH, TENKHACH FROM KHACHHANG", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cbKhachHang.Items.Add(new { Value = reader["MAKHACH"], Display = reader["TENKHACH"].ToString() });
                            }
                        }
                    }
                    cbKhachHang.DisplayMember = "Display";
                    cbKhachHang.ValueMember = "Value";
                    if (cbKhachHang.Items.Count > 0) cbKhachHang.SelectedIndex = 0;
                    
                    // Load kinh doanh
                    cbKinhDoanh.Items.Add("Tất cả");
                    using (var cmd = new SqlCommand("SELECT MAKINHDOANH, TENKINHDOANH FROM KINHDOANH", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cbKinhDoanh.Items.Add(new { Value = reader["MAKINHDOANH"], Display = reader["TENKINHDOANH"].ToString() });
                            }
                        }
                    }
                    cbKinhDoanh.DisplayMember = "Display";
                    cbKinhDoanh.ValueMember = "Value";
                    if (cbKinhDoanh.Items.Count > 0) cbKinhDoanh.SelectedIndex = 0;
                    
                    // Load mác bê tông
                    cbMacBeTong.Items.Add("Tất cả");
                    using (var cmd = new SqlCommand("SELECT DISTINCT MACBETONG FROM CAPPHOI", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cbMacBeTong.Items.Add(reader["MACBETONG"].ToString());
                            }
                        }
                    }
                    if (cbMacBeTong.Items.Count > 0) cbMacBeTong.SelectedIndex = 0;
                    
                    // Load xe trộn
                    cbXeTron.Items.Add("Tất cả");
                    using (var cmd = new SqlCommand("SELECT MAXE, BIENSO FROM XE", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cbXeTron.Items.Add(new { Value = reader["MAXE"], Display = reader["BIENSO"].ToString() });
                            }
                        }
                    }
                    cbXeTron.DisplayMember = "Display";
                    cbXeTron.ValueMember = "Value";
                    if (cbXeTron.Items.Count > 0) cbXeTron.SelectedIndex = 0;
                    
                    // Load ký hiệu đơn
                    cbKyHieuDon.Items.Add("Tất cả");
                    using (var cmd = new SqlCommand("SELECT DISTINCT KYHIEUDON FROM DONHANG", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cbKyHieuDon.Items.Add(reader["KYHIEUDON"].ToString());
                            }
                        }
                    }
                    if (cbKyHieuDon.Items.Count > 0) cbKyHieuDon.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu ComboBox: " + ex.Message);
            }
        }
        
        private void LoadStatisticsData(DataGridView dgv, DataGridView materialGrid, Label lblTong, DateTime fromDate, DateTime toDate, 
                                        ComboBox cbTram, bool allStations, ComboBox cbHangMuc, ComboBox cbKhachHang, 
                                        ComboBox cbKinhDoanh, ComboBox cbMacBeTong, ComboBox cbXeTron, ComboBox cbKyHieuDon,
                                        bool soChuyen, bool chiTiet, bool tong)
        {
            try
            {
                // Xóa dữ liệu cũ
                dgv.DataSource = null;
                dgv.Columns.Clear();
                materialGrid.Rows.Clear();
                
                // Xây dựng câu truy vấn động dựa trên bộ lọc
                string query = @"
                SELECT px.MAPHIEUXUAT, dh.MADONHANG, kh.TENKHACH, px.NGAYXUAT, ct.HANGMUC, px.MACBETONG, 
                       x.BIENSO, px.KHOILUONG, px.SOPHIEU, kd.TENKINHDOANH
                FROM PHIEUXUAT px
                INNER JOIN DONHANG dh ON px.MADONHANG = dh.MADONHANG
                LEFT JOIN KHACHHANG kh ON dh.MAKHACH = kh.MAKHACH
                LEFT JOIN CONGTRINH ct ON dh.MACONGTRINH = ct.MACONGTRINH
                LEFT JOIN KINHDOANH kd ON dh.MAKINHDOANH = kd.MAKINHDOANH
                LEFT JOIN XE x ON px.MAXE = x.MAXE
                WHERE 1=1";
                
                // Thêm điều kiện lọc
                if (!allStations && cbTram.SelectedIndex >= 0)
                {
                    query += " AND px.MATRAM = @maTram";
                }
                
                if (fromDate != DateTime.MinValue)
                {
                    query += " AND CAST(px.NGAYXUAT AS DATE) >= @fromDate";
                }
                
                if (toDate != DateTime.MinValue)
                {
                    query += " AND CAST(px.NGAYXUAT AS DATE) <= @toDate";
                }
                
                if (cbHangMuc.SelectedIndex > 0) // Không phải "Tất cả"
                {
                    query += " AND ct.HANGMUC = @hangMuc";
                }
                
                if (cbKhachHang.SelectedIndex > 0) // Không phải "Tất cả"
                {
                    query += " AND dh.MAKHACH = @khachHang";
                }
                
                if (cbKinhDoanh.SelectedIndex > 0) // Không phải "Tất cả"
                {
                    query += " AND dh.MAKINHDOANH = @maKinhDoanh";
                }
                
                if (cbMacBeTong.SelectedIndex > 0) // Không phải "Tất cả"
                {
                    query += " AND px.MACBETONG = @macBeTong";
                }
                
                if (cbXeTron.SelectedIndex > 0) // Không phải "Tất cả"
                {
                    query += " AND px.MAXE = @bienSo";
                }
                
                if (cbKyHieuDon.SelectedIndex > 0) // Không phải "Tất cả"
                {
                    query += " AND dh.KYHIEUDON = @kyHieuDon";
                }
                
                query += " ORDER BY px.NGAYXUAT DESC";
                
                DataTable dt = new DataTable();
                
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        // Thêm tham số
                        if (!allStations && cbTram.SelectedIndex >= 0)
                        {
                            cmd.Parameters.Add("@maTram", SqlDbType.Int).Value = (cbTram.SelectedItem as dynamic).Value;
                        }
                        
                        if (fromDate != DateTime.MinValue)
                        {
                            cmd.Parameters.Add("@fromDate", SqlDbType.Date).Value = fromDate.Date;
                        }
                        
                        if (toDate != DateTime.MinValue)
                        {
                            cmd.Parameters.Add("@toDate", SqlDbType.Date).Value = toDate.Date;
                        }
                        
                        if (cbHangMuc.SelectedIndex > 0)
                        {
                            cmd.Parameters.Add("@hangMuc", SqlDbType.NVarChar).Value = cbHangMuc.SelectedItem.ToString();
                        }
                        
                        if (cbKhachHang.SelectedIndex > 0)
                        {
                            cmd.Parameters.Add("@khachHang", SqlDbType.Int).Value = (cbKhachHang.SelectedItem as dynamic).Value;
                        }
                        
                        if (cbKinhDoanh.SelectedIndex > 0)
                        {
                            cmd.Parameters.Add("@maKinhDoanh", SqlDbType.Int).Value = (cbKinhDoanh.SelectedItem as dynamic).Value;
                        }
                        
                        if (cbMacBeTong.SelectedIndex > 0)
                        {
                            cmd.Parameters.Add("@macBeTong", SqlDbType.NVarChar).Value = cbMacBeTong.SelectedItem.ToString();
                        }
                        
                        if (cbXeTron.SelectedIndex > 0)
                        {
                            cmd.Parameters.Add("@bienSo", SqlDbType.Int).Value = (cbXeTron.SelectedItem as dynamic).Value;
                        }
                        
                        if (cbKyHieuDon.SelectedIndex > 0)
                        {
                            cmd.Parameters.Add("@kyHieuDon", SqlDbType.NVarChar).Value = cbKyHieuDon.SelectedItem.ToString();
                        }
                        
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
                
                // Hiển thị dữ liệu theo chế độ thống kê
                if (soChuyen)
                {
                    // Thống kê số chuyến
                    dgv.Columns.Add("stt", "STT");
                    dgv.Columns.Add("sophieu", "Số phiếu");
                    dgv.Columns.Add("ngayxuat", "Ngày xuất");
                    dgv.Columns.Add("khachhang", "Khách hàng");
                    dgv.Columns.Add("hangmuc", "Hạng mục");
                    dgv.Columns.Add("macbetong", "Mác bê tông");
                    dgv.Columns.Add("bienso", "Xe trộn");
                    dgv.Columns.Add("khoiluong", "Khối lượng (m³)");
                    
                    int stt = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        dgv.Rows.Add(
                            stt++,
                            row["SOPHIEU"].ToString(),
                            Convert.ToDateTime(row["NGAYXUAT"]).ToString("dd/MM/yyyy"),
                            row["TENKHACH"].ToString(),
                            row["HANGMUC"].ToString(),
                            row["MACBETONG"].ToString(),
                            row["BIENSO"].ToString(),
                            row["KHOILUONG"].ToString()
                        );
                    }
                }
                else if (chiTiet)
                {
                    // Thống kê chi tiết
                    dgv.Columns.Add("stt", "STT");
                    dgv.Columns.Add("sophieu", "Số phiếu");
                    dgv.Columns.Add("ngayxuat", "Ngày xuất");
                    dgv.Columns.Add("mado", "Mã đơn");
                    dgv.Columns.Add("khachhang", "Khách hàng");
                    dgv.Columns.Add("hangmuc", "Hạng mục");
                    dgv.Columns.Add("macbetong", "Mác bê tông");
                    dgv.Columns.Add("bienso", "Xe trộn");
                    dgv.Columns.Add("kinhdoanh", "Kinh doanh");
                    dgv.Columns.Add("khoiluong", "Khối lượng (m³)");
                    
                    int stt = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        dgv.Rows.Add(
                            stt++,
                            row["SOPHIEU"].ToString(),
                            Convert.ToDateTime(row["NGAYXUAT"]).ToString("dd/MM/yyyy"),
                            row["MADONHANG"].ToString(),
                            row["TENKHACH"].ToString(),
                            row["HANGMUC"].ToString(),
                            row["MACBETONG"].ToString(),
                            row["BIENSO"].ToString(),
                            row["TENKINHDOANH"].ToString(),
                            row["KHOILUONG"].ToString()
                        );
                    }
                }
                else if (tong)
                {
                    // Thống kê tổng hợp
                    dgv.Columns.Add("stt", "STT");
                    dgv.Columns.Add("nhom", "Nhóm");
                    dgv.Columns.Add("soluong", "Số lượng (m³)");
                    dgv.Columns.Add("tyle", "Tỷ lệ (%)");
                    
                    // Tính tổng khối lượng
                    decimal totalQuantity = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        decimal quantity = 0;
                        decimal.TryParse(row["KHOILUONG"].ToString(), out quantity);
                        totalQuantity += quantity;
                    }
                    
                    // Nhóm theo khách hàng
                    var groups = dt.AsEnumerable()
                        .GroupBy(r => r.Field<string>("TENKHACH"))
                        .Select(g => new { 
                            Name = g.Key, 
                            Quantity = g.Sum(r => r.Field<decimal?>("KHOILUONG") ?? 0) 
                        })
                        .ToList();
                    
                    int stt = 1;
                    foreach (var group in groups)
                    {
                        decimal percentage = totalQuantity > 0 ? (group.Quantity / totalQuantity) * 100 : 0;
                        dgv.Rows.Add(
                            stt++,
                            group.Name,
                            group.Quantity,
                            percentage.ToString("N2")
                        );
                    }
                }
                
                // Tính toán tổng
                decimal totalKhoiLuong = 0;
                foreach (DataRow row in dt.Rows)
                {
                    decimal quantity = 0;
                    decimal.TryParse(row["KHOILUONG"].ToString(), out quantity);
                    totalKhoiLuong += quantity;
                }
                
                // Cập nhật label tổng
                lblTong.Text = $"{dt.Rows.Count} chuyến\n{totalKhoiLuong:N2} m³";
                
                // Load dữ liệu vật liệu
                LoadMaterialData(materialGrid, dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu thống kê: " + ex.Message);
            }
        }
        
        private void LoadMaterialData(DataGridView materialGrid, DataTable dt)
        {
            try
            {
                // Xóa dữ liệu cũ
                materialGrid.Rows.Clear();
                
                // Lấy danh sách các phiếu xuất từ DataTable
                var phieuXuatIds = dt.AsEnumerable().Select(r => r.Field<int>("MAPHIEUXUAT")).ToList();
                
                if (phieuXuatIds.Count == 0) return;
                
                // Tạo câu truy vấn để lấy thông tin vật liệu
                string query = @"
                SELECT ct.MAPHIEUXUAT, v.TENVATTU, ct.SOLUONG
                FROM CHITIETPHIEUXUAT ct
                INNER JOIN VATTU v ON ct.MAVATTU = v.MAVATTU
                WHERE ct.MAPHIEUXUAT IN (" + string.Join(",", phieuXuatIds) + ")";
                
                DataTable materialData = new DataTable();
                
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(materialData);
                        }
                    }
                }
                
                // Nhóm vật liệu theo loại
                var materials = materialData.AsEnumerable()
                    .GroupBy(r => r.Field<string>("TENVATTU"))
                    .Select(g => new { 
                        Name = g.Key, 
                        TotalQuantity = g.Sum(r => r.Field<decimal?>("SOLUONG") ?? 0) 
                    })
                    .ToList();
                
                // Thêm dữ liệu vào DataGridView
                foreach (var material in materials)
                {
                    string materialName = material.Name.ToLower();
                    string displayValue = material.TotalQuantity.ToString("N2");
                    
                    // Xác định cột tương ứng
                    if (materialName.Contains("ximăng") || materialName.Contains("xi măng"))
                    {
                        materialGrid.Rows.Add("Xi măng", "", "", displayValue, "", "");
                    }
                    else if (materialName.Contains("cát"))
                    {
                        materialGrid.Rows.Add("Cát", displayValue, "", "", "", "");
                    }
                    else if (materialName.Contains("đá"))
                    {
                        materialGrid.Rows.Add("Đá", "", displayValue, "", "", "");
                    }
                    else if (materialName.Contains("nước"))
                    {
                        materialGrid.Rows.Add("Nước", "", "", "", displayValue, "");
                    }
                    else if (materialName.Contains("phụ gia"))
                    {
                        materialGrid.Rows.Add("Phụ gia", "", "", "", "", displayValue);
                    }
                    else
                    {
                        // Các vật liệu khác
                        materialGrid.Rows.Add(material.Name, "", "", "", "", displayValue);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu vật liệu: " + ex.Message);
            }
        }
        
        private void PrintData(DataGridView dgvMain, DataGridView dgvMaterial, string totalText, string reportTitle)
        {
            try
            {
                // Tạo đối tượng PrintDocument
                PrintDialog printDialog = new PrintDialog();
                PrintDocument printDocument = new PrintDocument();
                
                // Thiết lập sự kiện in
                printDocument.PrintPage += (sender, e) => {
                    // Thiết lập font và màu
                    Font titleFont = new Font("Segoe UI", 16, FontStyle.Bold);
                    Font headerFont = new Font("Segoe UI", 12, FontStyle.Bold);
                    Font contentFont = new Font("Segoe UI", 10);
                    Font footerFont = new Font("Segoe UI", 8);
                    
                    // Vị trí bắt đầu in
                    float yPos = e.MarginBounds.Top;
                    float xPos = e.MarginBounds.Left;
                    float centerPos = e.MarginBounds.Width / 2 + e.MarginBounds.Left;
                    
                    // In tiêu đề
                    e.Graphics.DrawString(reportTitle, titleFont, Brushes.Black, centerPos, yPos, new StringFormat { Alignment = StringAlignment.Center });
                    yPos += titleFont.GetHeight(e.Graphics) + 20;
                    
                    // In ngày in
                    e.Graphics.DrawString("Ngày in: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), contentFont, Brushes.Black, xPos, yPos);
                    yPos += contentFont.GetHeight(e.Graphics) + 20;
                    
                    // In bảng dữ liệu chính
                    float tableWidth = e.MarginBounds.Width;
                    float colWidth = tableWidth / dgvMain.Columns.Count;
                    
                    // In header
                    foreach (DataGridViewColumn col in dgvMain.Columns)
                    {
                        e.Graphics.DrawString(col.HeaderText, headerFont, Brushes.Black, xPos, yPos);
                        xPos += colWidth;
                    }
                    
                    xPos = e.MarginBounds.Left;
                    yPos += headerFont.GetHeight(e.Graphics) + 5;
                    
                    // Vẽ đường kẻ
                    e.Graphics.DrawLine(Pens.Black, e.MarginBounds.Left, yPos, e.MarginBounds.Right, yPos);
                    yPos += 5;
                    
                    // In dữ liệu
                    foreach (DataGridViewRow row in dgvMain.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            e.Graphics.DrawString(cell.Value?.ToString() ?? "", contentFont, Brushes.Black, xPos, yPos);
                            xPos += colWidth;
                        }
                        xPos = e.MarginBounds.Left;
                        yPos += contentFont.GetHeight(e.Graphics) + 2;
                    }
                    
                    // Vẽ đường kẻ cuối bảng
                    e.Graphics.DrawLine(Pens.Black, e.MarginBounds.Left, yPos, e.MarginBounds.Right, yPos);
                    yPos += 20;
                    
                    // In bảng vật liệu
                    e.Graphics.DrawString("THỐNG KÊ VẬT LIỆU", headerFont, Brushes.Black, centerPos, yPos, new StringFormat { Alignment = StringAlignment.Center });
                    yPos += headerFont.GetHeight(e.Graphics) + 10;
                    
                    // In header bảng vật liệu
                    float materialColWidth = tableWidth / dgvMaterial.Columns.Count;
                    foreach (DataGridViewColumn col in dgvMaterial.Columns)
                    {
                        e.Graphics.DrawString(col.HeaderText, headerFont, Brushes.Black, xPos, yPos);
                        xPos += materialColWidth;
                    }
                    
                    xPos = e.MarginBounds.Left;
                    yPos += headerFont.GetHeight(e.Graphics) + 5;
                    
                    // Vẽ đường kẻ
                    e.Graphics.DrawLine(Pens.Black, e.MarginBounds.Left, yPos, e.MarginBounds.Right, yPos);
                    yPos += 5;
                    
                    // In dữ liệu vật liệu
                    foreach (DataGridViewRow row in dgvMaterial.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            e.Graphics.DrawString(cell.Value?.ToString() ?? "", contentFont, Brushes.Black, xPos, yPos);
                            xPos += materialColWidth;
                        }
                        xPos = e.MarginBounds.Left;
                        yPos += contentFont.GetHeight(e.Graphics) + 2;
                    }
                    
                    // Vẽ đường kẻ cuối bảng vật liệu
                    e.Graphics.DrawLine(Pens.Black, e.MarginBounds.Left, yPos, e.MarginBounds.Right, yPos);
                    yPos += 20;
                    
                    // In tổng
                    string[] totalLines = totalText.Split('\n');
                    foreach (string line in totalLines)
                    {
                        e.Graphics.DrawString(line, headerFont, Brushes.Black, centerPos, yPos, new StringFormat { Alignment = StringAlignment.Center });
                        yPos += headerFont.GetHeight(e.Graphics) + 5;
                    }
                    
                    // In footer
                    yPos = e.MarginBounds.Bottom - 20;
                    };
                
                printDialog.Document = printDocument;
                
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDocument.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi in dữ liệu: " + ex.Message);
            }
        }
        
        private void ExportToExcel(DataGridView dgvMain, DataGridView dgvMaterial, string totalText, string reportTitle)
        {
            try
            {
                // Tạo dialog lưu file
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Xuất file Excel",
                    FileName = $"ThongKe_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx"
                };
                
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                
                // Tạo workbook mới
                using (var workbook = new XLWorkbook())
                {
                    // Tạo worksheet cho dữ liệu chính
                    var worksheet = workbook.Worksheets.Add("Thống kê");
                    
                    // Thiết lập tiêu đề
                    worksheet.Cell(1, 1).Value = reportTitle;
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                    worksheet.Range(1, 1, 1, dgvMain.Columns.Count).Merge();
                    
                    // Thêm ngày xuất
                    worksheet.Cell(2, 1).Value = "Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    worksheet.Range(2, 1, 2, dgvMain.Columns.Count).Merge();
                    
                    // Thêm header
                    int colIndex = 1;
                    foreach (DataGridViewColumn col in dgvMain.Columns)
                    {
                        worksheet.Cell(4, colIndex).Value = col.HeaderText;
                        worksheet.Cell(4, colIndex).Style.Font.Bold = true;
                        worksheet.Cell(4, colIndex).Style.Fill.BackgroundColor = XLColor.SteelBlue;
                        worksheet.Cell(4, colIndex).Style.Font.FontColor = XLColor.White;
                        colIndex++;
                    }
                    
                    // Thêm dữ liệu
                    int rowIndex = 5;
                    foreach (DataGridViewRow row in dgvMain.Rows)
                    {
                        colIndex = 1;
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            worksheet.Cell(rowIndex, colIndex).Value = cell.Value?.ToString();
                            colIndex++;
                        }
                        rowIndex++;
                    }
                    
                    // Tạo worksheet cho vật liệu
                    var materialWorksheet = workbook.Worksheets.Add("Vật liệu");
                    
                    // Tiêu đề
                    materialWorksheet.Cell(1, 1).Value = "THỐNG KÊ VẬT LIỆU";
                    materialWorksheet.Cell(1, 1).Style.Font.Bold = true;
                    materialWorksheet.Cell(1, 1).Style.Font.FontSize = 14;
                    materialWorksheet.Range(1, 1, 1, dgvMaterial.Columns.Count).Merge();
                    
                    // Header vật liệu
                    colIndex = 1;
                    foreach (DataGridViewColumn col in dgvMaterial.Columns)
                    {
                        materialWorksheet.Cell(3, colIndex).Value = col.HeaderText;
                        materialWorksheet.Cell(3, colIndex).Style.Font.Bold = true;
                        materialWorksheet.Cell(3, colIndex).Style.Fill.BackgroundColor = XLColor.SteelBlue;
                        materialWorksheet.Cell(3, colIndex).Style.Font.FontColor = XLColor.White;
                        colIndex++;
                    }
                    
                    // Dữ liệu vật liệu
                    rowIndex = 4;
                    foreach (DataGridViewRow row in dgvMaterial.Rows)
                    {
                        colIndex = 1;
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            materialWorksheet.Cell(rowIndex, colIndex).Value = cell.Value?.ToString();
                            colIndex++;
                        }
                        rowIndex++;
                    }
                    
                    // Tạo worksheet cho tổng
                    var totalWorksheet = workbook.Worksheets.Add("Tổng hợp");
                    
                    // Tiêu đề
                    totalWorksheet.Cell(1, 1).Value = "THỐNG KÊ TỔNG";
                    totalWorksheet.Cell(1, 1).Style.Font.Bold = true;
                    totalWorksheet.Cell(1, 1).Style.Font.FontSize = 14;
                    
                    // Thêm thông tin tổng
                    string[] totalLines = totalText.Split('\n');
                    rowIndex = 3;
                    foreach (string line in totalLines)
                    {
                        totalWorksheet.Cell(rowIndex, 1).Value = line;
                        totalWorksheet.Cell(rowIndex, 1).Style.Font.Bold = true;
                        rowIndex++;
                    }
                    
                    // Auto-fit columns
                    worksheet.Columns().AdjustToContents();
                    materialWorksheet.Columns().AdjustToContents();
                    totalWorksheet.Columns().AdjustToContents();
                    
                    // Lưu workbook
                    workbook.SaveAs(saveFileDialog.FileName);
                }
                
                MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Mở file sau khi xuất
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = saveFileDialog.FileName,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file Excel: " + ex.Message);
            }
        }
        
        private void ExportToCSV(DataGridView dgvMain, DataGridView dgvMaterial, string totalText, string reportTitle)
        {
            try
            {
                // Tạo dialog lưu file
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV Files|*.csv",
                    Title = "Xuất file CSV",
                    FileName = $"ThongKe_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv"
                };
                
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                
                // Tạo file CSV
                using (var writer = new StreamWriter(saveFileDialog.FileName, false, System.Text.Encoding.UTF8))
                {
                    // Ghi tiêu đề
                    writer.WriteLine(reportTitle);
                    writer.WriteLine("Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                    writer.WriteLine();
                    
                    // Ghi header bảng chính
                    for (int i = 0; i < dgvMain.Columns.Count; i++)
                    {
                        writer.Write(dgvMain.Columns[i].HeaderText);
                        if (i < dgvMain.Columns.Count - 1)
                            writer.Write(",");
                    }
                    writer.WriteLine();
                    
                    // Ghi dữ liệu bảng chính
                    foreach (DataGridViewRow row in dgvMain.Rows)
                    {
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            // Xử lý ký tự đặc biệt trong CSV
                            string cellValue = row.Cells[i].Value?.ToString().Replace("\"", "\"\"") ?? "";
                            writer.Write("\"" + cellValue + "\"");
                            
                            if (i < row.Cells.Count - 1)
                                writer.Write(",");
                        }
                        writer.WriteLine();
                    }
                    
                    writer.WriteLine();
                    writer.WriteLine("THỐNG KÊ VẬT LIỆU");
                    writer.WriteLine();
                    
                    // Ghi header bảng vật liệu
                    for (int i = 0; i < dgvMaterial.Columns.Count; i++)
                    {
                        writer.Write(dgvMaterial.Columns[i].HeaderText);
                        if (i < dgvMaterial.Columns.Count - 1)
                            writer.Write(",");
                    }
                    writer.WriteLine();
                    
                    // Ghi dữ liệu bảng vật liệu
                    foreach (DataGridViewRow row in dgvMaterial.Rows)
                    {
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            // Xử lý ký tự đặc biệt trong CSV
                            string cellValue = row.Cells[i].Value?.ToString().Replace("\"", "\"\"") ?? "";
                            writer.Write("\"" + cellValue + "\"");
                            
                            if (i < row.Cells.Count - 1)
                                writer.Write(",");
                        }
                        writer.WriteLine();
                    }
                    
                    writer.WriteLine();
                    writer.WriteLine("THỐNG KÊ TỔNG");
                    writer.WriteLine();
                    
                    // Ghi thông tin tổng
                    string[] totalLines = totalText.Split('\n');
                    foreach (string line in totalLines)
                    {
                        writer.WriteLine(line);
                    }
                }
                
                MessageBox.Show("Xuất file CSV thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Mở file sau khi xuất
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = saveFileDialog.FileName,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file CSV: " + ex.Message);
            }
        }
    }
}