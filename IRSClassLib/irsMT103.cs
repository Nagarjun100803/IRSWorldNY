using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Data.SqlClient;
namespace IRSClassLib
{
    public class irsMT103 : sqlHelper
    {
        private DataTable dt        = new DataTable();
        private string _tranxdate   = string.Empty;
        private string _message     = string.Empty;
        private string _filename    = string.Empty;
        private string _path        = string.Empty;
        private bool _peso;
        private int ctr             = 0;
        private int maxctr;
        private int newrecord_ctr;
        csAuditLog oLog             = new csAuditLog();
        string sysid                = "irsMT103";
        Object bocCTR               = new Object();

        private bool got_peso_account
        {
            get { return this._peso; }
            set { this._peso = value; }
        }

        public irsMT103(string tranxdate, string filename, string path)
        {
            this._tranxdate = tranxdate;
            this._filename = filename;
            this._path = path;
        }
        public void extract()
        {
            string filecontent = string.Empty;
            
            csFileIo io = null;

            try
            {
                this.prepareData();
                this.getData();

                if (this.dt != null)
                {
                    if (this.dt.Rows.Count > 0)
                    {
                        //this.get_BOC_ctr();
                        //ctr = Convert.ToInt32(ictr.Rows[0]["refnum_ours"]);

                        io = new csFileIo();
                        if (io.OutPutFileName == null)
                        {
                            if (!io.directoryExist(this._path)) { io.createDirectory(this._path); }
                            Directory.SetCurrentDirectory(this._path);

                            this.got_peso_account = io.open_output(this._filename, (int)csFileIo.write_modes.CREATE);
                        }

                        foreach (DataRow dr in dt.Rows)
                        {
                            newrecord_ctr = newrecord_ctr + 1;
                            ctr += 1;
                            filecontent = this.construct_line_record(dr, ctr);
                            if (this._message.Length == 0)
                            {
                                if (newrecord_ctr != this.dt.Rows.Count)
                                {
                                    filecontent += "$";
                                }
                                io.putrecord(filecontent);
                            }
                        }
                        this.maxctr = ctr;
                        oLog.writeAudit(sysid, sysid + ".extract", "Running Extract BOC module.", "Successfully extracted " + ctr + " BOC trxn. Trxndate:" + this._tranxdate + "ExtractNo:" + this.bocCTR, "", csAuditLog.Category.Select);
                    }
                    else
                    {
                        oLog.writeAudit(sysid, sysid + ".extract", "Running Extract BOC module.", "Extracted " + ctr + " BOC trxn. Trxndate:" + this._tranxdate + "ExtractNo:" + this.bocCTR, "", csAuditLog.Category.Select);
                    }
                }
            }
            catch (Exception e)
            {
                _message += "ERROR IN METHOD: PULL_RECORDSET.  MESSAGE: " + e.Message;
                oLog.writeAudit(sysid, sysid+".run_eot", "Running Extract BOC module.", "Error: "+ _message, "", csAuditLog.Category.Select);
            }
            finally
            {                
                if (io != null)
                {
                    if (io.OutPutFileName != null & io.OutPutFileName.Trim().Length > 0)
                    {
                        io.close_output(); io = null;
                    }
                }
            }
        }

