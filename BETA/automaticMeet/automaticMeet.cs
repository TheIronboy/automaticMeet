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
    public partial class automaticMeet : Form
    {
        string[] sessionFile;

        public void getSession()
        {
            sessionFile = new string[3];

            using (StreamReader sr = File.OpenText(@"C:\automaticMeet\.session.txt"))
            {
                for (int i = 0; i < sessionFile.Length; i++)
                {
                    sessionFile[i] = sr.ReadLine();
                }
            }
        }

        string[] settingsFile;

        int coordX = 0, coordY = 0, colR = 0, colG = 0, colB = 0;

        public void loadSettings()
        {
            settingsFile = new string[19];

            if (File.Exists(@"C:\automaticMeet\" + sessionFile[0] + @"\settings.txt"))
            {
                using (StreamReader sr = File.OpenText(@"C:\automaticMeet\" + sessionFile[0] + @"\settings.txt"))
                {
                    for (int i = 0; i < settingsFile.Length; i++)
                    {
                        settingsFile[i] = sr.ReadLine();
                    }
                }
            }

            string[] fileEntries = Directory.GetFiles(@"C:\automaticMeet\" + sessionFile[0] + @"\codes");

            comboBox1.Items.Clear();

            foreach (string fileName in fileEntries)
                comboBox1.Items.Add(Path.GetFileName(fileName.Remove(fileName.Length - 4)));

            if (settingsFile[0] == null && settingsFile[1] == null)
            {
                for (int i = 0; i <= 4; i++)
                {
                    settingsFile[i] = "0";
                }
            }

            coordX = Convert.ToInt32(settingsFile[0]);
            coordY = Convert.ToInt32(settingsFile[1]);

            colR = Convert.ToInt32(settingsFile[2]);
            colG = Convert.ToInt32(settingsFile[3]);
            colB = Convert.ToInt32(settingsFile[4]);

            comboBox1.Text = settingsFile[5];

            if (settingsFile[6] != null)
                domainUpDown1.Text = settingsFile[6];
            else
                domainUpDown1.Text = "1";

            numericUpDown1.Value = Convert.ToInt32(settingsFile[7]);

            if (settingsFile[8] != null)
                numericUpDown2.Value = Convert.ToInt32(settingsFile[8]);
            else
                numericUpDown2.Value = 1;

            checkBox1.Checked = Convert.ToBoolean(settingsFile[9]);
            checkBox2.Checked = Convert.ToBoolean(settingsFile[10]);
            checkBox3.Checked = Convert.ToBoolean(settingsFile[11]);
            checkBox4.Checked = Convert.ToBoolean(settingsFile[12]);
            checkBox5.Checked = Convert.ToBoolean(settingsFile[13]);
            checkBox6.Checked = Convert.ToBoolean(settingsFile[14]);

            if (checkBox6.Checked)
                textBox1.Text = settingsFile[15];

            radioButton1.Checked = Convert.ToBoolean(settingsFile[16]);
            radioButton2.Checked = Convert.ToBoolean(settingsFile[17]);
            radioButton3.Checked = Convert.ToBoolean(settingsFile[18]);

            if (!radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked)
                radioButton2.Checked = true;
        }

        public automaticMeet()
        {
            InitializeComponent();

            getSession();

            loadSettings();
        }

        public void saveSettings()
        {
            settingsFile[5] = comboBox1.Text;

            settingsFile[6] = domainUpDown1.Text;

            settingsFile[7] = numericUpDown1.Value.ToString();
            settingsFile[8] = numericUpDown2.Value.ToString();

            settingsFile[9] = checkBox1.Checked.ToString();
            settingsFile[10] = checkBox2.Checked.ToString();
            settingsFile[11] = checkBox3.Checked.ToString();
            settingsFile[12] = checkBox4.Checked.ToString();
            settingsFile[13] = checkBox5.Checked.ToString();
            settingsFile[14] = checkBox6.Checked.ToString();

            settingsFile[15] = textBox1.Text;

            settingsFile[16] = radioButton1.Checked.ToString();
            settingsFile[17] = radioButton2.Checked.ToString();
            settingsFile[18] = radioButton3.Checked.ToString();

            using (StreamWriter sw = File.CreateText(@"C:\automaticMeet\" + sessionFile[0] + @"\settings.txt"))
            {
                for (int i = 0; i < settingsFile.Length; i++)
                {
                    sw.WriteLine(settingsFile[i]);
                }
            }
        }

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

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

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(0x02, xpos, ypos, 0, 0);
            mouse_event(0x04, xpos, ypos, 0, 0);
        }

        public async void button1_Click(object sender, EventArgs e)
        {
            bool start = true;

            saveSettings();

            if (button1.Text != "STOP")
            {
                string selectedCode = "";
                if (File.Exists(@"C:\automaticMeet\" + sessionFile[0] + @"\codes\" + comboBox1.Text + ".txt"))
                {
                    using (StreamReader sr = File.OpenText(@"C:\automaticMeet\" + sessionFile[0] + @"\codes\" + comboBox1.Text + ".txt"))
                    {
                        selectedCode = sr.ReadLine();
                    }
                }
                else
                {
                    start = false;
                    MessageBox.Show("Seleziona un codice valido oppure creane uno nuovo.");
                }

                if (start)
                {
                    saveSettings();

                    if (checkBox1.Checked)
                        selectedCode += "-" + domainUpDown1.Text;

                    string messaggio = "";
                    if (checkBox5.Checked && checkBox6.Checked)
                    {
                        if (textBox1.Text != "")
                            messaggio = textBox1.Text;
                        else
                        {
                            start = false;
                            MessageBox.Show("Inserisci il messaggio.");
                        }
                    }
                    else if (!checkBox5.Checked && checkBox6.Checked)
                    {
                        start = false;
                        MessageBox.Show("Devi attivare (Auto-Join/Stop) per inviare un messaggio in chat.");
                    }

                    if (start)
                    {
                        saveSettings();

                        int speed = 0;
                        if (radioButton1.Checked)
                            speed = 4;
                        else if (radioButton2.Checked)
                            speed = 2;
                        else if (radioButton3.Checked)
                            speed = 1;

                        string url = "https://meet.google.com/landing?authuser=" + numericUpDown1.Value;
                        if ((coordX != 0 && coordY != 0) || !checkBox5.Checked)
                        {
                            button1.Text = "STOP";

                            if (!checkBox5.Checked && (checkBox3.Checked || checkBox4.Checked))
                            {
                                MessageBox.Show("Attenzione! Le opzioni (Disattiva Camera) e (Disattiva Microfono) non sono disponibili se (Auto-Start/Stop) è disabilitato.");

                                checkBox3.Checked = false;
                                checkBox4.Checked = false;
                            }

                            progressBar1.Value = 0;
                            progressBar1.Maximum = 10;

                            saveSettings();

                            await Task.Delay(1000 * speed);
                            Process chrome = Process.Start("chrome.exe", url);

                            progressBar1.Increment(1);
                            await Task.Delay(3000 * speed);

                            while (start)
                            {
                                SendKeys.Send("{F6}");
                                await Task.Delay(1000 * speed);

                                SendKeys.SendWait(url);
                                SendKeys.Send("{Enter}");

                                progressBar1.Increment(1);
                                await Task.Delay(3000 * speed);

                                if (!checkBox2.Checked)
                                {
                                    SendKeys.Send("{TAB}");
                                    await Task.Delay(1000 * speed);
                                }

                                SendKeys.SendWait(selectedCode);
                                SendKeys.Send("{Enter}");

                                progressBar1.Increment(1);
                                await Task.Delay(5000 * speed);

                                Color colorFound = GetColorAt(coordX, coordY);

                                if (colorFound.R == colR && colorFound.G == colG && colorFound.B == colB)
                                {
                                    if (checkBox3.Checked)
                                    {
                                        SendKeys.Send("^e");

                                        progressBar1.Increment(1);
                                        await Task.Delay(1000 * speed);
                                    }
                                    else
                                        progressBar1.Increment(1);

                                    if (checkBox4.Checked)
                                    {
                                        SendKeys.Send("^d");

                                        progressBar1.Increment(1);
                                        await Task.Delay(1000 * speed);
                                    }
                                    else
                                        progressBar1.Increment(1);

                                    if (checkBox5.Checked)
                                    {
                                        LeftMouseClick(coordX, coordY);
                                        progressBar1.Increment(1);
                                    }
                                    else
                                        progressBar1.Increment(1);

                                    if (checkBox6.Checked)
                                    {
                                        await Task.Delay(3000 * speed);
                                        SendKeys.Send("^%c");
                                        progressBar1.Increment(1);

                                        await Task.Delay(2000 * speed);
                                        SendKeys.SendWait(messaggio);

                                        await Task.Delay(1000 * speed);
                                        SendKeys.Send("{Enter}");
                                        progressBar1.Increment(1);

                                        await Task.Delay(3000 * speed);
                                        SendKeys.Send("^%c");
                                        progressBar1.Increment(1);
                                    }
                                    else
                                        progressBar1.Increment(3);

                                    start = false;
                                }
                                else
                                {
                                    int minuti = Convert.ToInt32(numericUpDown2.Value);

                                    progressBar1.Value = 0;
                                    progressBar1.Maximum = minuti * 2;

                                    for (int i = 0; i < minuti * 2; i++)
                                    {
                                        await Task.Delay(30000);
                                        progressBar1.Increment(1);
                                    }
                                }
                            }

                            progressBar1.Increment(1);
                            button1.Text = "Riavvia";
                            MessageBox.Show("Connesso!");
                        }
                        else
                        {
                            start = false;
                            MessageBox.Show("Coordinate per (Auto-Join/Stop) non definite, usa (Calibrator)");

                            saveSettings();

                            calibrator calibrator = new calibrator();
                            calibrator.ShowDialog();

                            loadSettings();
                        }
                    }
                }
            }
            else
                Application.Exit();
        }

        private void gestisciCodiciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveSettings();

            Form codeManager = new codeManager();
            codeManager.ShowDialog();

            loadSettings();
        }

        private void calibratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (button1.Text != "STOP")
            {
                saveSettings();

                calibrator calibrator = new calibrator();
                calibrator.ShowDialog();

                loadSettings();
            }
            else
                MessageBox.Show("Non avviare (Calibrator) mentre il programma è avviato.");
        }

        int i = 0;

        private void creditiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            i++;

            if (i == 10)
            {
                MessageBox.Show("Android pattumiera");
                i = 0;
            }
            else
                MessageBox.Show("Idea by Misael Canova. Developed by Luca Giordano.");
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming Soon");
        }
    }
}
