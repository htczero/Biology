using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tset
{
    class Ktuple2
    {
        #region 私有字段及其属性
        private Dictionary<string, int> dicKtuple = new Dictionary<string, int>();
        private string saveDir;
        private string seqName;
        private string filePath;

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
        private void GetKtupleDic(int k)
        {
            dicKtuple.Clear();
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
                dicKtuple.Add(sb.ToString(), 0);
                sb.Clear();
            }
        }

        public void GetTupleCount(int k, bool bOverlap)
        {
            GetKtupleDic(k);
            string path = SaveDir + "\\" + SeqName + "_k" + k + "_tupleCount.txt";

            using (StreamReader sr = new StreamReader(filePath, Encoding.ASCII))
            {
                sr.ReadLine();
                int i = 1;
                while (true)
                {
                    i++;
                    string tmp = sr.ReadLine();
                    if (tmp == null)
                        break;

                    string tmpRev = GetRev(tmp);
                    if (i % 2 == 0)
                    {
                        for (int j = 0; j < tmp.Length - k + 1; j++)
                        {
                            string key = tmp.Substring(j, k);
                            dicKtuple[key]++;
                            key = tmpRev.Substring(j, k);
                            dicKtuple[key]++;
                        }
                    }
                }
            }
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.ASCII))
            {
                string titleLine = "ktuple\tNumber of ktuple";
                sw.WriteLine(titleLine);
                foreach (KeyValuePair<string, int> item in dicKtuple)
                {
                    sw.WriteLine(item.Key + "\t" + item.Value);
                }
            }
        }
        private string GetRev(string tmp)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = tmp.Length - 1; i > -1; i--)
            {
                if (tmp[i] == 'A')
                    sb.Append("T");

                else if (tmp[i] == 'T')
                    sb.Append("A");

                else if (tmp[i] == 'C')
                    sb.Append("G");

                else if (tmp[i] == 'G')
                    sb.Append("C");
            }
            return sb.ToString();
        }

        #endregion

        public Ktuple2(string filePath, string savePath)
        {
            seqName = Path.GetFileNameWithoutExtension(filePath);
            saveDir = savePath + "\\Ktuple2\\" + SeqName;
            Directory.CreateDirectory(SaveDir);
            this.filePath = filePath;
        }
    }
}
