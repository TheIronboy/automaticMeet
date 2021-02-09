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
            string classe = "", url = "";
            bool start = true;

            if (button1.Text == "STOP")
                Application.Exit();
            else
            {
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
                            button1.Text = "STOP";

                            for (int i = 1; i <= 10; i++)
                            {
                                await Task.Delay(1000);
                                SendKeys.Send("^{ESC}");
                                await Task.Delay(1000);

                                SendKeys.SendWait("chrome");
                                SendKeys.Send("{Enter}");
                                await Task.Delay(2000);

                                SendKeys.SendWait(url);
                                SendKeys.Send("{Enter}");
                                await Task.Delay(5000);

                                SendKeys.SendWait(classe);
                                SendKeys.Send("{Enter}");
                                await Task.Delay(5000);

                                SendKeys.Send("^e");
                                SendKeys.Send("^d");
                                await Task.Delay(2000);

                                LeftMouseClick(1250, 600);
                                await Task.Delay(60000);

                                SendKeys.Send("%{F4}");
                            }

                            button1.Text = "Collegati!";
                            MessageBox.Show("Tentativi terminati, se non sei connesso, riprova.");
                        }
                    }
                }
            }
        }
    }
}
