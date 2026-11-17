using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

namespace IRSClassLib
{
    [Serializable]
    public class irsCurrencyRates : irsSqlCommonsWs
    {
        private DataTable dtsrc;
        private DataTable dttgt;

        private bool fx_ready;

        private DataSet dsFxRates;
        private DateTime dtFxdate;
        private String dtFxmsg;

        private Hashtable dart; // in Ws version, this shall contain command keep object.
        private Hashtable dsrt;
        private Hashtable dsDeletes;
        private Hashtable brcodes;
        private String TableName;
        private String IdentityColumn;

        private String upload_column = "BUYING_RATE";
        public String UploadColumn
        {
            get { return upload_column; }
            set { upload_column = value; }
        }

        private bool has_delete(int rate_type)
        {
            try
            {
                return (bool)dsDeletes[rate_type];
            }
            catch { }

            return false;
        }

        private void setHasDelete(int rate_type, bool has)
        {
            if (dsDeletes == null) dsDeletes = new Hashtable();

            if (dsDeletes.ContainsKey(rate_type))
            {
                dsDeletes[rate_type] = has;
            }
            else
            {
                dsDeletes.Add(rate_type, has);
            }
        }

        private String currentBranch(int rate_type)
        {
            try
            {
                return brcodes[rate_type].ToString();
            }
            catch { }

            return String.Empty;
        }

        private void setCurrentBranch(int rate_type, String brcode)
        {
            if (brcodes.ContainsKey(rate_type))
            {
                brcodes[rate_type] = brcode;
            }
            else
            {
                brcodes.Add(rate_type, brcode);
            }
        }

        private bool isUpdatable(int rate_type)
        {
            if (!dsrt.ContainsKey(rate_type)) return false;
            DataSet ds = (DataSet)dsrt[rate_type];
            return (ds != null && ds.Tables.Count > 0);
        }

        private DataTable getTable(int rate_type)
        {
            if (!dsrt.ContainsKey(rate_type)) return null;
            DataSet ds = (DataSet)dsrt[rate_type];
            if (ds.Tables.Count > 0) return ds.Tables[0];
            return null;
        }

        public bool hasChanges(int rate_type)
        {
            if (!dsrt.ContainsKey(rate_type)) return false;
            return ((DataSet)dsrt[rate_type]).HasChanges();
        }

        public void clearDatasets(int rate_type)
        {
            if (dart.ContainsKey(rate_type))
            {
                dart[rate_type] = null;
                dart.Remove(rate_type);
            }

            if (dsrt.ContainsKey(rate_type))
            {
                ((DataSet)dsrt[rate_type]).Dispose();
                dsrt[rate_type] = null;
                dsrt.Remove(rate_type);
            }
        }

        public int[] hasChanges()
        {
            try
            {
                ArrayList al = new ArrayList();
                foreach (Object key in dsrt.Keys)
                {
                    int rate_type = Convert.ToInt32(key);
                    if (hasChanges(rate_type))
                    {
                        al.Add(rate_type);
                    }
                }

                if (al.Count == 0) return null;

                int[] ii = new int[al.Count];
                for (int i = 0; i < al.Count; i++)
                {
                    ii[i] = (int)al[i];
                }
                return ii;
            }
            catch //(Exception e) // commented to remove warning
            { }
            return null;
        }

        private bool getUpdateDataSet(String keep, int rate_type) // (SqlCommand cmd, int rate_type) // not for ws
        {
            /* not for ws
            SqlDataAdapter da = getDataAdapter(cmd); 
            if (da == null) return false;

            DataSet ds = base.getDataSet(da, this.TableName);
            if (ds == null) return false; 

            sqlHelperCommandKeep keep = new sqlHelperCommandKeep();
            keep.keep(cmd, true); 
             * not for ws */

            DataSet ds = base.getDataSet(keep, false, new Object[] { }, new String[] { this.TableName });
            if (ds == null) return false;

            if (dart.ContainsKey(rate_type))
            {
                dart[rate_type] = keep;
            }
            else
            {
                dart.Add(rate_type, keep);
            }

            if (dsrt.ContainsKey(rate_type))
            {
                dsrt[rate_type] = ds;
            }
            else
            {
                dsrt.Add(rate_type, ds);
            }
            return true;
        }

        public bool isComplete(String brcode, DateTime trandate)
        {
            return this.isComplete(brcode, trandate, 1);
        }

