using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessement2
{
    internal class AdminController
    {

        /* 
            addCourses(SqlConnection conn)
            getCoursesDetail(SqlConnection conn)
            getCourseDetailById(SqlConnection conn)

            addDepartment
            
         */






        //  ------------ COURSE RELATED CONTROLLERS ----------------

        public void addCourses(SqlConnection conn)
        {
            Console.Write("Enter Course Id : ");
            int courseId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Course Name : ");
            string courseName = Console.ReadLine();
            Console.Write("Enter Department Id : ");
            int deptId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Professor Id : ");
            int profId = Convert.ToInt32(Console.ReadLine());

            string insertQuery = "INSERT INTO Courses VALUES(@CourseId, @CourseName, @DepartmentId, @ProfessorId)";
            SqlCommand cmd = new SqlCommand(insertQuery, conn);
            cmd.Parameters.AddWithValue("@CourseId", courseId);
            cmd.Parameters.AddWithValue("@CourseName", courseName);
            cmd.Parameters.AddWithValue("@DepartmentId", deptId);
            cmd.Parameters.AddWithValue("@ProfessorId", profId);

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Course added successfully..." : "Failed to add course.");
        }
        public void getCoursesDetail(SqlConnection conn)
        {
            string query = "SELECT * FROM Courses";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader["CourseId"]} - {reader["CourseName"]} | DeptId: {reader["DepartmentId"]} | ProfId: {reader["ProfessorId"]}");
            }
            reader.Close();
        }


        public void updateCourse(SqlConnection conn) { }
        public void deleteCourse(SqlConnection conn) { }
        public void getCourseDetailById(SqlConnection conn)
        {
            Console.Write("Enter Course Id: ");
            int id = Convert.ToInt32(Console.ReadLine());
            string query = "SELECT * FROM Courses WHERE CourseId = @CourseId";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CourseId", id);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine($"Course: {reader["CourseName"]}\nDept ID: {reader["DepartmentId"]}\nProfessor ID: {reader["ProfessorId"]}");
            }
            else
            {
                Console.WriteLine("Course not found!");
            }
            reader.Close();
        }


        // ------------------- DEPARTMENT RELATED CONTROLLER -------------------


        public void addDepartment(SqlConnection conn)
        {
            Console.Write("Enter Department Id : ");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Department Name : ");
            string name = Console.ReadLine();
            Console.Write("Enter HOD Name : ");
            string hod = Console.ReadLine();

            string insertQuery = "INSERT INTO Departments VALUES(@DepartmentId, @DepartmentName, @HOD)";
            SqlCommand cmd = new SqlCommand(insertQuery, conn);
            cmd.Parameters.AddWithValue("@DepartmentId", id);
            cmd.Parameters.AddWithValue("@DepartmentName", name);
            cmd.Parameters.AddWithValue("@HOD", hod);

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Department added successfully..." : "Failed to add department.");
        }

        public void UpdateDepartment(SqlConnection conn)
        {
            Console.Write("Enter Department ID to update: ");
            int deptId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter new Department Name: ");
            string deptName = Console.ReadLine();
            Console.Write("Enter new HOD Name: ");
            string hod = Console.ReadLine();

            string query = @"UPDATE Departments 
                             SET DepartmentName = @DeptName, HOD = @HOD
                             WHERE DepartmentId = @DeptId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DeptId", deptId);
            cmd.Parameters.AddWithValue("@DeptName", deptName);
            cmd.Parameters.AddWithValue("@HOD", hod);

            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
                Console.WriteLine("Department updated successfully.");
            else
                Console.WriteLine("No department found with the given ID.");
        }


        public void DeleteDepartment(SqlConnection conn)
        {
            Console.Write("Enter Department ID to delete: ");
            int deptId = Convert.ToInt32(Console.ReadLine());

            string query = "DELETE FROM Departments WHERE DepartmentId = @DeptId";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DeptId", deptId);

            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
                Console.WriteLine("Department deleted successfully.");
            else
                Console.WriteLine("No department found with the given ID.");
        }

        // ------------------- PROFESSOR RELATED CONTROLLER ---------------------


        public void addProfessor(SqlConnection conn)
        {
            Professors professor = new Professors();
            Console.Write("Enter Professor Id : ");
            professor.ProfessorId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter First Name : ");
            professor.FirstName = Console.ReadLine();
            Console.Write("Enter Last Name : ");
            professor.LastName = Console.ReadLine();
            Console.Write("Enter Email : ");
            professor.email = Console.ReadLine();
            Console.Write("Enter Phone Number : ");
            professor.PhoneNumber = Console.ReadLine();
            Console.Write("Enter Department Id : ");
            professor.DepartmentId = Convert.ToInt32(Console.ReadLine());

            string insertQuery = "INSERT INTO Professors VALUES(@ProfessorId, @FirstName, @LastName, @Email, @PhoneNumber, @DepartmentId)";
            SqlCommand cmd = new SqlCommand(insertQuery, conn);
            cmd.Parameters.AddWithValue("@ProfessorId", professor.ProfessorId);
            cmd.Parameters.AddWithValue("@FirstName", professor.FirstName);
            cmd.Parameters.AddWithValue("@LastName", professor.LastName);
            cmd.Parameters.AddWithValue("@Email", professor.email);
            cmd.Parameters.AddWithValue("@PhoneNumber", professor.PhoneNumber);
            cmd.Parameters.AddWithValue("@DepartmentId", professor.DepartmentId);

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Professor added successfully..." : "Failed to add professor.");
        }

        public void getProfessorsDetails(SqlConnection conn)
        {
            string query = "SELECT * FROM Professors";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader["ProfessorId"]} - {reader["FirstName"]} {reader["LastName"]} | DeptId: {reader["DepartmentId"]}");
            }
            reader.Close();
        }

        public void getProfessorDetailById(SqlConnection conn)
        {
            Console.Write("Enter Professor Id: ");
            int id = Convert.ToInt32(Console.ReadLine());
            string query = "SELECT * FROM Professors WHERE ProfessorId = @ProfessorId";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ProfessorId", id);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine($"Name: {reader["FirstName"]} {reader["LastName"]}\nEmail: {reader["Email"]}\nPhone: {reader["PhoneNumber"]}\nDept ID: {reader["DepartmentId"]}");
            }
            else
            {
                Console.WriteLine("Professor not found!");
            }
            reader.Close();
        }


        public void UpdateProfessor(SqlConnection conn)
        {
            Console.Write("Enter Professor ID to update: ");
            int profId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter new Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter new Phone Number: ");
            string phone = Console.ReadLine();

            string query = @"UPDATE Professors 
                             SET Email = @Email, PhoneNumber = @Phone 
                             WHERE ProfessorId = @ProfId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ProfId", profId);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Phone", phone);

            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
                Console.WriteLine("Professor updated successfully.");
            else
                Console.WriteLine("No professor found with the given ID.");
        }

        public void DeleteProfessor(SqlConnection conn)
        {
            Console.Write("Enter Professor ID to delete: ");
            int profId = Convert.ToInt32(Console.ReadLine());

            string query = "DELETE FROM Professors WHERE ProfessorId = @ProfId";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ProfId", profId);

            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
                Console.WriteLine("Professor deleted successfully.");
            else
                Console.WriteLine("No professor found with the given ID.");
        }


        // ------------------ STUDENT RELATED CONTROLLER ------------------------


        public void addStudent(SqlConnection conn)
        {
            Students student = new Students();
            Console.Write("Enter Student Id : ");
            student.StudentId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Student First Name : ");
            student.FirstName = Console.ReadLine();
            Console.Write("Enter Student Last Name : ");
            student.LastName = Console.ReadLine();
            Console.Write("Enter Student Date of Birth : ");
            string date = Console.ReadLine();
            if (DateOnly.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateOnly db))
            {
                student.DOB = db;
            }
            else
            {
                Console.WriteLine("Invalid date format! Please enter in yyyy-MM-dd format.");
            }
            Console.Write("Enter Student email : ");
            student.email = Console.ReadLine();
            Console.Write("Enter Student Phone Number : ");
            student.PhoneNumber = Console.ReadLine();
            Console.Write("Enter Student Department Id : ");
            student.DepartmentId = Convert.ToInt32(Console.ReadLine());


            int studentId = student.StudentId;
            string firstname = student.FirstName;
            string lastname = student.LastName;
            DateOnly dob = student.DOB;
            string email = student.email;
            string phonenumber = student.PhoneNumber;
            int departmentid = student.DepartmentId;

            string insertQuery = "insert into Students values(@StudentId,@FirstName, @LastName, @DOB,@email, @PhoneNumber, @DepartmentId)";
            SqlCommand cmd = new SqlCommand(insertQuery, conn);
            cmd.Parameters.AddWithValue("@StudentId", studentId);
            cmd.Parameters.AddWithValue("@FirstName", firstname);
            cmd.Parameters.AddWithValue("@LastName", lastname);
            cmd.Parameters.AddWithValue("@DOB", dob);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@PhoneNumber", phonenumber);
            cmd.Parameters.AddWithValue("@DepartmentId", departmentid);

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Professor added successfully..." : "Failed to add professor.");
        }

        
        public void getStudentsDetails(SqlConnection conn)
        {
            string query = "SELECT * FROM Students";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader["StudentId"]} - {reader["FirstName"]} {reader["LastName"]} | DeptId: {reader["DepartmentId"]}");
            }
            reader.Close();
        }

        public void getStudentDetailsById(SqlConnection conn)
        {
            Console.Write("Enter Student Id: ");
            int id = Convert.ToInt32(Console.ReadLine());
            string query = "SELECT * FROM Students WHERE StudentId = @StudentId";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@StudentId", id);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine($"Name: {reader["FirstName"]} {reader["LastName"]}\nEmail: {reader["Email"]}\nPhone: {reader["PhoneNumber"]}\nDept ID: {reader["DepartmentId"]}");
            }
            else
            {
                Console.WriteLine("Student not found!");
            }
            reader.Close();
        }
    }
}
