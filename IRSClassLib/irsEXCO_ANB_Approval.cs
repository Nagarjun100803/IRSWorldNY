/*
 * Author: Jeffrey M. Buizon
 * Desc:    Program to handle ANB file
 * Date modified: 07/19/2012
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.IO;

namespace IRSClassLib
{
    public class irsEXCO_ANB_Approval
    {
        private Hashtable htItems;
        private StreamReader sr;
        private string _path = "";
        private string _fname = "";
        string newPath = "";
        public irsEXCO_ANB_Approval()
        {

        }

        public irsEXCO_ANB_Approval(string fname)
        {
            this._fname = fname;
            this.fileLocation();
        }
        #region PUBLI METHODS
        public string read_amount(string sbuff)
        {
            string samount = "0";
            string[] scontents = sbuff.Split("|".ToCharArray());
            try
            {
                samount = scontents[14].ToString();
            }
            finally
            {

            }
            return samount;

            
        }
        public bool read_header(string sbuff)
        {
            string samount = "0";
            string[] scontents = sbuff.Split("|".ToCharArray());
            try
            {
                if( scontents[4].ToString().Trim().Length == 0)
                {
                    return true;
                }
            }
            finally
            {

            }
            return false;


        }
        
        public DataSet read_summary()
        {
            this.getFileStreamReader();
            int recctr = 1;
            int fctr = 1;
            DataSet ds = new DataSet();
            DataTable tempDt = this.summaryDataTable();
            string currency = "";
            bool pass = false;
            if (sr != null)
            {
                try
                {
                    
                    while (true)
                    {
                        string buff = sr.ReadLine();

                        if (buff == null) break;
                        if (buff.Length == 1 && Encoding.Default.GetBytes(buff)[0] == 26) continue;
                        if (this.read_header(buff)) { continue; }

                        string[] file_rec = buff.Split('|');

                        currency = this.MappedCurrency(file_rec[12].ToString());
                        if (tempDt.Rows.Count == 0)
                        {
                            DataRow dr = null;
                            dr = tempDt.NewRow();
                            dr["CURRENCY"] = currency;
                            dr["NOOFITEMS"] = 1;
                            dr["AMOUNT"] = Convert.ToDecimal(file_rec[14].ToString());
                            tempDt.Rows.Add(dr);
                            tempDt.AcceptChanges();
                        }
                        else
                        {
                            pass = false;
                            foreach (DataRow r in tempDt.Rows)
                            {
                                if (r["CURRENCY"].ToString().IndexOf(currency) > -1)
                                {
                                    r.BeginEdit();
                                    r["NOOFITEMS"] = Convert.ToInt32(r["NOOFITEMS"].ToString()) + 1;
                                    r["AMOUNT"] = Convert.ToDecimal(r["AMOUNT"].ToString()) + Convert.ToDecimal(file_rec[14].ToString());
                                    r.EndEdit();
                                    r.AcceptChanges();
                                    pass = true;
                                }

                            }
                            if (!pass)
                            {
                                DataRow dr = null;
                                dr = tempDt.NewRow();
                                dr["CURRENCY"] = currency;
                                dr["NOOFITEMS"] = 1;
                                dr["AMOUNT"] = Convert.ToDecimal(file_rec[14].ToString());
                                tempDt.Rows.Add(dr);
                                tempDt.AcceptChanges();
                            }
                        }
                        fctr++;
                    }

                    ds.Tables.Add(tempDt);
                    
                    return ds;
                }
                catch (Exception e) { throw e; }
                finally
                {
                    sr.Close();
                    sr.Dispose();
                    sr = null;
                }
                return null;
                
    
            }

            throw new Exception("Error: validARSFile");


        }

        public void fileLocation()
        {
            try
            {
                this._path = new irsConfiguration().appsetting("temp_dir").ToString() + this._fname;
                this.newPath = new irsConfiguration().appsetting("temp_dir").ToString() + @"\ARSConvert\";
            }
            catch { throw new Exception("Incomplete config info: temp_dir"); }
        }
        private string MappedCurrency(string currcode)
        {
            string curr = "";
            switch (currcode.Trim().ToUpper())
            {
                case "200" :
                    curr = "PHP";
                    break;
                case "67":
                    curr = "USD";
                    break;
                default:
                    curr = currcode;
                    break;

            }
            return curr;
        }
        private DataTable summaryDataTable()
        {
            DataTable dt = new DataTable();
            DataColumn col = new DataColumn();
            
            try
            {
                col.ColumnName = "CURRENCY";
                dt.Columns.Add(col);

                col = new DataColumn();
                col.ColumnName = "AMOUNT";
                dt.Columns.Add(col);

                col = new DataColumn();
                col.ColumnName = "NOOFITEMS";
                dt.Columns.Add(col);

                
                return dt;
            }
            catch { }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                if (col != null)
                {
                    col.Dispose();
                    col = null;
                }
               
            }
            return null;
        }

        private void getFileStreamReader()
        {
            //string path = AppDomain.CurrentDomain.BaseDirectory + "temp" + @"\" + this.fn;
            csFileIo io = new csFileIo();

            try
            {
                if (io.exist(this._path))
                {
                    this.sr = new StreamReader(this._path);

                }
            }
            catch { }
            finally { io = null; }
        }


        #endregion
    }
}
