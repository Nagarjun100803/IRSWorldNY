using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.SessionState;


namespace IRSClassLib
{
    public class irsCIFPhonetics
    {
        private irsSqlCommonsWs com = new irsSqlCommonsWs();


        public void refresh_table(string strBranchCode)
        {
            string s = null;

            s = "DELETE " +
                "FROM TBL_CONVERT_CIF " +
                "WHERE BranchCode = (SELECT distinct substring(fldcode, 1, 2) " +
					                "FROM tbl_lookup " +
                                    "WHERE fldgroup = 'overseasgroup' " +
					                "AND substring(fldcode, 4, 3) = '" + strBranchCode + "')";
    
            com.execScalar(s);

            s = "INSERT INTO TBL_CONVERT_CIF " +
                "SELECT  " +
                            "tcc.cifclass						    AS	CIFClass, " +
                            "SUBSTRING (tl.FldCode, 1, 2) 			AS 	BranchCode, " +
                            "tcm.cifid                              AS  CIF_ID, " +
		                    "ISNULL(tcm.LastName, '') AS LastName, " +
		                    "ISNULL(tcm.FirstName, '') AS FirstName, " +
		                    "ISNULL(tcm.MiddleName,'') AS MiddleName " +
                "FROM 		    TBL_CIF_MASTER  tcm " +
                "INNER JOIN 	TBL_CIF_CLASS	tcc	    ON	tcm.cifid       = tcc.cifid " +
                                                     "AND tcc.cifclass    IN (1,2) " +
                "INNER JOIN 	TBL_LOOKUP	    tl	    ON 	tcm.BRANCHCODE  = SUBSTRING (tl.FLDCODE, 4, 3) " +
                                                     "AND tl.FLDGROUP     = 'OVERSEASGROUP' " +
						                      "AND SUBSTRING (tl.FLDCODE, 4, 3) = '" + strBranchCode + "' " +
                "WHERE	    LTRIM(RTRIM(tcm.FirstName)) <>  '' " +
                  "AND	LTRIM(RTRIM(tcm.LastName))  <>  '' " +
                  "AND	tcm.Firstname               IS  NOT NULL " +
                  "AND 	tcm.LastName                IS  NOT NULL " +
                  "AND 	tcm.Deleted                 =   0 " +
                  "AND	tcm.cifid 			        NOT IN (SELECT DISTINCT CIFID " +
                                                      "FROM tbl_cif_soundex)";


            com.execScalar(s);            
        }

        public string count_not_ConvertedCIF(string strBranchCode)
        {
            string s = null;

            s = "SELECT COUNT(*) " +
                "FROM       tbl_convert_CIF cf " +
                "INNER JOIN TBL_LOOKUP lp ON substring(lp.fldcode, 1, 2) = cf.BranchCode " +
                                                "AND lp.fldgroup = 'overseasgroup' " +
                                                "AND substring(lp.fldcode, 4, 3) = '" + strBranchCode + "' " +
                "WHERE cifid NOT IN (SELECT " +
                                    "DISTINCT CIFID " +
                                    "FROM TBL_CIF_SOUNDEX) ";

            DataTable dt = new DataTable("cif");
            dt = getDataTable(s);
            return dt.Rows[0][0].ToString();
            
        }

