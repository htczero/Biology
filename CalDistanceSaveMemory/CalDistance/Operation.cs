using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalDistance
{
    class Operation
    {

        private List<TreeNode> listNode = new List<TreeNode>();

        private List<int> listK = new List<int>();

        private List<int> listM = new List<int>();

        private List<string> listFun = new List<string>();

        private Action<int, string> act = null;

        //string:序列名字  int：k  
        private Dictionary<string, Dictionary<int, List<int>>> dicKtuple = new Dictionary<string, Dictionary<int, List<int>>>();
        private Dictionary<string, Dictionary<int, int>> dicTotal = new Dictionary<string, Dictionary<int, int>>();
        private Dictionary<string, Dictionary<int, List<double>>> dicMarkovHao = new Dictionary<string, Dictionary<int, List<double>>>();
        private Dictionary<string, Dictionary<int, Dictionary<int, List<double>>>> dicMarkov = new Dictionary<string, Dictionary<int, Dictionary<int, List<double>>>>();

        public List<TreeNode> ListNode
        {
            get
            {
                return listNode;
            }
        }

        public List<int> ListK
        {
            get
            {
                return listK;
            }
        }

        public List<int> ListM
        {
            get
            {
                return listM;
            }
        }

        public List<string> ListFun
        {
            get
            {
                return listFun;
            }

            set
            {
                listFun = value;
            }
        }

        public Operation(Action<int, string> act)
        {
            this.act = act;
        }

        #region 准备工作
        public void ComboBoxSetting(ComboBox cbx1, ComboBox cbx2)
        {
            int tmp1 = int.Parse(cbx1.SelectedItem.ToString());
            int tmp2 = int.Parse(cbx1.Items[cbx1.Items.Count - 1].ToString());
            cbx2.Items.Clear();
            for (int i = tmp1; i <= tmp2; i++)
            {
                cbx2.Items.Add(i);
            }
            cbx2.SelectedIndex = 0;
        }

        public void TreeViewAddNode(TreeNodeCollection nodes, params string[] path)
        {
            foreach (var item in path)
            {
                TreeNode node = new TreeNode(Path.GetFileName(item));
                node.Tag = item;
                nodes.Add(node);
                if (!File.Exists(item))
                {
                    string[] files = Directory.GetFiles(item);
                    TreeViewAddNode(node.Nodes, files);
                    string[] dirs = Directory.GetDirectories(item);
                    TreeViewAddNode(node.Nodes, dirs);
                }
                //else
                //{
                //    if (!listNode.Contains(node.Parent))
                //        listNode.Add(node.Parent);
                //}
            }
            return;
        }

        public void GetValueFromComboBox(ComboBox cbx1, ComboBox cbx2, List<int> list)
        {
            int tmp1 = int.Parse(cbx1.SelectedItem.ToString());
            int tmp2 = int.Parse(cbx2.SelectedItem.ToString());
            for (int i = tmp1; i <= tmp2; i++)
            {
                list.Add(i);
            }
        }

        private void GetListNode(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                string path = (string)node.Tag;
                if (!File.Exists(path))
                {
                    GetListNode(node.Nodes);
                }
                else
                {
                    if (!ListNode.Contains(node.Parent))
                        ListNode.Add(node.Parent);
                }
            }
            return;
        }

        private void InitalState(ProgressBar pgb, bool b = false)
        {
            Action act = () =>
            {
                pgb.Maximum = 0;
                pgb.Value = 0;
                //ktuple
                int seqNum = 0;
                foreach (TreeNode node in ListNode)
                {
                    seqNum += node.Nodes.Count;
                }
                if (b)
                {
                    seqNum += ListNode.Count;
                }
                pgb.Maximum += seqNum * ListK.Count;

                int tmpK = 0;
                foreach (var item in ListK)
                {
                    if (item > 2)
                    {
                        tmpK++;
                    }
                }

                bool bMarkov = false;

                foreach (var item in listFun)
                {
                    if (item == "Eu" || item == "Ma" || item == "Ch" || item == "D2")
                    {
                        pgb.Maximum += ListK.Count * ListNode.Count;
                    }
                    else if (item == "D2S" || item == "D2Star")
                    {
                        pgb.Maximum += tmpK * listM.Count * ListNode.Count;
                        if (!bMarkov)
                        {
                            pgb.Maximum += tmpK * seqNum * listM.Count;
                            bMarkov = true;
                        }
                    }
                    else if (item == "Hao")
                    {
                        pgb.Maximum += tmpK * ListNode.Count;
                        pgb.Maximum += tmpK * seqNum;
                    }
                }
            };
            pgb.BeginInvoke(act);

        }
        #endregion

        public void Star(string saveDir, TreeNodeCollection nodes, ProgressBar p, bool b, string oneSeqPath = "")
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();
            GetListNode(nodes);
            InitalState(p, b);

            #region 准备工作
            List<string> listEuMaChD2 = new List<string>();
            List<string> listD2SD2Star = new List<string>();
            List<Action<string, int>> listAct = new List<Action<string, int>>();

            Action<string, int> act1 = (string saveTmp, int kTmp) =>
            {
                GetEuMaChD2(saveTmp, kTmp, listEuMaChD2, b);
            };


            Action<string, int> act2 = (string saveTmp, int kTmp) =>
            {
                if (kTmp > 2)
                {
                    GetMarkovHao(kTmp);
                    GetHao(saveTmp, kTmp, b);
                }
            };

            Action<string, int> act3 = (string saveTmp, int kTmp) =>
            {
                if (kTmp > 2)
                {
                    GetMarkovZeroToTwo(kTmp);
                    GetD2SD2Star(saveTmp, kTmp, listD2SD2Star, b);
                }
            };

            bool bHao = false;
            foreach (string item in ListFun)
            {
                if (item == "Eu" || item == "Ma" || item == "Ch" || item == "D2")
                {
                    listEuMaChD2.Add(item);
                }
                if (item == "Hao")
                {
                    listAct.Add(act2);
                    bHao = true;
                }
                if (item == "D2S" || item == "D2Star")
                {
                    listD2SD2Star.Add(item);
                }
            }
            if (listEuMaChD2.Count != 0)
            {
                listAct.Add(act1);
            }
            if (listD2SD2Star.Count != 0)
            {
                listAct.Add(act3);
            }
            #endregion

            int i = 1;
            //分组节点
            foreach (TreeNode node in ListNode)
            {
                act(0, "第" + i + "组：\r\n");
                i++;
                string savePath = saveDir + "\\" + node.FullPath;
                List<string> list = GetPathList(node, oneSeqPath);
                GetMarkovHaoReady();
                GetMarkovZeroToTwoReady();
                SequenceIntList sil = new SequenceIntList(list);
                foreach (int k in ListK)
                {

                    GetKtuple(sil, k);
                    Parallel.For(0, listAct.Count, (j) =>
                    {
                        listAct[j](savePath, k);
                    });

                    //foreach (var item in listAct)
                    //{
                    //    item(savePath, k);
                    //}
                    if (k > 3)
                    {
                        DictionaryRemove(k, bHao);
                    }
                }

                dicKtuple.Clear();
                dicTotal.Clear();
                dicMarkov.Clear();
                dicMarkovHao.Clear();
                act(0, "\r\n\r\n");
            }
            sp.Stop();
            MessageBox.Show("o(*￣▽￣*)ブ总算算完了" + (sp.ElapsedMilliseconds * 1.0 / 1000).ToString(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Process.Start(saveDir);
        }

        public void Inital()
        {
            listNode.Clear();
            ListK.Clear();
            ListM.Clear();
            listFun.Clear();
            dicKtuple.Clear();
            dicTotal.Clear();
            dicMarkov.Clear();
            dicMarkovHao.Clear();
        }

        #region ktuple
        private void GetKtuple(SequenceIntList sil, int k)
        {
            act(0, "\r\n\t正在计算ktuple\r\n\t......\r\n");
            Stopwatch sp = new Stopwatch();
            sp.Start();

            //序列名字节点

            //Parallel.ForEach(list, (item) =>
            // {
            //     string seqName = Path.GetFileNameWithoutExtension(item);
            //     KtupleCount kc = new KtupleCount(item, k);
            //     kc.GetKtupleList();
            //     dicKtuple[seqName].Add(k, kc.ListCount);
            //     dicTotal[seqName].Add(k, kc.Total);
            //     act(1, "");
            // });
            Parallel.For(0, sil.Count, (i) =>
            {
                KtupleCount kc = new KtupleCount(sil.ListSequenceIntList[i], k);
                kc.GetKtupleList();
                dicKtuple[sil.ListSeqName[i]].Add(k, kc.ListCount);
                dicTotal[sil.ListSeqName[i]].Add(k, kc.Total);
                act(1, "");
            });
            sp.Stop();
            act(0, "\tk = " + k + " 计算完毕，用时 " + sp.ElapsedMilliseconds * 1.0 / 1000 + " s\r\n\r\n");

        }

        #endregion

        #region EuMaChD2
        private void GetEuMaChD2(string savePath, int k, List<string> listFunName, bool b = false)
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();


            string tmp = "\r\n\t正在计算";
            foreach (var item in listFunName)
            {
                tmp += item + "  ";
            }
            tmp += "\r\n\t......\r\n";
            act(0, tmp);
            //Parallel.For(0, ListK.Count, (i) =>
            //{
            Dictionary<string, List<int>> dic = new Dictionary<string, List<int>>();
            Dictionary<string, int> dicTotalTmp = new Dictionary<string, int>();
            foreach (KeyValuePair<string, Dictionary<int, List<int>>> item in dicKtuple)
            {
                dic.Add(item.Key, item.Value[k]);
                dicTotalTmp.Add(item.Key, dicTotal[item.Key][k]);
            }
            DissimiliratyEuMaChD2 d = new DissimiliratyEuMaChD2(dic, dicTotalTmp, savePath, listFunName, k);
            d.GetDissimiliratyMatrix(b);
            act(listFunName.Count, "");
            //});

            sp.Stop();
            act(0, "\tk = " + k + " 计算完毕，用时 " + sp.ElapsedMilliseconds * 1.0 / 1000 + " s\r\n\r\n");
        }
        #endregion

        #region Markov
        private void GetMarkovHao(int k)
        {
            act(0, "\r\n\t正在计算MarkovHao\r\n\t......\r\n");
            Stopwatch sp = new Stopwatch();
            sp.Start();

            foreach (KeyValuePair<string, Dictionary<int, List<int>>> item in dicKtuple)
            {
                MarkovPossibilityHao m = new MarkovPossibilityHao(item.Value, dicTotal[item.Key]);
                //dicMarkovHao.Add(item.Key, new Dictionary<int, List<double>>());
                //Parallel.For(0, ListK.Count, (i) =>
                //{
                //if (k > 2)
                //{
                Dictionary<int, List<double>> dic = dicMarkovHao[item.Key];
                //dicMarkovHao[item.Key].Add(k, m.GetMarkovPossibility(k));
                dic.Add(k, m.GetMarkovPossibility(k));
                act(1, "");
                //}//if
                //});
            }//foreach

            sp.Stop();
            act(0, "\tk = " + k + " 计算完毕，用时 " + sp.ElapsedMilliseconds * 1.0 / 1000 + " s\r\n\r\n");
        }

        private void GetMarkovHaoReady()
        {

            foreach (KeyValuePair<string, Dictionary<int, List<int>>> item in dicKtuple)
            {
                dicMarkovHao.Add(item.Key, new Dictionary<int, List<double>>());
            }//foreach
        }

        private void GetMarkovZeroToTwo(int k)
        {
            act(0, "\r\n\t正在计算Markov\r\n\t......\r\n");
            Stopwatch sp = new Stopwatch();
            sp.Start();

            foreach (KeyValuePair<string, Dictionary<int, List<int>>> item in dicKtuple)
            {
                MarkovPossibilityZeroToThree m = new MarkovPossibilityZeroToThree(item.Value, dicTotal[item.Key], ListM);
                //dicMarkov.Add(item.Key, new Dictionary<int, Dictionary<int, List<double>>>());
                //Parallel.For(0, ListK.Count, (i) =>
                //{
                //if (k > 2)
                //{
                dicMarkov[item.Key].Add(k, m.GetMarkovPossibility(k));
                act(listM.Count, "");
                //}//if
                //});
            }//foreach

            sp.Stop();
            act(0, "\tk = " + k + " 计算完毕，用时 " + sp.ElapsedMilliseconds * 1.0 / 1000 + " s\r\n\r\n");
        }

        private void GetMarkovZeroToTwoReady()
        {

            foreach (KeyValuePair<string, Dictionary<int, List<int>>> item in dicKtuple)
            {
                dicMarkov.Add(item.Key, new Dictionary<int, Dictionary<int, List<double>>>());
            }//foreach
        }
        #endregion

        #region D2S,D2Star
        private void GetD2SD2Star(string savePath, int k, List<string> listFunName, bool b = false)
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();

            string tmp = "\r\n\t正在计算";
            foreach (var item in listFunName)
            {
                tmp += item + "  ";
            }
            tmp += "\r\n\t......\r\n";
            act(0, tmp);
            //Parallel.For(0, ListK.Count, (i) =>
            //{
            //    if (ListK[i] > 2)
            //    {
            Dictionary<string, List<int>> dic = new Dictionary<string, List<int>>();
            Dictionary<string, int> dicTotalTmp = new Dictionary<string, int>();
            Dictionary<string, Dictionary<int, List<double>>> dicMarkovTmp = new Dictionary<string, Dictionary<int, List<double>>>();
            foreach (KeyValuePair<string, Dictionary<int, List<int>>> item in dicKtuple)
            {
                dic.Add(item.Key, item.Value[k]);
                dicTotalTmp.Add(item.Key, dicTotal[item.Key][k]);
                dicMarkovTmp.Add(item.Key, dicMarkov[item.Key][k]);
            }
            DissimiliratyD2SD2Star d = new DissimiliratyD2SD2Star(dic, dicMarkovTmp, dicTotalTmp, savePath, listFunName, ListM, k);
            d.GetDissimiliratyMatrix(b);
            act(listM.Count * listFunName.Count, "");
            //    }//if
            //});

            sp.Stop();
            act(0, "\tk = " + k + " 计算完毕，用时 " + sp.ElapsedMilliseconds * 1.0 / 1000 + " s\r\n\r\n");
        }
        #endregion

        #region Hao
        private void GetHao(string savePath, int k, bool b = false)
        {
            act(0, "\r\n\t正在计算Hao\r\n\t......\r\n");
            Stopwatch sp = new Stopwatch();
            sp.Start();
            //Parallel.For(0, ListK.Count, (i) =>
            //{
            //if (k > 2)
            //{
            Dictionary<string, List<int>> dic = new Dictionary<string, List<int>>();
            Dictionary<string, int> dicTotalTmp = new Dictionary<string, int>();
            Dictionary<string, List<double>> dicHao = new Dictionary<string, List<double>>();
            foreach (KeyValuePair<string, Dictionary<int, List<int>>> item in dicKtuple)
            {
                dic.Add(item.Key, item.Value[k]);
                dicTotalTmp.Add(item.Key, dicTotal[item.Key][k]);
                dicHao.Add(item.Key, dicMarkovHao[item.Key][k]);
            }
            //Dictionary<string, List<int>> dic, Dictionary<string, Dictionary<int, int>> dicTotal,
            DissimiliratyHao d = new DissimiliratyHao(dic, dicHao, dicTotalTmp, savePath, k);
            d.GetDissimiliratyMatrix(b);
            act(1, "");
            //}
            //});
            sp.Stop();
            act(0, "\tk = " + k + " 计算完毕，用时 " + sp.ElapsedMilliseconds * 1.0 / 1000 + " s\r\n\r\n");
        }
        #endregion

        private List<string> GetPathList(TreeNode node, string oneSeqPath = "")
        {
            List<string> list = new List<string>();
            if (oneSeqPath != "")
            {
                list.Add(oneSeqPath);
                string seqName = Path.GetFileNameWithoutExtension(oneSeqPath);
                dicTotal.Add(seqName, new Dictionary<int, int>());
                dicKtuple.Add(seqName, new Dictionary<int, List<int>>());
            }
            foreach (TreeNode item in node.Nodes)
            {
                string tmp = (string)item.Tag;
                list.Add(tmp);
                string seqName = Path.GetFileNameWithoutExtension(tmp);
                dicTotal.Add(seqName, new Dictionary<int, int>());
                dicKtuple.Add(seqName, new Dictionary<int, List<int>>());
            }
            return list;
        }

        private void DictionaryRemove(int k, bool bHao)
        {
            foreach (string item in dicKtuple.Keys)
            {
                if (bHao)
                {
                    if (k - 2 > 3)
                    {
                        dicKtuple[item].Remove(k - 2);
                        dicMarkovHao[item].Remove(k - 2);
                        dicTotal[item].Remove(k - 2);
                    }
                }
                else
                {
                    dicKtuple[item].Remove(k);
                    dicTotal[item].Remove(k);
                }
                if (listFun.Contains("D2S") || listFun.Contains("D2Star"))
                {
                    dicMarkov[item].Remove(k);
                }
            }
        }

    }
}