        public bool isComplete(String brcode, DateTime trandate, int rate_type)
        {
            if (!isUpdatable(rate_type))
            {
                this.getCurrencyRates(trandate, brcode, rate_type, false);
            }
            if (dtsrc == null)
            {
                this.getCurrencyTables(brcode);
            }

            object[] oo = getNextCombo(rate_type, upload_column, true);
            return ((short)oo[0] == -1 & (short)oo[1] == -1);
        }

        private decimal getFxUsdRate(String src_curr, String tgt_curr, String fxcol, out String msg)
        {
            int slim = 0;
            msg = "";

            while (!fx_ready)
            {
                Thread.Sleep(20);
                slim += 20;
                if (slim > 60000) return 0.000000000M;
            }

            if (src_curr.Equals(tgt_curr)) return 1.000000000M;

            String src = (src_curr.Equals("YEN")) ? "JPY" : src_curr;
            String tgt = (tgt_curr.Equals("YEN")) ? "JPY" : tgt_curr;
            int tabnum = (fxcol.Equals("BOOKING_RATE") | fxcol.Equals("MID_RATE")) ? 2 : 1;
            DataTable dt = dsFxRates.Tables[tabnum];

            DataRow[] row = dt.Select("CURRENCY_CODE = '" + tgt + "'");
            if (row.Length > 0)
            {
                Decimal d = 0.000000000M;
                try
                {
                    d = Convert.ToDecimal(row[0][fxcol]);
                    row = dt.Select("CURRENCY_CODE = '" + src + "'");
                    Decimal d_src = (row.Length > 0) ? Convert.ToDecimal(row[0][fxcol]) : 0.000000000M;

                    if (src.Equals("PHP") & fxcol.Equals("MID_RATE"))
                    {
                        d = d_src / d;
                    }
                    else if (tgt.Equals("PHP") & fxcol.Equals("MID_RATE"))
                    {
                        d = d / d_src;
                    }
                    else if (src.Equals("PHP"))
                    {
                        d = (1 / d_src) / d;
                    }
                    else if (tgt.Equals("PHP"))
                    {
                        d = d / (1 / d_src);
                    }
                    else if (src.Equals("USD"))
                    {
                        d = d_src / d;
                    }
                    else if (tgt.Equals("USD"))
                    {
                        d = d_src / d;
                    }
                    else
                    {
                        d = d_src / d;
                    }
                }
                catch (Exception e)
                {
                    msg = e.Message + ": Currency not in Cybermag src = " + src + " - " + tgt;
                    d = 0.000000000M;
                }

                return Math.Round(d, 9);
            }

            msg = "Currency not in Cybermag: " + tgt;
            return 0.000000000M;
        }

        public bool comboExist(int rate_type, short src_curr, short tgt_curr)
        {
            if (isUpdatable(rate_type))
            {
                DataRow[] rows = getTable(rate_type).Select("SourceCurrency = " + src_curr + " AND TargetCurrency = " + tgt_curr);
                return (rows.Length > 0);
            }

            return false;
        }

        public Object[] getNextCombo(int rate_type, String fxcol, bool fxskip)
        {
            if (!isUpdatable(rate_type) & dtsrc == null & dttgt == null)
            {
                Message = "Initialization has not been done.";
                return null;
            }

            if (dtsrc.Rows.Count == 0 | dttgt.Rows.Count == 0)
            {
                return new Object[] { (short)0, (short)0, 0.000000000M };
            }

            String msg = "";

            for (int i = 0; i < dtsrc.Rows.Count; i++)
            {
                short src_curr = Convert.ToInt16(dtsrc.Rows[i]["fldValue"]);
                String src_curr_code = (String)dtsrc.Rows[i]["fldCode"];
                for (int j = 0; j < dttgt.Rows.Count; j++)
                {
                    short tgt_curr = Convert.ToInt16(dttgt.Rows[j]["fldValue"]);
                    String tgt_curr_code = (String)dttgt.Rows[j]["fldCode"];
                    if (comboExist(rate_type, src_curr, tgt_curr)) continue;
                    if (fxskip)
                    {
                        return new Object[] { src_curr, tgt_curr };
                    }
                    decimal d = getFxUsdRate(src_curr_code, tgt_curr_code, fxcol, out msg);
                    return new Object[] { src_curr, tgt_curr, d, msg };
                }
            }
            return new Object[] { (short)-1, (short)-1, 0.000000000M, "" };
        }

