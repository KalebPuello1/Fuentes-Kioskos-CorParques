using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Transversales.Util
{
    public static class Encripcion
    {

       
        public static string Encriptar(string textoEncriptar, string llave)
        {
            string encrypted = string.Empty;
            byte[] hash = new byte[32];

            RijndaelManaged AES = new RijndaelManaged();
            MD5CryptoServiceProvider Hash_AES = new MD5CryptoServiceProvider();

            byte[] temp = Hash_AES.ComputeHash(ASCIIEncoding.ASCII.GetBytes(llave));
            Array.Copy(temp, 0, hash, 0, 16);
            Array.Copy(temp, 0, hash, 15, 16);
            AES.Key = hash;
            AES.Mode = CipherMode.ECB;

            ICryptoTransform DESEncrypter = AES.CreateEncryptor();
            byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(textoEncriptar);
            encrypted = Convert.ToBase64String(DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length));

            return encrypted;
        }
        
        public static string Desencriptar(string textoDesencriptar, string llave)
        {
            string decrypted = string.Empty;
            byte[] hash = new byte[32];

            RijndaelManaged AES = new RijndaelManaged();
            MD5CryptoServiceProvider Hash_AES = new MD5CryptoServiceProvider();
            byte[] temp = Hash_AES.ComputeHash(ASCIIEncoding.ASCII.GetBytes(llave));
            Array.Copy(temp, 0, hash, 0, 16);
            Array.Copy(temp, 0, hash, 15, 16);

            AES.Key = hash;
            AES.Mode = CipherMode.ECB;
            ICryptoTransform DESDecrypter = AES.CreateDecryptor();
            byte[] Buffer = Convert.FromBase64String(textoDesencriptar);
            decrypted = Encoding.ASCII.GetString(DESDecrypter.TransformFinalBlock(Buffer, 0, Buffer.Length));

            return decrypted;
        }
    }
}
