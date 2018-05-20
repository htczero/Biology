using System;

namespace d2_tools
{
    class DissimiliratyCh:DissimiliratyEuMaChD2
    {
        public DissimiliratyCh(string savePath,string []seqNames) : base(savePath, seqNames) { }

        public override void GetDissimiliratyMatrix(int k)
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
    }
}
