using System;
using System.Collections.Generic;


namespace tset
{
    static class Operation
    {
        #region rubbis
        ///// <summary>
        ///// 添加节点到TreeView
        ///// </summary>
        ///// <param name="tree"></param>
        ///// <param name="nodes"></param>
        ///// <param name="dic"></param>
        ///// <param name="filePath"></param>
        //static private void AddDateToTreeView(TreeView tree, TreeNodeCollection nodes, Dictionary<string, string> dic, string[] filePath)
        //{
        //    Action actDelegate = () =>
        //    {
        //        foreach (var item in filePath)
        //        {
        //            string dirName = GetDirNameFromPath(item);
        //            string fileName = Path.GetFileNameWithoutExtension(item);

        //            //文件夹节点已经存在并且文件节点也存在
        //            TreeNode node = CheckNodeExist(nodes, dirName);
        //            if (node != null && CheckNodeExist(node.Nodes, fileName) != null)
        //                continue;

        //            //文件夹节点不存在
        //            if (node == null)
        //            {
        //                node = new TreeNode(dirName);
        //                nodes.Add(node);
        //            }
        //            node.Nodes.Add(fileName);
        //            dic.Add(fileName, item);
        //        }
        //    };
        //    tree.BeginInvoke(actDelegate);
        //}

        ///// <summary>
        ///// 从文件路径获取上级文件夹的路径
        ///// </summary>
        ///// <param name="path">文件路径</param>
        ///// <returns>返回文件上级文件夹的名字</returns>
        //static private string GetDirNameFromPath(string path)
        //{
        //    string[] tmps = path.Split('\\');
        //    return tmps[tmps.Length - 2];
        //}

        ///// <summary>
        ///// 添加文件或者文件夹
        ///// </summary>
        ///// <param name="tree"></param>
        ///// <param name="nodes"></param>
        ///// <param name="dic"></param>
        //static public void AddFileAndDirToTreeView(TreeView tree, TreeNodeCollection nodes, Dictionary<string, Dictionary<string, string>> dicDir, string path)
        //{
        //    GetAllFile(tree, nodes, dicDir, path);
        //}

        ///// <summary>
        ///// 添加文件或者文件夹
        ///// </summary>
        ///// <param name="tree"></param>
        ///// <param name="nodes"></param>
        ///// <param name="dic"></param>
        //static public void AddFileAndDirToTreeView(TreeView tree, TreeNodeCollection nodes, Dictionary<string, Dictionary<string, string>> dicDir)
        //{
        //    FolderBrowserDialog fbd = new FolderBrowserDialog();
        //    fbd.ShowDialog();
        //    if (fbd.SelectedPath != "")
        //        GetAllFile(tree, tree.Nodes, dicDir, fbd.SelectedPath);
        //}

        ///// <summary>
        ///// 移除节点操作
        ///// </summary>
        ///// <param name="tree"></param>
        ///// <param name="selectedNode"></param>
        ///// <param name="dic"></param>
        //static public void RemoveNode(TreeView tree, TreeNode selectedNode, Dictionary<string, Dictionary<string, string>> dicDir)
        //{
        //    if (selectedNode == null)
        //        return;
        //    Dictionary<string, string> dicTmp = null;
        //    if (selectedNode.Nodes.Count != 0)
        //    {
        //        dicTmp = dicDir[selectedNode.Text];
        //        dicDir.Remove(selectedNode.Text);
        //        foreach (TreeNode item in selectedNode.Nodes)
        //        {
        //            dicTmp.Remove(item.Text);
        //        }
        //        tree.Nodes.Remove(selectedNode);
        //    }
        //    else
        //    {
        //        dicTmp = dicDir[selectedNode.Parent.Text];
        //        dicTmp.Remove(selectedNode.Text);
        //        if (selectedNode.Parent.Nodes.Count == 1)
        //        {
        //            tree.Nodes.Remove(selectedNode.Parent);
        //            dicDir.Remove(selectedNode.Parent.Text);
        //        }
        //        selectedNode.Parent.Nodes.Remove(selectedNode);
        //    }
        //}

