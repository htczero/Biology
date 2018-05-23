using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace d2_toolsPro
{
    class OperationKtuple : Operation
    {
        public OperationKtuple(Dictionary<string, TreeView> dicTreeView, Dictionary<string, ComboBox> dicComboBox, TextBox txtState, TextBox txtSave, Label lblTo, ProgressBar pgb, RadioButton rbnSingle) : base(dicTreeView, dicComboBox, txtState, txtSave, lblTo, pgb, rbnSingle)
        {

        }

        protected override bool Prepare(bool bSingle, Dictionary<string, string[]> dic)
        {
            if (!CheckSetting(bSingle))
            {
                return false;
            }
            InitalizeState();

            int fileCount = GetDicPath(dic);


            GetParameter(bSingle, fileCount);
            return true;
        }

        protected override int GetDicPath(Dictionary<string, string[]> dic)
        {
            int fileCount = 0;
            foreach (TreeNode node in DicTreeView["treLoad"].Nodes)
            {
                string[] tmps = Directory.GetFiles(LoadPath + "\\" + node.FullPath);
                fileCount += tmps.Length;
                if (DicTreeView["treLoad"].Nodes.Count == 1)
                {
                    dic.Add("", tmps);
                    break;
                }
                dic.Add(node.Text, tmps);
            }
            return fileCount;
        }

        /// <summary>
        /// 计算kTuple
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="k1"></param>
        /// <param name="k2"></param>
        private void CalKtupleCout(string str, int k1, int k2, params string[] tmps)
        {
            Action<string,string> act = ((string filePath,string sp) =>
            {
                KtupleCount kc = new KtupleCount(filePath, sp);
                for (int i = k1; i < k2 + 1; i++)
                {
                    kc.GetKtupleCount(i);
                    ShowState(kc.SeqName + "\tk = " + i);
                }
            });
            string savePath = txtSave.Text + "\\Ktuple";
            if (str != "")
            {
                savePath = savePath + "\\" + str;
            }

            Parallel.ForEach(tmps, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, (filePath) =>
            {
                act(filePath,savePath);
            });
        }

        /// <summary>
        /// 获得kTuple文件
        /// </summary>
        public override void StarCalculation()
        {
            try
            {
                Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
                if (!Prepare(rbnSingle.Checked, dic))
                {
                    Form1._bRun = false;
                    return;
                }

                //开始计算
                Task task = new Task(() =>
                {
                    Stopwatch sp = new Stopwatch();
                    sp.Start();

                    Parallel.ForEach(dic, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, (item) =>
                    {
                        CalKtupleCout(item.Key, K1, K2, item.Value);
                    });

                    sp.Stop();
                    double sec = sp.ElapsedMilliseconds / 1000.0;
                    savePath = txtSave.Text + "\\Ktuple";
                    AddToTreeView(DicTreeView["treSave"], DicTreeView["treSave"].Nodes, "", Directory.GetDirectories(SavePath));
                    MessageBox.Show("Missition Completed!\r\nTime : " + sec + " sec", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Form1._bRun = false;
                });
                task.Start();
            }
            catch
            {
                MessageBox.Show("Error!");
                Form1.frm1.Close();
            }
        }

    }
}
