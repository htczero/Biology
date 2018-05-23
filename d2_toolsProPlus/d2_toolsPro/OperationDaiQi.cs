using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace d2_toolsPro
{
    class OperationDaiQi: OperationHao
    {
        protected List<CheckBox> listR = new List<CheckBox>();

        public OperationDaiQi(Dictionary<string, TreeView> dicTreeView, Dictionary<string, ComboBox> dicComboBox, TextBox txtState, TextBox txtSave, Label lblTo, ProgressBar pgb, List<CheckBox> listR, RadioButton rbnSingle) : base(dicTreeView, dicComboBox, txtState, txtSave, lblTo, pgb, rbnSingle)
        {
            this.listR = listR;
        }

        protected bool Prepare(bool bSingle, Dictionary<string, string[]> dic, List<int> list)
        {
            if (!Prepare(bSingle, dic))
                return false;

            for (int i = 0; i < listR.Count; i++)
            {
                if (listR[i].Checked)
                    list.Add(i);
            }
            if (list.Count == 0)
                return false;

            return true;
        }

        public override void StarCalculation()
        {
            try
            {
                Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
                List<int> listR = new List<int>();
                if (!Prepare(rbnSingle.Checked, dic, listR))
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
                            DissimiliratyDaiQi distance = new DissimiliratyDaiQi(path.Value, savePath, listR, item.Key, Precision);
                            act(distance, path.Key);
                            ShowState("k = " + path.Key);
                        });
                        dicPathK = new Dictionary<int, Dictionary<string, string>>();
                    }
                    Form1._bRun = false;
                    sp.Stop();
                    double sec = sp.ElapsedMilliseconds / 1000.0;
                    savePath = txtSave.Text + "\\DaiQi";
                    if (dic.Count == 1)
                    {
                        AddToTreeView(dicTreeView["treSave"], dicTreeView["treSave"].Nodes, "", Directory.GetFiles(savePath));
                    }
                    else
                    {
                        AddToTreeView(dicTreeView["treSave"], dicTreeView["treSave"].Nodes, "", Directory.GetDirectories(savePath));
                    }
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
    }
}
