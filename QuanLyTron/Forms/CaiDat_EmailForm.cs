using System;
using System.Drawing;
using FontAwesome.Sharp;


namespace QuanLyTron.Forms
{
    public class CaiDat_EmailForm : Form
    {
        public CaiDat_EmailForm()
        {
            BackColor = Color.LightYellow;
            Font = new Font("Segoe UI", 10);

            var gbEmail = new GroupBox
            {
                Text = "CẤU HÌNH GỬI EMAIL BÁO CÁO",
                Location = new Point(20, 20),
                Size = new Size(1180, 200),
                BackColor = Color.White
            };

            var rNone   = new RadioButton { Text = "Không gửi", Location = new Point(20, 35), AutoSize = true };
            var rHourly = new RadioButton { Text = "Gửi hàng giờ", Location = new Point(150, 35), AutoSize = true };
            var rDaily  = new RadioButton { Text = "Gửi hằng ngày", Location = new Point(290, 35), AutoSize = true };

            var lblLink = new Label { Text = "LINK FILE BÁO CÁO:", Location = new Point(20, 85), AutoSize = true };
            var txtLink = new TextBox { Location = new Point(170, 80), Width = 550 };
            var btnBrowse = new Button { Text = "...", Location = new Point(730, 78), Size = new Size(40, 30) };
            btnBrowse.Click += (s, e) =>
            {
                using var ofd = new OpenFileDialog { Filter = "Tất cả|*.*" };
                if (ofd.ShowDialog() == DialogResult.OK) txtLink.Text = ofd.FileName;
            };

            var lblEmail = new Label { Text = "EMAIL NHẬN:", Location = new Point(820, 35), AutoSize = true };
            var txtEmails = new TextBox
            {
                Location = new Point(820, 60),
                Size = new Size(330, 110),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            gbEmail.Controls.AddRange(new Control[] { rNone, rHourly, rDaily, lblLink, txtLink, btnBrowse, lblEmail, txtEmails });

            // Hàng giờ
            var gbHourly = new GroupBox
            {
                Text = "CẤU HÌNH GỬI BÁO CÁO HÀNG GIỜ",
                Location = new Point(20, 240),
                Size = new Size(580, 200),
                BackColor = Color.White
            };

            var lblSoGio = new Label { Text = "SỐ GIỜ GỬI:", Location = new Point(20, 45), AutoSize = true };
            var numSoGio = new NumericUpDown { Location = new Point(110, 42), Minimum = 1, Maximum = 24, Value = 1, Width = 60 };

            var lblMau1 = new Label { Text = "MẪU BÁO CÁO:", Location = new Point(20, 95), AutoSize = true };
            var rTong1 = new RadioButton { Text = "Số liệu trộn theo tổng", Location = new Point(130, 93), AutoSize = true, Checked = true };
            var rChiTiet1 = new RadioButton { Text = "Số liệu trộn chi tiết", Location = new Point(320, 93), AutoSize = true };

            var lblFrom1 = new Label { Text = "TÍNH TỔNG TỪ NGÀY:", Location = new Point(20, 140), AutoSize = true };
            var dtFrom1 = new DateTimePicker { Location = new Point(170, 136), Width = 180 };

            gbHourly.Controls.AddRange(new Control[] { lblSoGio, numSoGio, lblMau1, rTong1, rChiTiet1, lblFrom1, dtFrom1 });

            // Hàng ngày
            var gbDaily = new GroupBox
            {
                Text = "CẤU HÌNH GỬI BÁO CÁO HÀNG NGÀY",
                Location = new Point(620, 240),
                Size = new Size(580, 200),
                BackColor = Color.White
            };

            var lblGio = new Label { Text = "GIỜ GỬI:", Location = new Point(20, 45), AutoSize = true };
            var numHour = new NumericUpDown { Location = new Point(90, 42), Minimum = 0, Maximum = 23, Width = 60 };
            var lblSep = new Label { Text = ":", Location = new Point(155, 45), AutoSize = true };
            var numMin = new NumericUpDown { Location = new Point(170, 42), Minimum = 0, Maximum = 59, Width = 60 };

            var lblMau2 = new Label { Text = "MẪU BÁO CÁO:", Location = new Point(20, 95), AutoSize = true };
            var rTong2 = new RadioButton { Text = "Số liệu trộn theo tổng", Location = new Point(130, 93), AutoSize = true, Checked = true };
            var rChiTiet2 = new RadioButton { Text = "Số liệu trộn chi tiết", Location = new Point(320, 93), AutoSize = true };

            var lblFrom2 = new Label { Text = "TÍNH TỔNG TỪ NGÀY:", Location = new Point(20, 140), AutoSize = true };
            var dtFrom2 = new DateTimePicker { Location = new Point(170, 136), Width = 180 };

            gbDaily.Controls.AddRange(new Control[] { lblGio, numHour, lblSep, numMin, lblMau2, rTong2, rChiTiet2, lblFrom2, dtFrom2 });

            // Footer – nút lưu
            var footer = new Panel { Dock = DockStyle.Bottom, Height = 85, BackColor = Color.LightYellow };
            var btnSave = new IconButton
            {
                Text = "LƯU",
                IconChar = IconChar.Save,
                IconColor = Color.White,
                IconSize = 22,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Size = new Size(150, 45),
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.FlatAppearance.BorderSize = 0;
            footer.Controls.Add(btnSave);
            footer.Resize += (s, e) =>
            {
                btnSave.Location = new Point((footer.Width - btnSave.Width) / 2, (footer.Height - btnSave.Height) / 2);
            };

            Controls.AddRange(new Control[] { gbEmail, gbHourly, gbDaily, footer });
        }
    }
}

