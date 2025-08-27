using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QuanLyTram.DAL;
using QuanLyTram.Properties; // dùng Settings thủ công


namespace QuanLyTram.Forms
{
    public class LoginForm : Form
    {
        // Controls
        private TextBox txtUsername;
        private TextBox txtPassword;
        private CheckBox chkRemember;
        private Button btnLogin;
        private Button btnCancel;
        private GroupBox grpLogin;
        private Label lblTitle;
        private Label lblUsername;
        private Label lblPassword;

        public LoginForm()
        {
            InitializeComponent();
            LoadSavedLogin(); // load dữ liệu ghi nhớ khi mở form
        }

        private void InitializeComponent()
        {
            // Tạo các controls
            this.txtUsername = new TextBox();
            this.txtPassword = new TextBox();
            this.chkRemember = new CheckBox();
            this.btnLogin = new Button();
            this.btnCancel = new Button();
            this.grpLogin = new GroupBox();
            this.lblTitle = new Label();
            this.lblUsername = new Label();
            this.lblPassword = new Label();

            // Cấu hình GroupBox
            this.grpLogin.SuspendLayout();
            this.SuspendLayout();
            this.grpLogin.Controls.Add(this.lblPassword);
            this.grpLogin.Controls.Add(this.lblUsername);
            this.grpLogin.Controls.Add(this.lblTitle);
            this.grpLogin.Controls.Add(this.chkRemember);
            this.grpLogin.Controls.Add(this.btnCancel);
            this.grpLogin.Controls.Add(this.btnLogin);
            this.grpLogin.Controls.Add(this.txtPassword);
            this.grpLogin.Controls.Add(this.txtUsername);
            this.grpLogin.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular);
            this.grpLogin.Location = new Point(20, 20);
            this.grpLogin.Name = "grpLogin";
            this.grpLogin.Size = new Size(360, 240);
            this.grpLogin.TabStop = false;
            this.grpLogin.Text = "ĐĂNG NHẬP HỆ THỐNG";

            // Label Password
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            this.lblPassword.Location = new Point(20, 100);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new Size(68, 17);
            this.lblPassword.TabIndex = 7;
            this.lblPassword.Text = "Mật khẩu:";

            // Label Username
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            this.lblUsername.Location = new Point(20, 60);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new Size(107, 17);
            this.lblUsername.TabIndex = 6;
            this.lblUsername.Text = "Tên đăng nhập:";

            // Label Title
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.Red;
            this.lblTitle.Location = new Point(90, 25);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(185, 21);
            this.lblTitle.TabIndex = 5;
            this.lblTitle.Text = "HỆ THỐNG QUẢN LÝ TRẠM";

            // CheckBox Remember
            this.chkRemember.AutoSize = true;
            this.chkRemember.Location = new Point(20, 140);
            this.chkRemember.Name = "chkRemember";
            this.chkRemember.Size = new Size(133, 21);
            this.chkRemember.TabIndex = 4;
            this.chkRemember.Text = "Ghi nhớ đăng nhập";
            this.chkRemember.UseVisualStyleBackColor = true;

            // Button Cancel
            this.btnCancel.BackColor = Color.IndianRed;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.Location = new Point(190, 180);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(150, 40);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "HỦY";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

            // Button Login
            this.btnLogin.BackColor = Color.MediumSeaGreen;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = FlatStyle.Flat;
            this.btnLogin.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            this.btnLogin.ForeColor = Color.White;
            this.btnLogin.Location = new Point(20, 180);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new Size(150, 40);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "ĐĂNG NHẬP";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new EventHandler(this.BtnLogin_Click);

            // TextBox Password
            this.txtPassword.Location = new Point(140, 100);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new Size(200, 23);
            this.txtPassword.TabIndex = 1;

            // TextBox Username
            this.txtUsername.Location = new Point(140, 60);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new Size(200, 23);
            this.txtUsername.TabIndex = 0;

            // Form
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.WhiteSmoke;
            this.ClientSize = new Size(400, 280);
            this.Controls.Add(this.grpLogin);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập";
            this.grpLogin.ResumeLayout(false);
            this.grpLogin.PerformLayout();
            this.ResumeLayout(false);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtUsername.Text.Trim();
            string matKhau = txtPassword.Text.Trim();

            // Kiểm tra dữ liệu nhập vào
            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(@"
                        SELECT COUNT(*) 
                        FROM NGUOIDUNG
                        WHERE USERNAME = @u 
                        AND PASSWORD = @p", conn))
                    {
                        cmd.Parameters.Add("@u", SqlDbType.NVarChar, 50).Value = tenDangNhap;
                        cmd.Parameters.Add("@p", SqlDbType.NVarChar, 50).Value = matKhau;
                        int count = (int)cmd.ExecuteScalar();
                        if (count > 0)
                        {
                            // Ghi nhớ đăng nhập
                            SaveLogin();

                            MessageBox.Show("Đăng nhập thành công!");
                            this.Hide();
                            MainForm mainForm = new MainForm();
                            mainForm.Show();
                        }
                        else
                        {
                            MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi đăng nhập",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát chương trình?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // Lưu thông tin đăng nhập vào Settings
        private void SaveLogin()
        {
            Properties.Settings.Default.RememberMe = chkRemember.Checked;
            if (chkRemember.Checked)
            {
                Properties.Settings.Default.SavedUsername = txtUsername.Text;
                Properties.Settings.Default.SavedPassword = txtPassword.Text; // Warning: plain text
            }
            else
            {
                Properties.Settings.Default.SavedUsername = string.Empty;
                Properties.Settings.Default.SavedPassword = string.Empty;
            }
            Properties.Settings.Default.Save();
        }

        // Load thông tin đăng nhập từ Settings
        private void LoadSavedLogin()
        {
            if (Properties.Settings.Default.RememberMe)
            {
                txtUsername.Text = Properties.Settings.Default.SavedUsername;
                txtPassword.Text = Properties.Settings.Default.SavedPassword;
                chkRemember.Checked = true;
            }
        }
    }
}
