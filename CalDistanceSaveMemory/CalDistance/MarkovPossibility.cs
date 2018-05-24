using System;
using System.Collections.Generic;


namespace CalDistance
{
    class MarkovPossibility
    {
        #region 字段

        //protected List<int> list1 = new List<int>();
        //protected List<int> list2 = new List<int>();
        //protected List<int> list3 = new List<int>();
        private Dictionary<int, int> dicTotal = new Dictionary<int, int>();
        private Dictionary<int, List<int>> dicKtuple = new Dictionary<int, List<int>>();
        //protected string seqName;
        //protected string saveDir;

        //public string SeqName
        //{
        //    get
        //    {
        //        return seqName;
        //    }
        //}

        protected Dictionary<int, int> DicTotal
        {
            get
            {
                return dicTotal;
            }
        }

        protected Dictionary<int, List<int>> DicKtuple
        {
            get
            {
                return dicKtuple;
            }
        }

        #endregion

        public MarkovPossibility(Dictionary<int, List<int>> dicKtuple, Dictionary<int, int> dicTotal)
        {
            this.dicKtuple = dicKtuple;
            this.dicTotal = dicTotal;
            //this.saveDir = saveDir;
        }

        protected List<double> CalMarkovPossibility(int r, int total, List<int> list1, List<int> list2, List<int> list3)
        {
            List<double> listPos = new List<double>();
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
                    listPos.Add(tmp);
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
                        listPos.Add(-1);
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
                    listPos.Add(tmp);
                }//for
                return listPos;
            }//else
        }

        //protected void WriteFile(string saveFilePath, string titleLine, List<int> listKtuple, params List<double>[] listPossibility)
        //{
        //    using (StreamWriter sw = new StreamWriter(saveFilePath, false, Encoding.ASCII))
        //    {
        //        sw.WriteLine(titleLine);
        //        string tmp = null;
        //        for (int i = 0; i < listKtuple.Count; i++)
        //        {
        //            if (listKtuple[i] == 0)
        //            {
        //                tmp = i + "\t1.000000E-10\t";
        //            }
        //            else
        //            {
        //                tmp = i + "\t" + listKtuple[i] + "\t";
        //            }
        //            foreach (var item in listPossibility)
        //            {
        //                tmp += "\t" + item[i].ToString("e");
        //            }
        //            sw.WriteLine(tmp);
        //        }
        //    }//using
        //}



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
                int tmp = 1;
                for (int j = 0; j < length - i - 1; j++)
                {
                    tmp *= 4;
                }
                result += (list[i + r] * tmp);
            }
            return result;
        }
    }
}
