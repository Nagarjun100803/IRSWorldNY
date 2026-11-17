using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace IRSClassLib
{
	// (A) Carlos H. Raymundo CPI 2005 for PNB
    [Serializable]
	public class TriDes
	{
		private PasswordDeriveBytes pdkey;
		private byte[] salt;
		private string key;

		public TriDes()
		{
			setSalt("qwertyasdfgzxcvb18794512513");
		}

		public TriDes(String key, String salt)
		{
			setSalt(salt);
			setKey(key);
		}

		public void setKey(string key)
		{
			this.key = key;

			if (salt == null || salt.Length < 1)
			{
				this.salt = Encoding.ASCII.GetBytes("qwertyasdfgzxcvb18794512513");
			}
			pdkey = new PasswordDeriveBytes(key, salt);
		}

		public void setSalt(string salt)
		{
			this.salt = Encoding.ASCII.GetBytes(salt);
			if (key != null)
			{
				setKey(this.key);
			}
		}

		private bool FileDecrypt(string infile, string outfile)
		{
			if (pdkey == null) return false;

			try
			{
				FileStream fin = new FileStream(infile, FileMode.Open, FileAccess.Read, FileShare.Read);
				FileStream fout = new FileStream(outfile, FileMode.Create, FileAccess.Write, FileShare.Read);

				TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();          
				CryptoStream decstream = new CryptoStream(fin, tdes.CreateDecryptor(pdkey.GetBytes(24), pdkey.GetBytes(24)), CryptoStreamMode.Read);
			
				byte[] buff = new byte[1028];
			
				while (true)
				{
					int len = decstream.Read(buff, 0, buff.Length);
					if (len < 1) break;
					fout.Write(buff, 0, len);
				}

				decstream.Close();
				fin.Close();
				fout.Close();
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			return false;
		}

		public bool DecryptFile(string infile, string outfile, string key)
		{
			if (key == null || key.Length < 1) return false;
			
			setKey(key);
			return FileDecrypt(infile, outfile);		
		}

		private bool FileEncrypt(string infile, string outfile)
		{
			if (pdkey == null) return false;

			try
			{
				FileStream fin = new FileStream(infile, FileMode.Open, FileAccess.Read, FileShare.Read);
				FileStream fout = new FileStream(outfile, FileMode.Create, FileAccess.Write, FileShare.Read);

				TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();          
				CryptoStream encstream = new CryptoStream(fout, tdes.CreateEncryptor(pdkey.GetBytes(24), pdkey.GetBytes(24)), CryptoStreamMode.Write);
			
				byte[] buff = new byte[1028];
			
				while (true)
				{
					int len = fin.Read(buff, 0, buff.Length);
					if (len < 1) break;
					encstream.Write(buff, 0, len);
				}

				encstream.Close();
				fin.Close();
				fout.Close();
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			return false;
		}

		public bool EncryptFile(string infile, string outfile, string key)
		{
			setKey(key);
			return FileEncrypt(infile, outfile);
		}

		private byte[] Transform(byte[] inputvar, ICryptoTransform transform) 
		{
			byte[] returnvar;

			System.IO.MemoryStream stream = new System.IO.MemoryStream(2048);
			CryptoStream encryptstream = new CryptoStream(stream, transform, CryptoStreamMode.Write);

			encryptstream.Write(inputvar, 0, (int)inputvar.Length);
			encryptstream.FlushFinalBlock();

			stream.Position = 0;	
			returnvar = new byte[(int)stream.Length];
			stream.Read(returnvar, 0, (int)returnvar.Length);

			encryptstream.Close();
			stream.Close();
			encryptstream = null;
			stream = null;

			return returnvar;
		}

		public string Encrypt(string s) 
		{
			if (pdkey == null) return null;

			byte[] b = Encoding.Default.GetBytes(s);
			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider(); 

			return Encoding.Default.GetString(Transform(b, tdes.CreateEncryptor(pdkey.GetBytes(24), pdkey.GetBytes(24))));
		}

		public string EncryptString(string s, string key)
		{
			if (key == null || key.Length < 1) return null;
			setKey(key);
			return Encrypt(s);
		}

		public string Decrypt(string s) 
		{
			if (pdkey == null) return null;

			byte[] b = Encoding.Default.GetBytes(s);
			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider(); 

			return Encoding.Default.GetString(Transform(b, tdes.CreateDecryptor(pdkey.GetBytes(24), pdkey.GetBytes(24))));
		}

		public string DecryptString(string s, string key)
		{
			if (key == null || key.Length < 1) return null;
			setKey(key);
			return Decrypt(s);
		}

	}
}
