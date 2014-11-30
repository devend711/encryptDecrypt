using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace Encryption
{
    public partial class Form1 : Form
    {
        private byte[] keyArray = new byte[8]; // the actual key
        string fileIn, fileOut; // file names
        private Cryption cryption = new Cryption(); // custom class

        public Form1()
        {
            InitializeComponent();
        }

        private void keyArrayToZeros() // reinitialize the array to 0s
        {
            for (int i = 0; i < keyArray.Length; i++)
            {
                keyArray[i] = 0;
            }
        }

        private byte[] getKey() // returns a byte array
        {
            keyArrayToZeros();
            char[] charArray = keyTextBox.Text.ToCharArray();
            byte thisByte;
            for (int i = 0; i < charArray.Length; i++)
            {
                thisByte = (byte) charArray[i];
                keyArray[i % (keyArray.Length)] += (byte) thisByte; // add lower 8 bits of this character to the keyArray, make sure to loop around the byte array (keyString[9] -> keyArray[1])
            }
            return keyArray; // return the byte array
        }

        private string getInputFileName()
        {
            return fileNameBox.Text;
        }

        private void openFileButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) fileNameBox.Text = openFileDialog1.FileName; // update the text box (this is where we get the file name from for encrypt/decrypt
        }

        private bool validKey()
        {
            if (keyTextBox.Text == "")
            {
                MessageBox.Show("Please enter a key", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool overwrite(string fileName)
        {
            if (File.Exists(fileName))
            {
                var shouldOverwrite = MessageBox.Show("Output file exists. Overwrite?", "File Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return shouldOverwrite == DialogResult.Yes;
            }
            return true;
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            if (validKey())
            {
                try // setup encryption streams
                {
                    fileIn = getInputFileName();
                    FileStream inputStream = new FileStream(fileIn, FileMode.Open, FileAccess.Read);
                    if (inputStream.Length <= 0) throw new System.IO.IOException();
                    fileOut = fileIn + ".des";
                    if (!overwrite(fileOut)) return;
                    FileStream encryptedStream = new FileStream(fileOut, FileMode.Create, FileAccess.Write);
                    cryption.encryptFile(inputStream, encryptedStream, getKey());
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Could not open source or destination file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } 
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            if (validKey())
            {
                try // setup decryption streams
                {
                    fileIn = getInputFileName();
                    if (!fileIn.EndsWith(".des")) // make sure the input file ends in .des
                    {
                        MessageBox.Show("Not a .des file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else // file is a .des file, decrypt it
                    {
                        fileOut = fileIn.Substring(0, fileIn.Length - (".des").Length); // the output file is the input file - ".des"
                        if (!overwrite(fileOut))
                        {
                            return;
                        }
                        FileStream inputStream = new FileStream(fileIn, FileMode.Open, FileAccess.Read);
                        FileStream decryptedStream = new FileStream(fileOut, FileMode.Create, FileAccess.Write);
                        cryption.decryptFile(inputStream, decryptedStream, getKey());
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Could not open source or destination file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
