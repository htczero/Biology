using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CalDistance
{
    class SequenceIntList
    {
        private List<List<List<int>>> listSequenceIntList = new List<List<List<int>>>();

        private List<string> listSeqName = new List<string>();

        private int count;


        public List<string> ListSeqName
        {
            get
            {
                return listSeqName;
            }

            set
            {
                listSeqName = value;
            }
        }

        public int Count
        {
            get
            {
                return listSeqName.Count;
            }
        }

        public List<List<List<int>>> ListSequenceIntList
        {
            get
            {
                return listSequenceIntList;
            }
        }

        public SequenceIntList(List<string> listPath)
        {
            foreach (string path in listPath)
            {
                GetOneSequence(path);
            }
        }

        private void GetOneSequence(string filePath)
        {
            string seqName = Path.GetFileNameWithoutExtension(filePath);
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(filePath, Encoding.ASCII))
            {
                string firstLine = sr.ReadLine();
                if (firstLine[0] == 'A' || firstLine[0] == 'G' || firstLine[0] == 'C' || firstLine[0] == 'T' || firstLine[0] == 'N')
                    sb.Append(firstLine);
                while (true)
                {
                    string tmp = sr.ReadLine();
                    if (tmp == null)
                        break;
                    sb.Append(tmp);
                }
            }
            ConvertToIntList(sb.ToString().Split(new char[] { 'N' }, StringSplitOptions.RemoveEmptyEntries), seqName);
            ListSeqName.Add(seqName);
        }

        private void ConvertToIntList(string[] arrSeq, string seqName)
        {
            List<List<int>> listSeqTmp = new List<List<int>>();
            foreach (string seq in arrSeq)
            {
                List<int> list = new List<int>();
                foreach (char s in seq)
                {
                    int tmp = 0;
                    switch (s)
                    {
                        case 'A':
                            tmp = 0;
                            break;

                        case 'G':
                            tmp = 1;
                            break;

                        case 'C':
                            tmp = 2;
                            break;

                        default:
                            tmp = 3;
                            break;
                    }
                    list.Add(tmp);
                }
                listSeqTmp.Add(list);
            }
            listSequenceIntList.Add(listSeqTmp);
        }

    }
}
