
function frm_bills()
{
    this.hide_panel = function()
    {
        document.getElementById("tblMDetails").style.display = "none";
        document.getElementById("tblBPMerchants").style.display = "none";
        document.getElementById("divBPRemitter").style.overflow = "hidden";
        document.getElementById("tblSubMain").style.width = "45%";
        document.getElementById("trxncolumn").style.display = "none";
    }
}

function bills_rem_teller()
{               
    this.SEQ_ID               = null;
    this.UserID               = null;
    this.RemitMethod          = null;
    this.RemitStationID       = null;
    this.RemittingBranch      = null;
    this.ReferenceNo          = null;
    this.ExtlReferenceNo      = null;
    this.Transtatus           = null;
    this.TranxDate            = null;
    this.ProductChargesID     = null;
    this.RemitterCIFID        = null;
    this.BeneficiaryCIFID     = null;
    this.AccountSEQID         = null;
    this.RemittingCurrency    = null;
    this.GrossRemAmount       = null;
    this.NetRemAmount         = null;
    this.NettoBeneCurrency    = null;
    this.NettoBeneAmount      = null;
    this.TotalLocalCharge     = null;
    this.TransactionSource    = null;
    this.ReceiptMode          = null;
    this.TransactionPurpose   = null;
    this.OtherPurpose         = null;
    this.SpecialMessage       = null;
    this.DateImplemented      = null;
    this.DateSent             = null;
    this.printed              = null;
    this.ForexSourceToTarget  = null;
    this.ForexTargetToUSD     = null;
    this.ForexTargetToPHP     = null;
    this.ForexSourceToUSD     = null;
    this.ForexSourceToPHP     = null;
    this.ForexTargetToAccount = null;
    this.SourceofFunds        = null;

    this.RemitterFullName     = null;
    this.RemitterAddress      = null;
    this.RemitterPhoneNumber  = null;
    this.RemitterID           = null;

    this.MerchantName         = null;
    this.MerchantCode         = null;
    this.MemberID          = null;
    this.MerAccountCurr      = null;
    this.MerAccountNumber    = null;
    this.MerProdCode         = null;
    this.MerProdDesc         = null;
    this.MerchantCode2       = null;
    this.grid_ajax_page = "../AjxPage/ajaxgrid.aspx";   
    this.LookUpMode = null;
    this.LIFldRef = null;                         

    this.callLookUp = function(parameter)
    {   
        this.LookUpMode = parameter;        
        this.ajax_grid = new grid_selector();        
        this.grid_ajax_query = this.getLookUpSQL();
        this.ajax_grid.open_selection_grid(this.grid_ajax_page, this.grid_ajax_query, 3, this.SEQ_ID );                     
    } 

    this.getLookUpSQL = function()
    {   
        var SQL = null;
       
        if (this.LookUpMode == "REMITTER")
        {
            SQL = "SELECT M.LastName AS [LAST NAME], M.FirstName AS [FIRST NAME], M.MiddleName AS [MIDDLE NAME], M.CIFID AS [RECORD ID] FROM TBL_CIF_MASTER M";
            SQL = SQL + " WHERE CIFID IN (SELECT CIFID FROM TBL_CIF_CLASS WHERE CIFCLASS NOT IN (3,2,7))";
        }
        else if (this.LookUpMode == "MERCHANT")    
        {
//            SQL = "SELECT DISTINCT M.LastName AS [MERCHANT DESCRIPTION], M.FirstName AS [MERCHANT CODE], '' AS [ ], SEQ_ID AS [M.MEMBER ID] FROM TBL_CIF_MASTER M";
//            SQL = SQL +  " WHERE CIFID IN (SELECT CIFID FROM TBL_CIF_CLASS WHERE CIFCLASS = 3)";
            SQL = "usp_GetMerchantList~" + this.RemitterID
        }
        return SQL;
    } 

    this.do_ajax = function(parameter)
    {   
        this.ajax_grid.process_ajax(parameter);
        this.call_record_loader(parameter);
    }
    
   
    this.call_record_loader = function(parameter)
    {        
        var command_param = null;
        
        if (parameter != null && parameter != "undefined")
        {
            command_param = parameter.split("$");
            
            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {
                    this.SEQ_ID = command_param[1];                    
                    
                    if (this.LookUpMode == "REMITTER")
                    {
                        this.popRemDetail();
                        this.popMerDetail();
//                        this.popDDL(document.getElementById('ddlGateWay'),'');       
//                        this.popDDL(document.getElementById('ddlCardNo'),this.RemitterID); 
//                        this.popDDL(document.getElementById('ddlPayMode'),''); 
                    }
                    else if (this.LookUpMode == "MERCHANT")
                    {
                        this.popMerDetail();
                    }
                }
            }
        }
    }
         
    this.get_remitter_info = function()
    {
        var SQL = "SELECT TOP 1 M.SEQ_ID ";
            SQL += ",M.CIFID "; 
            SQL += ",Name = isnull(M.LastName,'') + ', ' + isnull(M.FirstName,'') + ' ' + isnull(M.MiddleName,'') ";
            SQL += ",Address = isnull(A.Address1,'') + ' ' + isnull(A.Address2,'') ";
            SQL += ",ContactNo = isnull(C.ContactNo,'')   ";
            SQL += "FROM TBL_CIF_MASTER M ";
            SQL += "left outer join TBL_CIF_ADDRESS A ";
            SQL += "on M.CIFID = A.CIFID ";
            SQL += "left outer join TBL_CIF_CONTACTS C ";
            SQL += "on M.CIFID = C.CIFID ";
            SQL += "where M.CIFID = '" + this.SEQ_ID + "' ";
            SQL += "order by M.SEQ_ID ";
                 
        var dt = BillsPayment.getDataTable(SQL).value;
        
        if (dt != null && dt.Rows.Length != 0)
        {
            this.RemitterFullName     = dt.Rows[0].Name.toString();
            this.RemitterAddress      = dt.Rows[0].Address.toString();
            this.RemitterPhoneNumber  = dt.Rows[0].ContactNo.toString();
            this.RemitterID           = dt.Rows[0].CIFID.toString();
            
            dt = null;
            
            document.getElementById("tblBPMerchants").style.display          = "block";
            document.getElementById("tblBPMerchants").style.width          = "100%";
            document.getElementById("tblBPMerchants").style.height          = "100%";
            document.getElementById("lbl_MID").innerText = "Member ID:";  
        }          
    }
    
    this.get_enrollmerchant_info = function()
    {
         var SQL = "SELECT M.SEQ_ID ";
            SQL += ",M.MiddleName ";
            SQL += ",MerchantCode = isnull(M.FirstName,'') "; 
            SQL += ",Name = isnull(M.LastName,'') ";
            SQL += ",AccountNo = isnull(A.AccountNo,'') ";
            SQL += ",AccountCurrency = isnull(L.FldDescription,'') ";
            SQL += ",ProductCode = isnull(,'') ";
            SQL += ",ProductDesc = isnull(,'') ";
            SQL += "FROM TBL_CIF_MASTER M ";
            SQL += "left outer join TBL_CIF_ACCOUNTS A ";
            SQL += "on M.CIFID = A.CIFID ";
            SQL += "left outer join TBL_LOOKUP L ";
            SQL += "on A.Currency = L.Fldvalue and L.Fldgroup = 'Currency' ";
            SQL += "right outer join TBL_CIF_MERCHANTS MCS ";
            SQL += "on MCS.CIFID = M.CIFID ";
            SQL += "where MCS.accountseqid = " + this.SEQ_ID + " ";
            SQL += "order by M.SEQ_ID ";
                 
        var dt = BillsPayment.getDataTable(SQL).value;
        
        if (dt != null && dt.Rows.Length > 0)
        { 
            this.MerchantName = dt.Rows[0].Name.toString();
            this.MerchantCode = dt.Rows[0].MerchantCode.toString();
            this.MemberID = dt.Rows[0].MiddleName.toString();
            this.MerAccountCurr = dt.Rows[0].AccountCurrency.toString();
//            this.MerAccountNumber = dt.Rows[0].AccountNo.toString();
            this.MerProdCode = dt.Rows[0].ProductCode.toString();
            this.MerProdDesc = dt.Rows[0].ProductDesc.toString();
//            document.getElementById("btnSearchMerchant").enable = false;
            dt = null;
            document.getElementById("tblMDetails").style.display             = "block"; 
            document.getElementById("tblMDetails").style.width             = "100%"; 
            document.getElementById("tblMDetails").style.height             = "100%"; 
        }    
    }
    
    this.get_merchant_info = function()
    {
         var SQL = "SELECT TOP 1 M.SEQ_ID ";
            SQL += ",M.MiddleName ";
            SQL += ",MerchantCode = isnull(M.FirstName,'') "; 
            SQL += ",Name = isnull(M.LastName,'') ";
            SQL += ",AccountNo = isnull(A.AccountNo,'') ";
            SQL += ",AccountCurrency = isnull(L.FldDescription,'') ";
            SQL += ",ProductCode = isnull(P.FldCode,'') ";
            SQL += ",ProductDesc = isnull(P.FldDescription,'') ";
            SQL += "FROM TBL_CIF_MASTER M ";
            SQL += "left outer join TBL_CIF_ACCOUNTS A ";
            SQL += "on M.CIFID = A.CIFID ";
            SQL += "left outer join TBL_LOOKUP L ";
            SQL += "on A.Currency = L.Fldvalue and L.Fldgroup = 'Currency' ";
            SQL += "left outer join TBL_CIF_MERCHANTS MC ";
            SQL += "on M.CIFID = MC.CIFID ";
            SQL += "left outer join TBL_LOOKUP P ";
            SQL += "on MC.ProductCode = P.Fldvalue and P.Fldgroup = 'Product Code' ";
            SQL += "where M.SEQ_ID = " + this.SEQ_ID + " ";
            SQL += "order by M.SEQ_ID ";
                 
        var dt = BillsPayment.getDataTable(SQL).value;
        
        if (dt != null && dt.Rows.Length != 0)
        {
            this.MerchantName = dt.Rows[0].Name.toString();
            this.MerchantCode = dt.Rows[0].MerchantCode.toString();
            this.MemberID = dt.Rows[0].MiddleName.toString();
            this.MerAccountCurr = dt.Rows[0].AccountCurrency.toString();
            this.MerAccountNumber = dt.Rows[0].AccountNo.toString();
            this.MerProdCode = dt.Rows[0].ProductCode.toString();
            this.MerProdDesc = dt.Rows[0].ProductDesc.toString();
            
            dt = null;            
        }
        else
        {
            var sSQL = "SELECT * FROM MERCHANT WHERE MERCHANTCODE = '" + this.SEQ_ID + "'";
            var dt2 = BillsPayment.getOBPData(sSQL).value;
            if (dt2 != null && dt2.Rows.length != 0)
            {
                this.MerchantName = dt2.Rows[0].MerchantDescription.toString();
                this.MerchantCode = dt2.Rows[0].MerchantCode.toString();
//                this.MerAccountCurr
//                this.MerAccountNumber
//                this.MerProdCode
//                this.MerProdDesc
            }
        }
    }
    
    this.popMerDetail = function()
    {
       if (this.LookUpMode == "MERCHANT")
       {
            this.get_merchant_info();
       }
       else
       {
            this.get_enrollmerchant_info();
       }
       if (this.MerchantName != null)
       {
           document.getElementById("lbl_merchant_name").innerText = this.MerchantName;
           this.MerchantProducts();
           this.MerchantAccounts();
    //       document.getElementById('lbl_merchant_acctno').innerText = this.MerAccountNumber;
//           document.getElementById('lbl_merchant_id').innerText = this.MemberID;       
//           document.getElementById('lbl_merchant_currency').innerText = this.MerAccountCurr;
//           document.getElementById('lbl_merchant_prodcode').innerText = this.MerProdCode;       
//           document.getElementById('lbl_merchant_prodname').innerText = this.MerProdDesc;
            document.getElementById("txtMemberID").style.display = "block";
            this.MerchantFields();
            document.getElementById("divBPRemitter").style.height = "450px";
       }
    }
    
    this.MerchantProducts = function()
    {
        var SQL = "SELECT DISTINCT PRODUCTCODE FROM PRODUCTTYPE WHERE MERCHANTCODE = '" + this.MerchantCode + "'";
        var dt = BillsPayment.getOBPData(SQL).value;
        document.getElementById('lbl_merchant_prodcode').innerText = "";
        document.getElementById("ddMProductCode").style.display = "none";
        if (dt != null && dt.Rows.length != 0)
        {
            if (dt.Rows.length > 1)
            {
                document.getElementById("ddMProductCode").style.display = "block";
                this.popDDL(document.getElementById("ddMProductCode"), this.MerchantCode);                
                this.MerProdCode = document.getElementById("ddMProductCode")[document.getElementById("ddMProductCode").selectedIndex].text;
                document.getElementById("ddMProductCode").onchange = Function('remteller.MerchantFields()');
            }
            else
            {
                document.getElementById('lbl_merchant_prodcode').innerText = dt.Rows[0].PRODUCTCODE.toString();
                this.MerProdCode = dt.Rows[0].PRODUCTCODE.toString();
            }            
        }
    }
    
    this.MerchantAccounts = function()
    {
        var SQL = "SELECT DISTINCT CURRENCY FROM CASA WHERE MERCHANTCODE = '" + this.MerchantCode + "'";
        var dt = BillsPayment.getOBPData(SQL).value;
        document.getElementById('lbl_merchant_currency').innerText = "";
        document.getElementById("ddMAccountCurrency").style.display = "none";
        if (dt != null && dt.Rows.length != 0)
        {
            if (dt.Rows.length > 1)
            {
                document.getElementById("ddMAccountCurrency").style.display = "block";
                this.popDDL(document.getElementById("ddMAccountCurrency"), this.MerchantCode);
            }
            else
            {
                document.getElementById('lbl_merchant_currency').innerText = dt.Rows[0].CURRENCY.toString();
            }            
        }
    }
    
    this.MerchantFields = function()
    {
        if (document.getElementById("ddMProductCode").style.display != "none")
        {
            this.MerProdCode = document.getElementById("ddMProductCode")[document.getElementById("ddMProductCode").selectedIndex].text;
        }
        this.removeTableRow();
        this.insertTableRow();
        document.getElementById("trxncolumn").style.display = "block";
        this.popDDL(document.getElementById("ddSourceFunds"),"SourceOfFunds");
        this.popDDL(document.getElementById("ddPayMode"),"PayMode");
        this.popDDL(document.getElementById("ddRemCurrency"),"1068");
    }
    
    this.insertTableRow = function()
    {
        var theTable = document.getElementById("tblMDetails");
        var theTableBody = theTable.tBodies[0];
        
         
        var SQL = "SELECT * FROM OPTIONALFIELDSVALIDATION WHERE MERCHANTCODE = '" + this.trim(this.MerchantCode) + "' AND PRODUCTCODE = '" + this.trim(this.MerProdCode) + "' AND ISDELETED = 0 ORDER BY DISPLAYHIERARCHY";
        
        var dt = BillsPayment.getOBPData(SQL).value;
        
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
                var rowData = [dt.Rows[cnt].FieldLabel.toString(), dt.Rows[cnt].FieldName1.toString()];
        //        clearBGColors()
                var newCell;
                var newRow = theTableBody.insertRow(cnt + 1);
                if (dt.Rows[cnt].FieldName1.toString() == "id1")
                {
                    document.getElementById("lbl_MID").innerText = dt.Rows[cnt].FieldLabel.toString()
                }
//                var lastRow = theTable.rows.Length;
//                var iteration = lastRow;
//                var row = tbl.insertRow(lastRow);
                else
                {
                for (var i = 0; i < rowData.length; i++) {
                    newCell = newRow.insertCell(i);                 

                      if (i == 0)
                      {
                          newCell.innerHTML = rowData[i];
                          newCell.style.backgroundColor = "lavender"
                          newCell.style.fontSize = "12px"
                          newCell.style.fontFamily = "Arial"
                          newCell.style.fontWeight = "Bold"
                          newCell.style.color = "MidnightBlue"
                          newCell.style.borderBottom = "solid 1px midnightblue"
                          newCell.style.textAlign = "right"
                          newCell.style.width = "31%"
                          newCell.style.height = "24px"
                      }
                      else
                      {
                          if(dt.Rows[cnt].FieldName1.toString().substring(0,2) == "tf")
                          {
                              tfld = tfld + 1;
                              if(dt.Rows[cnt].ValidationType.toString() == "LI")
                              {
                                  var el = document.createElement('select')
                                  el.name = 'txt' + tfld;
                                  el.id = 'txt' + tfld;
                                  newCell.appendChild(el);
                                  this.LIFldRef = dt.Rows[cnt].LIFldREf.toString();
                                  this.popDDL(el, this.LIFldRef);
                              }
                              else
                              {
                                  var el = document.createElement('input')
                                  el.type = 'text';
                                  el.name = 'txt' + tfld;
                                  el.id = 'txt' + tfld;
                                  newCell.appendChild(el);
                              }
                          }
                          else if (dt.Rows[cnt].FieldName1.toString().substring(0,2) == "df")
                          {
                              dfld = dfld + 1;
                              var el = document.createElement('input')
                              el.type = 'text';
                              el.name = 'dt' + dfld;
                              el.id = 'dt' + dfld;
                              el.readOnly = true;
                              newCell.appendChild(el);
                              var el2 = document.createElement('img')
                              el2.name = 'dtim' + dfld;
                              el2.id = 'dtim' + dfld;
                              el2.src = '../images/calendar/imgcalen.png';
                              el2.alt = 'Pick a date.'
                              
                              newCell.appendChild(el2);
                              var popc = 'popUpCalendar(this,document.forms[0].' + el.id + ',"mm/dd/yyyy")';
                              document.getElementById('dtim' + dfld).onclick = Function(popc);//{popUpCalendar(document.getElementById('dtim' + cnt),document.getElementById('dt' + cnt),'mm/dd/yyyy')};
                          }
                          else if (dt.Rows[cnt].FieldName1.toString().substring(0,2) == "rf")
                          {
                              rfld = rfld + 1;
                              var el = document.createElement('textarea')
//                              el.type = 'text';
                              el.name = 'mtxt' + rfld;
                              el.rows = '5';
                              el.cols = '45';
                              newCell.appendChild(el);
                          }
                          else if (dt.Rows[cnt].FieldName1.toString().substring(0,2) == "nf")
                          {
                              nfld = nfld + 1;
                              if(dt.Rows[cnt].ValidationType.toString() == "LI")
                              {
                                  var el = document.createElement('select')
                                  el.name = 'ntxt' + nfld;
                                  el.id = 'txt' + nfld;
                                  newCell.appendChild(el);
                                  this.LIFldRef = dt.Rows[cnt].LIFldREf.toString();
                                  this.popDDL(el, this.LIFldRef);
                              }
                              else
                              {
                                  var el = document.createElement('input')
                                  el.type = 'text';
                                  el.name = 'ntxt' + nfld;
                                  el.id = 'txt' + nfld;
                                  newCell.appendChild(el);
                              }
                          }
                          else if (dt.Rows[cnt].FieldName1.toString().substring(0,2) == "am")
                          {
                              amfld = amfld + 1;
                          }
                          newCell.colSpan = "2"
                          newCell.style.width = "75%"
                          newCell.style.height = "24px"
                          newCell.style.textAlign = "left"
                      }
                 }
                 }
                 document.getElementById("tblMDetails").style.display = "block";
                 document.getElementById("divBPRemitter").style.overflow = "scroll";
             }
             
