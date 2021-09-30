using System;
using System.IO;
using System.Windows.Forms;

namespace automaticMeet
{
    public partial class accountManager : Form
    {
        publicFunctions publicFunctionsRef = new publicFunctions();

        public accountManager()
        {
            InitializeComponent();
        }

        private void getUserListAndLoginData(string[] sessionData, ComboBox usersList, TextBox passwordText, CheckBox rememberMe)
        {
            string[] foundDirectory = Directory.GetDirectories(publicFunctionsRef.mainDir + @"\");
            usersList.Items.Clear();

            foreach (string directoryName in foundDirectory)
                usersList.Items.Add(Path.GetFileName(directoryName));

            if (Directory.Exists(publicFunctionsRef.mainDir + @"\" + sessionData[0]) && sessionData[2] == "True")
            {
                usersList.Text = sessionData[0];
                passwordText.Text = sessionData[1];
                rememberMe.Checked = true;
            }
            else
            {
                File.Create(publicFunctionsRef.sessionFile).Close();
                usersList.Text = "";
                passwordText.Text = "";
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(publicFunctionsRef.mainDir))
                Directory.CreateDirectory(publicFunctionsRef.mainDir);

            if (!File.Exists(publicFunctionsRef.sessionFile))
                File.Create(publicFunctionsRef.sessionFile).Close();

            getUserListAndLoginData(publicFunctionsRef.getSessionData(), comboBox1, textBox1, checkBox1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.UseSystemPasswordChar == false)
            {
                textBox1.UseSystemPasswordChar = true;
                button1.Text = "Show";
            }
            else if (textBox1.UseSystemPasswordChar == true)
            {
                textBox1.UseSystemPasswordChar = false;
                button1.Text = "Hide";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && textBox1.Text != "")
            {
                string inputUsername = comboBox1.Text, inputPassword = publicFunctionsRef.EncryptString(publicFunctionsRef.encryptionKey, textBox1.Text), accountDir = publicFunctionsRef.mainDir + @"\" + inputUsername;

                if (Directory.Exists(accountDir))
                {
                    using (StreamReader file = File.OpenText(accountDir + @"\password.txt"))
                    {
                        if (inputPassword != file.ReadLine())
                        {
                            MessageBox.Show("Password errata.");
                            file.Close();
                            return;
                        }
                        else
                            file.Close();
                    }
                }
                else if (!Directory.Exists(accountDir))
                {
                    if (inputUsername.IndexOf('@') != -1 && inputUsername.IndexOf('.') != -1 && inputUsername.IndexOf(' ') == -1)
                    {
                        Directory.CreateDirectory(accountDir);

                        using (StreamWriter file = File.CreateText(accountDir + @"\password.txt"))
                        {
                            file.WriteLine(inputPassword);
                            file.Close();
                        }

                        MessageBox.Show("Rilevato nuovo utente... mi raccomando, assicurati di aver inserito le credenziali giuste dell'account con cui vuoi collegarti alla riunione, detto ciò, Benvenuto!");
                    }
                    else
                    {
                        MessageBox.Show("Inserisci una mail valida.");
                        return;
                    }
                }

                using (StreamWriter file = File.CreateText(publicFunctionsRef.sessionFile))
                {
                    file.WriteLine(inputUsername);
                    file.WriteLine(inputPassword);

                    if (checkBox1.Checked)
                        file.WriteLine("True");
                    else if (!checkBox1.Checked)
                        file.WriteLine("False");

                    file.Close();
                }

                this.Hide();
                automaticMeet automaticMeetRef = new automaticMeet();
                automaticMeetRef.Closed += (s, args) => this.Close();
                automaticMeetRef.Show();
            }
            else
                MessageBox.Show("Riempi tutti i campi.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && textBox1.Text != "")
            {
                string inputUsername = comboBox1.Text, inputPassword = publicFunctionsRef.EncryptString(publicFunctionsRef.encryptionKey, textBox1.Text), accountDir = publicFunctionsRef.mainDir + @"\" + inputUsername;

                if (Directory.Exists(accountDir))
                {
                    using (StreamReader file = File.OpenText(accountDir + @"\password.txt"))
                    {
                        if (inputPassword == file.ReadLine())
                        {
                            file.Close();
                            Directory.Delete(accountDir, true);
                            File.Create(publicFunctionsRef.sessionFile).Close();

                            getUserListAndLoginData(publicFunctionsRef.getSessionData(), comboBox1, textBox1, checkBox1);
                            MessageBox.Show("Eliminato con successo!");
                        }
                        else
                        {
                            MessageBox.Show("Password errata.");
                            file.Close();
                            return;
                        }
                    }
                }
                else
                    MessageBox.Show("Account non esistente.");
            }
            else
                MessageBox.Show("Riempi tutti i campi.");
        }
    }
}
