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
                if (radioButton1.Checked == true)
                    classe = "3finf";
                else if (radioButton2.Checked == true)
                {
                    if (textBox1.Text != "")
                        classe = textBox1.Text;
                    else
                    {
                        start = false;
                        MessageBox.Show("Inserisci una classe.");
                    }
                }
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
                        start = true;
                    else if (radioButton10.Checked == true)
                    {
                        if (textBox2.Text != "")
                            classe += textBox2.Text;
                        else
                        {
                            start = false;
                            MessageBox.Show("Inserisci un'ora.");
                        }
                    }
                    else
                    {
                        start = false;
                        MessageBox.Show("Seleziona un'ora.");
                    }

                    if (start == true)
                    {
                        if (textBox3.Text != "" && textBox4.Text != "")
                        {
                            minuti = Convert.ToInt32(textBox3.Text) * 60000;
                            volte = Convert.ToInt32(textBox4.Text);
                        }
                        else
                        {
                            start = false;
                            MessageBox.Show("Inserisci un intervallo.");
                        }

                        if (start == true)
                        {
                            if (textBox5.Text != "")
                                url = "https://meet.google.com/landing?authuser=" + textBox5.Text;
                            else
                            {
                                start = false;
                                MessageBox.Show("Inserisci il codice dell'account scolastico.");
                            }

                            if (start == true)
                            {
                                if (checkBox1.Checked == true)
                                {
                                    if (checkBox2.Checked == true)
                                    {
                                        if (textBox6.Text != "")
                                            messaggio = textBox6.Text;
                                        else
                                        {
                                            start = false;
                                            MessageBox.Show("Inserisci il messaggio.");
                                        }
                                    }
                                    else
                                    {
                                        start = false;
                                        MessageBox.Show("Devi attivare (Auto-Connect) per inviare un messaggio in chat.");
                                    }
                                }

                                if (start == true)
                                {
                                    button1.Text = "STOP";

                                    for (int i = 1; i <= volte; i++)
                                    {
                                        await Task.Delay(1000);
                                        SendKeys.Send("^{ESC}");
                                        await Task.Delay(1000);

                                        SendKeys.SendWait("chrome");
                                        await Task.Delay(1000);
                                        SendKeys.Send("{Enter}");
                                        await Task.Delay(3000);

                                        SendKeys.SendWait(url);
                                        await Task.Delay(1000);
                                        SendKeys.Send("{Enter}");
                                        await Task.Delay(5000);

                                        SendKeys.SendWait(classe);
                                        await Task.Delay(1000);
                                        SendKeys.Send("{Enter}");
                                        await Task.Delay(5000);

                                        SendKeys.Send("^e");
                                        await Task.Delay(1000);
                                        SendKeys.Send("^d");
                                        
                                        if (checkBox2.Checked == true)
                                        {
                                            await Task.Delay(3000);
                                            LeftMouseClick(1250, 600);
                                            await Task.Delay(2000);
                                        }

                                        if (checkBox1.Checked == true)
                                        {
                                            LeftMouseClick(1700, 130);
                                            await Task.Delay(2000);

                                            SendKeys.SendWait(messaggio);
                                            await Task.Delay(1000);
                                            SendKeys.Send("{Enter}");
                                        }

                                        await Task.Delay(minuti);

                                        if (i != volte)
                                            SendKeys.Send("%{F4}");
                                    }

                                    button1.Text = "Riprova.";

                                    MessageBox.Show("Tentativi terminati, se non sei connesso, riprova.");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
