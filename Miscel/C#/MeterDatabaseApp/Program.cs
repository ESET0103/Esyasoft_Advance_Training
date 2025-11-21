using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MeterDatabaseApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("User Bulk Insert Application");
            Console.WriteLine("=============================");

            string connectionString = @"Data Source=LAPTOP-P4L1AUR8;Initial Catalog=EsyasoftMeterDatabase;Trusted_Connection=True;TrustServerCertificate=true;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    Console.WriteLine("Database Connected Successfully....");

                    // Call table-valued function
                    await GetUsers(conn);

                    // Call stored procedure
                    Console.WriteLine("\nCalling CheckForRecentUser procedure...");
                    await CallProcedure(conn, 1); // Pass userId = 1
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n❌ Error: {ex.Message}");
                }
            }
        }

        public static async Task GetUsers(SqlConnection conn)
        {
            Console.Write("Enter Days : ");
            int x = Convert.ToInt32(Console.ReadLine());

            string query = "SELECT * FROM dbo.getRecentUsersTable(@days)";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@days", x);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    Console.WriteLine("\nRecent Users:\n");

                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine(
                            $"UserId: {reader["userId"]}, " +
                            $"UserName: {reader["userName"]}, " +
                            $"DisplayName: {reader["displayName"]}, " +
                            $"LastLoginUtc: {reader["LastLoginUtc"]}"
                        );
                    }
                }
            }
        }

        public static async Task CallProcedure(SqlConnection conn, int userId)
        {
            using (SqlCommand cmd = new SqlCommand("CheckForRecentUser", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);

                // If procedure uses PRINT statements, they won't return rows.
                // For demonstration, we will use ExecuteNonQuery
                int result = await cmd.ExecuteNonQueryAsync();
                Console.WriteLine($"\nProcedure executed for UserId: {userId}");
            }
        }
    }
}
