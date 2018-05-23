using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tset
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> list = new List<string>();
            string[] tmps = { "12", "123", "123" };
            list.AddRange(tmps);
            tmps = list.ToArray();
            list.Clear();


            Console.WriteLine(tmps[0]);
            Console.ReadKey();
        }

        static void g(List<string> list)
        {
            list.Add("0");
        }
    }
}
