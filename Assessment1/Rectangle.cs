using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment1
{
    internal class Rectangle : Class1
    {
        internal double length { get; set; }
        internal double breadth { get; set; }

        public Rectangle(double length, double breadth)
        {
            this.length = length;
            this.breadth = breadth;
        }

        public Rectangle()
        {
            this.length = 0;
            this.breadth = 0;
        }
        public override double getArea()
        {
            return length * breadth;
        }

        public override double getPerimeter()
        {
            return 2 * (length + breadth);
        }

        public void shape() { 
            if(length == breadth)
            {
                Console.WriteLine("I am square");
            }
            else
            {
                Console.WriteLine("I am Rectangel");
            }
        }
        public static double getTotalArea(Rectangle rec1, Rectangle rec2)
        {
            return rec1.getArea() + rec2.getArea(); 
        }

        


        public static Rectangle operator  +(Rectangle rec1, Rectangle rec2)
        {
            //return rec1.getArea() + rec2.getArea();
            return new Rectangle(rec1.getArea() , rec2.getArea());

        }

        public static double operator -(Rectangle rec1, Rectangle rec2)
        {
            return rec1.getArea()*2 - rec2.getArea()*2;
        }

        public static Rectangle newRect(Rectangle rec1, Rectangle rec2)
        {
            //return rec1.getArea() + rec2.getArea();
            Rectangle rec = new Rectangle();
            rec.length = rec1.length + rec2.length;
            rec.breadth = rec2.breadth + rec1.breadth;
            return rec;
        }




    }
}
