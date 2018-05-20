using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace d2_tools
{
    class SeqSign
    {
        #region 私有字段
        private string seqOne;
        private string seqTwo;
        //private int D2_Value;
        //private int d2_Value;
        private int k;
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pathOne">序列一的路径</param>
        /// <param name="pathTwo">序列二的路径</param>
        /// <param name="k">字长</param>
        public SeqSign(string pathOne,string pathTwo,int k)
        {
            this.k = k;
            seqOne = pathOne;
            seqTwo = pathTwo;
        }

        public double D2()
        {
            int D2_Value = 0;
            int dataX = 0;
            int dataY = 0;
            int tmpX = 0;
            int tmpY = 0;
            using(StreamReader sr1=new StreamReader(seqOne, Encoding.ASCII))
            {
                using(StreamReader sr2=new StreamReader(seqTwo, Encoding.ASCII))
                {
                    sr1.ReadLine();
                    sr2.ReadLine();
                    while (true)
                    {
                        dataX = GetData(sr1);
                        dataY = GetData(sr2);
                        if (dataX == -1)
                            break;
                        else if (dataX == 0 || dataY == 0)
                            continue;
                        else
                        {
                            D2_Value += (dataX * dataY);
                            tmpX += dataX * dataX;
                            tmpY += dataY * dataY;
                        }
                    }
                }//using
            }//using
            if (D2_Value < 0 || tmpX < 0 || tmpY < 0)
            {
                return -1;
            }
            double sqrtX = Math.Sqrt(tmpX * 1.0);
            double sqrtY = Math.Sqrt(tmpY * 1.0);
            double d2_Value = 0.5 * (1 - D2_Value / (tmpX * tmpY));
            return d2_Value;
        }

        private int GetData(StreamReader sr)
        {
            string tmp = sr.ReadLine();
            if (tmp != null)
            {
                string[] datas = tmp.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                return (int.Parse(datas[1]));
            }
            else
            {
                return -1;
            }
        }
    }
}
