using System;
using System.Collections.Generic;

namespace CalDistance
{
    class MarkovData
    {
        private List<double> _listMarkov = new List<double>();

        public List<double> ListMarkov { get => _listMarkov; set => _listMarkov = value; }



        /// <summary>
        /// 10进制转4进制
        /// </summary>
        /// <param name="temp">10进制数</param>
        /// <param name="k">4进制的位数</param>
        /// <returns>返回4进制数链表</returns>
        private void Get4Bits(int temp, int k, int[] arr)
        {
            int b = 3;
            for (int i = 0; i < k - 1; i++)
            {
                arr[k - 1 - i] = temp & b;
                temp = temp >> 2;
            }
            arr[0] = temp;
        }

        /// <summary>
        /// 4进制数截取一部分后转10进制
        /// </summary>
        /// <param name="list">4进制数</param>
        /// <param name="r">截取位置</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        private int Get10Bits(int[] arr, int r, int length)
        {
            int result = 0;
            for (int i = 0; i < length; i++)
            {
                //int tmp = 1;  
                result += (arr[i + r] << ((length - i - 1) << 1));   //(arr[i + r]*4的（length - i - 1）次方

            }
            return result;
        }

        /// <summary>
        /// 计算马尔科夫（非0阶）
        /// </summary>
        /// <param name="k">k值</param>
        /// <param name="r">马尔科夫阶数</param>
        /// <param name="total">序列的归一化长度</param>
        /// <param name="list1">kTuple统计数据表1</param>
        /// <param name="list2">kTuple统计数据表2</param>
        /// <returns></returns>
        private List<double> CalMarkovPossibility(int k, int r, int total, List<int> list1, List<int> list2)
        {
            int count = 1 << (k << 1);
            List<double> listPos = new List<double>(count);

            double[] arr = new double[list2.Count];
            for (int i = 0; i < list2.Count; i += 4)
            {
                double tmp = list2[i] + list2[i + 1] + list2[i + 2] + list2[i + 3];
                for (int j = 0; j < 4; j++)
                {
                    if (tmp != 0)
                    {
                        arr[i + j] = list2[i + j] / tmp;
                    }                   
                }
            }

            int[] arrBits = new int[k];
            for (int i = 0; i < count; i++)
            {
                Get4Bits(i, k, arrBits);
                int index = Get10Bits(arrBits, 0, r);
                double tmp = list1[index] * 1.0 / total;
                for (int j = 0; j < k - r; j++)
                {
                    int w1 = Get10Bits(arrBits, j, r + 1);
                    tmp *= arr[w1];
                }//for
                listPos.Add(tmp);
            }//for
            return listPos;
        }

        /// <summary>
        /// 计算0阶马尔科夫
        /// </summary>
        /// <param name="k">k值</param>
        /// <param name="total">序列归一化长度</param>
        /// <param name="list1">kTuple统计数据表</param>
        /// <returns></returns>
        private List<double> CalMarkovPossibility(int k, int total, List<int> list1)
        {
            int _count = 1 << (k << 1);   //4的k次方
            List<double> listPos = new List<double>(_count);
            List<double> listTmp = new List<double>(4);
            for (int i = 0; i < 4; i++)
            {
                listTmp.Add(list1[i] * 1.0 / total);
            }

            int[] arrBits = new int[k];
            for (int i = 0; i < _count; i++)
            {
                double tmp = 1;
                Get4Bits(i, k, arrBits);
                foreach (var item in arrBits)
                {
                    tmp *= listTmp[item];
                }
                listPos.Add(tmp);
            }
            return listPos;
        }

        /// <summary>
        /// 计算MArkov
        /// </summary>
        /// <param name="k">k值</param>
        /// <param name="m">马尔科夫阶数</param>
        /// <param name="sd">序列</param>
        public void CalMarkov(int k, int m, SequenceData sd)
        {
            if (k <= m)
            {
                return;
            }
            try
            {
                if (m == 0)  //0阶马尔科夫
                {
                    int total = sd.GetTotal(1);
                    List<int> list1 = sd.GetKtupleData(1);
                    ListMarkov = CalMarkovPossibility(k, total, list1);
                }
                else
                {
                    int total = sd.GetTotal(m);
                    List<int> list1 = sd.GetKtupleData(m);
                    List<int> list2 = sd.GetKtupleData(m + 1);
                    ListMarkov = CalMarkovPossibility(k, m, total, list1, list2);
                }
            }
            catch
            {
                throw new Exception("GetMarkov has an error!");
            }
        }
    }
}
