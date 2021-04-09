using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace automaticMeet
{
    public partial class Form2 : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

        public Form2()
        {
            InitializeComponent();

            if (!Directory.Exists(@"C:\automaticMeet\"))
                Directory.CreateDirectory(@"C:\automaticMeet\");

            loadData();
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

        private void loadData()
        {
            try
            {
                using (StreamReader sr = File.OpenText(@"C:\automaticMeet\coords.txt"))
                {
                    for (int i = 0; i <= 1; i++)
                    {
                        if (i == 0)
                            numericUpDown1.Value = Convert.ToInt32(sr.ReadLine());
                        else if (i == 1)
                            numericUpDown2.Value = Convert.ToInt32(sr.ReadLine());
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Inizializzo file delle coordinate...");
            }

            try
            {
                using (StreamReader sr = File.OpenText(@"C:\automaticMeet\color.txt"))
                {
                    for (int i = 0; i <= 2; i++)
                    {
                        if (i == 0)
                            textBox1.Text = sr.ReadLine();
                        else if (i == 1)
                            textBox2.Text = sr.ReadLine();
                        else if (i == 2)
                            textBox3.Text = sr.ReadLine();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Inizializzo file dei colori...");
            }
        }

        int coordX, coordY, colR, colG, colB;

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

        public void button2_Click(object sender, EventArgs e)
        {
            if (coordX != 0 && coordY != 0)
            {
                if (File.Exists(@"C:\automaticMeet\coords.txt"))
                    File.Delete(@"C:\automaticMeet\coords.txt");

                if (File.Exists(@"C:\automaticMeet\color.txt"))
                    File.Delete(@"C:\automaticMeet\color.txt");

                using (StreamWriter sw = File.CreateText(@"C:\automaticMeet\coords.txt"))
                {
                    sw.WriteLine(coordX);
                    sw.WriteLine(coordY);
                }

                using (StreamWriter sw = File.CreateText(@"C:\automaticMeet\color.txt"))
                {
                    sw.WriteLine(colR);
                    sw.WriteLine(colG);
                    sw.WriteLine(colB);
                }

                loadData();

                MessageBox.Show("Applicate con successo!");
                this.Close();
            }
            else
                MessageBox.Show("Clicca (TEST) prima di applicare.");
        }
    }
}
