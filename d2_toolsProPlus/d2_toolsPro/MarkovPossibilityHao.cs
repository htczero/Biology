using System.Collections.Generic;
using System.IO;

namespace d2_toolsPro
{
    class MarkovPossibilityHao : MarkovPossibility
    {
        private List<string> listPos = new List<string>();
        public MarkovPossibilityHao(string[] filePath, string savePath) : base(filePath, savePath)
        {

        }

        public override void GetMarkovPossibility(int k)
        {
            int r = k - 2;
            if (list1.Count == 0)
            {
                GetDateFromOneFile(r, list1);
                GetDateFromOneFile(r + 1, list2);
            }

            GetDateFromOneFile(k, list3);
            listPos = CalMarkovPossibility(r, (int)dicTotal[r], list1, list2, list3);

            string titleLine = "k-tuple" + "\t" + "number_of_occurrences" + "\t" + "M" + r + "_possibility";
            string saveFilePath = saveDir  + "\\" + SeqName + "_k" + k + "_hao_wdpw.txt";
            WriteFile(saveFilePath, titleLine, list3, listPos);

            list1.Clear();
            list1 = list2;
            list2 = list3;
            list3 = new List<int>();
        }
    }
}
