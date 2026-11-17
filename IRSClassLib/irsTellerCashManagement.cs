using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;



namespace IRSClassLib
{
    public class irsTellerCashManagement: sqlHelper
    {

        public DataTable getCashBreakdownTxn(string fieldCode)
        {                                                                                  
            SqlCommand sqcommand = getSqlCommand("sp_cash_breakdown_txn");
            sqcommand.Parameters.Add("@fieldCode", SqlDbType.VarChar).Value = fieldCode;
            base.getUpdateDataSet(sqcommand, new String[] { this.TableName });
            if (isUpdatable) return this.table;

            return null;

        }

        public DataTable getCashMgt(string fieldCode)
        {
            SqlCommand sqcommand = getSqlCommand("sp_cash_denomination");
            sqcommand.Parameters.Add("@fieldCode", SqlDbType.VarChar).Value = fieldCode;
            base.getUpdateDataSet(sqcommand, new String[] { this.TableName });
            if (isUpdatable) return this.table;

            return null;            
            
        }

         public bool applyUpdates(String crud, int row, Hashtable ht)
         {
             if (crud.Equals("DELETE"))
             {
                 selectedRow = row;
                 return base.Delete(sqlHelper.RETAIN_DATASET);
             }
             bool ok = false;
             if (crud.Equals("EDIT"))
             {
                 selectedRow = row;
                 ok = base.edit();
             }
             if (crud.Equals("ADD"))
             {
                 ok = base.newRow(sqlHelper.RETAIN_DATASET);
             }
             if (ok)
             {
                 foreach (Object key in ht.Keys)
                 {
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
             return base.save(sqlHelper.RETAIN_DATASET);
         }
                                    






         public irsTellerCashManagement()
        {
            this.TableName = "TBL_DENOMINATION_DETAIL";
            this.IdentityColumn = "SEQ_ID";
        }

    }

    
}
