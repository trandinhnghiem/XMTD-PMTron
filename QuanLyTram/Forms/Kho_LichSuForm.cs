using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp; // Thêm thư viện FontAwesome
using QuanLyTram.DAL;
using System.IO;
using System.Linq;

namespace QuanLyTram.Forms
{
    public class Kho_LichSuForm : Form
    {
        private DateTimePicker dtpFrom, dtpTo;
        private ComboBox cbSoTram, cbNhapXuat, cbVatLieu;
        private IconButton btnXem, btnExcel; // Thay đổi từ Button sang IconButton
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
                var mainLayout = new Panel
                {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(16, 10, 16, 16),
                    BackColor = Color.Beige
                };
                this.Controls.Add(mainLayout);
                
                // ========== Bộ lọc (GroupBox Thời gian thống kê) ==========
                var gbFilter = new GroupBox
                {
                    Text = "THỜI GIAN THỐNG KÊ",
                    Location = new Point(20, 8),
                    Size = new Size(1210, 130),
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.Red
                };
                mainLayout.Controls.Add(gbFilter);
                
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
                btnXem = new IconButton
                {
                    Text = "XEM",
                    Width = 100,
                    Height = 30,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(65, 131, 215), // Màu xanh dương
                    FlatStyle = FlatStyle.Flat,
                    IconChar = IconChar.Search,
                    IconColor = Color.White,
                    IconFont = IconFont.Auto,
                    IconSize = 15,
                    TextImageRelation = TextImageRelation.ImageBeforeText,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ImageAlign = ContentAlignment.MiddleCenter,
                    Padding = new Padding(15, 0, 0, 0),
                    Cursor = Cursors.Hand
                };
                btnXem.FlatAppearance.BorderSize = 0;
                
                btnExcel = new IconButton
                {
                    Text = "EXCEL",
                    Width = 100,
                    Height = 30,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(110, 170, 60),
                    FlatStyle = FlatStyle.Flat,
                    IconChar = IconChar.FileExcel,
                    IconColor = Color.White,
                    IconFont = IconFont.Auto,
                    IconSize = 15,
                    TextImageRelation = TextImageRelation.ImageBeforeText,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ImageAlign = ContentAlignment.MiddleCenter,
                    Padding = new Padding(15, 0, 0, 0),
                    Cursor = Cursors.Hand
                };
                btnExcel.FlatAppearance.BorderSize = 0;
                
                pnlButtons.Controls.Add(btnXem);
                pnlButtons.Controls.Add(btnExcel);
                // thêm panel nút vào cột 3, hàng 2
                pnlFilter.Controls.Add(pnlButtons, 3, 2);
                
                gbFilter.Controls.Add(pnlFilter);
                
                // ========== Khối lượng tồn ==========
                var gbTon = new GroupBox
                {
                    Text = "KHỐI LƯỢNG TỒN CÁC THÀNH PHẦN",
                    Location = new Point(20, gbFilter.Bottom + 10),
                    Size = new Size(1210, 80),
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.Red
                };
                mainLayout.Controls.Add(gbTon);
                
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
                var lblGridTitle = new Label
                {
                    Text = "LỊCH SỬ NHẬP XUẤT THỐNG KÊ",
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    ForeColor = Color.Red,
                    AutoSize = true,
                    Location = new Point(20, gbTon.Bottom + 10)
                };
                mainLayout.Controls.Add(lblGridTitle);
                
                dgv = new DataGridView
                {
                    Location = new Point(20, lblGridTitle.Bottom + 8),
                    Size = new Size(1210, 430),
                    ReadOnly = true,
                    MultiSelect = false,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    RowHeadersVisible = true
                };
                mainLayout.Controls.Add(dgv);
                
