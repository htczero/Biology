using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace d2_toolsPro
{
    class DissimiliratyS2 : Dissimiliraty
    {
        #region 私有字段及其属性
        protected List<string> listResult = new List<string>();
        protected List<double[]> listPosOne = new List<double[]>();
        private List<double[]> listPosTwo = new List<double[]>();
        private List<int> listR = new List<int>();
        Dictionary<int, List<string>> dicResult = new Dictionary<int, List<string>>();
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dic">key为序列名字，value为指定k值的MarkovPossibility_Hao文件</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="k">指定k的值</param>
        /// <param name="precision">最后结果的精确度</param>
        public DissimiliratyD2S(Dictionary<string, string> dic, string savePath, int k, List<int> listR, int precision = 5)
        {
            foreach (KeyValuePair<string, string> item in dic)
            {
                listSeqname.Add(item.Key);
                listPath.Add(item.Value);
            }
            this.k = k;
            this.precision = precision;
            saveDir = savePath;
            this.listR = listR;
            if (k == 3 && listR.Contains(3))
                listR.Remove(3);
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
        private void CalDissimiliratyMatrix()
        {
            //public
            double nX = totalOne;
            double nY = totalTwo;

            //D2S
            double resultS2 = 0;
            double tmpS2 = 0;
            double tmpXS2 = 0;
            double tmpYS2 = 0;

            foreach (var j in listR)
            {
                for (int i = 0; i < listKtupleOne.Count; i++)
                {
                    //public
                    double cXi = listKtupleOne[i];
                    double cYi = listKtupleTwo[i];

                    double pXi = listPosOne[i][j];
                    double pYi = listPosTwo[i][j];
					
					double fXi = cXi / nX;
					double fYi = cYi / nY;
                    double cXi_sigma = cXi * pXi;
                    double cYi_sigma = cYi * pYi;

					//S2
					double tmp1 = 0;
					double tmp2 = 0;
					double tmp3 = 0;
					double tmp4 = 0;
					if(cXi_sigma==cYi_sigma)
					{
						tmp1 = 0;
						tmp2 = 0;
					}
					else
					{
						tmpS2 = cXi_sigma + cYi_sigma;
						tmp3 = 2 * cXi_sigma / tmpS2;
						tmp1 = cXi_sigma * Math.Log(tmp3);
						tmp4 = cYi_sigma / tmpS2;
						tmp2 = cYi_sigma * Math.Log(tmp4);
					}
					tmpXS2 += tmp1;
					tmpYS2 += tmp2;
                }
				if(tmpXS2 == 0 && tmpYS2 == 0)
					resultS2 = 0;
                //D2S
                else
				{
					resultS2 = (tmpXS2 + tmpYS2) / nX + 2 * Math.Log(2);
				}
                if (!dicResult.ContainsKey(j))
                {
                    List<string> list = new List<string>();
                    dicResult.Add(j, list);
                }
                dicResult[j].Add(Math.Round(resultD2S, precision).ToString());
            }
        }

        /// <summary>
        /// 获取距离矩阵一行的值
        /// </summary>
        private void GetDissimiliratyMatrixForOneRow(Action act)
        {
            List<double> listKtuple = new List<double>();
            List<double[]> listPos = new List<double[]>();
            double totalTmp = 0;
            int SeqCount = listPath.Count;
            int n = current + 1;
            totalTmp = GetDateFromOneOneFile(listPath[current + 1], listKtuple, listPos);

            Action actCal = () =>
            {
                CalDissimiliratyMatrix();
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
                Parallel.Invoke(actCal, actGetDate, act);
                listKtupleTwo.Clear();
                listPosTwo.Clear();
            }
            foreach (KeyValuePair<int, List<string>> item in dicResult)
            {
                WriteFile("D2S_M" + item.Key, item.Value);
            }
        }

        /// <summary>
        /// 获得距离矩阵
        /// </summary>
        public void GetDissimiliratyMatrix(Action act)
        {
            for (int i = 0; i < listSeqname.Count - 1; i++)
            {
                current = i;
                totalOne = GetDateFromOneOneFile(listPath[i], listKtupleOne, listPosOne);
                GetDissimiliratyMatrixForOneRow(act);
                listKtupleOne.Clear();
                listPosOne.Clear();
            }
            current++;
            List<string> list = new List<string>();
            foreach (int item in listR)
            {
                WriteFile("D2S_M" + item, list);
            }
        }

    }
}
