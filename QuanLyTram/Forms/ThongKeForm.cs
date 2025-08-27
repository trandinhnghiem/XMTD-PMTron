using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QuanLyTram.DAL;

namespace QuanLyTram.Forms
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
            dgv.Columns.Add("soluong", "Số lượng (m³)");
            
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
            
            // Đăng ký sự kiện
            btnXem.Click += (s, e) => {
                LoadStatisticsData(dgv, dtpFrom.Value, dtpTo.Value, cbTram, chkTatCa.Checked, 
                                    cbHangMuc, cbKhachHang, cbKinhDoanh, cbMacBeTong, cbXeTron, cbKyHieuDon,
                                    radSoChuyen.Checked, radChiTiet.Checked, radTong.Checked);
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
                    using (var cmd = new SqlCommand("SELECT TENKHACH FROM KHACHHANG", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cbKhachHang.Items.Add(reader["TENKHACH"].ToString());
                            }
                        }
                    }
                    if (cbKhachHang.Items.Count > 0) cbKhachHang.SelectedIndex = 0;
                    
                    // Load kinh doanh
                    cbKinhDoanh.Items.Add("Tất cả");
                    using (var cmd = new SqlCommand("SELECT TENKINHDOANH FROM KINHDOANH", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cbKinhDoanh.Items.Add(reader["TENKINHDOANH"].ToString());
                            }
                        }
                    }
                    if (cbKinhDoanh.Items.Count > 0) cbKinhDoanh.SelectedIndex = 0;
                    
                    // Load mác bê tông
                    cbMacBeTong.Items.Add("Tất cả");
                    using (var cmd = new SqlCommand("SELECT DISTINCT MACBETONG FROM CAUPHOI", conn))
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
                    using (var cmd = new SqlCommand("SELECT BIENSO FROM XE", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cbXeTron.Items.Add(reader["BIENSO"].ToString());
                            }
                        }
                    }
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
        
        private void LoadStatisticsData(DataGridView dgv, DateTime fromDate, DateTime toDate, 
                                        ComboBox cbTram, bool allStations, ComboBox cbHangMuc, ComboBox cbKhachHang, 
                                        ComboBox cbKinhDoanh, ComboBox cbMacBeTong, ComboBox cbXeTron, ComboBox cbKyHieuDon,
                                        bool soChuyen, bool chiTiet, bool tong)
        {
            try
            {
                // Xây dựng câu truy vấn động dựa trên bộ lọc
                string query = @"
                SELECT dh.MADONHANG, kh.TENKHACH, dh.NGAYDAT, ct.HANGMUC, cp.MACBETONG, 
                       x.BIENSO, dh.KHOILUONG
                FROM DONHANG dh
                LEFT JOIN KHACHHANG kh ON dh.MAKHACH = kh.MAKHACH
                LEFT JOIN CONGTRINH ct ON dh.MACONGTRINH = ct.MACONGTRINH
                LEFT JOIN CAUPHOI cp ON dh.MADONHANG = cp.MACAUPHOI
                LEFT JOIN XE x ON dh.MAXE = x.MAXE
                WHERE 1=1";
                
                // Thêm điều kiện lọc
                if (!allStations && cbTram.SelectedIndex >= 0)
                {
                    query += " AND dh.MATRAM = @maTram";
                }
                
                if (fromDate != DateTime.MinValue)
                {
                    query += " AND CAST(dh.NGAYDAT AS DATE) >= @fromDate";
                }
                
                if (toDate != DateTime.MinValue)
                {
                    query += " AND CAST(dh.NGAYDAT AS DATE) <= @toDate";
                }
                
                if (cbHangMuc.SelectedIndex > 0) // Không phải "Tất cả"
                {
                    query += " AND ct.HANGMUC = @hangMuc";
                }
                
                if (cbKhachHang.SelectedIndex > 0) // Không phải "Tất cả"
                {
                    query += " AND kh.TENKHACH = @khachHang";
                }
                
                if (cbKinhDoanh.SelectedIndex > 0) // Không phải "Tất cả"
                {
                    query += " AND dh.MAKINHDOANH = @maKinhDoanh";
                }
                
                if (cbMacBeTong.SelectedIndex > 0) // Không phải "Tất cả"
                {
                    query += " AND cp.MACBETONG = @macBeTong";
                }
                
                if (cbXeTron.SelectedIndex > 0) // Không phải "Tất cả"
                {
                    query += " AND x.BIENSO = @bienSo";
                }
                
                if (cbKyHieuDon.SelectedIndex > 0) // Không phải "Tất cả"
                {
                    query += " AND dh.KYHIEUDON = @kyHieuDon";
                }
                
                query += " ORDER BY dh.NGAYDAT DESC";
                
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
                            cmd.Parameters.Add("@khachHang", SqlDbType.NVarChar).Value = cbKhachHang.SelectedItem.ToString();
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
                            cmd.Parameters.Add("@bienSo", SqlDbType.NVarChar).Value = cbXeTron.SelectedItem.ToString();
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
                
                // Đổi tên cột để hiển thị
                dt.Columns["MADONHANG"].ColumnName = "madon";
                dt.Columns["TENKHACH"].ColumnName = "khachhang";
                dt.Columns["NGAYDAT"].ColumnName = "ngaylap";
                dt.Columns["HANGMUC"].ColumnName = "hangmuc";
                dt.Columns["MACBETONG"].ColumnName = "macbetong";
                dt.Columns["BIENSO"].ColumnName = "xetroi";
                dt.Columns["KHOILUONG"].ColumnName = "soluong";
                
                // Hiển thị dữ liệu
                dgv.DataSource = dt;
                dgv.Columns["ngaylap"].DefaultCellStyle.Format = "dd/MM/yyyy";
                
                // Tính toán tổng
                decimal totalQuantity = 0;
                foreach (DataRow row in dt.Rows)
                {
                    decimal quantity = 0;
                    decimal.TryParse(row["soluong"].ToString(), out quantity);
                    totalQuantity += quantity;
                }
                
                // Cập nhật label tổng
                var totalBox = dgv.Parent.Parent.Controls.Find("totalBox", true).FirstOrDefault() as GroupBox;
                if (totalBox != null)
                {
                    var lblTong = totalBox.Controls[0] as Label;
                    if (lblTong != null)
                    {
                        lblTong.Text = $"{dt.Rows.Count} đơn\n{totalQuantity} m³";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu thống kê: " + ex.Message);
            }
        }
    }
}