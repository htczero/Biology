using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace d2_toolsPro
{
    class Operation
    {
        #region 私有变量及其属性
        protected Dictionary<string, TreeView> dicTreeView = new Dictionary<string, TreeView>();
        protected Dictionary<string, ComboBox> dicComboBox = new Dictionary<string, ComboBox>();
        protected TextBox txtState;
        protected TextBox txtSave;
        protected Label lblTo;
        protected ProgressBar pgb;
        protected RadioButton rbnSingle;
        protected string loadPath;
        protected string savePath;
        protected int k1;
        protected int k2;

        public Dictionary<string, TreeView> DicTreeView
        {
            get
            {
                return dicTreeView;
            }
        }

        public Dictionary<string, ComboBox> DicComboBox
        {
            get
            {
                return dicComboBox;
            }
        }

        public TextBox TxtState
        {
            get
            {
                return txtState;
            }
        }

        public TextBox TxtSave
        {
            get
            {
                return txtSave;
            }           
        }

        public Label LblTo
        {
            get
            {
                return lblTo;
            }
        }

        public ProgressBar Pgb
        {
            get
            {
                return pgb;
            }
        }

        public string LoadPath
        {
            get
            {
                return loadPath;
            }
        }

        public string SavePath
        {
            get
            {
                return savePath;
            }
        }

        public int K1
        {
            get
            {
                return k1;
            }
        }

        public int K2
        {
            get
            {
                return k2;
            }
        }

        public Operation(Dictionary<string, TreeView> dicTreeView, Dictionary<string, ComboBox> dicComboBox, TextBox txtState, TextBox txtSave, Label lblTo, ProgressBar pgb, RadioButton rbnSingle)
        {
            this.dicTreeView = dicTreeView;
            this.dicComboBox = dicComboBox;
            this.txtState = txtState;
            this.txtSave = txtSave;
            this.lblTo = lblTo;
            this.pgb = pgb;
            this.rbnSingle = rbnSingle;
        }

        #endregion

        /// <summary>
        /// 移除节点操作
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="selectedNode"></param>
        /// <param name="dic"></param>
        public void RemoveNode()
        {
            TreeNode selectedNode = DicTreeView["treLoad"].SelectedNode;
            if (selectedNode == null)
                return;

            selectedNode.Remove();
        }

        /// <summary>
        /// 拖拽文件或者文件夹操作
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="nodes"></param>
        /// <param name="dic"></param>
        /// <param name="arr"></param>
        public virtual void DragFileOrDir(TreeView tre, string arr)
        {
            loadPath = arr;
            string[] tmps = Directory.GetDirectories(arr);
            if (tmps.Length == 0)
            {
                tmps = new string[] { arr };
                loadPath = Path.GetDirectoryName(arr);
            }
            AddToTreeView(tre, tre.Nodes, loadPath, tmps);
        }

        /// <summary>
        /// 预览文件
        /// </summary>
        /// <param name="selectedNode"></param>
        /// <param name="dicDir"></param>
        public void Preview(bool bExplorer = false)
        {
            TreeNode selectedNode = null;
            string path = loadPath;
            foreach (KeyValuePair<string, TreeView> item in DicTreeView)
            {
                if (item.Value.SelectedNode != null)
                {
                    selectedNode = item.Value.SelectedNode;
                    if (item.Key == "treSave")
                    {
                        path = SavePath;
                    }
                    break;
                }
            }
            if (selectedNode == null)
                return;

            path = path + "\\" + selectedNode.FullPath;
            if (bExplorer)
                path = Path.GetDirectoryName(path);

            Process.Start(path);
        }

        /// <summary>
        /// 递归添加到TreeView中
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="nodes"></param>
        /// <param name="arrPath"></param>
        public void AddToTreeView(TreeView tre, TreeNodeCollection nodes, string str, params string[] arrPath)
        {
            foreach (string path in arrPath)
            {
                Action act = (() =>
                {
                    //定义添加文件夹节点     
                    TreeNode node = new TreeNode(Path.GetFileName(path));

                    if (!File.Exists(path))
                    {
                        string[] tmps = Directory.GetFiles(path);
                        if (tmps.Length != 0)  //当前路径下文件夹存在文件
                        {
                            nodes.Add(node);
                            foreach (string filePath in tmps)
                            {
                                string fileName = Path.GetFileName(filePath);
                                node.Nodes.Add(fileName);
                            }
                        }
                        else     //当前路径下文件夹存在文件夹
                        {
                            tmps = Directory.GetDirectories(path);
                            if (tmps.Length == 1)
                            {
                                str = str + "\\" + path;
                                AddToTreeView(tre, nodes, str, tmps);
                            }
                            else
                            {
                                nodes.Add(node);
                                AddToTreeView(tre, node.Nodes, str, tmps);
                            }
                        }
                    }
                    else
                    {
                        nodes.Add(node);
                    }
                });
                tre.BeginInvoke(act);
            }
        }

        /// <summary>
        /// 显示状态
        /// </summary>
        /// <param name="pgb"></param>
        /// <param name="txt"></param>
        /// <param name="info"></param>
        public void ShowState(string info = null)
        {
            Action pgbDelegate = () =>
            {
                Pgb.Value++;
            };

            Pgb.BeginInvoke(pgbDelegate);
            if (info != null)
            {
                Action txtDelegate = () =>
                {
                    TxtState.Text += info + "\tCompleted!" + "\r\n";
                };
                TxtState.BeginInvoke(txtDelegate);
            }
        }

        /// <summary>
        /// 参数检查
        /// </summary>
        /// <param name="tre"></param>
        /// <param name="cbx1"></param>
        /// <param name="cbx2"></param>
        /// <param name="txtSave"></param>
        /// <returns></returns>
        protected bool CheckSetting(bool bSingle)
        {
            TreeView tre = DicTreeView["treLoad"];
            ComboBox cbx1 = DicComboBox["cbxk1"];
            ComboBox cbx2 = DicComboBox["cbxk2"];
            if (tre.Nodes.Count == 0)
            {
                MessageBox.Show("Please load the sequence files!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int tmp = 0;
            if (int.TryParse(cbx1.SelectedItem.ToString(), out tmp))
            {
                if (!bSingle)
                {
                    if (cbx2.SelectedItem == null)
                    {
                        MessageBox.Show("Please select the value of k", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select the value of k", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txtSave.Text == "")
            {
                MessageBox.Show("Please select the path to save the files", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 单k值设定响应
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="cbx"></param>
        /// <param name="b"></param>
        public void SingleKSet(bool b)
        {
            LblTo.Visible = DicComboBox["cbxk2"].Visible = !b;
        }

        /// <summary>
        /// k2设定
        /// </summary>
        /// <param name="cbx"></param>
        /// <param name="min"></param>
        public void ComboBoxSet(int min)
        {
            ComboBox cbx = DicComboBox["cbxk2"];
            cbx.Items.Clear();
            for (int i = min + 1; i < 11; i++)
            {
                cbx.Items.Add(i);
            }
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="bSingle">是否单k值</param>
        /// <param name="n">文件数量</param>
        /// <param name="k1">k1的值</param>
        /// <param name="k2">k2的值</param>
        /// <param name="bKtuple">是否是Ktuple的参数</param>
        /// <returns></returns>
        protected void GetParameter(bool bSingle, int n)
        {
            ComboBox cbx = DicComboBox["cbxk1"];
            k1 = int.Parse(cbx.SelectedItem.ToString());
            k2 = 0;
            if (bSingle)
                k2 = k1;
            else
            {
                cbx = DicComboBox["cbxk2"];
                k2 = int.Parse(cbx.SelectedItem.ToString());
            }

            Pgb.Maximum = (k2 - k1 + 1) * n;
        }

        /// <summary>
        /// 初始化状态信息
        /// </summary>
        /// <param name="pgb"></param>
        /// <param name="tbx"></param>
        protected void InitalizeState()
        {
            Pgb.Value = 0;
            TxtState.Clear();
            DicTreeView["treSave"].Nodes.Clear();
        }

        /// <summary>
        /// 检查数量和k值是否匹配
        /// </summary>
        /// <param name="k1"></param>
        /// <param name="k2"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        protected virtual bool CheckFileCount(Dictionary<string, string[]> dicLoad)
        {
            //List<int> list = new List<int>();
            //foreach (string[] item in dicLoad.Values)
            //{
               
            //    foreach (string path in item)
            //    {
            //        string[] dir = Directory.GetFiles(path);
            //        foreach (string file in dir)
            //        {
            //            list.Add(GetKValue(file));
            //        }
            //        for (int i = k1; i < k2 + 1; i++)
            //        {
            //            if (!list.Contains(i))
            //            {
            //                MessageBox.Show("The number of inputed files does not math k!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //                return false;
            //            }
            //        }
            //        list.Clear();
            //    }
            //}
            return true;
        }

        /// <summary>
        /// 获取文件k值
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected int GetKValue(string path)
        {
            string key = Path.GetFileNameWithoutExtension(path);
            string[] tmps = key.Split(new string[] { "_k" }, StringSplitOptions.RemoveEmptyEntries);
            string tmp = tmps[tmps.Length - 1].Split('_')[0];
            return int.Parse(tmp);
        }

        protected virtual bool Prepare(bool bSingle, Dictionary<string, string[]> dic)
        {
            return true;
        }

        protected void GetPathOfK(Dictionary<int, Dictionary<string, string>> dicPathK, string[] tmps)
        {
            foreach (string item in tmps)
            {
                string key = Path.GetFileName(item);
                string[] files = Directory.GetFiles(item);
                foreach (string path in files)
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    int k = GetKValue(path);
                    if (k >= k1 && k <= k2)
                    {
                        if (dicPathK.ContainsKey(k))
                            dic = dicPathK[k];

                        else
                        {
                            dicPathK.Add(k, dic);
                        }
                        dic.Add(key, path);
                    }
                }//foreach
            }//foreach
        }

        public virtual void StarCalculation()
        {

        }

        protected virtual int GetDicPath(Dictionary<string, string[]> dic)
        {
            int fileCount = 0;
            foreach (TreeNode node in DicTreeView["treLoad"].Nodes)
            {
                string[] tmps = Directory.GetFiles(LoadPath + "\\" + node.FullPath);
                if (tmps.Length != 0)
                {
                    dic.Add("", Directory.GetDirectories(loadPath));
                    fileCount += DicTreeView["treLoad"].Nodes.Count;
                    break;
                }

                tmps = Directory.GetDirectories(LoadPath + "\\" + node.FullPath);
                fileCount += tmps.Length;
                dic.Add(node.Text, tmps);
            }
            if (Form1.spPath != null)
            {
                List<string> list = new List<string>();
                Dictionary<string, string[]> dicTmp = new Dictionary<string, string[]>();
                foreach (KeyValuePair<string, string[]> item in dic)
                {
                    list.Add(Form1.spPath);
                    list.AddRange(item.Value);
                    dicTmp.Add(item.Key, list.ToArray());
                    list.Clear();
                }
                foreach (KeyValuePair<string, string[]> item in dicTmp)
                {
                    dic[item.Key] = item.Value;
                }
            }
            return fileCount;
        }

        protected Action<Dissimiliraty, int> GetCalMode()
        {
            Action<Dissimiliraty, int> act = null;
            if (Form1.spPath != null)
            {
                act = (Dissimiliraty d, int k) =>
                {
                    d.GetDissimiliratyOneToN(k);
                };
            }
            else
            {
                act = (Dissimiliraty d, int k) =>
                {
                    d.GetDissimiliratyMatrix(k);
                };
            }
            return act;
        }

    }
}
