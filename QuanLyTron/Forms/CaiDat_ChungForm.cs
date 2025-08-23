using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp; // cần cho IconButton và IconChar

namespace QuanLyTron.Forms
{
    public class CaiDat_ChungForm : Form
    {
        // Định nghĩa margin và textboxWidth
        private const int margin = 20;
        private const int textboxWidth = 300;

        public CaiDat_ChungForm()
        {
            Text = "Cài đặt chung";
            BackColor = Color.White;
            Width = 1280;
            Height = 720;
            StartPosition = FormStartPosition.CenterScreen;

            // GroupBox Thông tin trạm trộn
            var gbThongTin = new GroupBox
            {
                Text = "THÔNG TIN TRẠM TRỘN",
                Location = new Point(margin, 40),
                Size = new Size(560, 240),
                BackColor = Color.White
            };

            var lblMaTram = new Label { Text = "MÃ TRẠM:", Location = new Point(20, 40), AutoSize = true };
            var txtMaTram = new TextBox { Location = new Point(150, 35), Width = textboxWidth, Font = new Font("Segoe UI", 11) };

            var lblTenTram = new Label { Text = "TÊN TRẠM:", Location = new Point(20, 85), AutoSize = true };
            var txtTenTram = new TextBox { Location = new Point(150, 80), Width = textboxWidth, Font = new Font("Segoe UI", 11) };

            var lblChuTram = new Label { Text = "CHỦ TRẠM:", Location = new Point(20, 130), AutoSize = true };
            var txtChuTram = new TextBox { Location = new Point(150, 125), Width = textboxWidth - 50, Font = new Font("Segoe UI", 11) };
            var btnRefresh = new Button { Text = "⟳", Location = new Point(150 + textboxWidth - 50 + 10, 125), Size = new Size(40, 32) };

            var lblDiaDiem = new Label { Text = "ĐỊA ĐIỂM:", Location = new Point(20, 175), AutoSize = true };
            var txtDiaDiem = new TextBox { Location = new Point(150, 170), Width = textboxWidth, Font = new Font("Segoe UI", 11) };

            gbThongTin.Controls.AddRange(new Control[] { lblMaTram, txtMaTram, lblTenTram, txtTenTram, lblChuTram, txtChuTram, btnRefresh, lblDiaDiem, txtDiaDiem });

            // GroupBox Đường dẫn
            var gbDuongDan = new GroupBox
            {
                Text = "ĐƯỜNG DẪN CHƯƠNG TRÌNH",
                Location = new Point(600, 40),
                Size = new Size(620, 120),
                BackColor = Color.White
            };

            var lblFile = new Label { Text = "FILE RUNTIME:", Location = new Point(20, 55), AutoSize = true };
            var txtFile = new TextBox { Location = new Point(150, 50), Width = 360, Font = new Font("Segoe UI", 11) };
            var btnBrowse = new Button { Text = "...", Location = new Point(520, 50), Size = new Size(40, 32) };
            btnBrowse.Click += (s, e) =>
            {
                using var ofd = new OpenFileDialog { Filter = "Tất cả|*.*" };
                if (ofd.ShowDialog() == DialogResult.OK) txtFile.Text = ofd.FileName;
            };

            gbDuongDan.Controls.AddRange(new Control[] { lblFile, txtFile, btnBrowse });

            // GroupBox Phiếu in
            var gbPhieuIn = new GroupBox
            {
                Text = "CÀI ĐẶT THÔNG TIN PHIẾU IN",
                Location = new Point(600, 180),
                Size = new Size(620, 200),
                BackColor = Color.White
            };

            var rdoChuan = new RadioButton { Text = "Theo cấp phối chuẩn", Location = new Point(20, 40), AutoSize = true };
            var rdoMe = new RadioButton { Text = "Theo cấp phối từng mẻ", Location = new Point(300, 40), AutoSize = true };

            var chkPhuGia = new CheckBox { Text = "Sử dụng phụ gia binh", Location = new Point(20, 85), AutoSize = true };
            var chkInTrucTiep = new CheckBox { Text = "In trực tiếp trên máy in", Location = new Point(300, 85), AutoSize = true };

            var btnMayIn = new IconButton
            {
                IconChar = IconChar.Print,
                IconColor = Color.White,
                IconSize = 28,
                Text = "MÁY IN",
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Size = new Size(150, 45),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point((620 - 150) / 2, 130)
            };
            btnMayIn.FlatAppearance.BorderSize = 0;

            gbPhieuIn.Controls.AddRange(new Control[] { rdoChuan, rdoMe, chkPhuGia, chkInTrucTiep, btnMayIn });

            // Footer – LƯU
            var panelFooter = new Panel { Dock = DockStyle.Bottom, Height = 90, BackColor = Color.LightYellow };
            var btnLuu = new IconButton
            {
                IconChar = IconChar.Save,
                IconColor = Color.White,
                IconSize = 28,
                Text = "LƯU",
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Size = new Size(160, 50),
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLuu.FlatAppearance.BorderSize = 0;
            panelFooter.Controls.Add(btnLuu);
            panelFooter.Resize += (s, e) =>
            {
                btnLuu.Location = new Point((panelFooter.Width - btnLuu.Width) / 2, (panelFooter.Height - btnLuu.Height) / 2);
            };

            // Thêm tất cả vào form
            Controls.AddRange(new Control[] { gbThongTin, gbDuongDan, gbPhieuIn, panelFooter });
        }
    }
}
