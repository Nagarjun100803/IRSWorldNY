using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace IRSClassLib
{
    [Serializable]
    internal class irsPasswordArchive : sqlHelperWs
    {
        private String userid;
        private String unit;
        private short interval;

        internal bool deleteArchive(String userid)
        {
            try
            {
                return (base.sqlUpdate("DELETE FROM " + TableName +
                    " WHERE UserId = '" + userid + "'") > -1);
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        internal bool archivePassword(String new_pwd, out String errmsg)
        {
            errmsg = "";
            String pwd = irsUsers.getTriDes().Encrypt(new_pwd);
            try
            {
                if (isUpdatable)
                {
                    int i = find_row(pwd);

                    if (i > -1)
                    {
                        table.Rows[i]["DatePasswordChanged"] = DateTime.Now;
                    }
                    else
                    {
                        if (newRow(sqlHelperWs.RETAIN_DATASET))
                        {
                            uprow["UserId"] = this.userid;
                            uprow["DatePasswordChanged"] = DateTime.Now;
                            uprow["Password"] = pwd;
                        }
                        else
                        {
                            errmsg = Message;
                            return false;
                        }
                    }
                    /* VAPT 20180723 */
                    save(sqlHelperWs.RETAIN_DATASET);
                    errmsg = Message;
                    return true;
                }
            }
            catch (Exception e)
            {
                errmsg = e.Message;
            }
            return false;
        }

        private int find_row(String pwd)
        {
            for ( int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i]["Password"].ToString().Equals(pwd)) return i;
            }

            return -1;
        }

        internal bool isReusable(String userid, String pwd, out String errmsg)
        {
            this.userid = userid;
            errmsg = "";
            try
            {
                String pwd_crypt = irsUsers.getTriDes().Encrypt(pwd);
                base.getUpdateDS("sp_getPasswordArchive", new Object[] { userid, String.Empty });
                if (isUpdatable)
                {
                    if (table.Rows.Count == 0) return true;
                    int i = find_row(pwd_crypt);
                    /* VAPT 20180723 */
                    int j = 0;
                    int count = 0;
                    bool found = false;

                    if (i == -1)
                    {
                        return true;
                    }
                    else
                    {
                        if (table.Rows.Count <= interval) /* # of password archive is less than the interval */
                        {
                            for (j = 0; j <= (table.Rows.Count - 1); j++)
                            {
                                if (pwd == irsUsers.getTriDes().Decrypt(table.Rows[j]["Password"].ToString()))
                                {
                                    found = true;
                                    break;
                                }
                            }
                        }
                        else /* # of password archive is greater than the interval, user may use the first-in after the interval */
                        {
                            for (j = 0; j <= (table.Rows.Count - 1); j++)
                            {
                                count++;
                                if (pwd == irsUsers.getTriDes().Decrypt(table.Rows[j]["Password"].ToString()))
                                {
                                    if (count <= interval)
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                            }
                        }

                        if (found)
                        {
                            errmsg = "Password cannot be reused for " + interval + " " + unit + ". Please use another password.";
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errmsg =  e.Message;
            }

            return false;
        }

        private void getRetentionLowDate()
        {
            unit = null;
            interval = -1;
            DataTable dt = getLookup(new Object[] { "Password", "Reuse Period", -1 });
            if (dt != null && dt.Rows.Count > 0)
            {
                unit = (String)dt.Rows[0]["fldDescription"];
                interval = Convert.ToInt16(dt.Rows[0]["fldValue"]);
            }
            /* VAPT 20180723 */
            if (unit == null) unit = "Cycles";
            if (interval == -1) interval = 6;
        }

        internal irsPasswordArchive()
        {
            base.TableName = "Tbl_Password_Archive";
            base.IdentityColumn = "Seq_id";
            getRetentionLowDate();
        }
    }
}
