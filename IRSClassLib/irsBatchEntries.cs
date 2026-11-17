using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace IRSClassLib
{
    [Serializable]
    public class irsGRSBatchEntries : sqlHelperWs
    {

        #region PRIVATE VARIABLE        
        private string error_message            = null;
        private int _seq_id                  ;
        private int _noofitems;
        private int _curitems;
        private decimal  _running_amount;
        private decimal _total_amount;
        private string _batchnum;
        private int _fileformat;
        private int _filetype = 3;
        private int _ratetype;
        private int _batch_historyid;
        private string _formattedbatchnum;
        private int _batchnumlength;
        #endregion
        #region PROPERTY GET & SET

        public string messages
        {
            get { return this.error_message; }
        }
        public int p_filetype
        {
            get { return this._filetype ; }
        }
        public int p_batch_seq_id
        {
            get { return this._seq_id; }
            set { _seq_id = value; }
        }
        public int p_batch_historyid
        {
            get { return this._batch_historyid; }
            set { _batch_historyid = value; }
        }
        public int p_noofitems
        {
            get { return this._noofitems; }
            set { _noofitems = value; }
        }
        public int p_curitems
        {
            get { return this._curitems; }
            set { _curitems = value; }
        }
        public decimal p_running_amount
        {
            get { return this._running_amount; }
            set { _running_amount = value; }
        }
        public decimal p_total_amount
        {
            get { return this._total_amount; }
            set { _total_amount = value; }
        }
        public string p_batch_no
        {
            get { return this._batchnum; }
            set { _batchnum = value; }
        }
        public int p_fileformat
        {
            get { return this._fileformat; }
            set { _fileformat = value; }
        }
        public int p_ratetype
        {
            get { return this._ratetype; }
            set { _ratetype = value; }
        }

        DateTime _transaction_date;

        public DateTime p_transaction_date
        {
            get { return this._transaction_date; }
            set {_transaction_date = value;}
        }
        public bool p_isbatch_complete
        {

            get { return ((this.p_noofitems - this.p_curitems) == 0) ? true : false; }
            
        }
        public string p_formattedbatchnum
        {

            get { return this. _formattedbatchnum; }
            set { this._formattedbatchnum = value; }

        }
        public int p_batchnumlength
        {

            get { return this._batchnumlength; }
            set { this._batchnumlength = value; }

        }
        #endregion
        #region PUBLIC / PRIVATE METHOD
        public irsGRSBatchEntries()
        {
            this.TableName = "TBL_IBATCH_BATCH_HISTORY";
            this.IdentityColumn = "SEQ_ID";
        }
        public irsGRSBatchEntries(string tbl_name)
        {
            this.TableName = tbl_name;
            this.IdentityColumn = "SEQ_ID";
        }
    
        public DataSet getdynamicsql(string sql)
        {   
             return base.getDataSet(base.getSqlCommand(sql,false),"TBLSQL");                
        }

        public DataSet load_lookup_values(DateTime tranxdate,string branch)
        {

            SqlCommand cmd = new SqlCommand();
            cmd = base.getSqlCommand("proc_load_dataentry_lookup", true);
            cmd.Parameters.Add("@v_tranxdate", SqlDbType.DateTime).Value = tranxdate;
            cmd.Parameters.Add("@v_branchcode", SqlDbType.VarChar ,10).Value = branch ;

            return base.getDataSet(cmd,"tbl_lookup");
        }
        public string  create_batch_header(DateTime tranxdate,string userid,int noofitems,
                                        decimal totalamount,string fileformat
                                        , string filetype,int ratetype)
        {

            int batch_seqid = this.get_batch_seq_id(fileformat, filetype);
            
            SqlCommand cmd = base.getSqlCommand("proc_prepare_batch_hdrdtl", true);
                       

            cmd.Parameters.Add("@v_tranxdate", SqlDbType.DateTime).Value = tranxdate;
            cmd.Parameters.Add("@v_userid", SqlDbType.VarChar, 10).Value = userid;
            cmd.Parameters.Add("@v_noofitems", SqlDbType.Int).Value = noofitems;
            cmd.Parameters.Add("@v_totalamount", SqlDbType.Decimal).Value = totalamount;
            cmd.Parameters.Add("@v_batchseq_id", SqlDbType.Int).Value = batch_seqid;            
            cmd.Parameters.Add("@v_ratetype", SqlDbType.Int).Value = ratetype;
            cmd.Parameters.Add("@v_newbatchnum", SqlDbType.VarChar ,10).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@v_errmsg", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@v_batch_historyid", SqlDbType.Int ).Direction = ParameterDirection.Output;
            string  batch_num = "0";


            int ra = base.sqlUpdate(cmd);
            try
            {
                if (cmd.Parameters["@v_errmsg"].Value.ToString().Length == 0)
                {
                    batch_num = cmd.Parameters["@v_newbatchnum"].Value.ToString();

                    this.p_batch_seq_id = batch_seqid;
                    this.p_batch_historyid = Convert.ToInt32(cmd.Parameters["@v_batch_historyid"].Value.ToString());
                    this.p_curitems = 0;
                    this.p_running_amount = 0;
                    this.p_noofitems = noofitems;
                    this.p_total_amount  = totalamount;
                    this.p_batch_no = batch_num;
                    this.p_fileformat = Convert.ToInt32(fileformat);
                    this.p_ratetype = ratetype;
                    this.p_transaction_date = tranxdate;
                    this.load_to_session();
                    
                    return batch_num;
                }
                
            }
            catch
            {
                return batch_num;
            }
            return batch_num;
        }

        public void load_to_session()
        {
            System.Web.HttpContext.Current.Session["IBATCH_HEADER"] = this;
        }

        /*
        public bool load_data(DataTable dt)
        {
            try
            {
                this.p_batch_no         = dt.Rows[0]["batch_num"].ToString();
                this.p_batch_seq_id     = Convert.ToInt32(dt.Rows[0]["batch_setup_seqid"].ToString());
                this.p_curitems         = Convert.ToInt32(dt.Rows[0]["curr_items"].ToString());
                this.p_running_amount   = Convert.ToDecimal(dt.Rows[0]["curr_amount"].ToString());
                this.p_total_amount     = Convert.ToDecimal(dt.Rows[0]["batch_amount"].ToString());
                this.p_noofitems        = Convert.ToInt32(dt.Rows[0]["batch_items"].ToString());
                this.p_transaction_date = Convert.ToDateTime(dt.Rows[0]["TranxDate"].ToString());

                return true;
            }
            catch (Exception e)
            {

            }
            return false;
        }
        */
 
        public int get_batch_seq_id(string fileformat, string filetype)
        {
            string sql = "Select seq_id,BatchNumLength from tbl_batch_setup nolock  where filetype = "+ filetype +" and fileformat = " + fileformat ;
            DataSet ds = new DataSet();
            try
            {
                
                ds = base.getDataSet(base.getSqlCommand(sql, false), "tbl_bs");
                this._batchnumlength =Convert.ToInt32( ds.Tables[0].Rows[0]["BatchNumLength"].ToString());
                return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                return -1;
            }
            finally
            {
                ds.Dispose();
                ds = null;
            }
        }

        public int get_batch_fileformat(int batch_setup_seqid)
        {
            string sql = "Select top 1  fileformat from tbl_batch_setup where seq_id = " + batch_setup_seqid ;

            return Convert.ToInt32(base.getDataSet(base.getSqlCommand(sql, false), "tbl_bs").Tables[0].Rows[0][0].ToString());
        }


        public int get_batch_filetype(int batch_setup_seqid)
        {
            string sql = "Select top 1  filetype from tbl_batch_setup where seq_id = " + batch_setup_seqid;

            return Convert.ToInt32(base.getDataSet(base.getSqlCommand(sql, false), "tbl_bs").Tables[0].Rows[0][0].ToString());
        }

        public DataSet getBatchInfo(DateTime tranxdate,string batchnum, string fileformat ,string filetype,string userid)
        {
            int batchseqid = this.get_batch_seq_id(fileformat, filetype);
            SqlCommand cmd = base.getSqlCommand("proc_getbatchinfo");

            cmd.Parameters.Add("@v_tranxdate", SqlDbType.DateTime ).Value = tranxdate;
            cmd.Parameters.Add("@v_batchseqid", SqlDbType.Int).Value = batchseqid;
            cmd.Parameters.Add("@v_batchnum", SqlDbType.VarChar ,10).Value = batchnum ;
            cmd.Parameters.Add("@v_userid", SqlDbType.VarChar , 50).Value = userid  ;
            cmd.Parameters.Add("@v_errmsg", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;

            return base.getDataSet(cmd, "tbl_batchinfo");

        }

        public DataSet get_loaded_batch(DateTime tranxdate, int batchnum, int batchseq_id, string userid)
        {
         
            SqlCommand cmd = base.getSqlCommand("proc_getloadedbatch");

            cmd.Parameters.Add("@v_tranxdate", SqlDbType.DateTime).Value = tranxdate;
            cmd.Parameters.Add("@v_batchseqid", SqlDbType.Int).Value = batchseq_id;
            cmd.Parameters.Add("@v_batchnum", SqlDbType.Int).Value = batchnum;
            cmd.Parameters.Add("@v_userid", SqlDbType.VarChar, 50).Value = userid;
            cmd.Parameters.Add("@v_errmsg", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;

            return base.getDataSet(cmd, "tbl_batchinfo");

        }
        public bool setBatchInfo(DateTime tranxdate, string batchnum, string fileformat, string filetype,string userid)
        {
            int batchseqid = this.get_batch_seq_id(fileformat, filetype);
            
            SqlCommand cmd = base.getSqlCommand("proc_getbatchctr");
            DataSet ds     = new DataSet();

            try
            {
                cmd.Parameters.Add("@v_tranxdate", SqlDbType.DateTime).Value = tranxdate;
                cmd.Parameters.Add("@v_batchseqid", SqlDbType.Int).Value = batchseqid;
                cmd.Parameters.Add("@v_batchnum", SqlDbType.VarChar ,10).Value = batchnum;
                cmd.Parameters.Add("@v_userid", SqlDbType.VarChar, 20).Value = userid;
                cmd.Parameters.Add("@v_errmsg", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;

                ds = base.getDataSet(cmd,"tblBatch");

                if (cmd.Parameters["@v_errmsg"].Value.ToString().Length == 0)
                {
                    this.p_batch_no = batchnum ;
                    this.pad_batchnum(Convert.ToInt32(this.p_batch_no));

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        this.p_curitems       = Convert.ToInt32(ds.Tables[1].Rows[0]["curritems"].ToString());
                        this.p_running_amount = Convert.ToDecimal(ds.Tables[1].Rows[0]["runamt"].ToString());
                    }
                    this.p_batch_historyid  = Convert.ToInt32(ds.Tables[0].Rows[0]["Seq_id"].ToString());
                    this.p_transaction_date = Convert.ToDateTime(ds.Tables[0].Rows[0]["TranxDate"].ToString());
                    this.p_total_amount     = Convert.ToDecimal(ds.Tables[0].Rows[0]["batch_amount"].ToString());
                    this.p_noofitems        = Convert.ToInt32(ds.Tables[0].Rows[0]["batch_items"].ToString());
                    this.p_fileformat       = Convert.ToInt32(ds.Tables[0].Rows[0]["fileformat"].ToString());
                    this.p_ratetype         = Convert.ToInt32(ds.Tables[0].Rows[0]["ratetype"].ToString());
                    this.p_batch_seq_id     = batchseqid;
                    this.load_to_session(); return true;
                }
            }
            catch (Exception e)
            {
                this.error_message = e.Message;
            }

            return false;
        }

        public bool updateBatchInfo(int batch_setup_seqid, decimal amount_added)
        {
            int fileformat = this.get_batch_fileformat(batch_setup_seqid);
            int filetype   = this.get_batch_filetype(batch_setup_seqid);

            if (setBatchInfoTable(filetype, fileformat, this.p_noofitems, this.p_total_amount, this._transaction_date, this.p_batch_no, (this.p_curitems + 1), (this.p_running_amount + amount_added)))                            
            {
                return true;
            }

            return false;
        }

        public bool updateBatchInfo(int batch_setup_seqid, DateTime tranxdate, decimal amount_added)
        {
            int fileformat = this.get_batch_fileformat(batch_setup_seqid);
            int filetype   = this.get_batch_filetype(batch_setup_seqid);

            if (setBatchInfoTable(filetype, fileformat, this.p_noofitems, this.p_total_amount, tranxdate, this.p_batch_no, (this.p_curitems + 1), (this.p_running_amount + amount_added)))                            
            {
                return true;
            }

            return false ;
        }
        public bool setBatchInfoTable(  int filetype ,int fileformat,
                                        int noofitems,decimal totalamount,
                                        DateTime tranxdate,string batchnum,
                                        int curr_items,decimal curr_amount)
        {
            int batchseqid = this.get_batch_seq_id(fileformat.ToString(), filetype.ToString());
            SqlCommand cmd = base.getSqlCommand("proc_setbatchinfo");
            DataSet ds = new DataSet();
            try
            {
                cmd.Parameters.Add("@v_batchseqid", SqlDbType.Int).Value = batchseqid;
                cmd.Parameters.Add("@v_tranxdate", SqlDbType.DateTime).Value = tranxdate;                
                cmd.Parameters.Add("@v_batchnum", SqlDbType.VarChar ,10 ).Value = batchnum;
                cmd.Parameters.Add("@v_noofitems", SqlDbType.Int).Value = noofitems;
                cmd.Parameters.Add("@v_totalamt", SqlDbType.Decimal ).Value = totalamount;
                cmd.Parameters.Add("@v_curr_items", SqlDbType.Int ).Value = curr_items;
                cmd.Parameters.Add("@v_curr_amount", SqlDbType.Decimal).Value = curr_amount;
                cmd.Parameters.Add("@v_errmsg", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;

                int r = base.sqlUpdate(cmd);
                if (r != -1)
                {
                    if (cmd.Parameters["@v_errmsg"].Value.ToString().Length == 0)
                    {
                        this.p_batch_no = batchnum;
                        this.p_curitems = curr_items;
                        this.p_running_amount = curr_amount;
                        this.p_total_amount = totalamount;
                        this.p_noofitems = noofitems;
                        this.p_fileformat = fileformat;
                        this.p_batch_seq_id = batchseqid;
                        return true;
                    }
                }
                
            }
            catch 
            {
            }

            return false;
        }


        
        public void test_ord_batch()
        {
            string tdate = "10/05/2010";
            int noofitems = 1;
            decimal totalamount = 1000.00M;
            string userid = "sysadm1";            
            string fileformat = "1";
            string filetype = "3";
            int ratetype = 1;
            this.create_batch_header(Convert.ToDateTime(tdate), userid, noofitems, totalamount, fileformat ,filetype ,ratetype );

            System.Web.HttpContext.Current.Response.Redirect("../Remittance/RemOTC.aspx");

        }

        public bool isordbatchcomplete()
        {

            string userid = new irsClientData().UserId;
            try
            {
                this.setBatchInfo(this.p_transaction_date , this.p_batch_no ,this.p_fileformat.ToString(), this.p_filetype.ToString() , userid);

                return this.p_isbatch_complete;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
        #endregion
        #region PRIVATE METHODS
        private void pad_batchnum(int _batchnum)
        {
            this._formattedbatchnum = this.p_batch_no.ToString().PadLeft(Math.Abs(this.p_batchnumlength - _batchnum.ToString().Length), '0'); 
        }       
        #endregion

    }
}
