using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

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
            this.Width = 1000;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;

            // 🎨 Nền vàng nhạt cho toàn bộ form
            this.BackColor = Color.FromArgb(255, 250, 230);

            // ====== SplitContainer chia đôi trên ======
            SplitContainer splitTop = new SplitContainer
            {
                Dock = DockStyle.Top,
                Height = 300,
                SplitterDistance = 650
            };

            // Bảng dữ liệu bên trái
            dgvCapPhoi = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvCapPhoi.Columns.Add("STT", "STT");
            dgvCapPhoi.Columns.Add("MacBT", "Mác BT");
            dgvCapPhoi.Columns.Add("CuongDo", "Cường Độ");
            dgvCapPhoi.Columns.Add("CotLieuMax", "Cốt Liệu Max");
            dgvCapPhoi.Columns.Add("DoSut", "Độ sụt");

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

            // Panel nền xám bên phải
            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightGray
            };
            splitTop.Panel2.Controls.Add(rightPanel);

            // ====== Panel dưới chứa nhập liệu và nút ======
            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(255, 250, 230) // vàng nhạt
            };

            int labelLeft = 20;
            int textLeft = 140;
            int topBase = 20;
            int lineHeight = 40;

            lblSTT = new Label { Text = "STT:", Left = labelLeft, Top = topBase, Width = 100 };
            txtSTT = new TextBox { Left = textLeft, Top = topBase, Width = 200 };

            lblMacBT = new Label { Text = "Mác BT:", Left = labelLeft, Top = topBase + lineHeight, Width = 100 };
            txtMacBT = new TextBox { Left = textLeft, Top = topBase + lineHeight, Width = 200 };

            lblCuongDo = new Label { Text = "Cường Độ:", Left = labelLeft, Top = topBase + lineHeight * 2, Width = 100 };
            txtCuongDo = new TextBox { Left = textLeft, Top = topBase + lineHeight * 2, Width = 200 };

            lblCotLieuMax = new Label { Text = "Cốt Liệu Max:", Left = labelLeft, Top = topBase + lineHeight * 3, Width = 100 };
            txtCotLieuMax = new TextBox { Left = textLeft, Top = topBase + lineHeight * 3, Width = 200 };

            lblDoSut = new Label { Text = "Độ Sụt:", Left = labelLeft, Top = topBase + lineHeight * 4, Width = 100 };
            txtDoSut = new TextBox { Left = textLeft, Top = topBase + lineHeight * 4, Width = 200 };

            // Tổng khối lượng (readonly)
            lblTongKhoiLuong = new Label { Text = "Tổng khối lượng:", Left = 450, Top = topBase, Width = 120 };
            txtTongKhoiLuong = new TextBox
            {
                Left = 580,
                Top = topBase,
                Width = 200,
                ReadOnly = true,
                BackColor = Color.White
            };

            // Buttons
            btnThemMoi = new Button
            {
                Left = 450,
                Top = topBase + lineHeight * 2,
                Width = 150,
                Height = 40,
                Text = "➕ THÊM MỚI",
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnThemMoi.Click += (s, e) => MessageBox.Show("Thêm mới!");

            btnCapNhat = new Button
            {
                Left = 620,
                Top = topBase + lineHeight * 2,
                Width = 150,
                Height = 40,
                Text = "💾 CẬP NHẬT",
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnCapNhat.Click += (s, e) => MessageBox.Show("Cập nhật!");

            bottomPanel.Controls.AddRange(new Control[]
            {
                lblSTT, txtSTT,
                lblMacBT, txtMacBT,
                lblCuongDo, txtCuongDo,
                lblCotLieuMax, txtCotLieuMax,
                lblDoSut, txtDoSut,
                lblTongKhoiLuong, txtTongKhoiLuong,
                btnThemMoi, btnCapNhat
            });

            // Add vào form
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
