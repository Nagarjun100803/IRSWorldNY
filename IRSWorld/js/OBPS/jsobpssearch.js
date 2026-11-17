function execute_ajx_command(param)
{
    if (obps_transearch != null && obps_transearch != "undefined")
    {
        obps_transearch.do_gridsearch(param);
    }
}

function execute_ajaxgridview_command(param)
{
    if (obps_transearch != null && obps_transearch != "undefined")
    {
        obps_transearch.do_gridview(param);
    }
}
function obps_transearch()
{
    this.grid_ajax_page      = "../AjxPage/ajaxgrid.aspx";
    this.grid_ajax_query     = null;    
    this.ajax_grid           = null;
    this.ajax_container      = null;
    this.commons             = null;
    this.Internet_Sidebar_class = null;  
    
    this.MerchantCode        = null;
    this.MerProdCode         = null;

    this.merchant_name       = null;
    this.currency            = null;
    this.MerchantProdCode    = null;
    this.new_value           = null;
    this.less_value          = null;
    
    this.load = function()
    {
        document.getElementById("txtSearchLastname").value = "";
        document.getElementById("txtSearchFirstname").value = "";
        document.getElementById("txtSearchMiddlename").value = "";
        document.getElementById("ddlIdType").selectedIndex = "";
        document.getElementById("txtSearchCardNo").value = "";       
        this.open_division("div_search");
        this.dropdown_idtype();
    }
    
    
    this.dropdown_idtype = function()
    {
        ObpsTranSearch.getdropdown_idtype("IDType", this.dropdown_idtype_callback)
    }
    
    this.dropdown_idtype_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlIdType", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }
  
    this.close_divisions = function()
    {
        document.getElementById("div_search").style.display = 'none';
        document.getElementById("div_improd_change").style.display = 'none';
        document.getElementById("div_results").style.display = 'none';
        document.getElementById("div_merchants").style.display = 'none';
        document.getElementById("div_charges").style.display = 'none';
    }
    
    this.open_division = function(division_id)
    {
        this.close_divisions(); document.getElementById(division_id).style.display = 'block';       
    }
    
    this.search_customers_info = function()
    {
//           document.getElementById("ddlIdType").style.display = 'none'; 
           if(document.getElementById("txtSearchLastname").value == "" &&
            document.getElementById("txtSearchFirstname").value == "" &&
            document.getElementById("txtSearchMiddlename").value == "" &&
            document.getElementById("ddlIdType").selectedIndex == 0 &&
            document.getElementById("txtSearchCardNo").value == "")
            {
                alert("Invalid Search Value");
            }
            else
            {
                        
                var cif_class = 1;
                var last_name          = document.getElementById("txtSearchLastname").value;
                var first_name         = document.getElementById("txtSearchFirstname").value;
                var middle_name        = document.getElementById("txtSearchMiddlename").value;
                var id_type            = document.getElementById("ddlIdType").selectedIndex;
                var card_no            = document.getElementById("txtSearchCardNo").value;
                this.ajax_grid = new grid_selector();
                
               this.grid_ajax_query = "sp_obps_fullsearch~" + cif_class + "~" + last_name + "~" + first_name + "~" + middle_name + "~" + id_type + "~" + card_no + "~";
               this.ajax_grid.open_selection_grid(this.grid_ajax_page, this.grid_ajax_query, 0, this.ajax_container ); 
            }
    }
    
    this.do_gridsearch = function(parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {
                    var command_values = command_param[1].split("~");

                    this.remitter_id      = command_values[0];
                    this.remitter_name    = command_values[1];
                    this.remitter_addr    = command_values[2];

                    this.load_remitter_and_merchant_information();
                    this.load_remitter_transactions();                    
//                    this.load_rem_product_implementation();
                    this.load_merchants_prod_imp();
                    this.open_division("div_results");
                }                
                this.ajax_grid.process_ajax(parameter);
            }
        }
    }
    
    this.load_remitter_and_merchant_information = function()
    {
        document.getElementById("lblRemitterName").innerText    = '';
        document.getElementById("lblRemitterAddress").innerText = '';
        document.getElementById("lblRemitterName").innerText    = this.remitter_name;
        document.getElementById("lblRemitterAddress").innerText = this.remitter_addr;

    }
    
    this.load_remitter_transactions = function()
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_obps_search_remitters_trans~" + this.remitter_id;
        this.grid.div_container  = document.getElementById("DIV_REM_TXN");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.load_merchants_prod_imp = function()
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_obps_merchsearch~" + this.remitter_id;
        this.grid.div_container  = document.getElementById("DIV_BENE_PROD_IMP");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;        
    }
    
    this.do_gridview = function(parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");
                                    
            if (command_param.length > 0)
            {                                                
                if (command_param[0] == "ajx_selected")
                {
                            var command_value = command_param[1].split('~');

                            this.remitter_id        = command_value[0];
                            this.merchant_name      = command_value[1];
                            this.MerchantCode       = command_value[2];
                            this.MerProdCode        = command_value[3];
                            this.currency           = command_value[4];
                            this.MerchantProdCode   = command_value[5];
                }                                
                    this.populate_merchants();
                    this.open_division("div_merchants");
            }
        }
    }
    
    this.populate_merchants = function()
    {
                document.getElementById("lbl_merchant_name").innerText = this.merchant_name;
                document.getElementById("lbl_merchant_prodcode").innerText = this.MerchantProdCode;

                document.getElementById("txtMemberID").style.display = "block";
                document.getElementById("lbl_merchant_currency").innerText = this.currency;
                
                if(this.MerchantProdCode == "CONT")
                {
                        document.getElementById("txt_target_amount").style.display = "none";
                        document.getElementById("txt_amount").style.display = "none";                          
                        document.getElementById("lbl_target_amount").style.display = "block";
                        document.getElementById("lbl_amount").style.display = "block";
                }
                else
                {
                        document.getElementById("lbl_target_amount").style.display = "none";
                        document.getElementById("lbl_amount").style.display = "none";                          
                        document.getElementById("txt_target_amount").style.display = "block";
                        document.getElementById("txt_amount").style.display = "block";
                }
                this.insertTableRow();
                
                     
    }
    
    this.insertTableRow = function()
    {
        var theTable = document.getElementById("tblBillsPayOBPSTransactionsMDetails");
        var theTableBody = theTable.tBodies[0];
        var sp = 'spGetOptionalField';
        var dt = ObpsTranSearch.getOBPData(sp,this.MerchantCode,this.MerProdCode).value;
        
        if (dt != null && dt.Rows.length != 0)
        {
            var rowcnt = dt.Rows;
            var tfld = 0;
            var dfld = 0;
            var rfld = 0;
            var nfld = 0;
            var amfld = 0;
            
            for (var cnt = 0; cnt < dt.Rows.length; cnt++)
            {                
                var rowData = [dt.Rows[cnt].FieldLabel.toString(), dt.Rows[cnt].FieldName.toString()];
                var newCell;
                var newRow = theTableBody.insertRow(cnt + 1);
                if (dt.Rows[cnt].FieldName.toString() == "id1")
                {
                    document.getElementById("lbl_MID").innerText = dt.Rows[cnt].FieldLabel.toString();
                }
                else
                {
                for (var i = 0; i < rowData.length; i++) {
                    newCell = newRow.insertCell(i);                 

                      if (i == 0)
                      {
                          newCell.innerHTML = rowData[i];
                          newCell.id = cnt;
                          newCell.style.backgroundColor = "lavender"
                          newCell.style.fontSize = "12px"
                          newCell.style.fontFamily = "Arial"
                          newCell.style.fontWeight = "Bold"
                          newCell.style.color = "MidnightBlue"
                          newCell.style.borderBottom = "solid 1px midnightblue"
                          newCell.style.textAlign = "right"
                            //newCell.style.width = "31%"
                          newCell.style.width = "26%"
                          newCell.style.height = "24px"
                      }
                      else
                      {
                          if(dt.Rows[cnt].FieldName.toString().substring(0,2) == "tf")
                          {
                              tfld = tfld + 1;
                              if(dt.Rows[cnt].ValidationType.toString() == "LI")
                              {
                                  var el = document.createElement('select')
                                  el.name = 'MerchantInfo'; //'txt' + tfld;
                                  el.id = 'txt' + tfld;
                                  newCell.appendChild(el);
                                  this.LIFldRef = dt.Rows[cnt].LIFldREf.toString();
                                  this.popDDL(el, this.LIFldRef);
                              }
                              else
                              {
                                  var el = document.createElement('input')
                                  el.type = 'text';
                                  el.name = 'MerchantInfo'; //'txt' + tfld;
                                  el.id = 'txt' + tfld;
                                  newCell.appendChild(el);
                              }
                          }
                          else if (dt.Rows[cnt].FieldName.toString().substring(0,2) == "df")
                          {
                                //book1
                              dfld = dfld + 1;
                              var el = document.createElement('input')
                              el.type = 'text';
                              el.name = 'MerchantInfo'; //'dt' + dfld;
                              el.id = 'dt' + dfld;
                              //temporary
                              el.width = '100px'
                              el.readOnly = true;
                              newCell.appendChild(el);
                              var el2 = document.createElement('img')
                              el2.name = 'MerchantInfo'; //'dtim' + dfld;
                              el2.id = 'dtim' + dfld;
                              el2.src = '../images/calendar/imgcalen.png';
                              el2.alt = 'Pick a date.'
                              
                              newCell.appendChild(el2);
                              //var popc = 'popUpCalendar(this,document.forms[0].' + el.id + ',"mm/dd/yyyy")';
                              var popc = 'popUpCalendar(this,document.forms[0].' + el.id + ',"mm/dd/yyyy")';
                              document.getElementById('dtim' + dfld).onclick = Function(popc);//{popUpCalendar(document.getElementById('dtim' + cnt),document.getElementById('dt' + cnt),'mm/dd/yyyy')};
                          }
                          else if (dt.Rows[cnt].FieldName.toString().substring(0,2) == "rf")
                          {
                              rfld = rfld + 1;
                              var el = document.createElement('textarea')
//                              el.type = 'text';
                              el.name = 'MerchantInfo'; //'mtxt' + rfld;
                              //el.id = mtxt' + rfld;
                              el.rows = '5';
                              el.cols = '45';
                              
                              newCell.appendChild(el);
                          }
                          else if (dt.Rows[cnt].FieldName.toString().substring(0,2) == "nf")
                          {
                              nfld = nfld + 1;
                              if(dt.Rows[cnt].ValidationType.toString() == "LI")
                              {
                                  var el = document.createElement('select')
                                  el.name = 'MerchantInfo'; //'ntxt' + nfld;
                                  el.id = 'txt' + nfld;
                                  newCell.appendChild(el);
                                  this.LIFldRef = dt.Rows[cnt].ControlName.toString();
                                  this.popDDL(el, this.LIFldRef); 
                                  
                                  //document.getElementById('txt' + 1).onchange = ObpsTranSearch.get_contribution(document.getElementById('txt' + 1).options[document.getElementById('txt' + 1).selectedIndex].value, this.get_contribution);
                           //     document.getElementById('txt' + 1).onchange = ObpsTranSearch.get_contribution(document.getElementById('txt' + 1).options[document.getElementById('txt' + 1).selectedIndex].value, this.get_contribution)
                                  if(nfld == 1)
                                  {
                                    var onchange = 'get_data_value("tblBillsPayOBPSTransactionsMDetails", false)';
                                    document.getElementById('txt' + nfld).onchange = Function(onchange);

                                  }
                                  else
                                  {
                                    var multiply_value = 'multiply_data_value()';
                                    document.getElementById('txt' + nfld).onchange = Function(multiply_value);
                                  }
                              
                              }
                              else
                              {
                                  var el = document.createElement('input')
                                  el.type = 'text';
                                  el.name = 'MerchantInfo'; //'ntxt' + nfld;
                                  el.id = 'txt' + nfld;
                                  newCell.appendChild(el);
                                  var get_value_txt = 'add_data_value()';
                                  document.getElementById('txt' + nfld).onchange = Function(get_value_txt);
                              }
                          }
                          else if (dt.Rows[cnt].FieldName.toString().substring(0,2) == "am")
                          {
                              amfld = amfld + 1;
                          }
                          newCell.colSpan = "2"
                          newCell.style.height = "24px"
                          newCell.style.textAlign = "left"
                      }
                 }
                 }
                 document.getElementById("tblBillsPayOBPSTransactionsMDetails").style.display = "block";
                 

             }
             
         }
    }
    
    
    this.popDDL = function(DDL, REFID)
    {   //this.trim(this.MerchantCode)
        var SQL =  this.getDDLSQL(DDL, REFID);
        var dt = ObpsTranSearch.getDataTable(SQL).value; 
        var dt2 = ObpsTranSearch.getDataTable(SQL).value; 
        this.cleanDDL(DDL);
        
        
        if (dt != null && dt.Rows.length > 0)
        {            
            for (var i = 0; i < dt.Rows.length; i++)
            {
                var opt = document.createElement("option");                 
                opt.text = dt.Rows[i].Description.toString();
                opt.value = dt.Rows[i].Code.toString();
                DDL.options.add(opt); 
            }      
        }
        else if(dt2 != null && dt2.Rows.length > 0)
        {
            for (var i = 0; i < dt2.Rows.length; i++)
            {
                var opt = document.createElement("option");                 
                opt.text = dt2.Rows[i].Description.toString();
                opt.value = dt2.Rows[i].Code.toString();
                DDL.options.add(opt); 
            }
        }
    }
    
    this.getDDLSQL = function(DDL, REFID)
    {
        var SQL;
        var DDLid = DDL.id.substring(0,DDL.id.length - 1);
        
        switch(DDLid)
        {
            case 'txt':
                if (this.MerchantCode == "SSS" & this.MerProdCode == 1)
                {
                //SQL = "select [Code] = isnull(code,''), [Description] = isnull(Description,'') from syscode where Fldref = '" + REFID + "' and Isdeleted = 0";
                SQL = "select fldvalue as Code,flddescription as Description from tbl_lookup where fldgroup = '"+ REFID +"'";
                }
                else
                {
                //SQL = "select [Code] = isnull(description,''), [Description] = isnull(code,'') from syscode where Fldref = '" + REFID + "' and Isdeleted = 0";
                SQL = "select fldvalue as Code,flddescription as Description from tbl_lookup where fldgroup = '"+ REFID +"'";
                }
                break; 
            case 'ddSourceFund':
                SQL = "select [Code] = isnull(fldcode,''), [Description] = isnull(fldDescription,'') from tbl_lookup where FldGroup = '" + REFID + "'"
                break; 
            case 'ddPayMod': 
                SQL = "select [Code] = isnull(fldcode,''), [Description] = isnull(fldDescription,'') from tbl_lookup where FldGroup = '" + REFID + "'"
                break; 
            case 'ddMProductCod':   
                SQL =  "select productcode AS Code,flddescription as Description from tbl_cif_merchants_info a";
                SQL += " left join (select fldvalue,flddescription from tbl_lookup where fldgroup='MerchantProdCode') b";
                SQL += " ON b.fldvalue=a.productcode";
                SQL += " where merchantcode='"+ this.MerchantCode +"'";
                break; 
            case 'ddMAccountCurrenc':   
                SQL = "SELECT DISTINCT [Code] = CURRENCY, [Description] = CASANO FROM CASA WHERE MERCHANTCODE = '" + REFID + "'"
                break;    
            case 'ddRemCurrenc':  
                SQL = "SELECT DISTINCT [Code] = isnull(L.FLDVALUE,''), [Description] = isnull(L.fldcode,'') FROM TBL_CURRENCY_CATEGORIES CC LEFT OUTER JOIN TBL_LOOKUP L ON CC.CURRENCY = L.FLDVALUE AND L.FLDGROUP = 'Currency' WHERE BRANCHCODE = '" + REFID + "'";
                break;   
            default:
                SQL = "select [Code] = isnull(description,''), [Description] = isnull(code,'') from syscode where Fldref = '" + REFID + "' and Isdeleted = 0"
                break;   
        }        
        
       return  SQL;        
    }   
    
    this.cleanDDL = function(DDL)
    {        
        DDL.length = 0;
    }

    this.get_contribution = function(result)
    {
        if (result != null && result != "undefined")
        {
                 if(result.value.Tables[0].Rows[0].FldValue < 22)
                 {  
                    this.less_value = result.value.Tables[0].Rows[0].FldDescription;
                    document.getElementById("lbl_target_amount").innerText = parseInt(this.less_value) * parseInt(document.getElementById('txt2').value);
                    document.getElementById("lbl_amount").innerText = this.less_value;
                 }
                 else
                 {
                    var over_value = result.value.Tables[0].Rows[0].FldDescription;
                    this.new_value = parseInt(over_value.slice(5));
                    document.getElementById("lbl_target_amount").innerText = parseInt(this.new_value) * parseInt(document.getElementById('txt2').value);
                 }
                 
        }
    }
        
}


    function get_data_value(tabid, read_only)
    {

        var tabid = document.getElementById(tabid);
        tblrows = tabid.rows;
        if (tblrows == undefined) return;
        
        for (var i = 0; i < tblrows.length; i++)
        {
            var sells = tblrows(i).cells;
            for (var j = 0; j < sells.length; j++)
            {
                if (sells.item(j).hasChildNodes())
                {
                    var childs = sells.item(j).childNodes;
                    for (var k=0; k < childs.length; k++)
                    {
//                        if (childs(k).nodeName == 'INPUT') //&& childs(k).type == 'text')
//                        {
//                            if (childs(k).type == 'text') alert(childs(k).value);
//                            else if (childs(k).type == 'checkbox' | childs(k).type == 'button' | childs(k).type == 'img' ) childs(k).disabled = read_only;
//                        }
                        if (childs(k).nodeName == 'SELECT')
                        {
                            if(i==2)
                            {
                                if(childs(k).value == 22)
                                {
                                    document.getElementById('txt3').style.display = "block";
                                    document.getElementById('6').style.display = "block";
                                    ObpsTranSearch.get_contribution(childs(k).value, obps_transearch.get_contribution)  
                                }
                                else
                                {
                                    document.getElementById('txt3').style.display = "none";
                                    document.getElementById('6').style.display = "none";
                                    ObpsTranSearch.get_contribution(childs(k).value, obps_transearch.get_contribution)
                                }
                            }
//                            alert(childs(k).value);
                            //childs(k).disabled = read_only;
                        }
//                        else if (childs(k).nodeName == 'TEXTAREA')
//                        {
//                            alert(childs(k).value);
//                            //childs(k).disabled = read_only;
//                        } 
//                        else if (childs(k).nodeName == 'TABLE')
//                        {
//                            set_read_only(childs(k), read_only); // go deeper!
//                        }               
                    }
                }
            }

        }
    }
    
    function add_data_value()
    {
        if(document.getElementById('txt3').value != null && document.getElementById('txt3').value != "")
        {
            document.getElementById("lbl_target_amount").innerText = (this.new_value * parseInt(document.getElementById('txt2').value)) + parseInt(document.getElementById('txt3').value);
        }
        else
        {
            document.getElementById("lbl_target_amount").innerText = this.new_value * parseInt(document.getElementById('txt2').value);
        }
    }
    
    function multiply_data_value()
    {
        if(document.getElementById('txt1').value == 1)
        {
            document.getElementById('txt3').style.display = "none";
            document.getElementById('6').style.display = "none";
            ObpsTranSearch.get_contribution(document.getElementById('txt1').value, obps_transearch.get_contribution)
        }
        else
        {

            if(document.getElementById('txt1').value > 21)  
            {
                if(document.getElementById('txt3').value != null && document.getElementById('txt3').value != "")
                {
                    document.getElementById("lbl_target_amount").innerText = (this.new_value * parseInt(document.getElementById('txt2').value) + parseInt(document.getElementById('txt3').value));
                }
                else
                {
                    document.getElementById("lbl_target_amount").innerText = (this.new_value * parseInt(document.getElementById('txt2').value));
                }
            }
            else
            {
            
                document.getElementById("lbl_target_amount").innerText = (this.less_value * parseInt(document.getElementById('txt2').value));
            }
        }
    }