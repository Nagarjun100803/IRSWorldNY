using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;
//using GRSFileLoaderLib;

namespace IRSClassLib
{
   public class splitter_ui_code
    {
        private splitter_struct.do_splitter_struct  pstruct = null;

        private int total_non_deducted = 0;
        private int total_deducted_PUNLA = 0;
        private int total_orig_record_count = 0; 
        private int total_record_wPUNLA = 0;
        
        private csFileIo io = null;
        private string path_of_gold = string.Empty;

        private string get_filename()
        {
            if (this.io != null) { return this.io.getFilename(this.path_of_gold).ToString(); } return string.Empty;
        }
        private string get_filesize()
        {
            if (this.io != null) { return this.io.getFileSize(this.path_of_gold).ToString(); } return string.Empty;
        }

        private string get_orig_total_records()
        {
            return this.total_orig_record_count.ToString("#,###");
        }

        private string get_total_non_deducted()
        {
            return this.total_non_deducted.ToString("#,###");
        }
        private string get_total_deducted_PUNLA()
        {
            return this.total_deducted_PUNLA.ToString("#,###");
        }
        private string get_total_record_wPUNLA()
        {
            return this.total_record_wPUNLA.ToString("#,###");
        }

       public string validate_input_file(string path)
       {
           //string vresults = this.pre_validate_source(path);
           string date_part = string.Empty;

           //if (vresults != null && vresults.Trim().Length > 0)
           //{
           //    return vresults;
           //}

           csFileIo fio = new csFileIo();

           if (fio.open_input(path, (int)csFileIo.access_modes.BINARY))
           {
               int i = 0;
               while (!fio.EOF)
               {
                   String s = fio.readLine();

                   if (s == null) continue;
                   if (s.Length == 1 && Encoding.Default.GetBytes(s)[0] == 0x1A) continue;
                   if (i == 0 && s.Substring(0, 3).Equals("HDR")) continue;

                   try
                   {
                       i++;

                       date_part = s.Substring(4, 8);

                       if (s.Length == 143)
                       {
                           DateTime.Parse(date_part.Substring(4, 2) + "/" +
                                          date_part.Substring(6, 2) + "/" +
                                          date_part.Substring(0, 4));

                           return string.Empty;
                       }
                       else
                       {
                           return "INVALID PAYROLL FILE";
                       }


                   }
                   catch (Exception e)
                   {
                       return e.Message;
                   }
               }

               fio.close_input();
           }
           return "INVALID PAYROLL FILE";
       }

        public string PUNLA_summary(string path)
        { 
            this.io = new csFileIo();
            this.path_of_gold = path;
            string filename;
            long filesize;
            filename = io.getFilename(path);
            filesize = io.getFileSize(path);
        
            if (this.io.open_input(path, (int)csFileIo.access_modes.BINARY))
            {
                int i = 0;

                while (!this.io.EOF)
                {
                    String s = this.io.readLine();
                    if (s == null) continue;
                    if (s.Length == 1 && Encoding.Default.GetBytes(s)[0] == 0x1A) continue;
                    if (i == 0 && s.Substring(0, 3).Equals("HDR")) continue;

                    try
                    {
                        
                        this.pstruct = new splitter_struct.do_splitter_struct(s); i++;
                        this.total_orig_record_count++;
                        decimal lPUNLAvalue = PUNLA_splitter(this.pstruct.acct_num);

                        //code PUNLA here get method
                        if (lPUNLAvalue != 0)
                        {
                             this.total_deducted_PUNLA++;
                        }
                        else
                        {
                           this.total_non_deducted++;
                        }
                       
                    }
                    catch { }
                    finally
                    {
                        if (pstruct != null) { pstruct = null; }
                    }
                }

                    //this.total_non_deducted = (total_orig_record_count - total_deducted_PUNLA);
                if (total_deducted_PUNLA != 0)
                {
                    this.total_record_wPUNLA = (total_orig_record_count + total_deducted_PUNLA);
                }
                else
                {
                    this.total_record_wPUNLA = 0;
                }
                    
            }
            if (this.io != null) { this.io.close_input(); }

            return string.Concat(filename + ";" + filesize + ";" + get_total_non_deducted() + ";" +
                                get_total_deducted_PUNLA() + ";" + get_orig_total_records() + ";" + get_total_record_wPUNLA());
        }

