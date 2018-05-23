using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace d2_toolsPro
{
    class MarkovPossibility
    {
        #region 字段

        protected List<int> list1 = new List<int>();
        protected List<int> list2 = new List<int>();
        protected List<int> list3 = new List<int>();
        protected Dictionary<int, double> dicTotal = new Dictionary<int, double>();
        protected Dictionary<string, string> dicPath = new Dictionary<string, string>();
        protected string seqName;
        protected string saveDir;

        public string SeqName
        {
            get
            {
                return seqName;
            }
        }

        #endregion

        public MarkovPossibility(string[] filePath, string savePath)
        {
            string key = null;
            foreach (string item in filePath)
            {
                key = GetKValue(Path.GetFileName(item));
                dicPath.Add(key, item);
            }
            seqName = Path.GetFileName(Path.GetDirectoryName(filePath[0]));
            saveDir = savePath + "\\" + seqName;
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);
        }
        protected void GetDateFromOneFile(int k, List<int> list)
        {
            list.Clear();
            using (StreamReader sr = new StreamReader(dicPath["k" + k], Encoding.ASCII))
            {
                sr.ReadLine();
                char[] flag = { '\t' };
                double total = 0;
                while (true)
                {
                    string tmp = sr.ReadLine();
                    if (tmp == null)
                        break;
                    else
                    {
                        string[] str = tmp.Split(flag, StringSplitOptions.RemoveEmptyEntries);
                        int n = int.Parse(str[1]);
                        list.Add(n);
                        total += n;
                    }
                }//while
                if (!dicTotal.ContainsKey(k))
                    dicTotal.Add(k, total);
            }
        }

        public virtual void GetMarkovPossibility(int k)
        {

        }

        protected List<string> CalMarkovPossibility(int r, int total, List<int> list1, List<int> list2, List<int> list3)
        {
            List<string> listPos = new List<string>();
            if (r == 0)
            {
                List<double> listTmp = new List<double>();
                for (int i = 0; i < 4; i++)
                {
                    listTmp.Add(list2[i] * 1.0 / total);
                }

                List<int> listBits = new List<int>();
                int k = (int)(Math.Log10(list3.Count) / Math.Log10(4));
                for (int i = 0; i < list3.Count; i++)
                {
                    listBits.Clear();
                    double tmp = 1;
                    listBits = Get4Bits(i, k);
                    foreach (var item in listBits)
                    {
                        tmp *= listTmp[item];
                    }
                    listPos.Add(tmp.ToString("e"));
                }
                return listPos;
            }//if
            else
            {
                List<int> listBits = new List<int>();
                for (int i = 0; i < list3.Count; i++)
                {
                    listBits.Clear();
                    int k = (int)(Math.Log10(list3.Count) / Math.Log10(4));
                    if (k <= r)
                    {
                        listPos.Add("NA");
                        continue;
                    }
                    listBits = Get4Bits(i, k);
                    int index = Get10Bits(listBits, 0, r);
                    double tmp = list1[index] * 1.0 / total;
                    for (int j = 0; j < k - r; j++)
                    {
                        int w1 = Get10Bits(listBits, j, r + 1);
                        int w2 = Get10Bits(listBits, j, r) * 4;
                        int nk1 = list2[w1];
                        int nk2 = 0;

                        for (int t = 0; t < 4; t++)
                        {
                            nk2 += list2[w2 + t];
                        }

                        if (nk2 == 0)
                        {
                            tmp = 0;
                            continue;
                        }
                        else
                        {
                            tmp *= nk1 * 1.0 / nk2;
                        }//else
                    }//for
                    listPos.Add(tmp.ToString("e"));
                }//for
                return listPos;
            }//else
        }

        protected void WriteFile(string saveFilePath, string titleLine, List<int> list_kTuple, params List<string>[] listPossibility)
        {
            using (StreamWriter sw = new StreamWriter(saveFilePath, false, Encoding.ASCII))
            {
                sw.WriteLine(titleLine);
                string tmp = null;
                for (int i = 0; i < list_kTuple.Count; i++)
                {
                    if (list_kTuple[i] == 0)
                    {
                        tmp = i + "\t1.000000E-10\t";
                    }
                    else
                    {
                        tmp = i + "\t" + list_kTuple[i] + "\t";
                    }
                    foreach (var item in listPossibility)
                    {
                        tmp += "\t" + item[i];
                    }
                    sw.WriteLine(tmp);
                }
            }//using
        }



        private List<int> Get4Bits(int temp, int k)
        {
            List<int> list = new List<int>();
            int index = 0;
            for (int i = 0; i < k - 1; i++)
            {
                index = temp % 4;
                temp = temp / 4;
                list.Add(index);
            }
            list.Add(temp);
            list.Reverse();
            return list;
        }

        private int Get10Bits(List<int> list, int r, int length)
        {
            int result = 0;
            for (int i = 0; i < length; i++)
            {
                result += (int)(list[i + r] * Math.Pow(4, length - i - 1));
            }
            return result;
        }

        private string GetKValue(string path)
        {
            string key = Path.GetFileNameWithoutExtension(path);
            string[] tmps = key.Split(new string[] { "_k" }, StringSplitOptions.RemoveEmptyEntries);
            string tmp = tmps[tmps.Length - 1].Split('_')[0];
            key = "k" + tmp;
            return key;
        }
    }
}
