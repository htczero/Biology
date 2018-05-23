using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace d2_toolsPro
{
    class OperationHao : Operation
    {
        protected int precision = 5;
        public OperationHao(Dictionary<string, TreeView> dicTreeView, Dictionary<string, ComboBox> dicComboBox, TextBox txtState, TextBox txtSave, Label lblTo, ProgressBar pgb, RadioButton rbnSingle) : base(dicTreeView, dicComboBox, txtState, txtSave, lblTo, pgb, rbnSingle)
        {

        }

        public int Precision
        {
            get
            {
                return precision;
            }
        }

        protected override bool Prepare(bool bSingle, Dictionary<string, string[]> dic)
        {
            if (!CheckSetting(bSingle))
            {
                return false;
            }
            InitalizeState();

            GetDicPath(dic);
            int fileCount = GetPgbMax(dic);



            GetParameter(bSingle, fileCount);

            if (dicComboBox["precision"].SelectedItem != null)
            {
                precision = int.Parse(dicComboBox["precision"].SelectedItem.ToString());
            }

            return CheckFileCount(dic);
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
                Action<Dissimiliraty, int> act = GetCalMode();

                Dictionary<int, Dictionary<string, string>> dicPathK = new Dictionary<int, Dictionary<string, string>>();
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
                              DissimiliratyHao distance = new DissimiliratyHao(path.Value, savePath, item.Key, Precision);
                          //distance.GetDissimiliratyMatrix(path.Key);
                          act(distance, path.Key);
                              ShowState(item.Key + "\tk = " + path.Key);
                          });
                          dicPathK = new Dictionary<int, Dictionary<string, string>>();
                      }
                      Form1._bRun = false;
                      sp.Stop();
                      double sec = sp.ElapsedMilliseconds / 1000.0;
                      savePath = txtSave.Text + "\\Hao";
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

        protected int GetPgbMax(Dictionary<string, string[]> dic)
        {
            return dic.Count;
        }


    }
}
