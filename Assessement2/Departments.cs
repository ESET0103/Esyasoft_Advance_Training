using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessement2
{
    internal class Departments
    {
        public int DepartmentId {  get; set; }
        public string DepartmentName { get; set; }
        public string HOD { get; set; }

        public Departments() { }
        public Departments(int departmentId, string departmentName, string hOD)
        {
            DepartmentId = departmentId;
            DepartmentName = departmentName;
            HOD = hOD;
        }

        public void AddProfessor(Professors professor)
        {
            Console.WriteLine($"{professor.FirstName} {professor.LastName} added to {DepartmentName} department.");
        }

        public void ShowCourses(List<Courses> courses)
        {
            Console.WriteLine($"Courses in {DepartmentName}:");
            foreach (var c in courses.Where(c => c.DepartmentId == DepartmentId))
            {
                Console.WriteLine(c.CourseName);
            }
        }


    }
}