        public int convert_CIF_Phonetics(int noToConvert, string strBranchCode)
        {
            string s = null;
            int intTotalSuccessfulCount = 0;
    
            s = "SELECT TOP " + noToConvert.ToString() + " cf.* " +
                "FROM tbl_convert_CIF   cf " +
                "INNER JOIN TBL_LOOKUP lp ON substring(lp.fldcode, 1, 2) = cf.BranchCode " +
                                            "AND lp.fldgroup = 'overseasgroup' " +
                                            "AND substring(lp.fldcode, 4, 3) = '" + strBranchCode + "' " +
                "WHERE cifid NOT IN (SELECT " +
                                    "DISTINCT CIFID " +
                                    "FROM TBL_CIF_SOUNDEX " +
                                    "WHERE Branchcode = cf.BranchCode) ";

            string SQLINSERT = null;
            string SQLADD = null;

            DataTable dt = new DataTable("cif");
            dt = getDataTable(s);

            string CIFID = null;
            int CIFCLASS = 0;
            string BranchCode = null;

            string FirstName = null;
            string LastName = null;
            string MiddleName = null;

            string firstnamesoundex = null;
            string lastnamesoundex = null;
            string middlenamesoundex = null;
            string soundexfinalvalue = null;
            irsMethaphone o;
            foreach (DataRow r in dt.Rows)
            {
                o = new irsMethaphone();
                CIFID = (string)r["CIFID"];
                CIFCLASS = Convert.ToInt16(r["CIFCLASS"]);
                BranchCode = (string)r["BranchCode"];
                FirstName = (string)r["FirstName"];
                LastName = (string)r["LastName"];
                MiddleName = (string)r["MiddleName"];
                intTotalSuccessfulCount++;

                for (int a = 0; a <= 2; a++)
                {
                    try
                    {
                        if (a == 0)
                        {
                            if (FirstName.Trim().Length > 0)
                            {
                                firstnamesoundex = o.DoubleMethaphone(FirstName.Trim());
                            }
                            else
                            {
                                firstnamesoundex = "~";
                            }
                        }
                        if (a == 1)
                        {
                            if (LastName.Trim().Length > 0)
                            {
                                lastnamesoundex = o.DoubleMethaphone(LastName.Trim());
                            }
                            else
                            {
                                lastnamesoundex = "~";
                            }
                        }
                        if (a == 2)
                        {
                            if (MiddleName.Trim().Length > 0)
                            {
                                middlenamesoundex = o.DoubleMethaphone(MiddleName.Trim());
                            }
                            else
                            {
                                middlenamesoundex = "~";
                            }
                        }
                    }
                    catch //(Exception e)
                    {
                        return intTotalSuccessfulCount--;
                    }
                }

                for (int UnifiedWatchlistCode = 1; UnifiedWatchlistCode <= 8; UnifiedWatchlistCode++)
                {
                    if (UnifiedWatchlistCode == 1)
                    {
                        soundexfinalvalue = firstnamesoundex + middlenamesoundex + lastnamesoundex;
                    }
                    else if (UnifiedWatchlistCode == 2)
                    {
                        soundexfinalvalue = firstnamesoundex + lastnamesoundex + middlenamesoundex;
                    }
                    else if (UnifiedWatchlistCode == 3)
                    {
                        soundexfinalvalue = lastnamesoundex + firstnamesoundex + middlenamesoundex;
                    }
                    else if (UnifiedWatchlistCode == 4)
                    {
                        soundexfinalvalue = lastnamesoundex + middlenamesoundex + firstnamesoundex;
                    }
                    else if (UnifiedWatchlistCode == 5)
                    {
                        soundexfinalvalue = middlenamesoundex + firstnamesoundex + lastnamesoundex;
                    }
                    else if (UnifiedWatchlistCode == 6)
                    {
                        soundexfinalvalue = middlenamesoundex + lastnamesoundex + firstnamesoundex;
                    }
                    else if (UnifiedWatchlistCode == 7)
                    {
                        soundexfinalvalue = lastnamesoundex + firstnamesoundex;
                        UnifiedWatchlistCode = 13;
                    }
                    else if (UnifiedWatchlistCode == 8)
                    {
                        soundexfinalvalue = firstnamesoundex + lastnamesoundex;
                        UnifiedWatchlistCode = 14;
                    }

                    if (soundexfinalvalue.Replace("~", "").Trim().Length > 0) //(soundexfinalvalue.Trim().Length != 0)
                    {
                        try
                        {

                            SQLINSERT = "INSERT INTO tbl_cif_soundex (CIFID, " +
                                                                     "CIFCLASS, " +
                                                                     "SoundExValue, " +
                                                                     "UnifiedWatchlistValue, " +
                                                                     "BranchCode) " +
                                        "VALUES('" + CIFID + "'," +
                                                    CIFCLASS + ",'" +
                                                    soundexfinalvalue + "'," +
                                                    UnifiedWatchlistCode + ", '" +
                                                    BranchCode + "') ";

                            com.execScalar(SQLINSERT);
                        }
                        catch //(Exception e)
                        {
                            return intTotalSuccessfulCount;
                        }
                    }

                    if (UnifiedWatchlistCode == 13)
                    {
                        UnifiedWatchlistCode = 7;
                    }
                    else if (UnifiedWatchlistCode == 14)
                    {
                        UnifiedWatchlistCode = 8;
                    }
                    
                }
                o = null;

            }

            try
            {
                //INSERT MATCH CIF AND WATCHLISTED 
                SQLADD = "INSERT INTO tbl_cifhitlist " + 
                            "(CIFID, " + 
                            "FileFormat, " + 
                            "isDeleted, " + 
                            "FullnameCif, " + 
                            "FullnameUni, " + 
                            "BranchCode) " + 
                        "SELECT " + 
                            "A.CIFID as [CIFID], " + 
                            "D.Fileformat, " + 
                            "0, " + 
                            "C.FullName as [FullnameCif], " + 
                            "D.FullName as [FullnameUni], " + 
                            "A.BranchCode " + 
                        "FROM  tbl_cif_soundex              A " + 
                        "INNER JOIN tbl_compliance_soundex  B ON  B.soundexvalue = A.soundexvalue " + 
                                                             "AND B.UnifiedWatchlistValue = A.UnifiedWatchlistValue " + 
                                                             "AND B.BranchCode = A.BranchCode " + 
                        "INNER JOIN tbl_cif_master          C ON  C.CIFID = A.CIFID " + 
                        "INNER JOIN tbl_unifiedwatchlist    D ON  D.SEQ_ID = B.SEQ_ID_Unified_Table " + 
                        "WHERE A.CIFID NOT IN ( " + 
                                              "SELECT distinct CIFID " + 
                                              "FROM tbl_cifhitlist " + 
                                              "WHERE isdeleted = 0 " + 
                                              "AND FileFormat = B.FileFormat " + 
                                              "AND UnifiedWatchlistValue = B.UnifiedWatchlistValue " +
                                              "AND BranchCode = B.BranchCode " + 
                                              ")";
                com.execScalar(SQLADD);                
            }
            catch //(Exception e)
            {
                return intTotalSuccessfulCount;
            }

            try
            {
                //INSERT MATCH CIF AND WATCHLISTED 
                SQLADD = "DELETE " +
                        "FROM tbl_convert_cif " +
                        "WHERE cifid IN ( " +
		                                "SELECT DISTINCT CIFID " +
		                                "FROM TBL_CIF_SOUNDEX " +
                                        "WHERE Branchcode = '" + BranchCode + "' " +
		                                ") " +
                        "AND BranchCode = '" + BranchCode + "'";
                com.execScalar(SQLADD);                
            }
            catch //(Exception e)
            {
                return intTotalSuccessfulCount;
            }

            return intTotalSuccessfulCount;
        }

        private DataTable getDataTable(String SQL)
        {
            try
            {
                irsSqlCommonsWs com = new irsSqlCommonsWs();
                DataSet ds = com.getDataSet(com.getSqlCommand(SQL, false), "RemBenNames");

                return ds.Tables[0];
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get datatable: " + e.Message);
            }
        }
    }
}