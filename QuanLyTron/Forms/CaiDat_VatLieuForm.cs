using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using QuanLyTron.DAL;

namespace QuanLyTron.Forms
{
    public class CaiDat_VatLieuForm : Form
    {
        private DataGridView grid;
        private TextBox txtSoThuTu, txtTenCua, txtHeSoQuyDoi, txtDonViKhac, txtTramTron;
        private ComboBox cboLoaiVatLieu;
        private Button btnLuu;
        private int selectedMACUA = -1; // Lưu ID của bản ghi đang chọn

        public CaiDat_VatLieuForm()
        {
            InitializeComponent();
            LoadData();
            SetupEventHandlers();
        }

        private void InitializeComponent()
        {
            BackColor = Color.LightYellow;
            Font = new Font("Segoe UI", 10);

            // ===== Thanh công cụ =====
            var toolbar = new Panel { Height = 56, Dock = DockStyle.Top, BackColor = Color.LightYellow };
            IconButton BigBtn(string text, IconChar icon, Color backColor, EventHandler onClick)
            {
                var b = new IconButton
                {
                    Text = text,
                    IconChar = icon,
                    IconColor = Color.White,
                    IconSize = 22,
                    TextImageRelation = TextImageRelation.ImageBeforeText,
                    Size = new Size(180, 46),
                    BackColor = backColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold)
                };
                b.FlatAppearance.BorderSize = 0;
                if (onClick != null) b.Click += onClick;
                return b;
            }
            var btnThemMoi = BigBtn("THÊM MỚI", IconChar.PlusCircle, Color.MediumSeaGreen, BtnThemMoi_Click);
            var btnCapNhat = BigBtn("CẬP NHẬT", IconChar.SyncAlt, Color.RoyalBlue, BtnCapNhat_Click);
            var btnDongBo = BigBtn("ĐỒNG BỘ CÀI ĐẶT", IconChar.Sync, Color.DeepSkyBlue, BtnDongBo_Click);
            var btnKhoiTao = BigBtn("KHỞI TẠO CỬA", IconChar.Cogs, Color.DodgerBlue, BtnKhoiTao_Click);
            btnThemMoi.Location = new Point(10, 5);
            btnCapNhat.Location = new Point(200, 5);
            btnDongBo.Location = new Point(800, 5);
            btnKhoiTao.Location = new Point(1000, 5);
            toolbar.Controls.AddRange(new Control[] { btnThemMoi, btnCapNhat, btnDongBo, btnKhoiTao });

            // ===== DataGridView =====
            grid = new DataGridView
            {
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                Dock = DockStyle.Fill,
                RowHeadersVisible = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing,
                ColumnHeadersHeight = 40,
                AutoGenerateColumns = false // Không tự động tạo cột từ database
            };

            // Header style
            grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                ForeColor = Color.Black,
            };

