using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsClientMerchant_nWs : sqlHelper
    {
        #region PRIVATE VARIABLES

        private DataSet data_set = null;
        private string error_message = null;

        private string _seq_id = null;
        private string _cifid = null;
        private string _remitter_cifid = null;
        private string _mer_info_seq_id = null;
        private string _member_idno = null;
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

        public string remitter_cifid
        {
            get { return this._remitter_cifid; }
            set { _remitter_cifid = value; }
        }

        public string mer_info_seq_id
        {
            get { return this._mer_info_seq_id; }
            set { _mer_info_seq_id = value; }
        }

        public string member_idno
        {
            get { return this._member_idno; }
            set { _member_idno = value; }
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
                _remitter_cifid = this.getRecord(base.getColumn("RemitterCIFID"));
                _mer_info_seq_id = this.getRecord(base.getColumn("MerInfoSeqId"));
                _member_idno = this.getRecord(base.getColumn("MemberIDNo"));

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

        public irsClientMerchant_nWs()
        {
            this.TableName = "TBL_CIF_MERCHANTS";
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
        }

        public override void getUpdateDS(int id)
        {
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_CIF_MERCHANTS WHERE SEQ_ID = " + id, false);
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
            string merc = this.count_record(param_values);

            String sql = "SELECT COUNT(*) FROM " + TableName + " WHERE RemitterCIFID = '" + ((merc.Split('&'))[0]).Trim() + "' AND CIFID = '" + ((merc.Split('&'))[1]).Trim() + "'";
            sql += "AND MerInfoSeqId = '" + ((merc.Split('&'))[2]).Trim() + "'";
            sql += "AND (MemberIDNo = '" + ((merc.Split('&'))[3]).Trim() + "' OR MemberIDNo IS NULL)";
            sql += "AND ISNULL(Deleted, '') != 'True'";

            return (Convert.ToInt32(execScalar(sql)) > 0);
        }

        public string count_record(string param_values)
        {
            string[] cols = param_values.Split('|');
            string rem_cifid = null;
            string mer_cifid = null;
            string merinfoseq = null;
            string memidno = null;

            for (int i = 0; i < cols.Length; i++)
            {
                string[] vals = cols[i].Split('~');

                if (vals[0].ToUpper() == "REMITTERCIFID") rem_cifid = (cols[i].Split('~'))[1];
                if (vals[0].ToUpper() == "CIFID") mer_cifid = (cols[i].Split('~'))[1];
                if (vals[0].ToUpper() == "MERINFOSEQID") merinfoseq = (cols[i].Split('~'))[1];
                if (vals[0].ToUpper() == "MEMBERIDNO") memidno = (cols[i].Split('~'))[1];
            }

            return rem_cifid + '&' + mer_cifid + '&' + merinfoseq + '&' + memidno;
        }

        public bool getMerchanSeqId(string[] param)
        {
            try
            {
                this._seq_id = execScalar("SP_GET_MERCHANT_SEQ_ID", param).ToString();
                return true;
            }
            catch { }
            return false;
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
