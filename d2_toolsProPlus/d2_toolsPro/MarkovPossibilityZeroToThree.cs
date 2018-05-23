using System;
using System.Collections.Generic;
using System.IO;

namespace d2_toolsPro
{
    class MarkovPossibilityZeroToThree : MarkovPossibility
    {
        private List<int> list4 = new List<int>();
        private List<int> listK = new List<int>();
        private List<string>[] arrListPos = new List<string>[4];

        public MarkovPossibilityZeroToThree(string[] filePath, string savePath) : base(filePath, savePath)
        {

        }
        public override void GetMarkovPossibility(int k)
        {
            if (list1.Count == 0)
            {
                GetDateFromOneFile(1, list1);
                GetDateFromOneFile(2, list2);
                GetDateFromOneFile(3, list3);
                GetDateFromOneFile(4, list4);
            }
            GetDateFromOneFile(k, listK);

            arrListPos[0] = CalMarkovPossibility(0, (int)dicTotal[1], list1, list1, listK);
            arrListPos[1] = CalMarkovPossibility(1, (int)dicTotal[1], list1, list2, listK);
            arrListPos[2] = CalMarkovPossibility(2, (int)dicTotal[2], list2, list3, listK);
            arrListPos[3] = CalMarkovPossibility(3, (int)dicTotal[3], list3, list4, listK);

            string titleLine = "k-tuple" + "\t" + "number_of_occurrences" + "\t" + "M0_possibility" + "\t" + "M1_possibility" + "\t" + "M2_possibility" + "\t" + "M3_possibility";
            string saveFilePath = saveDir + "\\" + SeqName + "_k" + k + "_wordcount_pw.txt";

            WriteFile(saveFilePath, titleLine, listK, arrListPos);
            listK.Clear();
        }
    }
}
