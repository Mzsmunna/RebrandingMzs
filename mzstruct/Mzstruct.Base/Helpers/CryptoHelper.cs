using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Mzstruct.Base.Helpers
{
    public static class CryptoHelper
    {
        //public static string plainText;
        private static readonly string passPhrase = "Pas5pr@se";
        private static readonly string saltValue = "s@1tValue";
        private static readonly string hashAlgorithm = "MD5";
        private static readonly int passwordIterations = 2;
        private static readonly string initVector = "@1B2c3D4e5F6g7H8";
        private static readonly int keySize = 256;

        public static string? Encrypt(string plainText)
        {
            if (!string.IsNullOrEmpty(plainText))
            {
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                PasswordDeriveBytes password = new(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
                byte[] keyBytes = password.GetBytes(keySize / 8);
                using Aes aes = Aes.Create();
                aes.Mode = CipherMode.CBC;
                aes.KeySize = keySize;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = keyBytes;
                aes.IV = initVectorBytes;
                using ICryptoTransform encryptor = aes.CreateEncryptor();
                using MemoryStream memoryStream = new();
                using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] cipherTextBytes = memoryStream.ToArray();
                string cipherText = Convert.ToBase64String(cipherTextBytes);
                return EncryptionUrlFriendlyString(cipherText);
            }
            return null;
        }

        public static string? Decrypt(string cipherText)
        {
            if (!string.IsNullOrEmpty(cipherText))
            {
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
                byte[] cipherTextBytes = Convert.FromBase64String(DecryptionUrlFriendlyString(cipherText)!);
                PasswordDeriveBytes password = new(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
                byte[] keyBytes = password.GetBytes(keySize / 8);
                using Aes aes = Aes.Create();
                aes.Mode = CipherMode.CBC;
                aes.KeySize = keySize;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = keyBytes;
                aes.IV = initVectorBytes;
                using ICryptoTransform decryptor = aes.CreateDecryptor();
                using MemoryStream memoryStream = new(cipherTextBytes);
                using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                return plainText;
            }
            return null;
        }

        private static string? DecryptionUrlFriendlyString(string stringToDecrypt)
        {
            if (!string.IsNullOrEmpty(stringToDecrypt))
            {
                stringToDecrypt = stringToDecrypt.Replace("-2F-", "/");
                stringToDecrypt = stringToDecrypt.Replace("-21-", "!");
                stringToDecrypt = stringToDecrypt.Replace("-23-", "#");
                stringToDecrypt = stringToDecrypt.Replace("-24-", "$");
                stringToDecrypt = stringToDecrypt.Replace("-26-", "&");
                stringToDecrypt = stringToDecrypt.Replace("-27-", "'");
                stringToDecrypt = stringToDecrypt.Replace("-28-", "(");
                stringToDecrypt = stringToDecrypt.Replace("-29-", ")");
                stringToDecrypt = stringToDecrypt.Replace("-2A-", "*");
                stringToDecrypt = stringToDecrypt.Replace("-2B-", "+");
                stringToDecrypt = stringToDecrypt.Replace("-2C-", ",");
                stringToDecrypt = stringToDecrypt.Replace("-3A-", ":");
                stringToDecrypt = stringToDecrypt.Replace("-3B-", ";");
                stringToDecrypt = stringToDecrypt.Replace("-3D-", "=");
                stringToDecrypt = stringToDecrypt.Replace("-3F-", "?");
                stringToDecrypt = stringToDecrypt.Replace("-40-", "@");
                stringToDecrypt = stringToDecrypt.Replace("-5B-", "[");
                stringToDecrypt = stringToDecrypt.Replace("-5D-", "]");

                return stringToDecrypt;
            }
            return null;
        }

        private static string? EncryptionUrlFriendlyString(string returnstring)
        {
            if (!string.IsNullOrEmpty(returnstring))
            {
                returnstring = returnstring.Replace("/", "-2F-");
                returnstring = returnstring.Replace("!", "-21-");
                returnstring = returnstring.Replace("#", "-23-");
                returnstring = returnstring.Replace("$", "-24-");
                returnstring = returnstring.Replace("&", "-26-");
                returnstring = returnstring.Replace("'", "-27-");
                returnstring = returnstring.Replace("(", "-28-");
                returnstring = returnstring.Replace(")", "-29-");
                returnstring = returnstring.Replace("*", "-2A-");
                returnstring = returnstring.Replace("+", "-2B-");
                returnstring = returnstring.Replace(",", "-2C-");
                returnstring = returnstring.Replace(":", "-3A-");
                returnstring = returnstring.Replace(";", "-3B-");
                returnstring = returnstring.Replace("=", "-3D-");
                returnstring = returnstring.Replace("?", "-3F-");
                returnstring = returnstring.Replace("@", "-40-");
                returnstring = returnstring.Replace("[", "-5B-");
                returnstring = returnstring.Replace("]", "-5D-");
                return returnstring;
            }
            return null;
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
