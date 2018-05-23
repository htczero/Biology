using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace d2_toolsPro
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        Action act = null;
        public Form2(int x, int y, Action act)
        {
            Location = new Point(x - ClientSize.Width, y + 41);
            this.act = act;
            InitializeComponent();
        }

        Operation operation = null;
        private void treeView1_DragDrop(object sender, DragEventArgs e)
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

            Form1.spPath = tmp;
            operation.DragFileOrDir(treLoadSP, tmp);
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            if (Form1._bRun)
                return;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link; //重要代码：表明是链接类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            Dictionary<string, TreeView> dic = new Dictionary<string, TreeView>();
            dic.Add("treLoad", treLoadSP);
            operation = new Operation(dic, null, null, null, null, null, null);
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Form1._bRun)
                return;

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            if (fbd.SelectedPath != "")
            {
                operation.DragFileOrDir(operation.DicTreeView["treLoad"], fbd.SelectedPath);
                Form1.spPath = fbd.SelectedPath;
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treLoadSP.SelectedNode != null)
                treLoadSP.SelectedNode.Remove();
        }

        private void previewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            operation.Preview();
        }

        private void Form2_DoubleClick(object sender, EventArgs e)
        {
            if (treLoadSP.SelectedNode != null)
                operation.Preview();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1.spPath = null;
            act();
        }
    }
}
