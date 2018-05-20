using System;
using System.Collections.Concurrent;
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
        static void Main(string[] args)
        {
            Stopwatch sp1 = new Stopwatch();
            Stopwatch sp2 = new Stopwatch();
            ConcurrentDictionary<int, int> dic2 = new ConcurrentDictionary<int, int>();
            //string tmp = "00.txt";
            //string tmp1 = "11.txt";
            //Action act = new Action(() =>
            //  {
            //      Dictionary<string, string> dic = new Dictionary<string, string>();
            //      dic = Gun(tmp);                 
            //  });
            //Action act1 = new Action(() =>
            //  {
            //      sp1.Start();
            //      Dictionary<string, string> dic = new Dictionary<string, string>();
            //      dic = Gun(tmp1);
            //      dic = Gun(tmp1);
            //      sp1.Stop();
            //      Console.WriteLine("sp1:"+sp1.ElapsedMilliseconds);
            //  });
            //Action act2 = new Action(() =>
            //  {
            //      sp2.Start();
            //      Parallel.Invoke(act,act,act,act,act);
            //      sp2.Stop();
            //      Console.WriteLine("sp2:"+sp2.ElapsedMilliseconds);
            //  });
            //Task task = new Task(() =>
            //  {
            //      Parallel.Invoke(act1, act2);
            //  });
            //task.Start();
            Dictionary<int, int> dic = new Dictionary<int, int>();
            int n = 10;
            for (int i = 0; i < n; i++)
            {
                dic.Add(i, i);
                dic2.TryAdd(i, i);
            }
            Action act1 = new Action(() =>
              {
                  sp1.Start();
                  foreach (KeyValuePair<int, int> item in dic)
                  {
                      F(item.Value);
                  }
                  sp1.Stop();
              });
            Action act2 = new Action(() =>
              {
                  sp2.Start();
                  Parallel.ForEach(dic2, (item) =>
                  {
                      F(item.Value);
                  });

              });
            Task task = new Task(() =>
              {
                  Parallel.Invoke(act1, act2);
              });
            task.Start();
            Task.WaitAll(task);
            Console.WriteLine("sp1" + sp1.ElapsedMilliseconds);
            Console.WriteLine("sp2" + sp2.ElapsedMilliseconds);
            Console.ReadKey();
        }

        private static Dictionary<string, string> Gun(string path)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            using (StreamReader sr = new StreamReader(path))
            {
                sr.ReadLine();
                string tmp;
                char[] flag = { '\t' };
                while (true)
                {
                    tmp = sr.ReadLine();
                    if (tmp == null)
                        break;
                    string[] tmps = tmp.Split(flag, StringSplitOptions.RemoveEmptyEntries);
                    dic.Add(tmps[0], tmps[1]);
                }
            }
            return dic;
        }
        private static void F(int i)
        {
            int a = i;
            a++;
        }

    }
}
