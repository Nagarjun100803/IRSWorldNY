using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace IRSClassLib
{
    [Serializable]
    public class irsMenu : sqlHelperWs
    {
        private String userid;
        private String crud;

        public override void getUpdateDS(int seqid)
        {
            String sql = "SELECT * FROM " + this.TableName + " WHERE SEQ_ID = " + seqid;
            base.getUpdateDS(sql);
        }

        public int getNextMenuItemNo(int gid, int pid)
        {
            try
            {
                String oper = (pid == 0) ? "MenuGroup = " + gid : "MenuParentID = " + pid;
                Object o = base.execScalar("SELECT max(MenuItemNo) FROM " + this.TableName + " WHERE " + oper);
                if (o != DBNull.Value)
                {
                    int ino = Convert.ToInt32(o);
                    return ino + 1;
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return -1;
        }

        private bool delete_menu(Hashtable ht, int seqid)
        {
            base.getUpdateDS("SELECT * FROM " + this.TableName + " WHERE MenuParentID = " + seqid);
            addMode = false;
            if (isUpdatable && table.Rows.Count > 0)
            {
                bool err = false;
                foreach (DataRow row in table.Rows)
                {
                    try
                    {
                        row.Delete();
                    }
                    catch (Exception e)
                    {
                        Message = e.Message;
                        err = true;
                        break;
                    }
                }
                if (!err)
                {
                    base.save(false);
                }
                else
                {
                    cancel(false);
                    return false;
                }
            }
            getUpdateDS(seqid);
            selectedRow = 0;
            return base.Delete(false);
        }

        public bool edit_menu(Hashtable ht, int seqid)
        {
            int gid = Convert.ToInt32(ht["MenuGroup"]);
            int pid = Convert.ToInt32(ht["MenuParentID"]);

            int dt_gid = Convert.ToInt32(getColumn("MenuGroup"));
            int dt_pid = Convert.ToInt32(getColumn("MenuParentID"));
            if (dt_pid == 0)
            {
                if (dt_gid != gid) // migrate the group into the new group
                {
                    String sqlstring = "UPDATE " + this.TableName + " SET MenuGroup = " + gid + " WHERE MenuGroup = " + dt_gid + " AND SEQ_ID != " + seqid;
                    int ra = sqlUpdate(sqlstring);
                    if (ra < 0)
                    {
                        Message = "Unable to migrate items into new group.";
                        return false;
                    }
                }
            }

            return base.edit();
        }

        public bool applyUpdates(Hashtable ht)
        {
            //=======================================================================
            //RFC-33165 : Phase 2_IRSW Maker-Checker for Parameter Admin Function
            irsUsers irs_users = new irsUsers();
            irsInternetPage inetpage = new irsInternetPage();
            bool ok = false;

            string sessionUserId = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
            this.userid = sessionUserId;

            if (this.ParamApproverFlag() == true) {
                crud = ht["crud"].ToString().ToUpper();

                if (crud.Equals("ADD"))
                {
                    base.addMode = true;
                    this.setBase("SELECT TOP 1 * FROM TBL_MENU_TMP", true);
                    setColumn("Main_SEQ_ID", Convert.ToInt32(ht["SEQ_ID"]));
                    setColumn("MenuGroup", Convert.ToString(ht["MenuGroup"]));
                    setColumn("MenuItemNo", Convert.ToString(ht["MenuItemNo"]));
                    setColumn("MenuParentID", Convert.ToString(ht["MenuParentID"]));
                    setColumn("MenuName", Convert.ToString(ht["MenuName"]));
                    setColumn("ModuleId", Convert.ToString(ht["ModuleId"]));
                    setColumn("Maker", sessionUserId);
                    setColumn("Checker", "");
                    setColumn("DateAdded", DateTime.Now);
                    setColumn("CurrStatus", "For Approval");
                    setColumn("StatusDate", DateTime.Now);
                    setColumn("ActionType", ActionType.ADD.ToString());
                    setColumn("Module", "System Administration >> Menu Maintenance");
                    setColumn("MenuName_tmp", "Menu Table");

                    ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    if (ok)
                    {
                        try
                        {
                            string receiver = inetpage.getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipient", "FldDescription");
                            string receivercc = inetpage.getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientCC", "FldDescription");
                            string receiverbcc = inetpage.getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientBCC", "FldDescription");
                            string[] arrReceiver = receiver.Split(';');
                            string[] arrReceiverCC = receivercc.Split(';');
                            string[] arrReceiverBCC = receiverbcc.Split(';');
                            string subject = string.Empty;
                            string body = string.Empty;
                            List<string> recipient = new List<string>();
                            List<string> cc = new List<string>();
                            List<string> bcc = new List<string>();

                            foreach (var email in arrReceiver)
                            {
                                recipient.Add(email.ToString().Trim());
                            }

                            foreach (var emailcc in arrReceiverCC)
                            {
                                cc.Add(emailcc.ToString().Trim());
                            }

                            foreach (var emailbcc in arrReceiverBCC)
                            {
                                bcc.Add(emailbcc.ToString().Trim());
                            }

                            subject = inetpage.getLookupValue("ParamApproverEmail", "FldCode", "ParamSubject", "FldDescription").Replace("@Region", System.Configuration.ConfigurationManager.AppSettings["SMTPRegion"]);
                            body = inetpage.getLookupValue("ParamApproverEmail", "FldCode", "MenuParamBody", "FldDescription");

                            body = body.Replace("@ModuleName", "System Administration >> Menu Maintenance");
                            body = body.Replace("@MakerID", sessionUserId);
                            body = body.Replace("@ActionType", ActionType.ADD.ToString());
                            body = body.Replace("@MenuName", Convert.ToString(ht["MenuName"]));
                            body = body.Replace("@DateRequested", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            irs_users.SendEmail(subject, recipient, cc, bcc, body, null);
                        }
                        catch (Exception ex)
                        {
                            new csAuditLog().writeAudit("", "Menu Maintenance (ADD) Email Sending Failed", "Maker Checker Modules", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
                        }
                        ok = true;
                    }
                    else
                    {
                        new csAuditLog().writeAudit("", "Menu Maintenance (ADD) Error", "Maker Checker Modules", "", base.Message, csAuditLog.Category.Insert);
                        ok = false;
                    }
                }

                if (crud.Equals("EDIT"))
                {
                    base.addMode = true;
                    this.setBase("SELECT TOP 1 * FROM TBL_MENU_TMP", true);
                    setColumn("Main_SEQ_ID", Convert.ToInt32(ht["SEQ_ID"]));
                    setColumn("MenuGroup", Convert.ToString(ht["MenuGroup"]));
                    setColumn("MenuItemNo", Convert.ToString(ht["MenuItemNo"]));
                    setColumn("MenuParentID", Convert.ToString(ht["MenuParentID"]));
                    setColumn("MenuName", Convert.ToString(ht["MenuName"]));
                    setColumn("ModuleId", Convert.ToString(ht["ModuleId"]));
                    setColumn("Maker", sessionUserId);
                    setColumn("Checker", "");
                    setColumn("DateAdded", DateTime.Now);
                    setColumn("CurrStatus", "For Approval");
                    setColumn("StatusDate", DateTime.Now);
                    setColumn("ActionType", ActionType.EDIT.ToString());
                    setColumn("Module", "System Administration >> Menu Maintenance");
                    setColumn("MenuName_tmp", "Menu Table");

                    ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    if (ok)
                    {
                        try
                        {
                            string receiver = inetpage.getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipient", "FldDescription");
                            string receivercc = inetpage.getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientCC", "FldDescription");
                            string receiverbcc = inetpage.getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientBCC", "FldDescription");
                            string[] arrReceiver = receiver.Split(';');
                            string[] arrReceiverCC = receivercc.Split(';');
                            string[] arrReceiverBCC = receiverbcc.Split(';');
                            string subject = string.Empty;
                            string body = string.Empty;
                            List<string> recipient = new List<string>();
                            List<string> cc = new List<string>();
                            List<string> bcc = new List<string>();

                            foreach (var email in arrReceiver)
                            {
                                recipient.Add(email.ToString().Trim());
                            }

                            foreach (var emailcc in arrReceiverCC)
                            {
                                cc.Add(emailcc.ToString().Trim());
                            }

                            foreach (var emailbcc in arrReceiverBCC)
                            {
                                bcc.Add(emailbcc.ToString().Trim());
                            }

                            subject = inetpage.getLookupValue("ParamApproverEmail", "FldCode", "ParamSubject", "FldDescription").Replace("@Region", System.Configuration.ConfigurationManager.AppSettings["SMTPRegion"]);
                            body = inetpage.getLookupValue("ParamApproverEmail", "FldCode", "MenuParamBody", "FldDescription");

                            body = body.Replace("@ModuleName", "System Administration >> Menu Maintenance");
                            body = body.Replace("@MakerID", sessionUserId);
                            body = body.Replace("@ActionType", ActionType.EDIT.ToString());
                            body = body.Replace("@MenuName", Convert.ToString(ht["MenuName"]));
                            body = body.Replace("@DateRequested", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            irs_users.SendEmail(subject, recipient, cc, bcc, body, null);
                        }
                        catch (Exception ex)
                        {
                            new csAuditLog().writeAudit("", "Menu Maintenance (EDIT) Email Sending Failed", "Maker Checker Modules", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
                        }
                        ok = true;
                    }
                    else
                    {
                        new csAuditLog().writeAudit("", "Menu Maintenance (EDIT) Error", "Maker Checker Modules", "", base.Message, csAuditLog.Category.Insert);
                        ok = false;
                    }
                }

                if (crud.Equals("DELETE"))
                {
                    base.addMode = true;
                    this.setBase("SELECT TOP 1 * FROM TBL_MENU_TMP", true);
                    setColumn("Main_SEQ_ID", Convert.ToInt32(ht["SEQ_ID"]));
                    setColumn("MenuGroup", Convert.ToString(ht["MenuGroup"]));
                    setColumn("MenuItemNo", Convert.ToString(ht["MenuItemNo"]));
                    setColumn("MenuParentID", Convert.ToString(ht["MenuParentID"]));
                    setColumn("MenuName", Convert.ToString(ht["MenuName"]));
                    setColumn("ModuleId", Convert.ToString(ht["ModuleId"]));
                    setColumn("Maker", sessionUserId);
                    setColumn("Checker", "");
                    setColumn("DateAdded", DateTime.Now);
                    setColumn("CurrStatus", "For Approval");
                    setColumn("StatusDate", DateTime.Now);
                    setColumn("ActionType", ActionType.DELETE.ToString());
                    setColumn("Module", "System Administration >> Menu Maintenance");
                    setColumn("MenuName_tmp", "Menu Table");

                    ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    if (ok)
                    {
                        try
                        {
                            string receiver = inetpage.getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipient", "FldDescription");
                            string receivercc = inetpage.getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientCC", "FldDescription");
                            string receiverbcc = inetpage.getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientBCC", "FldDescription");
                            string[] arrReceiver = receiver.Split(';');
                            string[] arrReceiverCC = receivercc.Split(';');
                            string[] arrReceiverBCC = receiverbcc.Split(';');
                            string subject = string.Empty;
                            string body = string.Empty;
                            List<string> recipient = new List<string>();
                            List<string> cc = new List<string>();
                            List<string> bcc = new List<string>();

                            foreach (var email in arrReceiver)
                            {
                                recipient.Add(email.ToString().Trim());
                            }

                            foreach (var emailcc in arrReceiverCC)
                            {
                                cc.Add(emailcc.ToString().Trim());
                            }

                            foreach (var emailbcc in arrReceiverBCC)
                            {
                                bcc.Add(emailbcc.ToString().Trim());
                            }

                            subject = inetpage.getLookupValue("ParamApproverEmail", "FldCode", "ParamSubject", "FldDescription").Replace("@Region", System.Configuration.ConfigurationManager.AppSettings["SMTPRegion"]);
                            body = inetpage.getLookupValue("ParamApproverEmail", "FldCode", "MenuParamBody", "FldDescription");

                            body = body.Replace("@ModuleName", "System Administration >> Menu Maintenance");
                            body = body.Replace("@MakerID", sessionUserId);
                            body = body.Replace("@ActionType", ActionType.DELETE.ToString());
                            body = body.Replace("@MenuName", Convert.ToString(ht["MenuName"]));
                            body = body.Replace("@DateRequested", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            irs_users.SendEmail(subject, recipient, cc, bcc, body, null);
                        }
                        catch (Exception ex)
                        {
                            new csAuditLog().writeAudit("", "Menu Maintenance (DELETE) Email Sending Failed", "Maker Checker Modules", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
                        }
                        ok = true;
                    }
                    else
                    {
                        new csAuditLog().writeAudit("", "Menu Maintenance (DELETE) Error", "Maker Checker Modules", "", base.Message, csAuditLog.Category.Insert);
                        ok = false;
                    }
                }
            }
            //=========================================================
            else {
                int seqid = -1;
                crud = ht["crud"].ToString().ToUpper();

                try
                {
                    seqid = Convert.ToInt32(ht["SEQ_ID"]);
                }
                catch (Exception e)
                {
                    Message = e.Message;
                }

                if (seqid < 1 & !crud.Equals("ADD")) return false;
                if (crud.Equals("DELETE"))
                {
                    return delete_menu(ht, seqid);
                }

                //=======================================================================
                //RFC-33165 : Phase 2_IRSW Maker-Checker for Parameter Admin Function
                //bool ok = false;
                //=======================================================================

                getUpdateDS(seqid);
                selectedRow = 0;
                if (crud.Equals("EDIT"))
                {
                    ok = edit_menu(ht, seqid);
                }
                if (crud.Equals("ADD"))
                {
                    ok = base.newRow(sqlHelperWs.RETAIN_DATASET);
                }
                if (ok)
                {
                    foreach (Object key in ht.Keys)
                    {
                        if (key.ToString().ToUpper().Equals("MODULEURL")) continue;
                        if (key.ToString().Equals("SEQ_ID")) continue;
                        if (key.ToString().Equals("crud")) continue;
                        Object val = ht[key];
                        if (key.ToString().Equals("ModuleId"))
                        {
                            String s = val.ToString();
                            if (s.Trim().Equals("0")) val = null;
                        }
                        if (!setColumn(key.ToString(), val))
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok & addMode)
                    {
                        table.Rows.Add(uprow);
                        addMode = false;
                    }
                }
            }
            return ok;
        }

        public DataTable saveUpdates()
        {
            if (crud.Equals("DELETE")) return this.getMenuView(this.userid);
            if (base.save())
            {
                return this.getMenuView(this.userid);
            }

            return null;
        }
        
        public DataTable getMenuView(String userid)
        {
            if (userid == null & base.isProd)
            {
                Message = "Userid is required when not Devel run mode.";
                return null;
            }

            /* VAPT FINDINGS. ACCESS PRIVILAGE ESCALATION MARCH 2017 BOYET*/

            if (string.IsNullOrEmpty(userid) == true)
            {
                this.userid = null; Message = "Login is required."; return null;
            }

            if (ClientData == null || ClientData.UserId == null || ClientData.Location == null)
            {
                this.userid = null; Message = "Login is required."; return null;
            }

            if (string.IsNullOrEmpty(userid) == true)
            {
                this.userid = null; Message = "Login is required."; return null;
            }

            try
            {
                if (new irsUsers().user_exist("USERID", userid) == false)
                {
                    Message = "Login is required."; return null;
                }

                if (ClientData != null && string.IsNullOrEmpty(ClientData.UserId).Equals(false))
                {
                    if (ClientData.UserId.ToUpper().Trim() != userid.ToUpper().Trim())
                    {
                        this.userid = null; Message = "Login is required."; return null;
                    }
                }

            }
            catch
            {
            }
            /* VAPT FINDINGS. ACCESS PRIVILAGE ESCALATION */

            short role = -1;
            string brcode = "";
            if (userid != null)
            {
                try
                {
                    role = ClientData.Role;
                    brcode = ClientData.Location;
                    if (role < 0)
                    {
                        Message = "User must have associated role.";
                        return null;
                    }
                }
                catch (Exception e)
                {
                    Message = "Role: " + e.Message;
                    return null;
                }
            }

            this.userid = userid;
            //if (userid == null | role < 1) return getMenuView(); ORIGINAL CODE COMMENTED BY BOYET TO AVOID PRIVILAGE ESCALATION.
            return new irsModuleProfile().getMenuPermissionView(userid, brcode, role);
        }

        public int getLastGroupNo()
        {
            int gid = 0;
            try
            {
                Object o = execScalar("SELECT max(MenuGroup) FROM " + base.TableName + " WHERE MenuName != 'Logout'");
                if (o != DBNull.Value)
                {
                    gid = Convert.ToInt32(o);
                }
                return gid + 1;
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return -1;
        }

        public DataTable getMenuView()
        {
            if (base.isProd)
            {
                Message = "Userid is required when not Devel run mode.";
                return null;
            }

            DataSet ds = getDataSet("sp_getMenuView", true, new Object[] { });
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        public irsMenu()
        {
            TableName = "TBL_MENU"; //TBL_MENU
            IdentityColumn = "Seq_id";
        }

        //=======================================================================
        //RFC-33165 : Phase 2_IRSW Maker-Checker for Parameter Admin Function
        private void setBase(string SQL, bool IsAdd)
        {
            base.getUpdateDataSet(getSqlCommand(SQL, false));
            if (IsAdd)
            {
                base.newRow(sqlHelperWs.RETAIN_DATASET);
            }
            else
            {
                selectedRow = 0;
            }
        }

        public enum ActionType
        {
            ADD,
            EDIT,
            DELETE
        }

        public bool IsMenuActionAllowed(string actionType, string Menu_Name)
        {
            bool ret = false;
            try
            {
                base.EnforceConstraint = false;
                getUpdateDS("mc_MenuCheckMakerAllowed", new Object[] { Menu_Name, actionType });
                if (hasRows)
                {
                    selectedRow = 0;
                    if (Convert.ToInt32(table.Rows[0]["Strict"].ToString()) > 0)
                    {
                        ret = true;
                    }
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "IsMenuActionAllowed Error", "Validate Strict Menu Action", ex.Message, ex.StackTrace, csAuditLog.Category.Select);
            }
            return ret;
        }

        public bool ParamApproverFlag()
        {
            DataTable dtLookup = new DataTable();
            irsInternetPage inetpage = new irsInternetPage();

            try
            {
                dtLookup = inetpage.getLookup("ParamApproverFlag");

                if (dtLookup != null && dtLookup.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dtLookup.Rows[0]["FldCode"].ToString()) > 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                inetpage = null;
                dtLookup = null;
            }
            return false;
        }
        //=======================================================================
    }
}
