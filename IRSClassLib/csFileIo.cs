using System;
using System.IO;
using System.Text;

namespace IRSClassLib
{
	/*
	 * Author	: CHR
	 */
    [Serializable]
	public class csFileIo
	{
		private BufferedStream bsin;
		private StreamReader srin;
		private BufferedStream srout;

		private String infile;
		private String outfile;
		private bool eof;

        public String Message;


		public bool EOF
		{
			get { return eof; }
		}

		private int i_mode;
		private int o_write_mode;

		public int write_mode
		{
			get { return o_write_mode; }
			set { o_write_mode = value; }
		}

		public enum write_modes
		{
			CREATE,
			CREATE_NEW,
			APPEND
		}

		public enum access_modes
		{
			TEXT,
			BINARY
		}

		public int input_mode
		{
			get { return i_mode; }
			set { i_mode = (value < (int) access_modes.TEXT & value > (int) access_modes.BINARY) ? (int) access_modes.BINARY : value; }
		}

		public bool isReadable
		{
			get

			{
				return (input_mode == (int) access_modes.TEXT) ? (srin != null) : (bsin != null);
			}
		}

		public bool isWritable
		{
			get
			{
				return srout != null;
			}
		}

        /*
        public void writeLog(String Message)
        {
            csLogger.writeLog(Message);
        }
        */

		private bool open_byte_input()
		{
			try
			{
				bsin = new BufferedStream(new FileStream(infile, FileMode.Open, FileAccess.Read, FileShare.Read));
				return true;
			}
			catch (Exception e)
			{
                Message = e.Message;
			}
			return false;
		}

		private bool open_text_input()
		{
			try
			{
				srin = new StreamReader (infile);
				return true;
			}
			catch (Exception e)
			{
                Message = e.Message;
            }
			return false;
		}

		public bool open_input (String filename, int mode)
		{
			infile = filename;
			input_mode = mode;
			return (input_mode == (int) access_modes.BINARY) ? open_byte_input() : open_text_input();
		}

		public bool open_input (String filename)
		{
			return open_input (filename, (int) access_modes.BINARY);
		}

		public void close_input()
		{
            try
            {
                if (input_mode == (int)access_modes.BINARY)
                {
                    bsin.Close();
                    bsin.Dispose();
                    bsin = null;
                }
                else
                {
                    srin.Close();
                    srin.Dispose();
                    srin = null;
                }
                eof = false;
            }
            catch { }
		}

		private BufferedStream open_output(String filename, FileMode fm)
		{
			try
			{
				FileStream fs = new FileStream(filename, fm, FileAccess.Write, FileShare.Read);
				return new BufferedStream(fs);
			}
			catch (Exception e)
			{
                Message = e.Message;
            }
			return null;
		}

		public bool open_output(String filename, int write_mode)
		{
			this.outfile = filename;
			this.write_mode = write_mode;
			FileMode fm;
			if (this.write_mode == (int) write_modes.APPEND) fm = FileMode.Append;
			else if (this.write_mode == (int) write_modes.CREATE) fm = FileMode.Create;
			else if (this.write_mode == (int) write_modes.CREATE_NEW) fm = FileMode.CreateNew;
			else
			{
                Message = "open_output: Problem with write mode";
                return false;
			}
			srout = open_output(outfile, fm);
			return (srout != null);
		}

		public int readBytes(byte[] b, int offset, int count)
		{
			int i = bsin.Read(b, offset, count);
			if (i == 0) eof = true;
			return i;
		}

		private String readBytes()
		{
			StringBuilder sb = new StringBuilder();
			if (eof) return null;
			bool cr = false;
			while (true)
			{
				byte[] b = {0x00};
				int i = readBytes(b, 0, 1);
				if (EOF) 
				{
					break;
				}
                if (b[0] == 10)    //(cr & b[0] == 10) //LF
				{
					break;
				}
				if (b[0] == 13)		// CR
				{
					cr = true;
				}
				else
				{
					sb.Append(Convert.ToChar(b[0]));
					if (cr) cr = false;
				}
			}
			if (sb.ToString().Length < 1) return null;
			return sb.ToString();
		}

