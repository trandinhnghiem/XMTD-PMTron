using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace QuanLyTram.Forms
{
    public class KhoForm : Form
    {
        private Panel pnlTabs;
        private Button tabNhapXuat, tabLichSu, tabCuaVL;
        private Panel mainContent;
        private Form _currentChild;
        private List<Button> _allTabs;
        private Panel activeIndicator;
        // Fade animation
        private Timer fadeTimer;
        private Form nextChild;
        private Button nextTab;
        private double fadeStep = 1; // tốc độ fade 
        public KhoForm()
        {
            Text = "QUẢN LÝ KHO";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1250, 750);
            BackColor = Color.WhiteSmoke;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            BuildTabs();
            BuildMainContent();
            // Mặc định mở NHẬP XUẤT VẬT LIỆU
            OpenChild(new Kho_NhapXuatForm(), tabNhapXuat, firstLoad: true);
        }
        private void BuildTabs()
        {
            pnlTabs = new Panel
            {
                Dock = DockStyle.Top,
                Height = 55,
                BackColor = Color.FromArgb(238, 238, 238)
            };
            Controls.Add(pnlTabs);
            tabNhapXuat = MakeTab("NHẬP XUẤT VẬT LIỆU");
            tabLichSu = MakeTab("LỊCH SỬ NHẬP XUẤT");
            tabCuaVL = MakeTab("CỬA VẬT LIỆU");
            _allTabs = new List<Button> { tabNhapXuat, tabLichSu, tabCuaVL };
            int x = 14;
            foreach (var b in _allTabs)
            {
                b.Location = new Point(x, 8);
                pnlTabs.Controls.Add(b);
                x += b.Width + 10;
            }
            activeIndicator = new Panel
            {
                Height = 4,
                BackColor = Color.DimGray,
                Visible = false
            };
            pnlTabs.Controls.Add(activeIndicator);
            tabNhapXuat.Click += (s, e) => OpenChild(new Kho_NhapXuatForm(), tabNhapXuat);
            tabLichSu.Click += (s, e) => OpenChild(new Kho_LichSuForm(), tabLichSu);
            tabCuaVL.Click += (s, e) => OpenChild(new Kho_CuaVatLieuForm(), tabCuaVL);
        }
        private Button MakeTab(string text)
        {
            var b = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(16, 6, 16, 6),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(230, 230, 230),
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderColor = Color.Gainsboro;
            b.FlatAppearance.BorderSize = 1;
            return b;
        }
        private void BuildMainContent()
        {
            mainContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            Controls.Add(mainContent);
            mainContent.BringToFront();
        }
        private void OpenChild(Form child, Button senderTab, bool firstLoad = false)
        {
            if (_currentChild == null || firstLoad)
            {
                _currentChild = child;
                child.TopLevel = false;
                child.FormBorderStyle = FormBorderStyle.None;
                child.Dock = DockStyle.Fill;
                mainContent.Controls.Clear();
                mainContent.Controls.Add(child);
                child.Show();
                SetActiveTab(senderTab);
                return;
            }
            if (fadeTimer != null && fadeTimer.Enabled)
                return; // đang fade, tránh bấm liên tục gây lỗi
            // Chuẩn bị fade
            nextChild = child;
            nextTab = senderTab;
            nextChild.TopLevel = false;
            nextChild.FormBorderStyle = FormBorderStyle.None;
            nextChild.Dock = DockStyle.Fill;
            nextChild.Opacity = 0.0;
            mainContent.Controls.Add(nextChild);
            nextChild.Show();
            fadeTimer = new Timer();
            fadeTimer.Interval = 15; // tick nhanh để mượt
            fadeTimer.Tick += FadeTimer_Tick;
            fadeTimer.Start();
        }
        private void FadeTimer_Tick(object sender, EventArgs e)
        {
            if (_currentChild != null)
            {
                _currentChild.Opacity -= fadeStep;
                if (_currentChild.Opacity < 0) _currentChild.Opacity = 0;
            }
            if (nextChild != null)
            {
                nextChild.Opacity += fadeStep;
                if (nextChild.Opacity > 1) nextChild.Opacity = 1;
            }
            if ((_currentChild == null || _currentChild.Opacity <= 0) && (nextChild != null && nextChild.Opacity >= 1))
            {
                fadeTimer.Stop();
                fadeTimer.Tick -= FadeTimer_Tick;
                fadeTimer.Dispose();
                fadeTimer = null;
                if (_currentChild != null)
                {
                    mainContent.Controls.Remove(_currentChild);
                    _currentChild.Close();
                    _currentChild.Dispose();
                }
                _currentChild = nextChild;
                nextChild = null;
                SetActiveTab(nextTab);
            }
        }
        private void SetActiveTab(Button active)
        {
            foreach (var btn in _allTabs)
            {
                btn.BackColor = Color.FromArgb(230, 230, 230);
                btn.Font = new Font("Segoe UI", 10.5f, FontStyle.Regular);
            }
            active.BackColor = Color.LightGray;
            active.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            activeIndicator.Width = active.Width;
            activeIndicator.Left = active.Left;
            activeIndicator.Top = pnlTabs.Height - activeIndicator.Height;
            activeIndicator.Visible = true;
            activeIndicator.BringToFront();
        }
    }
}