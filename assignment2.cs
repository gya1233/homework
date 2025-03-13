using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
namespace ConsoleApp3
{
    class triangle
    {
        public int side1;
        public int side2;
        public int side3;
        public triangle(int s1, int s2, int s3)
        {
            side1 = s1;
            side2 = s2;
            side3 = s3;
        }
        public void area()
        {
            double s = (side1 + side2 + side3) / 2;
            double a = Math.Sqrt(s * (s - side1) * (s - side2) * (s - side3));
            WriteLine("三角形面积为: " + a);
        }
        public void  IsTriangle()
        {
            if (side1 + side2 > side3 && side1 + side3 > side2 && side2 + side3 > side1)
            {
                WriteLine("是三角形");
            }
            else
            {
                WriteLine("不是三角形");
            }
        }
    }
    class rectangle
    {
        public int length;
        public int width;
        public rectangle(int l, int w)
        {
            length = l;
            width = w;
        }
        public void area()
        {
            int a = length * width;
            Console.WriteLine("矩形面积为: " + a);
        }
        public void IsCube()
        {
            if (length == width)
            {
                Console.WriteLine("是正方形");
            }
            else
            {
               Console.WriteLine("是长方形");
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            int a, b, c;
            Console.WriteLine("请输入三角形的三边长: ");
            a = int.Parse(Console.ReadLine());
            Beep();
            b = int.Parse(Console.ReadLine());    
            c = int.Parse(Console.ReadLine());    
            triangle t = new triangle(a,b,c);
            t.area();
            t.IsTriangle();
            int e, d;
            Console.WriteLine("请输入矩形的长和宽: ");
            e = int.Parse(Console.ReadLine());
            d = int.Parse(Console.ReadLine());
            rectangle r = new rectangle(e, d);
            r.area();
            r.IsCube();

        }
    }
}
