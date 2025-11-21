using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassDemo
{
    internal class Student
    {
        int student_id;
        string name;
        int age;
        string contact_no;
        string email;

        public Student(int student_id, string name, int age, string contact_no, string email)
        {
            this.student_id = student_id;
            this.name = name;
            this.age = age;
            this.contact_no = contact_no;
            this.email = email;
        }

        //public void set(int id, string name, int age ,string contact_no, string email)
        //{
        //    this.student_id = id;
        //    this.name = name;   
        //    this.age = age; 
        //    this.contact_no = contact_no;
        //    this.email = email;

        //}

        public int total_marks() { return 0; }
        public int resultStatus(int status) { return 0; }


        public void showDetails()
        {
            Console.WriteLine($"Student Id : {student_id}");
            Console.WriteLine($"Name : {name}");
            Console.WriteLine($"Age : {age}");
            Console.WriteLine($"Email : {email}");
            Console.WriteLine($"Contact Number : {contact_no}");
            //Console.WriteLine("Total Marks: ");
        }
    }
}
