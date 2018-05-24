using System.Collections.Generic;

namespace CalDistance
{
    class MarkovPossibilityHao : MarkovPossibility
    {
        public MarkovPossibilityHao(Dictionary<int, List<int>> dicKtuple, Dictionary<int, int> dicTotal) : base(dicKtuple, dicTotal)
        {

        }
        public List<double> GetMarkovPossibility(int k)
        {
            List<double> listPos = new List<double>();
            int r = k - 2;

            listPos = CalMarkovPossibility(r, DicTotal[r], DicKtuple[r], DicKtuple[r + 1], DicKtuple[k]);

            //string titleLine = "k-tuple" + "\t" + "number_of_occurrences" + "\t" + "M" + r + "_possibility";
            //string saveFilePath = saveDir + "\\" + SeqName + "_k" + k + "_hao_wdpw.txt";
            //WriteFile(saveFilePath, titleLine, DicKtuple[k], listPos);
            return listPos;
        }
    }
}