        public string PUNLA_split_file(string path)
        {
            string str_write = string.Empty;
            string msg = string.Empty;
            decimal lfor_PUNLA = 0.0M;
            decimal lfor_deduction = 0.0M;
            csFileIo io = null; 
            StreamReader sr = new StreamReader(path); 
            String new_path = getOutputPath(path);

           try
            {
                io = new csFileIo();
                if (io.open_output(new_path, (int)csFileIo.write_modes.CREATE))
                {
                    int i = 0;
                    
                    while (!io.EOF)
                    {
                        String s = sr.ReadLine();
                        if (s == null) break; 
                       // if (s == null) continue;
                        if (s.Length == 1 && Encoding.Default.GetBytes(s)[0] == 0x1A) continue;
                        if (i == 0 && s.Substring(0, 3).Equals("HDR")) continue;
                       
                            this.pstruct = new splitter_struct.do_splitter_struct(s); i++;
                            decimal lpercent = Convert.ToDecimal(PUNLA_splitter(this.pstruct.acct_num)) / 100;
                            decimal lorig_amount = Convert.ToDecimal(pstruct.pay_amount);
                           
                            // CODE PUNLA splitting here include the other filtering for punla,
                  
                            if (lpercent == 0)
                            {
                                   io.putLine(s) ; 
                            }
                            else if (lpercent != 0)
                            {    
                                lfor_PUNLA = lorig_amount * lpercent; 
                                string str_PUNLA = Convert.ToString(lfor_PUNLA).Replace(".0000", " ");
                                str_write = pstruct.first_substring + str_PUNLA.PadRight(20) + pstruct.acct_num + pstruct.third_substing;
                                //this.io.putLine(str_write);
                                io.putLine(str_write); 

                                lfor_deduction = lorig_amount - lfor_PUNLA;
                                string str_deduction = Convert.ToString(lfor_deduction).Replace(".0000", " ");
                                str_write = pstruct.first_substring + str_deduction.PadRight(20) + pstruct.acct_num + pstruct.third_substing;
                                io.putLine(str_write); 
                            }     
                    }
                    io.close_output();
                    return io.OutPutFileName;         
                }
                else
                {
                    msg = "Unable to open output " + new_path;
                }
           }
            catch (Exception e)
            {   
                msg = e.Message;
            }
            finally
            {
                if (io != null) { io = null; }
                if (sr != null) { sr.Dispose(); sr = null; }
            }

            return null;
        }
    
        private decimal PUNLA_splitter(string acctnum)
        {
            irsSqlCommons com = new irsSqlCommons();

            DataSet ds = com.getDataSet(com.getSqlCommand("SELECT PercentContri FROM tbl_PUNLAMaint	WHERE	AccountNo = '" + acctnum + "' ", false), "PercentContri");
              //  ("sp_PUNLA_splitting", new Object[] { acctnum }, "tbl_PUNLAMaint");
  
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    decimal i = Convert.ToDecimal(ds.Tables[0].Rows[0]["PercentContri"]);
                    return i;
                }
                else
                {
                    return 0;
                }
        }

       private String getOutputPath(String local_path)
       {
           String fname = new csFileIo().getFilename(local_path);
           //hard code muna.. ask sir jeff
           String path = "C:\\GRSFILELOADER\\GRSWEBFileLoader\\GRSWebFileLoader\\";//AppDomain.CurrentDomain.BaseDirectory;
           return path + @"temp\" +  fname.Replace(".txt"," ")  + "split.txt";
       }
    }
}
