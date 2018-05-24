using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CalDistance
{
    class DissimiliratyD2SD2Star
    {
        #region 私有字段及其属性
        private List<string> listSeqname = new List<string>();
        private List<int> listR = new List<int>();
        private string saveDir;
        private int precision = 5;
        private List<string> listFunName = new List<string>();
        private List<List<int>> listKtuple = new List<List<int>>();
        private List<Dictionary<int, List<double>>> listMarkov = new List<Dictionary<int, List<double>>>();
        private List<int> listTotal = new List<int>();
        private int k;
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dic">key为序列名字，value为指定k值的MarkovPossibility_Hao文件</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="k">指定k的值</param>
        /// <param name="precision">最后结果的精确度</param>
        public DissimiliratyD2SD2Star(Dictionary<string, List<int>> dic, Dictionary<string, Dictionary<int, List<double>>> dicMarkov, Dictionary<string, int> dicTotal, string savePath, List<string> listFunName, List<int> listR, int k, int precision = 5)
        {
            foreach (KeyValuePair<string, List<int>> item in dic)
            {
                listKtuple.Add(item.Value);
                listSeqname.Add(item.Key);
                listTotal.Add(dicTotal[item.Key]);
                listMarkov.Add(dicMarkov[item.Key]);
            }
            this.listFunName = listFunName;
            this.precision = precision;
            saveDir = savePath;
            this.k = k;
            this.listR = listR;
        }

        /// <summary>
        /// 计算距离
        /// </summary>
        private void CalDissimiliratyMatrix(List<int> listKtupleOne, List<int> listKtupleTwo, Dictionary<int, List<double>> dicPosOne, Dictionary<int, List<double>> dicPosTwo, int totalOne, int totalTwo, Dictionary<string, Dictionary<int, List<List<double>>>> dicResult, int row)
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
                    double pXi = dicPosOne[j][i];
                    double pYi = dicPosTwo[j][i];
                    double tmpX = nX * pXi;
                    double tmpY = nY * pYi;
                    double cXi_bar = cXi - tmpX;
                    double cYi_bar = cYi - tmpY;
                    double tmp1 = cXi_bar * cXi_bar;
                    double tmp2 = cYi_bar * cYi_bar;


                    //D2S
                    tmpD2S = Math.Sqrt(tmp1 + tmp2);
                    if (tmpD2S == 0)
                        tmpD2S = 1;

                    resultD2S += cXi_bar * cYi_bar / tmpD2S;
                    tmpXD2S += tmp1 / tmpD2S;
                    tmpYD2S += tmp2 / tmpD2S;


                    //D2Star
                    double tmp3 = Math.Sqrt(tmpX * tmpY);
                    if (tmpX == 0)
                    {
                        tmpX = tmp3 = 1;
                    }
                    if (tmpY == 0)
                    {
                        tmpY = tmp3 = 1;
                    }
                    resultD2Star += cXi_bar * cYi_bar / tmp3;
                    tmpXD2Star += tmp1 / tmpX;
                    tmpYD2Star += tmp2 / tmpY;

                }

                tmpXD2S = Math.Sqrt(tmpXD2S);
                tmpYD2S = Math.Sqrt(tmpYD2S);
                resultD2S = (1 - resultD2S / (tmpXD2S * tmpYD2S)) * 0.5;
                dicResult["D2S"][j][row].Add(Math.Round(resultD2S, precision));


                //D2Star

                tmpXD2Star = Math.Sqrt(tmpXD2Star);
                tmpYD2Star = Math.Sqrt(tmpYD2Star);
                resultD2Star = 0.5 * (1 - resultD2Star / (tmpXD2Star * tmpYD2Star));
                dicResult["D2Star"][j][row].Add(Math.Round(resultD2Star, precision));
            }
        }

        private void CalDissimiliratyMatrixD2S(List<int> listKtupleOne, List<int> listKtupleTwo, Dictionary<int, List<double>> dicPosOne, Dictionary<int, List<double>> dicPosTwo, int totalOne, int totalTwo, Dictionary<string, Dictionary<int, List<List<double>>>> dicResult, int row)
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

                for (int i = 0; i < listKtupleOne.Count; i++)
                {
                    //public
                    double cXi = listKtupleOne[i];
                    double cYi = listKtupleTwo[i];
                    double pXi = dicPosOne[j][i];
                    double pYi = dicPosTwo[j][i];
                    double tmpX = nX * pXi;
                    double tmpY = nY * pYi;
                    double cXi_bar = cXi - tmpX;
                    double cYi_bar = cYi - tmpY;
                    double tmp1 = cXi_bar * cXi_bar;
                    double tmp2 = cYi_bar * cYi_bar;



                    tmpD2S = Math.Sqrt(tmp1 + tmp2);
                    if (tmpD2S == 0)
                        tmpD2S = 1;
                    resultD2S += cXi_bar * cYi_bar / tmpD2S;
                    tmpXD2S += tmp1 / tmpD2S;
                    tmpYD2S += tmp2 / tmpD2S;




                }

                tmpXD2S = Math.Sqrt(tmpXD2S);
                tmpYD2S = Math.Sqrt(tmpYD2S);
                resultD2S = (1 - resultD2S / (tmpXD2S * tmpYD2S)) * 0.5;
                dicResult["D2S"][j][row].Add(Math.Round(resultD2S, precision));
            }
        }

        private void CalDissimiliratyMatrixD2Star(List<int> listKtupleOne, List<int> listKtupleTwo, Dictionary<int, List<double>> dicPosOne, Dictionary<int, List<double>> dicPosTwo, int totalOne, int totalTwo, Dictionary<string, Dictionary<int, List<List<double>>>> dicResult, int row)
        {
            //public
            double nX = totalOne;
            double nY = totalTwo;

            foreach (var j in listR)
            {
                //D2Star
                double tmpXD2Star = 0;
                double tmpYD2Star = 0;
                double resultD2Star = 0;

                for (int i = 0; i < listKtupleOne.Count; i++)
                {
                    //public
                    double cXi = listKtupleOne[i];
                    double cYi = listKtupleTwo[i];
                    double pXi = dicPosOne[j][i];
                    double pYi = dicPosTwo[j][i];
                    double tmpX = nX * pXi;
                    double tmpY = nY * pYi;
                    double cXi_bar = cXi - tmpX;
                    double cYi_bar = cYi - tmpY;
                    double tmp1 = cXi_bar * cXi_bar;
                    double tmp2 = cYi_bar * cYi_bar;


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

                //D2Star

                tmpXD2Star = Math.Sqrt(tmpXD2Star);
                tmpYD2Star = Math.Sqrt(tmpYD2Star);
                resultD2Star = 0.5 * (1 - resultD2Star / (tmpXD2Star * tmpYD2Star));
                dicResult["D2Star"][j][row].Add(Math.Round(resultD2Star, precision));
            }
        }


        /// <summary>
        /// 获得距离矩阵
        /// </summary>
        public void GetDissimiliratyMatrix(bool b = false)
        {
            Action<List<int>, List<int>, Dictionary<int, List<double>>, Dictionary<int, List<double>>, int, int, Dictionary<string, Dictionary<int, List<List<double>>>>, int> act = CalDissimiliratyMatrix;
            if (listFunName.Count == 1)
            {
                if (listFunName[0] == "D2S")
                {
                    act = CalDissimiliratyMatrixD2S;
                }
                else
                {
                    act = CalDissimiliratyMatrixD2Star;
                }
            }
            //string:FunName  int: M   List<List<double>>第几行  List<double>某一行的结果
            Dictionary<string, Dictionary<int, List<List<double>>>> dicResult = new Dictionary<string, Dictionary<int, List<List<double>>>>();
            foreach (var item in listFunName)
            {
                dicResult.Add(item, new Dictionary<int, List<List<double>>>());
                foreach (var m in listR)
                {
                    dicResult[item].Add(m, new List<List<double>>());
                }
            }
            //并行部分
            int length = listSeqname.Count - 1;
            if (b)
            {
                length = 1;
            }
            for (int i = 0; i < length; i++)
            {
                foreach (Dictionary<int, List<List<double>>> item in dicResult.Values)
                {
                    foreach (List<List<double>> list in item.Values)
                    {
                        list.Add(new List<double>());
                    }
                }

                for (int j = i + 1; j < listSeqname.Count; j++)
                {
                    act(listKtuple[i], listKtuple[j], listMarkov[i], listMarkov[j], listTotal[i], listTotal[j], dicResult, i);
                }
            }
            foreach (KeyValuePair<string, Dictionary<int, List<List<double>>>> item in dicResult)
            {
                WriteFile(item.Value, item.Key);
            }
        }
        private void WriteFile(Dictionary<int, List<List<double>>> dicResult, string funName)
        {
            int length = listSeqname.Count;

            string savePath = saveDir + "\\" + funName;
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            foreach (KeyValuePair<int, List<List<double>>> item in dicResult)
            {
                string save = savePath + ("\\" + funName + "_M" + item.Key + "_k" + k + ".txt");
                using (StreamWriter sw = new StreamWriter(save, false, Encoding.ASCII))
                {
                    //写入第一行序列名字
                    for (int j = 0; j < length; j++)
                    {
                        sw.Write("\t" + listSeqname[j]);
                    }
                    sw.Write("\r\n");
                    for (int j = 0; j < item.Value.Count; j++)
                    {
                        //写入左边序列名字
                        sw.Write(listSeqname[j]);

                        //写入0
                        int zeroCount = length - item.Value[j].Count;
                        for (int i = 0; i < zeroCount; i++)
                        {
                            sw.Write("\t0");
                        }

                        //写入结果
                        for (int i = 0; i < item.Value[j].Count; i++)
                        {
                            sw.Write("\t" + item.Value[j][i]);
                        }
                        sw.Write("\r\n");
                    }//for
                }//using
            }//foreach
        }
    }
}
