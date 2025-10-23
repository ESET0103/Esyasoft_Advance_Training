using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Assessement2
{
    class RegistrationController
    {
        string email;
        string password;
        string firstName;
        string lastName;
        string role = "";
        //string isStudent = "";

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
        public bool checkIfUserExist(SqlConnection conn, string email)
        {
            string query = "SELECT 1 FROM [User] WHERE email = @Email";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    bool exists = reader.Read();
                    reader.Close();
                    return exists;
                }
            }
        }
        public bool Register(SqlConnection conn)
        {
            Console.Write("Enter Your First Name : ");
            firstName = Console.ReadLine();
            Console.Write("Enter Your Last Name : ");
            lastName = Console.ReadLine();
            Console.Write("Enter Your Email : ");
            email = Console.ReadLine();
            Console.Write("Enter Your Password :");
            password = Console.ReadLine();

            string hashPassword = HashPassword(password);
            Console.Write("Enter Your Role(1. Professor  2. Student");
            int  x = Convert.ToInt32(Console.ReadLine());
            if(x == 1)
            {
                role = "Professor";
            }
            else
            {
                role = "Student";
            }

            if (checkIfUserExist(conn, email)) {
                Console.WriteLine("User Already Exist....");
                return false;
            }
            else
            {
                string query = "insert into [User] (FirstName,LastName,email,Password,UserRole) values(@firstName, @lastName, @email, @hashPassword, @role)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@firstName",firstName);
                cmd.Parameters.AddWithValue("@lastName",lastName);
                cmd.Parameters.AddWithValue("@email",email);
                cmd.Parameters.AddWithValue("@hashPassword",hashPassword);
                cmd.Parameters.AddWithValue("@role",role);
                //cmd.Parameters.AddWithValue("@isStudent",isStudent);

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0) return true;

                return false;

            }





        }

        

    }
}
