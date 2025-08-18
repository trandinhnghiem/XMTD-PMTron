using System.Configuration; // Để đọc App.config
using System.Data.SqlClient;

namespace QuanLyTram.DAL
{
    public static class DatabaseHelper
    {
        // Lấy chuỗi kết nối từ App.config
        private static readonly string connectionString =
            ConfigurationManager.ConnectionStrings["QuanLyTramDB"].ConnectionString;

        // Hàm trả về một SqlConnection
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
