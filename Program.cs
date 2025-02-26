using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("请输入两个数字：");
            Console.WriteLine("");

            var a = Console.ReadLine();
            var b = Console.ReadLine();
            Console.WriteLine("请输入运算符：");
            var c = Console.ReadLine();
            if (c == "+")
            { Console.WriteLine(a+ "+"+ b+ "="+ (int.Parse(a) + int.Parse(b))); }
            else if (c == "-"){ 
                Console.WriteLine(a + "-" + b + "=" 
                    + (int.Parse(a) - int.Parse(b))); }
            
               else if (c == "*")
                {
                    Console.WriteLine(a + "*" + b + "="
                        + (int.Parse(a) * int.Parse(b)));
                }

            else if (c == "/")
            {
                Console.WriteLine(a + "/" + b + "="
                    + (int.Parse(a) / int.Parse(b)));
            }
        }
    }
}
