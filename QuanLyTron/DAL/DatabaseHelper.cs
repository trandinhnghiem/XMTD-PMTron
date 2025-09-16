using System.Configuration; // Để đọc App.config
using System.Data.SqlClient;

namespace QuanLyTron.DAL
{
    public static class DatabaseHelper
    {
        // Lấy chuỗi kết nối từ App.config
        private static readonly string connectionString =
            ConfigurationManager.ConnectionStrings["QuanLyTramDB"].ConnectionString;
            
        // Lấy StationId từ App.config
        public static int CurrentStationId
        {
            get
            {
                // Đọc giá trị StationId từ file cấu hình
                string stationIdStr = ConfigurationManager.AppSettings["StationId"];
                
                // Chuyển đổi sang số nguyên, nếu không hợp lệ thì trả về 1 (mặc định)
                if (int.TryParse(stationIdStr, out int stationId))
                {
                    return stationId;
                }
                
                // Giá trị mặc định nếu không tìm thấy hoặc không hợp lệ
                return 1;
            }
        }
        
        // Hàm trả về một SqlConnection
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
        
        // Hàm lấy tên trạm hiện tại (dùng cho hiển thị)
        public static string GetCurrentStationName()
        {
            string stationName = "Không xác định";
            
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT TENTRAM FROM TRAM WHERE MATRAM = @stationId";
                    
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@stationId", CurrentStationId);
                        
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            stationName = result.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi nếu cần
                    System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy tên trạm: {ex.Message}");
                }
            }
            
            return stationName;
        }
    }
}