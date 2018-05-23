using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace d2_toolsPro
{
    class OperationEuMaChD2 : OperationHao
    {
        protected List<CheckBox> listCheckbox = new List<CheckBox>();

        protected List<CheckBox> ListCheckbox
        {
            get
            {
                return listCheckbox;
            }
        }
        public OperationEuMaChD2(Dictionary<string, TreeView> dicTreeView, Dictionary<string, ComboBox> dicComboBox, TextBox txtState, TextBox txtSave, Label lblTo, ProgressBar pgb, List<CheckBox> listCbx, RadioButton rbnSingle) : base(dicTreeView, dicComboBox, txtState, txtSave, lblTo, pgb, rbnSingle)
        {
            listCheckbox = listCbx;
        }

        protected bool Prepare(bool bSingle, Dictionary<string, string[]> dic, List<string> listFun)
        {
            if (!Prepare(bSingle, dic))
                return false;

            foreach (CheckBox chb in ListCheckbox)
            {
                if (chb.Checked)
                    listFun.Add(chb.Text);
            }

            if (listFun.Count == 0)
                return false;

            return true;
        }

        public override void StarCalculation()
        {
            try
            {
                Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
                List<string> listFun = new List<string>();
                if (!Prepare(rbnSingle.Checked, dic, listFun))
                {
                    Form1._bRun = false;
                    return;
                }
                Dictionary<int, Dictionary<string, string>> dicPathK = new Dictionary<int, Dictionary<string, string>>();
                Action<Dissimiliraty, int> act = GetCalMode();

                Task task = new Task(() =>
                {
                    Stopwatch sp = new Stopwatch();
                    sp.Start();
                    foreach (KeyValuePair<string, string[]> item in dic)
                    {
                        string savePath = txtSave.Text;
                        GetPathOfK(dicPathK, item.Value);

                        Parallel.ForEach(dicPathK, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount / 2 }, (path) =>
                        {
                            Dissimiliraty_Eu_Ma_Ch_D2 distance = new Dissimiliraty_Eu_Ma_Ch_D2(path.Value, savePath, path.Key, listFun, item.Key, Precision);
                        //distance.GetDissimiliratyMatrix(path.Key);
                        act(distance, path.Key);
                            ShowState(item.Key + "\tk = " + path.Key);
                        });
                        dicPathK = new Dictionary<int, Dictionary<string, string>>();
                    }
                    Form1._bRun = false;
                    sp.Stop();
                    double sec = sp.ElapsedMilliseconds / 1000.0;
                    savePath = txtSave.Text;
                    AddSaveFilesToTree(dic);
                    Form1.spPath = null;
                    MessageBox.Show("Missition Completed!\r\nTime : " + sec + " sec", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
                task.Start();
            }
            catch
            {
                MessageBox.Show("Error!");
                Form1.frm1.Close();
            }
        }

        protected void AddSaveFilesToTree(Dictionary<string, string[]> dic)
        {
            TreeView treSave = DicTreeView["treSave"];
            Action act = () =>
                {
                   
                    foreach (CheckBox cbx in ListCheckbox)
                    {
                        if (cbx.Checked)
                        {
                            TreeNode node = new TreeNode(cbx.Text);
                            treSave.Nodes.Add(node);
                            string[] tmps = Directory.GetFiles(savePath + "\\" + cbx.Text);
                            if (dic.Count > 1)
                                tmps = Directory.GetDirectories(savePath + "\\" + cbx.Text);
                             AddToTreeView(treSave, node.Nodes, "", tmps);
                        }
                    }
                };
            treSave.BeginInvoke(act);
        }


    }
}
