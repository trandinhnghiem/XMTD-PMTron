using System;
using System.Windows.Forms;
using QuanLyTram.Forms;

namespace QuanLyTram
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.MainForm()); 
        }
    }
}
