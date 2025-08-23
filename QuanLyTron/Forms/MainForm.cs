using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace QuanLyTron.Forms
{
    public class MainForm : Form
    {
        private ComboBox cbMac, cbBienSo;
        private TextBox txtKyHieu;
        private CheckBox chkBom;
        private Label lblVersion;
        private DataGridView dgvData;
        private TableLayoutPanel mainLayout, rightPanel;

        // --- quản lý form con ---
        private Form currentChildForm;

        public MainForm()
        {
            Text = "Quản lý tại trạm";

            // --- Chỉnh cửa sổ ---
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1200, 500);

            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle; // khóa resize

            InitializeComponents();
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
                RowCount = 2
            };
            leftPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainLayout.Controls.Add(leftPanel, 0, 0);

            // --- Bộ lọc ---
            var filterPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true
            };

            filterPanel.Controls.Add(new Label { Text = "Mác:", AutoSize = true, Padding = new Padding(5) });
            cbMac = new ComboBox { Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            filterPanel.Controls.Add(cbMac);

            filterPanel.Controls.Add(new Label { Text = "Biển số:", AutoSize = true, Padding = new Padding(5) });
            cbBienSo = new ComboBox { Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            filterPanel.Controls.Add(cbBienSo);

            filterPanel.Controls.Add(new Label { Text = "Ký hiệu:", AutoSize = true, Padding = new Padding(5) });
            txtKyHieu = new TextBox { Width = 150 };
            filterPanel.Controls.Add(txtKyHieu);

            chkBom = new CheckBox { Text = "Bơm BT", AutoSize = true, Padding = new Padding(5) };
            filterPanel.Controls.Add(chkBom);

            lblVersion = new Label { Text = "V2.1", AutoSize = true, Padding = new Padding(5) };
            filterPanel.Controls.Add(lblVersion);

            leftPanel.Controls.Add(filterPanel, 0, 0);

            // --- DataGridView ---
            dgvData = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                RowHeadersVisible = false
            };

            dgvData.Columns.Add("STT", "STT");
            dgvData.Columns.Add("KhachHang", "Khách hàng");
            dgvData.Columns.Add("KLDatHang", "KL đặt hàng");
            dgvData.Columns.Add("KLDaCap", "KL đã cấp");
            dgvData.Columns.Add("SoPhieu", "Số phiếu");
            dgvData.Columns.Add("DiaDiem", "Địa điểm CT");

            leftPanel.Controls.Add(dgvData, 0, 1);

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
            AddButton("IN PHIẾU", IconChar.Print, Color.MediumSeaGreen, 1, 1, (s, e) => OpenChildForm(new InPhieuForm()));
            AddButton("THỐNG KÊ", IconChar.ChartPie, Color.MediumPurple, 2, 0, (s, e) => OpenChildForm(new ThongKeForm()));
            AddButton("CÀI ĐẶT", IconChar.Cogs, Color.DarkSlateGray, 2, 1, (s, e) => OpenChildForm(new CaiDatForm()));

            // ===== Nút đặc biệt (tròn) =====
            AddCircleIconButton(IconChar.Wifi, Color.DodgerBlue, 3, 0, (s, e) =>
            {
                MessageBox.Show("Đang kết nối mạng...", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void AddCircleIconButton(IconChar icon, Color backColor, int row, int col, EventHandler onClick)
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

            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(backColor);
            btn.MouseLeave += (s, e) => btn.BackColor = backColor;

            btn.Paint += (s, e) =>
            {
                System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
                gp.AddEllipse(0, 0, btn.Width - 1, btn.Height - 1);
                btn.Region = new Region(gp);
            };

            rightPanel.Controls.Add(btn, col, row);
        }
    }
}
