using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QuanLyTram.DAL;

namespace QuanLyTram.Forms
{
    public class CaiDat_ChungForm : Form
    {
        // Thêm sự kiện để thông báo khi dữ liệu thay đổi
        public event EventHandler DataChanged;
        
        private Button btnThemMoi, btnCapNhat, btnLuu;
        private TextBox txtMaTram, txtTenTram, txtChuTram, txtDiaDiem, txtSoDienThoai, txtCongSuat;
        private RadioButton rdTonTai, rdPhoiMe, rdPhoiChuan;
        private DataGridView dgvTram;
        private DataTable dtTram;
        
        public CaiDat_ChungForm()
        {
            this.Text = "CÀI ĐẶT CHUNG";
            this.Size = new Size(1220, 720);
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
            
            // ===== Panel chứa 2 GroupBox =====
            Panel topPanel = new Panel()
            {
                Location = new Point(20, 20),
                Size = new Size(1180, 200)
            };
            
            // ===== GroupBox Thông tin trạm (trái) =====
            GroupBox groupInfoLeft = new GroupBox()
            {
                Text = "THÔNG TIN TRẠM",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(580, 200),
                Location = new Point(0, 0)
            };
            
            Label lblMaTram = new Label() { Text = "Mã trạm:", Location = new Point(20, 30), AutoSize = true };
            txtMaTram = new TextBox() { Location = new Point(120, 25), Width = 200, ReadOnly = true };
            
            Label lblTenTram = new Label() { Text = "Tên trạm:", Location = new Point(20, 60), AutoSize = true };
            txtTenTram = new TextBox() { Location = new Point(120, 55), Width = 200 };
            
            Label lblChuTram = new Label() { Text = "Chủ trạm:", Location = new Point(20, 90), AutoSize = true };
            txtChuTram = new TextBox() { Location = new Point(120, 85), Width = 200 };
            
            Label lblDiaDiem = new Label() { Text = "Địa điểm:", Location = new Point(20, 120), AutoSize = true };
            txtDiaDiem = new TextBox() { Location = new Point(120, 115), Width = 200 };
            
            groupInfoLeft.Controls.AddRange(new Control[] {
                lblMaTram, txtMaTram,
                lblTenTram, txtTenTram,
                lblChuTram, txtChuTram,
                lblDiaDiem, txtDiaDiem
            });
            
            // ===== GroupBox Đường dẫn chương trình (phải) =====
            GroupBox groupInfoRight = new GroupBox()
            {
                Text = "THÔNG TIN KHÁC",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(580, 200),
                Location = new Point(600, 0)
            };
            
            Label lblSoDienThoai = new Label() { Text = "Số điện thoại:", Location = new Point(20, 30), AutoSize = true };
            txtSoDienThoai = new TextBox() { Location = new Point(140, 25), Width = 250 };
            
            Label lblCongSuat = new Label() { Text = "Công suất:", Location = new Point(20, 60), AutoSize = true };
            txtCongSuat = new TextBox() { Location = new Point(140, 55), Width = 250 };
            
            rdTonTai = new RadioButton() { Text = "Trạm đang tồn tại", Location = new Point(20, 90), AutoSize = true };
            rdPhoiMe = new RadioButton() { Text = "Theo cấp phối từng mẻ", Location = new Point(20, 120), AutoSize = true };
            rdPhoiChuan = new RadioButton() { Text = "Theo cấp phối chuẩn", Location = new Point(20, 150), AutoSize = true };
            
            groupInfoRight.Controls.AddRange(new Control[] {
                lblSoDienThoai, txtSoDienThoai,
                lblCongSuat, txtCongSuat,
                rdTonTai, rdPhoiMe, rdPhoiChuan
            });
            
            topPanel.Controls.AddRange(new Control[] { groupInfoLeft, groupInfoRight });
            this.Controls.Add(topPanel);
            
            // ===== Label tiêu đề DataGridView =====
            Label dgvTitle = new Label()
            {
                Text = "DANH SÁCH TRẠM TRỘN",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 240),
                AutoSize = true
            };
            this.Controls.Add(dgvTitle);
            
