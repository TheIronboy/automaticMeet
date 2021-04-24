using System;
using System.IO;
using System.Windows.Forms;

namespace automaticMeet
{
    public partial class Login : Form
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
            string[] foundDirectory = Directory.GetDirectories(@"C:\automaticMeet\");

            comboBox1.Items.Clear();

            foreach (string directoryName in foundDirectory)
                comboBox1.Items.Add(Path.GetFileName(directoryName));

            if (Directory.Exists(@"C:\automaticMeet\" + sessionFile[0]) && sessionFile[2] == "True")
            {
                comboBox1.Text = sessionFile[0];
                textBox1.Text = sessionFile[1];
                checkBox1.Checked = true;
            }
            else
            {
                File.Create(@"C:\automaticMeet\.session.txt").Close();
                comboBox1.Text = "";
                textBox1.Text = "";
            }
        }

        public Login()
        {
            InitializeComponent();

            if (!Directory.Exists(@"C:\automaticMeet"))
                Directory.CreateDirectory(@"C:\automaticMeet");

            if (!File.Exists(@"C:\automaticMeet\.session.txt"))
                File.Create(@"C:\automaticMeet\.session.txt").Close();

            getSession();

            loadSettings();
        }

        bool passShown = false;

        private void button1_Click(object sender, EventArgs e)
        {
            if (!passShown)
            {
                passShown = true;
                textBox1.UseSystemPasswordChar = false;
                button1.Text = "Hide";
            }
            else if (passShown)
            {
                passShown = false;
                textBox1.UseSystemPasswordChar = true;
                button1.Text = "Show";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && textBox1.Text != "")
            {
                string inputUsername = comboBox1.Text, inputPassword = textBox1.Text, filePassword = "";

                if (Directory.Exists(@"C:\automaticMeet\" + inputUsername))
                {
                    using (StreamReader sr = File.OpenText(@"C:\automaticMeet\" + inputUsername + @"\password.txt"))
                    {
                        filePassword = sr.ReadLine();
                    }

                    if (inputPassword != filePassword)
                    {
                        MessageBox.Show("Password errata.");
                        return;
                    }
                }
                else if (!Directory.Exists(@"C:\automaticMeet\" + inputUsername))
                {
                    Directory.CreateDirectory(@"C:\automaticMeet\" + inputUsername + @"\codes");

                    using (StreamWriter sw = File.CreateText(@"C:\automaticMeet\" + inputUsername + @"\password.txt"))
                    {
                        sw.WriteLine(inputPassword);
                    }

                    File.Create(@"C:\automaticMeet\" + inputUsername + @"\settings.txt").Close();

                    MessageBox.Show("Rilevato nuovo utente, benvenuto!");
                }

                using (StreamWriter sw = File.CreateText(@"C:\automaticMeet\.session.txt"))
                {
                    sw.WriteLine(inputUsername);
                    sw.WriteLine(inputPassword);

                    if (checkBox1.Checked)
                        sw.WriteLine("True");
                    else if (!checkBox1.Checked)
                        sw.WriteLine("False");
                }

                this.Hide();
                Form automaticMeet = new automaticMeet();

                automaticMeet.Closed += (s, args) => this.Close();
                automaticMeet.Show();
            }
            else
                MessageBox.Show("Riempi tutti i campi.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && textBox1.Text != "")
            {
                string inputUsername = comboBox1.Text, inputPassword = textBox1.Text, filePassword = "";

                if (Directory.Exists(@"C:\automaticMeet\" + inputUsername))
                {
                    using (StreamReader sr = File.OpenText(@"C:\automaticMeet\" + inputUsername + @"\password.txt"))
                    {
                        filePassword = sr.ReadLine();
                    }

                    if (inputPassword == filePassword)
                    {
                        Directory.Delete(@"C:\automaticMeet\" + inputUsername, true);

                        File.Create(@"C:\automaticMeet\.session.txt").Close();

                        getSession();
                        loadSettings();

                        MessageBox.Show("Eliminato con successo!");
                    }
                    else
                    {
                        MessageBox.Show("Password errata.");
                        return;
                    }
                }
                else
                    MessageBox.Show("Account non trovato.");
            }
            else
                MessageBox.Show("Seleziona un account.");
        }
    }
}
