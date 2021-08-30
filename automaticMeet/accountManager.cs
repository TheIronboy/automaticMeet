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
            string[] foundDirectory = Directory.GetDirectories(@"C:\automaticMeet\");

            usersList.Items.Clear();

            foreach (string directoryName in foundDirectory)
                usersList.Items.Add(Path.GetFileName(directoryName));

            if (Directory.Exists(@"C:\automaticMeet\" + sessionData[0]) && sessionData[2] == "True")
            {
                usersList.Text = sessionData[0];
                passwordText.Text = sessionData[1];
                rememberMe.Checked = true;
            }
            else
            {
                File.Create(@"C:\automaticMeet\.session.txt").Close();
                usersList.Text = "";
                passwordText.Text = "";
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(@"C:\automaticMeet"))
                Directory.CreateDirectory(@"C:\automaticMeet");

            if (!File.Exists(@"C:\automaticMeet\.session.txt"))
                File.Create(@"C:\automaticMeet\.session.txt").Close();

            publicFunctionsRef.getSessionFile();
            getUserListAndLoginData(publicFunctionsRef.sessionFile, comboBox1, textBox1, checkBox1);
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
                string inputUsername = comboBox1.Text, inputPassword = textBox1.Text, filePassword = "";

                inputPassword = publicFunctionsRef.EncryptString(publicFunctionsRef.encryptionKey, inputPassword);

                if (Directory.Exists(@"C:\automaticMeet\" + inputUsername))
                {
                    using (StreamReader file = File.OpenText(@"C:\automaticMeet\" + inputUsername + @"\password.txt"))
                    {
                        filePassword = file.ReadLine();
                        file.Close();
                    }

                    if (inputPassword != filePassword)
                    {
                        MessageBox.Show("Password errata.");
                        return;
                    }
                }
                else if (!Directory.Exists(@"C:\automaticMeet\" + inputUsername))
                {
                    if (inputUsername.IndexOf('@') != -1 && inputUsername.IndexOf(' ') == -1)
                    {
                        Directory.CreateDirectory(@"C:\automaticMeet\" + inputUsername);

                        using (StreamWriter file = File.CreateText(@"C:\automaticMeet\" + inputUsername + @"\password.txt"))
                        {
                            file.WriteLine(inputPassword);
                            file.Close();
                        }

                        MessageBox.Show("Rilevato nuovo utente, benvenuto!");
                    }
                    else
                    {
                        MessageBox.Show("Inserisci una mail valida.");
                        return;
                    }
                }

                using (StreamWriter file = File.CreateText(@"C:\automaticMeet\.session.txt"))
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
                string inputUsername = comboBox1.Text, inputPassword = textBox1.Text, filePassword = "";
                inputPassword = publicFunctionsRef.EncryptString(publicFunctionsRef.encryptionKey, inputPassword);

                if (Directory.Exists(@"C:\automaticMeet\" + inputUsername))
                {
                    using (StreamReader file = File.OpenText(@"C:\automaticMeet\" + inputUsername + @"\password.txt"))
                    {
                        filePassword = file.ReadLine();
                        file.Close();
                    }

                    if (inputPassword == filePassword)
                    {
                        Directory.Delete(@"C:\automaticMeet\" + inputUsername, true);
                        File.Create(@"C:\automaticMeet\.session.txt").Close();

                        getUserListAndLoginData(publicFunctionsRef.sessionFile, comboBox1, textBox1, checkBox1);
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
