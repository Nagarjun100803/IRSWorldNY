
function product_setup()
{
    this.commons                 = null;
    this.generic                 = null;
    this.SEQ_ID                  = null;
    this.message                 = null;
    this.customer_info           = null;    
    this.ajax_grid               = null;
    this.grid                    = null;
    this.ProductCode             = null;
    this.ImplementationType      = null;
    this.FieldName               = null;
    this.ControlName             = null;
    this.ControlType             = null;
    this.FieldValue              = null;
    this.Fixed                   = null;
    this.FieldLength             = null;
    this.Source                  = null;
    this.Source_Seq_Id           = null;
    this.FieldValidation         = null;
    this.ProdImplem_Seq_Id       = null;
    this.SqlQuerying             = null;
    this.TableName               = null;
    this.ColumnName              = null;
    this.FieldValue              = null;
    this.ProdImplem_TableSource_Seq_Id   = null;
    this.Criteria_ColumnName     = null;
    this.Criteria_Value          = null;
    this.Operator                = null;
    this.Destination_Table       = null;
    this.Destination_Table_Column_Name   = null;
    
    
    this.set_product_setup = function()
    {
        this.commons = new common_method();
        this.ProductCode            = document.getElementById("ddProductCode").value;
        this.ImplementationType     = document.getElementById("ddImplemType").value;
       // this.FieldName              = document.getElementById("").value;
        this.ControlName            = document.getElementById("txtControlName").value;
        this.ControlType            = document.getElementById("ddControlType").value;
       // this.Fixed                  = document.getElementById("chkFixed").value;
        this.FieldLength            = document.getElementById("txtFixed").value;
        this.Source                 = document.getElementById("ddFieldSource").value;
        this.SqlQuerying            = document.getElementById("txtQuery").value;
        //this.TableName              = document.getElementById("ddTableName").value;
        //this.ColumnName             = document.getElementById("ddColumnName").value;
        //this.FieldValue             = document.getElementById("").value;
       // this.Criteria_ColumnName    = document.getElementById("cbo"+g).value;
        //this.Operator               = document.getElementById("cbooperational"+g).value;
       // this.Criteria_Value         = document.getElementById("txt"+g).value;
        
       // this.Fixed = false;
          
       // if (document.getElementById("chkFixed").value != null && document.getElementById("chkFixed").checked == true)
       // {
       //     this.Fixed = true;
       //     this.FieldValue             = document.getElementById("txtControlValue").value;
       // }
        
        this.commons = null;
        return this.push_data_to_server();
     }     
    
    
    this.push_data_to_server = function()
    {
        var data_row = new Array();
                               
        data_row[data_row.length]  = "ProductCode~"         + this.ProductCode;
        data_row[data_row.length]  = "ImplementationType~"  + this.ImplementationType;
        data_row[data_row.length]  = "ControlName~"         + this.ControlName;
        data_row[data_row.length]  = "ControlType~"         + this.ControlType;        
        data_row[data_row.length]  = "Source~"              + this.Source;
        var s = data_row.join("|") + "|TableName~ "          +' tbl_prodimplem_setup '+"" + "^" ;
        data_row = new Array();
        data_row[data_row.length]  = "SqlQueryingString~"   + this.SqlQuerying;
        s +=  data_row.join("|") + "|TableName~ "            +' tbl_prodimplem_setup_querysource '+"" ;
        
        
         IRSWorld.Admin.insert(s);
        
    }              
 
    
    this.load_all_dropdownlist = function()
    {
        this.dropdown_product_code();
        this.dropdown_implementation_type();
        this.dropdown_control_name();
        this.dropdown_field_source();
        this.dropdown_get_tables();
        this.show_result(false);
    }
    
    this.dropdown_get_tables = function()
    {
       IRSWorld.Admin.ProductSetup.get_tables(this.dropdown_get_tables_callback)
    }
    
    this.dropdown_get_tables_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddTableName", result.value, "table_name", "table_name");
            this.commons = null;
        }
    }
    
    this.dropdown_get_columns = function()
    {
       IRSWorld.Admin.ProductSetup.get_columns(document.getElementById("ddTableName").value, this.dropdown_get_columns_callback)
    }
    
    this.dropdown_get_columns_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddColumnName", result.value, "column_name", "column_name");
            this.commons = null;
        }
    }
    this.dropdown_get_columncriteria = function()
    {
       IRSWorld.Admin.ProductSetup.get_columns(document.getElementById("ddTableName").value, this.dropdown_get_columncriteria_callback)
    }
    
    this.dropdown_get_columncriteria_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddColumnNameCriteria1", result.value, "column_name", "column_name");
            this.commons = null;
        }
    }
    
    this.dropdown_product_code = function()
    {
       IRSWorld.Admin.ProductSetup.getdropdown_source("ProductCode", this.dropdown_product_code_callback)
    }
    
    this.dropdown_product_code_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddProductCode", result.value, "FldValue", "FldDescription");
            this.commons = null;
        }
    }
    
    
    this.dropdown_implementation_type = function()
    {
       IRSWorld.Admin.ProductSetup.getdropdown_source("ImplementationType", this.dropdown_implementation_type_callback)
    }
    
    this.dropdown_implementation_type_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddImplemType", result.value, "FldValue", "FldDescription");
            this.commons = null;
        }
    }
    
    this.dropdown_control_name = function()
    {
       IRSWorld.Admin.ProductSetup.getdropdown_source("ControlName", this.dropdown_control_name_callback)
    }
    
    this.dropdown_control_name_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddControlType", result.value, "FldValue", "FldDescription");
            this.commons = null;
        }
    }
    
    this.dropdown_field_source = function()
    {
       IRSWorld.Admin.ProductSetup.getdropdown_source("FieldSource", this.dropdown_field_source_callback)
    }
    
    
    this.dropdown_field_source_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddFieldSource", result.value, "FldValue", "FldDescription");
            this.commons = null;
        }
    }
    
    this.enable_control_value = function()
    {
        if(document.getElementById("ddControlType").value !='3'&& document.getElementById("ddControlType").value !='null')
        {
           document.getElementById("divcontrolvalue").style.display = "block";
        }
        else if(document.getElementById("ddControlType").value =='null' && document.getElementById("ddControlType").value !='3')
        {
            document.getElementById("divcontrolvalue").style.display = "none";
        }
        else
        {
            document.getElementById("divcontrolvalue").style.display = "none"; 
        }
        
    }
    
    this.fixed = function()
    {
        if(document.getElementById("chkFixed").checked == true)
        {
            document.getElementById("txtFixed").readOnly = false;
        }
        if(document.getElementById("chkFixed").checked == false)
        {
            document.getElementById("txtFixed").value = "";
            document.getElementById("txtFixed").readOnly = true;
        }
    }
    
    this.alert = function()
    {
//        alert(document.getElementById("ddColumnName").value);
        //alert(document.getElementById("ddNoOfCriteria").innerText);
       // alert(mi);
    }
    
    this.field_source_option = function()
    {
        if (document.getElementById("ddFieldSource").value =='1')
        {
            this.query_option();
        }
        if (document.getElementById("ddFieldSource").value =='2')
        {
            this.table_option();
        }
    }
    
    this.table_option = function()
    {
        product_setup.show_result(false);
        document.getElementById("divtable").style.display = "block";
        document.getElementById("divquery").style.display = "none"; 
        document.getElementById("txtQuery").value ="";
        document.getElementById("divbuttons").style.display = "block"; 
    }
    
    this.query_option = function()
    {
        product_setup.show_result(false);
        document.getElementById("divquery").style.display = "block"; 
        document.getElementById("divtable").style.display = "none";
        document.getElementById("divbuttons").style.display = "block"; 
    }
    
    this.create_criteria = function()
    {
       var div = document.getElementById("divcriteria1");
       
       var txtctr = document.getElementById("txtNoOfCriteria").value;
       
       if (txtctr > 20)
       {
            alert("Unable to create options more than 20");
            return false;
       }

       var s; 
        s = "<table style='width:100%'> ";
        for (i=0 ; i< txtctr; ++i)
        {
            s = s + "<tr> ";
            s = s + "<td style='width:20%'> ";
            s = s + "<select id= '" + 'cbo' + i + "' ></select> ";
            s = s + "</td> ";
            s = s + "<td style='width:20%'> ";
            s = s + "<select id= '" + 'cbooperational' + i + "' ></select> ";
            s = s + "</td> ";	
            s = s + "<td style='width:20%'> ";
            s = s + "<input type='text' id= '" + 'txt' + i + "' > ";
            s = s + "</td> ";
            if((txtctr - 1) == i)
            {
                s = s + "<td style='width:20%'> </td>";
           
            }
            else
            {
                s = s + "<td style='width:20%'> ";
                s = s + "<select id= '" + 'cboandor' + i + "' ></select> ";
                s = s + "</td> ";
            }	
            s = s + "</tr> ";
            
            
        }
        s= s + " </table>"
        div.innerHTML = s;
        
        for (x=0; x< txtctr; x++)
        {
            var result1 = IRSWorld.Admin.ProductSetup.get_columns(document.getElementById("ddTableName").value);
            this.commons = new common_method();    
	        this.commons.to_load_dropdownlist(""+'cbo'+ x +"",result1.value, "column_name", "column_name");
	        this.commons =null;
	        
	        var result6 =  IRSWorld.Admin.ProductSetup.getdropdown_source2("Comparison Operators")
	        this.commons = new common_method();    
	        this.commons.to_load_dropdownlist(""+'cbooperational'+ x +"",result6.value, "FldCode", "FldCode");
	        this.commons =null;
	        
	        if((txtctr - 1) == x)
	        {
	        
	        }
	        else
	        {
	            var result2 = IRSWorld.Admin.ProductSetup.getdropdown_source2("Logical Operators");
	            this.commons = new common_method();    
	            this.commons.to_load_dropdownlist(""+'cboandor'+ x +"",result2.value, "FldCode", "FldCode");
	            this.commons =null;
            }
	    }
    }
    
    this.create_destination = function()
    {
       var div = document.getElementById("divDestination1");
       
       var txtctr1 = document.getElementById("txtNoOfDestination").value;
       
       if (txtctr1 > 20)
       {
            alert("Unable to create options more than 20");
            return false;
       }

      var g; 
        g = "<table style='width:100%'> ";
        for (k=0 ; k< txtctr1; k++)
        {
            g = g + "<tr> ";
            g = g + "<td style='width:50%'> ";
            g = g + "<select id= '" + 'ddtname' + k + "' onchange='javascript:product_setup.populate_column_destination();' ></select> ";
            g = g + "</td> ";
            g = g + "<td style='width:50%'> ";
            g = g + "<select id= '" + 'ddcname' + k + "' ></select> ";
            g = g + "</td> ";	
            g = g + "</tr> ";
        }
        g= g + " </table>"
        div.innerHTML = g;
        
        for (j=0; j< txtctr1; j++)
        {
            var result3 =IRSWorld.Admin.ProductSetup.get_tables();
            this.commons = new common_method();    
	        this.commons.to_load_dropdownlist(""+'ddtname'+ j +"",result3.value, "table_name", "table_name");
	        this.commons =null;
	    }
    }
    
    this.populate_column_destination = function()
    {
        var txtctr3 = document.getElementById("txtNoOfDestination").value;
        
        for (y=0; y< txtctr3; y++)
        {
            var result4 =  IRSWorld.Admin.ProductSetup.get_columns(document.getElementById(""+'ddtname'+ y +"").value);
	        this.commons = new common_method();    
	        this.commons.to_load_dropdownlist(""+'ddcname'+ y +"",result4.value, "column_name", "column_name");
	        this.commons =null;
        }
    }
    this.check_query = function()
    {
        var msg = null;
        var sql = document.getElementById("txtQuery").value;
       
        sql = sql.toLowerCase();
        
        if(sql.indexOf("update")>-1)
        {
            alert("Invalid SQL Statement");
            return false;
        }
        if(sql.indexOf("delete")>-1)
        {
            alert("Invalid SQL Statement");
            return false;
        }
        if(sql.indexOf("drop")>-1)
        {
            alert("Invalid SQL Statement");
            return false;
        }
        if(sql.indexOf("truncate")>-1)
        {
            alert("Invalid SQL Statement");
            return false;
        }
        
        var queryresult = IRSWorld.Admin.ProductSetup.parse_sql(sql);
        msg = IRSWorld.Admin.ProductSetup.read_message(); 
        
        if (msg.value !="")
        {
            alert(msg.value); return;
        }
       
        //this.load_grid_viewer(sql);
    }
     
    this.check_table_query =function()
    {
        var columnname = document.getElementById("ddColumnName").value;
        var tablename  = document.getElementById("ddTableName").value;
        var nocriteria = document.getElementById("txtNoOfCriteria").value
        if (columnname !='null' && tablename !='null')
        {
            var query = "select "+ columnname +" from "+ tablename  +" ";
            
            if (nocriteria !="" && nocriteria >"0" )
            {
                query +=" where ";
                
                for (g=0 ; g< nocriteria; g++)
                {
                    query += "  "+ document.getElementById("cbo"+g).value +" ";
                    query += " "+ document.getElementById("cbooperational"+g).value +" ";
                    query += " '"+ document.getElementById("txt"+g).value +"' ";
                    
                    if((nocriteria - 1) == g)
                    {
                        
                    }
                    else
                    {
                        query += " "+document.getElementById("cboandor"+g).value  +" ";
                    }
               } 
            }
            
            var finalquery = query;
            
            
               finalquery = finalquery.toLowerCase();
               IRSWorld.Admin.ProductSetup.parse_sql(finalquery, this.get_result);
        }
        else 
        {
            alert("Please Complete Necessary Fields (Table Name / Column Name");
        }
    }
    
     this.show_result = function(on)
     {
            var s = (on) ? 'block' : 'none';
            document.getElementById('divproductgrid').style.display = s;
            if (!on)
            {
                document.getElementById('divproductgrid').innerHTML = '';
            }
     }
     
    this.exec_stmt = function()
    {
            var sql = document.getElementById('txtQuery').value;
            if (sql.length < 1) 
            {
                alert('Nothing to execute');
                //enable_clear(false);
                return;
            }
                IRSWorld.Admin.ProductSetup.parse_sql(sql, this.get_result);
            
    }
    
    this.get_result =function(rs)
    {
            
            
            if (rs.error != null)
            {
                alert(rs.error.Message);
                return;
            }
            
            var tabs = rs.value;
            if (tabs.length < 1)
            {
                alert('No result set');
                return;
            }
            document.getElementById('divproductgrid').innerHTML = '';
            var div_result = document.getElementById('divproductgrid');
            
            for (var i = 0; i < tabs.length; i++)
            {
                var htm =  IRSWorld.Admin.ProductSetup.getResult(tabs[i]);
                var s = div_result.innerHTML;
                s = s + htm.value + '</br>';
                div_result.innerHTML = s;
            }           
             product_setup.show_result(true);
            
    }
    
}