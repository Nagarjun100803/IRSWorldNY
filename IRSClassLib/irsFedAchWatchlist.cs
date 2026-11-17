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
    public class irsFedAchWatchlist : sqlHelperWs
    {
        string User_ID = "";

        public DataSet Get_File_List(string datefrom, string dateto, string branchcode, string UserID)
        {
            csAuditLog writelog = new csAuditLog();
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            DataSet dsfilelist = new DataSet();

            try
            {
                dsfilelist = commons.execStoredProcedure("proc_Get_FedAch_Watchlist_Files", new Object[] { datefrom, dateto, branchcode });
                if (dsfilelist != null)
                {
                    if (dsfilelist.Tables[0].Rows.Count > 0)
                    {
                        return dsfilelist;
                    }
                }
            }
            catch (Exception e)
            {
                writelog.writeAudit(UserID, "FedAch Viewer", "Error in retrieving FEDACH Files. For Transaction Date : " + datefrom + " to " + dateto + e.Message.ToString() + e.StackTrace.ToString(), "", "", csAuditLog.Category.Select);
            }
            finally
            {
                dsfilelist = null;
                commons = null;
                writelog = null;
            }
            return null;
        }

        public DataSet Get_FedAch_CIF_to_Match(String File_Name, String Branch_Code, String UserID, int report_type)
        {
            User_ID = UserID;
            csAuditLog writelog = new csAuditLog();
            irsSqlCommonsWs commons = new irsSqlCommonsWs();

            string CIF_ID = "";
            string CIF_LastName = "";
            string CIF_FirstName = "";
            string CIF_MiddleName = "";
            string BranchCode = "";

            DataSet dsCIF = new DataSet();
            DataTable dtCIF = new DataTable();
            DataSet dsMatched_CIF = new DataSet();

            try
            {
                /* //Comment out code : New CIF matching for FedACH is done upon loading of file
                dsCIF = commons.execStoredProcedure("proc_Get_FedAch_NEW_CIF_to_Match", new Object[] { File_Name, Branch_Code });
                dtCIF = dsCIF.Tables[0];

                if (dtCIF != null)
                {
                    if (dtCIF.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtCIF.Rows.Count; i++)
                        {
                            CIF_ID = dtCIF.Rows[i][0].ToString();
                            CIF_FirstName = dtCIF.Rows[i][1].ToString();
                            CIF_MiddleName = dtCIF.Rows[i][2].ToString();
                            CIF_LastName = dtCIF.Rows[i][3].ToString();
                            BranchCode = dtCIF.Rows[i][4].ToString();

                            //Comment out to insert New CIF in New CIF Watchlist Table 06/09/2016
                            //this.get_hitlist(CIF_ID, CIF_FirstName, CIF_LastName, CIF_MiddleName, BranchCode);
                             
                            this.insert_FedAch_New_CIF(CIF_ID, CIF_LastName, CIF_FirstName, CIF_MiddleName, BranchCode);
                        }
                        writelog.writeAudit(UserID, "FedAch Viewer", "Successfully matched FedAch File " + File_Name + " against Watchlist CIF.", "", "", csAuditLog.Category.Insert);
                    }
                }
                 dsMatched_CIF = Get_Matched_CIF(File_Name, Branch_Code, User_ID);*/

                if (report_type == 1)
                {
                    dsMatched_CIF = Get_Matched_CIF_Regular(File_Name, Branch_Code, User_ID);
                }
                else if (report_type == 2)
                {
                    dsMatched_CIF = Get_Matched_CIF_Alias(File_Name, Branch_Code, User_ID);
                }
                else if (report_type == 3)
                {
                    dsMatched_CIF = Get_Matched_CIF_Exception(File_Name, Branch_Code, User_ID);
                }

                return dsMatched_CIF;
            }
            catch (Exception e)
            {
                writelog.writeAudit(UserID, "FedAch Viewer", "Error in matching FedAch File " + File_Name + " against Watchlist CIF. " + e.Message.ToString() + e.StackTrace.ToString() , "", "", csAuditLog.Category.Insert);
            }
            finally 
            {
                dsCIF = null;
                dtCIF = null;
                commons = null;
                writelog = null;
            }
            return null;
        }

        private void insert_FedAch_New_CIF(String CIFID, String LastName, String FirstName, String MiddleName, String BranchCode)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            csAuditLog writelog = new csAuditLog();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    int res = commons.execStoredProcedureUpdate("proc_FEDACH_Insert_NEW_CIF_Watchlist", new Object[] { CIFID, LastName, FirstName, MiddleName, BranchCode });
                    if (res == -1)
                    {
                        writelog.writeAudit(User_ID, "FedAch Viewer", commons.Message, "proc_FEDACH_Insert_NEW_CIF_Watchlist : " + CIFID + "," + LastName + "," + FirstName + "," + MiddleName + "," + BranchCode, "", csAuditLog.Category.Insert);
                    }
                }
            }
            catch (Exception ex)
            {
                writelog.writeAudit(User_ID, "FedAch Viewer", "Error in saving FedAch New CIF in New CIF Watchlist table. " + ex.Message.ToString() + ex.StackTrace.ToString(), "proc_Insert_FedAch_Watchlist_Matched : " + CIFID, "", csAuditLog.Category.Insert);
            }
            finally
            {
                if (commons != null) { commons = null; }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
                writelog = null;
            }
        }

        private DataSet Get_Matched_CIF(String File_Name, String Branch_Code, String UserID)
        {
            csAuditLog writelog = new csAuditLog();
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            DataSet ds_MatchedCIF = new DataSet();

            try
            {
                ds_MatchedCIF = commons.execStoredProcedure("rpt_Get_FedACH_Watchlist", new Object[] { "1900-01-01", "1900-01-01", File_Name, Branch_Code });
                if (ds_MatchedCIF != null)
                {
                    if (ds_MatchedCIF.Tables[0].Rows.Count > 0)
                    {
                        return ds_MatchedCIF;
                    }
                }
            }
            catch (Exception e)
            {
                writelog.writeAudit(UserID, "FedAch Viewer", "Error in retrieving Matched CIF for FileName " + File_Name + ". " + e.Message.ToString() + e.StackTrace.ToString(), "", "", csAuditLog.Category.Select);
            }
            finally
            {
                ds_MatchedCIF = null;
                commons = null;
                writelog = null;
            }
            return null;
        }

        private DataSet Get_Matched_CIF_Regular(String File_Name, String Branch_Code, String UserID)
        {
            csAuditLog writelog = new csAuditLog();
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            DataSet ds_MatchedCIF = new DataSet();

            try
            {
                ds_MatchedCIF = commons.execStoredProcedure("rpt_Get_FedACH_Watchlist_Regular", new Object[] { "1900-01-01", "1900-01-01", File_Name, Branch_Code });
                if (ds_MatchedCIF != null)
                {
                    if (ds_MatchedCIF.Tables[0].Rows.Count > 0)
                    {
                        return ds_MatchedCIF;
                    }
                }
            }
            catch (Exception e)
            {
                writelog.writeAudit(UserID, "FedAch Viewer", "Error in retrieving Matched CIF for FileName " + File_Name + ". " + e.Message.ToString() + e.StackTrace.ToString(), "", "", csAuditLog.Category.Select);
            }
            finally
            {
                ds_MatchedCIF = null;
                commons = null;
                writelog = null;
            }
            return null;
        }

        private DataSet Get_Matched_CIF_Alias(String File_Name, String Branch_Code, String UserID)
        {
            csAuditLog writelog = new csAuditLog();
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            DataSet ds_MatchedCIF = new DataSet();

            try
            {
                ds_MatchedCIF = commons.execStoredProcedure("rpt_Get_FedACH_Watchlist_Alias", new Object[] { "1900-01-01", "1900-01-01", File_Name, Branch_Code });
                if (ds_MatchedCIF != null)
                {
                    if (ds_MatchedCIF.Tables[0].Rows.Count > 0)
                    {
                        return ds_MatchedCIF;
                    }
                }
            }
            catch (Exception e)
            {
                writelog.writeAudit(UserID, "FedAch Viewer", "Error in retrieving Matched CIF for FileName " + File_Name + ". " + e.Message.ToString() + e.StackTrace.ToString(), "", "", csAuditLog.Category.Select);
            }
            finally
            {
                ds_MatchedCIF = null;
                commons = null;
                writelog = null;
            }
            return null;
        }

        private DataSet Get_Matched_CIF_Exception(String File_Name, String Branch_Code, String UserID)
        {
            csAuditLog writelog = new csAuditLog();
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            DataSet ds_MatchedCIF = new DataSet();

            try
            {
                ds_MatchedCIF = commons.execStoredProcedure("rpt_Get_FedACH_Watchlist_Exception", new Object[] { "1900-01-01", "1900-01-01", File_Name, Branch_Code });
                if (ds_MatchedCIF != null)
                {
                    if (ds_MatchedCIF.Tables[0].Rows.Count > 0)
                    {
                        return ds_MatchedCIF;
                    }
                }
            }
            catch (Exception e)
            {
                writelog.writeAudit(UserID, "FedAch Viewer", "Error in retrieving Matched CIF for FileName " + File_Name + ". " + e.Message.ToString() + e.StackTrace.ToString(), "", "", csAuditLog.Category.Select);
            }
            finally
            {
                ds_MatchedCIF = null;
                commons = null;
                writelog = null;
            }
            return null;
        }

        /*
        private void get_hitlist(String CIFID, String FirstName, String LastName, String MiddleName, String branchCode)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            csAuditLog writelog = new csAuditLog();

            string strNewEnrolledCIFPhonetics1 = "";
            string strNewEnrolledCIFPhonetics2 = "";
            string strNewEnrolledCIFPhonetics3 = "";
            string strNewEnrolledCIFPhonetics4 = "";
            string strNewEnrolledCIFPhonetics5 = "";
            string strNewEnrolledCIFPhonetics6 = "";
            string strNewEnrolledCIFPhonetics13 = "";
            string strNewEnrolledCIFPhonetics14 = "";

            try
            {
                DataSet dsp = commons.execStoredProcedure("proc_get_branch_setup_matching", new Object[] { branchCode });
                DataTable dtp = new DataTable();
                dtp = dsp.Tables[0];

                //For Percentage Matching Criteria
                if (dtp.Select("fldcode in ('UWPM', 'UW')").Length > 0)
                {
                    irsSqlCommonsWs getDistinctComplianceNames = new irsSqlCommonsWs();
                    DataSet dsComplianceNames = getDistinctComplianceNames.execStoredProcedure("proc_get_compliance_names", new Object[] { branchCode });
                    DataTable dtComplianceNames = new DataTable();
                    dtComplianceNames = dsComplianceNames.Tables[0];

                    if (dtComplianceNames.Rows.Count != 0)
                    {
                        string uwfirstname = "";
                        string uwlastname = "";
                        string uwmiddlename = "";
                        string uwaliasfirstname = "";
                        string uwaliaslastname = "";
                        string uwaliasmiddlename = "";
                        string uwfileformat = "";
                        string v_pscore = "";
                        string uw_seqid = "";

                        irsSqlCommonsWs cmns = new irsSqlCommonsWs();
                        DataSet dsps = cmns.execStoredProcedure("proc_get_branch_setup_percentagescore", new Object[] { branchCode });
                        DataTable dtps = new DataTable();

                        dtps = dsps.Tables[0];
                        v_pscore = dtps.Rows[0]["flddescription"].ToString();

                        for (int i = 0; i < dtComplianceNames.Rows.Count; i++)
                        {
                            uwfirstname = dtComplianceNames.Rows[i]["FirstName"].ToString();
                            uwlastname = dtComplianceNames.Rows[i]["LastName"].ToString();
                            uwmiddlename = dtComplianceNames.Rows[i]["MiddleName"].ToString();
                            uwaliasfirstname = dtComplianceNames.Rows[i]["AliasFirstName"].ToString();
                            uwaliaslastname = dtComplianceNames.Rows[i]["AliasLastName"].ToString();
                            uwaliasmiddlename = dtComplianceNames.Rows[i]["AliasMiddleName"].ToString();
                            uwfileformat = dtComplianceNames.Rows[i]["FileFormat"].ToString();
                            uw_seqid = dtComplianceNames.Rows[i]["uw_seqid"].ToString();

                            string uwname = uwfirstname.ToString() + " " + uwmiddlename.ToString() + " " + uwlastname.ToString();
                            string cifname = FirstName.ToString() + " " + MiddleName.ToString() + " " + LastName.ToString();
                            string uwaliasfullname = uwaliaslastname.ToString() + " " + uwaliasfirstname.ToString() + "" + uwaliasmiddlename.ToString();


                            MatchsMaker match = new MatchsMaker(uwname.ToUpper(), cifname.ToUpper());
                            float pscore = (match.Score);

                            if (pscore >= Convert.ToInt32(v_pscore))
                            {
                                if (cifname.Trim() != "" && uwname.Trim() != "")
                                {
                                    this.insert_watchlist(CIFID, uw_seqid, uwfileformat, cifname, uwname, branchCode, "Any Combination in Firstname, Lastname, Middlename (Percentage)", "0", pscore.ToString(), "UWPM");
                                }
                            }

                            //For Aliases
                            string a_uwname = uwaliasfirstname.ToString() + " " + uwaliasmiddlename.ToString() + " " + uwaliaslastname.ToString();

                            MatchsMaker a_match = new MatchsMaker(a_uwname, cifname);
                            float a_pscore = (a_match.Score);

                            if (a_pscore >= Convert.ToInt32(v_pscore))
                            {
                                if (cifname.Trim() != "" && a_uwname.Trim() != "")
                                {
                                    this.insert_watchlist(CIFID, uw_seqid, uwfileformat, cifname, a_uwname, branchCode, "Any Combination in Firstname, Lastname, Middlename (Percentage)", "0", pscore.ToString(), "UWPM");
                                }
                            }
                        }
                    }
                }

                //For NY-Percentage Matching Criteria (Fuzzy Logic)
                else if (dtp.Select("fldcode in ('UWFL', 'UW')").Length > 0)
                {
                    irsSqlCommonsWs getDistinctComplianceNames = new irsSqlCommonsWs();
                    DataSet dsComplianceNames = getDistinctComplianceNames.execStoredProcedure("proc_get_compliance_names", new Object[] { branchCode });
                    DataTable dtComplianceNames = new DataTable();
                    dtComplianceNames = dsComplianceNames.Tables[0];

                    if (dtComplianceNames.Rows.Count != 0)
                    {
                        string uwfirstname = "";
                        string uwlastname = "";
                        string uwmiddlename = "";
                        string uwaliasfirstname = "";
                        string uwaliaslastname = "";
                        string uwaliasmiddlename = "";
                        string uwfileformat = "";
                        string v_pscore = "";
                        string uw_seqid = "";

                        irsSqlCommonsWs cmns = new irsSqlCommonsWs();
                        DataSet dsps = cmns.execStoredProcedure("proc_get_branch_setup_percentagescore", new Object[] { branchCode });
                        DataTable dtps = new DataTable();

                        dtps = dsps.Tables[0];
                        v_pscore = dtps.Rows[0]["flddescription"].ToString();

                        for (int i = 0; i < dtComplianceNames.Rows.Count; i++)
                        {
                            uwfirstname = dtComplianceNames.Rows[i]["FirstName"].ToString();
                            uwlastname = dtComplianceNames.Rows[i]["LastName"].ToString();
                            uwmiddlename = dtComplianceNames.Rows[i]["MiddleName"].ToString();
                            uwaliasfirstname = dtComplianceNames.Rows[i]["AliasFirstName"].ToString();
                            uwaliaslastname = dtComplianceNames.Rows[i]["AliasLastName"].ToString();
                            uwaliasmiddlename = dtComplianceNames.Rows[i]["AliasMiddleName"].ToString();
                            uwfileformat = dtComplianceNames.Rows[i]["FileFormat"].ToString();
                            uw_seqid = dtComplianceNames.Rows[i]["uw_seqid"].ToString();

                            //Watchlist Name
                            string uwname = uwfirstname.ToString() + " " + uwmiddlename.ToString() + " " + uwlastname.ToString();
                            //CIF Name
                            string cifname = FirstName.ToString() + " " + MiddleName.ToString() + " " + LastName.ToString();
                            //Alias
                            string a_uwname = uwaliaslastname.ToString();

                            //used for Jaro (without spaces)
                            string uwname_j = uwname.Trim(' ').Replace(" ", "");
                            string cifname_j = cifname.Trim(' ').Replace(" ", "");
                            string a_uwname_j = a_uwname.Trim(' ').Replace(" ", "");

                            MatchsMakerFL match = new MatchsMakerFL(uwname.ToString(), cifname.ToString());
                            float uw_pscore = (match.Score);
                            if (uw_pscore >= Convert.ToInt32(v_pscore)) 
                            {
                                if (cifname.Trim() != "" && uwname.Trim() != "")
                                {
                                    this.insert_watchlist(CIFID, uw_seqid, uwfileformat, cifname, uwname, branchCode, "Any Combination in Firstname, Lastname, Middlename (Fuzzy)", "0", uw_pscore.ToString(), "UWFL");
                                }
                            }
                            else
                            {
                                double auw_drate = JaroWinkler.RateSimilarity(uwname_j.ToString(), cifname_j.ToString());
                                float auw_pscore_j = ((float)auw_drate) * 100;
                                if (auw_pscore_j >= Convert.ToInt32(v_pscore))
                                {
                                    if (cifname_j.Trim() != "" && uwname_j.Trim() != "")
                                    {
                                        this.insert_watchlist(CIFID, uw_seqid, uwfileformat, cifname, uwname, branchCode, "Any Combination in Firstname, Lastname, Middlename (Fuzzy)", "0", auw_pscore_j.ToString(), "UWFL");
                                    }
                                }
                            }

                            //For Aliases (LN+FN+MN)

                            MatchsMakerFL a_match = new MatchsMakerFL(a_uwname.ToString(), cifname.ToString());
                            float a_pscore_j = (a_match.Score);
                            if (a_pscore_j >= Convert.ToInt32(v_pscore))
                            {
                                if (cifname.Trim() != "" && a_uwname.Trim() != "")
                                {
                                    this.insert_watchlist(CIFID, uw_seqid, uwfileformat, cifname, a_uwname, branchCode, "Any Combination in Firstname, Lastname, Middlename (Fuzzy)", "0", a_pscore_j.ToString(), "UWFL");
                                }
                            }
                            else
                            {
                                double al_drate = JaroWinkler.RateSimilarity(a_uwname_j.ToString(), cifname_j.ToString());
                                float al_pscore_j = ((float)al_drate) * 100;
                                if (al_pscore_j >= Convert.ToInt32(v_pscore))
                                {
                                    if (cifname_j.Trim() != "" && a_uwname_j.Trim() != "")
                                    {
                                        this.insert_watchlist(CIFID, uw_seqid, uwfileformat, cifname, a_uwname, branchCode, "Any Combination in Firstname, Lastname, Middlename (Fuzzy)", "0", al_pscore_j.ToString(), "UWFL");
                                    }
                                }
                            }
                        }
                    }
                }

                //For UWEM, UWP, UWMEM Matching Criteria
                else
                {
                    irsMethaphone myMethaphone = new irsMethaphone();
                    string strFirstNamePhonetics = "";
                    string strLastNamePhonetics = "";
                    string strMiddleNamePhonetics = "";

                    strFirstNamePhonetics = myMethaphone.DoubleMethaphone(FirstName);
                    strLastNamePhonetics = myMethaphone.DoubleMethaphone(LastName);
                    strMiddleNamePhonetics = myMethaphone.DoubleMethaphone(MiddleName);

                    strNewEnrolledCIFPhonetics1 = strFirstNamePhonetics + strMiddleNamePhonetics + strLastNamePhonetics;
                    strNewEnrolledCIFPhonetics2 = strFirstNamePhonetics + strLastNamePhonetics + strMiddleNamePhonetics;
                    strNewEnrolledCIFPhonetics3 = strLastNamePhonetics + strFirstNamePhonetics + strMiddleNamePhonetics;
                    strNewEnrolledCIFPhonetics4 = strLastNamePhonetics + strMiddleNamePhonetics + strFirstNamePhonetics;
                    strNewEnrolledCIFPhonetics5 = strMiddleNamePhonetics + strFirstNamePhonetics + strLastNamePhonetics;
                    strNewEnrolledCIFPhonetics6 = strMiddleNamePhonetics + strLastNamePhonetics + strFirstNamePhonetics;
                    strNewEnrolledCIFPhonetics13 = strLastNamePhonetics + strFirstNamePhonetics;
                    strNewEnrolledCIFPhonetics14 = strFirstNamePhonetics + strLastNamePhonetics;

                    int o = commons.execStoredProcedureUpdate("sp_save_cif_hitlist_validations", new Object[] { CIFID, strNewEnrolledCIFPhonetics1, strNewEnrolledCIFPhonetics2, strNewEnrolledCIFPhonetics3, strNewEnrolledCIFPhonetics4, strNewEnrolledCIFPhonetics5, strNewEnrolledCIFPhonetics6, strNewEnrolledCIFPhonetics13, strNewEnrolledCIFPhonetics14, branchCode });
                }
            }
            catch (Exception ex)
            {
                writelog.writeAudit(User_ID, "FedAch Viewer", "Error in validating watchlist CIF. " + ex.Message.ToString() + ex.StackTrace.ToString(), "Get_HitList : " + CIFID, "", csAuditLog.Category.Insert);
            }
            finally
            {
                if (commons != null) { commons = null; }
                writelog = null;
            }
        }
        */

        public void insert_watchlist(String CIFID, String UW_SEQ_ID, String FileFormat, String CIFField, String UWField, String BranchCode, String MatchingType, String isExactMatch, String PercentageScore, String WatchListType)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            csAuditLog writelog = new csAuditLog();
            SqlCommand sqcommands = null;
            string ovg_branchcode = "";
            
            try
            {
                if (commons != null)
                {
                    DataTable ovg = commons.execStoredProcedureTable("proc_Get_Overseasgroup_Branchcode", new Object[] { BranchCode });

                    if (ovg != null)
                    {
                        if (ovg.Rows.Count > 0)
                        {
                            ovg_branchcode = ovg.Rows[0][0].ToString();
                        }
                    }

                    int res = commons.execStoredProcedureUpdate("proc_Insert_FedAch_Watchlist_Matched", new Object[] { CIFID, UW_SEQ_ID, FileFormat, CIFField, UWField, ovg_branchcode, MatchingType, isExactMatch, PercentageScore, WatchListType });
                    if (res == -1)
                    {
                        writelog.writeAudit(User_ID, "FedAch Viewer", commons.Message, "proc_Insert_FedAch_Watchlist_Matched : " + CIFID + "," + UW_SEQ_ID + "," + CIFField, "", csAuditLog.Category.Insert);
                    }
                }
            }
            catch (Exception ex)
            {
                writelog.writeAudit(User_ID, "FedAch Viewer", "Error in saving watchlist CIF. " + ex.Message.ToString() + ex.StackTrace.ToString(), "proc_Insert_FedAch_Watchlist_Matched : " + CIFID, "", csAuditLog.Category.Insert);
            }
            finally
            {
                if (commons != null) { commons = null; }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
                writelog = null;
            }
        }

        public int Check_Ongoing_Watchlist(String BranchCode)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            csAuditLog writelog = new csAuditLog();

            DataTable dtFileInv = new DataTable();

            try
            {
                dtFileInv = commons.execStoredProcedureTable("proc_Get_File_Watchlist_Status", new Object[] { BranchCode });

                if (dtFileInv != null)
                {
                    if (dtFileInv.Rows.Count > 0)
                    {
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                writelog.writeAudit(User_ID, "FedAch Viewer", "Error while checking On-going Watchlist matching files. " + ex.Message.ToString() + ex.StackTrace.ToString(), "proc_Get_File_Watchlist_Status : " + BranchCode, "", csAuditLog.Category.Select);
            }
            finally
            {
                dtFileInv = null;
                commons = null;
                writelog = null;
            }
            return 0;
        }
    }
}
