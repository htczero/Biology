using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace d2_toolsPro
{
    class KtupleCount
    {


        #region 私有字段及其属性
        private List<int> listKtuple = new List<int>();
        private string seqText;
        private string saveDir;
        private string seqName;

        public string SeqName
        {
            get
            {
                return seqName;
            }
        }

        public string SaveDir
        {
            get
            {
                return saveDir;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">序列文件路径</param>
        /// <param name="savePath">ktuple文件保存路径</param>
        public KtupleCount(string filePath, string savePath)
        {
            seqName = Path.GetFileNameWithoutExtension(filePath);
            saveDir = savePath + "\\" + SeqName;
            Directory.CreateDirectory(SaveDir);
            GetSeqText(filePath);
        }

        /// <summary>
        /// 获取ktuple链表
        /// </summary>
        /// <param name="k"></param>
        private void GetKtupleList(int k)
        {
            int length = (int)(Math.Pow(4, k));
            for (int i = 0; i < length; i++)
            {
                listKtuple.Add(0);
            }
        }

        /// <summary>
        /// 获取ktuple统计文件
        /// </summary>
        /// <param name="k">tuple长度k</param>
        /// <param name="bOverlap">是否重叠统计</param>
        public void GetKtupleCount(int k)
        {
            GetKtupleList(k);
            string path = SaveDir + "\\" + SeqName + "_k" + k + "_tupleCount.txt";
            for (int i = 0; i < seqText.Length - k + 1; i++)
            {
                string key = seqText.Substring(i, k);
                if (!key.Contains("N"))
                {
                    listKtuple[ConvertDate(key)]++;
                }
            }

            using (StreamWriter sw = new StreamWriter(path, false, Encoding.ASCII))
            {
                sw.WriteLine("k-tuple\tnumber_of_occurrences");
                for (int i = 0; i < listKtuple.Count; i++)
                {
                    sw.WriteLine(i + "\t" + listKtuple[i]);
                }
            }
            listKtuple.Clear();
        }

        /// <summary>
        /// 获得序列文件内容
        /// </summary>
        /// <param name="filePath"></param>
        private void GetSeqText(string filePath)
        {
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
            seqText = sb.ToString();
        }


        private int ConvertDate(string str)
        {
            int n = 0;
            int tmp = 0;
            int length = str.Length;
            for (int i = 0; i < length; i++)
            {
                switch (str[i])
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

                    case 'T':
                        tmp = 3;
                        break;
                }
                n += tmp * (int)Math.Pow(4, length - 1 - i);
            }
            return n;
        }
        #endregion


    }
}
