using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace automaticMeet
{
    public partial class calibrator : Form
    {
        publicFunctions publicFunctionsRef = new publicFunctions();
        string calibratorSettingsFile;

        NumericUpDown[] numericUpDowns;
        TextBox[] textBoxes;

        private void loadCalibratorData(CheckBox enabled, NumericUpDown[] coords, TextBox[] color)
        {
            string[] settingsData = new string[6];

            using (StreamReader file = File.OpenText(calibratorSettingsFile))
            {
                for (int i = 0; i < settingsData.Length; i++)
                {
                    settingsData[i] = file.ReadLine();
                }

                file.Close();
            }

            enabled.Checked = Convert.ToBoolean(settingsData[0]);

            int cont = 1;
            foreach (NumericUpDown numericUpDown in coords)
            {
                numericUpDown.Value = Convert.ToInt32(settingsData[cont]);
                cont++;
            }

            cont = 3;
            foreach (TextBox textBox in color)
            {
                textBox.Text = settingsData[cont];
                cont++;
            }
        }

        private void saveCalibratorData(CheckBox enabled, NumericUpDown[] coords, TextBox[] color)
        {
            string[] settingsData = new string[6];

            settingsData[0] = enabled.Checked.ToString();

            int cont = 1;
            foreach (NumericUpDown numericUpDown in coords)
            {
                settingsData[cont] = numericUpDown.Value.ToString();
                cont++;
            }

            cont = 3;
            foreach (TextBox textBox in color)
            {
                settingsData[cont] = textBox.Text;
                cont++;
            }

            using (StreamWriter file = File.CreateText(calibratorSettingsFile))
            {
                for (int i = 0; i < settingsData.Length; i++)
                {
                    file.WriteLine(settingsData[i]);
                }

                file.Close();
            }
        }

        public calibrator()
        {
            InitializeComponent();
        }

        private void calibrator_Load(object sender, EventArgs e)
        {
            calibratorSettingsFile = publicFunctionsRef.mainDir + publicFunctionsRef.getSessionData()[0] + @"\calibratorSettings.txt";

            numericUpDowns = new NumericUpDown[2] { numericUpDown1, numericUpDown2 };
            textBoxes = new TextBox[3] { textBox1, textBox2, textBox3 };

            if (!File.Exists(calibratorSettingsFile))
                saveCalibratorData(checkBox1, numericUpDowns, textBoxes);
            else
                loadCalibratorData(checkBox1, numericUpDowns, textBoxes);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value != 0 && numericUpDown2.Value != 0)
            {
                int coordX = Convert.ToInt32(numericUpDown1.Value), coordY = Convert.ToInt32(numericUpDown2.Value);
                publicFunctionsRef.LeftMouseClick(coordX, coordY, true);

                Color capturedColor = publicFunctionsRef.GetColorAt(coordX, coordY);

                textBox1.Text = capturedColor.R.ToString();
                textBox2.Text = capturedColor.G.ToString();
                textBox3.Text = capturedColor.B.ToString();
            }
            else
                MessageBox.Show("Inserisci coordinate valide.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value != 0 && numericUpDown2.Value != 0)
            {
                saveCalibratorData(checkBox1, numericUpDowns, textBoxes);
                loadCalibratorData(checkBox1, numericUpDowns, textBoxes);

                MessageBox.Show("Applicate con successo!");
                this.Close();
            }
            else
                MessageBox.Show("Clicca (TEST) prima di applicare.");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            saveCalibratorData(checkBox1, numericUpDowns, textBoxes);
        }
    }
}
