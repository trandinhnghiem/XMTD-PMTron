using System;
using System.Windows.Forms;
using QuanLyTron.Forms;

namespace QuanLyTron
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
