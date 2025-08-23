using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace QuanLyTron.Forms
{
    public class DM_PhuGiaForm : Form
    {
        private enum EditMode { None, Add, Edit }
        private EditMode _mode = EditMode.None;

        private Panel pnlActions;
        private IconButton btnThemMoi, btnCapNhat;
        private IconButton btnLuu, btnHuy;

        private DataGridView dgv;

        private GroupBox grpInfo;
        private Label lblTenPhuGia, lblGhiChu;
        private TextBox txtTenPhuGia, txtGhiChu;

        private Color BgPink = Color.FromArgb(245, 225, 223);
        private Color PanelWhite = Color.FromArgb(245, 245, 255);
        private Font FTitle => new Font(new FontFamily("Segoe UI"), 11.5f, FontStyle.Bold);
        private Font FText => new Font(new FontFamily("Segoe UI"), 10f, FontStyle.Regular);

        private DataTable dtData; 

        private string _originalTenPhuGia = "";
        private string _originalGhiChu = "";

        public DM_PhuGiaForm()
        {
            Text = "PHỤ GIA";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1200, 720);
            BackColor = BgPink;

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
                BackColor = BgPink
            };
            Controls.Add(main);
            main.BringToFront();

            var lblGridTitle = new Label
            {
                Text = "DỮ LIỆU PHỤ GIA",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.Red,
                AutoSize = true,
                Location = new Point(20, 8)
            };
            main.Controls.Add(lblGridTitle);

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
                RowHeadersVisible = true
            };
            main.Controls.Add(dgv);

            grpInfo = new GroupBox
            {
                Text = "THÔNG TIN PHỤ GIA",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.Red,
                BackColor = Color.Transparent,
                Size = new Size(400, 560),
                Location = new Point(dgv.Right + 20, 8),
                Padding = new Padding(18)
            };
            main.Controls.Add(grpInfo);

            lblTenPhuGia = new Label
            {
                Text = "Tên phụ gia:",
                Font = new Font(FText.FontFamily, FText.Size, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 50),
                ForeColor = Color.Black
            };
            txtTenPhuGia = new TextBox
            {
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Location = new Point(20, 75),
                Width = 340
            };

            lblGhiChu = new Label
            {
                Text = "Ghi chú:",
                Font = new Font(FText.FontFamily, FText.Size, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 130),
                ForeColor = Color.Black
            };
            txtGhiChu = new TextBox
            {
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Location = new Point(20, 155),
                Width = 340
            };

            btnLuu = new IconButton
            {
                Text = " LƯU",
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                BackColor = Color.LightSkyBlue,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(160, 46),
                Location = new Point(20, 220),
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
                Location = new Point(btnLuu.Right + 16, 220),
                IconChar = IconChar.TimesCircle,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Padding = new Padding(28, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnHuy.FlatAppearance.BorderSize = 0;

            grpInfo.Controls.Add(lblTenPhuGia);
            grpInfo.Controls.Add(txtTenPhuGia);
            grpInfo.Controls.Add(lblGhiChu);
            grpInfo.Controls.Add(txtGhiChu);
            grpInfo.Controls.Add(btnLuu);
            grpInfo.Controls.Add(btnHuy);
        }

        private void WireEvents()
        {
            btnThemMoi.Click += (s, e) =>
            {
                ApplyMode(EditMode.Add);
                txtTenPhuGia.Focus();
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
                if (txtTenPhuGia.Text != _originalTenPhuGia || txtGhiChu.Text != _originalGhiChu)
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
                    txtTenPhuGia.Text = dgv.CurrentRow.Cells["Tên phụ gia"].Value.ToString();
                    txtGhiChu.Text = dgv.CurrentRow.Cells["Ghi chú"].Value.ToString();
                }
            };
        }

        private void InitData()
        {
            dtData = new DataTable();
            dtData.Columns.Add("Mã phụ gia");
            dtData.Columns.Add("Tên phụ gia");
            dtData.Columns.Add("Ghi chú");

            dtData.Rows.Add("1", "Phụ gia siêu dẻo", "Tăng độ chảy, giảm nước khi trộn bê tông");
            dtData.Rows.Add("2", "Phụ gia chậm đông kết", "Kéo dài thời gian ninh kết, phù hợp công trình lớn");
            dtData.Rows.Add("3", "Phụ gia chống thấm", "Tăng khả năng chống thấm cho bê tông và vữa");
            dtData.Rows.Add("4", "Phụ gia khoáng hoạt tính", "Cải thiện cường độ và độ bền lâu dài");
            dtData.Rows.Add("5", "Phụ gia cuốn khí", "Tăng khả năng chống nứt và chống băng giá");
            dtData.Rows.Add("6", "Phụ gia tăng nhanh đông kết", "Rút ngắn thời gian ninh kết, thi công nhanh");
            dtData.Rows.Add("7", "Phụ gia giảm co ngót", "Giảm hiện tượng nứt do co ngót");
            dtData.Rows.Add("8", "Phụ gia khoáng mịn (Silica Fume)", "Tăng độ đặc chắc, chống xâm thực hóa chất");


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
                txtTenPhuGia.Text = string.Empty;
                txtGhiChu.Text = string.Empty;
                _originalTenPhuGia = "";
                _originalGhiChu = "";
                txtTenPhuGia.Focus();
            }
            else if (mode == EditMode.Edit && dgv.CurrentRow != null)
            {
                txtTenPhuGia.Text = dgv.CurrentRow.Cells["Tên phụ gia"].Value.ToString();
                txtGhiChu.Text = dgv.CurrentRow.Cells["Ghi chú"].Value.ToString();

                _originalTenPhuGia = txtTenPhuGia.Text;
                _originalGhiChu = txtGhiChu.Text;

                txtTenPhuGia.Focus();
                txtTenPhuGia.SelectAll();
            }
            else if (mode == EditMode.None && dgv.CurrentRow != null)
            {
                txtTenPhuGia.Text = dgv.CurrentRow.Cells["Tên phụ gia"].Value.ToString();
                txtGhiChu.Text = dgv.CurrentRow.Cells["Ghi chú"].Value.ToString();

                _originalTenPhuGia = txtTenPhuGia.Text;
                _originalGhiChu = txtGhiChu.Text;
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
            var tenPG = (txtTenPhuGia.Text ?? string.Empty).Trim();
            var ghiChu = (txtGhiChu.Text ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(tenPG))
            {
                MessageBox.Show("Vui lòng nhập Tên phụ gia.", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenPhuGia.Focus();
                return;
            }

            if (_mode == EditMode.Add)
            {
                DataRow newRow = dtData.NewRow();
                newRow["Mã phụ gia"] = (dtData.Rows.Count + 1).ToString();
                newRow["Tên phụ gia"] = tenPG;
                newRow["Ghi chú"] = ghiChu;
                dtData.Rows.Add(newRow);
                dgv.CurrentCell = dgv.Rows[dgv.Rows.Count - 1].Cells[0];
            }
            else if (_mode == EditMode.Edit && dgv.CurrentRow != null)
            {
                dgv.CurrentRow.Cells["Tên phụ gia"].Value = tenPG;
                dgv.CurrentRow.Cells["Ghi chú"].Value = ghiChu;
            }

            ApplyMode(EditMode.None);
        }
    }
}
