using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IRSClassLib.IBATCH_Class
{
    [Serializable]
    public class ARSValidation : sqlHelperWs
    {
        private string fn;
        private string _err_msg;
        private string path;// = AppDomain.CurrentDomain.BaseDirectory + "temp" + @"\";
        private string newPath;// = AppDomain.CurrentDomain.BaseDirectory + "temp" + @"\ARSConvert\";

        private double _header_amount;
        private double _actual_amount;
        
        private StreamReader sr;
        private csFileIo io;

        public enum ARSField
        {
            AgentTransactionRefNo,
            RemitFirstName,
            RemitLastName,
            BeneFirstName,
            BeneLastName,
            BeneContactNo,
            BeneAddress,
            ProductCode,
            AccountNo,
            GFMCCardNo,
            BankCode,
            BankName,
            RemittingCurrency,
            Amount
        }

        public string ErrorMessage
        {
            get { return this._err_msg; }
        }

        public string getFieldValue(string buff, ARSField field)
        {
            int ndx = 0;
            string val = "";
            switch (field)
            {
                case ARSField.AgentTransactionRefNo: ndx = 0; break;
                case ARSField.RemitFirstName: ndx = 1; break;
                case ARSField.RemitLastName: ndx = 2; break;
                case ARSField.BeneFirstName: ndx = 3; break;
                case ARSField.BeneLastName: ndx = 4; break;
                case ARSField.BeneContactNo: ndx = 5; break;
                case ARSField.BeneAddress: ndx = 6; break;
                case ARSField.ProductCode: ndx = 7; break;
                case ARSField.AccountNo: ndx = 8; break;
                case ARSField.GFMCCardNo: ndx = 9; break;
                case ARSField.BankCode: ndx = 10; break;
                case ARSField.BankName: ndx = 11; break;
                case ARSField.RemittingCurrency: ndx = 12; break;
                case ARSField.Amount: ndx = 13; break;
            }

            val = buff.Split(';')[ndx];
            return val;
        }

        public string[] paymentDetails(string buff)
        {
            return new string[] { getFieldValue(buff, ARSField.RemittingCurrency), getFieldValue(buff, ARSField.Amount) };
        }

        public bool validARSFile(string arsFormat)
        {
            string format = arsFormat.ToUpper().Trim();
            bool toConvert = Convert.ToBoolean(new irsBatchFileUpload().getLookupValue("ConvertFiles", format));

            if (toConvert)
            {
                io = new csFileIo();
                if (!io.directoryExist(newPath)) io.createDirectory(newPath);
                if (io.copy(path, newPath + this.fn, true)) io.delete(path);

                Converter converter = new Converter();
                converter.ConvertFile(this.newPath + this.fn, this.path, format);
            }

            return validARSFile();
        }
        
        public bool validARSFile()
        {
            this.getFileStreamReader();
            bool valid = false;
            if (sr != null)
            {
                try
                {
                    while (true)
                    {
                        string buff = sr.ReadLine();

                        if (buff == null) break;
                        if (buff.Length == 1 && Encoding.Default.GetBytes(buff)[0] == 26) continue;
                        if (ARSHeader(buff)) continue;

                        string[] file_rec = buff.Split(';');
                        string s_target_amt = "";
                        try
                        {
                            s_target_amt = file_rec[13];
                        }
                        catch
                        {
                            throw new Exception("Invalid Record. Incomplete number of field.");
                        }

                        try
                        {
                            double d_target_amt = Convert.ToDouble(s_target_amt);
                            this._actual_amount = Math.Round(this._actual_amount + d_target_amt, 2);//this._actual_amount += d_target_amt;
                        }
                        catch
                        {
                            throw new Exception("Invalid Record. Error converting " + s_target_amt + " to Double.");
                        }
                    }

                    if ((this._actual_amount > 0 & this._header_amount > 0))
                    {
                        if (this._actual_amount == this._header_amount) valid = true;
                        else this._err_msg = this._err_msg = "ERROR: File " + this.fn + " is rejected!\n\nReason - Header amount and total record amount are not equal.";
                    }
                    
                    return valid;
                }
                catch (Exception e) { throw e; }
                finally
                {
                    sr.Close();
                    sr.Dispose();
                    sr = null;
                }
                //return ((this._actual_amount == this._header_amount) & (this._actual_amount > 0 & this._header_amount > 0));
            }

            throw new Exception("Error: validARSFile");
        }

        private bool ARSHeader(string buff)
        {
            if (buff.ToUpper().Contains("HDR"))
            {
                string[] header = buff.Split(';');
                string eur_amt = header[8];
                string php_amt = header[6];
                string usd_amt = header[7];

                try
                {
                    this._header_amount = Math.Round(Convert.ToDouble(eur_amt) + Convert.ToDouble(php_amt) + Convert.ToDouble(usd_amt), 2);
                }
                catch
                {
                    throw new Exception("Invalid Header. \n\nPlease check the header info, some records are not valid format.");
                }

                return true;
            }
            if (buff.ToUpper().Contains("TRL"))
            {
                return true;
            }

            return false;
        }

        private void getFileStreamReader()
        {
            //string path = AppDomain.CurrentDomain.BaseDirectory + "temp" + @"\" + this.fn;
            csFileIo io = new csFileIo();

            try
            {
                if (io.exist(this.path))
                {
                    this.sr = new StreamReader(this.path);
                    
                }
            }
            catch { }
        }

        public void fileLocation()
        {
            try
            {
                this.path = new irsConfiguration().appsetting("temp_dir").ToString();
                this.newPath = new irsConfiguration().appsetting("temp_dir").ToString() + @"\ARSConvert\";
            }
            catch { throw new Exception("Incomplete config info: temp_dir"); }
        }

        public ARSValidation(string fn)
        {
            this.fileLocation();
            this._header_amount = 0;
            this._actual_amount = 0;
            this.fn = fn;
            this.path += @"\" + fn;
        }

        public ARSValidation()
        {
            this.fileLocation();
        }
    }
}