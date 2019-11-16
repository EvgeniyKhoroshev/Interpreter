using System;
using System.Linq;
using System.Windows.Forms;

namespace Int_something
{
    public partial class Form1 : Form
    {

        execute_all execution;
        Compare cmp;
        bool isDeveloper = false;
        BS imageForm;
        string basic_text = "Program\n{\n\n}";
        public Form1()
        {
            InitializeComponent();
            cmp = new Compare();
            richTextBox1.Text = basic_text;
            taskTextBox.Text = cmp.currentProblem.problemText;
            execution = new execute_all();


            this.KeyPreview = true;
            if (!isDeveloper)
            {
                tabControl1.TabPages.Remove(tabControl1.TabPages[0]);
                tabControl1.TabPages.Remove(tabControl1.TabPages[1]);
                tabControl1.TabPages.Remove(tabControl1.TabPages[tabControl1.TabPages.IndexOfKey("tabPage2")]);
                taskTextBox.ReadOnly = true;
                resultTextBox.ReadOnly = true;
            }
            else
            {
                tabControl1.Show();
            }
            if (!isDeveloper)
            {
                Info f = new Info();
                f.ShowDialog();
            }
            tabControl1.Update();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                button1_Click(sender, e);
            if (e.KeyCode == Keys.F1)
                теорияToolStripMenuItem_Click(sender, e);
            if (e.KeyCode == Keys.F9)
                Application.ExitThread();
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            if (cmp.currentScore >= 80)
            {

                if (!cmp.getNextProblem())
                {
                    cmp.addCurrentResult();
                    MessageBox.Show("Это была последняя задача. Ваш результат:" + (cmp.getFinalResult()).ToString());
                    resultTextBox.Text = "Это была последняя задача. Ваш результат:" + (cmp.getFinalResult()).ToString();
                    return;
                }
                richTextBox1.Text = basic_text;
                MessageBox.Show("Задача выполнена, можно перейти к следующей.");
                taskTextBox.Text = cmp.currentProblem.problemText;
                resultTextBox.Text = "";
                cmp.addCurrentResult();
                if (!cmp.currentProblem.problemImage)
                {
                    button4.Visible = false;
                    button5.Visible = false;
                }
                else
                {
                    button4.Visible = true;
                    button5.Visible = true;
                }
                tabControl1.SelectedIndex = 0;
            }
            else MessageBox.Show("Задача решена менее чем на 80%, попробуйте еще.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "" || richTextBox1.Text == "\t" || richTextBox1.Text == "\n" || richTextBox1.Text == basic_text)
            {
                MessageBox.Show("Необходимо ввести текст программы.", "Предупреждение");
                return;
            }
            execution = new execute_all();
            button2_Click(sender, e);
            execution.solve(richTextBox1.Lines);
            richTextBox5.AppendText("Количество токенов: " + Convert.ToString(execution.tokens_count) + "\n");
            richTextBox3.Text += execution.toOut;
            if (execution.lex.ErrorListLA.Count > 0)
            foreach (string s in execution.lex.ErrorListLA)
            {
                richTextBox2.Text += '\n' + s;
            }
            richTextBox3.AppendText("\nСписок состояний:\n");
            richTextBox3.AppendText(execution.lex.stateLogLA + "\n");

            foreach (string s in execution.source.Identifiers.errors)
                richTextBox2.AppendText(s);
            richTextBox4.AppendText(execution.synt.Log + "\n");
            foreach (var s in execution.synt.errLog)
                richTextBox2.AppendText(s + "\n");
            foreach (var x in execution.source.Identifiers.intTable)
                richTextBox5.AppendText(x.Value.name + "  " + "INT \n");
            foreach (var x in execution.source.Identifiers.boolTable)
                richTextBox5.AppendText(x.Value.name + "  " + "BOOL \n");
            richTextBox5.AppendText("\n-------------------\n\n");
            foreach (var x in execution.regroupedTable.TranslationList)
            {
                if (x.Token == '{' || x.Token == ';')
                {
                    richTextBox5.AppendText(x.Value + "  " + "\n  ");
                    continue;
                }
                richTextBox5.AppendText(x.Value + "  ");
            }

            richTextBox5.AppendText("\n-------------------\n\n");
            foreach (var x in execution.output)
            {
                if (x.Token == '{' || x.Token == ';' || x.Token == ':' || x.Token == '>')
                {
                    if (x.Token != ';')
                        richTextBox5.AppendText(x.Token + "  " + "\n  ");
                    continue;
                }
                richTextBox5.AppendText(x.Token + "  ");
            }
            if (execution.out_log != "")
            {
                richTextBox5.AppendText("\n-------------------\n\n");
                richTextBox2.AppendText("\n" + execution.out_log + "\n");
                richTextBox5.AppendText("\n-------------------\n\n");
            }
            foreach (var x in execution.ThreeAddressCode)
            {
                richTextBox5.AppendText(Convert.ToString(x.triadNumber) + "   "
                    + x.FirstOperand.Value + "  " + x.Operation.Value + "  " + x.SecondOperand.Value + "\n");
            }
            richTextBox5.AppendText("\n-------------------\n\n");
            foreach (var x in execution.source.Identifiers.intTable)
                richTextBox5.AppendText(x.Value.name + " = " + Convert.ToString(x.Value.value) + " INT  \n");
            foreach (var x in execution.source.Identifiers.boolTable)
                richTextBox5.AppendText(x.Value.name + "  " + Convert.ToString(x.Value.value) + " BOOL \n");
            if (execution.error != "")
                richTextBox2.AppendText(execution.error + "\n");
            richTextBox5.AppendText("\n-------------------\n\n");
            for (int i = 0; i < execution.triadResult.Count(); ++i)
                if (execution.triadResult[i] != null)
                    richTextBox5.AppendText(i.ToString()+". "+execution.triadResult[i] + "\n");
            string cmpResult = cmp.getResult(execution);
            if (cmpResult != "")
            {
                resultTextBox.Text = "";
                resultTextBox.Text = cmpResult;
            }
            resultTextBox.AppendText("\nЗначения объявленных переменных: \n");
            foreach (var x in execution.source.Identifiers.intTable)
                resultTextBox.AppendText("INT " + x.Value.name + " = " + x.Value.value.ToString() + "\n");
            foreach (var x in execution.source.Identifiers.boolTable)
                resultTextBox.AppendText("BOOL " + x.Value.name + " = " + x.Value.value.ToString()+ "\n");
            richTextBox5.AppendText("\n-------------------\n\n");
            tabControl1.SelectedIndex = 1;
        }
        internal execute_all execute_all
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
            richTextBox3.Text = "";
            richTextBox4.Text = "";
            richTextBox5.Text = "";
            GC.Collect();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            imageForm = new BS();
            if (cmp.currentProblem.problemImage)
            {
                imageForm.Show();
                imageForm.setImageIndex(cmp.currentProblem.problemIndex);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button5_Click(sender, e);
        }

        private void теорияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Info f = new Info();
            f.Show();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show(tabControl1.SelectedIndex.ToString());
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            int k = 0, j = 0;
            int []sm = new int[6];
            for (int i = 1001; i < 1000000; ++i)
            {
                for (k = 10, j = 0; j < 10; k = k * 10, ++j)
                {
                    sm[j] = i % k;
                }

                richTextBox1.Text += "\n";
                for (int l = 5; l >= 0; ++l)
                {
                    richTextBox1.Text += Convert.ToString(sm[l]);

                }
            }
        }
    }
}
