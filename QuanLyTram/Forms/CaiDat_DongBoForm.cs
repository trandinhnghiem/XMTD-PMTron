using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace QuanLyTram.Forms
{
    public class CaiDat_DongBoForm  : Form
    {
        public CaiDat_DongBoForm()
        {
            BackColor = Color.WhiteSmoke;
            Font = new Font("Segoe UI", 10);

            // === GroupBox chính ===
            var gbDongBo = new GroupBox
            {
                Text = "CÀI ĐẶT KIỂU ĐỒNG BỘ DỮ LIỆU DANH MỤC - ĐẶT HÀNG",
                Location = new Point(20, 20),
                Size = new Size(1200, 200)
            };

            // --- Đồng bộ mác bê tông ---
            var gbMac = new GroupBox
            {
                Text = "Đồng bộ mác bê tông",
                Location = new Point(20, 30),
                Size = new Size(500, 120)
            };
            var rMacTram = new RadioButton { Text = "Đồng bộ từ máy trạm", Location = new Point(20, 30), AutoSize = true };
            var rMacChu  = new RadioButton { Text = "Đồng bộ từ máy chủ", Location = new Point(20, 55), AutoSize = true };
            var rMacNone = new RadioButton { Text = "Không đồng bộ", Location = new Point(20, 80), AutoSize = true, Checked = true };
            gbMac.Controls.AddRange(new Control[] { rMacTram, rMacChu, rMacNone });

            // --- Đồng bộ bảng danh mục ---
            var gbDanhMuc = new GroupBox
            {
                Text = "Đồng bộ bảng danh mục",
                Location = new Point(550, 30),
                Size = new Size(500, 120)
            };
            var rDmTram = new RadioButton { Text = "Đồng bộ từ máy trạm", Location = new Point(20, 30), AutoSize = true };
            var rDmChu  = new RadioButton { Text = "Đồng bộ từ máy chủ", Location = new Point(20, 55), AutoSize = true };
            var rDmNone = new RadioButton { Text = "Không đồng bộ", Location = new Point(20, 80), AutoSize = true, Checked = true };
            gbDanhMuc.Controls.AddRange(new Control[] { rDmTram, rDmChu, rDmNone });

            gbDongBo.Controls.AddRange(new Control[] { gbMac, gbDanhMuc });

            // === Textbox + nút refresh ===
            var txtPath = new TextBox { Location = new Point(40, 230), Width = 1050 };
            var btnRefresh = new IconButton
            {
                IconChar = IconChar.Sync,
                IconColor = Color.DarkGoldenrod,
                IconSize = 24,
                Size = new Size(40, 28),
                Location = new Point(1100, 228),
                FlatStyle = FlatStyle.Flat
            };
            btnRefresh.FlatAppearance.BorderSize = 0;

            // === Nút Lưu ===
            var btnLuu = new Button
            {
                Text = "LƯU",
                Image = FontAwesome.Sharp.IconChar.Save.ToBitmap(Color.Green, 24, 24),
                TextAlign = ContentAlignment.MiddleLeft,
                ImageAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(120, 40),
                Location = new Point(560, 270),
                BackColor = Color.White,
                ForeColor = Color.Green
            };

            // === Ghi chú ===
            var gbGhiChu = new GroupBox
            {
                Text = "GHI CHÚ",
                Location = new Point(20, 330),
                Size = new Size(1200, 120)
            };
            var lbl1 = new Label
            {
                Text = "- ĐỒNG BỘ TỪ MÁY TRẠM: cho phép dữ liệu danh mục, đặt hàng, mác bê tông từ máy trạm được cập nhật tự động về máy chủ.",
                Location = new Point(20, 40),
                AutoSize = true
            };
            var lbl2 = new Label
            {
                Text = "- ĐỒNG BỘ TỪ MÁY CHỦ: cho phép dữ liệu danh mục, đặt hàng, mác bê tông từ máy chủ được cập nhật tự động về máy trạm.",
                Location = new Point(20, 70),
                AutoSize = true
            };
            gbGhiChu.Controls.AddRange(new Control[] { lbl1, lbl2 });

            Controls.AddRange(new Control[] { gbDongBo, txtPath, btnRefresh, btnLuu, gbGhiChu });
        }
    }

    // Helper để convert IconChar sang Bitmap (Button thường không hỗ trợ icon FontAwesome trực tiếp)
    public static class IconCharExtensions
    {
        public static Bitmap ToBitmap(this IconChar icon, Color color, int width, int height)
        {
            var iconImage = new IconPictureBox
            {
                IconChar = icon,
                IconColor = color,
                IconSize = width,
                BackColor = Color.Transparent,
                Size = new Size(width, height)
            };
            var bmp = new Bitmap(width, height);
            iconImage.DrawToBitmap(bmp, new Rectangle(Point.Empty, bmp.Size));
            return bmp;
        }
    }
}
