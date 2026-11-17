using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Configuration;

namespace IRSClassLib
{
    public class irsCIFProfiler : sqlHelperWs
    {
        public DataTable GetCIFProfiles()
        {
            base.EnforceConstraint = false;
            getUpdateDS("CIF_Profiles", new Object[] { });
            if (hasRows)
            {   
                selectedRow = 0;
                return table;
            }
            else 
            {
                return null;
            }
        }

        public DataTable LoadProfile(string profileName)
        {
            string sql = "";
            sql = "Select * From TBL_CIF_PROFILER Where ProfileName = '" + profileName + "'";
            DataSet ds = getDataSet(sql, false, new object[] { });

            return ds.Tables[0];
        }

        public DataTable GetCIFClass()
        {
            base.EnforceConstraint = false;
            getUpdateDS("CIF_Class_Filter", new Object[] {  });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable GetCIFBranch(string branchcode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("CIF_Branch_Filter", new Object[] { branchcode });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable GetCIFAcctBranch()
        {
            base.EnforceConstraint = false;
            getUpdateDS("CIF_Acct_Branch_Filter", new Object[] { });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else { return null; }
        }

        public DataTable GetCIFType()
        {
            base.EnforceConstraint = false;
            getUpdateDS("CIF_Type_Filter", new Object[] { });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable GetCIFAcccountType()
        {
            base.EnforceConstraint = false;
            getUpdateDS("CIF_Account_Filter", new Object[] { });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable LoadCIFFields()
        {
            base.EnforceConstraint = false;
            getUpdateDS("CIF_Select_Field", new Object[] { });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable GetCIFDates()
        {
            base.EnforceConstraint = false;
            getUpdateDS("CIF_Date_Filter", new Object[] { });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable GetCIFAcctStatus()
        {
            base.EnforceConstraint = false;
            getUpdateDS("CIF_Acct_Status_Filter", new Object[] { });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable GetCIFAcctCcy()
        {
            base.EnforceConstraint = false;
            getUpdateDS("CIF_Currecny_Filter", new Object[] { });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable GetCIFMerchCode()
        {
            base.EnforceConstraint = false;
            getUpdateDS("CIF_Merch_Prod_Code", new Object[] { });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable GetCIFMerchants()
        {
            base.EnforceConstraint = false;
            getUpdateDS("CIF_Merch_code", new Object[] { });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable ParseQuery(string top,string idNo, string guid, string cifName, string acctNo, string[] acctType, string[] acctStatus, string[] acctCcy,string []acctBranch,
                                    string[] cifClass, string[] cifType, string dateFilter,string dateFrom, string dateTo, string[] cifBranch, string[] SelectField,
                                    bool chkAllCIFBranch, bool chkAllCIFType, bool chkAllAcctBranch, string remInputId, string merchMemID, string merchProdCode, string [] merchName, bool chkAllMerchs,
                                    bool chkAllCifClass,bool chkAllAcctStatus,bool chkAllAcctCcy,bool chkAllAcctType)
        {
            string sql = "";
            string arrAcctStatus = ArrayIn(acctStatus);
            string arrAcctCcy = ArrayIn(acctCcy);
            string arrAcctType = ArrayIn(acctType);
            string arrCifClass = ArrayIn(cifClass);
            string branches = ArrayIn(cifBranch);
            string ciftype = ArrayIn(cifType);
            string merchants = ArrayIn(merchName);
            string Select = FieldSelections(SelectField);
            string acctBr = ArrayIn(acctBranch);
            sql = "Select ";
            if (top != "")
            {
                sql = sql + "Top " + top;
            }
            sql = sql + "* From ";
            sql = sql + "(Select Distinct ' ' [ ],";
            sql = sql + Select;
            sql = sql + "From (Select * From Tbl_cif_master Where CIFClass in (" + arrCifClass + ")";
            if (branches.Length != 0)
            {
                sql = sql + " And IsNull(locationCode,branchCode) in (" + branches + ")";
            }
            if (dateFilter.IndexOf("dateCreated") >= 0 || dateFilter.IndexOf("BirthDate") >= 0)
            {
                sql = sql + " And " + dateFilter + " Between Convert(varchar(10),'" + dateFrom + "',121) and Convert(varchar(10),'" + dateTo + "',121)";
            }
            if ((!chkAllCIFType) && ciftype.Length != 0)
            {
                sql = sql + " And CIFType in (" + ciftype + ")";
            }
            if (guid != "")
            {
                sql = sql + " And CIFGUID = '" + guid + "'";
            }
            if (remInputId != "")
            {
                sql = sql + "And Remitter_Inputed_ID = '" + remInputId + "'";
            }
            if (cifName != "")
            {
                sql = sql + " And FullName like '%" + cifName + "%'";
            }
            if (idNo != "")
            {
                sql = sql + " And CIFID = '" + idNo + "'";
            }
            sql = sql + " And IsNull(Deleted,0) = 0) CIF ";

            if (sql.IndexOf("cifAddr") >= 0)
            {
                sql = sql + "Left Join Tbl_cif_address cifAddr on cifAddr.seq_ID = (Select top 1 seq_id From Tbl_cif_address addr Where CIF.cifid = addr.cifid and preferredMailAdd = 1 and IsNull(Deleted,0) = 0) ";
                sql = sql + "Left Join Tbl_ZipCodes cifZip on cifAddr.ZipCodeSeqId = cifZip.seq_ID ";
            }
            if (sql.IndexOf("cifContact") >= 0)
            {
                sql = sql + "Left Join Tbl_cif_contacts cifContact on cifContact.seq_ID = (Select top 1 seq_id From Tbl_cif_contacts cont Where CIF.cifid = cont.cifid and PreferredNo = 1 and IsNull(Deleted,0) = 0) ";
            }
            sql = sql + "Left Join (Select * From Tbl_cif_accounts Where IsNull(Deleted,0) = 0) cifAcct on CIF.CIFID = cifAcct.CIFID ";
            if (sql.IndexOf("lkpAcct") >= 0)
            {
                sql = sql + "Left Join (Select Fldvalue,FldDescription,FldCode From Tbl_Lookup Where fldGroup = 'AccountType') lkpAcct on cifAcct.accountType = lkpAcct.fldValue ";
            }
            if (sql.IndexOf("acctBranch") >= 0)
            {
                sql = sql + "Left Join Tbl_branches acctBranch on cifAcct.branchCode = acctBranch.branchCode ";
            }
            if (sql.IndexOf("lkpAcctStat") >= 0)
            {
                sql = sql + "Left Join (Select Fldvalue,FldDescription,FldCode From Tbl_Lookup Where fldGroup = 'AccountstatusFC') lkpAcctStat on cifAcct.Status = lkpAcctStat.fldvalue ";
            }
            if ((sql.IndexOf("cifOccup") >= 0) || (sql.IndexOf("lkpOccup") >= 0))
            {
                sql = sql + "Left Join Tbl_cif_occupations cifOccup on cifOccup.seq_id = (Select top 1 seq_id From Tbl_cif_occupations occ Where occ.CIFID = CIF.CIFID and IsNull(occ.Deleted,0) = 0) ";
                sql = sql + "Left Join (Select Fldvalue,FldDescription,FldCode From Tbl_Lookup Where fldGroup = 'Occupation') lkpOccup on cifOccup.Occupation = lkpOccup.fldValue ";
            }
            if (sql.IndexOf("lkpCS") >= 0)
            {
                sql = sql + "Left Join (Select Fldvalue,FldDescription,FldCode From Tbl_Lookup Where fldGroup = 'CivilStatus') lkpCS on CIF.CivilStatus = lkpCS.fldValue ";
            }
            if (sql.IndexOf("lkpCZ") >= 0)
            {
                sql = sql + "Left Join (Select Fldvalue,FldDescription,FldCode From Tbl_Lookup Where fldGroup = 'Citizenship') lkpCZ on CIF.Citizenship = lkpCZ.fldValue ";
            }
            if (sql.IndexOf("lkpSex") >= 0)
            {
                sql = sql + "Left Join (Select Fldvalue,FldDescription,FldCode From Tbl_Lookup Where fldGroup = 'Gender') lkpSex on CIF.Gender = lkpSex.fldValue ";
            }
            if (sql.IndexOf("lkpRel") >= 00)
            {
                sql = sql + "Left Join (Select Fldvalue,FldDescription,FldCode From Tbl_Lookup Where fldGroup = 'Religion' ) lkpRel on CIF.Religion = lkpRel.fldValue ";
            }
            if (sql.IndexOf("cifBranch") >= 0)
            {
                sql = sql + "Left Join Tbl_branches cifBranch on CIF.locationCode = cifBranch.branchCode ";
                sql = sql + "Left Join Tbl_branches cifBranch1 on CIF.branchCode = cifBranch1.branchCode ";
            }
            sql = sql + "Left Join (Select * From Tbl_cif_ids Where IsNull(Deleted,0) = 0 and ((IsNull(idClassification,1) = 1) or (idClassification = 2))) cifIDs on CIF.CIFID = cifIDs.CIFID ";
            if (sql.IndexOf("lkpIDs") >= 0)
            {
                sql = sql + "Left Join (Select Fldvalue,FldDescription,FldCode From Tbl_Lookup Where fldGroup = 'IDType') lkpIDs on cifIDs.idType = lkpIDs.fldValue ";
            }
            sql = sql + "Left Join Tbl_cif_cards cifCard on cifCard.seq_id = (Select top 1 seq_id From Tbl_cif_cards crd Where CIF.CIFID = crd.CIFID) ";
            if (sql.IndexOf("lkpCard") >= 0)
            {
                sql = sql + "Left Join (Select Fldvalue,FldDescription,FldCode From Tbl_Lookup Where fldGroup='CardType') lkpCard on cifCard.cardType = lkpCard.fldValue ";
            }
            sql = sql + " Left Join (Select CIFID,RemitterCIFID,MemberIDNo,MerInfoSeqId From Tbl_cif_merchants Where IsNull(Deleted,0) = 0) cifMerch on CIF.CIFID = cifMerch.remitterCIFID ";
            sql = sql + " Left Join Tbl_cif_merchants_info cifMerchInf on cifMerch.MerInfoSeqId = cifMerchInf.seq_id and IsNull(cifMerchInf.Deleted,0)=0 ";
            if (sql.IndexOf("lkpMerch") >= 0)
            {
                sql = sql + "Left Join (Select Fldvalue,FldDescription,FldCode From Tbl_Lookup Where fldGroup='MerchantProdCode') lkpMerch on cifMerchInf.MerchantProductCode = lkpMerch.fldValue ";
            }
            if (sql.IndexOf("cifRep") >= 0)
            {
                sql = sql + "Left Join Tbl_cif_representative cifRep on CIF.CIFID = cifRep.CIFID ";
            }
            sql = sql + "Left Join Tbl_cif_beneficiaries bene on CIF.CIFID = bene.remitterCIFID ";
            if (sql.IndexOf("cifBene") >= 0)
            {
                sql = sql + "Left Join Tbl_cif_master cifBene on bene.CIFID = cifBene.CIFID and IsNull(cifBene.Deleted,0)=0 ";
            }
            if (sql.IndexOf("beneAddr") >= 0)
            {
                sql = sql + "Left Join Tbl_cif_address beneAddr on beneAddr.seq_ID = (Select top 1 seq_id From Tbl_cif_address addr Where cifBene.cifid = addr.cifid and preferredMailAdd = 1 and IsNull(Deleted,0) = 0) ";
                sql = sql + "Left Join Tbl_ZipCodes beneZip on beneAddr.ZipCodeSeqId = beneZip.seq_ID ";
            }
            if (sql.IndexOf("lkpBenRel") > 0)
            {
                sql = sql + "Left Join (Select Fldvalue,FldDescription,FldCode From Tbl_Lookup Where fldGroup='Relationship') lkpBenRel on bene.Relationship = lkpBenRel.fldValue ";
            }
            if (sql.IndexOf("lkpBenProd") >= 0)
            {
                sql = sql + "Left Join (Select Fldvalue,FldDescription,FldCode From Tbl_Lookup Where fldGroup= 'ProductCode') lkpBenProd on bene.productCode = lkpBenProd.fldValue ";
            }
            if (sql.IndexOf("lkpBenImp") >= 0)
            {
                sql = sql + "Left Join (Select Fldvalue,FldDescription,FldCode From Tbl_Lookup Where fldGroup='ImplementationType')lkpBenImp on bene.implementationCode = lkpBenImp.fldValue ";
            }
            if ((sql.IndexOf("beneAcct") >= 0) || (sql.IndexOf("lkpBeneAcct")) >= 0)
            {
                sql = sql + "Left Join Tbl_cif_accounts beneAcct on bene.AccountSeqId = beneAcct.seq_Id ";
                sql = sql + "Left Join (Select Fldvalue,FldDescription,FldCode From Tbl_Lookup Where fldGroup= 'AccountType') lkpBeneAcct on beneAcct.accountType = lkpBeneAcct.fldValue ";
            }
            if (sql.IndexOf("ach.") > 0)
            {
                sql = sql + "Left Join Tbl_CIF_AchWorks ach on CIF.CIFID = ach.CIFID ";
            }
            sql = sql + "Where IsNull(CIF.Deleted,0) = 0";
            if (dateFilter.IndexOf("expiry") >= 0)
            {
                sql = sql + " And " + dateFilter + " between convert(varchar(10),'" + dateFrom + "',121) and convert(varchar(10),'" + dateTo + "',121)";
            }
            if (acctNo != "")
            {
                sql = sql + " And cifAcct.accountNo = '" + acctNo + "'";
            }
            if (arrAcctType.Length != 0)
            {
                sql = sql + " And cifAcct.accountType in (" + arrAcctType + ")";
            }
            if (arrAcctStatus.Length != 0)
            {
                sql = sql + " And cifAcct.Status in (" + arrAcctStatus + ")";
            }
            if ((arrAcctCcy.Length != 0) && (chkAllAcctCcy = false))
            {
                sql = sql + " And cifAcct.Currency in (" + arrAcctCcy + ")";
            }
            if ((!chkAllAcctBranch) && acctBr.Length != 0)
            {
                sql = sql + " And cifAcct.branchCode in (" + acctBr + ")";
            }
            if (merchMemID != "")
            {
                sql = sql + " And cifMerch.memberIDNo = '"+ merchMemID +"'";
            }
            if(merchProdCode!="")
            {
                sql = sql + " And cifMerchInf.MerchantProductCode = "+ merchProdCode;
            }
            if (merchName.Length != 0)
            {
                sql = sql + " And cifMerch.cifid in (" + merchants + ")";
            }
            sql = sql + ")knd";
            if (dateFilter.IndexOf("[Last Tranx Date]") >= 0)
            {
                sql = sql + " Where " + dateFilter + " Between Convert(Varchar(10),'" + dateFrom + "',121) And Convert(Varchar(10),'" + dateTo + "',121)";
            }
            if (dateFilter.IndexOf("[First Tranx Date]") >= 0)
            {
                sql = sql + " Where " + dateFilter + " Between Convert(Varchar(10),'" + dateFrom + "',121) And Convert(Varchar(10),'" + dateTo + "',121)";
            }

            base.EnforceConstraint = false;
            DataSet ds = getDataSet(sql, false, new object[] { });

            if (ds.Tables[0].Rows.Count > 0)
            {
                System.Web.HttpContext.Current.Session["tblData"] = ds;
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public string ExtractFile(string profilename)
        {
            base.EnforceConstraint = false;
            DataSet ds = (DataSet)System.Web.HttpContext.Current.Session["tblData"];

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 65000)
                {
                    return "Result exceeds 65000 records Please check filter.";
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        csFileIo io = new csFileIo();
                        string Filename = "";
                        Filename = profilename + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                        string ReportProfilerDir = this.GetAppSetting("CIFProfilerDir");
                        string ReportPath = ReportProfilerDir + @"\" + Filename;

                        if (io.directoryExist(ReportProfilerDir))
                        {
                            CreateExcelXML(ReportPath, ds, profilename, 65000);
                        }
                        else
                        {
                            io.createDirectory(ReportProfilerDir);
                            CreateExcelXML(ReportPath, ds, profilename, 65000);
                        }
                        return "Extract Complete: " + ReportPath;
                    }
                    else
                    {
                        return "There is nothing to extract.";
                    }
                }
            }
            else
            {
                return "There is nothing to extract.";
            }
        }

        public static void CreateExcelXML(string FileName, DataSet Ds, string SheetName, int RecordsPerSheet)
        {
            StringWriter S = new StringWriter();
            XmlTextWriter X = new XmlTextWriter(S);
            StreamWriter Writer = new StreamWriter(FileName, false);
            try
            {
                SheetName = (SheetName.Trim().Length == 0) ? "WorkSheet" : SheetName;
                RecordsPerSheet = (RecordsPerSheet <= 0) ? 650000 : RecordsPerSheet;

                X.Formatting = Formatting.Indented;
                X.Indentation = 4;
                X.WriteProcessingInstruction("xml", "version=\"1.0\"");
                X.WriteProcessingInstruction("mso-application", "progid=\"Excel.Sheet\"");
                X.WriteStartElement("Workbook", string.Empty);
                X.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:spreadsheet");
                X.WriteAttributeString("xmlns:o", "urn:schemas-microsoft-com:office:office");
                X.WriteAttributeString("xmlns:x", "urn:schemas-microsoft-com:office:excel");
                X.WriteAttributeString("xmlns:ss", "urn:schemas-microsoft-com:office:spreadsheet");
                X.WriteAttributeString("xmlns:html", "http://www.w3.org/TR/REC-html40");

                X.WriteStartElement("DocumentProperties", string.Empty);
                X.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:office");
                X.WriteElementString("Author", "Knd");
                X.WriteElementString("LastAuthor", string.Empty);
                X.WriteElementString("Created", string.Empty);
                X.WriteElementString("Company", string.Empty);
                X.WriteElementString("Version", "1.0");
                X.WriteEndElement();

                X.WriteStartElement("ExcelWorkBook", string.Empty);
                X.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:office");
                X.WriteEndElement();

                X.WriteStartElement("Styles", string.Empty);
                X.WriteStartElement("Style", string.Empty);
                X.WriteAttributeString("ss:ID", "Default");
                X.WriteAttributeString("ss:Name", "Normal");
                X.WriteStartElement("Alignment", string.Empty);
                X.WriteAttributeString("ss:Vertical", "Bottom");
                X.WriteEndElement();
                X.WriteElementString("Borders", string.Empty);
                X.WriteElementString("Font", string.Empty);
                X.WriteElementString("Interior", string.Empty);
                X.WriteElementString("NumberFormat", string.Empty);
                X.WriteElementString("Protection", string.Empty);
                X.WriteEndElement();
                X.WriteEndElement();

                for (int i = 0; i < Ds.Tables.Count; i++)
                {
                    int k = 0;
                    for (int j = 0; j < Ds.Tables[i].Rows.Count; j++)
                    {
                        string Name = string.Empty;
                        if (j == 0)
                        {
                            Name = SheetName + " " + Convert.ToString(i + 1) + "." + k.ToString();
                            WriteWorkSheetStart(X, Ds, i, Name);
                            k++;
                        }
                        else if ((j % RecordsPerSheet) == 0)
                        {
                            Name = SheetName + " " + Convert.ToString(i + 1) + "." + k.ToString();
                            WriteWorkSheetEnd(X);
                            WriteWorkSheetStart(X, Ds, i, Name);
                            k++;
                        }
                        X.WriteStartElement("Row");
                        for (int x = 0; x < Ds.Tables[i].Columns.Count; x++)
                        {
                            X.WriteStartElement("Cell");
                            X.WriteStartElement("Data");
                            X.WriteAttributeString("ss:Type", "String");
                            X.WriteValue(Ds.Tables[i].Rows[j][x].ToString());
                            X.WriteEndElement();
                            X.WriteEndElement();
                        }
                        X.WriteEndElement();
                    }
                    WriteWorkSheetEnd(X);
                }

                X.WriteEndElement();
                X.Flush();
                Writer.Write(S.ToString());
            }
            finally
            {
                Writer.Close();
                S.Close();
                X.Close();
                S.Dispose();
                Writer.Dispose();
            }
        }

        private static void WriteWorkSheetStart(XmlTextWriter X, DataSet Ds, int i, string SheetName)
        {
            X.WriteStartElement("Worksheet", string.Empty);
            X.WriteAttributeString("ss:Name", SheetName);
            X.WriteStartElement("Table", string.Empty);
            X.WriteAttributeString("ss:ExpandedColumnCount", Ds.Tables[i].Columns.Count.ToString());
            X.WriteAttributeString("ss:ExpandedRowCount", Convert.ToString(Ds.Tables[i].Rows.Count + 1));
            X.WriteAttributeString("x:FullColumns", "1");
            X.WriteAttributeString("x:FullRows", "1");
            X.WriteStartElement("Row");
            for (int j = 0; j < Ds.Tables[i].Columns.Count; j++)
            {
                X.WriteStartElement("Cell");
                X.WriteStartElement("Data");
                X.WriteAttributeString("ss:Type", "String");
                X.WriteValue(Ds.Tables[i].Columns[j].ColumnName);
                X.WriteEndElement();
                X.WriteEndElement();
            }
            X.WriteEndElement();
        }
        
        private static void WriteWorkSheetEnd(XmlTextWriter X)
        {
            X.WriteEndElement();
            X.WriteStartElement("WorksheetOptions", string.Empty);
            X.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:excel");
            X.WriteStartElement("Panes", string.Empty);
            X.WriteStartElement("Pane", string.Empty);
            X.WriteElementString("ActiveRow", "2");
            X.WriteElementString("ActiveCol", "1");
            X.WriteEndElement();
            X.WriteEndElement();
            X.WriteElementString("ProtectObjects", "False");
            X.WriteElementString("ProtectScenarios", "False");
            X.WriteEndElement();
            X.WriteEndElement();
        }

        public string SaveProfile(string top,string idNo, string guid, string cifName, string acctNo, string[] acctType, string[] acctStatus, string[] acctCcy, string[] acctBranch,
                                    string[] cifClass, string[] cifType, string dateFilter, string dateFrom, string dateTo, string[] cifBranch, string[] selectText,
                                    bool chkAllCIFBranch, bool chkAllCIFType, bool chkAllAcctBranch, string remInputId, string merchMemID, string merchProdCode,
                                    string profileName, string[] merchName, bool chkAllMerchs, bool chkAllCifClass, bool chkAllAcctStatus ,bool chkAllAcctCcy, bool chkAllAcctType)
        {
            string ciftypes = AddDelimeter(cifType,"|");
            string branches = AddDelimeter(cifBranch,"|");
            string acctbranches = AddDelimeter(acctBranch,"|");
            string select = AddDelimeter(selectText, "|");
            string merchantName = AddDelimeter(merchName, "|");
            string arrCIFCLass = AddDelimeter(cifClass, "|");
            string arrAcctStatus = AddDelimeter(acctStatus, "|");
            string arrAcctType = AddDelimeter(acctType, "|");
            string arrAcctCcy = AddDelimeter(acctCcy, "|");
             
            string sql = "Insert Into TBL_CIF_PROFILER Values(";
            sql = sql + "'" + profileName + "',";
            sql = sql + "getdate(),";
           
            if (chkAllCIFBranch)
            {
                sql = sql + "1,";
            }
            else
            {
                sql = sql + "0,";
            }
            if (chkAllCIFType)
            {
                sql = sql + "1,";
            }
            else
            {
                sql = sql + "0,";
            }
            if (chkAllAcctBranch)
            {
                sql = sql + "1,";
            }
            else
            {
                sql = sql + "0,";
            }
            if (chkAllMerchs)
            {
                sql = sql + "1,";
            }
            else
            {
                sql = sql + "0,";
            }
            sql = sql + "'" + ciftypes + "',";
            sql = sql + "'" + arrCIFCLass + "',";
            sql = sql + "'" + idNo + "',";
            sql = sql + "'" + guid + "',";
            sql = sql + "'" + branches + "',";
            sql = sql + "'" + remInputId + "',";
            sql = sql + "'" + cifName + "',";
            sql = sql + "'" + arrAcctType + "',";
            sql = sql + "'" + arrAcctStatus + "',";
            sql = sql + "'" + acctbranches + "',";
            sql = sql + "'" + arrAcctCcy + "',";
            sql = sql + "'" + acctNo + "',";
            sql = sql + "'" + merchProdCode + "',";
            sql = sql + "'" + merchMemID + "',";
            sql = sql + "'" + merchantName + "',";
            sql = sql + "'" + dateFilter + "',";
            sql = sql + "'" + dateFrom + "',";
            sql = sql + "'" + dateTo + "',";
            sql = sql + "'" + top + "',";
            sql = sql + "'" + select + "',";
            if (chkAllCifClass)
            {
                sql = sql + "1,";
            }
            else
            {
                sql = sql + "0,";
            }
            if (chkAllAcctStatus)
            {
                sql = sql + "1,";
            }
            else
            {
                sql = sql + "0,";
            }
            if (chkAllAcctCcy)
            {
                sql = sql + "1,";
            }
            else
            {
                sql = sql + "0,";
            }
            if (chkAllAcctType)
            {
                sql = sql + "1";
            }
            else
            {
                sql = sql + "0";
            }
            sql = sql + ")";

            try
            {
                SqlCommand cmd = getSqlCommand(sql);
                int i = sqlUpdate(sql);
                if (i < 0)
                {
                    return "ERROR: \n" + Message.ToString();
                }
                else
                {
                    return profileName.Trim() + " is successfully saved to CIF profile";
                }
            }
            catch(SqlException ex)
            {
                return "Database Connection Error. \n" + ex.Message;
            }

        }

        public string UpdateProfile(string top, string idNo, string guid, string cifName, string acctNo, string[] acctType, string[] acctStatus, string[] acctCcy, string[] acctBranch,
                            string[] cifClass, string[] cifType, string dateFilter, string dateFrom, string dateTo, string[] cifBranch, string[] selectText,
                            bool chkAllCIFBranch, bool chkAllCIFType, bool chkAllAcctBranch, string remInputId, string merchMemID, string merchProdCode,
                            string profileName, string[] merchName, bool chkAllMerchs, bool chkAllCifClass, bool chkAllAcctStatus, bool chkAllAcctCcy, bool chkAllAcctType)
        {
            string ciftypes = AddDelimeter(cifType, "|");
            string branches = AddDelimeter(cifBranch, "|");
            string acctbranches = AddDelimeter(acctBranch, "|");
            string select = AddDelimeter(selectText, "|");
            string merchantName = AddDelimeter(merchName, "|");
            string arrCIFCLass = AddDelimeter(cifClass, "|");
            string arrAcctStatus = AddDelimeter(acctStatus, "|");
            string arrAcctType = AddDelimeter(acctType, "|");
            string arrAcctCcy = AddDelimeter(acctCcy, "|");

            string sql = "Update Tbl_CIF_Profiler SET ";
            sql = sql + "Cbx_Branches = '" + chkAllCIFBranch + "',";
            sql = sql + "Cbx_CIFType = '" + chkAllCIFType + "',";
            sql = sql + "Cbx_AcctBranch ='" + chkAllAcctBranch + "',";
            sql = sql + "Cbx_Merchants = '" + chkAllMerchs + "',";

            sql = sql + "CIFType = '" + ciftypes + "',";
            sql = sql + "CIFClass = '" + arrCIFCLass + "',";
            sql = sql + "CIFID = '" + idNo + "',";
            sql = sql + "CIFGUID = '" + guid + "',";
            sql = sql + "BranchCode = '" + branches + "',";
            sql = sql + "Rem_Inputed_Id = '" + remInputId + "',";
            sql = sql + "Fullname = '" + cifName + "',";
            sql = sql + "Account_Type = '" + arrAcctType + "',";
            sql = sql + "Account_Status = '" + arrAcctStatus + "',";
            sql = sql + "Account_Branch = '" + acctbranches + "',";
            sql = sql + "Account_Ccy = '" + arrAcctCcy + "',";
            sql = sql + "Account_No = '" + acctNo + "',";
            sql = sql + "Merchant_ProductCode = '" + merchProdCode + "',";
            sql = sql + "Merchant_Member_Id = '" + merchMemID + "',";
            sql = sql + "Merchant_Name = '" + merchantName + "',";
            sql = sql + "DateFilter = '" + dateFilter + "',";
            sql = sql + "DateFrom = '" + dateFrom + "',";
            sql = sql + "DateTo = '" + dateTo + "',";
            sql = sql + "RecordCount = '" + top + "',";
            sql = sql + "SelectField = '" + select + "',";
            sql = sql + "Cbx_CIFClass = '" + chkAllCifClass + "',";
            sql = sql + "Cbx_AcctStatus = '" + chkAllAcctStatus + "',";
            sql = sql + "Cbx_AcctCcy = '" + chkAllAcctCcy +"',";
            sql = sql + "Cbx_AcctType = '" + chkAllAcctType + "'";
            sql = sql + "Where ProfileName = '" + profileName + "'";
            
            try
            {
                SqlCommand cmd = getSqlCommand(sql);
                int i = sqlUpdate(sql);
                if (i < 0)
                {
                    return "ERROR: \n" + Message.ToString();
                }
                else
                {
                    return profileName.Trim() + " is successfully updated.";
                }
            }
            catch (SqlException ex)
            {
                return "Database Connection Error. \n" + ex.Message;
            }

        }

        public String DeleteProfile(string profilename)
        {
            base.EnforceConstraint = false;
            string sql;
            string pName = profilename.Trim();
            sql = "Delete From TBL_CIF_PROFILER where profileName = '" + pName + "'";

            if (IsUsed(profilename))
            {
                try
                {
                    int k = sqlUpdate(sql);
                    if (k < 0)
                    {
                        return "ERROR \n" + Message.ToString();
                    }
                }
                catch (SqlException ex)
                {
                    return "ERROR \n" + ex.Message;
                }
                return "Update Successful!";
            }
            else 
            {
                return "Profile " + profilename + "Not Found!";
            }
        }

        public bool IsUsed(string profileName)
        {
            string sql = "";
            sql = "Select ProfileName From TBL_CIF_PROFILER Where ProfileName = '" + profileName+"'";
            getUpdateDS(sql);
            if (hasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string ArrayIn(string[] StrArray)
        {
            string StrBuilder = "";
            for (int k = 0; k <= StrArray.Length - 1; k++)
            {
                if (StrArray != null)
                {
                    StrBuilder = StrBuilder + "'";
                    StrBuilder = StrBuilder + StrArray[k].ToString();
                    StrBuilder = StrBuilder + "',";
                }
            }
            if (StrBuilder != "")
            {
                StrBuilder = StrBuilder.Substring(0, StrBuilder.Length - 1);
            }
            return StrBuilder;
        }

        private string FieldSelections(string[] StrArray)
        {
            string StrBuilder ="";
            for (int k = 0; k <= StrArray.Length - 1; k++)
            {
                if (StrArray != null)
                {
                    StrBuilder = StrBuilder + StrArray[k].ToString();
                    StrBuilder = StrBuilder + ",";
                }
            }
            if (StrBuilder != "")
            {
                StrBuilder = StrBuilder.Substring(0, StrBuilder.Length - 1);
            }
            return StrBuilder;
        }

        private string AddDelimeter(string[] StrArray, string Delimeter)
        {
            string NewString;
            NewString = "";

            for (int i = 0; i <= StrArray.Length - 1; i++)
            {
                if (StrArray != null)
                {
                    NewString = NewString + StrArray[i].ToString();
                    NewString = NewString + Delimeter;
                }
            }

            if (NewString != "")
            {
                NewString = NewString.Substring(0, NewString.Length - 1);
            }

            return NewString;
        }

        private string GetAppSetting(string Key)
        {
            try
            {
                string sTemp = null;
                sTemp = new irsConfiguration().appsetting(Key);
                if (sTemp != null)
                    return sTemp;
                else
                    return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }


}
