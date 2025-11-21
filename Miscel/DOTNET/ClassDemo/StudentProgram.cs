namespace ClassDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Student student = new Student(1, "Sahil khan", 26, "987654315", "sahilkhan@gmail.com");
            //student.set(1, "Sahil khan",26,"987654315","sahilkhan@gmail.com");

            student.showDetails();
        }
    }
}