        private string construct_line_record(DataRow dr, int ctr)
        {
            String nl = Environment.NewLine;
            int maxctr;
            int maxctr_address1;
            int maxctr_address2;
            int ctr_diff = 0;
            StringBuilder s     = new StringBuilder();
            string ts           = dr["tran_source"].ToString().Trim();
            string td           = dr["tran_date"].ToString().Trim();
            string datefx       = dr["date_fxcurr_amount"].ToString().Trim();
            string refnum       = dr["trxnref_num"].ToString().Trim();
            string remname      = dr["rem_name"].ToString().Trim();
            string remadd       = dr["rem_address"].ToString().Trim();
            string benebnk      = dr["benebank"].ToString();
            string benebankcity = dr["benebankcity"].ToString();
            string act          = dr["acct_num"].ToString().Trim();
            string benename     = dr["benef_name"].ToString().Trim();
            string beneadd      = dr["benf_address1"].ToString().Trim();
            string beneadd2     = dr["benf_address2"].ToString().Trim();
            string benenum      = dr["benf_mobileno"].ToString().Trim();
            string remnum       = dr["rem_mobileno"].ToString().Trim();

            try
            {
                //string[] spl_mesg = dr["spl_mesg"].ToString().Trim().Split('/');
                if (dr != null)
                {
                    s.Append("{1:F01PNBMPHMMAXXX0000000000}{2:I103BKCHPHMMXXXXN}{4:" + nl);

                    s.Append(":20:PNB" + ts + td + ctr.ToString().PadLeft(5, '0') + nl);
                    s.Append(":23B:CRED" + nl);
                    s.Append(":32A:" + datefx + nl);
                    s.Append(":50F:");
                    s.Append("/" + refnum + nl); // Remitter ID
                    s.Append("1/" + remname + nl);
                    if (remadd != String.Empty | remadd.Length != 0)
                    {
                        maxctr = remadd.Length;
                        if (maxctr <= 33)
                        {
                            s.Append("2/" + remadd + nl);
                        }
                        else
                        {
                            s.Append("2/" + remadd.Substring(0, 32) + nl);
                            s.Append("3/" + remadd.Substring(32, maxctr - 32) + nl);
                        }
                    }

                    s.Append(":52A:PNBMPHMM" + nl);
                    s.Append(":57D://CHINA" + nl);
                    if (benebnk != String.Empty | benebnk.Trim().Length != 0) // bene bank
                    {
                        maxctr = benebnk.Length;
                        if (maxctr <= 35)
                        {
                            s.Append(benebnk + nl);
                        }
                        else
                        {
                            s.Append(benebnk.Substring(0, 34) + nl);
                            s.Append(benebnk.Substring(34, maxctr - 35) + nl);
                        }
                    }

                    if (benebankcity != String.Empty | benebankcity.Length != 0) // bene bank city
                    {
                        maxctr = benebankcity.Length;
                        if (maxctr <= 35)
                        {
                            s.Append(benebankcity + nl); 
                        }
                        else
                        {
                            s.Append(benebankcity.Substring(0, 34) + nl); 
                            s.Append(benebankcity.Substring(34, maxctr - 35) + nl);
                        }

                    }
                    s.Append(":59:");
                    if (act != String.Empty | act.Length != 0)
                    {
                        s.Append("/" + get_account(act) + nl);
                    }
                    //s.Append("/");
                    if (benename != String.Empty | benename.Length != 0)
                    {
                        s.Append(benename + nl);
                    }
                    if (beneadd != String.Empty | beneadd.Length != 0)
                    {
                        maxctr_address1 = beneadd.Length;
                        if (maxctr_address1 <= 34)
                        {
                            ctr_diff = 0;
                            s.Append(beneadd + nl);
                        }
                        else
                        {
                            ctr_diff = maxctr_address1 - 34;
                            s.Append(beneadd.Substring(0, 34) + nl);
                            s.Append(beneadd.Substring(34, maxctr_address1 - 34));
                            if (beneadd2 == String.Empty | beneadd2.Length == 0)
                            {
                                s.Append(nl);
                            }
                        }
                    }
                    if (beneadd2 != String.Empty | beneadd2.Length != 0)
                    {
                        maxctr_address2 = beneadd2.Length;
                        if (ctr_diff == 0)
                        {
                            if (maxctr_address2 <= 34)
                            {
                                s.Append(beneadd2 + nl);
                                if (benenum != String.Empty | benenum.Length != 0)
                                {
                                    s.Append(benenum + nl);
                                }
                            }
                            else
                            {
                                s.Append(beneadd2.Substring(0, 34) + nl);
                                s.Append(beneadd2.Substring(34, maxctr_address2 - 34) + nl);
                            }
                        }
                        else
                        {
                            if (maxctr_address2 <= (35 - ctr_diff))
                            {
                                s.Append(beneadd2 + nl);
                            }
                            else
                            {
                                s.Append(beneadd2.Substring(0, (34 - ctr_diff)) + nl);
                            }
                        }
                    }

                    if (remnum != String.Empty | remnum.Length != 0 | remadd.Length >= 71) // remitter add
                    {
                        s.Append(":70:"); // if any
                        maxctr = remadd.Length;
                        if (maxctr >= 71)
                        {
                            s.Append(remadd.Substring(69, 35) + nl);
                            if (maxctr >= 106 & maxctr <= 140)
                            {
                                s.Append(remadd.Substring(104, maxctr - 70) + nl);
                                if (remnum != String.Empty | remnum.Length != 0)
                                {
                                    s.Append(remnum + nl); // Remitter Contact No
                                }
                            }
                            else
                            {
                                s.Append(remadd.Substring(104, 35) + nl);
                                s.Append(remadd.Substring(139, 35) + nl);
                            }
                        }
                        else
                        {
                            if (remnum != String.Empty | remnum.Length != 0)
                            {
                                s.Append(remnum + nl); // Remitter Contact No
                            }
                        }
                    }

                    s.Append(":71A:SHA" + nl);
                    s.Append("-}");
                }
            }
            catch (Exception e) { _message += "Error: " + e.StackTrace + e.Message + ""; }

            return s.ToString();
        }
        private string get_account(string acctno)
        {
            string act = string.Empty;

            int i_nos = 0;
            int i = 0;


            for (i = 0; i < acctno.Length; i++)
            {
                if (int.TryParse(acctno.Substring(i, 1), out i_nos) == true)
                {
                    act += acctno.Substring(i, 1);
                }
            }
            return act;
        }
        
