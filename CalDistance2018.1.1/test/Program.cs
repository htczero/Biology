using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        //private static object listFilePath;

        static void Main(string[] args)
        {
            int c = 83;
            int[] arr1 = new int[5];
            int[] arr2 = new int[5];
            Get4Bits(c, 5, arr1);
            Get4b(c, 5, arr2);
            foreach (var item in arr1)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("========");
            foreach (var item in arr2)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
        static void Get4Bits(int temp, int k, int[] arr)
        {
            int index = 0;
            for (int i = 0; i < k - 1; i++)
            {
                index = temp % 4;
                temp = temp >> 2;
                arr[k - 1 - i] = index;
            }
            arr[0] = temp;
        }

        static void Get4b(int temp, int k, int[] arr)
        {
            int b = 3;
            for (int i = 0; i < k-1; i++)
            {
                arr[k - 1 - i] = temp & b;
                temp = temp >> 2;
            }
        }

    }
}
