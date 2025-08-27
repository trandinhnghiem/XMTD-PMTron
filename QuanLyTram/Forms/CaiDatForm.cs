using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QuanLyTram.DAL;

namespace QuanLyTram.Forms
{
    public class CaiDatForm : Form
    {
        private Button btnThemMoi, btnCapNhat, btnLuu;
        private TextBox txtMaTram, txtTenTram, txtDiaDiem, txtSoDienThoai, txtCongSuat;
        private RadioButton rdTonTai, rdPhoiMe, rdPhoiChuan;
        private DataGridView dgvTram;
        private DataTable dtTram;

        public CaiDatForm()
        {
            this.Text = "CÀI ĐẶT HỆ THỐNG";
            this.Size = new Size(1250, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Beige;
            Font btnFont = new Font("Segoe UI", 10, FontStyle.Bold);
            
            // ===== Nút chức năng =====
            btnThemMoi = new Button()
            {
                Text = " THÊM MỚI",
                Width = 150,
                Height = 50,
                BackColor = Color.LightGreen,
                Font = btnFont,
                FlatStyle = FlatStyle.Standard,
                Image = SystemIcons.Application.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText
            };
            
            btnCapNhat = new Button()
            {
                Text = " CẬP NHẬT",
                Width = 150,
                Height = 50,
                BackColor = Color.Khaki,
                Font = btnFont,
                FlatStyle = FlatStyle.Standard,
                Image = SystemIcons.Information.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText
            };
            
            btnLuu = new Button()
            {
                Text = " LƯU",
                Width = 150,
                Height = 50,
                BackColor = Color.LightSkyBlue,
                Font = btnFont,
                FlatStyle = FlatStyle.Standard,
                Image = SystemIcons.Shield.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText
            };
            
            // ===== Panel chứa 2 GroupBox trên =====
            Panel topPanel = new Panel()
            {
                Location = new Point(20, 20),
                Size = new Size(1180, 180)
            };
            
            // ===== GroupBox Thông tin trạm (trái) =====
            GroupBox groupInfoLeft = new GroupBox()
            {
                Text = "THÔNG TIN TRẠM",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(580, 180),
                Location = new Point(0, 0)
            };
            
            Label lblMaTram = new Label() { Text = "Mã trạm:", Location = new Point(20, 30), AutoSize = true };
            txtMaTram = new TextBox() { Location = new Point(120, 25), Width = 200 };
            Label lblTenTram = new Label() { Text = "Tên trạm:", Location = new Point(20, 60), AutoSize = true };
            txtTenTram = new TextBox() { Location = new Point(120, 55), Width = 200 };
            Label lblDiaDiem = new Label() { Text = "Địa điểm:", Location = new Point(20, 90), AutoSize = true };
            txtDiaDiem = new TextBox() { Location = new Point(120, 85), Width = 200 };
            
            groupInfoLeft.Controls.AddRange(new Control[] { lblMaTram, txtMaTram, lblTenTram, txtTenTram, lblDiaDiem, txtDiaDiem });
            
            // ===== GroupBox Đường dẫn chương trình (phải) =====
            GroupBox groupInfoRight = new GroupBox()
            {
                Text = "ĐƯỜNG DẪN CHƯƠNG TRÌNH",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(580, 180),
                Location = new Point(600, 0)
            };
            
            Label lblSoDienThoai = new Label() { Text = "Số điện thoại:", Location = new Point(20, 30), AutoSize = true };
            txtSoDienThoai = new TextBox() { Location = new Point(140, 25), Width = 250 };
            Label lblCongSuat = new Label() { Text = "Công suất:", Location = new Point(20, 60), AutoSize = true };
            txtCongSuat = new TextBox() { Location = new Point(140, 55), Width = 250 };
            rdTonTai = new RadioButton() { Text = "Trạm đang tồn tại", Location = new Point(20, 90), AutoSize = true };
            rdPhoiMe = new RadioButton() { Text = "Theo cấp phối từng mẻ", Location = new Point(20, 120), AutoSize = true };
            rdPhoiChuan = new RadioButton() { Text = "Theo cấp phối chuẩn", Location = new Point(20, 150), AutoSize = true };
            
            groupInfoRight.Controls.AddRange(new Control[] { lblSoDienThoai, txtSoDienThoai, lblCongSuat, txtCongSuat, rdTonTai, rdPhoiMe, rdPhoiChuan });
            
            topPanel.Controls.AddRange(new Control[] { groupInfoLeft, groupInfoRight });
            this.Controls.Add(topPanel);
            
            // ===== Label tiêu đề DataGridView =====
            Label dgvTitle = new Label()
            {
                Text = "DANH SÁCH TRẠM TRỘN",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 220),
                AutoSize = true
            };
            this.Controls.Add(dgvTitle);
            
            // ===== DataGridView =====
            dgvTram = new DataGridView()
            {
                Location = new Point(20, 250),
                Size = new Size(1180, 300),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(dgvTram);
            
            // Nút chức năng dưới DataGridView
            btnThemMoi.Location = new Point(20, 570);
            btnCapNhat.Location = new Point(190, 570);
            btnLuu.Location = new Point(360, 570);
            this.Controls.AddRange(new Control[] { btnThemMoi, btnCapNhat, btnLuu });
            
            // Load dữ liệu
            LoadData();
            
            // Đăng ký sự kiện
            btnThemMoi.Click += BtnThemMoi_Click;
            btnCapNhat.Click += BtnCapNhat_Click;
            btnLuu.Click += BtnLuu_Click;
            dgvTram.SelectionChanged += DgvTram_SelectionChanged;
        }
        
        private void LoadData()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT MATRAM, TENTRAM, CHUTRAM, DIADIEM, CONGSUAT, DIENTHOAI, TRANGTHAI FROM TRAM", conn))
                    {
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            dtTram = new DataTable();
                            adapter.Fill(dtTram);
                            
                            // Đổi tên cột để hiển thị
                            dtTram.Columns["MATRAM"].ColumnName = "Mã trạm";
                            dtTram.Columns["TENTRAM"].ColumnName = "Tên trạm";
                            dtTram.Columns["CHUTRAM"].ColumnName = "Chủ trạm";
                            dtTram.Columns["DIADIEM"].ColumnName = "Địa điểm";
                            dtTram.Columns["CONGSUAT"].ColumnName = "Công suất";
                            dtTram.Columns["DIENTHOAI"].ColumnName = "Số điện thoại";
                            dtTram.Columns["TRANGTHAI"].ColumnName = "Trạng thái";
                            
                            dgvTram.DataSource = dtTram;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu trạm: " + ex.Message);
            }
            
            if (dgvTram.Rows.Count > 0)
            {
                dgvTram.CurrentCell = dgvTram.Rows[0].Cells[0];
                LoadRowToForm(dgvTram.Rows[0]);
            }
        }
        
