using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using QuanLyTram.DAL;

namespace QuanLyTram.Forms
{
    public class CapPhoiForm : Form
    {
        private DataGridView dgvCapPhoi;
        private TextBox txtSTT, txtMacBT, txtCuongDo, txtCotLieuMax, txtDoSut, txtTongKhoiLuong;
        private Label lblSTT, lblMacBT, lblCuongDo, lblCotLieuMax, lblDoSut, lblTongKhoiLuong;
        private Button btnThemMoi, btnCapNhat, btnXoa, btnLamMoi;
        private DataTable dtCapPhoi;
        private int currentId = -1; // Lưu ID của bản ghi đang được chọn
        
        // Định nghĩa font cho tiêu đề button
        private Font FTitle = new Font("Segoe UI", 11, FontStyle.Bold);
        
        public CapPhoiForm()
        {
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            this.Text = "QUẢN LÝ CẤP PHỐI";
            this.Width = 1220; // Toàn màn hình ngang
            this.Height = 720;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(255, 250, 230);
            
            // ====== SplitContainer ======
            SplitContainer splitTop = new SplitContainer
            {
                Dock = DockStyle.Top,
                Height = 340, // Giảm chiều cao DataGridView
                Orientation = Orientation.Horizontal,
                IsSplitterFixed = false,
                BackColor = Color.White
            };
            
            // Tăng padding của Panel1 để tạo khoảng cách phía trên
            splitTop.Panel1.Padding = new Padding(0, 15, 0, 0); // Top = 15px
            
            // ====== GroupBox Danh sách ======
            GroupBox grpDanhSach = new GroupBox
            {
                Text = "DANH SÁCH MÁC CẤP PHỐI",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.Red, // Chỉ tiêu đề màu đỏ
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };
            splitTop.Panel1.Controls.Add(grpDanhSach);
            
            // ====== DataGridView ======
            dgvCapPhoi = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToResizeColumns = true,
                AllowUserToResizeRows = true,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = true,
                EnableHeadersVisualStyles = true,
                Font = new Font("Segoe UI", 9, FontStyle.Regular) // Font mặc định cho toàn bộ DataGridView
            };
            
            // Tô đậm header cột
            dgvCapPhoi.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvCapPhoi.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black; // Header màu đen
            dgvCapPhoi.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvCapPhoi.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            // Thiết lập font cho dữ liệu trong DataGridView - đảm bảo không in đậm
            dgvCapPhoi.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            dgvCapPhoi.DefaultCellStyle.ForeColor = Color.Black; // Dữ liệu màu đen
            
            // Đảm bảo các ô không bị in đậm
            dgvCapPhoi.RowsDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            dgvCapPhoi.RowsDefaultCellStyle.ForeColor = Color.Black;
            
            grpDanhSach.Controls.Add(dgvCapPhoi);
            
            // ====== Panel dưới ======
            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(255, 250, 230)
            };
            
            // ====== GroupBox Thông tin ======
            GroupBox grpThongTin = new GroupBox
            {
                Text = "THÔNG TIN MÁC",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.Red, // Chỉ tiêu đề màu đỏ
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };
            bottomPanel.Controls.Add(grpThongTin);
            
            int labelLeft = 50;          // vị trí label bên trái
            int textLeft = 200;          // textbox bên trái
            int rightLabelLeft = 650;    // label bên phải
            int rightTextLeft = 850;     // textbox bên phải
            int textWidth = 280;
            int controlHeight = 40;
            int paddingY = 20;
            Font labelFont = new Font("Segoe UI", 10, FontStyle.Bold);
            Font textFont = new Font("Segoe UI", 10);
            
            // ====== Cột trái ======
            lblSTT = new Label { Text = "STT:", Font = labelFont, Left = labelLeft, Top = 40, AutoSize = true, TextAlign = ContentAlignment.TopLeft, ForeColor = Color.Black };
            txtSTT = new TextBox { Font = textFont, Left = textLeft, Top = lblSTT.Top, Width = textWidth, Height = controlHeight, ForeColor = Color.Black };
            lblMacBT = new Label { Text = "Mác BT:", Font = labelFont, Left = labelLeft, Top = lblSTT.Bottom + paddingY, AutoSize = true, TextAlign = ContentAlignment.TopLeft, ForeColor = Color.Black };
            txtMacBT = new TextBox { Font = textFont, Left = textLeft, Top = lblMacBT.Top, Width = textWidth, Height = controlHeight, ForeColor = Color.Black };
            lblCuongDo = new Label { Text = "Cường độ:", Font = labelFont, Left = labelLeft, Top = lblMacBT.Bottom + paddingY, AutoSize = true, TextAlign = ContentAlignment.TopLeft, ForeColor = Color.Black };
            txtCuongDo = new TextBox { Font = textFont, Left = textLeft, Top = lblCuongDo.Top, Width = textWidth, Height = controlHeight, ForeColor = Color.Black };
            
