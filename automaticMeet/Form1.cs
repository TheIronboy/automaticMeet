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
            string classe = "", messaggio = "", url = "https://meet.google.com/landing?authuser=" + numericUpDown3.Value;
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
                        if (numericUpDown1.Value != 0 && numericUpDown2.Value != 0)
                        {
                            minuti = 60000 * Convert.ToInt32(numericUpDown1.Value);
                            volte = Convert.ToInt32(numericUpDown2.Value);
                        }
                        else
                        {
                            start = false;
                            MessageBox.Show("Inserisci un intervallo valido.");
                        }

                        if (start == true)
                        {
                            if (checkBox4.Checked == true)
                            {
                                if (checkBox3.Checked == true)
                                {
                                    if (textBox2.Text != "")
                                        messaggio = textBox2.Text;
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
                                    textBox1.Text = "";
                                    await Task.Delay(2000);

                                    SendKeys.Send("^{ESC}");
                                    textBox1.Text += "Ho aperto il menu start." + Environment.NewLine;
                                    await Task.Delay(2000);

                                    SendKeys.SendWait("chrome");
                                    await Task.Delay(2000);
                                    SendKeys.Send("{Enter}");
                                    textBox1.Text += Environment.NewLine + "Ho aperto Chrome." + Environment.NewLine;
                                    await Task.Delay(5000);

                                    SendKeys.SendWait(url);
                                    await Task.Delay(2000);
                                    SendKeys.Send("{Enter}");
                                    textBox1.Text += Environment.NewLine + "Ho inserito l'url di meet con l'account specificato." + Environment.NewLine;
                                    await Task.Delay(5000);

                                    SendKeys.SendWait(classe);
                                    await Task.Delay(2000);
                                    SendKeys.Send("{Enter}");
                                    textBox1.Text += Environment.NewLine + "Ho inserito il codice e ho tentato il collegamento." + Environment.NewLine;
                                    await Task.Delay(5000);

                                    if (checkBox1.Checked == true)
                                    {
                                        SendKeys.Send("^e");
                                        textBox1.Text += Environment.NewLine + "Ho disattivato la telecamera. " + Environment.NewLine;
                                        await Task.Delay(1000);
                                    }
                                        
                                    if (checkBox2.Checked == true)
                                    {
                                        SendKeys.Send("^d");
                                        textBox1.Text += Environment.NewLine + "Ho disattivato il microfono. " + Environment.NewLine;
                                        await Task.Delay(1000);
                                    }

                                    if (checkBox3.Checked == true)
                                    {
                                        LeftMouseClick(1250, 600);
                                        textBox1.Text += Environment.NewLine + "Ho tentato il collegamento." + Environment.NewLine;
                                        await Task.Delay(5000);
                                    }

                                    if (checkBox4.Checked == true)
                                    {
                                        SendKeys.Send("^%c");
                                        textBox1.Text += Environment.NewLine + "Ho aperto la chat." + Environment.NewLine;
                                        await Task.Delay(2000);

                                        SendKeys.SendWait(messaggio);
                                        await Task.Delay(2000);
                                        SendKeys.Send("{Enter}");
                                        textBox1.Text += Environment.NewLine + "Ho inviato il messaggio inserito." + Environment.NewLine;

                                        await Task.Delay(2000);
                                        SendKeys.Send("^%c");
                                        textBox1.Text += Environment.NewLine + "Ho chiuso la chat." + Environment.NewLine;
                                    }

                                    if (i != volte)
                                    {
                                        textBox1.Text += Environment.NewLine + "In caso di riuscita, chiudere il programma ORA, altrimenti, riproverò automaticamente tra: " + numericUpDown1.Value + " minuti." + Environment.NewLine;
                                        await Task.Delay(minuti);

                                        textBox1.Text += Environment.NewLine + "Sto riprovando, chiudo chrome...";
                                        await Task.Delay(2000);
                                        SendKeys.Send("%{F4}");
                                    }
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
