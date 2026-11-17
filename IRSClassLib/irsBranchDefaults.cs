using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib 
{
    [Serializable]
    public class irsBranchDefaults : sqlHelperWs
    {
        private DataTable dtCopy;

        public irsBranchDefaults()
        {
            this.TableName = "tbl_branch_defaults";
            this.IdentityColumn = "SEQ_ID";
        }

        public DataSet GetBenType()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='BeneficiaryType'", false));
            return ds;
        }

        public DataSet getNotifyMode()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='NotificationMode'", false));
            return ds;
        }

        public DataSet getRelationToRemitter()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='Relationship'", false));
            return ds;
        }

        public DataSet getCountry()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='Country'", false));
            return ds;
        }

        public DataSet getProductCode()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='ProductCode'", false));
            return ds;
        }

        public DataSet getPayMode()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='PayMode'", false));
            return ds;
        }

        public DataSet getRiskRating()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='RiskRating'", false));
            return ds;
        }

        public DataSet GetBranches(string location)
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = execStoredProcedure("get_dropdown_branches_byaccess", new object[] { location });  //commons.getDataSet(commons.getSqlCommand("get_dropdown_branches"), "tblBranches");
            return ds;
        }


        public DataSet getLocalCurrency(string brCode)
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = execStoredProcedure("sp_getCurrencyCategories", new object[] { brCode,1 });  //commons.getDataSet(commons.getSqlCommand("get_dropdown_branches"), "tblBranches");
            return ds;
        }

        public DataSet getAccountCurrency(string brCode)
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = execStoredProcedure("sp_getCurrencyCategories", new object[] { brCode, 2 });  //commons.getDataSet(commons.getSqlCommand("get_dropdown_branches"), "tblBranches");
            return ds;
        }

        public DataSet getReceiptMode()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='ReceiptMode'", false));
            return ds;
        }

        public DataSet getAccountType()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='AccountType'", false));
            return ds;
        }

        public DataSet getCoveringBank()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='CoveringBank'", false));
            return ds;
        }

        public DataSet getRapidRemit()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='RapidRemitCode'", false));
            return ds;
        }

        public DataSet getImplementationType()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='ImplementationType'", false));
            return ds;
        }

        public DataSet getTransactionSource()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='TranxSource'", false));
            return ds;
        }

        public DataSet getTransactionPurpose()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='TranxPurpose'", false));
            return ds;
        }

        public DataSet getComputation()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='Computation'", false));
            return ds;
        }

        public DataSet getSourceOfFunds()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='SourceOfFunds'", false));
            return ds;
        }

        public DataTable getDefaultSetup(string brcode)
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = execStoredProcedure("get_branchdefaultsetup", new object[] { brcode });
            return ds.Tables[0];
        }

        public DataSet getRoundUpOBPS()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='RoundupOBPSAmount'", false));
            return ds;
        }

        public DataSet getRemitAggregation1()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='RemittanceAggregation1'", false));
            return ds;
        }

        public DataSet getWorkerType()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='WorkerType'", false));
            return ds;
        }

        public DataSet getRemitterType()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds =getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='RemitterType'", false));
            return ds;
        }

        public DataSet getChargeType()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds =getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='ChargeType'", false));
            return ds;
        }

        public DataSet getVIPStatus()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='VIPCardStatus'", false));
            return ds;
        }

        public DataSet getGender()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='Gender'", false));
            return ds;
        }

        public DataSet getCivilStatus()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='CivilStatus'", false));
            return ds;
        }

        public DataSet getOccupation()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='Occupation'", false));
            return ds;
        }

        public DataSet getCitizenship()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='Citizenship'", false));
            return ds;
        }

        public DataSet getMail()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='PreferredMailingAddress'", false));
            return ds;
        }

        public DataSet getContact()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='ContactType'", false));
            return ds;
        }

        public DataSet getChargeGroup()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='ChargeGroup'", false));
            return ds;
        }

        public DataTable getBranchDefaults(string brCode)
        {
            DataTable dt = execStoredProcedureTable("sp_getbranchdefaults", new object[] { brCode });
            return dt;
        }
        public DataSet getVIPType()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds =getDataSet(getSqlCommand("SELECT flddescription,fldcode,fldvalue from tbl_lookup where fldgroup='VIPType'", false));
            return ds;
        }

       

        public DataSet getVIPTable(string branchcode)
        {
            //sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = getDataSet(getSqlCommand("SELECT * FROM TBL_VIP_TYPE where BranchCode='"+ branchcode +"'", false));
            return ds;
        }


        public bool save_defaults(string data, string brcode)
        {
            string fldGroup;
            Int16 fldvalue=0;
            string[] datavalue;
            string[] parsevalue;
            string value;
            int rowCount=0;
            bool forUpdate;
            try
            {
                //base.getUpdateDS("select * from tbl_branch_defaults where BranchCode='" + brcode + "' and ProductCode = " + prodcode + "");
                base.getUpdateDS("sp_getbranchdefaults", new object[] { brcode });
                dtCopy = table.Copy();
          
                forUpdate = (table.Rows.Count >0) ? true : false;
                datavalue = data.Split(',');
                int counter = (table.Rows.Count > 0) ? table.Rows.Count : datavalue.Length; //this refers to the row counter important for updating field

                for (int itemcount = 0; itemcount <= datavalue.Length - 1; itemcount++)
                {
                    #region minimize comments
                    // Int32 seq_id = 0; // commented to remove warning
                    //if (itemcount > table.Rows.Count - 1)
                    //{
                    //    seq_id = 0;
                    //}
                    //else
                    //{
                    //    seq_id = (forUpdate) ? Convert.ToInt32(table.Rows[itemcount]["SEQ_ID"]) : itemcount;
                    //}
                    //if (dtCopy.Rows.Count <= 0)
                    //{
                    //    seq_id = (forUpdate) ? Convert.ToInt32(table.Rows[itemcount]["SEQ_ID"]) : itemcount;
                    //}
                    //else if(itemcount > table.Rows.Count - 1)
                    //{
                    //    seq_id = 0;
                    //}

                    #endregion
                    int rdx = -1;

                    value = (string)datavalue.GetValue(itemcount);
                    parsevalue = value.Split('=');

                    //begin insert record
                    if (parsevalue.Length >= 2) //may naiiwang isang null value sa dulo
                    {
                        fldGroup = parsevalue[0];
                        if (fldGroup != "BranchCode" && fldGroup != "")
                        {
                            if (!isRemittanceAggregate(fldGroup))
                            {
                                fldvalue = check_null(parsevalue[1]);
                            }
                            else
                            {
                                string[] temp = parsevalue[1].Split(';');
                                string[] fld_value = temp[1].Split(':');
                                string[] fld_group = temp[0].Split(':');
                                fldvalue = check_null(fld_value[1]);
                                fldGroup = fldGroup + "_" + fld_group[1];
                            }

                            #region minimize comments
                            //string sqlcmd="";
                                //dtCopy = table.Copy(); //refresh the table
                                //sqlcmd = "SEQ_ID = " + seq_id + "";
                                ////string[] fld = fldGroup.Split('_');
                                ////if (fld[0] == "BranchDefaultSetup")
                                ////{
                                ////    sqlcmd = "BranchDefValue like 'BranchDefaultSetup%' ";
                            ////}
                            #endregion
                            if (fldGroup.IndexOf("BranchDefaultSetup") > - 1)
                            {
                                string[] br_setup = fldGroup.Split('_');
                                //string sqlselect = "BranchDefCode = " + fldvalue + " AND " + "SUBSTRING(BranchDefValue,1,18) = '" + br_setup[0] + "' ";
                                //DataRow[] row = table.Select(sqlselect);
                                bool isUpdates = this.find(br_setup[0], fldvalue.ToString(), br_setup[0].Length, out rdx);

                                if (!isUpdates) // new values for BranchDefaultsetup (Grid)
                                {
                                    if (newRow(sqlHelperWs.RETAIN_DATASET))
                                    {
                                        uprow["BranchCode"] = brcode;
                                        //uprow["ProductCode"] = prodcode;
                                        uprow["BranchDefCode"] = fldvalue;//(fldvalue==null) ? (short)0 : fldvalue;
                                        uprow["BranchDefValue"] = fldGroup;
                                        uprow["DataType"] = "String";
                                        table.Rows.Add(uprow);
                                    }
                                    base.addMode = false;
                                }
                                else //for updating of BranchDefault Setup (grid)
                                {
                                    selectedRow = rdx; //rowCount;
                                    rowCount += 1;
                                    base.setColumn("BranchDefCode", fldvalue);
                                    base.setColumn("BranchDefValue", fldGroup);

                                }
                            }
                            else if (fldGroup.IndexOf("RemittanceAggregation") > -1)
                            {
                                string[] rem_aggr = fldGroup.Split('_');
                                //string sqlselect = "SUBSTRING(BranchDefValue,1,22) = '" + rem_aggr[0] + "' ";
                                //DataRow[] row = table.Select(sqlselect);
                                bool isUpdate = this.findWithSubstring(rem_aggr[0], 22, out rdx);
                                if (!isUpdate)//(row.Length <= 0) // new values for BranchDefaultsetup (Grid)
                                {
                                    if (newRow(sqlHelperWs.RETAIN_DATASET))
                                    {
                                        uprow["BranchCode"] = brcode;
                                        //uprow["ProductCode"] = prodcode;
                                        uprow["BranchDefCode"] = fldvalue;//(fldvalue==null) ? (short)0 : fldvalue;
                                        uprow["BranchDefValue"] = fldGroup;
                                        uprow["DataType"] = "String";
                                        table.Rows.Add(uprow);
                                    }
                                    base.addMode = false;
                                }
                                else //for updating of BranchDefault Setup (grid)
                                {
                                    selectedRow = rdx; //rowCount;
                                    rowCount += 1;
                                    base.setColumn("BranchDefCode", fldvalue);
                                    base.setColumn("BranchDefValue", fldGroup);
                                }
                            }
                            else if (fldGroup.IndexOf("_") > -1)
                            {
                                string[] val = fldGroup.Split('_');
                                //string sqlselect = "SUBSTRING(BranchDefValue,1," + val[0].Length + ") = '" + val[0] + "' ";

                                bool isUpdates = this.findWithSubstring(val[0], val[0].Length, out rdx);
                                // DataRow[] row = table.Select(sqlselect);
                                if (!isUpdates) // new values for BranchDefaultsetup (Grid)
                                {
                                    if (newRow(sqlHelperWs.RETAIN_DATASET))
                                    {
                                        uprow["BranchCode"] = brcode;
                                        //uprow["ProductCode"] = prodcode;
                                        uprow["BranchDefCode"] = fldvalue;//(fldvalue==null) ? (short)0 : fldvalue;
                                        uprow["BranchDefValue"] = fldGroup;
                                        uprow["DataType"] = "String";
                                        table.Rows.Add(uprow);
                                    }
                                    base.addMode = false;
                                }
                                else //for updating of BranchDefault Setup (grid)
                                {
                                    selectedRow = rdx; //rowCount;
                                    rowCount += 1;
                                    base.setColumn("BranchDefCode", fldvalue);
                                    base.setColumn("BranchDefValue", fldGroup);
                                } 
                            }
                            else
                            {

                                //DataRow[] row = table.Select("BranchDefValue='" + fldGroup + "'");
                                forUpdate = this.find(fldGroup, out rdx);//(row.Length > 0) ? true : false;

                                if (forUpdate)
                                {
                                    selectedRow = rdx; //rowCount;
                                    rowCount += 1; //determines row for updating
                                    base.setColumn("BranchDefCode", fldvalue);
                                    base.setColumn("BranchDefValue", fldGroup);
                                }
                                else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    uprow["BranchCode"] = brcode;
                                    //uprow["ProductCode"] = prodcode;
                                    uprow["BranchDefCode"] = fldvalue;//(fldvalue==null) ? (short)0 : fldvalue;
                                    uprow["BranchDefValue"] = fldGroup;
                                    uprow["DataType"] = "String";
                                    table.Rows.Add(uprow);
                                }
                                base.addMode = false;
                            }
                        }
                    }
                    
                }
                return base.save(sqlHelperWs.DISPOSE_DATASET); //sqlHelperWs.RETAIN_DATASET);
                
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        public bool saveToTemp_defaults(string data, string brcode, string userid)
        {
            string fldGroup;
            Int16 fldvalue = 0;
            string[] datavalue;
            string[] parsevalue;
            string value;
            int rowCount = 0;
            bool forUpdate;
            string tmpSEQID = "0";
            try
            {
                //insert to tmp table
                string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                base.getUpdateDS("select top 1 * from TBL_BRANCH_DEFAULTS_TMP where 1=0");
                if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                {
                    uprow["Maker"] = userid;
                    uprow["DateAdded"] = dateNow;
                    uprow["CurrStatus"] = "For Approval";
                    uprow["StatusDate"] = dateNow;
                    uprow["ActionType"] = "EDIT";
                    uprow["Module"] = "Branch Administration >> Branch Defaults";
                    uprow["MenuName"] = "Branch Defaults";
                    table.Rows.Add(uprow);
                }
                base.addMode = false;
                base.save(sqlHelperWs.RETAIN_DATASET);
                base.getUpdateDS(string.Format("select top 1 * from TBL_BRANCH_DEFAULTS_TMP where maker='{0}' and CurrStatus='For Approval' order by SEQ_ID desc", userid));
                tmpSEQID = Convert.ToString(table.Rows[0]["SEQ_ID"]);


                base.getUpdateDS("sp_getbranchdefaults", new object[] { brcode });
                dtCopy = table.Copy();

                forUpdate = (table.Rows.Count > 0) ? true : false;
                datavalue = data.Split(',');
                int counter = (table.Rows.Count > 0) ? table.Rows.Count : datavalue.Length; //this refers to the row counter important for updating field

                for (int itemcount = 0; itemcount <= datavalue.Length - 1; itemcount++)
                {
                    int rdx = -1;

                    value = (string)datavalue.GetValue(itemcount);
                    parsevalue = value.Split('=');

                    //begin insert record
                    if (parsevalue.Length >= 2) //may naiiwang isang null value sa dulo
                    {
                        fldGroup = parsevalue[0];
                        if (fldGroup != "BranchCode" && fldGroup != "")
                        {
                            if (!isRemittanceAggregate(fldGroup))
                            {
                                fldvalue = check_null(parsevalue[1]);
                            }
                            else
                            {
                                string[] temp = parsevalue[1].Split(';');
                                string[] fld_value = temp[1].Split(':');
                                string[] fld_group = temp[0].Split(':');
                                fldvalue = check_null(fld_value[1]);
                                fldGroup = fldGroup + "_" + fld_group[1];
                            }

                            if (fldGroup.IndexOf("BranchDefaultSetup") > -1)
                            {
                                string[] br_setup = fldGroup.Split('_');
                                bool isUpdates = this.find(br_setup[0], fldvalue.ToString(), br_setup[0].Length, out rdx);

                                if (!isUpdates) // new values for BranchDefaultsetup (Grid)
                                {
                                    if (newRow(sqlHelperWs.RETAIN_DATASET))
                                    {
                                        uprow["BranchCode"] = brcode;
                                        uprow["BranchDefCode"] = fldvalue;
                                        uprow["BranchDefValue"] = fldGroup;
                                        uprow["DataType"] = "String";
                                        table.Rows.Add(uprow);
                                    }
                                    base.addMode = false;
                                }
                                else //for updating of BranchDefault Setup (grid)
                                {
                                    selectedRow = rdx; //rowCount;
                                    rowCount += 1;
                                    base.setColumn("BranchDefCode", fldvalue);
                                    base.setColumn("BranchDefValue", fldGroup);

                                }
                            }
                            else if (fldGroup.IndexOf("RemittanceAggregation") > -1)
                            {
                                string[] rem_aggr = fldGroup.Split('_');
                                bool isUpdate = this.findWithSubstring(rem_aggr[0], 22, out rdx);
                                if (!isUpdate)//(row.Length <= 0) // new values for BranchDefaultsetup (Grid)
                                {
                                    if (newRow(sqlHelperWs.RETAIN_DATASET))
                                    {
                                        uprow["BranchCode"] = brcode;
                                        //uprow["ProductCode"] = prodcode;
                                        uprow["BranchDefCode"] = fldvalue;//(fldvalue==null) ? (short)0 : fldvalue;
                                        uprow["BranchDefValue"] = fldGroup;
                                        uprow["DataType"] = "String";
                                        table.Rows.Add(uprow);
                                    }
                                    base.addMode = false;
                                }
                                else //for updating of BranchDefault Setup (grid)
                                {
                                    selectedRow = rdx; //rowCount;
                                    rowCount += 1;
                                    base.setColumn("BranchDefCode", fldvalue);
                                    base.setColumn("BranchDefValue", fldGroup);
                                }
                            }
                            else if (fldGroup.IndexOf("_") > -1)
                            {
                                string[] val = fldGroup.Split('_');

                                bool isUpdates = this.findWithSubstring(val[0], val[0].Length, out rdx);
                                if (!isUpdates) // new values for BranchDefaultsetup (Grid)
                                {
                                    if (newRow(sqlHelperWs.RETAIN_DATASET))
                                    {
                                        uprow["BranchCode"] = brcode;
                                        //uprow["ProductCode"] = prodcode;
                                        uprow["BranchDefCode"] = fldvalue;//(fldvalue==null) ? (short)0 : fldvalue;
                                        uprow["BranchDefValue"] = fldGroup;
                                        uprow["DataType"] = "String";
                                        table.Rows.Add(uprow);
                                    }
                                    base.addMode = false;
                                }
                                else //for updating of BranchDefault Setup (grid)
                                {
                                    selectedRow = rdx; //rowCount;
                                    rowCount += 1;
                                    base.setColumn("BranchDefCode", fldvalue);
                                    base.setColumn("BranchDefValue", fldGroup);
                                }
                            }
                            else
                            {

                                //DataRow[] row = table.Select("BranchDefValue='" + fldGroup + "'");
                                forUpdate = this.find(fldGroup, out rdx);//(row.Length > 0) ? true : false;

                                if (forUpdate)
                                {
                                    selectedRow = rdx; //rowCount;
                                    rowCount += 1; //determines row for updating
                                    base.setColumn("BranchDefCode", fldvalue);
                                    base.setColumn("BranchDefValue", fldGroup);
                                }
                                else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    uprow["BranchCode"] = brcode;
                                    //uprow["ProductCode"] = prodcode;
                                    uprow["BranchDefCode"] = fldvalue;//(fldvalue==null) ? (short)0 : fldvalue;
                                    uprow["BranchDefValue"] = fldGroup;
                                    uprow["DataType"] = "String";
                                    table.Rows.Add(uprow);
                                }
                                base.addMode = false;
                            }
                        }
                    }

                }
                dtCopy = table.Copy();

                base.getUpdateDS("select top 1 * from TBL_BRANCH_DEFAULTS_TMPDESC where 1=0");
                if (isUpdatable)
                {
                    for (int x = 0; x < dtCopy.Rows.Count; x++)
                    {
                        if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                        {
                            uprow["SEQ_ID"] = Convert.ToInt64(tmpSEQID);
                            uprow["MAIN_SEQ_ID"] = Convert.ToInt64(dtCopy.Rows[x]["SEQ_ID"]);
                            uprow["BranchCode"] = Convert.ToString(dtCopy.Rows[x]["BranchCode"]);
                            uprow["BranchDefCode"] = Convert.ToString(dtCopy.Rows[x]["BranchDefCode"]);
                            uprow["BranchDefValue"] = Convert.ToString(dtCopy.Rows[x]["BranchDefValue"]);
                            uprow["DataType"] = Convert.ToString(dtCopy.Rows[x]["DataType"]);
                            table.Rows.Add(uprow);
                        }
                    }
                    base.addMode = false;
                    return base.save(sqlHelperWs.RETAIN_DATASET);

                }
                //return base.save(sqlHelperWs.DISPOSE_DATASET); //sqlHelperWs.RETAIN_DATASET);

            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        private bool isBranchDefaults(string value)
        {
            if (value.Substring(0, 19) == "BranchDefaultsSetup")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Int16 check_null(string value)
        {
            if (value == "null" || value == string.Empty || value == "false" || value == "undefined") { return (short)0; }
            else if (value == "true") { return (short)1; }
            else if (Convert.ToDouble(value) > 32767) { return 0; } //hindi kasi pwede sa short value ang greater than 32767
            else { return Convert.ToInt16(value); }
        }

        private bool isRemittanceAggregate(string value)
        {
            if (value.Length >= 21)
            {
                if (value.Substring(0, 21) == "RemittanceAggregation")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private string determine_datatype(string value)
        {
            if (value == "true" || value == "false")
            {
                return "Bit";
            }
            else
            {
                return "String";
            }
        }

        private int getselectedRow(int seq_id)
        {
            DataView dv = table.DefaultView;
            dv.Sort = "SEQ_ID";
            int i = dv.Find(new Object[] { seq_id });
            if (i > -1)
            {
                //dv[i]["allow_access"] = allow;
                selectedRow = i;
            }
            return i;
        }

        private bool find(string para1, out int rdx)
        {
            rdx = -1;
            for (int i = 0; i <= dtCopy.Rows.Count - 1; i++)
            {
                if (para1 == dtCopy.Rows[i]["BranchDefValue"].ToString())
                {
                    rdx = i;
                    return true;
                }
            
            }
            return false;
        
        }

        private bool findWithSubstring(string para1, int len, out int rdx)
        {
            rdx = -1;
            for (int i = 0; i <= dtCopy.Rows.Count - 1; i++)
            {
                if (dtCopy.Rows[i]["BranchDefValue"].ToString().Length > len)
                {
                    if (para1 == dtCopy.Rows[i]["BranchDefValue"].ToString().Substring(0, len))
                    {
                        rdx = i;
                        return true;
                    }
                }

            }
            return false;
        
        }

        private bool find(string para1, string para2, int len, out int rdx)
        {
            rdx = -1;

            for (int i = 0; i <= dtCopy.Rows.Count - 1; i++)
            {
                if (dtCopy.Rows[i]["BranchDefValue"].ToString().Length > len)
                {
                    if (para1 == dtCopy.Rows[i]["BranchDefValue"].ToString().Substring(0, len) && para2 == dtCopy.Rows[i]["BranchDefCode"].ToString())
                    {
                        rdx = i;
                        return true;
                    }
                }

            }
            return false;
        
        }



        private bool allow_delete(int seq_id)
        {
            DataView dv = table.DefaultView;
            dv.Sort = "seq_id";
            int i = dv.Find(new Object[] { seq_id });
            if (i > -1)
            {
                //dv[i]["allow_access"] = allow;
                selectedRow = i;
                return base.Delete(RETAIN_DATASET);
            }
            return false;
        }

        public bool save_vip(Hashtable ht, string brcode, bool deleted)
        {
            try
            {
                String viptype = (String)ht["VipType"];
                String datatype = (string)ht["DataType"];
                String value = (string)ht["Value"];
                int seq_id = ht["SEQ_ID"].Equals(string.Empty) ? 0 : Convert.ToInt32(ht["SEQ_ID"]);

                if (viptype.Length > 0)
                {
                    string sqlstmt = "SELECT * FROM TBL_VIP_TYPE WHERE SEQ_ID=" + seq_id + " ";
                    base.getUpdateDS(sqlstmt);
                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("SEQ_ID"))
                            {
                                if (table.Rows.Count > 0)
                                {
                                    selectedRow = getselectedRow(seq_id);
                                    base.setColumn("VipType", viptype);
                                    base.setColumn("DataType", datatype);
                                    base.setColumn("Value", value);
                                }
                                else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    uprow["VipType"] = viptype;
                                    uprow["DataType"] = datatype;
                                    uprow["Value"] = value;
                                    uprow["BranchCode"] = brcode;
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
                    Message = "SEQ ID Required.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        public bool saveToTemp_vip(Hashtable ht, string brcode, bool deleted, string userid)
        {
            try
            {
                string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                String viptype = (String)ht["VipType"];
                String datatype = (string)ht["DataType"];
                String value = (string)ht["Value"];
                int seq_id = ht["SEQ_ID"].Equals(string.Empty) ? 0 : Convert.ToInt32(ht["SEQ_ID"]);

                if (viptype.Length > 0)
                {
                    string sqlstmt = "SELECT top 1 * FROM TBL_VIP_TYPE_TMP WHERE 1=0";
                    base.getUpdateDS(sqlstmt);
                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("SEQ_ID"))
                            {
                                if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    uprow["MAIN_SEQ_ID"] = seq_id;
                                    uprow["VipType"] = viptype;
                                    uprow["DataType"] = datatype;
                                    uprow["Value"] = value;
                                    uprow["BranchCode"] = brcode;

                                    uprow["Maker"] = userid;
                                    uprow["DateAdded"] = dateNow;
                                    uprow["CurrStatus"] = "For Approval";
                                    uprow["StatusDate"] = dateNow;
                                    uprow["ActionType"] = (seq_id == 0) ? "ADD" : ((deleted == true) ? "DELETE" : "EDIT");
                                    uprow["Module"] = "Branch Administration >> Branch Defaults >> Setup VIP";
                                    uprow["MenuName"] = "VIP Type";

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
                    Message = "SEQ ID Required.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        public bool checkSetupVIP(string branchcode, string viptype)
        {
            sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = commons.getDataSet(commons.getSqlCommand("SELECT * FROM TBL_VIP_TYPE where BranchCode='" + branchcode + "' AND VipType='"+ viptype +"'", false));
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }


    }
}
