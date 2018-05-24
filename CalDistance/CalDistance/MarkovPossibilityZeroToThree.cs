using System.Collections.Generic;

namespace CalDistance
{
    class MarkovPossibilityZeroToThree : MarkovPossibility
    {
        private List<int> listM = new List<int>();

        public MarkovPossibilityZeroToThree(Dictionary<int, List<int>> dicKtuple, Dictionary<int, int> dicTotal, List<int> listM, string saveDir = null) : base(dicKtuple, dicTotal, saveDir)
        {
            this.listM = listM;
        }
        public Dictionary<int, List<double>> GetMarkovPossibility(int k)
        {
            //if (list1.Count == 0)
            //{
            //    GetDateFromOneFile(1, list1);
            //    GetDateFromOneFile(2, list2);
            //    GetDateFromOneFile(3, list3);
            //    GetDateFromOneFile(4, list4);
            //}
            //GetDateFromOneFile(k, listK);

            //arrListPos[0] = CalMarkovPossibility(0, (int)DicTotal[1], list1, list1, listK);
            //arrListPos[1] = CalMarkovPossibility(1, (int)DicTotal[1], list1, list2, listK);
            //arrListPos[2] = CalMarkovPossibility(2, (int)DicTotal[2], list2, list3, listK);
            //arrListPos[3] = CalMarkovPossibility(3, (int)DicTotal[3], list3, list4, listK);
            Dictionary<int, List<double>> dicMarkov = new Dictionary<int, List<double>>();
            foreach (int item in listM)
            {
                if (item == 0)
                {
                    dicMarkov.Add(0, CalMarkovPossibility(0, DicTotal[1], DicKtuple[1], DicKtuple[1], DicKtuple[k]));
                }
                else
                {
                    dicMarkov.Add(item, CalMarkovPossibility(item, DicTotal[item], DicKtuple[item], DicKtuple[item + 1], DicKtuple[k]));
                }
            }
            return dicMarkov;
            //string titleLine = "k-tuple" + "\t" + "number_of_occurrences" + "\t" + "M0_possibility" + "\t" + "M1_possibility" + "\t" + "M2_possibility" + "\t" + "M3_possibility";
            //string saveFilePath = saveDir + "\\" + SeqName + "_k" + k + "_wordcount_pw.txt";

            //WriteFile(saveFilePath, titleLine, listK, arrListPos);
            //using (StreamWriter sw = new StreamWriter(saveFilePath, false, Encoding.ASCII))
            //{
            //    sw.WriteLine(titleLine);
            //    string tmp = null;
            //    for (int i = 0; i < DicKtuple[k].Count; i++)
            //    {
            //        if (DicKtuple[k][i] == 0)
            //        {
            //            tmp = i + "\t1.000000E-10\t";
            //        }
            //        else
            //        {
            //            tmp = i + "\t" + DicKtuple[k][i] + "\t";
            //        }
            //        foreach (KeyValuePair<int, List<double>> item in dicMarkov)
            //        {
            //            tmp += "\t" + item.Value[i].ToString("e");
            //        }
            //        sw.WriteLine(tmp);
            //    }
            //}
        }
    }
}
