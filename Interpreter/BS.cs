using System;
using System.Windows.Forms;

namespace Interpreter
{
    public partial class BS : Form
    {
        private System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BS));
        public void setImageIndex(int index)
        {
            tabControl2.SelectedIndex = index - 1;
        }
        public BS()
        {
            InitializeComponent();
        }

        private void BS_Load(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void image1_Click(object sender, EventArgs e)
        {

        }

        private void image3_Click(object sender, EventArgs e)
        {

        }
    }
}