        private void LoadRowToForm(DataGridViewRow row)
        {
            txtMaTram.Text = row.Cells["Mã trạm"].Value.ToString();
            txtTenTram.Text = row.Cells["Tên trạm"].Value.ToString();
            txtDiaDiem.Text = row.Cells["Địa điểm"].Value.ToString();
            txtSoDienThoai.Text = row.Cells["Số điện thoại"].Value.ToString();
            txtCongSuat.Text = row.Cells["Công suất"].Value.ToString();
            
            // Set radio button based on LOAICAUPHOI
            string loaiCapPhoi = "Trạm đang tồn tại"; // Default value
            if (row.Cells["Loại cấp phối"] != null && row.Cells["Loại cấp phối"].Value != null)
            {
                loaiCapPhoi = row.Cells["Loại cấp phối"].Value.ToString();
            }
            
            if (loaiCapPhoi == "Trạm đang tồn tại")
                rdTonTai.Checked = true;
            else if (loaiCapPhoi == "Theo cấp phối từng mẻ")
                rdPhoiMe.Checked = true;
            else if (loaiCapPhoi == "Theo cấp phối chuẩn")
                rdPhoiChuan.Checked = true;
        }
        
        private void BtnThemMoi_Click(object sender, EventArgs e)
        {
            // Clear form
            txtMaTram.Text = "";
            txtTenTram.Text = "";
            txtDiaDiem.Text = "";
            txtSoDienThoai.Text = "";
            txtCongSuat.Text = "";
            rdTonTai.Checked = true;
            txtMaTram.Focus();
        }
        
        private void BtnCapNhat_Click(object sender, EventArgs e)
        {
            if (dgvTram.CurrentRow != null)
            {
                LoadRowToForm(dgvTram.CurrentRow);
            }
        }
        
        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenTram.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên trạm.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenTram.Focus();
                return;
            }
            
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    
                    string loaiCapPhoi = rdTonTai.Checked ? "Trạm đang tồn tại" : 
                                          rdPhoiMe.Checked ? "Theo cấp phối từng mẻ" : 
                                          "Theo cấp phối chuẩn";
                    
