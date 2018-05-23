using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tset
{
    class Ktuple1
    {
        #region 私有字段及其属性
        private List<string> listKtuple = new List<string>();
        private string seqText;
        private string saveDir;
        private string seqName;

        public string SeqName
        {
            get
            {
                return seqName;
            }
        }

        public string SaveDir
        {
            get
            {
                return saveDir;
            }
        }

        #endregion

        #region 方法
        private void GetKtupleList(int k)
        {
            listKtuple.Clear();
            StringBuilder sb = new StringBuilder();
            int length = (int)(Math.Pow(4, k));
            int index = 0;
            int temp = 0;
            string[] element = { "A", "G", "C", "T" };
            for (int i = 0; i < length; i++)
            {

                temp = i;
                for (int j = 0; j < k - 1; j++)
                {
                    index = temp % 4;
                    temp = temp / 4;
                    sb.Append(element[index]);
                }
                sb.Append(element[temp]);
                listKtuple.Add(sb.ToString());
                sb.Clear();
            }
        }

        public void GetKtupleCount(int k, bool bOverlap)
        {
            GetKtupleList(k);
            int n = k;
            if (bOverlap)
                n = 1;
            GetTupleCount(k, n);
            //listKtuple.Clear();
        }

        private void GetTupleCount(int k, int n)
        {
            string path = SaveDir + "\\" + SeqName + "_k" + k + "_tupleCount.txt";
            StringBuilder sb = new StringBuilder();
            double total = 0;
            foreach (var tuple in listKtuple)
            {
                int r = 0;
                int num = 0;
                while (true)
                {
                    r = seqText.IndexOf(tuple, r);
                    if (r == -1 || r > seqText.Length)
                        break;
                    else
                    {
                        num++;
                        r += n;
                    }
                }//while
                sb.Append(tuple + "\t" + num + "\r\n");
                if (num == 0)
                    total += 1e-10;
                else
                    total += num;
            }//foreach     
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.ASCII))
            {
                sw.WriteLine("k-tuple\tnumber_of_occurrences");
                sw.WriteLine("Total\t" + total.ToString("e"));
                sw.Write(sb.ToString());
            }
        }

        private void GetSeqText(string filePath)
        {
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(filePath, Encoding.ASCII))
            {
               // sr.ReadLine();
                while (true)
                {
                    string tmp = sr.ReadLine();
                    if (tmp == null)
                        break;
                    sb.Append(tmp);
                }
            }
            seqText = sb.ToString();
        }
        #endregion

        public Ktuple1(string filePath, string savePath)
        {
            seqName = Path.GetFileNameWithoutExtension(filePath);
            saveDir = savePath + "\\Ktuple1\\" + SeqName;
            Directory.CreateDirectory(SaveDir);
            GetSeqText(filePath);
        }
    }
}
