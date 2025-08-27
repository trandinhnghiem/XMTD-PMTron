using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QuanLyTram.DAL;

namespace QuanLyTram.Forms
{
    public class InPhieuForm : Form
    {
        public InPhieuForm()
        {
            // Form
            Text = "PHIẾU GIAO NHẬN BÊ TÔNG";
            Size = new Size(1260, 740);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.LightYellow;
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            
            // --- Panel Trái ---
            Panel panelLeft = new Panel()
            {
                Location = new Point(10, 10),
                Size = new Size(250, 660),
                BackColor = Color.Transparent
            };
            
            // Chọn phiếu
            GroupBox grpChonPhieu = CreateGroupBox("CHỌN PHIẾU", 0, 0, 240, 300);
            Label lblNgay = CreateLabelBold("Ngày:", 10, 25);
            DateTimePicker dtpNgay = new DateTimePicker() { Location = new Point(60, 20), Width = 150, Format = DateTimePickerFormat.Short, ForeColor = Color.Black };
            Button btnTim = CreateButton("Tìm", 60, 50, 150);
            ListBox lstPhieu = new ListBox() { Location = new Point(10, 85), Size = new Size(210, 200), ForeColor = Color.Black };
            
            // Load dữ liệu phiếu
            LoadPhieuData(lstPhieu);
            
            grpChonPhieu.Controls.AddRange(new Control[] { lblNgay, dtpNgay, btnTim, lstPhieu });
            ResetChildControls(grpChonPhieu);
            
            // Tùy chọn in
            GroupBox grpIn = CreateGroupBox("TÙY CHỌN IN", 0, 300, 240, 140);
            RadioButton radMau1 = new RadioButton()
            {
                Text = "Mẫu 1 (In chi tiết)",
                Location = new Point(10, 20),
                AutoSize = true,
                ForeColor = Color.Blue,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Checked = true
            };
            RadioButton radMau2 = new RadioButton()
            {
                Text = "Mẫu 2 (In chi tiết)",
                Location = new Point(10, 40),
                AutoSize = true,
                ForeColor = Color.Red,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            RadioButton radMau3 = new RadioButton()
            {
                Text = "Mẫu 3 (In chi tiết)",
                Location = new Point(10, 60),
                AutoSize = true,
                ForeColor = Color.Orange,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            RadioButton radMau4 = new RadioButton()
            {
                Text = "Mẫu 4 (In tổng)",
                Location = new Point(10, 80),
                AutoSize = true,
                ForeColor = Color.Purple,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            Button btnSave = new Button()
            {
                Text = "💾 Lưu",
                Location = new Point(150, 20),
                Size = new Size(80, 35),
                ForeColor = Color.SeaGreen,
                Font = new Font("Segoe UI Emoji", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            Button btnPrint = new Button()
            {
                Text = "🖨 In",
                Location = new Point(150, 65),
                Size = new Size(80, 35),
                ForeColor = Color.RoyalBlue,
                Font = new Font("Segoe UI Emoji", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            grpIn.Controls.AddRange(new Control[] { radMau1, radMau2, radMau3, radMau4, btnSave, btnPrint });
            
            // Thời gian trộn
            GroupBox grpThoiGian = CreateGroupBox("THỜI GIAN TRỘN", 0, 450, 240, 120);
            Label lblBD = CreateLabelBold("Thời gian bắt đầu", 10, 25);
            TextBox txtBD = new TextBox() { Location = new Point(130, 20), Width = 90, Text = "08:30", ForeColor = Color.Black };
            Label lblKT = CreateLabelBold("Thời gian kết thúc", 10, 60);
            TextBox txtKT = new TextBox() { Location = new Point(130, 55), Width = 90, Text = "09:15", ForeColor = Color.Black };
            grpThoiGian.Controls.AddRange(new Control[] { lblBD, txtBD, lblKT, txtKT });
            ResetChildControls(grpThoiGian);
            panelLeft.Controls.AddRange(new Control[] { grpChonPhieu, grpIn, grpThoiGian });
            Controls.Add(panelLeft);
            
            // --- Panel Phải ---
            Panel panelRight = new Panel()
            {
                Location = new Point(270, 10),
                Size = new Size(980, 660),
                BackColor = Color.Transparent
            };
            
            // Thông tin phiếu xuất
            GroupBox grpThongTin = CreateGroupBox("THÔNG TIN PHIẾU XUẤT", 0, 0, 960, 140);
            Label lblKH = CreateLabelBold("Khách hàng:", 10, 25);
            ComboBox cboKH = new ComboBox() { Location = new Point(100, 20), Width = 220, ForeColor = Color.Black };
            Label lblMaPhieu = CreateLabelBold("Mã phiếu:", 340, 25);
            TextBox txtMaPhieu = new TextBox() { Location = new Point(415, 20), Width = 180, Text = "PX001", ForeColor = Color.Black };
            Label lblDD = CreateLabelBold("Địa điểm XD:", 10, 60);
            ComboBox cboDD = new ComboBox() { Location = new Point(100, 55), Width = 220, ForeColor = Color.Black };
            Label lblSoPhieu = CreateLabelBold("Số phiếu:", 340, 60);
            TextBox txtSoPhieu = new TextBox() { Location = new Point(415, 55), Width = 180, Text = "S001", ForeColor = Color.Black };
            Label lblHM = CreateLabelBold("Hạng mục:", 10, 95);
            ComboBox cboHM = new ComboBox() { Location = new Point(100, 90), Width = 220, ForeColor = Color.Black };
            Label lblNgayTron = CreateLabelBold("Ngày trộn:", 340, 95);
            DateTimePicker dtpTron = new DateTimePicker() { Location = new Point(415, 90), Width = 180, Format = DateTimePickerFormat.Short, Value = DateTime.Today, ForeColor = Color.Black };
            Label lblTB = CreateLabelBold("Thiết bị bơm:", 620, 25);
            ComboBox cboTB = new ComboBox() { Location = new Point(710, 20), Width = 220, ForeColor = Color.Black };
            Label lblMacBT = CreateLabelBold("Mác bê tông:", 620, 60);
            TextBox txtMacBT = new TextBox() { Location = new Point(710, 55), Width = 220, Text = "M300", ForeColor = Color.Black };
            CheckBox chkBom = new CheckBox() { Text = "Sử dụng bơm", Location = new Point(710, 95), AutoSize = true, Checked = true, ForeColor = Color.Black };
            
            // Load dữ liệu ComboBox
            LoadComboBoxData(cboKH, cboDD, cboHM, cboTB);
            
            grpThongTin.Controls.AddRange(new Control[] {
                lblKH, cboKH, lblMaPhieu, txtMaPhieu,
                lblDD, cboDD, lblSoPhieu, txtSoPhieu,
                lblHM, cboHM, lblNgayTron, dtpTron,
                lblTB, cboTB, lblMacBT, txtMacBT, chkBom
            });
            ResetChildControls(grpThongTin);
            
            // Tổng khối lượng
            GroupBox grpKhoiLuong = CreateGroupBox("TỔNG KHỐI LƯỢNG", 0, 150, 480, 280);
            DataGridView dgvKhoiLuong = new DataGridView()
            {
                ColumnCount = 2,
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToResizeColumns = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.None,
                ForeColor = Color.Black
            };
            // dgvKhoiLuong
            dgvKhoiLuong.Columns[0].Name = "VẬT LIỆU";
            dgvKhoiLuong.Columns[1].Name = "SỐ LƯỢNG";
            dgvKhoiLuong.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold); // 👈
            dgvKhoiLuong.Rows.Add("XI MĂNG", "500 Kg");
            dgvKhoiLuong.Rows.Add("CÁT", "1200 Kg");
            dgvKhoiLuong.Rows.Add("ĐÁ", "1500 Kg");
            dgvKhoiLuong.Rows.Add("NƯỚC", "250 L");
            dgvKhoiLuong.Rows.Add("PHỤ GIA", "20 Kg");
            dgvKhoiLuong.AllowUserToAddRows = false;
            grpKhoiLuong.Controls.Add(dgvKhoiLuong);
            ResetChildControls(grpKhoiLuong);
            
            // Thông số
            GroupBox grpThongSo = CreateGroupBox("THÔNG SỐ", 490, 150, 470, 280);
            TableLayoutPanel tblThongSo = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1
            };
            DataGridView dgv1 = CreateGrid(4, 2);
            dgv1.Rows[0].Cells[0].Value = "Xe";
            dgv1.Rows[0].Cells[1].Value = "Biển số";
            dgv1.Rows[1].Cells[0].Value = "Xe 1";
            dgv1.Rows[1].Cells[1].Value = "51D-12345";
            DataGridView dgv2 = CreateGrid(5, 2);
            dgv2.Rows[0].Cells[0].Value = "STT";
            dgv2.Rows[0].Cells[1].Value = "Khối lượng";
            dgv2.Rows[1].Cells[0].Value = "1";
            dgv2.Rows[1].Cells[1].Value = "7 m³";
            DataGridView dgv3 = CreateGrid(5, 4);
            dgv3.Rows[0].Cells[0].Value = "STT";
            dgv3.Rows[0].Cells[1].Value = "Tên";
            dgv3.Rows[0].Cells[2].Value = "SL";
            dgv3.Rows[0].Cells[3].Value = "Ghi chú";
            dgv3.Rows[1].Cells[0].Value = "1";
            dgv3.Rows[1].Cells[1].Value = "Xe bồn";
            dgv3.Rows[1].Cells[2].Value = "2";
            dgv3.Rows[1].Cells[3].Value = "Chở đủ tải";
            tblThongSo.Controls.Add(dgv1, 0, 0);
            tblThongSo.Controls.Add(dgv2, 0, 1);
            tblThongSo.Controls.Add(dgv3, 0, 2);
            grpThongSo.Controls.Add(tblThongSo);
            ResetChildControls(grpThongSo);
            
            // Thông tin chi tiết
            GroupBox grpChiTiet = CreateGroupBox("THÔNG TIN CHI TIẾT", 0, 430, 960, 270);
            Panel pnlChiTiet = new Panel() { Dock = DockStyle.Fill };
            DataGridView dgvChiTiet = new DataGridView()
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.Gainsboro,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToResizeColumns = false,
                AllowUserToResizeRows = false,
                ForeColor = Color.Black
            };
            // dgvChiTiet
            dgvChiTiet.ColumnCount = 5;
            dgvChiTiet.Columns[0].Name = "STT";
            dgvChiTiet.Columns[1].Name = "Xe";
            dgvChiTiet.Columns[2].Name = "Khối lượng (m³)";
            dgvChiTiet.Columns[3].Name = "Thời gian xuất";
            dgvChiTiet.Columns[4].Name = "Ghi chú";
            dgvChiTiet.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold); // 👈
            dgvChiTiet.Rows.Add("1", "51D-12345", "7.0", "08:45", "OK");
            dgvChiTiet.Rows.Add("2", "51D-67890", "6.5", "09:10", "Trễ 5p");
            dgvChiTiet.Rows.Add("3", "51C-54321", "7.5", "09:40", "OK");
            pnlChiTiet.Controls.Add(dgvChiTiet);
            grpChiTiet.Controls.Add(pnlChiTiet);
            ResetChildControls(grpChiTiet);
            panelRight.Controls.AddRange(new Control[] { grpThongTin, grpKhoiLuong, grpThongSo, grpChiTiet });
            
            // Add panels vào form
            Controls.Add(panelRight);
            Controls.Add(panelLeft);
            
            // Đăng ký sự kiện
            btnTim.Click += (s, e) => {
                // Tìm phiếu theo ngày
                LoadPhieuData(lstPhieu, dtpNgay.Value);
            };
            
            lstPhieu.SelectedIndexChanged += (s, e) => {
                if (lstPhieu.SelectedIndex >= 0)
                {
                    // Load thông tin chi tiết phiếu
                    string selectedPhieu = lstPhieu.SelectedItem.ToString();
                    string maPhieu = selectedPhieu.Split('-')[0].Trim();
                    LoadPhieuChiTiet(maPhieu, txtMaPhieu, cboKH, cboDD, cboHM, txtMacBT, cboTB, chkBom, dgvKhoiLuong, dgvChiTiet);
                }
            };
        }
        
        private void LoadPhieuData(ListBox lstPhieu, DateTime? selectedDate = null)
        {
            try
            {
                lstPhieu.Items.Clear();
                
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    
                    string query = "SELECT MAPHIEUXUAT, NGAYXUAT, KH.TENKHACH FROM PHIEUXUAT px JOIN KHACHHANG KH ON px.MAKHACH = KH.MAKHACH";
                    
                    if (selectedDate.HasValue)
                    {
                        query += " WHERE CAST(NGAYXUAT AS DATE) = @ngay";
                    }
                    
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        if (selectedDate.HasValue)
                        {
                            cmd.Parameters.Add("@ngay", SqlDbType.Date).Value = selectedDate.Value;
                        }
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string ngayXuat = Convert.ToDateTime(reader["NGAYXUAT"]).ToString("dd/MM/yyyy");
                                lstPhieu.Items.Add($"{reader["MAPHIEUXUAT"]} - {reader["TENKHACH"]} ({ngayXuat})");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu phiếu: " + ex.Message);
            }
        }
        
        private void LoadComboBoxData(ComboBox cboKH, ComboBox cboDD, ComboBox cboHM, ComboBox cboTB)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    
                    // Load khách hàng
                    cboKH.Items.Clear();
                    using (var cmd = new SqlCommand("SELECT TENKHACH FROM KHACHHANG", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cboKH.Items.Add(reader["TENKHACH"].ToString());
                            }
                        }
                    }
                    if (cboKH.Items.Count > 0) cboKH.SelectedIndex = 0;
                    
                    // Load địa điểm công trình
                    cboDD.Items.Clear();
                    using (var cmd = new SqlCommand("SELECT DIADIEM FROM CONGTRINH", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cboDD.Items.Add(reader["DIADIEM"].ToString());
                            }
                        }
                    }
                    if (cboDD.Items.Count > 0) cboDD.SelectedIndex = 0;
                    
                    // Load hạng mục
                    cboHM.Items.Clear();
                    using (var cmd = new SqlCommand("SELECT DISTINCT HANGMUC FROM CONGTRINH", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cboHM.Items.Add(reader["HANGMUC"].ToString());
                            }
                        }
                    }
                    if (cboHM.Items.Count > 0) cboHM.SelectedIndex = 0;
                    