                    // Kiểm tra xem là thêm mới hay cập nhật
                    if (string.IsNullOrWhiteSpace(txtMaTram.Text))
                    {
                        // Thêm mới
                        using (var cmd = new SqlCommand(@"
                        INSERT INTO TRAM (TENTRAM, CHUTRAM, DIADIEM, CONGSUAT, DIENTHOAI, TRANGTHAI, LOAICAUPHOI)
                        VALUES (@tentram, @chutram, @diadiem, @congsuat, @dienthoai, @trangthai, @loaicauphoi);
                        SELECT SCOPE_IDENTITY();", conn))
                        {
                            cmd.Parameters.Add("@tentram", SqlDbType.NVarChar).Value = txtTenTram.Text;
                            cmd.Parameters.Add("@chutram", SqlDbType.NVarChar).Value = txtTenTram.Text; // Giả sử chủ trạm = tên trạm
                            cmd.Parameters.Add("@diadiem", SqlDbType.NVarChar).Value = txtDiaDiem.Text;
                            cmd.Parameters.Add("@congsuat", SqlDbType.NVarChar).Value = txtCongSuat.Text;
                            cmd.Parameters.Add("@dienthoai", SqlDbType.NVarChar).Value = txtSoDienThoai.Text;
                            cmd.Parameters.Add("@trangthai", SqlDbType.NVarChar).Value = "Online";
                            cmd.Parameters.Add("@loaicauphoi", SqlDbType.NVarChar).Value = loaiCapPhoi;
                            
                            int newId = Convert.ToInt32(cmd.ExecuteScalar());
                            
                            // Thêm vào DataTable
                            DataRow newRow = dtTram.NewRow();
                            newRow["Mã trạm"] = newId;
                            newRow["Tên trạm"] = txtTenTram.Text;
                            newRow["Chủ trạm"] = txtTenTram.Text;
                            newRow["Địa điểm"] = txtDiaDiem.Text;
                            newRow["Công suất"] = txtCongSuat.Text;
                            newRow["Số điện thoại"] = txtSoDienThoai.Text;
                            newRow["Trạng thái"] = "Online";
                            newRow["Loại cấp phối"] = loaiCapPhoi;
                            dtTram.Rows.Add(newRow);
                            
                            // Cập nhật DataGridView
                            dgvTram.DataSource = dtTram;
                            dgvTram.CurrentCell = dgvTram.Rows[dgvTram.Rows.Count - 1].Cells[0];
                            
                            MessageBox.Show("Thêm trạm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        // Cập nhật
                        int maTram = Convert.ToInt32(txtMaTram.Text);
                        
                        using (var cmd = new SqlCommand(@"
                        UPDATE TRAM 
                        SET TENTRAM = @tentram, CHUTRAM = @chutram, DIADIEM = @diadiem, 
                            CONGSUAT = @congsuat, DIENTHOAI = @dienthoai, LOAICAUPHOI = @loaicauphoi
                        WHERE MATRAM = @matram", conn))
                        {
                            cmd.Parameters.Add("@matram", SqlDbType.Int).Value = maTram;
                            cmd.Parameters.Add("@tentram", SqlDbType.NVarChar).Value = txtTenTram.Text;
                            cmd.Parameters.Add("@chutram", SqlDbType.NVarChar).Value = txtTenTram.Text; // Giả sử chủ trạm = tên trạm
                            cmd.Parameters.Add("@diadiem", SqlDbType.NVarChar).Value = txtDiaDiem.Text;
                            cmd.Parameters.Add("@congsuat", SqlDbType.NVarChar).Value = txtCongSuat.Text;
                            cmd.Parameters.Add("@dienthoai", SqlDbType.NVarChar).Value = txtSoDienThoai.Text;
                            cmd.Parameters.Add("@loaicauphoi", SqlDbType.NVarChar).Value = loaiCapPhoi;
                            
                            cmd.ExecuteNonQuery();
                            
                            // Cập nhật DataTable
                            foreach (DataRow row in dtTram.Rows)
                            {
                                if (Convert.ToInt32(row["Mã trạm"]) == maTram)
                                {
                                    row["Tên trạm"] = txtTenTram.Text;
                                    row["Chủ trạm"] = txtTenTram.Text;
                                    row["Địa điểm"] = txtDiaDiem.Text;
                                    row["Công suất"] = txtCongSuat.Text;
                                    row["Số điện thoại"] = txtSoDienThoai.Text;
                                    row["Loại cấp phối"] = loaiCapPhoi;
                                    break;
                                }
                            }
                            
                            // Cập nhật DataGridView
                            dgvTram.DataSource = dtTram;
                            
                            MessageBox.Show("Cập nhật trạm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu trạm: " + ex.Message);
            }
        }
        
        private void DgvTram_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTram.CurrentRow != null)
            {
                LoadRowToForm(dgvTram.CurrentRow);
            }
        }
    }
}