using System;
using System.Data;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace QuanLyTram.Forms
{
    public class CapPhoiForm : Form
    {
        private DataGridView dgvCapPhoi;
        private TextBox txtSTT, txtMacBT, txtCuongDo, txtCotLieuMax, txtDoSut, txtTongKhoiLuong;
        private Label lblSTT, lblMacBT, lblCuongDo, lblCotLieuMax, lblDoSut, lblTongKhoiLuong;
        private Button btnThemMoi, btnCapNhat;

        public CapPhoiForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "QUẢN LÝ CẤP PHỐI";
            this.Width = 1200;
            this.Height = 720;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(255, 250, 230);

            // ====== SplitContainer ======
            SplitContainer splitTop = new SplitContainer
            {
                Dock = DockStyle.Top,
                Height = 350,
                Orientation = Orientation.Vertical,
                IsSplitterFixed = false,
                BackColor = Color.White
            };

            this.Load += (s, e) =>
            {
                splitTop.SplitterDistance = splitTop.Width / 2;
            };

            // ====== DataGridView ======
            dgvCapPhoi = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToResizeColumns = true,
                AllowUserToResizeRows = true,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                GridColor = Color.LightGray,
                RowHeadersVisible = false
            };

            dgvCapPhoi.Columns.Add("STT", "STT");
            dgvCapPhoi.Columns.Add("MacBT", "Mác BT");
            dgvCapPhoi.Columns.Add("CuongDo", "Cường Độ");
            dgvCapPhoi.Columns.Add("CotLieuMax", "Cốt Liệu Max");
            dgvCapPhoi.Columns.Add("DoSut", "Độ sụt");

            // Dữ liệu mẫu
            dgvCapPhoi.Rows.Add("1", "C30R28-10±2", "25", "20", "10±2");
            dgvCapPhoi.Rows.Add("2", "C20R28-12±2", "20", "16", "12±2");
            dgvCapPhoi.Rows.Add("3", "C25R28-14±2", "22", "18", "14±2");
            dgvCapPhoi.Rows.Add("4", "C35R28-10±2", "28", "22", "10±2");
            dgvCapPhoi.Rows.Add("5", "C40R28-16±2", "30", "24", "16±2");
            dgvCapPhoi.Rows.Add("6", "C25R14-12±2", "21", "17", "12±2");
            dgvCapPhoi.Rows.Add("7", "C30R14-10±2", "24", "19", "10±2");
            dgvCapPhoi.Rows.Add("8", "C20R14-14±2", "18", "15", "14±2");

            dgvCapPhoi.CellClick += DgvCapPhoi_CellClick;
            splitTop.Panel1.Controls.Add(dgvCapPhoi);

            // ====== Panel bên phải ======
            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightGray
            };
            splitTop.Panel2.Controls.Add(rightPanel);

            // ====== Panel dưới ======
            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(255, 250, 230)
            };

            int labelLeft = 50;          // vị trí label bên trái
            int textLeft = 200;          // textbox bên trái
            int rightLabelLeft = 650;    // label bên phải
            int rightTextLeft = 850;     // textbox bên phải
            int textWidth = 280;
            int controlHeight = 40;
            int paddingY = 20;
            Font labelFont = new Font("Segoe UI", 12, FontStyle.Bold);
            Font textFont = new Font("Segoe UI", 12);

            // ====== Cột trái ======
            lblSTT = new Label { Text = "STT:", Font = labelFont, Left = labelLeft, Top = 40, AutoSize = true, TextAlign = ContentAlignment.TopLeft };
            txtSTT = new TextBox { Font = textFont, Left = textLeft, Top = lblSTT.Top, Width = textWidth, Height = controlHeight };

            lblMacBT = new Label { Text = "Mác BT:", Font = labelFont, Left = labelLeft, Top = lblSTT.Bottom + paddingY, AutoSize = true, TextAlign = ContentAlignment.TopLeft };
            txtMacBT = new TextBox { Font = textFont, Left = textLeft, Top = lblMacBT.Top, Width = textWidth, Height = controlHeight };

            lblCuongDo = new Label { Text = "Cường Độ:", Font = labelFont, Left = labelLeft, Top = lblMacBT.Bottom + paddingY, AutoSize = true, TextAlign = ContentAlignment.TopLeft };
            txtCuongDo = new TextBox { Font = textFont, Left = textLeft, Top = lblCuongDo.Top, Width = textWidth, Height = controlHeight };

            // ====== Cột phải ======
            lblCotLieuMax = new Label { Text = "Cốt Liệu Max:", Font = labelFont, Left = rightLabelLeft, Top = 40, AutoSize = true, TextAlign = ContentAlignment.TopLeft };
            txtCotLieuMax = new TextBox { Font = textFont, Left = rightTextLeft, Top = lblCotLieuMax.Top, Width = textWidth, Height = controlHeight };

            lblDoSut = new Label { Text = "Độ Sụt:", Font = labelFont, Left = rightLabelLeft, Top = lblCotLieuMax.Bottom + paddingY, AutoSize = true, TextAlign = ContentAlignment.TopLeft };
            txtDoSut = new TextBox { Font = textFont, Left = rightTextLeft, Top = lblDoSut.Top, Width = textWidth, Height = controlHeight };

            lblTongKhoiLuong = new Label { Text = "Tổng khối lượng:", Font = labelFont, Left = rightLabelLeft, Top = lblDoSut.Bottom + paddingY, AutoSize = true, TextAlign = ContentAlignment.TopLeft };
            txtTongKhoiLuong = new TextBox { Font = textFont, Left = rightTextLeft, Top = lblTongKhoiLuong.Top, Width = textWidth, Height = controlHeight, ReadOnly = true };

            // Nút bấm THÊM MỚI
            btnThemMoi = new IconButton
            {
                Width = 180,
                Height = 45,
                Text = "THÊM MỚI",
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                IconChar = IconChar.PlusCircle,     // icon FontAwesome
                IconColor = Color.White,
                IconSize = 24,
                TextImageRelation = TextImageRelation.ImageBeforeText // icon nằm trước chữ
            };

            // Nút bấm CẬP NHẬT
            btnCapNhat = new IconButton
            {
                Width = 180,
                Height = 45,
                Text = "CẬP NHẬT",
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                IconChar = IconChar.Save,           // icon FontAwesome
                IconColor = Color.White,
                IconSize = 24,
                TextImageRelation = TextImageRelation.ImageBeforeText
            };

            btnThemMoi.Top = 250;
            btnCapNhat.Top = 250;
            btnThemMoi.Left = (bottomPanel.Width / 2) - btnThemMoi.Width - 20;
            btnCapNhat.Left = (bottomPanel.Width / 2) + 20;

            bottomPanel.Resize += (s, e) =>
            {
                btnThemMoi.Left = (bottomPanel.Width / 2) - btnThemMoi.Width - 20;
                btnCapNhat.Left = (bottomPanel.Width / 2) + 20;
            };

            bottomPanel.Controls.AddRange(new Control[]
            {
                lblSTT, txtSTT, lblMacBT, txtMacBT, lblCuongDo, txtCuongDo,
                lblCotLieuMax, txtCotLieuMax, lblDoSut, txtDoSut,
                lblTongKhoiLuong, txtTongKhoiLuong,
                btnThemMoi, btnCapNhat
            });

            this.Controls.Add(bottomPanel);
            this.Controls.Add(splitTop);
        }

        private void DgvCapPhoi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCapPhoi.Rows[e.RowIndex];
                txtSTT.Text = row.Cells["STT"].Value?.ToString() ?? "";
                txtMacBT.Text = row.Cells["MacBT"].Value?.ToString() ?? "";
                txtCuongDo.Text = row.Cells["CuongDo"].Value?.ToString() ?? "";
                txtCotLieuMax.Text = row.Cells["CotLieuMax"].Value?.ToString() ?? "";
                txtDoSut.Text = row.Cells["DoSut"].Value?.ToString() ?? "";
            }
        }
    }
}
