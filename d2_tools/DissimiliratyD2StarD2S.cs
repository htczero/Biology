using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace d2_tools
{
    class DissimiliratyD2StarD2S : Dissimiliraty
    {
        protected List<double[]> listSeqOnePos = new List<double[]>();
        protected List<double[]> listSeqTwoPos = new List<double[]>();
        protected int r1;
        protected int r2;
        public DissimiliratyD2StarD2S(string savePath, string[] seqNames, int r1, int r2) : base(savePath, seqNames)
        {
            this.r1 = r1;
            this.r2 = r2;
        }
        public override void GetDateFromOneK(int k, Dictionary<string, string> dicPathOne, Dictionary<string, string> dicPathTwo)
        {
            listSeqOnePos = new List<double[]>();
            listSeqTwoPos = new List<double[]>();
            Action act1 = new Action(() =>
              {
                  totalOne = GetDateFromOneKOneFile(dicPathOne["k" + k], listSeqOnekTuple, listSeqOnePos);
              });
            Action act2 = new Action(() =>
              {
                  totalTwo = GetDateFromOneKOneFile(dicPathTwo["k" + k], listSeqTwokTuple, listSeqTwoPos);
              });
            Task task = new Task(() =>
              {
                  Parallel.Invoke(act1, act2);
              });
            task.Start();
            Task.WaitAll(task);
        }
        protected double GetDateFromOneKOneFile(string filePath, List<double> listSeqkTuple, List<double[]> listPos)
        {
            double totalTmp = 0;
            using (StreamReader sr = new StreamReader(filePath, Encoding.ASCII))
            {
                sr.ReadLine();

                double[] pos = new double[4];
                string[] tmp = null;
                char[] flag = { '\t' };
                double n = 0;

                while (true)
                {
                    tmp = sr.ReadLine().Split(flag, StringSplitOptions.RemoveEmptyEntries);
                    if (tmp == null)
                        break;
                    totalTmp += double.Parse(tmp[1]);
                    for (int i = 0; i < 3; i++)
                    {
                        pos[i] = double.Parse(tmp[2 + i]);
                    }
                    double.TryParse(tmp[5], out n);
                    pos[4] = n;
                }
            }
            return totalTmp;
        }
        public  void GetDissimiliratyMatrixAll(int k)
        {

            //public
            double nX = totalOne;
            double nY = totalTwo;

            //D2S
            double resultD2S = 0;
            double tmpD2S = 0;
            double tmpXD2S = 0;
            double tmpYD2S = 0;

            //D2Star
            double resultD2Star = 0;
            double tmpXD2Star = 0;
            double tmpYD2Star = 0;

            int end = r2 + 1;
            if (k == 3)
                end--;

            for (int j = r1; j < end; j++)
            {
                for (int i = 0; i < Length; i++)
                {
                    //public
                    double cXi = listSeqOnekTuple[i];
                    double cYi = listSeqTwokTuple[i];

                    double pXi = listSeqOnePos[i][j];
                    double pYi = listSeqTwoPos[i][j];
                    double tmpX = nX * pXi;
                    double tmpY = nY * pYi;
                    double cXi_bar = cXi - tmpX;
                    double cYi_bar = cYi - tmpY;

                    //D2S               
                    double tmp1 = Math.Sqrt(cXi_bar);
                    double tmp2 = Math.Sqrt(cYi_bar);
                    tmpD2S = Math.Sqrt(tmp1 + tmp2);
                    if (tmpD2S == 0)
                        tmpD2S = 1;
                    resultD2S += cXi_bar * cYi_bar / tmpD2S;
                    tmpXD2S += cXi_bar * cXi_bar / tmpD2S;
                    tmpYD2S += cYi_bar * cYi_bar / tmpD2S;

                    //D2Star
                    resultD2Star += cXi_bar * cYi_bar / (Math.Sqrt(tmpX * tmpY));
                    tmpXD2Star += cXi_bar * cXi_bar / tmpX;
                    tmpYD2Star += cYi_bar * cYi_bar / tmpY;
                }
                //D2S
                tmpXD2S = Math.Sqrt(tmpXD2S);
                tmpYD2S = Math.Sqrt(tmpYD2S);
                resultD2S = (1 - resultD2S / (tmpXD2S * tmpYD2S)) * 0.5;
                WriteFile(resultD2S, "D2S", k);

                //D2Star
                tmpXD2Star = Math.Sqrt(tmpXD2Star);
                tmpYD2Star = Math.Sqrt(tmpYD2Star);
                resultD2Star = 0.5 * (1 - resultD2Star / (tmpXD2Star * tmpYD2Star));
                WriteFile(resultD2Star, "D2Star", k, j);
            }
        }
        public override void GetDissimiliratyMatrix(int k,string funName = "All")
        {
            if (funName == "All")
                GetDissimiliratyMatrixAll(k);

            if (funName == "D2S")
                GetDissimiliratyMatrixD2S(k);

            if (funName == "D2Star")
                GetDissimiliratyMatrixD2Star(k);
        }
        public  void GetDissimiliratyMatrixD2Star(int k)
        {
            //public
            double nX = totalOne;
            double nY = totalTwo;

            //D2Star
            double resultD2Star = 0;
            double tmpXD2Star = 0;
            double tmpYD2Star = 0;

            int end = r2 + 1;
            if (k == 3)
                end--;

            for (int j = r1; j < end; j++)
            {
                for (int i = 0; i < Length; i++)
                {
                    //public
                    double cXi = listSeqOnekTuple[i];
                    double cYi = listSeqTwokTuple[i];
                    double pXi = listSeqOnePos[i][j];
                    double pYi = listSeqTwoPos[i][j];
                    double tmpX = nX * pXi;
                    double tmpY = nY * pYi;
                    double cXi_bar = cXi - tmpX;
                    double cYi_bar = cYi - tmpY;


                    //D2Star
                    resultD2Star += cXi_bar * cYi_bar / (Math.Sqrt(tmpX * tmpY));
                    tmpXD2Star += cXi_bar * cXi_bar / tmpX;
                    tmpYD2Star += cYi_bar * cYi_bar / tmpY;

                }
                //D2Star
                tmpXD2Star = Math.Sqrt(tmpXD2Star);
                tmpYD2Star = Math.Sqrt(tmpYD2Star);
                resultD2Star = 0.5 * (1 - resultD2Star / (tmpXD2Star * tmpYD2Star));
                WriteFile(resultD2Star, "D2Star", k, j);
            }
        }
        public void GetDissimiliratyMatrixD2S(int k)
        {
            //public
            double nX = totalOne;
            double nY = totalTwo;

            //D2S
            double resultD2S = 0;
            double tmpD2S = 0;
            double tmpXD2S = 0;
            double tmpYD2S = 0;

            int end = r2 + 1;
            if (k == 3)
                end--;

            for (int j = r1; j < end; j++)
            {
                for (int i = 0; i < Length; i++)
                {
                    //public
                    double cXi = listSeqOnekTuple[i];
                    double cYi = listSeqTwokTuple[i];
                    double pXi = listSeqOnePos[i][j];
                    double pYi = listSeqTwoPos[i][j];
                    double tmpX = nX * pXi;
                    double tmpY = nY * pYi;
                    double cXi_bar = cXi - tmpX;
                    double cYi_bar = cYi - tmpY;

                    //D2S               
                    double tmp1 = Math.Sqrt(cXi_bar);
                    double tmp2 = Math.Sqrt(cYi_bar);
                    tmpD2S = Math.Sqrt(tmp1 + tmp2);
                    if (tmpD2S == 0)
                        tmpD2S = 1;
                    resultD2S += cXi_bar * cYi_bar / tmpD2S;
                    tmpXD2S += cXi_bar * cXi_bar / tmpD2S;
                    tmpYD2S += cYi_bar * cYi_bar / tmpD2S;
                }
                //D2S
                tmpXD2S = Math.Sqrt(tmpXD2S);
                tmpYD2S = Math.Sqrt(tmpYD2S);
                resultD2S = (1 - resultD2S / (tmpXD2S * tmpYD2S)) * 0.5;
                WriteFile(resultD2S, "D2S", k, j);
            }
        }
        protected void WriteFile(double result, string functionName, int k, int r)
        {
            Directory.CreateDirectory(savePath + "\\" + functionName);
            string saveFilePath = savePath + "\\" + functionName + "\\" + " _dissimilarity_matrix_r" + r + "_" + functionName + k + ".txt";

            StringBuilder sb = new StringBuilder();
            sb.Append("\t" + seqNames[0] + "\t" + seqNames[1] + "\r\n");
            sb.Append(seqNames[0] + "\t" + 0 + "\t" + 0 + "\r\n");
            sb.Append(seqNames[1] + "\t" + (Math.Round(result, 5)) + "\t" + 0);

            File.WriteAllText(saveFilePath, sb.ToString());
        }
    }
}
