using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace automaticMeet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }

        public async void button1_Click(object sender, EventArgs e)
        {
            string classe = "", url = "", messaggio = "";
            int minuti = 0, volte = 0;
            bool start = true;

            if (button1.Text == "STOP")
                Application.Exit();
            else
            {
                if (comboBox1.Text == "")
                {
                    start = false;
                    MessageBox.Show("Seleziona o inserisci un codice.");
                }
                else if (comboBox1.Text == "3F INFORMATICA")
                    classe = "3finf";
                else
                    classe = comboBox1.Text;

                if (start == true)
                {
                    if (comboBox2.Text == "")
                    {
                        start = false;
                        MessageBox.Show("Seleziona o inserisci un'ora.");
                    }
                    else if (comboBox2.Text == "1 ORA")
                        classe += "-1";
                    else if (comboBox2.Text == "2 ORA")
                        classe += "-2";
                    else if (comboBox2.Text == "3 ORA")
                        classe += "-3";
                    else if (comboBox2.Text == "4 ORA")
                        classe += "-4";
                    else if (comboBox2.Text == "5 ORA")
                        classe += "-5";
                    else if (comboBox2.Text == "6 ORA")
                        classe += "-6";
                    else if (comboBox2.Text == "NESSUNA")
                        start = true;

                    else
                        classe += comboBox2.Text;

                    if (start == true)
                    {
                        if (textBox1.Text != "" && textBox2.Text != "")
                        {
                            minuti = 60000 * Convert.ToInt32(textBox1.Text);
                            volte = Convert.ToInt32(textBox2.Text);
                        }
                        else
                        {
                            start = false;
                            MessageBox.Show("Inserisci un intervallo.");
                        }

                        if (start == true)
                        {
                            if (textBox3.Text != "")
                                url = "https://meet.google.com/landing?authuser=" + textBox3.Text;
                            else
                            {
                                start = false;
                                MessageBox.Show("Inserisci il codice dell'account scolastico.");
                            }

                            if (start == true)
                            {
                                if (checkBox4.Checked == true)
                                {
                                    if (checkBox3.Checked == true)
                                    {
                                        if (textBox5.Text != "")
                                            messaggio = textBox5.Text;
                                        else
                                        {
                                            start = false;
                                            MessageBox.Show("Inserisci il messaggio.");
                                        }
                                    }
                                    else
                                    {
                                        start = false;
                                        MessageBox.Show("Devi attivare (Auto Join) per inviare un messaggio in chat.");
                                    }
                                }

                                if (start == true)
                                {
                                    button1.Text = "STOP";

                                    for (int i = 1; i <= volte; i++)
                                    {
                                        textBox4.Text = "";
                                        await Task.Delay(2000);

                                        SendKeys.Send("^{ESC}");
                                        textBox4.Text += "Ho aperto il menu start." + Environment.NewLine;
                                        await Task.Delay(2000);

                                        SendKeys.SendWait("chrome");
                                        await Task.Delay(2000);
                                        SendKeys.Send("{Enter}");
                                        textBox4.Text += Environment.NewLine + "Ho aperto Chrome." + Environment.NewLine;
                                        await Task.Delay(5000);

                                        SendKeys.SendWait(url);
                                        await Task.Delay(2000);
                                        SendKeys.Send("{Enter}");
                                        textBox4.Text += Environment.NewLine + "Ho inserito l'url di meet con l'account specificato." + Environment.NewLine;
                                        await Task.Delay(5000);

                                        SendKeys.SendWait(classe);
                                        await Task.Delay(2000);
                                        SendKeys.Send("{Enter}");
                                        textBox4.Text += Environment.NewLine + "Ho inserito il codice e ho tentato il collegamento." + Environment.NewLine;
                                        await Task.Delay(5000);

                                        if (checkBox1.Checked == true)
                                        {
                                            SendKeys.Send("^e");
                                            textBox4.Text += Environment.NewLine + "Ho disattivato la telecamera. " + Environment.NewLine;
                                            await Task.Delay(1000);
                                        }
                                        
                                        if (checkBox2.Checked == true)
                                        {
                                            SendKeys.Send("^d");
                                            textBox4.Text += Environment.NewLine + "Ho disattivato il microfono. " + Environment.NewLine;
                                            await Task.Delay(1000);
                                        }

                                        if (checkBox3.Checked == true)
                                        {
                                            LeftMouseClick(1250, 600);
                                            textBox4.Text += Environment.NewLine + "Ho tentato il collegamento." + Environment.NewLine;
                                            await Task.Delay(5000);
                                        }

                                        if (checkBox4.Checked == true)
                                        {
                                            SendKeys.Send("^%c");
                                            textBox4.Text += Environment.NewLine + "Ho aperto la chat." + Environment.NewLine;
                                            await Task.Delay(2000);

                                            SendKeys.SendWait(messaggio);
                                            await Task.Delay(2000);
                                            SendKeys.Send("{Enter}");
                                            textBox4.Text += Environment.NewLine + "Ho inviato il messaggio inserito." + Environment.NewLine;

                                            await Task.Delay(2000);
                                            SendKeys.Send("^%c");
                                            textBox4.Text += Environment.NewLine + "Ho chiuso la chat." + Environment.NewLine;
                                        }

                                        textBox4.Text += Environment.NewLine + "In caso di riuscita, chiudere il programma ORA, altrimenti, riproverò automaticamente tra: " + textBox1.Text + " minuti.";
                                        await Task.Delay(minuti);

                                        if (i != volte)
                                            SendKeys.Send("%{F4}");
                                    }

                                    button1.Text = "Riprova.";
                                    MessageBox.Show("Tentativi terminati, se non sei connesso, riprova.");

                                    Form2 crediti = new Form2();
                                    crediti.Show();

                                    await Task.Delay(5000);
                                    crediti.Close();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
