using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using GRSMQ;
using System.IO;
namespace IRSClassLib
{
    public class irsBatchTransaction : sqlHelper
    {
        //irsCalculator calc = new irsCalculator();
        DataSet _dsUpload = new DataSet();


        public void prepare_CIF(string row)
        {

        }
        private void compute_charges()
        {
        }
        private void write_to_remittance()
        {
        }
        private void write_invalid()
        {

        }
        private void write_transaction_ctr()
        {
        }
    }
    public class irsFileMapper : sqlHelper
    {
        public StringBuilder  Message;
        csFlexcubeBasicTeller fc = new csFlexcubeBasicTeller();
        int FileUploadType = 0;
        int FileFormat = 0;
        string filename;
        #region constructor
        public irsFileMapper(int iFileUploadType, int iFileFormat,string ifilename)
        {
            this.FileUploadType = iFileUploadType;
            this.FileFormat = iFileFormat;
            this.filename = ifilename;
        }
        #endregion
        #region PUBLIC METHOD

        public void load()
        {
            string str = "";
            DataTable dtupload = this.load_inputfile_to_datatable(str);
            DataTable dtmapper;
            //create CIF accounts
            irsCIFMaster mast = new irsCIFMaster();
            irsBatchCIFClass cifcls = new irsBatchCIFClass();
            irsBatchCIFAccounts cifacct = new irsBatchCIFAccounts();
            irsBatchCIFBeneficiaries cifbene; 
            //get remitter first
            foreach (DataRow r in dtupload.Rows )
            {
                string remid = mast.create_cif_master(r, this.get_mapper("TBL_CIF_MASTER",1),1);

                if (cifcls.create_cif_class(remid, 1))
                {
                }

                string beneid = mast.create_cif_master(r, this.get_mapper("TBL_CIF_MASTER", 2),2);
                if (cifcls.create_cif_class(beneid, 2))
                {
                    DataTable dt =  this.get_mapper("TBL_CIF_ACCOUNTS",2);
                    string acct = r[dt.Rows[0]["fld_sourcefield"].ToString()].ToString();


                    Hashtable ht = new Hashtable();

                    ht = validate_Accounts(acct);

                    string acct_type  = ht["ACCOUNTTYPE"].ToString() ;
                    string acct_curr = ht["CURRENCYCODE"].ToString() ;
                    string pnbr_cd = ht["BRANCHCODE"].ToString() ;

                    string cifacctseqid = cifacct.create_cif_accounts(beneid, acct, acct_type, pnbr_cd, acct_curr);

                    cifbene = new irsBatchCIFBeneficiaries(beneid, remid );

                    if (cifbene.create_cif_beneficiaries(cifacctseqid))
                    {

                    }

                }
               


            }


        }
        private void get_acctnum()
        {

        }
        public DataTable load_inputfile_to_datatable(string buff)
        {
            String filecontents;
            DataTable dt = new DataTable();
            DataTable dtUpload = new DataTable();
            try
            {
                dt = this.get_file_format();
                //filecontents = get_input_file(filename);
                //prepare header
                int ctr = 0;
                int startposition = 0;
                int columnlength = 0;
                string fld_name = string.Empty;
                foreach (DataRow r in dt.Rows)
                {
                    fld_name        =   (string)r["TempTableField"].ToString();                 
                    DataColumn column = new DataColumn();
                    column.DataType = System.Type.GetType("System.String");
                    column.AllowDBNull = true;
                    column.Caption = fld_name;
                    column.ColumnName = fld_name;
                    column.DefaultValue = ""; 
                    dtUpload.Columns.Add(column);
                }
                //add values
                DataRow row;
                row = dtUpload.NewRow();

                foreach (DataRow r in dt.Rows)
                {
                    fld_name = (string)r["TempTableField"].ToString();
                    startposition = (int)r["RecordPosition"];
                    columnlength = (int)r["RecordLength"];
                    row[fld_name] = buff.Substring(startposition, columnlength);
                }
                dtUpload.Rows.Add(row);
                return dtUpload;
            }
            catch (Exception e)
            {
                Message.Append(e.Message);
            }
            return null;
            
        }
        #endregion
        #region PRIVATE METHOD
        private DataTable get_mapper(string targettable,int cifclass)
        {

            try
            {
                String sql = "select * from TBL_FILEMAPPER_REM where fileUploadType = " + FileUploadType + "  and FileFormat = " + FileFormat + " and fld_targettable ='"+ targettable    +"' and cifclass =" + cifclass ;
                
                return base.getDataSet(base.getSqlCommand(sql, false)).Tables[0];
            }
            catch { return null; }
        }
        private DataTable create_generic_datatable(DataTable dt )
        {
            DataTable dtgeneric = new DataTable();

            foreach (DataColumn col in dt.Columns)
            {
                dtgeneric.Columns.Add(col);  
            }
            return dtgeneric;
        }
        private string get_input_file(string filename)
        {
            csFileIo io = new csFileIo();
            string path  = @"C:\IRS_LOADING\PCHC\";
            try
            {
                if (io.exist(path + filename))
                {
                    StreamReader sr = new StreamReader(path + filename);
                    return sr.ReadToEnd();                    
                }
                else
                {
                    Message.Append("File not found!");
                }
            }
            catch (Exception e)
            {
                Message.Append(e.Message );
            }
            finally 
            {
                io.close_input();
                io = null;
            }
            return null; 
        }
        private DataTable get_file_format()
        {
            
            try
            {
                String sql = "select * from tbl_file_definition_mapper where fileUploadType = " + FileUploadType + "  and FileFormat = " + FileFormat;
                return base.getDataSet(base.getSqlCommand(sql,false)).Tables[0];   
            }
            catch { return null; }
        }
        private DataSet get_value(string sqlFilter, string fldtoreturn,string tablename )
        {
            try
            {
                String sql = "select " + fldtoreturn  + " from " + table + " " + sqlFilter ;
                return base.getDataSet(base.getSqlCommand(sql, false));
            }
            catch { return null; }
        }

        private Hashtable validate_Accounts(string acct_num)
        {
            Hashtable ht = new Hashtable();
            try
            {
                string tempacct = acct_num.Trim();
                if (fc == null)
                {
                    fc = new csFlexcubeBasicTeller();
                }
                string errmsg = string.Empty;
                ht = fc.getAccountDetails(acct_num, out errmsg);
            }
            catch (Exception e)
            {
                Message.Append("ERRORs:" + e.Message);
            }
            return ht;

        }
        #endregion
    }
}
