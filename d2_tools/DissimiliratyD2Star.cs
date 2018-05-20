using System;

namespace d2_tools
{
    class DissimiliratyD2Star : DissimiliratyD2StarD2S
    {
        public DissimiliratyD2Star(string savePath, string[] seqNames, int r1, int r2) : base(savePath, seqNames, r1, r2) { }
        public override void GetDissimiliratyMatrix(int k)
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
    }
}
