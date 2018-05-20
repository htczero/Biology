namespace d2_tools
{
    class DissimiliratyEu : DissimiliratyEuMaChD2
    {
        public DissimiliratyEu(string savePath, string[] seqNames) : base(savePath, seqNames) { }

        public override void GetDissimiliratyMatrix(int k)
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
       
    }
}
