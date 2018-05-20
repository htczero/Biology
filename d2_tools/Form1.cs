using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace d2_tools
{
    public partial class frm1 : Form
    {
        public frm1()
        {
            InitializeComponent();
        }
        #region kTupleCount
        #region 准备阶段

        /// <summary>
        /// 窗体初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>
        /// 单一k值或者范围k值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rbnSingleKTC.Checked == true)
            {
                lblToTC.Visible = false;
                lblInfoOfkTC.Visible = true;
                txtkTwoTC.Text = "";
                txtkTwoTC.Visible = false;
            }
            else
            {
                lblToTC.Visible = true;
                lblInfoOfkTC.Visible = false;
                txtkTwoTC.Visible = true;
            }
        }

        /// <summary>
        /// 选择序列文件导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSeqOne_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择序列文件";
            ofd.Filter = "序列文件|*.fasta|所有文件|*.*";
            ofd.ShowDialog();
            if (ofd.FileName == "")
            {
                return;
            }
            Button btn = (Button)sender;
            if (btn.Name == "btnSeqOneTC")
            {
                txtSeqOneTC.Text = ofd.FileName;
            }
            else
            {
                txtSeqTwoTC.Text = ofd.FileName;
            }
        }

        /// <summary>
        /// 检查k值是否合法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtkOne_TextChanged(object sender, EventArgs e)
        {
            TextBox tbx = (TextBox)sender;
            if (tbx.Text == "")
            {
                return;
            }
            else
            {
                int num = 0;
                if (!int.TryParse(tbx.Text, out num))
                {
                    MessageBox.Show("k值请输入整数", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (num <= 0)
                {
                    MessageBox.Show("k值为正整数", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// 保存路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            if (fbd.SelectedPath != "")
            {
                txtSavePathTC.Text = fbd.SelectedPath;
            }
        }

        /// <summary>
        /// 拖拽文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link; //重要代码：表明是链接类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void txtPath_DragDrop(object sender, DragEventArgs e)
        {
            string fullPath = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if (!File.Exists(fullPath))
            {
                txtSavePathTC.Text = fullPath;
            }//if
            else
            {
                MessageBox.Show("请放入文件夹", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 拖拽序列导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSeqOne_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link; //重要代码：表明是链接类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void txtSeqOne_DragDrop(object sender, DragEventArgs e)
        {
            TextBox tbx = (TextBox)sender;
            Array arrFiles = ((Array)e.Data.GetData(DataFormats.FileDrop));
            if (arrFiles.Length == 1)
            {
                string fullPath = (arrFiles.GetValue(0)).ToString();

                if (CheckSeqFile(fullPath))
                    tbx.Text = fullPath;

                else
                    MessageBox.Show("请放入fasta文件或者txt文件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (arrFiles.Length == 2)
            {
                string pathOne = (arrFiles.GetValue(0)).ToString();
                string pathTwo = (arrFiles.GetValue(1)).ToString();
                if (CheckSeqFile(pathOne, pathTwo))
                {
                    txtSeqOneTC.Text = pathOne;
                    txtSeqTwoTC.Text = pathTwo;
                }
                else
                {
                    MessageBox.Show("请放入fasta文件或者txt文件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
                MessageBox.Show("文件数目不能多于两个！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 检查序列文件
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        private bool CheckSeqFile(params string[] paths)
        {
            foreach (string path in paths)
            {
                //文件存在
                if (File.Exists(path))
                {
                    string ext = Path.GetExtension(path);
                    //文件格式符合
                    if (ext == ".fasta" || ext == ".txt")
                        continue;
                    //文件格式不符合
                    else
                        return false;
                }
                //文件不存在
                else
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 是否是单一序列文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (rbnSingleSeqTC.Checked == true)
            {
                txtSeqTwoTC.Text = "";
                txtSeqTwoTC.Visible = false;
                lblRateTwoTC.Visible = false;
                pgbTwoTC.Visible = false;
                btnSeqTwoTC.Visible = false;
                lblSeqTwoTC.Visible = false;
            }
            else
            {
                txtSeqTwoTC.Visible = true;
                lblRateTwoTC.Visible = true;
                pgbTwoTC.Visible = true;
                btnSeqTwoTC.Visible = true;
                lblSeqTwoTC.Visible = true;
            }
        }
        #endregion

        #region 开始阶段
        private void btnStar_Click(object sender, EventArgs e)
        {
            //检查参数设定
            if (!CheckSetting())
            {
                return;
            }
            //参数无异常
            else
            {
                txtStateOneTC.Clear();
                txtStateTwoTC.Clear();
                pgbOneTC.Value = 0;
                pgbTwoTC.Value = 0;
                Stopwatch sp = new Stopwatch();
                sp.Start();
                Task task = null;

                int k1 = int.Parse(txtkOneTC.Text);
                int k2 = k1;

                if (rbnRangeKTC.Checked)
                {
                    k2 = int.Parse(txtkTwoTC.Text);
                }

                pgbOneTC.Maximum = pgbTwoTC.Maximum = k2 - k1 + 1;

                if (rbnSingleSeqTC.Checked)
                {
                    task = new Task(() =>
                    {
                        CountSinglekTuple(k1, k2, txtSeqOneTC.Text, txtStateOneTC, pgbOneTC);
                    });
                }
                else
                {
                    Action act1 = new Action(() =>
                      {
                          CountSinglekTuple(k1, k2, txtSeqOneTC.Text, txtStateOneTC, pgbOneTC);

                      });
                    Action act2 = new Action(() =>
                      {
                          CountSinglekTuple(k1, k2, txtSeqTwoTC.Text, txtStateTwoTC, pgbTwoTC);
                      });
                    task = new Task(() =>
                     {
                         Parallel.Invoke(act1, act2);
                     });
                }
                task.Start();
                Task.WaitAll(task);
                sp.Stop();
                MessageBox.Show(this, "Mission Complete!\r\n用时 ：" + sp.Elapsed + " s", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 计算获取kTuple
        /// </summary>
        /// <param name="k1"></param>
        /// <param name="k2"></param>
        /// <param name="filePath"></param>
        /// <param name="tbxState"></param>
        /// <param name="pgb"></param>
        private void CountSinglekTuple(int k1, int k2, string filePath, TextBox tbxState, ProgressBar pgb)
        {
            string sampleName = Path.GetFileNameWithoutExtension(filePath);
            string sampleSeq = null;
            using (StreamReader sr = new StreamReader(filePath, Encoding.ASCII))
            {
                sr.ReadLine();
                StringBuilder sb = new StringBuilder();
                string tmp = null;
                while (true)
                {
                    tmp = sr.ReadLine();
                    if (tmp == null)
                        break;
                    sb.Append(tmp);
                }
                sampleSeq = sb.ToString();
            }


            Action act1 = new Action(() => { });
            Action act2 = null;
            Action<TupleCount, int> actTmp = new Action<TupleCount, int>((TupleCount tcTmp, int k) =>
                {
                    tcTmp.K = k;
                    tcTmp.GetkTupleCount();
                    tbxState.Text += "k = " + k + "完成！\r\n";
                    pgb.Value++;
                });

            if (k1 != k2)
            {
                act1 = new Action(() =>
              {
                  TupleCount tcTmp = new TupleCount(sampleName, sampleSeq, txtSavePathTC.Text);

                  for (int i = k1; i < k2; i++)
                  {
                      tcTmp.KTuples.Clear();
                      tcTmp.KTuples = new List<string>();
                      actTmp(tcTmp, i);
                  }

              });
            }
            act2 = new Action(() =>
              {
                  TupleCount tc = new TupleCount(sampleName, sampleSeq, txtSavePathTC.Text);
                  actTmp(tc, k2);
              });
            Task task = new Task(() =>
              {
                  Parallel.Invoke(act1, act2);
              });
            task.Start();
            Task.WaitAll(task);
        }

        /// <summary>
        /// 检查数据是否异常
        /// </summary>
        /// <returns></returns>
        private bool CheckSetting()
        {
            if (txtSeqOneTC.Text == "")
            {
                MessageBox.Show("第一条序列不能为空", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txtkOneTC.Text == "")
            {
                MessageBox.Show("k值不能为空", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (rbnDoubleSeqTC.Checked == true && txtSeqTwoTC.Text == "")
            {
                MessageBox.Show("第二条序列不能为空", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (rbnRangeKTC.Checked == true)
            {
                if (txtkTwoTC.Text == "")
                {
                    MessageBox.Show("k值不能为空", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                try
                {
                    if (int.Parse(txtkTwoTC.Text) <= int.Parse(txtkOneTC.Text))
                    {
                        MessageBox.Show("k值范围不正确", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                catch
                {
                    MessageBox.Show("k值应为正整数", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            if (txtSavePathTC.Text == "")
            {
                Directory.CreateDirectory("TC");
                txtSavePathTC.Text = "TC";
            }
            return true;
        }

        #endregion

        #endregion

        #region MarkovPossibility

        private Dictionary<string, Dictionary<string, int>> dickTupleDateOne = null;
        private Dictionary<string, Dictionary<string, int>> dickTupleDateTwo = null;
        private Dictionary<int, int> dicTotalOne = null;
        private Dictionary<int, int> dicTotalTwo = null;
        /// <summary>
        /// 用于标记全局变量是否已经初始化，flase表示没有
        /// </summary>
        private bool dicFlag = false;

        #region 准备工作
        /// <summary>
        /// TC文件夹选定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTC_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            if (fbd.SelectedPath != "")
            {
                txtKtuplePathMP.Text = fbd.SelectedPath;
            }
        }

        /// <summary>
        /// TC文件夹拖拽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTCPath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link; //重要代码：表明是链接类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// MP文件夹选定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            if (fbd.SelectedPath != "")
            {
                txtSavePathMP.Text = fbd.SelectedPath;
            }
        }

        private void txtTCPath_DragDrop(object sender, DragEventArgs e)
        {
            string fullPath = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if (!File.Exists(fullPath))
            {
                txtKtuplePathMP.Text = fullPath;
            }//if
            else
            {
                MessageBox.Show("请放入文件夹", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// MP文件夹拖拽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMPPath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link; //重要代码：表明是链接类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void txtMPPath_DragDrop(object sender, DragEventArgs e)
        {
            string fullPath = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if (!File.Exists(fullPath))
            {
                txtSavePathMP.Text = fullPath;
            }//if
            else
            {
                MessageBox.Show("请放入文件夹", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 单一k值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (rbnSingleKMP.Checked == true)
            {
                txtkTwoMP.Visible = false;
                lblToMP.Visible = false;
                lblInfoKMP1.Visible = false;
            }
            else
            {
                txtkTwoMP.Visible = true;
                lblToMP.Visible = true;
                lblInfoKMP1.Visible = true;
            }
        }

        /// <summary>
        /// 检查k是否合法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtk1_TextChanged(object sender, EventArgs e)
        {
            TextBox tbx = (TextBox)sender;
            if (tbx.Text == "")
                return;
            int tmp = 0;
            if (!(int.TryParse(tbx.Text, out tmp)))
            {
                tbx.Text = "";
                MessageBox.Show("请输入大于等于3的正整数", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (tmp < 3)
            {
                tbx.Text = "";
                MessageBox.Show("请输入大于等于3的正整数", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region 开始阶段
        /// <summary>
        /// 检查设置
        /// </summary>
        /// <returns></returns>
        private bool CheckMarkovSetting()
        {
            if (txtkOneMP.Text == "")
            {
                MessageBox.Show("k值不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (rbnRangeKMP.Checked == true)
            {
                if (txtkTwoMP.Text == "")
                {
                    MessageBox.Show("k值不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (int.Parse(txtkTwoMP.Text) <= int.Parse(txtkOneMP.Text))
                {
                    MessageBox.Show("k值范围不正确！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            if (txtKtuplePathMP.Text == "")
            {
                if (!Directory.Exists("TC"))
                {
                    MessageBox.Show("请输入kTupleCount文件所在路径!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            if (txtSavePathMP.Text == "")
            {
                string savePath = null;
                if (rbnHaoMP.Checked)
                    savePath = "MP_Hao";
                else
                    savePath = "MP_ZeroToThree";
                Directory.CreateDirectory(savePath);
                txtSavePathMP.Text = savePath;
            }
            return true;
        }

        /// <summary>
        /// 开始计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (!CheckMarkovSetting())
                return;
            else
            {
                if (!dicFlag)
                {
                    dickTupleDateOne = new Dictionary<string, Dictionary<string, int>>();
                    dickTupleDateTwo = new Dictionary<string, Dictionary<string, int>>();
                    dicTotalOne = new Dictionary<int, int>();
                    dicTotalTwo = new Dictionary<int, int>();
                    dicFlag = true;
                }
                pgbOneMP.Value = 0;
                pgbTwoMP.Value = 0;
                txtStateOneMP.Clear();
                txtStateTwoMP.Clear();
                Stopwatch sp = new Stopwatch();
                sp.Start();

                int k1 = int.Parse(txtkOneMP.Text);
                int k2 = k1;

                if (!rbnSingleKMP.Checked)
                    k2 = int.Parse(txtkTwoMP.Text);

                pgbOneMP.Maximum = pgbTwoMP.Maximum = k2 - k1 + 1;

                Dictionary<string, string> dicSeqOne = new Dictionary<string, string>();
                Dictionary<string, string> dicSeqTwo = new Dictionary<string, string>();
                GetFilesPath(txtKtuplePathMP.Text, out dicSeqOne, out dicSeqTwo, rbnSingleSeqMP.Checked);

                Task task = null;
                Action actTmp1 = new Action(() => { });
                Action actTmp2 = new Action(() => { });

                Action act1 = new Action(() =>
                  {
                      GetMarkovPossibilityHao(k1, k2, dickTupleDateOne, dicTotalOne, dicSeqOne, 1);
                  });
                Action act2 = new Action(() =>
                   {
                       GetMarkovPossibilityHao(k1, k2, dickTupleDateTwo, dicTotalTwo, dicSeqTwo, 2);
                   });

                Action act3 = new Action(() =>
                   {
                       GetMarkovPossibilityZeroToThree(k1, k2, dickTupleDateOne, dicTotalOne, dicSeqOne, 1);
                   });
                Action act4 = new Action(() =>
                   {
                       GetMarkovPossibilityZeroToThree(k1, k2, dickTupleDateTwo, dicTotalTwo, dicSeqTwo, 2);
                   });
                if (rbnSingleSeqMP.Checked)
                {
                    if (rbnHaoMP.Checked)
                        actTmp1 = act1;
                    else if (rbnZeroMP.Checked)
                        actTmp1 = act3;
                    else
                    {
                        actTmp1 = new Action(() =>
                          {
                              act1();
                              act3();
                          });
                    }
                }
                else
                {
                    if (rbnHaoMP.Checked)
                    {
                        actTmp1 = act1;
                        actTmp2 = act2;
                    }
                    else if (rbnZeroMP.Checked)
                    {
                        actTmp1 = act3;
                        actTmp2 = act4;
                    }
                    else
                    {
                        actTmp1 = new Action(() =>
                          {
                              act1();
                              act3();
                          });
                        actTmp2 = new Action(() =>
                          {
                              act2();
                              act4();
                          });
                    }
                }
                task = new Task(() =>
                  {
                      Parallel.Invoke(actTmp1, actTmp2);
                  });
                task.Start();
                Task.WaitAll(task);
                sp.Stop();
                MessageBox.Show(this, "Mission Complete!\r\n用时 ：" + sp.Elapsed + " s", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }//else
        }

        /// <summary>
        /// 获得所有kTuple文件路径
        /// </summary>
        /// <param name="dicSeqOne"></param>
        /// <param name="dicSeqTwo"></param>
        private void GetFilesPath(string dirPath, out Dictionary<string, string> dicSeqPathOne, out Dictionary<string, string> dicSeqPathTwo, bool single)
        {
            string[] arrFiles = Directory.GetFiles(dirPath, "*.txt");
            int half = arrFiles.Length / 2;
            char[] flag = { '_' };

            //序列一
            dicSeqPathOne = new Dictionary<string, string>();
            string[] tmp = null;

            for (int i = 0; i < half; i++)
            {
                tmp = arrFiles[i].Split(flag, StringSplitOptions.RemoveEmptyEntries);
                dicSeqPathOne.Add(tmp[1], arrFiles[i]);
            }
            tmp[0] = Path.GetFileNameWithoutExtension(tmp[0]);
            dicSeqPathOne.Add("Name", tmp[0]);

            if (single)
            {
                for (int i = half; i < arrFiles.Length; i++)
                {
                    tmp = arrFiles[i].Split(flag, StringSplitOptions.RemoveEmptyEntries);
                    dicSeqPathOne.Add(tmp[1], arrFiles[i]);
                }
                dicSeqPathTwo = null;
            }

            else
            {
                //序列二
                dicSeqPathTwo = new Dictionary<string, string>();
                for (int i = half; i < arrFiles.Length; i++)
                {
                    tmp = arrFiles[i].Split(flag, StringSplitOptions.RemoveEmptyEntries);
                    dicSeqPathTwo.Add(tmp[1], arrFiles[i]);
                }
                tmp[0] = Path.GetFileNameWithoutExtension(tmp[0]);
                dicSeqPathTwo.Add("Name", tmp[0]);
            }
        }

        /// <summary>
        /// 显示状态
        /// </summary>
        /// <param name="k"></param>
        /// <param name="flag"></param>
        private void StateShow(int k, int flag)
        {
            lock(this)
            {
                if (flag == 1)
                {
                    pgbOneMP.Value++;
                    txtStateOneMP.Text += "k = " + k + "\tComplete!\r\n";

                }
                else if (flag == 2)
                {
                    pgbTwoMP.Value++;
                    txtStateTwoMP.Text += "k = " + k + "\tComplete!\r\n";
                }
            }
        }

        /// <summary>
        /// 获得MarkovPossibilityHao文件
        /// </summary>
        /// <param name="k1"></param>
        /// <param name="k2"></param>
        /// <param name="dickTupleDate"></param>
        /// <param name="dicTotal"></param>
        /// <param name="dicSeqPath"></param>
        /// <param name="flag"></param>
        private void GetMarkovPossibilityHao(int k1, int k2, Dictionary<string, Dictionary<string, int>> dickTupleDate, Dictionary<int, int> dicTotal, Dictionary<string, string> dicSeqPath, int flag)
        {
            string seqName = dicSeqPath["Name"];
            MarkovPossibility mp = new MarkovPossibility();

            mp.GetkTupleDate(k1, k2, dicSeqPath, dickTupleDate, dicTotal);
            for (int i = k1; i < k2 + 1; i++)
            {
                mp.GetMarkovPossibilityHao(i, dicTotal, seqName, txtSavePathMP.Text, dickTupleDate);
                StateShow(i, flag);
            }
        }

        /// <summary>
        /// 获得MarkovPossibilityZeroToThree文件
        /// </summary>
        /// <param name="k1"></param>
        /// <param name="k2"></param>
        /// <param name="dickTupleDate"></param>
        /// <param name="dicTotal"></param>
        /// <param name="dicSeqPath"></param>
        /// <param name="flag"></param>
        private void GetMarkovPossibilityZeroToThree(int k1, int k2, Dictionary<string, Dictionary<string, int>> dickTupleDate, Dictionary<int, int> dicTotal, Dictionary<string, string> dicSeqPath, int flag)
        {
            string seqName = dicSeqPath["Name"];
            MarkovPossibility mp = new MarkovPossibility();

            mp.GetkTupleDate(k1, k2, dicSeqPath, dickTupleDate, dicTotal);
            for (int i = k1; i < k2 + 1; i++)
            {
                mp.GetMarkovPossibilityZeroToThree(i, dicTotal, seqName, txtSavePathMP.Text, dickTupleDate);
                StateShow(i, flag);
            }
        }

        #endregion


        #endregion

        #region Dissimilarity


        #region 准备阶段
        /// <summary>
        /// 路径选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTCPathD_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            Button btnTmp = (Button)sender;
            fbd.ShowDialog();
            if (fbd.SelectedPath != "")
            {
                if (btnTmp.Name == btnTCPathD.Name)
                    txtTCPathD.Text = fbd.SelectedPath;

                if (btnTmp.Name == btnMPHaoD.Name)
                    txtMPHaoPathD.Text = fbd.SelectedPath;

                if (btnTmp.Name == btnMPZeroD.Name)
                    txtMPZeroPathD.Text = fbd.SelectedPath;

                if (btnTmp.Name == btnSavePathD.Name)
                    txtSavePathD.Text = fbd.SelectedPath;
            }
        }

        /// <summary>
        /// 拖拽文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTCPathD_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link; //重要代码：表明是链接类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// 拖拽文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTCPathD_DragDrop(object sender, DragEventArgs e)
        {
            string fullPath = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            TextBox txtTmp = (TextBox)sender;
            if (!File.Exists(fullPath))
            {
                if (txtTmp.Name == txtTCPathD.Name)
                    txtTCPathD.Text = fullPath;

                if (txtTmp.Name == txtMPHaoPathD.Name)
                    txtMPHaoPathD.Text = fullPath;

                if (txtTmp.Name == txtMPZeroPathD.Name)
                    txtMPZeroPathD.Text = fullPath;

                if (txtTmp.Name == txtSavePathD.Name)
                    txtSavePathD.Text = fullPath;
            }
            else
            {
                MessageBox.Show("请放入文件夹", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 检查k值是否合法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtk1kTD_TextChanged(object sender, EventArgs e)
        {
            TextBox txtTmp = (TextBox)sender;
            if (txtTmp.Text == "")
                return;
            int k = 0;
            if (!int.TryParse(txtTmp.Text, out k) || k < 3)
            {
                txtTmp.Text = "";
                MessageBox.Show("请输入大于3的正整数", "j警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region 开始

        /// <summary>
        /// 点击开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_1(object sender, EventArgs e)
        {
            if (!CheckSettingDissimiliary())
                return;


            btnStarD.Enabled = false;
            Task task = new Task(() =>
            {
                Stopwatch sp = new Stopwatch();
                sp.Start();

                if (cbxCh.Checked || cbxD2.Checked || cbxMa.Checked || cbxEu.Checked)
                    GetDissimiliratyEuMaChD2(txtSavePathD.Text);

                if (cbxHao.Checked)
                    GetDissimiliratyHao(txtSavePathD.Text);

                if (cbxD2S.Checked || cbxD2Star.Checked)
                    GetDissimiliratyD2StarD2S(txtSavePathD.Text);

                sp.Stop();
                MessageBox.Show("Mission Completed!\r\n用时 : " + sp.Elapsed + " s", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnStarD.Enabled = true;
            });
            task.Start();
        }

        /// <summary>
        /// 检查参数设定
        /// </summary>
        /// <returns></returns>
        private bool CheckSettingDissimiliary()
        {
            if (cbxCh.Checked || cbxD2.Checked || cbxEu.Checked || cbxMa.Checked)
            {
                if (!CheckDirectory(txtTCPathD, "kTuple"))
                    return false;
                if (!CheckkValue(txtk1kTD, txtk2kTD))
                    return false;
            }
            if (cbxHao.Checked)
            {
                if (!CheckDirectory(txtMPHaoPathD, "Markov_Hao"))
                    return false;
                if (!CheckkValue(txtk1MHD, txtk2MHD))
                    return false;
            }
            if (cbxD2S.Checked || cbxD2Star.Checked)
            {
                if (!CheckDirectory(txtMPZeroPathD, "Markov_ZeroToThree"))
                    return false;
                if (!CheckkValue(txtk1MZD, txtk2MZD))
                    return false;

                int r1 = 0;
                int r2 = 0;
                if (int.TryParse(txtr1.Text, out r1) && int.TryParse(txtr2.Text, out r2))
                {
                    if (r1 < 0 || r1 > 3 || r2 < r1)
                    {
                        MessageBox.Show("马尔科夫阶数不正确(0,,1,2,3)", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
            }
            if (txtSavePathD.Text == "")
            {
                Directory.CreateDirectory("Dissimiliraty");
            }
            return true;
        }

        /// <summary>
        /// 检查路径
        /// </summary>
        /// <param name="txtTmp"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool CheckDirectory(TextBox txtTmp, string name)
        {
            if (txtTmp.Text == "")
            {
                if (!Directory.Exists("TC"))
                {
                    MessageBox.Show("请输入" + name + "文件所在路径", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                else
                    txtTmp.Text = Directory.GetCurrentDirectory();
            }
            string[] files = Directory.GetFiles(txtTmp.Text, "*.txt");
            if (files.Length == 0)
            {
                MessageBox.Show("文件不存在", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 检查k值
        /// </summary>
        /// <param name="txtk1"></param>
        /// <param name="txtk2"></param>
        /// <returns></returns>
        private bool CheckkValue(TextBox txtk1, TextBox txtk2)
        {
            int k1 = 0;
            int k2 = 0;
            if (int.TryParse(txtk1.Text, out k1) && int.TryParse(txtk2.Text, out k2))
            {
                if (k2 < k1)
                {
                    MessageBox.Show("k值范围不正确", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Eu Ma Ch D2
        /// </summary>
        /// <param name="savePath"></param>
        private void GetDissimiliratyEuMaChD2(string savePath)
        {
            pgbkD.Value = 0;
            Dictionary<string, string> dicFilePathOne = new Dictionary<string, string>();
            Dictionary<string, string> dicFilePathTwo = new Dictionary<string, string>();
            GetFilesPath(txtTCPathD.Text, out dicFilePathOne, out dicFilePathTwo, false);

            List<string[]> listFunName = new List<string[]>();
            List<string> listSetFun = new List<string>();
            string[] names = { dicFilePathOne["Name"], dicFilePathTwo["Name"] };

            int k1 = int.Parse(txtk1kTD.Text);
            int k2 = int.Parse(txtk2kTD.Text);
            //Eu  Ch  Ma  D2  
            if (cbxEu.Checked && cbxCh.Checked && cbxD2.Checked && cbxMa.Checked)
            {
                string[] tmpFun = { "Eu", "Ma", "Ch", "D2" };
                listFunName.Add(tmpFun);
                listSetFun.Add("All");
            }

            else
            {
                if (cbxEu.Checked)
                {
                    string[] tmpFun = { "Eu" };
                    listFunName.Add(tmpFun);
                    listSetFun.Add("Eu");
                }

                if (cbxCh.Checked)
                {
                    string[] tmpFun = { "Ch" };
                    listFunName.Add(tmpFun);
                    listSetFun.Add("Ch");
                }

                if (cbxMa.Checked)
                {
                    string[] tmpFun = { "Ma" };
                    listFunName.Add(tmpFun);
                    listSetFun.Add("Ma");
                }

                if (cbxD2.Checked)
                {
                    string[] tmpFun = { "D2" };
                    listFunName.Add(tmpFun);
                    listSetFun.Add("D2");
                }
            }

            int tmp = listSetFun.Count;
            pgbkD.Maximum = (k2 - k1 + 1) * tmp;


            Parallel.For(k1, k2 + 1, (i) =>
              {
                  DissimiliratyEuMaChD2 d = new DissimiliratyEuMaChD2(savePath, names);
                  d.GetDateFromOneK(i, dicFilePathOne, dicFilePathTwo);
                  for (int j = 0; j < listSetFun.Count; j++)
                  {
                      d.GetDissimiliratyMatrix(i, listSetFun[j]);
                      ShowState(i, pgbkD, listFunName[j]);
                  }
              });
        }

        /// <summary>
        /// Hao
        /// </summary>
        /// <param name="savePath"></param>
        private void GetDissimiliratyHao(string savePath)
        {
            pgbMHD.Value = 0;
            Dictionary<string, string> dicFilePathOne = new Dictionary<string, string>();
            Dictionary<string, string> dicFilePathTwo = new Dictionary<string, string>();
            GetFilesPath(txtMPHaoPathD.Text, out dicFilePathOne, out dicFilePathTwo, false);

            string[] names = { dicFilePathOne["Name"], dicFilePathTwo["Name"] };

            int k1 = int.Parse(txtk1MHD.Text);
            int k2 = int.Parse(txtk2MHD.Text);

            pgbMHD.Maximum = k2 - k1 + 1;


            Parallel.For(k1, k2 + 1, (i) =>
              {
                  DissimiliratyHao d = new DissimiliratyHao(savePath, names);
                  d.GetDateFromOneK(i, dicFilePathOne, dicFilePathTwo);
                  d.GetDissimiliratyMatrix(i);
                  ShowState(i, pgbMHD, "Hao");
              });

        }

        /// <summary>
        /// D2S  D2Star
        /// </summary>
        /// <param name="savePath"></param>
        private void GetDissimiliratyD2StarD2S(string savePath)
        {
            pgbMZD.Value = 0;
            Dictionary<string, string> dicFilePathOne = new Dictionary<string, string>();
            Dictionary<string, string> dicFilePathTwo = new Dictionary<string, string>();
            GetFilesPath(txtMPZeroPathD.Text, out dicFilePathOne, out dicFilePathTwo, false);

            string[] names = { dicFilePathOne["Name"], dicFilePathTwo["Name"] };
            string[] tmpFun = null;
            string funSet = "All";

            int k1 = int.Parse(txtk1MZD.Text);
            int k2 = int.Parse(txtk2MZD.Text);
            int r1 = int.Parse(txtr1.Text);
            int r2 = int.Parse(txtr2.Text);

            //D2S  D2Star
            if (cbxD2S.Checked && cbxD2Star.Checked)
            {
                tmpFun[0] = "D2S";
                tmpFun[1] = "D2STar";
            }
            else
            {
                if (cbxD2S.Checked)
                {
                    tmpFun[0] = "D2S";
                    funSet = "D2S";
                }
                if (cbxD2Star.Checked)
                {
                    tmpFun[0] = "D2Star";
                    funSet = "D2Star";
                }
            }

            pgbMZD.Maximum = k2 - k1 + 1;

            Parallel.For(k1, k2 + 1, (i) =>
              {
                  DissimiliratyD2StarD2S d = new DissimiliratyD2StarD2S(savePath, names, r1, r2);
                  d.GetDateFromOneK(i, dicFilePathOne, dicFilePathTwo);
                  d.GetDissimiliratyMatrix(i, funSet);
                  ShowState(i, pgbMZD, tmpFun);
              });
        }

        /// <summary>
        /// 显示状态
        /// </summary>
        /// <param name="k"></param>
        /// <param name="funNames"></param>
        private void ShowState(int k, ProgressBar pgb, params string[] funNames)
        {
            lock(this)
            {
                pgb.Value++;
                for (int i = 0; i < funNames.Length; i++)
                {
                    txtStateD.Text += funNames[i] + "  k = " + k + "  Completed!\r\n";
                }
            }

        }

        #endregion

        #endregion
    }
}
