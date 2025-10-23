using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessement2
{
    internal class CourseController
    {
        public void GetAllCourses(SqlConnection conn)
        {
            string query = @"SELECT c.CourseId, c.CourseName, d.DepartmentName, 
                                    p.FirstName + ' ' + p.LastName AS ProfessorName
                             FROM Courses c
                             LEFT JOIN Departments d ON c.DepartmentId = d.DepartmentId
                             LEFT JOIN Professors p ON c.ProfessorId = p.ProfessorId";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine("\n---- All Courses ----");
            while (reader.Read())
            {
                Console.WriteLine($"Course ID: {reader["CourseId"]}");
                Console.WriteLine($"Course Name: {reader["CourseName"]}");
                Console.WriteLine($"Department: {reader["DepartmentName"]}");
                Console.WriteLine($"Professor: {reader["ProfessorName"]}");
                Console.WriteLine("---------------------------");
            }
            reader.Close();
        }

        // 2️⃣ Get course by ID
        public void GetCourseById(SqlConnection conn)
        {
            Console.Write("Enter Course ID: ");
            int courseId = Convert.ToInt32(Console.ReadLine());

            string query = @"SELECT c.CourseId, c.CourseName, d.DepartmentName, 
                                    p.FirstName + ' ' + p.LastName AS ProfessorName
                             FROM Courses c
                             LEFT JOIN Departments d ON c.DepartmentId = d.DepartmentId
                             LEFT JOIN Professors p ON c.ProfessorId = p.ProfessorId
                             WHERE c.CourseId = @CourseId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CourseId", courseId);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine("\n---- Course Details ----");
                Console.WriteLine($"Course ID: {reader["CourseId"]}");
                Console.WriteLine($"Course Name: {reader["CourseName"]}");
                Console.WriteLine($"Department: {reader["DepartmentName"]}");
                Console.WriteLine($"Professor: {reader["ProfessorName"]}");
            }
            else
            {
                Console.WriteLine("No course found with the given ID.");
            }
            reader.Close();
        }

        // 3️⃣ Get courses by department
        public void GetCoursesByDepartment(SqlConnection conn)
        {
            Console.Write("Enter Department ID: ");
            int deptId = Convert.ToInt32(Console.ReadLine());

            string query = @"SELECT c.CourseId, c.CourseName, 
                                    p.FirstName + ' ' + p.LastName AS ProfessorName
                             FROM Courses c
                             LEFT JOIN Professors p ON c.ProfessorId = p.ProfessorId
                             WHERE c.DepartmentId = @DeptId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DeptId", deptId);

            SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine($"\n---- Courses in Department ID {deptId} ----");
            bool found = false;
            while (reader.Read())
            {
                found = true;
                Console.WriteLine($"Course ID: {reader["CourseId"]}");
                Console.WriteLine($"Course Name: {reader["CourseName"]}");
                Console.WriteLine($"Professor: {reader["ProfessorName"]}");
                Console.WriteLine("---------------------------");
            }
            if (!found)
                Console.WriteLine("No courses found in this department.");

            reader.Close();
        }

        // 4️⃣ Get courses by professor
        public void GetCoursesByProfessor(SqlConnection conn)
        {
            Console.Write("Enter Professor ID: ");
            int profId = Convert.ToInt32(Console.ReadLine());

            string query = @"SELECT c.CourseId, c.CourseName, d.DepartmentName
                             FROM Courses c
                             LEFT JOIN Departments d ON c.DepartmentId = d.DepartmentId
                             WHERE c.ProfessorId = @ProfId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ProfId", profId);

            SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine($"\n---- Courses Assigned to Professor ID {profId} ----");
            bool found = false;
            while (reader.Read())
            {
                found = true;
                Console.WriteLine($"Course ID: {reader["CourseId"]}");
                Console.WriteLine($"Course Name: {reader["CourseName"]}");
                Console.WriteLine($"Department: {reader["DepartmentName"]}");
                Console.WriteLine("---------------------------");
            }
            if (!found)
                Console.WriteLine("No courses assigned to this professor.");

            reader.Close();
        }
    }
}
