using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsClientContacts_nWs : sqlHelper
    {
        #region PRIVATE VARIABLES

        private DataSet data_set = null;
        private string error_message = null;

        private string _seq_id = null;
        private string _cifid = null;
        private string _contact_type = null;
        private string _contact_no = null;
        private string _remarks = null;
        private string _sms_alert = null;
        private string _email_address = null;
        private string _preferred_no = null;
        public bool ins_flag = false;

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

        public string cifid
        {
            get { return this._cifid; }
            set { _cifid = value; }
        }

        public string contact_type
        {
            get { return this._contact_type; }
            set { contact_type = value; }
        }

        public string contact_no
        {
            get { return this._contact_no; }
            set { contact_no = value; }
        }

        public string remarks
        {
            get { return this._remarks; }
            set { _remarks = value; }
        }

        public string sms_alert
        {
            get { return this._sms_alert; }
            set { _sms_alert = value; }
        }

        public string preferred_no
        {
            get { return this._preferred_no; }
            set { _preferred_no = value; }
        }

        public string email_address
        {
            get { return this._email_address; }
            set { _email_address = value; }
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
                return s.Replace("'", "''");
            }

            return null;
        }

        public bool get_record(string seq_id)
        {
            base.IdentityValue = seq_id; return get_record();
        }

        public bool get_record()
        {
            this.getUpdateDS(Convert.ToInt32(base.IdentityValue));

            if (dataset != null && dataset.Tables[0].Rows.Count > 0)
            {
                base.selectedRow = 0;

                _seq_id = this.getRecord(base.getColumn("SEQ_ID"));
                _cifid = this.getRecord(base.getColumn("CIFID"));
                _contact_type = this.getRecord(base.getColumn("ContactType"));
                _contact_no = this.getRecord(base.getColumn("ContactNo"));
                _remarks = this.getRecord(base.getColumn("Remarks"));
                _sms_alert = this.getRecord(base.getColumn("SMSAlert"));
                _email_address = this.getRecord(base.getColumn("EmailAddress"));
                _preferred_no = this.getRecord(base.getColumn("PreferredNo"));

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
                    if (s[1] == null || s[1].Trim().Length == 0 || s[1].ToString().Trim().ToUpper() == "NULL" || s[1].ToString().Trim() == string.Empty)
                    {
                        s[1] = null;
                    }
                    if (!base.setColumn(s[0].Trim(), s[1]))
                    {
                        error_list.Add(s[0] + ":" + this.Message);
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

        public irsClientContacts_nWs()
        {
            this.TableName = "TBL_CIF_CONTACTS";
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
        }

        public override void getUpdateDS(int id)
        {
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_CIF_CONTACTS WHERE SEQ_ID = " + id, false);
            getUpdateDataSet(command);
        }

        public bool insert_record()
        {
            ins_flag = true;

            if (base.newRow() == true)
            {
                return true;
            }
            this.get_error_dbase(); return false;
        }

        public bool update_record()
        {
            ins_flag = false;

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

        public bool is_existing(string param_values)
        {
            string cont = this.count_record(param_values);

            sqlHelper commons = new sqlHelper();
            return Convert.ToBoolean(commons.execScalar("sp_cif_is_existing_contact", new string[] { ((cont.Split('&'))[0]).Trim(), ((cont.Split('&'))[1]).Trim(), ((cont.Split('&'))[2]).Trim() }));
        }

        public string count_record(string param_values)
        {
            string[] cols = param_values.Split('|');
            string cont_cifid = null;
            string cont_no = null;
            string email = null;

            for (int i = 0; i < cols.Length; i++)
            {
                string[] vals = cols[i].Split('~');

                if (vals[0].ToUpper() == "CIFID") cont_cifid = (cols[i].Split('~'))[1];
                if (vals[0].ToUpper() == "CONTACTNO") cont_no = (cols[i].Split('~'))[1];
                if (vals[0].ToUpper() == "EMAILADDRESS") email = (cols[i].Split('~'))[1];
            }

            return cont_cifid + '&' + cont_no + '&' + email;
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
