using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyTram.Forms
{
    public class DM_KhachHangForm : Form
    {
        // … toàn bộ code CRUD Khách hàng bạn đã viết giữ nguyên …
        // (tui không copy lại hết ở đây cho gọn, chỉ đổi tên class)

        public DM_KhachHangForm()
        {
            Text = "Khách hàng";
            BackColor = Color.FromArgb(230, 220, 250);
            Dock = DockStyle.Fill;

            // Gọi các hàm BuildUI, WireEvents, InitData như cũ
        }
    }
}
