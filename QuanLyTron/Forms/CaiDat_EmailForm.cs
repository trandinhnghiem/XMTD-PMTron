using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using QuanLyTron.DAL;

namespace QuanLyTron.Forms
{
    public class CaiDat_EmailForm : Form
    {
        // Khai báo các controls
        private GroupBox gbEmail;
        private RadioButton rNone, rHourly, rDaily;
        private Label lblLink, lblEmail;
        private TextBox txtLink, txtEmails;
        private Button btnBrowse;
        
        private GroupBox gbHourly;
        private Label lblSoGio, lblMau1, lblFrom1;
        private NumericUpDown numSoGio;
        private RadioButton rTong1, rChiTiet1;
        private DateTimePicker dtFrom1;
        
        private GroupBox gbDaily;
        private Label lblGio, lblSep, lblMau2, lblFrom2;
        private NumericUpDown numHour, numMin;
        private RadioButton rTong2, rChiTiet2;
        private DateTimePicker dtFrom2;
        
        private Panel footer;
        private IconButton btnSave;
        
        // ID của trạm đang cấu hình
        private int currentTramID;

        public CaiDat_EmailForm(int tramID)
        {
            currentTramID = tramID;
            InitializeComponent();
            LoadEmailConfig();
        }

        private void InitializeComponent()
        {
            // Thiết lập form
            BackColor = Color.LightYellow;
            Font = new Font("Segoe UI", 10);
            Size = new Size(1220, 550);
            Text = "Cấu Hình Email Báo Cáo";
            
            // Groupbox cấu hình email
            gbEmail = new GroupBox
            {
                Text = "CẤU HÌNH GỬI EMAIL BÁO CÁO",
                Location = new Point(20, 20),
                Size = new Size(1180, 200),
                BackColor = Color.White
            };
            
            rNone = new RadioButton { Text = "Không gửi", Location = new Point(20, 35), AutoSize = true };
            rHourly = new RadioButton { Text = "Gửi hàng giờ", Location = new Point(150, 35), AutoSize = true };
            rDaily = new RadioButton { Text = "Gửi hằng ngày", Location = new Point(290, 35), AutoSize = true };
            
            lblLink = new Label { Text = "LINK FILE BÁO CÁO:", Location = new Point(20, 85), AutoSize = true };
            txtLink = new TextBox { Location = new Point(170, 80), Width = 550 };
            btnBrowse = new Button { Text = "...", Location = new Point(730, 78), Size = new Size(40, 30) };
            
            lblEmail = new Label { Text = "EMAIL NHẬN:", Location = new Point(820, 35), AutoSize = true };
            txtEmails = new TextBox
            {
                Location = new Point(820, 60),
                Size = new Size(330, 110),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            
            gbEmail.Controls.AddRange(new Control[] { rNone, rHourly, rDaily, lblLink, txtLink, btnBrowse, lblEmail, txtEmails });
            
            // Groupbox cấu hình gửi hàng giờ
            gbHourly = new GroupBox
            {
                Text = "CẤU HÌNH GỬI BÁO CÁO HÀNG GIỜ",
                Location = new Point(20, 240),
                Size = new Size(580, 200),
                BackColor = Color.White
            };
            
            lblSoGio = new Label { Text = "SỐ GIỜ GỬI:", Location = new Point(20, 45), AutoSize = true };
            numSoGio = new NumericUpDown { Location = new Point(110, 42), Minimum = 1, Maximum = 24, Value = 1, Width = 60 };
            lblMau1 = new Label { Text = "MẪU BÁO CÁO:", Location = new Point(20, 95), AutoSize = true };
            rTong1 = new RadioButton { Text = "Số liệu trộn theo tổng", Location = new Point(130, 93), AutoSize = true, Checked = true };
            rChiTiet1 = new RadioButton { Text = "Số liệu trộn chi tiết", Location = new Point(320, 93), AutoSize = true };
            lblFrom1 = new Label { Text = "TÍNH TỔNG TỪ NGÀY:", Location = new Point(20, 140), AutoSize = true };
            dtFrom1 = new DateTimePicker { Location = new Point(170, 136), Width = 180, Format = DateTimePickerFormat.Short };
            
            gbHourly.Controls.AddRange(new Control[] { lblSoGio, numSoGio, lblMau1, rTong1, rChiTiet1, lblFrom1, dtFrom1 });
            
            // Groupbox cấu hình gửi hàng ngày
            gbDaily = new GroupBox
            {
                Text = "CẤU HÌNH GỬI BÁO CÁO HÀNG NGÀY",
                Location = new Point(620, 240),
                Size = new Size(580, 200),
                BackColor = Color.White
            };
            
            lblGio = new Label { Text = "GIỜ GỬI:", Location = new Point(20, 45), AutoSize = true };
            numHour = new NumericUpDown { Location = new Point(90, 42), Minimum = 0, Maximum = 23, Width = 60 };
            lblSep = new Label { Text = ":", Location = new Point(155, 45), AutoSize = true };
            numMin = new NumericUpDown { Location = new Point(170, 42), Minimum = 0, Maximum = 59, Width = 60 };
            lblMau2 = new Label { Text = "MẪU BÁO CÁO:", Location = new Point(20, 95), AutoSize = true };
            rTong2 = new RadioButton { Text = "Số liệu trộn theo tổng", Location = new Point(130, 93), AutoSize = true, Checked = true };
            rChiTiet2 = new RadioButton { Text = "Số liệu trộn chi tiết", Location = new Point(320, 93), AutoSize = true };
            lblFrom2 = new Label { Text = "TÍNH TỔNG TỪ NGÀY:", Location = new Point(20, 140), AutoSize = true };
            dtFrom2 = new DateTimePicker { Location = new Point(170, 136), Width = 180, Format = DateTimePickerFormat.Short };
            
            gbDaily.Controls.AddRange(new Control[] { lblGio, numHour, lblSep, numMin, lblMau2, rTong2, rChiTiet2, lblFrom2, dtFrom2 });
            
            // Footer với nút lưu
            footer = new Panel { Dock = DockStyle.Bottom, Height = 85, BackColor = Color.LightYellow };
            btnSave = new IconButton
            {
                Text = "LƯU",
                IconChar = IconChar.Save,
                IconColor = Color.White,
                IconSize = 22,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Size = new Size(150, 45),
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.FlatAppearance.BorderSize = 0;
            footer.Controls.Add(btnSave);
            
            // Thêm controls vào form
            Controls.AddRange(new Control[] { gbEmail, gbHourly, gbDaily, footer });
            
            // Xử lý sự kiện
            btnBrowse.Click += BtnBrowse_Click;
            btnSave.Click += BtnSave_Click;
            rNone.CheckedChanged += RNone_CheckedChanged;
            rHourly.CheckedChanged += RHourly_CheckedChanged;
            rDaily.CheckedChanged += RDaily_CheckedChanged;
            
            footer.Resize += (s, e) =>
            {
                btnSave.Location = new Point((footer.Width - btnSave.Width) / 2, (footer.Height - btnSave.Height) / 2);
            };
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog { Filter = "Tất cả|*.*" };
            if (ofd.ShowDialog() == DialogResult.OK) 
                txtLink.Text = ofd.FileName;
        }

        private void RNone_CheckedChanged(object sender, EventArgs e)
        {
            gbHourly.Enabled = false;
            gbDaily.Enabled = false;
        }

        private void RHourly_CheckedChanged(object sender, EventArgs e)
        {
            gbHourly.Enabled = true;
            gbDaily.Enabled = false;
        }

        private void RDaily_CheckedChanged(object sender, EventArgs e)
        {
            gbHourly.Enabled = false;
            gbDaily.Enabled = true;
        }

        private void LoadEmailConfig()
        {
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM EMAILCONFIG WHERE MATRAM = @Matram";
                    
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Matram", currentTramID);
                        
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string cheDoGui = reader["CHEDOGUI"].ToString();
                                
                                // Thiết lập chế độ gửi
                                switch (cheDoGui)
                                {
                                    case "Không gửi":
                                        rNone.Checked = true;
                                        break;
                                    case "Hàng giờ":
                                        rHourly.Checked = true;
                                        break;
                                    case "Hàng ngày":
                                        rDaily.Checked = true;
                                        break;
                                }
                                
                                // Thiết lập link báo cáo
                                if (!reader.IsDBNull(reader.GetOrdinal("GHICHU")))
                                    txtLink.Text = reader["GHICHU"].ToString();
                                
                                // Thiết lập ngày gửi
                                if (!reader.IsDBNull(reader.GetOrdinal("GUITUNGAY")))
                                {
                                    DateTime guiTuNgay = Convert.ToDateTime(reader["GUITUNGAY"]);
                                    dtFrom1.Value = guiTuNgay;
                                    dtFrom2.Value = guiTuNgay;
                                }
                                
                                // Thiết lập mẫu báo cáo
                                string mauBaoCao = reader["MAUBAOCAO"].ToString();
                                if (mauBaoCao == "Tổng")
                                {
                                    rTong1.Checked = true;
                                    rTong2.Checked = true;
                                }
                                else
                                {
                                    rChiTiet1.Checked = true;
                                    rChiTiet2.Checked = true;
                                }
                            }
                            else
                            {
                                // Mặc định nếu không có cấu hình
                                rNone.Checked = true;
                                gbHourly.Enabled = false;
                                gbDaily.Enabled = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải cấu hình email: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string cheDoGui = "";
                if (rNone.Checked)
                    cheDoGui = "Không gửi";
                else if (rHourly.Checked)
                    cheDoGui = "Hàng giờ";
                else if (rDaily.Checked)
                    cheDoGui = "Hàng ngày";
                
                string mauBaoCao = "";
                if (rTong1.Checked || rTong2.Checked)
                    mauBaoCao = "Tổng";
                else
                    mauBaoCao = "Chi tiết";
                
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    
                    // Kiểm tra xem đã có cấu hình cho trạm này chưa
                    string checkQuery = "SELECT COUNT(*) FROM EMAILCONFIG WHERE MATRAM = @Matram";
                    using (var checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Matram", currentTramID);
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                        
                        if (count > 0)
                        {
                            // Cập nhật cấu hình hiện có
                            string updateQuery = @"UPDATE EMAILCONFIG 
                                                SET CHEDOGUI = @CheDoGui, 
                                                    GUITUNGAY = @GuiTuNgay, 
                                                    MAUBAOCAO = @MauBaoCao,
                                                    GHICHU = @GhiChu
                                                WHERE MATRAM = @Matram";
                            
                            using (var updateCommand = new SqlCommand(updateQuery, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@Matram", currentTramID);
                                updateCommand.Parameters.AddWithValue("@CheDoGui", cheDoGui);
                                updateCommand.Parameters.AddWithValue("@GuiTuNgay", dtFrom1.Value);
                                updateCommand.Parameters.AddWithValue("@MauBaoCao", mauBaoCao);
                                updateCommand.Parameters.AddWithValue("@GhiChu", txtLink.Text);
                                
                                updateCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // Thêm cấu hình mới
                            string insertQuery = @"INSERT INTO EMAILCONFIG 
                                                (MATRAM, CHEDOGUI, GUITUNGAY, MAUBAOCAO, GHICHU)
                                                VALUES 
                                                (@Matram, @CheDoGui, @GuiTuNgay, @MauBaoCao, @GhiChu)";
                            
                            using (var insertCommand = new SqlCommand(insertQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@Matram", currentTramID);
                                insertCommand.Parameters.AddWithValue("@CheDoGui", cheDoGui);
                                insertCommand.Parameters.AddWithValue("@GuiTuNgay", dtFrom1.Value);
                                insertCommand.Parameters.AddWithValue("@MauBaoCao", mauBaoCao);
                                insertCommand.Parameters.AddWithValue("@GhiChu", txtLink.Text);
                                
                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
                
                MessageBox.Show("Lưu cấu hình email thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu cấu hình email: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}