using System.Drawing;
using System.Windows.Forms;

namespace QuanLyTram.Forms
{
    public class DM_CongTrinhForm : Form
    {
        public DM_CongTrinhForm()
        {
            Text = "Công Trình";
            BackColor = Color.Beige;

            Label lbl = new Label
            {
                Text = "QUẢN LÝ CÔNG TRÌNH",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 30)
            };
            Controls.Add(lbl);
        }
    }
}
