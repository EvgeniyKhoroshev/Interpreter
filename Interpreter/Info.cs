using System;
using System.IO;
using System.Windows.Forms;


namespace Interpreter
{
    public partial class Info : Form
    {
        private bool flag = true;
        public Info()
        {
            InitializeComponent();
            if (!flag)
            {
                StreamReader s = new StreamReader("code_info.txt");
                richTextBox1.Text = s.ReadToEnd();
                tabControl1.SelectedIndex = tabControl1.SelectedIndex + 1;
                s.Close();
                flag = true;
            }
            else
            {
                StreamReader s = new StreamReader("bs_info.txt");
                richTextBox1.Text = s.ReadToEnd();
                tabControl1.SelectedIndex = tabControl1.SelectedIndex - 1;
                s.Close();
                flag = false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void переключитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!flag)
            {
                StreamReader s = new StreamReader("code_info.txt");
                richTextBox1.Text = s.ReadToEnd();
                tabControl1.SelectedIndex = tabControl1.SelectedIndex + 1;
                s.Close();
                flag = true;
            }
            else
            {
                StreamReader s = new StreamReader("bs_info.txt");
                richTextBox1.Text = s.ReadToEnd();
                tabControl1.SelectedIndex = tabControl1.SelectedIndex - 1;
                s.Close();
                flag = false;
            }
        }

        private void вФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void закрытьТеориюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
