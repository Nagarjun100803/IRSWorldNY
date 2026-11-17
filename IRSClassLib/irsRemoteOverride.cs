/*
 * AUTHOR       : JEFFREY M. BUIZON
 * DESCRIPTION  : PROGRAM FOR REMOTE OVERRIDE
 */
using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.IO;

namespace IRSClassLib
{
    [Serializable]
    public class irsRemoteOverride : sqlHelperWs
    {
        #region VARIABLES
        public ArrayList ErrorMessage = new ArrayList();
        private csAuditLog audit = new csAuditLog();
        #endregion

        public irsRemoteOverride()
        {
            this.TableName = "TBL_OVERRIDE";
            this.IdentityColumn = "SEQ_ID";
        }
        ~irsRemoteOverride()
        {
            if (audit != null)
            {
                audit = null;
            }
        }

        #region PUBLIC METHOD FOR OVERRIDE
        public string set_override_stat(string branchcode, string trandesc, string prodchargedid, string userid, string splmsg, string details,string overridetype, string overridedetails, DateTime tranxdate)
        {
            string sql = "SELECT TOP 1 * FROM TBL_OVERRIDE";            
            try
            {
                base.getUpdateDataSet(sql, false);
                bool ok = base.newRow(sqlHelperWs.RETAIN_DATASET);

                if (ok)
                {
                    base.setColumn("STATUS", 1);
                    base.setColumn("RODATE", tranxdate);
                    base.setColumn("TRANDESC", trandesc);
                    base.setColumn("PRODUCTCHARGESID", prodchargedid);
                    base.setColumn("BRANCHCODE", branchcode);
                    base.setColumn("USERID", userid);
                    base.setColumn("SPLMSG", splmsg);
                    base.setColumn("DETAILS", details);
                    base.setColumn("OVERRIDETYPE", overridetype);
                    base.setColumn("OVERRIDEDETAILS", overridedetails);
                    //base.table.Rows.Add(uprow);
                    if (base.save())
                    {
                        addMode = false;
                        return "1" + ";" + base.IdentityValue;
                    }
                    else
                    {
                        throw new Exception("ERROR WHILE SAVING STATUS");
                    }
                }
                throw new Exception("ERROR WHILE SETTING STATUS");
            }
            catch (Exception e)
            {
                try
                {
                    audit.writeAudit(userid, "ROERREX", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch { }
                return "-1;0";
            }
            

        }
        public string check_override_stat(string overrideid ,string branchcode, string userid)
        {
            string param = overrideid + "," + branchcode + "," + userid;
            DataSet ds = new DataSet();
            ds = this.execStoredProc("proc_check_override_stat", param.Split(','));

            try
            {
                if (ds.Tables[0].Rows[0]["PWD"].ToString().Trim().Length == 0)
                {
                    return ds.Tables[0].Rows[0]["ROSTATUS"].ToString() + "~" + ds.Tables[0].Rows[0]["OVERRIDETYPE"].ToString() + "~ ~ ";
                }
                return ds.Tables[0].Rows[0]["ROSTATUS"].ToString() + "~" + ds.Tables[0].Rows[0]["OVERRIDETYPE"].ToString()  
                        + "~" 
                        + ds.Tables[0].Rows[0]["APPROVER"].ToString()   
                        + "~" 
                        + this.decryptpassword(ds.Tables[0].Rows[0]["PWD"].ToString());
            }
            catch (Exception e)
            {
                try
                {
                    audit.writeAudit(userid, "ROERREX", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch { }
                return "-1;" + e.Message + ";ERROR";
            }
            finally { if (ds != null) { ds.Dispose(); ds = null; } }

        }
        public int cancel_override_stat(string overrideid, string branchcode, string userid)
        {
            string param = branchcode + "," +   userid  + "," + overrideid ;

            DataSet ds = new DataSet();
            ds = this.execStoredProc("proc_cancel_items", param.Split(','));
            try
            {
                return Convert.ToInt32(ds.Tables[0].Rows[0]["STATUS"].ToString());
            }
            catch (Exception e)
            {
                try
                {
                    audit.writeAudit(userid, "ROERREX", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch { }
                return 4;
            }
        }

        #endregion
        #region PUBLIC METHOD FOR APPROVER
        public Boolean check_approver_roles(string userid, string role,string prodchargesid)
        {
            string param = role + "," + prodchargesid;
            DataSet ds = new DataSet();
            ds = this.execStoredProc("proc_check_approvers", param.Split(','));
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                    if (ds.Tables[0].Rows[0]["APPROVERS"].ToString() != "0")
                        return true;
                return false;
            }
            catch (Exception e)
            {
                try
                {
                    audit.writeAudit(userid, "ROERREX", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch { }
                return false;
            }
            finally { if (ds != null) { ds.Dispose(); ds = null; } }
        }
        public DataTable check_items_forApproval(string _branchcode,int _seqid,string _userid,DateTime _tranxdate)
        {
            string param = _branchcode + ","  + _seqid  + "," + _userid + "," + _tranxdate.ToString();
            DataSet ds = new DataSet();
            ds = this.execStoredProc("proc_check_items_forApproval", param.Split(','));
            try
            {
                return ds.Tables[0];
            }
            catch (Exception e)
            {
                try
                {
                    audit.writeAudit(_userid, "ROERREX", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch { }
                return null;
            }
            finally { if (ds != null) { ds.Dispose(); ds = null; } }
        }
        public Boolean check_items_forApproval(string _branchcode,DateTime _tranxdate,string _userid)
        {
            string param = _branchcode + "," + _tranxdate.ToString();
            DataSet ds = new DataSet();
            csAuditLog log = new csAuditLog();
            ds = this.execStoredProc("proc_select_items_override", param.Split(','));
            try
            {
                return (ds.Tables[0].Rows.Count > 0) ? true : false;
            }
            catch (Exception e)
            {                
                try
                {
                    audit.writeAudit(_userid, "ROERREX", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch { }
                return false;
            }
            finally { log = null; if (ds != null) { ds.Dispose(); ds = null; } }
        }
        public int approve_items(string _branchcode,string _approverid, string _seq_id,string _password, int stat)
        {            
            string encpwd = this.encryptpassword(_password);
            string sql = "SELECT TOP 1 * FROM TBL_OVERRIDE WHERE BRANCHCODE = '" + _branchcode + "' AND SEQ_ID = " + _seq_id;
            try
            {
                base.getUpdateDataSet(sql, false);

                int frow = findRow("SEQ_ID", _seq_id );
                if (frow > -1)
                {
                    selectedRow = frow;
                    base.setColumn("STATUS", stat);
                    base.setColumn("APPROVER1", _approverid);
                    base.setColumn("APPROVER1PWD", encpwd);
                    //base.table.Rows.Add(uprow);
                    if (base.save())
                    {
                        addMode = false;
                        return stat;
                    }
                    
                    throw new Exception("ERROR WHILE UPDATING APPROVER INFO");
                        
                    
                }
                else
                {
                    throw new Exception("ERROR FINDING ROW ");
                }
                
                
            }
            catch (Exception e)
            {
                try
                {
                    audit.writeAudit(_approverid, "ROERREX", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch { }
                return 4;
            }
        }

        public int validate_password(string _branchcode, string _approverid, string _password)
        {
            irsUsers iu = new irsUsers();
            Object[] ireturns = iu.validate(_approverid , _password , Convert.ToByte(1), true);
            try
            {
                return (Convert.ToBoolean(ireturns[0])) ? 1 : 0;
            }
            catch { return 0; }
            finally { iu = null; }
        }
        public int cancel_items(string _branchcode, int _seq_id,string _userid)
        {
            string param = _branchcode + "," + _seq_id;                           
            DataSet ds = new DataSet();
            ds = this.execStoredProc("proc_cancel_items_forApproval", param.Split(','));
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                    return 1;
                return 0;
            }
            catch (Exception e)
            {
                try
                {
                    audit.writeAudit(_userid, "ROERREX", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch { }
                return 0;
            }
            finally { if (ds != null) { ds.Dispose(); ds = null; } }
        }        
        #endregion
        #region PUBLIC METHOD
        public DataSet execStoredProc(string spname, string[] param)
        {
            base.EnforceConstraint = false;
            DataSet ds = this.execStoredProcedure(spname, param);
            return ds;
        }
        #endregion
        #region PRIVATE METHOD
        private string encryptpassword(string pwd)
        {
            string encpwd =   irsUsers.getTriDes().Encrypt(pwd);
            return encpwd;
        }
        private string decryptpassword(string pwd)
        {
            string decpwd = irsUsers.getTriDes().Decrypt(pwd);
            return decpwd;
        }
        #endregion

        #region FOR UNLOCK
        public DataTable checkLockedItems(string branchcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand cmd = commons.getSqlCommand("SELECT SEQ_ID,PRODUCTCHARGESID,CONVERT(VARCHAR,RODATE,101) AS RODATE,TRANDESC,SPLMSG,LOCKED FROM TBL_OVERRIDE WHERE LOCKED=1 and STATUS=1and BRANCHCODE = '" + branchcode + "'", false);
                DataSet ds = commons.getDataSet(cmd);
          
            
            

            DataTable dtNew = ds.Tables[0].Clone();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DataRow urow = dtNew.NewRow();
                    urow["SEQ_ID"] = dr["SEQ_ID"];
                    urow["PRODUCTCHARGESID"] = dr["PRODUCTCHARGESID"];
                    urow["RODATE"] = dr["RODATE"];
                    urow["TRANDESC"] = dr["TRANDESC"];
                    urow["SPLMSG"] = dr["SPLMSG"];
                    urow["LOCKED"] = dr["LOCKED"];

                    dtNew.Rows.Add(urow);
                }

            }
            return dtNew;
        }

        public DataSet saveUnlock(String seqid, string branchcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand cmd = commons.getSqlCommand("UPDATE TBL_OVERRIDE set APPROVER1=null, APPROVER1PWD=null, LOCKED=null, LOCKEDUSERID=null WHERE SEQ_ID in (" + seqid + ") and BRANCHCODE = '" + branchcode + "'", false);
            DataSet ds = commons.getDataSet(cmd);

            return ds;
        }

        public DataSet getBranches(string location)
        {
            sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = commons.execStoredProcedure("get_dropdown_branches_byaccess", new object[] { location });  //commons.getDataSet(commons.getSqlCommand("get_dropdown_branches"), "tblBranches");
            return ds;
        }

        #endregion

        #region APPROVER LIST
        public DataTable displayItems(string branchcode)
        {
            DataSet ds;
            ds = execStoredProcedure("proc_select_items_override", new object[] { branchcode });
            return ds.Tables[0];

        }
        #endregion

        #region OBPS MERCHANT DETAILS
        public DataTable getEnrollMerchantInfo(string RemitterID, string merName, string prodDesc)
        {
            DataSet ds = execStoredProcedure("spGetMerchantInfo_forOverride", new object[] { RemitterID, merName, prodDesc });
            return ds.Tables[0];
        }

        public DataTable getOptionalField(string sp, string merchcode, int prodcode)
        {
            DataSet ds = execStoredProcedure(sp, new object[] { merchcode, prodcode });
            return ds.Tables[0];
        }
        #endregion
    }
}
