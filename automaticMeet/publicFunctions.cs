using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace automaticMeet
{
    class publicFunctions
    {
        public string mainDir = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\automaticMeet\";
        public string encryptionKey = "705ea6d9231f38e4484fcf8be5a1aa5d";

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

        public Color GetColorAt(int xpos, int ypos)
        {
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, xpos, ypos, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            return screenPixel.GetPixel(0, 0);
        }

        public void LeftMouseClick(int xpos, int ypos, bool onlySet)
        {
            SetCursorPos(xpos, ypos);

            if (!onlySet)
            {
                mouse_event(0x02, xpos, ypos, 0, 0);
                mouse_event(0x04, xpos, ypos, 0, 0);
            }
        }

        public string[] getSessionData()
        {
            string[] sessionData = new string[3];

            using (StreamReader file = File.OpenText(mainDir + @"\.session.txt"))
            {
                for (int i = 0; i < sessionData.Length; i++)
                    sessionData[i] = file.ReadLine();

                if (sessionData[1] != null)
                    sessionData[1] = DecryptString(encryptionKey, sessionData[1]);

                file.Close();
            }

            return sessionData;
        }

        public void getCodeList(ComboBox codeList)
        {
            string codeDir = mainDir + getSessionData()[0] + @"\codes\";

            if (Directory.Exists(codeDir))
            {
                string[] fileEntries = Directory.GetFiles(codeDir);

                codeList.Items.Clear();

                foreach (string fileName in fileEntries)
                    codeList.Items.Add(Path.GetFileName(fileName.Remove(fileName.Length - 4)));
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