                    // Load thiết bị bơm
                    cboTB.Items.Clear();
                    using (var cmd = new SqlCommand("SELECT DISTINCT THIETBIBOM FROM CONGTRINH", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cboTB.Items.Add(reader["THIETBIBOM"].ToString());
                            }
                        }
                    }
                    if (cboTB.Items.Count > 0) cboTB.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu ComboBox: " + ex.Message);
            }
        }
        
        private void LoadPhieuChiTiet(string maPhieu, TextBox txtMaPhieu, ComboBox cboKH, ComboBox cboDD, ComboBox cboHM, 
                                       TextBox txtMacBT, ComboBox cboTB, CheckBox chkBom, DataGridView dgvKhoiLuong, DataGridView dgvChiTiet)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    
                    // Load thông tin phiếu
                    using (var cmd = new SqlCommand(@"
                    SELECT px.MAPHIEUXUAT, px.NGAYXUAT, px.SOPHIEU, px.MACBETONG, px.KHOILUONG, px.SUDUNGBOM,
                           px.THOIGIANBATDAU, px.THOIGIANKETTHUC, px.THIETBIBOM,
                           kh.TENKHACH, ct.DIADIEM, ct.HANGMUC
                    FROM PHIEUXUAT px
                    JOIN KHACHHANG kh ON px.MAKHACH = kh.MAKHACH
                    JOIN CONGTRINH ct ON px.MACONGTRINH = ct.MACONGTRINH
                    WHERE px.MAPHIEUXUAT = @maPhieu", conn))
                    {
                        cmd.Parameters.Add("@maPhieu", SqlDbType.NVarChar).Value = maPhieu;
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtMaPhieu.Text = reader["MAPHIEUXUAT"].ToString();
                                cboKH.Text = reader["TENKHACH"].ToString();
                                cboDD.Text = reader["DIADIEM"].ToString();
                                cboHM.Text = reader["HANGMUC"].ToString();
                                txtMacBT.Text = reader["MACBETONG"].ToString();
                                cboTB.Text = reader["THIETBIBOM"].ToString();
                                chkBom.Checked = Convert.ToBoolean(reader["SUDUNGBOM"]);
                                
                                // Cập nhật thời gian trộn
                                if (reader["THOIGIANBATDAU"] != DBNull.Value)
                                {
                                    // Cập nhật TextBox thời gian bắt đầu
                                }
                                if (reader["THOIGIANKETTHUC"] != DBNull.Value)
                                {
                                    // Cập nhật TextBox thời gian kết thúc
                                }
                            }
                        }
                    }
                    
                    // Load chi tiết vật liệu
                    dgvKhoiLuong.Rows.Clear();
                    using (var cmd = new SqlCommand(@"
                    SELECT vt.TENVATTU, ct.SOLUONG
                    FROM CHITIETPHIEUXUAT ct
                    JOIN VATTU vt ON ct.MAVATTU = vt.MAVATTU
                    WHERE ct.MAPHIEUXUAT = @maPhieu", conn))
                    {
                        cmd.Parameters.Add("@maPhieu", SqlDbType.NVarChar).Value = maPhieu;
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dgvKhoiLuong.Rows.Add(reader["TENVATTU"].ToString(), reader["SOLUONG"].ToString());
                            }
                        }
                    }
                    
                    // Load chi tiết phiếu (có thể cần bảng khác trong DB)
                    // Đây là dữ liệu mẫu
                    dgvChiTiet.Rows.Clear();
                    dgvChiTiet.Rows.Add("1", "51D-12345", "7.0", "08:45", "OK");
                    dgvChiTiet.Rows.Add("2", "51D-67890", "6.5", "09:10", "Trễ 5p");
                    dgvChiTiet.Rows.Add("3", "51C-54321", "7.5", "09:40", "OK");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết phiếu: " + ex.Message);
            }
        }
        
        // ---------------- Helper ----------------
        private GroupBox CreateGroupBox(string text, int x, int y, int w, int h)
        {
            return new GroupBox()
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(w, h),
                ForeColor = Color.Red,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
        }
        
        private Label CreateLabelBold(string text, int x, int y)
        {
            return new Label()
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.Black
            };
        }
        
        private Button CreateButton(string text, int x, int y, int w, int h = 30)
        {
            return new Button()
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(w, h),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
        }
        
        private RadioButton CreateRadio(string text, int x, int y, bool check = false)
        {
            return new RadioButton()
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                ForeColor = Color.Black,
                Checked = check,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
        }
        
        private void ResetChildControls(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is Label lbl && lbl.Font.Bold) continue; // giữ in đậm cho label quan trọng
                if (ctrl is GroupBox) continue; // giữ nguyên groupbox
                ctrl.ForeColor = Color.Black;
                ctrl.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            }
        }
        
        private DataGridView CreateGrid(int cols, int rows)
        {
            DataGridView dgv = new DataGridView()
            {
                Dock = DockStyle.Fill,
                ColumnCount = cols,
                RowCount = rows,
                BackgroundColor = Color.WhiteSmoke,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToResizeColumns = false,
                AllowUserToResizeRows = false,
                ForeColor = Color.Black
            };
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold); // 👈 in đậm header
            dgv.EnableHeadersVisualStyles = false;
            return dgv;
        }
    }
}