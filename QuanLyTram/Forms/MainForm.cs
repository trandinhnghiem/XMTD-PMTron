using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace QuanLyTram.Forms
{
    public class MainForm : Form
    {
        private DataGridView dgvData;
        private ComboBox cbMaTram;
        private TextBox txtChuTram, txtDiaDiem, txtCongSuat, txtSoDienThoai;
        private TableLayoutPanel tlpToolbar;
        private Form currentChildForm;

        public MainForm()
        {
            Text = "QUẢN LÝ SỐ LIỆU TRẠM TRỘN";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1200, 400);
            BackColor = Color.White;

            // 🚫 Không cho kéo nhỏ hơn 1200x600
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // cũng khóa kéo thay đổi kích thước

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

            var menuItems = new (string Text, Type FormType)[]
            {
                ("DANH MỤC", typeof(DanhMucForm)),
                ("ĐẶT HÀNG", typeof(DatHangForm)),
                ("CẤP PHỐI", typeof(CapPhoiForm)),
                ("IN PHIẾU", typeof(InPhieuForm)),
                ("THỐNG KÊ", typeof(ThongKeForm)),
                ("KHO", typeof(KhoForm)),
                ("CÀI ĐẶT", typeof(CaiDatForm))
            };

            int col = 0;
            foreach (var (text, formType) in menuItems)
            {
                Button btn = MakeBigButton(text);
                btn.Click += (s, e) =>
                {
                    if (currentChildForm != null && !currentChildForm.IsDisposed)
                        currentChildForm.Close();

                    var form = (Form)Activator.CreateInstance(formType);
                    form.StartPosition = FormStartPosition.CenterScreen;

                    currentChildForm = form;
                    form.FormClosed += (s2, e2) => { currentChildForm = null; };
                    form.Show();
                };
                tlpToolbar.Controls.Add(btn, col++, 0);
            }

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
                    Hide();
                    using (var loginForm = new LoginForm())
                    {
                        Application.Exit();
                    }
                    Close();
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

            tlpToolbar.Controls.Add(btnLogout, col++, 0);
            tlpToolbar.Controls.Add(btnExit, col++, 0);

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
            cbMaTram.Items.AddRange(new object[]
            {
                "1 - CÔNG TY CỔ PHẦN BÊ TÔNG TÂY ĐÔ",
                "2 - TRẠM THỬ NGHIỆM"
            });
            if (cbMaTram.Items.Count > 0) cbMaTram.SelectedIndex = 0;

            var lblChuTram = new Label { Text = "Chủ trạm:", AutoSize = true, Location = new Point(15, 65) };
            txtChuTram = new TextBox { Location = new Point(90, 61), Width = 420, Text = "CÔNG TY CỔ PHẦN BÊ TÔNG TÂY ĐÔ" };

            var lblDiaDiem = new Label { Text = "Địa điểm:", AutoSize = true, Location = new Point(540, 30) };
            txtDiaDiem = new TextBox { Location = new Point(610, 26), Width = 300, Text = "T 90 Băng Tải - Hậu Giang" };

            var lblCongSuat = new Label { Text = "Công suất:", AutoSize = true, Location = new Point(540, 65) };
            txtCongSuat = new TextBox { Location = new Point(610, 61), Width = 150, Text = "90m3" };

            var lblSdt = new Label { Text = "Số điện thoại:", AutoSize = true, Location = new Point(770, 65) };
            txtSoDienThoai = new TextBox { Location = new Point(860, 61), Width = 150, Text = "123456789" };

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

        private void MainForm_Load(object sender, EventArgs e)
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

            dt.Rows.Add("1", "90m3 - T 90 Đặng Tài - Hậu Giang", "0.0 m3", "----", "----", "0.0 m3", "----", "🖥 ❌");
            dt.Rows.Add("2", "82m3 - T 82 Xe kíp - M", "0.0 m3", "----", "----", "0.0 m3", "----", "🖥 ❌");

            dgvData.DataSource = dt;
        }
    }
}
