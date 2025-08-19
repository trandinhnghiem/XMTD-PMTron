using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace QuanLyTram.Forms
{
    public class DM_DanhSachXeForm : Form
    {
        private enum EditMode { None, Add, Edit }
        private EditMode _mode = EditMode.None;

        private Panel pnlActions;
        private IconButton btnThemMoi, btnCapNhat;
        private IconButton btnLuu, btnHuy;

        private DataGridView dgv;

        private GroupBox grpInfo;
        private Label lblBienSo, lblLaiXe;
        private TextBox txtBienSo, txtLaiXe;

        private Color BgBlue = Color.FromArgb(220, 240, 255);
        private Color PanelWhite = Color.FromArgb(255, 255, 240);
        private Font FTitle => new Font(new FontFamily("Segoe UI"), 11.5f, FontStyle.Bold);
        private Font FText => new Font(new FontFamily("Segoe UI"), 10f, FontStyle.Regular);

        private DataTable dtData;

        // Biến lưu dữ liệu gốc để so sánh khi Hủy
        private string _originalBienSo = "";
        private string _originalLaiXe = "";

        public DM_DanhSachXeForm()
        {
            Text = "DANH SÁCH XE";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1200, 720);
            BackColor = BgBlue;

            BuildActionBar();
            BuildMainArea();
            WireEvents();

            InitData();
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
                Text = "THÊM MỚI",
                Font = FTitle,
                BackColor = Color.FromArgb(110, 170, 60),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(220, 46),
                Location = new Point(24, 12),
                IconChar = IconChar.PlusCircle,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Padding = new Padding(28, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnThemMoi.FlatAppearance.BorderSize = 0;

            btnCapNhat = new IconButton
            {
                Text = "CẬP NHẬT",
                Font = FTitle,
                BackColor = Color.FromArgb(70, 130, 180),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(220, 46),
                Location = new Point(260, 12),
                IconChar = IconChar.Edit,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Padding = new Padding(28, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnCapNhat.FlatAppearance.BorderSize = 0;

            pnlActions.Controls.Add(btnThemMoi);
            pnlActions.Controls.Add(btnCapNhat);
        }

        private void BuildMainArea()
        {
            var main = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16, 10, 16, 16),
                BackColor = BgBlue
            };
            Controls.Add(main);
            main.BringToFront();

            var lblGridTitle = new Label
            {
                Text = "DỮ LIỆU DANH SÁCH XE",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(8, 8)
            };
            main.Controls.Add(lblGridTitle);

            dgv = new DataGridView
            {
                Location = new Point(8, lblGridTitle.Bottom + 8),
                Size = new Size(740, 530),
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
            main.Controls.Add(dgv);

            grpInfo = new GroupBox
            {
                Text = "THÔNG TIN XE",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                Size = new Size(400, 560),
                Location = new Point(dgv.Right + 16, 8),
                Padding = new Padding(18)
            };
            main.Controls.Add(grpInfo);

            lblBienSo = new Label
            {
                Text = "BIỂN SỐ XE:",
                Font = FText,
                AutoSize = true,
                Location = new Point(20, 50),
                ForeColor = Color.DimGray
            };
            txtBienSo = new TextBox
            {
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Location = new Point(120, 47),  // Cùng hàng với Label
                Width = 240                     // Ngắn gọn lại
            };

            lblLaiXe = new Label
            {
                Text = "LÁI XE:",
                Font = FText,
                AutoSize = true,
                Location = new Point(20, 95),
                ForeColor = Color.DimGray
            };
            txtLaiXe = new TextBox
            {
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Location = new Point(120, 92),  // Cùng hàng
                Width = 240
            };

            btnLuu = new IconButton
            {
                Text = " LƯU",
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                BackColor = Color.LightSkyBlue,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(140, 40),
                Location = new Point(50, 160), // đẩy lên gần field
                IconChar = IconChar.Save,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 24,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnLuu.FlatAppearance.BorderSize = 0;

            btnHuy = new IconButton
            {
                Text = " HỦY",
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                BackColor = Color.LightCoral,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(140, 40),
                Location = new Point(btnLuu.Right + 16, 160), // cùng hàng với LƯU
                IconChar = IconChar.TimesCircle,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 24,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnHuy.FlatAppearance.BorderSize = 0;

            grpInfo.Controls.Add(lblBienSo);
            grpInfo.Controls.Add(txtBienSo);
            grpInfo.Controls.Add(lblLaiXe);
            grpInfo.Controls.Add(txtLaiXe);
            grpInfo.Controls.Add(btnLuu);
            grpInfo.Controls.Add(btnHuy);
        }

        private void WireEvents()
        {
            btnThemMoi.Click += (s, e) =>
            {
                ApplyMode(EditMode.Add);
                txtBienSo.Focus();
            };

            btnCapNhat.Click += (s, e) =>
            {
                if (dgv.CurrentRow != null)
                {
                    ApplyMode(EditMode.Edit);
                }
            };

            btnLuu.Click += (s, e) => SaveCurrent();

            btnHuy.Click += (s, e) =>
            {
                if (txtBienSo.Text != _originalBienSo || txtLaiXe.Text != _originalLaiXe)
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
                    txtBienSo.Text = dgv.CurrentRow.Cells["Biển số xe"].Value.ToString();
                    txtLaiXe.Text = dgv.CurrentRow.Cells["Lái xe"].Value.ToString();
                }
            };
        }

        private void InitData()
        {
            dtData = new DataTable();
            dtData.Columns.Add("Mã xe");
            dtData.Columns.Add("Biển số xe");
            dtData.Columns.Add("Lái xe");

            dtData.Rows.Add("1", "51F-12345", "Nguyễn Văn A");
            dtData.Rows.Add("2", "51G-67890", "Trần Văn B");
            dtData.Rows.Add("3", "60A-24680", "Lê Văn C");

            dgv.DataSource = dtData;

            if (dgv.Rows.Count > 0)
                dgv.CurrentCell = dgv.Rows[0].Cells[0];
        }

        private void ApplyMode(EditMode mode)
        {
            _mode = mode;
            bool editing = mode != EditMode.None;

            UpdateButtonState(btnThemMoi, !editing, Color.FromArgb(110, 170, 60));
            UpdateButtonState(btnCapNhat, !editing && dgv.CurrentRow != null, Color.FromArgb(70, 130, 180));
            UpdateButtonState(btnLuu, editing, Color.FromArgb(90, 90, 150));
            UpdateButtonState(btnHuy, editing, Color.FromArgb(180, 50, 50));

            if (mode == EditMode.Add)
            {
                txtBienSo.Text = string.Empty;
                txtLaiXe.Text = string.Empty;
                _originalBienSo = "";
                _originalLaiXe = "";
                txtBienSo.Focus();
            }
            else if (mode == EditMode.Edit && dgv.CurrentRow != null)
            {
                txtBienSo.Text = dgv.CurrentRow.Cells["Biển số xe"].Value.ToString();
                txtLaiXe.Text = dgv.CurrentRow.Cells["Lái xe"].Value.ToString();

                _originalBienSo = txtBienSo.Text;
                _originalLaiXe = txtLaiXe.Text;

                txtBienSo.Focus();
                txtBienSo.SelectAll();
            }
            else if (mode == EditMode.None && dgv.CurrentRow != null)
            {
                txtBienSo.Text = dgv.CurrentRow.Cells["Biển số xe"].Value.ToString();
                txtLaiXe.Text = dgv.CurrentRow.Cells["Lái xe"].Value.ToString();

                _originalBienSo = txtBienSo.Text;
                _originalLaiXe = txtLaiXe.Text;
            }
        }

        private void UpdateButtonState(IconButton btn, bool enabled, Color normalColor)
        {
            btn.BackColor = enabled ? normalColor : Color.FromArgb(200, 200, 200);
            Color inactiveColor = Color.FromArgb(120, 120, 120);
            btn.ForeColor = enabled ? Color.White : inactiveColor;
            btn.IconColor = btn.ForeColor;
        }

        private void SaveCurrent()
        {
            var bienso = (txtBienSo.Text ?? string.Empty).Trim();
            var laixe = (txtLaiXe.Text ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(bienso))
            {
                MessageBox.Show("Vui lòng nhập Biển số xe.", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBienSo.Focus();
                return;
            }

            if (_mode == EditMode.Add)
            {
                DataRow newRow = dtData.NewRow();
                newRow["Mã xe"] = (dtData.Rows.Count + 1).ToString();
                newRow["Biển số xe"] = bienso;
                newRow["Lái xe"] = laixe;
                dtData.Rows.Add(newRow);
                dgv.CurrentCell = dgv.Rows[dgv.Rows.Count - 1].Cells[0];
            }
            else if (_mode == EditMode.Edit && dgv.CurrentRow != null)
            {
                dgv.CurrentRow.Cells["Biển số xe"].Value = bienso;
                dgv.CurrentRow.Cells["Lái xe"].Value = laixe;
            }

            ApplyMode(EditMode.None);
        }
    }
}
