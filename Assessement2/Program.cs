using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Assessement2
{
    internal class Program
    {
        static void Main(string[] args)
        {
           
            Console.WriteLine("Connecting to Database...");

            DBConnection database = new DBConnection();
            SqlConnection conn = database.connectDB();
            Professors profesor = new Professors();
            Courses course = new Courses();
            Departments departments = new Departments();
            Students student = new Students(2,"Kaushik","Mano", DateOnly.Parse("2004-04-1"), "nknidf@nitjsr.ac.in", "919369585823",101);
            //StudentController admin = new StudentController();
            AdminController adminController = new AdminController();
            DepartmentController departmentController = new DepartmentController(); 
            CourseController courseController = new CourseController(); 
            ProfessorController professorController = new ProfessorController();
            
            try
            {
                conn.Open();
                Console.WriteLine("connection established.");
                //now called the function for inserting data or fetching data

                // first login into your app
                // username = mantukr@gmail.com
                LoginController loginController = new LoginController();
                //Console.WriteLine
                //string HashPassword(string password)
                //{
                //    using (SHA256 sha256 = SHA256.Create())
                //    {
                //        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                //        StringBuilder sb = new StringBuilder();
                //        foreach (byte b in bytes)
                //        {
                //            sb.Append(b.ToString("x2")); // convert to hex string
                //        }
                //        return sb.ToString();
                //    }
                //}

                //string ssp = HashPassword("123654");
                //string ttp = "6460662e217c7a9f899208dd70a2c28abdea42f128666a9b78e6c0c064846493";
                //if(ssp == ttp)
                //{
                //    Console.WriteLine("pass matched...");

                //}
                //return ;
                int role = loginController.login(conn);
                if (role > 0)
                {
                    //bool exitApp = false;
                    Console.WriteLine($"role : {role}");

                    while (true)
                    {
                        switch (role)
                        {
                            case 1: // Admin
                                Console.WriteLine("\n---- Admin Section ----");
                                Console.WriteLine("1. Student Management");
                                Console.WriteLine("2. Professor Management");
                                Console.WriteLine("3. Department Management");
                                Console.WriteLine("4. Course Management");
                                Console.WriteLine("5. Exit");
                                int category = Convert.ToInt32(Console.ReadLine());

                                switch (category)
                                {
                                    case 1: // Student Management
                                        Console.WriteLine("1. Add Student");
                                        Console.WriteLine("2. Delete Student");
                                        Console.WriteLine("3. View All Students");
                                        int studentChoice = Convert.ToInt32(Console.ReadLine());
                                        switch (studentChoice)
                                        {
                                            case 1:
                                                adminController.addStudent(conn);
                                                break;
                                            case 2:
                                                // Implement deleteStudent
                                                break;
                                            case 3:
                                                adminController.getStudentsDetails(conn);
                                                break;
                                        }
                                        break;

                                    case 2: // Professor Management
                                        Console.WriteLine("1. Add Professor");
                                        Console.WriteLine("2. Update Professor");
                                        Console.WriteLine("3. Delete Professor");
                                        Console.WriteLine("4. View All Professors");
                                        int profChoice = Convert.ToInt32(Console.ReadLine());
                                        switch (profChoice)
                                        {
                                            case 1:
                                                adminController.addProfessor(conn);
                                                break;
                                            case 2:
                                                // implement updateProfessor
                                                break;
                                            case 3:
                                                // implement deleteProfessor
                                                break;
                                            case 4:
                                                adminController.getProfessorsDetails(conn);
                                                break;
                                        }
                                        break;

                                    case 3: // Department Management
                                        Console.WriteLine("1. Add Department");
                                        Console.WriteLine("2. Update Department");
                                        Console.WriteLine("3. Delete Department");
                                        Console.WriteLine("4. View Departments");
                                        int deptChoice = Convert.ToInt32(Console.ReadLine());
                                        switch (deptChoice)
                                        {
                                            case 1:
                                                adminController.addDepartment(conn);
                                                break;
                                            case 2:
                                                adminController.UpdateDepartment(conn);
                                                break;
                                            case 3:
                                                adminController.DeleteDepartment(conn);
                                                break;
                                            case 4:
                                                departmentController.GetAllDepartments(conn);
                                                break;
                                        }
                                        break;

                                    case 4: // Course Management
                                        Console.WriteLine("1. Add Course");
                                        Console.WriteLine("2. Update Course");
                                        Console.WriteLine("3. Delete Course");
                                        Console.WriteLine("4. View All Courses");
                                        int courseChoice = Convert.ToInt32(Console.ReadLine());
                                        switch (courseChoice)
                                        {
                                            case 1:
                                                adminController.addCourses(conn);
                                                break;
                                            case 2:
                                                adminController.updateCourse(conn);
                                                break;
                                            case 3:
                                                adminController.deleteCourse(conn);
                                                break;
                                            case 4:
                                                courseController.GetAllCourses(conn);
                                                break;
                                        }
                                        break;

                                    case 5: // Exit
                                        //exitApp = true;
                                        break;
                                }
                                break;

                            case 2: // Professor
                                Console.WriteLine("\n---- Professor Section ----");
                                Console.WriteLine("1. View Profile");
                                Console.WriteLine("2. Update Name");
                                Console.WriteLine("3. Change Password");
                                Console.WriteLine("4. Update Student Marks");
                                Console.WriteLine("5. Delete Student Marks");
                                Console.WriteLine("6. View Courses Teaching");
                                Console.WriteLine("7. Exit");
                                int profMenu = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter Your ID : ");
                                int pid = Convert.ToInt32(Console.ReadLine());
                                switch (profMenu)
                                {
                                    case 1:
                                        // implement viewProfile for professor
                                        professorController.viewProfile(conn);
                                        break;
                                    case 2:
                                        //int id = Convert.ToInt32(Console.ReadLine());
                                        professorController.changeUserName(conn,pid);
                                        break;
                                    case 3:
                                        //int id = Convert.ToInt32(Console.ReadLine());
                                        professorController.changePassword(conn,pid);
                                        break;
                                    case 4:
                                        professorController.updateStudentMarks(conn);
                                        break;
                                    case 5:
                                        professorController.deleteStudentMarks(conn);
                                        break;
                                    case 6:
                                        professorController.GetCoursesTaughtByProfessor(conn);
                                        break;
                                    case 7:
                                        //exitApp = true;
                                        break;
                                }
                                break;

                            case 3: // Student
                                Console.WriteLine("\n---- Student Section ----");
                                Console.WriteLine("1. View Profile");
                                Console.WriteLine("2. Update Profile");
                                Console.WriteLine("3. Change Password");
                                Console.WriteLine("4. View Courses");
                                Console.WriteLine("5. View Marks");
                                Console.WriteLine("6. Exit");
                                int studentMenu = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter Your ID : ");
                                int sid = Convert.ToInt32(Console.ReadLine());
                                StudentController studentController = new StudentController(sid);
                                switch (studentMenu)
                                {
                                    case 1:
                                        studentController.ViewProfile(conn);
                                        break;
                                    case 2:
                                        studentController.UpdateProfile(conn);
                                        break;
                                    case 3:
                                        studentController.ChangePassword(conn);
                                        break;
                                    case 4:
                                        studentController.ViewCourses(conn);
                                        break;
                                    case 5:
                                        studentController.ViewMarks(conn);
                                        break;
                                    case 6:
                                        //exitApp = true;
                                        break;
                                }
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid credentials. If you are a new user, press 1 to register:");
                    int x = Convert.ToInt32(Console.ReadLine());
                    if (x == 1)
                    {
                        RegistrationController registrationController = new RegistrationController();
                        registrationController.Register(conn);
                    }
                }              
                
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.ToString());
            }

            
        }
    }
}