            // ====== Cột phải ======
            lblCotLieuMax = new Label { Text = "Cốt liệu Max:", Font = labelFont, Left = rightLabelLeft, Top = 40, AutoSize = true, TextAlign = ContentAlignment.TopLeft, ForeColor = Color.Black };
            txtCotLieuMax = new TextBox { Font = textFont, Left = rightTextLeft, Top = lblCotLieuMax.Top, Width = textWidth, Height = controlHeight, ForeColor = Color.Black };
            lblDoSut = new Label { Text = "Độ sụt:", Font = labelFont, Left = rightLabelLeft, Top = lblCotLieuMax.Bottom + paddingY, AutoSize = true, TextAlign = ContentAlignment.TopLeft, ForeColor = Color.Black };
            txtDoSut = new TextBox { Font = textFont, Left = rightTextLeft, Top = lblDoSut.Top, Width = textWidth, Height = controlHeight, ForeColor = Color.Black };
            lblTongKhoiLuong = new Label { Text = "Tổng khối lượng:", Font = labelFont, Left = rightLabelLeft, Top = lblDoSut.Bottom + paddingY, AutoSize = true, TextAlign = ContentAlignment.TopLeft, ForeColor = Color.Black };
            txtTongKhoiLuong = new TextBox { Font = textFont, Left = rightTextLeft, Top = lblTongKhoiLuong.Top, Width = textWidth, Height = controlHeight, ForeColor = Color.Black };
            
            // ====== Panel chứa các nút ======
            Panel buttonPanel = new Panel
            {
                Height = 130, // Tăng chiều cao để tạo khoảng trống dưới
                Dock = DockStyle.Bottom,
                BackColor = Color.Transparent
            };
            
