using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace automaticMeet
{
    public partial class automaticMeet : Form
    {
        string[] sessionFile = new string[3];

        public void getSessionAndCodes()
        {
            //carica nome utente loggato

            using (StreamReader sr = File.OpenText(@"C:\automaticMeet\.session.txt"))
            {
                for (int i = 0; i < sessionFile.Length; i++)
                {
                    sessionFile[i] = sr.ReadLine();
                }
            }

            //aggiunge classi esistenti all'interno del combobox.

            string[] fileEntries = Directory.GetFiles(@"C:\automaticMeet\" + sessionFile[0] + @"\codes");

            comboBox1.Items.Clear();

            foreach (string fileName in fileEntries)
                comboBox1.Items.Add(Path.GetFileName(fileName.Remove(fileName.Length - 4)));
        }

        string[] settingsFile = new string[14];

        public void loadSettings()
        {
            //carica file settings.txt dell'utente loggato

            if (File.Exists(@"C:\automaticMeet\" + sessionFile[0] + @"\settings.txt"))
            {
                using (StreamReader sr = File.OpenText(@"C:\automaticMeet\" + sessionFile[0] + @"\settings.txt"))
                {
                    for (int i = 0; i < settingsFile.Length; i++)
                    {
                        settingsFile[i] = sr.ReadLine();
                    }
                }
            }

            //carica file settings.txt

            comboBox1.Text = settingsFile[0];

            if (settingsFile[1] != null)
                domainUpDown1.Text = settingsFile[1];
            else
                domainUpDown1.Text = "1";

            if (settingsFile[2] != null)
                numericUpDown1.Value = Convert.ToInt32(settingsFile[2]);
            else
                numericUpDown1.Value = 1;

            if (settingsFile[3] != null)
                numericUpDown2.Value = Convert.ToInt32(settingsFile[3]);
            else
                numericUpDown2.Value = 5;

            checkBox1.Checked = Convert.ToBoolean(settingsFile[4]);
            checkBox2.Checked = Convert.ToBoolean(settingsFile[5]);
            checkBox3.Checked = Convert.ToBoolean(settingsFile[7]);
            checkBox4.Checked = Convert.ToBoolean(settingsFile[8]);
            checkBox5.Checked = Convert.ToBoolean(settingsFile[9]);

            textBox1.Text = settingsFile[10];

            radioButton1.Checked = Convert.ToBoolean(settingsFile[11]);
            radioButton2.Checked = Convert.ToBoolean(settingsFile[12]);
            radioButton3.Checked = Convert.ToBoolean(settingsFile[13]);

            if (!radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked)
                radioButton2.Checked = true;
        }

        public void saveSettings()
        {
            //assegna nuovi valori al file settings.txt

            settingsFile[0] = comboBox1.Text;

            settingsFile[1] = domainUpDown1.Text;
            settingsFile[2] = numericUpDown1.Value.ToString();
            settingsFile[3] = numericUpDown2.Value.ToString();

            settingsFile[4] = checkBox1.Checked.ToString();
            settingsFile[5] = checkBox2.Checked.ToString();
            settingsFile[7] = checkBox3.Checked.ToString();
            settingsFile[8] = checkBox4.Checked.ToString();
            settingsFile[9] = checkBox5.Checked.ToString();

            settingsFile[10] = textBox1.Text;

            settingsFile[11] = radioButton1.Checked.ToString();
            settingsFile[12] = radioButton2.Checked.ToString();
            settingsFile[13] = radioButton3.Checked.ToString();

            //scrive file settings.txt

            using (StreamWriter sw = File.CreateText(@"C:\automaticMeet\" + sessionFile[0] + @"\settings.txt"))
            {
                for (int i = 0; i < settingsFile.Length; i++)
                {
                    sw.WriteLine(settingsFile[i]);
                }
            }
        }

        public async void mainConnect(string email, string pass, string selectedCode, string messaggio, int speed)
        {
            progressBar1.Value = 0;
            progressBar1.Maximum = 3;
            button1.Enabled = false;

            //crea instanza di chrome con impostazioni

            ChromeOptions options = new ChromeOptions();
            options.AddArguments("start-maximized", "--disable-notifications", "use-fake-ui-for-media-stream");

            IWebDriver chrome = new ChromeDriver(options);
            chrome.Navigate().GoToUrl("https://meet.google.com/landing");

            progressBar1.Increment(1);

            // fase di login

            try
            {
                chrome.FindElement(By.Id("identifierId")).SendKeys(email + OpenQA.Selenium.Keys.Enter);
                await Task.Delay(2000 * speed);

                progressBar1.Increment(1);

                chrome.FindElement(By.Name("password")).SendKeys(pass + OpenQA.Selenium.Keys.Enter);
                await Task.Delay(2000 * speed);

                progressBar1.Increment(1);
            }
            catch (NoSuchElementException)
            {
                MessageBox.Show("Ho riscontrato un problema nella fase di login, assicurati di aver attivato l'impostazione (Consenti app meno sicure) (Non lo consiglio per gli account principali) e di aver disabilitato la verifica in due fattori nell'account google da cui stai facendo l'accesso. Usa questo link: https://myaccount.google.com/lesssecureapps");
                button1.Enabled = true;
                progressBar1.Value = 0;
                return;
            }

            // entrata loop

            bool stop = false;

            while (stop == false)
            {
                progressBar1.Value = 0;
                progressBar1.Maximum = 7;

                // inserimento codice riunione

                chrome.FindElement(By.Id("i3")).SendKeys(selectedCode + OpenQA.Selenium.Keys.Enter); ;
                await Task.Delay(5000 * speed);

                progressBar1.Increment(1);

                if (checkBox4.Checked)
                {
                    try
                    {
                        // disattiva microfono

                        if (checkBox2.Checked)
                        {
                            chrome.FindElement(By.XPath("/html/body/div[1]/c-wiz/div/div/div[9]/div[3]/div/div/div[2]/div/div[1]/div[1]/div[1]/div/div[4]/div[1]")).Click();
                            await Task.Delay(1000 * speed);

                            progressBar1.Increment(1);
                        }
                        else
                            progressBar1.Increment(1);

                        // disattiva camera

                        if (checkBox3.Checked)
                        {
                            chrome.FindElement(By.XPath("/html/body/div[1]/c-wiz/div/div/div[9]/div[3]/div/div/div[2]/div/div[1]/div[1]/div[1]/div/div[4]/div[2]")).Click();
                            await Task.Delay(1000 * speed);

                            progressBar1.Increment(1);
                        }
                        else
                            progressBar1.Increment(1);

                        // clicca "partecipa"

                        chrome.FindElement(By.XPath("/html/body/div[1]/c-wiz/div/div/div[9]/div[3]/div/div/div[2]/div/div[1]/div[2]/div/div[2]/div/div[1]/div[1]")).Click();
                        await Task.Delay(5000 * speed);

                        progressBar1.Increment(1);

                        // scrive messaggio in chat

                        if (checkBox5.Checked)
                        {
                            chrome.FindElement(By.XPath("/html/body/div[1]/c-wiz/div[1]/div/div[9]/div[3]/div[1]/div[3]/div/div[2]/div[3]")).Click();
                            await Task.Delay(1000 * speed);

                            progressBar1.Increment(1);

                            chrome.FindElement(By.Name("chatTextInput")).SendKeys(messaggio + OpenQA.Selenium.Keys.Enter);
                            await Task.Delay(1000 * speed);

                            progressBar1.Increment(1);

                            chrome.FindElement(By.XPath("/html/body/div[1]/c-wiz/div[1]/div/div[9]/div[3]/div[4]/div/div[2]/div[1]/div[2]/div/span/button")).Click();

                            progressBar1.Increment(1);
                        }
                        else
                            progressBar1.Increment(3);

                        // aspetta 15 minuti prima di iniziare a controllare il numero di partecipanti

                        progressBar1.Value = 0;
                        progressBar1.Maximum = 15;

                        for (int timer = 1; timer <= 15; timer++)
                        {
                            await Task.Delay(60000);
                            progressBar1.Increment(1);
                        }

                        progressBar1.Value = 1;
                        progressBar1.Maximum = 2;

                        // loop auto uscita

                        bool quit = false;

                        while (quit == false)
                        {
                            try
                            {
                                int nPart = Convert.ToInt32(chrome.FindElement(By.XPath("/html/body/div[1]/c-wiz/div[1]/div/div[9]/div[3]/div[1]/div[3]/div/div[2]/div[1]/span/span/div/div/span[2]")).Text), quitAt = Convert.ToInt32(numericUpDown2.Value);

                                if (nPart <= quitAt)
                                {
                                    quit = true;
                                    chrome.Navigate().GoToUrl("https://meet.google.com/landing");

                                    button1.Enabled = true;
                                    progressBar1.Increment(1);

                                    MessageBox.Show("Finito.");
                                }
                                else
                                    await Task.Delay(1000);
                            }
                            catch (FormatException)
                            {
                                await Task.Delay(1000);
                            }
                        }

                        stop = true;
                    }
                    catch (NoSuchElementException)
                    {
                        chrome.Navigate().GoToUrl("https://meet.google.com/landing");

                        int minuti = Convert.ToInt32(numericUpDown1.Value);

                        progressBar1.Value = 0;
                        progressBar1.Maximum = minuti * 2;

                        for (int timer = 1; timer <= minuti * 2; timer++)
                        {
                            await Task.Delay(30000);
                            progressBar1.Increment(1);
                        }
                    }
                }
                else
                {
                    int minuti = Convert.ToInt32(numericUpDown1.Value);

                    progressBar1.Value = 0;
                    progressBar1.Maximum = minuti * 2;

                    for (int timer = 1; timer <= minuti * 2; timer++)
                    {
                        await Task.Delay(30000);
                        progressBar1.Increment(1);
                    }

                    chrome.Navigate().GoToUrl("https://meet.google.com/landing");
                }
            }
        }

        public automaticMeet()
        {
            InitializeComponent();

            getSessionAndCodes();

            loadSettings();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            saveSettings();

            if (comboBox1.Text != "")
            {
                //cerca codice inserito nel combobox

                string selectedCode = "";
                using (StreamReader sr = File.OpenText(@"C:\automaticMeet\" + sessionFile[0] + @"\codes\" + comboBox1.Text + ".txt"))
                {
                    selectedCode = sr.ReadLine();
                }

                //se "aggiungi" è attivo, aggiungi - numero al codice

                if (checkBox1.Checked && domainUpDown1.Text != "")
                    selectedCode += "-" + domainUpDown1.Text;
                else
                {
                    MessageBox.Show("Inserisci una stringa da aggiungere.");
                    return;
                }

                //se (Auto-Join/Exit) è attivo e messaggio programmato è attivo, messaggio = textbox1.text

                string messaggio = "";
                if (checkBox4.Checked && checkBox5.Checked)
                {
                    if (textBox1.Text != "")
                        messaggio = textBox1.Text;
                    else
                    {
                        MessageBox.Show("Inserisci il messaggio.");
                        return;
                    }
                }
                else if (!checkBox4.Checked && checkBox5.Checked)
                {
                    MessageBox.Show("Devi attivare (Auto-Join/Exit) per inviare un messaggio in chat.");
                    return;
                }

                // carica la velocità selezionata

                int speed = 0;
                if (radioButton1.Checked)
                    speed = 1;
                else if (radioButton2.Checked)
                    speed = 2;
                else if (radioButton3.Checked)
                    speed = 4;

                // se (Auto-Join/Exit) non è attiva, disattiva camera e microfono non disponibili.

                if ((checkBox2.Checked || checkBox3.Checked) && !checkBox4.Checked)
                {
                    MessageBox.Show("Attenzione! Le opzioni (Disattiva Camera) e (Disattiva Microfono) non sono disponibili se (Auto-Join/Exit) è disabilitato.");

                    checkBox2.Checked = false;
                    checkBox3.Checked = false;
                }

                mainConnect(sessionFile[0], sessionFile[1], selectedCode, messaggio, speed);
            }
            else
                MessageBox.Show("Seleziona un codice valido oppure creane uno nuovo.");
        }

        private void gestisciCodiciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveSettings();

            Form codeManager = new codeManager();
            codeManager.ShowDialog();

            getSessionAndCodes();
        }

        int i = 0;

        private void creditiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            i++;

            if (i == 10)
            {
                MessageBox.Show("Android pattumiera");
                i = 0;
            }
            else
                MessageBox.Show("Idea by Misael Canova. Developed by Luca Giordano.");
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming Soon");
        }

        private void programmaMessaggiOrariToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming Soon");
        }
    }
}
