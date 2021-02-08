using System;
using System.Windows.Forms;

namespace automaticMeet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string classe = "", url = "";
            bool start = true;

            if (radioButton1.Checked == true)
                classe = "3finf";
            else if (radioButton2.Checked == true)
                classe = textBox1.Text;
            else
            {
                start = false;
                MessageBox.Show("Seleziona una classe.");
            }

            if (start == true)
            {
                if (radioButton3.Checked == true)
                    classe += "-1";
                else if (radioButton4.Checked == true)
                    classe += "-2";
                else if (radioButton5.Checked == true)
                    classe += "-3";
                else if (radioButton6.Checked == true)
                    classe += "-4";
                else if (radioButton7.Checked == true)
                    classe += "-5";
                else if (radioButton8.Checked == true)
                    classe += "-6";
                else if (radioButton9.Checked == true)
                    classe += textBox2.Text;
                else
                {
                    start = false;
                    MessageBox.Show("Seleziona un'ora.");
                }

                if (start == true)
                {
                    if (textBox3.Text != "")
                        url = "https://meet.google.com/?authuser=" + textBox3.Text;
                    else
                    {
                        start = false;
                        MessageBox.Show("Inserisci il codice dell'account scolastico.");
                    }

                    if (start == true)
                    {
                        SendKeys.Send("^{ESC}");
                        System.Threading.Thread.Sleep(1000);

                        SendKeys.Send("chrome");
                        SendKeys.Send("{Enter}");
                        System.Threading.Thread.Sleep(2000);

                        SendKeys.Send(url);
                        SendKeys.Send("{Enter}");
                        System.Threading.Thread.Sleep(2000);

                        for (int i = 0; i <= 5; i++)
                        {
                            SendKeys.Send(classe);
                            SendKeys.Send("{Enter}");
                            System.Threading.Thread.Sleep(5000);
                            SendKeys.Send("^D");
                            SendKeys.Send("^E");
                            System.Threading.Thread.Sleep(1000);
                            SendKeys.Send("{Enter}");
                            System.Threading.Thread.Sleep(120000);
                        }

                        MessageBox.Show("Tentativi terminati.");
                    }
                }
            }
        }
    }
}
