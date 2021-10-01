using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace automaticMeet
{
    public partial class automationManager : Form
    {
        publicFunctions publicFunctionsRef = new publicFunctions();
        string automationsSettingsFile;

        automaticMeet automaticMeetRef;
        bool isHidden = true;

        CheckBox[] enabledCheckBoxes, addStringCheckBoxes;
        NumericUpDown[] numericUpDownsHours, numericUpDownsMinutes;
        ComboBox[] comboBoxes;
        DomainUpDown[] domainUpDowns;
        TextBox[] textBoxes;

        public automationManager()
        {
            InitializeComponent();

            enabledCheckBoxes = new CheckBox[10] { checkBox1, checkBox2, checkBox3, checkBox4, checkBox5, checkBox6, checkBox7, checkBox8, checkBox9, checkBox10 };
            addStringCheckBoxes = new CheckBox[10] { checkBox11, checkBox12, checkBox13, checkBox14, checkBox15, checkBox16, checkBox17, checkBox18, checkBox19, checkBox20 };
            numericUpDownsHours = new NumericUpDown[10] { numericUpDown1, numericUpDown3, numericUpDown5, numericUpDown7, numericUpDown9, numericUpDown11, numericUpDown13, numericUpDown15, numericUpDown17, numericUpDown19 };
            numericUpDownsMinutes = new NumericUpDown[10] { numericUpDown2, numericUpDown4, numericUpDown6, numericUpDown8, numericUpDown10, numericUpDown12, numericUpDown14, numericUpDown16, numericUpDown18, numericUpDown20 };
            comboBoxes = new ComboBox[10] { comboBox1, comboBox2, comboBox3, comboBox4, comboBox5, comboBox6, comboBox7, comboBox8, comboBox9, comboBox10 };
            domainUpDowns = new DomainUpDown[10] { domainUpDown1, domainUpDown2, domainUpDown3, domainUpDown4, domainUpDown5, domainUpDown6, domainUpDown7, domainUpDown8, domainUpDown9, domainUpDown10 };
            textBoxes = new TextBox[10] { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10 };
        }

        private void setAutomationsStatus(CheckBox enabledcheckBox, CheckBox addStringCheckBoxes, NumericUpDown hour, NumericUpDown minute, ComboBox codeText, DomainUpDown stringToAdd, TextBox messages)
        {
            addStringCheckBoxes.Enabled = enabledcheckBox.Checked;

            hour.Enabled = enabledcheckBox.Checked;
            minute.Enabled = enabledcheckBox.Checked;

            codeText.Enabled = enabledcheckBox.Checked;

            if (enabledcheckBox.Checked && addStringCheckBoxes.Checked)
                stringToAdd.Enabled = true;
            else
                stringToAdd.Enabled = false;

            messages.Enabled = enabledcheckBox.Checked;
        }

        private string hourAndMinutesExtractor(int type, string value)
        {
            string hour = "";
            int pos = 0;

            while (value[pos].ToString().IndexOf(":") == -1)
            {
                hour += value[pos];
                pos++;
            }

            string minutes = "";
            pos++;

            while (pos < value.Length)
            {
                minutes += value[pos];
                pos++;
            }

            string result = "";

            if (type == 0)
                result = hour;
            else if (type == 1)
                result = minutes;

            if (result.Length == 1)
                result = "0" + result;

            return result;
        }

        private void loadAutomationsSettings(CheckBox[] enabledCheckBoxes, CheckBox[] addStringCheckBoxes, NumericUpDown[] numericUpDownsHours, NumericUpDown[] numericUpDownsMinutes, ComboBox[] comboBoxes, DomainUpDown[] stringToAdd, TextBox[] textBoxes)
        {
            string[,] settingsData = new string[10, 6];

            using (StreamReader file = File.OpenText(automationsSettingsFile))
            {
                for (int i = 0; i < settingsData.GetLength(0); i++)
                    for (int k = 0; k < settingsData.GetLength(1); k++)
                        settingsData[i, k] = file.ReadLine();

                file.Close();
            }

            for (int i = 0; i < settingsData.GetLength(0); i++)
            {
                enabledCheckBoxes[i].Checked = Convert.ToBoolean(settingsData[i, 0]);
                addStringCheckBoxes[i].Checked = Convert.ToBoolean(settingsData[i, 1]);
                numericUpDownsHours[i].Value = Convert.ToInt32(hourAndMinutesExtractor(0, settingsData[i, 2]));
                numericUpDownsMinutes[i].Value = Convert.ToInt32(hourAndMinutesExtractor(1, settingsData[i, 2]));
                comboBoxes[i].Text = settingsData[i, 3];
                stringToAdd[i].Text = settingsData[i, 4];
                textBoxes[i].Text = settingsData[i, 5];
            }
        }

        private void saveAutomationsSettings(CheckBox[] enabledCheckBoxes, CheckBox[] addStringCheckBoxes, NumericUpDown[] numericUpDownsHours, NumericUpDown[] numericUpDownsMinutes, ComboBox[] comboBoxes, DomainUpDown[] stringToAdd, TextBox[] textBoxes)
        {
            string[,] settingsData = new string[10, 6];

            for (int i = 0; i < settingsData.GetLength(0); i++)
            {
                settingsData[i, 0] = enabledCheckBoxes[i].Checked.ToString();
                settingsData[i, 1] = addStringCheckBoxes[i].Checked.ToString();
                settingsData[i, 2] = numericUpDownsHours[i].Value.ToString() + ":" + numericUpDownsMinutes[i].Value.ToString();
                settingsData[i, 3] = comboBoxes[i].Text;
                settingsData[i, 4] = stringToAdd[i].Text;
                settingsData[i, 5] = textBoxes[i].Text;
            }

            using (StreamWriter file = File.CreateText(automationsSettingsFile))
            {
                for (int i = 0; i < settingsData.GetLength(0); i++)
                    for (int k = 0; k < settingsData.GetLength(1); k++)
                        file.WriteLine(settingsData[i, k]);

                file.Close();
            }
        }

        private void automationManager_Load(object sender, EventArgs e)
        {
            automationsSettingsFile = publicFunctionsRef.mainDir + publicFunctionsRef.getSessionData()[0] + @"\automationsSettings.txt";

            for (int i = 0; i < enabledCheckBoxes.Length; i++)
            {
                publicFunctionsRef.getCodeList(comboBoxes[i]);
                setAutomationsStatus(enabledCheckBoxes[i], addStringCheckBoxes[i], numericUpDownsHours[i], numericUpDownsMinutes[i], comboBoxes[i], domainUpDowns[i], textBoxes[i]);
            }

            if (!File.Exists(automationsSettingsFile))
                saveAutomationsSettings(enabledCheckBoxes, addStringCheckBoxes, numericUpDownsHours, numericUpDownsMinutes, comboBoxes, domainUpDowns, textBoxes);
            else
                loadAutomationsSettings(enabledCheckBoxes, addStringCheckBoxes, numericUpDownsHours, numericUpDownsMinutes, comboBoxes, domainUpDowns, textBoxes);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            int cont = 0;

            foreach (ComboBox comboBox in comboBoxes)
            {
                if (comboBox.Enabled)
                    cont++;

                foreach (CheckBox checkBox in addStringCheckBoxes)
                {
                    foreach (DomainUpDown domainUpDown in domainUpDowns)
                    {
                        if (comboBox.Enabled && comboBox.Text == "")
                        {
                            MessageBox.Show("Seleziona un codice valido oppure creane uno nuovo.");
                            return;
                        }

                        if (domainUpDown.Enabled && checkBox.Checked && domainUpDown.Text == "")
                        {
                            MessageBox.Show("Inserisci una stringa da aggiungere.");
                            return;
                        }
                    }
                }
            }

            if (cont == 0)
            {
                MessageBox.Show("Nessuna automazione abilitata, attivane almeno una oppure chiudi il programma.");
                return;
            }

            this.Hide();
            saveAutomationsSettings(enabledCheckBoxes, addStringCheckBoxes, numericUpDownsHours, numericUpDownsMinutes, comboBoxes, domainUpDowns, textBoxes);

            for (int i = 0; i < enabledCheckBoxes.Length; i++)
            {
                if (enabledCheckBoxes[i].Checked)
                {
                    automaticMeetRef = new automaticMeet();
                    automaticMeetRef.isAutomated = true;
                    automaticMeetRef.Show();

                    automaticMeetRef.comboBox1.Text = comboBoxes[i].Text;
                    automaticMeetRef.checkBoxes[3].Checked = true;

                    automaticMeetRef.checkBoxes[0].Checked = addStringCheckBoxes[i].Checked;
                    automaticMeetRef.domainUpDown1.Text = domainUpDowns[i].Text;

                    if (domainUpDown1.Text == "")
                    {
                        automaticMeetRef.checkBoxes[0].Checked = false;
                        automaticMeetRef.checkBoxes[0].Enabled = false;
                    }

                    automaticMeetRef.checkBoxes[4].Checked = true;
                    automaticMeetRef.textBox1.Text = textBoxes[i].Text;

                    if (textBoxes[i].Text == "")
                    {
                        automaticMeetRef.checkBoxes[4].Enabled = false;
                        automaticMeetRef.checkBoxes[4].Checked = false;
                    }

                    automaticMeetRef.Hide();
                    notifyIcon1.Visible = true;

                    while (true)
                    {
                        if (automaticMeetRef.forceTerminate)
                            this.Close();

                        if (DateTime.Now.Hour == numericUpDownsHours[i].Value && DateTime.Now.Minute >= numericUpDownsMinutes[i].Value)
                        {
                            if (isHidden)
                                automaticMeetRef.Show();

                            automaticMeetRef.button1_Click(e, e);

                            while (true)
                            {
                                if (automaticMeetRef.chrome == null)
                                    break;
                                else
                                    await Task.Delay(15000);
                            }

                            automaticMeetRef.Close();
                            break;
                        }
                        else
                            await Task.Delay(30000);
                    }
                }
            }

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            codeManager codeManagerRef = new codeManager();
            codeManagerRef.ShowDialog();

            automationManager_Load(e, e);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (isHidden && !automaticMeetRef.forceTerminate)
            {
                automaticMeetRef.Show();
                isHidden = false;
            }
            else if (!isHidden && !automaticMeetRef.forceTerminate)
            {
                automaticMeetRef.Hide();
                isHidden = true;
            }
            else
                MessageBox.Show("Automazione terminata forzatamente, impossibile aprire." + Environment.NewLine + "Terminazione dei processi in corso...");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            setAutomationsStatus(checkBox1, checkBox11, numericUpDown1, numericUpDown2, comboBox1, domainUpDown1, textBox1);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            setAutomationsStatus(checkBox2, checkBox12, numericUpDown3, numericUpDown4, comboBox2, domainUpDown2, textBox2);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            setAutomationsStatus(checkBox3, checkBox13, numericUpDown5, numericUpDown6, comboBox3, domainUpDown3, textBox3);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            setAutomationsStatus(checkBox4, checkBox14, numericUpDown7, numericUpDown8, comboBox4, domainUpDown4, textBox4);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            setAutomationsStatus(checkBox5, checkBox15, numericUpDown9, numericUpDown10, comboBox5, domainUpDown5, textBox5);
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            setAutomationsStatus(checkBox6, checkBox16, numericUpDown11, numericUpDown12, comboBox6, domainUpDown6, textBox6);
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            setAutomationsStatus(checkBox7, checkBox17, numericUpDown13, numericUpDown14, comboBox7, domainUpDown7, textBox7);
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            setAutomationsStatus(checkBox8, checkBox18, numericUpDown15, numericUpDown16, comboBox8, domainUpDown8, textBox8);
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            setAutomationsStatus(checkBox9, checkBox19, numericUpDown17, numericUpDown18, comboBox9, domainUpDown9, textBox9);
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            setAutomationsStatus(checkBox10, checkBox20, numericUpDown19, numericUpDown20, comboBox10, domainUpDown10, textBox10);
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (enabledCheckBoxes[0].Checked)
                domainUpDown1.Enabled = checkBox11.Checked;
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (enabledCheckBoxes[1].Checked)
                domainUpDown2.Enabled = checkBox12.Checked;
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            if (enabledCheckBoxes[2].Checked)
                domainUpDown3.Enabled = checkBox13.Checked;
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            if (enabledCheckBoxes[3].Checked)
                domainUpDown4.Enabled = checkBox14.Checked;
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            if (enabledCheckBoxes[4].Checked)
                domainUpDown5.Enabled = checkBox15.Checked;
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            if (enabledCheckBoxes[5].Checked)
                domainUpDown6.Enabled = checkBox16.Checked;
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            if (enabledCheckBoxes[6].Checked)
                domainUpDown7.Enabled = checkBox17.Checked;
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            if (enabledCheckBoxes[7].Checked)
                domainUpDown8.Enabled = checkBox18.Checked;
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            if (enabledCheckBoxes[8].Checked)
                domainUpDown9.Enabled = checkBox19.Checked;
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            if (enabledCheckBoxes[9].Checked)
                domainUpDown10.Enabled = checkBox20.Checked;
        }
    }
}
