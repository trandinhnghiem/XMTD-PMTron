using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using QuanLyTram.DAL;

namespace QuanLyTram.Forms
{
    public class Kho_CuaVatLieuForm : Form
    {
        private enum EditMode { None, Add, Edit }
        private EditMode _mode = EditMode.None;
        private Panel pnlActions;
        private IconButton btnThemMoi, btnCapNhat;
        private IconButton btnLuu, btnHuy;
        private DataGridView dgv;
        private GroupBox grpInfo;
        private Label lblSTT, lblTenCua, lblHeSo, lblDonVi;
        private TextBox txtSTT, txtTenCua, txtHeSo, txtDonVi;
        private Color BgBeige = Color.FromArgb(245, 245, 220); 
        private Color PanelWhite = Color.FromArgb(245, 245, 255);
        private Font FTitle => new Font(new FontFamily("Segoe UI"), 11.5f, FontStyle.Bold);
        private Font FText => new Font(new FontFamily("Segoe UI"), 10f, FontStyle.Regular);
        private DataTable dtData;
        
        public Kho_CuaVatLieuForm()
        {
            Text = "CỬA VẬT LIỆU";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1200, 720);
            BackColor = BgBeige;
            BuildActionBar();
            BuildMainArea();
            WireEvents();
            LoadData();
            ApplyMode(EditMode.None);
        }
        
