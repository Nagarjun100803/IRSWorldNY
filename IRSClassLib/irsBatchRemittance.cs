using System;
using System.Collections.Generic;
using System.Text;
using System.Data; 
namespace IRSClassLib
{
    public class irsBatchRemittance : sqlHelper
    {
        int CIFTYPE = 1; //individual        
        StringBuilder Message;
        DataRow _drUpload;
        DataTable _dtRemittance = new DataTable();
        public irsBatchRemittance()
        {
            this.TableName = "TBL_REMITTANCE";
            this.IdentityColumn = "SEQ_ID";
        }
        ~irsBatchRemittance()
        {
            this._dtRemittance.Dispose();
            this._dtRemittance = null;
            this._drUpload = null;

        }
        public  string create_remittance(DataRow drUpload, DataTable dtRemittance)
        {

            this._drUpload = drUpload; // dt from uploaded file 
            this._dtRemittance = dtRemittance;

            if (!isUpdatable)
            {
                this.getRemittance();
            }
            try
            {
                base.newRow();

                foreach (DataRow r in this._dtRemittance.Rows)
                {
                    string mappername = r["fld_sourcedfield"].ToString();
                    Object val = this._drUpload[mappername];
                    if (!setColumn(r["fld_targetfield"].ToString(), val))
                    {

                    }
                }
                //bool addciftype = setColumn("CIFTYPE", CIFTYPE);

                if (base.addMode)
                {
                    base.table.Rows.Add(uprow);
                    base.addMode = false;
                }

                base.save();
                string cifid = this.getCIFID(base.IdentityValue);
                if (!cifid.Contains("ERROR"))
                {
                    return cifid;
                }
                return "";


            }
            catch (Exception e) { Message.Append("ERROR:" + e.Message); return ""; }
        }
        private string getCIFID(Object master_seqid)
        {
            try
            {
                DataRow row = base.getDataSet(base.getSqlCommand("SELECT TOP 1 CIFID FROM TBL_CIF_MASTER WHERE SEQ_ID =" + master_seqid, false)).Tables[0].Rows[0];
                return row["CIFID"].ToString();
            }
            catch (Exception e)
            {
                Message.Append(e.Message);
                return "ERROR: " + e.Message;
            }
        }
        public void getRemittance()
        {
            base.getUpdateDataSet(base.getSqlCommand("SELECT TOP 1 * FROM TBL_REMITTANCE ", false));
        }
        public void do_transaction(string buff)
        {

        }

    }
}
