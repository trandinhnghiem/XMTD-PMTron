using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace QuanLyTron.Forms
{
    public class CaiDat_VatLieuForm : Form
    {
        private DataGridView grid;
        private TextBox txtSoThuTu, txtTenCua, txtHeSoQuyDoi, txtDonViKhac, txtTramTron;
        private ComboBox cboLoaiVatLieu;
        private Button btnLuu;

        public CaiDat_VatLieuForm()
        {
            BackColor = Color.WhiteSmoke;
            Font = new Font("Segoe UI", 10);

            // ===== Thanh công cụ =====
            var toolbar = new Panel { Height = 56, Dock = DockStyle.Top, BackColor = Color.White };

            // Bỏ ? khỏi EventHandler
            IconButton BigBtn(string text, IconChar icon, Color backColor, EventHandler onClick)
            {
                var b = new IconButton
                {
                    Text = text,
                    IconChar = icon,
                    IconColor = Color.White,
                    IconSize = 22,
                    TextImageRelation = TextImageRelation.ImageBeforeText,
                    Size = new Size(180, 46),
                    BackColor = backColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold)
                };
                b.FlatAppearance.BorderSize = 0;
                if (onClick != null) b.Click += onClick;
                return b;
            }

            var btnThemMoi = BigBtn("THÊM MỚI", IconChar.PlusCircle, Color.MediumSeaGreen, null);
            var btnCapNhat = BigBtn("CẬP NHẬT", IconChar.SyncAlt, Color.RoyalBlue, null);
            var btnDongBo  = BigBtn("ĐỒNG BỘ CÀI ĐẶT", IconChar.Sync, Color.DeepSkyBlue, null);
            var btnKhoiTao = BigBtn("KHỞI TẠO CỬA", IconChar.Cogs, Color.DodgerBlue, null);

            btnThemMoi.Location = new Point(10, 5);
            btnCapNhat.Location = new Point(200, 5);
            btnDongBo.Location  = new Point(600, 5);
            btnKhoiTao.Location = new Point(820, 5);

            toolbar.Controls.AddRange(new Control[] { btnThemMoi, btnCapNhat, btnDongBo, btnKhoiTao });

            // ===== DataGridView =====
            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White
            };
            grid.Columns.Add("STT", "STT");
            grid.Columns.Add("TenTram", "Tên trạm");
            grid.Columns.Add("LoaiVL", "Loại vật liệu");
            grid.Columns.Add("TenCua", "Tên cửa vật liệu");

            var gbGrid = new GroupBox
            {
                Text = "CỬA VẬT LIỆU",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Dock = DockStyle.Fill
            };
            gbGrid.Controls.Add(grid);

            // ===== Thông tin cửa =====
            var panelInfo = new Panel
            {
                Dock = DockStyle.Right,
                Width = 350,
                BackColor = Color.Gainsboro
            };

            var lblTitle = new Label
            {
                Text = "THÔNG TIN CỬA:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            panelInfo.Controls.Add(lblTitle);

            int y = 60;
            int spacing = 40;

            Label Lbl(string text) =>
                new Label { Text = text, Location = new Point(20, y), AutoSize = true };

            TextBox Txt() =>
                new TextBox { Location = new Point(150, y - 5), Width = 170 };

            // Số thứ tự
            panelInfo.Controls.Add(Lbl("SỐ THỨ TỰ CỬA:"));
            txtSoThuTu = Txt(); panelInfo.Controls.Add(txtSoThuTu); y += spacing;

            // Loại vật liệu
            panelInfo.Controls.Add(Lbl("LOẠI VẬT LIỆU:"));
            cboLoaiVatLieu = new ComboBox
            {
                Location = new Point(150, y - 5),
                Width = 170,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboLoaiVatLieu.Items.AddRange(new[] { "CAT", "ĐÁ", "XI MĂNG", "NƯỚC", "PHỤ GIA" });
            cboLoaiVatLieu.SelectedIndex = 0;
            panelInfo.Controls.Add(cboLoaiVatLieu);
            y += spacing;

            // Tên cửa
            panelInfo.Controls.Add(Lbl("TÊN CỬA:"));
            txtTenCua = Txt(); panelInfo.Controls.Add(txtTenCua); y += spacing;

            // Hệ số quy đổi
            panelInfo.Controls.Add(Lbl("HỆ SỐ QUY ĐỔI:"));
            txtHeSoQuyDoi = Txt(); panelInfo.Controls.Add(txtHeSoQuyDoi); y += spacing;

            // Đơn vị tính khác
            panelInfo.Controls.Add(Lbl("ĐƠN VỊ TÍNH KHÁC:"));
            txtDonViKhac = Txt(); panelInfo.Controls.Add(txtDonViKhac); y += spacing;

            // Trạm trộn
            panelInfo.Controls.Add(Lbl("TRẠM TRỘN:"));
            txtTramTron = Txt(); panelInfo.Controls.Add(txtTramTron); y += spacing + 10;

            // Nút Lưu (Button thường)
            btnLuu = new Button
            {
                Text = "LƯU",
                Size = new Size(120, 36),
                Location = new Point(150, y),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Enabled = true
            };
            panelInfo.Controls.Add(btnLuu);

            // ===== Layout chính =====
            var mainPanel = new Panel { Dock = DockStyle.Fill };
            mainPanel.Controls.Add(gbGrid);
            mainPanel.Controls.Add(panelInfo);

            // Thứ tự add để không bị đè
            Controls.Add(mainPanel);
            Controls.Add(toolbar);
        }
    }
}
