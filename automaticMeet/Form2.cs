using System;
using System.IO;
using System.Windows.Forms;

namespace automaticMeet
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        int coordX, coordY;

        private void Form2_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(@"C:\automaticMeet\"))
                Directory.CreateDirectory(@"C:\automaticMeet\");

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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            coordX = Convert.ToInt32(numericUpDown1.Value);
            coordY = Convert.ToInt32(numericUpDown2.Value);

            if (coordX == 0 || coordY == 0)
                MessageBox.Show("Inserisci coordinate valide.");
            else
                SetCursorPos(coordX, coordY);
        }

        public void button2_Click(object sender, EventArgs e)
        {
            if (coordX != 0 && coordY != 0)
            {
                if (File.Exists(@"C:\automaticMeet\coords.txt"))
                    File.Delete(@"C:\automaticMeet\coords.txt");

                using (StreamWriter sw = File.CreateText(@"C:\automaticMeet\coords.txt"))
                {
                    sw.WriteLine(coordX);
                    sw.WriteLine(coordY);
                }

                MessageBox.Show("Applicate con successo!");
                this.Close();
            }
            else
                MessageBox.Show("Clicca (TEST) prima di applicare.");
        }
    }
}
