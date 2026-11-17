using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsChargesUsers : sqlHelperWs
    {
        internal int curr_charge_id;
        internal short curr_usertype;

        public bool isOverrider(int ChargeId, short role)
        {
            Object o = execScalar("SELECT COUNT(*) FROM " + TableName + " WHERE ProductChargesId = " + ChargeId + " AND UserType = 2 AND Role = " + role);
            if (o != DBNull.Value)
            {
                try
                {
                    int count = (int)o;
                    return (count > 0);
                }
                catch (Exception e)
                {
                    Message = e.Message;
                }
            }
            return false;
        }

        public DataTable saveUpdates()
        {
            bool ok = true;
            if (dataset.HasChanges())
            { 
                ok = base.save(sqlHelperWs.RETAIN_DATASET);
            }
            if (ok)
            {
                return this.getUserPermissions(this.curr_charge_id);
            }

            return null;
        }

        public bool applyUpdates(String crud, Hashtable ht)
        {
            // one thing to remember here is that the table contains roles that are allowed.
            bool ok = false;
            if (crud.Equals("EDIT"))
            {
                int seqid = -1;
                bool allowed = false;
                try 
                { 
                    seqid = Convert.ToInt32(ht["SEQ_ID"]);
                    allowed = Convert.ToBoolean(ht["Allowed"]);
                }
                catch { }

                if (allowed)
                {
                    if (seqid < 1)
                    {
                        // this must be add operation.
                        if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                        {
                            ok = true;
                            foreach (Object key in ht.Keys)
                            {
                                if (key.ToString().Equals(this.IdentityColumn)) continue;
                                if (key.ToString().Equals("RoleDesc") | key.ToString().Equals("Allowed")) continue;
                                if (!setColumn(key.ToString(), ht[key]))
                                {
                                    ok = false;
                                    break;
                                }
                            }
                            if (ok)
                            {
                                table.Rows.Add(uprow);
                                addMode = false;
                            }
                        }
                    }
                    else
                    {
                        // else should never happen.
                        ok = true;
                    }
                }
                else
                {
                    if (seqid > 0)
                    {
                        // here must be delete operation
                        selectedRow = findRow("SEQ_ID", seqid);
                        if (selectedRow > -1)
                        {
                            ok = Delete(sqlHelperWs.RETAIN_DATASET);
                        }
                    }
                    else
                    {
                        ok = true;
                    }
                }
            }
            return ok;
        }

        public DataTable getUserRoles()
        {
            String asdec = (curr_usertype == 1) ? "ASC" : "DESC";
            String sql = "SELECT * FROM TBL_LOOKUP WHERE FldGroup = 'User Roles' ORDER BY fldValue " + asdec;
            DataSet ds = getDataSet(getSqlCommand(sql, false));
            if (ds != null)
            {
                return ds.Tables[0];
            }

            return null;
        }

        public override void getUpdateDS(int id)
        {
            String sqlstm = "SELECT * FROM " + TableName +
                            " WHERE ProductChargesID = " + id + " AND UserType = " + curr_usertype;
            base.getUpdateDS(sqlstm);
        }

        private DataTable getUsersView()
        {
            DataTable dt = new DataTable((curr_usertype == 1) ? "Users" : "Overrides");
            dt.Columns.Add(new DataColumn("SEQ_ID", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("ProductChargesID", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("UserType", typeof(System.Int16)));
            dt.Columns.Add(new DataColumn("Role", typeof(System.Int16)));
            dt.Columns.Add(new DataColumn("RoleDesc", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Allowed", typeof(System.Boolean)));

            return dt;
        }

        private DataTable generate_users()
        {
            DataTable dt = getUsersView();
            try
            {
                DataTable dtroles = getUserRoles();
                if (dtroles != null && dtroles.Rows.Count > 0)
                {
                    foreach (DataRow rrow in dtroles.Rows)
                    {
                        DataRow uprow = dt.NewRow();
                        uprow["SEQ_ID"] = 0;
                        uprow["ProductChargesID"] = curr_charge_id;
                        uprow["UserType"] = curr_usertype;
                        uprow["Role"] = rrow["fldValue"];
                        uprow["RoleDesc"] = rrow["fldDescription"];
                        uprow["Allowed"] = false;

                        DataRow[] urows = table.Select("Role = " + rrow["fldValue"].ToString());
                        if (urows.Length > 0)
                        {
                            uprow["SEQ_ID"] = urows[0]["SEQ_ID"];
                            uprow["Allowed"] = true;
                        }

                        dt.Rows.Add(uprow);
                    }

                    return dt;
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            Message = "Failed to get user roles: " + Message;
            return null;
        }

        public DataTable getUserPermissions(int charge_id)
        {
            this.curr_charge_id = charge_id;
            this.getUpdateDS(curr_charge_id);
            if (isUpdatable)
            {
                return generate_users();
            }
            Message = "Failed to get Charges users. " + Message;
            return null;
        }

        public irsChargesUsers(short utype)
        {
            base.TableName = "TBL_CHARGES_USERS";
            base.IdentityColumn = "SEQ_ID";

            curr_usertype = utype;
        }
    }

    [Serializable]
    public class irsChargesTier : sqlHelperWs
    {

        internal int curr_charge_id;

        public override void getUpdateDS(int id)
        {
            String sqlstm = "SELECT * FROM " + TableName + " WHERE ChargesID = " + id + " AND Deleted = 0";
            curr_charge_id = id;
            base.getUpdateDS(sqlstm);
        }

        private bool markDeleted(int row)
        {
            if (row > -1)
            {
                selectedRow = row;
                if (base.edit())
                {
                    if (setColumn("Deleted", true))
                    {
                        return setColumn("LastUpdate", DateTime.Today);
                    }
                }
            }
            else
            {
                Message = "Unable to find the row to delete.";
            }

            return false;
        }

        public bool deleteTiers(int chid)
        {
            if (!isUpdatable)
            {
                this.getUpdateDS(chid);
            }
            if (isUpdatable && table.Rows.Count > 0)
            {
                int ecount = 0;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (!markDeleted(i)) ecount++;
                }
                if (ecount == 0)
                {
                    return save(sqlHelperWs.RETAIN_DATASET);
                }

                return false;
            }

            return true;
        }

        private Hashtable keep_old_values()
        {
            Hashtable old_tier_values = new Hashtable();
            old_tier_values.Add("ChargesID", table.Rows[selectedRow]["ChargesID"]);
            old_tier_values.Add("AmountFrom", table.Rows[selectedRow]["AmountFrom"]);
            old_tier_values.Add("AmountTo", table.Rows[selectedRow]["AmountTo"]);
            old_tier_values.Add("ChargeAmount", table.Rows[selectedRow]["ChargeAmount"]);

            return old_tier_values;
        }

        public bool applyUpdates(String crud, Hashtable ht)
        {
            bool ok = false;
            Hashtable old_values = null; 

            if (crud.Equals("DELETE"))
            {
                if (markDeleted(findRow("SEQ_ID", Convert.ToInt32(ht["SEQ_ID"]))))
                {
                    return save(sqlHelperWs.RETAIN_DATASET);
                }
                return false;
            }
            if (crud.Equals("EDIT"))
            {
                selectedRow = findRow("SEQ_ID", Convert.ToInt32(ht["SEQ_ID"]));
                if (selectedRow > -1)
                {
                    old_values = keep_old_values();
                    ok = base.edit();
                }
                else
                {
                    Message = "Unable to locate the row to edit";
                }
            }
            if (crud.Equals("ADD"))
            {
                ok = base.newRow(sqlHelperWs.RETAIN_DATASET);
                if (ok)
                {
                    uprow["ChargesID"] = this.curr_charge_id;
                }
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
                double dfrom = Convert.ToDouble((addMode) ? uprow["AmountFrom"] : getColumn("AmountFrom"));
                double dto = Convert.ToDouble((addMode) ? uprow["AmountTo"] : getColumn("AmountTo"));
                if (dto > 0 && dto <= dfrom)
                {
                    Message = "From value must be less than to value.";
                    ok = false;
                }
                else
                {
                    setColumn("LastUpdate", DateTime.Today);
                    if (ok & addMode)
                    {
                        uprow["Deleted"] = false;
                        table.Rows.Add(uprow);
                        addMode = false;
                    }
                }
            }
            if (ok & crud.Equals("EDIT"))
            {
                ht.Add("OLD_VALUES", old_values);
            }
            return ok;
        }

        public DataTable saveUpdates()
        {
            if (base.save(sqlHelperWs.RETAIN_DATASET))
            {
                selectedRow = 0;
                this.curr_charge_id = Convert.ToInt32(getColumn("ChargesID"));
                getUpdateDS(this.curr_charge_id);
                if (isUpdatable) return table;
            }

            return null;
        }

        public irsChargesTier()
        {
            TableName = "TBL_CHARGES_TIER";
            IdentityColumn = "SEQ_ID";
        }
    }

    [Serializable]
    public class irsChargeDetail : sqlHelperWs
    {
        internal int curr_prod_id;
        private short[] ctype_tiers;
        private ArrayList del_tiers;

        public override void getUpdateDS(int id)
        {
            String sqlstm = "SELECT * FROM " + TableName + " WHERE ProductChargesID = " + id + " AND Deleted = 0";
            curr_prod_id = id;
            base.getUpdateDS(sqlstm);
        }

        private bool markDeleted(int row, irsChargesTier tier)
        {
            selectedRow = row;
            try
            {
                if (base.edit())
                {
                    int chid = (int)getColumn("SEQ_ID");
                    if (tier.deleteTiers(chid))
                    {
                        if (setColumn("Deleted", true))
                        {
                            return setColumn("LastUpdate", DateTime.Today);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return false;
        }

        public bool deleteDetails(irsChargesTier tier)
        {
            if (isUpdatable && table.Rows.Count > 0)
            {
                int ecount = 0;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (!markDeleted(i, tier)) ecount++;
                }
                if (ecount == 0)
                {
                    return save(sqlHelperWs.RETAIN_DATASET);
                }

                return false;
            }

            return true;
        }

        private Hashtable keep_old_values()
        {
            Hashtable old_values = new Hashtable();
            old_values.Add("ProductChargesID", table.Rows[selectedRow]["ProductChargesID"]);
            old_values.Add("ChargeName", table.Rows[selectedRow]["ChargeName"]);
            old_values.Add("ComputationType", table.Rows[selectedRow]["ComputationType"]);
            old_values.Add("ChargeValue", table.Rows[selectedRow]["ChargeValue"]);
            old_values.Add("MinimumAmount", table.Rows[selectedRow]["MinimumAmount"]);
            old_values.Add("Denominator", table.Rows[selectedRow]["Denominator"]);
            old_values.Add("ChargeToBenef", table.Rows[selectedRow]["ChargeToBenef"]);
            old_values.Add("ChargeLocally", table.Rows[selectedRow]["ChargeLocally"]);

            return old_values;
        }

        private bool isTier(short ctype)
        {
            try
            {
                if (ctype_tiers.Length > 0)
                {
                    for (int i = 0; i < ctype_tiers.Length; i++)
                    {
                        if (ctype == ctype_tiers[i])
                        {
                            return true;
                        }
                    }
                }
            }
            catch { }
            return false;
        }

        public bool applyUpdates(String crud, Hashtable ht, irsChargesTier tier)
        {
            bool ok = false;
            Hashtable old_values = null;

            if (crud.Equals("DELETE"))
            {

                if (markDeleted(findRow("SEQ_ID", Convert.ToInt32(ht["SEQ_ID"])), tier))
                {
                    return save(RETAIN_DATASET);
                }

                return false;
            }
            if (crud.Equals("EDIT"))
            {
                selectedRow = findRow("SEQ_ID", Convert.ToInt32(ht["SEQ_ID"]));
                if (selectedRow > -1)
                {
                    old_values = keep_old_values();
                    ok = base.edit();
                }
                else
                {
                    Message = "Unable to find the row to edit.";
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
                    if (key.ToString().Equals(this.IdentityColumn)) continue;
                    if (crud.Equals("Edit") && key.ToString().Equals("ComputationType"))
                    {
                        short oldct = Convert.ToInt16(old_values["ComputationType"]);
                        short newct = Convert.ToInt16(ht[key]);
                        bool old_istier = isTier(oldct);
                        bool new_isnot_tier = !isTier(newct);
                        if (oldct != newct && old_istier && new_isnot_tier)
                        {
                            if (del_tiers == null) del_tiers = new ArrayList();
                            del_tiers.Add(ht["SEQ_ID"]);
                        }
                    }
                    Object val = ht[key];
                    if (key.ToString().Equals("Currency"))
                    {
                        short curr = 0;
                        try
                        {
                            curr = Convert.ToInt16(ht[key]);
                        }
                        catch { }
                        val = (curr == 0) ? null : ht[key];
                    }
                    if (!setColumn(key.ToString(), val))
                    {
                        ok = false;
                        break;
                    }
                }
                setColumn("LastUpdate", DateTime.Today);
                if (ok & addMode)
                {
                    uprow["Deleted"] = false;
                    table.Rows.Add(uprow);
                    addMode = false;
                }
            }

            if (ok & crud.Equals("EDIT"))
            {
                ht.Add("OLD_VALUES", old_values);
            }

            return ok;
        }

        public DataTable saveUpdates()
        {
            if (base.save(sqlHelperWs.RETAIN_DATASET))
            {
                if (del_tiers != null && del_tiers.Count > 0)
                {
                    for (int i = 0; i < del_tiers.Count; i++)
                    {
                        try
                        {
                            int cid = Convert.ToInt16(del_tiers[i]);
                            String sql = "DELETE FROM tbl_charges_tier WHERE ChargesID = " + cid;
                            int ra = sqlUpdate(sql);
                        }
                        catch { }
                    }
                }
                del_tiers = null;
                selectedRow = 0;
                this.curr_prod_id = Convert.ToInt32(getColumn("ProductChargesID"));
                getUpdateDS(this.curr_prod_id);
                if (isUpdatable) return table;
            }

            return null;
        }

        private void load_tier_values()
        {
            String sql = "SELECT FldValue FROM tbl_lookup WHERE  (FldGroup = 'ComputationType') AND (FldDescription LIKE '%tier%')";
            DataSet ds = getDataSet(sql, false, new Object[] { });
            if (ds != null && ds.Tables.Count > 0)
            {
                ctype_tiers = new short[ds.Tables[0].Rows.Count];
                int i = 0;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ctype_tiers[i] = Convert.ToInt16(row["FldValue"]);
                    i++;
                }
            }
        }

        public irsChargeDetail()
        {
            base.TableName = "TBL_CHARGES";
            base.IdentityColumn = "SEQ_ID";
            load_tier_values();
        }
    }

    [Serializable]
    public class irsCharges : sqlHelperWs
    {
        protected irsChargeDetail detail;
        protected irsChargesTier tiers;

        protected Object[] chusers;

        protected short currentChargeGroup = 0;

        public short ChargeGroup
        {
            get { return currentChargeGroup; }
            set
            {
                if (value == 0 || isChargeGroup(value))
                {
                    currentChargeGroup = value;
                }
                else
                {
                    throw new Exception("Invalid charge group: " + value);
                }
            }
        }

        public short setChargeGroup(String brcode)
        {
            currentChargeGroup = this.getChargeGroup(brcode);
            return currentChargeGroup;
        }

        public bool isChargeGroup(short chgroup)
        {
            Object o = execScalar("SELECT COUNT(*) AS CGROUP_COUNT FROM TBL_LOOKUP WHERE FldGroup = 'ChargeGroup' AND FldValue = " + chgroup);
            if (o != DBNull.Value)
            {
                return (Convert.ToInt16(o) > 0);
            }

            return false;
        }

        public DataTable getChargeGroupBranches(short chgroup)
        {
            String sql = "SELECT BranchCode, BranchName FROM TBL_BRANCHES WHERE ChargeGroup = " + chgroup;
            DataSet ds = base.getDataSet(getSqlCommand(sql, false));
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        public DataTable getCharges(String brcode, short prodcode, short paymode, short implem, short curr)
        {
            String sql = "SELECT TOP 1 * FROM " + TableName + " WHERE BranchCode = '" + brcode + "' " +
                "AND ProductCode = " + prodcode + " " +
                "AND PayMode = " + paymode + " " +
                "AND ImplementationType = " + implem;

            if (curr > -1)
            {
                sql += " AND Currency = " + curr;
            }

            sql += " AND (Deleted is null OR Deleted = 0)";

            DataSet ds = getDataSet(getSqlCommand(sql, false));
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        public DataTable getCharges(String brcode, short prodcode, short paymode, short implem)
        {
            return getCharges(brcode, prodcode, paymode, implem, -1);
        }

        public bool isChargesAvailable(String brcode, short prodcode, short paymode, short implem, short comp)
        {
            String sql = "SELECT Count(*) FROM " + TableName + "WHERE BranchCode = '" + brcode + "' " +
                "AND ProductCode = " + prodcode + " " +
                "AND PayMode = " + paymode + " " +
                "AND ImplementationType = " + implem + " " +
                "AND (Deleted is null OR Deleted = 0)";

            Object o = execScalar(sql);
            if (o != DBNull.Value)
            {
                return (Convert.ToInt32(o) > 0);
            }

            return false;
        }

        public short getChargeGroup(String brcode)
        {
            Object o = execScalar("SELECT ChargeGroup FROM TBL_BRANCHES WHERE BranchCode = '" + brcode + "'");
            if (o != DBNull.Value)
            {
                try
                {
                    return Convert.ToInt16(o);
                }
                catch { }
            }

            return 0;
        }

        public DataSet getLookupBranches()
        {
            String sqlstmts = 
                "exec dbo.get_dropdown_branches; " +
                "select fldValue, fldDescription from tbl_lookup where fldGroup = 'ChargeGroup';";
            DataSet ds = getDataSet(getSqlCommand(sqlstmts, false));
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }

            return null;
        }

        public DataSet get_ddl_data(String brcode, out String errmsg)
        {
            base.Message = "";
            errmsg = "";

            DataSet ds = getDataSet("sp_getChargesLookups", true, new Object[] { brcode });

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }

            return null;
        }

        public DataSet get_local_curr_data(String brcode)
        {
            return getDataSet("sp_getCurrencyByCategory", true, new Object[] { brcode, "LOCAL CURRENCY" });
        }

        public String gen_expression(Hashtable ht)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Object key in ht.Keys)
            {
                String s = key.ToString();
                if (s.Equals("ComputationLevel")) continue;
                if (s.Equals("ChargeGroup")) continue;

                sb.Append(s);
                if (ht[key] == null | ht[key].ToString().Length == 0)
                {
                    sb.Append(" is null");
                }
                else if (ht[key].Equals("false") | ht[key].Equals("true"))
                {
                    String bol = (ht[key].Equals("true")) ? "1" : "0";
                    if (bol.Equals("0") & s.Equals("Promotion"))
                    {
                        sb.Append("(Promotion = 0 OR Promotion is null)");
                    }
                    else
                        sb.Append(" = " + bol);
                }
                else
                {
                    sb.Append(" = ");
                    if (s.Equals("BranchCode") | s.Equals("ValidityFrom") | s.Equals("ValidityTo")) sb.Append("'");
                    sb.Append(ht[key].ToString());
                    if (s.Equals("BranchCode") | s.Equals("ValidityTo") | s.Equals("ValidityTo")) sb.Append("'");
                }

                sb.Append(" AND ");
            }

            String s1 = sb.ToString();
            if (s1.EndsWith(" AND "))
            {
                return s1.Substring(0, s1.Length - 5);
            }

            return s1;
        }

        public bool rowExist(Hashtable ht, int seqid)
        {
            String wexpr = gen_expression(ht);

            bool exist = false;
            try
            {
                String sql = "SELECT count(*) as count_exist FROM " + TableName + " WHERE " + wexpr + " AND (Deleted is null OR Deleted = 0)";
                if (seqid > 0)
                {
                    sql += " AND (SEQ_ID <> " + seqid + ")";
                }
                Object o = execScalar(sql);
                if (o != DBNull.Value)
                {
                    int cnt = Convert.ToInt32(o);
                    exist = (cnt > 0);
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return exist;
        }

        public DataTable getChargeMaster(String brcode, out String errmsg)
        {
            errmsg = "";
            base.getUpdateDS("sp_getChargeMaster", new Object[] { brcode, (short) 0 });
            if (isUpdatable)
            {
                return table;
            }

            errmsg = Message;

            return null;
        }

        public DataTable getChargeMaster(short chgroup, out String errmsg)
        {
            errmsg = "";
            base.getUpdateDS("sp_getChargeMaster", new Object[] { String.Empty, chgroup });
            if (isUpdatable)
            {
                return table;
            }

            errmsg = Message;

            return null;
        }

        private bool applyDelete(String crud, Hashtable ht)
        {
            selectedRow = findRow("SEQ_ID", Convert.ToInt32(ht["SEQ_ID"]));
            if (base.edit())
            {
                if (setColumn("Deleted", true))
                {
                    if (setColumn("LastUpdate", DateTime.Today))
                    {
                        int iseqid = Convert.ToInt32(ht["SEQ_ID"]);
                        if (detail == null) detail = new irsChargeDetail();
                        detail.getUpdateDS(iseqid);
                        if (detail.isUpdatable)
                        {
                            if (tiers == null) tiers = new irsChargesTier();
                            if (detail.deleteDetails(tiers))
                            {
                                return save(sqlHelperWs.RETAIN_DATASET);
                            }
                        }
                    }
                }
            }
            return false;
        }

        private Hashtable keep_old_values()
        {
            Hashtable old_values = new Hashtable();
            old_values.Add("ProductCode", table.Rows[selectedRow]["ProductCode"]);
            old_values.Add("ImplementationType", table.Rows[selectedRow]["ImplementationType"]);
            old_values.Add("Currency", table.Rows[selectedRow]["Currency"]);
            old_values.Add("ChargeType", table.Rows[selectedRow]["ChargeType"]);
            old_values.Add("PayMode", table.Rows[selectedRow]["PayMode"]);
            old_values.Add("DefaultStatus", table.Rows[selectedRow]["DefaultStatus"]);
            old_values.Add("Promotion", table.Rows[selectedRow]["Promotion"]);
            old_values.Add("ValidityFrom", table.Rows[selectedRow]["ValidityFrom"]);
            old_values.Add("ValidityTo", table.Rows[selectedRow]["ValidityTo"]);
            return old_values;
        }

        public bool applyUpdates(String crud, Hashtable ht)
        {
            bool ok = false;
            Hashtable old_values = null;

            if (crud.Equals("DELETE"))
            {
                return applyDelete(crud, ht);
            }
            if (crud.Equals("EDIT"))
            {
                selectedRow = findRow("SEQ_ID", ht["SEQ_ID"]);
                if (selectedRow > -1)
                {
                    old_values = keep_old_values();
                    ok = base.edit();
                }
                else
                {
                    Message = "Charges: Unable to locate the row for edit.";
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
                    Object val = ht[key];
                    if (key.ToString().Equals(this.IdentityColumn)) continue;
                    if (key.ToString().Equals("BranchGroup") | key.ToString().Equals("BranchCode"))
                    {
                        if ((val.GetType() == typeof(System.String) && val.ToString().Equals("0")) ||
                            (val.GetType() == typeof(System.Int32)))
                        {
                            try
                            {
                                short sh = Convert.ToInt16(val);
                                if (sh == 0)
                                {
                                    val = DBNull.Value;
                                }
                            }
                            catch (Exception e)
                            {
                                Message = "applyUpdate " + e.Message;
                                return false;
                            }
                        }
                    }
                    if (val.GetType() == typeof(System.String) && (val.ToString().Equals("null") || val.ToString() == String.Empty))
                    {
                        val = DBNull.Value;
                    }

                    if (!setColumn(key.ToString(), val))
                    {
                        ok = false;
                        break;
                    }
                }
                setColumn("LastUpdate", DateTime.Today);
                if (ok & addMode)
                {
                    uprow["Deleted"] = false;
                    table.Rows.Add(uprow);
                    addMode = false;
                }
            }

            if (ok & crud.Equals("EDIT"))
            {
                ht.Add("OLD_VALUES", old_values);
            }

            return ok;
        }

        public bool applyUpdates(String type, String crud, Hashtable ht)
        {
            bool ok = false;

            if (type.Equals("Product"))
            {
                ok = applyUpdates(crud, ht);
            }
            else if (type.Equals("Charges"))
            {
                if (detail != null)
                {
                    if (tiers == null) tiers = new irsChargesTier();
                    ok = detail.applyUpdates(crud, ht, tiers);
                }
                else
                {
                    Message = "Charges have not been initialized.";
                }
            }
            else if (type.Equals("Tier"))
            {
                if (tiers != null)
                {
                    ok = tiers.applyUpdates(crud, ht);
                }
                else
                {
                    Message = "Tiers table is not initialized.";
                }
            }
            else if (type.Equals("Users") | type.Equals("Over"))
            {
                irsChargesUsers iusr = (irsChargesUsers) chusers[(type.Equals("Users")) ? 0 : 1];
                if (iusr != null)
                {
                    ok = iusr.applyUpdates(crud, ht);
                }
            }

            return ok;
        }

        public DataTable saveUpdates(String type, out String errmsg)
        {
            errmsg = "";
            if (type.Equals("Product"))
            {
                if (base.save(sqlHelperWs.RETAIN_DATASET))
                {
                    DataTable dtm = null;
                    Object obr = table.Rows[0]["BranchCode"];
                    if (obr == DBNull.Value || obr == null)
                    {
                        short cg = Convert.ToInt16(table.Rows[0]["BranchGroup"]);
                        dtm = getChargeMaster(cg, out errmsg);
                    }
                    else
                    {
                        dtm = getChargeMaster(obr.ToString(), out errmsg);
                    }
                    if (dtm != null)
                    {
                        return dtm;
                    }
                }
            }
            else if (type.Equals("Charges"))
            {
                if (detail != null)
                {
                    DataTable dtl =  detail.saveUpdates();
                    if (detail.isUpdatable) return dtl;
                }
            }
            else if (type.Equals("Tier"))
            {
                if (tiers != null)
                {
                    DataTable tdt = tiers.saveUpdates();
                    if (tiers.isUpdatable)
                    {
                        return tdt;
                    }
                }
            }
            else if (type.Equals("Users") | type.Equals("Over"))
            {
                irsChargesUsers iusr = (irsChargesUsers)chusers[(type.Equals("Users")) ? 0 : 1];
                if (iusr != null)
                {
                    return iusr.saveUpdates();
                }
            }

            errmsg = Message;

            return null;
        }

        public DataTable getChargesDetail(Object seqid)
        {
            try
            {
                int iseqid = Convert.ToInt32(seqid);
                if (detail == null) detail = new irsChargeDetail();

                detail.getUpdateDS(iseqid);
                if (detail.isUpdatable)
                {
                    return detail.table;
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return null;
        }

        public Object[] getChargesUsers(int seqid)
        {
            short user_types = 1;
            chusers = new Object[2];
            DataTable[] odt = new DataTable[2];
            String[] errmsg = { "", "" };
            while (user_types < 3)
            {
                int j = user_types - 1;
                irsChargesUsers usr = new irsChargesUsers(user_types);
                chusers[j] = usr;
                odt[j] = usr.getUserPermissions(seqid);
                if (odt[j] == null)
                {
                    errmsg[j] = usr.Message;
                }
                user_types++;
            }

            return new Object[] { odt, errmsg };
        }

        public DataTable getChargesTier(Object seqid)
        {
            try
            {
                int iseqid = Convert.ToInt32(seqid);
                if (tiers == null) tiers = new irsChargesTier();

                tiers.getUpdateDS(iseqid);
                if (tiers.isUpdatable)
                {
                    return tiers.table;
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return null;
        }

        public irsCharges()
		{            
			this.TableName      = "TBL_CHARGE_MASTER";
			this.IdentityColumn = "SEQ_ID";
		}

        ~irsCharges()
        {
            detail = null;
            tiers = null;
            chusers = null;
        }
    }

    [Serializable]
    public class irsChargeGroupUpdate : irsCharges
    {

        private String get_right_hand(Object o)
        {
            if (o == DBNull.Value)
            {
                return "is null";
            }

            return o.ToString();
        }

        private String gen_expression(DataTable dt, Hashtable ht)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ProductCode = " + get_right_hand(dt.Rows[0]["ProductCode"]) + " ");
            sb.Append("AND ImplementationType = " + get_right_hand(dt.Rows[0]["ImplementationType"]) + " ");
            sb.Append("AND Currency = " + get_right_hand(dt.Rows[0]["Currency"]) + " ");
            sb.Append("AND ChargeType = " + get_right_hand(dt.Rows[0]["ChargeType"]) + " ");
            sb.Append("AND PayMode = " + get_right_hand(dt.Rows[0]["PayMode"]) + " ");
            sb.Append("AND DefaultStatus = " + get_right_hand(dt.Rows[0]["DefaultStatus"]) + " ");
            String promo = get_right_hand(dt.Rows[0]["Promotion"]);
            if (!promo.Equals("is null"))
            {
                promo = (promo.Equals("False")) ? "0" : (promo.Equals("True")) ? "1" : promo;
            }
            if (promo.Equals("0"))
            {
                sb.Append("AND (Promotion = 0 OR Promotion is null) ");
            }
            else
            {
                sb.Append("AND Promotion = " + promo + " ");
            }
            sb.Append("AND Deleted = 0");

            return sb.ToString();
        }

        private int getProductID(Hashtable ht, String ht_key)
        {
            String sql = "SELECT * FROM " + TableName + " WHERE SEQ_ID = " + Convert.ToInt32(ht[ht_key]);
            DataSet ds = getDataSet(getSqlCommand(sql, false));
            if (ds != null && ds.Tables.Count > 0)
            {
                String wexpress = gen_expression(ds.Tables[0], ht);
                DataRow[] rows = base.table.Select(wexpress);
                if (rows.Length > 0)
                {
                    return Convert.ToInt32(rows[0]["SEQ_ID"]);
                }
            }

            return -1;
        }

        private int getProductID(Hashtable ht)
        {
            Hashtable htin = (ht.ContainsKey("OLD_VALUES")) ? (Hashtable)ht["OLD_VALUES"] : ht;
            try
            {
                htin.Remove("OLD_VALUES");
            }
            catch { }
            String wexpress = gen_expression(htin);

            DataRow[] rows = base.table.Select(wexpress);
            if (rows.Length > 0)
            {
                return Convert.ToInt32(rows[0]["SEQ_ID"]);
            }

            return -1;
        }

        private bool doUpdateProduct(String crud, Hashtable ht)
        {
            String xcrud = crud;
            int iseqid = getProductID(ht);
            if (iseqid < 0)
            {
                //how to delete if it is not there.
                if (crud.Equals("DELETE")) return true;
                //We need to add this.
                xcrud = "ADD";
            }
            else
            {
                if (crud.Equals("ADD")) xcrud = "EDIT";
                ht["SEQ_ID"] = iseqid;
            }

            Hashtable htin = ht;
            try
            {
                htin.Remove("OLD_VALUES");
            }
            catch { }

            return applyUpdates(xcrud, htin);
        }

        private int getChargesID(Hashtable ht)
        {
            Hashtable htin = (ht.ContainsKey("OLD_VALUES")) ? (Hashtable)ht["OLD_VALUES"] : ht;
            DataRow[] rows = detail.table.Select("ProductChargesId = " + (int)ht["ProductChargesID"] + " AND ChargeName = " + Convert.ToInt16(htin["ChargeName"]));
            if (rows.Length > 0)
            {
                return Convert.ToInt32(rows[0]["SEQ_ID"]);
            }

            return -1;
        }

        private bool load_charges(Hashtable ht)
        {
            int cpid = getProductID(ht, "ProductChargesID");
            if (cpid < 0)
            {
                Message = "Unable to link Charges to Product";
                return false;
            }

            ht["ProductChargesID"] = cpid;

            DataTable cdt = null;
            if (detail != null && detail.curr_prod_id == cpid)
            {
                cdt = detail.table;
            }
            else
            {
                cdt = getChargesDetail(cpid);
            }
            if (cdt == null)
            {
                Message = "Unable to get Charges data.";
                return false;
            }

            return true;
        }

        private bool doUpdateCharges(String crud, Hashtable ht)
        {
            String xcrud = crud;

            if (load_charges(ht))
            {
                int cseqid = getChargesID(ht);
                if (cseqid < 0)
                {
                    if (crud.Equals("DELETE")) return true;
                    xcrud = "ADD";
                }
                else
                {
                    if (crud.Equals("ADD")) xcrud = "EDIT";
                    ht["SEQ_ID"] = cseqid;
                }

                Hashtable htin = ht;
                try
                {
                    htin.Remove("OLD_VALUES");
                }
                catch { }

                return base.applyUpdates("Charges", crud, ht);
            }

            return false;
        }

        private String getTierExpression(Hashtable old_values)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("AmountFrom = " + ((decimal)old_values["AmountFrom"]).ToString() + " ");
            sb.Append("AND AmountTo = " + ((decimal)old_values["AmountTo"]).ToString() + " ");
            sb.Append("AND ChargeAmount = " + ((double)old_values["ChargeAmount"]).ToString() + " ");

            return sb.ToString();
        }

        private bool doUpdateTier(String crud, Hashtable ht)
        {
            Hashtable old_values = null;

            if (crud.Equals("EDIT"))
            {
                if (ht.ContainsKey("OLD_VALUES"))
                {
                    old_values = (Hashtable)ht["OLD_VALUES"];
                    ht.Remove("OLD_VALUES");
                }
                else
                {
                    Message = "Tier update requires OLD_VALUES.";
                    return false;
                }
            }

            int chgroup_chid = Convert.ToInt32(ht["ChargesID"]);
            String sql = "SELECT ProductChargesID, ChargeName FROM TBL_CHARGES WHERE SEQ_ID = " + chgroup_chid;
            DataSet ds = getDataSet(getSqlCommand(sql, false));
            if (ds == null && ds.Tables.Count == 0)
            {
                return false;
            }
            String chgroup_chname = ds.Tables[0].Rows[0]["ChargeName"].ToString();
            int chgroup_pid = Convert.ToInt32(ds.Tables[0].Rows[0]["ProductChargesID"]);

            Hashtable temp = new Hashtable();
            temp.Add("ProductChargesID", chgroup_pid);

            int br_pid = getProductID(temp, "ProductChargesID");

            sql = "SELECT SEQ_ID FROM tbl_charges WHERE ProductChargesID = " + br_pid + " AND ChargeName = " + chgroup_chname;
            Object o = execScalar(sql);
            if (o == null | o == DBNull.Value)
            {
                if (o == DBNull.Value)
                {
                    Message = "Unable to get the ChargeID.";
                }
                return false;
            }

            int br_chid = Convert.ToInt32(o);
            ht["ChargesID"] = br_chid;
            // Do I have to load the charges data, try go directly to tier. 
            if (base.tiers == null || base.tiers.curr_charge_id == br_chid)
            {
                base.getChargesTier(br_chid);
            }

            String xcrud = crud;
            if (base.tiers.isUpdatable)
            {
                if (!crud.ToUpper().Equals("ADD"))
                {
                    DataRow[] rows = base.tiers.table.Select(getTierExpression(old_values));
                    if (rows.Length > 0)
                    {
                        if (crud.Equals("ADD")) xcrud = "EDI";
                        ht["SEQ_ID"] = Convert.ToInt32(rows[0]["SEQ_ID"]);
                    }
                    else
                    {
                        if (crud.Equals("DELETE")) return true;
                        xcrud = "ADD";
                    }
                }
                return base.applyUpdates("Tier", crud, ht);
            }

            return false;
        }

        private bool doUpdateUsers(String type, String crud, Hashtable ht)
        {
            int pid = this.getProductID(ht, "ProductChargesID");
            if (pid > -1)
            {
                ht["ProductChargesID"] = pid;
                int uidx = type.Equals("Users") ? 0 : 1;
                if (base.chusers == null || chusers[uidx] == null || ((irsChargesUsers)chusers[uidx]).curr_charge_id != pid)
                {
                    Object[] dtdt = base.getChargesUsers(pid);
                }
                // crud is always "EDIT"
                irsChargesUsers icu = (irsChargesUsers)chusers[uidx];
                if (icu.isUpdatable)
                {
                    int ht_seqid = Convert.ToInt32(ht["SEQ_ID"]);
                    if (ht_seqid > 0) // need to find the seqid
                    {
                        short role = Convert.ToInt16(ht["Role"]);
                        short ut = Convert.ToInt16(ht["UserType"]);
                        DataRow[] rows = icu.table.Select("UserType = " + ut + " AND Role = " + role);
                        if (rows.Length > 0)
                        {
                            ht["SEQ_ID"] = rows[0]["SEQ_ID"];
                        }
                        else
                        {
                            // its not there, we need to add this? only if set allowed
                            bool allowed = Convert.ToBoolean(ht["Allowed"]);
                            if (allowed) ht["SEQ_ID"] = 0;
                        }
                    }

                    return base.applyUpdates(type, crud, ht);
                }
            }

            return false;
        }

        private bool doUpdates(String type, String crud, Hashtable ht)
        {
            switch (type.ToUpper())
            {
                case "PRODUCT": return doUpdateProduct(crud, ht);

                case "CHARGES": return doUpdateCharges(crud, ht);

                case "TIER": return doUpdateTier(crud, ht);

                case "USERS": 
                case "OVER": return doUpdateUsers(type, crud, ht);
            }

            Message = "Unsupported type: " + type;
            return false;
        }

        private Hashtable copy_ht(Hashtable htin)
        {
            Hashtable ht = new Hashtable();
            foreach (Object key in htin.Keys)
            {
                ht.Add(key, htin[key]);
            }

            return ht;
        }

        public bool cascade_edits(String brcode, String type, ArrayList htcol)
        {
            if (htcol.Count > 0)
            {
                String msg = String.Empty;
                base.getChargeMaster(brcode, out msg);

                if (isUpdatable)
                {
                    ArrayList errmsg = new ArrayList();
                    foreach (Object o in htcol)
                    {
                        Object[] oo = (Object[])o;
                        String crud = (String)oo[0];
                        Hashtable ht = copy_ht((Hashtable)oo[1]);

                        if (type.ToLower().Equals("product"))
                        {
                            ht["BranchCode"] = brcode;
                            ht["BranchGroup"] = ChargeGroup;
                        }

                        this.doUpdates(type, crud, ht);
                    }
                    if (errmsg.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (Object os in errmsg)
                        {
                            sb.Append(os.ToString() + "\n");
                        }
                        Message = sb.ToString();
                    }
                    else
                    {
                        DataTable dt = base.saveUpdates(type, out msg);
                        if (dt == null)
                        {
                            Message = msg;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    Message = msg;
                }
            }
            else
            {
                return true;
            }

            return false;
        }
    }
}
