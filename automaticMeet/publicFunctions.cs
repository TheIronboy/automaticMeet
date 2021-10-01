using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace automaticMeet
{
    class publicFunctions
    {
        public string mainDir = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\automaticMeet\";
        public string encryptionKey = "705ea6d9231f38e4484fcf8be5a1aa5d";

        public string[] getSessionData()
        {
            string[] sessionData = new string[3];
            string sessionFileDir = mainDir + @"\.session.txt";

            using (StreamReader file = File.OpenText(sessionFileDir))
            {
                for (int i = 0; i < sessionData.Length; i++)
                    sessionData[i] = file.ReadLine();

                if (sessionData[1] != null)
                    sessionData[1] = DecryptString(encryptionKey, sessionData[1]);

                file.Close();
            }

            return sessionData;
        }

        public void getCodeList(ComboBox comboBox)
        {
            string codeDir = mainDir + getSessionData()[0] + @"\codes\";

            if (Directory.Exists(codeDir))
            {
                string[] fileEntries = Directory.GetFiles(codeDir);

                comboBox.Items.Clear();

                foreach (string fileName in fileEntries)
                    comboBox.Items.Add(Path.GetFileName(fileName.Remove(fileName.Length - 4)));
            }
            else
                Directory.CreateDirectory(codeDir);
        }

        public string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                            streamWriter.Close();
                        }

                        array = memoryStream.ToArray();
                        cryptoStream.Close();
                    }

                    memoryStream.Close();
                }
            }

            return Convert.ToBase64String(array);
        }

        public string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);
            string decryptedString;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            decryptedString = streamReader.ReadToEnd();
                            streamReader.Close();
                        }

                        cryptoStream.Close();
                    }

                    memoryStream.Close();
                }
            }

            return decryptedString;
        }
    }
}
