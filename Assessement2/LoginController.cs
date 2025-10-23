using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Assessement2
{
    internal class LoginController
    {
        ProfessorController professorController = new ProfessorController();


        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                {
                    sb.Append(b.ToString("x2")); // convert to hex string
                }
                return sb.ToString();
            }
        }
        public int login(SqlConnection conn)
        {

            int role = 0;
            Console.Write("Enter email : ");
            string Email = Console.ReadLine();
            Console.Write("Enter Password : ");
            string password = Console.ReadLine();

            string hashpassword = HashPassword(password);

            string query = "select UserRole from [User] where email = @Email and Password =  @Password ";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Password", hashpassword);
            //Console.WriteLine(HASHBYTES('SHA2_256', @Password))
            SqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                Console.WriteLine("LOGIN SUCCESSFULLY...");

                if (reader["UserRole"].ToString() == "Admin")
                {
                    Console.WriteLine("Welcome Admin...");
                    role = 1;
         
                }
                else if (reader["UserRole"].ToString() == "Professor")
                {
                    Console.WriteLine("Welcome Professor...");
                    role = 2;
                }
                else{
                    Console.WriteLine("Student of NITJSR");
                    role = 3;
                }
                return role;
            }
            reader.Close();

            return 0;
        }
    }
}
