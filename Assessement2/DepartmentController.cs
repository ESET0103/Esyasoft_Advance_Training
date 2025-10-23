using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessement2
{


    /* 
        GetAllDepartments(SqlConnection conn)
        GetDepartmentById(SqlConnection conn)
        GetProfessorsInDepartment(SqlConnection conn)
        GetStudentsInDepartment(SqlConnection conn)
     */


    internal class DepartmentController
    {
        // 1️⃣ Get all departments
        public void GetAllDepartments(SqlConnection conn)
        {
            string query = "SELECT * FROM Departments";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine("\n---- Department List ----");
            while (reader.Read())
            {
                Console.WriteLine($"Department ID: {reader["DepartmentId"]}");
                Console.WriteLine($"Name: {reader["DepartmentName"]}");
                Console.WriteLine($"HOD: {reader["HOD"]}");
                Console.WriteLine("---------------------------");
            }
            reader.Close();
        }

        // 2️⃣ Get department by ID
        public void GetDepartmentById(SqlConnection conn)
        {
            Console.Write("Enter Department ID: ");
            int deptId = Convert.ToInt32(Console.ReadLine());

            string query = "SELECT * FROM Departments WHERE DepartmentId = @DeptId";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DeptId", deptId);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine("\n---- Department Details ----");
                Console.WriteLine($"Department ID: {reader["DepartmentId"]}");
                Console.WriteLine($"Department Name: {reader["DepartmentName"]}");
                Console.WriteLine($"HOD: {reader["HOD"]}");
            }
            else
            {
                Console.WriteLine("No department found with the given ID.");
            }
            reader.Close();
        }

        // 3️⃣ Get professors in a department
        public void GetProfessorsInDepartment(SqlConnection conn)
        {
            Console.Write("Enter Department ID: ");
            int deptId = Convert.ToInt32(Console.ReadLine());

            string query = @"SELECT ProfessorId, FirstName, LastName, Email 
                             FROM Professors 
                             WHERE DepartmentId = @DeptId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DeptId", deptId);

            SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine($"\n---- Professors in Department ID {deptId} ----");
            bool found = false;
            while (reader.Read())
            {
                found = true;
                Console.WriteLine($"Professor ID: {reader["ProfessorId"]}");
                Console.WriteLine($"Name: {reader["FirstName"]} {reader["LastName"]}");
                Console.WriteLine($"Email: {reader["Email"]}");
                Console.WriteLine("---------------------------");
            }
            if (!found)
                Console.WriteLine("No professors found in this department.");
            reader.Close();
        }

        // 4️⃣ Get students in a department
        public void GetStudentsInDepartment(SqlConnection conn)
        {
            Console.Write("Enter Department ID: ");
            int deptId = Convert.ToInt32(Console.ReadLine());

            string query = @"SELECT StudentId, FirstName, LastName, Email 
                             FROM Students 
                             WHERE DepartmentId = @DeptId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DeptId", deptId);

            SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine($"\n---- Students in Department ID {deptId} ----");
            bool found = false;
            while (reader.Read())
            {
                found = true;
                Console.WriteLine($"Student ID: {reader["StudentId"]}");
                Console.WriteLine($"Name: {reader["FirstName"]} {reader["LastName"]}");
                Console.WriteLine($"Email: {reader["Email"]}");
                Console.WriteLine("---------------------------");
            }
            if (!found)
                Console.WriteLine("No students found in this department.");
            reader.Close();
        }
        
    }
}
