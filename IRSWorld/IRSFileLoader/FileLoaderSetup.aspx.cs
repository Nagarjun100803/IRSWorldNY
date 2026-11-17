using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IRSClassLib;

namespace IRSWorld.IRSFileLoader
{
    public partial class FileLoaderSetup : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        { 
            AjaxPro.Utility.RegisterTypeForAjax(typeof(FileLoaderSetup));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        #region AJAX PUBLIC METHOD
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record()
        {
            return this.do_IRS_File_Loader("INSERT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string SEQ_ID)
        {
            Session["FILE_LOADER_SEQ_ID"] = SEQ_ID;
            return this.do_IRS_File_Loader("UPDATE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record(string param_values)
        {
            this.parameter_values = param_values;
            return this.do_IRS_File_Loader("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.do_IRS_File_Loader("ROLLBACK");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["FILE_LOADER_MESSAGE"] != null)
            {
                return (string)Session["FILE_LOADER_MESSAGE"];
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["FILE_LOADER_MESSAGE"] = null;
            Session["FILE_LOADER_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_FILELOADER_RECORD(string column_values)
        {
            irsFileLoader irsfileloader = this.IRS_FileLoader();
            //string[] s = column_values.Split('$');
            string[] splitstring = new string[] { "^%$" };
            string[] s = column_values.Split(splitstring, StringSplitOptions.None);

            if (s != null && s.Length > 0)
            {
                if (irsfileloader.set_record(s) == false)
                {
                    this.set_message(irsfileloader.messages); return false;
                }
                Session["IRS_FILE_LOADER"] = irsfileloader;
                return true;
            }
            irsfileloader = null;
            this.set_message("UNABLE TO EXECUTE METHOD: SET_FILELOADER_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_FILELOADER_RECORD(string record_seq_id)
        {
            DataSet mydataset = new DataSet();
            DataSet dsChildTable = null;
            DataSet dsParentTable = null;
            DataSet dsFieldNames = null;
            DataSet dsFileFormat = null;
            DataSet dsTableList = null;
            DataSet dsColumnList = null;
            DataTable tempTable = null;
            irsFileLoader irsfileloader = this.IRS_FileLoader();
            //String strRecordQuery = "";
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand command = new SqlCommand();
            try
            {
                if (irsfileloader != null)
                {
                    if (irsfileloader.get_record(record_seq_id) == true)
                    {
                        mydataset = irsfileloader.getdataRecord();
                        command = commons.getSqlCommand("SELECT filesequence, " +
                                                               "tablename, " +
                                                               "childfileformat, " +
                                                               "'childfileformat' as [format] " +
                                                        "FROM tbl_list_temp_tables " +
                                                        "WHERE childfileformat =" + mydataset.Tables[0].Rows[0]["fileformat"] + " " + 
                                                        "AND fileuploadtype = " + mydataset.Tables[0].Rows[0]["fileuploadtype"], false);
                        dsChildTable = commons.getDataSet(command, "ChildTable");
                        if (dsChildTable.Tables["ChildTable"].Rows.Count > 0)
                        {
                            tempTable = dsChildTable.Tables["ChildTable"];
                            mydataset.Tables.Add(tempTable.Copy());

                            dsFieldNames = getFieldNames(Convert.ToString(dsChildTable.Tables["ChildTable"].Rows[0]["tablename"]));

                            tempTable = null;
                            tempTable = dsFieldNames.Tables["FieldNames"];
                            mydataset.Tables.Add(tempTable.Copy());

                            command = commons.getSqlCommand("SELECT a.FldValue, " +
                                                                    "UPPER(ISNULL(a.FldCode,'')) AS FldCode " +
                                                             "FROM TBL_LOOKUP a " +
                                                             "WHERE UPPER(a.FldGroup) = (SELECT FldCode " +
                                                                                        "FROM tbl_lookup " +
                                                                                        "WHERE fldvalue = " + mydataset.Tables[0].Rows[0]["fileuploadtype"] + " " +
                                                                                        "AND fldgroup = 'fileuploadtype')", false);
                            dsFileFormat = commons.getDataSet(command, "FileFormat");

                            if (dsFileFormat.Tables["FileFormat"].Rows.Count > 0)
                            {
                                tempTable = dsFileFormat.Tables["FileFormat"];
                                mydataset.Tables.Add(tempTable.Copy());
                            }
                            return mydataset;
                        }
                        else 
                        {
                            command = commons.getSqlCommand("SELECT filesequence, " +
                                                                   "tablename, " +
                                                                   "parentfileformat, " +
                                                                   "'parentfileformat' as [format] " +
                                                            "FROM tbl_list_temp_tables " +
                                                            "WHERE parentfileformat =" + mydataset.Tables[0].Rows[0]["fileformat"] + " " +
                                                            "AND fileuploadtype = " + mydataset.Tables[0].Rows[0]["fileuploadtype"] + "", false);

                            dsParentTable = commons.getDataSet(command, "ParentTable");
                            if (dsParentTable.Tables["ParentTable"].Rows.Count > 0)
                            {
                                tempTable = dsParentTable.Tables["ParentTable"];
                                mydataset.Tables.Add(tempTable.Copy());
                                dsFieldNames = getFieldNames(Convert.ToString(dsParentTable.Tables["ParentTable"].Rows[0]["tablename"]));

                                tempTable = null;
                                tempTable = dsFieldNames.Tables["FieldNames"];
                                mydataset.Tables.Add(tempTable.Copy());

                                command = commons.getSqlCommand("SELECT a.FldValue, " +
                                                                        "UPPER(ISNULL(a.FldCode,'')) AS FldCode " +
                                                                 "FROM TBL_LOOKUP a " +
                                                                 "WHERE UPPER(a.FldGroup) = (SELECT FldCode " +
                                                                                            "FROM tbl_lookup " +
                                                                                            "WHERE fldvalue = " + mydataset.Tables[0].Rows[0]["fileuploadtype"] + " " +
                                                                                            "AND fldgroup = 'fileuploadtype')", false);
                                dsFileFormat = commons.getDataSet(command, "FileFormat");

                                if (dsFileFormat.Tables["FileFormat"].Rows.Count > 0)
                                {
                                    tempTable = dsFileFormat.Tables["FileFormat"];
                                    mydataset.Tables.Add(tempTable.Copy());
                                }

                                tempTable = null;
                                dsFieldNames = null;
                                dsFieldNames = getFieldNames("Compliance");
                                dsFieldNames.Tables["FieldNames"].TableName = "DestinationTableFieldNames";
                                tempTable = dsFieldNames.Tables["DestinationTableFieldNames"];
                                mydataset.Tables.Add(tempTable.Copy());

                                return mydataset;
                            }
                            else
                            {
                                

                                if (Convert.ToInt16(mydataset.Tables[0].Rows[0]["fileuploadtype"]) == 3)
                                {
                                    tempTable = null;
                                    command = commons.getSqlCommand("SELECT table_name " +
                                                                    "FROM information_schema.tables " +
                                                                    "WHERE table_type = 'base table' " +
                                                                    "ORDER BY table_name", false);
                                    dsTableList = commons.getDataSet(command, "TableList");
                                    tempTable = dsTableList.Tables["TableList"];
                                    mydataset.Tables.Add(tempTable.Copy());

                                    tempTable = null;
                                    command = commons.getSqlCommand("SELECT column_name " +
                                                                    "FROM information_schema.columns " +
                                                                    "WHERE table_name = '" + mydataset.Tables[0].Rows[0]["DestinationTable"] + "'" , false);

                                    dsColumnList = commons.getDataSet(command, "ColumnList");
                                    tempTable = dsColumnList.Tables["ColumnList"];
                                    mydataset.Tables.Add(tempTable.Copy());

                                    tempTable = null;
                                    command = commons.getSqlCommand("SELECT a.FldValue, " +
                                                                         "UPPER(ISNULL(a.FldCode,'')) AS FldCode " +
                                                                "FROM TBL_LOOKUP a " +
                                                                "WHERE UPPER(a.FldGroup) = (SELECT FldCode " +
                                                                                           "FROM tbl_lookup " +
                                                                                           "WHERE fldvalue = " + mydataset.Tables[0].Rows[0]["fileuploadtype"] + " " +
                                                                                           "AND fldgroup = 'fileuploadtype')", false);
                                    dsFileFormat = commons.getDataSet(command, "FileFormat");

                                    if (dsFileFormat.Tables["FileFormat"].Rows.Count > 0)
                                    {
                                        tempTable = dsFileFormat.Tables["FileFormat"];
                                        mydataset.Tables.Add(tempTable.Copy());
                                    }

                                    return mydataset;
                                }
                                else
                                {
                                    command = commons.getSqlCommand("SELECT a.FldValue, " +
                                                                         "UPPER(ISNULL(a.FldCode,'')) AS FldCode " +
                                                                "FROM TBL_LOOKUP a " +
                                                                "WHERE UPPER(a.FldGroup) = (SELECT FldCode " +
                                                                                           "FROM tbl_lookup " +
                                                                                           "WHERE fldvalue = " + mydataset.Tables[0].Rows[0]["fileuploadtype"] + " " +
                                                                                           "AND fldgroup = 'fileuploadtype')", false);
                                    dsFileFormat = commons.getDataSet(command, "FileFormat");

                                    if (dsFileFormat.Tables["FileFormat"].Rows.Count > 0)
                                    {
                                        tempTable = dsFileFormat.Tables["FileFormat"];
                                        mydataset.Tables.Add(tempTable.Copy());
                                    }

                                    tempTable = null;
                                    dsFieldNames = getFieldNames("Compliance");
                                    tempTable = dsFieldNames.Tables["FieldNames"];
                                    mydataset.Tables.Add(tempTable.Copy());

                                    return mydataset;
                                }                                
                            }
                        }
                    }
                }
                return null;
            }
            catch  // commented to remove warning (Exception  ex)
            {
                return null;
            }
            finally
            {
                Session["IRS_FILE_LOADER"] = irsfileloader;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string FILE_LOADER_SEQ_ID()
        {
            if (Session["FILE_LOADER_SEQ_ID"] != null)
            {
                return (string)Session["FILE_LOADER_SEQ_ID"];
            }
            return "0";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Boolean Update_Temp_Table(String OrigFieldNames, String EditedFieldNames, String tablename)
        {
            String[] OrigFldNames;
            String[] EditedFldNames;

            OrigFldNames = OrigFieldNames.Split('~');
            EditedFldNames = EditedFieldNames.Split('~');
            sqlHelperWs commons = new sqlHelperWs ();
            DataTable dtUpdateTempTable = new DataTable();

            try
            {
                for (int intIndex = 0; intIndex <= OrigFldNames.GetUpperBound(0); intIndex++)
                {
                    if (OrigFldNames[intIndex] != EditedFldNames[intIndex])
                    {
                        UpdateTempTable(tablename, OrigFldNames[intIndex], EditedFldNames[intIndex]);
                        UpdateFileDefinition(tablename, EditedFldNames[intIndex], OrigFldNames[intIndex]);
                    }
                }
                    return true; 
            }
            catch
            {
                return false;
            }
            finally
            {

            }
        }

        private void UpdateTempTable(String tablename, String oldcolname, String newcolname)
        {
            sqlHelperWs commons = new sqlHelperWs ();
            //SqlCommand sqlcommands = null;
            commons.execScalar("EXEC sp_rename '" + tablename + ".[" + oldcolname + "]', '" + newcolname + "', 'COLUMN'");
        }

        //private void DeleteFileDefinition(String tablename, String newcolname, String oldcolname)
        //{
        //    sqlHelper commons = new sqlHelper();

        //    commons.execScalar("UPDATE tbl_file_definition_mapper " +
        //                       "SET  temptablefield ='" + newcolname + "' " +
        //                       "WHERE temptablefield ='" + oldcolname + "'");

        //    commons.execScalar("UPDATE tbl_file_definition_mapper " +
        //                       "SET  Condition1 ='" + newcolname + "' " +
        //                       "WHERE Condition1 ='" + oldcolname + "'");

        //    commons.execScalar("UPDATE tbl_file_definition_mapper " +
        //                       "SET  Condition2 ='" + newcolname + "' " +
        //                       "WHERE Condition2 ='" + oldcolname + "'");

        //    commons.execScalar("UPDATE tbl_file_definition_mapper " +
        //                       "SET  Condition3 ='" + newcolname + "' " +
        //                       "WHERE Condition3 ='" + oldcolname + "'");
        //}

        private void UpdateFileDefinition(String tablename, String newcolname, String oldcolname)
        {
            sqlHelperWs commons = new sqlHelperWs ();

            commons.execScalar("UPDATE tbl_file_definition_mapper " +
                               "SET  temptablefield ='" + newcolname + "' " +
                               "WHERE temptablefield ='" + oldcolname + "'");

            commons.execScalar("UPDATE tbl_file_definition_mapper " +
                               "SET  Condition1 ='" + newcolname + "' " +
                               "WHERE Condition1 ='" + oldcolname + "'");

            commons.execScalar("UPDATE tbl_file_definition_mapper " +
                               "SET  Condition2 ='" + newcolname + "' " +
                               "WHERE Condition2 ='" + oldcolname + "'");

            commons.execScalar("UPDATE tbl_file_definition_mapper " +
                               "SET  Condition3 ='" + newcolname + "' " +
                               "WHERE Condition3 ='" + oldcolname + "'");   
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Boolean TempTableExist(String tablename)
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;
            DataSet myDataSet = new DataSet();
            try
            {
                sqlcommands = commons.getSqlCommand("SELECT count(*) " +
                                                    "FROM sysobjects " +
                                                    "WHERE name = '" + tablename + "'", false);
                myDataSet = commons.getDataSet(sqlcommands);
                if (Convert.ToInt16(myDataSet.Tables[0].Rows[0][0]) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
                if (myDataSet != null)
                {
                    myDataSet.Dispose();
                    myDataSet = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getFilterBy()
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT DISTINCT B.fldcode " +
                                                        "FROM tbl_file_definition_mapper A " +
                                                        "INNER JOIN tbl_lookup B ON B.fldvalue = A.fileformat " +
                                                        "AND fldgroup  IN ('IRSCompliance', 'otherchannels','IBatch') " +
                                                        "INNER JOIN tbl_lookup C on C.fldcode = b.fldgroup " +
                                                        "AND C.fldvalue = A.fileuploadtype", false);
                    return commons.getDataSet(sqlcommands, "GridFilter");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getFileNames(int FileFormat, int FileUploadType)
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT fldcode " +
                                                        "FROM tbl_lookup " +
                                                        "WHERE fldgroup = 'IRSCompliance' " +
                                                        "AND fldvalue IN (SELECT childfileformat " +
                                                                         "FROM tbl_list_temp_tables " +
                                                                         "WHERE parentfileformat = " + FileFormat + 
                                                                         "AND fileuploadtype =" + FileUploadType + ")", false);                    
                    return commons.getDataSet(sqlcommands, "FieldNames");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_Destination_Column_Schema(string TableName, string ColumnName, Int16 FileUploadType, Int16 FileFormat)
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;
            //DataSet dsChildTable = null;
            //DataSet dsParentTable = null;

            try
            {
                if (commons != null)
                {
                    if (FileUploadType == 1)
                    {
                        int intCount = 0;

                        intCount = Convert.ToInt16(commons.execScalar("SELECT count(*) " +
                                                                      "FROM tbl_list_temp_tables " +
                                                                      "WHERE fileuploadtype =" + FileUploadType + " " +
                                                                      "AND childfileformat = " + FileFormat + ""));
                        if (intCount == 0)
                        {
                            sqlcommands = commons.getSqlCommand("SELECT data_type, isnull(character_maximum_length, 0) as character_maximum_length " +
                                                                "FROM information_schema.columns " +
                                                                "WHERE table_name = 'tbl_unifiedwatchlist' " +
                                                                "AND column_name = '" + ColumnName + "'", false);
                            return commons.getDataSet(sqlcommands, "ColumnSchema");
                        }
                        else
                        {
                            TableName = "";

                            TableName = Convert.ToString(commons.execScalar("SELECT TableName " +
                                                                            "FROM tbl_list_temp_tables " +
                                                                            "WHERE fileuploadtype =" + FileUploadType + " " +
                                                                            "AND childfileformat = " + FileFormat + ""));
                            sqlcommands = commons.getSqlCommand("SELECT data_type, isnull(character_maximum_length, 0) as character_maximum_length " +
                                                                "FROM information_schema.columns " +
                                                                "WHERE table_name = '" + TableName + "' " +
                                                                "AND column_name = '" + ColumnName + "'", false);
                            return commons.getDataSet(sqlcommands, "ColumnSchema");
                        }
                    }
                    else // 2 or 3
                    { 
                        sqlcommands = commons.getSqlCommand("SELECT data_type, isnull(character_maximum_length, 0) as character_maximum_length " +
                                                            "FROM information_schema.columns " +
                                                            "WHERE table_name = '" + TableName + "' " +
                                                            "AND column_name = '" + ColumnName + "'", false);                    
                        return commons.getDataSet(sqlcommands, "ColumnSchema");
                    }
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getTempTableSchema(string TableName)
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                   sqlcommands = commons.getSqlCommand("SELECT column_name, data_type, character_maximum_length " +
                                                       "FROM information_schema.columns " +
                                                       "WHERE table_name = '" + TableName + "' " +
                                                       "ORDER BY ordinal_position", false);
                    
                    return commons.getDataSet(sqlcommands, "FieldNames");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getTemp_FinalFieldNames(string TableName)
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;
            DataSet dsFinalTable = new DataSet();
            DataSet mydataset = new DataSet();
            DataTable tempTable = null;

            try
            {
                if (commons != null)
                {
                        sqlcommands = commons.getSqlCommand("SELECT name " +
                                                           "FROM syscolumns " +
                                                           "WHERE id = (SELECT id " +
                                                                       "FROM sysobjects " +
                                                                       "WHERE name = '" + TableName + "') " +
                                                           "AND name != 'SEQ_ID' ", false);
                    
                        mydataset = commons.getDataSet(sqlcommands, "TempTable");

                        sqlcommands = commons.getSqlCommand("SELECT name " +
                                                            "FROM syscolumns " +
                                                            "WHERE id = (SELECT id " +
                                                                        "FROM sysobjects " +
                                                                        "WHERE name = 'TBL_UNIFIEDWATCHLIST') " +
                                                            "AND name != 'SEQ_ID' " +
                                                            "ORDER BY name", false);

                        dsFinalTable = commons.getDataSet(sqlcommands, "FinalTable");
                        tempTable = dsFinalTable.Tables["FinalTable"];
                        mydataset.Tables.Add(tempTable.Copy());
                        return mydataset;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getFieldNames(string TableName)
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    if (TableName.ToUpper() == "IRSCOMPLIANCE")
                    {
                        sqlcommands = commons.getSqlCommand("SELECT name " +
                                                            "FROM syscolumns " +
                                                            "WHERE id = (SELECT id " +
                                                                        "FROM sysobjects " +
                                                                        "WHERE name = 'TBL_UNIFIEDWATCHLIST') " +
                                                            "AND name != 'SEQ_ID' " +
                                                            "ORDER BY name", false);   
                    }
                    else if (TableName == "OtherChannels")
                    {
                        sqlcommands = commons.getSqlCommand("SELECT name " +
                                                            "FROM syscolumns " +
                                                            "WHERE id = (SELECT id " +
                                                                        "FROM sysobjects " +
                                                                        "WHERE name = 'tbl_otherchannels_uploads') " +
                                                            "AND name != 'SEQ_ID' " +
                                                            "ORDER BY name", false);
                        //return commons.getDataSet(sqlcommands, "FieldNames");
                    }
                    else if (TableName == "GRSFileUpload")
                    {
                        sqlcommands = commons.getSqlCommand("SELECT name " +
                                                            "FROM syscolumns " +
                                                            "WHERE id = (SELECT id " +
                                                                        "FROM sysobjects " +
                                                                        "WHERE name = 'TBL_UNIFIEDWATCHLIST') " +
                                                            "AND name != 'SEQ_ID' " +
                                                            "ORDER BY name", false);
                    }
                    else
                    {
                        sqlcommands = commons.getSqlCommand("SELECT name " +
                                                           "FROM syscolumns " +
                                                           "WHERE id = (SELECT id " +
                                                                       "FROM sysobjects " +
                                                                       "WHERE name = '" + TableName + "') " +
                                                           "AND name != 'SEQ_ID' ", false);                            
                    }
                    return commons.getDataSet(sqlcommands, "FieldNames");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Int16 getComplianceMaxValue(string queryMax)
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                return Convert.ToInt16(commons.execScalar(queryMax));
            }

            catch
            { 
                return -1;
            }

            finally
            { 
                if (commons != null)
                {
                    commons = null;
                }

                if (sqlcommands != null)
                {
                    sqlcommands.Dispose ();
                    sqlcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Boolean createTempTable(string queryCreateTempTable)
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                commons.execScalar(queryCreateTempTable);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }

                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String checkTempTableList(string queryCreateTempTable)
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                return Convert.ToString(commons.execScalar(queryCreateTempTable));
            }
            catch
            {
                return "";
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }

                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Boolean addRecordtoTempTableList(string queryCreateTempTable)
        {
            sqlHelperWs  commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                commons.execScalar(queryCreateTempTable);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }

                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Boolean addRecordtoLookup(string queryCreateTempTable)
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                commons.execScalar(queryCreateTempTable);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }

                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getCIFClass()
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("sp_get_CIFClass");
                    return commons.getDataSet(sqlcommands, "CIFClass");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getFileUploadType()
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("sp_get_File_Upload_Type");
                    return commons.getDataSet(sqlcommands, "FileUploadType");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getFileFormat(string fldgroup)
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;
            DataSet dsTableNames = new DataSet();
            DataSet mydataset = new DataSet();            
            DataTable tempTable = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT a.FldValue, " +
                                                               "UPPER(ISNULL(a.FldCode,'')) AS FldCode " +
                                                       "FROM TBL_LOOKUP a " +
                                                       "WHERE UPPER(a.FldGroup) = '" + fldgroup + "' " +
                                                       "ORDER BY a.FldCode", false);

                    //return commons.getDataSet( sqlcommands, "FileFormat");
                    mydataset = commons.getDataSet(sqlcommands, "FileFormat");

                    if (fldgroup == "IBatch")
                    {
                        sqlcommands = commons.getSqlCommand("SELECT table_name " +
                                                            "FROM information_schema.tables " +
                                                            "WHERE table_type = 'base table' " +
                                                            "ORDER BY table_name", false);                        
                        dsTableNames = commons.getDataSet(sqlcommands, "TableList");

                            if (dsTableNames.Tables["TableList"].Rows.Count > 0)
                            {
                                tempTable = dsTableNames.Tables["TableList"];
                                mydataset.Tables.Add(tempTable.Copy());
                                return mydataset;
                            }
                    }
                    else
                    {
                        return mydataset;
                    }
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getDestinationFields(string DestinationTableName)
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT name " +
                                                       "FROM syscolumns " +
                                                       "WHERE id = (SELECT id " +
                                                                   "FROM sysobjects " +
                                                                   "WHERE name = '" + DestinationTableName + "') " +
                                                       "AND name != 'SEQ_ID' ", false);                           
                    
                    return commons.getDataSet(sqlcommands, "DestinationFields");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }        

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getParentFileFormat(string fldgroup, int fileuploadtype)
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT a.FldValue, " +            
                                                                "UPPER(ISNULL(a.FldCode,'')) AS FldCode " +
                                                        "FROM TBL_LOOKUP a " +
                                                        "WHERE UPPER(a.FldGroup) = '" + fldgroup + "' " +
                                                        "AND a.FldValue NOT IN (SELECT ChildFileFormat " + 
                                                                               "FROM TBL_LIST_TEMP_TABLES " +
                                                                               "WHERE FileUploadType = " + fileuploadtype + 
                                                                               "AND parentfileformat != childfileformat)", false);

                    return commons.getDataSet(sqlcommands, "ParenFileFormat");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        #endregion

        #region PRIVATE METHOD: IRS_FileLoader

        private irsFileLoader IRS_FileLoader()
        {
            if (Session["IRS_FILE_LOADER"] == null)
            {
                Session["IRS_FILE_LOADER"] = new irsFileLoader();
            }
            return (irsFileLoader)Session["IRS_FILE_LOADER"];
        }

        private bool do_IRS_File_Loader(string instruction)
        {
            irsFileLoader irsfileloader = this.IRS_FileLoader();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            irsfileloader.selectedRow = -1;
                            if (irsfileloader.insert_record() == false)
                            {
                                this.set_message(irsfileloader.messages); return false;
                            }
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "UPDATE")
                    {
                        try
                        {
                            irsfileloader.seq_id = this.FILE_LOADER_SEQ_ID();
                            irsfileloader.selectedRow = 0;

                            if (irsfileloader.update_record() == false)
                            {
                                this.set_message(irsfileloader.messages); return false;
                            }
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }                     
                    }

                    if (instruction == "COMMIT")
                    {
                        try
                        {
                            if (this.SET_FILELOADER_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (irsfileloader.commit_record() == false)
                            {
                                this.set_message(irsfileloader.messages); return false;
                            }

                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "ROLLBACK")
                    {
                        try
                        {
                            irsfileloader.rollback_record(); return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                Session["IRS_FILE_LOADER"] = null;
                Session["IRS_FILE_LOADER"] = irsfileloader;
                Session["FILE_LOADER_SEQ_ID"] = null;
                Session["FILE_LOADER_SEQ_ID"] = irsfileloader.seq_id;
                irsfileloader = null;
            }
            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }
        #endregion

    }
}
