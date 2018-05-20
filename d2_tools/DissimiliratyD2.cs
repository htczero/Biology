using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d2_tools
{
    class DissimiliratyD2:DissimiliratyEuMaChD2
    {
        public DissimiliratyD2(string savePath, string[] seqNames) : base(savePath, seqNames) { }
        public override void GetDissimiliratyMatrix(int k)
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
    }
}
