using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace automaticMeet
{
    public partial class Form1 : Form
    {
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public Form1()
        {
            InitializeComponent();
        }

        public Color GetColorAt(int xpos, int ypos)
        {
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, xpos, ypos, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }
            return screenPixel.GetPixel(0, 0);
        }

        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(0x02, xpos, ypos, 0, 0);
            mouse_event(0x04, xpos, ypos, 0, 0);
        }

        public async void button1_Click(object sender, EventArgs e)
        {
            bool start = true;
            string classe = "", messaggio = "", url = "https://meet.google.com/landing?authuser=" + numericUpDown2.Value;
            int coordX = 0, coordY = 0, colR = 0, colG = 0, colB = 0, minuti = 0;

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
                        if (numericUpDown1.Value != 0)
                            minuti = Convert.ToInt32(numericUpDown1.Value) * 2;
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
                                    if (textBox1.Text != "")
                                        messaggio = textBox1.Text;
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
                                try
                                {
                                    using (StreamReader sr = File.OpenText(@"C:\automaticMeet\coords.txt"))
                                        for (int i = 0; i <= 1; i++)
                                        {
                                            if (i == 0)
                                                coordX = Convert.ToInt32(sr.ReadLine());
                                            else if (i == 1)
                                                coordY = Convert.ToInt32(sr.ReadLine());
                                        }

                                    using (StreamReader sr = File.OpenText(@"C:\automaticMeet\color.txt"))
                                    {
                                        for (int i = 0; i <= 2; i++)
                                        {
                                            if (i == 0)
                                                colR = Convert.ToInt32(sr.ReadLine());
                                            else if (i == 1)
                                                colG = Convert.ToInt32(sr.ReadLine());
                                            else if (i == 2)
                                                colB = Convert.ToInt32(sr.ReadLine());
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("Coordinate e/o colori da inizializzare, perfavore usa (Calibrator).");
                                }

                                if (coordX != 0 && coordY != 0)
                                {
                                    button1.Text = "STOP";

                                    progressBar1.Value = 0;
                                    progressBar1.Maximum = 10;

                                    await Task.Delay(2000);
                                    Process processo = Process.Start("chrome");

                                    progressBar1.Increment(1);
                                    await Task.Delay(5000);

                                    SendKeys.SendWait(url);
                                    SendKeys.Send("{Enter}");

                                    await Task.Delay(2000);

                                    while (start == true)
                                    {
                                        SendKeys.Send("^+r");

                                        progressBar1.Increment(1);
                                        await Task.Delay(5000);

                                        SendKeys.SendWait(classe);
                                        SendKeys.Send("{Enter}");

                                        progressBar1.Increment(1);
                                        await Task.Delay(5000);

                                        Color colorFound = GetColorAt(coordX, coordY);

                                        if (colorFound.R == colR && colorFound.G == colG && colorFound.B == colB)
                                        {
                                            if (checkBox1.Checked == true)
                                            {
                                                SendKeys.Send("^e");

                                                progressBar1.Increment(1);
                                                await Task.Delay(1000);
                                            }
                                            else
                                                progressBar1.Increment(1);

                                            if (checkBox2.Checked == true)
                                            {
                                                SendKeys.Send("^d");

                                                progressBar1.Increment(1);
                                                await Task.Delay(1000);
                                            }
                                            else
                                                progressBar1.Increment(1);

                                            if (checkBox3.Checked == true)
                                            {
                                                LeftMouseClick(coordX, coordY);

                                                progressBar1.Increment(1);
                                                await Task.Delay(5000);
                                            }
                                            else
                                                progressBar1.Increment(1);

                                            if (checkBox4.Checked == true)
                                            {
                                                SendKeys.Send("^%c");

                                                progressBar1.Increment(1);
                                                await Task.Delay(2000);

                                                SendKeys.SendWait(messaggio);
                                                SendKeys.Send("{Enter}");

                                                progressBar1.Increment(1);
                                                await Task.Delay(2000);

                                                SendKeys.Send("^%c");

                                                progressBar1.Increment(1);
                                                await Task.Delay(2000);
                                            }
                                            else
                                                progressBar1.Increment(3);

                                            start = false;
                                            button1.Text = "Riavvia";
                                            progressBar1.Increment(1);
                                        }
                                        else
                                        {
                                            progressBar1.Value = 0;
                                            progressBar1.Maximum = minuti;

                                            for (int i = 0; i < minuti; i++)
                                            {
                                                await Task.Delay(30000);
                                                progressBar1.Increment(1);
                                            }
                                        }
                                    }

                                    MessageBox.Show("Connesso!");
                                }
                            }
                        }
                    }
                }
            }
        }

        private void calibratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (button1.Text == "STOP")
                MessageBox.Show("Non avviare (Calibrator) mentre il programma è avviato.");
            else
            {
                Form2 Calibrator = new Form2();
                Calibrator.ShowDialog();
            }
        }

        private void creditiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Idea by Misael Canova. Developed by Luca Giordano.");
        }
    }
}
