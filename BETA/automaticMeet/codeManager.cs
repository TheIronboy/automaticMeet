using System;
using System.IO;
using System.Windows.Forms;

namespace automaticMeet
{
    public partial class codeManager : Form
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

        public void loadSettings()
        {
            string[] fileEntries = Directory.GetFiles(@"C:\automaticMeet\" + sessionFile[0] + @"\codes");

            comboBox1.Items.Clear();

            foreach (string fileName in fileEntries)
                comboBox1.Items.Add(Path.GetFileName(fileName.Remove(fileName.Length - 4)));
        }

        public codeManager()
        {
            InitializeComponent();

            getSession();

            loadSettings();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && textBox1.Text != "")
            {
                string codeName = comboBox1.Text, actualCode = textBox1.Text;

                if (!File.Exists(@"C:\automaticMeet\" + sessionFile[0] + @"\codes\" + codeName + ".txt"))
                {
                    using (StreamWriter sw = File.CreateText(@"C:\automaticMeet\" + sessionFile[0] + @"\codes\" + codeName + ".txt"))
                    {
                        sw.WriteLine(actualCode);
                    }

                    MessageBox.Show("Codice creato con successo!");

                    this.Close();
                }
                else
                    MessageBox.Show("Codice già esistente.");
            }
            else
                MessageBox.Show("Riempi tutti i campi.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                string codeName = comboBox1.Text;

                if (File.Exists(@"C:\automaticMeet\" + sessionFile[0] + @"\codes\" + codeName + ".txt"))
                {
                    File.Delete(@"C:\automaticMeet\" + sessionFile[0] + @"\codes\" + codeName + ".txt");

                    loadSettings();

                    MessageBox.Show("Eliminato con successo!");

                    this.Close();
                }
                else
                    MessageBox.Show("Codice non trovato.");
            }
            else
                MessageBox.Show("Seleziona un codice.");
        }
    }
}
