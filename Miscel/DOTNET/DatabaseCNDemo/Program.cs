using System;
using System.Data.SqlClient;
using Npgsql;
namespace DatabaseCNDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Console.WriteLine("Getting Connection ..."); 

            var datasource = @"LAPTOP-P4L1AUR8"; // your server
            var database = "db_demo"; // your database name
            //var username = "DESKTOP-LCNNVH9\\kk"; // username of server to connect
            //var password = ""; // password

            // Create your connection string
            string connString = @"Data Source=" + datasource +
                ";Initial Catalog=" + database + "; Trusted_Connection=True;";


            Console.WriteLine(connString);

            SqlConnection conn = new SqlConnection(connString);

            try
            {
                Console.WriteLine("Opening Connection ...");
                // Open the connection
                conn.Open();
                Console.WriteLine("Connection successful!");
                //InsertStaff(conn);
                displayStaff(conn);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                // Close the connection
                conn.Close();
            }


        }
        static void InsertStaff(SqlConnection conn)
        {
            Console.Write("Enter the Staff Name...");

            string myname = "Naruto";
            int studentId = 1;
            string contactno = "5653253654";

            string querry = "insert into StudentTable values(@studentId,@myname,@contactno)";
            SqlCommand cm = new SqlCommand(querry, conn);
            cm.Parameters.AddWithValue("@Myname", myname);
            cm.Parameters.AddWithValue("@studentId", studentId);
            cm.Parameters.AddWithValue("@contactno", contactno);

            int rows = cm.ExecuteNonQuery();

            if (rows > 0)
            {
                Console.WriteLine("Inserted recordsuccessfully");
            }


        }
        static void displayStaff(SqlConnection conn)
        {
            string query = "select * from StudentTable";
            SqlCommand cm = new SqlCommand(query, conn);
            SqlDataReader reader = cm.ExecuteReader();
            Console.WriteLine("Student :");
            while (reader.Read())
            {
                {
                    Console.WriteLine($"Name : {reader["Student_name"]}");
                    Console.WriteLine($"Student Id : {reader["StudentId"]}");
                    Console.WriteLine($"Contact Number : {reader["Contact_no"]}");

                }
            }
        }
    }
}
