using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyTram.Forms
{
    public class DatHangForm : Form
    {
        private Button btnThemMoi, btnCapNhat, btnXoa, btnLuu;
        private DateTimePicker dtpNgay;
        private TextBox txtMaDon, txtKyHieu, txtSoPhieu, txtDatHang, txtTichLuy;
        private ComboBox cbTramTron, cbKhachHang, cbDiaDiem, cbKinhDoanh;
        private CheckBox chkHoatDong;
        private DataGridView dgvDonHang;
        private DataTable dtDonHang;
        private bool isAddingNew = false;

        public DatHangForm()
        {
            this.Text = "QUẢN LÝ ĐƠN ĐẶT HÀNG";
            this.Size = new Size(1250, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Beige;

            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            Font btnFont = new Font("Segoe UI", 10, FontStyle.Bold);

            // Nút Thêm mới
            btnThemMoi = new Button()
            {
                Text = " THÊM MỚI",
                Width = 150,
                Height = 50,
                Padding = new Padding(16,0,0,0),
                Location = new Point(20, 20),
                BackColor = Color.LightGreen,
                Font = btnFont,
                FlatStyle = FlatStyle.Flat,
                Image = SystemIcons.Application.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText
            };
            btnThemMoi.FlatAppearance.BorderSize = 0;
            btnThemMoi.FlatAppearance.MouseOverBackColor = Color.FromArgb(152, 251, 152);

            // Nút Cập nhật
            btnCapNhat = new Button()
            {
                Text = " CẬP NHẬT",
                Width = 150,
                Height = 50,
                Padding = new Padding(16,0,0,0),
                Location = new Point(190, 20),
                BackColor = Color.Khaki,
                Font = btnFont,
                FlatStyle = FlatStyle.Flat,
                Image = SystemIcons.Information.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText
            };
            btnCapNhat.FlatAppearance.BorderSize = 0;
            btnCapNhat.FlatAppearance.MouseOverBackColor = Color.FromArgb(255 ,246 ,143);

            // Nút Xóa
            btnXoa = new Button()
            {
                Text = " XÓA",
                Width = 150,
                Height = 50,
                Padding = new Padding(16,0,0,0),
                Location = new Point(360, 20),
                BackColor = Color.LightCoral,
                Font = btnFont,
                FlatStyle = FlatStyle.Flat,
                Image = SystemIcons.Error.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText
            };
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 123, 115);

            this.Controls.Add(btnThemMoi);
            this.Controls.Add(btnCapNhat);
            this.Controls.Add(btnXoa);

            // ---------- GroupBox "THÔNG TIN ĐẶT HÀNG" ----------
            GroupBox groupInfo = new GroupBox()
            {
                Text = "THÔNG TIN ĐẶT HÀNG",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.Red,
                BackColor = Color.Transparent,
                Size = new Size(900, 280),
                Location = new Point(20, 90),
                Padding = new Padding(18)
            };

            int startY = 35;  
            int gapY = 38;    

            // Labels
            Label lblNgay = new Label() { Text = "Ngày hệ thống:", Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Color.Black, Location = new Point(20, startY), AutoSize = true };
            Label lblMaDon = new Label() { Text = "Mã đơn hàng:", Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Color.Black, Location = new Point(20, startY + 1 * gapY), AutoSize = true };
            Label lblKyHieu = new Label() { Text = "Ký hiệu đơn:", Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Color.Black, Location = new Point(20, startY + 2 * gapY), AutoSize = true };
            Label lblSoPhieu = new Label() { Text = "Số phiếu:", Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Color.Black, Location = new Point(20, startY + 3 * gapY), AutoSize = true };
            Label lblDatHang = new Label() { Text = "Đặt hàng (m3):", Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Color.Black, Location = new Point(20, startY + 4 * gapY), AutoSize = true };
            Label lblTichLuy = new Label() { Text = "Tích lũy (m3):", Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Color.Black, Location = new Point(20, startY + 5 * gapY), AutoSize = true };

            Label lblTramTron = new Label() { Text = "Trạm trộn:", Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Color.Black, Location = new Point(400, startY), AutoSize = true };
            Label lblKhach = new Label() { Text = "Khách hàng:", Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Color.Black, Location = new Point(400, startY + 1 * gapY), AutoSize = true };
            Label lblDiaDiem = new Label() { Text = "Địa điểm:", Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Color.Black, Location = new Point(400, startY + 2 * gapY), AutoSize = true };
            Label lblKD = new Label() { Text = "Kinh doanh:", Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Color.Black, Location = new Point(400, startY + 3 * gapY), AutoSize = true };

            // TextBoxes, ComboBoxes
            dtpNgay = new DateTimePicker() { Font = new Font("Segoe UI", 10.5f), Location = new Point(150, startY), Width = 200, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" };
            txtMaDon = new TextBox() { Font = new Font("Segoe UI", 10.5f), Location = new Point(150, startY + 1 * gapY), Width = 200 };
            txtKyHieu = new TextBox() { Font = new Font("Segoe UI", 10.5f), Location = new Point(150, startY + 2 * gapY), Width = 200 };
            txtSoPhieu = new TextBox() { Font = new Font("Segoe UI", 10.5f), Location = new Point(150, startY + 3 * gapY), Width = 200 };
            txtDatHang = new TextBox() { Font = new Font("Segoe UI", 10.5f), Location = new Point(150, startY + 4 * gapY), Width = 200 };
            txtTichLuy = new TextBox() { Font = new Font("Segoe UI", 10.5f), Location = new Point(150, startY + 5 * gapY), Width = 200 };

            cbTramTron = new ComboBox() { Font = new Font("Segoe UI", 10.5f), Location = new Point(500, startY), Width = 350 };
            cbKhachHang = new ComboBox() { Font = new Font("Segoe UI", 10.5f), Location = new Point(500, startY + 1 * gapY), Width = 350 };
            cbDiaDiem = new ComboBox() { Font = new Font("Segoe UI", 10.5f), Location = new Point(500, startY + 2 * gapY), Width = 350 };
            cbKinhDoanh = new ComboBox() { Font = new Font("Segoe UI", 10.5f), Location = new Point(500, startY + 3 * gapY), Width = 350 };

            // CheckBox
            chkHoatDong = new CheckBox() { Text = "Hoạt động", Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Color.Black, Location = new Point(500, 190), AutoSize = true };

            // Nút Lưu
            btnLuu = new Button()
            {
                Text = " LƯU",
                Width = 120,
                Height = 40,
                Padding = new Padding(12,0,0,0),
                Location = new Point(730, 215),
                BackColor = Color.LightSkyBlue,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Image = SystemIcons.Shield.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText
            };
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.FlatAppearance.MouseOverBackColor =  Color.FromArgb(176 ,226,255);

            groupInfo.Controls.AddRange(new Control[] {
                lblNgay, dtpNgay, lblMaDon, txtMaDon, lblKyHieu, txtKyHieu, lblSoPhieu, txtSoPhieu,
                lblTramTron, cbTramTron, lblKhach, cbKhachHang, lblDiaDiem, cbDiaDiem, lblKD, cbKinhDoanh,
                lblDatHang, txtDatHang, lblTichLuy, txtTichLuy, chkHoatDong, btnLuu
            });

            this.Controls.Add(groupInfo);

            // Label tiêu đề DataGridView
            Label lblDGVTitle = new Label()
            {
                Text = "DANH SÁCH ĐƠN HÀNG",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.Red,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Width = 1180,
                Location = new Point(20, groupInfo.Bottom + 10)
            };
            this.Controls.Add(lblDGVTitle);

            // DataGridView (đồng bộ style với DM_KhachHangForm)
            dgvDonHang = new DataGridView()
            {
                Location = new Point(20, lblDGVTitle.Bottom + 5),
                Size = new Size(1190, 260),
                ReadOnly = true,
                MultiSelect = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = true,
                EnableHeadersVisualStyles = true
            };
            this.Controls.Add(dgvDonHang);

            // Dữ liệu mẫu
            dtDonHang = new DataTable();
            dtDonHang.Columns.Add("Mã");
            dtDonHang.Columns.Add("Ký hiệu đơn");
            dtDonHang.Columns.Add("Khách hàng");
            dtDonHang.Columns.Add("M3 đặt hàng");
            dtDonHang.Columns.Add("Ngày tháng", typeof(DateTime));
            dtDonHang.Columns.Add("Số phiếu");
            dtDonHang.Columns.Add("Tích lũy");
            dtDonHang.Columns.Add("Địa điểm CT");

            dtDonHang.Rows.Add("1", "A01", "CTY TNHH TV TK XD Tây Đô", "24", new DateTime(2025, 7, 27), "0", "0", "KHO BẠC NHÀ NƯỚC HẬU GIANG");
            dtDonHang.Rows.Add("2", "A02", "CTY TNHH Trường Sơn 145", "20", new DateTime(2025, 7, 5), "0", "0", "QL61C, H.CHÂU THÀNH");
            dtDonHang.Rows.Add("3", "A03", "CTY CP Xây Dựng Minh Phát", "15", new DateTime(2025, 7, 10), "0", "0", "KCN TÂN PHÚ THẠNH");
            dtDonHang.Rows.Add("4", "A04", "CTY TNHH Bê Tông Long An", "30", new DateTime(2025, 7, 15), "0", "0", "QL1A, P.7, TP.VĨNH LONG");
            dtDonHang.Rows.Add("5", "A05", "CTY CP XD Giao Thông 8", "18", new DateTime(2025, 7, 20), "0", "0", "CẦU CÁI RĂNG - CẦN THƠ");
            dtDonHang.Rows.Add("6", "A06", "CTY TNHH XD Phương Nam", "25", new DateTime(2025, 7, 22), "0", "0", "KDC HƯNG PHÚ 1, CẦN THƠ");
            dtDonHang.Rows.Add("7", "A07", "CTY CP Đầu Tư Xây Dựng Hòa Bình", "40", new DateTime(2025, 7, 24), "0", "0", "KĐT MỚI NAM CẦN THƠ");
            dtDonHang.Rows.Add("8", "A08", "CTY TNHH MTV XD Nam Việt", "12", new DateTime(2025, 7, 30), "0", "0", "KCN TRÀ NÓC 2, CẦN THƠ");


            dgvDonHang.DataSource = dtDonHang;
            dgvDonHang.Columns["Ngày tháng"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvDonHang.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

            // Sự kiện
            btnThemMoi.Click += BtnThemMoi_Click;
            btnCapNhat.Click += BtnCapNhat_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLuu.Click += BtnLuu_Click;
            dgvDonHang.SelectionChanged += DgvDonHang_SelectionChanged;

            LoadFirstRow();
        }

        private void LoadFirstRow()
        {
            if (dgvDonHang.Rows.Count > 0)
            {
                dgvDonHang.CurrentCell = dgvDonHang.Rows[0].Cells[0];
                LoadRowToForm(dgvDonHang.Rows[0]);
            }
        }

        private void LoadRowToForm(DataGridViewRow row)
        {
            txtMaDon.Text = row.Cells["Mã"].Value.ToString();
            txtKyHieu.Text = row.Cells["Ký hiệu đơn"].Value.ToString();
            cbKhachHang.Text = row.Cells["Khách hàng"].Value.ToString();
            txtDatHang.Text = row.Cells["M3 đặt hàng"].Value.ToString();
            dtpNgay.Value = (DateTime)row.Cells["Ngày tháng"].Value;
            txtSoPhieu.Text = row.Cells["Số phiếu"].Value.ToString();
            txtTichLuy.Text = row.Cells["Tích lũy"].Value.ToString();
            cbDiaDiem.Text = row.Cells["Địa điểm CT"].Value.ToString();
        }

        private void BtnThemMoi_Click(object sender, EventArgs e)
        {
            isAddingNew = true;
            txtMaDon.Text = "";
            txtKyHieu.Text = "";
            txtDatHang.Text = "";
            txtSoPhieu.Text = "0";
            txtTichLuy.Text = "0";
            cbKhachHang.SelectedIndex = -1;
            cbDiaDiem.SelectedIndex = -1;
            dtpNgay.Value = DateTime.Today;
            txtMaDon.Focus();
        }

        private void BtnCapNhat_Click(object sender, EventArgs e)
        {
            if (dgvDonHang.CurrentRow != null)
            {
                LoadRowToForm(dgvDonHang.CurrentRow);
                isAddingNew = false;
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDonHang.CurrentRow != null)
            {
                dgvDonHang.Rows.Remove(dgvDonHang.CurrentRow);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaDon.Text)) return;

            if (isAddingNew)
            {
                dtDonHang.Rows.Add(
                    txtMaDon.Text,
                    txtKyHieu.Text,
                    cbKhachHang.Text,
                    txtDatHang.Text,
                    dtpNgay.Value,
                    txtSoPhieu.Text,
                    txtTichLuy.Text,
                    cbDiaDiem.Text
                );
                isAddingNew = false;
            }
            else
            {
                var row = dgvDonHang.CurrentRow;
                if (row != null)
                {
                    row.Cells["Mã"].Value = txtMaDon.Text;
                    row.Cells["Ký hiệu đơn"].Value = txtKyHieu.Text;
                    row.Cells["Khách hàng"].Value = cbKhachHang.Text;
                    row.Cells["M3 đặt hàng"].Value = txtDatHang.Text;
                    row.Cells["Ngày tháng"].Value = dtpNgay.Value;
                    row.Cells["Số phiếu"].Value = txtSoPhieu.Text;
                    row.Cells["Tích lũy"].Value = txtTichLuy.Text;
                    row.Cells["Địa điểm CT"].Value = cbDiaDiem.Text;
                }
            }
        }

        private void DgvDonHang_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDonHang.CurrentRow != null && !isAddingNew)
            {
                LoadRowToForm(dgvDonHang.CurrentRow);
            }
        }
    }
}