            // ===== DataGridView =====
            dgvTram = new DataGridView()
            {
                Location = new Point(20, 270),
                Size = new Size(1180, 300),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            this.Controls.Add(dgvTram);
            
            // Nút chức năng dưới DataGridView
            btnThemMoi.Location = new Point(20, 590);
            btnCapNhat.Location = new Point(190, 590);
            btnLuu.Location = new Point(360, 590);
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
                    using (var cmd = new SqlCommand(
                        "SELECT MATRAM, TENTRAM, CHUTRAM, DIADIEM, CONGSUAT, DIENTHOAI, TRANGTHAI, LOAICAPPHOI FROM TRAM", conn))
                    {
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            dtTram = new DataTable();
                            adapter.Fill(dtTram);
                            dgvTram.DataSource = dtTram;
                            // Gán HeaderText thay vì đổi ColumnName
                            dgvTram.Columns["MATRAM"].HeaderText = "Mã trạm";
                            dgvTram.Columns["TENTRAM"].HeaderText = "Tên trạm";
                            dgvTram.Columns["CHUTRAM"].HeaderText = "Chủ trạm";
                            dgvTram.Columns["DIADIEM"].HeaderText = "Địa điểm";
                            dgvTram.Columns["CONGSUAT"].HeaderText = "Công suất";
                            dgvTram.Columns["DIENTHOAI"].HeaderText = "Số điện thoại";
                            dgvTram.Columns["TRANGTHAI"].HeaderText = "Trạng thái";
                            dgvTram.Columns["LOAICAPPHOI"].HeaderText = "Loại cấp phối";
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
            txtMaTram.Text = row.Cells["MATRAM"].Value.ToString();
            txtTenTram.Text = row.Cells["TENTRAM"].Value.ToString();
            txtChuTram.Text = row.Cells["CHUTRAM"].Value.ToString();
            txtDiaDiem.Text = row.Cells["DIADIEM"].Value.ToString();
            txtSoDienThoai.Text = row.Cells["DIENTHOAI"].Value.ToString();
            txtCongSuat.Text = row.Cells["CONGSUAT"].Value.ToString();
            
            string loaiCapPhoi = row.Cells["LOAICAPPHOI"].Value?.ToString();
            rdTonTai.Checked = loaiCapPhoi == "Trạm đang tồn tại";
            rdPhoiMe.Checked = loaiCapPhoi == "Theo cấp phối từng mẻ";
            rdPhoiChuan.Checked = loaiCapPhoi == "Theo cấp phối chuẩn";
        }
        
        private void BtnThemMoi_Click(object sender, EventArgs e)
        {
            txtMaTram.Text = "";
            txtTenTram.Text = "";
            txtChuTram.Text = "";
            txtDiaDiem.Text = "";
            txtSoDienThoai.Text = "";
            txtCongSuat.Text = "";
            rdTonTai.Checked = true;
            txtTenTram.Focus();
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
                                          
                    if (string.IsNullOrWhiteSpace(txtMaTram.Text))
                    {
                        // Thêm mới
                        using (var cmd = new SqlCommand(@"
                            INSERT INTO TRAM (TENTRAM, CHUTRAM, DIADIEM, CONGSUAT, DIENTHOAI, TRANGTHAI, LOAICAPPHOI)
                            VALUES (@tentram, @chutram, @diadiem, @congsuat, @dienthoai, @trangthai, @loaiCAPPHOI);
                            SELECT SCOPE_IDENTITY();", conn))
                        {
                            cmd.Parameters.Add("@tentram", SqlDbType.NVarChar).Value = txtTenTram.Text;
                            cmd.Parameters.Add("@chutram", SqlDbType.NVarChar).Value = txtChuTram.Text;
                            cmd.Parameters.Add("@diadiem", SqlDbType.NVarChar).Value = txtDiaDiem.Text;
                            cmd.Parameters.Add("@congsuat", SqlDbType.NVarChar).Value = txtCongSuat.Text;
                            cmd.Parameters.Add("@dienthoai", SqlDbType.NVarChar).Value = txtSoDienThoai.Text;
                            cmd.Parameters.Add("@trangthai", SqlDbType.NVarChar).Value = "Online";
                            cmd.Parameters.Add("@loaiCAPPHOI", SqlDbType.NVarChar).Value = loaiCapPhoi;
                            int newId = Convert.ToInt32(cmd.ExecuteScalar());
                            DataRow newRow = dtTram.NewRow();
                            newRow["MATRAM"] = newId;
                            newRow["TENTRAM"] = txtTenTram.Text;
                            newRow["CHUTRAM"] = txtChuTram.Text;
                            newRow["DIADIEM"] = txtDiaDiem.Text;
                            newRow["CONGSUAT"] = txtCongSuat.Text;
                            newRow["DIENTHOAI"] = txtSoDienThoai.Text;
                            newRow["TRANGTHAI"] = "Online";
                            newRow["LOAICAPPHOI"] = loaiCapPhoi;
                            dtTram.Rows.Add(newRow);
                            dgvTram.CurrentCell = dgvTram.Rows[dgvTram.Rows.Count - 1].Cells[0];
                            MessageBox.Show("Thêm trạm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            // Kích hoạt sự kiện dữ liệu đã thay đổi
                            OnDataChanged(EventArgs.Empty);
                        }
                    }
                    else
                    {
                        // Cập nhật
                        int maTram = Convert.ToInt32(txtMaTram.Text);
                        using (var cmd = new SqlCommand(@"
                            UPDATE TRAM 
                            SET TENTRAM = @tentram, CHUTRAM = @chutram, DIADIEM = @diadiem, 
                                CONGSUAT = @congsuat, DIENTHOAI = @dienthoai, LOAICAPPHOI = @loaiCAPPHOI
                            WHERE MATRAM = @matram", conn))
                        {
                            cmd.Parameters.Add("@matram", SqlDbType.Int).Value = maTram;
                            cmd.Parameters.Add("@tentram", SqlDbType.NVarChar).Value = txtTenTram.Text;
                            cmd.Parameters.Add("@chutram", SqlDbType.NVarChar).Value = txtChuTram.Text;
                            cmd.Parameters.Add("@diadiem", SqlDbType.NVarChar).Value = txtDiaDiem.Text;
                            cmd.Parameters.Add("@congsuat", SqlDbType.NVarChar).Value = txtCongSuat.Text;
                            cmd.Parameters.Add("@dienthoai", SqlDbType.NVarChar).Value = txtSoDienThoai.Text;
                            cmd.Parameters.Add("@loaiCAPPHOI", SqlDbType.NVarChar).Value = loaiCapPhoi;
                            cmd.ExecuteNonQuery();
                            foreach (DataRow row in dtTram.Rows)
                            {
                                if (Convert.ToInt32(row["MATRAM"]) == maTram)
                                {
                                    row["TENTRAM"] = txtTenTram.Text;
                                    row["CHUTRAM"] = txtChuTram.Text;
                                    row["DIADIEM"] = txtDiaDiem.Text;
                                    row["CONGSUAT"] = txtCongSuat.Text;
                                    row["DIENTHOAI"] = txtSoDienThoai.Text;
                                    row["LOAICAPPHOI"] = loaiCapPhoi;
                                    break;
                                }
                            }
                            MessageBox.Show("Cập nhật trạm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            // Kích hoạt sự kiện dữ liệu đã thay đổi
                            OnDataChanged(EventArgs.Empty);
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
        
        // Thêm phương thức để kích hoạt sự kiện
        protected virtual void OnDataChanged(EventArgs e)
        {
            DataChanged?.Invoke(this, e);
        }

    }
}