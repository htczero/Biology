using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CalDistance
{
    class DissimiliratyEuMaChD2
    {
        #region 私有字段及其属性

        private List<string> listSeqname = new List<string>();
        private string saveDir;
        private int precision = 5;
        private List<string> listFunName = new List<string>();
        private List<List<int>> listKtuple = new List<List<int>>();
        private List<int> listTotal = new List<int>();
        private int k;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dic">key为序列名字，value为指定k值的ktuple值</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="k">指定k的值</param>
        /// <param name="precision">最后结果的精确度</param>
        public DissimiliratyEuMaChD2(Dictionary<string, List<int>> dic, Dictionary<string, int> dicTotal, string savePath, List<string> listFunName, int k, int precision = 5)
        {
            foreach (KeyValuePair<string, List<int>> item in dic)
            {
                listKtuple.Add(item.Value);
                listSeqname.Add(item.Key);
                listTotal.Add(dicTotal[item.Key]);
            }
            this.listFunName = listFunName;
            this.precision = precision;
            saveDir = savePath;
            this.k = k;
        }

        #region 计算距离
        /// <summary>
        /// 计算距离
        /// </summary>
        private void CalDissimiliratyMatrixAll(List<int> listKtupleOne, List<int> listKtupleTwo, int totalOne, int totalTwo, Dictionary<string, List<List<double>>> dicResult, int row)
        {
            //Eu
            double resultEu = 0;
            double tmpEu = 0;

            //Ma
            double tmpMa = 0;
            double resultMa = 0;

            //public
            double fXi = 0;
            double fYi = 0;
            double cXi = 0;
            double cYi = 0;

            //Ch
            double resultCh = 0;

            //D2
            double resultD2 = 0;
            double tmpXD2 = 0;
            double tmpYD2 = 0;
            double tmpD2 = 0;

            for (int i = 0; i < listKtupleOne.Count; i++)
            {
                //public
                cXi = listKtupleOne[i];
                cYi = listKtupleTwo[i];
                fXi = cXi / totalOne;
                fYi = cYi / totalTwo;

                //D2
                tmpD2 += cYi * cXi;
                tmpXD2 += cXi * cXi;
                tmpYD2 += cYi * cYi;

                //Eu
                tmpEu = (fXi - fYi) * (fXi - fYi);
                resultEu += tmpEu;

                //Ma
                tmpMa = Math.Abs(fXi - fYi);
                resultMa += tmpMa;

                //Ch
                resultCh = tmpMa > resultCh ? tmpMa : resultCh;
            }
            //D2
            tmpXD2 = Math.Sqrt(tmpXD2);
            tmpYD2 = Math.Sqrt(tmpYD2);
            tmpD2 = tmpD2 / (tmpXD2 * tmpYD2);
            resultD2 = 0.5 * (1 - tmpD2);

            //Eu
            resultEu = Math.Sqrt(resultEu);

            if (dicResult.ContainsKey("Eu"))
                dicResult["Eu"][row].Add(Math.Round(resultEu, precision));
            if (dicResult.ContainsKey("Ma"))
                dicResult["Ma"][row].Add(Math.Round(resultMa, precision));
            if (dicResult.ContainsKey("Ch"))
                dicResult["Ch"][row].Add(Math.Round(resultCh, precision));
            if (dicResult.ContainsKey("D2"))
                dicResult["D2"][row].Add(Math.Round(resultD2, precision));
        }

        /// <summary>
        /// 计算Ch
        /// </summary>
        private void CalDissimiliratyMatrixCh(List<int> listKtupleOne, List<int> listKtupleTwo, int totalOne, int totalTwo, Dictionary<string, List<List<double>>> dicResult, int row)
        {
            //public
            double fXi = 0;
            double fYi = 0;
            double cXi = 0;
            double cYi = 0;

            //Ch
            double resultCh = 0;
            double tmpCh = 0;

            for (int i = 0; i < listKtupleOne.Count; i++)
            {
                //public
                cXi = listKtupleOne[i];
                cYi = listKtupleTwo[i];
                fXi = cXi / totalOne;
                fYi = cYi / totalTwo;

                //Ch
                tmpCh = Math.Abs(fXi - fYi);
                resultCh = tmpCh > resultCh ? tmpCh : resultCh;
            }
            //Ch
            dicResult["Ch"][row].Add(Math.Round(resultCh, precision));
        }

        /// <summary>
        /// 计算D2
        /// </summary>
        /// <param name="k"></param>
        private void CalDissimiliratyMatrixD2(List<int> listKtupleOne, List<int> listKtupleTwo, int totalOne, int totalTwo, Dictionary<string, List<List<double>>> dicResult, int row)
        {
            //public
            double cXi = 0;
            double cYi = 0;

            //D2
            double resultD2 = 0;
            double tmpXD2 = 0;
            double tmpYD2 = 0;
            double tmpD2 = 0;


            for (int i = 0; i < listKtupleOne.Count; i++)
            {
                //public
                cXi = listKtupleOne[i];
                cYi = listKtupleTwo[i];

                //D2
                tmpD2 += cYi * cXi;
                tmpXD2 += cXi * cXi;
                tmpYD2 += cYi * cYi;
            }
            //D2
            tmpXD2 = Math.Sqrt(tmpXD2);
            tmpYD2 = Math.Sqrt(tmpYD2);
            tmpD2 = tmpD2 / (tmpXD2 * tmpYD2);
            resultD2 = 0.5 * (1 - tmpD2);
            dicResult["D2"][row].Add(Math.Round(resultD2, precision));
        }

        /// <summary>
        /// 计算Eu
        /// </summary>
        /// <param name="k"></param>
        private void CalDissimiliratyMatrixEu(List<int> listKtupleOne, List<int> listKtupleTwo, int totalOne, int totalTwo, Dictionary<string, List<List<double>>> dicResult, int row)
        {
            //Eu
            double resultEu = 0;
            double tmpEu = 0;

            //public
            double fXi = 0;
            double fYi = 0;
            double cXi = 0;
            double cYi = 0;

            for (int i = 0; i < listKtupleOne.Count; i++)
            {
                //public
                cXi = listKtupleOne[i];
                cYi = listKtupleTwo[i];
                fXi = cXi / totalOne;
                fYi = cYi / totalTwo;

                //Eu
                tmpEu = (fXi - fYi) * (fXi - fYi);
                resultEu += tmpEu;
            }
            //Eu
            resultEu = Math.Sqrt(resultEu);
            dicResult["Eu"][row].Add(Math.Round(resultEu, precision));
        }

        /// <summary>
        /// 计算Ma
        /// </summary>
        private void CalDissimiliratyMatrixMa(List<int> listKtupleOne, List<int> listKtupleTwo, int totalOne, int totalTwo, Dictionary<string, List<List<double>>> dicResult, int row)
        {
            //Ma
            double tmpMa = 0;
            double resultMa = 0;

            //public
            double fXi = 0;
            double fYi = 0;
            double cXi = 0;
            double cYi = 0;

            for (int i = 0; i < listKtupleOne.Count; i++)
            {
                //public
                cXi = listKtupleOne[i];
                cYi = listKtupleTwo[i];
                fXi = cXi / totalOne;
                fYi = cYi / totalTwo;


                //Ma
                tmpMa = Math.Abs(fXi - fYi);
                resultMa += tmpMa;
            }
            //Ma
            dicResult["Ma"][row].Add(Math.Round(resultMa, precision));
        }
        #endregion

        /// <summary>
        /// 获取距离矩阵
        /// </summary>
        public void GetDissimiliratyMatrix(bool b = false)
        {
            Action<List<int>, List<int>, int, int, Dictionary<string, List<List<double>>>, int> act = CalDissimiliratyMatrixAll;
            if (listFunName.Count == 1)
            {
                string funName = listFunName[0];
                if (funName == "Eu")
                    act = CalDissimiliratyMatrixEu;

                else if (funName == "Ma")
                    act = CalDissimiliratyMatrixMa;

                else if (funName == "Ch")
                    act = CalDissimiliratyMatrixCh;

                else if (funName == "D2")
                    act = CalDissimiliratyMatrixD2;
            }

            //string: FunName   List<List<double>>第几行  List<double>某一行的结果
            Dictionary<string, List<List<double>>> dicResult = new Dictionary<string, List<List<double>>>();
            foreach (var fun in listFunName)
            {
                dicResult.Add(fun, new List<List<double>>());
            }

            int length = listSeqname.Count - 1;
            if (b)
            {
                length = 1;
            }
            //并行部分
            for (int i = 0; i < length; i++)
            {
                foreach (List<List<double>> item in dicResult.Values)
                {
                    item.Add(new List<double>());
                }
                for (int j = i + 1; j < listSeqname.Count; j++)
                {
                    act(listKtuple[i], listKtuple[j], listTotal[i], listTotal[j], dicResult, i);
                }
            }
            WriteFile(dicResult);
        }

        private void WriteFile(Dictionary<string, List<List<double>>> dicResult)
        {
            int length = listSeqname.Count;
            foreach (KeyValuePair<string, List<List<double>>> item in dicResult)
            {
                string savePath = saveDir + "\\" + item.Key;
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);

                savePath += ("\\" + item.Key + "_k" + k + ".txt");
                using (StreamWriter sw = new StreamWriter(savePath, false, Encoding.ASCII))
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
