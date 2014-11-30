using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace Encryption
{
    class Cryption // does encryption and decryption
    {
        private DESCryptoServiceProvider DES = new DESCryptoServiceProvider(); // cryptography provider
        private ICryptoTransform encryptor, decryptor;

        public void decryptFile(FileStream inputStream, FileStream decryptedStream, byte[] key)
        {
            // setup DES
            DES.Key = key;
            DES.IV = key;
            decryptor = DES.CreateDecryptor(DES.Key, DES.IV);
            try
            {
                // setup cryptostream
                CryptoStream cryptostream = new CryptoStream(decryptedStream, decryptor, CryptoStreamMode.Write);
                // write to cryptostream
                byte[] byteArrayInput = new byte[inputStream.Length];
                inputStream.Read(byteArrayInput, 0, byteArrayInput.Length);
                cryptostream.Write(byteArrayInput, 0, byteArrayInput.Length);
                cryptostream.Close();
                inputStream.Close();
                decryptedStream.Close();
                Console.WriteLine("Successfully decrypted");
            }
            catch (System.IO.IOException e)
            {
                MessageBox.Show("Could not open source or destination file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (System.ArgumentException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (System.Security.Cryptography.CryptographicException e)
            {
                MessageBox.Show("Bad key or file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public void encryptFile(FileStream inputStream, FileStream encryptedStream, byte[] key)
        {
            // setup DES
            DES.IV = key;
            DES.Key = key;
            encryptor = DES.CreateEncryptor(DES.Key, DES.IV);
            try
            {
                // setup cryptostream
                CryptoStream cryptostream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write);
                // write to cryptostream
                byte[] byteArrayInput = new byte[inputStream.Length];
                inputStream.Read(byteArrayInput, 0, byteArrayInput.Length);
                cryptostream.Write(byteArrayInput, 0, byteArrayInput.Length);
                cryptostream.Close();
                inputStream.Close();
                encryptedStream.Close();
                Console.WriteLine("Successfully encrypted");
            }
            catch (System.IO.IOException e)
            {
                MessageBox.Show("Could not open source or destination file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (System.ArgumentException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (System.Security.Cryptography.CryptographicException e)
            {
                MessageBox.Show("Bad key or file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
