using System.Drawing;
using System.Windows.Forms;

namespace QuanLyTram.Forms
{
    public class DM_PhuGiaForm : Form
    {
        public DM_PhuGiaForm()
        {
            Text = "Phụ Gia";
            BackColor = Color.LightPink;

            Label lbl = new Label
            {
                Text = "QUẢN LÝ PHỤ GIA",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 30)
            };
            Controls.Add(lbl);
        }
    }
}
