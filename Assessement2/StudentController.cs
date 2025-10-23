using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Assessement2
{
    internal class StudentController
    {

        private int studentId; // Current logged-in student

        public StudentController(int studentId)
        {
            this.studentId = studentId;
        }

        // 1️⃣ View own profile
        public void ViewProfile(SqlConnection conn)
        {
            string query = @"SELECT s.StudentId, s.FirstName, s.LastName, s.DOB, s.Email, s.PhoneNumber, d.DepartmentName
                             FROM Students s
                             LEFT JOIN Departments d ON s.DepartmentId = d.DepartmentId
                             WHERE s.StudentId = @StudentId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@StudentId", studentId);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine("\n---- Student Profile ----");
                Console.WriteLine($"ID: {reader["StudentId"]}");
                Console.WriteLine($"Name: {reader["FirstName"]} {reader["LastName"]}");
                Console.WriteLine($"DOB: {reader["DOB"]}");
                Console.WriteLine($"Email: {reader["Email"]}");
                Console.WriteLine($"Phone: {reader["PhoneNumber"]}");
                Console.WriteLine($"Department: {reader["DepartmentName"]}");
            }
            else
            {
                Console.WriteLine("Profile not found.");
            }
            reader.Close();
        }

        // 2️⃣ Update own profile (email / phone)
        public void UpdateProfile(SqlConnection conn)
        {
            Console.Write("Enter new Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter new Phone Number: ");
            string phone = Console.ReadLine();

            string query = @"UPDATE Students
                             SET Email = @Email, PhoneNumber = @Phone
                             WHERE StudentId = @StudentId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@StudentId", studentId);

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "✅ Profile updated successfully." : "❌ Failed to update profile.");
        }

        // 3️⃣ View all marks
        public void ViewMarks(SqlConnection conn)
        {
            string query = @"SELECT c.CourseName, m.Marks
                             FROM Marks m
                             LEFT JOIN Courses c ON m.CourseId = c.CourseId
                             WHERE m.StudentId = @StudentId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@StudentId", studentId);

            SqlDataReader reader = cmd.ExecuteReader();
            Console.WriteLine("\n---- Marks ----");
            bool found = false;
            while (reader.Read())
            {
                found = true;
                Console.WriteLine($"Course: {reader["CourseName"]}, Marks: {reader["Marks"]}");
            }
            if (!found)
                Console.WriteLine("No marks available yet.");
            reader.Close();
        }

        // 4️⃣ View all courses in department
        public void ViewCourses(SqlConnection conn)
        {
            string query = @"SELECT c.CourseId, c.CourseName, p.FirstName + ' ' + p.LastName AS Professor
                             FROM Courses c
                             LEFT JOIN Students s ON s.DepartmentId = c.DepartmentId
                             LEFT JOIN Professors p ON c.ProfessorId = p.ProfessorId
                             WHERE s.StudentId = @StudentId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@StudentId", studentId);

            SqlDataReader reader = cmd.ExecuteReader();
            Console.WriteLine("\n---- Courses ----");
            bool found = false;
            while (reader.Read())
            {
                found = true;
                Console.WriteLine($"Course ID: {reader["CourseId"]}");
                Console.WriteLine($"Course Name: {reader["CourseName"]}");
                Console.WriteLine($"Professor: {reader["Professor"]}");
                Console.WriteLine("-----------------------");
            }
            if (!found)
                Console.WriteLine("No courses found.");
            reader.Close();
        }

        // 5️⃣ View specific course details
        public void ViewCourseById(SqlConnection conn)
        {
            Console.Write("Enter Course ID: ");
            int courseId = Convert.ToInt32(Console.ReadLine());

            string query = @"SELECT c.CourseId, c.CourseName, d.DepartmentName, 
                                    p.FirstName + ' ' + p.LastName AS Professor
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
                Console.WriteLine($"Professor: {reader["Professor"]}");
            }
            else
            {
                Console.WriteLine("No course found with the given ID.");
            }
            reader.Close();
        }

        // 6️⃣ Change own password (hashed)
        public void ChangePassword(SqlConnection conn)
        {
            Console.Write("Enter new password: ");
            string newPassword = Console.ReadLine();

            string query = @"UPDATE [User]
                             SET Password = HASHBYTES('SHA2_256', @Password)
                             WHERE UserId = @StudentId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Password", newPassword);
            cmd.Parameters.AddWithValue("@StudentId", studentId);

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "✅ Password changed successfully." : "❌ Failed to change password.");
        }

    }
}
