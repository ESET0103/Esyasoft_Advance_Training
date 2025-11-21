namespace ConsoleApp1
{
    internal class Program
    {
        //static string nameStudent() {

        //    Console.Write("Enter Name : ");
        //    string name = Console.ReadLine();
        //    return name;
            
        //}

        //static int[] takeStudentDetails()
        //{
        //    int[] arr = new int[3];
        //    Console.WriteLine("Enter the marks of 3 subjects");
        //    for (int i = 0; i < 3; i++)
        //    {
        //        arr[i] = Convert.ToInt32(Console.ReadLine());
        //    }
        //    return arr;
        //}

        //static int getSumOfMarks(int[] arr)
        //{
        //    int sum = 0;
        //    for(int i = 0; i < arr.Length; i++)
        //    {
        //        sum += arr[i];
        //    }
        //    return sum;
        //}


        //static int getAvg(int[] arr)
        //{
        //    //Console.WriteLine($"lnsdivs {sum}");
        //    int avg = 0;
        //    avg = getSumOfMarks(arr) / arr.Length;
        //    return avg;

        //}
        static void Main(string[] args)
        {
            //Console.Write("Enter Name : ");
            //string name = Console.ReadLine();

            //string in = Console.ReadLine(); // Read input as a string
            //int number = Convert.ToInt32(inputString);

            //int[] arr = new int[3] { 95,63,89};
            //int sum = 0;
            //for(int i = 0; i < 3; i++)
            //{
            //string x = Console.ReadLine();
            //arr[i] = Convert.ToInt32(Console.ReadLine());
            //sum += arr[i];

            //}
            //string name = nameStudent();
            //int[] arr = takeStudentDetails();
            //int sum = getSumOfMarks(arr);
            //Console.WriteLine(sum);
            //int avg = getAvg(arr);
            //Console.WriteLine($"Name : {name}"); 
            //Console.WriteLine($"Avg of his marks : {avg}");

            Student student1 = new Student("Mantu");
            //string school = Student.SchoolName;
            //student1.subjectScores();
            //int sum = student1.getSumMarks();
            //int avg = student1.getAvg(sum);
            //Console.WriteLine($"Name : {student1.name}");
            //Console.WriteLine($"Total Marks : {sum}");
            //Console.WriteLine($"School Name : {Student.SchoolName}");
            //student1.gender = "Male";
            //Console.WriteLine($"gender is : {student1.gender}");
            //student1.work();

            //student1.whoAmI("John");
            student1.eat();


            
        }
    }
}