        private void BuildActionBar()
        {
            pnlActions = new Panel
            {
                Dock = DockStyle.Top,
                Height = 75,
                BackColor = PanelWhite
            };
            Controls.Add(pnlActions);
            pnlActions.BringToFront();
            btnThemMoi = new IconButton
            {
                Text = "THÊM MỚI",
                Font = FTitle,
                BackColor = Color.FromArgb(110, 170, 60),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(220, 46),
                Location = new Point(24, 12),
                IconChar = IconChar.PlusCircle,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Padding = new Padding(28, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnThemMoi.FlatAppearance.BorderSize = 0;
            btnCapNhat = new IconButton
            {
                Text = "CẬP NHẬT",
                Font = FTitle,
                BackColor = Color.FromArgb(70, 130, 180),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(220, 46),
                Location = new Point(260, 12),
                IconChar = IconChar.Edit,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Padding = new Padding(28, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnCapNhat.FlatAppearance.BorderSize = 0;
            pnlActions.Controls.Add(btnThemMoi);
            pnlActions.Controls.Add(btnCapNhat);
        }
        
        private void BuildMainArea()
        {
            var main = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16, 10, 16, 16),
                BackColor = BgBeige
            };
            Controls.Add(main);
            main.BringToFront();
            var lblGridTitle = new Label
            {
                Text = "DỮ LIỆU CỬA VẬT LIỆU",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.Red,
                AutoSize = true,
                Location = new Point(20, 8)
            };
            main.Controls.Add(lblGridTitle);
            dgv = new DataGridView
            {
                Location = new Point(20, lblGridTitle.Bottom + 8),
                Size = new Size(740, 530),
                ReadOnly = true,
                MultiSelect = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = true
            };
            main.Controls.Add(dgv);
            grpInfo = new GroupBox
            {
                Text = "THÔNG TIN CỬA",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.Red,
                BackColor = Color.Transparent,
                Size = new Size(400, 560),
                Location = new Point(dgv.Right + 20, 8),
                Padding = new Padding(18)
            };
            main.Controls.Add(grpInfo);
            lblSTT = new Label
            {
                Text = "Số thứ tự:",
                Font = new Font(FText.FontFamily, FText.Size, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 50),
                ForeColor = Color.Black
            };
            txtSTT = new TextBox
            {
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Location = new Point(20, 75),
                Width = 340
            };
            lblTenCua = new Label
            {
                Text = "Tên cửa vật liệu:",
                Font = new Font(FText.FontFamily, FText.Size, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 130),
                ForeColor = Color.Black
            };
            txtTenCua = new TextBox
            {
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Location = new Point(20, 155),
                Width = 340
            };
            lblHeSo = new Label
            {
                Text = "Hệ số quy đổi (Kg):",
                Font = new Font(FText.FontFamily, FText.Size, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 210),
                ForeColor = Color.Black
            };
            txtHeSo = new TextBox
            {
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Location = new Point(20, 235),
                Width = 340
            };
            lblDonVi = new Label
            {
                Text = "Đơn vị tính:",
                Font = new Font(FText.FontFamily, FText.Size, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 290),
                ForeColor = Color.Black
            };
            txtDonVi = new TextBox
            {
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Location = new Point(20, 315),
                Width = 340
            };
            btnLuu = new IconButton
            {
                Text = " LƯU",
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                BackColor = Color.LightSkyBlue,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(160, 46),
                Location = new Point(20, 380),
                IconChar = IconChar.Save,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Padding = new Padding(28, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnLuu.FlatAppearance.BorderSize = 0;
            btnHuy = new IconButton
            {
                Text = " HỦY",
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                BackColor = Color.LightCoral,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(160, 46),
                Location = new Point(btnLuu.Right + 16, 380),
                IconChar = IconChar.TimesCircle,
                IconColor = Color.White,
                IconFont = IconFont.Auto,
                IconSize = 28,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Padding = new Padding(28, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnHuy.FlatAppearance.BorderSize = 0;
            grpInfo.Controls.Add(lblSTT);
            grpInfo.Controls.Add(txtSTT);
            grpInfo.Controls.Add(lblTenCua);
            grpInfo.Controls.Add(txtTenCua);
            grpInfo.Controls.Add(lblHeSo);
            grpInfo.Controls.Add(txtHeSo);
            grpInfo.Controls.Add(lblDonVi);
            grpInfo.Controls.Add(txtDonVi);
            grpInfo.Controls.Add(btnLuu);
            grpInfo.Controls.Add(btnHuy);
        }
        
        private void WireEvents()
        {
            btnThemMoi.Click += (s, e) => { ApplyMode(EditMode.Add); txtSTT.Focus(); };
            btnCapNhat.Click += (s, e) => { if (dgv.CurrentRow != null) ApplyMode(EditMode.Edit); };
            btnLuu.Click += (s, e) => SaveCurrent();
            btnHuy.Click += (s, e) => ApplyMode(EditMode.None);
            dgv.SelectionChanged += (s, e) =>
            {
                if (_mode == EditMode.None && dgv.CurrentRow != null)
                {
                    txtSTT.Text = dgv.CurrentRow.Cells["STT"].Value.ToString();
                    txtTenCua.Text = dgv.CurrentRow.Cells["Tên cửa vật liệu"].Value.ToString();
                    txtHeSo.Text = dgv.CurrentRow.Cells["Hệ số quy đổi"].Value.ToString();
                    txtDonVi.Text = dgv.CurrentRow.Cells["Đơn vị tính"].Value.ToString();
                }
            };
        }
        
        private void LoadData()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT MACUA, STT, TENTRAM, LOAIVATTU, TENCUA, HESOQUYDOI, DONVITINH FROM CUA_VATTU", conn))
                    {
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            dtData = new DataTable();
                            adapter.Fill(dtData);
                            
                            // Đổi tên cột để hiển thị
                            dtData.Columns["MACUA"].ColumnName = "ID";
                            dtData.Columns["STT"].ColumnName = "STT";
                            dtData.Columns["TENTRAM"].ColumnName = "Tên trạm";
                            dtData.Columns["LOAIVATTU"].ColumnName = "Loại vật liệu";
                            dtData.Columns["TENCUA"].ColumnName = "Tên cửa vật liệu";
                            dtData.Columns["HESOQUYDOI"].ColumnName = "Hệ số quy đổi";
                            dtData.Columns["DONVITINH"].ColumnName = "Đơn vị tính";
                            
                            dgv.DataSource = dtData;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu cửa vật liệu: " + ex.Message);
            }
            
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            if (dgv.Rows.Count > 0)
                dgv.CurrentCell = dgv.Rows[0].Cells[0];
        }
        
        private void ApplyMode(EditMode mode)
        {
            _mode = mode;
            bool editing = mode != EditMode.None;
            UpdateButtonState(btnThemMoi, !editing, Color.FromArgb(110, 170, 60));
            UpdateButtonState(btnCapNhat, !editing && dgv.CurrentRow != null, Color.FromArgb(70, 130, 180));
            UpdateButtonState(btnLuu, editing, Color.FromArgb(90, 90, 150));
            UpdateButtonState(btnHuy, editing, Color.FromArgb(180, 50, 50));
            if (mode == EditMode.Add)
            {
                txtSTT.Text = txtTenCua.Text = txtHeSo.Text = txtDonVi.Text = "";
            }
            else if (mode == EditMode.Edit && dgv.CurrentRow != null)
            {
                txtSTT.Text = dgv.CurrentRow.Cells["STT"].Value.ToString();
                txtTenCua.Text = dgv.CurrentRow.Cells["Tên cửa vật liệu"].Value.ToString();
                txtHeSo.Text = dgv.CurrentRow.Cells["Hệ số quy đổi"].Value.ToString();
                txtDonVi.Text = dgv.CurrentRow.Cells["Đơn vị tính"].Value.ToString();
            }
            else if (mode == EditMode.None && dgv.CurrentRow != null)
            {
                txtSTT.Text = dgv.CurrentRow.Cells["STT"].Value.ToString();
                txtTenCua.Text = dgv.CurrentRow.Cells["Tên cửa vật liệu"].Value.ToString();
                txtHeSo.Text = dgv.CurrentRow.Cells["Hệ số quy đổi"].Value.ToString();
                txtDonVi.Text = dgv.CurrentRow.Cells["Đơn vị tính"].Value.ToString();
            }
        }
        
        private void UpdateButtonState(IconButton btn, bool enabled, Color normalColor)
        {
            btn.BackColor = enabled ? normalColor : Color.FromArgb(200, 200, 200);
            Color inactiveColor = Color.FromArgb(120, 120, 120);
            btn.ForeColor = enabled ? Color.White : inactiveColor;
            btn.IconColor = btn.ForeColor;
        }
        
        private void SaveCurrent()
        {
            var stt = txtSTT.Text.Trim();
            var tenCua = txtTenCua.Text.Trim();
            var heSo = txtHeSo.Text.Trim();
            var donVi = txtDonVi.Text.Trim();
            if (string.IsNullOrWhiteSpace(tenCua))
            {
                MessageBox.Show("Vui lòng nhập Tên cửa.", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenCua.Focus();
                return;
            }
            
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    decimal hesoValue = 0;
                    decimal.TryParse(heSo, out hesoValue);
                    
                    if (_mode == EditMode.Add)
                    {
                        // Nếu STT trống, lấy giá trị tiếp theo
                        int sttValue = 0;
                        if (!int.TryParse(stt, out sttValue))
                        {
                            sttValue = GetNextSTT();
                        }
                        
                        using (var cmd = new SqlCommand(@"
                        INSERT INTO CUA_VATTU (STT, TENTRAM, LOAIVATTU, TENCUA, HESOQUYDOI, DONVITINH)
                        VALUES (@stt, @tentram, @loaivt, @tencua, @heso, @donvi);
                        SELECT SCOPE_IDENTITY();", conn))
                        {
                            cmd.Parameters.Add("@stt", SqlDbType.Int).Value = sttValue;
                            cmd.Parameters.Add("@tentram", SqlDbType.NVarChar).Value = "Trạm mới"; // Giá trị mặc định
                            cmd.Parameters.Add("@loaivt", SqlDbType.NVarChar).Value = "Chưa rõ"; // Giá trị mặc định
                            cmd.Parameters.Add("@tencua", SqlDbType.NVarChar).Value = tenCua;
                            cmd.Parameters.Add("@heso", SqlDbType.Decimal).Value = hesoValue;
                            cmd.Parameters.Add("@donvi", SqlDbType.NVarChar).Value = donVi;
                            
                            int newId = Convert.ToInt32(cmd.ExecuteScalar());
                            
                            // Thêm vào DataTable
                            DataRow newRow = dtData.NewRow();
                            newRow["ID"] = newId;
                            newRow["STT"] = sttValue;
                            newRow["Tên trạm"] = "Trạm mới";
                            newRow["Loại vật liệu"] = "Chưa rõ";
                            newRow["Tên cửa vật liệu"] = tenCua;
                            newRow["Hệ số quy đổi"] = hesoValue;
                            newRow["Đơn vị tính"] = donVi;
                            dtData.Rows.Add(newRow);
                            
                            // Cập nhật DataGridView
                            dgv.DataSource = dtData;
                            dgv.CurrentCell = dgv.Rows[dgv.Rows.Count - 1].Cells[0];
                        }
                    }
                    else if (_mode == EditMode.Edit && dgv.CurrentRow != null)
                    {
                        int maCua = Convert.ToInt32(dgv.CurrentRow.Cells["ID"].Value);
                        
                        using (var cmd = new SqlCommand(@"
                        UPDATE CUA_VATTU 
                        SET STT = @stt, TENTRAM = @tentram, LOAIVATTU = @loaivt, 
                            TENCUA = @tencua, HESOQUYDOI = @heso, DONVITINH = @donvi
                        WHERE MACUA = @maCua", conn))
                        {
                            cmd.Parameters.Add("@maCua", SqlDbType.Int).Value = maCua;
                            cmd.Parameters.Add("@stt", SqlDbType.Int).Value = int.Parse(stt);
                            cmd.Parameters.Add("@tentram", SqlDbType.NVarChar).Value = "Trạm cập nhật"; // Giá trị mặc định
                            cmd.Parameters.Add("@loaivt", SqlDbType.NVarChar).Value = "Chưa rõ"; // Giá trị mặc định
                            cmd.Parameters.Add("@tencua", SqlDbType.NVarChar).Value = tenCua;
                            cmd.Parameters.Add("@heso", SqlDbType.Decimal).Value = hesoValue;
                            cmd.Parameters.Add("@donvi", SqlDbType.NVarChar).Value = donVi;
                            
                            cmd.ExecuteNonQuery();
                            
                            // Cập nhật DataTable
                            dgv.CurrentRow.Cells["STT"].Value = stt;
                            dgv.CurrentRow.Cells["Tên cửa vật liệu"].Value = tenCua;
                            dgv.CurrentRow.Cells["Hệ số quy đổi"].Value = hesoValue;
                            dgv.CurrentRow.Cells["Đơn vị tính"].Value = donVi;
                        }
                    }
                }
                ApplyMode(EditMode.None);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu cửa vật liệu: " + ex.Message);
            }
        }
        
        private int GetNextSTT()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT ISNULL(MAX(STT), 0) + 1 FROM CUA_VATTU", conn))
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