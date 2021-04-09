using System;
using System.IO;
using System.Windows.Forms;

namespace automaticMeet
{
    public partial class accountManager : Form
    {
        public void loadSettings()
        {
            string[] foundDirectory = Directory.GetDirectories(@"C:\automaticMeet\");

            comboBox1.Items.Clear();

            foreach (string directoryName in foundDirectory)
                comboBox1.Items.Add(Path.GetFileName(directoryName));
        }

        public accountManager()
        {
            InitializeComponent();

            loadSettings();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && textBox1.Text != "" && textBox2.Text != "")
            {
                string newUsername = comboBox1.Text, newPass = textBox1.Text, confNewPass = textBox2.Text;

                if (newPass == confNewPass)
                {
                    if (!Directory.Exists(@"C:\automaticMeet\" + newUsername))
                    {
                        Directory.CreateDirectory(@"C:\automaticMeet\" + newUsername + @"\codes");

                        using (StreamWriter sw = File.CreateText(@"C:\automaticMeet\" + newUsername + @"\password.txt"))
                        {
                            sw.WriteLine(newPass);
                        }

                        File.Create(@"C:\automaticMeet\" + newUsername + @"\settings.txt").Close();

                        MessageBox.Show("Utente creato con successo!");

                        this.Hide();
                        Form login = new Login();

                        login.Closed += (s, args) => this.Close();
                        login.Show();
                    }
                    else
                        MessageBox.Show("Utente già esistente.");
                }
                else
                    MessageBox.Show("Le password non corrispondono.");
            }
            else
                MessageBox.Show("Riempi tutti i campi.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                string directoryName = comboBox1.Text;

                if (Directory.Exists(@"C:\automaticMeet\" + directoryName))
                {
                    Directory.Delete(@"C:\automaticMeet\" + directoryName, true);

                    loadSettings();

                    MessageBox.Show("Eliminato con successo!");

                    this.Hide();
                    Form login = new Login();

                    login.Closed += (s, args) => this.Close();
                    login.Show();
                }
                else
                    MessageBox.Show("Account non trovato.");
            }
            else
                MessageBox.Show("Seleziona un account.");
        }
    }
}
