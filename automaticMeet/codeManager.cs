using System;
using System.IO;
using System.Windows.Forms;

namespace automaticMeet
{
    public partial class codeManager : Form
    {
        publicFunctions publicFunctionsRef = new publicFunctions();

        string codeDir;

        public codeManager()
        {
            InitializeComponent();
        }

        private void codeManager_Load(object sender, EventArgs e)
        {
            codeDir = publicFunctionsRef.mainDir + @"\" + publicFunctionsRef.getSessionData()[0] + @"\codes";
            publicFunctionsRef.getCodeList(comboBox1);

            comboBox1.Text = "";
            textBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && textBox1.Text != "")
            {
                string codeName = comboBox1.Text, actualCode = textBox1.Text, codeFile = codeDir + @"\" + codeName + ".txt";

                if (!File.Exists(codeFile))
                {
                    using (StreamWriter file = File.CreateText(codeFile))
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
                string codeName = comboBox1.Text, codeFile = codeDir + @"\" + codeName + ".txt"; ;

                if (File.Exists(codeFile))
                {
                    File.Delete(codeFile);

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
