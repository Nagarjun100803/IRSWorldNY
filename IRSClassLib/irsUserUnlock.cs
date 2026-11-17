using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsUserUnlock : sqlHelperWs
    {

        private Object[] search_args;
        private DataSet search_ds;

        public DataTable getUserFlags()
        {
            DataTable dt = getLookup(new Object[] { "User Flags", DBNull.Value, -1 });
            if (dt != null)
            {
                DataTable dtclone = dt.Clone();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    String s = dt.Rows[i]["FldCode"].ToString().ToLower();
                    if (s.Equals("active") | s.Equals("disabled"))
                    {
                        DataRow crow = dtclone.NewRow();
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            crow[j] = dt.Rows[i][j];
                        }
                        dtclone.Rows.Add(crow);
                    }
                }

                return dtclone;
            }

            return null;
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

        private void doSearch()
        {
            if (search_args == null)
            {
                Message = "Set search_args before search.";
                search_ds = null;
                return;
            }

            search_ds = getDataSet("sp_SearchUsers", true, search_args);
        }

        public DataTable search(String args, String brcode, int flag, int logged)
        {
            try
            {
                /* VAPT 20180801 */
                string userID = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
                search_args = new Object[] { 0, args, brcode, 0, (short)flag, (short)logged, userID };
                doSearch();

                if (search_ds != null && search_ds.Tables.Count > 0)
                {
                    return search_ds.Tables[0];
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return null;
        }

        public bool user_exist(String key, String value)
        {
            if (isUpdatable && hasRows && table.Rows.Count == 1)
            {
                String s = getColumn(key).ToString();
                if (s.Equals(value)) return true;
            }

            getUpdateDS("sp_getUser", new Object[] { key, value });

            if (hasRows)
            {
                selectedRow = 0;
                return true;
            }

            return false;
        }

        public bool applyUpdates(String crud, int row, Hashtable ht)
        {
            if (crud.Equals("DELETE") | crud.Equals("ADD"))
            {
                Message = "DELETE is not supported by this module.";
                return false;
            }

            bool ok = false;
            if (crud.Equals("EDIT"))
            {
                String userid = (String)ht["UserID"];
                if (user_exist("USERID", userid))
                {
                    if (base.edit())
                    {
                        ok = true;
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("Flags") | key.ToString().Equals("LoggedOn"))
                            {
                                if (!setColumn(key.ToString(), ht[key]))
                                {
                                    ok = false;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        ok = false;
                    }
                }
                else
                {
                    ok = false;
                }
            }


            return ok;
        }

        public DataTable saveUpdates()
        {
            if (base.save(sqlHelperWs.RETAIN_DATASET))
            {
                doSearch();
                if (search_ds != null) return search_ds.Tables[0];

            }
            return null;
        }

        public irsUserUnlock()
        {
            base.TableName = "TBL_Users";
        }
    }
}
