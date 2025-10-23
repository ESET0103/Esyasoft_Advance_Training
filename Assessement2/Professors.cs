using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Assessement2
{
    internal class Professors
    {
        public int ProfessorId {  get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string email { get; set; }
        public string PhoneNumber { get; set; }
        public int DepartmentId { get; set; }

    }
}
