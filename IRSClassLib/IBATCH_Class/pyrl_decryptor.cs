using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System;
using System.Security.Cryptography;


namespace IRSClassLib
{
    public class pyrl_decryptor :sqlHelper 
    {
        private string _temp_dir = AppDomain.CurrentDomain.BaseDirectory + "temp";//"C:\\IRS_Loading\\temp";


         public  pyrl_decryptor()
         {
                
         }

        private byte[] key =  { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
        private byte[] iv = { 8, 7, 6, 5, 4, 3, 2, 1 };
        private byte[] m_key
        {
            get { return key; }
            set { key = value; }
        }
        private byte[] m_iv
        {
            get { return iv; }
            set { iv = value; }
        }
        TripleDESCryptoServiceProvider m_des = new TripleDESCryptoServiceProvider();
        UTF8Encoding m_utf8 = new UTF8Encoding();
        public string Encrypt(string text)
        {
            byte[] input = m_utf8.GetBytes(text);
            byte[] output = Transform(input, m_des.CreateEncryptor(m_key, m_iv));
            return Convert.ToBase64String(output);
        }

        public string Decrypt(string text)
        {
            byte[] input = Convert.FromBase64String(text);
            byte[] output = this.Transform(input, m_des.CreateDecryptor(m_key, m_iv));
            return m_utf8.GetString(output);
        }

        private byte[] Transform(byte[] input, ICryptoTransform CryptoTransform)
        {
            // create the necessary streams
            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptStream = new CryptoStream(memStream, CryptoTransform, CryptoStreamMode.Write);
            // transform the bytes as requested
            cryptStream.Write(input, 0, input.Length);
            cryptStream.FlushFinalBlock();
            // Read the memory stream and convert it back into byte array
            memStream.Position = 0;
            byte[] result = new byte[(Int32)memStream.Length - 1];
            memStream.Read(result, 0, ((Int32)result.Length));
            // close and release the streams
            memStream.Close();
            cryptStream.Close();
            // hand back the encrypted buffer
            return result;
        }
        private string get_ibatchpath()
        {
            try
            {
                return base.getDataSet(base.getSqlCommand("select flddescription from tbl_lookup where fldgroup = 'IBATCHPATH'", false), "TPATH").Tables[0].Rows[0][0].ToString(); 
            }
            catch { return ""; }
        }

        private string[] inventory_rec(string filename)
        {
            string select_processed = "SELECT Processed, UploadedBy FROM TBL_FILE_INVENTORY WHERE FileName = '" + filename + "'";
            DataTable dt = getDataSet(getSqlCommand(select_processed, false)).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string[] ret = new string[] { dt.Rows[0][0].ToString(), 
                                                dt.Rows[0][1].ToString()
                                            };
                return ret;
            }
            return null;
        }

        public DataTable populate_ftpfiles(int format, DateTime tran_date)
        {

            //sp_uploader_getfilepath 3 , 1
            irsConfiguration conf = new irsConfiguration();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "sp_uploader_getfilepath";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@type", SqlDbType.Int).Value = 3;
            cmd.Parameters.Add("@format", SqlDbType.Int).Value = format;

            DataSet ds = base.getDataSet(cmd);
            string rootpath = this._temp_dir;
            DataTable dt = null;
            DataRow dr = null;
            csFileIo io = new csFileIo();

            if (ds.Tables.Count > 0)
            {
                string workingpath = rootpath + "\\" + ds.Tables[0].Rows[0][0].ToString() + "\\" + ds.Tables[0].Rows[0][1].ToString();

                if (!io.directoryExist(workingpath))
                    io.createDirectory(workingpath); 
                try
                {
                    dt = new DataTable("MyTable");
                    dt.Columns.Add("FILEDATE");
                    dt.Columns.Add("FILENAME");
                    dt.Columns.Add("FILESIZE");
                    dt.Columns.Add("ENCRYPTED");
                    dt.Columns.Add("FULLPATH");
                    dt.Columns.Add("OPTIONS");
                    dt.Columns.Add("UploadedBy");

                    if (io != null)
                    {

                        string[] s = io.listFiles(workingpath);
                        
                        foreach (string filename in s)
                        {
                            string fname = io.getFilename(filename);
                            string[] file_inv = this.inventory_rec(fname);
                            if (file_inv != null)
                            {
                                if (!Convert.ToBoolean(file_inv[0]))
                                {
                                    dr = dt.NewRow();
                                    dr["FILEDATE"] = io.getCreationDate(filename);
                                    dr["FILENAME"] = fname;//io.getFilename(filename);
                                    dr["FILESIZE"] = io.getFileSize(filename);
                                    dr["FULLPATH"] = filename;
                                    dr["UploadedBy"] = file_inv[1];
                                    //dr["ENCRYPTED"] = encrypt[i];
                                    dr["OPTIONS"] = "GET";
                                    dt.Rows.Add(dr);
                                    dt.AcceptChanges();
                                }
                            }
                        }
                        return dt;
                    }
                    return null;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    io = null;
                    dt = null;
                    dr = null;
                }
            }
            return null;
        }
        public void populate_pyrlfile()
        {
            
        /*    csFileIo io = new csFileIo();
            DataTable dt = null;
            DataRow dr = null;
            irsConfiguration conf = new irsConfiguration(); 
            int i = 0;

            string[] path = new string[2];
            string[] encrypt = new string[2];
            

            try
            {
                path[0] = prispath + @"\PRIS_UNENCRYPTED\";
                path[1] = prispath + @"\PRIS_ENCRYPTED\";
                encrypt[0] = "NO";
                encrypt[1] = "YES";

                dt = new DataTable("MyTable");
                dt.Columns.Add("FILEDATE");
                dt.Columns.Add("FILENAME");
                dt.Columns.Add("FILESIZE");
                dt.Columns.Add("ENCRYPTED");
                dt.Columns.Add("FULLPATH");
                dt.Columns.Add("OPTIONS");

                if (io != null)
                {
                    for (i = 0; i <= 1; i++)
                    {
                        string[] s = io.listFiles(path[i]);

                        foreach (string filename in s)
                        {
                            dr = dt.NewRow();
                            dr["FILEDATE"] = io.getCreationDate(filename);
                            dr["FILENAME"] = io.getFilename(filename);
                            dr["FILESIZE"] = io.getFileSize(filename);
                            dr["FULLPATH"] =  filename;
                            dr["ENCRYPTED"] = encrypt[i];
                            dr["OPTIONS"] = "GET";
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                        }
                    }
                    return dt;
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                io = null;
                dt = null;
                dr = null;
            }
         */
        }

        
    }    
}
