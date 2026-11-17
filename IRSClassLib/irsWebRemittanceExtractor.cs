using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.IO;
using PublicDomain;


namespace IRSClassLib
{
    public class irsWebRemittanceExtractor : sqlHelper
    {
        private string irsconn = new irsConfiguration().ConnectionStringNoRunMode("ConnectionString");
        private string ExtractType = new irsConfiguration().appsetting("ExtractType");
        private string FileFormat = new irsConfiguration().appsetting("FileFormat");
        private string rootpath = new irsConfiguration().appsetting("RootPath");
        private string temppath = new irsConfiguration().appsetting("RootPath");
        private string ArchivePath = new irsConfiguration().appsetting("ArchivePath");
        private string logpath = new irsConfiguration().appsetting("TempPath");

        private string service_class_code = new irsConfiguration().appsetting("ServiceClassCode");
        private string IRSW_WebRem_branchcode = new irsConfiguration().appsetting("WebRemittanceBranchCode");
        private string ImmediateDestination = new irsConfiguration().appsetting("ImmediateDestination");
        private string ImmediateDestinationName = new irsConfiguration().appsetting("ImmediateDestinationName");
        private string ImmediateOrigin = new irsConfiguration().appsetting("ImmediateOrigin");
        private string ImmediateOriginName = new irsConfiguration().appsetting("ImmediateOriginName");
        private string ImmediateOrigin_BankRoutingNo = new irsConfiguration().appsetting("BankRoutingNo");
        private string ReceivingDFI_GatewayOperator = new irsConfiguration().appsetting("ReceivingDFIGatewayOperator");
        private string ImmediateOriginAddress = new irsConfiguration().appsetting("ImmediateOriginAddress");
        private string ImmediateOriginCity = new irsConfiguration().appsetting("ImmediateOriginCity");
        private string ImmediateOriginState = new irsConfiguration().appsetting("ImmediateOriginState");
        private string ImmediateOriginCountry = new irsConfiguration().appsetting("ImmediateOriginCountry");
        private string ImmediateOriginPostalCode = new irsConfiguration().appsetting("ImmediateOriginPostalCode");
        private string OriginatingDFICountryCode = new irsConfiguration().appsetting("OriginatingDFICountryCode");
        private string ReceivingDFICountryCode = new irsConfiguration().appsetting("ReceivingDFICountryCode");

        private string ImmediateOriginCountryandPostalCode = "";
        private string ImmediateOriginCityandState = "";

        private string irs_filename = "";
        private string Summary_FileName = "";
                
        private DataTable irs_file_contents = null;
        private DataTable irs_file_contents_ALL = null;

        string RemitterCIFID = "";
        string RemitterName = "";
        string BeneficiaryCIFID = "";
        string BeneficiaryName = "";
        string RemitterAccount = "";
        string AccountType = "";
        string ClearingDate = "";
        string TargetAmount = "";
        string Same_Remitter_value = "";
        string Same_Remitter_value_Web = "";
        string TraceNo = "";
        string Summary_Entry = "";
        string RemitterBank_RoutingNo = "";
        string StandardEntryClass = "";
        string NettoBeneAmount = "";
        string RemitterAddress = "";
        string RemitterCity = "";
        string RemitterCountry = "";
        string BenefAddress = "";
        string BenefCity = "";
        string BenefCountry = "";
        string content_value_identifier = "";
        string ReceivingDFI_BankName = "";

        DateTime ClearingDate_value = System.DateTime.Now;
        DateTime batchdate = System.DateTime.Now;

        int Entry_detail = 0;
        int Batch_count = 0;
        int Batch_Number = 0;
        int Block_count = 0;
        int Batch_hash = 0;
        int File_hash = 0;
        int total_Entry_detail = 0;
        int total_Entry_detail_addenda = 0;
        int total_records = 0;
        int total_amount = 0;
        int total_amount_all_trxn = 0;
        int total_addenda_line_count = 0;

        bool Batch_Record_Created = false;

        public void Create_irsworld_WebRemittance_data()
        {
            try
            {
                logme("IRSW_WebRemittance FILE EXTRACTOR : starting to extract data : " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());

                //get WebRemittance Data
                if (ExtractType == "1") //Extract USW branch Transactions in 
                {
                    Extract_WebRemittance_Data();
                    Clear_All_Variables();
                }
                else if (ExtractType == "2") //Extract US branch Transactions except USW : RFS # Auto Extract of Transaction with Payment Type ACH into an ACH ready FEDLINE format
                {
                    Extract_WebRemittance_Data_ALL();
                    Clear_All_Variables();
                }
                else if (ExtractType == "3")//Extract both USW and other Branch Web Remittance Transaction
                {
                    Extract_WebRemittance_Data();
                    Clear_All_Variables();
                    Extract_WebRemittance_Data_ALL();
                    Clear_All_Variables();
                }
                else if (ExtractType == "4") //Extract USW branch WebRemittance Transactions in excel format for loading to Treasury Software
                {
                    Extract_WebRemittance_Data_Excel();
                }
                else
                {
                    logme("IRSW_WebRemittance FILE EXTRACTOR : No selected ExtractType. Check IRSWebRemittanceExtractor config. : " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());
                }
            }
            catch (Exception e)
            {
                irsFileExtracter.irsIRSFunction.logme("Create Extract File : " + e.Message + ":" + e.StackTrace);
            }
        }

