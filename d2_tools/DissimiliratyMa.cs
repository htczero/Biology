using System;

namespace d2_tools
{
    class DissimiliratyMa:DissimiliratyEuMaChD2
    {
        public DissimiliratyMa(string savePath, string[] seqNames) : base(savePath, seqNames) { }

        public override void GetDissimiliratyMatrix(int k)
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
