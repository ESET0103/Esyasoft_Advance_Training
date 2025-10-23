namespace Assessment1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");
            Rectangle rec1 = new Rectangle(8.0, 3.3);
            rec1.shape();
            Rectangle rec2 = new Rectangle(6, 5.2);



            double area_of_rec1 = rec1.getArea();
            double area_of_rec2 = rec2.getArea();
            double totalArea = Rectangle.getTotalArea(rec1, rec2);

            //Rectangle rec = Rectangle.newRect(rec1, rec2);
            //Rectangle rec = rec1 + rec2;
            double rec = rec1 - rec2;

            //Console.WriteLine($"Total of Area of Two rectangles is {rec.getArea()}");
            Console.WriteLine($"Total of Area of Two rectangles is {rec}");
            //Console.WriteLine($" length and breadth of new rectangle is {rec.length} x {rec.breadth}");
                
        }
    } 
}
