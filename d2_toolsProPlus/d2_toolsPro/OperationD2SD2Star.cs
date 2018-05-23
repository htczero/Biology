using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace d2_toolsPro
{
    class OperationD2SD2Star : OperationEuMaChD2
    {
        protected List<CheckBox> listR = new List<CheckBox>();
        public OperationD2SD2Star(Dictionary<string, TreeView> dicTreeView, Dictionary<string, ComboBox> dicComboBox, TextBox txtState, TextBox txtSave, Label lblTo, ProgressBar pgb, List<CheckBox> listCbx, List<CheckBox> listR, RadioButton rbnSingle) : base(dicTreeView, dicComboBox, txtState, txtSave, lblTo, pgb, listCbx, rbnSingle)
        {
            this.listR = listR;
        }

        protected bool Prepare(bool bSingle, Dictionary<string, string[]> dic, List<string> listFun, List<int> list)
        {
            if (!Prepare(bSingle, dic, listFun))
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
                List<string> listFun = new List<string>();
                List<int> listR = new List<int>();
                if (!Prepare(rbnSingle.Checked, dic, listFun, listR))
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
                            DissimiliratyD2SD2Star distance = new DissimiliratyD2SD2Star(path.Value, savePath, listR, listFun, item.Key, Precision);
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
    }
}
