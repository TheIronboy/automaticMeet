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
            if (Directory.Exists(@"C:\automaticMeet\" + sessionFile[0]) && sessionFile[2] == "True")
            {
                textBox1.Text = sessionFile[0];
                textBox2.Text = sessionFile[1];
                checkBox1.Checked = true;
            }
            else
            {
                File.Create(@"C:\automaticMeet\.session.txt").Close();
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
                textBox2.UseSystemPasswordChar = false;
                button1.Text = "Hide";
            }
            else if (passShown)
            {
                passShown = false;
                textBox2.UseSystemPasswordChar = true;
                button1.Text = "Show";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                string inputUsername = textBox1.Text, inputPassword = textBox2.Text, filePassword = "";

                if (Directory.Exists(@"C:\automaticMeet\" + inputUsername))
                {
                    using (StreamReader sr = File.OpenText(@"C:\automaticMeet\" + inputUsername + @"\password.txt"))
                    {
                        filePassword = sr.ReadLine();
                    }

                    if (inputPassword == filePassword)
                    {
                        using (StreamWriter sw = File.CreateText(@"C:\automaticMeet\.session.txt"))
                        {
                            sw.WriteLine(inputUsername);
                            sw.WriteLine(filePassword);

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
                        MessageBox.Show("Password errata.");
                }
                else
                    MessageBox.Show("Username non esistente.");
            }
            else
                MessageBox.Show("Riempi tutti i campi.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form accountManager = new accountManager();

            accountManager.Closed += (s, args) => this.Close();
            accountManager.Show();
        }
    }
}
