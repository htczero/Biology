using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace d2_toolsPro
{
    class DissimiliratyD2SD2Star : Dissimiliraty
    {
        #region 私有字段及其属性
        protected List<string> listResult = new List<string>();
        protected List<double[]> listPosOne = new List<double[]>();
        private List<double[]> listPosTwo = new List<double[]>();
        private List<int> listR = new List<int>();
        private Dictionary<int, List<string>> dicResultD2S = new Dictionary<int, List<string>>();
        private Dictionary<int, List<string>> dicResultD2Star = new Dictionary<int, List<string>>();
        string funName = "All";
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dic">key为序列名字，value为指定k值的MarkovPossibility_Hao文件</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="k">指定k的值</param>
        /// <param name="precision">最后结果的精确度</param>
        public DissimiliratyD2SD2Star(Dictionary<string, string> dic, string savePath, List<int> list, List<string> funNames, string str, int precision = 5) : base(str)
        {
            foreach (KeyValuePair<string, string> item in dic)
            {
                listSeqname.Add(item.Key);
                listPath.Add(item.Value);
            }
            this.precision = precision;
            SaveDir = savePath;
            for (int i = 0; i < list.Count; i++)
            {
                listR.Add(list[i]);
            }
            if (funNames.Count != 2)
            {
                funName = funNames[0];
            }
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
        private void CalDissimiliratyMatrix(string funName = "All")
        {
            //public
            double nX = totalOne;
            double nY = totalTwo;

            foreach (var j in listR)
            {
                //D2S
                double resultD2S = 0;
                double tmpD2S = 0;
                double tmpXD2S = 0;
                double tmpYD2S = 0;

                //D2Star
                double tmpXD2Star = 0;
                double tmpYD2Star = 0;
                double resultD2Star = 0;

                for (int i = 0; i < listKtupleOne.Count; i++)
                {
                    //public
                    double cXi = listKtupleOne[i];
                    double cYi = listKtupleTwo[i];
                    double pXi = listPosOne[i][j];
                    double pYi = listPosTwo[i][j];
                    double tmpX = nX * pXi;
                    double tmpY = nY * pYi;
                    double cXi_bar = cXi - tmpX;
                    double cYi_bar = cYi - tmpY;
                    double tmp1 = cXi_bar * cXi_bar;
                    double tmp2 = cYi_bar * cYi_bar;


                    //D2S               
                    if (funName == "All" || funName == "D2S")
                    {
                        tmpD2S = Math.Sqrt(tmp1 + tmp2);
                        if (tmpD2S == 0)
                            tmpD2S = 1;
                        resultD2S += cXi_bar * cYi_bar / tmpD2S;
                        tmpXD2S += tmp1 / tmpD2S;
                        tmpYD2S += tmp2 / tmpD2S;
                    }

                    //D2Star
                    if (funName == "All" || funName == "D2Star")
                    {
                        double tmp3 = Math.Sqrt(tmpX * tmpY);
                        if (tmpX == 0)
                        {
                            tmpX = tmp3 = 1;
                        }
                        if (tmpY == 0)
                        {
                            tmpX = tmp3 = 1;
                        }
                        resultD2Star += cXi_bar * cYi_bar / tmp3;
                        tmpXD2Star += tmp1 / tmpX;
                        tmpYD2Star += tmp2 / tmpY;
                    }
                }
                //D2S
                if (funName == "All" || funName == "D2S")
                {
                    tmpXD2S = Math.Sqrt(tmpXD2S);
                    tmpYD2S = Math.Sqrt(tmpYD2S);
                    resultD2S = (1 - resultD2S / (tmpXD2S * tmpYD2S)) * 0.5;
                    if (!dicResultD2S.ContainsKey(j))
                    {
                        List<string> list = new List<string>();
                        dicResultD2S.Add(j, list);
                    }
                    dicResultD2S[j].Add(Math.Round(resultD2S, precision).ToString());
                }

                //D2Star
                if (funName == "All" || funName == "D2Star")
                {
                    tmpXD2Star = Math.Sqrt(tmpXD2Star);
                    tmpYD2Star = Math.Sqrt(tmpYD2Star);
                    resultD2Star = 0.5 * (1 - resultD2Star / (tmpXD2Star * tmpYD2Star));
                    if (!dicResultD2Star.ContainsKey(j))
                    {
                        List<string> list = new List<string>();
                        dicResultD2Star.Add(j, list);
                    }
                    dicResultD2Star[j].Add(Math.Round(resultD2Star, precision).ToString());
                }
            }
        }

        /// <summary>
        /// 获取距离矩阵一行的值
        /// </summary>
        private void GetDissimiliratyMatrixForOneRow(int k, string funName = "All")
        {
            List<double> listKtuple = new List<double>();
            List<double[]> listPos = new List<double[]>();
            double totalTmp = 0;
            int SeqCount = listPath.Count;
            int n = current + 1;
            totalTmp = GetDateFromOneOneFile(listPath[current + 1], listKtuple, listPos);

            Action actCal = () =>
            {
                CalDissimiliratyMatrix(funName);
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
            if (funName == "All" || funName == "D2S")
            {
                foreach (KeyValuePair<int, List<string>> item in dicResultD2S)
                {
                    WriteFile("D2S", item.Value, "D2S_M" + item.Key, k);
                }
            }
            if (funName == "All" || funName == "D2Star")
            {
                foreach (KeyValuePair<int, List<string>> item in dicResultD2Star)
                {
                    WriteFile("D2Star", item.Value, "D2Star_M" + item.Key, k);
                }
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
                GetDissimiliratyMatrixForOneRow(k, funName);
                listKtupleOne.Clear();
                listPosOne.Clear();
            }
            current++;
            List<string> list = new List<string>();
            if (funName == "All" || funName == "D2S")
            {
                foreach (int item in listR)
                {
                    WriteFile("D2S", list, "D2S_M" + item, k);
                }
            }
            if (funName == "All" || funName == "D2Star")
            {
                foreach (int item in listR)
                {
                    WriteFile("D2Star", list, "D2Star_M" + item, k);
                }
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
            GetDissimiliratyMatrixForOneRow(k, funName);
            listKtupleOne.Clear();
            listPosOne.Clear();
        }

    }
}
