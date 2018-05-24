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
            List<int> list = new List<int>();
            for (int i = 0; i < 10000000; i++)
            {
                list.Add(i);
            }
            List<int> list2 = new List<int>();
            Null(list2);
            Console.WriteLine(list2.Count);
            Console.ReadKey();
        }
        static void Null(List<int> list)
        {
            List<int> list2 = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                list2.Add(i);
            }
            list.AddRange(list2);
        }
    }
}