        public Object[] getNextCombo(int rate_type, bool is_ord)
        {
            Object[] buy_rates = getNextCombo(rate_type, upload_column, false);
            if (is_ord & rate_type == 1)
            {
                Object[] mid_rates = getNextCombo(rate_type, "MID_RATE", false);
                String msg = "";
                if (((String)buy_rates[buy_rates.Length - 1]).Length > 0) msg += (String)buy_rates[buy_rates.Length - 1] + " ";
                if (((String)mid_rates[mid_rates.Length - 1]).Length > 0) msg += (String)mid_rates[mid_rates.Length - 1];
                return new Object[] { buy_rates[0], buy_rates[1], buy_rates[2], mid_rates[2], msg.Trim() };
            }

            return buy_rates;
        }

        public DataTable getBranches()
        {
            try
            {
                DataSet ds = getDataSet(getSqlCommand("get_dropdown_branches"));
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            catch { }

            return null;
        }

        public bool get_edit_setup(String currbr)
        {
            String sql = "SELECT BranchDefCode FROM tbl_Branch_Defaults where branchcode = '" + currbr + "' and BranchDefValue = 'CurrencyRatesNoEdit'";
            try
            {
                Object o = base.execScalar(sql);
                if (o != null || o != System.DBNull.Value || o.ToString().IndexOf("DBNull") == -1)
                {
                    return Convert.ToBoolean(o);
                }
            }
            catch { }

            return false;
        }

        public DataTable getRateTypes()
        {
            String sql = "SELECT * FROM TBL_LOOKUP WHERE FldGroup = 'RATETYPE' ORDER BY FldValue";
            DataSet ds = base.getDataSet(getSqlCommand(sql, false));
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        public DataTable getCurrencyTable()
        {
            return getLookup(new Object[] { "Currency", DBNull.Value, -1 });
        }

        private DataTable getCurrencyGroupTable(String tname, DataTable dtin)
        {
            DataTable dt = new DataTable(tname);
            dt.Columns.Add(new DataColumn("fldValue", typeof(System.Int16)));
            dt.Columns.Add(new DataColumn("fldCode", typeof(System.String)));

            for (int i = 0; i < dtin.Rows.Count; i++)
            {
                String s = dtin.Rows[i]["fldDescription"].ToString().ToUpper();
                if (s.StartsWith(tname.ToUpper()))
                {
                    DataRow uprow = dt.NewRow();
                    uprow["fldValue"] = dtin.Rows[i]["fldValue"];
                    uprow["fldCode"] = dtin.Rows[i]["fldCode"];
                    dt.Rows.Add(uprow);
                }
            }

            return dt;
        }

        public DataSet getCurrencyTables()
        {
            DataSet dsnew = new DataSet();
            dsnew.Tables.Add(dtsrc);
            dsnew.Tables.Add(dttgt);

            return dsnew;
        }

        private void merge_src_to_target()
        {
            for (int i = 0; i < dtsrc.Rows.Count; i++)
            {
                if (dttgt.Select("fldValue = " + dtsrc.Rows[i]["fldValue"].ToString()).Length == 0)
                {
                    DataRow nrow = dttgt.NewRow();
                    nrow["FldValue"] = dtsrc.Rows[i]["FldValue"];
                    nrow["FldCode"] = dtsrc.Rows[i]["FldCode"];
                    dttgt.Rows.Add(nrow);
                }
            }
        }

        public DataSet getCurrencyTables(String location)
        {
            try
            {
                /* not for ws
                SqlCommand cmd = base.getSqlCommand("sp_getCurrencyByCategory"); //("sp_getBranchCurrencyByCategories");
                base.deriveParameters(cmd, new Object[] { location, String.Empty });
                DataSet ds = base.getDataSet(cmd);
                not for ws * */

                DataSet ds = getDataSet("sp_getCurrencyByCategory", true, new Object[] { location, String.Empty });

                if (ds != null && ds.Tables.Count > 0)
                {
                    dtsrc = getCurrencyGroupTable("LOCAL", ds.Tables[0]);
                    dttgt = getCurrencyGroupTable("ACCOUNT", ds.Tables[0]);

                    //try to merge src to target account
                    merge_src_to_target();

                    return getCurrencyTables();
                }
            }
            catch { }

            return null;
        }

        private void getCybermagRates(Object vdate)
        {
            fx_ready = false;
            dtFxmsg = String.Empty;

            irsForexUpload fx = new irsForexUpload();
            Object[] fxrates = fx.getUploadRates((DateTime)vdate);

            if (fxrates == null)
            {
                dtFxmsg = "Unable to connect to cybermag database.";
            }
            else
            {
                dsFxRates = (DataSet)fxrates[0];
            }
            fx_ready = true;
        }

        public Object[] getUploadRates(String brcode, DateTime vdate)
        {
            DateTime xvdate = getServerTime(vdate, brcode);

            TimeSpan vtime = new TimeSpan(1, 0, 0);
            if (xvdate.Subtract(dtFxdate) > vtime | dsFxRates == null)
            {
                dtFxdate = DateTime.Now;
                getCybermagRates(xvdate);
            }

            return new Object[] { dsFxRates, dtFxmsg };
        }

        public DataTable getCurrencyRates(DateTime vdate, String location)
        {
            return this.getCurrencyRates(vdate, location, 1, false);
        }

        private DataTable getCloneTable(int rate_type)
        {
            DataTable table = getTable(rate_type);
            bool has_dels = has_delete(rate_type);
            if (has_dels)
            {
                DataTable dtClone = table.Clone();
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
            return table;
        }

        private DateTime getServerTime(DateTime vdate, String from_brcode)
        {
            irsTimeZone tz = new irsTimeZone();
            String from_tz = tz.branchTimeZone(from_brcode);
            return tz.tzConvert(vdate, from_tz, tz.ServerTimezone);
        }

        public DataTable getCurrencyRates(DateTime vdate, String location, int rate_type, bool setup)
        {
            DateTime xvdate = getServerTime(vdate, location);
            if (setup)
            {
                try
                {

                    TimeSpan vtime = new TimeSpan(1, 0, 0);

                    if (xvdate.Subtract(dtFxdate) > vtime | dsFxRates == null)
                    {
                        this.dtFxdate = DateTime.Now;
                        Thread t = new Thread(new ParameterizedThreadStart(getCybermagRates));
                        t.Start(vdate);
                    }

                }
                catch { }
            }

            if (this.isUpdatable(rate_type) && currentBranch(rate_type).Equals(location))
            {
                if (hasChanges(rate_type))
                {
                    return getCloneTable(rate_type);
                }
            }

            setCurrentBranch(rate_type, location);
            clearDatasets(rate_type);

            //==========================================================
            // RFC-22918 : Currency rates issue in edit/delete record
            //String rts = (rate_type == 1) ? "(CurrencyType is null or CurrencyType = 1)" : "CurrencyType = " + rate_type;
            //String sql = "SELECT * FROM " + this.TableName + " " +
            //                "WHERE BranchCode = '" + location + "' " +
            //                "AND tranxdate = '" + vdate.ToString("MM/dd/yyyy") + "' " +
            //                "AND " + rts + " " +
            //                "ORDER BY DateInserted";
            String sql = "exec proc_get_currencyrates_record 0, '" + location + "', '" + vdate.ToString("MM/dd/yyyy") + "', 1";
            //==========================================================

            // SqlCommand cmd = getSqlCommand(sql, false); // not for ws


            if (!getUpdateDataSet(sql, rate_type)) return null; // cmd, rate_type)) return null; // not for ws
            if (isUpdatable(rate_type))
            {
                return getTable(rate_type);
            }

            return null;
        }

        private void doAdd(DataRow uprow)
        {
            int rate_type = Convert.ToInt32(uprow["CurrencyType"]);
            DataTable table = getTable(rate_type);
            table.Rows.Add(uprow);

            if (dtsrc != null & dttgt != null)
            {
                short tgt_curr = Convert.ToInt16(uprow["TargetCurrency"]);
                short src_curr = Convert.ToInt16(uprow["SourceCurrency"]);
                DataRow[] src_row = dtsrc.Select("fldValue = " + tgt_curr);
                if (src_row.Length > 0 && !comboExist(rate_type, tgt_curr, src_curr))
                {
                    DataRow revrow = table.NewRow();
                    revrow["BranchCode"] = uprow["BranchCode"];
                    revrow["TranxDate"] = uprow["TranxDate"];
                    revrow["SourceCurrency"] = uprow["TargetCurrency"];
                    revrow["TargetCurrency"] = uprow["SourceCurrency"];
                    revrow["SourceToTarget"] = uprow["TargetToSource"];

                    try
                    {
                        decimal d = Convert.ToDecimal(uprow["TargetToSource"]);
                        revrow["TargetToSource"] = Math.Round(1 / d, 9);
                    }
                    catch (Exception e)
                    {
                        Message = e.Message;
                        revrow["TargetToSource"] = 0.000000000M;
                    }

                    String brcode = uprow["BranchCode"].ToString();
                    if (brcode.Equals("1170") & rate_type == 1) // is_ord
                    {
                        revrow["MidSourceToTarget"] = uprow["MidTargetToSource"];
                        decimal md = Convert.ToDecimal(uprow["MidTargetToSource"]);
                        try
                        {
                            revrow["MidTargetToSource"] = Math.Round(1 / md, 9);
                        }
                        catch //(Exception e) // commented to remove warning
                        {
                            revrow["MidTargetToSource"] = 0.000000000M;
                        }
                    }

                    DateTime dtin = new irsTimeZone().branchLocalTime(brcode);
                    revrow["DateInserted"] = dtin;
                    revrow["CurrencyType"] = rate_type;
                    revrow["RatesMode"] = uprow["RatesMode"];
                    table.Rows.Add(revrow);
                }
            }
        }

        private void doEdit(int rate_type, int row)
        {
            //find the reverse
            DataTable table = getTable(rate_type);

            short tgt_curr = Convert.ToInt16(table.Rows[row]["TargetCurrency"]);
            short src_curr = Convert.ToInt16(table.Rows[row]["SourceCurrency"]);
            if (comboExist(rate_type, tgt_curr, src_curr))
            {
                DataRow[] dr = table.Select("SourceCurrency = " + tgt_curr + " AND TargetCurrency = " + src_curr);
                if (dr.Length > 0)
                {
                    decimal dsrc = Convert.ToDecimal(table.Rows[row]["SourceToTarget"]);

                    dr[0]["SourceToTarget"] = table.Rows[row]["TargetToSource"];
                    dr[0]["TargetToSource"] = table.Rows[row]["SourceToTarget"];
                    dr[0]["RatesMode"] = table.Rows[row]["RatesMode"];
                }
            }
        }

        private int find_row(Hashtable ht)
        {
            Object oid = ht["SEQ_ID"];
            int rate_type = Convert.ToInt32(ht["CurrencyType"]);
            DataTable table = getTable(rate_type);

            if (oid != null && (oid.ToString() == String.Empty | oid.ToString().Equals("0")))
            {
                short src_curr = Convert.ToInt16(ht["SourceCurrency"]);
                short tgt_curr = Convert.ToInt16(ht["TargetCurrency"]);

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i].RowState == DataRowState.Deleted) continue;
                    short tbl_src_curr = Convert.ToInt16(table.Rows[i]["SourceCurrency"]);
                    short tbl_tgt_curr = Convert.ToInt16(table.Rows[i]["TargetCurrency"]);
                    if (tbl_src_curr == src_curr & tbl_tgt_curr == tgt_curr) return i;
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

        private bool doDelete(Hashtable ht)
        {
            int rate_type = Convert.ToInt32(ht["CurrencyType"]);
            int selectedRow = -1;

            selectedRow = find_row(ht);
            if (selectedRow == -1)
            {
                Message = "Unable to find row to delete.";
                return false;
            }

            try
            {
                getTable(rate_type).Rows[selectedRow].Delete();
                setHasDelete(rate_type, true);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return false;
            }
            return true;
        }

        public bool applyUpdates(String crud, int row, Hashtable ht)
        {
            if (!ht.ContainsKey("CurrencyType"))
            {
                base.Message = "CurrencyType is missing in update parameters.";
                return false;
            }

            int rate_type = Convert.ToInt32(ht["CurrencyType"]);
            int selectedRow = -1;

            if (crud.Equals("DELETE"))
            {
                return doDelete(ht);
            }

            bool ok = false;
            DataRow arow = null;
            DataTable table = getTable(rate_type);

            if (crud.Equals("EDIT"))
            {
                selectedRow = find_row(ht);
                if (selectedRow == -1)
                {
                    Message = "Unable to find row to Edit.";
                    return false;
                }
                ok = true;
            }
            if (crud.Equals("ADD"))
            {
                short tgt_curr = Convert.ToInt16(ht["TargetCurrency"]);
                short src_curr = Convert.ToInt16(ht["SourceCurrency"]);
                if (!comboExist(rate_type, src_curr, tgt_curr))
                {
                    arow = table.NewRow();
                    ok = true;
                }
                else
                {
                    Message = "The SourceCurrency - TargetCurrency combination already exist.";
                    ok = false;
                }
            }
            if (ok)
            {
                foreach (Object key in ht.Keys)
                {
                    if (key.ToString().Equals(this.IdentityColumn)) continue;
                    if (crud.Equals("ADD"))
                    {
                        if (key.ToString().Equals("DateInserted"))
                        {
                            String brcode = ht["BranchCode"].ToString();
                            DateTime dtin = new irsTimeZone().branchLocalTime(brcode);
                            ok = base.setColumn(arow, key.ToString(), dtin);
                        }
                        else
                        {
                            ok = base.setColumn(arow, key.ToString(), ht[key]);
                        }
                    }
                    else ok = base.setColumn(table.Rows[selectedRow], key.ToString(), ht[key]);
                    if (!ok) break;
                }
                if (ok & crud.Equals("ADD"))
                {
                    doAdd(arow);
                }
                else if (ok & crud.Equals("EDIT"))
                {
                    doEdit(rate_type, selectedRow);
                }
            }
            return ok;
        }

        private bool detect_duplicat(DataSet ds)
        {
            int dups = 0;
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;
                    int seqid = Convert.ToInt32(row["SEQ_ID"]);
                    short fx_src = Convert.ToInt16(row["SourceCurrency"]);
                    short fx_tgt = Convert.ToInt16(row["TargetCurrency"]);
                    DataRow[] duprow = ds.Tables[0].Select("(SourceCurrency = " + fx_src + " AND TargetCurrency = " + fx_tgt + ") AND SEQ_ID <> " + seqid);
                    if (duprow.Length > 0) dups++;
                }
            }

            if (dups > 0) Message = "There are " + dups + "duplicate rows in this currency rates setup.";

            return (dups == 0);
        }

        public bool saveUpdates(int rate_type)
        {
            if (!dart.ContainsKey(rate_type))
            {
                Message = "Rate type: " + rate_type + " is not in the table collection.";
            }

            String sql = (String)dart[rate_type];

            if (sql != null && sql != String.Empty)
            {
                DataSet ds = (DataSet)dsrt[rate_type];
                if (detect_duplicat(ds))
                {
                    /* VAPT 20180222 */
                    int i = 0;
                    int j = 0;
                    string seqID = string.Empty;
                    string oldValue = string.Empty;
                    string newValue = string.Empty;
                    bool b = false;
                    DataTable dt = getTable(rate_type);
                    DataTable dtOld;

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (i = 0; i <= (dt.Rows.Count - 1); i++)
                        {
                            if (Convert.ToInt64(dt.Rows[i][0].ToString()) == 0)
                            {
                                break;
                            }
                            else
                            {
                                seqID += dt.Rows[i][0].ToString() + ", ";
                                b = true;
                            }
                        }
                    }

                    if (!b) //ADD
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (i = 0; i <= (dt.Rows.Count - 1); i++)
                            {
                                newValue = "BRANCHCODE=" + dt.Rows[i][1].ToString() + " ~ ";
                                newValue += "TRANXDATE=" + dt.Rows[i][2].ToString() + " ~ ";
                                newValue += "SOURCECURRENCY=" + new irsInternetPage().getLookupValue("Currency", "FldValue", dt.Rows[i][3].ToString(), "FldCode") + " ~ ";
                                newValue += "TARGETCURRENCY=" + new irsInternetPage().getLookupValue("Currency", "FldValue", dt.Rows[i][4].ToString(), "FldCode") + " ~ ";
                                newValue += "SOURCETOTARGET=" + dt.Rows[i][5].ToString() + " ~ ";
                                newValue += "TARGETTOSOURCE=" + dt.Rows[i][6].ToString() + " ~ ";
                                newValue += "DATEINSERTED=" + dt.Rows[i][7].ToString();
                                new csAuditLog().writeVAPTLogger("", "BOD", "CREATED RATES FOR THE DAY", "", newValue, "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                            }
                        }
                    }
                    else //EDIT
                    {
                        seqID = seqID.Substring(0, seqID.Length - 2); //Remove Ending Comma (,)
                        dtOld = this.getDataTable("SELECT * FROM TBL_CURRENCY_RATES WHERE SEQ_ID IN (" + seqID + ")");
                        if (dtOld != null && dt.Rows.Count > 0)
                        {
                            for (i = 0; i <= (dt.Rows.Count - 1); i++)
                            {
                                oldValue = string.Empty;
                                newValue = string.Empty;
                                for (j = 0; j <= (dtOld.Rows.Count - 1); j++)
                                {
                                    if (dt.Rows[i][0].ToString() == dtOld.Rows[j][0].ToString()) //SEQ_ID IS THE SAME
                                    {
                                        if (Convert.ToDouble(dt.Rows[i][5].ToString()).ToString("##,###.#########") != Convert.ToDouble(dtOld.Rows[j][5].ToString()).ToString("##,###.#########"))
                                        {
                                            oldValue += "SOURCECURRENCY=" + new irsInternetPage().getLookupValue("Currency", "FldValue", dtOld.Rows[i][3].ToString(), "FldCode") + " ~ ";
                                            oldValue += "TARGETCURRENCY=" + new irsInternetPage().getLookupValue("Currency", "FldValue", dtOld.Rows[i][4].ToString(), "FldCode") + " ~ ";
                                            oldValue += "SOURCETOTARGET=" + dtOld.Rows[i][5].ToString() + " ~ ";
                                            oldValue += "TARGETTOSOURCE=" + dtOld.Rows[i][6].ToString();

                                            newValue += "SOURCECURRENCY=" + new irsInternetPage().getLookupValue("Currency", "FldValue", dt.Rows[i][3].ToString(), "FldCode") + " ~ ";
                                            newValue += "TARGETCURRENCY=" + new irsInternetPage().getLookupValue("Currency", "FldValue", dt.Rows[i][4].ToString(), "FldCode") + " ~ ";
                                            newValue += "SOURCETOTARGET=" + dt.Rows[i][5].ToString() + " ~ ";
                                            newValue += "TARGETTOSOURCE=" + dt.Rows[i][6].ToString();
                                            new csAuditLog().writeVAPTLogger("", "BOD", "EDITED RATES FOR THE DAY", oldValue, newValue, "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    int ra = updateDataSet(sql, false, new Object[] { }, ds, this.TableName);

                    if (ra > -1)
                    {
                        setHasDelete(rate_type, false);
                    }

                    return (ra > -1);
                }
            }
            else
            {
                Message = "Unable to recreate query for rate: " + rate_type;
            }
            return false;
        }

        /* VAPT 20180222 */
        public DataTable getDataTable(string SQL)
        {
            DataSet ds = base.getDataSet(getSqlCommand(SQL, false));
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        public bool saveUpdates()
        {
            int rate_type = 0;
            int scount = 0;
            try
            {
                foreach (Object key in dsrt.Keys)
                {
                    rate_type = Convert.ToInt32(key);
                    if (hasChanges(rate_type))
                    {
                        if (!saveUpdates(rate_type))
                        {
                            scount++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return (scount == 0);
        }

        private void trim_table(int rate_type, ArrayList al)
        {
            DataTable dt = getTable(rate_type);

            foreach (Object o in al)
            {
                Object[] err_combo = (Object[])o;
                short src_curr = (short)err_combo[0];
                short tgt_curr = (short)err_combo[1];

                Hashtable ht = new Hashtable();
                ht.Add("SEQ_ID", 0);
                ht.Add("CurrencyType", rate_type);
                ht.Add("SourceCurrency", src_curr);
                ht.Add("TargetCurrency", tgt_curr);
                int row = find_row(ht);
                if (row > -1)
                {
                    dt.Rows[row].Delete();
                    setHasDelete(rate_type, true);
                }

                ht["SourceCurrency"] = tgt_curr;
                ht["TargetCurrency"] = src_curr;
                row = find_row(ht);
                if (row > -1)
                {
                    dt.Rows[row].Delete();
                    setHasDelete(rate_type, true);
                }
            }
        }

        public bool upload_cybermag_rates(DateTime dt, String brcode, bool over, int rate_type, bool is_ord)
        {
            if (!this.isUpdatable(rate_type))
            {
                this.getCurrencyRates(dt, brcode, rate_type, true);
            }

            DataTable table = getTable(rate_type);
            if (hasChanges(rate_type))
            {
                if (over) table.DataSet.RejectChanges();
                else table.DataSet.AcceptChanges();
            }
            if (table.Rows.Count > 0 & over)
            {
                foreach (DataRow row in table.Rows)
                {
                    row.Delete();
                    setHasDelete(rate_type, true);
                }
            }

            ArrayList err_combo = new ArrayList();

            while (true)
            {
                Object[] next_combo = this.getNextCombo(rate_type, is_ord);
                short src_curr = (short)next_combo[0];
                short tgt_curr = (short)next_combo[1];
                if (src_curr == -1 & tgt_curr == -1 | src_curr == 0 & tgt_curr == 0) break;
                decimal src2tgt = (decimal)next_combo[2];
                if (((String)next_combo[next_combo.Length - 1]).Length > 0)
                {
                    err_combo.Add(next_combo);
                }
                decimal mid_src2tgt = (is_ord & rate_type == 1) ? (decimal)next_combo[3] : 0.000000000M;
                decimal mid_tgt2src = 0.000000000M;

                if (src_curr == tgt_curr)
                {
                    src2tgt = 1.000000000M;
                }
                decimal tgt2src = 0.000000000M;
                try
                {
                    tgt2src = (src2tgt != 0) ? 1 / src2tgt : 0.000000000M;
                }
                catch (Exception e)
                {
                    Message = e.Message + " source: " + src_curr + " target: " + tgt_curr;
                    return false;
                }
                if (is_ord & rate_type == 1)
                {
                    if (src_curr == tgt_curr)
                    {
                        mid_src2tgt = 1.000000000M;
                    }
                    try
                    {
                        mid_tgt2src = (mid_src2tgt != 0) ? 1 / mid_src2tgt : 0.000000000M;
                    }
                    catch (Exception e)
                    {
                        Message = e.Message + "mid source: " + src_curr + " mid target: " + tgt_curr;
                        return false;
                    }
                }
                DataRow arow = table.NewRow();
                ArrayList al = new ArrayList();

                if (!setColumn(arow, "BranchCode", brcode)) al.Add("BranchCode: " + Message);
                if (!setColumn(arow, "TranxDate", dt)) al.Add("TranxDate: " + Message);
                if (!setColumn(arow, "SourceCurrency", src_curr)) al.Add("SourceCurrency: " + Message);
                if (!setColumn(arow, "TargetCurrency", tgt_curr)) al.Add("TargetCurrency: " + Message);
                if (!setColumn(arow, "SourceToTarget", src2tgt)) al.Add("SourceToTarget: " + Message);
                if (!setColumn(arow, "TargetToSource", tgt2src)) al.Add("TargetToSource:" + Message);
                DateTime dtin = new irsTimeZone().branchLocalTime(brcode);
                if (!setColumn(arow, "DateInserted", dtin)) al.Add("DateInserted:" + Message);
                if (!setColumn(arow, "CurrencyType", (short)rate_type)) al.Add("Rate type: " + Message);
                if (is_ord & rate_type == 1)
                {
                    if (!setColumn(arow, "MidSourceToTarget", mid_src2tgt)) al.Add("MidSourceToTarget: " + Message);
                    if (!setColumn(arow, "MidTargetToSource", mid_tgt2src)) al.Add("MidTargetToSource:" + Message);
                }
                if (!setColumn(arow, "RatesMode", 0)) al.Add("RatesMode: " + Message);
                if (al.Count > 0)
                {
                    Message = String.Empty;
                    StringBuilder sb = new StringBuilder();
                    foreach (Object o in al)
                    {
                        sb.Append(o.ToString() + "\n");
                    }
                    Message = sb.ToString();
                    return false;
                }
                doAdd(arow);
            }

            if (err_combo.Count > 0)
            {
                trim_table(rate_type, err_combo);
            }

            return true;
        }

        public irsCurrencyRates()
        {
            this.TableName = "tbl_Currency_Rates";
            this.IdentityColumn = "SEQ_ID";

            dsrt = new Hashtable();
            dart = new Hashtable();
            brcodes = new Hashtable();
        }
    }
}
