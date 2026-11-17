using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace IRSClassLib
{
    [Serializable]
    public class irsGRSApproval : sqlHelper
    {
        #region PRIVATE VARIABLE
        private string error_message = null;
        private string _seq_id = null;
        private string _fldgroup = null;
        private string _fldbranchcode = null;
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

        public string fldgroup
        {
            get { return this._fldgroup; }
            set { _fldgroup = value; }
        }
        public string fldbranchcode
        {
            get { return this._fldbranchcode; }
            set { _fldbranchcode = value; }
        }
        #endregion
        #region PUBLIC / PRIVATE METHOD
        public irsGRSApproval()
        {
            this.TableName = "TBL_SUB_LOOKUP";
            this.IdentityColumn = "SEQ_ID";
        }
        public irsGRSApproval(string tbl_name)
        {
            this.TableName = tbl_name;
            this.IdentityColumn = "SEQ_ID";
        }

        public void getDataEntryHeaderTable()
        {
            base.getUpdateDataSet("select * from tbl_ibatch_crdr_hdr");
        }
        public string get_new_id()
        {
            return base.getDataSet(base.getSqlCommand("select newid()", false), "TSP").Tables[0].Rows[0][0].ToString();
        }
        public DataSet load_lookup_values(DateTime tranxdate, string branchcode)
        {
            SqlCommand cmd = base.getSqlCommand("proc_load_dataentry_lookup", true);
            cmd.Parameters.Add("@v_tranxdate", SqlDbType.DateTime).Value = tranxdate;
            cmd.Parameters.Add("@v_branchcode", SqlDbType.VarChar, 10).Value = branchcode;
            return base.getDataSet(cmd, "tbl_lookup_values");
        }
        public DataSet load_implementation(string prodcode, string branchcode)
        {
            string sql = "select 	distinct chargesseqid [VALUE], implementationtypedesc [CODE]" +
                        " from	v_tbl_charges_setup vw where	vw.branchcode	=	'" + branchcode + "'" +
                        " and	vw.productcode	= '" + prodcode + "'";


            return base.getDataSet(base.getSqlCommand(sql, false), "tbl_lookup_values");
        }
        public DataSet construct_batch_items(string guid, string userid, DateTime tranxdate, int fileformat, int ratetype, int totitems, decimal totrunamt)
        {
            SqlCommand cmd = base.getSqlCommand("proc_prepare_batch_hdrdtl", true);

            cmd.Parameters.Add("@v_tranxdate", SqlDbType.DateTime).Value = tranxdate;
            cmd.Parameters.Add("@v_userid", SqlDbType.VarChar, 10).Value = userid;
            cmd.Parameters.Add("@v_ratetype", SqlDbType.Int).Value = ratetype;
            cmd.Parameters.Add("@v_noofitems", SqlDbType.Int).Value = totitems;
            cmd.Parameters.Add("@v_RUNAMOUNT", SqlDbType.Decimal).Value = totrunamt;
            cmd.Parameters.Add("@v_guid", SqlDbType.VarChar, 150).Value = guid;
            cmd.Parameters.Add("@v_fileformat", SqlDbType.Int).Value = fileformat;
            cmd.Parameters.Add("@v_errmsg", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;

            return base.getDataSet(cmd, "tbl_hdrdtl");
        }


        public DataSet getCorrBanks()
        {
            string sql = "select Corr_Bank_Code,Corr_Bank_Name from tbl_Grs_corrbank";
            return base.getDataSet(base.getSqlCommand(sql, false), "TBLCORRBANKS");
        }
        public DataSet getdynamicsql(string sql)
        {
            return base.getDataSet(base.getSqlCommand(sql, false), "TBLSQL");
        }
        #endregion

    }
}