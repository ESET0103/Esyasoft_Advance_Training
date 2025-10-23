using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessement2
{
    internal class Students
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DOB { get; set; }
        public string email { get; set; }
        public string PhoneNumber { get; set; }
        public int DepartmentId { get; set; }



        public Students() { }
        public Students(int id)
        {
            this.StudentId = id;
        }
        public Students(int id, string fname, string lname,DateOnly dob, string email, string number, int did)
        {
            this.FirstName = fname; 
            this.LastName = lname;
            this.DepartmentId = id;
            this.email = email;
            this.PhoneNumber = number;
            this.DepartmentId = did;
            this.DOB = dob;
        }

        public int getAge()
        {
            return DOB.Year;
        }
    }
}
