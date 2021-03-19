using System;
using System.IO;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Phony.WPF.Data
{
    public class Encryption
    {
        /// <summary>
        /// The Encryption key as a SecureString
        /// </summary>
        protected readonly static SecureString Key = new NetworkCredential("i%B86!@%%xuF$zK%XV#q", "Vm2%#5%Xe!*jnFRfGo%Q").SecurePassword;

        private static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;
            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using (MemoryStream ms = new())
            {
                using (RijndaelManaged AES = new())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;
                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }
            return encryptedBytes;
        }

        private static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;
            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using (MemoryStream ms = new())
            {
                using (RijndaelManaged AES = new())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;
                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }
            return decryptedBytes;
        }

        /// <summary>
        /// Encrypt a text
        /// </summary>
        /// <param name="input">The text</param>
        /// <returns></returns>
        public static string EncryptText(string input)
        {
            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(new NetworkCredential("", Key).Password);
            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);
            string result = Convert.ToBase64String(bytesEncrypted);
            return result;
        }

        /// <summary>
        /// Decrypt a text
        /// </summary>
        /// <param name="input">The text</param>
        /// <returns></returns>
        public static string DecryptText(string input)
        {
            // Get the bytes of the string
            byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(new NetworkCredential("", Key).Password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);
            string result = Encoding.UTF8.GetString(bytesDecrypted);
            return result;
        }

        /// <summary>
        /// Encrypt a file
        /// </summary>
        /// <param name="inFile">File full path</param>
        /// <param name="outFile">new file full path</param>
        public static void EncryptFile(string inFile, string outFile)
        {
            byte[] bytesToBeEncrypted = File.ReadAllBytes(inFile);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(new NetworkCredential("", Key).Password);
            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);
            File.WriteAllBytes(outFile, bytesEncrypted);
        }

        /// <summary>
        /// Decrypt a file
        /// </summary>
        /// <param name="inFile">File full path</param>
        /// <param name="outFile">new file full path</param>
        public static void DecryptFile(string inFile, string outFile)
        {
            byte[] bytesToBeDecrypted = File.ReadAllBytes(inFile);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(new NetworkCredential("", Key).Password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);
            File.WriteAllBytes(outFile, bytesDecrypted);
        }
    }
}