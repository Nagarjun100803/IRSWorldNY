using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    public class irsCashMgmtEntries:sqlHelper
    {
        public string error_message = string.Empty;

        #region "PRIVATE METHOD"

        private bool has_exceptions(int currency_id)
        {
            SqlCommand commander = this.getSqlCommand("sp_get_cash_txn_status",true);
            DataTable dt = this.getDataSet(commander, "TXN_STATUS").Tables[0];

            if (dt != null & dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["count_txn_stat"]) > 0)
                {
                    return false;
                }
            }

            return true;
        }


        #endregion


        #region "PUBLIC METHOD"

        public bool insert_currency_denomination_range(int currency_id)
        {            
            if (this.has_exceptions(currency_id) == true)
            {
                this.error_message = "UNABLE TO INSERT CASH MANAGEMENT RECORD. CASH MANAGEMENT EOT IS REQUIRED."; return false;
            }
            
            SqlCommand commander = this.getSqlCommand("sp_get_denomination_range", true); commander.Parameters.Add("@currency_id", SqlDbType.SmallInt).Value = currency_id;
            DataTable dt         = this.getDataSet(commander, "CURR_DOMAIN").Tables[0];

            this.error_message = string.Empty;

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    SqlCommand upcmd = this.getSqlCommand("SELECT * FROM TBL_DENOMINATION_DETAIL WHERE SEQ_ID = -1", false);
                    base.getUpdateDataSet(upcmd);

                    foreach (DataRow rows in dt.Rows)
                    {
                        base.uprow = base.upds.Tables[0].NewRow();
                        base.uprow["currency"]     = rows["currency_code"];
                        base.uprow["denomination"] = rows["currency_value"];
                        base.uprow["date_entered"] = rows["current_dates"];
                        base.uprow["txn_status"]   = rows["currency_txn_status"];
                        base.upds.Tables[0].Rows.Add(base.uprow);
                    }

                    bool returns =  base.save();

                    if (base.Message != null && base.Message.Trim().Length > 0)
                    {
                        if (returns == false)
                        {
                            this.error_message = base.Message;
                        }
                    }

                    return returns;
                }
            }

            return false;
        }

        #endregion



    }
}