                // Thiết lập tiêu đề cột
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                // Tạo các cột thủ công
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "STT", HeaderText = "STT", Width = 50 });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Ngay", HeaderText = "Ngày" });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Tram", HeaderText = "Trạm" });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "VatLieu", HeaderText = "Vật liệu" });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Loai", HeaderText = "Loại" });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoLuong", HeaderText = "Số lượng" });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "GhiChu", HeaderText = "Ghi chú" });
                
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
                            DataTable dtTram = new DataTable();
                            dtTram.Load(reader);
                            
                            foreach (DataRow row in dtTram.Rows)
                            {
                                cbSoTram.Items.Add(new { Value = row["MATRAM"], Display = row["TENTRAM"].ToString() });
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
                            DataTable dtVatTu = new DataTable();
                            dtVatTu.Load(reader);
                            
                            foreach (DataRow row in dtVatTu.Rows)
                            {
                                cbVatLieu.Items.Add(new { Value = row["MAVATTU"], Display = row["TENVATTU"].ToString() });
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
            
            // Load dữ liệu mặc định
            LoadData();
        }
        
        private void LoadData()
        {
            try
            {
                // Xóa dữ liệu hiện có trong DataGridView
                dgv.Rows.Clear();
                
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
                            _table = new DataTable();
                            adapter.Fill(_table);
                            
                            // Thêm dữ liệu vào DataGridView
                            for (int i = 0; i < _table.Rows.Count; i++)
                            {
                                var row = _table.Rows[i];
                                dgv.Rows.Add(
                                    i + 1, // STT
                                    Convert.ToDateTime(row["NGAYGIAODICH"]).ToString("dd/MM/yyyy"),
                                    row["TENTRAM"],
                                    row["TENVATTU"],
                                    row["LOAIGIAODICH"],
                                    row["SOLUONG"],
                                    row["GHICHU"] ?? "" // Xử lý giá trị null
                                );
                            }
                        }
                    }
                }
                
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
            
            // Sử dụng một truy vấn duy nhất để tính tồn kho cho tất cả vật liệu
            string query = @"
            SELECT 
                v.MAVATTU,
                v.TENVATTU,
                ISNULL(SUM(CASE WHEN k.LOAIGIAODICH = N'Nhập' THEN k.SOLUONG ELSE 0 END), 0) AS TongNhap,
                ISNULL(SUM(CASE WHEN k.LOAIGIAODICH = N'Xuất' THEN k.SOLUONG ELSE 0 END), 0) AS TongXuat
            FROM VATTU v
            LEFT JOIN KHO k ON v.MAVATTU = k.MAVATTU
            GROUP BY v.MAVATTU, v.TENVATTU
            ORDER BY v.MAVATTU";
            
            using (var cmd = new SqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                var inventoryText = "";
                
                while (reader.Read())
                {
                    string tenVatTu = reader["TENVATTU"].ToString();
                    decimal tongNhap = Convert.ToDecimal(reader["TongNhap"]);
                    decimal tongXuat = Convert.ToDecimal(reader["TongXuat"]);
                    decimal tonKho = tongNhap - tongXuat;
                    
                    inventoryText += $"{tenVatTu}: {tonKho}\n";
                }
                
                txtKhoiLuongTon.Text = inventoryText;
            }
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
            try
            {
                // Tạo một tạm file Excel
                string tempPath = Path.GetTempPath();
                string fileName = $"LichSuNhapXuat_{DateTime.Now:ddMMyyyy_HHmmss}.xlsx";
                string filePath = Path.Combine(tempPath, fileName);
                
                // Tạo file Excel và ghi dữ liệu
                using (var writer = new StreamWriter(filePath))
                {
                    // Ghi tiêu đề
                    writer.WriteLine("STT\tNgày\tTrạm\tVật liệu\tLoại\tSố lượng\tGhi chú");
                    
                    // Ghi dữ liệu
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        var row = dgv.Rows[i];
                        writer.WriteLine(
                            $"{row.Cells["STT"].Value}\t" +
                            $"{row.Cells["Ngay"].Value}\t" +
                            $"{row.Cells["Tram"].Value}\t" +
                            $"{row.Cells["VatLieu"].Value}\t" +
                            $"{row.Cells["Loai"].Value}\t" +
                            $"{row.Cells["SoLuong"].Value}\t" +
                            $"{row.Cells["GhiChu"].Value}"
                        );
                    }
                }
                
                // Mở file Excel vừa tạo
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
                
                MessageBox.Show("Xuất dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất dữ liệu ra Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}