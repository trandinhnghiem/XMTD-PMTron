using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using QuanLyTram.DAL;

namespace QuanLyTram.Forms
{
    public class MainForm : Form
    {
        private DataGridView dgvData;
        private ComboBox cbMaTram;
        private TextBox txtChuTram, txtDiaDiem, txtCongSuat, txtSoDienThoai;
        private TableLayoutPanel tlpToolbar;
        private Form currentChildForm;
        
        // Thông tin người dùng và quyền truy cập
        private int userId;
        private string username;
        private string hoten;
        private string capdo;
        private string quyen;
        private Button btnDanhMuc, btnDatHang, btnCapPhoi, btnInPhieu, btnThongKe, btnKho, btnCaiDat;
        
        public MainForm()
        {
            Text = "QUẢN LÝ SỐ LIỆU TRẠM TRỘN";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1200, 400);
            BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            
            // === TOOLBAR ===
            tlpToolbar = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 90,
                ColumnCount = 9, // 7 menu chữ + 2 nút tròn
                BackColor = Color.FromArgb(235, 240, 245),
                Padding = new Padding(10)
            };
            for (int i = 0; i < 7; i++)
                tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70f / 7f));
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60));
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60));
            
            // Tạo các nút menu
            btnDanhMuc = MakeBigButton("DANH MỤC");
            btnDatHang = MakeBigButton("ĐẶT HÀNG");
            btnCapPhoi = MakeBigButton("CẤP PHỐI");
            btnInPhieu = MakeBigButton("IN PHIẾU");
            btnThongKe = MakeBigButton("THỐNG KÊ");
            btnKho = MakeBigButton("KHO");
            btnCaiDat = MakeBigButton("CÀI ĐẶT");
            
            // Thêm sự kiện click cho các nút menu
            btnDanhMuc.Click += (s, e) => OpenForm(typeof(DanhMucForm));
            btnDatHang.Click += (s, e) => OpenForm(typeof(DatHangForm));
            btnCapPhoi.Click += (s, e) => OpenForm(typeof(CapPhoiForm));
            btnInPhieu.Click += (s, e) => OpenForm(typeof(InPhieuForm));
            btnThongKe.Click += (s, e) => OpenForm(typeof(ThongKeForm));
            btnKho.Click += (s, e) => OpenForm(typeof(KhoForm));
            btnCaiDat.Click += (s, e) => OpenForm(typeof(CaiDat_ChungForm));
            
            // Thêm các nút vào toolbar
            tlpToolbar.Controls.Add(btnDanhMuc, 0, 0);
            tlpToolbar.Controls.Add(btnDatHang, 1, 0);
            tlpToolbar.Controls.Add(btnCapPhoi, 2, 0);
            tlpToolbar.Controls.Add(btnInPhieu, 3, 0);
            tlpToolbar.Controls.Add(btnThongKe, 4, 0);
            tlpToolbar.Controls.Add(btnKho, 5, 0);
            tlpToolbar.Controls.Add(btnCaiDat, 6, 0);
            
            // --- Nút Logout ---
            IconButton btnLogout = new IconButton
            {
                Size = new Size(50, 50),
                BackColor = Color.Orange,
                ForeColor = Color.White,
                IconChar = IconChar.SignOutAlt,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(5, 6, 6, 6),
                Anchor = AnchorStyles.None,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += (s, e) =>
            {
                if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Hide();
                    using (var loginForm = new LoginForm())
                    {
                        loginForm.ShowDialog();
                    }
                    Close();
                }
            };
            btnLogout.MouseEnter += (s, e) => btnLogout.BackColor = ControlPaint.Light(btnLogout.BackColor);
            btnLogout.MouseLeave += (s, e) => btnLogout.BackColor = Color.Orange;
            btnLogout.Layout += (s, e) =>
            {
                using (var gp = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    gp.AddEllipse(0, 0, btnLogout.Width - 1, btnLogout.Height - 1);
                    btnLogout.Region = new Region(gp);
                }
            };
            
            // --- Nút Exit ---
            IconButton btnExit = new IconButton
            {
                Size = new Size(50, 50),
                BackColor = Color.Red,
                ForeColor = Color.White,
                IconChar = IconChar.PowerOff,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(4, 6, 10, 6),
                Anchor = AnchorStyles.None,
                Cursor = Cursors.Hand
            };
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.Click += (s, e) =>
            {
                if (MessageBox.Show("Bạn có chắc thoát ứng dụng này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Application.Exit();
                }
            };
            btnExit.MouseEnter += (s, e) => btnExit.BackColor = ControlPaint.Light(btnExit.BackColor);
            btnExit.MouseLeave += (s, e) => btnExit.BackColor = Color.Red;
            btnExit.Layout += (s, e) =>
            {
                if (btnExit.Width > 0 && btnExit.Height > 0)
                {
                    using (var gp = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        gp.AddEllipse(0, 0, btnExit.Width - 1, btnExit.Height - 1);
                        btnExit.Region = new Region(gp);
                    }
                }
            };
            tlpToolbar.Controls.Add(btnLogout, 7, 0);
            tlpToolbar.Controls.Add(btnExit, 8, 0);
            Controls.Add(tlpToolbar);
            
            // === GROUPBOX: Thông tin trạm ===
            var grpTram = new GroupBox
            {
                Text = "THÔNG TIN TRẠM",
                Dock = DockStyle.Top,
                Height = 120,
            };
            var lblMaTram = new Label { Text = "Mã trạm:", AutoSize = true, Location = new Point(15, 30) };
            cbMaTram = new ComboBox
            {
                Location = new Point(90, 26),
                Width = 420,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            // Thêm sự kiện SelectedIndexChanged
            cbMaTram.SelectedIndexChanged += CbMaTram_SelectedIndexChanged;
            
            LoadTramData();
            
            var lblChuTram = new Label { Text = "Chủ trạm:", AutoSize = true, Location = new Point(15, 65) };
            txtChuTram = new TextBox { Location = new Point(90, 61), Width = 420, ReadOnly = true };
            var lblDiaDiem = new Label { Text = "Địa điểm:", AutoSize = true, Location = new Point(540, 30) };
            txtDiaDiem = new TextBox { Location = new Point(610, 26), Width = 300, ReadOnly = true };
            var lblCongSuat = new Label { Text = "Công suất:", AutoSize = true, Location = new Point(540, 65) };
            txtCongSuat = new TextBox { Location = new Point(610, 61), Width = 150, ReadOnly = true };
            var lblSdt = new Label { Text = "Số điện thoại:", AutoSize = true, Location = new Point(770, 65) };
            txtSoDienThoai = new TextBox { Location = new Point(860, 61), Width = 150, ReadOnly = true };
            
            grpTram.Controls.AddRange(new Control[] {
                lblMaTram, cbMaTram, lblChuTram, txtChuTram,
                lblDiaDiem, txtDiaDiem, lblCongSuat, txtCongSuat, lblSdt, txtSoDienThoai
            });
            
            // === DATAGRIDVIEW (chỉnh style đồng bộ) ===
            dgvData = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                MultiSelect = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = true,
                EnableHeadersVisualStyles = true
            };
            dgvData.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "STT", DataPropertyName = "STT", Width = 60, FillWeight = 40 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Công suất - Địa điểm", DataPropertyName = "CongSuatDiaDiem" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tổng KL trong ngày", DataPropertyName = "TongKLNgay" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Khách hàng vừa trộn", DataPropertyName = "KhachHang" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Biển xe vừa trộn", DataPropertyName = "BienXe" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "KL xe vừa trộn", DataPropertyName = "KLXe" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mác bê tông vừa trộn", DataPropertyName = "MacBeTong" });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Trạng thái", DataPropertyName = "TrangThai" });
            
            Controls.Add(dgvData);
            Controls.Add(grpTram);
            Controls.Add(tlpToolbar);
            
            Load += MainForm_Load;
        }
        
        // Xử lý sự kiện thay đổi trạm trong ComboBox
        private void CbMaTram_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMaTram.SelectedItem != null)
            {
                try
                {
                    // Sử dụng DataRowView thay vì dynamic để an toàn hơn
                    DataRowView selectedRow = cbMaTram.SelectedItem as DataRowView;
                    if (selectedRow != null)
                    {
                        int maTram = Convert.ToInt32(selectedRow["Value"]);
                        LoadTramInfo(maTram);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xử lý chọn trạm: " + ex.Message);
                }
            }
        }
        
        // Tải thông tin chi tiết của trạm
        private void LoadTramInfo(int maTram)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT TENTRAM, CHUTRAM, DIADIEM, CONGSUAT, DIENTHOAI, TRANGTHAI FROM TRAM WHERE MATRAM = @maTram";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTram", maTram);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Kiểm tra null trước khi gán giá trị
                                txtChuTram.Text = reader["CHUTRAM"] != DBNull.Value ? reader["CHUTRAM"].ToString() : "";
                                txtDiaDiem.Text = reader["DIADIEM"] != DBNull.Value ? reader["DIADIEM"].ToString() : "";
                                txtCongSuat.Text = reader["CONGSUAT"] != DBNull.Value ? reader["CONGSUAT"].ToString() : "";
                                txtSoDienThoai.Text = reader["DIENTHOAI"] != DBNull.Value ? reader["DIENTHOAI"].ToString() : "";
                            }
                            else
                            {
                                // Xóa thông tin nếu không tìm thấy trạm
                                txtChuTram.Text = "";
                                txtDiaDiem.Text = "";
                                txtCongSuat.Text = "";
                                txtSoDienThoai.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin trạm: " + ex.Message);
            }
        }
        
        // Phương thức thiết lập thông tin người dùng và quyền truy cập
        public void SetUserInfo(int userId, string username, string hoten, string capdo, string quyen)
        {
            this.userId = userId;
            this.username = username;
            this.hoten = hoten;
            this.capdo = capdo;
            this.quyen = quyen;
            
            // Cập nhật quyền truy cập cho các nút menu
            UpdateMenuPermissions();
        }
        
        // Cập nhật trạng thái các nút menu dựa trên quyền truy cập
        private void UpdateMenuPermissions()
        {
            // Mặc định disable tất cả các nút
            btnDanhMuc.Enabled = false;
            btnDatHang.Enabled = false;
            btnCapPhoi.Enabled = false;
            btnInPhieu.Enabled = false;
            btnThongKe.Enabled = false;
            btnKho.Enabled = false;
            btnCaiDat.Enabled = false;
            
            // Phân tách chuỗi quyền thành mảng
            if (string.IsNullOrEmpty(quyen))
                return;
                
            string[] permissions = quyen.Split(new string[] { ", " }, StringSplitOptions.None);
            
            // Enable các nút tương ứng với quyền
            foreach (string permission in permissions)
            {
                switch (permission)
                {
                    case "Danh mục":
                        btnDanhMuc.Enabled = true;
                        break;
                    case "Cấp phối":
                        btnCapPhoi.Enabled = true;
                        break;
                    case "Thống kê":
                        btnThongKe.Enabled = true;
                        break;
                    case "Cài đặt":
                        btnCaiDat.Enabled = true;
                        break;
                    case "Kho":
                        btnKho.Enabled = true;
                        break;
                    case "Đặt hàng":
                        btnDatHang.Enabled = true;
                        break;
                    case "In phiếu":
                        btnInPhieu.Enabled = true;
                        break;
                }
            }
        }
        
        // Mở form dựa trên quyền truy cập
        private void OpenForm(Type formType)
        {
            if (currentChildForm != null && !currentChildForm.IsDisposed)
                currentChildForm.Close();
                
            var form = (Form)Activator.CreateInstance(formType);
            form.StartPosition = FormStartPosition.CenterScreen;
            
            // Kiểm tra nếu form là CaiDat_ChungForm thì đăng ký sự kiện DataChanged
            if (form is CaiDat_ChungForm caiDatForm)
            {
                caiDatForm.DataChanged += (sender, e) => 
                {
                    // Reload dữ liệu trạm khi nhận được sự kiện
                    LoadTramData();
                    // Cập nhật DataGridView hiển thị danh sách trạm
                    LoadDataGridViewData();
                };
            }
            
            currentChildForm = form;
            form.FormClosed += (s, e) => { currentChildForm = null; };
            form.Show();
        }
        
        private void LoadTramData()
        {
            try
            {
                // Tạo DataTable để làm DataSource cho ComboBox
                DataTable dtTram = new DataTable();
                dtTram.Columns.Add("Value", typeof(int));
                dtTram.Columns.Add("Display", typeof(string));
                
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT MATRAM, TENTRAM FROM TRAM", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dtTram.Rows.Add(reader["MATRAM"], reader["TENTRAM"].ToString());
                            }
                        }
                    }
                }
                
                // Lưu lại trạm đang chọn trước khi cập nhật
                int selectedTramId = -1;
                if (cbMaTram.SelectedValue != null)
                {
                    selectedTramId = Convert.ToInt32(cbMaTram.SelectedValue);
                }
                
                // Gán DataSource cho ComboBox
                cbMaTram.DataSource = dtTram;
                cbMaTram.DisplayMember = "Display";
                cbMaTram.ValueMember = "Value";
                
                // Khôi phục lại trạm đã chọn nếu còn tồn tại
                if (selectedTramId > 0)
                {
                    foreach (DataRow row in dtTram.Rows)
                    {
                        if (Convert.ToInt32(row["Value"]) == selectedTramId)
                        {
                            cbMaTram.SelectedValue = selectedTramId;
                            break;
                        }
                    }
                }
                // Nếu không tìm thấy trạm đã chọn, chọn mục đầu tiên
                else if (cbMaTram.Items.Count > 0)
                {
                    cbMaTram.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu trạm: " + ex.Message);
            }
        }
        
        // Thêm phương thức để tải lại dữ liệu cho DataGridView
        private void LoadDataGridViewData()
        {
            var dt = new DataTable();
            dt.Columns.Add("STT");
            dt.Columns.Add("CongSuatDiaDiem");
            dt.Columns.Add("TongKLNgay");
            dt.Columns.Add("KhachHang");
            dt.Columns.Add("BienXe");
            dt.Columns.Add("KLXe");
            dt.Columns.Add("MacBeTong");
            dt.Columns.Add("TrangThai");
            
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(@"
                    SELECT MATRAM, TENTRAM, CHUTRAM, DIADIEM, CONGSUAT, DIENTHOAI, TRANGTHAI 
                    FROM TRAM", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            int stt = 1;
                            while (reader.Read())
                            {
                                string trangThai = reader["TRANGTHAI"].ToString();
                                string statusIcon = trangThai == "Online" ? "🖥 ✔" : "🖥 ❌";
                                dt.Rows.Add(
                                    stt++,
                                    $"{reader["CONGSUAT"]} - {reader["TENTRAM"]} - {reader["DIADIEM"]}",
                                    "0.0 m3",
                                    "----",
                                    "----",
                                    "0.0 m3",
                                    "----",
                                    statusIcon
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu trạm: " + ex.Message);
            }
            
            dgvData.DataSource = dt;
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadDataGridViewData();
            
            // ⚡ gán màu cho trạng thái
            dgvData.CellFormatting += DgvData_CellFormatting;
        }
        
        private void DgvData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvData.Columns[e.ColumnIndex].DataPropertyName == "TrangThai" && e.Value != null)
            {
                string status = e.Value.ToString();
                if (status.Contains("✔"))
                {
                    e.CellStyle.ForeColor = Color.Green; // màu xanh lá cho ✔
                }
                else if (status.Contains("❌"))
                {
                    e.CellStyle.ForeColor = Color.Red;   // màu đỏ cho ❌
                }
            }
        }
        
        private static Button MakeBigButton(string text)
        {
            var b = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(230, 230, 230),
                ForeColor = Color.Black,
                Dock = DockStyle.Fill,
                Margin = new Padding(6),
                Height = 60,
                Cursor = Cursors.Hand,
                Padding = new Padding(12, 0, 12, 0),
                TextAlign = ContentAlignment.MiddleCenter
            };
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = Color.Gainsboro;
            b.MouseEnter += (s, e) => b.BackColor = Color.WhiteSmoke;
            b.MouseLeave += (s, e) => b.BackColor = Color.FromArgb(230, 230, 230);
            return b;
        }
    }
}