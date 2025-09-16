using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp; // Thêm thư viện FontAwesome
using QuanLyTron.DAL;

namespace QuanLyTron.Forms
{
    public class DatHangForm : Form
    {
        private IconButton btnThemMoi, btnCapNhat, btnXoa, btnLuu; // Thay đổi từ Button sang IconButton
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
            Font btnFont = new Font("Segoe UI", 11, FontStyle.Bold); // Tăng cỡ font cho button
            
            // Nút Thêm mới
            btnThemMoi = new IconButton()
            {
                Text = "THÊM MỚI",
                Width = 150,
                Height = 50,
                Font = btnFont,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(46,204,113),
                FlatStyle = FlatStyle.Flat,
                IconChar = IconChar.PlusCircle,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleCenter,
                ImageAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(15, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnThemMoi.FlatAppearance.BorderSize = 0;
            btnThemMoi.Location = new Point(20, 20);
            
            // Nút Cập nhật
            btnCapNhat = new IconButton()
            {
                Text = "CẬP NHẬT",
                Width = 150,
                Height = 50,
                Font = btnFont,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(65, 131, 215),
                FlatStyle = FlatStyle.Flat,
                IconChar = IconChar.Save,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleCenter,
                ImageAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(15, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnCapNhat.FlatAppearance.BorderSize = 0;
            btnCapNhat.Location = new Point(190, 20);
            
            // Nút Xóa
            btnXoa = new IconButton()
            {
                Text = "XÓA",
                Width = 150,
                Height = 50,
                Font = btnFont,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(231, 76, 60),
                FlatStyle = FlatStyle.Flat,
                IconChar = IconChar.Trash,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleCenter,
                ImageAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(25, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.Location = new Point(360, 20);
            
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
            txtMaDon = new TextBox() { Font = new Font("Segoe UI", 10.5f), Location = new Point(150, startY + 1 * gapY), Width = 200, ReadOnly = true };
            txtKyHieu = new TextBox() { Font = new Font("Segoe UI", 10.5f), Location = new Point(150, startY + 2 * gapY), Width = 200 };
            txtSoPhieu = new TextBox() { Font = new Font("Segoe UI", 10.5f), Location = new Point(150, startY + 3 * gapY), Width = 200 };
            txtDatHang = new TextBox() { Font = new Font("Segoe UI", 10.5f), Location = new Point(150, startY + 4 * gapY), Width = 200 };
            txtTichLuy = new TextBox() { Font = new Font("Segoe UI", 10.5f), Location = new Point(150, startY + 5 * gapY), Width = 200 };
            cbTramTron = new ComboBox() { Font = new Font("Segoe UI", 10.5f), Location = new Point(500, startY), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
            cbKhachHang = new ComboBox() { Font = new Font("Segoe UI", 10.5f), Location = new Point(500, startY + 1 * gapY), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
            cbDiaDiem = new ComboBox() { Font = new Font("Segoe UI", 10.5f), Location = new Point(500, startY + 2 * gapY), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
            cbKinhDoanh = new ComboBox() { Font = new Font("Segoe UI", 10.5f), Location = new Point(500, startY + 3 * gapY), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
            
            // CheckBox
            chkHoatDong = new CheckBox() { Text = "Hoạt động", Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Color.Black, Location = new Point(500, 190), AutoSize = true };
            
            // Nút Lưu
            btnLuu = new IconButton()
            {
                Text = "LƯU",
                Width = 120,
                Height = 40,
                Font = btnFont,
                BackColor = Color.MediumPurple,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                IconChar = IconChar.Save,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleCenter,
                ImageAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(15, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.Location = new Point(730, 215);
            
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
            
            // Load dữ liệu
            LoadData();
            
            // Sự kiện
            btnThemMoi.Click += BtnThemMoi_Click;
            btnCapNhat.Click += BtnCapNhat_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLuu.Click += BtnLuu_Click;
            dgvDonHang.SelectionChanged += DgvDonHang_SelectionChanged;
            
            LoadFirstRow();
        }
        
        private void LoadData()
        {
            try
            {
                // Lấy ID trạm hiện tại
                int stationId = DatabaseHelper.CurrentStationId;
                
                // Load dữ liệu đơn hàng theo trạm
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(@"
                    SELECT dh.MADONHANG, dh.NGAYDAT, dh.KYHIEUDON, dh.KHOILUONG, dh.TICHLUY, 
                           dh.SOPHIEU, dh.HOATDONG, kh.TENKHACH, ct.DIADIEM, kd.TENKINHDOANH
                    FROM DONHANG dh
                    LEFT JOIN KHACHHANG kh ON dh.MAKHACH = kh.MAKHACH
                    LEFT JOIN CONGTRINH ct ON dh.MACONGTRINH = ct.MACONGTRINH
                    LEFT JOIN KINHDOANH kd ON dh.MAKINHDOANH = kd.MAKINHDOANH
                    WHERE dh.MATRAM = @stationId
                    ORDER BY dh.NGAYDAT DESC", conn))
                    {
                        cmd.Parameters.AddWithValue("@stationId", stationId);
                        
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            dtDonHang = new DataTable();
                            adapter.Fill(dtDonHang);
                            
                            // Đổi tên cột để hiển thị
                            dtDonHang.Columns["MADONHANG"].ColumnName = "Mã";
                            dtDonHang.Columns["NGAYDAT"].ColumnName = "Ngày tháng";
                            dtDonHang.Columns["KYHIEUDON"].ColumnName = "Ký hiệu đơn";
                            dtDonHang.Columns["KHOILUONG"].ColumnName = "M3 đặt hàng";
                            dtDonHang.Columns["TICHLUY"].ColumnName = "Tích lũy";
                            dtDonHang.Columns["SOPHIEU"].ColumnName = "Số phiếu";
                            dtDonHang.Columns["HOATDONG"].ColumnName = "Hoạt động";
                            dtDonHang.Columns["TENKHACH"].ColumnName = "Khách hàng";
                            dtDonHang.Columns["DIADIEM"].ColumnName = "Địa điểm CT";
                            dtDonHang.Columns["TENKINHDOANH"].ColumnName = "Kinh doanh";
                            
                            dgvDonHang.DataSource = dtDonHang;
                            dgvDonHang.Columns["Ngày tháng"].DefaultCellStyle.Format = "dd/MM/yyyy";
                        }
                    }
                }
                
                // Load dữ liệu cho ComboBox
                LoadComboBoxData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu đơn hàng: " + ex.Message);
            }
            
            dgvDonHang.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
        }
        
        private void LoadComboBoxData()
        {
            try
            {
                // Lấy ID trạm hiện tại
                int stationId = DatabaseHelper.CurrentStationId;
                
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    
                    // Load trạm trộn - chỉ hiển thị trạm hiện tại
                    cbTramTron.Items.Clear();
                    using (var cmd = new SqlCommand("SELECT MATRAM, TENTRAM FROM TRAM WHERE MATRAM = @stationId", conn))
                    {
                        cmd.Parameters.AddWithValue("@stationId", stationId);
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cbTramTron.Items.Add(new { Value = reader["MATRAM"], Display = reader["TENTRAM"].ToString() });
                            }
                        }
                    }
                    cbTramTron.DisplayMember = "Display";
                    cbTramTron.ValueMember = "Value";
                    if (cbTramTron.Items.Count > 0) cbTramTron.SelectedIndex = 0;
                    
                    // Load khách hàng
                    cbKhachHang.Items.Clear();
                    using (var cmd = new SqlCommand("SELECT MAKHACH, TENKHACH FROM KHACHHANG ORDER BY TENKHACH", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cbKhachHang.Items.Add(new { Value = reader["MAKHACH"], Display = reader["TENKHACH"].ToString() });
                            }
                        }
                    }
                    cbKhachHang.DisplayMember = "Display";
                    cbKhachHang.ValueMember = "Value";
                    if (cbKhachHang.Items.Count > 0) cbKhachHang.SelectedIndex = 0;
                    
                    // Load địa điểm công trình
                    cbDiaDiem.Items.Clear();
                    using (var cmd = new SqlCommand("SELECT MACONGTRINH, DIADIEM FROM CONGTRINH ORDER BY DIADIEM", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cbDiaDiem.Items.Add(new { Value = reader["MACONGTRINH"], Display = reader["DIADIEM"].ToString() });
                            }
                        }
                    }
                    cbDiaDiem.DisplayMember = "Display";
                    cbDiaDiem.ValueMember = "Value";
                    if (cbDiaDiem.Items.Count > 0) cbDiaDiem.SelectedIndex = 0;
                    
                    // Load kinh doanh
                    cbKinhDoanh.Items.Clear();
                    using (var cmd = new SqlCommand("SELECT MAKINHDOANH, TENKINHDOANH FROM KINHDOANH ORDER BY TENKINHDOANH", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cbKinhDoanh.Items.Add(new { Value = reader["MAKINHDOANH"], Display = reader["TENKINHDOANH"].ToString() });
                            }
                        }
                    }
                    cbKinhDoanh.DisplayMember = "Display";
                    cbKinhDoanh.ValueMember = "Value";
                    if (cbKinhDoanh.Items.Count > 0) cbKinhDoanh.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu ComboBox: " + ex.Message);
            }
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
            
            // Set checkbox
            if (row.Cells["Hoạt động"] != null && row.Cells["Hoạt động"].Value != DBNull.Value)
            {
                chkHoatDong.Checked = Convert.ToBoolean(row.Cells["Hoạt động"].Value);
            }
        }
        
        private void BtnThemMoi_Click(object sender, EventArgs e)
        {
            isAddingNew = true;
            txtMaDon.Text = "";
            txtKyHieu.Text = "";
            txtDatHang.Text = "";
            txtSoPhieu.Text = "0";
            txtTichLuy.Text = "0";
            // Không cần reset cbTramTron vì đã được thiết lập chỉ có 1 trạm
            cbKhachHang.SelectedIndex = -1;
            cbDiaDiem.SelectedIndex = -1;
            dtpNgay.Value = DateTime.Today;
            chkHoatDong.Checked = true;
            txtKyHieu.Focus();
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
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa đơn hàng này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int maDonHang = Convert.ToInt32(dgvDonHang.CurrentRow.Cells["Mã"].Value);
                        int stationId = DatabaseHelper.CurrentStationId;
                        
                        using (var conn = DatabaseHelper.GetConnection())
                        {
                            conn.Open();
                            using (var cmd = new SqlCommand("DELETE FROM DONHANG WHERE MADONHANG = @maDonHang AND MATRAM = @stationId", conn))
                            {
                                cmd.Parameters.AddWithValue("@maDonHang", maDonHang);
                                cmd.Parameters.AddWithValue("@stationId", stationId);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        
                        // Xóa khỏi DataTable
                        dgvDonHang.Rows.Remove(dgvDonHang.CurrentRow);
                        
                        MessageBox.Show("Xóa đơn hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa đơn hàng: " + ex.Message);
                    }
                }
            }
        }
        
        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKyHieu.Text))
            {
                MessageBox.Show("Vui lòng nhập Ký hiệu đơn.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKyHieu.Focus();
                return;
            }
            
            try
            {
                // Lấy ID trạm hiện tại
                int stationId = DatabaseHelper.CurrentStationId;
                
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    
                    // Lấy giá trị từ ComboBox
                    int maTram = cbTramTron.SelectedIndex >= 0 ? Convert.ToInt32((cbTramTron.SelectedItem as dynamic).Value) : stationId;
                    int maKhach = cbKhachHang.SelectedIndex >= 0 ? Convert.ToInt32((cbKhachHang.SelectedItem as dynamic).Value) : 0;
                    int maCongTrinh = cbDiaDiem.SelectedIndex >= 0 ? Convert.ToInt32((cbDiaDiem.SelectedItem as dynamic).Value) : 0;
                    int maKinhDoanh = cbKinhDoanh.SelectedIndex >= 0 ? Convert.ToInt32((cbKinhDoanh.SelectedItem as dynamic).Value) : 0;
                    
                    decimal khoiLuong = 0;
                    decimal.TryParse(txtDatHang.Text, out khoiLuong);
                    
                    decimal tichLuy = 0;
                    decimal.TryParse(txtTichLuy.Text, out tichLuy);
                    
                    if (isAddingNew)
                    {
                        using (var cmd = new SqlCommand(@"
                        INSERT INTO DONHANG (NGAYDAT, KYHIEUDON, SOPHIEU, KHOILUONG, TICHLUY, MATRAM, MAKHACH, MAKINHDOANH, MACONGTRINH, HOATDONG)
                        VALUES (@ngaydat, @kyhieu, @sophieu, @khoiluong, @tichluy, @matram, @makhach, @makd, @mact, @hoatdong);
                        SELECT SCOPE_IDENTITY();", conn))
                        {
                            cmd.Parameters.Add("@ngaydat", SqlDbType.DateTime).Value = dtpNgay.Value;
                            cmd.Parameters.Add("@kyhieu", SqlDbType.NVarChar).Value = txtKyHieu.Text;
                            cmd.Parameters.Add("@sophieu", SqlDbType.NVarChar).Value = txtSoPhieu.Text;
                            cmd.Parameters.Add("@khoiluong", SqlDbType.Decimal).Value = khoiLuong;
                            cmd.Parameters.Add("@tichluy", SqlDbType.Decimal).Value = tichLuy;
                            cmd.Parameters.Add("@matram", SqlDbType.Int).Value = stationId; // Luôn sử dụng trạm hiện tại
                            cmd.Parameters.Add("@makhach", SqlDbType.Int).Value = maKhach;
                            cmd.Parameters.Add("@makd", SqlDbType.Int).Value = maKinhDoanh;
                            cmd.Parameters.Add("@mact", SqlDbType.Int).Value = maCongTrinh;
                            cmd.Parameters.Add("@hoatdong", SqlDbType.Bit).Value = chkHoatDong.Checked;
                            
                            int newId = Convert.ToInt32(cmd.ExecuteScalar());
                            
                            // Thêm vào DataTable
                            DataRow newRow = dtDonHang.NewRow();
                            newRow["Mã"] = newId;
                            newRow["Ký hiệu đơn"] = txtKyHieu.Text;
                            newRow["Khách hàng"] = cbKhachHang.Text;
                            newRow["M3 đặt hàng"] = khoiLuong;
                            newRow["Ngày tháng"] = dtpNgay.Value;
                            newRow["Số phiếu"] = txtSoPhieu.Text;
                            newRow["Tích lũy"] = tichLuy;
                            newRow["Địa điểm CT"] = cbDiaDiem.Text;
                            newRow["Hoạt động"] = chkHoatDong.Checked;
                            dtDonHang.Rows.Add(newRow);
                            
                            // Cập nhật DataGridView
                            dgvDonHang.DataSource = dtDonHang;
                            dgvDonHang.CurrentCell = dgvDonHang.Rows[dgvDonHang.Rows.Count - 1].Cells[0];
                            
                            MessageBox.Show("Thêm đơn hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        int maDonHang = Convert.ToInt32(txtMaDon.Text);
                        
                        using (var cmd = new SqlCommand(@"
                        UPDATE DONHANG 
                        SET NGAYDAT = @ngaydat, KYHIEUDON = @kyhieu, SOPHIEU = @sophieu, 
                            KHOILUONG = @khoiluong, TICHLUY = @tichluy, MATRAM = @matram, 
                            MAKHACH = @makhach, MAKINHDOANH = @makd, MACONGTRINH = @mact, HOATDONG = @hoatdong
                        WHERE MADONHANG = @maDonHang AND MATRAM = @stationId", conn))
                        {
                            cmd.Parameters.Add("@maDonHang", SqlDbType.Int).Value = maDonHang;
                            cmd.Parameters.Add("@ngaydat", SqlDbType.DateTime).Value = dtpNgay.Value;
                            cmd.Parameters.Add("@kyhieu", SqlDbType.NVarChar).Value = txtKyHieu.Text;
                            cmd.Parameters.Add("@sophieu", SqlDbType.NVarChar).Value = txtSoPhieu.Text;
                            cmd.Parameters.Add("@khoiluong", SqlDbType.Decimal).Value = khoiLuong;
                            cmd.Parameters.Add("@tichluy", SqlDbType.Decimal).Value = tichLuy;
                            cmd.Parameters.Add("@matram", SqlDbType.Int).Value = stationId; // Luôn sử dụng trạm hiện tại
                            cmd.Parameters.Add("@makhach", SqlDbType.Int).Value = maKhach;
                            cmd.Parameters.Add("@makd", SqlDbType.Int).Value = maKinhDoanh;
                            cmd.Parameters.Add("@mact", SqlDbType.Int).Value = maCongTrinh;
                            cmd.Parameters.Add("@hoatdong", SqlDbType.Bit).Value = chkHoatDong.Checked;
                            cmd.Parameters.Add("@stationId", SqlDbType.Int).Value = stationId; // Điều kiện WHERE
                            
                            cmd.ExecuteNonQuery();
                            
                            // Cập nhật DataTable
                            var row = dgvDonHang.CurrentRow;
                            if (row != null)
                            {
                                row.Cells["Mã"].Value = maDonHang;
                                row.Cells["Ký hiệu đơn"].Value = txtKyHieu.Text;
                                row.Cells["Khách hàng"].Value = cbKhachHang.Text;
                                row.Cells["M3 đặt hàng"].Value = khoiLuong;
                                row.Cells["Ngày tháng"].Value = dtpNgay.Value;
                                row.Cells["Số phiếu"].Value = txtSoPhieu.Text;
                                row.Cells["Tích lũy"].Value = tichLuy;
                                row.Cells["Địa điểm CT"].Value = cbDiaDiem.Text;
                                row.Cells["Hoạt động"].Value = chkHoatDong.Checked;
                            }
                            
                            MessageBox.Show("Cập nhật đơn hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                
                isAddingNew = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu đơn hàng: " + ex.Message);
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