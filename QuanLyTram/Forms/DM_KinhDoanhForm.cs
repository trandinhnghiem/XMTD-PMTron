using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace QuanLyTram.Forms
{
    public class DM_KinhDoanhForm : Form
    {
        private enum EditMode { None, Add, Edit }
        private EditMode _mode = EditMode.None;

        private Panel pnlActions;
        private IconButton btnThemMoi, btnCapNhat;
        private IconButton btnLuu, btnHuy;

        private DataGridView dgv;

        private GroupBox grpInfo;
        private Label lblTenKD;
        private TextBox txtTenKD;

        private Color BgGreen = Color.FromArgb(220, 245, 220); 
        private Color PanelWhite = Color.FromArgb(245, 245, 255);
        private Font FTitle => new Font(new FontFamily("Segoe UI"), 11.5f, FontStyle.Bold);
        private Font FText => new Font(new FontFamily("Segoe UI"), 10f, FontStyle.Regular);

        private DataTable dtData; 

        // Biến lưu dữ liệu gốc để so sánh khi Hủy
        private string _originalTenKD = "";

        public DM_KinhDoanhForm()
        {
            Text = "KINH DOANH";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1000, 600);
            BackColor = BgGreen;

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
                BackColor = BgGreen
            };
            Controls.Add(main);
            main.BringToFront();

            var lblGridTitle = new Label
            {
                Text = "DỮ LIỆU KINH DOANH",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.Red,
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
                Text = "THÔNG TIN KINH DOANH",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.Red,
                BackColor = Color.Transparent,
                Size = new Size(400, 560),
                Location = new Point(dgv.Right + 16, 8),
                Padding = new Padding(18)
            };
            main.Controls.Add(grpInfo);

            lblTenKD = new Label
            {
                Text = "Tên kinh doanh:",
                Font = new Font(FText.FontFamily, FText.Size, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 50),
                ForeColor = Color.Black
            };
            txtTenKD = new TextBox
            {
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Location = new Point(20, 75),
                Width = 340
            };

            btnLuu = new IconButton
            {
                Text = " LƯU",
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                BackColor = Color.LightSkyBlue,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(160, 46),
                Location = new Point(20, 140),
                IconChar = IconChar.Save,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
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
                Location = new Point(btnLuu.Right + 16, 140),
                IconChar = IconChar.TimesCircle,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Padding = new Padding(28, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnHuy.FlatAppearance.BorderSize = 0;

            grpInfo.Controls.Add(lblTenKD);
            grpInfo.Controls.Add(txtTenKD);
            grpInfo.Controls.Add(btnLuu);
            grpInfo.Controls.Add(btnHuy);
        }

        private void WireEvents()
        {
            btnThemMoi.Click += (s, e) =>
            {
                ApplyMode(EditMode.Add);
                txtTenKD.Focus();
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
                if (txtTenKD.Text != _originalTenKD)
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
                    txtTenKD.Text = dgv.CurrentRow.Cells["Tên kinh doanh"].Value.ToString();
                }
            };
        }

        private void InitData()
        {
            dtData = new DataTable();
            dtData.Columns.Add("Mã kinh doanh");
            dtData.Columns.Add("Tên kinh doanh");

            dtData.Rows.Add("1", "Kinh doanh A");
            dtData.Rows.Add("2", "Kinh doanh B");
            dtData.Rows.Add("3", "Kinh doanh C");

            dgv.DataSource = dtData;

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
            UpdateButtonState(btnLuu, editing, Color.FromArgb(90, 90, 150));
            UpdateButtonState(btnHuy, editing, Color.FromArgb(180, 50, 50));

            if (mode == EditMode.Add)
            {
                txtTenKD.Text = string.Empty;
                _originalTenKD = "";
                txtTenKD.Focus();
            }
            else if (mode == EditMode.Edit && dgv.CurrentRow != null)
            {
                txtTenKD.Text = dgv.CurrentRow.Cells["Tên kinh doanh"].Value.ToString();
                _originalTenKD = txtTenKD.Text;
                txtTenKD.Focus();
                txtTenKD.SelectAll();
            }
            else if (mode == EditMode.None && dgv.CurrentRow != null)
            {
                txtTenKD.Text = dgv.CurrentRow.Cells["Tên kinh doanh"].Value.ToString();
                _originalTenKD = txtTenKD.Text;
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
            var tenkd = (txtTenKD.Text ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(tenkd))
            {
                MessageBox.Show("Vui lòng nhập Tên kinh doanh.", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKD.Focus();
                return;
            }

            if (_mode == EditMode.Add)
            {
                DataRow newRow = dtData.NewRow();
                newRow["Mã kinh doanh"] = (dtData.Rows.Count + 1).ToString();
                newRow["Tên kinh doanh"] = tenkd;
                dtData.Rows.Add(newRow);
                dgv.CurrentCell = dgv.Rows[dgv.Rows.Count - 1].Cells[0];
            }
            else if (_mode == EditMode.Edit && dgv.CurrentRow != null)
            {
                dgv.CurrentRow.Cells["Tên kinh doanh"].Value = tenkd;
            }

            ApplyMode(EditMode.None);
        }
    }
}
