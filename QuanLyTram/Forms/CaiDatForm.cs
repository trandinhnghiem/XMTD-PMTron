using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyTram.Forms
{
    public class CaiDatForm : Form
    {
        private Button btnThemMoi, btnCapNhat, btnLuu;
        private TextBox txtMaTram, txtTenTram, txtDiaDiem, txtSoDienThoai, txtCongSuat;
        private RadioButton rdTonTai, rdPhoiMe, rdPhoiChuan;
        private DataGridView dgvTram;

        public CaiDatForm()
        {
            this.Text = "CÀI ĐẶT HỆ THỐNG";
            this.Size = new Size(1250, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Beige;

            Font btnFont = new Font("Segoe UI", 10, FontStyle.Bold);

            // ===== Nút chức năng =====
            btnThemMoi = new Button()
            {
                Text = " THÊM MỚI",
                Width = 150,
                Height = 50,
                BackColor = Color.LightGreen,
                Font = btnFont,
                FlatStyle = FlatStyle.Standard,
                Image = SystemIcons.Application.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText
            };

            btnCapNhat = new Button()
            {
                Text = " CẬP NHẬT",
                Width = 150,
                Height = 50,
                BackColor = Color.Khaki,
                Font = btnFont,
                FlatStyle = FlatStyle.Standard,
                Image = SystemIcons.Information.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText
            };

            btnLuu = new Button()
            {
                Text = " LƯU",
                Width = 150,
                Height = 50,
                BackColor = Color.LightSkyBlue,
                Font = btnFont,
                FlatStyle = FlatStyle.Standard,
                Image = SystemIcons.Shield.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText
            };

            // ===== Panel chứa 2 GroupBox trên =====
            Panel topPanel = new Panel()
            {
                Location = new Point(20, 20),
                Size = new Size(1180, 180)
            };

            // ===== GroupBox Thông tin trạm (trái) =====
            GroupBox groupInfoLeft = new GroupBox()
            {
                Text = "THÔNG TIN TRẠM",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(580, 180),
                Location = new Point(0, 0)
            };

            Label lblMaTram = new Label() { Text = "Mã trạm:", Location = new Point(20, 30), AutoSize = true };
            txtMaTram = new TextBox() { Location = new Point(120, 25), Width = 200 };

            Label lblTenTram = new Label() { Text = "Tên trạm:", Location = new Point(20, 60), AutoSize = true };
            txtTenTram = new TextBox() { Location = new Point(120, 55), Width = 200 };

            Label lblDiaDiem = new Label() { Text = "Địa điểm:", Location = new Point(20, 90), AutoSize = true };
            txtDiaDiem = new TextBox() { Location = new Point(120, 85), Width = 200 };

            groupInfoLeft.Controls.AddRange(new Control[] { lblMaTram, txtMaTram, lblTenTram, txtTenTram, lblDiaDiem, txtDiaDiem });

            // ===== GroupBox Đường dẫn chương trình (phải) =====
            GroupBox groupInfoRight = new GroupBox()
            {
                Text = "ĐƯỜNG DẪN CHƯƠNG TRÌNH",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(580, 180),
                Location = new Point(600, 0)
            };

            Label lblSoDienThoai = new Label() { Text = "Số điện thoại:", Location = new Point(20, 30), AutoSize = true };
            txtSoDienThoai = new TextBox() { Location = new Point(140, 25), Width = 250 };

            Label lblCongSuat = new Label() { Text = "Công suất:", Location = new Point(20, 60), AutoSize = true };
            txtCongSuat = new TextBox() { Location = new Point(140, 55), Width = 250 };

            rdTonTai = new RadioButton() { Text = "Trạm đang tồn tại", Location = new Point(20, 90), AutoSize = true };
            rdPhoiMe = new RadioButton() { Text = "Theo cấp phối từng mẻ", Location = new Point(20, 120), AutoSize = true };
            rdPhoiChuan = new RadioButton() { Text = "Theo cấp phối chuẩn", Location = new Point(20, 150), AutoSize = true };

            groupInfoRight.Controls.AddRange(new Control[] { lblSoDienThoai, txtSoDienThoai, lblCongSuat, txtCongSuat, rdTonTai, rdPhoiMe, rdPhoiChuan });

            topPanel.Controls.AddRange(new Control[] { groupInfoLeft, groupInfoRight });
            this.Controls.Add(topPanel);

            // ===== Label tiêu đề DataGridView =====
            Label dgvTitle = new Label()
            {
                Text = "DANH SÁCH TRẠM TRỘN",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 220),
                AutoSize = true
            };
            this.Controls.Add(dgvTitle);

            // ===== DataGridView =====
            dgvTram = new DataGridView()
            {
                Location = new Point(20, 250),
                Size = new Size(1180, 300),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(dgvTram);

            // Thêm dữ liệu demo
            DataTable dt = new DataTable();
            dt.Columns.Add("Mã trạm");
            dt.Columns.Add("Tên trạm");
            dt.Columns.Add("Địa điểm");
            dt.Columns.Add("Số điện thoại");
            dt.Columns.Add("Công suất");

            // Danh sách 4 trạm 
            dt.Rows.Add("T01", "90m3 - T 90 Đặng Tài - Hậu Giang", "0909123456", "Hậu Giang", "90");
            dt.Rows.Add("T02", "82m3 - T 82 Xe kíp - M", "0912345678", "TP.HCM", "82");
            dt.Rows.Add("T03", "150m3 - T 150 - Ô Môn 1", "0913456789", "Cần Thơ", "150");
            dt.Rows.Add("T04", "150m3 - T 150 - Ô Môn 2", "0914567890", "Cần Thơ", "150");


            dgvTram.DataSource = dt;

            // ===== Nút chức năng dưới DataGridView =====
            btnThemMoi.Location = new Point(20, 570);
            btnCapNhat.Location = new Point(190, 570);
            btnLuu.Location = new Point(360, 570);

            this.Controls.AddRange(new Control[] { btnThemMoi, btnCapNhat, btnLuu });
        }
    }
}
