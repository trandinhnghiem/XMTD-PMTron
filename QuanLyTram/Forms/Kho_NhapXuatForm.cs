using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp; // Thêm thư viện FontAwesome
using QuanLyTram.DAL;

namespace QuanLyTram.Forms
{
    public class Kho_NhapXuatForm : Form
    {
        private IconButton btnThemMoi, btnCapNhat, btnLuu, btnInPhieu; // Thay đổi từ Button sang IconButton
        private ComboBox cbTram, cbNhapXuat, cbDonVi, cbPhuongTien, cbDonViVC, cbNhaCungCap;
        private TextBox txtVatLieu, txtSoPhieu, txtSoHoaDon, txtSLNhapXuat, txtQuyDoi, txtLaiXe, txtSoLuongTon;
        private DateTimePicker dtpNgay;
        private DataGridView dgvKho;
        private DataTable dtKho;
        private bool isAddingNew = false;
        
        public Kho_NhapXuatForm()
        {
            this.Text = "NHẬT XUẤT KHO";
            this.Size = new Size(1250, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;
            Font btnFont = new Font("Segoe UI", 10, FontStyle.Bold);
            
            // ================== Thanh nút chức năng ==================
            btnThemMoi = new IconButton()
            {
                Text = "THÊM MỚI",
                Width = 150,
                Height = 50,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(46,204,113), // Màu xanh lá
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
            
            btnCapNhat = new IconButton()
            {
                Text = "CẬP NHẬT",
                Width = 150,
                Height = 50,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(65, 131, 215), // Màu xanh dương
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
            
            btnLuu = new IconButton()
            {
                Text = "LƯU",
                Width = 150,
                Height = 50,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.MediumPurple, // Màu tím
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
            btnLuu.Location = new Point(360, 20);
            
            btnInPhieu = new IconButton()
            {
                Text = "IN PHIẾU",
                Width = 150,
                Height = 50,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(238,180,34), 
                FlatStyle = FlatStyle.Flat,
                IconChar = IconChar.Print,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleCenter,
                ImageAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(15, 0, 0, 0),
                Cursor = Cursors.Hand,
            };
            btnInPhieu.FlatAppearance.BorderSize = 0;
            btnInPhieu.Location = new Point(530, 20);
            
            this.Controls.AddRange(new Control[] { btnThemMoi, btnCapNhat, btnLuu, btnInPhieu });
            
            // ================== GroupBox Thông tin nhập xuất ==================
            GroupBox groupInfo = new GroupBox()
            {
                Text = "Thông tin nhập xuất",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(1200, 200),
                Location = new Point(20, 90)
            };
            
            int  txtW = 220;
            // Trạm
            Label lblTram = new Label() { Text = "Trạm:", Location = new Point(20, 35), AutoSize = true };
            cbTram = new ComboBox() { Location = new Point(140, 30), Width = txtW, DropDownStyle = ComboBoxStyle.DropDownList };
            // Nhập xuất
            Label lblNhapXuat = new Label() { Text = "Nhập/Xuất:", Location = new Point(20, 70), AutoSize = true };
            cbNhapXuat = new ComboBox() { Location = new Point(140, 65), Width = txtW, DropDownStyle = ComboBoxStyle.DropDownList };
            cbNhapXuat.Items.AddRange(new string[] { "Nhập", "Xuất", "Tịnh kho" });
            // Tên VL
            Label lblVatLieu = new Label() { Text = "Tên vật liệu:", Location = new Point(20, 105), AutoSize = true };
            txtVatLieu = new TextBox() { Location = new Point(140, 100), Width = txtW };
            // Đơn vị
            Label lblDonVi = new Label() { Text = "Đơn vị:", Location = new Point(20, 140), AutoSize = true };
            cbDonVi = new ComboBox() { Location = new Point(140, 135), Width = 80, DropDownStyle = ComboBoxStyle.DropDownList };
            Label lblQuyDoi = new Label() { Text = "Quy đổi:", Location = new Point(230, 140), AutoSize = true };
            txtQuyDoi = new TextBox() { Location = new Point(300, 135), Width = 60 };
            // Số phiếu
            Label lblSoPhieu = new Label() { Text = "Số phiếu:", Location = new Point(400, 35), AutoSize = true };
            txtSoPhieu = new TextBox() { Location = new Point(530, 30), Width = txtW };
            // Số hóa đơn
            Label lblSoHoaDon = new Label() { Text = "Số hóa đơn:", Location = new Point(400, 70), AutoSize = true };
            txtSoHoaDon = new TextBox() { Location = new Point(530, 65), Width = txtW };
            // Ngày
            Label lblNgay = new Label() { Text = "Ngày:", Location = new Point(400, 105), AutoSize = true };
            dtpNgay = new DateTimePicker()
            {
                Location = new Point(530, 100),
                Width = txtW,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy"
            };
            // SL Nhập/Xuất
            Label lblSLNX = new Label() { Text = "SL nhập xuất:", Location = new Point(400, 140), AutoSize = true };
            txtSLNhapXuat = new TextBox() { Location = new Point(530, 135), Width = txtW };
            // Quy đổi về KG
            Label lblQuyDoiKg = new Label() { Text = "Quy đổi về Kg:", Location = new Point(780, 35), AutoSize = true };
            TextBox txtQuyDoiKg = new TextBox() { Location = new Point(915, 30), Width = txtW, ReadOnly = true };
            // Phương tiện
            Label lblPhuongTien = new Label() { Text = "Tên phương tiện:", Location = new Point(780, 70), AutoSize = true };
            cbPhuongTien = new ComboBox() { Location = new Point(915, 65), Width = txtW };
            // Lái xe
            Label lblLaiXe = new Label() { Text = "Tên lái xe:", Location = new Point(780, 105), AutoSize = true };
            txtLaiXe = new TextBox() { Location = new Point(915, 100), Width = txtW };
            // Đơn vị VC
            Label lblDonViVC = new Label() { Text = "Đơn vị vận chuyển:", Location = new Point(780, 140), AutoSize = true };
            cbDonViVC = new ComboBox() { Location = new Point(915, 135), Width = txtW };
            // Nhà cung cấp
            Label lblNCC = new Label() { Text = "Nhà cung cấp:", Location = new Point(780, 175), AutoSize = true };
            cbNhaCungCap = new ComboBox() { Location = new Point(915, 170), Width = txtW };
            // Số lượng tồn
            Label lblTon = new Label() { Text = "Số lượng tồn:", Location = new Point(400, 175), AutoSize = true };
            txtSoLuongTon = new TextBox() { Location = new Point(530, 170), Width = txtW, ReadOnly = true };
            
            groupInfo.Controls.AddRange(new Control[] {
                lblTram, cbTram,
                lblNhapXuat, cbNhapXuat,
                lblVatLieu, txtVatLieu,
                lblDonVi, cbDonVi, lblQuyDoi, txtQuyDoi,
                lblSoPhieu, txtSoPhieu,
                lblSoHoaDon, txtSoHoaDon,
                lblNgay, dtpNgay,
                lblSLNX, txtSLNhapXuat,
                lblQuyDoiKg, txtQuyDoiKg,
                lblPhuongTien, cbPhuongTien,
                lblLaiXe, txtLaiXe,
                lblDonViVC, cbDonViVC,
                lblNCC, cbNhaCungCap,
                lblTon, txtSoLuongTon
            });
            
            this.Controls.Add(groupInfo);
            
            // ================== DataGridView Danh sách ==================
            dgvKho = new DataGridView()
            {
                Location = new Point(20, 310),
                Size = new Size(1200, 350),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                AutoGenerateColumns = false, // Ngăn tự động tạo cột
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true
            };
            
            // Tạo các cột thủ công
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "STT", HeaderText = "STT", Width = 50 });
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoPhieu", HeaderText = "Số phiếu" });
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoHoaDon", HeaderText = "Số hóa đơn" });
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "NhapXuat", HeaderText = "Nhập/Xuất" });
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "ThoiGian", HeaderText = "Thời gian" });
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenVatLieu", HeaderText = "Tên vật liệu" });
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "BienXe", HeaderText = "Phương tiện" });
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "LaiXe", HeaderText = "Lái xe" });
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "DonViVC", HeaderText = "Đơn vị VC" });
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "NhaCungCap", HeaderText = "Nhà cung cấp" });
            
            // Thêm cột ẩn để lưu MAKHO
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "MAKHO", HeaderText = "MAKHO", Visible = false });
            
            this.Controls.Add(dgvKho);
            
            // Load dữ liệu
            LoadData();
            
            // Load dữ liệu cho ComboBox
            LoadComboBoxData();
            
            // Đăng ký sự kiện
            btnThemMoi.Click += BtnThemMoi_Click;
            btnCapNhat.Click += BtnCapNhat_Click;
            btnLuu.Click += BtnLuu_Click;
            dgvKho.SelectionChanged += DgvKho_SelectionChanged;
            cbDonVi.SelectedIndexChanged += CbDonVi_SelectedIndexChanged;
            txtSLNhapXuat.TextChanged += TxtSLNhapXuat_TextChanged;
            txtQuyDoi.TextChanged += TxtQuyDoi_TextChanged;
        }
        
        private void LoadData()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(@"
                    SELECT k.MAKHO, k.SOPHIEU, k.SOHOPDONG, k.LOAIGIAODICH, k.NGAYGIAODICH, 
                           v.TENVATTU, k.PHUONGTIEN, k.LAIXE, k.DONVIVANCHUYEN, 
                           k.NHACUNGCAP, k.SOLUONG, k.TONKHO, k.MAVATTU
                    FROM KHO k
                    JOIN VATTU v ON k.MAVATTU = v.MAVATTU
                    ORDER BY k.NGAYGIAODICH DESC", conn))
                    {
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            dtKho = new DataTable();
                            adapter.Fill(dtKho);
                            
                            // Xóa dữ liệu hiện có trong DataGridView
                            dgvKho.Rows.Clear();
                            
                            // Thêm dữ liệu vào DataGridView
                            for (int i = 0; i < dtKho.Rows.Count; i++)
                            {
                                var row = dtKho.Rows[i];
                                dgvKho.Rows.Add(
                                    i + 1, // STT
                                    row["SOPHIEU"],
                                    row["SOHOPDONG"],
                                    row["LOAIGIAODICH"],
                                    Convert.ToDateTime(row["NGAYGIAODICH"]).ToString("dd/MM/yyyy"),
                                    row["TENVATTU"],
                                    row["PHUONGTIEN"],
                                    row["LAIXE"],
                                    row["DONVIVANCHUYEN"],
                                    row["NHACUNGCAP"],
                                    row["MAKHO"] // Cột ẩn
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu kho: " + ex.Message);
            }
        }
        
        private void LoadComboBoxData()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    
                    // Load trạm
                    cbTram.Items.Clear();
                    using (var cmd = new SqlCommand("SELECT MATRAM, TENTRAM FROM TRAM", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            DataTable dtTram = new DataTable();
                            dtTram.Load(reader);
                            cbTram.DisplayMember = "TENTRAM";
                            cbTram.ValueMember = "MATRAM";
                            cbTram.DataSource = dtTram;
                        }
                    }
                    if (cbTram.Items.Count > 0) cbTram.SelectedIndex = 0;
                    
                    // Load vật liệu
                    cbDonVi.Items.Clear();
                    using (var cmd = new SqlCommand("SELECT MAVATTU, TENVATTU, DONVITINH, HESOQUYDOI FROM VATTU", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            DataTable dtVatTu = new DataTable();
                            dtVatTu.Load(reader);
                            cbDonVi.DisplayMember = "TENVATTU";
                            cbDonVi.ValueMember = "MAVATTU";
                            cbDonVi.DataSource = dtVatTu;
                        }
                    }
                    if (cbDonVi.Items.Count > 0) cbDonVi.SelectedIndex = 0;
                    
                    // Load phương tiện
                    cbPhuongTien.Items.Clear();
                    using (var cmd = new SqlCommand("SELECT DISTINCT PHUONGTIEN FROM KHO WHERE PHUONGTIEN IS NOT NULL", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cbPhuongTien.Items.Add(reader["PHUONGTIEN"].ToString());
                            }
                        }
                    }
                    if (cbPhuongTien.Items.Count > 0) cbPhuongTien.SelectedIndex = 0;
                    
                    // Load đơn vị vận chuyển
                    cbDonViVC.Items.Clear();
                    using (var cmd = new SqlCommand("SELECT DISTINCT DONVIVANCHUYEN FROM KHO WHERE DONVIVANCHUYEN IS NOT NULL", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cbDonViVC.Items.Add(reader["DONVIVANCHUYEN"].ToString());
                            }
                        }
                    }
                    if (cbDonViVC.Items.Count > 0) cbDonViVC.SelectedIndex = 0;
                    
                    // Load nhà cung cấp
                    cbNhaCungCap.Items.Clear();
                    using (var cmd = new SqlCommand("SELECT DISTINCT NHACUNGCAP FROM KHO WHERE NHACUNGCAP IS NOT NULL", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cbNhaCungCap.Items.Add(reader["NHACUNGCAP"].ToString());
                            }
                        }
                    }
                    if (cbNhaCungCap.Items.Count > 0) cbNhaCungCap.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu ComboBox: " + ex.Message);
            }
        }
        
        private void CbDonVi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDonVi.SelectedItem != null)
            {
                // Lấy thông tin vật liệu từ DataTable
                DataRowView row = cbDonVi.SelectedItem as DataRowView;
                if (row != null)
                {
                    txtVatLieu.Text = row["TENVATTU"].ToString();
                    txtQuyDoi.Text = row["HESOQUYDOI"].ToString();
                    
                    // Tính toán lại quy đổi về kg
                    CalculateQuyDoiKg();
                }
            }
        }
        
        private void TxtSLNhapXuat_TextChanged(object sender, EventArgs e)
        {
            CalculateQuyDoiKg();
        }
        
        private void TxtQuyDoi_TextChanged(object sender, EventArgs e)
        {
            CalculateQuyDoiKg();
        }
        
        private void CalculateQuyDoiKg()
        {
            decimal soLuong = 0;
            decimal quyDoi = 0;
            
            decimal.TryParse(txtSLNhapXuat.Text, out soLuong);
            decimal.TryParse(txtQuyDoi.Text, out quyDoi);
            
            // Tính quy đổi về kg
            decimal quyDoiKg = soLuong * quyDoi;
            
            // Hiển thị kết quả (giả sử có TextBox txtQuyDoiKg)
            foreach (Control control in this.Controls)
            {
                if (control is GroupBox)
                {
                    foreach (Control innerControl in control.Controls)
                    {
                        if (innerControl is TextBox && innerControl.Name == "txtQuyDoiKg")
                        {
                            innerControl.Text = quyDoiKg.ToString();
                            break;
                        }
                    }
                }
            }
        }
        
        private void BtnThemMoi_Click(object sender, EventArgs e)
        {
            isAddingNew = true;
            // Clear form
            txtSoPhieu.Text = "";
            txtSoHoaDon.Text = "";
            txtSLNhapXuat.Text = "";
            txtQuyDoi.Text = "";
            txtLaiXe.Text = "";
            txtSoLuongTon.Text = "";
            dtpNgay.Value = DateTime.Today;
            cbNhapXuat.SelectedIndex = 0;
            
            // Focus vào ô đầu tiên
            txtSoPhieu.Focus();
        }
        
        private void BtnCapNhat_Click(object sender, EventArgs e)
        {
            if (dgvKho.CurrentRow != null)
            {
                isAddingNew = false;
                LoadRowToForm(dgvKho.CurrentRow);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một bản ghi để cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void LoadRowToForm(DataGridViewRow row)
        {
            try
            {
                // Lấy MAKHO từ dòng được chọn
                int maKho = Convert.ToInt32(row.Cells["MAKHO"].Value);
                
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    
                    // Lấy thông tin chi tiết của phiếu kho
                    using (var cmd = new SqlCommand(@"
                    SELECT k.*, v.TENVATTU, v.DONVITINH, v.HESOQUYDOI
                    FROM KHO k
                    JOIN VATTU v ON k.MAVATTU = v.MAVATTU
                    WHERE k.MAKHO = @maKho", conn))
                    {
                        cmd.Parameters.AddWithValue("@maKho", maKho);
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtSoPhieu.Text = reader["SOPHIEU"].ToString();
                                txtSoHoaDon.Text = reader["SOHOPDONG"].ToString();
                                cbNhapXuat.Text = reader["LOAIGIAODICH"].ToString();
                                dtpNgay.Value = Convert.ToDateTime(reader["NGAYGIAODICH"]);
                                txtVatLieu.Text = reader["TENVATTU"].ToString();
                                cbPhuongTien.Text = reader["PHUONGTIEN"].ToString();
                                txtLaiXe.Text = reader["LAIXE"].ToString();
                                cbDonViVC.Text = reader["DONVIVANCHUYEN"].ToString();
                                cbNhaCungCap.Text = reader["NHACUNGCAP"].ToString();
                                txtSLNhapXuat.Text = reader["SOLUONG"].ToString();
                                txtSoLuongTon.Text = reader["TONKHO"].ToString();
                                
                                // Chọn vật liệu trong combobox
                                int maVatTu = Convert.ToInt32(reader["MAVATTU"]);
                                for (int i = 0; i < cbDonVi.Items.Count; i++)
                                {
                                    DataRowView item = cbDonVi.Items[i] as DataRowView;
                                    if (item != null && Convert.ToInt32(item["MAVATTU"]) == maVatTu)
                                    {
                                        cbDonVi.SelectedIndex = i;
                                        break;
                                    }
                                }
                                
                                // Chọn trạm trong combobox
                                int maTram = Convert.ToInt32(reader["MATRAM"]);
                                for (int i = 0; i < cbTram.Items.Count; i++)
                                {
                                    DataRowView item = cbTram.Items[i] as DataRowView;
                                    if (item != null && Convert.ToInt32(item["MATRAM"]) == maTram)
                                    {
                                        cbTram.SelectedIndex = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu chi tiết: " + ex.Message);
            }
        }
        
        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSoPhieu.Text))
            {
                MessageBox.Show("Vui lòng nhập Số phiếu.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoPhieu.Focus();
                return;
            }
            
            if (cbDonVi.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn vật liệu.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbDonVi.Focus();
                return;
            }
            
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    
                    // Lấy giá trị từ ComboBox
                    int maTram = cbTram.SelectedValue != null ? Convert.ToInt32(cbTram.SelectedValue) : 0;
                    int maVatTu = cbDonVi.SelectedValue != null ? Convert.ToInt32(cbDonVi.SelectedValue) : 0;
                    
                    decimal soLuong = 0;
                    decimal.TryParse(txtSLNhapXuat.Text, out soLuong);
                    
                    decimal quyDoi = 0;
                    decimal.TryParse(txtQuyDoi.Text, out quyDoi);
                    
                    decimal soLuongKg = soLuong * quyDoi;
                    
                    decimal tonKho = 0;
                    decimal.TryParse(txtSoLuongTon.Text, out tonKho);
                    
                    if (isAddingNew)
                    {
                        using (var cmd = new SqlCommand(@"
                        INSERT INTO KHO (NGAYGIAODICH, LOAIGIAODICH, MAVATTU, SOPHIEU, SOHOPDONG, 
                                        SOLUONG, SOLUONGKG, PHUONGTIEN, LAIXE, DONVIVANCHUYEN, 
                                        NHACUNGCAP, TONKHO, MATRAM)
                        VALUES (@ngay, @loai, @mavt, @sophieu, @sohd, @soluong, @slkg, 
                                @pt, @laixe, @donvivc, @ncc, @tonkho, @matram);
                        SELECT SCOPE_IDENTITY();", conn))
                        {
                            cmd.Parameters.Add("@ngay", SqlDbType.DateTime).Value = dtpNgay.Value;
                            cmd.Parameters.Add("@loai", SqlDbType.NVarChar).Value = cbNhapXuat.SelectedItem.ToString();
                            cmd.Parameters.Add("@mavt", SqlDbType.Int).Value = maVatTu;
                            cmd.Parameters.Add("@sophieu", SqlDbType.NVarChar).Value = txtSoPhieu.Text;
                            cmd.Parameters.Add("@sohd", SqlDbType.NVarChar).Value = txtSoHoaDon.Text;
                            cmd.Parameters.Add("@soluong", SqlDbType.Decimal).Value = soLuong;
                            cmd.Parameters.Add("@slkg", SqlDbType.Decimal).Value = soLuongKg;
                            cmd.Parameters.Add("@pt", SqlDbType.NVarChar).Value = cbPhuongTien.Text;
                            cmd.Parameters.Add("@laixe", SqlDbType.NVarChar).Value = txtLaiXe.Text;
                            cmd.Parameters.Add("@donvivc", SqlDbType.NVarChar).Value = cbDonViVC.Text;
                            cmd.Parameters.Add("@ncc", SqlDbType.NVarChar).Value = cbNhaCungCap.Text;
                            cmd.Parameters.Add("@tonkho", SqlDbType.Decimal).Value = tonKho;
                            cmd.Parameters.Add("@matram", SqlDbType.Int).Value = maTram;
                            
                            int newId = Convert.ToInt32(cmd.ExecuteScalar());
                            
                            // Load lại dữ liệu
                            LoadData();
                            
                            MessageBox.Show("Thêm phiếu nhập xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        if (dgvKho.CurrentRow == null)
                        {
                            MessageBox.Show("Vui lòng chọn một bản ghi để cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        
                        int maKho = Convert.ToInt32(dgvKho.CurrentRow.Cells["MAKHO"].Value);
                        
                        using (var cmd = new SqlCommand(@"
                        UPDATE KHO 
                        SET NGAYGIAODICH = @ngay, LOAIGIAODICH = @loai, MAVATTU = @mavt, SOPHIEU = @sophieu, 
                            SOHOPDONG = @sohd, SOLUONG = @soluong, SOLUONGKG = @slkg, PHUONGTIEN = @pt, 
                            LAIXE = @laixe, DONVIVANCHUYEN = @donvivc, NHACUNGCAP = @ncc, 
                            TONKHO = @tonkho, MATRAM = @matram
                        WHERE MAKHO = @maKho", conn))
                        {
                            cmd.Parameters.Add("@maKho", SqlDbType.Int).Value = maKho;
                            cmd.Parameters.Add("@ngay", SqlDbType.DateTime).Value = dtpNgay.Value;
                            cmd.Parameters.Add("@loai", SqlDbType.NVarChar).Value = cbNhapXuat.SelectedItem.ToString();
                            cmd.Parameters.Add("@mavt", SqlDbType.Int).Value = maVatTu;
                            cmd.Parameters.Add("@sophieu", SqlDbType.NVarChar).Value = txtSoPhieu.Text;
                            cmd.Parameters.Add("@sohd", SqlDbType.NVarChar).Value = txtSoHoaDon.Text;
                            cmd.Parameters.Add("@soluong", SqlDbType.Decimal).Value = soLuong;
                            cmd.Parameters.Add("@slkg", SqlDbType.Decimal).Value = soLuongKg;
                            cmd.Parameters.Add("@pt", SqlDbType.NVarChar).Value = cbPhuongTien.Text;
                            cmd.Parameters.Add("@laixe", SqlDbType.NVarChar).Value = txtLaiXe.Text;
                            cmd.Parameters.Add("@donvivc", SqlDbType.NVarChar).Value = cbDonViVC.Text;
                            cmd.Parameters.Add("@ncc", SqlDbType.NVarChar).Value = cbNhaCungCap.Text;
                            cmd.Parameters.Add("@tonkho", SqlDbType.Decimal).Value = tonKho;
                            cmd.Parameters.Add("@matram", SqlDbType.Int).Value = maTram;
                            
                            cmd.ExecuteNonQuery();
                            
                            // Load lại dữ liệu
                            LoadData();
                            
                            MessageBox.Show("Cập nhật phiếu nhập xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                
                isAddingNew = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu phiếu nhập xuất: " + ex.Message);
            }
        }
        
        private void DgvKho_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvKho.CurrentRow != null && !isAddingNew)
            {
                LoadRowToForm(dgvKho.CurrentRow);
            }
        }
    }
}