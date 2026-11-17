/* Created by Rogelio D. Guanlao 06/08/2010*/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace IRSClassLib
{
    public class irsPCHCExtract: sqlHelperWs
    {
        csAuditLog audit = new csAuditLog();
        
        public DataSet getExtract(int extractType, String BankCode, int Currency, DateTime TranDate, String FileInd, String SFileInd, out String errMsg)
        {
            DataSet ds = null;
            errMsg = "";
            try
            {                
                ds = execStoredProcedure("PROC_EOT_EXTRACT_PCHC", new object[] { extractType, BankCode, Currency, TranDate, FileInd, SFileInd, errMsg });

                if (errMsg != null && errMsg != String.Empty)
                {
                    throw new Exception(errMsg);
                }

                if (ds != null)
                {
                    return ds;
                }
                else
                {
                    throw new Exception("Dataset is Null");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataTable getLookUp(string fldGroup)
        {
            DataTable dt = null;
            try
            {
                DataSet ds = getDataSet(getSqlCommand("SELECT FldValue,FldDescription,FldCode FROM TBL_LOOKUP WHERE  FldGroup = '" + fldGroup + "'", false));
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public DataTable getFileNo(DateTime trxDate)
        {
            DataTable dt = null;
            try
            {
                dt = execStoredProcedureTable("sp_PCHC_getFileNo", new object[] { trxDate });
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public DataTable getSFileNo(DateTime trxDate, string fileNo, int CurrCd, int ExtractCd, string brCode)
        {
            DataTable dt = null;
            try
            {
                dt = execStoredProcedureTable("sp_PCHC_getSFileNo", new object[] { trxDate, fileNo, CurrCd, ExtractCd, brCode });
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public DataRow getExtDetails(String ExtractType, out string errmsg)
        {
            errmsg = String.Empty;
            DataSet ds = null;
            try
            {
                String sql = String.Empty;
                sql = "sp_getExtDetails";

                ds = execStoredProcedure(sql, new object[] { ExtractType });

                if (ds != null)
                {
                    return ds.Tables[0].Rows[0];
                }
                else
                {
                    if (Message != null && Message != String.Empty) { throw new Exception(Message); }
                    throw new Exception("No dataset returned.");
                }
            }
            catch (Exception e)
            {
                errmsg = "Get Extract Type Detail Error, Reason : " + e.Message;
            }
            return null;
        }

        public bool tagExtracted(DataSet ds, DateTime trandate, string file_ind, out String msg, out String sFileInd, string curr, string extracttype, string bnkcode)
        {
            sFileInd = "";
            try
            {
                msg = "";
                sFileInd = GetSubFileInd(trandate, file_ind, out msg, curr, extracttype, bnkcode).ToString();


                sFileInd = (Convert.ToInt16(sFileInd) == 0) ? "1" : sFileInd;
                if (msg != null && msg != String.Empty) { throw new Exception("Failed to get sub file indicator, Reason : " + msg); }

                for (int i = 1; i < ds.Tables.Count; i++)
                {
                    if (ds.Tables[i].Rows.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                        {
                            if (!tagExt(ds.Tables[i].Rows[j], Convert.ToInt16(sFileInd), trandate, out msg))
                            {
                                throw new Exception(msg);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                msg = e.Message;
            }
            return false;
        }

        private int GetSubFileInd(DateTime trandate, string file_ind, out String errmsg, string curr, string extracttype, string bnkcode)
        {
            errmsg = "";
            try
            {
                String sql = "";

                sql += "select isnull(max(sub_file_ind),'0')";
                sql += "from	v_tbl_remittance_ibatch ";
                sql += "where	TranxDate = '" + trandate.ToString("MM/dd/yyyy") + "' ";
                sql += "and isnull(fileind,'') = '" + file_ind.ToString() + "' ";
                sql += "and PayTypeCode = '3' ";

                if (extracttype == "0")
                {
                    sql += "   and	substring(RemBankCode,1,3)	<>	'000'";
                    sql += "   and	substring(RemBankCode,1,3)	not in 	(";
                    sql += "   				select	substring(fldcode, 1, 3)";
                    sql += "   								from	tbl_lookup";
                    sql += "   								where	fldgroup	=	'GRSXTRCT'";
                    sql += "   								and	substring(fldcode, 1, 3)	<>	'000'	)";
                    sql += "   		and	substring(RemBankCode,1,3)	not in	(";
                    sql += "   								select	SUBSTRING(FLDCODE,5,3)";
                    sql += "   								from	tbl_lookup";
                    sql += "   								where	fldgroup	=	'TIEUP'";
                    sql += "   								)";
                    sql += "  and  acctcurr_cd = '" + curr + "';";
                }
                if (extracttype == "1")
                {
                    string imp_cd = (curr == "PHP") ? "6" : "3";
                    sql += "    and	imp_cd			=	'" + imp_cd + "'";
                    sql += "  and	substring(RemBankCode,1,3)	=	'" + bnkcode + "';";
                }
                if (extracttype == "2")
                {
                    sql += "    and	imp_cd		in ('6','3')	";
                    sql += "  and	substring(RemBankCode,1,3)	=	'" + bnkcode + "';";
                }

                DataSet ds = getDataSet(getSqlCommand(sql, false), "Remittance");

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    int maxsfile = 0;
                    try
                    {
                        maxsfile = int.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1;
                    }
                    catch
                    {
                        maxsfile = 1;
                    }
                    return maxsfile;
                }
            }
            catch (Exception e)
            {
                errmsg = e.Message;
            }
            return 0;
        }

        private bool tagExt(DataRow dr, int subFileInd, DateTime trandate, out String errmsg)
        {
            errmsg = "";
            try
            {                
                SqlCommand cmd = getSqlCommand("proc_tagExtracts");

                Object[] plist = 
                    {
                        trandate,
                        dr["refnum_ours"].ToString(),
                        dr["Trxnref_num"].ToString(),                     
                        subFileInd,
                        dr["SEQ_ID"].ToString(),
                        String.Empty
                    };

                deriveParameters(cmd, plist);
                int ra = sqlUpdate(cmd);
                Object err = (Object)cmd.Parameters["@v_errmsg"].Value;

                if (ra > 0)
                { return true; }

                if (err != DBNull.Value && err.ToString() != String.Empty)
                {
                    throw new Exception(err.ToString());
                }
                else if (Message != null && Message.ToString() != String.Empty)
                {
                    throw new Exception(Message.ToString());
                }
                return true;
            }
            catch (Exception e)
            {
                errmsg = "Failed to Tag Extracted, Reason : " + e.Message;
            }
            return false;
        }

        public String createFileName(DateTime Trandate, String Curr, String Format, String FileInd, String SFileInd, out String errmsg)
        {
            errmsg = String.Empty;
            try
            {
                String[] Ext = null;
                String[] Name = null;
                String[] F = null;
                String Folder = String.Empty;
                String Fname = String.Empty;
                String Ename = String.Empty;

                F = Format.Split(':');
                if (F.Length > 1) { Folder = F[1]; }

                F = F[0].Split('.');
                if (F.Length > 1) { Ext = F[1].Split('~'); }

                F = F[0].Split('~');
                Name = F;

                // File name
                for (int i = 0; i < Name.Length; i++)
                {
                    switch (Name[i])
                    {
                        case "CCC":
                            Fname += Curr;
                            break;
                        case "yyyyMMdd":
                            Fname += Trandate.ToString(Name[i]);
                            break;
                        case "FF":
                            /*string vfileind = FileInd;
                            if (vfileind.Trim().Length == 0)
                            {
                                FileInd = this.get_fileind(Trandate.ToString("MM/dd/yyyy"), FileInd);
                            }
                            if (vfileind.Trim() == "0")
                            {
                                FileInd = this.get_fileind(Trandate.ToString("MM/dd/yyyy"), FileInd);
                            }*/
                            Fname += formatString(FileInd, 2, "0", false);
                            break;
                        case "SS":
                            Fname += formatString(SFileInd, 2, "0", false);
                            break;
                        default:
                            Fname += Name[i];
                            break;
                    }
                }
                // File extension
                for (int i = 0; i < Ext.Length; i++)
                {
                    switch (Ext[i])
                    {
                        case "CCC":
                            Ename += Curr;
                            break;
                        case "yyyyMMdd":
                            Ename += Trandate.ToString(Ext[i]);
                            break;
                        case "FF":
                            string vfileind = FileInd;
                            if (vfileind.Trim().Length == 0)
                            {
                                FileInd = this.get_fileind(Trandate.ToString("MM/dd/yyyy"), FileInd);
                            }
                            if (vfileind.Trim() == "0")
                            {
                                FileInd = this.get_fileind(Trandate.ToString("MM/dd/yyyy"), FileInd);
                            }
                            Ename += formatString(FileInd, 2, "0", false);
                            break;
                        case "SS":

                            Ename += formatString(SFileInd, 2, "0", false);
                            break;
                        default:
                            Ename += Ext[i];
                            break;
                    }
                }

                if (Ename != "") { Ename = "." + Ename; }
                if (Folder != "") { Folder = Folder + "\\"; }

                String msg = "";

                String Dir = getRootDir(out msg);
                if (Dir == "")
                {
                    if (msg != "") { throw new Exception(msg); }
                    else { throw new Exception("No root directory defined."); }
                }

                Dir = prepareDir(Dir + Folder, out msg);
                if (Dir == "")
                {
                    if (msg != "") { throw new Exception(msg); }
                    else { throw new Exception("Prepare directory Failed, Reason : Unknown"); }
                }

                if (!createWorkingFolder(Dir, out msg))
                { throw new Exception(msg); }

                Fname = Dir + Fname + Ename;

                return Fname;
            }
            catch (Exception e)
            {
                errmsg = e.Message;
            }
            return "";
        }

        private String formatString(String s, int maxChar, String paddingChar, bool toRight)
        {
            String S = s.Trim();
            String newS = String.Empty;

            if (S.Length < maxChar)
            {
                int len = maxChar - S.Length;

                if (toRight)
                {
                    newS = S;
                    for (int i = 0; i < len; i++) { newS += paddingChar; }
                }
                else
                {
                    for (int i = 0; i < len; i++) { newS += paddingChar; }
                    newS += S;
                }

                int a = newS.Length;
            }
            else { newS = S.Substring(0, maxChar); }

            return newS;
        }

        private string get_fileind(string dateoftransfer, string origfileind)
        {            
            try
            {
                string sql = "";
                sql += "select max(isnull(file_ind,0)) + 1 ";
                sql += "from tbl_grs_remittance ";
                sql += "where date_of_transfer = '" + dateoftransfer.Trim() + "'";


                DataSet ds = getDataSet(getSqlCommand(sql, false), "BOD");

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                        return ds.Tables[0].Rows[0][0].ToString();
                }
                return origfileind;
            }
            catch
            {
                return origfileind;
            }
            finally
            {
                
            }
        }

        private String getRootDir(out String errmsg)
        {
            errmsg = "";
            irsConfiguration config = new irsConfiguration();
            try
            {                
                return config.appsetting("ExtractRootPath").ToString();
            }
            catch (Exception e)
            {
                errmsg = "Get Root Directory Failed, Reason : " + e.Message;
            }
            return "";

        }

        private String prepareDir(String Dir, out String errmsg)
        {
            errmsg = "";
            try
            {
                String dir = "";
                String[] d = Dir.Replace('/', '\\').Split('\\');

                for (int i = 0; i < d.Length; i++)
                {
                    if (d[i].ToString().Trim() != "")
                    {
                        dir += d[i].ToString().Trim() + "\\";
                    }
                }
                return dir;
            }
            catch (Exception e)
            {
                errmsg = "Preparing directory failed, Reason : " + e.Message;
            }
            return "";
        }

        private bool createWorkingFolder(String Folder, out String errmsg)
        {
            errmsg = "";
            try
            {
                csFileIo fio = new csFileIo();

                if (!fio.directoryExist(Folder))
                {
                    fio.createDirectory(Folder);
                }
            }
            catch (Exception e)
            {
                errmsg = e.Message;
                return false;
            }
            return true;
        }

        public bool createExtractFile(DataTable dt, String curr, String FileName, out String msg)
        {
            msg = String.Empty;
            try
            {
                String data = String.Empty;
                csFileIo fio = new csFileIo();
                int totRec = 0;
                double totAmt = 0;

                if (fio.exist(FileName)) { fio.delete(FileName); }

                if (fio.open_output(FileName, (int)csFileIo.write_modes.CREATE))
                {
                    // create details
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        data = String.Empty;
                        String strRCV_BRSTN = "";
                        if (dt.Rows[i]["receiving_brstn"] == DBNull.Value) { strRCV_BRSTN = "000000000"; }
                        else { strRCV_BRSTN = dt.Rows[i]["receiving_brstn"].ToString(); }

                        String currCD = "";
                        double amt = 0;
                        if (curr == "PHP")
                        {
                            amt = double.Parse(dt.Rows[i]["Net_Peso"].ToString());
                            currCD = "0";
                        }
                        else
                        {
                            amt = double.Parse(dt.Rows[i]["Net_Dollar"].ToString());
                            currCD = "1";
                        }

                        totAmt += amt;
                        totRec++;
                        amt = amt * 100;

                        String Benfname = String.Empty;
                        Benfname += formatString(dt.Rows[i]["Benf_LName"].ToString(), 15, " ", true);
                        Benfname += formatString(dt.Rows[i]["Benf_FName"].ToString(), 15, " ", true);
                        Benfname += formatString(dt.Rows[i]["Benf_Midinit"].ToString(), 1, " ", true);

                        String Spl_Mesg = String.Empty;
                        Spl_Mesg += dt.Rows[i]["PayBank_Name"].ToString();
                        Spl_Mesg += formatString(dt.Rows[i]["Rem_Name"].ToString(), 50, " ", true);
                        Spl_Mesg += dt.Rows[i]["Spl_Mesg"].ToString();

                        data += "I"; // Control Identifier
                        data += DateTime.Now.ToString("yyyMMdd"); // Clearing Date
                        data += "010080010"; // Sending Bank's BRSTN (Format: rrbbbnnnc)
                        data += formatString(dt.Rows[i]["RefNum_Ours"].ToString(), 16, " ", true); // OFI Reference Number
                        data += formatString(" ", 16, " ", true); // RFI Reference Number
                        data += formatString(" ", 16, " ", true); // OFI Customer Reference Number
                        data += formatString(" ", 16, " ", true); // RFI Customer Reference Number
                        data += formatString(strRCV_BRSTN, 9, " ", true); // Receiving Bank / Branch BRSTN(Format: rrbbbnnnc)
                        data += "0"; // Transaction Code: (0 = Regular Credit, 2 = With Collection Notice (PAYMENT ONLY) 9 = Reclamation / Refund)
                        data += currCD; // Currency Code (Value = 0)
                        data += "0"; // Charge Code(0 = No charge, 1 = Charge Payer,2 = Charge Payee)
                        data += formatString(" ", 3, " ", true); // Activity Code
                        data += formatString(amt.ToString(), 14, "0", false); // Amount (2 Decimal Place)
                        data += formatString(dt.Rows[i]["rem_acct_num"].ToString(), 16, "0", true); // Remitter's / Customer's Account Number
                        data += formatString(cleanAcctNum(dt.Rows[i]["Acct_Num"].ToString()), 16, "0", false); // Payer / Beneficiary Account Number(Right Justified)
                        data += formatString(Benfname, 50, " ", true); // Payer / Beneficiary Name
                        data += formatString(dt.Rows[i]["Benf_Address1"].ToString(), 50, " ", true); // Payer / Beneficiary Address
                        data += formatString(dt.Rows[i]["Rem_Name"].ToString(), 50, " ", true); // Remitter's / Customer's Name
                        data += formatString(dt.Rows[i]["rem_address"].ToString(), 50, " ", true); // Remitter's / Customer's Address
                        data += formatString(Spl_Mesg, 139, " ", true); // Instructions (Variable)

                        fio.putLine(data);
                    }
                    // create footer
                    data = String.Empty;
                    totAmt = totAmt * 100;
                    data += "T";
                    data += totAmt.ToString();
                    data += totRec.ToString() + "   ";

                    fio.putLine(data);

                    fio.close_output();
                }
                return true;
            }
            catch (Exception e) { msg = e.Message; }
            return false;
        }

        private String cleanAcctNum(String acctno)
        {
            String acct = acctno.Replace("-", "").Trim();
            String CleanAcct = String.Empty;

            for (int i = 0; i < acct.Length; i++)
            {
                if (isNumeric(acct.Substring(i, 1))) { CleanAcct += acct.Substring(i, 1); }
            }
            return CleanAcct;
        }

        private bool isNumeric(String s)
        {
            try
            {
                int i = int.Parse(s);
                return true;
            }
            catch { }
            return false;
        }

        public DataTable appendFileName(DataTable dt, DataTable FName, out String errmsg, String FileInd, string SFileInd)
        {
            errmsg = String.Empty;

            try
            {
                DataTable newDt = new DataTable();

                newDt.Columns.Add("Currency");
                newDt.Columns.Add("total_amount");
                newDt.Columns.Add("total_items");
                newDt.Columns.Add("FileName");
                newDt.Columns.Add("FileInd");
                newDt.Columns.Add("SFileInd");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < FName.Rows.Count; j++)
                    {
                        if (dt.Rows[i]["Currency"].ToString() == FName.Rows[j]["Currency"].ToString())
                        {
                            Object[] data = 
                            {
                                dt.Rows[i]["Currency"],
                                Convert.ToDouble(dt.Rows[i]["total_amount"].ToString()).ToString("#,###.00"),
                                dt.Rows[i]["total_items"],
                                FName.Rows[j]["FileName"].ToString(),
                                FileInd,
                                SFileInd

                            };

                            newDt.Rows.Add(data);
                        }
                    }
                }
                return newDt;
            }
            catch (Exception e)
            {
                errmsg = e.Message;
            }
            return null;
        }
    }
}
