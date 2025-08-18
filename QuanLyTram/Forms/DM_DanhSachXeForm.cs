using System.Drawing;
using System.Windows.Forms;

namespace QuanLyTram.Forms
{
    public class DM_DanhSachXeForm : Form
    {
        public DM_DanhSachXeForm()
        {
            Text = "Danh Sách Xe";
            BackColor = Color.LightYellow;

            Label lbl = new Label
            {
                Text = "QUẢN LÝ DANH SÁCH XE",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 30)
            };
            Controls.Add(lbl);
        }
    }
}
