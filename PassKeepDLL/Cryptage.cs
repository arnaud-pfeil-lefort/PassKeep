using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;

namespace PassKeepDLL
{
    public abstract class Cryptage
    {

        private static readonly byte[] _key = Encoding.UTF8.GetBytes("PassKeepSecret!!");

        public static string hacherChaine(string chaine)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(chaine));
            return Convert.ToHexString(bytes).ToLower();
        }

        public static string encrypterChaine(string chaine)
        {
            using Aes aes = Aes.Create();
            aes.Key = _key;
            aes.GenerateIV();

            using MemoryStream ms = new();
            ms.Write(aes.IV, 0, aes.IV.Length);

            using (CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (StreamWriter sw = new(cs))
                sw.Write(chaine);

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string decrypterChaine(string chaineEncryptee)
        {
            byte[] data = Convert.FromBase64String(chaineEncryptee);

            using Aes aes = Aes.Create();
            aes.Key = _key;

            byte[] iv = new byte[aes.BlockSize / 8];
            Array.Copy(data, 0, iv, 0, iv.Length);
            aes.IV = iv;

            using MemoryStream ms = new(data, iv.Length, data.Length - iv.Length);
            using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using StreamReader sr = new(cs);
            return sr.ReadToEnd();
        }
    }
}