		public String readLine()
		{
			try
			{
				if (input_mode == (int) access_modes.TEXT)
				{
					String s = srin.ReadLine();
					if (s == null) eof = true;
					return s;
				}
				else
				{
					return readBytes();
				}
			}
			catch (Exception e)
			{
                Message = e.Message;
			}
			return null;
		}

		public void put(byte[] b, int offset, int count)
		{
			srout.Write(b, offset, count);
		}

		public byte[] stringToByte(String s)
		{
			return Encoding.Default.GetBytes(s);
		}

		public void putLine(string buff)
		{
			byte[] b = stringToByte(buff + Environment.NewLine);
			put(b, 0, b.Length);
		}

        public void putrecord(string buff)
        {
            byte[] b = stringToByte(buff);
            put(b, 0, b.Length);
        }

		public void close_output()
		{
            try
            {
                srout.Flush();
                srout.Close();
                srout.Dispose();
                srout = null;
            }
            catch { }
		}

		public String OutPutFileName
		{
			get
			{
				return outfile;
			}
		}

		public String InputFileName
		{
			get
			{
				return infile;
			}
		}

        public bool delete(String path)
        {
            try
            {
                File.Delete(path);
            }
            catch { }
            return false;
        }

        public String readAllText(String path)
        {
            return File.ReadAllText(path);
        }

        public String[] readAllLines(String path)
        {
            return File.ReadAllLines(path);
        }

		public DateTime getCreationDate(String path)
		{
			return File.GetLastWriteTime(path);
		}

		public long getFileSize(String path)
		{
			FileInfo fi = new FileInfo(path);
			return fi.Length;
		}

		public bool exist(String path)
		{
			return File.Exists(path);
		}

		public String getFileExtension(String path)
		{
			return (Path.GetExtension(path)).Substring(1);
		}

		public String getDirectory(String path)
		{
			return Path.GetDirectoryName(path);
		}

		public String getFilename(String path)
		{
			return Path.GetFileName(path);
		}

		public String getPathSeparator(String path)
		{
			return Path.DirectorySeparatorChar.ToString();
		}

		public bool directoryExist(String path)
		{
			return Directory.Exists(path);
		}

		public DirectoryInfo createDirectory(String path)
		{
			if (! directoryExist(path))
			{
				return Directory.CreateDirectory(path);
			}
			return new DirectoryInfo(path);
		}

        public String[] listFiles(String dir)
        {
            if (!directoryExist(dir)) return null;
            return Directory.GetFileSystemEntries(dir);
        }

        public String[] listFiles(String dir, String pattern)
        {
            if (!directoryExist(dir)) return null;
            return Directory.GetFileSystemEntries(dir, pattern);
        }

		public String getFilenameNoExt(String path)
		{
			return Path.GetFileNameWithoutExtension(path);
		}

        public bool rename(String old_path, String new_path)
        {
            if (exist(new_path))
            {
                Message = "rename: " + new_path + "Exist";
                return false;
            }
            try
            {
                File.Move(old_path, new_path);
            }
            catch (Exception e)
            {
                Message = "rename: "  + e.Message;
                return false;
            }
            return true;
        }

		public String byteArrayToString(byte[] b)
		{
			return byteArrayToString(b, 0, b.Length);
		}

		public String byteArrayToString(byte[] b, int index, int count)
		{
			char[] c = new char[count];
			int j = Encoding.Default.GetChars(b, index, count, c, 0);
			return new String(c);
		}

        public bool copy(String inpath, String outpath, bool overwrite)
        {
            try
            {
                File.Copy(inpath, outpath, overwrite);
                return true;
            }
            catch { }
            return false;
        }

		public csFileIo()
		{
			eof = false;
		}

        ~csFileIo()
        {
            bsin = null;
            srin = null;
            srout = null;
            infile = null;
            outfile = null;
        }
	}
}