        private void getData()
        {
            sqlHelper com = new sqlHelper();
            SqlCommand cmnd = com.getSqlCommand("sp_Ibatch_extract_BOC_data");

            try
            {
                Object[] param = { this._tranxdate, this.bocCTR };
                com.deriveParameters(cmnd, param);                
                com.Message = String.Empty;                
                this.dt = com.getDataSet(cmnd, "TBL_DATA").Tables[0];
            }

            catch(Exception e) 
            {
                oLog.writeAudit(sysid, sysid+".getData", "Running Extract BOC module.", "Error: "+ e.Message, "", csAuditLog.Category.Select);
            }
            finally
            {
                if (cmnd != null)
                {
                    cmnd.Dispose(); cmnd = null;
                }
            }
        }
        private void prepareData()
        {
            sqlHelper com = new sqlHelper();
            SqlCommand cmnd = com.getSqlCommand("sp_Ibatch_extract_BOC");

            try
            {
                Object[] param = { this._tranxdate, String.Empty , String.Empty};

                com.deriveParameters(cmnd, param);
                cmnd.Parameters["@v_errmsg"].Direction = ParameterDirection.Output;
                cmnd.Parameters["@ctr"].Direction = ParameterDirection.Output;
                com.Message = String.Empty;
                int i = com.sqlUpdate(cmnd);
                Object err  = (Object)cmnd.Parameters["@v_errmsg"].Value;
                bocCTR      = (Object)cmnd.Parameters["@ctr"].Value;
                if (err != DBNull.Value && err.ToString() != String.Empty)
                {
                    _message += "ERROR IN METHOD: PROCEDURE GET BOC CTR.  MESSAGE: " + err.ToString();
                }
                else if (com.Message != null && com.Message != String.Empty)
                {
                    _message += "ERROR IN METHOD: GET BOC CTR.  MESSAGE: " + com.Message;
                }
                DataSet ds = com.getDataSet(cmnd);
            }

            catch (Exception e)
            {
                _message += "ERROR IN METHOD: GET BOC CTR EXCEPTION.  MESSAGE: " + e.Message;
            }
            finally
            {
                if (cmnd != null)
                {
                    cmnd.Dispose(); cmnd = null;
                }
            }
        }

        ~irsMT103() { }
    }
}
