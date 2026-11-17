using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using IRSClassLib;
using System.Collections;



namespace IRSClassLib
{
    public class irsPOLINK : sqlHelperWs
    {

        public bool save_file_info()
        {
            
            object save_file = base.execScalar("update tbl_polink_temp set P_flag='1' where P_status = '1' and BatchNumber = '1' and p_date = '2011-07-27'");

            return true;
        }
        public bool set_process_flag(String querytype)
        {
            if (querytype == "PO")
            {
                object delete_current = base.execScalar("update tbl_polink_temp set P_flag='1' where P_status = '1' and v_transferbranch is null and v_transferbank is null");
            }
            else
            {
                object delete_current = base.execScalar("update tbl_polink_temp set P_flag='1' where P_status = '1' and v_transferbranch is not null and v_transferbank is not null");
            }
            

            return true;
        }
        public DataTable getCodeOpts(string fldgroup,string ses_location)
        {
            
            DataSet ds;
            ds = execStoredProcedure("SP_GET_FLDDESCRIPTION_SUB", new object[] { fldgroup, ses_location });
            return ds.Tables[0];
        }
        public int get_total_null(int null_total,int batchno)
        {
            
            object temp_total_null = base.execScalar("SELECT count(*) FROM tbl_polink_temp LEFT OUTER JOIN tbl_cif_van ON tbl_polink_temp.VirtualNo = tbl_cif_van.van LEFT OUTER JOIN tbl_cif_master ON tbl_cif_van.CIFID = tbl_cif_master.CIFID where batchnumber ='" + batchno + "' and P_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' and tbl_cif_master.FullName is null");
            object temp_remark_null = base.execScalar("Update tbl_polink_temp set tbl_polink_temp.Null_remarks = 'Not enrolled' from tbl_polink_temp LEFT OUTER JOIN tbl_cif_van ON tbl_polink_temp.VirtualNo = tbl_cif_van.van LEFT OUTER JOIN tbl_cif_master ON tbl_cif_van.CIFID = tbl_cif_master.CIFID where tbl_cif_master.FullName is null");
            object temp_remark_ok =   base.execScalar("Update tbl_polink_temp set tbl_polink_temp.Null_remarks = 'Record OK' from tbl_polink_temp LEFT OUTER JOIN tbl_cif_van ON tbl_polink_temp.VirtualNo = tbl_cif_van.van LEFT OUTER JOIN tbl_cif_master ON tbl_cif_van.CIFID = tbl_cif_master.CIFID where tbl_cif_master.FullName is not null");
            object temp_status_over = base.execScalar("Update tbl_polink_temp set tbl_polink_temp.Null_remarks = 'Over Limit' from tbl_polink_temp LEFT OUTER JOIN tbl_cif_van ON tbl_polink_temp.VirtualNo = tbl_cif_van.van LEFT OUTER JOIN tbl_cif_master ON tbl_cif_van.CIFID = tbl_cif_master.CIFID where p_status = '4' and p_flag = '3'");
            object temp_hold_not_enrolled = base.execScalar("Update tbl_polink_temp set tbl_polink_temp.p_status = '2' from tbl_polink_temp LEFT OUTER JOIN tbl_cif_van ON tbl_polink_temp.VirtualNo = tbl_cif_van.van LEFT OUTER JOIN tbl_cif_master ON tbl_cif_van.CIFID = tbl_cif_master.CIFID where tbl_cif_master.FullName is null and p_status = '1' and p_flag = '0'");
            int total_null = Convert.ToInt32(temp_total_null);
            return total_null;
        }
        public bool delete_temp_upload(int curr_batch)
        {
            object delete_current = base.execScalar("delete FROM tbl_polink_temp where batchnumber='" + curr_batch + "' and p_date ='" + DateTime.Now.ToString("yyyy-MM-dd") + "'");
            return true;
        }
        public DataSet generate_txt(String querytype)
        {
            
            if (querytype == "PO")
            {
                DataSet ds;
                ds = execStoredProcedure("SP_VirtualPO_Export", new object[] { "Polink" });
                return ds;
            }
            else
            {
                DataSet ds;
                ds = execStoredProcedure("SP_VirtualPO_Export", new object[] { "Virtual" });
                return ds;
            }
           

        }
        public DataSet getTable(int batchno)
        {
            DataSet ds;
            ds = execStoredProcedure("SP_PO_filters", new object[] { "Approved" });
            return ds;
        }
        public DataSet getTable_virtual(int batchno)
        {
            DataSet ds;
            ds = execStoredProcedure("SP_VA_filters", new object[] { "Approved" });
            return ds;
        }
        public DataSet getTable_processed(int batchno)
        {
            DataSet ds;
            ds = execStoredProcedure("SP_PO_filters", new object[] { "Processed" });
            return ds;
        }
        public DataSet getTable_processed_virtual(int batchno)
        {
            DataSet ds;
            ds = execStoredProcedure("SP_VA_filters", new object[] { "Processed" });
            return ds;
        }
        public DataSet getTable_hold(int batchno)
        {
            DataSet ds;
            ds = execStoredProcedure("SP_PO_filters", new object[] { "Hold" });
            return ds;
        }
        public DataSet getTable_hold_virtual(int batchno)
        {
            DataSet ds;
            ds = execStoredProcedure("SP_VA_filters", new object[] { "Hold" });
            return ds;
        }
        public DataSet getTable_other(int batchno)
        {
            DataSet ds;
            ds = execStoredProcedure("SP_PO_filters", new object[] { "OTH" });
            return ds;
        }
        public DataSet getTable_other_virtual(int batchno)
        {
            DataSet ds;
            ds = execStoredProcedure("SP_VA_filters", new object[] { "OTH" });
            return ds;
        }
        public DataSet getTable_pdep(int batchno)
        {
            DataSet ds;
            ds = execStoredProcedure("SP_PO_filters", new object[] { "PDEP" });
            return ds;
        }
        public DataSet getTable_pdep_virtual(int batchno)
        {
            DataSet ds;
            ds = execStoredProcedure("SP_VA_filters", new object[] { "PDEP" });
            return ds;
        }
        public DataSet getTable_overlimit(int batchno)
        {
            DataSet ds;
            ds = execStoredProcedure("SP_PO_filters", new object[] { "Overlimit" });
            return ds;
        }
        public DataSet getTable_overlimit_virtual(int batchno)
        {
            DataSet ds;
            ds = execStoredProcedure("SP_VA_filters", new object[] { "Overlimit" });
            return ds;
        }
        public DataSet getTable2(string fdate, string tdate)
        {

            DateTime dtf = Convert.ToDateTime(fdate);
            fdate = dtf.ToString("MM-dd-yyyy");
            DateTime dtt = Convert.ToDateTime(tdate);
            tdate = dtt.ToString("MM-dd-yyyy");
            DataSet ds = execStoredProcedure("SP_POLINK_generate_table", new object[] { "POLINK", fdate, tdate });
            return ds;
        }
        public DataSet getTable2_va(string fdate, string tdate)
        {

            DateTime dtf = Convert.ToDateTime(fdate);
            fdate = dtf.ToString("MM-dd-yyyy");
            DateTime dtt = Convert.ToDateTime(tdate);
            tdate = dtt.ToString("MM-dd-yyyy");
            DataSet ds = execStoredProcedure("SP_POLINK_generate_table", new object[] { "VA" , fdate, tdate });
            return ds;
        }
        public DataSet getTable_date(string fdate, string tdate)
        {
            DataSet ds = base.getDataSet(base.getSqlCommand("SELECT * FROM tbl_polink_temp LEFT OUTER JOIN tbl_cif_van ON tbl_polink_temp.VirtualNo = tbl_cif_van.van LEFT OUTER JOIN tbl_cif_master ON tbl_cif_van.CIFID = tbl_cif_master.CIFID where batchnumber ='" + System.Web.HttpContext.Current.Session["batch_no"] + "' and P_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' order by tbl_polink_temp.Seq_ID", false));
            return ds;
        }
        public int get_current_batch(int btchno)
        {
            int con_batch; 
            object temp_lastdate = base.execScalar("select MAX(batchnumber) from tbl_polink_temp where v_transferbank is null and v_transferbranch is null");
            try {
                con_batch = Convert.ToInt32(temp_lastdate);
            }            
            catch{
                con_batch = 0;
            }
            return con_batch;
        }
        public int get_current_batch_virtual(int btchno)
        {
            int con_batch;
            object temp_lastdate = base.execScalar("select MAX(batchnumber) from tbl_polink_temp where v_transferbank is not null and v_transferbranch is not null");
            try
            {
                con_batch = Convert.ToInt32(temp_lastdate);
            }
            catch
            {
                con_batch = 0;
            }
            return con_batch;
        }
        public bool loader_check_pdo(double current_amount,string u_type)
        {
            bool return_value;
            double branch_limit;
            object temp_obj = base.execScalar("select MAX(tellerlimit) from tbl_branch_setup"
            + " where branchcode = 'tok' and CONVERT(VARCHAR(10), CutOffStart, 121) = "
            + "(select CONVERT(VARCHAR(10),MAX(CutOffStart), 121) "
            + "from tbl_branch_setup where branchcode = 'tok')");
            
                
                string temp_obj_string = temp_obj.ToString();
                branch_limit = Convert.ToDouble(temp_obj_string);
                if (current_amount > branch_limit)
                {
                    return_value = false;
                }
                else
                {
                    return_value = true;
                }
                 
            
            return return_value;
        }
        public int getbatchno(int btchno)
        {
            //DateTime.Now.ToString("yyddMM")
            object temp_lastdate = base.execScalar("select MAX(p_date) from tbl_polink_temp");
            string comp_date = temp_lastdate.ToString();
            if (comp_date != "")
            {
                DateTime dt = Convert.ToDateTime(comp_date);
                comp_date = dt.ToString("yyddMM");
            }


            object temp_batch = base.execScalar("select MAX(batchnumber) from tbl_polink_temp where p_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'");
            string temp_string = temp_batch.ToString();
            Int32 mybatchno;

            if (temp_string == "")
            {

                mybatchno = 1;
            }
            else
            {
                if (comp_date == DateTime.Now.ToString("yyddMM"))
                {
                    mybatchno = Int32.Parse(temp_string);
                    mybatchno = mybatchno + 1;
                }
                else
                {
                    mybatchno = 1;
                }

            }
            System.Web.HttpContext.Current.Session["batch_no"] = mybatchno;
//            Session["batch_no"] = mybatchno;
            return mybatchno;
        }
        public bool save_file_changes(string f_filename, string f_filesize, DateTime b_datetime)
        {
            object check_exist = base.execScalar("SELECT count(*) FROM tbl_file_inventory where filename='" + f_filename + "'");
            string total_file_exist = check_exist.ToString();
            if (total_file_exist == "0")
            {
                
                return false;
            }
            else
            {
                return true;
            }


        }
        public bool save_filename(string f_filename, string f_filesize, DateTime b_datetime,string my_uid,string my_location)
        {
            //SP_VirtualPO_insert 'mytestfile.csv','1000','08-08-2011','sysadm1','TOK'
            execStoredProcedure("SP_VirtualPO_insert", new object[] { f_filename, f_filesize, b_datetime, my_uid, my_location });
                //object save_current_info = base.execScalar("insert into tbl_file_inventory (Filetype, fileformat, filename, " +
                //"Uploadingbranch, uploadedby, approvedby, Totalnoofrecords,  noofrecordsloaded, noofrecordserror, " +
                //"filesize, dateupdated, dateuploaded, dateuploadedlocal, dateapproved, dateprocessed, status, isdeleted, destination, rate_type, hashtotal)" +
                //"values ('5','2','" + f_filename + "','"+ my_location +"','"+ my_uid +"',Null,Null,Null,Null,'" + f_filesize + "'," +
                //"'" + DateTime.Now + "','" + DateTime.Now + "','" + b_datetime + "','" + b_datetime + "','" + DateTime.Now + "','0','0','\\IRS_Loading\\IRS\\POLINK','-1','0')");
                return true;
        }
        public bool save_changes(Hashtable ht)
        {
            try
            {
                String sequencenumber = (String)ht["SequenceNumber"];
                String p_status = (string)ht["P_status"];
                String hold_remarks = (string)ht["Hold_remarks"];

                if (sequencenumber.Length > 0)
                {
                    string sqlstmt = "SELECT * FROM tbl_polink_temp WHERE Sequencenumber='" + sequencenumber + "' ";
                    base.getUpdateDS(sqlstmt);
                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("SequenceNumber"))
                            {
                                if (table.Rows.Count > 0)
                                {
                                    selectedRow = getselectedRow(sequencenumber);
                                    base.setColumn("P_status", p_status);
                                    base.setColumn("Hold_remarks", hold_remarks);
                                }
                                else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    uprow["Sequencenumber"] = sequencenumber;
                                    uprow["P_status"] = p_status;
                                    uprow["Hold_remarks"] = hold_remarks;
                                    table.Rows.Add(uprow);
                                }
                            }
                        }
                        base.addMode = false;
                        return base.save(sqlHelperWs.RETAIN_DATASET);
                    }
                }
                else
                {
                    Message = "Sequence Number is Required.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }
        public bool save_me_va(Hashtable ht)
        {

            try
            {
                String sequence_id = (String)ht["Seq_ID"];
                String datasource = (String)ht["Datasource"];
                int batchnumber = (int)ht["BatchNumber"];
                String sequencenumber = (String)ht["SequenceNumber"];
                String virtualNo = (String)ht["VirtualNo"];
                String amount = (String)ht["Amount"];
                String p_date = (String)ht["P_date"];
                String p_time = (String)ht["P_time"];
                String p_status = (String)ht["P_status"];
                String p_flag = (String)ht["P_flag"];
                String v_referencenumber = (String)ht["V_referencenumber"];
                String v_customerinfo = (String)ht["V_customerinfo"];
                String v_transferbank = (String)ht["V_transferbank"];
                String v_transferbranch = (String)ht["V_transferbranch"];


                if (sequencenumber.Length > 0)
                {

                    string sqlstmt = "SELECT * FROM tbl_polink_temp WHERE SequenceNumber='" + sequencenumber + "' ";
                    base.getUpdateDS(sqlstmt);
                    foreach (Object key in ht.Keys)
                    {
                        if (key.ToString().Equals("SequenceNumber"))
                        {
                            if (table.Rows.Count > 0)
                            {
                                selectedRow = getselectedRow(sequencenumber);
                                base.setColumn("Datasource", datasource);
                                base.setColumn("BatchNumber", batchnumber);
                            }
                            else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                            {
                                uprow["Seq_ID"] = sequence_id;
                                uprow["Datasource"] = datasource;
                                uprow["BatchNumber"] = batchnumber;
                                uprow["SequenceNumber"] = sequencenumber;
                                uprow["VirtualNo"] = virtualNo;
                                uprow["Amount"] = amount;
                                uprow["P_date"] = p_date;
                                uprow["P_time"] = p_time;
                                uprow["P_status"] = p_status;
                                uprow["P_flag"] = p_flag;
                                uprow["V_referenceno"] = v_referencenumber;
                                uprow["V_customerinfo"] = v_customerinfo;
                                uprow["V_transferbank"] = v_transferbank;
                                uprow["V_transferbranch"] = v_transferbranch;
                                table.Rows.Add(uprow);
                            }
                        }
                    }
                    base.addMode = false;
                    return base.save(sqlHelperWs.RETAIN_DATASET);
                }
                else
                {
                    Message = "Sequencenumber is Required.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }
        public bool save_me(Hashtable ht)
        {
            
            try
            {
                String sequence_id = (String)ht["Seq_ID"];
                String datasource = (String)ht["Datasource"];
                int batchnumber = (int)ht["BatchNumber"];
                String sequencenumber = (String)ht["SequenceNumber"];
                String virtualNo = (String)ht["VirtualNo"];
                String amount = (String)ht["Amount"];
                String p_date = (String)ht["P_date"];
                String p_time = (String)ht["P_time"];
                String p_status = (String)ht["P_status"];
                String p_flag = (String)ht["P_flag"];
                String filename_seqid = (String)ht["filename_seqID"];

                if (sequencenumber.Length > 0)
                {

                    string sqlstmt = "SELECT * FROM tbl_polink_temp WHERE SequenceNumber='" + sequencenumber + "' ";
                    base.getUpdateDS(sqlstmt);
                    //if (isUpdatable)
                    //{
                    foreach (Object key in ht.Keys)
                    {
                        if (key.ToString().Equals("SequenceNumber"))
                        {
                            if (table.Rows.Count > 0)
                            {
                                selectedRow = getselectedRow(sequencenumber);
                                base.setColumn("Datasource", datasource);
                                base.setColumn("BatchNumber", batchnumber);
                            }
                            else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                            {
                                uprow["Seq_ID"] = sequence_id;
                                uprow["Datasource"] = datasource;
                                uprow["BatchNumber"] = batchnumber;
                                uprow["SequenceNumber"] = sequencenumber;
                                uprow["VirtualNo"] = virtualNo;
                                uprow["Amount"] = amount;
                                uprow["P_date"] = p_date;
                                uprow["P_time"] = p_time;
                                uprow["P_status"] = p_status;
                                uprow["P_flag"] = p_flag;
                                uprow["filename_seqID"] = filename_seqid;
                                table.Rows.Add(uprow);
                            }
                        }
                    }
                    base.addMode = false;
                    return base.save(sqlHelperWs.RETAIN_DATASET);
                    //}
                }
                else
                {
                    Message = "Sequencenumber is Required.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }



        private int getselectedRow(string sequencenumber)
        {
            DataView dv = table.DefaultView;
            dv.Sort = "SequenceNumber";
            int i = dv.Find(new Object[] { sequencenumber });
            if (i > -1)
            {
                //dv[i]["allow_access"] = allow;
                selectedRow = i;
            }
            return i;
        }
        //ftp sending functions from mak @ irsbatchfileupload
        public void sendToFTP(string csvfilepath)
        {
            string ftpstring = new irsConfiguration().FTPConnectionString;
           // string ftpstring = "server=192.168.51.117; User=ftpirsworld; Password=Password1234; Port=21";
            csFtp ftp = new csFtp(ftpstring);
            string ftpTemp = "\\IRS_Loading\\OtherChannels\\Mizuho";
            try
            {
                if (ftp.connect())
                {
                    if (!ftp.cwd(ftpTemp))
                    {
                        ftp.mkdir(ftpTemp);
                        ftp.cwd(ftpTemp);
                    }

                    if (ftp.put(csvfilepath))
                    {
                        
                        return;
                    }
                }

                //_message = ftp.Message;
            }
            catch
            {
                //_message = "ERROR: " + ftp.Message;
                //writeAudit(_message, csAuditLog.Category.FTP);
            }
            finally
            {
                ftp.disconnect();
                ftp = null;
//                deleteTempFile();
            }
        }
       
    }
}
