using System.Drawing;
using System.Windows.Forms;

namespace QuanLyTram.Forms
{
    public class DM_KinhDoanhForm : Form
    {
        public DM_KinhDoanhForm()
        {
            Text = "Kinh Doanh";
            BackColor = Color.LightCyan;

            Label lbl = new Label
            {
                Text = "QUẢN LÝ KINH DOANH",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 30)
            };
            Controls.Add(lbl);
        }
    }
}
