using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using QuanLyTron.DAL;

namespace QuanLyTron.Forms
{
    public class MainForm : Form
    {
        private ComboBox cbMac, cbBienSo;
        private TextBox txtKyHieu;
        private CheckBox chkBom;
        private DataGridView dgvData;
        private TableLayoutPanel mainLayout, rightPanel;
        private IconButton wifiButton; // Thêm biến để lưu nút Wifi
        private Form currentChildForm;
        private Label lblStationInfo; // Thêm label hiển thị thông tin trạm
        
        public MainForm()
        {
            Text = "Quản lý tại trạm";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1200, 500);
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeComponents();
            
            // Đăng ký sự kiện Load form
            this.Load += MainForm_Load;
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Hiển thị thông tin trạm hiện tại
            UpdateStationInfo();
            
            // Kiểm tra kết nối và cập nhật trạng thái nút Wifi
            UpdateConnectionStatus();
            
            // Tải dữ liệu cho các điều khiển
            LoadDataToControls();
        }
        
        // Phương thức cập nhật thông tin trạm
        private void UpdateStationInfo()
        {
            try
            {
                int stationId = DatabaseHelper.CurrentStationId;
                string stationName = DatabaseHelper.GetCurrentStationName();
                
                if (lblStationInfo != null)
                {
                    lblStationInfo.Text = $"Trạm: {stationName} (Mã trạm: {stationId})";
                }
                
                // Cập nhật tiêu đề form
                this.Text = $"Quản lý tại trạm - {stationName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật thông tin trạm: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        // Phương thức kiểm tra kết nối CSDL
        private bool CheckDatabaseConnection()
        {
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối cơ sở dữ liệu: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        // Phương thức cập nhật trạng thái kết nối (màu nút Wifi)
        private void UpdateConnectionStatus()
        {
            bool isConnected = CheckDatabaseConnection();
            
            if (wifiButton != null)
            {
                wifiButton.BackColor = isConnected ? Color.MediumSeaGreen : Color.Black;
                wifiButton.IconColor = isConnected ? Color.White : Color.Gray;
                
                // Cập nhật sự kiện MouseEnter/MouseLeave
                wifiButton.MouseEnter -= WifiButton_MouseEnter;
                wifiButton.MouseLeave -= WifiButton_MouseLeave;
                
                wifiButton.MouseEnter += WifiButton_MouseEnter;
                wifiButton.MouseLeave += WifiButton_MouseLeave;
            }
        }
        
        private void WifiButton_MouseEnter(object sender, EventArgs e)
        {
            bool isConnected = CheckDatabaseConnection();
            wifiButton.BackColor = isConnected ? ControlPaint.Light(Color.MediumSeaGreen) : ControlPaint.Light(Color.Black);
        }
        
        private void WifiButton_MouseLeave(object sender, EventArgs e)
        {
            bool isConnected = CheckDatabaseConnection();
            wifiButton.BackColor = isConnected ? Color.MediumSeaGreen : Color.Black;
        }
        
        // Phương thức tải dữ liệu cho các điều khiển
        private void LoadDataToControls()
        {
            // Tải dữ liệu cho ComboBox Mác
            LoadMacComboBox();
            
            // Tải dữ liệu cho ComboBox Biển số
            LoadBienSoComboBox();
            
            // Tải dữ liệu cho DataGridView
            LoadDataGridView();
            
            // Đăng ký sự kiện thay đổi bộ lọc
            cbMac.SelectedIndexChanged += FilterChanged;
            cbBienSo.SelectedIndexChanged += FilterChanged;
            txtKyHieu.TextChanged += FilterChanged;
            chkBom.CheckedChanged += FilterChanged;
        }
        
        // Sự kiện khi bộ lọc thay đổi
        private void FilterChanged(object sender, EventArgs e)
        {
            LoadDataGridView();
        }
        
        // Tải dữ liệu cho ComboBox Mác
        private void LoadMacComboBox()
        {
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    
                    // Lấy ID trạm hiện tại
                    int stationId = DatabaseHelper.CurrentStationId;
                    
                    // Cập nhật câu truy vấn để lọc theo trạm
                    string query = "SELECT DISTINCT MACBETONG FROM CAPPHOI WHERE MATRAM = @stationId ORDER BY MACBETONG";
                    
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@stationId", stationId);
                        
                        using (var adapter = new SqlDataAdapter(command))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            
                            // Thêm mục "Tất cả"
                            DataRow allRow = dataTable.NewRow();
                            allRow["MACBETONG"] = "Tất cả";
                            dataTable.Rows.InsertAt(allRow, 0);
                            
                            cbMac.DisplayMember = "MACBETONG";
                            cbMac.ValueMember = "MACBETONG";
                            cbMac.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu mác bê tông: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        // Tải dữ liệu cho ComboBox Biển số
        private void LoadBienSoComboBox()
        {
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    
                    // Bảng XE là dùng chung cho tất cả các trạm nên không cần lọc theo MATRAM
                    string query = "SELECT BIENSO FROM XE ORDER BY BIENSO";
                    
                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var adapter = new SqlDataAdapter(command))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            
                            // Thêm mục "Tất cả"
                            DataRow allRow = dataTable.NewRow();
                            allRow["BIENSO"] = "Tất cả";
                            dataTable.Rows.InsertAt(allRow, 0);
                            
                            cbBienSo.DisplayMember = "BIENSO";
                            cbBienSo.ValueMember = "BIENSO";
                            cbBienSo.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu biển số xe: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        // Tải dữ liệu cho DataGridView
        private void LoadDataGridView()
        {
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    
                    // Lấy ID trạm hiện tại
                    int stationId = DatabaseHelper.CurrentStationId;
                    
                    // Lấy giá trị bộ lọc
                    string selectedMac = cbMac.SelectedValue?.ToString();
                    string selectedBienSo = cbBienSo.SelectedValue?.ToString();
                    string kyHieu = txtKyHieu.Text.Trim();
                    bool useBom = chkBom.Checked;
                    
                    // Xây dựng câu truy vấn với bộ lọc
                    string query = @"
                        SELECT 
                            ROW_NUMBER() OVER (ORDER BY dh.MADONHANG) AS STT,
                            kh.TENKHACH AS KhachHang,
                            dh.KHOILUONG AS KLDatHang,
                            ISNULL(SUM(px.KHOILUONG), 0) AS KLDaCap,
                            MAX(px.SOPHIEU) AS SoPhieu,
                            ct.DIADIEM AS DiaDiem
                        FROM DONHANG dh
                        LEFT JOIN KHACHHANG kh ON dh.MAKHACH = kh.MAKHACH
                        LEFT JOIN CONGTRINH ct ON dh.MACONGTRINH = ct.MACONGTRINH
                        LEFT JOIN PHIEUXUAT px ON dh.MADONHANG = px.MADONHANG
                        WHERE dh.MATRAM = @stationId"; // Thêm điều kiện lọc theo trạm
                    
                    // Thêm điều kiện lọc
                    if (selectedMac != null && selectedMac != "Tất cả")
                    {
                        query += " AND px.MACBETONG = @Mac";
                    }
                    
                    if (selectedBienSo != null && selectedBienSo != "Tất cả")
                    {
                        query += " AND px.MAXE IN (SELECT MAXE FROM XE WHERE BIENSO = @BienSo)";
                    }
                    
                    if (!string.IsNullOrEmpty(kyHieu))
                    {
                        query += " AND dh.KYHIEUDON LIKE '%' + @KyHieu + '%'";
                    }
                    
                    if (useBom)
                    {
                        query += " AND px.SUDUNGBOM = 1";
                    }
                    
                    query += " GROUP BY dh.MADONHANG, kh.TENKHACH, dh.KHOILUONG, ct.DIADIEM";
                    
                    using (var command = new SqlCommand(query, connection))
                    {
                        // Thêm tham số trạm
                        command.Parameters.AddWithValue("@stationId", stationId);
                        
                        // Thêm tham số
                        if (selectedMac != null && selectedMac != "Tất cả")
                        {
                            command.Parameters.AddWithValue("@Mac", selectedMac);
                        }
                        
                        if (selectedBienSo != null && selectedBienSo != "Tất cả")
                        {
                            command.Parameters.AddWithValue("@BienSo", selectedBienSo);
                        }
                        
                        if (!string.IsNullOrEmpty(kyHieu))
                        {
                            command.Parameters.AddWithValue("@KyHieu", kyHieu);
                        }
                        
                        using (var adapter = new SqlDataAdapter(command))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            
                            // Cập nhật DataGridView
                            dgvData.DataSource = dataTable;
                            
                            // Đổi tên header sang tiếng Việt
                            dgvData.Columns["STT"].HeaderText = "STT";
                            dgvData.Columns["KhachHang"].HeaderText = "Khách Hàng";
                            dgvData.Columns["KLDatHang"].HeaderText = "Khối Lượng Đặt Hàng";
                            dgvData.Columns["KLDaCap"].HeaderText = "Khối Lượng Đã Cấp";
                            dgvData.Columns["SoPhieu"].HeaderText = "Số Phiếu";
                            dgvData.Columns["DiaDiem"].HeaderText = "Địa Điểm Công Trình";
                            
                            // In đậm header
                            dgvData.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                            dgvData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            
                            // Định dạng cột
                            dgvData.Columns["STT"].Width = 50;
                            dgvData.Columns["KhachHang"].Width = 250;
                            dgvData.Columns["KLDatHang"].Width = 150;
                            dgvData.Columns["KLDaCap"].Width = 150;
                            dgvData.Columns["SoPhieu"].Width = 100;
                            dgvData.Columns["DiaDiem"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            
                            // Định dạng số cho cột KL
                            dgvData.Columns["KLDatHang"].DefaultCellStyle.Format = "N2";
                            dgvData.Columns["KLDaCap"].DefaultCellStyle.Format = "N2";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void InitializeComponents()
        {
            // Main layout
            mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            Controls.Add(mainLayout);
            
            // ===== Left Panel =====
            var leftPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3  // Tăng lên 3 để chứa thông tin trạm
            };
            leftPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));  // Cho thông tin trạm
            leftPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));  // Cho bộ lọc
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));  // Cho DataGridView
            mainLayout.Controls.Add(leftPanel, 0, 0);
            
            // --- Thông tin trạm ---
            var stationPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight
            };
            
