using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CalDistance
{
    class DissimiliratyHao
    {
        #region 私有字段及其属性

        private List<string> listSeqname = new List<string>();
        private List<List<int>> listKtuple = new List<List<int>>();
        private List<List<double>> listMarkov = new List<List<double>>();
        private List<int> listTotal = new List<int>();
        private int k;
        private int precision = 5;
        private string saveDir;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dic">key为序列名字，value为指定k值的MarkovPossibility_Hao文件</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="k">指定k的值</param>
        /// <param name="precision">最后结果的精确度</param>
        public DissimiliratyHao(Dictionary<string, List<int>> dicKtuple, Dictionary<string, List<double>> dicMarkov, Dictionary<string, int> dicTotal, string savePath, int k)
        {
            foreach (KeyValuePair<string, List<int>> item in dicKtuple)
            {
                listKtuple.Add(item.Value);
                listSeqname.Add(item.Key);
                listTotal.Add(dicTotal[item.Key]);
                listMarkov.Add(dicMarkov[item.Key]);
            }
            saveDir = savePath;
            this.k = k;
        }

        /// <summary>
        /// 计算距离
        /// </summary>
        private void CalDissimiliratyMatrix(List<int> listKtupleOne, List<int> listKtupleTwo, List<double> listPosOne, List<double> listPosTwo, int totalOne, int totalTwo, List<List<double>> listResult, int row)
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
                fXi = listKtupleOne[i] * 1.0 / totalOne;
                fYi = listKtupleTwo[i] * 1.0 / totalTwo;
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
            listResult[row].Add(Math.Round(result, precision));
        }
        /// <summary>
        /// 获得距离矩阵
        /// </summary>
        public void GetDissimiliratyMatrix(bool b = false)
        {
            int length = listSeqname.Count - 1;
            if (b)
            {
                length = 1;
            }
            List<List<double>> listResult = new List<List<double>>();
            for (int i = 0; i < length; i++)
            {
                listResult.Add(new List<double>());
                for (int j = i + 1; j < listSeqname.Count; j++)
                {
                    CalDissimiliratyMatrix(listKtuple[i], listKtuple[j], listMarkov[i], listMarkov[j], listTotal[i], listTotal[j], listResult, i);
                }
            }
            WriteFile(listResult);
        }

        private void WriteFile(List<List<double>> listResult)
        {
            int length = listSeqname.Count;
            string savePath = saveDir + "\\Hao";
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            savePath += ("\\Hao_k" + k + ".txt");
            using (StreamWriter sw = new StreamWriter(savePath, false, Encoding.ASCII))
            {
                //写入第一行序列名字
                for (int j = 0; j < length; j++)
                {
                    sw.Write("\t" + listSeqname[j]);
                }
                sw.Write("\r\n");
                for (int i = 0; i < listResult.Count; i++)
                {
                    //写入左边序列名字
                    sw.Write(listSeqname[i]);
                    int zeroCount = length - listResult[i].Count;
                    for (int j = 0; j < zeroCount; j++)
                    {
                        sw.Write("\t0");
                    }

                    for (int j = 0; j < listResult[i].Count; j++)
                    {
                        sw.Write("\t" + listResult[i][j]);
                    }
                    sw.Write("\r\n");
                }//for
            }//using
        }


    }
}
