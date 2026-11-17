using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsCurrencyGroup : sqlHelperWs
    {
        public DataTable getBranches()
        {
            SqlCommand cmd = getSqlCommand("get_dropdown_branches");
            base.deriveParameters(cmd, new Object[] { });
            base.getUpdateDataSet(cmd);
            if (isUpdatable)
            {
                return table;
            }
            return null;
        }

        public DataTable GetGridValue(string brcode, string currgrp)
        {
            SqlCommand cmd = getSqlCommand("sp_grid_currencygroup");
            DataSet ds = base.execStoredProcedure(cmd, new object[] { brcode,currgrp });
            return ReturnValue(ds.Tables[0]);
        }

        public DataTable ReturnValue(DataTable dt)
        { 
            DataTable dtNew  = dt.Clone();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow urow = dtNew.NewRow();
                    urow["fldcode"] = dr["fldcode"];
                    urow["SEQ_ID"] = dr["SEQ_ID"];
                    urow["flddescription"] = dr["flddescription"];
                    urow["currency"] = dr["currency"];
                    urow["used"] = dr["used"];


                    //short used = 0;
                    //if (dt.Rows.Count > 0)
                    //{
                    //    DataRow[] trow = dt.Select("SEQ_ID = " + Convert.ToString(dr["SEQ_ID"]));
                    //    if (trow.Length > 0)
                    //    {
                    //        used = (short)trow[0]["used"];
                    //    }
                    //    urow["used"] = used;
                       dtNew.Rows.Add(urow);
                    //}
                }
   
            }
            return dtNew;
        }


        public DataTable GetCurrencyCategory(string brcode,string currgrp)
        {
            SqlCommand cmd = getSqlCommand("sp_getcurrencycategories");
            base.deriveParameters(cmd, new Object[] { brcode, currgrp });
            base.getUpdateDataSet(cmd);
            if (isUpdatable)
            {
                return table;
            }
            return null;
        }

        public bool applyUpdates(String crud, int row, Hashtable ht)
        {
            if (crud.Equals("DELETE"))
            {
                selectedRow = row;
                return base.Delete(sqlHelperWs.RETAIN_DATASET);
            }
            bool ok = false;
            if (crud.Equals("EDIT"))
            {
                selectedRow = row;
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

        public bool saveUpdates()
        {
            return base.save(sqlHelperWs.RETAIN_DATASET);
        }

        public bool save_currency_categories(Hashtable ht,string brcode,string currGrp)
        {
            try
            {
                string used = (string)ht["used"];
                int seq_id;
                //int seq_id = (Convert.ToString(ht["seq_id"])=="null") ? (short)0 : Convert.ToInt16(ht["seq_id"]);
                if (Convert.ToString(ht["seq_id"]) == "null" || Convert.ToString(ht["seq_id"]) == string.Empty)
                {
                    seq_id = 0;
                }
                else
                {
                    seq_id = Convert.ToInt16(ht["seq_id"]);
                }

                bool check = (Convert.ToString(ht["used"])=="true") ? true : false;
                if (brcode.Length > 0)
                {
                    base.getUpdateDS("SELECT * FROM tbl_currency_categories WHERE BranchCode='" + brcode + "' AND CurrencyGroup= '" + currGrp + "' AND Currency = '" + Convert.ToString(ht["currency"]) + "' ");
                    bool forUpdate = (table.Rows.Count > 0) ? true : false;
                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("seq_id"))
                            {
                                getselectedRow(seq_id);
                                if (forUpdate)
                                {
                                    base.setColumn("CurrencyGroup", currGrp);
                                    base.setColumn("Currency", Convert.ToString(ht["currency"]));
                                    base.setColumn("Checked", check);
                                }
                                else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    uprow["BranchCode"] = brcode;
                                    uprow["CurrencyGroup"] = currGrp;
                                    uprow["Currency"] = Convert.ToString(ht["currency"]);
                                    uprow["Checked"] = check;
                                    table.Rows.Add(uprow);
                                }
                            }
                        }
                        base.addMode = false;
                        return base.save(sqlHelperWs.RETAIN_DATASET);

                    }
                }
                else
                {
                    Message = "BranchCode is missing.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        public bool save_currency_categories_tmp(Hashtable ht, string brcode, string currGrp, string userID)
        {
            this.TableName = "tbl_currency_categories_tmp";
            this.IdentityColumn = "SEQ_ID";
            string sqlstmt = "SELECT * FROM tbl_currency_categories_tmp WHERE 1=0";
            base.getUpdateDS(sqlstmt);

            try
            {
                string used = (string)ht["used"];
                int seq_id;
                //int seq_id = (Convert.ToString(ht["seq_id"])=="null") ? (short)0 : Convert.ToInt16(ht["seq_id"]);
                if (Convert.ToString(ht["seq_id"]) == "null" || Convert.ToString(ht["seq_id"]) == string.Empty)
                {
                    seq_id = 0;
                }
                else
                {
                    seq_id = Convert.ToInt16(ht["seq_id"]);
                }

                bool check = (Convert.ToString(ht["used"]) == "true") ? true : false;
                if (brcode.Length > 0)
                {
                    bool forUpdate = (table.Rows.Count > 0) ? true : false;
                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("seq_id"))
                            {
                                getselectedRow(seq_id);
                                if (forUpdate)
                                {
                                    base.setColumn("CurrencyGroup", currGrp);
                                    base.setColumn("Currency", Convert.ToString(ht["currency"]));
                                    base.setColumn("Checked", check);
                                }
                                else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                                    uprow["BranchCode"] = brcode;
                                    uprow["CurrencyGroup"] = currGrp;
                                    uprow["Currency"] = Convert.ToString(ht["currency"]);
                                    uprow["Checked"] = check;
                                    uprow["MAIN_SEQ_ID"] = Convert.ToInt32(seq_id);
                                    uprow["Maker"] = userID;
                                    uprow["DateAdded"] = dateNow;
                                    uprow["CurrStatus"] = "For Approval";
                                    uprow["StatusDate"] = dateNow;
                                    uprow["ActionType"] = "EDIT";
                                    uprow["Module"] = "Branch Administration >> Currency Categories";
                                    uprow["MenuName"] = "Currency Categories";
                                    table.Rows.Add(uprow);
                                }
                            }
                        }
                        base.addMode = false;
                        return base.save(sqlHelperWs.RETAIN_DATASET);

                    }
                }
                else
                {
                    Message = "BranchCode is missing.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        public irsCurrencyGroup()
        {
            this.TableName = "tbl_currency_categories";
            this.IdentityColumn = "SEQ_ID";
        }

        private bool allow_delete(int seq_id)
        {
            DataView dv = table.DefaultView;
            dv.Sort = "seq_id";
            int i = dv.Find(new Object[] { seq_id });
            if (i > -1)
            {
                //dv[i]["allow_access"] = allow;
                selectedRow = i;
                return base.Delete(RETAIN_DATASET); 
            }
            return false;
        }

        private void getselectedRow(int seq_id)
        {
            DataView dv = table.DefaultView;
            dv.Sort = "SEQ_ID";
            int i = dv.Find(new Object[] { seq_id });
            if (i > -1)
            {
                //dv[i]["allow_access"] = allow;
                selectedRow = i;
            }
        }

    }
}
