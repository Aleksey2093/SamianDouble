using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestРасстояние
{
    class Program
    {
        public struct Tochka
        {
            public int x;
            public int y;
        }
        public struct Line
        {
            public int x1;
            public int y1;
            public int x2;
            public int y2;
        }
        static void Main(string[] args)
        {
            Tochka toch = new Tochka();
            toch.x = 10; toch.y = 10;
            Line line = new Line();
            line.x1 = 5; line.x2 = 15;
            line.y1 = 15; line.y2 = 15;

            double A = line.y2 - line.y1;
            double B = line.x1 - line.x2;
            double C = (-1) * line.x1 * (line.y2 - line.y1) + line.y1 * (line.x2 - line.x1);
            double T = A * A + B * B; T = Math.Sqrt(T);
            double res = A * toch.x + B * toch.y + C;
            res = res / T;
            Console.WriteLine("res = " + res);

            Console.Read();
        }
    }
}