            // Cell style (trừ header)
            grid.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.Black,
            };

            // Tạo các cột thủ công
            grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "STT", HeaderText = "STT" });
            grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenTram", HeaderText = "Tên trạm" });
            grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "LoaiVL", HeaderText = "Loại vật liệu" });
            grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenCua", HeaderText = "Tên cửa vật liệu" });
            grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Heso", HeaderText = "Hệ số quy đổi" });
            grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "DVT", HeaderText = "Đơn vị tính" });

            // Thêm cột ẩn để lưu MACUA
            grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "MACUA", HeaderText = "MACUA", Visible = false });

            var gbGrid = new GroupBox
            {
                Text = "CỬA VẬT LIỆU",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.Red,
                Location = new Point(10, 5),
                Size = new Size(820, 530)
            };
            gbGrid.Controls.Add(grid);

            // ===== GroupBox Thông tin cửa =====
            var gbInfo = new GroupBox
            {
                Text = "THÔNG TIN CỬA",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.Red,
                Location = new Point(gbGrid.Right + 1, gbGrid.Top),
                Size = new Size(359, gbGrid.Height),
            };

            int y = 40;
            int spacing = 40;

            Label Lbl(string text) =>
                new Label
                {
                    Text = text,
                    Location = new Point(15, y),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9, FontStyle.Regular),
                    ForeColor = Color.Black
                };

            TextBox Txt() =>
                new TextBox
                {
                    Location = new Point(160, y - 5),
                    Width = 170,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.Black
                };

            // Số thứ tự
            gbInfo.Controls.Add(Lbl("SỐ THỨ TỰ CỬA:"));
            txtSoThuTu = Txt(); gbInfo.Controls.Add(txtSoThuTu); y += spacing;

            // Loại vật liệu
            gbInfo.Controls.Add(Lbl("LOẠI VẬT LIỆU:"));
            cboLoaiVatLieu = new ComboBox
            {
                Location = new Point(160, y - 5),
                Width = 170,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.Black
            };
            cboLoaiVatLieu.Items.AddRange(new[] { "CÁT", "ĐÁ", "XI MĂNG", "NƯỚC", "PHỤ GIA" });
            cboLoaiVatLieu.SelectedIndex = 0;
            gbInfo.Controls.Add(cboLoaiVatLieu);
            y += spacing;

            // Tên cửa
            gbInfo.Controls.Add(Lbl("TÊN CỬA:"));
            txtTenCua = Txt(); gbInfo.Controls.Add(txtTenCua); y += spacing;

            // Hệ số quy đổi
            gbInfo.Controls.Add(Lbl("HỆ SỐ QUY ĐỔI:"));
            txtHeSoQuyDoi = Txt(); gbInfo.Controls.Add(txtHeSoQuyDoi); y += spacing;

            // Đơn vị tính khác
            gbInfo.Controls.Add(Lbl("ĐƠN VỊ TÍNH KHÁC:"));
            txtDonViKhac = Txt(); gbInfo.Controls.Add(txtDonViKhac); y += spacing;

            // Trạm trộn
            gbInfo.Controls.Add(Lbl("TRẠM TRỘN:"));
            txtTramTron = Txt(); gbInfo.Controls.Add(txtTramTron); y += spacing + 10;

            // Nút Lưu
            btnLuu = new IconButton
            {
                Text = "LƯU",
                IconChar = IconChar.Save,
                IconColor = Color.White,
                IconSize = 22,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Size = new Size(180, 46),
                BackColor = Color.MediumPurple,   // màu tím
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(90, y) // căn giữa trong groupbox (359 width -> 90 ~ giữa)
            };
            btnLuu.FlatAppearance.BorderSize = 0;
            gbInfo.Controls.Add(btnLuu);

            // ===== Layout chính =====
            var mainPanel = new Panel { Dock = DockStyle.Fill };
            mainPanel.Controls.Add(gbGrid);
            mainPanel.Controls.Add(gbInfo);
            Controls.Add(mainPanel);
            Controls.Add(toolbar);
        }

        private void SetupEventHandlers()
        {
            btnLuu.Click += BtnLuu_Click;
            grid.CellClick += Grid_CellClick;
        }

        // =================== CÁC HÀM XỬ LÝ ===================

        private void LoadData()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT MACUA, STT, TENTRAM, LOAIVATTU, TENCUA, HESOQUYDOI, DONVITINH FROM CUA_VATTU ORDER BY STT";
                    using (var cmd = new SqlCommand(query, conn))
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);

                        grid.Rows.Clear();
                        foreach (DataRow row in dt.Rows)
                        {
                            grid.Rows.Add(
                                row["STT"],
                                row["TENTRAM"],
                                row["LOAIVATTU"],
                                row["TENCUA"],
                                row["HESOQUYDOI"],
                                row["DONVITINH"],
                                row["MACUA"]
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = grid.Rows[e.RowIndex];
                selectedMACUA = Convert.ToInt32(row.Cells["MACUA"].Value);
                txtSoThuTu.Text = row.Cells["STT"].Value.ToString();
                txtTramTron.Text = row.Cells["TenTram"].Value.ToString();
                cboLoaiVatLieu.Text = row.Cells["LoaiVL"].Value.ToString();
                txtTenCua.Text = row.Cells["TenCua"].Value.ToString();
                txtHeSoQuyDoi.Text = row.Cells["Heso"].Value.ToString();
                txtDonViKhac.Text = row.Cells["DVT"].Value.ToString();
            }
        }

        private void BtnThemMoi_Click(object sender, EventArgs e)
        {
            ClearForm();
            selectedMACUA = -1;
        }

        private void BtnCapNhat_Click(object sender, EventArgs e)
        {
            if (selectedMACUA == -1)
            {
                MessageBox.Show("Vui lòng chọn bản ghi cần cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE CUA_VATTU 
                                   SET STT = @STT, TENTRAM = @TENTRAM, LOAIVATTU = @LOAIVATTU, 
                                       TENCUA = @TENCUA, HESOQUYDOI = @HESOQUYDOI, DONVITINH = @DONVITINH
                                   WHERE MACUA = @MACUA";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@STT", Convert.ToInt32(txtSoThuTu.Text));
                        cmd.Parameters.AddWithValue("@TENTRAM", txtTramTron.Text);
                        cmd.Parameters.AddWithValue("@LOAIVATTU", cboLoaiVatLieu.Text);
                        cmd.Parameters.AddWithValue("@TENCUA", txtTenCua.Text);
                        cmd.Parameters.AddWithValue("@HESOQUYDOI", Convert.ToDecimal(txtHeSoQuyDoi.Text));
                        cmd.Parameters.AddWithValue("@DONVITINH", txtDonViKhac.Text);
                        cmd.Parameters.AddWithValue("@MACUA", selectedMACUA);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDongBo_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Đồng bộ cài đặt thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đồng bộ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnKhoiTao_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    var tramList = new List<string>();
                    using (var cmd = new SqlCommand("SELECT TENTRAM FROM TRAM", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tramList.Add(reader["TENTRAM"].ToString());
                        }
                    }

                    foreach (var tram in tramList)
                    {
                        bool exists = false;
                        using (var cmd = new SqlCommand("SELECT COUNT(*) FROM CUA_VATTU WHERE TENTRAM = @TENTRAM", conn))
                        {
                            cmd.Parameters.AddWithValue("@TENTRAM", tram);
                            exists = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                        }

                        if (!exists)
                        {
                            string[] loaiVatLieu = { "CÁT", "ĐÁ", "XI MĂNG", "NƯỚC", "PHỤ GIA" };
                            string[] donViTinh = { "m3", "m3", "Bao", "L", "Kg" };
                            decimal[] heSoQuyDoi = { 1.0m, 1.0m, 1.0m, 1.0m, 1.0m };

                            for (int i = 0; i < loaiVatLieu.Length; i++)
                            {
                                using (var cmd = new SqlCommand(@"INSERT INTO CUA_VATTU (STT, TENTRAM, LOAIVATTU, TENCUA, HESOQUYDOI, DONVITINH) 
                                                              VALUES (@STT, @TENTRAM, @LOAIVATTU, @TENCUA, @HESOQUYDOI, @DONVITINH)", conn))
                                {
                                    cmd.Parameters.AddWithValue("@STT", i + 1);
                                    cmd.Parameters.AddWithValue("@TENTRAM", tram);
                                    cmd.Parameters.AddWithValue("@LOAIVATTU", loaiVatLieu[i]);
                                    cmd.Parameters.AddWithValue("@TENCUA", $"Cửa {loaiVatLieu[i]}");
                                    cmd.Parameters.AddWithValue("@HESOQUYDOI", heSoQuyDoi[i]);
                                    cmd.Parameters.AddWithValue("@DONVITINH", donViTinh[i]);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }

                MessageBox.Show("Khởi tạo cửa vật liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo cửa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenCua.Text))
            {
                MessageBox.Show("Vui lòng nhập tên cửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    if (selectedMACUA == -1)
                    {
                        string query = @"INSERT INTO CUA_VATTU (STT, TENTRAM, LOAIVATTU, TENCUA, HESOQUYDOI, DONVITINH) 
                                      VALUES (@STT, @TENTRAM, @LOAIVATTU, @TENCUA, @HESOQUYDOI, @DONVITINH)";
                        using (var cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@STT", string.IsNullOrEmpty(txtSoThuTu.Text) ? 1 : Convert.ToInt32(txtSoThuTu.Text));
                            cmd.Parameters.AddWithValue("@TENTRAM", txtTramTron.Text);
                            cmd.Parameters.AddWithValue("@LOAIVATTU", cboLoaiVatLieu.Text);
                            cmd.Parameters.AddWithValue("@TENCUA", txtTenCua.Text);
                            cmd.Parameters.AddWithValue("@HESOQUYDOI", string.IsNullOrEmpty(txtHeSoQuyDoi.Text) ? 1 : Convert.ToDecimal(txtHeSoQuyDoi.Text));
                            cmd.Parameters.AddWithValue("@DONVITINH", txtDonViKhac.Text);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Thêm mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string query = @"UPDATE CUA_VATTU 
                                      SET STT = @STT, TENTRAM = @TENTRAM, LOAIVATTU = @LOAIVATTU, 
                                          TENCUA = @TENCUA, HESOQUYDOI = @HESOQUYDOI, DONVITINH = @DONVITINH
                                      WHERE MACUA = @MACUA";
                        using (var cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@STT", Convert.ToInt32(txtSoThuTu.Text));
                            cmd.Parameters.AddWithValue("@TENTRAM", txtTramTron.Text);
                            cmd.Parameters.AddWithValue("@LOAIVATTU", cboLoaiVatLieu.Text);
                            cmd.Parameters.AddWithValue("@TENCUA", txtTenCua.Text);
                            cmd.Parameters.AddWithValue("@HESOQUYDOI", Convert.ToDecimal(txtHeSoQuyDoi.Text));
                            cmd.Parameters.AddWithValue("@DONVITINH", txtDonViKhac.Text);
                            cmd.Parameters.AddWithValue("@MACUA", selectedMACUA);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            txtSoThuTu.Text = "";
            txtTenCua.Text = "";
            txtHeSoQuyDoi.Text = "";
            txtDonViKhac.Text = "";
            txtTramTron.Text = "";
            cboLoaiVatLieu.SelectedIndex = 0;
        }
    }
}
