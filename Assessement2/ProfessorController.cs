using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessement2
{
    class ProfessorController
    {
        public void GetAllProfessors(SqlConnection conn)
        {
            string query = @"SELECT p.ProfessorId, p.FirstName, p.LastName, p.Email, 
                                    d.DepartmentName 
                             FROM Professors p
                             LEFT JOIN Departments d ON p.DepartmentId = d.DepartmentId";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine("\n---- All Professors ----");
            while (reader.Read())
            {
                Console.WriteLine($"Professor ID: {reader["ProfessorId"]}");
                Console.WriteLine($"Name: {reader["FirstName"]} {reader["LastName"]}");
                Console.WriteLine($"Email: {reader["Email"]}");
                Console.WriteLine($"Department: {reader["DepartmentName"]}");
                Console.WriteLine("---------------------------");
            }
            reader.Close();
        }

        //  Get professor by ID
        public void GetProfessorById(SqlConnection conn)
        {
            Console.Write("Enter Professor ID: ");
            int profId = Convert.ToInt32(Console.ReadLine());

            string query = @"SELECT p.ProfessorId, p.FirstName, p.LastName, p.Email, 
                                    p.PhoneNumber, d.DepartmentName
                             FROM Professors p
                             LEFT JOIN Departments d ON p.DepartmentId = d.DepartmentId
                             WHERE p.ProfessorId = @ProfId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ProfId", profId);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine("\n---- Professor Details ----");
                Console.WriteLine($"Professor ID: {reader["ProfessorId"]}");
                Console.WriteLine($"Name: {reader["FirstName"]} {reader["LastName"]}");
                Console.WriteLine($"Email: {reader["Email"]}");
                Console.WriteLine($"Phone: {reader["PhoneNumber"]}");
                Console.WriteLine($"Department: {reader["DepartmentName"]}");
            }
            else
            {
                Console.WriteLine("No professor found with the given ID.");
            }
            reader.Close();
        }

        // 3️⃣ Get professors by department
        public void GetProfessorsByDepartment(SqlConnection conn)
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

        // 4️⃣ Add new professor (Admin only)
        public void AddProfessor(SqlConnection conn)
        {
            Console.Write("Enter Professor ID: ");
            int profId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter First Name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter Last Name: ");
            string lastName = Console.ReadLine();
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Phone Number: ");
            string phone = Console.ReadLine();
            Console.Write("Enter Department ID: ");
            int deptId = Convert.ToInt32(Console.ReadLine());

            string query = "INSERT INTO Professors VALUES (@ProfId, @FirstName, @LastName, @Email, @Phone, @DeptId)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ProfId", profId);
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@DeptId", deptId);

            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
                Console.WriteLine("Professor added successfully.");
            else
                Console.WriteLine("Failed to add professor.");
        }

       

        // 7️⃣ Get all courses taught by a specific professor
        public void GetCoursesTaughtByProfessor(SqlConnection conn)
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

            Console.WriteLine($"\n---- Courses taught by Professor ID {profId} ----");
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
                Console.WriteLine("This professor is not assigned to any course.");
            reader.Close();
        }
        public void changeUserName(SqlConnection conn, int professorId)
        {
            Console.Write("Enter new First Name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter new Last Name: ");
            string lastName = Console.ReadLine();

            string query = @"UPDATE Professors
                             SET FirstName = @FirstName, LastName = @LastName
                             WHERE ProfessorId = @ProfId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            cmd.Parameters.AddWithValue("@ProfId", professorId);

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "✅ Name updated successfully." : "❌ Failed to update name.");
        }

        // Change professor's password (hashed)
        public void changePassword(SqlConnection conn, int professorId)
        {
            Console.Write("Enter new password: ");
            string newPassword = Console.ReadLine();

            // Hash the password using HASHBYTES in SQL Server
            string query = @"UPDATE [User]
                             SET Password = HASHBYTES('SHA2_256', @Password)
                             WHERE UserId = @ProfId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Password", newPassword);
            cmd.Parameters.AddWithValue("@ProfId", professorId);

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "✅ Password changed successfully." : "❌ Failed to change password.");
        }

        // Update student marks
        public void updateStudentMarks(SqlConnection conn)
        {
            Console.Write("Enter Student ID: ");
            int studentId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Course ID: ");
            int courseId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Marks: ");
            int marks = Convert.ToInt32(Console.ReadLine());

            // Check if marks already exist for student & course
            string checkQuery = @"SELECT COUNT(*) FROM Marks 
                                  WHERE StudentId = @StudentId AND CourseId = @CourseId";
            SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
            checkCmd.Parameters.AddWithValue("@StudentId", studentId);
            checkCmd.Parameters.AddWithValue("@CourseId", courseId);
            int count = (int)checkCmd.ExecuteScalar();

            string query;
            if (count > 0)
            {
                // Update existing marks
                query = @"UPDATE Marks SET Marks = @Marks 
                          WHERE StudentId = @StudentId AND CourseId = @CourseId";
            }
            else
            {
                // Insert new marks
                query = @"INSERT INTO Marks (StudentId, CourseId, Marks) 
                          VALUES (@StudentId, @CourseId, @Marks)";
            }

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@StudentId", studentId);
            cmd.Parameters.AddWithValue("@CourseId", courseId);
            cmd.Parameters.AddWithValue("@Marks", marks);

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "✅ Marks updated successfully." : "❌ Failed to update marks.");
        }

        // Delete student marks
        public void deleteStudentMarks(SqlConnection conn)
        {
            Console.Write("Enter Student ID: ");
            int studentId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Course ID: ");
            int courseId = Convert.ToInt32(Console.ReadLine());

            string query = @"DELETE FROM Marks 
                             WHERE StudentId = @StudentId AND CourseId = @CourseId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@StudentId", studentId);
            cmd.Parameters.AddWithValue("@CourseId", courseId);

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "✅ Marks deleted successfully." : "❌ No marks found to delete.");
        }

        public void viewProfile(SqlConnection conn) { }

    }
}