//             document.getElementById("tblMDetails").style.width             = "100%"; 
//             document.getElementById("tblMDetails").style.height             = "100%";
         }
    }
        
    this.removeTableRow = function()
    {
        var theTable = document.getElementById("tblMDetails");
        var theTableBody = theTable.tBodies[0];
        
        if (theTableBody.rows.length > 1)
        {
            var lastrow = theTableBody.rows.length;
            for (var i = 0; i < lastrow; i++)
            {
                if (theTableBody.rows.length != 1)
                {
                    theTableBody.deleteRow(theTableBody.rows.length - 1)
                }
            }
        }
    }
    this.LTrim = function( value ) 
    {	
	    var re = /\s*((\S+\s*)*)/;
	    return value.replace(re, "$1");	
    }
    
    this.RTrim = function( value ) 
    {	
	    var re = /((\s*\S+)*)\s*/;
	    return value.replace(re, "$1");
	}
  
    this.trim = function( value ) 
    {	
	    return this.LTrim(this.RTrim(value));
	}

    this.popRemDetail = function()
    {
       this.get_remitter_info();
       document.getElementById("lbl_rem_fullname").innerText = this.RemitterFullName;              
       document.getElementById('lbl_rem_id').innerText = this.RemitterID;
       document.getElementById('lbl_rem_address').innerText = this.RemitterAddress;       
       document.getElementById('lbl_rem_telno').innerText = this.RemitterPhoneNumber;       
    }    
    
    this.remteller_vaporize = function()
    {       
        this.commons.to_clear_screen("tblTellerRemitter");
        this.commons.to_clear_screen("tblTellerRemitter");        
    }    

    this.getRemInfo = function()
    {     
        var mw = new modal_window();
        
        mw.open_modal_window();
        mw = null;

    }
 
    this.popDDL = function(DDL, REFID)
    {   //this.trim(this.MerchantCode)
        var SQL =  this.getDDLSQL(DDL, REFID);
        var dt = BillsPayment.getOBPData(SQL).value; 
        var dt2 = BillsPayment.getDataTable(SQL).value; 
        this.cleanDDL(DDL);
        
        if (dt != null && dt.Rows.length > 0)
        {            
            for (var i = 0; i < dt.Rows.length; i++)
            {
                var opt = document.createElement("option");                 
                opt.text = dt.Rows[i].Code.toString();
                opt.value = dt.Rows[i].Description.toString();
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
    
    this.cleanDDL = function(DDL)
    {        
        DDL.length = 0;
    }  
     
    this.getDDLSQL = function(DDL, REFID)
    {
        var SQL;
        var DDLid = DDL.id.substring(0,DDL.id.length - 1);
        
        switch(DDLid)
        {
            case 'txt':
                if (this.MerchantCode == "SSS" & this.MerProdCode == "CONT")
                {
                SQL = "select [Code] = isnull(code,''), [Description] = isnull(Description,'') from syscode where Fldref = '" + REFID + "' and Isdeleted = 0";
                }
                else
                {
                SQL = "select [Code] = isnull(description,''), [Description] = isnull(code,'') from syscode where Fldref = '" + REFID + "' and Isdeleted = 0";
                }
                break; 
            case 'ddSourceFund':
                SQL = "select [Code] = isnull(fldcode,''), [Description] = isnull(fldDescription,'') from tbl_lookup where FldGroup = '" + REFID + "'"
                break; 
            case 'ddPayMod': 
                SQL = "select [Code] = isnull(fldcode,''), [Description] = isnull(fldDescription,'') from tbl_lookup where FldGroup = '" + REFID + "'"
                break; 
            case 'ddMProductCod':   
                SQL = "SELECT DISTINCT [Code] = PRODUCTCODE, [Description] = PRODUCTDESCRIPTION FROM PRODUCTTYPE WHERE MERCHANTCODE = '" + REFID + "'"
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
   
}