            // Nút bấm THÊM MỚI
            btnThemMoi = new IconButton
            {
                Text = "THÊM MỚI",
                Font = FTitle,
                ForeColor= Color.White,
                BackColor = Color.FromArgb(46,204,113),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(220, 46),
                IconChar = IconChar.PlusCircle,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleCenter,
                ImageAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(28, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnThemMoi.FlatAppearance.BorderSize = 0;
            
            // Nút bấm CẬP NHẬT
            btnCapNhat = new IconButton
            {
                Text = "CẬP NHẬT",
                Font = FTitle,
                ForeColor= Color.White,
                BackColor = Color.FromArgb(52, 152, 219), // Màu xanh dương đẹp hơn
                FlatStyle = FlatStyle.Flat,
                Size = new Size(220, 46),
                IconChar = IconChar.Save,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleCenter,
                ImageAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(28, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnCapNhat.FlatAppearance.BorderSize = 0;
            
            // Nút bấm XÓA
            btnXoa = new IconButton
            {
                Text = "XÓA",
                Font = FTitle,
                ForeColor= Color.White,
                BackColor = Color.FromArgb(231, 76, 60), // Màu đỏ đẹp hơn
                FlatStyle = FlatStyle.Flat,
                Size = new Size(220, 46),
                IconChar = IconChar.Trash,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleCenter,
                ImageAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(35, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnXoa.FlatAppearance.BorderSize = 0;
            
            // Nút bấm LÀM MỚI
            btnLamMoi = new IconButton
            {
                Text = "LÀM MỚI",
                Font = FTitle,
                ForeColor= Color.White,
                BackColor = Color.MediumPurple,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(220, 46),
                IconChar = IconChar.Sync,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleCenter,
                ImageAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(28, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnLamMoi.FlatAppearance.BorderSize = 0;
            
            // Đặt vị trí nút - tăng khoảng cách từ trên xuống
            int buttonTop = 40; // Tăng từ 15 lên 40 để tạo khoảng trống trên
            int buttonSpacing = 20;
            int buttonsStartLeft = (buttonPanel.Width - (4 * 220 + 3 * buttonSpacing)) / 2;
            
            btnThemMoi.Top = buttonTop;
            btnThemMoi.Left = buttonsStartLeft;
            
            btnCapNhat.Top = buttonTop;
            btnCapNhat.Left = btnThemMoi.Right + buttonSpacing;
            
            btnXoa.Top = buttonTop;
            btnXoa.Left = btnCapNhat.Right + buttonSpacing;
            
            btnLamMoi.Top = buttonTop;
            btnLamMoi.Left = btnXoa.Right + buttonSpacing;
            
            buttonPanel.Resize += (s, e) =>
            {
                buttonsStartLeft = (buttonPanel.Width - (4 * 220 + 3 * buttonSpacing)) / 2;
                btnThemMoi.Left = buttonsStartLeft;
                btnCapNhat.Left = btnThemMoi.Right + buttonSpacing;
                btnXoa.Left = btnCapNhat.Right + buttonSpacing;
                btnLamMoi.Left = btnXoa.Right + buttonSpacing;
            };
            
            buttonPanel.Controls.AddRange(new Control[] { btnThemMoi, btnCapNhat, btnXoa, btnLamMoi });
            
            // Thêm các controls vào GroupBox Thông tin
            grpThongTin.Controls.AddRange(new Control[] {
                lblSTT, txtSTT, lblMacBT, txtMacBT, lblCuongDo, txtCuongDo,
                lblCotLieuMax, txtCotLieuMax, lblDoSut, txtDoSut,
                lblTongKhoiLuong, txtTongKhoiLuong, buttonPanel
            });
            
            this.Controls.Add(bottomPanel);
            this.Controls.Add(splitTop);
            
            // Đăng ký sự kiện
            btnThemMoi.Click += BtnThemMoi_Click;
            btnCapNhat.Click += BtnCapNhat_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += BtnLamMoi_Click;
            dgvCapPhoi.CellClick += DgvCapPhoi_CellClick;
            
            // Đặt vị trí Splitter sau khi form đã được khởi tạo
            this.Load += (s, e) =>
            {
                splitTop.SplitterDistance = splitTop.Height; // Đặt splitter ở dưới cùng
                // Load dữ liệu sau khi tất cả các điều khiển đã được khởi tạo
                LoadData();
            };
        }
        
        // Các phương thức còn lại giữ nguyên...
        private void LoadData()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT MACAPPHOI, STT, MACBETONG, CUONGDO, COTLIEUMAX, DOSUT, TONGSLVATTU FROM CAPPHOI", conn))
                    {
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            dtCapPhoi = new DataTable();
                            adapter.Fill(dtCapPhoi);
                            
                            // Đổi tên cột để hiển thị
                            dtCapPhoi.Columns["MACAPPHOI"].ColumnName = "ID";
                            dtCapPhoi.Columns["MACBETONG"].ColumnName = "MacBT";
                            dtCapPhoi.Columns["CUONGDO"].ColumnName = "CuongDo";
                            dtCapPhoi.Columns["COTLIEUMAX"].ColumnName = "CotLieuMax";
                            dtCapPhoi.Columns["DOSUT"].ColumnName = "DoSut";
                            dtCapPhoi.Columns["TONGSLVATTU"].ColumnName = "TongKhoiLuong";
                            
                            dgvCapPhoi.DataSource = dtCapPhoi;
                            
                            // Ẩn cột ID trong DataGridView
                            if (dgvCapPhoi.Columns["ID"] != null)
                                dgvCapPhoi.Columns["ID"].Visible = false;
                            
                            // Định dạng các cột sau khi gán DataSource
                            if (dgvCapPhoi.Columns["STT"] != null)
                                dgvCapPhoi.Columns["STT"].HeaderText = "STT";
                            if (dgvCapPhoi.Columns["MacBT"] != null)
                                dgvCapPhoi.Columns["MacBT"].HeaderText = "Mác BT";
                            if (dgvCapPhoi.Columns["CuongDo"] != null)
                                dgvCapPhoi.Columns["CuongDo"].HeaderText = "Cường Độ";
                            if (dgvCapPhoi.Columns["CotLieuMax"] != null)
                                dgvCapPhoi.Columns["CotLieuMax"].HeaderText = "Cốt Liệu Max";
                            if (dgvCapPhoi.Columns["DoSut"] != null)
                                dgvCapPhoi.Columns["DoSut"].HeaderText = "Độ Sụt";
                            if (dgvCapPhoi.Columns["TongKhoiLuong"] != null)
                                dgvCapPhoi.Columns["TongKhoiLuong"].HeaderText = "Tổng KL Vật Tư";
                            
                            // Tăng chiều cao header để chữ đậm dễ đọc hơn
                            dgvCapPhoi.ColumnHeadersHeight = 30;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu cấp phối: " + ex.Message);
                return;
            }
            
            if (dgvCapPhoi.Rows.Count > 0)
            {
                // Tìm cột đầu tiên không bị ẩn để làm CurrentCell
                foreach (DataGridViewColumn col in dgvCapPhoi.Columns)
                {
                    if (col.Visible)
                    {
                        dgvCapPhoi.CurrentCell = dgvCapPhoi.Rows[0].Cells[col.Index];
                        break;
                    }
                }
                LoadRowToForm(dgvCapPhoi.Rows[0]);
            }
            else
            {
                // Xóa form nếu không có dữ liệu
                ClearForm();
            }
        }
        
        private void ClearForm()
        {
            // Kiểm tra xem các điều khiển đã được khởi tạo chưa
            if (txtSTT != null)
            {
                txtSTT.Text = "";
                txtMacBT.Text = "";
                txtCuongDo.Text = "";
                txtCotLieuMax.Text = "";
                txtDoSut.Text = "";
                txtTongKhoiLuong.Text = "";
                currentId = -1;
            }
        }
        
        private void LoadRowToForm(DataGridViewRow row)
        {
            // Lấy giá trị từ DataTable thay vì DataGridView
            DataRowView dataRow = (DataRowView)row.DataBoundItem;
            currentId = Convert.ToInt32(dataRow["ID"]);
            txtSTT.Text = dataRow["STT"]?.ToString() ?? "";
            txtMacBT.Text = dataRow["MacBT"]?.ToString() ?? "";
            txtCuongDo.Text = dataRow["CuongDo"]?.ToString() ?? "";
            txtCotLieuMax.Text = dataRow["CotLieuMax"]?.ToString() ?? "";
            txtDoSut.Text = dataRow["DoSut"]?.ToString() ?? "";
            txtTongKhoiLuong.Text = dataRow["TongKhoiLuong"]?.ToString() ?? "";
        }
        
        private void DgvCapPhoi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCapPhoi.Rows[e.RowIndex];
                LoadRowToForm(row);
            }
        }
        
        private void BtnThemMoi_Click(object sender, EventArgs e)
        {
            ClearForm();
            txtSTT.Text = GetNextSTT().ToString();
            txtMacBT.Focus();
        }
        
        private void BtnCapNhat_Click(object sender, EventArgs e)
        {
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(txtMacBT.Text))
            {
                MessageBox.Show("Vui lòng nhập Mác BT.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMacBT.Focus();
                return;
            }
            
            // Kiểm tra TongKhoiLuong có phải là số không
            if (!decimal.TryParse(txtTongKhoiLuong.Text, out decimal tongKhoiLuong))
            {
                MessageBox.Show("Tổng khối lượng phải là một số.", "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTongKhoiLuong.Focus();
                return;
            }
            
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    
                    if (currentId == -1) // Thêm mới
                    {
                        // Kiểm tra STT đã tồn tại chưa
                        using (var checkCmd = new SqlCommand("SELECT COUNT(*) FROM CAPPHOI WHERE STT = @stt", conn))
                        {
                            checkCmd.Parameters.AddWithValue("@stt", int.Parse(txtSTT.Text));
                            int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                            
                            if (count > 0)
                            {
                                MessageBox.Show("STT đã tồn tại. Vui lòng chọn STT khác.", "Trùng STT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                txtSTT.Focus();
                                return;
                            }
                        }
                        
                        using (var cmd = new SqlCommand(@"
                        INSERT INTO CAPPHOI (STT, MACBETONG, CUONGDO, COTLIEUMAX, DOSUT, TONGSLVATTU)
                        VALUES (@stt, @macbt, @cuongdo, @cotlieumax, @dosut, @tongsl);
                        SELECT SCOPE_IDENTITY();", conn))
                        {
                            cmd.Parameters.Add("@stt", SqlDbType.Int).Value = int.Parse(txtSTT.Text);
                            cmd.Parameters.Add("@macbt", SqlDbType.NVarChar).Value = txtMacBT.Text;
                            cmd.Parameters.Add("@cuongdo", SqlDbType.NVarChar).Value = txtCuongDo.Text;
                            cmd.Parameters.Add("@cotlieumax", SqlDbType.NVarChar).Value = txtCotLieuMax.Text;
                            cmd.Parameters.Add("@dosut", SqlDbType.NVarChar).Value = txtDoSut.Text;
                            cmd.Parameters.Add("@tongsl", SqlDbType.Decimal).Value = tongKhoiLuong;
                            
                            object result = cmd.ExecuteScalar();
                            
                            if (result != null && int.TryParse(result.ToString(), out int newId))
                            {
                                // Load lại dữ liệu
                                LoadData();
                                
                                // Tìm và chọn dòng vừa thêm
                                foreach (DataGridViewRow row in dgvCapPhoi.Rows)
                                {
                                    if (Convert.ToInt32(((DataRowView)row.DataBoundItem)["ID"]) == newId)
                                    {
                                        row.Selected = true;
                                        dgvCapPhoi.FirstDisplayedScrollingRowIndex = row.Index;
                                        LoadRowToForm(row);
                                        break;
                                    }
                                }
                                
                                MessageBox.Show("Thêm cấp phối thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Không thể thêm cấp phối. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else // Cập nhật
                    {
                        using (var cmd = new SqlCommand(@"
                        UPDATE CAPPHOI 
                        SET STT = @stt, MACBETONG = @macbt, CUONGDO = @cuongdo, 
                            COTLIEUMAX = @cotlieumax, DOSUT = @dosut, TONGSLVATTU = @tongsl
                        WHERE MACAPPHOI = @id", conn))
                        {
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = currentId;
                            cmd.Parameters.Add("@stt", SqlDbType.Int).Value = int.Parse(txtSTT.Text);
                            cmd.Parameters.Add("@macbt", SqlDbType.NVarChar).Value = txtMacBT.Text;
                            cmd.Parameters.Add("@cuongdo", SqlDbType.NVarChar).Value = txtCuongDo.Text;
                            cmd.Parameters.Add("@cotlieumax", SqlDbType.NVarChar).Value = txtCotLieuMax.Text;
                            cmd.Parameters.Add("@dosut", SqlDbType.NVarChar).Value = txtDoSut.Text;
                            cmd.Parameters.Add("@tongsl", SqlDbType.Decimal).Value = tongKhoiLuong;
                            
                            int rowsAffected = cmd.ExecuteNonQuery();
                            
                            if (rowsAffected > 0)
                            {
                                // Load lại dữ liệu
                                LoadData();
                                
                                // Tìm và chọn dòng vừa cập nhật
                                foreach (DataGridViewRow row in dgvCapPhoi.Rows)
                                {
                                    if (Convert.ToInt32(((DataRowView)row.DataBoundItem)["ID"]) == currentId)
                                    {
                                        row.Selected = true;
                                        dgvCapPhoi.FirstDisplayedScrollingRowIndex = row.Index;
                                        LoadRowToForm(row);
                                        break;
                                    }
                                }
                                
                                MessageBox.Show("Cập nhật cấp phối thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Không thể cập nhật cấp phối. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu cấp phối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (currentId == -1)
            {
                MessageBox.Show("Vui lòng chọn một cấp phối để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa cấp phối này?", "Xác nhận xóa", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (var conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand("DELETE FROM CAPPHOI WHERE MACAPPHOI = @id", conn))
                        {
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = currentId;
                            cmd.ExecuteNonQuery();
                            
                            // Load lại dữ liệu
                            LoadData();
                            
                            MessageBox.Show("Xóa cấp phối thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa dữ liệu cấp phối: " + ex.Message);
                }
            }
        }
        
        private void BtnLamMoi_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        
        private int GetNextSTT()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT ISNULL(MAX(STT), 0) + 1 FROM CAPPHOI", conn))
                    {
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch
            {
                return 1;
            }
        }
    }
}