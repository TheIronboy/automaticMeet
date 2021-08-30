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
        publicFunctions publicFunctionsRef = new publicFunctions();

        public bool isAutomated = false;
        public IWebDriver chrome;

        public NumericUpDown[] numericUpDowns;
        public CheckBox[] checkBoxes;
        public RadioButton[] radioButtons;

        string[] settingsFile = new string[13];

        public automaticMeet()
        {
            InitializeComponent();

            numericUpDowns = new NumericUpDown[2] { numericUpDown1, numericUpDown2 };
            checkBoxes = new CheckBox[5] { checkBox1, checkBox2, checkBox3, checkBox4, checkBox5 };
            radioButtons = new RadioButton[3] { radioButton1, radioButton2, radioButton3 };
        }

        private void setMode(bool isAutomated, bool modeToSet)
        {
            if (!isAutomated)
            {
                comboBox1.Enabled = modeToSet;

                checkBox1.Enabled = modeToSet;
                domainUpDown1.Enabled = modeToSet;

                foreach (RadioButton radioButton in radioButtons)
                    radioButton.Enabled = modeToSet;

                checkBox5.Enabled = modeToSet;
                textBox1.Enabled = modeToSet;

                progressBar1.Value = 0;
                button1.Enabled = modeToSet;
            }
            else if (isAutomated && modeToSet == true)
            {
                domainUpDown1.Enabled = false;
                textBox1.Enabled = false;

                checkBox4.Enabled = false;

                button1.Enabled = false;
                button1.Text = "AUTOMATICO.";
            }
            else if (isAutomated && modeToSet == false)
            {
                comboBox1.Enabled = false;

                checkBox1.Enabled = false;
                checkBox5.Enabled = false;

                foreach (RadioButton radioButton in radioButtons)
                    radioButton.Enabled = false;

                progressBar1.Value = 0;
            }
        }

        private void loadAutomaticMeetSettings(string[] sessionData, string[] settingsData, ComboBox codeText, DomainUpDown hourText, NumericUpDown[] numericUpDowns, CheckBox[] checkBoxes, RadioButton[] radioButtons, TextBox message)
        {
            using (StreamReader file = File.OpenText(@"C:\automaticMeet\" + sessionData[0] + @"\automaticMeetSettings.txt"))
            {
                for (int i = 0; i < settingsData.Length; i++)
                {
                    settingsData[i] = file.ReadLine();
                }

                file.Close();
            }

            codeText.Text = settingsData[0];
            hourText.Text = settingsData[1];

            int cont = 2;
            foreach (NumericUpDown numericUpDown in numericUpDowns)
            {
                numericUpDown.Value = Convert.ToInt32(settingsData[cont]);
                cont++;
            }

            foreach (CheckBox checkBox in checkBoxes)
            {
                checkBox.Checked = Convert.ToBoolean(settingsData[cont]);
                cont++;
            }

            message.Text = settingsData[9];

            cont = 10;
            foreach (RadioButton radioButton in radioButtons)
            {
                radioButton.Checked = Convert.ToBoolean(settingsData[cont]);
                cont++;
            }
        }

        private void saveAutomaticMeetSettings(string[] sessionData, string[] settingsData, ComboBox codeText, DomainUpDown hourText, NumericUpDown[] numericUpDowns, CheckBox[] checkBoxes, RadioButton[] radioButtons, TextBox message)
        {
            settingsData[0] = codeText.Text;
            settingsData[1] = hourText.Text;

            int cont = 2;
            foreach (NumericUpDown numericUpDown in numericUpDowns)
            {
                settingsData[cont] = numericUpDown.Value.ToString();
                cont++;
            }

            foreach (CheckBox checkBox in checkBoxes)
            {
                settingsData[cont] = checkBox.Checked.ToString();
                cont++;
            }

            settingsData[9] = message.Text;

            cont = 10;
            foreach (RadioButton radioButton in radioButtons)
            {
                settingsData[cont] = radioButton.Checked.ToString();
                cont++;
            }

            using (StreamWriter file = File.CreateText(@"C:\automaticMeet\" + sessionData[0] + @"\automaticMeetSettings.txt"))
            {
                for (int i = 0; i < settingsData.Length; i++)
                {
                    file.WriteLine(settingsData[i]);
                }
            }
        }

        public async void mainConnect(string email, string pass, string selectedCode, int speed, string messaggio, CheckBox[] checkBoxes, NumericUpDown[] numericUpDowns)
        {
            progressBar1.Maximum = 3;

            //crea instanza di chrome con impostazioni

            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            ChromeOptions options = new ChromeOptions();
            options.AddArguments("start-maximized", "disable-notifications", "use-fake-ui-for-media-stream");

            chrome = new ChromeDriver(service, options, TimeSpan.FromMinutes(5));

            // tenta il collegaento al sito di google meet e tenta il login

            try
            {
                chrome.Navigate().GoToUrl("https://meet.google.com/landing");

                progressBar1.Increment(1);

                chrome.FindElement(By.Id("identifierId")).SendKeys(email + OpenQA.Selenium.Keys.Enter);
                await Task.Delay(2000 * speed);

                progressBar1.Increment(1);

                chrome.FindElement(By.Name("password")).SendKeys(pass + OpenQA.Selenium.Keys.Enter);
                await Task.Delay(2000 * speed);

                progressBar1.Increment(1);
            }
            catch
            {
                if (chrome != null)
                {
                    chrome.Quit();
                    chrome = null;
                }

                if (!isAutomated)
                {
                    MessageBox.Show("Ho riscontrato un problema nella fase di login, assicurati di:" + Environment.NewLine +
                    "- Essere collegato ad internet." + Environment.NewLine +
                    "- Di avere chrome aggiornato all'ultima versione." + Environment.NewLine +
                    "- Di aver inserito le credenziali corrette." + Environment.NewLine +
                    "- Di aver disabilitato la verifica in due fattori nell'account google da cui stai facendo l'accesso. Usa questo link: https://myaccount.google.com/lesssecureapps" + Environment.NewLine +
                    "- Di aver attivato l'impostazione (Consenti app meno sicure) (Non consigliato per gli account principali, e non è assicurato il suo funzionamento).");

                    setMode(isAutomated, true);
                }

                return;
            }

            // entrata loop

            int count = 1;

            while (true)
            {
                try
                {
                    if (count != 1)
                        chrome.Navigate().GoToUrl("https://meet.google.com/landing");

                    progressBar1.Value = 0;
                    progressBar1.Maximum = 7;

                    // inserimento codice riunione

                    chrome.FindElement(By.Id("i3")).SendKeys(selectedCode + OpenQA.Selenium.Keys.Enter);
                    await Task.Delay(5000 * speed);

                    progressBar1.Increment(1);

                    if (checkBoxes[3].Checked)
                    {
                        // disattiva microfono

                        if (checkBoxes[1].Checked)
                        {
                            chrome.FindElement(By.XPath("/html/body/div[1]/c-wiz/div/div/div[9]/div[3]/div/div/div[4]/div/div/div[1]/div[1]/div/div[4]/div[1]")).Click();
                            await Task.Delay(1000 * speed);
                        }

                        progressBar1.Increment(1);

                        // disattiva camera

                        if (checkBoxes[2].Checked)
                        {
                            chrome.FindElement(By.XPath("/html/body/div[1]/c-wiz/div/div/div[9]/div[3]/div/div/div[4]/div/div/div[1]/div[1]/div/div[4]/div[2]")).Click();
                            await Task.Delay(1000 * speed);
                        }

                        progressBar1.Increment(1);

                        // clicca "partecipa"

                        chrome.FindElement(By.XPath("/html/body/div[1]/c-wiz/div/div/div[9]/div[3]/div/div/div[4]/div/div/div[2]/div/div[2]/div/div[1]/div[1]")).Click();
                        await Task.Delay(5000 * speed);

                        progressBar1.Increment(1);

                        // scrive messaggio in chat

                        if (checkBoxes[4].Checked)
                        {
                            chrome.FindElement(By.XPath("/html/body/div[1]/c-wiz/div[1]/div/div[9]/div[3]/div[10]/div[3]/div[2]/div/div/div[3]/span/button")).Click();
                            await Task.Delay(1000 * speed);

                            chrome.FindElement(By.Name("chatTextInput")).SendKeys(messaggio + OpenQA.Selenium.Keys.Enter);
                            await Task.Delay(1000 * speed);

                            chrome.FindElement(By.XPath("/html/body/div[1]/c-wiz/div[1]/div/div[9]/div[3]/div[10]/div[3]/div[2]/div/div/div[3]/span/button")).Click();
                        }

                        progressBar1.Increment(3);

                        // aspetta 15 minuti prima di iniziare a controllare il numero di partecipanti

                        progressBar1.Value = 0;
                        progressBar1.Maximum = 15;

                        for (int timer = 1; timer <= progressBar1.Maximum; timer++)
                        {
                            await Task.Delay(60000);
                            progressBar1.Increment(1);
                        }

                        progressBar1.Value = 1;
                        progressBar1.Maximum = 2;

                        // loop auto uscita

                        while (true)
                        {
                            //legge numero partecipanti

                            int nPart = Convert.ToInt32(chrome.FindElement(By.XPath("/html/body/div[1]/c-wiz/div[1]/div/div[9]/div[3]/div[10]/div[3]/div[2]/div/div/div[2]/div/div")).Text), quitAt = Convert.ToInt32(numericUpDowns[1].Value);

                            //quitta se il numero dei partecipanti e <= a quello inserito

                            if (nPart <= quitAt)
                            {
                                chrome.Quit();
                                chrome = null;

                                progressBar1.Increment(1);

                                if (!isAutomated)
                                    MessageBox.Show("Riunione terminata, connesso in " + count + " tentativi");

                                break;
                            }
                            else
                                await Task.Delay(5000);
                        }
                    }
                    else
                    {
                        if (checkBoxes[1].Checked)
                        {
                            chrome.FindElement(By.XPath("/html/body/div[1]/c-wiz/div/div/div[9]/div[3]/div/div/div[4]/div/div/div[1]/div[1]/div/div[4]/div[1]")).Click();
                            await Task.Delay(1000 * speed);
                        }

                        progressBar1.Increment(1);

                        if (checkBoxes[2].Checked)
                        {
                            chrome.FindElement(By.XPath("/html/body/div[1]/c-wiz/div/div/div[9]/div[3]/div/div/div[4]/div/div/div[1]/div[1]/div/div[4]/div[2]")).Click();
                            await Task.Delay(1000 * speed);
                        }

                        progressBar1.Increment(1);

                        chrome.FindElement(By.XPath("/html/body/div[1]/c-wiz/div/div/div[9]/div[3]/div/div/div[4]/div/div/div[2]/div/div[2]/div/div[1]/div[1]"));

                        progressBar1.Increment(4);

                        MessageBox.Show("Riunione iniziata, trovata in " + count + " tentativi, ATTENZIONE:" + Environment.NewLine +
                            "Quando la sessione di chrome è terminata, perfavore usare (Kill ChromeDriver.exe).");

                        break;
                    }
                }
                catch (NoSuchElementException)
                {
                    int minuti = Convert.ToInt32(numericUpDowns[0].Value);

                    progressBar1.Value = 0;
                    progressBar1.Maximum = minuti * 2;

                    for (int timer = 1; timer <= minuti * 2; timer++)
                    {
                        await Task.Delay(30000);
                        progressBar1.Increment(1);
                    }

                    count++;
                }
                catch
                {
                    if (chrome != null)
                    {
                        chrome.Quit();
                        chrome = null;
                    }

                    if (!isAutomated)
                        MessageBox.Show("ERRORE... Chrome terminato.");

                    break;
                }
            }

            if (!isAutomated)
                setMode(isAutomated, true);

            return;
        }

        private void automaticMeet_Load(object sender, EventArgs e)
        {
            setMode(isAutomated, true);

            publicFunctionsRef.getSessionFile();
            publicFunctionsRef.getCodesList(publicFunctionsRef.sessionFile, comboBox1);

            if (!File.Exists(@"C:\automaticMeet\" + publicFunctionsRef.sessionFile[0] + @"\automaticMeetSettings.txt") && !isAutomated)
                saveAutomaticMeetSettings(publicFunctionsRef.sessionFile, settingsFile, comboBox1, domainUpDown1, numericUpDowns, checkBoxes, radioButtons, textBox1);
            else
                loadAutomaticMeetSettings(publicFunctionsRef.sessionFile, settingsFile, comboBox1, domainUpDown1, numericUpDowns, checkBoxes, radioButtons, textBox1);

            checkBox1_CheckedChanged(e, e);
            checkBox5_CheckedChanged(e, e);
        }

        public void button1_Click(object sender, EventArgs e)
        {
            string selectedCode = "";

            if (comboBox1.Text != "")
            {
                using (StreamReader file = File.OpenText(@"C:\automaticMeet\" + publicFunctionsRef.sessionFile[0] + @"\codes\" + comboBox1.Text + ".txt"))
                {
                    selectedCode = file.ReadLine();
                    file.Close();
                }
            }
            else
            {
                MessageBox.Show("Seleziona un codice valido oppure creane uno nuovo.");
                return;
            }

            if (checkBox1.Checked && domainUpDown1.Text != "")
                selectedCode += "-" + domainUpDown1.Text;
            else if (checkBox1.Checked && domainUpDown1.Text == "")
            {
                MessageBox.Show("Inserisci una stringa da aggiungere.");
                return;
            }

            int speed = 0;
            if (radioButton3.Checked)
                speed = 1;
            else if (radioButton2.Checked)
                speed = 2;
            else if (radioButton1.Checked)
                speed = 4;

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

            setMode(isAutomated, false);

            if (!isAutomated)
                saveAutomaticMeetSettings(publicFunctionsRef.sessionFile, settingsFile, comboBox1, domainUpDown1, numericUpDowns, checkBoxes, radioButtons, textBox1);


            mainConnect(publicFunctionsRef.sessionFile[0], publicFunctionsRef.sessionFile[1], selectedCode, speed, messaggio, checkBoxes, numericUpDowns);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!isAutomated)
                domainUpDown1.Enabled = checkBox1.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (!isAutomated)
                textBox1.Enabled = checkBox5.Checked;
        }

        private void gestisciCodiciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isAutomated)
            {
                saveAutomaticMeetSettings(publicFunctionsRef.sessionFile, settingsFile, comboBox1, domainUpDown1, numericUpDowns, checkBoxes, radioButtons, textBox1);

                codeManager codeManagerRef = new codeManager();
                codeManagerRef.ShowDialog();

                publicFunctionsRef.getCodesList(publicFunctionsRef.sessionFile, comboBox1);

                loadAutomaticMeetSettings(publicFunctionsRef.sessionFile, settingsFile, comboBox1, domainUpDown1, numericUpDowns, checkBoxes, radioButtons, textBox1);
            }
            else
                MessageBox.Show("Impossibile modificare i codici mentre automatizzato.");
        }

        private void programmaMessaggiOrariToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isAutomated)
            {
                saveAutomaticMeetSettings(publicFunctionsRef.sessionFile, settingsFile, comboBox1, domainUpDown1, numericUpDowns, checkBoxes, radioButtons, textBox1);
                this.Hide();

                automationManager automationManagerRef = new automationManager();
                automationManagerRef.Closed += (s, args) => this.Close();
                automationManagerRef.Show();
            }
            else
                MessageBox.Show("Hey, che stai provando a fare amico? :)");
        }

        private void killChromeDriverexeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (chrome != null)
            {
                chrome.Quit();
                chrome = null;

                MessageBox.Show("Chrome terminato.");

                if (!isAutomated)
                    setMode(isAutomated, true);
            }
            else
                MessageBox.Show("ChromeDriver non inizializzato.");
        }

        int easterEggCount = 0;

        private void creditiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            easterEggCount++;

            if (easterEggCount == 10)
            {
                MessageBox.Show("Never gonna give you up.");

                easterEggCount = 0;
            }
            else
                MessageBox.Show("Idea by Misael Canova. Developed by Luca Giordano.");
        }
    }
}
