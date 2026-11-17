using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace IRSClassLib
{
    [Serializable]
    public class irsModules : sqlHelperWs
    {
        private bool has_deletes = false;

        public void listDirectories(String s, ref short seqno, short parid, ArrayList al)
        {
			String[] ff = Directory.GetFileSystemEntries(s);
            foreach (String f in ff)
            {
                FileAttributes fa = File.GetAttributes(f);
                if (fa > 0)
                {
                    if (fa == FileAttributes.Directory)
                    {
                        seqno++;
                        al.Add(new Object[] { seqno, parid, Path.GetFileName(f), "D" });
                        listDirectories(f, ref seqno, seqno, al);
                    }
                    else 
                    {
                        String ext = Path.GetExtension(f);
                        if (ext != null && ext.Length > 0)
                        {
                            ext = ext.ToUpper();
                            if (ext.Equals(".ASPX") | ext.Equals(".JS"))
                            {
                                seqno++;
                                al.Add(new Object[] { seqno, parid, Path.GetFileName(f), "F" });
                            }
                        }
                    }
                }
            }
        }

        public Object[] getFs(String root)
        {
            short seqno = 0;
            short parid = -1;
            String item = "IRSWorld";
            ArrayList al = new ArrayList();
            al.Add(new Object[] { seqno, parid, item, "R" });

            listDirectories(root, ref seqno, seqno, al);

            if (al.Count > 0)
            {
                Object[] lsdir = new Object[al.Count];
                al.CopyTo(lsdir);

                return lsdir;
            }

            return null;
        }

        public DataTable getModules()
        {
            if (this.dataset.HasChanges() || this.isUpdating)
            {
                if (has_deletes)
                {
                    DataTable dtClone = this.table.Clone();
                    foreach (DataRow row in table.Rows)
                    {
                        if (row.RowState == DataRowState.Deleted) continue;
                        DataRow row_clone = dtClone.NewRow();
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            row_clone[i] = row[i];
                        }
                        dtClone.Rows.Add(row_clone);
                    }
                    return dtClone;
                }
                return this.table;
            }

            //base.getUpdateDataSet(getSqlCommand("SELECT * FROM " + this.TableName + " ORDER BY ModuleName", false), new String[] { this.TableName });
            base.getUpdateDataSet("SELECT * FROM " + this.TableName + " ORDER BY ModuleName", false);
            if (isUpdatable) return this.table;

            return null;
        }

        private int find_row(Hashtable ht)
        {
            Object oid = ht["SEQ_ID"];
            if (oid != null && (oid.ToString() == String.Empty | oid.ToString().Equals("0")))
            {
                String mname = ht["ModuleName"].ToString();
                String murl = ht["ModuleUrl"].ToString();
                String mtgt = ht["ModuleTarget"].ToString();

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i].RowState == DataRowState.Deleted) continue;
                    String tbl_mname = table.Rows[i]["ModuleName"].ToString();
                    String tbl_murl = table.Rows[i]["ModuleUrl"].ToString();
                    String tbl_mtgt = table.Rows[i]["ModuleTarget"].ToString();

                    if (tbl_mname == mname & tbl_murl == murl & tbl_mtgt == mtgt) return i;
                }

                return -1;

            }

            long k = Convert.ToInt64(oid);

            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i].RowState == DataRowState.Deleted) continue;
                Object o = table.Rows[i]["SEQ_ID"];
                long j = Convert.ToInt64(o);
                if (j == k) return i;
            }

            return -1;
        }

        public bool applyUpdates(String crud, int row, Hashtable ht)
        {
            //=======================================================================
            //RFC-33165 : Phase 2_IRSW Maker-Checker for Parameter Admin Function
            irsUsers irs_users = new irsUsers();
            irsInternetPage inetpage = new irsInternetPage();
            bool ok = false;

            string sessionUserId = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;

            if (this.ParamApproverFlag() == true) {
                if (crud.Equals("ADD")) {
                    base.addMode = true;
                    this.setBase("SELECT TOP 1 * FROM TBL_MODULES_TMP", true);
                    setColumn("Main_SEQ_ID", Convert.ToInt32(ht["SEQ_ID"]));
                    setColumn("ModuleName", Convert.ToString(ht["ModuleName"]));
                    setColumn("ModuleUrl", Convert.ToString(ht["ModuleUrl"]));
                    setColumn("ModuleTarget", Convert.ToString(ht["ModuleTarget"]));
                    setColumn("Maker", sessionUserId);
                    setColumn("Checker", "");
                    setColumn("DateAdded", DateTime.Now);
                    setColumn("CurrStatus", "For Approval");
                    setColumn("StatusDate", DateTime.Now);
                    setColumn("ActionType", ActionType.ADD.ToString());
                    setColumn("Module", "System Administration >> Modules Entry");
                    setColumn("MenuName", "Modules Table");

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
                            body = inetpage.getLookupValue("ParamApproverEmail", "FldCode", "ModulesParamBody", "FldDescription");

                            body = body.Replace("@ModuleName", "System Administration >> Modules Entry");
                            body = body.Replace("@MakerID", sessionUserId);
                            body = body.Replace("@ActionType", ActionType.ADD.ToString());
                            body = body.Replace("@Module_Name", Convert.ToString(ht["ModuleName"]));
                            body = body.Replace("@ModuleUrl", Convert.ToString(ht["ModuleUrl"]));
                            body = body.Replace("@DateRequested", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            irs_users.SendEmail(subject, recipient, cc, bcc, body, null);
                        }
                        catch (Exception ex)
                        {
                            new csAuditLog().writeAudit("", "Modules Entry (ADD) Email Sending Failed", "Maker Checker Modules", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
                        }
                        ok = true;
                    }
                    else
                    {
                        new csAuditLog().writeAudit("", "Modules Entry (ADD) Error", "Maker Checker Modules", "", base.Message, csAuditLog.Category.Insert);
                        ok = false;
                    }
                }

                if (crud.Equals("EDIT")) {
                    base.addMode = true;
                    this.setBase("SELECT TOP 1 * FROM TBL_MODULES_TMP", true);
                    setColumn("Main_SEQ_ID", Convert.ToInt32(ht["SEQ_ID"]));
                    setColumn("ModuleName", Convert.ToString(ht["ModuleName"]));
                    setColumn("ModuleUrl", Convert.ToString(ht["ModuleUrl"]));
                    setColumn("ModuleTarget", Convert.ToString(ht["ModuleTarget"]));
                    setColumn("Maker", sessionUserId);
                    setColumn("Checker", "");
                    setColumn("DateAdded", DateTime.Now);
                    setColumn("CurrStatus", "For Approval");
                    setColumn("StatusDate", DateTime.Now);
                    setColumn("ActionType", ActionType.EDIT.ToString());
                    setColumn("Module", "System Administration >> Modules Entry");
                    setColumn("MenuName", "Modules Table");

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
                            body = inetpage.getLookupValue("ParamApproverEmail", "FldCode", "ModulesParamBody", "FldDescription");

                            body = body.Replace("@ModuleName", "System Administration >> Modules Entry");
                            body = body.Replace("@MakerID", sessionUserId);
                            body = body.Replace("@ActionType", ActionType.EDIT.ToString());
                            body = body.Replace("@Module_Name", Convert.ToString(ht["ModuleName"]));
                            body = body.Replace("@ModuleUrl", Convert.ToString(ht["ModuleUrl"]));
                            body = body.Replace("@DateRequested", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            irs_users.SendEmail(subject, recipient, cc, bcc, body, null);
                        }
                        catch (Exception ex)
                        {
                            new csAuditLog().writeAudit("", "Modules Entry (EDIT) Email Sending Failed", "Maker Checker Modules", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
                        }
                        ok = true;
                    }
                    else
                    {
                        new csAuditLog().writeAudit("", "Modules Entry (EDIT) Error", "Maker Checker Modules", "", base.Message, csAuditLog.Category.Insert);
                        ok = false;
                    }
                }

                if (crud.Equals("DELETE")) {
                    base.addMode = true;
                    this.setBase("SELECT TOP 1 * FROM TBL_MODULES_TMP", true);
                    setColumn("Main_SEQ_ID", Convert.ToInt32(ht["SEQ_ID"]));
                    setColumn("ModuleName", Convert.ToString(ht["ModuleName"]));
                    setColumn("ModuleUrl", Convert.ToString(ht["ModuleUrl"]));
                    setColumn("ModuleTarget", Convert.ToString(ht["ModuleTarget"]));
                    setColumn("Maker", sessionUserId);
                    setColumn("Checker", "");
                    setColumn("DateAdded", DateTime.Now);
                    setColumn("CurrStatus", "For Approval");
                    setColumn("StatusDate", DateTime.Now);
                    setColumn("ActionType", ActionType.DELETE.ToString());
                    setColumn("Module", "System Administration >> Modules Entry");
                    setColumn("MenuName", "Modules Table");

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
                            body = inetpage.getLookupValue("ParamApproverEmail", "FldCode", "ModulesParamBody", "FldDescription");

                            body = body.Replace("@ModuleName", "System Administration >> Modules Entry");
                            body = body.Replace("@MakerID", sessionUserId);
                            body = body.Replace("@ActionType", ActionType.DELETE.ToString());
                            body = body.Replace("@Module_Name", Convert.ToString(ht["ModuleName"]));
                            body = body.Replace("@ModuleUrl", Convert.ToString(ht["ModuleUrl"]));
                            body = body.Replace("@DateRequested", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            irs_users.SendEmail(subject, recipient, cc, bcc, body, null);
                        }
                        catch (Exception ex)
                        {
                            new csAuditLog().writeAudit("", "Modules Entry (DELETE) Email Sending Failed", "Maker Checker Modules", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
                        }
                        ok = true;
                    }
                    else
                    {
                        new csAuditLog().writeAudit("", "Modules Entry (DELETE) Error", "Maker Checker Modules", "", base.Message, csAuditLog.Category.Insert);
                        ok = false;
                    }
                }
            }
            //=======================================================================
            else
            {
                if (crud.Equals("DELETE"))
                {
                    selectedRow = find_row(ht);
                    if (selectedRow > -1)
                    {
                        bool dels = base.Delete(sqlHelperWs.RETAIN_DATASET, false);
                        if (dels)
                        {
                            has_deletes = true;
                        }
                        return dels;
                    }

                    return false;
                }

                //=======================================================================
                //RFC-33165 : Phase 2_IRSW Maker-Checker for Parameter Admin Function
                //bool ok = false;
                //=======================================================================

                if (crud.Equals("EDIT"))
                {
                    selectedRow = find_row(ht);

                    if (selectedRow > -1)
                    {
                        ok = base.edit();
                    }
                    else
                    {
                        ok = false;
                    }
                }
                if (crud.Equals("ADD"))
                {
                    ok = base.newRow(sqlHelperWs.RETAIN_DATASET);
                }
                if (ok)
                {
                    foreach (Object key in ht.Keys)
                    {
                        if (key.ToString().Equals("SEQ_ID")) continue;
                        if (!setColumn(key.ToString(), ht[key]))
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

        public bool saveUpdates()
        {
            bool ok = base.save(sqlHelperWs.RETAIN_DATASET);
            if (ok) has_deletes = false;

            return ok;
        }

        public irsModules()
        {
            this.TableName = "TBL_MODULES";
            this.IdentityColumn = "SEQ_ID";
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