        ///// <summary>
        ///// 拖拽文件或者文件夹操作
        ///// </summary>
        ///// <param name="tree"></param>
        ///// <param name="nodes"></param>
        ///// <param name="dic"></param>
        ///// <param name="arr"></param>
        //static public void DragFileOrDir(TreeView tree, TreeNodeCollection nodes, Dictionary<string, Dictionary<string, string>> dicDir, Array arr)
        //{
        //    int length = arr.Length;
        //    string[] tmps = new string[length];
        //    for (int i = 0; i < length; i++)
        //    {
        //        tmps[i] = arr.GetValue(i).ToString();
        //    }
        //    GetAllFile(tree, nodes, dicDir, tmps);
        //}

        ///// <summary>
        ///// 预览文件
        ///// </summary>
        ///// <param name="selectedNode"></param>
        ///// <param name="dicDir"></param>
        //static public void Preview(TreeNode selectedNode, Dictionary<string, Dictionary<string, string>> dicDir)
        //{
        //    if (selectedNode == null)
        //        return;
        //    if (selectedNode.Nodes.Count != 0)
        //    {
        //        return;
        //    }
        //    Process.Start(dicDir[selectedNode.Parent.Text][selectedNode.Text]);
        //}

        ///// <summary>
        ///// 在资源管理中打开
        ///// </summary>
        ///// <param name="selectedNode"></param>
        ///// <param name="dicDir"></param>
        //static public void ViewInExplorer(TreeNode selectedNode, Dictionary<string, Dictionary<string, string>> dicDir)
        //{
        //    if (selectedNode == null)
        //        return;
        //    string path;
        //    if (selectedNode.Nodes.Count != 0)
        //    {
        //        path = Path.GetDirectoryName(dicDir[selectedNode.Text][selectedNode.Nodes[0].Text]);
        //        if (selectedNode.Parent != null)
        //            path = Path.GetDirectoryName(path);
        //    }
        //    else
        //    {
        //        path = Path.GetDirectoryName(dicDir[selectedNode.Parent.Text][selectedNode.Text]);
        //    }
        //    Process.Start(path);
        //}

        ///// <summary>
        ///// 递归获取所有文件
        ///// </summary>
        ///// <param name="tree"></param>
        ///// <param name="nodes"></param>
        ///// <param name="dicDir"></param>
        ///// <param name="path"></param>
        //static private void GetAllFile(TreeView tree, TreeNodeCollection nodes, Dictionary<string, Dictionary<string, string>> dicDir, params string[] path)
        //{
        //    foreach (var item in path)
        //    {
        //        Dictionary<string, string> dic = new Dictionary<string, string>();

        //        //为文件夹的情况
        //        if (!File.Exists(item))
        //        {
        //            //获取所有的txt文件
        //            string[] tmps = Directory.GetFiles(item);
        //            //文件存在
        //            if (tmps.Length != 0)
        //            {
        //                //获取文件夹的名字作为键值
        //                string name = Path.GetFileName(item);
        //                //没有重复，则添加
        //                if (!dicDir.ContainsKey(name))
        //                {
        //                    dicDir.Add(name, dic);
        //                }
        //                else
        //                {
        //                    dic = dicDir[name];
        //                }
        //                AddDateToTreeView(tree, nodes, dic, tmps);
        //            }
        //            //还存在文件夹，递归访问
        //            else
        //            {
        //                tmps = Directory.GetDirectories(item);
        //                GetAllFile(tree, nodes, dicDir, tmps);
        //            }
        //        }

        //        //为文件的情况
        //        else
        //        {
        //            //获得文件夹名字
        //            string name = GetDirNameFromPath(item);
        //            string tmp =Path.GetDirectoryName(item);
        //            if (!dicDir.ContainsKey(name))
        //            {
        //                dicDir.Add(name, dic);
        //                //获得序列名字(不含后缀)
        //                GetAllFile(tree, nodes, dicDir, tmp);
        //            }//if
        //        }//else
        //    }//foreach        
        //}

