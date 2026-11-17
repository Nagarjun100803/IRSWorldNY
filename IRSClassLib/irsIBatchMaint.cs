using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    public class irsIBatchMaint : sqlHelper
    {
        #region PRIVATE VARIABLE

        private DataSet data_set                = null;
        private string error_message            = null;
        private string _seq_id                  = null;
        private string _ddclass                 = null;
        private string _type                  = null;
        private string _targetfield             = null;
        private string _sourcefield             = null;
        private string _fileformat              = null;
        private string _sqlcmd                  = null;
        private string _targettable             = null;

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

        public string ddclass
        {
            get { return this._ddclass ; }
            set { _ddclass  = value; }
        }

        public string type
        {
            get { return this._type; }
            set { _type = value; }
        }

        public string targetfield
        {
            get { return this._targetfield; }
            set { _targetfield = value; }
        }

        public string sourcefield
        {
            get { return this._sourcefield ; }
            set { _sourcefield = value; }

        }

        public string fileformat
        {
            get { return this._fileformat; }
            set { _fileformat = value; }

        }

        public string sqlcmd
        {
            get { return this._sqlcmd ; }
            set { _sqlcmd = value; }

        }

        public string targettable
        {
            get { return this._targetfield ; }
            set { _targetfield = value; }

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
                _ddclass        = this.getRecord(base.getColumn("CIFClass"));
                _type           = this.getRecord(base.getColumn("fileUploadType"));
                _targetfield    = this.getRecord(base.getColumn("fld_targetfield"));
                _sourcefield    = this.getRecord(base.getColumn("fld_sourcefield"));
                _fileformat     = this.getRecord(base.getColumn("FileFormat"));
                _sqlcmd         = this.getRecord(base.getColumn("fld_sqlcmd"));
                _targettable    = this.getRecord(base.getColumn("fld_targettable"));
          
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

		public irsIBatchMaint()
		{
            this.TableName = "TBL_FILEMAPPER_REM";
            this.IdentityColumn = "fld_seqID";
            this.error_message = null;
		}
         
		public override void getUpdateDS(int id)
		{
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_FILEMAPPER_REM WHERE fld_seqID = " + id, false);
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
