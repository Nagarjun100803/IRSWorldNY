using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using IRSClassLib;
using System.Collections;

namespace IRSClassLib
{
    public class irsVirtualPOLoader : sqlHelper
    {

        public bool save_file_info()
        {
            
            object save_file = base.execScalar("update tbl_polink_temp set P_flag='1' where P_status = '1' and BatchNumber = '1' and p_date = '2011-07-27'");

            return true;
        }
        public bool set_process_flag(int curr_batch)
        {
            object delete_current = base.execScalar("update tbl_polink_temp set P_flag='1' where P_status = '1' and BatchNumber = '" + curr_batch + "' and p_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'");

            return true;
        }
        public DataTable getCodeOpts(string fldgroup)
        {

            DataSet ds;
            ds = execStoredProcedure("sp_get_FldDescription", new object[] { fldgroup });
            return ds.Tables[0];
        }
        public int get_total_null(int null_total,int batchno)
        {
            object temp_total_null = base.execScalar("SELECT count(*) FROM tbl_polink_temp LEFT OUTER JOIN tbl_cif_van ON tbl_polink_temp.VirtualNo = tbl_cif_van.van LEFT OUTER JOIN tbl_cif_master ON tbl_cif_van.CIFID = tbl_cif_master.CIFID where batchnumber ='" + batchno + "' and P_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' and tbl_cif_master.FullName is null");
            object temp_remark_null = base.execScalar("Update tbl_polink_temp set tbl_polink_temp.Null_remarks = 'Not enrolled' from tbl_polink_temp LEFT OUTER JOIN tbl_cif_van ON tbl_polink_temp.VirtualNo = tbl_cif_van.van LEFT OUTER JOIN tbl_cif_master ON tbl_cif_van.CIFID = tbl_cif_master.CIFID where batchnumber ='" + batchno + "' and P_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' and tbl_cif_master.FullName is null");
            object temp_remark_ok = base.execScalar("Update tbl_polink_temp set tbl_polink_temp.Null_remarks = 'Record OK' from tbl_polink_temp LEFT OUTER JOIN tbl_cif_van ON tbl_polink_temp.VirtualNo = tbl_cif_van.van LEFT OUTER JOIN tbl_cif_master ON tbl_cif_van.CIFID = tbl_cif_master.CIFID where batchnumber ='" + batchno + "' and P_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' and tbl_cif_master.FullName is not null");
            int total_null = Convert.ToInt32(temp_total_null);
            return total_null;
        }
        public bool delete_temp_upload(int curr_batch)
        {
            object delete_current = base.execScalar("delete FROM tbl_polink_temp where batchnumber='" + curr_batch + "' and p_date ='" + DateTime.Now.ToString("yyyy-MM-dd") + "'");
            return true;
        }
        public DataSet generate_txt(int batchno)
        {
            DataSet ds = base.getDataSet(base.getSqlCommand("SELECT * FROM tbl_polink_temp LEFT OUTER JOIN tbl_cif_van ON tbl_polink_temp.VirtualNo = tbl_cif_van.van LEFT OUTER JOIN tbl_cif_master ON tbl_cif_van.CIFID = tbl_cif_master.CIFID where batchnumber ='" + batchno + "' and P_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' and p_status = '1' and tbl_cif_master.FullName is not null order by tbl_polink_temp.seq_ID", false));
            return ds;

        }
        public DataSet getTable(int batchno)
        {

           // DataSet ds = base.getDataSet(base.getSqlCommand("SELECT * FROM tbl_polink_temp LEFT OUTER JOIN tbl_cif_van ON tbl_polink_temp.VirtualNo = tbl_cif_van.van LEFT OUTER JOIN tbl_cif_master ON tbl_cif_van.CIFID = tbl_cif_master.CIFID where batchnumber ='" + batchno + "' and P_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' order by tbl_polink_temp.Seq_ID", false));

            string myquery = "SELECT "
            + "isnull(fullname,'') as FullName ,"
            + "isnull(Hold_remarks,'') as Hold_remarks ,"
            + "VirtualNo,Amount,P_date,P_time,Null_remarks,SequenceNumber,P_status "
            + "FROM tbl_polink_temp "
            + "LEFT JOIN tbl_cif_van ON tbl_polink_temp.VirtualNo = tbl_cif_van.van "
            + "LEFT JOIN tbl_cif_master ON tbl_cif_van.CIFID = tbl_cif_master.CIFID "
            + "where batchnumber ='" 
            + batchno +"' and P_date = '" 
            + DateTime.Now.ToString("yyyy-MM-dd") 
            //remove p_flag = '0' to query for PDeposit
            + "' and P_flag = '0' order by tbl_polink_temp.Seq_ID";
            DataSet ds = base.getDataSet(base.getSqlCommand(myquery, false));
             
            return ds;
        }
        public DataSet getTable_virtual(int batchno)
        {

            // DataSet ds = base.getDataSet(base.getSqlCommand("SELECT * FROM tbl_polink_temp LEFT OUTER JOIN tbl_cif_van ON tbl_polink_temp.VirtualNo = tbl_cif_van.van LEFT OUTER JOIN tbl_cif_master ON tbl_cif_van.CIFID = tbl_cif_master.CIFID where batchnumber ='" + batchno + "' and P_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' order by tbl_polink_temp.Seq_ID", false));

            string myquery = "SELECT "
            + "isnull(fullname,'') as FullName ,"
            + "isnull(Hold_remarks,'') as Hold_remarks ,"
            + "VirtualNo,Amount,P_date,P_time,Null_remarks,SequenceNumber,V_customerinfo "
            + "FROM tbl_polink_temp "
            + "LEFT JOIN tbl_cif_van ON tbl_polink_temp.VirtualNo = tbl_cif_van.van "
            + "LEFT JOIN tbl_cif_master ON tbl_cif_van.CIFID = tbl_cif_master.CIFID "
            + "where batchnumber ='"
            + batchno + "' and P_date = '"
            + DateTime.Now.ToString("yyyy-MM-dd")
            + "' order by tbl_polink_temp.Seq_ID";
            DataSet ds = base.getDataSet(base.getSqlCommand(myquery, false));

            return ds;
        }
        public DataSet getTable2(string fdate, string tdate)
        {

            DateTime dtf = Convert.ToDateTime(fdate);
            fdate = dtf.ToString("yyyy-MM-dd");
            DateTime dtt = Convert.ToDateTime(tdate);
            tdate = dtt.ToString("yyyy-MM-dd");
            DataSet ds = base.getDataSet(base.getSqlCommand("SELECT * FROM tbl_polink_temp LEFT OUTER JOIN tbl_cif_van ON tbl_polink_temp.VirtualNo = tbl_cif_van.van LEFT OUTER JOIN tbl_cif_master ON tbl_cif_van.CIFID = tbl_cif_master.CIFID where tbl_polink_temp.p_date between '" + fdate + "' and '" + tdate + "' order by tbl_polink_temp.Sequencenumber", false));
            //DataSet ds = base.getDataSet(base.getSqlCommand("SELECT * FROM tbl_polink_temp LEFT OUTER JOIN tbl_cif_van ON tbl_polink_temp.VirtualNo = tbl_cif_van.van LEFT OUTER JOIN tbl_cif_master ON tbl_cif_van.CIFID = tbl_cif_master.CIFID where tbl_polink_temp.p_date between '2011-07-25' and '2011-07-26' order by tbl_polink_temp.Seq_ID", false));
            return ds;
        }
        public DataSet getTable2_va(string fdate, string tdate)
        {

            DateTime dtf = Convert.ToDateTime(fdate);
            fdate = dtf.ToString("yyyy-MM-dd");
            DateTime dtt = Convert.ToDateTime(tdate);
            tdate = dtt.ToString("yyyy-MM-dd");
            DataSet ds = base.getDataSet(base.getSqlCommand("SELECT * FROM tbl_polink_temp LEFT OUTER JOIN tbl_cif_van ON tbl_polink_temp.VirtualNo = tbl_cif_van.van LEFT OUTER JOIN tbl_cif_master ON tbl_cif_van.CIFID = tbl_cif_master.CIFID where tbl_polink_temp.p_date between '" + fdate + "' and '" + tdate + "' and v_transferbank is not null and v_transferbranch is not null  order by tbl_polink_temp.Sequencenumber", false));
            //DataSet ds = base.getDataSet(base.getSqlCommand("SELECT * FROM tbl_polink_temp LEFT OUTER JOIN tbl_cif_van ON tbl_polink_temp.VirtualNo = tbl_cif_van.van LEFT OUTER JOIN tbl_cif_master ON tbl_cif_van.CIFID = tbl_cif_master.CIFID where tbl_polink_temp.p_date between '2011-07-25' and '2011-07-26' order by tbl_polink_temp.Seq_ID", false));
            return ds;
        }
        public DataSet getTable_date(string fdate, string tdate)
        {
            DataSet ds = base.getDataSet(base.getSqlCommand("SELECT * FROM tbl_polink_temp LEFT OUTER JOIN tbl_cif_van ON tbl_polink_temp.VirtualNo = tbl_cif_van.van LEFT OUTER JOIN tbl_cif_master ON tbl_cif_van.CIFID = tbl_cif_master.CIFID where batchnumber ='" + GlobalVariable.batch_no + "' and P_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' order by tbl_polink_temp.Seq_ID", false));
            return ds;
        }
        public int get_current_batch(int btchno)
        {
            int con_batch;
            object temp_lastdate = base.execScalar("select MAX(batchnumber) from tbl_polink_temp where v_transferbank is null and v_transferbranch is null");
            string batch_comp = temp_lastdate.ToString();
            if (batch_comp != "")
            {
                
                con_batch = Convert.ToInt32(temp_lastdate);
            }
            else
            {
                con_batch = 0;
            }
            return con_batch;
        }
        public int get_current_batch_virtual(int btchno)
        {
            int con_batch;
            object temp_lastdate = base.execScalar("select MAX(batchnumber) from tbl_polink_temp where v_transferbank is not null and v_transferbranch is not null");
            string batch_comp = temp_lastdate.ToString();
            if (batch_comp != "")
            {

                con_batch = Convert.ToInt32(temp_lastdate);
            }
            else
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
            GlobalVariable.batch_no = mybatchno;
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
        public bool save_filename(string f_filename, string f_filesize, DateTime b_datetime)
        {
                object save_current_info = base.execScalar("insert into tbl_file_inventory (Filetype, fileformat, filename, " +
                "Uploadingbranch, uploadedby, approvedby, Totalnoofrecords,  noofrecordsloaded, noofrecordserror, " +
                "filesize, dateupdated, dateuploaded, dateuploadedlocal, dateapproved, dateprocessed, status, isdeleted, destination, rate_type, hashtotal)" +
                "values ('5','2','" + f_filename + "','TOK','USER',Null,Null,Null,Null,'" + f_filesize + "'," +
                "'" + DateTime.Now + "','" + DateTime.Now + "','" + b_datetime + "','" + b_datetime + "','" + DateTime.Now + "','0','0','\\IRS_Loading\\IRS\\POLINK','-1','0')");
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
                        return base.save(sqlHelper.RETAIN_DATASET);
                    }
                }
                else
                {
                    Message = "Username is Required.";
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
                                uprow["V_referenceno"] = v_referencenumber;
                                uprow["V_customerinfo"] = v_customerinfo;
                                uprow["V_transferbank"] = v_transferbank;
                                uprow["V_transferbranch"] = v_transferbranch;
                                uprow["filename_seqID"] = filename_seqid;
                                table.Rows.Add(uprow);
                            }
                        }
                    }
                    base.addMode = false;
                    return base.save(sqlHelper.RETAIN_DATASET);
                    //}
                }
                else
                {
                    Message = "Username is Required.";
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
                    return base.save(sqlHelper.RETAIN_DATASET);
                    //}
                }
                else
                {
                    Message = "Username is Required.";
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
        static class GlobalVariable
        {
            public static int batch_no = 0;
        }

    }
}