        private string get_WebRemittance_TraceNumber(string refno)
        {
            //Get TEL and WEB clearing dates
            string traceno = "";
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            SqlConnection cn = new SqlConnection(this.irsconn);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "SELECT TRACENUMBER FROM TBL_REMITTANCE_WEBREMITTANCE WHERE REFERENCENO = '" + refno + "'";
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();

                ds = base.getDataSet(cmd);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    traceno = ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    traceno = "";
                }
            }
            catch (Exception e)
            {
                logme(e.Message + ":" + e.StackTrace);
            }
            finally
            {
                cn.Close();
                cn.Dispose();
                cmd.Dispose();
                dt = null;
                ds = null;
            }
            return traceno;
        }

        private void Extract_WebRemittance_Data_Excel()
        {
            try
            {
                if (this.get_irsworld_WebRemittance_data_excel() == true)
                {
                    //insert data to tbl_remittance_webRemittance
                    for (int i = 0; i < this.irs_file_contents.Rows.Count; i++)
                    {
                        this.insert_WebRemittance_Data_Excel(irs_file_contents.Rows[i]);
                    }
                    //create WebRemittance File
                    logme("IRSW_WebRemittance FILE EXTRACTOR USW : Done fetching data, creating ACH file ");
                    this.create_WebRemittance_Excel_file();

                    //FTP WebRemittance file to ACH
                    logme("IRSW_WebRemittance FILE EXTRACTOR USW : Done creating ACH file, ftp ACH file to FED / auto email receipients ");

                    /* ===================================================================================
                     * Comment out since Treasury works already has archive and FTP function. 10212013
                    this.ftp_irs_file();
                    logme("IRSW_WebRemittance FILE EXTRACTOR USW : Successfully loaded ACH file : " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());

                    Archive_File();
                    logme("IRSW_WebRemittance FILE EXTRACTOR USW : Successfully Archived ACH file : " + this.irs_filename + " " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());
                    ====================================================================================== */
                }
                else
                {
                    logme("IRSW_WebRemittance FILE EXTRACTOR USW : No ACH data to extract ");
                    return;
                }
            }
            catch (Exception e)
            {
                irsFileExtracter.irsIRSFunction.logme("Create Extract File USW : " + e.Message + ":" + e.StackTrace);
            }
            
        }

        private void Extract_WebRemittance_Data()
        {
            try
            {
                //Format Immediate Destionation and Origin fixed length
                int ImmediateOrigin_len = (ImmediateOrigin.Length < 10) ? ImmediateOrigin.Length : 10;
                if (ImmediateOrigin_len == 10)
                {
                    ImmediateOrigin = ImmediateOrigin.Substring(0, 10);
                }
                else
                {
                    ImmediateOrigin = ImmediateOrigin.PadLeft(10, ' ');
                }

                int ImmediateOriginName_len = (ImmediateOriginName.Length < 23) ? ImmediateOriginName.Length : 23;
                if (ImmediateOriginName_len == 23)
                {
                    ImmediateOriginName = ImmediateOriginName.Substring(0, 23);
                }


                int ImmediateDestination_len = (ImmediateDestination.Length < 10) ? ImmediateDestination.Length : 10;
                if (ImmediateDestination_len == 10)
                {
                    ImmediateDestination = ImmediateDestination.Substring(0, 10);
                }
                else
                {
                    ImmediateDestination = ImmediateDestination.PadLeft(10, ' ');
                }


                int ImmediateDestinationName_len = (ImmediateDestinationName.Length < 23) ? ImmediateDestinationName.Length : 23;
                if (ImmediateDestinationName_len == 23)
                {
                    ImmediateDestinationName = ImmediateDestinationName.Substring(0, 23);
                }

                if (this.get_irsworld_WebRemittance_data() == true)
                {
                    //insert data to tbl_remittance_WebRemittance
                    for (int i = 0; i < this.irs_file_contents.Rows.Count; i++)
                    {
                        this.insert_WebRemittance_Data(irs_file_contents.Rows[i]);
                    }

                    //create WebRemittance File
                    logme("IRSW_WebRemittance FILE EXTRACTOR USW : Done fetching data, creating ACH file ");
                    this.create_WebRemittance_file_text("USW", irs_file_contents);

                    //create WebRemittance Summary File
                    if (content_value_identifier != "")
                    {
                        logme("IRSW_WebRemittance FILE EXTRACTOR USW : Creating ACH summary file ");
                        this.create_Summary_file("USW");
                        //FTP WebRemittance file to ACH
                        logme("IRSW_WebRemittance FILE EXTRACTOR USW : Done creating ACH file, ftp ACH file to FED / auto email receipients ");
                        this.ftp_irs_file();
                        logme("IRSW_WebRemittance FILE EXTRACTOR USW : Deleting TEMP data");
                        logme("IRSW_WebRemittance FILE EXTRACTOR USW : Successfully loaded ACH file : " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());

                        Archive_File();
                        logme("IRSW_WebRemittance FILE EXTRACTOR USW : Successfully Archived ACH file : " + this.irs_filename + " " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());
                    }
                    this.delete_temp();
                }
                else
                {
                    logme("IRSW_WebRemittance FILE EXTRACTOR USW : No ACH data to extract ");
                    return;
                }
            }
            catch (Exception e)
            {
                irsFileExtracter.irsIRSFunction.logme("Create Extract File USW : " + e.Message + ":" + e.StackTrace);
            }

        }

        private void Extract_WebRemittance_Data_ALL()
        {
            try
            {
                //Format Immediate Destionation and Origin fixed length
                int ImmediateOrigin_len = (ImmediateOrigin.Length < 10) ? ImmediateOrigin.Length : 10;
                if (ImmediateOrigin_len == 10)
                {
                    ImmediateOrigin = ImmediateOrigin.Substring(0, 10);
                }
                else
                {
                    ImmediateOrigin = ImmediateOrigin.PadLeft(10, ' ');
                }

                int ImmediateOriginName_len = (ImmediateOriginName.Length < 23) ? ImmediateOriginName.Length : 23;
                if (ImmediateOriginName_len == 23)
                {
                    ImmediateOriginName = ImmediateOriginName.Substring(0, 23);
                }


                int ImmediateDestination_len = (ImmediateDestination.Length < 10) ? ImmediateDestination.Length : 10;
                if (ImmediateDestination_len == 10)
                {
                    ImmediateDestination = ImmediateDestination.Substring(0, 10);
                }
                else
                {
                    ImmediateDestination = ImmediateDestination.PadLeft(10, ' ');
                }


                int ImmediateDestinationName_len = (ImmediateDestinationName.Length < 23) ? ImmediateDestinationName.Length : 23;
                if (ImmediateDestinationName_len == 23)
                {
                    ImmediateDestinationName = ImmediateDestinationName.Substring(0, 23);
                }

                if (this.get_irsworld_WebRemittance_data_ALL() == true)
                {
                    //insert data to tbl_remittance_WebRemittance
                    for (int i = 0; i < this.irs_file_contents_ALL.Rows.Count; i++)
                    {
                        this.insert_WebRemittance_Data(irs_file_contents_ALL.Rows[i]);
                    }
                    //create WebRemittance File
                    logme("IRSW_WebRemittance FILE EXTRACTOR ALL : Done fetching data, creating ACH file ");
                    this.create_WebRemittance_file_text("ALL", irs_file_contents_ALL);
                    //create WebRemittance Summary File
                    if (content_value_identifier != "")
                    {
                        logme("IRSW_WebRemittance FILE EXTRACTOR ALL : Creating ACH summary file ");
                        this.create_Summary_file("ALL");
                        //FTP WebRemittance file to ACH
                        logme("IRSW_WebRemittance FILE EXTRACTOR ALL : Done creating ACH file, ftp ACH file to FED / auto email receipients ");
                        this.ftp_irs_file();
                        logme("IRSW_WebRemittance FILE EXTRACTOR ALL : Deleting TEMP data");
                        logme("IRSW_WebRemittance FILE EXTRACTOR ALL : Successfully loaded ACH file : " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());

                        Archive_File();
                        logme("IRSW_WebRemittance FILE EXTRACTOR ALL : Successfully Archived ACH file : " + this.irs_filename + " " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());
                    }
                    this.delete_temp();
                }
                else
                {
                    logme("IRSW_WebRemittance FILE EXTRACTOR ALL : No ACH data to extract ");
                    return;
                }
            }
            catch (Exception e)
            {
                irsFileExtracter.irsIRSFunction.logme("Create Extract File ALL : " + e.Message + ":" + e.StackTrace);
            }
        }

        private string Get_Branch_TimeZone()
        {
            SqlConnection cn_TimeZone = new SqlConnection(this.irsconn);
            SqlCommand cmd_TimeZone = new SqlCommand();
            DataTable dt_TimeZone = new DataTable();
            SqlDataAdapter da_TimeZone = new SqlDataAdapter();

            string TimeZone_date = "";

            try
            {
                cn_TimeZone.Open();
                cmd_TimeZone.Connection = cn_TimeZone;
                cmd_TimeZone.CommandText = "SELECT DISTINCT TOP 1 TIMEZONE FROM TBL_BRANCHES WHERE BRANCHCODE in (" + IRSW_WebRem_branchcode + ")";
                cmd_TimeZone.CommandType = CommandType.Text;
                cmd_TimeZone.CommandTimeout = 0;
                cmd_TimeZone.ExecuteNonQuery();
                
                da_TimeZone.SelectCommand = cmd_TimeZone;
                da_TimeZone.Fill(dt_TimeZone);
                
                if (dt_TimeZone.Rows.Count > 0)
                {
                    TimeZone_date = TzTimeZone.GetTimeZone(dt_TimeZone.Rows[0][0].ToString()).Now.DateTimeLocal.ToString("yyyy-MM-dd");
                }
                else
                {
                    TimeZone_date = "";
                }
            }
            catch (Exception e)
            {
                logme("IRSW_WebRemittance FILE EXTRACTOR - Get Branch TimeZone : " + e.Message + ":" + e.StackTrace);
            }
            finally
            {
                cn_TimeZone.Close();
                cn_TimeZone.Dispose();
                cmd_TimeZone.Dispose();
                dt_TimeZone = null;
                da_TimeZone = null;
            }

            return TimeZone_date;
        }

        private bool get_irsworld_WebRemittance_data_excel()
        {
            //Populate TEL and WEB Remittance from IRSWorld
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            SqlConnection cn = new SqlConnection(this.irsconn);
            SqlCommand cmd = new SqlCommand();

            //string date_value = Get_Server_Date();
            string date_value = Get_Branch_TimeZone();

            try
            {
                if (date_value != "")
                {
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandText = "proc_Get_WEBREMITTANCE_Data_Excel";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@TRANXDATE", SqlDbType.VarChar, 10).Value = date_value;
                    cmd.Parameters.Add("@ImmediateDestination", SqlDbType.VarChar, 9).Value = ImmediateDestination;
                    cmd.Parameters.Add("@Originator_Name", SqlDbType.VarChar, 360).Value = ImmediateOriginName;
                    cmd.Parameters.Add("@Originator_Street", SqlDbType.VarChar, 100).Value = ImmediateOriginAddress;
                    cmd.Parameters.Add("@Originator_City", SqlDbType.VarChar, 100).Value = ImmediateOriginCity;
                    cmd.Parameters.Add("@Originator_State", SqlDbType.VarChar, 100).Value = ImmediateOriginState;
                    cmd.Parameters.Add("@Originator_Country", SqlDbType.VarChar, 2).Value = ImmediateOriginCountry;
                    cmd.Parameters.Add("@Originator_Zip", SqlDbType.VarChar, 7).Value = ImmediateOriginPostalCode;
                    cmd.Parameters.Add("@ImmediateOrigin", SqlDbType.VarChar, 9).Value = ImmediateOrigin;
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();

                    ds = base.getDataSet(cmd);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        irs_file_contents = ds.Tables[0];
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    logme("IRSW_WebRemittance FILE EXTRACTOR - Get IRSWorld Web Remittance Data USW : No TimeZone Date set in USW branch.");
                    return false;
                }
            }
            catch (Exception e)
            {
                logme("IRSW_WebRemittance FILE EXTRACTOR - Get IRSWorld Web Remittance Data USW : " + e.Message + ":" + e.StackTrace);
                return false;
            }
            finally
            {
                cn.Close();
                cn.Dispose();
                cmd.Dispose();
                dt = null;
                ds = null;
            }
        }

        private bool get_irsworld_WebRemittance_data()
        {
            //Populate TEL and WEB Remittance from IRSWorld
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            SqlConnection cn = new SqlConnection(this.irsconn);
            SqlCommand cmd = new SqlCommand();

            //string date_value = Get_Server_Date();
            string date_value = Get_Branch_TimeZone();

            try
            {
                if (date_value != "")
                {
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandText = "proc_Get_WEBREMITTANCE_Data";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@TRANXDATE", SqlDbType.VarChar, 10).Value = date_value;
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();

                    ds = base.getDataSet(cmd);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        irs_file_contents = ds.Tables[0];
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    logme("IRSW_WebRemittance FILE EXTRACTOR - Get IRSWorld Web Remittance Data USW : No TimeZone Date set in USW branch.");
                    return false;
                }
            }
            catch (Exception e)
            {
                logme("IRSW_WebRemittance FILE EXTRACTOR - Get IRSWorld Web Remittance Data USW : " + e.Message + ":" + e.StackTrace);
                return false;
            }
            finally
            {
                cn.Close();
                cn.Dispose();
                cmd.Dispose();
                dt = null;
                ds = null;
            }
        }

        private bool get_irsworld_WebRemittance_data_ALL()
        {
            //Populate TEL and WEB Remittance from IRSWorld
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            SqlConnection cn = new SqlConnection(this.irsconn);
            SqlCommand cmd = new SqlCommand();

            //string date_value = Get_Server_Date();
            string date_value = Get_Branch_TimeZone();

            try
            {
                if (date_value != "")
                {
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandText = "proc_Get_WEBREMITTANCE_Data_ALL";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@TRANXDATE", SqlDbType.VarChar, 10).Value = date_value;
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();

                    ds = base.getDataSet(cmd);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        irs_file_contents_ALL = ds.Tables[0];
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    logme("IRSW_WebRemittance FILE EXTRACTOR - Get IRSWorld Web Remittance Data ALL : No TimeZone Date set in RCI branch.");
                    return false;
                }
            }
            catch (Exception e)
            {
                logme("IRSW_WebRemittance FILE EXTRACTOR - Get IRSWorld Web Remittance Data ALL : " + e.Message + ":" + e.StackTrace);
                return false;
            }
            finally
            {
                cn.Close();
                cn.Dispose();
                cmd.Dispose();
                dt = null;
                ds = null;
            }
        }

        private void insert_WebRemittance_Data(DataRow dr_data)
        {
            //Insert selected data to TBL_Remittance_WebRemittance table
            SqlConnection cn = new SqlConnection(this.irsconn);
            SqlCommand cmd = new SqlCommand();

            int accounttype = 0;
            if (dr_data[43].ToString() == "")
            {
                accounttype = 0;
            }
            else
            {
                accounttype = Convert.ToInt32(dr_data[43].ToString());
            }

            try
            {
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "proc_Insert_WEBREMITTANCE_Data";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TRXN_SEQ_ID", SqlDbType.VarChar, 10).Value = dr_data[41].ToString();
                cmd.Parameters.Add("@BRANCHCODE", SqlDbType.VarChar, 4).Value = dr_data[47].ToString();
                cmd.Parameters.Add("@REFERENCENO", SqlDbType.VarChar, 16).Value = dr_data[25].ToString();
                cmd.Parameters.Add("@REM_CIFID", SqlDbType.VarChar, 16).Value = dr_data[22].ToString();
                cmd.Parameters.Add("@REM_NAME", SqlDbType.VarChar, 360).Value = dr_data[4].ToString();
                cmd.Parameters.Add("@BENE_CIFID", SqlDbType.VarChar, 16).Value = dr_data[3].ToString();
                cmd.Parameters.Add("@BENE_NAME", SqlDbType.VarChar, 360).Value = dr_data[1].ToString();
                cmd.Parameters.Add("@ACCOUNTNO", SqlDbType.VarChar, 40).Value = dr_data[42].ToString();
                cmd.Parameters.Add("@ACCOUNTTYPE", SqlDbType.Int, 4).Value = accounttype;
                cmd.Parameters.Add("@CLEARINGDATE", SqlDbType.VarChar, 16).Value = dr_data[44].ToString();
                cmd.Parameters.Add("@AMOUNT", SqlDbType.VarChar, 8).Value = dr_data[2].ToString();
                cmd.Parameters.Add("@BANKROUTINGNO", SqlDbType.VarChar, 40).Value = dr_data[45].ToString();
                cmd.Parameters.Add("@STANDARDENTRYCODE", SqlDbType.VarChar, 3).Value = dr_data[46].ToString();
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                logme("IRSW_WebRemittance FILE EXTRACTOR - Insert WebRemittance Data : " + e.Message + ":" + e.StackTrace);
            }
            finally
            {
                cn.Close();
                cn.Dispose();
                cmd.Dispose();
            }
        }

        private void insert_WebRemittance_Data_Excel(DataRow dr_data)
        {
            //Insert selected data to TBL_Remittance_WebRemittance table
            SqlConnection cn = new SqlConnection(this.irsconn);
            SqlCommand cmd = new SqlCommand();

            int accounttype = 0;
            if (dr_data[43].ToString() == "")
            {
                accounttype = 0;
            }
            else
            {
                accounttype = Convert.ToInt32(dr_data[43].ToString());
            }

            try
            {
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "proc_Insert_WEBREMITTANCE_Data";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TRXN_SEQ_ID", SqlDbType.VarChar, 10).Value = dr_data[41].ToString();
                cmd.Parameters.Add("@BRANCHCODE", SqlDbType.VarChar, 4).Value = dr_data[44].ToString();
                cmd.Parameters.Add("@REFERENCENO", SqlDbType.VarChar, 16).Value = dr_data[25].ToString();
                cmd.Parameters.Add("@REM_CIFID", SqlDbType.VarChar, 16).Value = dr_data[3].ToString();
                cmd.Parameters.Add("@REM_NAME", SqlDbType.VarChar, 360).Value = dr_data[1].ToString();
                cmd.Parameters.Add("@BENE_CIFID", SqlDbType.VarChar, 16).Value = dr_data[45].ToString();
                cmd.Parameters.Add("@BENE_NAME", SqlDbType.VarChar, 360).Value = dr_data[46].ToString();
                cmd.Parameters.Add("@ACCOUNTNO", SqlDbType.VarChar, 40).Value = dr_data[17].ToString();
                cmd.Parameters.Add("@ACCOUNTTYPE", SqlDbType.Int, 4).Value = dr_data[42].ToString();
                cmd.Parameters.Add("@CLEARINGDATE", SqlDbType.VarChar, 16).Value = dr_data[43].ToString();
                cmd.Parameters.Add("@AMOUNT", SqlDbType.VarChar, 8).Value = dr_data[2].ToString();
                cmd.Parameters.Add("@BANKROUTINGNO", SqlDbType.VarChar, 40).Value = dr_data[16].ToString();
                cmd.Parameters.Add("@STANDARDENTRYCODE", SqlDbType.VarChar, 3).Value = dr_data[19].ToString();
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                logme("IRSW_WebRemittance FILE EXTRACTOR - Insert WebRemittance Data : " + e.Message + ":" + e.StackTrace);
            }
            finally
            {
                cn.Close();
                cn.Dispose();
                cmd.Dispose();
            }
        }

        private void create_WebRemittance_Excel_file()
        {
            csFileIo io;
            string irswsubalias = new irsConfiguration().ConnectionStringNoRunMode("IRSWSUBALIAS");
            DataSet ds_WebRemittance = new DataSet();
            DataTable dt_file_values = new DataTable();

            try
            {
                dt_file_values = irs_file_contents.Copy();

                //delete extra column not needed in the extract 06252013
                int dtColCount = dt_file_values.Columns.Count;
                for (int idel = 43; idel < dtColCount; idel++)
                {
                    dt_file_values.Columns.RemoveAt(43);
                }
               
                io = new csFileIo();

                if (!io.directoryExist(rootpath))
                {
                    io.createDirectory(rootpath);
                }

                this.irs_filename = "IRSWACH_" + System.DateTime.Now.ToString("yyyyMMdd") + "_" + irswsubalias + ".xls";
                string irs_tempfilename = rootpath + this.irs_filename;

                if (io.exist(irs_tempfilename))
                {
                    io.delete(irs_tempfilename);
                }

                ds_WebRemittance.Tables.Add(dt_file_values);

                //Create Excel file
                ExcelLibrary.DataSetHelper.CreateWorkbook(irs_tempfilename,ds_WebRemittance, false);

            }
            catch (Exception e)
            {
                logme("IRSW_WebRemittance FILE EXTRACTOR - Create WebRemittance file : " + e.Message + ":" + e.StackTrace);
            }
            finally
            {
                io = null;
            }

        }

        private void create_WebRemittance_file_text(string Branch, DataTable dtContent)
        {
            csFileIo io;
            int i = 0;
            string endFile = "";
            string content_value = "";

            string irswsubalias = new irsConfiguration().ConnectionStringNoRunMode("IRSWSUBALIAS");

            if (Branch != "ALL")
            {
                if (FileFormat == "1")
                {
                    irswsubalias = irswsubalias + "_USW_IAT";
                }
                else
                {
                    irswsubalias = irswsubalias + "_USW_NCH";
                }
            }
            else
            {
                irswsubalias = irswsubalias + "_ALL";
            }

            try
            {
                total_records = dtContent.Rows.Count;

                io = new csFileIo();

                this.irs_filename = "IRSWACH" + System.DateTime.Now.ToString("yyyyMMdd") + irswsubalias + ".txt";
                string irs_tempfilename = temppath + this.irs_filename;

                if (!io.directoryExist(temppath))
                {
                    io.createDirectory(temppath);
                }

                if (io.exist(irs_tempfilename))
                {
                    io.delete(irs_tempfilename);
                }

                //Create File Data
                io.open_output(irs_tempfilename, (int)csFileIo.write_modes.CREATE_NEW);

                string hdr = this.create_File_header();

                io.putLine(hdr);

                //Write records to FIle
                for (i = 0; i < dtContent.Rows.Count; i++)
                {
                    content_value = irsformat_tostring(dtContent, i);
                    if (content_value.Trim() != "")
                    {
                        io.putLine(content_value);
                        content_value_identifier += content_value.Substring(0, 4) + "~";
                    }
                }

                if (content_value_identifier != "")
                {
                    //Last Batch Control Record
                    if (i == dtContent.Rows.Count)
                    {
                        if (Batch_Record_Created == false)
                        {
                            string batchcontrol_record = this.create_Batch_Control_Record();
                            io.putLine(batchcontrol_record);
                        }
                    }
                    io.close_output();

                    //Create File Control Record
                    int count_lines_filecontrol = io.readAllLines(irs_tempfilename).Length;
                    io.open_output(irs_tempfilename, (int)csFileIo.write_modes.APPEND);
                    string footer = this.create_File_Control_Record(count_lines_filecontrol);
                    io.putLine(footer);
                    io.close_output();

                    //Create "9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999" as additional File Control Record
                    int count_lines = io.readAllLines(irs_tempfilename).Length;
                    io.open_output(irs_tempfilename, (int)csFileIo.write_modes.APPEND);
                    endFile = this.create_End_File(count_lines);
                    if (endFile != "")
                    {
                        //io.put(Encoding.Default.GetBytes(endFile), 0, Encoding.Default.GetBytes(endFile).Length);
                        io.putLine(endFile);
                    }
                    io.close_output();

                    //Move created temporary ach to ACH root directory
                    io.copy(irs_tempfilename, rootpath + this.irs_filename, true);
                }
                else
                {
                    io.close_output();
                    if (io.exist(irs_tempfilename))
                    {
                        io.delete(irs_tempfilename);
                    }
                    logme("IRSW_WebRemittance FILE EXTRACTOR : No File was created due to Error's Encountred in creating T$$ file. ");
                }

            }
            catch (Exception e)
            {
                logme(e.Message + ":" + e.StackTrace);
            }
            finally
            {
                io = null;
            }
        }

        private string irsformat_tostring(DataTable dt, int rowcount)
        {
            string strRemitter = "";
            string strBeneficiary = "";
            string str = "";
            string transactioncode = "";
            string Batch_Control = "";
            //string Next_StandardEntryCode = "";

            try
            {
                //Assign values from TBL_Remittance_WebRemittance
                RemitterCIFID = dt.Rows[rowcount][22].ToString();
                RemitterName = dt.Rows[rowcount][4].ToString();
                BeneficiaryCIFID = dt.Rows[rowcount][3].ToString();
                BeneficiaryName = dt.Rows[rowcount][1].ToString();
                RemitterAccount = dt.Rows[rowcount][42].ToString();
                AccountType = dt.Rows[rowcount][43].ToString();
                TraceNo = get_WebRemittance_TraceNumber(dt.Rows[rowcount][25].ToString());

                if (dt.Rows[rowcount][0].ToString() != "")
                {
                    ClearingDate_value = Convert.ToDateTime(dt.Rows[rowcount][0]);
                    ClearingDate = ClearingDate_value.ToString("yyMMdd");
                }
                else
                {
                    logme("IRSW_WebRemittance FILE EXTRACTOR : No Clearing date value for Trace Number : " + TraceNo);
                    return "";
                }

                TargetAmount = dt.Rows[rowcount][2].ToString().Replace(".", "").PadLeft(10, '0');
                RemitterBank_RoutingNo = dt.Rows[rowcount][45].ToString();
                ReceivingDFI_BankName = dt.Rows[rowcount][48].ToString();
                StandardEntryClass = dt.Rows[rowcount][46].ToString();
                NettoBeneAmount = dt.Rows[rowcount][2].ToString().Replace(".", "");
                RemitterAddress = dt.Rows[rowcount][5].ToString();
                RemitterCity = dt.Rows[rowcount][6].ToString();
                RemitterCountry = dt.Rows[rowcount][20].ToString();
                BenefAddress = dt.Rows[rowcount][28].ToString();

                if (RemitterBank_RoutingNo == "" || RemitterBank_RoutingNo == "0")
                {
                    if (ReceivingDFI_GatewayOperator != "")
                    {
                        RemitterBank_RoutingNo = ReceivingDFI_GatewayOperator.ToString().Trim();
                    }
                    else
                    {
                        RemitterBank_RoutingNo = "0";
                    }
                }

                if (dt.Rows[rowcount][29].ToString() != "" && dt.Rows[rowcount][30].ToString() != "")
                {
                    BenefCity = dt.Rows[rowcount][29].ToString() + "*" + dt.Rows[rowcount][30].ToString() + "\\";
                }
                else if (dt.Rows[rowcount][29].ToString() != "")
                {
                    BenefCity = dt.Rows[rowcount][29].ToString() + "\\";
                }
                else if (dt.Rows[rowcount][30].ToString() != "")
                {
                    BenefCity = dt.Rows[rowcount][30].ToString() + "\\";
                }
                else
                {
                    BenefCity = "";
                }
               
                if (dt.Rows[rowcount][31].ToString() != "" && dt.Rows[rowcount][32].ToString() != "")
                {
                    BenefCountry = dt.Rows[rowcount][31].ToString() + "*" + dt.Rows[rowcount][32].ToString() + "\\";
                }
                else if (dt.Rows[rowcount][31].ToString() != "")
                {
                    BenefCountry = dt.Rows[rowcount][31].ToString() + "\\";
                }
                else if (dt.Rows[rowcount][32].ToString() != "")
                {
                    BenefCountry = dt.Rows[rowcount][32].ToString() + "\\";
                }
                else
                {
                    BenefCountry = "";
                }

                if (TargetAmount != "")
                {
                    total_amount_all_trxn += Convert.ToInt32(TargetAmount);
                }
                else
                {
                    logme("IRSW_WebRemittance FILE EXTRACTOR : No Amount value for Trace Number : " + TraceNo);
                    return "";
                }

                if (RemitterBank_RoutingNo != "" && RemitterBank_RoutingNo.Length >= 9)
                {
                    File_hash += Convert.ToInt32(RemitterBank_RoutingNo.Substring(0, 8));
                }
                else
                {
                    File_hash += 0;
                    logme("IRSW_WebRemittance FILE EXTRACTOR : No Routing Number or Routing number length is less than 9 for RemitterCIFID : " + RemitterCIFID + " ; Routing Number : " + RemitterBank_RoutingNo);
                    //return ""; //comment out to handle Remitter without Routing No. 07172013
                }

                if (AccountType == "" || AccountType == "0" || AccountType == "2")
                {
                    transactioncode = "27";
                }
                else
                {
                    transactioncode = "37";
                }

                //Format Remitter / Beneficiary Name
                int Remitter_len = (RemitterName.Length < 16) ? RemitterName.Length : 16;
                int Beneficiary_len = (BeneficiaryName.Length < 22) ? BeneficiaryName.Length : 22;
                int RoutingNo_len = (RemitterBank_RoutingNo.Length < 10) ? RemitterBank_RoutingNo.Length : 10;
                int RemitterAddress_len = (RemitterAddress.Length < 35) ? RemitterAddress.Length : 35;
                int RemitterCity_len = (RemitterCity.Length < 35) ? RemitterCity.Length : 35;
                int RemitterCountry_len = (RemitterCountry.Length < 35) ? RemitterCountry.Length : 35;
                int BenefAddress_len = (BenefAddress.Length < 35) ? BenefAddress.Length : 35;
                int BenefCity_len = (BenefCity.Length < 35) ? BenefCity.Length : 35;
                int BenefCountry_len = (BenefCountry.Length < 35) ? BenefCountry.Length : 35;

                if (Remitter_len == 16)
                {
                    RemitterName = RemitterName.Substring(0, 16);
                }
                else
                {
                    RemitterName = RemitterName.PadRight(16, ' ');
                }

                if (Beneficiary_len == 22)
                {
                    BeneficiaryName = BeneficiaryName.Substring(0, 22);
                }
                else
                {
                    BeneficiaryName = BeneficiaryName.PadLeft(22, ' ');
                }

                if (RoutingNo_len == 10)
                {
                    RemitterBank_RoutingNo = RemitterBank_RoutingNo.Substring(0, 10);
                }
                else
                {
                    RemitterBank_RoutingNo = RemitterBank_RoutingNo.PadRight(10, '0');
                }

                if (RemitterAddress_len == 35)
                {
                    RemitterAddress = RemitterAddress.Substring(0, 35);
                }
                else
                {
                    RemitterAddress = RemitterAddress.PadRight(35, ' ');
                }

                if (RemitterCity_len == 35)
                {
                    RemitterCity = RemitterCity.Substring(0, 35);
                }
                else
                {
                    RemitterCity = RemitterCity.PadRight(35, ' ');
                }

                if (RemitterCountry_len == 35)
                {
                    RemitterCountry = RemitterCountry.Substring(0, 35);
                }
                else
                {
                    RemitterCountry = RemitterCountry.PadRight(35, ' ');
                }


                if (BenefAddress_len == 35)
                {
                    BenefAddress = BenefAddress.Substring(0, 35);
                }
                else
                {
                    BenefAddress = BenefAddress.PadRight(35, ' ');
                }

                if (BenefCity_len == 35)
                {
                    BenefCity = BenefCity.Substring(0, 35);
                }
                else
                {
                    BenefCity = BenefCity.PadRight(35, ' ');
                }

                if (BenefCountry_len == 35)
                {
                    BenefCountry = BenefCountry.Substring(0, 35);
                }
                else
                {
                    BenefCountry = BenefCountry.PadRight(35, ' ');
                }

                ImmediateOriginCityandState = "";
                ImmediateOriginCityandState = ImmediateOriginCity + "*" + ImmediateOriginState + "\\";
                ImmediateOriginCountryandPostalCode = "";
                ImmediateOriginCountryandPostalCode = ImmediateOriginCountry + "*" + ImmediateOriginPostalCode + "\\";
                
                //Populate FEDACH Transaction Data
                //TEL Remittance
                if (StandardEntryClass == "TEL")
                {
                    if (RemitterCIFID != Same_Remitter_value)
                    {
                        //Batch Header Record
                        Batch_Number = Batch_Number + 1;

                        strRemitter = "5" +
                                      service_class_code +
                                      RemitterName +
                                      "FF3" +
                                      "               " +
                                      "US" +
                                      ImmediateOrigin.Trim().PadRight(10, ' ') +
                                      StandardEntryClass +
                                      "ACH       " +
                                      "USDUSD" +
                                      ClearingDate +
                                      "   " +
                                      "1" +
                                      ImmediateOrigin.Trim().PadLeft(8, '0').Substring(0, 8) +
                                      Batch_Number.ToString().PadLeft(7, '0');
                                      //batchdate.ToString("yyMMdd").PadLeft(6, '0');

                        Batch_count = Batch_count + 1;

                        //Entry Detail Record
                        strBeneficiary = "6" + transactioncode +
                                        RemitterBank_RoutingNo.Substring(0, 8) +
                                        RemitterBank_RoutingNo.Substring(8, 1) +
                                        "0000" +
                                        "             " +
                                        TargetAmount.PadLeft(10, '0') +
                                        RemitterAccount.Trim().PadRight(15, ' ') +
                                        BeneficiaryName.Trim().PadRight(20, ' ') +
                                        "  " +
                                        "  " +
                                        "0" +
                                        ImmediateOrigin_BankRoutingNo + TraceNo.PadLeft(7, '0');

                        Entry_detail = Entry_detail + 1;
                        total_amount += Convert.ToInt32(TargetAmount);
                        Batch_hash += Convert.ToInt32(RemitterBank_RoutingNo.Substring(0, 8));

                        Summary_Entry += "DEBIT  " +
                                        TargetAmount + "  " +
                                        ImmediateOrigin + "  " +
                                        RemitterBank_RoutingNo + "  " +
                                        AccountType + "  " +
                                        BeneficiaryName + "  [" +
                                        ImmediateOrigin_BankRoutingNo + TraceNo.PadLeft(7, '0') + "]" + System.Environment.NewLine;
                    }
                    else
                    {
                        //Entry Detail Record
                        strBeneficiary = "6" +
                                        transactioncode +
                                        RemitterBank_RoutingNo.Substring(0, 8) +
                                        RemitterBank_RoutingNo.Substring(8, 1) +
                                        "0000" +
                                        "             " +
                                        TargetAmount.PadLeft(10, '0') +
                                        RemitterAccount.Trim().PadRight(15, ' ') +
                                        BeneficiaryName.Trim().PadRight(20, ' ') +
                                        "  " +
                                        "  " +
                                        "0" +
                                        ImmediateOrigin_BankRoutingNo + TraceNo.PadLeft(7, '0');

                        Entry_detail = Entry_detail + 1;
                        total_amount += Convert.ToInt32(TargetAmount);
                        Batch_hash += Convert.ToInt32(RemitterBank_RoutingNo.Substring(0, 8));

                        Summary_Entry += "DEBIT  " +
                                        TargetAmount + "  " +
                                        ImmediateOrigin + "  " +
                                        RemitterBank_RoutingNo + "  " +
                                        AccountType + "  " +
                                        BeneficiaryName + "  [" +
                                        ImmediateOrigin_BankRoutingNo + TraceNo.PadLeft(7, '0') + "]" + System.Environment.NewLine;
                    }

                    //Batch Control Record
                    if (RemitterCIFID != Same_Remitter_value && Same_Remitter_value != "")
                    {
                        Batch_Control = this.create_Batch_Control_Record();
                    }
                    else if (Same_Remitter_value == "")
                    {
                        string Next_Remitter = "";
                        Next_Remitter = dt.Rows[rowcount + 1][22].ToString();
                        if (RemitterCIFID != Next_Remitter)
                        {
                            Batch_Control = this.create_Batch_Control_Record();
                        }
                    }

                    /*Comment out for Extract only has 1 Standard Entry Class Code 07242013
                    if (dt.Rows.Count != (rowcount + 1))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            Next_StandardEntryCode = dt.Rows[rowcount + 1][46].ToString();
                            if (Next_StandardEntryCode != StandardEntryClass)
                            {
                                Batch_Control = this.create_Batch_Control_Record();
                            }
                         }
                     }*/

                    if (RemitterCIFID != Same_Remitter_value)
                    {
                        if (Batch_Control != "")
                        {
                            str = strRemitter + System.Environment.NewLine + strBeneficiary + System.Environment.NewLine + Batch_Control;
                        }
                        else
                        {
                            str = strRemitter + System.Environment.NewLine + strBeneficiary;
                        }

                    }
                    else
                    {
                        if (Batch_Control != "")
                        {
                            str = strBeneficiary + System.Environment.NewLine + Batch_Control;
                        }
                        else
                        {
                            string Next_RemitterID = "";
                            Next_RemitterID = dt.Rows[rowcount + 1][22].ToString();
                            if (RemitterCIFID != Next_RemitterID)
                            {
                                Batch_Control = this.create_Batch_Control_Record();
                            }
                            //str = strBeneficiary; comment out 07262013
                            str = strBeneficiary + System.Environment.NewLine + Batch_Control;
                        }

                    }

                    Same_Remitter_value = RemitterCIFID;
                }

                //WEB Remittance
                else if (StandardEntryClass == "WEB" && FileFormat == "1")
                {
                    if (RemitterCIFID != Same_Remitter_value_Web)
                    {
                        //Batch Header Record
                        Batch_Number = Batch_Number + 1;
                        strRemitter = "5" +
                                      service_class_code +
                                      RemitterName +
                                      "FF3" +
                                      "               " +
                                      "US" +
                                      ImmediateOrigin.Trim().PadRight(10, ' ') +
                                      StandardEntryClass +
                                      "ACH       " +
                                      "USDUSD" +
                                      ClearingDate +
                                      "   " +
                                      "1" +
                                      ImmediateOrigin.Trim().PadLeft(8, '0').Substring(0, 8) +
                                      Batch_Number.ToString().PadLeft(7, '0'); 
                                      //batchdate.ToString("yyMMdd").PadLeft(6, '0');

                        Batch_count = Batch_count + 1;

                        //Entry Detail Record
                        strBeneficiary = "6" + transactioncode +
                                        RemitterBank_RoutingNo.Substring(0, 8) +
                                        RemitterBank_RoutingNo.Substring(8, 1) +
                                        "0007" +
                                        "             " +
                                        TargetAmount.PadLeft(10, '0') +
                                        RemitterAccount.Trim().PadRight(15,' ') + 
                                        "                    " +
                                        "  " +
                                        "  " +
                                        "1" +
                                        ImmediateOrigin_BankRoutingNo + TraceNo.PadLeft(7, '0') + System.Environment.NewLine +
                                        "7" + "10" + "WEB" + NettoBeneAmount.PadLeft(18, '0') + "                      " + BeneficiaryName.Trim().PadRight(35, ' ') + "      " + TraceNo + System.Environment.NewLine +
                                        "7" + "11" + ImmediateOriginName.Trim().PadRight(35, ' ') + ImmediateOriginAddress.PadRight(35,' ') + "              " + TraceNo + System.Environment.NewLine +
                                        "7" + "12" + ImmediateOriginCityandState.PadRight(35, ' ' ) + ImmediateOriginCountryandPostalCode.PadRight(35, ' ') + "              " + TraceNo + System.Environment.NewLine +
                                        "7" + "13" + ImmediateOriginName.Trim().PadRight(35, ' ') + "01" + ImmediateOrigin.Trim().PadRight(34, ' ') + " " + OriginatingDFICountryCode + "          " + TraceNo + System.Environment.NewLine +
                                        "7" + "14" + ReceivingDFI_BankName.Trim().PadRight(35, ' ') + "01" + RemitterBank_RoutingNo.Trim().PadRight(34, ' ') + " " + ReceivingDFICountryCode + "          " + TraceNo + System.Environment.NewLine +
                                        "7" + "15" + BeneficiaryCIFID.Trim().PadRight(15, ' ') + BenefAddress + "                                  " + TraceNo + System.Environment.NewLine +
                                        "7" + "16" + BenefCity + BenefCountry + "              " + TraceNo; //+ System.Environment.NewLine +
                                        //"7" + "99" + "R01" + TraceNo.PadLeft(15, '0') + "              " + TargetAmount.PadLeft(10, '0') + "                                  " + TraceNo.Substring(0, 15);

                        total_amount += Convert.ToInt32(TargetAmount);
                        total_addenda_line_count = total_addenda_line_count + 8;
                        Batch_hash += Convert.ToInt32(RemitterBank_RoutingNo.Substring(0, 8));

                        Summary_Entry += "DEBIT  " +
                                        TargetAmount + "  " +
                                        ImmediateOrigin + "  " +
                                        RemitterBank_RoutingNo + "  " +
                                        AccountType + "  " +
                                        BeneficiaryName + "  [" +
                                        ImmediateOrigin_BankRoutingNo + TraceNo.PadLeft(7, '0') + "]" + System.Environment.NewLine;
                    }
                    else
                    {
                        //Entry Detail Record
                        strBeneficiary = "6" +
                                        transactioncode +
                                        RemitterBank_RoutingNo.Substring(0, 8) +
                                        RemitterBank_RoutingNo.Substring(8, 1) +
                                        "0007" +
                                        "             " +
                                        TargetAmount.PadLeft(10, '0') +
                                        RemitterAccount.Trim().PadRight(15, ' ') +
                                        "                    " +
                                        "  " +
                                        "  " +
                                        "1" +
                                        ImmediateOrigin_BankRoutingNo + TraceNo.PadLeft(7, '0') + System.Environment.NewLine +
                                        "7" + "10" + "WEB" + NettoBeneAmount.PadLeft(18, '0') + "                      " + BeneficiaryName.Trim().PadRight(35, ' ') + "      " + TraceNo + System.Environment.NewLine +
                                        "7" + "11" + ImmediateOriginName.Trim().PadRight(35, ' ') + ImmediateOriginAddress.PadRight(35, ' ') + "              " + TraceNo + System.Environment.NewLine +
                                        "7" + "12" + ImmediateOriginCityandState.PadRight(35, ' ') + ImmediateOriginCountryandPostalCode.PadRight(35, ' ') + "              " + TraceNo + System.Environment.NewLine +
                                        "7" + "13" + ImmediateOriginName.Trim().PadRight(35, ' ') + "01" + ImmediateOrigin.Trim().PadRight(34, ' ') + " " + OriginatingDFICountryCode + "          " + TraceNo + System.Environment.NewLine +
                                        "7" + "14" + ReceivingDFI_BankName.Trim().PadRight(35, ' ') + "01" + RemitterBank_RoutingNo.Trim().PadRight(34, ' ') + " " + ReceivingDFICountryCode + "          " + TraceNo + System.Environment.NewLine +
                                        "7" + "15" + BeneficiaryCIFID.Trim().PadRight(15, ' ') + BenefAddress + "                                  " + TraceNo + System.Environment.NewLine +
                                        "7" + "16" + BenefCity + BenefCountry + "              " + TraceNo; //+ System.Environment.NewLine +
                                        //"7" + "99" + "R01" + TraceNo.PadLeft(15, '0') + "              " + TargetAmount.PadLeft(10, '0') + "                                  " + TraceNo.Substring(0, 15);

                        total_amount += Convert.ToInt32(TargetAmount);
                        total_addenda_line_count = total_addenda_line_count + 8;
                        Batch_hash += Convert.ToInt32(RemitterBank_RoutingNo.Substring(0, 8));

                        Summary_Entry += "DEBIT  " +
                                        TargetAmount + "  " +
                                        ImmediateOrigin + "  " +
                                        RemitterBank_RoutingNo + "  " +
                                        AccountType + "  " +
                                        BeneficiaryName + "  [" +
                                        ImmediateOrigin_BankRoutingNo + TraceNo.PadLeft(7, '0') + "]" + System.Environment.NewLine;
                    }

                    //Batch Control Record
                    if (RemitterCIFID != Same_Remitter_value_Web && Same_Remitter_value_Web != "")
                    {
                        Batch_Control = this.create_Batch_Control_Record();
                    }
                    else if (Same_Remitter_value_Web == "")
                    {
                        string Next_Remitter = "";

                        if (dt.Rows.Count > 1 && dt.Rows.Count != (rowcount + 1))
                        {
                            Next_Remitter = dt.Rows[rowcount + 1][22].ToString();
                            if (RemitterCIFID != Next_Remitter)
                            {
                                Batch_Control = this.create_Batch_Control_Record();
                            }
                        }
                    }

                    /*Comment out for Extract only has 1 Standard Entry Class Code 07242013
                    if (dt.Rows.Count != (rowcount + 1))
                    {
                        if (dt.Rows.Count > 1)
                        {
                            Next_StandardEntryCode = dt.Rows[rowcount + 1][46].ToString();
                            if (Next_StandardEntryCode != StandardEntryClass)
                            {
                                Batch_Control = this.create_Batch_Control_Record();
                            }
                        }
                    }*/

                    if (RemitterCIFID != Same_Remitter_value_Web)
                    {
                        if (Batch_Control != "")
                        {
                            str = strRemitter + System.Environment.NewLine + strBeneficiary + System.Environment.NewLine + Batch_Control;
                        }
                        else
                        {
                            str = strRemitter + System.Environment.NewLine + strBeneficiary;
                        }
                    }
                    else
                    {
                        if (Batch_Control != "")
                        {
                            str = strBeneficiary + System.Environment.NewLine + Batch_Control;
                        }
                        else
                        {
                            string Next_RemitterID = "";
                            if (dt.Rows.Count > 1 && dt.Rows.Count != (rowcount + 1))
                            {
                                Next_RemitterID = dt.Rows[rowcount + 1][22].ToString();
                                if (RemitterCIFID != Next_RemitterID)
                                {
                                    Batch_Control = this.create_Batch_Control_Record();
                                }
                                //str = strBeneficiary; comment out 07262013
                                str = strBeneficiary + System.Environment.NewLine + Batch_Control;
                            }
                            else
                            {
                                Batch_Control = this.create_Batch_Control_Record();
                                str = strBeneficiary + System.Environment.NewLine + Batch_Control;
                            }
                        }

                    }
                    Same_Remitter_value_Web = RemitterCIFID;
                }

                else if (StandardEntryClass == "WEB" && FileFormat == "2")
                {
                    if (RemitterCIFID != Same_Remitter_value_Web)
                    {
                        //Batch Header Record
                        Batch_Number = Batch_Number + 1;
                        strRemitter = "5" +
                                      service_class_code +
                                      RemitterName +
                                      "                    " +
                                      ImmediateOrigin.Trim().PadRight(10, ' ') +
                                      StandardEntryClass +
                                      "ACH       " +
                                      ClearingDate +
                                      ClearingDate +
                                      "   " +
                                      "1" +
                                      ImmediateOrigin.Trim().PadLeft(8, '0').Substring(0, 8) +
                                      Batch_Number.ToString().PadLeft(7, '0'); 
                                      //batchdate.ToString("yyMMdd").PadLeft(6, '0');

                        Batch_count = Batch_count + 1;

                        //Entry Detail Record
                        strBeneficiary = "6" +
                                        transactioncode +
                                        RemitterBank_RoutingNo.Substring(0, 8) +
                                        RemitterBank_RoutingNo.Substring(8, 1) +
                                        RemitterAccount.Trim().PadRight(17, ' ') +
                                        TargetAmount.PadLeft(10, '0') +
                                        "               " +
                                        BeneficiaryName.Trim().PadRight(22, ' ') +
                                        "  " +
                                        "0" +
                                        ImmediateOrigin_BankRoutingNo + TraceNo.PadLeft(7, '0');

                        total_amount += Convert.ToInt32(TargetAmount);
                        Batch_hash += Convert.ToInt32(RemitterBank_RoutingNo.Substring(0, 8));

                        Summary_Entry += "DEBIT  " +
                                        TargetAmount + "  " +
                                        ImmediateOrigin + "  " +
                                        RemitterBank_RoutingNo + "  " +
                                        AccountType + "  " +
                                        BeneficiaryName + "  [" +
                                        ImmediateOrigin_BankRoutingNo + TraceNo.PadLeft(7, '0') + "]" + System.Environment.NewLine;
                    }
                    else
                    {
                        //Entry Detail Record
                        strBeneficiary = "6" +
                                        transactioncode +
                                        RemitterBank_RoutingNo.Substring(0, 8) +
                                        RemitterBank_RoutingNo.Substring(8, 1) +
                                        RemitterAccount.Trim().PadRight(17, ' ') +
                                        TargetAmount.PadLeft(10, '0') +
                                        "               " +
                                        BeneficiaryName.Trim().PadRight(22, ' ') +
                                        "  " +
                                        "0" +
                                        ImmediateOrigin_BankRoutingNo + TraceNo.PadLeft(7, '0');

                        total_amount += Convert.ToInt32(TargetAmount);
                        Batch_hash += Convert.ToInt32(RemitterBank_RoutingNo.Substring(0, 8));

                        Summary_Entry += "DEBIT  " +
                                        TargetAmount + "  " +
                                        ImmediateOrigin + "  " +
                                        RemitterBank_RoutingNo + "  " +
                                        AccountType + "  " +
                                        BeneficiaryName + "  [" +
                                        ImmediateOrigin_BankRoutingNo + TraceNo.PadLeft(7, '0') + "]" + System.Environment.NewLine;
                    }

                    //Batch Control Record
                    if (RemitterCIFID != Same_Remitter_value_Web && Same_Remitter_value_Web != "")
                    {
                        Batch_Control = this.create_Batch_Control_Record();
                    }
                    else if (Same_Remitter_value_Web == "")
                    {
                        string Next_Remitter = "";

                        if (dt.Rows.Count > 1 && dt.Rows.Count != (rowcount + 1))
                        {
                            Next_Remitter = dt.Rows[rowcount + 1][22].ToString();
                            if (RemitterCIFID != Next_Remitter)
                            {
                                Batch_Control = this.create_Batch_Control_Record();
                            }
                        }
                    }

                    /*Comment out for Extract only has 1 Standard Entry Class Code 07242013
                    if (dt.Rows.Count != (rowcount + 1))
                    {
                        if (dt.Rows.Count > 1)
                        {
                            Next_StandardEntryCode = dt.Rows[rowcount + 1][46].ToString();
                            if (Next_StandardEntryCode != StandardEntryClass)
                            {
                                Batch_Control = this.create_Batch_Control_Record();
                            }
                        }
                    }*/

                    if (RemitterCIFID != Same_Remitter_value_Web)
                    {
                        if (Batch_Control != "")
                        {
                            str = strRemitter + System.Environment.NewLine + strBeneficiary + System.Environment.NewLine + Batch_Control;
                        }
                        else
                        {
                            str = strRemitter + System.Environment.NewLine + strBeneficiary;
                        }
                    }
                    else
                    {
                        if (Batch_Control != "")
                        {
                            str = strBeneficiary + System.Environment.NewLine + Batch_Control;
                        }
                        else
                        {
                            string Next_RemitterID = "";
                            if (dt.Rows.Count > 1 && dt.Rows.Count != (rowcount + 1))
                            {
                                Next_RemitterID = dt.Rows[rowcount + 1][22].ToString();
                                if (RemitterCIFID != Next_RemitterID)
                                {
                                    Batch_Control = this.create_Batch_Control_Record();
                                }
                                //str = strBeneficiary; comment out 07262013
                                str = strBeneficiary + System.Environment.NewLine + Batch_Control;
                            }
                            else
                            {
                                Batch_Control = this.create_Batch_Control_Record();
                                str = strBeneficiary + System.Environment.NewLine + Batch_Control;
                            }
                        }
                    }

                    Same_Remitter_value_Web = RemitterCIFID;
                }

            }
            catch (Exception e)
            {
                logme(e.Message + ":" + e.StackTrace);
            }
            finally
            {
                Clear_Data_value();
            }
            return str;
        }

        private void Clear_Data_value()
        {
            BeneficiaryCIFID = "";
            BeneficiaryName = "";
            RemitterAccount = "";
            TargetAmount = "";
        }

        private string create_File_header()
        {
            string shdr = "";
            try
            {
                shdr = "1" +
                             "01" +
                             ImmediateDestination.PadLeft(10, ' ') +
                             ImmediateOrigin.PadLeft(10, ' ') +
                             System.DateTime.Now.ToString("yyMMddhhmm") +
                             "A" +
                             "094" +
                             "10" +
                             "1" +
                             ImmediateDestinationName.PadRight(23, ' ') +
                             ImmediateOriginName.PadRight(23, ' ') +
                             "00000000"; //Reference code : optional. default value 00000000.
                             //batchdate.ToString("yyMMdd").PadLeft(8, '0');
            }
            catch (Exception e)
            {
                logme(e.Message + ":" + e.StackTrace);
            }
            return shdr;
        }

        private string create_Batch_Control_Record()
        {
            string Batch_control = "";
            Batch_Record_Created = false;

            try
            {
                if (StandardEntryClass == "TEL")
                {
                    Batch_control = "8" +
                                           service_class_code +
                                           Entry_detail.ToString().PadLeft(6, '0') +
                                           Batch_hash.ToString().PadLeft(10, '0') +
                                           total_amount.ToString().PadLeft(12, '0') +
                                           "000000000000" +
                                           ImmediateOrigin.Trim().PadLeft(10, '0') +
                                           "                   " +
                                           "      " +
                                           ImmediateOrigin.Substring(1, 8).PadRight(8, '0') +
                                           Batch_Number.ToString().PadLeft(7, '0');
                                           //batchdate.ToString("yyMMdd").PadLeft(6, '0');
                    total_Entry_detail += Entry_detail;
                }
                else if (StandardEntryClass == "WEB")
                {
                    Batch_control = "8" +
                                           service_class_code +
                                           total_addenda_line_count.ToString().PadLeft(6, '0') +
                                           Batch_hash.ToString().PadLeft(10, '0') +
                                           total_amount.ToString().PadLeft(12, '0') +
                                           "000000000000" +
                                           ImmediateOrigin.Trim().PadLeft(10, '0') +
                                           "                   " +
                                           "      " +
                                           ImmediateOrigin.Substring(1, 8).PadRight(8, '0') +
                                           Batch_Number.ToString().PadLeft(7, '0');
                                           //batchdate.ToString("yyMMdd").PadLeft(6, '0');
                    total_Entry_detail_addenda += total_addenda_line_count;
                }
                else
                {
                    Batch_control = "8" +
                                          service_class_code +
                                          total_addenda_line_count.ToString().PadLeft(6, '0') +
                                          Batch_hash.ToString().PadLeft(10, '0') +
                                          total_amount.ToString().PadLeft(12, '0') +
                                          "000000000000" +
                                          ImmediateOrigin.Trim().PadLeft(10, '0') +
                                          "                   " +
                                          "      " +
                                          ImmediateOrigin.Substring(1, 8).PadRight(8, '0') +
                                          Batch_Number.ToString().PadLeft(7, '0');
                                          //batchdate.ToString("yyMMdd").PadLeft(6, '0');
                    total_Entry_detail_addenda += total_addenda_line_count;
                }
                Entry_detail = 0;
                total_addenda_line_count = 0;
                total_amount = 0;
                Batch_hash = 0;
                Batch_Record_Created = true;
            }
            catch (Exception e)
            {
                logme(e.Message + ":" + e.StackTrace);
            }
            return Batch_control;
        }

        private string create_File_Control_Record(int count_per_line)
        {
            string File_control = "";
            int Entry_AddendaCount = 0;
            int Block_remainder = 0;

            try
            {
                Block_remainder = (count_per_line % 10);

                if (Block_remainder.ToString() == "0")
                {
                    Block_count = Convert.ToInt32(count_per_line.ToString().Substring(0, 1));
                }
                else
                {
                    Block_count = Convert.ToInt32(count_per_line.ToString().Substring(0, 1)) + 1;
                }

                Entry_AddendaCount = total_Entry_detail_addenda + total_Entry_detail;


                File_control = "9" +
                                      Batch_count.ToString().PadLeft(6, '0') +
                                      Block_count.ToString().PadLeft(6, '0') +
                                      Entry_AddendaCount.ToString().PadLeft(8, '0') +
                                      File_hash.ToString().PadLeft(10, '0') +
                                      total_amount_all_trxn.ToString().PadLeft(12, '0') +
                                      "000000000000" +
                                      "                                       ";
                                      //"999999999999999999999999999999999999999";
            }
            catch (Exception e)
            {
                logme(e.Message + ":" + e.StackTrace);
            }
            return File_control;
        }

        private string create_End_File(int line_count)
        {
            string return_strEndFile = "";
            int i = 0;
            int remainder = (line_count % 10);

            if (remainder != 0)
            {
                int total_line_count = (10 - remainder);
                for (i = 0; i < total_line_count; i++)
                {
                    string strEndFile;

                    if (i == total_line_count - 1)
                    {
                        strEndFile = "9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999";
                    }
                    else
                    {
                        strEndFile = "9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999" + System.Environment.NewLine;
                    }
                    return_strEndFile += strEndFile;
                }
            }
            else
            {
                return_strEndFile = "";
            }

            return return_strEndFile;
        }

        private void create_Summary_file(string Branch)
        {
            csFileIo io;
            string summary = "";

            try
            {
                io = new csFileIo();
                string irswsubalias = new irsConfiguration().ConnectionStringNoRunMode("IRSWSUBALIAS");

                if (Branch != "ALL")
                {
                    if (FileFormat == "1")
                    {
                        Summary_FileName = "IRSWACH" + System.DateTime.Now.ToString("yyyyMMdd") + irswsubalias + "_USW_IAT_Summary.txt";
                    }
                    else
                    {
                        Summary_FileName = "IRSWACH" + System.DateTime.Now.ToString("yyyyMMdd") + irswsubalias + "_USW_NCH_Summary.txt";
                    }
                }
                else
                {
                    Summary_FileName = "IRSWACH" + System.DateTime.Now.ToString("yyyyMMdd") + irswsubalias + "_Summary.txt";
                }

                string irs_tempfilename = temppath + this.Summary_FileName;

                if (!io.directoryExist(temppath))
                {
                    io.createDirectory(temppath);
                }

                if (io.exist(irs_tempfilename))
                {
                    io.delete(irs_tempfilename);
                }

                io.open_output(irs_tempfilename, (int)csFileIo.write_modes.CREATE_NEW);

                summary = this.create_FED_Summary();
                io.putLine(summary);

                io.close_output();
                io.copy(irs_tempfilename, rootpath + this.Summary_FileName, true);

            }
            catch (Exception e)
            {
                logme(e.Message + ":" + e.StackTrace);
            }
            finally
            {
                io = null;
                summary = "";
            }
        }

        private string create_FED_Summary()
        {
            string strHeader = "";
            try
            {
                strHeader = "File Source             : " + this.irs_filename + System.Environment.NewLine +
                                    "Destination             : " + ImmediateDestination + "  " + ImmediateDestinationName + System.Environment.NewLine +
                                    "Origin                  : " + ImmediateOrigin + "  " + ImmediateOriginName + System.Environment.NewLine +
                                    "File Creation           : [" + batchdate.ToLongDateString() + "]   Time    :  [" + batchdate.ToLongTimeString() + "]" + System.Environment.NewLine +
                                    "Reference Code/Batch No : " + batchdate.ToString("yyMMdd").PadLeft(8 - batchdate.ToString("yyMMdd").Length, '0') + System.Environment.NewLine + System.Environment.NewLine +
                                    Summary_Entry + System.Environment.NewLine +
                                    "ACHFILE FILE SUMMARY FOR: <" + this.irs_filename + System.Environment.NewLine +
                                    "TOTAL Number of Batche(s)             : " + Batch_count + System.Environment.NewLine +
                                    "TOTAL Number of Block(s)              : " + Block_count.ToString() + System.Environment.NewLine +
                                    "TOTAL Number of Entries               : " + total_records.ToString() + System.Environment.NewLine + System.Environment.NewLine +
                                    "Total Debits      : $ " + total_amount_all_trxn.ToString().Substring(0, total_amount_all_trxn.ToString().Length - 2) + "." + total_amount_all_trxn.ToString().Substring(total_amount_all_trxn.ToString().Length - 2, 2) + System.Environment.NewLine +
                                    "Total Credits      : $ 0" + System.Environment.NewLine + System.Environment.NewLine +
                                    "CONGRATULATIONS! ACHFile Created, No Error detected";

                total_amount_all_trxn = 0;
            }
            catch (Exception e)
            {
                logme(e.Message + ":" + e.StackTrace);
            }

            return strHeader;
        }

        //Delete Temporary ACH file from temp directory
        private void delete_temp()
        {
            try
            {
                new csFileIo().delete(temppath + this.irs_filename);
            }
            catch (Exception ex)
            {
                logme("IRSW_WebRemittance FILE EXTRACTOR : Temp delete = " + ex.Message + ":" + ex.StackTrace);
            }
        }

        //FTP created ACH file to FED FTP
        private void ftp_irs_file()
        {
            string host = new irsConfiguration().appsetting("FTPIP");
            string username = new irsConfiguration().appsetting("FTPUSER");
            string password = new irsConfiguration().appsetting("FTPPASS");
            string port = new irsConfiguration().appsetting("FTPPORT");
            string WebRemittanceftppath = new irsConfiguration().appsetting("FTPWEBREMITTANCEPATH");
            csFtp ftp;
            try
            {
                ftp = new csFtp(host, Convert.ToInt32(port), username, password);
                if (ftp.connect())
                {
                    ftp.mkdir(WebRemittanceftppath);

                    if (ftp.cwd(WebRemittanceftppath))
                    {
                        ftp.put(this.rootpath + this.irs_filename, this.irs_filename);
                        //ftp.put(this.rootpath + this.Summary_FileName, this.Summary_FileName);
                        this.Mailer();
                    }
                    else
                    {
                        logme("IRSW_WebRemittance FILE EXTRACTOR : cannot change FTP path");
                    }
                    ftp.disconnect();
                }
                else
                {
                    logme("IRSW_WebRemittance FILE EXTRACTOR : cannot connect to FTP");
                }
            }
            catch (Exception e)
            {
                logme(e.Message + ":" + e.StackTrace);

            }
            finally
            {
                ftp = null;
            }
        }

        //FTP created ACH file to FED FTP
        private void ftp_irs_file_ALL()
        {
            string host = new irsConfiguration().appsetting("FTPIP");
            string username = new irsConfiguration().appsetting("FTPUSER");
            string password = new irsConfiguration().appsetting("FTPPASS");
            string port = new irsConfiguration().appsetting("FTPPORT");
            string WebRemittanceftppath = new irsConfiguration().appsetting("FTPWEBREMITTANCEPATH");
            csFtp ftp;
            try
            {
                ftp = new csFtp(host, Convert.ToInt32(port), username, password);
                if (ftp.connect())
                {
                    ftp.mkdir(WebRemittanceftppath);

                    if (ftp.cwd(WebRemittanceftppath))
                    {
                        ftp.put(this.rootpath + this.irs_filename, this.irs_filename);
                        //ftp.put(this.rootpath + this.Summary_FileName, this.Summary_FileName);
                        this.Mailer_ALL();
                    }
                    else
                    {
                        logme("IRSW_WebRemittance FILE EXTRACTOR : cannot change FTP path");
                    }
                    ftp.disconnect();
                }
                else
                {
                    logme("IRSW_WebRemittance FILE EXTRACTOR : cannot connect to FTP");
                }
            }
            catch (Exception e)
            {
                logme(e.Message + ":" + e.StackTrace);

            }
            finally
            {
                ftp = null;
            }
        }

        //Send E-mail Alert        
        private void Mailer()
        {
            csMailer mail = new csMailer();
            string subject = "IRSWORLD WEB Remittance EXTRACT - " + this.irs_filename;
            string[] recipient = this.emailRecipient();

            try
            {
                if (recipient != null)
                {
                    string[] attachments = this.Get_attachments();
                    string msg = "";
                    msg = this.defaultEmailMessage();
                    mail.sendMail(subject, recipient, msg, attachments);
                }
                else
                {
                    logme("IRSW_WebRemittance FILE EXTRACTOR : no set-up email receipient in branch setup");
                }
            }
            catch (Exception ex)
            {
                logme("IRSW_WebRemittance FILE EXTRACTOR : email error message = " + ex.Message.ToString() + ex.StackTrace.ToString());
            }
        }

        private void Mailer_ALL()
        {
            csMailer mail = new csMailer();
            string subject = "IRSWORLD ACH EXTRACT - " + this.irs_filename;
            string[] recipient = this.emailRecipient_ALL();

            try
            {
                if (recipient != null)
                {
                    string[] attachments = this.Get_attachments_ALL();
                    string msg = "";
                    msg = this.defaultEmailMessage_ALL();
                    mail.sendMail(subject, recipient, msg, attachments);
                }
                else
                {
                    logme("IRSW_WebRemittance FILE EXTRACTOR : no set-up email receipient in branch setup");
                }
            }
            catch (Exception ex)
            {
                logme("IRSW_WebRemittance FILE EXTRACTOR : email error message = " + ex.Message.ToString() + ex.StackTrace.ToString());
            }
        }

        public string[] emailRecipient()
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            SqlConnection cn = new SqlConnection(this.irsconn);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "proc_Get_Email_ACH";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();

                ds = base.getDataSet(cmd);
                dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    string recipients = "";
                    for (int ndx = 0; ndx < dt.Rows.Count; ndx++)
                    {
                        recipients += dt.Rows[ndx]["Email"].ToString();
                        recipients += "~";
                    }
                    return recipients.Split('~');
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                logme("IRSW_WebRemittance FILE EXTRACTOR : error in retrieving receipient e-mail address = " + e.Message + ":" + e.StackTrace);
                return null;
            }
            finally
            {
                cn.Close();
                cn.Dispose();
                cmd.Dispose();
                dt = null;
                ds = null;
            }
        }

        public string[] emailRecipient_ALL()
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            SqlConnection cn = new SqlConnection(this.irsconn);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "proc_Get_Email_ACH_ALL";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();

                ds = base.getDataSet(cmd);
                dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    string recipients = "";
                    for (int ndx = 0; ndx < dt.Rows.Count; ndx++)
                    {
                        recipients += dt.Rows[ndx]["Email"].ToString();
                        recipients += "~";
                    }
                    return recipients.Split('~');
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                logme("IRSW_WebRemittance FILE EXTRACTOR : error in retrieving receipient e-mail address = " + e.Message + ":" + e.StackTrace);
                return null;
            }
            finally
            {
                cn.Close();
                cn.Dispose();
                cmd.Dispose();
                dt = null;
                ds = null;
            }
        }

        private string defaultEmailMessage()
        {
            StringBuilder sbMail = new StringBuilder();

            try
            {
                sbMail.Append("Remittance Transaction generated from IRSWorld Phone Remittance and Web Remittance Transaction.");
                sbMail.Append("\n\nACH File Name : " + this.irs_filename);
                sbMail.Append("\nTotal number of Debit Transaction in the file : " + this.total_records.ToString());
                sbMail.Append("\n\n\nNote: This is a system generated message.\nPlease do not reply to this email.");
            }
            catch (Exception ex)
            {
                logme("IRSW_WebRemittance FILE EXTRACTOR : default email message error = " + ex.Message.ToString() + ex.StackTrace.ToString());
             }
            return sbMail.ToString();           
        }

        private string defaultEmailMessage_ALL()
        {
            StringBuilder sbMail = new StringBuilder();

            try
            {
                sbMail.Append("Remittance Transaction generated from IRSWorld RCI Branches Web Remittance Transaction.");
                sbMail.Append("\n\nACH File Name : " + this.irs_filename);
                sbMail.Append("\nTotal number of Debit Transaction in the file : " + this.total_records.ToString());
                sbMail.Append("\n\n\nNote: This is a system generated message.\nPlease do not reply to this email.");
            }
            catch (Exception ex)
            {
                logme("IRSW_WebRemittance FILE EXTRACTOR : default email message error = " + ex.Message.ToString() + ex.StackTrace.ToString());
            }
            return sbMail.ToString();
        }

        public string[] Get_attachments()
        {
            try
            {
                string xls_file = "";
                string[] nameoffile = this.irs_filename.Split('.');

                xls_file = this.rootpath + nameoffile[0].ToString() + ".xls";

                string[] attached = xls_file.Split('~');
                return attached;
            }
               catch (Exception ex)
            {
                logme("IRSW_WebRemittance FILE EXTRACTOR : error in getting e-mail attachments = " + ex.Message.ToString() + ex.StackTrace.ToString());
                return null;
            }
        }

        public string[] Get_attachments_ALL()
        {
            try
            {
                string txt_file = "";
                string[] nameoffile = this.irs_filename.Split('.');

                txt_file = this.rootpath + nameoffile[0].ToString() + ".txt";

                string[] attached = txt_file.Split('~');
                return attached;
            }
            catch (Exception ex)
            {
                logme("IRSW_WebRemittance FILE EXTRACTOR : error in getting e-mail attachments = " + ex.Message.ToString() + ex.StackTrace.ToString());
                return null;
            }
        }

        private void Archive_File()
        {
            csFileIo io;

            io = new csFileIo();
            string arhive_filename = this.ArchivePath + this.irs_filename;
            string source_filename = this.rootpath + this.irs_filename;
            string arhive_summary_filename = this.ArchivePath + this.irs_filename.Replace(".txt","") + "_Summary.txt";
            string summary_filename = this.rootpath + this.irs_filename.Replace(".txt", "") + "_Summary.txt";

            try
            {
                if (io.exist(this.ArchivePath) == false)
                {
                    io.createDirectory(this.ArchivePath);
                }

                if (io.exist(source_filename) == true)
                {
                    if (io.exist(arhive_filename))
                    {
                        io.delete(arhive_filename);
                    }

                    io.copy(source_filename, arhive_filename, false);
                    io.delete(source_filename);
                }

                if (io.exist(summary_filename) == true)
                {
                    if (io.exist(arhive_summary_filename))
                    {
                        io.delete(arhive_summary_filename);
                    }

                    io.copy(summary_filename, arhive_summary_filename, false);
                    io.delete(summary_filename);
                }
            }
            catch (Exception ex)
            {
                logme("IRSW_WebRemittance FILE EXTRACTOR : error in archiving file = " + this.irs_filename + "      Reason : "+ ex.Message.ToString() + ex.StackTrace.ToString());
            }
        }

        public static void logme(string msg)
        {
            try
            {
                StreamWriter sw;
                string logpath = new irsConfiguration().appsetting("TempPath");
                string str = DateTime.Now.ToString() + ": " + msg;
                string dt = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0');
                sw = File.AppendText(logpath + "\\" + "IRSWWebRemittanceFE" + dt + ".log");
                try
                {
                    sw.WriteLine(str);
                    sw.Close();
                }
                catch { return; }
                finally
                {
                    str = null;
                    sw.Dispose();
                    sw = null;
                }
            }
            catch { return; }
            finally { }
        }

        private void Clear_All_Variables()
        {
            irs_filename = "";
            Summary_FileName = "";
            RemitterCIFID = "";
            RemitterName = "";
            BeneficiaryCIFID = "";
            BeneficiaryName = "";
            RemitterAccount = "";
            AccountType = "";
            ClearingDate = "";
            TargetAmount = "";
            Same_Remitter_value = "";
            Same_Remitter_value_Web = "";
            TraceNo = "";
            Summary_Entry = "";
            RemitterBank_RoutingNo = "";
            StandardEntryClass = "";
            NettoBeneAmount = "";
            RemitterAddress = "";
            RemitterCity = "";
            RemitterCountry = "";
            BenefAddress = "";
            BenefCity = "";
            BenefCountry = "";
            content_value_identifier = "";
            Entry_detail = 0;
            Batch_count = 0;
            Batch_Number = 0;
            Block_count = 0;
            Batch_hash = 0;
            File_hash = 0;
            total_Entry_detail = 0;
            total_Entry_detail_addenda = 0;
            total_records = 0;
            total_amount = 0;
            total_amount_all_trxn = 0;
            total_addenda_line_count = 0;
        }

        public void log_dtls(string msg)
        {
            try
            {
                logme(msg);

            }
            finally { }
        }

    }
}
