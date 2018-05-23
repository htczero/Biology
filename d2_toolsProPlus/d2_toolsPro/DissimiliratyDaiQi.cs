
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace d2_toolsPro
{
    class DissimiliratyDaiQi : Dissimiliraty
    {

        #region 私有字段及其属性
        protected List<string> listResult = new List<string>();
        protected List<double[]> listPosOne = new List<double[]>();
        private List<double[]> listPosTwo = new List<double[]>();
        private List<int> listR = new List<int>();
        private Dictionary<int, List<string>> dicResultS2 = new Dictionary<int, List<string>>();
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dic">key为序列名字，value为指定k值的MarkovPossibility_Hao文件</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="k">指定k的值</param>
        /// <param name="precision">最后结果的精确度</param>
        public DissimiliratyDaiQi(Dictionary<string, string> dic, string savePath, List<int> list, string str, int precision = 5) : base(str)
        {
            foreach (KeyValuePair<string, string> item in dic)
            {
                listSeqname.Add(item.Key);
                listPath.Add(item.Value);
            }
            for (int i = 0; i < list.Count; i++)
            {
                listR.Add(list[i]);
            }
            this.precision = precision;
            SaveDir = savePath;
        }

        /// <summary>
        /// 从问件中获取数据
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="listSeqkTuple">kTuple值</param>
        /// <param name="listPos">Possibility值</param>
        /// <returns></returns>
        private double GetDateFromOneOneFile(string filePath, List<double> listSeqkTuple, List<double[]> listPos)
        {
            double totalTmp = 0;
            using (StreamReader sr = new StreamReader(filePath, Encoding.ASCII))
            {
                sr.ReadLine();


                string[] tmps = null;
                char[] flag = { '\t' };
                double n = 0;

                while (true)
                {
                    string tmp = sr.ReadLine();
                    if (tmp == null)
                        break;

                    tmps = tmp.Split(flag, StringSplitOptions.RemoveEmptyEntries);
                    n = double.Parse(tmps[1]);
                    listSeqkTuple.Add(n);
                    totalTmp += n;
                    double[] pos = new double[4];
                    for (int i = 0; i < 3; i++)
                    {
                        pos[i] = double.Parse(tmps[2 + i]);
                    }
                    double.TryParse(tmps[5], out n);
                    pos[3] = n;
                    listPos.Add(pos);
                }
            }
            return totalTmp;
        }

        /// <summary>
        /// 计算距离
        /// </summary>
        private void CalDissimiliratyMatrix(int k)
        {
            double nX = totalOne;
            double nY = totalTwo;

            foreach (var j in listR)
            {
                double resultDa = 0;
                double tmpDa = 0;
                double n = Math.Pow(4, k);

                for (int i = 0; i < listKtupleOne.Count; i++)
                {
                    double cXi = listKtupleOne[i];
                    double cYi = listKtupleTwo[i];

                    double pXi = listPosOne[i][j];
                    double pYi = listPosTwo[i][j];

                    double fXi = cXi / nX;
                    double fYi = cYi / nY;

                    double tmp1 = 0;
                    double tmp2 = 0;
                    if (pXi == 0)
                    {
                        tmp1 = -1;
                    }
                    else
                    {
                        tmp2 = pYi * Math.Log10(pXi);
                    }
                    if (pYi == 0)
                    {
                        tmp2 = -1;
                    }
                    else
                    {
                        tmp1 = pXi * Math.Log10(pYi);
                    }




                    tmpDa = (tmp1 - tmp2) * (tmp1 - tmp2);
                    resultDa += tmpDa;
                }

                resultDa = Math.Sqrt(resultDa);

                if (!dicResultS2.ContainsKey(j))
                {
                    List<string> list = new List<string>();
                    dicResultS2.Add(j, list);
                }
                dicResultS2[j].Add(Math.Round(resultDa, precision).ToString());
            }//for
        }

        /// <summary>
        /// 获取距离矩阵一行的值
        /// </summary>
        private void GetDissimiliratyMatrixForOneRow(int k)
        {
            List<double> listKtuple = new List<double>();
            List<double[]> listPos = new List<double[]>();
            double totalTmp = 0;
            int SeqCount = listPath.Count;
            int n = current + 1;
            totalTmp = GetDateFromOneOneFile(listPath[current + 1], listKtuple, listPos);

            Action actCal = () =>
            {
                CalDissimiliratyMatrix(k);
            };
            Action actGetDate = () =>
            {
                if (n + 1 < SeqCount)
                {
                    listKtuple = new List<double>();
                    listPos = new List<double[]>();
                    totalTmp = GetDateFromOneOneFile(listPath[n + 1], listKtuple, listPos);
                    n++;
                }
            };

            for (int i = 1; i < SeqCount - current; i++)
            {
                listKtupleTwo = listKtuple;
                listPosTwo = listPos;
                totalTwo = totalTmp;
                Parallel.Invoke(actCal, actGetDate);
                listKtupleTwo.Clear();
                listPosTwo.Clear();
            }

            foreach (KeyValuePair<int, List<string>> item in dicResultS2)
            {
                WriteFile("DaiQi", item.Value, "_M" + item.Key, k);
            }

        }

        /// <summary>
        /// 获得距离矩阵
        /// </summary>
        public override void GetDissimiliratyMatrix(int k)
        {
            if (k == 3 && listR.Contains(3))
            {
                listR.Remove(3);
            }
            for (int i = 0; i < listSeqname.Count - 1; i++)
            {
                current = i;
                totalOne = GetDateFromOneOneFile(listPath[i], listKtupleOne, listPosOne);
                GetDissimiliratyMatrixForOneRow(k);
                listKtupleOne.Clear();
                listPosOne.Clear();
            }
            current++;
            List<string> list = new List<string>();
            foreach (int item in listR)
            {
                WriteFile("DaiQi", list, "_M" + item, k);
            }
        }


        public override void GetDissimiliratyOneToN(int k)
        {
            if (k == 3 && listR.Contains(3))
            {
                listR.Remove(3);
            }
            current = -1;
            totalOne = GetDateFromOneOneFile(listPath[0], listKtupleOne, listPosOne);
            GetDissimiliratyMatrixForOneRow(k);
            listKtupleOne.Clear();
            listPosOne.Clear();
        }
    }
}


