using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessement2
{
    public class DBConnection
    {
        private readonly string _connectionString;
        public DBConnection()
        {
            _connectionString = @"Data Source=" + @"LAPTOP-P4L1AUR8" +
                                    ";Initial Catalog=" + "dotnetApp" +
                                    ";Integrated Security=True;" +
                                    "Encrypt=True;" +
                                    "TrustServerCertificate=True;";
        }

        public SqlConnection connectDB()
        {
            return new SqlConnection(_connectionString);
        } 
    }
}
