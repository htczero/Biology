using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            List<int> list = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                dic.Add(i, i);
            }
            for (int i = 0; i < 10; i++)
            {
                list.Add(i);
            }
            foreach (var item in list)
            {
                Console.WriteLine(item) ;
            }
            Console.ReadKey();
        }
    }
}
