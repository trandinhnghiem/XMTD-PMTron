using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyTram.Forms
{
    public class DanhMucForm : Form
    {
        private Panel pnlTabs;
        private Button tabKhach, tabCongTrinh, tabDanhSachXe, tabPhuGia, tabKinhDoanh;
        private Panel mainContent;

        private Form _currentChild;
        private List<Button> _allTabs;

        // Panel highlight (dấu nhọn/underline)
        private Panel activeIndicator;

        public DanhMucForm()
        {
            Text = "BẢNG DANH MỤC";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1200, 720);
            BackColor = Color.FromArgb(215, 215, 255);
            
            // 🚫 Khóa nút phóng to
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // cũng khóa kéo thay đổi kích thước


            BuildTabs();
            BuildMainContent();

            // Mặc định mở KHÁCH HÀNG
            OpenChild(new DM_KhachHangForm(), tabKhach);
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

            tabKhach = MakeTab("KHÁCH HÀNG");
            tabCongTrinh = MakeTab("CÔNG TRÌNH");
            tabDanhSachXe = MakeTab("DANH SÁCH XE");
            tabPhuGia = MakeTab("PHỤ GIA");
            tabKinhDoanh = MakeTab("KINH DOANH");

            _allTabs = new List<Button> { tabKhach, tabCongTrinh, tabDanhSachXe, tabPhuGia, tabKinhDoanh };

            int x = 14;
            foreach (var b in _allTabs)
            {
                b.Location = new Point(x, 8);
                pnlTabs.Controls.Add(b);
                x += b.Width + 10;
            }

            // Thanh gạch highlight
            activeIndicator = new Panel
            {
                Height = 4,
                BackColor = Color.DimGray,
                Visible = false
            };
            pnlTabs.Controls.Add(activeIndicator);

            // gán sự kiện click
            tabKhach.Click += (s, e) => OpenChild(new DM_KhachHangForm(), tabKhach);
            tabCongTrinh.Click += (s, e) => OpenChild(new DM_CongTrinhForm(), tabCongTrinh);
            tabDanhSachXe.Click += (s, e) => OpenChild(new DM_DanhSachXeForm(), tabDanhSachXe);
            tabPhuGia.Click += (s, e) => OpenChild(new DM_PhuGiaForm(), tabPhuGia);
            tabKinhDoanh.Click += (s, e) => OpenChild(new DM_KinhDoanhForm(), tabKinhDoanh);
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
                BackColor = Color.FromArgb(230, 230, 230)
            };
            b.FlatAppearance.BorderColor = Color.Gainsboro;
            b.FlatAppearance.BorderSize = 1;
            b.Cursor = Cursors.Hand;
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

        private void OpenChild(Form child, Button senderTab)
        {
            // đóng child cũ nếu có
            if (_currentChild != null)
            {
                _currentChild.Close();
                _currentChild.Dispose();
                _currentChild = null;
            }

            // mở child mới
            _currentChild = child;
            child.TopLevel = false;
            child.FormBorderStyle = FormBorderStyle.None;
            child.Dock = DockStyle.Fill;
            mainContent.Controls.Clear();
            mainContent.Controls.Add(child);
            child.Show();

            // set active tab
            SetActiveTab(senderTab);
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

            // di chuyển thanh underline dưới nút đang chọn
            activeIndicator.Width = active.Width;
            activeIndicator.Left = active.Left;
            activeIndicator.Top = pnlTabs.Height - activeIndicator.Height;
            activeIndicator.Visible = true;
            activeIndicator.BringToFront();
        }
    }
}
