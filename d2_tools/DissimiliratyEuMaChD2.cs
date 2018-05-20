using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace d2_tools
{
    class DissimiliratyEuMaChD2 : Dissimiliraty
    {
        public DissimiliratyEuMaChD2(string savePath, string[] seqNames) : base(savePath, seqNames)
        {
            this.savePath = savePath;
            this.seqNames = seqNames;
        }
        public override void GetDateFromOneK(int k, Dictionary<string, string> dicPathOne, Dictionary<string, string> dicPathTwo)
        {
            Action act1 = new Action(() =>
            {
                totalOne = GetDateFromOneKOneFile(dicPathOne["k" + k], listSeqOnekTuple);
            });
            Action act2 = new Action(() =>
            {
                totalTwo = GetDateFromOneKOneFile(dicPathTwo["k" + k], listSeqTwokTuple);
            });
            Task task = new Task(() =>
            {
                Parallel.Invoke(act1, act2);
            });
            task.Start();
            Task.WaitAll(task);
        }
        protected double GetDateFromOneKOneFile(string filePath, List<double> listSeqkTuple)
        {
            double totalTmp = 0;
            using (StreamReader sr = new StreamReader(filePath, Encoding.ASCII))
            {
                sr.ReadLine();
                sr.ReadLine();
                char[] flag = { '\t' };
                double n = 0;

                while (true)
                {
                    string tmp = sr.ReadLine();
                    if (tmp == null)
                        break;
                    else
                    {
                        string[] str = tmp.Split(flag, StringSplitOptions.RemoveEmptyEntries);
                        n = double.Parse(str[1]);
                        if (n == 0)
                        {
                            n = 1E-10;
                        }
                        totalTmp += n;
                        listSeqkTuple.Add(n);
                    }
                }//while               
            }//using        
            return totalTmp;
        }

        public  void GetDissimiliratyMatrixAll(int k)
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

            for (int i = 0; i < Length; i++)
            {
                //public
                cXi = listSeqOnekTuple[i];
                cYi = listSeqTwokTuple[i];
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
            WriteFile(resultD2, "D2", k);

            //Eu
            WriteFile(resultEu, "Eu", k);

            //Ma
            WriteFile(resultMa, "Ma", k);

            //Ch
            WriteFile(resultCh, "Ch", k);
        }

        public override void GetDissimiliratyMatrix(int k, string funName = "All")
        {
            if (funName == "All")
                GetDissimiliratyMatrixAll(k);

            if (funName == "Eu")
                GetDissimiliratyMatrixEu(k);

            if (funName == "Ch")
                GetDissimiliratyMatrixCh(k);

            if(funName=="Ma")
                GetDissimiliratyMatrixMa(k);

            if(funName=="D2")
                GetDissimiliratyMatrixD2(k);
        }
        public void GetDissimiliratyMatrixCh(int k)
        {
            //public
            double fXi = 0;
            double fYi = 0;
            double cXi = 0;
            double cYi = 0;

            //Ch
            double resultCh = 0;
            double tmpCh = 0;

            for (int i = 0; i < Length; i++)
            {
                //public
                cXi = listSeqOnekTuple[i];
                cYi = listSeqTwokTuple[i];
                fXi = cXi / totalOne;
                fYi = cYi / totalTwo;

                //Ch
                tmpCh = Math.Abs(fXi - fYi);
                resultCh = tmpCh > resultCh ? tmpCh : resultCh;
            }
            //Ch
            WriteFile(resultCh, "Ch", k);
        }
        public void GetDissimiliratyMatrixD2(int k)
        {
            //public
            double cXi = 0;
            double cYi = 0;

            //D2
            double resultD2 = 0;
            double tmpXD2 = 0;
            double tmpYD2 = 0;
            double tmpD2 = 0;


            for (int i = 0; i < Length; i++)
            {
                //public
                cXi = listSeqOnekTuple[i];
                cYi = listSeqTwokTuple[i];

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
            WriteFile(resultD2, "D2", k);
        }
        public void GetDissimiliratyMatrixEu(int k)
        {
            //Eu
            double resultEu = 0;
            double tmpEu = 0;

            //public
            double fXi = 0;
            double fYi = 0;
            double cXi = 0;
            double cYi = 0;

            for (int i = 0; i < Length; i++)
            {
                //public
                cXi = listSeqOnekTuple[i];
                cYi = listSeqTwokTuple[i];
                fXi = cXi / totalOne;
                fYi = cYi / totalTwo;

                //Eu
                tmpEu = (fXi - fYi) * (fXi - fYi);
                resultEu += tmpEu;
            }
            //Eu
            WriteFile(resultEu, "Eu", k);
        }
        public void GetDissimiliratyMatrixMa(int k)
        {
            //Ma
            double tmpMa = 0;
            double resultMa = 0;

            //public
            double fXi = 0;
            double fYi = 0;
            double cXi = 0;
            double cYi = 0;

            for (int i = 0; i < Length; i++)
            {
                //public
                cXi = listSeqOnekTuple[i];
                cYi = listSeqTwokTuple[i];
                fXi = cXi / totalOne;
                fYi = cYi / totalTwo;

                //Ma
                tmpMa = Math.Abs(fXi - fYi);
                resultMa += tmpMa;
            }
            //Ma
            WriteFile(resultMa, "Ma", k);
        }

    }
}
