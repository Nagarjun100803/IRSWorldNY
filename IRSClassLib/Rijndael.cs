using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace IRSClassLib
{
    public class Rijndael
    {
        public String Message;

        private String password = "PNBAgentRS";
        private byte[] salt;

        public String Encrypt(String value)
        {
            String encdata = null;
            try
            {
                PasswordDeriveBytes skey = new PasswordDeriveBytes(password, salt);
                ICryptoTransform crypto = new RijndaelManaged().CreateEncryptor(skey.GetBytes(32), skey.GetBytes(16));
                MemoryStream ms = new MemoryStream();
                byte[] b = Encoding.Unicode.GetBytes(value);
                CryptoStream cs = new CryptoStream(ms, crypto, CryptoStreamMode.Write);
                cs.Write(b, 0, b.Length);
                cs.FlushFinalBlock();
                encdata = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                cs.Close();
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return encdata;
        }

        public String Decrypt(String value)
        {
            String decdata = null;
            String basestring = null;
            try
            {
                byte[] byt = Encoding.UTF8.GetBytes(value);
                basestring = Convert.ToBase64String(byt);
                byte[] encdata = Convert.FromBase64String(basestring);
                PasswordDeriveBytes skey = new PasswordDeriveBytes(password, salt);
                ICryptoTransform crypto = new RijndaelManaged().CreateDecryptor(skey.GetBytes(32), skey.GetBytes(16));
                MemoryStream ms = new MemoryStream(encdata);
                byte[] b = new byte[encdata.Length];
                CryptoStream cs = new CryptoStream(ms, crypto, CryptoStreamMode.Read);
                int count = cs.Read(b, 0, b.Length);
                decdata = Encoding.Unicode.GetString(b, 0, count);
                ms.Close();
                cs.Close();
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return decdata;
        }

        public Rijndael()
        {
            this.salt = Encoding.ASCII.GetBytes(this.password.Length.ToString());
        }

    }
}
