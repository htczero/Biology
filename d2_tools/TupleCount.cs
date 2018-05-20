using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace d2_tools
{
    class TupleCount
    {
        #region 私有字段及其属性
        private int k;
        private string savePath;
        private string seqName;
        private string sampleSeq;
        private List<string> kTuples = new List<string>();

        public int K
        {
            get
            {
                return k;
            }

            set
            {
                k = value;
            }
        }

        public List<string> KTuples
        {
            get
            {
                return kTuples;
            }

            set
            {
                kTuples = value;
            }
        }
        #endregion

        #region 方法
        private void GetkTupleSeries()
        {         
            StringBuilder sb = new StringBuilder();
            int length = (int)(Math.Pow(4, K));
            int index = 0;
            int temp = 0;
            string[] element = { "A", "G", "C", "T" };
            for (int i = 0; i < length; i++)
            {
                
                temp = i;
                for (int j = 0; j < K - 1; j++)
                {
                    index = temp % 4;
                    temp = temp / 4;
                    sb.Append(element[index]);
                }
                sb.Append(element[temp]);
                KTuples.Add(sb.ToString());
                sb.Clear();
            }
        }
        /// <summary>
        /// 计算一个Tuple出现的次数
        /// </summary>
        /// <param name="sample">样本序列</param>
        /// <param name="kTuple">kTuple</param>
        private string GetOnekTupleCount(string kTuple)
        {
            int r = 0;
            int num = 0;
            while (true)
            {
                r = sampleSeq.IndexOf(kTuple, r);
                if (r == -1 || r > sampleSeq.Length)
                    break;
                else
                {
                    num++;
                    r += k;
                }
            }//while
            return (kTuple + "\t" + num);
        }

        /// <summary>
        /// 计算kTuple
        /// </summary>
        public void GetkTupleCount()
        {
            string saveFilePath = savePath+"\\"+ seqName + "_k" + K + "_tupleCount.txt";
            GetkTupleSeries();
            using (StreamWriter sw = new StreamWriter(saveFilePath,false,Encoding.ASCII))
            {
                sw.WriteLine("k-tuple number_of_occurrences");
                foreach (string ktuple in KTuples)
                {
                    sw.WriteLine(GetOnekTupleCount(ktuple));
                }
            }
        }

      /// <summary>
      /// 构造函数
      /// </summary>
      /// <param name="seqName">样本序列名字</param>
      /// <param name="sampleSeq">样本序列本身</param>
      /// <param name="Savepath">保存的路径</param>
        public TupleCount(string seqName,string sampleSeq,string Savepath)
        {
            this.seqName = seqName;
            this.sampleSeq = sampleSeq;
            this.savePath = Savepath;            
        }
        #endregion
    }
}
