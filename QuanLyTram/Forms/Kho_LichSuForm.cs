using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QuanLyTram.DAL;

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
            cbNhapXuat = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10, FontStyle.Regular), ForeColor = Color.Black };
            cbVatLieu = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10, FontStyle.Regular), ForeColor = Color.Black };
            
            // Load dữ liệu cho ComboBox
            LoadComboBoxData();
            
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
            
            // Đăng ký sự kiện
            btnXem.Click += BtnXem_Click;
            btnExcel.Click += BtnExcel_Click;
        }
        
        private void LoadComboBoxData()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    
                    // Load trạm
                    cbSoTram.Items.Add("Tất cả");
                    using (var cmd = new SqlCommand("SELECT MATRAM, TENTRAM FROM TRAM", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cbSoTram.Items.Add(new { Value = reader["MATRAM"], Display = reader["TENTRAM"].ToString() });
                            }
                        }
                    }
                    cbSoTram.DisplayMember = "Display";
                    cbSoTram.ValueMember = "Value";
                    if (cbSoTram.Items.Count > 0) cbSoTram.SelectedIndex = 0;
                    
                    // Load loại nhập xuất
                    cbNhapXuat.Items.Add("Tất cả");
                    cbNhapXuat.Items.Add("Nhập");
                    cbNhapXuat.Items.Add("Xuất");
                    cbNhapXuat.SelectedIndex = 0;
                    
                    // Load vật liệu
                    cbVatLieu.Items.Add("Tất cả");
                    using (var cmd = new SqlCommand("SELECT MAVATTU, TENVATTU FROM VATTU", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cbVatLieu.Items.Add(new { Value = reader["MAVATTU"], Display = reader["TENVATTU"].ToString() });
                            }
                        }
                    }
                    cbVatLieu.DisplayMember = "Display";
                    cbVatLieu.ValueMember = "Value";
                    if (cbVatLieu.Items.Count > 0) cbVatLieu.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu ComboBox: " + ex.Message);
            }
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
            
            // Load dữ liệu mặc định
            LoadData();
            
            dgv.DataSource = _table;
        }
        
        private void LoadData()
        {
            try
            {
                _table.Clear();
                
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    
                    // Xây dựng câu truy vấn động dựa trên bộ lọc
                    string query = @"
                    SELECT k.NGAYGIAODICH, t.TENTRAM, v.TENVATTU, k.LOAIGIAODICH, k.SOLUONG, k.GHICHU
                    FROM KHO k
                    JOIN TRAM t ON k.MATRAM = t.MATRAM
                    JOIN VATTU v ON k.MAVATTU = v.MAVATTU
                    WHERE 1=1";
                    
                    // Thêm điều kiện lọc
                    if (dtpFrom.Value.Date != DateTime.MinValue)
                    {
                        query += " AND CAST(k.NGAYGIAODICH AS DATE) >= @fromDate";
                    }
                    
                    if (dtpTo.Value.Date != DateTime.MinValue)
                    {
                        query += " AND CAST(k.NGAYGIAODICH AS DATE) <= @toDate";
                    }
                    
                    if (cbSoTram.SelectedIndex > 0) // Không phải "Tất cả"
                    {
                        query += " AND k.MATRAM = @maTram";
                    }
                    
                    if (cbNhapXuat.SelectedIndex > 0) // Không phải "Tất cả"
                    {
                        query += " AND k.LOAIGIAODICH = @loaiGiaoDich";
                    }
                    
                    if (cbVatLieu.SelectedIndex > 0) // Không phải "Tất cả"
                    {
                        query += " AND k.MAVATTU = @maVatTu";
                    }
                    
                    query += " ORDER BY k.NGAYGIAODICH DESC";
                    
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        // Thêm tham số
                        if (dtpFrom.Value.Date != DateTime.MinValue)
                        {
                            cmd.Parameters.Add("@fromDate", SqlDbType.Date).Value = dtpFrom.Value.Date;
                        }
                        
                        if (dtpTo.Value.Date != DateTime.MinValue)
                        {
                            cmd.Parameters.Add("@toDate", SqlDbType.Date).Value = dtpTo.Value.Date;
                        }
                        
                        if (cbSoTram.SelectedIndex > 0)
                        {
                            cmd.Parameters.Add("@maTram", SqlDbType.Int).Value = (cbSoTram.SelectedItem as dynamic).Value;
                        }
                        
                        if (cbNhapXuat.SelectedIndex > 0)
                        {
                            cmd.Parameters.Add("@loaiGiaoDich", SqlDbType.NVarChar).Value = cbNhapXuat.SelectedItem.ToString();
                        }
                        
                        if (cbVatLieu.SelectedIndex > 0)
                        {
                            cmd.Parameters.Add("@maVatTu", SqlDbType.Int).Value = (cbVatLieu.SelectedItem as dynamic).Value;
                        }
                        
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(_table);
                        }
                    }
                }
                
                // Cập nhật DataGridView
                dgv.DataSource = _table;
                
                // Tính toán khối lượng tồn
                CalculateInventory();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu lịch sử nhập xuất: " + ex.Message);
            }
        }
        
        private void CalculateInventory()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    
                    // Lấy danh sách vật liệu
                    var inventoryText = "";
                    
                    using (var cmd = new SqlCommand("SELECT MAVATTU, TENVATTU FROM VATTU", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int maVatTu = Convert.ToInt32(reader["MAVATTU"]);
                                string tenVatTu = reader["TENVATTU"].ToString();
                                
                                // Tính tổng nhập
                                decimal tongNhap = 0;
                                using (var cmdNhap = new SqlCommand("SELECT ISNULL(SUM(SOLUONG), 0) FROM KHO WHERE MAVATTU = @maVatTu AND LOAIGIAODICH = 'Nhập'", conn))
                                {
                                    cmdNhap.Parameters.Add("@maVatTu", SqlDbType.Int).Value = maVatTu;
                                    tongNhap = Convert.ToDecimal(cmdNhap.ExecuteScalar());
                                }
                                
                                // Tính tổng xuất
                                decimal tongXuat = 0;
                                using (var cmdXuat = new SqlCommand("SELECT ISNULL(SUM(SOLUONG), 0) FROM KHO WHERE MAVATTU = @maVatTu AND LOAIGIAODICH = 'Xuất'", conn))
                                {
                                    cmdXuat.Parameters.Add("@maVatTu", SqlDbType.Int).Value = maVatTu;
                                    tongXuat = Convert.ToDecimal(cmdXuat.ExecuteScalar());
                                }
                                
                                // Tính tồn kho
                                decimal tonKho = tongNhap - tongXuat;
                                
                                inventoryText += $"{tenVatTu}: {tonKho}\n";
                            }
                        }
                    }
                    
                    txtKhoiLuongTon.Text = inventoryText;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tính toán tồn kho: " + ex.Message);
            }
        }
        
        private void BtnXem_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        
        private void BtnExcel_Click(object sender, EventArgs e)
        {
            // Xuất dữ liệu ra Excel
            MessageBox.Show("Chức năng xuất Excel đang được phát triển!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}