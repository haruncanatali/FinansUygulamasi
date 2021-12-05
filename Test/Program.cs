using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(DateTime.UtcNow.ToString());

            var value = 12345.50m;
            Console.WriteLine(value.ToString("#,##0.00"));

            Console.Read();
        }
    }
}
