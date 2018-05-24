using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CalDistance
{
    class KtupleCount
    {


        #region 私有字段及其属性
        private List<int> listCount = new List<int>();
        //private List<string> listSeq = new List<string>();
        private List<List<int>> listSeq = new List<List<int>>();
        private int total;
        private int k;

        public List<int> ListCount
        {
            get
            {
                return listCount;
            }
        }


        public int Total
        {
            get
            {
                return total;
            }
        }

        public KtupleCount(List<List<int>> listSeq, int k)
        {
            this.listSeq = listSeq;
            this.k = k;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取ktuple链表
        /// </summary>
        /// <param name="k"></param>
        public void GetKtupleList()
        {
            int count = 1;
            for (int i = 0; i < k; i++)
            {
                count *= 4;
            }
            int[] arrKtuple = new int[count];
            for (int i = 0; i < listSeq.Count; i++)
            {
                if (listSeq[i].Count >= k)
                {
                    total += (listSeq[i].Count - k + 1);
                    GetKtupleCount(k, listSeq[i], arrKtuple);
                }
            }
            ListCount.AddRange(arrKtuple);
        }

        /// <summary>
        /// 获取ktuple统计文件
        /// </summary>
        /// <param name="k">tuple长度k</param>
        private void GetKtupleCount(int k, List<int> listSeq, int[] arrKtuple)
        {
            int length = listSeq.Count - k + 1;

            int key = 0;
            for (int i = 0; i < k; i++)
            {
                int tmp = listSeq[i];
                for (int j = k - 1; j > i; j--)
                {
                    tmp *= 4;
                }
                key += tmp;
            }
            arrKtuple[key]++;

            int flag = (int)Math.Pow(4, k - 1);

            for (int i = 1; i < length; i++)
            {
                int head = listSeq[i - 1];
                key = (key - head * flag) * 4 + listSeq[i + k - 1];
                arrKtuple[key]++;
            }
        }

        /// <summary>
        /// 获得序列文件内容
        /// </summary>
        /// <param name="filePath"></param>
        //private void GetSeqText(string filePath)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    using (StreamReader sr = new StreamReader(filePath, Encoding.ASCII))
        //    {
        //        string firstLine = sr.ReadLine();
        //        if (firstLine[0] == 'A' || firstLine[0] == 'G' || firstLine[0] == 'C' || firstLine[0] == 'T' || firstLine[0] == 'N')
        //            sb.Append(firstLine);
        //        while (true)
        //        {
        //            string tmp = sr.ReadLine();
        //            if (tmp == null)
        //                break;
        //            sb.Append(tmp);
        //        }
        //    }
        //    listSeq.AddRange(sb.ToString().Split(new char[] { 'N' }, StringSplitOptions.RemoveEmptyEntries));
        //}


        //private int ConvertDate(char s)
        //{
        //    int tmp = 0;
        //    switch (s)
        //    {
        //        case 'A':
        //            tmp = 0;
        //            break;

        //        case 'G':
        //            tmp = 1;
        //            break;

        //        case 'C':
        //            tmp = 2;
        //            break;

        //        default:
        //            tmp = 3;
        //            break;
        //    }
        //    return tmp;
        //}
        #endregion


    }
}
