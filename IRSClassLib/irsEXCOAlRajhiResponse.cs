using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.IO;

using System.Web;
using System.Web.UI;

namespace IRSClassLib
{
    [Serializable]
    public class irsEXCOAlRajhiResponse : sqlHelperWs 
    {
        public DataSet getDataset(String SQL_String)
        {   
            return this.getDataset(SQL_String);
        }
        
        public bool update_alrajhi_reference(String PON, String IRSReferenceNo, String FileName)
        {
            try
            {
                Object[] plist = 
                {
                    PON
                    ,IRSReferenceNo
                    ,FileName
                };

                int res = this.execStoredProcedureUpdate("sp_update_al_rajhi_refno", plist);

                if (res <= 0)
                {
                    if (this.Message != null && this.Message != "sp_update_al_rajhi_refno") { throw new Exception(this.Message); }
                    throw new Exception("No Row to update");
                }
                else
                {
                    return true;
                }
            }
            catch(Exception ex)
            {
                writeAudit("Update Ref", "Failed to update PON : " + PON + "in File : " + FileName + ", Reason : " + ex.Message, "", "", WinAppLogger.Category.Update);
            }
            
            return false;
        }

        public void delete_existing_file(String FileName)
        {
            try
            {
                int res = this.execStoredProcedureUpdate("sp_delete_existing_rej_al_rajhi_file", new Object[] { FileName });

                if (this.Message != null && this.Message != "")
                {
                    throw new Exception(this.Message);
                }
            }
            catch (Exception ex)
            {
                writeAudit("DELETE EXIST", ex.Message, "", "", WinAppLogger.Category.Update);
            }
        }

        public void update_alrajhi_file_status(String FileName, bool Status)
        {
            try
            {
                int res = this.execStoredProcedureUpdate("sp_update_alrajhi_file_status", new Object[] { FileName, Status });

                if (this.Message != null && this.Message != "")
                {
                    throw new Exception(this.Message);
                }
            }
            catch (Exception ex)
            {
                writeAudit("UPDATE FILE STAT", ex.Message, "", "", WinAppLogger.Category.Update);
            }
        }

        public void write_to_al_rajhi_temp(String buff, String FileName)
        {
            try
            {
                string[] arrReadbuff = buff.Split('|');
                Object[] plist =
                {
                    FileName
                    ,arrReadbuff[1]
                    ,arrReadbuff[2]
                    ,arrReadbuff[3]
                    ,arrReadbuff[4]
                    ,arrReadbuff[5]
                    ,arrReadbuff[6]
                    ,arrReadbuff[7]
                    ,arrReadbuff[8]
                    ,arrReadbuff[9]
                    ,arrReadbuff[10]
                    ,arrReadbuff[11]
                    ,arrReadbuff[12]
                    ,arrReadbuff[13]
                    ,arrReadbuff[14]
                    ,arrReadbuff[15]
                    ,arrReadbuff[16]
                    ,""
                    ,""
                    ,""
                    ,arrReadbuff[17]
                };

                int res = this.execStoredProcedureUpdate("sp_insert_al_rajhi_to_temp", plist);

                if (res <= 0)
                {
                    throw new Exception(arrReadbuff[1] + " | " + this.Message);
                }
            }
            catch (Exception ex)
            {
                writeAudit("WRITE TEMP", "Failed to write to Al Rajhi table , Reason : " + ex.Message, "", "", WinAppLogger.Category.Insert);
            }
        }

        public void writeAudit(String Mode, String Description, String NewValue, String OldValue, WinAppLogger.Category Category)
        {
            String userId = irsClientData.UserId;
            String sDescription = "Location=" + irsClientData.Location + " : " + Description;

            new csAuditLog().writeAudit(userId, "AL RAJHI " + Mode, sDescription, NewValue, OldValue, (csAuditLog.Category)Category);
        }

        public bool check_if_alrajhi(int FileFormat, int FileType)
        {
            try
            {
                DataSet ds = base.execStoredProcedure("sp_check_if_alrajhi", new Object[] { FileFormat, FileType });
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    int isAlRajhi = 0;
                    int.TryParse(ds.Tables[0].Rows[0][0].ToString(), out isAlRajhi);

                    if (isAlRajhi == 1)
                    {
                        return true;
                    }
                }
            }   
            catch (Exception ex)
            {
                writeAudit("CHECK FILEFORMAT", ex.Message, "", "", WinAppLogger.Category.Select);
            }
            return false;
        }

        public bool create_response_file(String FileName, out String msg)
        {
            try
            {
                String CFileName = "";
                String errmsg = "";
                msg = "";

                csFileIo fio = new csFileIo();
                DataTable dt = get_response_data(FileName, out errmsg);
                String FileNameNoExt = Path.GetFileNameWithoutExtension(FileName);
                String DropDir = "";

                try
                {
                    DropDir = new irsConfiguration().appsetting("temp_dir").ToString() + @"\AlRajhiResponse";
                }
                catch { throw new Exception("Incomplete config info: temp_dir"); }

                if (!fio.directoryExist(DropDir))
                {
                    DirectoryInfo dir = fio.createDirectory(DropDir);
                }

                CFileName = DropDir + @"\" + FileNameNoExt + ".res";
                if (dt != null)
                {
                    fio.open_output(CFileName, (int)csFileIo.write_modes.CREATE);

                    if (fio.isWritable)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            fio.putLine(dt.Rows[i]["ALRAJHI"].ToString());
                        }
                        fio.close_output();
                    }

                    msg = CFileName;
                    return true;
                }
                else
                {
                    throw new Exception(errmsg);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return false;
        }

        

        private DataTable get_response_data(String FileName, out String errmsg)
        {
            errmsg = "";
            try
            {
                DataSet ds = this.execStoredProcedure("sp_get_al_rajhi_response", new Object[] { FileName });
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                {
                    if (this.Message != null && this.Message != "")
                    {
                        throw new Exception(this.Message);
                    }
                    else
                    {
                        throw new Exception("No data found");
                    }
                }
            }
            catch (Exception ex)
            {
                errmsg = "Failed to get response data, Reason : " + ex.Message;
            }
            return null;
        }

        public irsClientData irsClientData
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] == null)
                {
                    System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] = new irsClientData();
                }

                return (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            }
        }

        public void Show(string message)
        {
            // Cleans the message to allow single quotation marks
            string cleanMessage = message.Replace("'", "\\'");
            string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "');</script>";

            // Gets the executing web page
            Page page = HttpContext.Current.CurrentHandler as Page;

            // Checks if the handler is a Page and that the script isn't allready on the Page
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(typeof(irsEXCOAlRajhiResponse), "alert", script);
            }
        }
    }
}
