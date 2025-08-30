using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace QuanLyTram.Forms
{
    public class CaiDat_VatLieuForm : Form
    {
        private DataGridView grid;
        private TextBox txtSoThuTu, txtTenCua, txtHeSoQuyDoi, txtDonViKhac, txtTramTron;
        private ComboBox cboLoaiVatLieu;
        private Button btnLuu;

        public CaiDat_VatLieuForm()
        {
            BackColor = Color.LightYellow;
            Font = new Font("Segoe UI", 10);

            // ===== Thanh công cụ =====
            var toolbar = new Panel { Height = 56, Dock = DockStyle.Top, BackColor = Color.LightYellow };

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
            var btnDongBo = BigBtn("ĐỒNG BỘ CÀI ĐẶT", IconChar.Sync, Color.DeepSkyBlue, null);
            var btnKhoiTao = BigBtn("KHỞI TẠO CỬA", IconChar.Cogs, Color.DodgerBlue, null);

            btnThemMoi.Location = new Point(10, 5);
            btnCapNhat.Location = new Point(200, 5);
            btnDongBo.Location = new Point(800, 5);
            btnKhoiTao.Location = new Point(1000, 5);

            toolbar.Controls.AddRange(new Control[] { btnThemMoi, btnCapNhat, btnDongBo, btnKhoiTao });

            // ===== DataGridView =====
            grid = new DataGridView
            {
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                Dock = DockStyle.Fill,

                // ✅ bỏ cột trắng đầu tiên
                RowHeadersVisible = false,

                // ✅ chỉnh tiêu đề to hơn
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing,
                ColumnHeadersHeight = 40
            };

            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grid.Columns.Add("STT", "STT");
            grid.Columns.Add("TenTram", "Tên trạm");
            grid.Columns.Add("LoaiVL", "Loại vật liệu");
            grid.Columns.Add("TenCua", "Tên cửa vật liệu");
            grid.Columns.Add("Heso", "Hệ số quy đổi");
            grid.Columns.Add("DVT", "Đơn vị tính");

            var gbGrid = new GroupBox
            {
                Text = "CỬA VẬT LIỆU",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(10, 5),
                Size = new Size(820, 530)
            };
            gbGrid.Controls.Add(grid);

            // ===== GroupBox Thông tin cửa =====
            var gbInfo = new GroupBox
            {
                Text = "THÔNG TIN CỬA",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(gbGrid.Right + 1, gbGrid.Top),
                Size = new Size(359, gbGrid.Height),
                BackColor = Color.White
            };

            int y = 40;
            int spacing = 40;

            Label Lbl(string text) =>
                new Label { Text = text, Location = new Point(15, y), AutoSize = true };

            TextBox Txt() =>
                new TextBox { Location = new Point(160, y - 5), Width = 170 };

            // Số thứ tự
            gbInfo.Controls.Add(Lbl("SỐ THỨ TỰ CỬA:"));
            txtSoThuTu = Txt(); gbInfo.Controls.Add(txtSoThuTu); y += spacing;

            // Loại vật liệu
            gbInfo.Controls.Add(Lbl("LOẠI VẬT LIỆU:"));
            cboLoaiVatLieu = new ComboBox
            {
                Location = new Point(160, y - 5),
                Width = 170,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboLoaiVatLieu.Items.AddRange(new[] { "CÁT", "ĐÁ", "XI MĂNG", "NƯỚC", "PHỤ GIA" });
            cboLoaiVatLieu.SelectedIndex = 0;
            gbInfo.Controls.Add(cboLoaiVatLieu);
            y += spacing;

            // Tên cửa
            gbInfo.Controls.Add(Lbl("TÊN CỬA:"));
            txtTenCua = Txt(); gbInfo.Controls.Add(txtTenCua); y += spacing;

            // Hệ số quy đổi
            gbInfo.Controls.Add(Lbl("HỆ SỐ QUY ĐỔI:"));
            txtHeSoQuyDoi = Txt(); gbInfo.Controls.Add(txtHeSoQuyDoi); y += spacing;

            // Đơn vị tính khác
            gbInfo.Controls.Add(Lbl("ĐƠN VỊ TÍNH KHÁC:"));
            txtDonViKhac = Txt(); gbInfo.Controls.Add(txtDonViKhac); y += spacing;

            // Trạm trộn
            gbInfo.Controls.Add(Lbl("TRẠM TRỘN:"));
            txtTramTron = Txt(); gbInfo.Controls.Add(txtTramTron); y += spacing + 10;

            // Nút Lưu
            btnLuu = new Button
            {
                Text = "LƯU",
                Size = new Size(120, 36),
                Location = new Point(150, y),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            gbInfo.Controls.Add(btnLuu);

            // ===== Layout chính =====
            var mainPanel = new Panel { Dock = DockStyle.Fill };
            mainPanel.Controls.Add(gbGrid);
            mainPanel.Controls.Add(gbInfo);

            Controls.Add(mainPanel);
            Controls.Add(toolbar);
        }
    }
}
