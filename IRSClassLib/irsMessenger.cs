using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace IRSClassLib
{
    [Serializable]
    public class irsMessages : sqlHelperWs
    {
        private bool has_deletes = false;

        private bool isDate(String sdt)
        {
            try
            {
                DateTime.Parse(sdt);
                return true;
            }
            catch { }
            return false;
        }

        public bool isin_prod
        {
            get { return isProd; }
        }

        public DataTable getMessageTypes()
        {
            return getLookup(new Object[] { "Message Types", DBNull.Value, -1 });
        }

        public DataTable getTargetTypes()
        {
            return new irsMessageGroup().getMemberTypes();
        }

        public DataTable getListTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ID", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Description", typeof(System.String)));

            return dt;
        }

        public String getBranchName(String brcode)
        {
            String sql = "SELECT BranchName FROM TBL_BRANCHES WHERE BranchCode = '" + brcode + "'";
            Object o = execScalar(sql);
            if (o != DBNull.Value)
            {
                return (String)o;
            }

            return String.Empty;
        }

        private DataTable listBranches()
        {
            try
            {
                DataSet ds = getDataSet(getSqlCommand("get_dropdown_branches"));
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = getListTable();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow uprow = dt.NewRow();
                        uprow["ID"] = row["BranchCode"];
                        uprow["Description"] = row["BranchName"];
                        dt.Rows.Add(uprow);
                    }
                    return dt;
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return null;
        }

        private DataTable listUsers()
        {
            try
            {
                DataTable dtusers = new irsUsers().listUsers();
                if (dtusers != null)
                {
                    DataTable dt = getListTable();
                    foreach (DataRow row in dtusers.Rows)
                    {
                        DataRow uprow = dt.NewRow();
                        uprow["ID"] = row["UserID"];
                        uprow["Description"] = row["FullName"];
                        dt.Rows.Add(uprow);
                    }
                    return dt;
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return null;        
        }

        private DataTable listGroups()
        {
            try
            {
                DataTable dtgroups = new irsMessageGroup().listGroups();
                if (dtgroups != null)
                {
                    DataTable dt = getListTable();
                    foreach (DataRow row in dtgroups.Rows)
                    {
                        DataRow uprow = dt.NewRow();
                        uprow["ID"] = row["MsgGroupName"];
                        uprow["Description"] = row["MsgGroupName"];
                        dt.Rows.Add(uprow);
                    }
                    return dt;
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return null;
        }

        public DataTable getTargetTypes(String tgcode)
        {
            try
            {
                switch (int.Parse(tgcode))
                {
                    case 2: return listUsers();
                    case 3: return listBranches();
                    case 4: return listGroups();

                    default: Message = "Target type: " + tgcode + " - is not implemented yet."; break;
                }
            }
            catch (Exception e)
            {
                Message = "@getTargetTypes: " + e.Message;
            }

            return null;
        }

        private String genOriginClause(String origin)
        {
            String orig_clause = "";

            if (origin != null)
            {
                DataTable dtgroup = new irsMessageGroup().listGroups(origin);

                if (dtgroup != null && dtgroup.Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("IN (");
                    bool coma = false;
                    foreach (DataRow row in dtgroup.Rows)
                    {
                        if (coma) sb.Append(",");
                        sb.Append("'" + row["MsgGroupName"].ToString() + "'");
                        coma = true;
                    }
                    sb.Append(")");
                    orig_clause += "(MsgTarget " + sb.ToString() + " OR MsgUserId " + sb.ToString() + ")";
                }

                if (!ClientData.isSystemAdmin)
                {
                    String st = "(";
                    if (orig_clause.Length > 0)
                    {
                        orig_clause = st + orig_clause + " AND ";
                    }
                    else
                    {
                        orig_clause = st;
                    }
                    orig_clause += "MsgOriginBranch = '" + ClientData.Location + "')";
                }
            }

            return orig_clause;
        }

        private String getDateClause(String dttype, String dtfrom, String dtto)
        {
            String dt_clause = "(MsgExpireDate is null OR MsgExpireDate > getdate())"; //>= convert(varchar, getdate(), 101))";
            if (dttype != null && dttype.Length > 0)
            {
                dt_clause += " AND convert(varchar, " + "Msg" + dttype + "Date, 101)";
            }
            else if ((dtfrom != null && dtfrom.Length > 0 && isDate(dtfrom)) | (dtto != null && dtto.Length > 0 && isDate(dtto)))
            {
                dt_clause += " AND convert(varchar, MsgPostingDate, 101)";
            }

            if (dt_clause != String.Empty)
            {
                if ((dtfrom == null || dtfrom.Length == 0) & (dtto != null && dtto.Length > 0))
                {
                    dt_clause += " <= '" + dtto.Trim() + "'";
                }
                else if ((dtto == null || dtto.Length == 0) & (dtfrom != null && dtfrom.Length > 0))
                {
                    dt_clause += " >= '" + dtfrom.Trim() + "'";
                }
                else if ((dtto != null && dtto.Length > 0) & (dtfrom != null && dtfrom.Length > 0))
                {
                    if (dtto.Trim().Equals(dtfrom.Trim()))
                    {
                        dt_clause += " <= '" + dtto + "'";
                    }
                    else
                    {
                        dt_clause = "(" + dt_clause + " between '" + dtfrom.Trim() + "' AND '" + dtto.Trim() + "')";
                    }
                }

            }

            return dt_clause;
        }

        private String getOrigClause(String origin)
        {
            String orig_clause = genOriginClause(origin);
            String brcode = new irsUsers().getUserLocation(origin);
            if (brcode == null) brcode = "";
            if (!origin.ToUpper().Equals("ALL"))
            {
                if (brcode != null || brcode.Length > 0)
                {
                    String s = genOriginClause(brcode);
                    if (s.Length > 0 && orig_clause.Length > 0) orig_clause += " OR ";
                    if (s.Length > 0) orig_clause += s;
                }

                String sall = genOriginClause("ALL");
                if (sall.Length > 0 && orig_clause.Length > 0) orig_clause += " OR ";
                if (sall.Length > 0) orig_clause += genOriginClause("ALL");
            }
            else
            {
                brcode = ClientData.Location;
                String userid = ClientData.UserId;
                if (userid == null) userid = "";

                if (userid.Length > 0)
                {
                    String suser = genOriginClause(userid);
                    if (suser.Length > 0 && orig_clause.Length > 0) orig_clause += " OR ";
                    if (suser.Length > 0) orig_clause += suser;

                    if (userid.Length > 0 && orig_clause.Length > 0) orig_clause += " OR ";
                    if (userid.Length > 0) orig_clause += "(MsgTarget = '" + userid + "' OR MsgUserId = '" + userid + "')";
                }

                if (brcode.Length > 0)
                {
                    String s = genOriginClause(brcode);
                    if (s.Length > 0 && orig_clause.Length > 0) orig_clause += " OR ";
                    if (s.Length > 0) orig_clause += s;
                }
            }

            if (origin.Length > 0 && orig_clause.Length > 0) orig_clause += " OR ";
            if (origin.Length > 0) orig_clause += "(MsgTarget = '" + origin + "' OR MsgUserId = '" + origin + "')";

            if (brcode.Length > 0 && orig_clause.Length > 0) orig_clause += " OR ";
            if (brcode.Length > 0) orig_clause += "(MsgTarget = '" + brcode + "' OR MsgUserId = '" + brcode + "')";

            return orig_clause;
        }

        private String curr_orig = "";
        private String curr_dttype = "";
        private String curr_dtfrom = "";
        private String curr_dtto = "";

        private bool same_message(String origin, String dttype, String dtfrom, String dtto)
        {
            String orig = (origin == null || origin.Length == 0) ? "" : origin;
            String type = (dttype == null || dttype.Length == 0) ? "" : dttype;
            String from = (dtfrom == null || dtfrom.Length == 0) ? "" : dtfrom;
            String to = (dtto == null || dtto.Length == 0) ? "" : dtto;

            return (isUpdatable & orig.Equals(curr_orig) & curr_dttype.Equals(type) & curr_dtfrom.Equals(dtfrom) & curr_dtto.Equals(dtto));
        }

        private DataTable clone_table()
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

        public DataTable getMessages(String origin, String dttype, String dtfrom, String dtto)
        {
            if (dataset.HasChanges() & same_message(origin, dttype, dtfrom, dtto))
            {
                if (has_deletes)
                {
                    return clone_table();
                }
                return base.table;
            }

            this.curr_orig = origin;
            this.curr_dttype = dttype;
            this.curr_dtfrom = dtfrom;
            this.curr_dtto = dtto;

            bool isAdmin = false;
            String userid = null;
            try
            {
                isAdmin = ClientData.isSystemAdmin;
                userid = ClientData.UserId;
            }
            catch (Exception e) 
            { 
                Message = e.Message;
                return null;
            }
            String orig_clause = "";

            String sql = "SELECT * FROM " + this.TableName;

            orig_clause = "(" + getOrigClause(origin) + ")"; 
            String dt_clause = getDateClause(dttype, dtfrom, dtto);

            //bool where_is = false;
            if (orig_clause != String.Empty | dt_clause != String.Empty)
            {
                sql += " WHERE ";
                //where_is = true;
                if (orig_clause != String.Empty)
                {
                    sql += orig_clause;
                    if (dt_clause != String.Empty)
                    {
                        sql += " AND ";
                    }
                }

                if (dt_clause != String.Empty)
                {
                    sql += dt_clause;
                }
            }

            base.getUpdateDS(sql);

            if (isUpdatable) return table;

            return null;
        }

        public bool applyUpdates(String crud, int row, Hashtable ht)
        {
            if (crud.Equals("DELETE"))
            {
                selectedRow = findRow("SEQ_ID", Convert.ToInt32(ht["SEQ_ID"]));
                bool deloks = base.Delete(sqlHelperWs.RETAIN_DATASET, false);
                if (deloks) has_deletes = true;
                return deloks;
            }
            bool ok = false;
            if (crud.Equals("EDIT"))
            {
                selectedRow = findRow("SEQ_ID", Convert.ToInt32(ht["SEQ_ID"]));
                ok = base.edit();
            }
            if (crud.Equals("ADD"))
            {
                ok = base.newRow(sqlHelperWs.RETAIN_DATASET);
            }
            if (ok)
            {
                foreach (Object key in ht.Keys)
                {
                    if (key.ToString().Equals(this.IdentityColumn)) continue;
                    Object val = ht[key];
                    if (key.ToString().Equals("MsgExpireDate") & val.ToString().Length == 0) val = null;
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
            return ok;
        }

        public bool applyUpdates_tmp(String crud, int row, Hashtable ht, string userID)
        {
            this.TableName = "TBL_MESSAGES_TMP";
            this.IdentityColumn = "SEQ_ID";
            string sqlstmt = "SELECT * FROM TBL_MESSAGES_TMP WHERE 1=0";
            base.getUpdateDS(sqlstmt);
            bool ok = false;
            ok = base.newRow(sqlHelperWs.RETAIN_DATASET);
            if (ok)
            {
                foreach (Object key in ht.Keys)
                {
                    if (key.ToString().Equals(this.IdentityColumn)) continue;
                    Object val = ht[key];
                    if (key.ToString().Equals("MsgExpireDate") & val.ToString().Length == 0) val = null;
                    if (!setColumn(key.ToString(), val))
                    {
                        ok = false;
                        break;
                    }
                }
                if (ok)
                {
                    string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    if (crud != "ADD")
                    {
                        uprow["MAIN_SEQ_ID"] = Convert.ToInt32(ht["SEQ_ID"]);
                    }
                    uprow["Maker"] = userID;
                    uprow["DateAdded"] = dateNow;
                    uprow["CurrStatus"] = "For Approval";
                    uprow["StatusDate"] = dateNow;
                    uprow["ActionType"] = crud;
                    uprow["Module"] = "Branch Administration >> Messages";
                    uprow["MenuName"] = "Messages";
                    table.Rows.Add(uprow);
                    addMode = false;
                }
            }
            
            return ok;
        }

        public DataTable regetMessages()
        {
            return this.getMessages(curr_orig, curr_dttype, curr_dtfrom, curr_dtto);
        }

        public DataTable saveUpdates()
        {
            if (base.save(sqlHelperWs.RETAIN_DATASET))
            {
                has_deletes = false;
                return regetMessages();
            }

            return null;
        }

        public DataTable getOrigins()
        {
            String sql = "SELECT DISTINCT MsgUserId as origin FROM " + base.TableName;
            DataSet ds = getDataSet(getSqlCommand(sql, false));
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        public irsMessages()
        {
            TableName = "TBL_MESSAGES";
            IdentityColumn = "SEQ_ID";
        }

        ~irsMessages()
        {
            curr_orig = null;
            curr_dttype = null;
            curr_dtfrom = null;
            curr_dtto = null;        
        }
    }

}
