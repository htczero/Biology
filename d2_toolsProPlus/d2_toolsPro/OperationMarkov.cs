using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace d2_toolsPro
{
    class OperationMarkov : Operation
    {
        protected List<CheckBox> listCheckBox = new List<CheckBox>();
        public OperationMarkov(Dictionary<string, TreeView> dicTreeView, Dictionary<string, ComboBox> dicComboBox, TextBox txtState, TextBox txtSave, Label lblTo, ProgressBar pgb, List<CheckBox> cbx, RadioButton rbnSingle) : base(dicTreeView, dicComboBox, txtState, txtSave, lblTo, pgb, rbnSingle)
        {
            listCheckBox = cbx;
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
            int n = 0;
            foreach (CheckBox item in listCheckBox)
            {
                if (item.Checked)
                {
                    n++;
                }
            }
            Pgb.Maximum *= n;

            return CheckFileCount(dic);
        }

        protected override bool CheckFileCount(Dictionary<string, string[]> dicLoad)
        {
            //List<int> list = new List<int>();
            //foreach (string[] item in dicLoad.Values)
            //{
            //    list.Clear();
            //    foreach (string path in item)
            //    {
            //        string[] dir = Directory.GetFiles(path);
            //        foreach (string file in dir)
            //        {
            //            list.Add(GetKValue(file));
            //        }
            //    }
            //    for (int i = k1 - 2; i < k2 + 1; i++)
            //    {
            //        if (!list.Contains(i))
            //        {
            //            MessageBox.Show("The number of inputed files does not math k!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            return false;
            //        }
            //    }
            //}
            return true;
        }

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

                Task task = new Task(() =>
                {
                    Stopwatch sp = new Stopwatch();
                    sp.Start();

                    Parallel.ForEach(dic, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, (item) =>
                    {
                        CalMarkovPossibility(item.Key, item.Value);
                    });

                    sp.Stop();
                    savePath = txtSave.Text + "\\Markov";
                    AddToTreeView(dicTreeView["treSave"], dicTreeView["treSave"].Nodes, "", Directory.GetDirectories(savePath));
                    Form1.spPath = null;
                    MessageBox.Show("Mission Completed!\r\nTime : " + sp.ElapsedMilliseconds / 1000.0 + " sec", "Tip", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void CalMarkovPossibility(string str, params string[] tmps)
        {
            if (str != "")
                str = "\\" + str;

            Action<MarkovPossibility, string> actCal = ((MarkovPossibility mp, string funName) =>
            {
                for (int i = k1; i < k2 + 1; i++)
                {
                    mp.GetMarkovPossibility(i);
                    ShowState(mp.SeqName + "_" + funName + "\tk = " + i);
                }
            });

            Action<string> actPre = ((string funName) =>
            {
                Parallel.ForEach(tmps, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, (item) =>
                {
                    string savePath = txtSave.Text + "\\Markov\\" + funName + str;
                    string[] path = Directory.GetFiles(item);
                    MarkovPossibility mp = null;
                    if (funName == "Hao")
                        mp = new MarkovPossibilityHao(path, savePath);
                    else
                        mp = new MarkovPossibilityZeroToThree(path, savePath);

                    actCal(mp, funName);
                });
            });



            foreach (CheckBox item in listCheckBox)
            {
                if (item.Checked)
                {
                    actPre(item.Text);
                }
            }
        }


    }
}
