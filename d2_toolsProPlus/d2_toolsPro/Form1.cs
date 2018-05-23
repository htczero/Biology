using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace d2_toolsPro
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static public bool _bRun = false;
        static public string spPath = null;
        static public Form2 frm = null;
        private List<Operation> listOperation = new List<Operation>();
        static public Form1 frm1;

        #region 事件
        /// <summary>
        /// 窗体载入的时候
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            frm1 = this;
            Dictionary<string, TreeView> dicTreeView = new Dictionary<string, TreeView>();
            Dictionary<string, CheckBox> dicCheckBox = new Dictionary<string, CheckBox>();
            Dictionary<string, ComboBox> dicComboBox = new Dictionary<string, ComboBox>();
            List<CheckBox> listCbx = new List<CheckBox>();
            List<CheckBox> listR = new List<CheckBox>();

            #region ktuple
            dicTreeView.Add("treLoad", treLoadT);
            dicTreeView.Add("treSave", treSaveT);
            dicComboBox.Add("cbxk1", cbxk1T);
            dicComboBox.Add("cbxk2", cbxk2T);
            listOperation.Add(new OperationKtuple(dicTreeView, dicComboBox, txtStateT, txtSaveT, lblToT, pgbT, rbnSingleT));
            #endregion

            #region Markov
            dicTreeView = new Dictionary<string, TreeView>();
            dicComboBox = new Dictionary<string, ComboBox>();
            listCbx.Add(chbHM);
            listCbx.Add(chbZM);
            dicTreeView.Add("treLoad", treLoadM);
            dicTreeView.Add("treSave", treSaveM);
            dicComboBox.Add("cbxk1", cbxk1M);
            dicComboBox.Add("cbxk2", cbxk2M);
            listOperation.Add(new OperationMarkov(dicTreeView, dicComboBox, txtStateM, txtSaveM, lblToM, pgbM, listCbx, rbnSingleM));
            #endregion

            #region Hao
            dicTreeView = new Dictionary<string, TreeView>();
            dicComboBox = new Dictionary<string, ComboBox>();
            dicTreeView.Add("treLoad", treLoadDH);
            dicTreeView.Add("treSave", treSaveDH);
            dicComboBox.Add("cbxk1", cbxk1DH);
            dicComboBox.Add("cbxk2", cbxk2DH);
            dicComboBox.Add("precision", cbxPrecisionDH);
            listOperation.Add(new OperationHao(dicTreeView, dicComboBox, txtStateDH, txtSaveDH, lblToDH, pgbDH, rbnSingleDH));
            #endregion

            #region EuMaChD2
            dicTreeView = new Dictionary<string, TreeView>();
            dicComboBox = new Dictionary<string, ComboBox>();
            dicCheckBox = new Dictionary<string, CheckBox>();
            listCbx = new List<CheckBox>();
            listCbx.Add(chbEuDE);
            listCbx.Add(chbMaDE);
            listCbx.Add(chbChDE);
            listCbx.Add(chbD2DE);

            dicTreeView.Add("treLoad", treLoadDE);
            dicTreeView.Add("treSave", treSaveDE);
            dicComboBox.Add("cbxk1", cbxk1DE);
            dicComboBox.Add("cbxk2", cbxk2DE);
            dicComboBox.Add("precision", cbxPrecisionDE);
            listOperation.Add(new OperationEuMaChD2(dicTreeView, dicComboBox, txtStateDE, txtSaveDE, lblToDE, pgbDE, listCbx, rbnSingleDE));
            #endregion

            #region D2S_D2Star
            dicTreeView = new Dictionary<string, TreeView>();
            dicComboBox = new Dictionary<string, ComboBox>();
            listCbx = new List<CheckBox>();
            listCbx.Add(chbD2SDS);
            listCbx.Add(chbD2StarDS);
            listR.Add(chbM0DS);
            listR.Add(chbM1DS);
            listR.Add(chbM2DS);
            listR.Add(chbM3DS);
            dicTreeView.Add("treLoad", treLoadDS);
            dicTreeView.Add("treSave", treSaveDS);
            dicComboBox.Add("cbxk1", cbxk1DS);
            dicComboBox.Add("cbxk2", cbxk2DS);
            dicComboBox.Add("precision", cbxPrecisionDS);
            listOperation.Add(new OperationD2SD2Star(dicTreeView, dicComboBox, txtStateDS, txtSaveDS, lblToDS, pgbDS, listCbx, listR, rbnSingleDS));
            #endregion

            #region S2
            dicTreeView = new Dictionary<string, TreeView>();
            dicComboBox = new Dictionary<string, ComboBox>();
            listR = new List<CheckBox>();
            listR.Add(chbM0S2);
            listR.Add(chbM1S2);
            listR.Add(chbM2S2);
            listR.Add(chbM3S2);
            dicTreeView.Add("treLoad", treLoadS2);
            dicTreeView.Add("treSave", treSaveS2);
            dicComboBox.Add("cbxk1", cbxk1S2);
            dicComboBox.Add("cbxk2", cbxk2S2);
            dicComboBox.Add("precision", cbxPrecisionS2);
            listOperation.Add(new OperationS2(dicTreeView, dicComboBox, txtStateS2, txtSaveS2, lblToS2, pgbS2, listR, rbnSingleS2));
            #endregion

            #region DaiQi
            dicTreeView = new Dictionary<string, TreeView>();
            dicComboBox = new Dictionary<string, ComboBox>();
            listR = new List<CheckBox>();
            listR.Add(chbM0Da);
            listR.Add(chbM1Da);
            listR.Add(chbM2Da);
            listR.Add(chbM3Da);
            dicTreeView.Add("treLoad", treLoadDa);
            dicTreeView.Add("treSave", treSaveDa);
            dicComboBox.Add("cbxk1", cbxk1Da);
            dicComboBox.Add("cbxk2", cbxk2Da);
            dicComboBox.Add("precision", cbxPrecisionDa);
            listOperation.Add(new OperationDaiQi(dicTreeView, dicComboBox, txtStateDa, txtSaveDa, lblToDa, pgbDa, listR, rbnSingleDa));
            #endregion
        }

        /// <summary>
        /// k值选定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxk1T_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbx = (ComboBox)sender;
            int min = int.Parse(cbx.SelectedItem.ToString());
            listOperation[tabAll.SelectedIndex].ComboBoxSet(min);
        }

        /// <summary>
        /// 拖拽文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyDragEnter(object sender, DragEventArgs e)
        {
            if (_bRun)
                return;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link; //重要代码：表明是链接类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// 拖拽响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyDragDrop(object sender, DragEventArgs e)
        {
            Array arrFiles = ((Array)e.Data.GetData(DataFormats.FileDrop));
            if (arrFiles.Length != 1)
            {
                MessageBox.Show("The count of directory can not be more than one!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tmp = arrFiles.GetValue(0).ToString();
            if (File.Exists(tmp))
            {
                MessageBox.Show("Please import the directory!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (sender is TreeView)
            {
                Operation op = listOperation[tabAll.SelectedIndex];
                op.DragFileOrDir(op.DicTreeView["treLoad"], tmp);
            }
            else if (sender is TextBox)
            {
                TextBox txt = (TextBox)sender;
                txt.Text = tmp;
            }
        }

        /// <summary>
        /// 双击打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treLoadT_DoubleClick(object sender, EventArgs e)
        {
            listOperation[tabAll.SelectedIndex].Preview();
        }

        /// <summary>
        /// 单k值判定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbnSingleT_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbn = (RadioButton)sender;
            listOperation[tabAll.SelectedIndex].SingleKSet(rbn.Checked);
        }

        /// <summary>
        /// 保存路径选择按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveT_Click(object sender, EventArgs e)
        {
            if (_bRun)
                return;
            Button btn = (Button)sender;
            if (btn.Text == "Open")
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.ShowDialog();
                if (fbd.SelectedPath != null)
                {
                    listOperation[tabAll.SelectedIndex].TxtSave.Text = fbd.SelectedPath;
                }
            }
            else
            {
                _bRun = true;
                listOperation[tabAll.SelectedIndex].StarCalculation();
            }
        }

        #endregion

        #region Menue

        /// <summary>
        /// 导入文件或者文件夹事件点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_bRun)
                return;

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            if (fbd.SelectedPath != "")
            {
                Operation op = listOperation[tabAll.SelectedIndex];
                op.DragFileOrDir(op.DicTreeView["treLoad"], fbd.SelectedPath);
            }
        }

        /// <summary>
        /// 移除点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listOperation[tabAll.SelectedIndex].RemoveNode();
        }

        /// <summary>
        /// 预览点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ms = (ToolStripMenuItem)sender;
            bool bExplorer = true;
            if (ms.Text == "Preview")
            {
                bExplorer = false;
            }
            listOperation[tabAll.SelectedIndex].Preview(bExplorer);
        }

        /// <summary>
        /// 控件没有激活时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treLoadT_Leave(object sender, EventArgs e)
        {
            TreeView tre = (TreeView)sender;
            tre.SelectedNode = null;
        }

        #endregion

        #region kTuple



        #endregion


        #region Markov Possibility

        /// <summary>
        /// 获得Markov Possbility文件
        /// </summary>


        #endregion



        #region Dissimilraty
        //internal void GetPathOfK(Dictionary<string, Dictionary<string, string>> dicPathK, Dictionary<string, Dictionary<string, string>> dicLoad)
        //{
        //    char[] flag = { '_' };
        //    foreach (KeyValuePair<string, Dictionary<string, string>> item in dicLoad)
        //    {

        //        foreach (KeyValuePair<string, string> path in item.Value)
        //        {
        //            Dictionary<string, string> dic = new Dictionary<string, string>();

        //            string name = null;
        //            string kn = Operation.GetPathOfK(path.Value, out name);
        //            if (dicPathK.ContainsKey(kn))
        //            {
        //                dic = dicPathK[kn];
        //            }
        //            else
        //            {
        //                dicPathK.Add(kn, dic);
        //            }
        //            dic.Add(name, path.Value);
        //        }//foreach
        //    }//foreach
        //}

        //private void CalDissimility(string way, Dictionary<string, Dictionary<string, string>> dicPathK, int k1, int k2, int precision, string[] funName, List<int> list)
        //{
        //    Task task2 = new Task(() =>
        //    {
        //        Stopwatch sp = new Stopwatch();
        //        sp.Start();

        //        Parallel.For(k1, k2 + 1, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, (i) =>
        //            {
        //                //for (int i = k1; i < k2 + 1; i++)
        //                //{


        //                #region Hao
        //                if (way == "Hao")
        //                {
        //                    Action act = () =>
        //                    {
        //                        Operation.ShowState(pgbDH, txtStateDH);
        //                    };
        //                    DissimiliratyHao d = new DissimiliratyHao(dicPathK["k" + i], txtSaveDH.Text, i, precision);
        //                    d.GetDissimiliratyMatrix(act);
        //                    Operation.ShowState(pgbDH, txtStateDH, "k = " + i);
        //                    Operation.AddFileAndDirToTreeView(treSaveDH, treSaveDH.Nodes, _dicDirSaveDH, d.SaveDir + "\\Hao");
        //                }
        //                #endregion


        //                #region Eu Ma Ch D2
        //                else if (way == "EuMaChD2")
        //                {
        //                    Action act = () =>
        //                    {
        //                        Operation.ShowState(pgbDE, txtStateDE);
        //                    };
        //                    Dissimiliraty_Eu_Ma_Ch_D2 d = new Dissimiliraty_Eu_Ma_Ch_D2(dicPathK["k" + i], txtSaveDE.Text, i, precision);
        //                    d.GetDissimiliratyMatrix(act, funName);
        //                    Operation.ShowState(pgbDE, txtStateDE, "k = " + i);
        //                    Operation.AddFileAndDirToTreeView(treSaveDE, treSaveDE.Nodes, _dicDirSaveDE, d.SaveDir);
        //                }
        //                #endregion


        //                #region D2S DStar
        //                else if (way == "D2S" || way == "D2Star" || way == "All")
        //                {
        //                    Action act = () =>
        //                    {
        //                        Operation.ShowState(pgbDS, txtStateDS);
        //                    };
        //                    DissimiliratyD2SD2Star d = new DissimiliratyD2SD2Star(dicPathK["k" + i], txtSaveDS.Text, i, list, precision);
        //                    d.GetDissimiliratyMatrix(act, way);
        //                    Operation.ShowState(pgbDS, txtStateDS, "k = " + i);
        //                    Operation.AddFileAndDirToTreeView(treSaveDS, treSaveDS.Nodes, _dicDirSaveDS, d.SaveDir);
        //                }
        //                #endregion


        //                #region S2
        //                else if (way == "S2")
        //                {
        //                    Action act = () =>
        //                    {
        //                        Operation.ShowState(pgbS2, txtStateS2);
        //                    };
        //                    DissimiliratyS2 d = new DissimiliratyS2(dicPathK["k" + i], txtSaveS2.Text, i, list, precision);
        //                    d.GetDissimiliratyMatrix(act);
        //                    Operation.ShowState(pgbS2, txtStateS2, "k = " + i);
        //                    Operation.AddFileAndDirToTreeView(treSaveS2, treSaveS2.Nodes, _dicDirSaveS2, d.SaveDir + "\\S2");
        //                }
        //                #endregion
        //                //}
        //            });
        //        sp.Stop();
        //        MessageBox.Show("Mission Completed!\r\nTime : " + sp.ElapsedMilliseconds / 1000.0 + " sec", "Tip", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        _bRun = false;
        //    });
        //    task2.Start();
        //}

        //private void GetDissimility()
        //{
        //    #region public
        //    //key:k值
        //    Dictionary<string, Dictionary<string, string>> dicPathK = new Dictionary<string, Dictionary<string, string>>();
        //    int k1;
        //    int k2;
        //    int precision;
        //    #endregion

        //    #region Hao
        //    if (tabAll.SelectedTab.Text == "Dissimiliraty_Hao")
        //    {
        //        Operation.GetParameter(cbxk1DH, cbxk2DH, cbxPrecisionDH, rbnSingleDH.Checked, pgbDH, out k1, out k2, out precision, dicPathK, _dicDirLoadDH.Count);
        //        if (!Operation.CheckFileCount(k1, k2, _dicDirLoadDH))
        //        {
        //            _bRun = false;
        //            return;
        //        }
        //        Directory.CreateDirectory(txtSaveDH.Text + "\\Hao");
        //        Operation.InitalizeState(pgbDH, txtStateDH, treSaveDH);
        //        Operation.GetPathOfK(dicPathK, _dicDirLoadDH);
        //        CalDissimility("Hao", dicPathK, k1, k2, precision, null, null);
        //    }
        //    #endregion


        //    #region Eu Ma Ch D2
        //    else if (tabAll.SelectedTab.Text == "Dissimiliraty_Eu_Ma_Ch_D2")
        //    {
        //        Operation.GetParameter(cbxk1DE, cbxk2DE, cbxPrecisionDE, rbnSingleDE.Checked, pgbDE, out k1, out k2, out precision, dicPathK, _dicDirLoadDE.Count);
        //        if (!Operation.CheckFileCount(k1, k2, _dicDirLoadDE))
        //        {
        //            _bRun = false;
        //            return;
        //        }
        //        string[] funNames = Operation.GetFunctionNames(chbEuDE, chbMaDE, chbD2DE, chbChDE);
        //        if (funNames.Length == 0)
        //        {
        //            MessageBox.Show("Please select the way to calculate the dissimiliraty!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }
        //        foreach (var item in funNames)
        //        {
        //            Directory.CreateDirectory(txtSaveDE.Text + "\\EuMaChD2\\" + item);
        //        }
        //        Operation.InitalizeState(pgbDE, txtStateDE, treSaveDE);
        //        Operation.GetPathOfK(dicPathK, _dicDirLoadDE);
        //        CalDissimility("EuMaChD2", dicPathK, k1, k2, precision, funNames, null);
        //    }
        //    #endregion


        //    #region D2S  D2Star
        //    else if (tabAll.SelectedTab.Text == "Dissimiliraty_D2S_D2Star")
        //    {
        //        Operation.GetParameter(cbxk1DS, cbxk2DS, cbxPrecisionDS, rbnSingleDS.Checked, pgbDS, out k1, out k2, out precision, dicPathK, _dicDirLoadDS.Count);
        //        if (!Operation.CheckFileCount(k1, k2, _dicDirLoadDS))
        //        {
        //            _bRun = false;
        //            return;
        //        }
        //        List<int> list = Operation.GetMarkovOrderList(chbM0DS, chbM1DS, chbM2DS, chbM3DS);
        //        if (list.Count == 0)
        //        {
        //            MessageBox.Show("Please select the Markov Order!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }
        //        string way = null;
        //        if (chbD2SDS.Checked && chbD2StarDS.Checked)
        //        {
        //            way = "All";
        //            Directory.CreateDirectory(txtSaveDS.Text + "\\D2S_D2Star\\D2S");
        //            Directory.CreateDirectory(txtSaveDS.Text + "\\D2S_D2Star\\D2Star");
        //        }
        //        else if (chbD2SDS.Checked)
        //        {
        //            way = "D2S";
        //            Directory.CreateDirectory(txtSaveDS.Text + "\\D2S_D2Star\\D2S");
        //        }
        //        else if (chbD2StarDS.Checked)
        //        {
        //            way = "D2Star";
        //            Directory.CreateDirectory(txtSaveDS.Text + "\\D2S_D2Star\\D2Star");
        //        }
        //        else
        //        {
        //            MessageBox.Show("Please select the way to calculate the dissimiliraty!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }
        //        Operation.InitalizeState(pgbDS, txtStateDS, treSaveDS);
        //        Operation.GetPathOfK(dicPathK, _dicDirLoadDS);
        //        CalDissimility(way, dicPathK, k1, k2, precision, null, list);
        //    }
        //    #endregion


        //    #region S2
        //    else if (tabAll.SelectedTab.Text == "Dissimiliraty_S2")
        //    {
        //        Operation.GetParameter(cbxk1S2, cbxk2S2, cbxPrecisionS2, rbnSingleS2.Checked, pgbS2, out k1, out k2, out precision, dicPathK, _dicDirLoadS2.Count);
        //        if (!Operation.CheckFileCount(k1, k2, _dicDirLoadS2))
        //        {
        //            _bRun = false;
        //            return;
        //        }
        //        List<int> list = Operation.GetMarkovOrderList(chbM0S2, chbM1S2, chbM2S2, chbM3S2);
        //        if (list.Count == 0)
        //        {
        //            MessageBox.Show("Please select the Markov Order!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }
        //        Directory.CreateDirectory(txtSaveS2.Text + "\\S2");
        //        Operation.InitalizeState(pgbS2, txtStateS2, treSaveS2);
        //        Operation.GetPathOfK(dicPathK, _dicDirLoadS2);
        //        CalDissimility("S2", dicPathK, k1, k2, precision, null, list);
        //    }
        //    #endregion
        //}


        #endregion

        #region Dissimility New


        #endregion


        private void specialModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Action act = ModeChange;
            if (menuStrip1.Items[4].Text != "Normal Mode")
            {
                frm = new Form2(Location.X, Location.Y, act);
                frm.Show();
                menuStrip1.Items[4].Text = "Normal Mode";               
            }
            else
            {
                frm.Close();
                frm = null;
                spPath = null;
            }
        }

        private void ModeChange()
        {
            menuStrip1.Items[4].Text = "Special Mode";
        }
    }
}
