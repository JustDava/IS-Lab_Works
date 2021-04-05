using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace IS_Lab_3
{
    class Programm
    {
        public static void Main()
        {
            try
            {
                Console.WriteLine("Введите текст:");
                string original = Console.ReadLine();


                using (Rijndael myRijndael = Rijndael.Create())
                {

                    byte[] encrypted = EncryptStringToBytes(original, myRijndael.Key, myRijndael.IV);


                    Console.WriteLine("\nЗашифрованный текст:\n");

                    for (int i = 0; i < encrypted.Length; i+=8)
                    {
                        for (int k = i; k < i+8; k++)
                        {
                            Console.Write(encrypted[k] + "\t");
                        }
                        Console.WriteLine();

                        if ((i / 8 % 16) == 0 && i!=0)
                        {
                            Console.WriteLine('\n');
                        }
                    }


                    string roundtrip = DecryptStringFromBytes(encrypted, myRijndael.Key, myRijndael.IV);

                    Console.WriteLine("\nДешифрованный текст: {0}", roundtrip);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }
        static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.KeySize = 128;
                rijAlg.BlockSize = 128;
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }

        static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.KeySize = 128;
                rijAlg.BlockSize = 128;
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}
