function execute_ajaxgridview_command(param)
{
    if (irs_fileloader != null && irs_fileloader != "undefined")
    {
        fileloader.do_ajaxgridviewer(param);
    }
}

function irs_fileloader()
{
    this.commons                    = null;
    this.SEQ_ID                     = null;
    var FileUploadType              = null;
    var FileFormat                  = null;
    var FileSequenceNo              = null;
    var Delimiter                   = null;
    var RecordField                 = null;       
    var RecordPosition              = null;
    var RecordLength                = null;
    var TempTableField              = null;
    var DestinationTable            = null;
    var DestinationTableField       = null;
    var CIFClass                    = null;
    var StartAtLine                 = null;
    var SplitChar                   = null;
    var SplitIndex                  = null;
    var Condition1                  = null;
    var ConditionPosition1          = null;
    var Operator1                   = null;
    var Condition2                  = null;
    var ConditionPosition2          = null;
    var Operator2                   = null;
    var Condition3                  = null;
    var ConditionPosition3          = null;
    var InvalidCharacters           = null;
    var DateCreated                 = null;
    var DateLastModified            = null;
    var TableName                   = null;
    this.intMaxRecord               = null;
    var ds                          = null;
    this.Save_Update                = null;
    this.NoofFields                 = null;
    this.ListOfFields               = null;
    var div                         = null;
    var arrayFieldNames             = new Array(1);
    var arrayFieldNameEdited        = new Array(1);
    var intTotalTempFields          = 0;
    var strNoofFiles                = "";
    var strUploadType               = ""; //single, child, parent
    var intPreviousSelectedFileUPloadType = 0;
    var AddEditTempTable            = "";

    this.save_temp_table = function()
    {
        var FieldsCount = document.getElementById("ddNoofFields").value;
        var strFieldsName = "";
        var blnTempTableExist;

        if (AddEditTempTable == "Edit")
        {
            arrayFieldNameEdited[0] = new Array(Number(document.getElementById("ddNoofFields").value));
            for (var counter = 1; counter <= FieldsCount; counter ++)
            {
                if (counter == FieldsCount)
                {
                    strFieldsName += "" + document.getElementById("txtFieldName" + String(counter)).value + "";
                }
                else
                {
                    strFieldsName += "" + document.getElementById("txtFieldName" + String(counter)).value + "~";
                }

                arrayFieldNameEdited[0][counter -1] = document.getElementById("txtFieldName" + String(counter)).value;
            }
            IRSWorld.IRSFileLoader.FileLoaderSetup.Update_Temp_Table(arrayFieldNames[0].join('~'), arrayFieldNameEdited[0].join('~'), document.getElementById("txtTempTableName").value);
        }
        else
        {
            blnTempTableExist = IRSWorld.IRSFileLoader.FileLoaderSetup.TempTableExist(document.getElementById("txtTempTableName").value);
            Save_Update = "Update";
            if (blnTempTableExist.value == true )
            {
                alert("Table already exists");
            }
            else
            {   
                this.add_temptable_lookupvalues();
            }
        }
    }

    this.reloadfileformat = function()
    {
        IRSWorld.IRSFileLoader.FileLoaderSetup.getFileFormat (document.getElementById("ddFileUploadType").options[document.getElementById("ddFileUploadType").selectedIndex].text, this.dropdown_get_fileformat_callback);
    }

    this.reload_parent_fileformat = function()
    {
        this.commons = new common_method();
        this.commons.to_unlock_button("tbltemptablebuttons","btnAddTempTable~");
        this.commons = null;
        clear_fields(document.getElementById("tbmultiplefiles"));
        IRSWorld.IRSFileLoader.FileLoaderSetup.getParentFileFormat (document.getElementById("ddFileUploadTypeTempTable").options[document.getElementById("ddFileUploadTypeTempTable").selectedIndex].text, document.getElementById("ddFileUploadTypeTempTable").options[document.getElementById("ddFileUploadTypeTempTable").selectedIndex].value, this.dropdown_get_parent_fileformat_callback);
        this.temp_table_values()

        document.getElementById("multiplefiles").style.display          = "none";
        document.getElementById("ddNoofFilesTempTable").selectedIndex   = 0;
        document.getElementById("ddNoofFilesTempTable").disabled        = false;
        document.getElementById("divConditions").style.display          = "none";
        document.getElementById("txtDelimiter").value                   = "";
        document.getElementById("txtDelimiter").disabled                = false;
    }

    this.load_all_dropdownlist = function()
    {
        this.dropdown_fileloader_fields();
    }

    this.dropdown_fileloader_fields = function()
    {
        IRSWorld.IRSFileLoader.FileLoaderSetup.getFileUploadType   (this.dropdown_get_fileuploadtype_callback);
        IRSWorld.IRSFileLoader.FileLoaderSetup.getFieldNames       ("compliance", this.dropdown_fieldnames_callback);
        IRSWorld.IRSFileLoader.FileLoaderSetup.getFilterBy         (this.dropdown_filter_callback);
        IRSWorld.IRSFileLoader.FileLoaderSetup.getCIFClass         (this.dropdown_get_CIFClass_callback);
    }

    this.get_Destination_Fields = function()
    {
        IRSWorld.IRSFileLoader.FileLoaderSetup.getDestinationFields(document.getElementById("ddDestinationTable").value, this.dropdown_destination_table_fields_callback);
    }

    this.dropdown_destination_table_fields_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddDestinationTableField", result.value, "name", "name");
            this.commons = null;
        }
    }

    this.dropdown_final_table = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddDestinationTableField",   null, "name", "name", result);
            this.commons = null;
        }
    }

    this.dropdown_destination_table_field = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddDestinationTableField",   null, "column_name", "column_name", result);
            this.commons = null;
        }
    }

    this.dropdown_fieldnames_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddDestinationTableField", result.value, "name", "name");
            this.commons = null;
        }
    }

    this.dropdown_get_fileuploadtype_callback = function (result)
    {
        if (result.value != null && result.context != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddFileUploadType", result.value, "FldValue", "FldCode");
            this.commons.to_load_dropdownlist("ddFileUploadTypeTempTable", result.value, "FldValue", "FldCode");
            this.commons = null;
        }
    }

    this.dropdown_get_CIFClass_callback = function (result)
    {
        if (result.value != null && result.context != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddCIFClass", result.value, "FldValue", "FldCode");
            this.commons = null;
        }
    }

    this.dropdown_get_fileformat_callback = function (result)
    {
        var dataset;
        dataset = null;
        dataset = result.value;

        if (result.value != null && result.context != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddFileFormat", result.value, "FldValue", "FldCode");
            this.commons = null;
        }

        if (result.value.Tables.length == 2)
        {    
            fileloader.dropdown_TableList_callback(dataset.Tables[1]);  
            document.getElementById("divCIFClass").style.display                = "block";
            document.getElementById("divDestinationTable").style.display        = "block";
            document.getElementById("divDestinationTableField").style.display   = "block";
            document.getElementById("divConditions").style.display              = "none";
            document.getElementById("divTempTableField").style.display          = "none"; 
            document.getElementById("divRecordPosition").style.display          = "block";
            
            this.commons = new common_method();                        
            this.commons.to_clear_screen("tblCIFClass");
            this.commons.to_clear_screen("tblDestinationTable");
            this.commons.to_clear_screen("tblDestinationTableField");
            this.commons.to_clear_screen("tblConditions");
            this.commons.to_clear_screen("tblTempTableField");
            this.commons.to_clear_screen("tblRecordPosition");        
            this.commons = null;
        }
        else
        {
            document.getElementById("divDestinationTable").style.display        = "none";
            document.getElementById("divCIFClass").style.display                = "none";
            document.getElementById("divRecordPosition").style.display          = "block";
        }
    }
    
    this.dropdown_get_fileformat_table = function (datatable)
    {
        if (datatable.Rows.length > 0)
        {       
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddFileFormat", null, "FldValue", "FldCode", datatable);
            this.commons = null;
        }
    }
    
    this.dropdown_get_parent_fileformat_callback = function (result)
    {
        if (result.value != null && result.context != "undefined")
        {       
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddFileFormatTempTable", result.value, "FldValue", "FldCode");
            this.commons = null;
        }
    } 
    
    this.dropdown_filter_callback = function (result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddLoaderFilter", result.value, "fldcode", "fldcode");
            this.commons = null;
        }
    }  
    
    this.insert_record = function()
    {   
        this.clear_filesetup();
        IRSWorld.IRSFileLoader.FileLoaderSetup.insert_record(this.insert_record_callback);
        document.getElementById("divTempTableField").style.display  = "none";
        document.getElementById("divRecordPosition").style.display  = "block";
        document.getElementById("divDestinationTableField").style.display     = "block";
        document.getElementById("divConditions").style.display      = "none";
        
        document.getElementById("txtDateCreated").value         = get_current_date_string().substring(0, 10);       
        document.getElementById("txtDateLastModified").value    = get_current_date_string().substring(0, 10);
    }  
    
    this.insert_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.IRSFileLoader.FileLoaderSetup.read_message(); alert(msg.value); return;
            }

            this.commons = new common_method();
            this.commons.to_read_only("tblFileLoaderSetup",false);
            this.commons.to_read_only("tblFileUploadType",false);
            this.commons.to_read_only("tblCIFClass",false);
            this.commons.to_read_only("tblDestinationTable",false);
            this.commons.to_read_only("tblDestinationTableField",false);
            this.commons.to_read_only("tblRecordPosition",false);
            this.commons.to_read_only("tblTempTableField",true);
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;
        }
    }
    
    this.update_record = function()
    {
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = document.getElementById("txtHiddenSeqID").value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == "")
        {
            alert ("RECORD NOT FOUND."); return;
        }
        
        IRSWorld.IRSFileLoader.FileLoaderSetup.update_record(this.SEQ_ID, this.update_record_callback);
    }
    
    this.update_record_callback = function (result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.IRSFileLoader.FileLoaderSetup.read_message();
                alert(msg.value);return;
            }
            
            document.getElementById("txtDateLastModified").value    = get_current_date_string().substring(0, 10);
                              
            this.commons = new common_method();  
            this.commons.to_read_only("tblDestinationTableField",false);
            this.commons.to_read_only("tblDestinationTable",false);
            this.commons.to_read_only("tblFileLoaderSetup",false);
            this.commons.to_read_only("tblFileUploadType",false);
            this.commons.to_read_only("tblRecordPosition",false);  
            this.commons.to_read_only("tblTempTableField",false);
            this.commons.to_read_only("tblCIFClass",false);
            this.commons.to_read_only("tblConditions",false);
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;
            
            document.getElementById("divRecordPosition").style.display  = "block";            
            document.getElementById("txtDateCreated").disabled          = true;
            document.getElementById("txtDateLastModified").disabled     = true;
            document.getElementById("ddNoofFiles").disabled             = true;
            document.getElementById("txtFileSequenceNo").disabled       = true;
            
            if(strUploadType = "single")
            {
                document.getElementById("ddNoofFiles").disable          = true;
                document.getElementById("txtFileSequenceNo").disable    = true;
            }
        }
    }
    
    this.commit_record = function()
    {   
        IRSWorld.IRSFileLoader.FileLoaderSetup.commit_record(this.set_File_Loader(), this.commit_record_callback);
    }
    
    this.commit_record_callback = function(result)
    {        
         if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.IRSFileLoader.FileLoaderSetup.read_message();alert(msg.value);return;
            }
            
            alert("RECORD COMMITTED SUCCESSFULLY");
            this.SEQ_ID = IRSWorld.IRSFileLoader.FileLoaderSetup.FILE_LOADER_SEQ_ID().value;
            this.execute_ajaxgridview_command('ajx_search$' + document.getElementById("ddFileFormat").value);
            this.commons = new common_method();
            this.commons.to_unlock_button("tblbuttons","btnAdd~");
            //this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit");
            
            this.commons.to_read_only("tblDestinationTableField",true);
            this.commons.to_read_only("tblDestinationTable",true);
            this.commons.to_read_only("tblFileLoaderSetup",true);
            this.commons.to_read_only("tblFileUploadType",true);
            this.commons.to_read_only("tblTempTableField",true);            
            this.commons.to_read_only("tblRecordPosition",true);
            this.commons.to_read_only("tblRecordPosition",true);
            this.commons.to_read_only("tblConditions",true);
            this.commons.to_read_only("tblCIFClass",true);
            
            this.commons.to_clear_screen("tblDestinationTableField");
            this.commons.to_clear_screen("tblDestinationTable");
            this.commons.to_clear_screen("tblFileLoaderSetup");            
            this.commons.to_clear_screen("tblFileUploadType");
            this.commons.to_clear_screen("tblTempTableField");            
            this.commons.to_clear_screen("tblRecordPosition");
            this.commons.to_clear_screen("tblRecordPosition");
            this.commons.to_clear_screen("tblConditions");
            this.commons.to_clear_screen("tblCIFClass");
            this.commons = null;
            
            this.loadergrid = new grid_viewer();
            this.loadergrid.sql_source  = "sp_fileloader~";
            this.loadergrid.div_container = document.getElementById("DefinitionMapperGrid");
            this.loadergrid.datarow_column = 0;
            this.loadergrid.open_grid_view();
            this.grid = null;
            
            IRSWorld.IRSFileLoader.FileLoaderSetup.getFilterBy(this.dropdown_filter_callback);
        }
    }
            
    this.rollback_record = function()
    {        
        this.clear_filesetup();
        this.dropdown_fileloader_fields();
        IRSWorld.IRSFileLoader.FileLoaderSetup.rollback_record(this.rollback_record_callback);
    }    
    
    this.rollback_record_callback = function(result)
    {
        var msg = IRSWorld.IRSFileLoader.FileLoaderSetup.read_message();

        if (msg != null && msg.value.length > 0)
        {
            alert(msg.value);
        }
        this.commons = new common_method();        
        this.commons.to_read_only("tblDestinationTableField", true);
        this.commons.to_read_only("tblDestinationTable", true);
        this.commons.to_read_only("tblFileLoaderSetup",true);
        this.commons.to_read_only("tblFileUploadType",true);
        this.commons.to_read_only("tblRecordPosition", true);
        this.commons.to_read_only("tblTempTableField", true);
        this.commons.to_read_only("tblCIFClass", true);
        this.commons.to_read_only("tblConditions",true);
        this.commons.to_unlock_button("tblbuttons","btnAdd~");
        this.commons = null;                
        
        document.getElementById("divConditions").style.display              = "none"; 
        document.getElementById("divTempTableField").style.display          = "none";
        document.getElementById("divDestinationTableField").style.display   = "block";
        return;
    }
    
    this.clear_filesetup = function()
    {   
        this.SEQ_ID = null;
        document.getElementById("txtHiddenSeqID").value = "";
        //document.getElementById("btnEdit").disabled = "disabled"; 
        this.commons = new common_method();
        this.commons.to_clear_screen("tblFileLoaderSetup");
        this.commons.to_clear_screen("tblFileUploadType");
        this.commons.to_clear_screen("tblRecordPosition");
        this.commons.to_clear_screen("tblCIFClass");  
        this.commons.to_clear_screen("tblDestinationTable");  
        this.commons.to_clear_screen("tblDestinationTableField");        
        this.commons = null;
    }
    
    this.create_temp_table = function()
    {
        this.clear_temptablesetup();        
    } 
    
    this.clear_temptablesetup = function()
    {   
        this.commons = new common_method();
        this.commons.to_clear_screen("tblLoaderFields");
        this.commons = null;
    }    
    
    this.load_record = function()
    {
        this.pull_server_data();       
        this.populate_control();
    } 
    
    //called by page
    this.add_temptable_lookupvalues = function()
    {
        this.add_lookup_values();
        this.add_to_temp_table_list();
        this.create_temp_table();
    }
    
    this.check_numeric = function ()
    {
        if (event.keyCode >= 48 && event.keyCode <= 57)
        {
            event.returnValue = true;
        }
        else
        {
            event.returnValue=false;
            alert("Please input only numbers");
        }
    }
    
    this.disable_datalength = function(datatypename)
    {
        var controlno;

        if (document.getElementById(datatypename).value == "alphanumeric")
        {
            document.getElementById("txtDataLength" + String(datatypename.substring(10))).disabled  = false;
            document.getElementById("txtDataLength" + String(datatypename.substring(10))).value = ""
        }
        else
        {
            document.getElementById("txtDataLength" + String(datatypename.substring(10))).disabled  = true;  
            document.getElementById("txtDataLength" + String(datatypename.substring(10))).value = "system default"
        }
    }
    
    this.add_temp_table = function()
    {
        this.commons = new common_method();                        
        this.commons.to_clear_screen("tblCIFClass");
        this.commons.to_clear_screen("tblDestinationTable");
        this.commons.to_clear_screen("tblDestinationTableField");
        this.commons.to_clear_screen("tblConditions");
        this.commons.to_clear_screen("tblTempTableField");
        this.commons.to_clear_screen("tblRecordPosition");        

        this.commons.to_read_only("tblFileLoaderSetup",false);
        this.commons.to_read_only("tblFileUploadType",false);
        this.commons.to_read_only("tblCIFClass",false);
        this.commons = null;
    }
    
    this.create_temp_table = function()
    {
        var intNoofFields = document.getElementById("ddNoofFields").value;
        var strQueryCreateTempTable;
        var strFieldName;
        var strDataType;
        var strDataLength;
                        
        strQueryCreateTempTable = "CREATE TABLE " + document.getElementById("txtTempTableName").value + " " +
                                    "( ";
       
        for (var i = 1; i <= intNoofFields; i++)
        {
            strFieldName = String(document.getElementById("txtFieldName" + String(i)).value);

            if (document.getElementById("ddDataType" + String(i)).value == "numeric")
            {
                strDataType     = " numeric "
                strDataLength   = " (18, 0) null "
            }
            else if (document.getElementById("ddDataType" + String(i)).value == "alphanumeric")
            {
                strDataType     = " varchar "
                strDataLength   = "(" + document.getElementById("txtDataLength" + String(i)).value + ") null "
            }
            else if (document.getElementById("ddDataType" + String(i)).value == "money")
            {
                strDataType     = " money "
                strDataLength   = "null "
            }
            else if (document.getElementById("ddDataType" + String(i)).value == "date")
            {
                strDataType     = " datetime "
                strDatalength   = "null "
            }
            
            if (i == intNoofFields)
            {
                strQueryCreateTempTable += strFieldName + strDataType + strDataLength + ")"; // " varchar(250) null)";
            }
            else
            {
                strQueryCreateTempTable += strFieldName + strDataType + strDataLength + ", "; // " varchar(250) null, ";
            }
        }
        
        IRSWorld.IRSFileLoader.FileLoaderSetup.createTempTable (strQueryCreateTempTable);
        alert("Record Successfully Comitted");
        this.cancel_create_table_controls();
    }
    
    this.add_lookup_values = function ()
    {
        var intNoofFiles = document.getElementById("ddNoofFilesTempTable").value;
        var strQueryAddLookupValues;
        var strFileName;
        
        strQueryAddLookupValues =   "SELECT MAX(fldvalue) " +
                                    "FROM tbl_lookup " +
                                    "WHERE FLDGROUP = '" + document.getElementById("ddFileUploadTypeTempTable").options[document.getElementById("ddFileUploadTypeTempTable").selectedIndex].text + "'";
        
        intMaxRecord = IRSWorld.IRSFileLoader.FileLoaderSetup.getComplianceMaxValue(strQueryAddLookupValues);
        
        for (i = 1; i <= intNoofFiles; i++)
        {
            strFileName = String(document.getElementById("txtFileName" + String(i)).value);

            strQueryAddLookupValues =   "INSERT INTO TBL_LOOKUP " +
                                        "VALUES ('IRSCompliance', '" + 
                                        document.getElementById("ddFileFormatTempTable").options[document.getElementById("ddFileFormatTempTable").selectedIndex].text + "-" + strFileName + "', " + 
                                        (intMaxRecord.value + i) + ", '" +
                                        strFileName + "', " +
                                        "'0')";

            IRSWorld.IRSFileLoader.FileLoaderSetup.addRecordtoLookup(strQueryAddLookupValues);
        }
    }
    
    this.add_to_temp_table_list = function ()
    {
        var strQueryAddtoTableList;
        var strFileNames;
        var intNoofFiles = document.getElementById("ddNoofFilesTempTable").value;
     
        strFileName = "";
        if (intNoofFiles == 1)
        {                
            strQueryAddtoTableList =    "INSERT INTO TBL_LIST_TEMP_TABLES " +
                                        "VALUES ('" + document.getElementById("txtTempTableName").value + "', " +
                                        document.getElementById("ddFileUploadTypeTempTable").value + ", " +                                         
                                        document.getElementById("ddFileFormatTempTable").value + ", " + 
                                        document.getElementById("ddFileFormatTempTable").value + ", " + 
                                        1 + ", " +
                                        "getdate(), getdate())";
                                        
            IRSWorld.IRSFileLoader.FileLoaderSetup.addRecordtoTempTableList (strQueryAddtoTableList);  
        }
        else
        {
            for (i = 1; i <= intNoofFiles; i++)
            {
                strQueryAddtoTableList =    "INSERT INTO TBL_LIST_TEMP_TABLES " +
                                            "VALUES ('" + document.getElementById("txtTempTableName").value + "', " +
                                            document.getElementById("ddFileUploadTypeTempTable").value + ", " +
                                            document.getElementById("ddFileFormatTempTable").value + ", " + 
                                            (intMaxRecord.value + i) + ", " + 
                                            i + ", " +
                                            "getdate(), getdate())";

                IRSWorld.IRSFileLoader.FileLoaderSetup.addRecordtoTempTableList (strQueryAddtoTableList);
            }
        }
    }
    
    this.check_table = function ()
    {
        var strQueryCheckTempTable;
        var strTableName;
        var strFileSequence;
        var ds
        strTableName    = "";
        strFileSeqence  = "";
        strNoofFiles    = "";

       document.getElementById("multiplefiles").style.display     = "none";
        
        FileFormat = document.getElementById("ddFileFormat").options[document.getElementById("ddFileFormat").selectedIndex].value;
        FileUploadType = document.getElementById("ddFileUploadType").options[document.getElementById("ddFileUploadType").selectedIndex].value;

        strQueryCheckTempTable =    "SELECT tablename " +
                                    "FROM tbl_list_temp_tables " +
                                    "WHERE childfileformat =" + FileFormat + " " +
                                    "AND fileuploadtype =" + FileUploadType + "";   
                                        
        strTableName = IRSWorld.IRSFileLoader.FileLoaderSetup.checkTempTableList(strQueryCheckTempTable);

        if (strTableName.value != "" || strTableName.value.length > 0)
        {
            strQueryCheckTempTable =    "SELECT filesequence " +
                                        "FROM tbl_list_temp_tables " +
                                        "WHERE childfileformat =" + FileFormat + " " +
                                        "AND fileuploadtype =" + FileUploadType + "";
            
            strFileSequence = IRSWorld.IRSFileLoader.FileLoaderSetup.checkTempTableList(strQueryCheckTempTable);
            
            document.getElementById("divTempTableField").style.display          = "block";
            document.getElementById("ddtempTableField").disabled                = false;
            document.getElementById("divRecordPosition").style.display          = "block";
            document.getElementById("divDestinationTableField").style.display   = "none";
            document.getElementById("ddNoofFiles").selectedIndex                = 1;
            document.getElementById("ddNoofFiles").disabled                     = true;
            document.getElementById("txtFileSequenceNo").value                  = strFileSequence.value;
            document.getElementById("txtFileSequenceNo").disabled               = true;             
            document.getElementById("txtDelimiter").value                       = "";
            document.getElementById("txtDelimiter").disabled                    = false;
            document.getElementById("txtStartAtLine").value                     = "";              
            document.getElementById("txtStartAtLine").disabled                  = false;
            document.getElementById("txtRecordField").value                     = "";
            document.getElementById("txtRecordField").disabled                  = false;
            document.getElementById("txtRecordLen").value                       = "";
            document.getElementById("txtRecordLen").disabled                    = false;
                
            if (Number(strFileSequence.value) > 1)
            {
                IRSWorld.IRSFileLoader.FileLoaderSetup.getFieldNames(strTableName.value, this.dropdown_Condition_callback);                
                document.getElementById("divConditions").style.display          = "block";
                this.commons = new common_method();
                this.commons.to_read_only("tblConditions",false);
                this.commons = null;
            }
            else
            {
                document.getElementById("divConditions").style.display          = "none";
            }
            
            ds = IRSWorld.IRSFileLoader.FileLoaderSetup.getFieldNames (strTableName.value);
            fileloader.dropdown_temptablefields_callback(ds.value.Tables[0]);
            
            for (var i = 0; i < document.getElementById("ddtempTableField").length; i++)
            {
                if (document.getElementById("ddtempTableField").options[i].value == TempTableField)
                {
                    document.getElementById("ddtempTableField").selectedIndex   = i;
                    break;
                }
            }
        }
        else
        {
            strQueryCheckTempTable =    "SELECT count(*) " +
                                        "FROM tbl_list_temp_tables " +
                                        "WHERE parentfileformat =" + FileFormat + " " +
                                        "AND fileuploadtype =" + FileUploadType + "";

            strNoofFiles = IRSWorld.IRSFileLoader.FileLoaderSetup.checkTempTableList(strQueryCheckTempTable);

            if (Number(strNoofFiles.value) > 1)
            {
                document.getElementById("txtStartAtLine").disabled                  = true;
                document.getElementById("txtStartAtLine").value                     = "";
                document.getElementById("txtRecordField").disabled                  = true;
                document.getElementById("txtRecordField").value                     = "";
                document.getElementById("txtRecordLen").disabled                    = true;
                document.getElementById("txtRecordLen").value                       = "";
                
                document.getElementById("ddNoofFiles").selectedIndex                = strNoofFiles.value;
                document.getElementById("ddNoofFiles").disabled                     = true;
                document.getElementById("txtFileSequenceNo").value                  = 0;
                document.getElementById("txtFileSequenceNo").disabled               = true; 
                document.getElementById("txtDelimiter").value                       = "";
                document.getElementById("txtDelimiter").disabled                    = true;
                
                document.getElementById("divTempTableField").style.display          = "block";
                document.getElementById("divDestinationTableField").style.display   = "block"
                document.getElementById("divRecordPosition").style.display          = "none";
                document.getElementById("divConditions").style.display              = "none";   
                
                this.commons = new common_method();                    
                this.commons.to_read_only("tblTempTableField", false);
                if (strTableName.value == null || strTableName.value == "")
                { 
                    if (FileFormat == null)
                    {
                        FileFormat = document.getElementById("ddFileFormat").options[document.getElementById("ddFileFormat").selectedIndex].value;
                    } 
                    strQueryCheckTempTable =    "SELECT tablename " +
                                                "FROM tbl_list_temp_tables " +
                                                "WHERE parentfileformat =" + FileFormat + " " +
                                                "AND fileuploadtype =" + FileUploadType + "";  
                                                  
                    strTableName = IRSWorld.IRSFileLoader.FileLoaderSetup.checkTempTableList(strQueryCheckTempTable);
                }
                
                if (strTableName.value != "" || strTableName.value.length > 0)
                {
                    ds = IRSWorld.IRSFileLoader.FileLoaderSetup.getTemp_FinalFieldNames(strTableName.value);
                    fileloader.dropdown_temptablefields_callback(ds.value.Tables[0]);
                    fileloader.dropdown_final_table(ds.value.Tables[1]);
                }                
            }
            else
            {            
                document.getElementById("divTempTableField").style.display          = "none";
                document.getElementById("ddNoofFiles").selectedIndex                = 1;
                document.getElementById("ddNoofFiles").disabled                     = true;
                document.getElementById("txtFileSequenceNo").value                  = 0;;
                document.getElementById("txtFileSequenceNo").disabled               = true;                 
                document.getElementById("txtDelimiter").value                       = "";
                document.getElementById("txtDelimiter").disabled                    = false;  
                document.getElementById("txtStartAtLine").value                     = "";              
                document.getElementById("txtStartAtLine").disabled                  = false;
                document.getElementById("txtRecordField").value                     = "";
                document.getElementById("txtRecordField").disabled                  = false;
                document.getElementById("txtRecordLen").value                       = "";
                document.getElementById("txtRecordLen").disabled                    = false;
                document.getElementById("divConditions").style.display              = "none";            
                document.getElementById("divDestinationTableField").style.display   = "block"
                document.getElementById("divRecordPosition").style.display          = "block";
                
                IRSWorld.IRSFileLoader.FileLoaderSetup.getFieldNames(document.getElementById("ddFileUploadType").options[document.getElementById("ddFileUploadType").selectedIndex].text, this.dropdown_fieldnames_callback);
            }
        }
    }
        
    this.check_temp_table = function ()
    {
        var strQueryCheckTempTable;
        var strTableName;
        var strFileSequence;
        var strNoofFiles;
        strTableName = "";
        strFileSeqence = "";
        strNoofFiles = "";
        this.temp_table_values() 
        document.getElementById("multiplefiles").style.display     = "none";
        
        FileFormat = document.getElementById("ddFileFormatTempTable").options[document.getElementById("ddFileFormatTempTable").selectedIndex].value;
		FileUploadType = document.getElementById("ddFileUploadTypeTempTable").options[document.getElementById("ddFileUploadTypeTempTable").selectedIndex].value;

        strQueryCheckTempTable =    "SELECT count(*) " +
                                    "FROM tbl_list_temp_tables " +
                                    "WHERE parentfileformat =" + FileFormat + " " +
                                    "AND fileuploadtype =" + FileUploadType + "";

        strNoofFiles = IRSWorld.IRSFileLoader.FileLoaderSetup.checkTempTableList(strQueryCheckTempTable);

        if (Number(strNoofFiles.value) > 1)
        {
            document.getElementById("ddNoofFilesTempTable").selectedIndex   = strNoofFiles.value;
            document.getElementById("ddNoofFilesTempTable").disabled        = true;
            document.getElementById("multiplefiles").style.display          = "block"

            if (strTableName.value == null || strTableName.value == "")
            { 
                if (FileFormat == null)
                {
                    FileFormat = document.getElementById("ddFileFormatTempTable").options[document.getElementById("ddFileFormatTempTable").selectedIndex].value;
                }

                strQueryCheckTempTable =    "SELECT tablename " +
                                            "FROM tbl_list_temp_tables " +
                                            "WHERE parentfileformat =" + FileFormat + " " +
                                            "AND fileuploadtype =" + FileUploadType + "";
                                            
                strTableName = IRSWorld.IRSFileLoader.FileLoaderSetup.checkTempTableList(strQueryCheckTempTable);  
                document.getElementById("txtTempTableName").value = strTableName.value;
                this.show_temp_fields (strTableName.value);
            }             
            this.show_temp_file_names(FileUploadType);            
        }
        else if (Number(strNoofFiles.value) == 1)
        {   
            document.getElementById("ddNoofFilesTempTable").selectedIndex   = strNoofFiles.value;
            document.getElementById("ddNoofFilesTempTable").disabled        = true;
            document.getElementById("multiplefiles").style.display          = "block"

            if (strTableName.value == null || strTableName.value == "")
            { 
                if (FileFormat == null)
                {
                    FileFormat = document.getElementById("ddFileFormatTempTable").options[document.getElementById("ddFileFormatTempTable").selectedIndex].value;
                }

                strQueryCheckTempTable =    "SELECT tablename " +
                                            "FROM tbl_list_temp_tables " +
                                            "WHERE parentfileformat =" + FileFormat + " " +
                                            "AND fileuploadtype =" + FileUploadType + "";
                                            
                strTableName = IRSWorld.IRSFileLoader.FileLoaderSetup.checkTempTableList(strQueryCheckTempTable);  
                document.getElementById("txtTempTableName").value = strTableName.value;
                this.show_temp_fields (strTableName.value);
            } 
        }
        else
        {                            
            document.getElementById("ddNoofFilesTempTable").selectedIndex   = 0;
            document.getElementById("ddNoofFilesTempTable").disabled        = "disabled";
            document.getElementById("divConditions").style.display          = "none";
            this.commons = new common_method();
            this.commons.to_unlock_button("tbltemptablebuttons","btnAddTempTable~");
            this.commons = null;
        }
    }
    
    this.reset_create_temp_table = function()
    {
        document.getElementById("ddNoofFilesTempTable").selectedIndex   = 0;
        document.getElementById("ddNoofFilesTempTable").disabled        = false;
        document.getElementById("divConditions").style.display          = "none";
        document.getElementById("txtDelimiter").value                   = "";
        document.getElementById("txtDelimiter").disabled                = false;
        this.commons = new common_method();
        this.commons.to_unlock_button("tbltemptablebuttons","btnAddTempTable~");
        this.commons = null;    
    }
    
    this.enable_create_table_controls = function(result)
    {
        this.commons = new common_method();                        
        this.commons.to_read_only("tblmultiplefields",result);
        this.commons = null;
    }
    
    this.add_create_table_controls = function()
    {        
        AddEditTempTable = "Add";
        document.getElementById("ddNoofFilesTempTable").disabled = "";
        
        this.commons = new common_method();        
        this.commons.to_clear_screen("tbmultiplefiles"); 
        this.commons.to_clear_screen("tblmultiplefields"); 
        for (var i = 0; i < 40; i++)
        {
            this.commons.to_clear_screen("tbl" + String(i + 1));
            this.commons.to_read_only("tbl" + String(i + 1),false);
        }
        this.commons.to_unlock_button("tbltemptablebuttons","btnSaveTempTable~btnCancelTempTable");
        this.commons = null;
    }
    this.edit_create_table_controls = function()
    {
        AddEditTempTable = "Edit";
        
        this.commons = new common_method();       
        this.commons.to_read_only("tbmultiplefiles",false);  
        this.commons.to_read_only("tblmultiplefields",false);  
        
        for (var i = 0; i < 40; i++)
        {
            this.commons.to_read_only("tbl" + String(i + 1),false);            
        }
        
        for (var i = 0; i < 3; i++)
        {        
            this.commons.to_read_only("tblFileName" + String(i + 1),false);  
        }
        
        this.commons.to_unlock_button("tbltemptablebuttons","btnSaveTempTable~btnCancelTempTable");
        this.commons = null;
    }
    
    this.cancel_create_table_controls = function()
    {
        this.commons = new common_method();
        if (AddEditTempTable == "Add")
        {
            this.commons.to_clear_screen("tbmultiplefiles"); 
            this.commons.to_clear_screen("tblmultiplefields");    
            for (var i = 0; i < 40; i++)
            {
                this.commons.to_clear_screen("tbl" + String(i + 1));
                this.commons.to_read_only("tbl" + String(i + 1),true);            
            }           
            
            for (var i = 0; i < 3; i++)
            {        
                this.commons.to_clear_screen("tblFileName" + String(i + 1));
                this.commons.to_read_only("tblFileName" + String(i + 1),true);  
            }       
            
            this.commons.to_unlock_button("tbltemptablebuttons","btnAddTempTable~");            
            document.getElementById("ddFileUploadTypeTempTable").selectedIndex = -1;
            document.getElementById("ddFileFormatTempTable").selectedIndex = -1;
            document.getElementById("ddNoofFilesTempTable").selectedIndex = -1;
            document.getElementById("ddNoofFields").selectedIndex = -1;
            
            var y;
            
            var div = new Array("fileone","filetwo","filethree"); // condensed array
            for(y = 0; y < div.length; y++)
            {
                document.getElementById(div[y]).style.display       = "none";
            }            
            
            document.getElementById("multiplefiles").style.display  = "none";
            var div = new Array("Div1","Div2","Div3","Div4","Div5","Div6","Div7","Div8","Div9","Div10","Div11","Div12","Div13","Div14","Div15","Div16","Div17","Div18","Div19","Div20","Div21","Div22","Div23","Div24","Div25","Div26","Div27","Div28","Div29","Div30","Div31","Div32","Div33","Div34","Div35","Div36","Div37","Div38","Div39","Div40"); // condensed array
            for(y = 0; y < div.length; y++)
            {
                document.getElementById(div[y]).style.display       = "none";
            }     
            this.commons.to_unlock_button("tbltemptablebuttons","btnAddTempTable~");
        }
        else if (AddEditTempTable == "Edit")
        {
            for (var i = 0; i < 40; i++)
            {
                this.commons.to_read_only("tbl" + String(i + 1),true);            
            }           
            
            for (var i = 0; i < 3; i++)
            {        
                this.commons.to_read_only("tblFileName" + String(i + 1),true);  
            }       
                        
            var y;            
            var div = new Array("fileone","filetwo","filethree"); // condensed array            
            for(y = 0; y < Number(document.getElementById("ddNoofFilesTempTable").value); y++)
            {
                document.getElementById(div[y]).style.display       = "block";
            }            
            
            var div = new Array("Div1","Div2","Div3","Div4","Div5","Div6","Div7","Div8","Div9","Div10","Div11","Div12","Div13","Div14","Div15","Div16","Div17","Div18","Div19","Div20","Div21","Div22","Div23","Div24","Div25","Div26","Div27","Div28","Div29","Div30","Div31","Div32","Div33","Div34","Div35","Div36","Div37","Div38","Div39","Div40"); // condensed array
            
            for(y = 0; y < Number(document.getElementById("ddNoofFields").value); y++)
            {
                document.getElementById(div[y]).style.display       = "block";
            }            
            this.commons.to_read_only("tbmultiplefiles", true);                 
            this.commons.to_unlock_button("tbltemptablebuttons","btnEditTempTable~");
        }        
        this.commons = null;        
    }
    
    this.dropdown_TableList_callback = function(result)
    {            
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddDestinationTable",   null, "table_name", "table_name", result);
            this.commons = null;
        }
    }    
    
    this.dropdown_temptablefields_callback = function(result)
    {            
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddtempTableField",   null, "name", "name", result);
            this.commons.to_load_dropdownlist("ddCondition1",       null, "name", "name", result);
            this.commons.to_load_dropdownlist("ddCondition2",       null, "name", "name", result);
            this.commons.to_load_dropdownlist("ddCondition3",       null, "name", "name", result);
            this.commons = null;
        }
    }
    
    this.dropdown_Condition_callback = function (result)
    {
        var dtTable =  result.value.Tables[0];
        if (result.value != null && result.context != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddCondition1", null, "name", "name", dtTable);
            this.commons.to_load_dropdownlist("ddCondition2", null, "name", "name", dtTable);
            this.commons.to_load_dropdownlist("ddCondition3", null, "name", "name", dtTable);
            this.commons = null;
        }
    }      
    
    this.pull_server_data = function()
    {                 
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = IRSWorld.IRSFileLoader.FileLoaderSetup.FILE_LOADER_SEQ_ID().value;
        }
        if (this.SEQ_ID == null ||this.SEQ_ID == '' || this.SEQ_ID == 'undefined')
        {
            return;
        }
        
        IRSWorld.IRSFileLoader.FileLoaderSetup.GET_FILELOADER_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    } 
        
    this.populate_control = function()
    {
    
        this.commons = new common_method();
        this.commons.to_set_dropdownlist_index("ddFileFormat",      FileFormat);        
        this.commons.to_set_dropdownlist_index("ddNoofFiles",       strNoofFiles);
        this.commons.to_set_dropdownlist_index("ddCondition1",      Condition1);
        this.commons.to_set_dropdownlist_index("ddOperator1",       Operator1);
        this.commons.to_set_dropdownlist_index("ddCondition2",      Condition2);
        this.commons.to_set_dropdownlist_index("ddOperator2",       Operator2);
        this.commons.to_set_dropdownlist_index("ddCondition3",      Condition3);
        this.commons.to_set_dropdownlist_index("ddDestinationTableField",     DestinationTableField);
        this.commons.to_set_dropdownlist_index("ddtempTableField",  TempTableField);        
        this.commons.to_set_dropdownlist_index("ddFileFormat",      FileFormat);        
        this.commons.to_set_dropdownlist_index("ddNoofFiles",       strNoofFiles);
        this.commons.to_set_dropdownlist_index("ddCondition1",      Condition1);
        this.commons.to_set_dropdownlist_index("ddOperator1",       Operator1);
        this.commons.to_set_dropdownlist_index("ddCondition2",      Condition2);
        this.commons.to_set_dropdownlist_index("ddOperator2",       Operator2);
        this.commons.to_set_dropdownlist_index("ddCondition3",      Condition3);
        this.commons.to_set_dropdownlist_index("ddtempTableField",  TempTableField);        
        
        document.getElementById("divConditions").style.display              = "block"; 
        document.getElementById("divTempTableField").style.display          = "block";
        document.getElementById("divDestinationTableField").style.display   = "block";
        
        
        document.getElementById("txtHiddenSeqID").value                     = this.SEQ_ID;
        document.getElementById("txtFileSequenceNo").value                  = FileSequenceNo;
        document.getElementById("txtDelimiter").value                       = Delimiter;

        document.getElementById("txtRecordPosition").value                  = RecordPosition;
        if (RecordLength == -1)
        {
            document.getElementById("txtRecordLen").value                   = "";
        }
        else
        {
            document.getElementById("txtRecordLen").value                   = RecordLength;
        }
        if (StartAtLine == -1)
        {
            document.getElementById("txtStartAtLine").value                 = "";
        }
        else
        {
            document.getElementById("txtStartAtLine").value                 = StartAtLine;
        }
        
        if (SplitIndex == -1)
        {   
            document.getElementById("txtSplitIndex").value                  = "";
        }
        else
        {
            document.getElementById("txtSplitIndex").value                  = SplitIndex;
        }
        
        document.getElementById("txtInvalidCharacters").value               = InvalidCharacters;                
        
        if (ConditionPosition1 == -1)
        {
            document.getElementById("txtConditionPosition1").value          = "";
        }
        else
        {
            document.getElementById("txtConditionPosition1").value          = ConditionPosition1;
        }
        if (ConditionPosition2 == -1)
        {
            document.getElementById("txtConditionPosition2").value          = "";
        }
        else
        {
            document.getElementById("txtConditionPosition2").value          = ConditionPosition2;
        }
        
        if (ConditionPosition3 == -1)
        {
            document.getElementById("txtConditionPosition3").value          = "";
        }
        else
        {
            document.getElementById("txtConditionPosition3").value          = ConditionPosition3;
        }
        document.getElementById("txtDateCreated").value                     = DateCreated;
        document.getElementById("txtDateLastModified").value                = DateLastModified; 
    }

    this.pull_server_data_callback = function(result)
    {
        var dataset;
        dataset = null;
        dataset = result.value;
    
        if (result != null && result.value != null)
        {       
            this.SEQ_ID                 = dataset.Tables[0].Rows[0].SEQ_ID;
            FileUploadType              = dataset.Tables[0].Rows[0].FileUploadType;
            FileFormat                  = dataset.Tables[0].Rows[0].FileFormat;
            
            if (dataset.Tables[0].Rows[0].FileSequenceNo == null)
            {   
                FileSequenceNo              = "0";
            }
            else
            {
                FileSequenceNo              = dataset.Tables[0].Rows[0].FileSequenceNo;
            }
            
            if (dataset.Tables[0].Rows[0].Delimiter == null)
            {   
                Delimiter                   = "";
            }
            else
            {
                Delimiter                   = dataset.Tables[0].Rows[0].Delimiter;
            }            
            
            if (dataset.Tables[0].Rows[0].RecordField == null)
            {   
                RecordField                 = "";
            }
            else
            {
                RecordField                 = dataset.Tables[0].Rows[0].RecordField;
            }
            
            if (dataset.Tables[0].Rows[0].RecordPosition == null)
            {   
                RecordPosition                 = "";
            }
            else
            {
                RecordPosition              = dataset.Tables[0].Rows[0].RecordPosition + 1;
            }
                        
            if (dataset.Tables[0].Rows[0].RecordLength == null)
            {   
                RecordLength                 = "";
            }
            else
            {
                RecordLength                = dataset.Tables[0].Rows[0].RecordLength;
            }
            
            if (dataset.Tables[0].Rows[0].TempTableField == null)
            {   
                TempTableField                 = "";
            }
            else
            {
                TempTableField              = dataset.Tables[0].Rows[0].TempTableField;   
            }   
            
            if (dataset.Tables[0].Rows[0].DestinationTable == null)
            {   
                DestinationTable                 = "";
            }
            else
            {
                DestinationTable            = dataset.Tables[0].Rows[0].DestinationTable;    
            }    
            if (dataset.Tables[0].Rows[0].DestinationTableField == null)
            {   
                DestinationTableField                 = "";
            }
            else
            {
                DestinationTableField       = dataset.Tables[0].Rows[0].DestinationTableField;   
            }    
            
            if (dataset.Tables[0].Rows[0].CIFClass == null)
            {   
                CIFClass                 = "-1";
            }
            else
            {
                CIFClass                    = dataset.Tables[0].Rows[0].CIFClass; 
            }
            
            if (dataset.Tables[0].Rows[0].StartAtLine == null)
            {   
                StartAtLine               = "";
            }
            else
            {
                StartAtLine                 = dataset.Tables[0].Rows[0].StartAtLine;
            }    
            
            if (dataset.Tables[0].Rows[0].SplitChar == null)
            {   
                SplitChar               = "";
            }
            else
            {
                SplitChar                   = dataset.Tables[0].Rows[0].SplitChar;
            }    
            
            if (dataset.Tables[0].Rows[0].SplitIndex == null)
            {   
                SplitIndex               = "";
            }
            else
            {
                SplitIndex                  = dataset.Tables[0].Rows[0].SplitIndex + 1;
            }    
            
            if (dataset.Tables[0].Rows[0].InvalidCharacters == null)
            {   
                InvalidCharacters               = "";
            }
            else
            {
                InvalidCharacters           = dataset.Tables[0].Rows[0].InvalidCharacters;
            }    
                        
            if (dataset.Tables[0].Rows[0].Condition1 == null)
            {   
                Condition1               = "";
            }
            else
            {
                Condition1                  = dataset.Tables[0].Rows[0].Condition1;
            }    
            
            if (dataset.Tables[0].Rows[0].ConditionPosition1 == null)
            {   
                ConditionPosition1               = "";
            }
            else
            {
                ConditionPosition1          = dataset.Tables[0].Rows[0].ConditionPosition1 + 1;
            }
            
            if (dataset.Tables[0].Rows[0].Operator1 == null)
            {   
                Operator1               = "";
            }
            else
            {
                Operator1                   = dataset.Tables[0].Rows[0].Operator1;
            }
            
            if (dataset.Tables[0].Rows[0].Condition2 == null)
            {   
                Condition2               = "";
            }
            else
            {
                Condition2                  = dataset.Tables[0].Rows[0].Condition2;
            }
            
            if (dataset.Tables[0].Rows[0].ConditionPosition2 == null)
            {   
                ConditionPosition2               = "";
            }
            else
            {
                ConditionPosition2          = dataset.Tables[0].Rows[0].ConditionPosition2 + 1;
            }
            
            if (dataset.Tables[0].Rows[0].Operator2 == null)
            {   
                Operator2               = "";
            }
            else
            {
                Operator2                   = dataset.Tables[0].Rows[0].Operator2;
            }
            
            if (dataset.Tables[0].Rows[0].Condition3 == null)
            {   
                Condition3               = "";
            }
            else
            {
                Condition3                  = dataset.Tables[0].Rows[0].Condition3;
            }
            
            if (dataset.Tables[0].Rows[0].ConditionPosition3 == null)
            {   
                ConditionPosition3               = "";
            }
            else
            {
                ConditionPosition3          = dataset.Tables[0].Rows[0].ConditionPosition3 + 1;
            }
            DateCreated                 = dataset.Tables[0].Rows[0].DateCreated;
            DateLastModified            = dataset.Tables[0].Rows[0].DateLastModified;

            document.getElementById("divTempTableField").style.display          = "none";
            document.getElementById("divRecordPosition").style.display          = "none";
            document.getElementById("divDestinationTableField").style.display   = "none";
            document.getElementById("divConditions").style.display              = "none";
            document.getElementById("txtDateCreated").disabled                  = true;
            document.getElementById("txtDateLastModified").disabled             = true; 
            document.getElementById("ddNoofFiles").selectedIndex                = 1;            
            
            
            document.getElementById("ddCondition1").selectedIndex               = -1;
            document.getElementById("txtConditionPosition1").value              = "";
            document.getElementById("ddOperator1").selectedIndex                = -1;
             
            document.getElementById("ddCondition2").selectedIndex               = -1;
            document.getElementById("txtConditionPosition2").value              = "";
            document.getElementById("ddOperator2").selectedIndex                = -1;
            
            document.getElementById("ddCondition3").selectedIndex               = -1;
            document.getElementById("txtConditionPosition3").value              = "";
            
            document.getElementById("divDestinationTable").style.display        = "none";
            document.getElementById("ddDestinationTable").disabled              = true;                        
            document.getElementById("ddDestinationTable").selectedIndex         = -1;

            document.getElementById("divCIFClass").style.display                = "none";
            document.getElementById("ddCIFClass").disabled                      = true;                        
            document.getElementById("ddCIFClass").selectedIndex                 = -1;
                        
            for (var i = 0; i < document.getElementById("ddFileUploadType").length; i++)
            {
                if (document.getElementById("ddFileUploadType").options[i].value == FileUploadType)
                {
                    document.getElementById("ddFileUploadType").selectedIndex   = i;
                    break;
                }
            }
            
            for (var i = 0; i < document.getElementById("ddFileFormat").length; i++)
            {
                if (document.getElementById("ddFileFormat").options[i].value == FileFormat)
                {
                    document.getElementById("ddFileFormat").selectedIndex       = i;
                    break;
                }
            }
            
            if (result.value.Tables.length >= 4) //check the destination field blank
            {    
                fileloader.dropdown_temptablefields_callback(dataset.Tables[2]);              
                fileloader.dropdown_get_fileformat_table(dataset.Tables[3]);
              
                for (var i = 0; i < document.getElementById("ddFileFormat").length; i++)
                {
                    if (document.getElementById("ddFileFormat").options[i].value == FileFormat)
                    {
                        document.getElementById("ddFileFormat").selectedIndex   = i;
                        break;
                    }
                }
                
                if (dataset.Tables[1].Name == "ParentTable")
                {
                    strUploadType                                                       = "parent";
                    document.getElementById("divTempTableField").style.display          = "block";
                    document.getElementById("divDestinationTableField").style.display   = "block";               
                    fileloader.dropdown_final_table(dataset.Tables[4]);
                    
                    for (var i = 0; i < document.getElementById("ddDestinationTableField").length; i++)
                    {
                        if (document.getElementById("ddDestinationTableField").options[i].value == DestinationTableField)
                        {
                            document.getElementById("ddDestinationTableField").selectedIndex    = i;
                            break;
                        }
                    } 
                                        
                    for (var i = 0; i < document.getElementById("ddtempTableField").length; i++)
                    {
                        if (document.getElementById("ddtempTableField").options[i].value == TempTableField)
                        {
                            document.getElementById("ddtempTableField").selectedIndex   = i;
                            break;
                        }
                    }                    
                    document.getElementById("ddNoofFiles").selectedIndex = dataset.Tables[1].Rows.length;
                }
                else if (dataset.Tables[1].Name == "ChildTable")
                {
                    strUploadType = "child";                
                    document.getElementById("divRecordPosition").style.display  = "block";
                    
                    document.getElementById("divTempTableField").style.display  = "block";   
                    document.getElementById("ddtempTableField").disabled       = true;       
                                 
                    document.getElementById("ddNoofFiles").selectedIndex        = 1;
                    document.getElementById("ddNoofFiles").disabled             = true;
                    
                    document.getElementById("txtFileSequenceNo").value          = FileSequenceNo;
                    document.getElementById("txtFileSequenceNo").disabled       = true;    
                    
                    if (Number(dataset.Tables[1].Rows[0]["filesequence"]) > 1)
                    {            
                        document.getElementById("divConditions").style.display  = "block";
                    }
                     
                    for (var i = 0; i < document.getElementById("ddtempTableField").length; i++)
                    {
                        if (document.getElementById("ddtempTableField").options[i].value == TempTableField)
                        {
                            document.getElementById("ddtempTableField").selectedIndex   = i;
                            break;
                        }
                    }
                }
                else if (dataset.Tables[1].Name == "TableList")
                {
                    fileloader.dropdown_TableList_callback(dataset.Tables[1]);                    
                    for (var i = 0; i < document.getElementById("ddDestinationTable").length; i++)
                    {
                        if (document.getElementById("ddDestinationTable").options[i].value == DestinationTable)
                        {
                            document.getElementById("ddDestinationTable").selectedIndex     = i;                            
                            document.getElementById("divDestinationTable").style.display    = "block";
                            break;
                        }
                    }                    
                    
                    fileloader.dropdown_destination_table_field(dataset.Tables[2]);
                    for (var i = 0; i < document.getElementById("ddDestinationTableField").length; i++)
                    {
                        if (document.getElementById("ddDestinationTableField").options[i].value == DestinationTableField)
                        {
                            document.getElementById("ddDestinationTableField").selectedIndex   = i;                            
                            document.getElementById("divDestinationTableField").style.display  = "block";
                            break;
                        }
                    }    
                    
                    for (var i = 0; i < document.getElementById("ddCIFClass").length; i++)
                    {
                        if (document.getElementById("ddCIFClass").options[i].value == CIFClass)
                        {
                            document.getElementById("ddCIFClass").selectedIndex     = i;                            
                            document.getElementById("divCIFClass").style.display    = "block";
                            break;
                        }
                    }                                 
                }
            }
            else
            {
                fileloader.dropdown_get_fileformat_table(dataset.Tables[1]);              
                for (var i = 0; i < document.getElementById("ddFileFormat").length; i++)
                {
                    if (document.getElementById("ddFileFormat").options[i].value == FileFormat)
                    {
                        document.getElementById("ddFileFormat").selectedIndex   = i;
                        break;
                    }
                }
                
                strUploadType = "single";
                document.getElementById("divRecordPosition").style.display          = "block";
                document.getElementById("divDestinationTableField").style.display   = "block";
                fileloader.dropdown_final_table(dataset.Tables[2]);
                
                for (var i = 0; i < document.getElementById("ddDestinationTableField").length; i++)
                {
                    if (document.getElementById("ddDestinationTableField").options[i].value == DestinationTableField)
                    {
                        document.getElementById("ddDestinationTableField").selectedIndex  = i;
                        break;
                    }
                }
            }
            
            document.getElementById("txtFileSequenceNo").value              = FileSequenceNo;     
            document.getElementById("txtDelimiter").value                   = trim(String(Delimiter));
            document.getElementById("txtRecordField").value                 = RecordField;
        
            if (RecordPosition == -1)
            {
                document.getElementById("txtRecordPosition").value          = "";
            }
            else
            {
                document.getElementById("txtRecordPosition").value          = RecordPosition;
            }
            
            if (RecordLength == -1)
            {
                document.getElementById("txtRecordLen").value               = "";
            }
            else
            {
                document.getElementById("txtRecordLen").value               = RecordLength;
            }
            
                                
            if (StartAtLine == -1)
            {
                document.getElementById("txtStartAtLine").value             = "";
            }
            else
            {
                document.getElementById("txtStartAtLine").value             = StartAtLine;
            }

            if (SplitChar == -1)
            {
                document.getElementById("txtSplitChar").value               = "";
            }
            else
            {
                document.getElementById("txtSplitChar").value               = trim(String(SplitChar));
            }
            
            if (SplitIndex == -1)
            {
                document.getElementById("txtSplitIndex").value              = "";
            }
            else
            {
                document.getElementById("txtSplitIndex").value              = SplitIndex;
            }
            
            document.getElementById("txtInvalidCharacters").value           = InvalidCharacters;                
                                
            for (var i = 0; i < document.getElementById("ddCondition1").length; i++)
            {
                if (document.getElementById("ddCondition1").options[i].value == Condition1)
                {
                    document.getElementById("ddCondition1").selectedIndex   = i;
                    break;
                }
            }

            if (ConditionPosition1 == -1)
            {
                document.getElementById("txtConditionPosition1").value      = "";
            }
            else
            {
                document.getElementById("txtConditionPosition1").value      = ConditionPosition1;
            } 
            
            document.getElementById("ddOperator1").value                    = Operator1;
            
            for (var i = 0; i < document.getElementById("ddCondition2").length; i++)
            {
                if (document.getElementById("ddCondition2").options[i].value == Condition2)
                {
                    document.getElementById("ddCondition2").selectedIndex   = i;
                    break;
                }
            }                                    
            
            if (ConditionPosition2 == -1)
            {
                document.getElementById("txtConditionPosition2").value      = "";
            }
            else
            {
                document.getElementById("txtConditionPosition2").value      = ConditionPosition2;
            }       
                              
            document.getElementById("ddOperator2").value                    = Operator2;
            
            for (var i = 0; i < document.getElementById("ddCondition3").length; i++)
            {
                if (document.getElementById("ddCondition3").options[i].value == Condition3)
                {
                    document.getElementById("ddCondition3").selectedIndex   = i;
                    break;
                }
            }

            if (ConditionPosition3 == -1)
            {
                document.getElementById("txtConditionPosition3").value      = "";
            }
            else
            {
                document.getElementById("txtConditionPosition3").value      = ConditionPosition3;
            }

            document.getElementById("txtDateCreated").value                 = DateCreated;
            document.getElementById("txtDateLastModified").value            = DateLastModified; 
        }
    }    

    this.set_File_Loader = function()
    {   
        FileUploadType                  = document.getElementById("ddFileUploadType").value;
        FileFormat                      = document.getElementById("ddFileFormat").value;
        
        if (document.getElementById("txtFileSequenceNo").value == "null" || trim(document.getElementById("txtFileSequenceNo").value) == "")
        {
            FileSequenceNo              = 0;
        }
        else
        {
            FileSequenceNo              = document.getElementById("txtFileSequenceNo").value;
        }   
                
        if (document.getElementById("txtDelimiter").value == "null" || trim(document.getElementById("txtDelimiter").value) == "")
        {
            Delimiter              = '';
        }
        else
        {
            Delimiter              = document.getElementById("txtDelimiter").value;
        }   
        
        if (document.getElementById("txtRecordField").value == "null" || trim(document.getElementById("txtRecordField").value) == "")
        {
            RecordField              = "";
        }
        else
        {
            RecordField              = document.getElementById("txtRecordField").value;
        }          
              
        if (document.getElementById("txtRecordPosition").value == "null" || trim(document.getElementById("txtRecordPosition").value) == "")
        {
            RecordPosition              = -1;
        }
        else
        {
            RecordPosition              = document.getElementById("txtRecordPosition").value - 1;
        }   
        if (document.getElementById("txtRecordLen").value == "null" || trim(document.getElementById("txtRecordLen").value) == "")
        {
            RecordLength                = -1;
        }
        else
        {
            RecordLength                = document.getElementById("txtRecordLen").value;
        } 
        
        if (document.getElementById("ddtempTableField").value == "null" || trim(document.getElementById("ddtempTableField").value) == "")
        {
            TempTableField              = "";
        }
        else
        {
            TempTableField              = document.getElementById("ddtempTableField").value;
        }         
         
        if (document.getElementById("ddDestinationTable").value == "null" || trim(document.getElementById("ddDestinationTable").value) == "")
        {
            DestinationTable              = "";
        }
        else
        {
            DestinationTable              = document.getElementById("ddDestinationTable").value;
        }
                 
        if (document.getElementById("ddDestinationTableField").value == "null")
        {
            DestinationTableField       = "";
        }
        else
        {
            DestinationTableField       = document.getElementById("ddDestinationTableField").value;
        }
        
        if (document.getElementById("ddCIFClass").value == "null" ||trim(document.getElementById("ddCIFClass").value) == "")
        {
            CIFClass                 = -1;
        }
        else
        {
            CIFClass                 = document.getElementById("ddCIFClass").value;
        }
                
        if (document.getElementById("txtStartAtLine").value == "null" ||trim(document.getElementById("txtStartAtLine").value) == "")
        {
            StartAtLine                 = -1;
        }
        else
        {
            StartAtLine                 = document.getElementById("txtStartAtLine").value;
        }
        if (document.getElementById("txtSplitChar").value == "null" || trim(document.getElementById("txtSplitChar").value)== "")
        {
            SplitChar                   = "";
        }
        else
        {        
            SplitChar                   = document.getElementById("txtSplitChar").value;
        }
        
        if (document.getElementById("txtSplitIndex").value == "null" || trim(document.getElementById("txtSplitIndex").value) =="")
        {
            SplitIndex                  = -1;
        }
        else
        {
            SplitIndex                  = document.getElementById("txtSplitIndex").value - 1;
        }
        
        if (document.getElementById("txtInvalidCharacters").value == "null" || trim(document.getElementById("txtInvalidCharacters").value)=="")
        {   
            InvalidCharacters           = ""
        }
        else
        {
            InvalidCharacters           = document.getElementById("txtInvalidCharacters").value;
        }
        if (document.getElementById("ddCondition1").value == "null" || trim(document.getElementById("ddCondition1").value)=="")
        {
            Condition1                  = "";
        }
        else
        {
            Condition1                  = document.getElementById("ddCondition1").value;
        }
        
        if (document.getElementById("txtConditionPosition1").value == "null" || trim (document.getElementById("txtConditionPosition1").value) == "")
        {
            ConditionPosition1          = -1;
        }
        else
        {
            ConditionPosition1          = document.getElementById("txtConditionPosition1").value - 1;
        }
        
        if (document.getElementById("ddOperator1").value == "null" || trim(document.getElementById("ddOperator1").value)== "")
        {
            Operator1                   = "";
        }
        else
        {
            Operator1                   = document.getElementById("ddOperator1").value;
        }
        if (document.getElementById("ddCondition2").value == "null" || trim(document.getElementById("ddCondition2").value) == "")
        {
            Condition2                  = "";
        }
        else
        {
            Condition2                  = document.getElementById("ddCondition2").value;
        }
        if (document.getElementById("txtConditionPosition2").value == "null" || trim(document.getElementById("txtConditionPosition2").value) == "")
        {
            ConditionPosition2          = -1;
        }
        else
        {
            ConditionPosition2          = document.getElementById("txtConditionPosition2").value - 1;
        }
        
        if (document.getElementById("ddOperator2").value == "null" || trim(document.getElementById("ddOperator2").value)== "")
        {
            Operator2                   = "";
        }
        else
        {
            Operator2                   = document.getElementById("ddOperator2").value;
        }
        if (document.getElementById("ddCondition3").value == "null" || trim(document.getElementById("ddCondition3").value)== "")
        {
            Condition3                  = "";
        }
        else
        {
            Condition3                  = document.getElementById("ddCondition3").value;
        }
        if (document.getElementById("txtConditionPosition3").value == "null" || trim(document.getElementById("txtConditionPosition3").value)== "")
        {
            ConditionPosition3          = -1;
        }
        else
        {
            ConditionPosition3          = document.getElementById("txtConditionPosition3").value - 1;
        }        
                
        DateCreated                     = document.getElementById("txtDateCreated").value; //get_current_date_string();
        DateLastModified                = document.getElementById("txtDateLastModified").value; //get_current_date_string();       
        return this.push_data_to_server();
    }

    this.push_data_to_server = function()
    {    
        var data_row = new Array();
        
        data_row[data_row.length]   = "FileUploadType*&^#"             + FileUploadType;
        data_row[data_row.length]   = "FileFormat*&^#"                 + FileFormat;
        data_row[data_row.length]   = "FileSequenceNo*&^#"             + FileSequenceNo;
        data_row[data_row.length]   = "Delimiter*&^#"                  + Delimiter;
        data_row[data_row.length]   = "RecordField*&^#"                + RecordField;
        if (document.getElementById("divRecordPosition").style.display == "block")
        {
            data_row[data_row.length]   = "RecordPosition*&^#"         + RecordPosition;
        }
        else
        {
            data_row[data_row.length]   = "RecordPosition*&^#"         + "-1";
        }        
        data_row[data_row.length]   = "RecordLength*&^#"               + RecordLength;        
        if (document.getElementById("divTempTableField").style.display == "block")
        {
            data_row[data_row.length]   = "TempTableField*&^#"         + TempTableField;
        }
        else
        {
            data_row[data_row.length]   = "TempTableField*&^#"         + "";
        }
        data_row[data_row.length]   = "DestinationTable*&^#"           + DestinationTable;
        data_row[data_row.length]   = "DestinationTableField*&^#"      + DestinationTableField;
        data_row[data_row.length]   = "CIFClass*&^#"                   + CIFClass;                        
        data_row[data_row.length]   = "StartAtLine*&^#"                 + StartAtLine;        
        data_row[data_row.length]   = "SplitChar*&^#"                   + SplitChar;
        data_row[data_row.length]   = "SplitIndex*&^#"                  + SplitIndex;
        data_row[data_row.length]   = "InvalidCharacters*&^#"           + InvalidCharacters;
        data_row[data_row.length]   = "Condition1*&^#"                  + Condition1;
        data_row[data_row.length]   = "ConditionPosition1*&^#"          + ConditionPosition1;
        data_row[data_row.length]   = "Operator1*&^#"                   + Operator1;
        data_row[data_row.length]   = "Condition2*&^#"                  + Condition2;
        data_row[data_row.length]   = "ConditionPosition2*&^#"          + ConditionPosition2;
        data_row[data_row.length]   = "Operator2*&^#"                   + Operator2;
        data_row[data_row.length]   = "Condition3*&^#"                  + Condition3;
        data_row[data_row.length]   = "ConditionPosition3*&^#"          + ConditionPosition3;  
        data_row[data_row.length]   = "DateCreated*&^#"                 + DateCreated;
        data_row[data_row.length]   = "DateLastModified*&^#"            + DateLastModified;
        
        return data_row.join("^%$");
    }
   
    this.load_file_loader_grid_viewer = function()
    {
        this.commons = new common_method();
        this.commons.to_read_only("tblFileLoaderSetup",true);  
        this.commons.to_read_only("tblFileUploadType",true);
        this.commons.to_read_only("tblCIFClass", true);
        this.commons.to_read_only("tblDestinationTable", true);
        this.commons.to_read_only("tblDestinationTableField", true);
        this.commons.to_read_only("tblRecordPosition", true);
        this.commons.to_read_only("tblConditions", true);
        this.commons.to_unlock_button("tblbuttons","btnAdd~");
        //this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit");
        this.commons = null;
        
        this.loadergrid = new grid_viewer();
        this.loadergrid.sql_source  = "sp_fileloader~";
        this.loadergrid.div_container = document.getElementById("DefinitionMapperGrid");
        this.loadergrid.datarow_column = 0;
        this.loadergrid.open_grid_view();
        this.grid = null;
    }

    this.do_ajaxgridviewer = function (parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                document.getElementById("txtHiddenSeqID").value = command_param[1];

                if (command_param[0] == "ajx_selected")
                {
                    this.SEQ_ID = command_param[1];
                    this.load_record();
                    document.getElementById("btnEdit").disabled = ""; 
                    return;
                }
                this.process_grid_viewer(parameter);
            }
        }
    }
   
    this.process_grid_viewer = function (parameter)
    {
        this.loadergrid = new grid_viewer();        
        this.loadergrid.sql_source     = "sp_fileloader~";
        this.loadergrid.div_container  = document.getElementById("DefinitionMapperGrid");
        this.loadergrid.datarow_column = 0;
        this.loadergrid.process_ajax(parameter);
        this.loadergrid = null;
    }
   
   //called by page
    this.show_no_of_controls = function ()
    {
        if (document.getElementById("ddNoofFilesTempTable").value == 2)
        {
            this.hide_elements(2);
        } 
        else if (document.getElementById("ddNoofFilesTempTable").value == 3)
        {
            this.hide_elements(3);
        } 
        else
        {
            document.getElementById("fileone").style.display   = "none";
            document.getElementById("filetwo").style.display   = "none";
            document.getElementById("filethree").style.display   = "none";
            document.getElementById("multiplefiles").style.display   = "block";
        }
    }
    this.hide_elements = function(elements)
    {
        var div = new Array("fileone","filetwo","filethree"); // condensed array
        var y = 0;

        document.getElementById("multiplefiles").style.display  = "block";

        for(y = 0; y < div.length; y++)
        {
            document.getElementById(div[y]).style.display       = "none";
        }

        for(y = 0; y < elements; y++)
        {
            document.getElementById(div[y]).style.display       = "block";
        }
    }
   
   //called by page
    this.hide_elements_fields = function(elements)
    {
        var div = new Array("Div1","Div2","Div3","Div4","Div5","Div6","Div7","Div8","Div9","Div10","Div11","Div12","Div13","Div14","Div15","Div16","Div17","Div18","Div19","Div20","Div21","Div22","Div23","Div24","Div25","Div26","Div27","Div28","Div29","Div30","Div31","Div32","Div33","Div34","Div35","Div36","Div37","Div38","Div39","Div40"); // condensed array
        var y = 0;

        for(y = 0; y < div.length; y++)
        {
            document.getElementById(div[y]).style.display       = "none";
        }
        
        document.getElementById("multiplefields").style.display = "block";
        for(y = 0; y < elements; y++)
        {
            document.getElementById(div[y]).style.display       = "block";
        }
    }
    
    this.show_temp_file_names = function(FileUploadType)
    {
        var div = new Array("fileone", "filetwo", "filethree")
        var intTotalFile;
        var strTempFileNames
        
        strQueryCheckTempTable = "SELECT fldcode " +
                                 "FROM tbl_lookup " +
                                 "WHERE fldgroup = 'irscompliance' " +
                                 "AND fldvalue IN (SELECTchildfileformat " +
                                                  "FROM tbl_list_temp_tables " +
                                                  "WHERE parentfileformat = " + FileFormat + ")" +
                                                  "AND fileuploadtype =" + FileUploadType + "}";
                                            
        strTempFileNames = IRSWorld.IRSFileLoader.FileLoaderSetup.getFileNames(FileFormat, FileUploadType); 
        intTotalFile = strTempFileNames.value.Tables[0].Rows.length;
        
        this.commons = new common_method();        
        for (var i = 0; i < 3; i++)
        {
            this.commons.to_read_only("tblFileName" + String(i + 1),true);
        }
        this.commons.to_read_only("tbmultiplefiles",true);
        this.commons = null;
        
        for (y = 0; y < div.length; y++)
        {
            document.getElementById(div[y]).style.display   = "none";
        }
        
        for (y = 0; y < intTotalFile; y++)
        {
            document.getElementById(div[y]).style.display       = "block";
            document.getElementById("txtFileName" + Number(Number(y) + 1)).value  = strTempFileNames.value.Tables[0].Rows[y].fldcode;
        }
    }
    
    this.show_temp_fields = function (temptablename)
    {
        var div = new Array("Div1","Div2","Div3","Div4","Div5","Div6","Div7","Div8","Div9","Div10","Div11","Div12","Div13","Div14","Div15","Div16","Div17","Div18","Div19","Div20","Div21","Div22","Div23","Div24","Div25","Div26","Div27","Div28","Div29","Div30","Div31","Div32","Div33","Div34","Div35","Div36","Div37","Div38","Div39","Div40"); // condensed array
        var y = 0;
        var strTempFields;
        
        for(y = 0; y < div.length; y++)
        {
            document.getElementById(div[y]).style.display       = "none";
        }              
        
        strQueryCheckTempTable = "SELECT column_name, data_type, character_maximum_length " + 
                                 "FROM information_schema.columns " +
                                 "WHERE table_name = '" + temptablename + "'";

        strTempFields = IRSWorld.IRSFileLoader.FileLoaderSetup.getTempTableSchema(temptablename);

        document.getElementById("multiplefields").style.display = "block";
        intTotalTempFields = strTempFields.value.Tables[0].Rows.length;
        
        arrayFieldNames[0] = new Array(intTotalTempFields);        
        
        this.commons = new common_method();
        for(y = 0; y < intTotalTempFields; y++)
        {
            this.commons.to_read_only("tbl" + String(y + 1),true);
            document.getElementById(div[y]).style.display       = "block";
            document.getElementById("txtFieldName" + Number(Number(y) + 1)).value  = strTempFields.value.Tables[0].Rows[y].column_name;
            arrayFieldNames[0][y] = strTempFields.value.Tables[0].Rows[y].column_name;
            
            if (strTempFields.value.Tables[0].Rows[y].data_type == "numeric")
            {
                document.getElementById("ddDataType" + Number(Number(y) + 1)).selectedIndex = 1;
                document.getElementById("txtDataLength" + Number(Number(y) + 1)).value = "system default"
            }            
            else if (strTempFields.value.Tables[0].Rows[y].data_type == "varchar")
            {
                document.getElementById("ddDataType" + Number(Number(y) + 1)).selectedIndex = 2;
                document.getElementById("txtDataLength" + Number(Number(y) + 1)).value = strTempFields.value.Tables[0].Rows[y].character_maximum_length;
            }            
            else if (strTempFields.value.Tables[0].Rows[y].data_type == "money")
            {
                document.getElementById("ddDataType" + Number(Number(y) + 1)).selectedIndex = 3;
                document.getElementById("txtDataLength" + Number(Number(y) + 1)).value = "system default"
            }
            else if (strTempFields.value.Tables[0].Rows[y].data_type == "datetime")
            {
                document.getElementById("ddDataType" + Number(Number(y) + 1)).selectedIndex = 4;
                document.getElementById("txtDataLength" + Number(Number(y) + 1)).value = "system default"
            }            
        }
        document.getElementById("ddNoofFields").selectedIndex = y;
        this.commons.to_unlock_button("tbltemptablebuttons","btnEditTempTable~");
        this.commons = null;   
    }
    
    this.temp_table_values = function()
    {
        for (a = 0; a< 3; a++)
        {
            document.getElementById("txtFileName" + Number(Number(a) + 1)).value = "";        
        }
        
        for (a = 0; a< 40; a++)
        {
            document.getElementById("txtFieldName" + Number(Number(a) + 1)).value = "";        
            document.getElementById("ddDataType" + Number(Number(a) + 1)).selectedIndex = 0;
            document.getElementById("txtDataLength" + Number(Number(a) + 1)).value = "";        
        }
    }
    
    this.show_div = function(div)
    {
        this.clear_filesetup();
        
        this.commons = new common_method();        
        this.commons.to_read_only("tblDestinationTableField", true);
        this.commons.to_read_only("tblDestinationTable", true);
        this.commons.to_read_only("tblFileLoaderSetup",true);
        this.commons.to_read_only("tblFileUploadType",true);
        this.commons.to_read_only("tblRecordPosition", true);
        this.commons.to_read_only("tblTempTableField", true);
        this.commons.to_read_only("tblCIFClass", true);
        //this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit");
        this.commons.to_unlock_button("tblbuttons","btnAdd~");
        this.commons = null;                
        
        document.getElementById("divConditions").style.display              = "none"; 
        document.getElementById("divTempTableField").style.display          = "none";
        document.getElementById("divDestinationTableField").style.display   = "block";
        
        if (div == "divFileLoaderSetup")
        {
            document.getElementById("divFileLoaderSetup").style.display     = "block";
            document.getElementById("divCreateTempTable").style.display     = "none";        
            document.getElementById("DefinitionMapperGrid").style.display   = "block";
        }
        else
        {        
            document.getElementById("divFileLoaderSetup").style.display     = "none";
            document.getElementById("divCreateTempTable").style.display     = "block";            
            document.getElementById("DefinitionMapperGrid").style.display   = "none";
        }
    }
    
    this.check_alpha = function()
    {
        if (event.keyCode == 32 || (event.keyCode >= 65 && event.keyCode <= 90) || (event.keyCode >= 48 && event.keyCode <= 57) ||  (event.keyCode >= 97 && event.keyCode <= 122)) 
        {   
            event.returnValue = true;
        }
        else
        {
            event.returnValue = false;
            alert("Please input valid characters");
        }
    } 
}