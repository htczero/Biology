using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace d2_tools
{
    class MarkovPossibility
    {
        /// <summary>
        /// 获取一个文件的数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="k"></param>
        /// <param name="dicTotal"></param>
        /// <returns></returns>
        public Dictionary<string, int> GetDataFromOneFile(string filePath, int k, Dictionary<int, int> dicTotal)
        {
            Dictionary<string, int> dicTmp = new Dictionary<string, int>();
            if (!dicTotal.ContainsKey(k))
            {
                using (StreamReader sr = new StreamReader(filePath, Encoding.ASCII))
                {
                    sr.ReadLine();
                    char[] flag = { '\t' };
                    int total = 0;
                    while (true)
                    {
                        string tmp = sr.ReadLine();
                        if (tmp == null)
                            break;
                        else
                        {
                            string[] str = tmp.Split(flag, StringSplitOptions.RemoveEmptyEntries);
                            int n = int.Parse(str[1]);
                            dicTmp.Add(str[0], n);
                            total += n;
                        }
                    }//while
                    dicTotal.Add(k, total);
                }//using           
            } 
            return dicTmp;
        }

        /// <summary>
        /// 计算MarkovPossibility
        /// </summary>
        /// <param name="r">阶数</param>
        /// <param name="total">总数</param>
        /// <param name="dic1"></param>
        /// <param name="dic2"></param>
        /// <param name="dic_kTuple"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetMarkovPossibility(int r, int total, Dictionary<string, int> dic1, Dictionary<string, int> dic2, Dictionary<string, int> dic_kTuple)
        {
            Dictionary<string, string> dic_kTuplePossibility = new Dictionary<string, string>();
            if (r == 0)
            {
                Dictionary<char, double> dicTmp = new Dictionary<char, double>();
                dicTmp.Add('A', dic2["A"] * 1.0 / total);
                dicTmp.Add('G', dic2["G"] * 1.0 / total);
                dicTmp.Add('C', dic2["C"] * 1.0 / total);
                dicTmp.Add('T', dic2["T"] * 1.0 / total);
                foreach (KeyValuePair<string, int> ktuple in dic_kTuple)
                {
                    double tmp = 1;
                    foreach (var w in ktuple.Key)
                    {
                        tmp *= dicTmp[w];
                    }//foreach
                    dic_kTuplePossibility[ktuple.Key] = tmp.ToString("e");
                }//foreach
                return dic_kTuplePossibility;
            }//if
            else
            {
                foreach (KeyValuePair<string, int> ktuple in dic_kTuple)
                {
                    int k = ktuple.Key.Length;
                    if (k <= r)
                    {
                        dic_kTuplePossibility[ktuple.Key] = "NA";
                        continue;
                    }
                    if (ktuple.Key == "total")
                    {
                        continue;
                    }
                    string index = ktuple.Key.Substring(0, r);
                    double tmp = dic1[index] * 1.0 / total;
                    for (int i = 0; i < k - r; i++)
                    {
                        string w1 = ktuple.Key.Substring(i, r + 1);
                        string w2 = ktuple.Key.Substring(i, r);
                        string w2A = w2 + "A";
                        string w2G = w2 + "G";
                        string w2C = w2 + "C";
                        string w2T = w2 + "T";
                        int nk1 = dic2[w1];
                        int nk2 = dic2[w2A] + dic2[w2C] + dic2[w2G] + dic2[w2T];
                        if (nk2 == 0)
                        {
                            tmp = 0;
                            dic_kTuplePossibility[ktuple.Key] = tmp.ToString("e");
                            continue;
                        }
                        else
                        {
                            tmp *= nk1 * 1.0 / nk2;
                            dic_kTuplePossibility[ktuple.Key] = tmp.ToString("e");
                        }//else
                    }//for
                }//foreach
                return dic_kTuplePossibility;
            }//else
        }

        /// <summary>
        /// 获得kTuple文件的数据
        /// </summary>
        /// <param name="k1">起始k值</param>
        /// <param name="k2">结束k值</param>
        /// <param name="dicSeqPath">kTuple文件所在路径字典</param>
        /// <param name="listkTupleDate">用于记录kTuple文件数据字典的列表</param>
        /// <param name="dicTotal">用于记录每个kTuple总数</param>
        public void GetkTupleDate(int k1, int k2, Dictionary<string, string> dicSeqPath, Dictionary<string, Dictionary<string, int>> dickTupleData, Dictionary<int, int> dicTotal)
        {
            int indexStar = k1 - 2;
            if (indexStar < 1)
                indexStar = 1;
            for (int i = indexStar; i < k2 + 1; i++)
            {
                if (dickTupleData.ContainsKey("k" + i))
                    continue;
                dickTupleData.Add("k" + i, GetDataFromOneFile(dicSeqPath["k" + i], i, dicTotal));
            }
        }

        /// <summary>
        /// 计算MarkovPossibilityHao
        /// </summary>
        /// <param name="k">k值</param>
        /// <param name="dicTotal">记录每个kTuple总数的字典</param>
        /// <param name="seqName">序列名字</param>
        /// <param name="savePath">文件保存路径</param>
        /// <param name="listkTupleDate">kTuple文件数据列表</param>
        public void GetMarkovPossibilityHao(int k, Dictionary<int, int> dicTotal, string seqName, string savePath, Dictionary<string, Dictionary<string, int>> dickTupleData)
        {
            int r = k - 2;
            string titleLine = "k-tuple" + "\t" + "number_of_occurrences" + "\t" + "M" + r + "_possibility";
            string saveFilePath = savePath + "\\" + seqName + "_k" + k + "_hao_wdpw.txt";

            Dictionary<string, string> dicPossibility = GetMarkovPossibility(r, dicTotal[r], dickTupleData["k" + r], dickTupleData["k" + (r + 1)], dickTupleData["k" + k]);
            WriteFile(saveFilePath, titleLine, dickTupleData["k" + k], dicPossibility);
        }

        /// <summary>
        /// 计算MarkovPossibilityZeroToThree
        /// </summary>
        /// <param name="k">k值</param>
        /// <param name="dicTotal">每个kTuple文件总数</param>
        /// <param name="seqName">序列名字</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="dickTupleData">kTuplle文件数据</param>
        public void GetMarkovPossibilityZeroToThree(int k, Dictionary<int, int> dicTotal, string seqName, string savePath, Dictionary<string, Dictionary<string, int>> dickTupleData)
        {
            int r = k - 2;
            string titleLine = "k-tuple" + "\t" + "number_of_occurrences" + "\t" + "M0_possibility" + "\t" + "M1_possibility" + "\t" + "M2_possibility" + "\t" + "M3_possibility";
            string saveFilePath = savePath + "\\" + seqName + "_k" + k + "_wordcount_pw.txt";
            Dictionary<string, string>[] arrDicPossibility = new Dictionary<string, string>[4];

            Action act1 = new Action(() =>
              {                  
                  arrDicPossibility[3] = GetMarkovPossibility(3, dicTotal[3], dickTupleData["k3"], dickTupleData["k4"], dickTupleData["k" + k]);
              });
            Action act2 = new Action(() =>
              {
                  arrDicPossibility[0] = GetMarkovPossibility(0, dicTotal[1], dickTupleData["k1"], dickTupleData["k1"], dickTupleData["k" + k]);
                  arrDicPossibility[1] = GetMarkovPossibility(1, dicTotal[1], dickTupleData["k1"], dickTupleData["k2"], dickTupleData["k" + k]);
                  arrDicPossibility[2] = GetMarkovPossibility(2, dicTotal[2], dickTupleData["k2"], dickTupleData["k3"], dickTupleData["k" + k]);
              });
            Task task = new Task(() =>
              {
                  Parallel.Invoke(act1, act2);
              });
            task.Start();
            Task.WaitAll(task);

            WriteFile(saveFilePath, titleLine, dickTupleData["k" + k], arrDicPossibility);
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="saveFilePath">保存文件的路径</param>
        /// <param name="titleLine">标题行</param>
        /// <param name="dic_kTuple">kTuple文件数据</param>
        /// <param name="dicPossibility">MarkovPossibility文件数据</param>
        private void WriteFile(string saveFilePath, string titleLine, Dictionary<string, int> dic_kTuple, params Dictionary<string, string>[] dicPossibility)
        {
            using (StreamWriter sw = new StreamWriter(saveFilePath, false, Encoding.ASCII))
            {
                sw.WriteLine(titleLine);
                string tmp = null;
                foreach (KeyValuePair<string, string> item in dicPossibility[0])
                {
                    if (dic_kTuple[item.Key] == 0)
                    {
                        tmp = item.Key + "\t1.000000E-10\t";
                    }
                    else
                    {
                        tmp = item.Key + "\t" + dic_kTuple[item.Key] + "\t";
                    }
                    foreach (var dic in dicPossibility)
                    {
                        tmp += "\t" + dic[item.Key];
                    }
                    sw.WriteLine(tmp);
                }//foreach
            }//using
        }
    }
}
