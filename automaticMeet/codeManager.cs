using System;
using System.IO;
using System.Windows.Forms;

namespace automaticMeet
{
    public partial class codeManager : Form
    {
        publicFunctions publicFunctionsRef = new publicFunctions();

        public codeManager()
        {
            InitializeComponent();
        }

        private void codeManager_Load(object sender, EventArgs e)
        {
            publicFunctionsRef.getSessionFile();

            if (!Directory.Exists((@"C:\automaticMeet\" + publicFunctionsRef.sessionFile[0] + @"\codes")))
                Directory.CreateDirectory(@"C:\automaticMeet\" + publicFunctionsRef.sessionFile[0] + @"\codes");
            else
            {
                publicFunctionsRef.getCodesList(publicFunctionsRef.sessionFile, comboBox1);

                comboBox1.Text = "";
                textBox1.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && textBox1.Text != "")
            {
                string codeName = comboBox1.Text, actualCode = textBox1.Text;

                if (!File.Exists(@"C:\automaticMeet\" + publicFunctionsRef.sessionFile[0] + @"\codes\" + codeName + ".txt"))
                {
                    using (StreamWriter file = File.CreateText(@"C:\automaticMeet\" + publicFunctionsRef.sessionFile[0] + @"\codes\" + codeName + ".txt"))
                    {
                        file.WriteLine(actualCode);
                        file.Close();
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

                if (File.Exists(@"C:\automaticMeet\" + publicFunctionsRef.sessionFile[0] + @"\codes\" + codeName + ".txt"))
                {
                    File.Delete(@"C:\automaticMeet\" + publicFunctionsRef.sessionFile[0] + @"\codes\" + codeName + ".txt");

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
