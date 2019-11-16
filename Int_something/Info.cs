using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Int_something
{
    public partial class Info : Form
    {
        bool flag = true;
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
