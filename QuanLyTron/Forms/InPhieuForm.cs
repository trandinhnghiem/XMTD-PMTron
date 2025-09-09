using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QuanLyTron.DAL;
using System.Drawing.Printing;
using System.Collections.Generic;

namespace QuanLyTron.Forms
{
    public class InPhieuForm : Form
    {
        // Các controls chính
        private ListBox lstPhieu;
        private DataGridView dgvKhoiLuong, dgvChiTiet;
        private TextBox txtMaPhieu, txtSoPhieu, txtMacBT, txtBD, txtKT;
        private ComboBox cboKH, cboDD, cboHM, cboTB;
        private DateTimePicker dtpNgay, dtpTron;
        private CheckBox chkBom;
        private RadioButton radMau1, radMau2, radMau3, radMau4;
        private Button btnTim, btnSave, btnPrint;
        
        // Biến lưu trữ thông tin phiếu hiện tại
        private int currentPhieuId = -1;
        
        public InPhieuForm()
        {
            // Form
            Text = "PHIÉU GIAO NHẬN BÊ TÔNG";
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
            dtpNgay = new DateTimePicker() { Location = new Point(60, 20), Width = 150, Format = DateTimePickerFormat.Short, ForeColor = Color.Black };
            btnTim = CreateButton("Tìm", 60, 50, 150);
            lstPhieu = new ListBox() { Location = new Point(10, 85), Size = new Size(210, 200), ForeColor = Color.Black };
            
            // Load dữ liệu phiếu
            LoadPhieuData(lstPhieu);
            
            grpChonPhieu.Controls.AddRange(new Control[] { lblNgay, dtpNgay, btnTim, lstPhieu });
            ResetChildControls(grpChonPhieu);
            
            // Tùy chọn in
            GroupBox grpIn = CreateGroupBox("TÙY CHỌN IN", 0, 300, 240, 140);
            radMau1 = new RadioButton()
            {
                Text = "Mẫu 1 (In chi tiết)",
                Location = new Point(10, 20),
                AutoSize = true,
                ForeColor = Color.Blue,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Checked = true
            };
            radMau2 = new RadioButton()
            {
                Text = "Mẫu 2 (In chi tiết)",
                Location = new Point(10, 40),
                AutoSize = true,
                ForeColor = Color.Red,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            radMau3 = new RadioButton()
            {
                Text = "Mẫu 3 (In chi tiết)",
                Location = new Point(10, 60),
                AutoSize = true,
                ForeColor = Color.Orange,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            radMau4 = new RadioButton()
            {
                Text = "Mẫu 4 (In tổng)",
                Location = new Point(10, 80),
                AutoSize = true,
                ForeColor = Color.Purple,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            btnSave = new Button()
            {
                Text = "💾 Lưu",
                Location = new Point(150, 20),
                Size = new Size(80, 35),
                ForeColor = Color.SeaGreen,
                Font = new Font("Segoe UI Emoji", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnPrint = new Button()
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
            txtBD = new TextBox() { Location = new Point(130, 20), Width = 90, Text = "08:30", ForeColor = Color.Black };
            Label lblKT = CreateLabelBold("Thời gian kết thúc", 10, 60);
            txtKT = new TextBox() { Location = new Point(130, 55), Width = 90, Text = "09:15", ForeColor = Color.Black };
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
            cboKH = new ComboBox() { Location = new Point(100, 20), Width = 220, ForeColor = Color.Black };
            Label lblMaPhieu = CreateLabelBold("Mã phiếu:", 340, 25);
            txtMaPhieu = new TextBox() { Location = new Point(415, 20), Width = 180, Text = "PX001", ForeColor = Color.Black, ReadOnly = true };
            Label lblDD = CreateLabelBold("Địa điểm XD:", 10, 60);
            cboDD = new ComboBox() { Location = new Point(100, 55), Width = 220, ForeColor = Color.Black };
            Label lblSoPhieu = CreateLabelBold("Số phiếu:", 340, 60);
            txtSoPhieu = new TextBox() { Location = new Point(415, 55), Width = 180, Text = "S001", ForeColor = Color.Black };
            Label lblHM = CreateLabelBold("Hạng mục:", 10, 95);
            cboHM = new ComboBox() { Location = new Point(100, 90), Width = 220, ForeColor = Color.Black };
            Label lblNgayTron = CreateLabelBold("Ngày trộn:", 340, 95);
            dtpTron = new DateTimePicker() { Location = new Point(415, 90), Width = 180, Format = DateTimePickerFormat.Short, Value = DateTime.Today, ForeColor = Color.Black };
            Label lblTB = CreateLabelBold("Thiết bị bơm:", 620, 25);
            cboTB = new ComboBox() { Location = new Point(710, 20), Width = 220, ForeColor = Color.Black };
            Label lblMacBT = CreateLabelBold("Mác bê tông:", 620, 60);
            txtMacBT = new TextBox() { Location = new Point(710, 55), Width = 220, Text = "M300", ForeColor = Color.Black };
            chkBom = new CheckBox() { Text = "Sử dụng bơm", Location = new Point(710, 95), AutoSize = true, Checked = true, ForeColor = Color.Black };
            
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
            dgvKhoiLuong = new DataGridView()
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
            dgvKhoiLuong.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
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
            DataGridView dgv2 = CreateGrid(5, 2);
            dgv2.Rows[0].Cells[0].Value = "STT";
            dgv2.Rows[0].Cells[1].Value = "Khối lượng";
            DataGridView dgv3 = CreateGrid(5, 4);
            dgv3.Rows[0].Cells[0].Value = "STT";
            dgv3.Rows[0].Cells[1].Value = "Tên";
            dgv3.Rows[0].Cells[2].Value = "SL";
            dgv3.Rows[0].Cells[3].Value = "Ghi chú";
            tblThongSo.Controls.Add(dgv1, 0, 0);
            tblThongSo.Controls.Add(dgv2, 0, 1);
            tblThongSo.Controls.Add(dgv3, 0, 2);
            grpThongSo.Controls.Add(tblThongSo);
            ResetChildControls(grpThongSo);
            
            // Thông tin chi tiết
            GroupBox grpChiTiet = CreateGroupBox("THÔNG TIN CHI TIẾT", 0, 430, 960, 270);
            Panel pnlChiTiet = new Panel() { Dock = DockStyle.Fill };
            dgvChiTiet = new DataGridView()
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
            dgvChiTiet.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvChiTiet.AllowUserToAddRows = false;
            pnlChiTiet.Controls.Add(dgvChiTiet);
            grpChiTiet.Controls.Add(pnlChiTiet);
            ResetChildControls(grpChiTiet);
            panelRight.Controls.AddRange(new Control[] { grpThongTin, grpKhoiLuong, grpThongSo, grpChiTiet });
            
            // Add panels vào form
            Controls.Add(panelRight);
            Controls.Add(panelLeft);
            
            // Đăng ký sự kiện
            btnTim.Click += BtnTim_Click;
            lstPhieu.SelectedIndexChanged += LstPhieu_SelectedIndexChanged;
            btnSave.Click += BtnSave_Click;
            btnPrint.Click += BtnPrint_Click;
        }
        
        private void BtnTim_Click(object sender, EventArgs e)
        {
            // Tìm phiếu theo ngày
            LoadPhieuData(lstPhieu, dtpNgay.Value);
        }
        
        private void LstPhieu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstPhieu.SelectedIndex >= 0)
            {
                // Lấy mã phiếu từ item được chọn
                string selectedPhieu = lstPhieu.SelectedItem.ToString();
                string maPhieu = selectedPhieu.Split('-')[0].Trim();
                
                // Load thông tin chi tiết phiếu
                LoadPhieuChiTiet(maPhieu);
            }
        }
        
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (currentPhieuId == -1)
            {
                MessageBox.Show("Vui lòng chọn phiếu cần lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    
                    // Cập nhật thông tin phiếu
                    using (var cmd = new SqlCommand(@"
                    UPDATE PHIEUXUAT 
                    SET SOPHIEU = @soPhieu, MACBETONG = @macBT, SUDUNGBOM = @suDungBom,
                        THOIGIANBATDAU = @thoiGianBD, THOIGIANKETTHUC = @thoiGianKT,
                        THIETBIBOM = @thietBiBom, GHICHU = @ghiChu
                    WHERE MAPHIEUXUAT = @maPhieu", conn))
                    {
                        cmd.Parameters.Add("@maPhieu", SqlDbType.Int).Value = currentPhieuId;
                        cmd.Parameters.Add("@soPhieu", SqlDbType.NVarChar).Value = txtSoPhieu.Text;
                        cmd.Parameters.Add("@macBT", SqlDbType.NVarChar).Value = txtMacBT.Text;
                        cmd.Parameters.Add("@suDungBom", SqlDbType.Bit).Value = chkBom.Checked;
                        
                        // Xử lý thời gian
                        DateTime ngayTron = dtpTron.Value;
                        DateTime thoiGianBD = ngayTron.Add(TimeSpan.Parse(txtBD.Text));
                        DateTime thoiGianKT = ngayTron.Add(TimeSpan.Parse(txtKT.Text));
                        
                        cmd.Parameters.Add("@thoiGianBD", SqlDbType.DateTime).Value = thoiGianBD;
                        cmd.Parameters.Add("@thoiGianKT", SqlDbType.DateTime).Value = thoiGianKT;
                        cmd.Parameters.Add("@thietBiBom", SqlDbType.NVarChar).Value = cboTB.Text;
                        cmd.Parameters.Add("@ghiChu", SqlDbType.NVarChar).Value = ""; // Có thể thêm textbox ghi chú nếu cần
                        
                        cmd.ExecuteNonQuery();
                    }
                    
                    MessageBox.Show("Lưu thông tin phiếu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu thông tin phiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (currentPhieuId == -1)
            {
                MessageBox.Show("Vui lòng chọn phiếu cần in!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Xác định mẫu in được chọn
            string mauIn = "";
            if (radMau1.Checked) mauIn = "Mẫu 1";
            else if (radMau2.Checked) mauIn = "Mẫu 2";
            else if (radMau3.Checked) mauIn = "Mẫu 3";
            else if (radMau4.Checked) mauIn = "Mẫu 4";
            
            // Hiển thị hộp thoại xác nhận
            if (MessageBox.Show($"Bạn có chắc muốn in phiếu với {mauIn}?", "Xác nhận in", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Mở hộp thoại in
                PrintDialog printDialog = new PrintDialog();
                PrintDocument printDocument = new PrintDocument();
                
                printDocument.PrintPage += (s, ev) => 
                {
                    // Tạo nội dung in dựa trên mẫu được chọn
                    string printContent = CreatePrintContent(mauIn);
                    
                    // Thiết lập font in
                    Font printFont = new Font("Arial", 10);
                    
                    // Vẽ nội dung lên trang in
                    ev.Graphics.DrawString(printContent, printFont, Brushes.Black, ev.MarginBounds.Left, ev.MarginBounds.Top);
                };
                
                printDialog.Document = printDocument;
                
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        printDocument.Print();
                        MessageBox.Show("In phiếu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi in phiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        
        private string CreatePrintContent(string mauIn)
        {
            // Tạo nội dung in dựa trên mẫu được chọn
            string content = $"PHIÉU GIAO NHẬN BÊ TÔNG - {mauIn}\n\n";
            content += $"Mã phiếu: {txtMaPhieu.Text}\n";
            content += $"Số phiếu: {txtSoPhieu.Text}\n";
            content += $"Khách hàng: {cboKH.Text}\n";
            content += $"Địa điểm XD: {cboDD.Text}\n";
            content += $"Hạng mục: {cboHM.Text}\n";
            content += $"Ngày trộn: {dtpTron.Value.ToString("dd/MM/yyyy")}\n";
            content += $"Mác bê tông: {txtMacBT.Text}\n";
            content += $"Thiết bị bơm: {cboTB.Text}\n";
            content += $"Sử dụng bơm: {(chkBom.Checked ? "Có" : "Không")}\n";
            content += $"Thời gian bắt đầu: {txtBD.Text}\n";
            content += $"Thời gian kết thúc: {txtKT.Text}\n\n";
            
            // Thêm thông tin vật liệu
            content += "THÔNG TIN VẬT LIỆU:\n";
            foreach (DataGridViewRow row in dgvKhoiLuong.Rows)
            {
                content += $"{row.Cells[0].Value}: {row.Cells[1].Value}\n";
            }
            
            // Thêm thông tin chi tiết
            content += "\nTHÔNG TIN CHI TIẾT:\n";
            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                content += $"{row.Cells[0].Value}. {row.Cells[1].Value} - {row.Cells[2].Value} m³ - {row.Cells[3].Value} - {row.Cells[4].Value}\n";
            }
            
            return content;
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
                    
                    query += " ORDER BY NGAYXUAT DESC";
                    
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
                MessageBox.Show("Lỗi khi tải dữ liệu phiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    using (var cmd = new SqlCommand("SELECT TENKHACH FROM KHACHHANG ORDER BY TENKHACH", conn))
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
                    using (var cmd = new SqlCommand("SELECT DIADIEM FROM CONGTRINH ORDER BY DIADIEM", conn))
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
                    using (var cmd = new SqlCommand("SELECT DISTINCT HANGMUC FROM CONGTRINH WHERE HANGMUC IS NOT NULL ORDER BY HANGMUC", conn))
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
                    using (var cmd = new SqlCommand("SELECT DISTINCT THIETBIBOM FROM CONGTRINH WHERE THIETBIBOM IS NOT NULL ORDER BY THIETBIBOM", conn))
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
                MessageBox.Show("Lỗi khi tải dữ liệu ComboBox: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void LoadPhieuChiTiet(string maPhieu)
        {
            try
            {
                // Lấy ID phiếu từ chuỗi
                if (!int.TryParse(maPhieu, out int phieuId))
                {
                    MessageBox.Show("Mã phiếu không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                currentPhieuId = phieuId;
                
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    
                    // Dictionary để lưu đơn vị tính của vật tư
                    Dictionary<string, string> donViTinhDict = new Dictionary<string, string>();
                    
                    // Lấy danh sách đơn vị tính trước
                    using (var cmdDonVi = new SqlCommand("SELECT TENVATTU, DONVITINH FROM VATTU", conn))
                    {
                        using (var reader = cmdDonVi.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string tenVatTu = reader["TENVATTU"].ToString();
                                string donVi = reader["DONVITINH"] != DBNull.Value ? reader["DONVITINH"].ToString() : "";
                                donViTinhDict[tenVatTu] = donVi;
                            }
                        }
                    }
                    
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
                        cmd.Parameters.Add("@maPhieu", SqlDbType.Int).Value = phieuId;
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtMaPhieu.Text = reader["MAPHIEUXUAT"].ToString();
                                txtSoPhieu.Text = reader["SOPHIEU"] != DBNull.Value ? reader["SOPHIEU"].ToString() : "";
                                cboKH.Text = reader["TENKHACH"].ToString();
                                cboDD.Text = reader["DIADIEM"].ToString();
                                cboHM.Text = reader["HANGMUC"] != DBNull.Value ? reader["HANGMUC"].ToString() : "";
                                txtMacBT.Text = reader["MACBETONG"] != DBNull.Value ? reader["MACBETONG"].ToString() : "";
                                cboTB.Text = reader["THIETBIBOM"] != DBNull.Value ? reader["THIETBIBOM"].ToString() : "";
                                chkBom.Checked = reader["SUDUNGBOM"] != DBNull.Value && Convert.ToBoolean(reader["SUDUNGBOM"]);
                                
                                // Cập nhật ngày trộn
                                if (reader["NGAYXUAT"] != DBNull.Value)
                                {
                                    dtpTron.Value = Convert.ToDateTime(reader["NGAYXUAT"]);
                                }
                                
                                // Cập nhật thời gian trộn
                                if (reader["THOIGIANBATDAU"] != DBNull.Value)
                                {
                                    DateTime thoiGianBD = Convert.ToDateTime(reader["THOIGIANBATDAU"]);
                                    txtBD.Text = thoiGianBD.ToString("HH:mm");
                                }
                                
                                if (reader["THOIGIANKETTHUC"] != DBNull.Value)
                                {
                                    DateTime thoiGianKT = Convert.ToDateTime(reader["THOIGIANKETTHUC"]);
                                    txtKT.Text = thoiGianKT.ToString("HH:mm");
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
                        cmd.Parameters.Add("@maPhieu", SqlDbType.Int).Value = phieuId;
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string tenVatTu = reader["TENVATTU"].ToString();
                                string soLuong = reader["SOLUONG"].ToString();
                                string donVi = donViTinhDict.ContainsKey(tenVatTu) ? donViTinhDict[tenVatTu] : "";
                                
                                dgvKhoiLuong.Rows.Add(tenVatTu, $"{soLuong} {donVi}");
                            }
                        }
                    }
                    
                    // Load chi tiết phiếu
                    dgvChiTiet.Rows.Clear();
                    using (var cmd = new SqlCommand(@"
                    SELECT ROW_NUMBER() OVER (ORDER BY px.MAPHIEUXUAT) AS STT, 
                           x.BIENSO, px.KHOILUONG, 
                           FORMAT(px.THOIGIANTRON, 'HH:mm') AS THOIGIANXUAT, 
                           px.GHICHU
                    FROM PHIEUXUAT px
                    JOIN XE x ON px.MAXE = x.MAXE
                    WHERE px.MAPHIEUXUAT = @maPhieu", conn))
                    {
                        cmd.Parameters.Add("@maPhieu", SqlDbType.Int).Value = phieuId;
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string khoiLuong = reader["KHOILUONG"] != DBNull.Value ? reader["KHOILUONG"].ToString() : "";
                                string thoiGianXuat = reader["THOIGIANXUAT"] != DBNull.Value ? reader["THOIGIANXUAT"].ToString() : "";
                                string ghiChu = reader["GHICHU"] != DBNull.Value ? reader["GHICHU"].ToString() : "";
                                
                                dgvChiTiet.Rows.Add(
                                    reader["STT"].ToString(),
                                    reader["BIENSO"].ToString(),
                                    khoiLuong,
                                    thoiGianXuat,
                                    ghiChu
                                );
                            }
                        }
                    }
                    
                    // Cập nhật thông số
                    UpdateThongSo(phieuId, conn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết phiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void UpdateThongSo(int phieuId, SqlConnection conn)
        {
            // Cập nhật thông tin xe
            DataGridView dgv1 = null;
            DataGridView dgv2 = null;
            DataGridView dgv3 = null;
            
            // Tìm các DataGridView trong TableLayoutPanel
            foreach (Control control in Controls.Find("tblThongSo", true))
            {
                if (control is TableLayoutPanel tbl)
                {
                    dgv1 = tbl.GetControlFromPosition(0, 0) as DataGridView;
                    dgv2 = tbl.GetControlFromPosition(0, 1) as DataGridView;
                    dgv3 = tbl.GetControlFromPosition(0, 2) as DataGridView;
                    break;
                }
            }
            
            if (dgv1 != null)
            {
                // Xóa dữ liệu cũ (trừ header)
                for (int i = dgv1.Rows.Count - 1; i > 0; i--)
                {
                    dgv1.Rows.RemoveAt(i);
                }
                
                // Load thông tin xe
                using (var cmd = new SqlCommand(@"
                SELECT x.BIENSO
                FROM PHIEUXUAT px
                JOIN XE x ON px.MAXE = x.MAXE
                WHERE px.MAPHIEUXUAT = @maPhieu", conn))
                {
                    cmd.Parameters.Add("@maPhieu", SqlDbType.Int).Value = phieuId;
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        int rowIdx = 1;
                        while (reader.Read() && rowIdx < dgv1.Rows.Count)
                        {
                            dgv1.Rows[rowIdx].Cells[0].Value = "Xe " + rowIdx;
                            dgv1.Rows[rowIdx].Cells[1].Value = reader["BIENSO"].ToString();
                            rowIdx++;
                        }
                    }
                }
            }
            
            if (dgv2 != null)
            {
                // Xóa dữ liệu cũ (trừ header)
                for (int i = dgv2.Rows.Count - 1; i > 0; i--)
                {
                    dgv2.Rows.RemoveAt(i);
                }
                
                // Load thông tin khối lượng
                using (var cmd = new SqlCommand(@"
                SELECT KHOILUONG
                FROM PHIEUXUAT
                WHERE MAPHIEUXUAT = @maPhieu", conn))
                {
                    cmd.Parameters.Add("@maPhieu", SqlDbType.Int).Value = phieuId;
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        int rowIdx = 1;
                        while (reader.Read() && rowIdx < dgv2.Rows.Count)
                        {
                            string khoiLuong = reader["KHOILUONG"] != DBNull.Value ? reader["KHOILUONG"].ToString() : "";
                            dgv2.Rows[rowIdx].Cells[0].Value = rowIdx.ToString();
                            dgv2.Rows[rowIdx].Cells[1].Value = khoiLuong + " m³";
                            rowIdx++;
                        }
                    }
                }
            }
            
            if (dgv3 != null)
            {
                // Xóa dữ liệu cũ (trừ header)
                for (int i = dgv3.Rows.Count - 1; i > 0; i--)
                {
                    dgv3.Rows.RemoveAt(i);
                }
                
                // Load thông tin xe bồn
                using (var cmd = new SqlCommand(@"
                SELECT x.BIENSO, 'Xe bồn' AS TEN, 2 AS SOLUONG, 'Chở đủ tải' AS GHICHU
                FROM PHIEUXUAT px
                JOIN XE x ON px.MAXE = x.MAXE
                WHERE px.MAPHIEUXUAT = @maPhieu", conn))
                {
                    cmd.Parameters.Add("@maPhieu", SqlDbType.Int).Value = phieuId;
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        int rowIdx = 1;
                        while (reader.Read() && rowIdx < dgv3.Rows.Count)
                        {
                            dgv3.Rows[rowIdx].Cells[0].Value = rowIdx.ToString();
                            dgv3.Rows[rowIdx].Cells[1].Value = reader["TEN"].ToString();
                            dgv3.Rows[rowIdx].Cells[2].Value = reader["SOLUONG"].ToString();
                            dgv3.Rows[rowIdx].Cells[3].Value = reader["GHICHU"].ToString();
                            rowIdx++;
                        }
                    }
                }
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
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;
            return dgv;
        }
    }
}