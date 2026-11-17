using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace IRSClassLib
{
    [Serializable]
    public class irsMessageGroupMembers : sqlHelperWs
    {
        private int curr_gid = -99;

        public void getGroupMembers(int gid)
        {
            if (this.isUpdatable && this.table.Rows.Count > 0 && Convert.ToInt32(table.Rows[0]["MsgGroupId"]) == gid)
            {
                return;
            }
            //if (this.curr_gid == gid) return;
            //this.curr_gid = gid;
            String sql = "SELECT * FROM " + TableName + " WHERE MsgGroupId = " + gid;
            base.getUpdateDS(sql);

            this.curr_gid = gid;
        }

        internal bool deleteMembers(int gid)
        {
            getGroupMembers(gid);
            if (isUpdatable)
            {
                if (table.Rows.Count > 0)
                {
                    ArrayList errs = new ArrayList();
                    for (selectedRow = 0; selectedRow < table.Rows.Count; selectedRow++)
                    {
                        if (! base.Delete(sqlHelperWs.RETAIN_DATASET, false))
                        {
                            errs.Add(Message);
                        }
                    }
                    if (errs.Count == 0)
                    {
                        return base.save(sqlHelperWs.RETAIN_DATASET);
                    }
                    else
                    {
                        Message = "";
                        foreach (Object o in errs)
                        {
                            Message += o.ToString() + "\n";
                        }
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private String getUserName(String userid)
        {
            DataTable dtuser = new irsUsers().search(1, userid, String.Empty, 0, 0, 0);
            if (dtuser != null && dtuser.Rows.Count > 0)
            {
                return dtuser.Rows[0]["FullName"].ToString();
            }

            return "Not specified";
        }

        private String getBranchName(String brcode)
        {
            SqlCommand cmd = getSqlCommand("SELECT BranchName FROM TBL_BRANCHES WHERE BranchCode = '" + brcode + "'", false);
            DataSet ds = getDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["BranchName"].ToString();
            }

            return "Not specified";
        }

        public DataTable getGroupMembers()
        {
            if (isUpdatable)
            {
                DataTable dt = this.table.Clone();
                dt.Columns.Add(new DataColumn("MsgMemberName", typeof(System.String)));

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow row = dt.NewRow();
                    row["SEQ_ID"] = table.Rows[i]["SEQ_ID"];
                    row["MsgGroupId"] = table.Rows[i]["MsgGroupId"];
                    row["MsgGroupType"] = table.Rows[i]["MsgGroupType"];
                    row["MsgGroupMemberId"] = table.Rows[i]["MsgGroupMemberId"];
                    int mtype = Convert.ToInt32(row["MsgGroupType"]);
                    switch (mtype)
                    {
                        case 1: row["MsgMemberName"] = "All Users"; break;
                        case 2: row["MsgMemberName"] = getUserName(row["MsgGroupMemberId"].ToString()); break;
                        case 3: row["MsgMemberName"] = getBranchName(row["MsgGroupMemberId"].ToString()); break;
                    }

                    dt.Rows.Add(row);
                }
                return dt;
            }

            Message = "Get group members with id first.";
            return null;
        }

        public bool applyUpdates(String crud, int row, Hashtable ht)
        {
            bool ok = false;

            if (crud.Equals("DELETE"))
            {
                selectedRow = findRow("SEQ_ID", Convert.ToInt32(ht["SEQ_ID"])); ;
                return base.Delete(sqlHelperWs.RETAIN_DATASET, false);
            }
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
            return ok;
        }

        public DataTable saveUpdates(out String errmsg)
        {
            errmsg = "";
            if (base.save(sqlHelperWs.RETAIN_DATASET))
            {
                this.getGroupMembers(this.curr_gid);
                if (isUpdatable)
                {
                    return getGroupMembers();
                }
            }

            errmsg = Message;

            return null;
        }

        public irsMessageGroupMembers()
        {
            this.TableName = "TBL_MESSAGE_GROUP_MEMBERS";
            this.IdentityColumn = "SEQ_ID";
        }
    }

    [Serializable]
    public class irsMessageGroup : sqlHelperWs
    {
        private irsMessageGroupMembers mems;
        private String curr_group_brcode;

        public DataTable getBranches()
        {
            try
            {
                DataSet ds = null;
                if (ClientData.isSystemAdmin)
                {
                    ds = getDataSet(getSqlCommand("get_dropdown_branches"));
                }
                else
                {
                    if (ClientData.Location != null)
                    {
                        SqlCommand cmd = getSqlCommand("SELECT BranchCode, BranchName FROM TBL_BRANCHES WHERE BranchCode = '" + ClientData.Location + "'", false);
                        ds = getDataSet(cmd);
                    }
                    else
                    {
                        Message = "The branch to be search cannot be determined.";
                        return null;
                    }
                }
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            catch { }

            return null;
        }

        public DataTable getUsers()
        {
            irsUsers iu = new irsUsers();
            DataTable dtusers = iu.listUsers();
            if (dtusers != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("UserID", typeof(System.String)));
                dt.Columns.Add(new DataColumn("FullName", typeof(System.String)));
                foreach (DataRow row in dtusers.Rows)
                {
                    DataRow uprow = dt.NewRow();
                    uprow["UserID"] = row["UserID"];
                    uprow["FullName"] = row["FullName"];
                    dt.Rows.Add(uprow);
                }

                return dt;
            }

            Message = iu.Message;

            return null;
        }

        public DataTable getMemberTypes()
        {
            return getLookup(new Object[] { "MessageTargetType", DBNull.Value, -1 });
        }

        public DataTable getMessageGroups(String brcode)
        {
            if (!brcode.Equals("ALL"))
            {
                this.curr_group_brcode = brcode;
            }
            String sql = "SELECT * FROM " + this.TableName;
            if (!brcode.Equals("ALL"))
            {
                sql += " WHERE MsgGroupBranch = '" + brcode + "'";
            }
            if (brcode.Equals("ALL"))
            {
                sql += " WHERE MsgGroupName is not null ORDER by MsgGroupBranch";
            }
            if (brcode.Equals("ALL"))
            {
                DataSet ds = getDataSet(getSqlCommand(sql, false));
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            else
            {
                base.getUpdateDS(sql);
                if (isUpdatable)
                {
                    return base.table;
                }
            }

            return null;
        }

        internal DataTable listGroups()
        {
            try
            {
                bool ok = false;
                String sql = "SELECT * FROM " + this.TableName;
                if (ClientData.UserId != null && ClientData.UserId.Length > 0)
                {
                    if (ClientData.isSystemAdmin) ok = true;
                    else //if (ClientData.isBranchAdmin)
                    {
                        sql += " WHERE MsgGroupBranch = '" + ClientData.Location + "'";
                        ok = true;
                    }
                }
                else if (!isProd)
                {
                    ok = true;
                }

                if (ok)
                {
                    DataSet ds = getDataSet(getSqlCommand(sql, false));
                    if (ds != null && ds.Tables.Count > 0) return ds.Tables[0];
                }
                else
                {
                    Message = "This function requires certain capabilities.";
                }
            }
            catch (Exception e)
            {
                Message = "@listGroups: " + e.Message;
            }

            return null;
        }

        public DataTable listGroups(String membername)
        {
            String sql =
                "SELECT a.SEQ_ID, MsgGroupName FROM TBL_MESSAGE_GROUP a " +
                "INNER JOIN TBL_MESSAGE_GROUP_MEMBERS b " +
                "ON a.SEQ_ID = b.MsgGroupId " +
                "WHERE MsgGroupMemberId = '" + membername + "'";
            if (ClientData.UserId != null && ClientData.isSystemAdmin == false)
            {
                sql += " AND a.MsgGroupBranch = '" + ClientData.Location + "'";
            }
            else if (isProd && ClientData.UserId == null)
            {
                Message = "This function requires login.";
                return null;
            }

            DataSet ds = getDataSet(getSqlCommand(sql, false));
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        public DataTable getGroupMembers(int groupid, out String errmsg)
        {
            errmsg = "";
            irsMessageGroupMembers mems = new irsMessageGroupMembers();
            mems.getGroupMembers(groupid);
            if (mems.isUpdatable)
            {
                return mems.getGroupMembers();
            }
            errmsg = mems.Message;
            return null;
        }

        private bool applyDelete(String crud, int row, Hashtable ht)
        {
            int iseqid = Convert.ToInt32(ht["SEQ_ID"]);
            selectedRow = findRow("SEQ_ID", iseqid);
            bool ok = false;
            if (selectedRow > -1)
            {
                irsMessageGroupMembers detail = new irsMessageGroupMembers();
                ok = detail.deleteMembers(iseqid);

                if (ok)
                {
                    ok = base.Delete(sqlHelperWs.RETAIN_DATASET);
                }
            }
            else
            {
                Message = "Unable to find row for seqid: " + iseqid;
            }

            return ok;
        }

        public bool applyUpdates(String crud, int row, Hashtable ht)
        {
            bool ok = false;
            if (crud.Equals("DELETE"))
            {
                return applyDelete(crud, row, ht);
            }
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
            return ok;
        }

        public bool applyUpdates(String type, String crud, int row, Hashtable ht)
        {
            if (type.Equals("Groups"))
            {
                return applyUpdates(crud, row, ht);
            }
            else if (type.Equals("Members"))
            {
                if (mems == null)
                {
                    mems = new irsMessageGroupMembers();
                }
                mems.getGroupMembers(Convert.ToInt32(ht["MsgGroupId"]));
                return mems.applyUpdates(crud, row, ht);
            }

            Message = "Unknown type: " + type;

            return false;
        }

        public DataTable saveUpdates(String type, out String errmsg)
        {
            errmsg = "";
            if (type.Equals("Groups"))
            {
                if (base.save(sqlHelperWs.RETAIN_DATASET))
                {
                    DataTable dtm = this.getMessageGroups(curr_group_brcode); //table.Rows[0]["MsgGroupBranch"].ToString());
                    if (dtm != null)
                    {
                        return dtm;
                    }
                }
            }
            else if (type.Equals("Members"))
            {
                if (mems == null || !mems.isUpdatable)
                {
                    Message = "Updates have not been applied";
                }
                else
                {
                    return mems.saveUpdates(out errmsg);
                }
            }
            else
            {
                Message = "Unknown type: " + type;
            }

            errmsg = Message;

            return null;
        }

        public irsMessageGroup()
        {
            this.TableName = "TBL_MESSAGE_GROUP";
            this.IdentityColumn = "SEQ_ID";
        }
    } 

}
