using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{   
    public class irsGEFUMaintenance : sqlHelperWs 
    {
    #region PRIVATE VARIABLES
        private DataSet data_set                        = null;
        private string error_message                    = null;
        private string _SEQ_ID                          = null;
        private string _negbranchcode                   = null;
        private string _locationcode                    = null;
        private string _TranxSource                     = null;
        private string _ReceiptMode                     = null;
        private string _TranxCurrency                   = null;
        private string _ImplementationCurrency          = null;
        private string _TranxGLCode                     = null;
        private string _TranxDescription                = null;
        private string _ServiceChargeGLCode             = null; 
        private string _ServiceChargeDescription        = null;
        private string _ImplementationGLCode            = null;
        private string _ImplementationDescription       = null;
        private string _OPEGLCode                       = null;
        private string _OPEDescription                  = null;
        private string _DocStampGLCodes                 = null;
        private string _DocStampDescription             = null;
        private string _LocalCableChargesGLCode         = null;
        private string _LocalCableChargesDescription    = null;
        private string _LocalServiceFeeGLCode           = null;
        private string _LocalServiceFeeDescription      = null;
        private string _PRXGLCode                       = null;
        private string _MISCode                         = null;
        
    #endregion

    #region PROPERTY GET & SET

        public string messages
        {
            get { return this.error_message; }
        }

        public string seq_id
        {
            get { return this._SEQ_ID; }
            set { _SEQ_ID = value; }
        }

    #endregion

    #region PUBLIC METHOD

        public DataSet getdataRecord()
        {
            if (this.dataset != null)
            {
                return this.dataset;
            }
            return null;
        }

         public void rollback_record()
        {
            base.cancel();
        }

        public string getRecord(object obj)
        {
            if(obj != null)
            {
                string s = Convert.ToString (obj);

                if (s != null && s.Trim () == string.Empty)
                {
                    return null;
                }
                return s.Replace ("'","''");
            }
            return null;
        }

        public bool get_record(string SEQ_ID)
        { 
            base.IdentityValue = SEQ_ID; return get_record();    
        }

        public bool get_record()
        {
            this.getUpdateDS(Convert.ToInt32 (base.IdentityValue));

            if (dataset != null && dataset.Tables[0].Rows.Count >0)
            {
                base.selectedRow = 0;

                _SEQ_ID                         = this.getRecord(base.getColumn("SEQ_ID"));
                _negbranchcode                  = this.getRecord(base.getColumn("negbranchcode"));
                _locationcode                   = this.getRecord(base.getColumn("locationcode"));
                _TranxSource                    = this.getRecord(base.getColumn("TranxSource"));
                _ReceiptMode                    = this.getRecord(base.getColumn("ReceiptMode"));
                _TranxCurrency                  = this.getRecord(base.getColumn("TranxCurrency"));
                _ImplementationCurrency         = this.getRecord(base.getColumn("ImplementationCurrency"));
                _TranxGLCode                    = this.getRecord(base.getColumn("TranxGLCode"));
                _TranxDescription               = this.getRecord(base.getColumn("TranxDescription"));
                _ServiceChargeGLCode            = this.getRecord(base.getColumn("ServiceChargeGLCode"));
                _ServiceChargeDescription       = this.getRecord(base.getColumn("ServiceChargeDescription"));
                _ImplementationGLCode           = this.getRecord(base.getColumn("ImplementationGLCode"));
                _ImplementationDescription      = this.getRecord(base.getColumn("ImplementationDescription"));
                _OPEGLCode                      = this.getRecord(base.getColumn("OPEGLCode"));
                _OPEDescription                 = this.getRecord(base.getColumn("OPEDescription"));
                _DocStampGLCodes                = this.getRecord(base.getColumn("DocStampGLCodes"));
                _DocStampDescription            = this.getRecord(base.getColumn("DocStampDescription"));
                _LocalCableChargesGLCode        = this.getRecord(base.getColumn("LocalCableChargesGLCode"));
                _LocalCableChargesDescription   = this.getRecord(base.getColumn("LocalCableChargesDescription"));
                _LocalServiceFeeGLCode          = this.getRecord(base.getColumn("LocalServiceFeeGLCode"));
                _LocalServiceFeeDescription     = this.getRecord(base.getColumn("LocalServiceFeeDescription"));
                _PRXGLCode                      = this.getRecord(base.getColumn("PRXGLCode"));
                _MISCode                        = this.getRecord(base.getColumn("MISCode"));

                this.data_set = dataset; return true;
            }
            return false;
        }

        public bool set_record(string[] column_values)
        {
            ArrayList error_list = new ArrayList();

            for (int i = 0; i < column_values.Length; i++)
            {
                string[] s = column_values[i].Split('~');

                if (s != null && s.Length > 0)
                {
                    if (s[1] != null || s[1].Trim().Length > 0)
                    {
                        if (!base.setColumn(s[0].Trim(), s[1]))
                        {
                            error_list.Add(s[0] + ":" + this.Message);
                        }
                    }
                }
            }

            if (error_list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (Object errors in error_list)
                {
                    sb.Append((String)errors + "\n");
                }

                this.error_message = string.Empty;
                this.error_message = sb.ToString();

                return false;
            }

            return true;
        }

        public irsGEFUMaintenance()
        {
            this.TableName = "TBL_ACCOUNTINGENTRIES";
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
        }

        public override void getUpdateDS(int id)
        {
            SqlCommand fileloadercommand = getSqlCommand("SELECT * FROM TBL_ACCOUNTINGENTRIES WHERE SEQ_ID = " + id, false);
            getUpdateDataSet(fileloadercommand);
        }

        public bool insert_record()
        {
            if (base.newRow() == true)
            {
                return true;
            }
            this.get_error_dbase();
            return false;
        }

        public bool update_record()
        {
            base.IdentityValue = this.seq_id;

            if (base.edit(Convert.ToInt32(base.IdentityValue)) == true)
            {
                return true;
            }
            this.get_error_dbase(); return false;
        }

        public bool commit_record()
        {
            if (base.save() == true)
            {
                this.seq_id = base.IdentityValue.ToString(); return true;
            }
            this.get_error_dbase(); return false;
        }

    #endregion

    #region PRIVATE METHOD
        private void get_error_dbase()
        {
            this.error_message = string.Empty;

            if (base.Message != null && base.Message.Trim().Length > 0)
            {
                this.error_message = base.Message;
            }
        }
    #endregion

    }
}
