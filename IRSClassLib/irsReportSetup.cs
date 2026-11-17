using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace IRSClassLib
{
    public class irsReportSetup : sqlHelperWs
    {
        #region PRIVATE VARIABLE
        private string error_message = null;
        private string _seq_id = null;
        private string _fldgroup = null;
        private string _fldbranchcode = null;
        #endregion
        #region PROPERTY GET & SET

        public string messages
        {
            get { return this.error_message; }
        }

        public string seq_id
        {
            get { return this._seq_id; }
            set { _seq_id = value; }
        }

        public string fldgroup
        {
            get { return this._fldgroup; }
            set { _fldgroup = value; }
        }
        public string fldbranchcode
        {
            get { return this._fldbranchcode; }
            set { _fldbranchcode = value; }
        }
        #endregion
        #region PUBLIC METHOD
        public irsReportSetup()
        {
            base.EnforceConstraint = false;
            this.TableName = "tbl_Reports_per_branch";
            this.IdentityColumn = "SEQ_ID";
        }
        public void getReportLookup(string branchcode)
        {
            SqlCommand cmd = base.getSqlCommand("sp_get_tbl_reportsbranch",true) ;
            cmd.Parameters.Add("@v_branchcode",SqlDbType.VarChar,5).Value  = branchcode ;
            base.getUpdateDataSet(cmd);
            cmd = null;
        }
        
        public bool applyUpdates(String crud, int row, Hashtable ht)
        {
            Object oid = ht["SEQ_ID"];
            String stat = (String)ht["status"];
            _fldbranchcode = (String)ht["BRCD"];
       
            this.getReportLookup(_fldbranchcode);
       
            if ((oid == null | oid.ToString() == String.Empty | oid.ToString() == "null" | oid.ToString() == "0" ) & stat.Equals("ACTIVE"))
            {
                return do_add(ht);
            }
            if (oid.ToString() == "null" & stat.Equals("NOT ACTIVE"))
            {
                return do_add(ht);
                //return true;            
            }
            if ((oid.ToString() != "null" | oid != null) & stat.Equals("NOT ACTIVE"))
            {
                int frow = findRow("SEQ_ID", Convert.ToInt32(ht["SEQ_ID"]));
                if (frow > -1)
                {
                    selectedRow = frow;
                    bool deloks = base.Delete(sqlHelperWs.RETAIN_DATASET, false);
                    return deloks;
                }
                Message = "Unable to find SEQ_ID: " + ht["SEQ_ID"].ToString();
                return false;
            }
            if ((oid.ToString() != "null" | oid != null) & stat.Equals("ACTIVE"))
            {

                return true;
            }
            return true;
        }
        public DataTable saveUpdates(string branchcode)
        {
            if (dataset.HasChanges())
            {
                base.save(sqlHelperWs.RETAIN_DATASET);
                return this.get_reportlookup(branchcode).Tables[0];
            }
            return this.get_reportlookup(branchcode).Tables[0];

            
        }
        public DataSet get_reportlookup(string branchcode)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = base.getSqlCommand("sp_get_reportlookup");
                base.deriveParameters(cmd, new Object[] { branchcode });
                ds = base.getDataSet(cmd);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                ds = null;
            }

        }
        public DataSet load_report_maingroup(string branchcode,int isadmin)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd;
            try
            {
                ds = null;
                cmd = base.getSqlCommand("sp_get_rep_group",true);
                cmd.Parameters.Add("@v_branchcode", SqlDbType.VarChar, 5).Value = branchcode;
                cmd.Parameters.Add("@v_isadmin", SqlDbType.Bit ).Value = isadmin;
                ds = base.getDataSet(cmd, "tbl_reports");
                return ds;
            }
            catch(Exception e)
            {
                this.error_message = e.Message;
                return null;
            }
            
        }
        public DataSet load_report_perbranch(string branchcode)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd;
            try
            {
                ds = null;
                cmd = base.getSqlCommand("sp_get_reportlookup", true);
                cmd.Parameters.Add("@v_branchcode", SqlDbType.VarChar, 5).Value = branchcode;                
                ds = base.getDataSet(cmd, "tbl_reports");
                return ds;
            }
            catch (Exception e)
            {
                this.error_message = e.Message;
                return null;
            }

        }
        #endregion
        #region PRIVATE METHOD
        private void concat_errs(ArrayList al)
        {
            Message = "";
            foreach (Object o in al)
            {
                Message += (String)o + "\n";
            }
        }
        private bool do_add(Hashtable ht)
        {
            bool ok = base.newRow(sqlHelperWs.RETAIN_DATASET);
            if (ok)
            {
                foreach (Object key in ht.Keys)
                {
                    if (key.ToString().Equals(this.IdentityColumn)) continue;
                    if (!setColumn(key.ToString(), ht[key]))
                    {
                        //ok = false;
                        continue;
                    }
                }
                if (ok & addMode)
                {
                    table.Rows.Add(uprow);
                    addMode = false;
                }
            }
            return ok;
        }
        #endregion
    }
}
