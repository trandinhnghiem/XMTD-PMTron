using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.CompilerServices;
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
        private Button btnThemMoi, btnCapNhat;
        private DataTable dtCapPhoi;

        public CapPhoiForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "QUẢN LÝ CẤP PHỐI";
            this.Width = 1200;
            this.Height = 720;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(255, 250, 230);
            
            // ====== SplitContainer ======
            SplitContainer splitTop = new SplitContainer
            {
                Dock = DockStyle.Top,
                Height = 350,
                Orientation = Orientation.Vertical,
                IsSplitterFixed = false,
                BackColor = Color.White
            };
            this.Load += (s, e) =>
            {
                splitTop.SplitterDistance = splitTop.Width / 2;
            };
            
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
                GridColor = Color.LightGray,
                RowHeadersVisible = false
            };
            dgvCapPhoi.Columns.Add("STT", "STT");
            dgvCapPhoi.Columns.Add("MacBT", "Mác BT");
            dgvCapPhoi.Columns.Add("CuongDo", "Cường Độ");
            dgvCapPhoi.Columns.Add("CotLieuMax", "Cốt Liệu Max");
            dgvCapPhoi.Columns.Add("DoSut", "Độ sụt");
            
            // Load dữ liệu
            LoadData();
            
            dgvCapPhoi.CellClick += DgvCapPhoi_CellClick;
            splitTop.Panel1.Controls.Add(dgvCapPhoi);
            
            // ====== Panel bên phải ======
            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightGray
            };
            splitTop.Panel2.Controls.Add(rightPanel);
            
            // ====== Panel dưới ======
            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(255, 250, 230)
            };
            
            int labelLeft = 50;          // vị trí label bên trái
            int textLeft = 200;          // textbox bên trái
            int rightLabelLeft = 650;    // label bên phải
            int rightTextLeft = 850;     // textbox bên phải
            int textWidth = 280;
            int controlHeight = 40;
            int paddingY = 20;
            Font labelFont = new Font("Segoe UI", 12, FontStyle.Bold);
            Font textFont = new Font("Segoe UI", 12);
            
            // ====== Cột trái ======
            lblSTT = new Label { Text = "STT:", Font = labelFont, Left = labelLeft, Top = 40, AutoSize = true, TextAlign = ContentAlignment.TopLeft };
            txtSTT = new TextBox { Font = textFont, Left = textLeft, Top = lblSTT.Top, Width = textWidth, Height = controlHeight };
            lblMacBT = new Label { Text = "Mác BT:", Font = labelFont, Left = labelLeft, Top = lblSTT.Bottom + paddingY, AutoSize = true, TextAlign = ContentAlignment.TopLeft };
            txtMacBT = new TextBox { Font = textFont, Left = textLeft, Top = lblMacBT.Top, Width = textWidth, Height = controlHeight };
            lblCuongDo = new Label { Text = "Cường Độ:", Font = labelFont, Left = labelLeft, Top = lblMacBT.Bottom + paddingY, AutoSize = true, TextAlign = ContentAlignment.TopLeft };
            txtCuongDo = new TextBox { Font = textFont, Left = textLeft, Top = lblCuongDo.Top, Width = textWidth, Height = controlHeight };
            
            // ====== Cột phải ======
            lblCotLieuMax = new Label { Text = "Cốt Liệu Max:", Font = labelFont, Left = rightLabelLeft, Top = 40, AutoSize = true, TextAlign = ContentAlignment.TopLeft };
            txtCotLieuMax = new TextBox { Font = textFont, Left = rightTextLeft, Top = lblCotLieuMax.Top, Width = textWidth, Height = controlHeight };
            lblDoSut = new Label { Text = "Độ Sụt:", Font = labelFont, Left = rightLabelLeft, Top = lblCotLieuMax.Bottom + paddingY, AutoSize = true, TextAlign = ContentAlignment.TopLeft };
            txtDoSut = new TextBox { Font = textFont, Left = rightTextLeft, Top = lblDoSut.Top, Width = textWidth, Height = controlHeight };
            lblTongKhoiLuong = new Label { Text = "Tổng khối lượng:", Font = labelFont, Left = rightLabelLeft, Top = lblDoSut.Bottom + paddingY, AutoSize = true, TextAlign = ContentAlignment.TopLeft };
            txtTongKhoiLuong = new TextBox { Font = textFont, Left = rightTextLeft, Top = lblTongKhoiLuong.Top, Width = textWidth, Height = controlHeight, ReadOnly = true };
            
            // Nút bấm THÊM MỚI
            btnThemMoi = new IconButton
            {
                Width = 180,
                Height = 45,
                Text = "THÊM MỚI",
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                IconChar = IconChar.PlusCircle,     // icon FontAwesome
                IconColor = Color.White,
                IconSize = 24,
                TextImageRelation = TextImageRelation.ImageBeforeText // icon nằm trước chữ
            };
            
            // Nút bấm CẬP NHẬT
            btnCapNhat = new IconButton
            {
                Width = 180,
                Height = 45,
                Text = "CẬP NHẬT",
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                IconChar = IconChar.Save,           // icon FontAwesome
                IconColor = Color.White,
                IconSize = 24,
                TextImageRelation = TextImageRelation.ImageBeforeText
            };
            
            btnThemMoi.Top = 250;
            btnCapNhat.Top = 250;
            btnThemMoi.Left = (bottomPanel.Width / 2) - btnThemMoi.Width - 20;
            btnCapNhat.Left = (bottomPanel.Width / 2) + 20;
            bottomPanel.Resize += (s, e) =>
            {
                btnThemMoi.Left = (bottomPanel.Width / 2) - btnThemMoi.Width - 20;
                btnCapNhat.Left = (bottomPanel.Width / 2) + 20;
            };
            
            bottomPanel.Controls.AddRange(new Control[]
            {
                lblSTT, txtSTT, lblMacBT, txtMacBT, lblCuongDo, txtCuongDo,
                lblCotLieuMax, txtCotLieuMax, lblDoSut, txtDoSut,
                lblTongKhoiLuong, txtTongKhoiLuong,
                btnThemMoi, btnCapNhat
            });
            
            this.Controls.Add(bottomPanel);
            this.Controls.Add(splitTop);
            
            // Đăng ký sự kiện
            btnThemMoi.Click += BtnThemMoi_Click;
            btnCapNhat.Click += BtnCapNhat_Click;
        }
        
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
                            dtCapPhoi.Columns["STT"].ColumnName = "STT";
                            dtCapPhoi.Columns["MACBETONG"].ColumnName = "MacBT";
                            dtCapPhoi.Columns["CUONGDO"].ColumnName = "CuongDo";
                            dtCapPhoi.Columns["COTLIEUMAX"].ColumnName = "CotLieuMax";
                            dtCapPhoi.Columns["DOSUT"].ColumnName = "DoSut";
                            dtCapPhoi.Columns["TONGSLVATTU"].ColumnName = "TongKhoiLuong";
                            
                            dgvCapPhoi.DataSource = dtCapPhoi;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu cấp phối: " + ex.Message);
            }
            
            if (dgvCapPhoi.Rows.Count > 0)
            {
                dgvCapPhoi.CurrentCell = dgvCapPhoi.Rows[0].Cells[0];
                LoadRowToForm(dgvCapPhoi.Rows[0]);
            }
        }
        
        private void LoadRowToForm(DataGridViewRow row)
        {
            txtSTT.Text = row.Cells["STT"].Value?.ToString() ?? "";
            txtMacBT.Text = row.Cells["MacBT"].Value?.ToString() ?? "";
            txtCuongDo.Text = row.Cells["CuongDo"].Value?.ToString() ?? "";
            txtCotLieuMax.Text = row.Cells["CotLieuMax"].Value?.ToString() ?? "";
            txtDoSut.Text = row.Cells["DoSut"].Value?.ToString() ?? "";
            txtTongKhoiLuong.Text = row.Cells["TongKhoiLuong"].Value?.ToString() ?? "";
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
            // Clear form
            txtSTT.Text = "";
            txtMacBT.Text = "";
            txtCuongDo.Text = "";
            txtCotLieuMax.Text = "";
            txtDoSut.Text = "";
            txtTongKhoiLuong.Text = "";
            txtSTT.Focus();
        }
        
        private void BtnCapNhat_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMacBT.Text))
            {
                MessageBox.Show("Vui lòng nhập Mác BT.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMacBT.Focus();
                return;
            }
            
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    
                    // Kiểm tra xem là thêm mới hay cập nhật
                    if (string.IsNullOrWhiteSpace(txtSTT.Text) || !int.TryParse(txtSTT.Text, out int stt))
                    {
                        // Thêm mới
                        using (var cmd = new SqlCommand(@"
                        INSERT INTO CAPPHOI (STT, MACBETONG, CUONGDO, COTLIEUMAX, DOSUT, TONGSLVATTU)
                        VALUES (@stt, @macbt, @cuongdo, @cotlieumax, @dosut, @tongsl);
                        SELECT SCOPE_IDENTITY();", conn))
                        {
                            cmd.Parameters.Add("@stt", SqlDbType.Int).Value = GetNextSTT();
                            cmd.Parameters.Add("@macbt", SqlDbType.NVarChar).Value = txtMacBT.Text;
                            cmd.Parameters.Add("@cuongdo", SqlDbType.NVarChar).Value = txtCuongDo.Text;
                            cmd.Parameters.Add("@cotlieumax", SqlDbType.NVarChar).Value = txtCotLieuMax.Text;
                            cmd.Parameters.Add("@dosut", SqlDbType.NVarChar).Value = txtDoSut.Text;
                            cmd.Parameters.Add("@tongsl", SqlDbType.Decimal).Value = Convert.ToDecimal(txtTongKhoiLuong.Text);
                            
                            int newId = Convert.ToInt32(cmd.ExecuteScalar());
                            
                            // Load lại dữ liệu
                            LoadData();
                            
                            MessageBox.Show("Thêm cấp phối thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        // Cập nhật
                        int id = Convert.ToInt32(dgvCapPhoi.CurrentRow.Cells["ID"].Value);
                        
                        using (var cmd = new SqlCommand(@"
                        UPDATE CAPPHOI 
                        SET STT = @stt, MACBETONG = @macbt, CUONGDO = @cuongdo, 
                            COTLIEUMAX = @cotlieumax, DOSUT = @dosut, TONGSLVATTU = @tongsl
                        WHERE MACAPPHOI = @id", conn))
                        {
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                            cmd.Parameters.Add("@stt", SqlDbType.Int).Value = stt;
                            cmd.Parameters.Add("@macbt", SqlDbType.NVarChar).Value = txtMacBT.Text;
                            cmd.Parameters.Add("@cuongdo", SqlDbType.NVarChar).Value = txtCuongDo.Text;
                            cmd.Parameters.Add("@cotlieumax", SqlDbType.NVarChar).Value = txtCotLieuMax.Text;
                            cmd.Parameters.Add("@dosut", SqlDbType.NVarChar).Value = txtDoSut.Text;
                            cmd.Parameters.Add("@tongsl", SqlDbType.Decimal).Value = Convert.ToDecimal(txtTongKhoiLuong.Text);
                            
                            cmd.ExecuteNonQuery();
                            
                            // Load lại dữ liệu
                            LoadData();
                            
                            MessageBox.Show("Cập nhật cấp phối thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu cấp phối: " + ex.Message);
            }
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