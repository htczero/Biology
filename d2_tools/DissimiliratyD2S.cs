using System;

namespace d2_tools
{
    class DissimiliratyD2S : DissimiliratyD2StarD2S
    {
        public DissimiliratyD2S(string savePath, string[] seqNames, int r1, int r2) : base(savePath, seqNames, r1, r2) { }
        public override void GetDissimiliratyMatrix(int k)
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


    }
}
