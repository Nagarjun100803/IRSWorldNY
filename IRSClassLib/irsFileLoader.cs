using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    public class irsFileLoader : sqlHelperWs
    {
        #region PRIVATE VARIABLES
        private DataSet data_set                = null;
        private string error_message            = null;
        private string _seq_id                  = null;
        private string _FileUploadType          = null;
        private string _FileFormat              = null;
        private string _FileSequenceNo          = null;
        private string _Delimiter               = null;
        private string _RecordField             = null;
        private string _RecordPosition          = null;
        private string _RecordLength            = null;
        private string _TempTableField          = null;
        private string _DestinationTable        = null;
        private string _DestinationTableField   = null;        
        private string _CIFClass                = null;
        //private string _DataType                = null;
        //private string _DataLength              = null;
        private string _StartAtLine             = null; 
        private string _SplitChar               = null;
        private string _SplitIndex              = null;
        private string _InvalidCharacters       = null;
        private string _Condition1              = null;
        private string _ConditionPosition1      = null;
        private string _Operator1               = null;
        private string _Condition2              = null;
        private string _ConditionPosition2      = null;
        private string _Operator2               = null;
        private string _Condition3              = null;
        private string _ConditionPosition3      = null;
        private string _DateCreated             = null;
        private string _DateLastModified        = null;
        

        #endregion

        #region PROPERTY GET & SET

        public string messages
        {
            get { return this.error_message; }
        }

        public string seq_id
        {
            get { return this._seq_id; }
            set { _seq_id = value; }
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

                _seq_id                 = this.getRecord(base.getColumn("SEQ_ID"));
                _FileUploadType         = this.getRecord(base.getColumn("FileUploadType"));
                _FileFormat             = this.getRecord(base.getColumn("FileFormat"));
                _FileSequenceNo         = this.getRecord(base.getColumn("FileSequenceNo"));
                _Delimiter              = this.getRecord(base.getColumn("Delimiter"));
                _RecordField            = this.getRecord(base.getColumn("RecordField"));
                _RecordPosition         = this.getRecord(base.getColumn("RecordPosition"));
                _RecordLength           = this.getRecord(base.getColumn("RecordLength"));
                _TempTableField         = this.getRecord(base.getColumn("TempTableField"));
                _DestinationTable       = this.getRecord(base.getColumn("DestinationTable"));
                _DestinationTableField  = this.getRecord(base.getColumn("DestinationTableField"));
                _CIFClass               = this.getRecord(base.getColumn("CIFClass"));
                //_DataType               = this.getRecord(base.getColumn("DataType"));
                //_DataLength             = this.getRecord(base.getColumn("DataLength"));
                _StartAtLine            = this.getRecord(base.getColumn("StartAtLine"));
                _SplitChar              = this.getRecord(base.getColumn("SplitChar"));
                _SplitIndex             = this.getRecord(base.getColumn("SplitIndex"));
                _InvalidCharacters      = this.getRecord(base.getColumn("InvalidCharacters"));
                _Condition1             = this.getRecord(base.getColumn("Condition1"));
                _ConditionPosition1     = this.getRecord(base.getColumn("ConditionPosition1"));
                _Operator1              = this.getRecord(base.getColumn("Operator1"));
                _Condition2             = this.getRecord(base.getColumn("Condition2"));
                _ConditionPosition2     = this.getRecord(base.getColumn("ConditionPosition2"));
                _Operator2              = this.getRecord(base.getColumn("Operator2"));
                _Condition3             = this.getRecord(base.getColumn("Condition3"));
                _ConditionPosition3     = this.getRecord(base.getColumn("ConditionPosition3"));
                _DateCreated            = this.getRecord(base.getColumn("DateCreated"));
                _DateLastModified       = this.getRecord(base.getColumn("DateLastModified"));

                this.data_set = dataset; return true;
            }
            return false;
        }

        public bool set_record(string[] column_values)
        {
            ArrayList error_list = new ArrayList();

            string[] splitstring = new string[] { "*&^#" };

            for (int i = 0; i < column_values.Length; i++)
            {

                string[] s = column_values[i].Split(splitstring, StringSplitOptions.None);
                //string[] s = column_values[i].Split('~');

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

        public irsFileLoader()
        {
            this.TableName = "TBL_FILE_DEFINITION_MAPPER";
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
        }

        public override void getUpdateDS(int id)
        {
            SqlCommand fileloadercommand = getSqlCommand("SELECT * FROM TBL_FILE_DEFINITION_MAPPER WHERE SEQ_ID = " + id, false);
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
