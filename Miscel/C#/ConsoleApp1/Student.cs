using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Student : Person
    {
        public static string SchoolName = "";
        string sname { get; set; }
        public static int roll;
        int[] arr = new int[3];
        //public string gender;

        //public void setGender(string gender)
        //{
        //    this.gender = gender;
        //    Console.WriteLine($"Gender is {gender}");
        //}
        
        //public void work()
        //{
        //    Console.WriteLine("I work as Student");
        //}
        public Student(string sname):base(sname)
        {
            this.sname = sname;
            Console.WriteLine($"I work as Student {sname}");
            //SchoolName = "NITJSR";
        }
        //public Student() {
        //    //roll += 1;
        //    //SchoolName = "NITJSR";
        //}


        public void whoAmI( string sname) {
            this.sname = sname;
            //this.pname = sname;
            //this.hname = sname;
            Console.WriteLine($"I am a student {sname}");
        }

        public string getSchool()
        {
            return SchoolName;
        }
        public void subjectScores()
        {
            Console.WriteLine("Enter the marks of each Subject: ");
            for(int i = 0; i < arr.Length; i++)
            {
                arr[i] = Convert.ToInt32(Console.ReadLine());
            }
        }

        public int getSumMarks()
        {

            int sum = 0;
            for(int i=0;i<arr.Length; i++)
            {
                sum += arr[i];
            }

            return sum;
        }

        public int getAvg(int sum)
        {
            return sum / arr.Length;
        }

        public void eat()
        {
            Console.WriteLine("I eat eggs in brkfst");
        }

    }
}
