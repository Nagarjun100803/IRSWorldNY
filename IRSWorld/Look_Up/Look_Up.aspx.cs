using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using IRSClassLib;
namespace IRSWorld.Look_Up
{
    public partial class Look_Up : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Look_Up));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        /* RFC-31620 */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string isAdminApproverTag()
        {
            string ret = string.Empty;
            if (Session["AdminApprovalFlagLookupMessage"] != null)
            {
                if (new irsUsers().adminApproverSwitch())
                {
                    return Session["AdminApprovalFlagLookupMessage"].ToString();
                }
            }
            return ret;
        }

        #region AJAX PUBLIC METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record()
        {
            return this.do_look_up("INSERT");
        }

       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string field_group)
        {
            Session["LOOK_UP_SEQ_ID"] = field_group;

            return this.do_look_up("UPDATE");
        }

       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool delete_record()
        {
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record(string param_values)
        {
            this.parameter_values = param_values;
            return this.do_look_up("COMMIT");
        }

       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.do_look_up("ROLLBACK");
        }

       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["LOOKUP_MESSAGE"] != null)
            {
                return (string)Session["LOOKUP_MESSAGE"];
            }
            return string.Empty;
        }

       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["LOOKUP_MESSAGE"] = null;
            Session["LOOKUP_MESSAGE"] = msg;
        }

       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_LOOKUP_RECORD(string column_values)
        {            
            irsLookUp lookup = this.LOOK_UP();

            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (lookup.set_record(s) == false)
                {
                    this.set_message(lookup.messages); return false;
                }

                Session["IRS_LOOK_UP"] = lookup;

                return true;
            }

            lookup = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_LOOKUP_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_LOOK_UP_RECORD(string client_seq_id)
        {
            irsLookUp lookup = this.LOOK_UP();
            try
            {
                if (lookup != null)
                {
                    if (lookup.get_record(client_seq_id) == true)
                    {
                        return lookup.getdataRecord();
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                Session["IRS_LOOK_UP"] = lookup;
            }
        }

       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string LOOK_UP_SEQ_ID()
        {
            if (Session["LOOK_UP_SEQ_ID"] != null)
            {
                return (string)Session["LOOK_UP_SEQ_ID"];
            }

            return "0";
        }

        /* VAPT 20190206 */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getlookup_filter()
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_lookupfilter");
                    sqcommands.Parameters.Add("@Role", SqlDbType.Int, 100).Value = getRole();
                    return commons.getDataSet(sqcommands, "CIF_LOOKUP");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }

            return null;

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getfld_value(string fld_grp)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_getfldvalue");
                    sqcommands.Parameters.Add("@FldGroup", SqlDbType.VarChar, 100).Value = fld_grp;
                    return (commons.getDataSet(sqcommands, "getfld_value1"));
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null) { commons = null; }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
            
        }

       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet check_lookupvalue(string fldcode, string fldgroup)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_checklookupvalue");
                    sqcommands.Parameters.Add("@FLDCODE", SqlDbType.VarChar, 40).Value = fldcode;
                    sqcommands.Parameters.Add("@FLDGROUP", SqlDbType.VarChar, 30).Value = fldgroup;
                    return commons.getDataSet(sqcommands, "Lookup_FldValue");
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null) { commons = null; }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }


        #endregion


        #region PRIVATE METHOD: LOOK_UP 

        private irsLookUp LOOK_UP()
        {
            if (Session["IRS_LOOK_UP"] == null)
            {
                Session["IRS_LOOK_UP"] = new irsLookUp();
            }

            return (irsLookUp)Session["IRS_LOOK_UP"];
        }

        private bool do_look_up (string instruction)
        {
            irsLookUp cad = this.LOOK_UP();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            cad.selectedRow = -1;
                            if (cad.insert_record() == false)
                            {
                                this.set_message(cad.messages); return false;
                            }
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "UPDATE")
                    {
                        try
                        {
                            /* VAPT 20180222 */
                            Session["LOOKUP_EDIT_SEQID"] = cad.seq_id;
                            cad.seq_id = this.LOOK_UP_SEQ_ID();
                            cad.selectedRow = 0;

                            if (cad.update_record() == false)
                            {
                                this.set_message(cad.messages); return false;
                            }

                            return true;

                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "COMMIT")
                    {
                        try
                        {                            
                            if (this.SET_LOOKUP_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            /* VAPT 20180222 */
                            string oldValue = string.Empty;
                            string newValue = string.Empty;
                            string[] arrNew;
                            string[] arrValue;
                            string val = string.Empty;
                            int i = 0;
                            bool isEdit = false;
                            DataTable dtLookup = new DataTable();

                            if (Session["LOOKUP_EDIT_SEQID"] != null)
                            {
                                string sql = "SELECT * FROM tbl_Lookup WHERE SEQ_ID = " + Session["LOOKUP_EDIT_SEQID"].ToString();
                                DataSet ds = new irsSqlCommonsWs().getDataSet(sql, false, new Object[] { });

                                if (ds != null && ds.Tables[0].Rows.Count > 0)
                                {
                                    isEdit = true;
                                    dtLookup = ds.Tables[0];
                                    oldValue = "Field Group=" + ds.Tables[0].Rows[0]["FldGroup"].ToString() + " ~ ";
                                    oldValue += "Field Code=" + ds.Tables[0].Rows[0]["FldCode"].ToString() + " ~ ";
                                    oldValue += "Field Value=" + ds.Tables[0].Rows[0]["FldValue"].ToString() + " ~ ";
                                    oldValue += "Field Description=" + ds.Tables[0].Rows[0]["FldDescription"].ToString();
                                }
                            }

                            arrNew = this.parameter_values.Split('|');
                            for (i = 0; i <= (arrNew.Length - 1); i++)
                            {
                                arrValue = arrNew.GetValue(i).ToString().Split('~');
                                switch (arrValue.GetValue(0).ToString().ToUpper())
                                {
                                    case "FLDGROUP":
                                        val = "FIELD GROUP";
                                        break;

                                    case "FLDCODE":
                                        val = "FIELD CODE";
                                        break;

                                    case "FLDVALUE":
                                        val = "FIELD VALUE";
                                        break;

                                    case "FLDDESCRIPTION":
                                        val = "FIELD DESCRIPTION";
                                        break;
                                }
                                newValue += val + "=" + arrValue.GetValue(1).ToString() + " ~ ";
                            }
                            newValue = newValue.Substring(0, newValue.Length - 2);

                            /* RFC-31620 */
                            if (new irsUsers().adminApproverSwitch())
                            {
                                Session["AdminApprovalFlagLookupMessage"] = null;
                                string action = "RECORD SUBMITTED FOR APPROVAL";
                                DataTable dt = new DataTable();
                                DataRow dr;

                                dt.Columns.Add("Main_SEQ_ID");
                                dt.Columns.Add("FldGroup");
                                dt.Columns.Add("FldCode");
                                dt.Columns.Add("FldValue");
                                dt.Columns.Add("FldDescription");
                                dt.Columns.Add("IsEditable");
                                dt.Columns.Add("FC_Lookup");
                                dt.Columns.Add("IsSecurity");
                                dr = dt.NewRow();

                                string[] arrLookup = this.parameter_values.Split('|');
                                if (isEdit)
                                {
                                    dr[0] = cad.seq_id; //Main_SEQ_ID
                                }
                                else
                                {
                                    dr[0] = null;
                                }
                                dr[1] = arrLookup.GetValue(0).ToString().Split('~').GetValue(1); //FldGroup
                                dr[2] = arrLookup.GetValue(1).ToString().Split('~').GetValue(1); //FldCode
                                dr[3] = arrLookup.GetValue(2).ToString().Split('~').GetValue(1); //FldValue
                                dr[4] = arrLookup.GetValue(3).ToString().Split('~').GetValue(1); //FldDescription
                                dt.Rows.Add(dr);

                                if (isEdit)
                                {
                                    if (new irsUsers().IsMakerAllowedLookup(irsUsers.ActionType.EDIT, cad.seq_id))
                                    {
                                        throw new Exception("There is a pending approval for the same lookup. Submission for edit to checker failed.");
                                    }
                                    if (!new irsUsers().saveLookupMakerChecker(irsUsers.ActionType.EDIT, dt, dtLookup))
                                    {
                                        this.set_message("Maker Checker saving [edit] failed");
                                        return false;
                                    }
                                }
                                else
                                {
                                    if (!new irsUsers().saveLookupMakerChecker(irsUsers.ActionType.ADD, dt, dtLookup))
                                    {
                                        this.set_message("Maker Checker saving [add] failed");
                                        return false;
                                    }
                                }
                                Session["AdminApprovalFlagLookupMessage"] = action;
                            }
                            else
                            {
                                if (cad.commit_record() == false)
                                {
                                    /* VAPT 20180222 */
                                    new csAuditLog().writeVAPTLogger("", "SETUP", "FAIL TO CREATE NEW GLOBAL SETUP RECORD", cad.messages, this.parameter_values, "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                                    this.set_message(cad.messages); return false;
                                }

                                /* VAPT 20180222 */
                                if (oldValue != string.Empty)
                                {
                                    new csAuditLog().writeVAPTLogger("", "SETUP", "USER UPDATED GLOBAL SETUP RECORD", oldValue, newValue, "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                                else
                                {
                                    new csAuditLog().writeVAPTLogger("", "SETUP", "USER CREATED NEW GLOBAL SETUP RECORD", "", newValue, "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                            }
                            Session["LOOKUP_EDIT_SEQID"] = null;
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "ROLLBACK")
                    {
                        try
                        {
                            cad.rollback_record(); return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
              
                Session["IRS_LOOK_UP"] = null;
                Session["IRS_LOOK_UP"] = cad;
                Session["LOOK_UP_SEQ_ID"] = null;
                Session["LOOK_UP_SEQ_ID"] = cad.seq_id;
                cad = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        #endregion

        /* VAPT 20190206 */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRole()
        {
            return ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Role.ToString();
        }
    }
}
 