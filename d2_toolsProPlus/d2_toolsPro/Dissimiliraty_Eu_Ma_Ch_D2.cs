using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace d2_toolsPro
{
    class Dissimiliraty_Eu_Ma_Ch_D2 : Dissimiliraty
    {
        #region 私有字段及其属性

        protected Dictionary<string, List<string>> dicResult = new Dictionary<string, List<string>>();
        List<string> funName = new List<string>();

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dic">key为序列名字，value为指定k值的ktuple文件</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="k">指定k的值</param>
        /// <param name="precision">最后结果的精确度</param>
        public Dissimiliraty_Eu_Ma_Ch_D2(Dictionary<string, string> dic, string savePath, int k, List<string> funName, string str, int precision = 5) : base(str)
        {
            foreach (KeyValuePair<string, string> item in dic)
            {
                listSeqname.Add(item.Key);
                listPath.Add(item.Value);
            }
            this.funName = funName;
            this.precision = precision;
            SaveDir = savePath;
        }

        /// <summary>
        /// 从文件中获取数据
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="listSeqkTuple">kTuple值</param>
        /// <param name="listPos">Possibility值</param>
        /// <returns></returns>
        private double GetDateFromOneOneFile(string filePath, List<double> listSeqkTuple)
        {
            double totalTmp = 0;
            using (StreamReader sr = new StreamReader(filePath, Encoding.ASCII))
            {
                sr.ReadLine();
                char[] flag = { '\t' };
                double n = 0;
                string[] strs;//= sr.ReadLine().Split(flag, StringSplitOptions.RemoveEmptyEntries);
                //totalTmp = double.Parse(strs[1]);

                while (true)
                {
                    string tmp = sr.ReadLine();
                    if (tmp == null)
                        break;
                    else
                    {
                        strs = tmp.Split(flag, StringSplitOptions.RemoveEmptyEntries);
                        n = double.Parse(strs[1]);

                        totalTmp += n;

                        listSeqkTuple.Add(n);
                    }
                }//while               
            }//using        
            return totalTmp;
        }

        /// <summary>
        /// 计算距离
        /// </summary>
        private void CalDissimiliratyMatrixAll()
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
                dicResult["Eu"].Add(Math.Round(resultEu, precision).ToString());
            if (dicResult.ContainsKey("Ma"))
                dicResult["Ma"].Add(Math.Round(resultMa, precision).ToString());
            if (dicResult.ContainsKey("Ch"))
                dicResult["Ch"].Add(Math.Round(resultCh, precision).ToString());
            if (dicResult.ContainsKey("D2"))
                dicResult["D2"].Add(Math.Round(resultD2, precision).ToString());
        }

        /// <summary>
        /// 计算Ch
        /// </summary>
        private void CalDissimiliratyMatrixCh()
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
            dicResult["Ch"].Add(Math.Round(resultCh, precision).ToString());
        }

        /// <summary>
        /// 计算D2
        /// </summary>
        /// <param name="k"></param>
        private void CalDissimiliratyMatrixD2()
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
            dicResult["D2"].Add(Math.Round(resultD2, precision).ToString());
        }

        /// <summary>
        /// 计算Eu
        /// </summary>
        /// <param name="k"></param>
        private void CalDissimiliratyMatrixEu()
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
            dicResult["Eu"].Add(Math.Round(resultEu, precision).ToString());
        }

        /// <summary>
        /// 计算Ma
        /// </summary>
        private void CalDissimiliratyMatrixMa()
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
            dicResult["Ma"].Add(Math.Round(resultMa, precision).ToString());
        }

        /// <summary>
        /// 获取距离矩阵一行的值
        /// </summary>
        private void GetDissimiliratyMatrixForOneRow(int k, string funName = null)
        {
            List<double> listKtuple = new List<double>();
            double totalTmp = 0;
            int SeqCount = listPath.Count;
            int n = current + 1;
            totalTmp = GetDateFromOneOneFile(listPath[current + 1], listKtuple);

            Action act = CalDissimiliratyMatrixAll;
            if (funName == "Eu")
                act = CalDissimiliratyMatrixEu;

            else if (funName == "Ma")
                act = CalDissimiliratyMatrixMa;

            else if (funName == "Ch")
                act = CalDissimiliratyMatrixCh;

            else if (funName == "D2")
                act = CalDissimiliratyMatrixD2;

            Action actCal = () =>
            {
                act();
            };
            Action actGetDate = () =>
            {
                if (n + 1 < SeqCount)
                {
                    listKtuple = new List<double>();
                    totalTmp = GetDateFromOneOneFile(listPath[n + 1], listKtuple);
                    n++;
                }
            };

            for (int i = 1; i < SeqCount - current; i++)
            {
                listKtupleTwo = listKtuple;
                totalTwo = totalTmp;
                Parallel.Invoke(actCal, actGetDate);
                listKtupleTwo.Clear();
            }
            foreach (KeyValuePair<string, List<string>> item in dicResult)
            {
                WriteFile(item.Key, item.Value, item.Key, k);
            }
        }

        /// <summary>
        /// 获得距离矩阵
        /// </summary>
        public override void GetDissimiliratyMatrix(int k)
        {
            foreach (var item in funName)
            {
                List<string> list = new List<string>();
                dicResult.Add(item, list);
            }

            for (int i = 0; i < listSeqname.Count - 1; i++)
            {
                current = i;
                totalOne = GetDateFromOneOneFile(listPath[i], listKtupleOne);
                if (dicResult.Count == 1)
                    GetDissimiliratyMatrixForOneRow(k, funName[0]);
                else
                    GetDissimiliratyMatrixForOneRow(k);
                listKtupleOne.Clear();
                foreach (KeyValuePair<string, List<string>> item in dicResult)
                {
                    item.Value.Clear();
                }
            }
            current++;
            List<string> listTmp = new List<string>();
            foreach (KeyValuePair<string, List<string>> item in dicResult)
            {
                WriteFile(item.Key, listTmp, item.Key, k);
            }
        }
        public override void GetDissimiliratyOneToN(int k)
        {
            foreach (var item in funName)
            {
                List<string> list = new List<string>();
                dicResult.Add(item, list);
            }
            current = -1;
            totalOne = GetDateFromOneOneFile(listPath[0], listKtupleOne);
            if (dicResult.Count == 1)
                GetDissimiliratyMatrixForOneRow(k, funName[0]);
            else
                GetDissimiliratyMatrixForOneRow(k);
            listKtupleOne.Clear();
            foreach (KeyValuePair<string, List<string>> item in dicResult)
            {
                item.Value.Clear();
            }
        }
    }
}
