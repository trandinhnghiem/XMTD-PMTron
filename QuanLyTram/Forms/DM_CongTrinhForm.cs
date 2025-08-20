using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace QuanLyTram.Forms
{
    public class DM_CongTrinhForm : Form
    {
        private enum EditMode { None, Add, Edit }
        private EditMode _mode = EditMode.None;

        private Panel pnlActions;
        private IconButton btnThemMoi, btnCapNhat;
        private IconButton btnLuu, btnHuy;

        private DataGridView dgv;

        private GroupBox grpInfo;
        private Label lblDiaDiem, lblHangMuc, lblThietBi;
        private TextBox txtDiaDiem, txtHangMuc, txtThietBi;

        private Color BgBeige = Color.FromArgb(245, 245, 220); 
        private Color PanelWhite = Color.FromArgb(245, 245, 255);
        private Font FTitle => new Font(new FontFamily("Segoe UI"), 11.5f, FontStyle.Bold);
        private Font FText => new Font(new FontFamily("Segoe UI"), 10f, FontStyle.Regular);

        private DataTable dtData; 

        // Biến lưu dữ liệu gốc để so sánh khi Hủy
        private string _originalDiaDiem = "";
        private string _originalHangMuc = "";
        private string _originalThietBi = "";

        public DM_CongTrinhForm()
        {
            Text = "CÔNG TRÌNH";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1200, 720);
            BackColor = BgBeige;

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
                BackColor = BgBeige
            };
            Controls.Add(main);
            main.BringToFront();

            var lblGridTitle = new Label
            {
                Text = "DỮ LIỆU CÔNG TRÌNH",
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
                Text = "THÔNG TIN CÔNG TRÌNH",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.Red,
                BackColor = Color.Transparent,
                Size = new Size(400, 560),
                Location = new Point(dgv.Right + 20, 8),
                Padding = new Padding(18)
            };
            main.Controls.Add(grpInfo);

            lblDiaDiem = new Label
            {
                Text = "Địa điểm công trình:",
                Font = new Font(FText.FontFamily, FText.Size, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 50),
                ForeColor = Color.Black
            };
            txtDiaDiem = new TextBox
            {
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Location = new Point(20, 75),
                Width = 340
            };

            lblHangMuc = new Label
            {
                Text = "Hạng mục:",
                Font = new Font(FText.FontFamily, FText.Size, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 130),
                ForeColor = Color.Black
            };
            txtHangMuc = new TextBox
            {
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Location = new Point(20, 155),
                Width = 340
            };

            lblThietBi = new Label
            {
                Text = "Thiết bị bơm:",
                Font = new Font(FText.FontFamily, FText.Size, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 210),
                ForeColor = Color.Black
            };
            txtThietBi = new TextBox
            {
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Location = new Point(20, 235),
                Width = 340
            };

            btnLuu = new IconButton
            {
                Text = " LƯU",
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                BackColor = Color.LightSkyBlue,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(160, 46),
                Location = new Point(20, 300),
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
                Location = new Point(btnLuu.Right + 16, 300),
                IconChar = IconChar.TimesCircle,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Padding = new Padding(28, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnHuy.FlatAppearance.BorderSize = 0;

            grpInfo.Controls.Add(lblDiaDiem);
            grpInfo.Controls.Add(txtDiaDiem);
            grpInfo.Controls.Add(lblHangMuc);
            grpInfo.Controls.Add(txtHangMuc);
            grpInfo.Controls.Add(lblThietBi);
            grpInfo.Controls.Add(txtThietBi);
            grpInfo.Controls.Add(btnLuu);
            grpInfo.Controls.Add(btnHuy);
        }

        private void WireEvents()
        {
            btnThemMoi.Click += (s, e) =>
            {
                ApplyMode(EditMode.Add);
                txtDiaDiem.Focus();
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
                if (txtDiaDiem.Text != _originalDiaDiem || txtHangMuc.Text != _originalHangMuc || txtThietBi.Text != _originalThietBi)
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
                    txtDiaDiem.Text = dgv.CurrentRow.Cells["Địa điểm"].Value.ToString();
                    txtHangMuc.Text = dgv.CurrentRow.Cells["Hạng mục"].Value.ToString();
                    txtThietBi.Text = dgv.CurrentRow.Cells["Thiết bị bơm"].Value.ToString();
                }
            };
        }

        private void InitData()
        {
            dtData = new DataTable();
            dtData.Columns.Add("Mã công trình");
            dtData.Columns.Add("Địa điểm");
            dtData.Columns.Add("Hạng mục");
            dtData.Columns.Add("Thiết bị bơm");

            dtData.Rows.Add("1", "Công trình A", "Xây dựng hạ tầng", "Bơm nước 3HP");
            dtData.Rows.Add("2", "Công trình B", "Lắp đặt điện", "Bơm công nghiệp 5HP");
            dtData.Rows.Add("3", "Công trình C", "Thi công PCCC", "Bơm cứu hỏa");

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
                txtDiaDiem.Text = string.Empty;
                txtHangMuc.Text = string.Empty;
                txtThietBi.Text = string.Empty;
                _originalDiaDiem = "";
                _originalHangMuc = "";
                _originalThietBi = "";
                txtDiaDiem.Focus();
            }
            else if (mode == EditMode.Edit && dgv.CurrentRow != null)
            {
                txtDiaDiem.Text = dgv.CurrentRow.Cells["Địa điểm"].Value.ToString();
                txtHangMuc.Text = dgv.CurrentRow.Cells["Hạng mục"].Value.ToString();
                txtThietBi.Text = dgv.CurrentRow.Cells["Thiết bị bơm"].Value.ToString();

                _originalDiaDiem = txtDiaDiem.Text;
                _originalHangMuc = txtHangMuc.Text;
                _originalThietBi = txtThietBi.Text;

                txtDiaDiem.Focus();
                txtDiaDiem.SelectAll();
            }
            else if (mode == EditMode.None && dgv.CurrentRow != null)
            {
                txtDiaDiem.Text = dgv.CurrentRow.Cells["Địa điểm"].Value.ToString();
                txtHangMuc.Text = dgv.CurrentRow.Cells["Hạng mục"].Value.ToString();
                txtThietBi.Text = dgv.CurrentRow.Cells["Thiết bị bơm"].Value.ToString();

                _originalDiaDiem = txtDiaDiem.Text;
                _originalHangMuc = txtHangMuc.Text;
                _originalThietBi = txtThietBi.Text;
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
            var diadiem = (txtDiaDiem.Text ?? string.Empty).Trim();
            var hangmuc = (txtHangMuc.Text ?? string.Empty).Trim();
            var thietbi = (txtThietBi.Text ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(diadiem))
            {
                MessageBox.Show("Vui lòng nhập Địa điểm công trình.", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaDiem.Focus();
                return;
            }

            if (_mode == EditMode.Add)
            {
                DataRow newRow = dtData.NewRow();
                newRow["Mã công trình"] = (dtData.Rows.Count + 1).ToString();
                newRow["Địa điểm"] = diadiem;
                newRow["Hạng mục"] = hangmuc;
                newRow["Thiết bị bơm"] = thietbi;
                dtData.Rows.Add(newRow);
                dgv.CurrentCell = dgv.Rows[dgv.Rows.Count - 1].Cells[0];
            }
            else if (_mode == EditMode.Edit && dgv.CurrentRow != null)
            {
                dgv.CurrentRow.Cells["Địa điểm"].Value = diadiem;
                dgv.CurrentRow.Cells["Hạng mục"].Value = hangmuc;
                dgv.CurrentRow.Cells["Thiết bị bơm"].Value = thietbi;
            }

            ApplyMode(EditMode.None);
        }
    }
}
