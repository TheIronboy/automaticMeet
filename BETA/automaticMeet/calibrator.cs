using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace automaticMeet
{
    public partial class calibrator : Form
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

            numericUpDown1.Value = Convert.ToInt32(settingsFile[0]);
            numericUpDown2.Value = Convert.ToInt32(settingsFile[1]);

            textBox1.Text = settingsFile[2];
            textBox2.Text = settingsFile[3];
            textBox3.Text = settingsFile[4];
        }

        public calibrator()
        {
            InitializeComponent();

            getSession();

            loadSettings();
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

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

        private void button1_Click(object sender, EventArgs e)
        {
            coordX = Convert.ToInt32(numericUpDown1.Value);
            coordY = Convert.ToInt32(numericUpDown2.Value);

            if (coordX == 0 || coordY == 0)
                MessageBox.Show("Inserisci coordinate valide.");
            else
            {
                SetCursorPos(coordX, coordY);

                Color colorTest = GetColorAt(coordX, coordY);

                colR = colorTest.R;
                colG = colorTest.G;
                colB = colorTest.B;
            }
        }

        public void saveSettings()
        {
            settingsFile[0] = coordX.ToString();
            settingsFile[1] = coordY.ToString();
            settingsFile[2] = colR.ToString();
            settingsFile[3] = colG.ToString();
            settingsFile[4] = colB.ToString();

            using (StreamWriter sw = File.CreateText(@"C:\automaticMeet\" + sessionFile[0] + @"\settings.txt"))
            {
                for (int i = 0; i < settingsFile.Length; i++)
                {
                    sw.WriteLine(settingsFile[i]);
                }
            }
        }

        public void button2_Click(object sender, EventArgs e)
        {
            if (coordX != 0 && coordY != 0)
            {
                saveSettings();

                loadSettings();

                MessageBox.Show("Applicate con successo!");

                this.Close();
            }
            else
                MessageBox.Show("Clicca (TEST) prima di applicare.");
        }
    }
}
