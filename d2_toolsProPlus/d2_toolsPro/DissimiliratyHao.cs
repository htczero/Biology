using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace d2_toolsPro
{
    class DissimiliratyHao : Dissimiliraty
    {
        #region 私有字段及其属性

        protected List<double> listPosOne = new List<double>();
        private List<double> listPosTwo = new List<double>();
        protected List<string> listResult = new List<string>();

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dic">key为序列名字，value为指定k值的MarkovPossibility_Hao文件</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="k">指定k的值</param>
        /// <param name="precision">最后结果的精确度</param>
        public DissimiliratyHao(Dictionary<string, string> dic, string savePath, string str, int precision = 5) : base(str)
        {
            foreach (KeyValuePair<string, string> item in dic)
            {
                listSeqname.Add(item.Key);
                listPath.Add(item.Value);
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
        private double GetDateFromOneOneFile(string filePath, List<double> listSeqkTuple, List<double> listPos)
        {
            double totalTmp = 0;
            using (StreamReader sr = new StreamReader(filePath, Encoding.ASCII))
            {
                sr.ReadLine();
                char[] flag = { '\t' };
                double n = 0;
                double pos = 0;
                while (true)
                {
                    string tmp = sr.ReadLine();
                    if (tmp == null)
                        break;
                    else
                    {
                        string[] str = tmp.Split(flag, StringSplitOptions.RemoveEmptyEntries);
                        n = double.Parse(str[1]);
                        pos = double.Parse(str[2]);
                        totalTmp += n;
                        listSeqkTuple.Add(n);
                        listPos.Add(pos);
                    }
                }//while               
            }//using        
            return totalTmp;
        }

        /// <summary>
        /// 计算距离
        /// </summary>
        private void CalDissimiliratyMatrix()
        {
            double result = 0;
            double tmpXY = 0;
            double tmpX = 0;
            double tmpY = 0;
            double tmp1 = 0;
            double tmp2 = 0;
            double fXi = 0;
            double fYi = 0;


            for (int i = 0; i < listKtupleTwo.Count; i++)
            {
                fXi = listKtupleOne[i] / totalOne;
                fYi = listKtupleTwo[i] / totalTwo;
                if (listPosOne[i] == 0)
                    tmp1 = -1;
                else
                    tmp1 = (fXi / listPosOne[i]) - 1;
                if (listPosTwo[i] == 0)
                    tmp2 = -1;
                else
                    tmp2 = (fYi / listPosTwo[i]) - 1;
                tmpXY += tmp1 * tmp2;
                tmpX += tmp1 * tmp1;
                tmpY += tmp2 * tmp2;
            }
            tmpX = Math.Sqrt(tmpX);
            tmpY = Math.Sqrt(tmpY);
            result = (1 - tmpXY / (tmpX * tmpY)) / 2;
            listResult.Add(Math.Round(result, precision).ToString());
        }

        /// <summary>
        /// 获取距离矩阵一行的值
        /// </summary>
        private void GetDissimiliratyMatrixForOneRow(int k)
        {
            List<double> listKtuple = new List<double>();
            List<double> listPos = new List<double>();
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
                    listPos = new List<double>();
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
            WriteFile("Hao", listResult, "Hao", k);
        }

        /// <summary>
        /// 获得距离矩阵
        /// </summary>
        public override void GetDissimiliratyMatrix(int k)
        {
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
            WriteFile("Hao", list, "Hao", k);
        }

        public override void GetDissimiliratyOneToN(int k)
        {
            current = -1;
            totalOne = GetDateFromOneOneFile(listPath[0], listKtupleOne, listPosOne);
            GetDissimiliratyMatrixForOneRow(k);
            listKtupleOne.Clear();
            listPosOne.Clear();
        }

    }
}