            lblStationInfo = new Label
            {
                Text = "Đang tải thông tin trạm...",
                AutoSize = true,
                Padding = new Padding(5),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.MediumSeaGreen
            };
            
            stationPanel.Controls.Add(lblStationInfo);
            leftPanel.Controls.Add(stationPanel, 0, 0);
            
            // --- Bộ lọc ---
            var filterPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true
            };
            filterPanel.Controls.Add(new Label 
            { 
                Text = "Mác BT:", 
                AutoSize = true, 
                Padding = new Padding(5),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
            });
            cbMac = new ComboBox { Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            filterPanel.Controls.Add(cbMac);
            filterPanel.Controls.Add(new Label 
            { 
                Text = "Biển số:", 
                AutoSize = true, 
                Padding = new Padding(5),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
            });
            cbBienSo = new ComboBox { Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            filterPanel.Controls.Add(cbBienSo);
            filterPanel.Controls.Add(new Label 
            { 
                Text = "Ký hiệu SP:", 
                AutoSize = true, 
                Padding = new Padding(5),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
            });
            txtKyHieu = new TextBox { Width = 150 };
            filterPanel.Controls.Add(txtKyHieu);
            chkBom = new CheckBox 
            { 
                Text = "Bơm BT", 
                AutoSize = true, 
                Padding = new Padding(5),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
            };
            filterPanel.Controls.Add(chkBom);
            leftPanel.Controls.Add(filterPanel, 0, 1);
            
            // --- DataGridView ---
            dgvData = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true
            };
            leftPanel.Controls.Add(dgvData, 0, 2);
            
            // ===== Right Panel =====
            rightPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                BackColor = Color.LightGray,
                Padding = new Padding(10)
            };
            
            // Hàng cố định 110px
            for (int i = 0; i < 4; i++)
                rightPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 110));
            rightPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            rightPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            mainLayout.Controls.Add(rightPanel, 1, 0);
            
            // ===== Các nút chức năng =====
            AddButton("CẤP PHỐI", IconChar.Flask, Color.SteelBlue, 0, 0, (s, e) => OpenChildForm(new CapPhoiForm()));
            AddButton("DANH MỤC", IconChar.FolderOpen, Color.Teal, 0, 1, (s, e) => OpenChildForm(new DanhMucForm()));
            AddButton("ĐẶT HÀNG", IconChar.ShoppingCart, Color.OrangeRed, 1, 0, (s, e) => OpenChildForm(new DatHangForm()));
            AddButton("IN PHIẾU", IconChar.Print, Color.Orange, 1, 1, (s, e) => OpenChildForm(new InPhieuForm()));
            AddButton("THỐNG KÊ", IconChar.ChartPie, Color.MediumPurple, 2, 0, (s, e) => OpenChildForm(new ThongKeForm()));
            AddButton("CÀI ĐẶT", IconChar.Cogs, Color.DarkSlateGray, 2, 1, (s, e) => OpenChildForm(new CaiDatForm()));
            
            // ===== Nút đặc biệt (tròn) =====
            wifiButton = AddCircleIconButton(IconChar.Wifi, Color.MediumSeaGreen, 3, 0, (s, e) =>
            {
                // Kiểm tra lại kết nối khi nhấn nút
                UpdateConnectionStatus();
                
                if (CheckDatabaseConnection())
                {
                    MessageBox.Show("Kết nối cơ sở dữ liệu và Internet thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Lỗi kết nối đến cơ sở dữ liệu!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
            
            AddCircleIconButton(IconChar.PowerOff, Color.Red, 3, 1, (s, e) =>
            {
                if (MessageBox.Show("Bạn có chắc muốn thoát ứng dụng?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Application.Exit();
                }
            });
        }
        
        // --- Quản lý mở/đóng child form ---
        private void OpenChildForm(Form childForm)
        {
            if (currentChildForm != null)
            {
                currentChildForm.Close(); // đóng form cũ trước khi mở form mới
            }
            currentChildForm = childForm;
            currentChildForm.FormClosed += (s, e) => { currentChildForm = null; }; // khi user tự đóng thì reset
            childForm.Show(); // non-modal (song song với main)
        }
        
        // Nút Icon + Text
        private void AddButton(string text, IconChar icon, Color backColor, int row, int col, EventHandler onClick)
        {
            var btn = new IconButton
            {
                Text = text,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                ImageAlign = ContentAlignment.MiddleCenter,
                TextImageRelation = TextImageRelation.ImageAboveText,
                Padding = new Padding(0, 8, 0, 0),
                IconChar = icon,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 36,
                FlatStyle = FlatStyle.Flat,
                BackColor = backColor,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += onClick;
            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(backColor);
            btn.MouseLeave += (s, e) => btn.BackColor = backColor;
            rightPanel.Controls.Add(btn, col, row);
        }
        
        // Nút tròn (icon-only)
        private IconButton AddCircleIconButton(IconChar icon, Color backColor, int row, int col, EventHandler onClick)
        {
            var btn = new IconButton
            {
                Size = new Size(70, 70),
                BackColor = backColor,
                ForeColor = Color.White,
                IconChar = icon,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 32,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.None,
                Cursor = Cursors.Hand,
                Text = ""
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += onClick;
            btn.Paint += (s, e) =>
            {
                System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
                gp.AddEllipse(0, 0, btn.Width - 1, btn.Height - 1);
                btn.Region = new Region(gp);
            };
            rightPanel.Controls.Add(btn, col, row);
            return btn;
        }
    }
}