using System;
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
    public partial class IOform : Form
    {
        public IOform()
        {
            this.KeyPreview = true;
            InitializeComponent();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F9)
                Application.ExitThread();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = false;
            try
            {
                Convert.ToInt64(textBox1.Text);
                try
                {
                    Convert.ToInt32(textBox1.Text);
                    flag = true;
                }
                catch (OverflowException)
                {
                    MessageBox.Show("Число выходит за пределы разрядной сетки int");
                }
            }
            catch(FormatException)
            {
                    MessageBox.Show("Введите число.");
            }
            
            if (flag)
                this.Hide();
        }
    }
}
