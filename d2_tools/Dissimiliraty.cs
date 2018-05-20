using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace d2_tools
{
    class Dissimiliraty
    {
        protected List<double> listSeqOnekTuple = new List<double>();
        protected List<double> listSeqTwokTuple = new List<double>();

        protected double totalOne;
        protected double totalTwo;
        protected string savePath;
        protected string[] seqNames;
        protected int Length
        {
            get
            {
                return listSeqOnekTuple.Count;
            }
        }

        protected void WriteFile(double result, string functionName, int k)
        {
            Directory.CreateDirectory(savePath + "\\" + functionName);
            string saveFilePath = savePath + "\\" + functionName + "\\" + " dissimilarity_matrix_" + k + "_" + functionName + ".txt";

            StringBuilder sb = new StringBuilder();
            sb.Append("\t" + seqNames[0] + "\t" + seqNames[1] + "\r\n");
            sb.Append(seqNames[0] + "\t" + 0 + "\t" + 0 + "\r\n");
            //sb.Append(seqNames[1] + "\t" + (Math.Round(result, 5)) + "\t" + 0);
            sb.Append(seqNames[1] + "\t" + result + "\t" + 0);

            File.WriteAllText(saveFilePath, sb.ToString());
        }

        public Dissimiliraty(string savePath, string[] seqNames)
        {
            this.savePath = savePath;
            this.seqNames = seqNames;
        }

        public virtual void GetDissimiliratyMatrix(int k, string funName)
        {

        }
        public virtual void GetDateFromOneK(int k, Dictionary<string, string> dicPathOne, Dictionary<string, string> dicPathTwo)
        {

        }
    }
}
