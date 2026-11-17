using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsMICCS : sqlHelperWs
    {
        public bool save_data(Hashtable ht, bool deleted)
        {
            try
            {
                string accountno = (string)ht["AccountNo"];
                string remitter_jpn = (string)ht["RemitterNameJPN"];                
                string remitter_conv = (string)ht["RemitterCIFID"];
                string amount = (string)ht["AmountReceived"];
                string tran_type = (string)ht["TransactionType"];
                int status = 0;
                DataSet ds = get_fld_value((string)ht["TransactionStatus"]);
                
                if (ds != null)
                {
                    status = Convert.ToInt32(ds.Tables[0].Rows[0]["FldValue"]);                    
                }
                
                int seq_id = ht["SEQ_ID"].Equals(string.Empty) ? 0 : Convert.ToInt32(ht["SEQ_ID"]);

                    string sqlstmt = "SELECT * FROM TBL_MICCS WHERE SEQ_ID= " + seq_id + "";
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
                                    base.setColumn("AccountNo", accountno);
                                    base.setColumn("RemitterNameJPN", remitter_jpn);
                                    base.setColumn("RemitterCIFID", remitter_conv);                                    
                                    base.setColumn("AmountReceived", amount);
                                    base.setColumn("TransactionType", tran_type);
                                    base.setColumn("Status", status);
                                }
                                else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    uprow["AccountNo"] = Convert.ToString(ht["AccountNo"]);
                                    uprow["RemitterNameJPN"] = Convert.ToString(ht["RemitterNameJPN"]);
                                    uprow["AmountReceived"] = Convert.ToString(ht["AmountReceived"]);
                                    uprow["RemitterCIFID"] = Convert.ToString(ht["RemitterCIFID"]);                                    
                                    uprow["TransactionType"] = Convert.ToString(ht["TransactionType"]);
                                    uprow["Status"] = Convert.ToString(ht["TransactionStatus"]);
                                    table.Rows.Add(uprow);
                                }
                            }
                        }
                        base.addMode = false;
                        return base.save(sqlHelperWs.RETAIN_DATASET);

                    }
                }
            
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }


        public DataSet get_fld_value(string status)
        {
            sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(commons.getSqlCommand("SELECT FldValue FROM TBL_LOOKUP WHERE FldGroup = 'MICCSStatus' AND FldDescription ='" + status + "'", false));
            return ds;
        }


        public bool saveUpdates()
        {
            return base.save(sqlHelperWs.RETAIN_DATASET);
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
    }
}
