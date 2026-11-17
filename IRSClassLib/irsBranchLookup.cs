using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace IRSClassLib
{
    [Serializable]
    public class irsBranchLookup : sqlHelperWs
    {

        #region PRIVATE VARIABLE        
        private string error_message            = null;
        private string _seq_id                  = null;
        private string _fldgroup                = null;
        private string _fldbranchcode           = null;
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
        #region PUBLIC / PRIVATE METHOD
        public irsBranchLookup()
        {
            this.TableName = "TBL_SUB_LOOKUP";
            this.IdentityColumn = "SEQ_ID";
        }


        public void getSubLookup()
        {
            base.getUpdateDataSet("sp_sublookup");           
        }
        public void getSubLookup(string branchcode,string subgroupid)
        {
            SqlCommand cmd = base.getSqlCommand("sp_sublookup_by_filter", true);
            cmd.Parameters.Add("@v_branchcode", SqlDbType.VarChar, 5).Value = branchcode;
            cmd.Parameters.Add("@v_subgroupid", SqlDbType.VarChar, 50).Value = subgroupid;

            base.getUpdateDataSet(cmd);
        }
        public void getRefreshData(string Branchcode)
        {
            SqlCommand cmd = base.getSqlCommand("sp_get_branchsublookup");
            base.deriveParameters(cmd, new Object[] { Branchcode });
                base.getUpdateDataSet(cmd);
        }
        public DataSet get_branchsublookupbysubgroup(string branchcode,string sublookup)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = base.getSqlCommand("sp_get_branchsublookup_per_group");
                base.deriveParameters(cmd, new Object[] { branchcode,sublookup });
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
        public DataSet get_branches(string branchcode)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = base.getSqlCommand("get_dropdown_branches_byaccess");
                cmd.Parameters.Add("@Location", SqlDbType.VarChar, 5).Value = branchcode;
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
        public DataSet get_branchsublookupbygroup(string branchcode,int isadmin)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = base.getSqlCommand("sp_get_branch_group");
                base.deriveParameters(cmd, new Object[] { branchcode ,isadmin });
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
        public DataSet get_branchsublookupbyfilter(string branchcode,string sublookup,string searchval, string searchtype)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = base.getSqlCommand("sp_filter_branchsublookup");
                base.deriveParameters(cmd, new Object[] { branchcode, sublookup,searchval ,searchtype  });
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
        public DataSet get_branchsublookup(string branchcode)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = base.getSqlCommand("sp_get_branchsublookup");
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
            bool  ok = base.newRow(sqlHelperWs.RETAIN_DATASET);
            if (ok)
            {
                foreach (Object key in ht.Keys)
                {
                    if (key.ToString().Equals(this.IdentityColumn)) continue;
                    Object val = ht[key];                    
                    if (!setColumn(key.ToString(), val))
                    {
                       
                    }
                }
                if (ok & addMode)
                {
                    
                    addMode = false;                    
                    table.Rows.Add(uprow);
                    base.save(sqlHelperWs.RETAIN_DATASET);
                }
            }
            return ok;
        }

        private void validate_entry(Hashtable htentry)
        {

        }
        
        #region oldapply
        public bool applyUpdates(String crud, int row, Hashtable ht)
        {
            Object oid = ht["SEQ_ID"];
            String stat = (String)ht["status"];
            _fldbranchcode = (String)ht["BRCD"];
            _fldgroup = (String)ht["FldGroup"];

            this.getSubLookup(_fldbranchcode, _fldgroup);


          
            if ((oid.ToString() == "0" || oid.ToString()  == "" ) && stat.Equals("ACTIVE"))
            {
                return do_add(ht);
            }
            if (oid.ToString() == "null" & stat.Equals("ACTIVE"))
            {
                return do_add(ht);                
            }
            if ((oid.ToString() != "null" | oid != null) & stat.Equals("NOT ACTIVE"))
            {
                try
                {
                    int frow = findRow("SEQ_ID", Convert.ToInt32(ht["SEQ_ID"]));
                    if (frow > -1)
                    {
                        selectedRow = frow;
                        return base.Delete(sqlHelperWs.RETAIN_DATASET, true);
                    }
                    
                   
                }
                catch 
                {
                    
                }
                return true;
            }
            
            return true;
        }

        public bool applyUpdates_tmp(String crud, int row, Hashtable ht, string userID)
        {
            this.TableName = "select * from tbl_sub_lookup_tmp";
            this.IdentityColumn = "SEQ_ID";
            string sqlstmt = "SELECT * FROM tbl_sub_lookup_tmp WHERE 1=0";
            base.getUpdateDS(sqlstmt);

            //Object oid = ht["SEQ_ID"];
            //String stat = (String)ht["status"];
            //_fldbranchcode = (String)ht["BRCD"];
            //_fldgroup = (String)ht["FldGroup"];

            bool ok = false;
            ok = base.newRow(sqlHelperWs.RETAIN_DATASET);
            if (ok)
            {
                string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                uprow["BranchCode"] = (String)ht["BRCD"];
                uprow["FldGroup"] = (String)ht["FldGroup"];
                uprow["FldValue"] = (String)ht["FldValue"];
                uprow["FldCode"] = (String)ht["FldCode"];
                uprow["Status"] = (String)ht["status"];
                uprow["MAIN_SEQ_ID"] = Convert.ToInt32(ht["SEQ_ID"]);
                uprow["Maker"] = userID;
                uprow["DateAdded"] = dateNow;
                uprow["CurrStatus"] = "For Approval";
                uprow["StatusDate"] = dateNow;
                uprow["ActionType"] = "EDIT";
                uprow["Module"] = "Branch Administration >> Lookup Setup";
                uprow["MenuName"] = "Lookup Setup";
                table.Rows.Add(uprow);
                base.addMode = false;
                
            }
            return base.save(sqlHelperWs.RETAIN_DATASET);


        }

        #endregion
        public void saveUpdates()
        {
            if (dataset.HasChanges() )
            {
                base.save(sqlHelperWs.RETAIN_DATASET);
                this.getSubLookup(_fldbranchcode, _fldgroup);                  
            }

            
        }
        #endregion

    }
}
