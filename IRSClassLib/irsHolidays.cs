using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsHolidays : sqlHelperWs
    {

        public irsHolidays()
        {
            this.TableName = "tbl_holidays";
            this.IdentityColumn = "SEQ_ID";
        }

        public DataTable GetHolidays(string brcode)
        {
            SqlCommand cmd = getSqlCommand("sp_get_holidays");
            DataSet ds = base.execStoredProcedure(cmd, new object[] { brcode });
            return ds.Tables[0];
        }
        public DataSet GetBranches(string location)
        {
            sqlHelperWs commons = new sqlHelperWs();
            
            DataSet ds = commons.execStoredProcedure("get_dropdown_branches_byaccess", new object[] { location });  //commons.getDataSet(commons.getSqlCommand("get_dropdown_branches"), "tblBranches");
            return ds;
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


        public bool save_holidays(Hashtable ht,string brcode,bool deleted)
        {
            try
            {
                String holidaydate = (String)ht["HOL_DATE"];
                String day_of_week = (string)ht["DAYOFWEEK"];
                String description = (string)ht["DESCRIPTION"];
                bool recurring = ht["Recurring"].Equals("true") ? true:false;
                bool holiday = ht["HOLIDAY"].Equals("true") ? true : false;
                bool no_clearing = ht["NO_CLEAR"].Equals("true") ? true : false;

                if (holidaydate == string.Empty || holidaydate == null)
                {
                    Message = "Please input holiday date";
                    return false;
                }
                else if (description == string.Empty || description == null)
                {
                    Message = "Description must not be empty";
                    return false;
                }
                string[] get_indx_of_year = holidaydate.Split('/');


                day_of_week = (get_indx_of_year[2].Equals("xxxx")) ? "" : Convert.ToDateTime(holidaydate).DayOfWeek.ToString();
                int seq_id = ht["SEQ_ID"].Equals(string.Empty) ? 0 : Convert.ToInt32(ht["SEQ_ID"]);

                if (holidaydate.Length > 0)
                {
                    string sqlstmt = "SELECT * FROM TBL_HOLIDAYS WHERE SEQ_ID=" + seq_id + " ";
                    base.getUpdateDS(sqlstmt);
                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("SEQ_ID"))
                            {
                                if (table.Rows.Count > 0)
                                {
                                    selectedRow = getselectedRow(seq_id);
                                    base.setColumn("HOL_DATE", holidaydate);
                                    base.setColumn("DESCRIPTION", description);
                                    base.setColumn("Recurring", recurring);
                                    base.setColumn("NO_CLEAR", no_clearing);
                                    base.setColumn("HOLIDAY", holiday);
                                    base.setColumn("Deleted", deleted);
                                }
                                else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    uprow["BRANCHCODE"] = brcode;
                                    uprow["HOL_DATE"] = holidaydate;
                                    uprow["DAYOFWEEK"] = day_of_week;
                                    uprow["DESCRIPTION"] = description;
                                    uprow["Recurring"] = recurring;
                                    uprow["NO_CLEAR"] = no_clearing;
                                    uprow["HOLIDAY"] = holiday;
                                    uprow["Deleted"] = false;
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
                    Message = "Holiday Date Required.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        public bool save_holidays_totemp(Hashtable ht, string brcode, bool deleted,string userID,string actionType)
        {
            this.TableName = "TBL_HOLIDAYS_TMP";
            this.IdentityColumn = "SEQ_ID";
            try
            {
                String holidaydate = (String)ht["HOL_DATE"];
                String day_of_week = (string)ht["DAYOFWEEK"];
                String description = (string)ht["DESCRIPTION"];
                bool recurring = ht["Recurring"].Equals("true") ? true : false;
                bool holiday = ht["HOLIDAY"].Equals("true") ? true : false;
                bool no_clearing = ht["NO_CLEAR"].Equals("true") ? true : false;

                if (holidaydate == string.Empty || holidaydate == null)
                {
                    Message = "Please input holiday date";
                    return false;
                }
                else if (description == string.Empty || description == null)
                {
                    Message = "Description must not be empty";
                    return false;
                }
                string[] get_indx_of_year = holidaydate.Split('/');


                day_of_week = (get_indx_of_year[2].Equals("xxxx")) ? "" : Convert.ToDateTime(holidaydate).DayOfWeek.ToString();
                int seq_id = ht["SEQ_ID"].Equals(string.Empty) ? 0 : Convert.ToInt32(ht["SEQ_ID"]);

                if (holidaydate.Length > 0)
                {
                    string sqlstmt = "SELECT * FROM TBL_HOLIDAYS_TMP WHERE 1=0";
                    base.getUpdateDS(sqlstmt);
                    if (isUpdatable)
                    {
                        if(base.newRow(sqlHelperWs.RETAIN_DATASET))
                        {
                            string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                            uprow["BRANCHCODE"] = brcode;
                            uprow["HOL_DATE"] = holidaydate;
                            uprow["DAYOFWEEK"] = day_of_week;
                            uprow["DESCRIPTION"] = description;
                            uprow["Recurring"] = recurring;
                            uprow["NO_CLEAR"] = no_clearing;
                            uprow["HOLIDAY"] = holiday;
                            uprow["Deleted"] = false;
                            uprow["MAIN_SEQ_ID"] = seq_id;
                            uprow["Maker"] = userID;
                            uprow["DateAdded"] = dateNow;
                            uprow["CurrStatus"] = "For Approval";
                            uprow["StatusDate"] = dateNow;
                            uprow["ActionType"] = actionType;
                            uprow["Module"] = "Branch Administration >> Holidays";
                            uprow["MenuName"] = "Holidays";
                            table.Rows.Add(uprow);
                        }

                        base.addMode = false;
                        return base.save(sqlHelperWs.RETAIN_DATASET);
                    }
                }
                else
                {
                    Message = "Holiday Date Required.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        public void delete_holidays(int indx,string brcode)
        {
            base.getUpdateDS("sp_get_holidays", new Object[] { brcode });
            if (isUpdatable)
            {
                selectedRow = indx;
                base.setColumn("Deleted", true);
                base.save(sqlHelperWs.RETAIN_DATASET);
            }
        }


        private int getselectedRow(int seq_id)
        {
            DataView dv = table.DefaultView;
            dv.Sort = "SEQ_ID";
            int i = dv.Find(new Object[] { seq_id });
            if (i > -1)
            {
                //dv[i]["allow_access"] = allow;
                selectedRow = i;
            }
            return i;
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
    }
}
