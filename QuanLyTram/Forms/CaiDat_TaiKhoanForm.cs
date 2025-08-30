using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using QuanLyTram.DAL;

namespace QuanLyTram.Forms
{
    public class CaiDat_TaiKhoanForm : Form
    {
        private enum EditMode { None, Add, Edit }
        private EditMode _mode = EditMode.None;
        private Panel pnlActions;
        private IconButton btnThemMoi, btnCapNhat, btnXoa;
        private IconButton btnLuu, btnHuy;
        private DataGridView dgv;
        private GroupBox grpInfo;
        private Label lblTenTK, lblHoTen, lblDiaChi, lblEmail, lblMatKhau, lblCapDo, lblQuyenTruyCap;
        private TextBox txtTenTK, txtHoTen, txtDiaChi, txtEmail, txtMatKhau;
        private ComboBox cmbCapDo;
        private CheckBox chkSelectAll;
        private CheckBox chkDanhMuc, chkMacBeTong, chkThongKe, chkCaiDat, chkKho;
        
        private Color BgLavender = Color.FromArgb(230, 220, 250);
        private Color PanelWhite = Color.FromArgb(245, 245, 255);
        private Font FTitle => new Font(new FontFamily("Segoe UI"), 11.5f, FontStyle.Bold);
        private Font FText => new Font(new FontFamily("Segoe UI"), 10f, FontStyle.Regular);
        
        private DataTable dtData; // DataTable dùng cho dgv
        // Biến lưu dữ liệu gốc để so sánh khi Hủy
        private string _originalTenTK = "";
        private string _originalHoTen = "";
        private string _originalDiaChi = "";
        private string _originalEmail = "";
        private string _originalMatKhau = "";
        private string _originalCapDo = "";
        private string _originalQuyen = "";
        
        public CaiDat_TaiKhoanForm()
        {
            Text = "TÀI KHOẢN";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1200, 720);
            BackColor = BgLavender;
            BuildActionBar();
            BuildMainArea();
            WireEvents();
            LoadData();
            ApplyMode(EditMode.None);
        }
        
