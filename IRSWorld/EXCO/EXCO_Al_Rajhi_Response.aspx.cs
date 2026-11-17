using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using IRSClassLib;

namespace IRSWorld.EXCO
{
    public partial class EXCO_Al_Rajhi_Response : System.Web.UI.Page
    {
        public String fname = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(EXCO_Al_Rajhi_Response));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_transactions(String FileName, String FileDate, String ViewCode)
        {
            sqlHelperWs sql = new sqlHelperWs();
            update_status_table(FileName);

            DataSet ds = sql.execStoredProcedure("sp_get_al_rajhi_transaction_status", new Object[] { FileName, int.Parse(ViewCode) });
            return ds.Tables[0];
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update(String param)
        {
            String[] p = param.Split('|');

            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_status(String param)
        {   
            String[] paramlist = param.Split('|');
            String errmsg = "";
            String errlist = "";
            foreach (String p in paramlist)
            {
                if (p != null && p!= "")
                {
                    String[] p1 = p.Split('~');
                    if (!update(p1[0].ToString(), p1[1].ToString(), out errmsg))
                    {
                        errlist = errlist + p1[2] + " : " + errmsg + "\n";
                    }
                }
            }

            if (errlist != null && errlist != "")
            {
                throw new Exception("Failed to update status, please see details below\n\n" + errlist);
            }

            return true;
        }

        private bool update(String Seq_id, String Status, out String errmsg)
        {
            errmsg = "";
            sqlHelperWs commons = new sqlHelperWs();
            try
            {
                int i = commons.execStoredProcedureUpdate("sp_update_al_rajhi_status", new Object[] { Seq_id, Status });

                if (i < 1)
                {
                    errmsg = "no row updated";

                    if (commons.Message != null && commons.Message != "")
                    {
                        throw new Exception(commons.Message);
                    }

                    throw new Exception("Record not found");
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                errmsg = "no row updated" + (ex.Message);
            }
            finally
            {
                if (commons != null) { commons = null; }
            }

            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_status()
        {
            String sql_string = "select FldCode, FldDescription from tbl_lookup where fldgroup = 'ALRAJHI_TRXNSTATUS'";
            sqlHelperWs sql = new sqlHelperWs();
            DataSet ds = sql.getDataSet(sql.getSqlCommand(sql_string, false));
            return ds.Tables[0];
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_filelist(String Date)
        {
            sqlHelperWs sql = new sqlHelperWs();
            DataSet ds = sql.execStoredProcedure("sp_get_al_rajhi_filelist", new Object[] { Date });
            return ds;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_status_table(String FileName)
        {            
            sqlHelperWs commons = new sqlHelperWs();
            try
            {
                DataSet ds = commons.execStoredProcedure("sp_get_al_rajhi_transaction_status", new Object[] { FileName, 1 });
                String SearchInGRS = "";
                String SearchInAPAS = "";

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    String ProdCode = ds.Tables[0].Rows[i]["Prod_Code"].ToString();
                    String Refno = ds.Tables[0].Rows[i]["IRSWRefNo"].ToString();

                    if (Refno != null && Refno != "")
                    {
                        if (ProdCode == "01" | ProdCode == "02" | ProdCode == "04")
                        {
                            SearchInGRS += "'" + Refno + "',";
                        }
                        else if (ProdCode == "03")
                        {
                            SearchInAPAS += "'" + Refno + "',";
                        }
                    }
                }

                String grsmsg = "";
                String apasmsg = "";
                String errmsg = "";

                if (SearchInGRS != null && SearchInGRS != "")
                {
                    SearchInGRS = SearchInGRS.Substring(0, SearchInGRS.Length - 1);
                    DataSet dsGRS = get_status_from_grs(SearchInGRS, out grsmsg);

                    if (dsGRS == null && grsmsg != "")
                    {
                        new irsEXCOAlRajhiResponse().writeAudit("UPDATE STAT", "get_status_from_grs : " + grsmsg, "", "", WinAppLogger.Category.Update);
                    }

                    if (dsGRS != null)
                    {
                        if (!update_status_from_grs(dsGRS, ds, out errmsg))
                        {
                            new irsEXCOAlRajhiResponse().writeAudit("UPDATE STAT", "update_status_from_grs : " + errmsg, "", "", WinAppLogger.Category.Update);
                        }
                    }
                }
                
                if (SearchInAPAS != null && SearchInAPAS != "")
                {
                    SearchInAPAS = SearchInAPAS.Substring(0, SearchInAPAS.Length - 1);
                    DataSet dsAPAS = get_status_from_apas(SearchInAPAS, out apasmsg);
                
                    if (dsAPAS == null && apasmsg != "")
                    {
                        new irsEXCOAlRajhiResponse().writeAudit("UPDATE STAT", "get_status_from_apas : " + apasmsg, "", "", WinAppLogger.Category.Update);
                    }


                    if (dsAPAS != null)
                    {
                        if (!update_status_from_apas(dsAPAS, ds, out errmsg))
                        {
                            new irsEXCOAlRajhiResponse().writeAudit("UPDATE STAT", "update_status_from_apas : " + errmsg, "", "", WinAppLogger.Category.Update);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                new irsEXCOAlRajhiResponse().writeAudit("UPDATE STAT", ex.Message, "", "", WinAppLogger.Category.Update);
                return false;
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
        }

        private DataSet get_status_from_grs(String Param, out String errmsg)
        {
            errmsg = "";
            irsSqlCommonsWs com = new irsSqlCommonsWs("GRSConnectionString");
            try
            {
                String sql = "";

                sql = "select trxnref_num,";
                sql += "refnum_ours,";
                sql += "IPA = imp_cd + pay_mode + acct_type,";
                sql += "[3d_status] = isnull([3d_status],''),";
                sql += "fc_status = isnull(fc_status,''),";
                sql += "file_ind = isnull(file_ind,'') ";
                sql += "from tbl_grs_remittance ";
                sql += "where trxnref_num in (" + Param + ") ";
                sql += "and tran_source = 'irs' ";

                DataSet ds = com.getDataSet(com.getSqlCommand(sql, false));

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {
                    if (com.Message != null && com.Message != String.Empty)
                    {
                        throw new Exception(com.Message);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            finally
            {
                if (com != null) { com = null; }
            }

            return null;
        }

        private DataSet get_status_from_apas(String Param, out String errmsg)
        {
            errmsg = "";
            irsSqlCommonsWs com = new irsSqlCommonsWs("ConnectionStringAPAS");
            try
            {
                String sql = "";

                sql = "select rec_TrxnRefNum,";
                sql += "rec_RefNum_Ours,";
                sql += "rec_TrandInd ";
                sql += "from tbl_APTrandetails ";
                sql += "where rec_TrxnRefNum in (" + Param + ")";

                DataSet ds = com.getDataSet(com.getSqlCommand(sql, false));

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {
                    if (com.Message != null && com.Message != String.Empty)
                    {
                        throw new Exception(com.Message);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            finally
            {
                if (com != null) { com = null; }
            }

            return null;
        }

        private bool update_status_from_grs(DataSet GRSds, DataSet IRSWds, out String errmsg)
        {
            errmsg = "";
            irsSqlCommonsWs com = new irsSqlCommonsWs();
            try
            {
                String prodCode = "";
                String seq_id = "";
                String IPA = "";
                String File_ind = "";
                String fc_status = "";
                String errmsg1 = "";
                String errmsg2 = "";
                for (int i = 0; i < IRSWds.Tables[0].Rows.Count; i++)
                {
                    for (int j = 0; j < GRSds.Tables[0].Rows.Count; j++)
                    {
                        if (IRSWds.Tables[0].Rows[i]["IRSWRefNo"].ToString() == GRSds.Tables[0].Rows[j]["trxnref_num"].ToString())
                        {
                            prodCode = IRSWds.Tables[0].Rows[i]["Prod_Code"].ToString().Trim();
                            seq_id = IRSWds.Tables[0].Rows[i]["Seq_ID"].ToString().Trim();
                            IPA = GRSds.Tables[0].Rows[j]["IPA"].ToString().Trim();
                            File_ind = GRSds.Tables[0].Rows[j]["file_ind"].ToString().Trim();
                            fc_status = GRSds.Tables[0].Rows[j]["fc_status"].ToString().Trim();

                            if (prodCode == "01")
                            {
                                if (fc_status == "2")
                                {
                                    update(seq_id, "02", out errmsg1); // Credited Online
                                }
                                else if (IPA == "414" | IPA == "400")
                                {
                                    update(seq_id, "88", out errmsg1); // On Hold
                                }
                                //else if (IPA == "") // Rejected is temporarily not used
                                //{
                                //    update(seq_id, "99", out errmsg1); // Rejected
                                //}
                            }
                            else if(prodCode == "02")
                            {
                                if (File_ind != "")
                                {
                                    update(seq_id, "04", out errmsg1); // Processed non online
                                }
                            }
                            else if (prodCode == "04")
                            {   
                                if (File_ind != "")
                                {
                                    update(seq_id, "04", out errmsg1); // Processed non online
                                }
                            }

                            if (errmsg1 != "")
                            {
                                errmsg2 += IRSWds.Tables[0].Rows[i]["IRSWRefNo"].ToString() + "~" + errmsg1 + "|";
                            }
                        }
                    }
                }

                if (errmsg2 != "")
                {
                    throw new Exception(errmsg2);
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                errmsg = "Error updating record : " + ex.Message;
            }
            finally
            {
                if (com != null) { com = null; }
            }

            return false;
        }

        private bool update_status_from_apas(DataSet APASds, DataSet IRSWds, out String errmsg)
        {
            errmsg = "";
            irsSqlCommonsWs com = new irsSqlCommonsWs();
            try
            {
                String rec_TrandInd = "";
                String seq_id = "";
                String prodCode = "";

                String errmsg1 = "";
                String errmsg2 = "";
                for (int i = 0; i < IRSWds.Tables[0].Rows.Count; i++)
                {
                    for (int j = 0; j < APASds.Tables[0].Rows.Count; j++)
                    {
                        if (IRSWds.Tables[0].Rows[i]["IRSWRefNo"].ToString() == APASds.Tables[0].Rows[j]["rec_TrxnRefNum"].ToString())
                        {
                            rec_TrandInd = APASds.Tables[0].Rows[j]["rec_TrandInd"].ToString().Trim();
                            seq_id = IRSWds.Tables[0].Rows[i]["Seq_ID"].ToString().Trim();
                            prodCode = IRSWds.Tables[0].Rows[i]["Prod_Code"].ToString().Trim();

                            if (prodCode == "03")
                            {
                                if (rec_TrandInd == "FOR DISPOSITION" | rec_TrandInd == "NP")
                                {
                                    update(seq_id, "07", out errmsg1); // Money Ready for Pickup
                                }
                                else if (rec_TrandInd == "P")
                                {
                                    update(seq_id, "06", out errmsg1); // Paid to Customer
                                }
                                else if (rec_TrandInd == "C")
                                {
                                    update(seq_id, "99", out errmsg1); // Transaction was cancelled
                                }
                            }

                            if (errmsg1 != "")
                            {
                                errmsg2 += IRSWds.Tables[0].Rows[i]["IRSWRefNo"].ToString() + "~" + errmsg1 + "|";
                            }
                        }
                    }
                }

                if (errmsg2 != "")
                {
                    throw new Exception(errmsg2);
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                errmsg = "Error updating record : " + ex.Message;
            }
            finally
            {
                if (com != null) { com = null; }
            }

            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String create_response_file(String FileName)
        {
            try
            {
                String msg = "";
                if (new irsEXCOAlRajhiResponse().create_response_file(FileName, out msg))
                {
                    return msg;
                }
                else
                {
                    throw new Exception(msg);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create file, Reason : " + ex.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_viewlist()
        {
            String sql_string = "select FldCode, FldDescription from tbl_lookup where fldgroup = 'ALRAJHI_VIEWCODES'";
            sqlHelperWs sql = new sqlHelperWs();
            DataSet ds = sql.getDataSet(sql.getSqlCommand(sql_string, false));
            return ds;
        }

        
        protected void buttonDL_Click(object sender, EventArgs e)
        {
            String res = "";
            String filename = this.HPath.Value;
            
            try
            {
                res = create_response_file(filename);

                String msg = String.Empty;
                if (!download_File(res, out msg))
                {
                    throw new Exception(msg);
                }
            }
            catch(Exception ex)
            {
                new irsEXCOAlRajhiResponse().Show("Failed to Generate Respose For File : " + filename + ", Reason : " + ex.Message);
                //this.Label1.Text = "Failed to Generate Respose For File : " + filename + ", Reason : " + ex.Message;
            }
        }

        public bool download_File(String Path, out String msg)
        {
            msg = String.Empty;
            try
            {
                csFileIo fio = new csFileIo();
                String fname = fio.getFilename(Path);
                
                HttpResponse response = Context.Response;

                if (fio.exist(Path))
                {
                    response.ContentType = "application/zip";
                    response.AddHeader("content-Disposition", "attachment;filename=" + fname);
                    response.WriteFile(Path);
                    response.Flush();
                    response.Close();

                    fio.delete(Path);
                    
                    return true;
                }
                else { msg = "Error file: " + fname + " does not exist."; }
            }
            catch (Exception e) 
            { 
                msg = e.Message; 
            }
            return false;
        }


    }
}
