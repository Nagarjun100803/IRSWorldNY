using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    public class irsBatchMaint : sqlHelper
    {
        #region PRIVATE VARIABLE

        private DataSet data_set        = null;
        private string error_message    = null;
        private string _seq_id          = null;
        private string _ddlFileType     = null;
        private string _ddlFileFormat   = null;
        private string _ddlTargetTable  = null;
        private string _ddlTargetField  = null;
        private string _ddlSourceField  = null;
        private string _ddlCIFClass     =null;

        #endregion

        #region GET SET PROPERTY

        public string messages
        {
            get { return this.error_message; }
        }

        public string seq_id
        {
            get { return this._seq_id; }
            set { _seq_id = value; }
        }

        public string FileType
        {
            get { return this._ddlFileType; }
            set {_ddlFileType = value; }
        }

        public string FileFormat
        {
            get { return this._ddlFileFormat; }
            set { _ddlFileFormat = value; }
        }

        public string TargetTable
        {
            get { return this._ddlTargetTable; }
            set { _ddlTargetTable = value; }
        }

        public string TargetField
        {
            get { return this._ddlTargetField; }
            set { _ddlTargetField = value; }
        }

        public string SourceField
        {
            get { return this._ddlSourceField; }
            set { _ddlSourceField = value; }
        }

        public string CIFClass
        {
            get { return this._ddlCIFClass; }
            set { _ddlCIFClass = value; }
        }
        #endregion

        #region PUBLIC METHOD
        public DataSet getdataRecord()
        {
            if (this.dataset != null)
            {
                return this.data_set;
            }
            return null;
        }

        public string getRecord(object obj)
        {
            if (obj != null)
            {
                string s = Convert.ToString(obj);

                if (s != null && s.Trim() == string.Empty)
                {
                    return null;
                }
                return s.Replace("'","''");
            }
            return null;
        }
        public bool get_record(string SEQ_ID)
        {
            base.IdentityValue = SEQ_ID; return get_record();
        }

        public bool get_record()
        {
            this.getUpdateDS(Convert.ToInt32(base.IdentityValue));

            if (dataset != null && dataset.Tables[0].Rows.Count > 0)
            {
                base.selectedRow = 0;
                _seq_id         = this.getRecord(base.getColumn("fld_seqID"));
                _ddlFileType    = this.getRecord(base.getColumn("fileUploadType"));
                _ddlFileFormat  = this.getRecord(base.getColumn("FileFormat"));
                _ddlTargetTable = this.getRecord(base.getColumn("fld_targettable"));
                _ddlTargetField = this.getRecord(base.getColumn("fld_targetfield"));
                _ddlSourceField = this.getRecord(base.getColumn("fld_sourcefield"));
                _ddlCIFClass    = this.getRecord(base.getColumn("CIFClass"));

                this.data_set   = dataset; return true;
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
                        if (base.setColumn(s[0].Trim(), s[1]))
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

        public irsBatchMaint()
        {
            this.TableName = "TBL_FILEMAPPER_REM";
            this.IdentityColumn = "fld_seqID";
            this.error_message = null;
        }

        public override void getUpdateDS(int id)
        {
            SqlCommand command = getSqlCommand("SELECT fld_seqID ,fileUploadType ,FileFormat,fld_targettable  ,fld_targetfield ,fld_sourcefield,CIFClass FROM TBL_FILEMAPPER_REM WHERE fld_seqID = " + id, false);
            getUpdateDataSet(command);
        }

        public bool insert_record()
        {
            if (base.newRow() == true)
            {
                return true;
            }
            this.get_error_dbase(); return false;
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
        public bool delete_record()
        {
            base.IdentityValue = this.seq_id;

            if (base.Delete(Convert.ToBoolean(base.IdentityValue)) == true)
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
        public void rollback_record()
        {
            base.cancel();
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