        private void BuildActionBar()
        {
            pnlActions = new Panel
            {
                Dock = DockStyle.Top,
                Height = 75,
                BackColor = PanelWhite
            };
            Controls.Add(pnlActions);
            pnlActions.BringToFront();
            
            btnThemMoi = new IconButton
            {
                Text = "TẠO MỚI",
                Font = FTitle,
                BackColor = Color.FromArgb(110, 170, 60),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(160, 46),
                Location = new Point(24, 12),
                IconChar = IconChar.PlusCircle,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleCenter,
                ImageAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(18, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnThemMoi.FlatAppearance.BorderSize = 0;
            
            btnCapNhat = new IconButton
            {
                Text = "CẬP NHẬT",
                Font = FTitle,
                BackColor = Color.FromArgb(70, 130, 180),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(160, 46),
                Location = new Point(200, 12),
                IconChar = IconChar.Edit,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleCenter,
                ImageAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(17, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnCapNhat.FlatAppearance.BorderSize = 0;
            
            btnXoa = new IconButton
            {
                Text = "XÓA",
                Font = FTitle,
                BackColor = Color.FromArgb(200, 50, 50),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(160, 46),
                Location = new Point(376, 12),
                IconChar = IconChar.Trash,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleCenter,
                ImageAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(28, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnXoa.FlatAppearance.BorderSize = 0;
            
            pnlActions.Controls.Add(btnThemMoi);
            pnlActions.Controls.Add(btnCapNhat);
            pnlActions.Controls.Add(btnXoa);
        }
        
        private void BuildMainArea()
        {
            var main = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16, 10, 16, 16),
                BackColor = BgLavender
            };
            Controls.Add(main);
            main.BringToFront();
            
            // Label tiêu đề cho DataGridView
            var lblGridTitle = new Label
            {
                Text = "DANH SÁCH TÀI KHOẢN",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.Red,
                AutoSize = true,
                Location = new Point(20, 8)
            };
            main.Controls.Add(lblGridTitle);
            
            // DataGridView
            dgv = new DataGridView
            {
                Location = new Point(20, lblGridTitle.Bottom + 8),
                Size = new Size(740, 530),
                ReadOnly = true,
                MultiSelect = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = true,
                EnableHeadersVisualStyles = true
            };
            main.Controls.Add(dgv);
            
            // GroupBox
            grpInfo = new GroupBox
            {
                Text = "THÔNG TIN TÀI KHOẢN",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.Red,
                BackColor = Color.Transparent,
                Size = new Size(400, 560),
                Location = new Point(dgv.Right + 20, 8),
                Padding = new Padding(18)
            };
            main.Controls.Add(grpInfo);
            
            // Các control thông tin tài khoản
            lblTenTK = new Label
            {
                Text = "Tên tài khoản:",
                Font = new Font(FText.FontFamily, FText.Size, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 30),
                ForeColor = Color.Black
            };
            txtTenTK = new TextBox
            {
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Location = new Point(20, 55),
                Width = 340
            };
            
            lblHoTen = new Label
            {
                Text = "Họ tên:",
                Font = new Font(FText.FontFamily, FText.Size, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 90),
                ForeColor = Color.Black
            };
            txtHoTen = new TextBox
            {
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Location = new Point(20, 115),
                Width = 340
            };
            
            lblDiaChi = new Label
            {
                Text = "Địa chỉ:",
                Font = new Font(FText.FontFamily, FText.Size, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 150),
                ForeColor = Color.Black
            };
            txtDiaChi = new TextBox
            {
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Location = new Point(20, 175),
                Width = 340
            };
            
            lblEmail = new Label
            {
                Text = "Email:",
                Font = new Font(FText.FontFamily, FText.Size, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 210),
                ForeColor = Color.Black
            };
            txtEmail = new TextBox
            {
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Location = new Point(20, 235),
                Width = 340
            };
            
            lblMatKhau = new Label
            {
                Text = "Mật khẩu:",
                Font = new Font(FText.FontFamily, FText.Size, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 270),
                ForeColor = Color.Black
            };
            txtMatKhau = new TextBox
            {
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Location = new Point(20, 295),
                Width = 340,
                UseSystemPasswordChar = true
            };
            
            lblCapDo = new Label
            {
                Text = "Cấp độ:",
                Font = new Font(FText.FontFamily, FText.Size, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 330),
                ForeColor = Color.Black
            };
            cmbCapDo = new ComboBox
            {
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Location = new Point(20, 355),
                Width = 340,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCapDo.Items.AddRange(new object[] { "Quản lý", "Vận hành" });
            
            // Phần quyền truy cập
            lblQuyenTruyCap = new Label
            {
                Text = "Quyền truy cập:",
                Font = new Font(FText.FontFamily, FText.Size, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 390),
                ForeColor = Color.Black
            };

            // Checkbox "Chọn tất cả" - cùng hàng với label Quyền truy cập
            chkSelectAll = new CheckBox
            {
                Text = "Tất cả",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(285, 393), // Vị trí cùng hàng với label Quyền truy cập
                AutoSize = true
            };

            // Panel chứa 2 cột checkbox - nằm ngay dưới
            var pnlQuyen = new Panel
            {
                Location = new Point(30, 420), // Vị trí dưới label và checkbox chọn tất cả
                Size = new Size(360, 80), // Tăng chiều rộng và chiều cao
                BackColor = Color.Transparent
            };

            // Cột trái - 3 quyền
            chkDanhMuc = new CheckBox
            {
                Text = "Danh mục",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(0, 0),
                AutoSize = true
            };

            chkMacBeTong = new CheckBox
            {
                Text = "Mác bê tông",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(0, 25),
                AutoSize = true
            };

            chkThongKe = new CheckBox
            {
                Text = "Thống kê",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(0, 50),
                AutoSize = true
            };

            // Cột phải - 2 quyền
            chkCaiDat = new CheckBox
            {
                Text = "Cài đặt",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(180, 0), // Tăng khoảng cách giữa 2 cột
                AutoSize = true
            };

            chkKho = new CheckBox
            {
                Text = "Kho",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(180, 25), // Tăng khoảng cách giữa 2 cột
                AutoSize = true
            };

            pnlQuyen.Controls.AddRange(new Control[] { chkDanhMuc, chkMacBeTong, chkThongKe, chkCaiDat, chkKho });

            // Nút Lưu và Hủy - điều chỉnh lại vị trí
            btnLuu = new IconButton
            {
                Text = " LƯU",
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                BackColor = Color.LightSkyBlue,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(160, 46),
                Location = new Point(20, 505), // Điều chỉnh vị trí xuống dưới
                IconChar = IconChar.Save,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleCenter,
                ImageAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(28, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnLuu.FlatAppearance.BorderSize = 0;

            btnHuy = new IconButton
            {
                Text = " HỦY",
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                BackColor = Color.LightCoral,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(160, 46),
                Location = new Point(btnLuu.Right + 16, 505), // Điều chỉnh vị trí xuống dưới
                IconChar = IconChar.TimesCircle,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleCenter,
                ImageAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(28, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnHuy.FlatAppearance.BorderSize = 0;

            // Thêm các control vào GroupBox
            grpInfo.Controls.AddRange(new Control[] {
                lblTenTK, txtTenTK, lblHoTen, txtHoTen, lblDiaChi, txtDiaChi,
                lblEmail, txtEmail, lblMatKhau, txtMatKhau, lblCapDo, cmbCapDo,
                lblQuyenTruyCap, chkSelectAll, pnlQuyen, btnLuu, btnHuy
            });
        }
        
        private void WireEvents()
        {
            btnThemMoi.Click += (s, e) =>
            {
                ApplyMode(EditMode.Add);
                txtTenTK.Focus();
            };
            
            btnCapNhat.Click += (s, e) =>
            {
                if (dgv.CurrentRow != null)
                {
                    ApplyMode(EditMode.Edit);
                }
            };
            
            btnXoa.Click += (s, e) =>
            {
                if (dgv.CurrentRow != null)
                {
                    var result = MessageBox.Show(
                        "Bạn có chắc chắn muốn xóa tài khoản này?",
                        "Xác nhận xóa",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );
                    
                    if (result == DialogResult.Yes)
                    {
                        DeleteAccount();
                    }
                }
            };
            
            btnLuu.Click += (s, e) => SaveCurrent();
            
            btnHuy.Click += (s, e) =>
            {
                if (txtTenTK.Text != _originalTenTK || txtHoTen.Text != _originalHoTen || 
                    txtDiaChi.Text != _originalDiaChi || txtEmail.Text != _originalEmail ||
                    txtMatKhau.Text != _originalMatKhau || cmbCapDo.Text != _originalCapDo ||
                    GetSelectedPermissions() != _originalQuyen)
                {
                    var result = MessageBox.Show(
                        "Bạn có chắc chắn muốn hủy? Mọi thay đổi sẽ không được lưu.",
                        "Xác nhận hủy",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );
                    if (result == DialogResult.No)
                        return;
                }
                ApplyMode(EditMode.None);
            };
            
            dgv.SelectionChanged += (s, e) =>
            {
                if (_mode == EditMode.None && dgv.CurrentRow != null)
                {
                    txtTenTK.Text = dgv.CurrentRow.Cells["Tên tài khoản"].Value.ToString();
                    txtHoTen.Text = dgv.CurrentRow.Cells["Họ tên"].Value.ToString();
                    txtDiaChi.Text = dgv.CurrentRow.Cells["Địa chỉ"].Value.ToString();
                    txtEmail.Text = dgv.CurrentRow.Cells["Email"].Value.ToString();
                    txtMatKhau.Text = dgv.CurrentRow.Cells["Mật khẩu"].Value.ToString();
                    cmbCapDo.Text = dgv.CurrentRow.Cells["Cấp độ"].Value.ToString();
                    
                    // Xử lý quyền truy cập
                    string permissions = dgv.CurrentRow.Cells["Quyền truy cập"].Value.ToString();
                    SetPermissionsFromString(permissions);
                }
            };
            
            // Sửa lại sự kiện cho checkbox "Chọn tất cả"
            chkSelectAll.CheckedChanged += (s, e) =>
            {
                // Chỉ xử lý khi người dùng trực tiếp thay đổi checkbox "Tất cả"
                // không phải khi nó được thay đổi bởi code
                if (chkSelectAll.Focused)
                {
                    bool isChecked = chkSelectAll.Checked;
                    chkDanhMuc.Checked = isChecked;
                    chkMacBeTong.Checked = isChecked;
                    chkThongKe.Checked = isChecked;
                    chkCaiDat.Checked = isChecked;
                    chkKho.Checked = isChecked;
                }
            };

            // Sửa lại sự kiện cho các checkbox quyền riêng lẻ
            chkDanhMuc.CheckedChanged += UpdateSelectAllCheckBox;
            chkMacBeTong.CheckedChanged += UpdateSelectAllCheckBox;
            chkThongKe.CheckedChanged += UpdateSelectAllCheckBox;
            chkCaiDat.CheckedChanged += UpdateSelectAllCheckBox;
            chkKho.CheckedChanged += UpdateSelectAllCheckBox;
        }
        
        private void UpdateSelectAllCheckBox(object sender, EventArgs e)
        {
            // Ngăn chặn sự kiện đệ quy bằng cách kiểm tra xem checkbox "Tất cả" có đang được focus không
            if (!chkSelectAll.Focused)
            {
                // Cập nhật trạng thái của checkbox "Chọn tất cả" dựa trên các checkbox con
                bool allChecked = chkDanhMuc.Checked && chkMacBeTong.Checked && 
                                chkThongKe.Checked && chkCaiDat.Checked && chkKho.Checked;
                
                // Ch cập nhật nếu trạng thái thay đổi
                if (allChecked != chkSelectAll.Checked)
                {
                    chkSelectAll.Checked = allChecked;
                }
            }
        }
        
        private void LoadData()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT ID, USERNAME, PASSWORD, HOTEN, DIACHI, CAPDO, EMAIL, DANGNHAPCUOI, QUYEN FROM NGUOIDUNG", conn))
                    {
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            dtData = new DataTable();
                            adapter.Fill(dtData);
                            
                            // Đổi tên cột để hiển thị
                            dtData.Columns["ID"].ColumnName = "STT";
                            dtData.Columns["USERNAME"].ColumnName = "Tên tài khoản";
                            dtData.Columns["PASSWORD"].ColumnName = "Mật khẩu";
                            dtData.Columns["HOTEN"].ColumnName = "Họ tên";
                            dtData.Columns["DIACHI"].ColumnName = "Địa chỉ";
                            dtData.Columns["CAPDO"].ColumnName = "Cấp độ";
                            dtData.Columns["EMAIL"].ColumnName = "Email";
                            dtData.Columns["DANGNHAPCUOI"].ColumnName = "Đăng nhập cuối";
                            dtData.Columns["QUYEN"].ColumnName = "Quyền truy cập";
                            
                            dgv.DataSource = dtData;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu tài khoản: " + ex.Message);
            }
            
            // in đậm tiêu đề cột
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            if (dgv.Rows.Count > 0)
                dgv.CurrentCell = dgv.Rows[0].Cells[0];
        }
        
        private void ApplyMode(EditMode mode)
        {
            _mode = mode;
            bool editing = mode != EditMode.None;
            
            UpdateButtonState(btnThemMoi, !editing, Color.FromArgb(110, 170, 60));
            UpdateButtonState(btnCapNhat, !editing && dgv.CurrentRow != null, Color.FromArgb(70, 130, 180));
            UpdateButtonState(btnXoa, !editing && dgv.CurrentRow != null, Color.FromArgb(200, 50, 50));
            UpdateButtonState(btnLuu, editing, Color.FromArgb(90, 90, 150));
            UpdateButtonState(btnHuy, editing, Color.FromArgb(180, 50, 50));
            
            // Enable/disable controls
            txtTenTK.Enabled = editing;
            txtHoTen.Enabled = editing;
            txtDiaChi.Enabled = editing;
            txtEmail.Enabled = editing;
            txtMatKhau.Enabled = editing;
            cmbCapDo.Enabled = editing;
            chkSelectAll.Enabled = editing;
            chkDanhMuc.Enabled = editing;
            chkMacBeTong.Enabled = editing;
            chkThongKe.Enabled = editing;
            chkCaiDat.Enabled = editing;
            chkKho.Enabled = editing;
            
            if (mode == EditMode.Add)
            {
                txtTenTK.Text = string.Empty;
                txtHoTen.Text = string.Empty;
                txtDiaChi.Text = string.Empty;
                txtEmail.Text = string.Empty;
                txtMatKhau.Text = string.Empty;
                cmbCapDo.SelectedIndex = 0;
                
                // Bỏ chọn tất cả quyền
                chkSelectAll.Checked = false;
                chkDanhMuc.Checked = false;
                chkMacBeTong.Checked = false;
                chkThongKe.Checked = false;
                chkCaiDat.Checked = false;
                chkKho.Checked = false;
                
                _originalTenTK = "";
                _originalHoTen = "";
                _originalDiaChi = "";
                _originalEmail = "";
                _originalMatKhau = "";
                _originalCapDo = "";
                _originalQuyen = "";
                
                txtTenTK.Focus();
            }
            else if (mode == EditMode.Edit && dgv.CurrentRow != null)
            {
                txtTenTK.Text = dgv.CurrentRow.Cells["Tên tài khoản"].Value.ToString();
                txtHoTen.Text = dgv.CurrentRow.Cells["Họ tên"].Value.ToString();
                txtDiaChi.Text = dgv.CurrentRow.Cells["Địa chỉ"].Value.ToString();
                txtEmail.Text = dgv.CurrentRow.Cells["Email"].Value.ToString();
                txtMatKhau.Text = dgv.CurrentRow.Cells["Mật khẩu"].Value.ToString();
                cmbCapDo.Text = dgv.CurrentRow.Cells["Cấp độ"].Value.ToString();
                
                // Xử lý quyền truy cập
                string permissions = dgv.CurrentRow.Cells["Quyền truy cập"].Value.ToString();
                SetPermissionsFromString(permissions);
                
                _originalTenTK = txtTenTK.Text;
                _originalHoTen = txtHoTen.Text;
                _originalDiaChi = txtDiaChi.Text;
                _originalEmail = txtEmail.Text;
                _originalMatKhau = txtMatKhau.Text;
                _originalCapDo = cmbCapDo.Text;
                _originalQuyen = GetSelectedPermissions();
                
                txtTenTK.Focus();
                txtTenTK.SelectAll();
            }
            else if (mode == EditMode.None && dgv.CurrentRow != null)
            {
                txtTenTK.Text = dgv.CurrentRow.Cells["Tên tài khoản"].Value.ToString();
                txtHoTen.Text = dgv.CurrentRow.Cells["Họ tên"].Value.ToString();
                txtDiaChi.Text = dgv.CurrentRow.Cells["Địa chỉ"].Value.ToString();
                txtEmail.Text = dgv.CurrentRow.Cells["Email"].Value.ToString();
                txtMatKhau.Text = dgv.CurrentRow.Cells["Mật khẩu"].Value.ToString();
                cmbCapDo.Text = dgv.CurrentRow.Cells["Cấp độ"].Value.ToString();
                
                // Xử lý quyền truy cập
                string permissions = dgv.CurrentRow.Cells["Quyền truy cập"].Value.ToString();
                SetPermissionsFromString(permissions);
                
                _originalTenTK = txtTenTK.Text;
                _originalHoTen = txtHoTen.Text;
                _originalDiaChi = txtDiaChi.Text;
                _originalEmail = txtEmail.Text;
                _originalMatKhau = txtMatKhau.Text;
                _originalCapDo = cmbCapDo.Text;
                _originalQuyen = GetSelectedPermissions();
            }
        }
        
        private void UpdateButtonState(IconButton btn, bool enabled, Color normalColor)
        {
            btn.BackColor = enabled ? normalColor : Color.FromArgb(200, 200, 200);
            Color inactiveColor = Color.FromArgb(120, 120, 120);
            btn.ForeColor = enabled ? Color.White : inactiveColor;
            btn.IconColor = btn.ForeColor;
        }
        
        private string GetSelectedPermissions()
        {
            string permissions = "";
            if (chkDanhMuc.Checked)
            {
                if (permissions != "") permissions += ", ";
                permissions += "Danh mục";
            }
            if (chkMacBeTong.Checked)
            {
                if (permissions != "") permissions += ", ";
                permissions += "Mác bê tông";
            }
            if (chkThongKe.Checked)
            {
                if (permissions != "") permissions += ", ";
                permissions += "Thống kê";
            }
            if (chkCaiDat.Checked)
            {
                if (permissions != "") permissions += ", ";
                permissions += "Cài đặt";
            }
            if (chkKho.Checked)
            {
                if (permissions != "") permissions += ", ";
                permissions += "Kho";
            }
            return permissions;
        }
        
        private void SetPermissionsFromString(string permissions)
        {
            // Bỏ chọn tất cả trước
            chkDanhMuc.Checked = false;
            chkMacBeTong.Checked = false;
            chkThongKe.Checked = false;
            chkCaiDat.Checked = false;
            chkKho.Checked = false;
            
            if (string.IsNullOrEmpty(permissions))
            {
                chkSelectAll.Checked = false;
                return;
            }
            
            string[] permissionArray = permissions.Split(new string[] { ", " }, StringSplitOptions.None);
            
            foreach (string permission in permissionArray)
            {
                switch (permission)
                {
                    case "Danh mục":
                        chkDanhMuc.Checked = true;
                        break;
                    case "Mác bê tông":
                        chkMacBeTong.Checked = true;
                        break;
                    case "Thống kê":
                        chkThongKe.Checked = true;
                        break;
                    case "Cài đặt":
                        chkCaiDat.Checked = true;
                        break;
                    case "Kho":
                        chkKho.Checked = true;
                        break;
                }
            }
            
            // Cập nhật checkbox "Chọn tất cả" sau khi đã thiết lập tất cả các checkbox con
            bool allChecked = chkDanhMuc.Checked && chkMacBeTong.Checked && 
                            chkThongKe.Checked && chkCaiDat.Checked && chkKho.Checked;
            chkSelectAll.Checked = allChecked;
        }
        private void SaveCurrent()
{
    var tenTK = (txtTenTK.Text ?? string.Empty).Trim();
    var hoTen = (txtHoTen.Text ?? string.Empty).Trim();
    var diaChi = (txtDiaChi.Text ?? string.Empty).Trim();
    var email = (txtEmail.Text ?? string.Empty).Trim();
    var matKhau = (txtMatKhau.Text ?? string.Empty).Trim();
    var capDo = cmbCapDo.Text;
    var quyenTruyCap = GetSelectedPermissions();
    
    if (string.IsNullOrWhiteSpace(tenTK))
    {
        MessageBox.Show("Vui lòng nhập Tên tài khoản.", "Thiếu thông tin",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtTenTK.Focus();
        return;
    }
    
    if (string.IsNullOrWhiteSpace(matKhau))
    {
        MessageBox.Show("Vui lòng nhập Mật khẩu.", "Thiếu thông tin",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtMatKhau.Focus();
        return;
    }
    
    try
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            conn.Open();
            
            if (_mode == EditMode.Add)
            {
                using (var cmd = new SqlCommand(@"
                INSERT INTO NGUOIDUNG (USERNAME, PASSWORD, HOTEN, DIACHI, CAPDO, EMAIL, DANGNHAPCUOI, QUYEN)
                VALUES (@username, @password, @hoten, @diachi, @capdo, @email, @dangnhapcuoi, @quyen);
                SELECT SCOPE_IDENTITY();", conn))
                {
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = tenTK;
                    cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = matKhau;
                    cmd.Parameters.Add("@hoten", SqlDbType.NVarChar).Value = hoTen;
                    cmd.Parameters.Add("@diachi", SqlDbType.NVarChar).Value = diaChi;
                    cmd.Parameters.Add("@capdo", SqlDbType.NVarChar).Value = capDo;
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
                    cmd.Parameters.Add("@dangnhapcuoi", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@quyen", SqlDbType.NVarChar).Value = quyenTruyCap;
                    
                    int newId = Convert.ToInt32(cmd.ExecuteScalar());
                    
                    // Thêm vào DataTable
                    DataRow newRow = dtData.NewRow();
                    newRow["STT"] = newId;
                    newRow["Tên tài khoản"] = tenTK;
                    newRow["Mật khẩu"] = matKhau;
                    newRow["Họ tên"] = hoTen;
                    newRow["Địa chỉ"] = diaChi;
                    newRow["Cấp độ"] = capDo;
                    newRow["Email"] = email;
                    newRow["Đăng nhập cuối"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    newRow["Quyền truy cập"] = quyenTruyCap;
                    dtData.Rows.Add(newRow);
                    
                    // Cập nhật DataGridView
                    dgv.DataSource = dtData;
                    dgv.CurrentCell = dgv.Rows[dgv.Rows.Count - 1].Cells[0];
                }
            }
            else if (_mode == EditMode.Edit && dgv.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgv.CurrentRow.Cells["STT"].Value);
                
                using (var cmd = new SqlCommand(@"
                UPDATE NGUOIDUNG 
                SET USERNAME = @username, PASSWORD = @password, HOTEN = @hoten, 
                    DIACHI = @diachi, CAPDO = @capdo, EMAIL = @email, QUYEN = @quyen
                WHERE ID = @id", conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = tenTK;
                    cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = matKhau;
                    cmd.Parameters.Add("@hoten", SqlDbType.NVarChar).Value = hoTen;
                    cmd.Parameters.Add("@diachi", SqlDbType.NVarChar).Value = diaChi;
                    cmd.Parameters.Add("@capdo", SqlDbType.NVarChar).Value = capDo;
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
                    cmd.Parameters.Add("@quyen", SqlDbType.NVarChar).Value = quyenTruyCap;
                    
                    cmd.ExecuteNonQuery();
                    
                    // Cập nhật DataTable
                    dgv.CurrentRow.Cells["Tên tài khoản"].Value = tenTK;
                    dgv.CurrentRow.Cells["Mật khẩu"].Value = matKhau;
                    dgv.CurrentRow.Cells["Họ tên"].Value = hoTen;
                    dgv.CurrentRow.Cells["Địa chỉ"].Value = diaChi;
                    dgv.CurrentRow.Cells["Cấp độ"].Value = capDo;
                    dgv.CurrentRow.Cells["Email"].Value = email;
                    dgv.CurrentRow.Cells["Quyền truy cập"].Value = quyenTruyCap;
                }
            }
        }
        
        ApplyMode(EditMode.None);
    }
    catch (Exception ex)
    {
        MessageBox.Show("Lỗi khi lưu dữ liệu tài khoản: " + ex.Message);
    }
}
        
        private void DeleteAccount()
        {
            if (dgv.CurrentRow == null) return;
            
            try
            {
                int id = Convert.ToInt32(dgv.CurrentRow.Cells["STT"].Value);
                
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("DELETE FROM NGUOIDUNG WHERE ID = @id", conn))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                        cmd.ExecuteNonQuery();
                        
                        // Xóa khỏi DataTable
                        dtData.Rows.RemoveAt(dgv.CurrentRow.Index);
                        
                        // Cập nhật DataGridView
                        dgv.DataSource = dtData;
                        
                        if (dgv.Rows.Count > 0)
                            dgv.CurrentCell = dgv.Rows[0].Cells[0];
                        else
                        {
                            txtTenTK.Text = "";
                            txtHoTen.Text = "";
                            txtDiaChi.Text = "";
                            txtEmail.Text = "";
                            txtMatKhau.Text = "";
                            cmbCapDo.SelectedIndex = -1;
                            
                            // Bỏ chọn tất cả quyền
                            chkSelectAll.Checked = false;
                            chkDanhMuc.Checked = false;
                            chkMacBeTong.Checked = false;
                            chkThongKe.Checked = false;
                            chkCaiDat.Checked = false;
                            chkKho.Checked = false;
                        }
                    }
                }
                
                MessageBox.Show("Xóa tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa tài khoản: " + ex.Message);
            }
        }
    }
}