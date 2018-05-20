using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace d2_tools
{

    class DissimiliratyHao : Dissimiliraty
    {
        protected List<double> listSeqOnePos = new List<double>();
        protected List<double> listSeqTwoPos = new List<double>();
        public DissimiliratyHao(string savePath, string[] seqNames) : base(savePath, seqNames) { }
        public override void GetDateFromOneK(int k, Dictionary<string, string> dicPathOne, Dictionary<string, string> dicPathTwo)
        {
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
        private double GetDateFromOneKOneFile(string filePath, List<double> listSeqkTuple, List<double> listPos)
        {
            double totalTmp = 0;
            using (StreamReader sr = new StreamReader(filePath, Encoding.ASCII))
            {
                sr.ReadLine();
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
        public override void GetDissimiliratyMatrix(int k, string funName = null)
        {
            double result = 0;
            double tmpXY = 0;
            double tmpX = 0;
            double tmpY = 0;
            double tmp1 = 0;
            double tmp2 = 0;
            double fXi = 0;
            double fYi = 0;


            for (int i = 0; i < Length; i++)
            {
                fXi = listSeqOnekTuple[i] / totalOne;
                fYi = listSeqTwokTuple[i] / totalTwo;
                if (listSeqOnePos[i] == 0)
                    tmp1 = -1;
                else
                    tmp1 = (fXi / listSeqOnePos[i]) - 1;
                if (listSeqTwoPos[i] == 0)
                    tmp2 = -1;
                else
                    tmp2 = (fYi / listSeqTwoPos[i]) - 1;
                tmpXY += tmp1 * tmp2;
                tmpX += tmp1 * tmp1;
                tmpY += tmp2 * tmp2;
            }
            tmpX = Math.Sqrt(tmpX);
            tmpY = Math.Sqrt(tmpY);
            result = (1 - tmpXY / (tmpX * tmpY)) / 2;
            WriteFile(result, "Hao", k);
        }
    }
}