        ///// <summary>
        ///// 检查节点是否已经存在
        ///// </summary>
        ///// <param name="node"></param>
        ///// <param name="name"></param>
        ///// <returns>不存在返回null，存在则返回该节点</returns>
        //static private TreeNode CheckNodeExist(TreeNodeCollection nodes, string name)
        //{
        //    foreach (TreeNode item in nodes)
        //    {
        //        if (item.Text == name)
        //            return item;
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// 显示状态
        ///// </summary>
        ///// <param name="pgb"></param>
        ///// <param name="txt"></param>
        ///// <param name="info"></param>
        //static public void ShowState(ProgressBar pgb, TextBox txt, string info = null)
        //{
        //    Action pgbDelegate = () =>
        //    {
        //        pgb.Value++;
        //    };

        //    pgb.BeginInvoke(pgbDelegate);
        //    if (info != null)
        //    {
        //        Action txtDelegate = () =>
        //        {
        //            txt.Text += info + "\tCompleted!" + "\r\n";
        //        };
        //        txt.BeginInvoke(txtDelegate);
        //    }
        //}

        ///// <summary>
        ///// 参数检查
        ///// </summary>
        ///// <param name="tre"></param>
        ///// <param name="cbx1"></param>
        ///// <param name="cbx2"></param>
        ///// <param name="txtSave"></param>
        ///// <returns></returns>
        //static public bool CheckSetting(TreeView tre, ComboBox cbx1, ComboBox cbx2, TextBox txtSave, bool bSingle)
        //{
        //    if (tre.Nodes.Count == 0)
        //    {
        //        MessageBox.Show("Please load the sequence files!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return false;
        //    }

        //    int tmp = 0;
        //    if (int.TryParse(cbx1.SelectedItem.ToString(), out tmp))
        //    {
        //        if (!bSingle)
        //        {
        //            if (cbx2.SelectedItem == null)
        //            {
        //                MessageBox.Show("Please select the value of k", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                return false;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please select the value of k", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return false;
        //    }
        //    if (txtSave.Text == "")
        //        txtSave.Text = Environment.CurrentDirectory;

        //    return true;
        //}

        ///// <summary>
        ///// 单k值设定响应
        ///// </summary>
        ///// <param name="lbl"></param>
        ///// <param name="cbx"></param>
        ///// <param name="b"></param>
        //static public void SingleKSet(Label lbl, ComboBox cbx, bool b)
        //{
        //    if (b)
        //        lbl.Visible = cbx.Visible = false;

        //    else
        //        lbl.Visible = cbx.Visible = true;
        //}

        ///// <summary>
        ///// k2设定
        ///// </summary>
        ///// <param name="cbx"></param>
        ///// <param name="min"></param>
        //static public void ComboBoxSet(ComboBox cbx, int min)
        //{
        //    cbx.Items.Clear();
        //    for (int i = min; i < 11; i++)
        //    {
        //        cbx.Items.Add(i);
        //    }
        //}

        ///// <summary>
        ///// 获取参数
        ///// </summary>
        ///// <param name="cbxk1">k1的值</param>
        ///// <param name="cbxk2">k2的值</param>
        ///// <param name="bSingle">是否单k值</param>
        ///// <param name="pgb">进度条</param>
        ///// <param name="n">文件总数</param>
        ///// <param name="k1">输出k1</param>
        ///// <param name="k2">输出k2</param>
        //static public void GetParameter(ComboBox cbxk1, ComboBox cbxk2, bool bSingle, ProgressBar pgb, int n, out int k1, out int k2)
        //{
        //    k1 = int.Parse(cbxk1.SelectedItem.ToString());
        //    k2 = 0;
        //    if (bSingle)
        //        k2 = k1;
        //    else
        //        k2 = int.Parse(cbxk2.SelectedItem.ToString());

