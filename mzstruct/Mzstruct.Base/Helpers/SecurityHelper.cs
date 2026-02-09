using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;

namespace Mzstruct.Base.Helpers
{
    public static class SecurityHelper
    {
        private static readonly byte[] key = RandomNumberGenerator.GetBytes(32);
        const int iVSize = 16;

        public static string Encrypt(string plainText, byte[]? masterkey = null)
        {
            try
            {
                if (masterkey == null) masterkey = key;
            
                using var aes = Aes.Create();
                aes. Mode = CipherMode.CBC;
                aes. Padding = PaddingMode.PKCS7;
                aes. Key = masterkey;
                aes. IV = RandomNumberGenerator.GetBytes(iVSize);

                using var memoryStream = new MemoryStream();
                memoryStream.Write(aes.IV, 0, iVSize);

                using (var encryptor = aes.CreateEncryptor())
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                using (var streamWriter = new StreamWriter(cryptoStream))

                streamWriter.Write(plainText);
                return Convert.ToBase64String(memoryStream. ToArray());
            }
            catch (CryptographicException ex) 
            { 
                throw new InvalidOperationException("Encryption Failed", ex);
            }
        }

        public static string Decrypt(string cipherText, byte[]? masterkey = null)
        {
            try
            {
                if (masterkey == null) masterkey = key;
                byte[] cipherData = Convert.FromBase64String(cipherText);

                if (cipherData.Length < iVSize)
                    throw new InvalidOperationException("Invalid cipher text format.");

                byte[] iv = new byte[iVSize];
                byte[] encryptedData = new byte[cipherData.Length - iVSize];

                Buffer.BlockCopy(cipherData, 0, iv, 0, iVSize);
                Buffer.BlockCopy(cipherData, iVSize, encryptedData, 0, encryptedData.Length);

                using var aes = Aes.Create();
                aes.Mode = CipherMode.CBC;
                aes. Padding = PaddingMode.PKCS7;
                aes. Key = masterkey;
                aes.IV = iv;

                using MemoryStream memoryStream = new(encryptedData);
                using var decryptor = aes. CreateDecryptor();
                using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode. Read);
                using StreamReader streamReader = new(cryptoStream);

                return streamReader.ReadToEnd();
            }
            catch (CryptographicException ex) 
            { 
                throw new InvalidOperationException("Decryption Failed", ex);
            }
        }
    }
}
