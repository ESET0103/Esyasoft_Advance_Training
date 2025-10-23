using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessement2
{
    internal class Courses
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int DepartmentId { get; set; }
        public int ProfessorId { get; set; }

        public Courses() { }
        public Courses(int id)
        {
            CourseId = id;
        }

        public Courses(int id, string name)
        {
            CourseName = name;
            CourseId = id;
        }


    }
}
