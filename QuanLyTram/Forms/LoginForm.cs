using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QuanLyTram.DAL;

namespace QuanLyTram.Forms
{
    public class LoginForm : Form
    {
        TextBox txtUsername;
        TextBox txtPassword;
        Button btnLogin;

        public LoginForm()
        {
            this.Text = "Đăng nhập";
            this.Size = new Size(300, 200);

            Label lblUser = new Label { Text = "Tên đăng nhập", Top = 20, Left = 10 };
            Label lblPass = new Label { Text = "Mật khẩu", Top = 60, Left = 10 };

            txtUsername = new TextBox { Top = 20, Left = 110, Width = 150 };
            txtPassword = new TextBox { Top = 60, Left = 110, Width = 150, PasswordChar = '*' };

            btnLogin = new Button { Text = "Đăng nhập", Top = 100, Left = 110 };
            btnLogin.Click += BtnLogin_Click;

            this.Controls.Add(lblUser);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPass);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(@"
                    SELECT COUNT(*) 
                    FROM MC_ACCOUNT
                    WHERE USERNAME = @u 
                    AND PASSWORD = @p", conn))
                {
                    cmd.Parameters.Add("@u", SqlDbType.NVarChar, 50).Value = txtUsername.Text.Trim();
                    cmd.Parameters.Add("@p", SqlDbType.NVarChar, 50).Value = txtPassword.Text;

                        int count = (int)cmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Đăng nhập thành công!");
                            this.Hide();
                            new MainForm().ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng nhập: " + ex.Message);
            }
        }
    }
}