        //    pgb.Maximum = (k2 - k1 + 1) * n;
        //}

        ///// <summary>
        ///// 初始化状态信息
        ///// </summary>
        ///// <param name="pgb"></param>
        ///// <param name="tbx"></param>
        //static public void InitalizeState(ProgressBar pgb, TextBox tbx)
        //{
        //    pgb.Value = 0;
        //    tbx.Clear();
        //}

        ///// <summary>
        ///// 获取选中的计算方式
        ///// </summary>
        ///// <param name="cbxs"></param>
        ///// <returns></returns>
        //static public string[] GetFunctionNames(params CheckBox[] cbxs)
        //{
        //    List<string> list = new List<string>();
        //    foreach (var item in cbxs)
        //    {
        //        if (item.Checked)
        //            list.Add(item.Text);
        //    }
        //    return list.ToArray();
        //}

        ///// <summary>
        ///// 获取参数（Dissimiliraty）
        ///// </summary>
        ///// <param name="cbxk1"></param>
        ///// <param name="cbxk2"></param>
        ///// <param name="cbxPrecision"></param>
        ///// <param name="bSingle"></param>
        ///// <param name="pgb"></param>
        ///// <param name="k1"></param>
        ///// <param name="k2"></param>
        ///// <param name="precision"></param>
        ///// <param name="dicPathK"></param>
        ///// <param name="Count"></param>
        //static public void GetParameter(ComboBox cbxk1, ComboBox cbxk2, ComboBox cbxPrecision, bool bSingle, ProgressBar pgb, out int k1, out int k2, out int precision, Dictionary<string, Dictionary<string, string>> dicPathK, int Count)
        //{
        //    int n = (Count - 1) * Count / 2 + 1;
        //    GetParameter(cbxk1, cbxk2, bSingle, pgb, n, out k1, out k2);
        //    precision = 5;
        //    if (cbxPrecision.SelectedItem != null)
        //        precision = int.Parse(cbxPrecision.SelectedItem.ToString());

        //    for (int i = 3; i < k1; i++)
        //    {
        //        if (dicPathK.ContainsKey("k" + i))
        //            dicPathK.Remove("k" + i);
        //    }
        //    for (int i = k2 + 1; i <= Count; i++)
        //    {
        //        if (dicPathK.ContainsKey("k" + i))
        //            dicPathK.Remove("k" + i);
        //    }
        //}



        ///// <summary>
        ///// 获得Markov阶数链表
        ///// </summary>
        ///// <param name="chbs"></param>
        ///// <returns></returns>
        //static public List<int> GetMarkovOrderList(params CheckBox[] chbs)
        //{
        //    List<int> list = new List<int>();
        //    foreach (var item in chbs)
        //    {
        //        if (item.Checked)
        //        {
        //            list.Add(int.Parse(item.Text[1].ToString()));
        //        }
        //    }
        //    return list;
        //}


        #endregion

        /// <summary>
        /// 将ktuple转化为十进制
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public int ConvertDate(string str)
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

        /// <summary>
        /// 获取4进制
        /// </summary>
        /// <param name="n"></param>
        /// <param name="mod"></param>
        /// <returns></returns>
        static public List<int> Get4Bits(int temp,int k)
        {
            List<int> list = new List<int>();
            int index = 0;
            for (int i = 0; i < k-1; i++)
            {
                index = temp % 4;
                temp = temp / 4;
                list.Add(index);
            }
            list.Add(temp);
            list.Reverse();
            return list;
        }

        /// <summary>
        /// 获取10进制
        /// </summary>
        /// <param name="list"></param>
        /// <param name="r"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        static public int Get10Bits(List<int>list,int r,int length)
        {
            int result = 0;
            for (int i = 0; i < length; i++)
            {
                result += (int)(list[i + r] * Math.Pow(4, length - i - 1));
            }
            return result;
        }

    }
}
