var guserrolecode = '0';

$(document).ready(function () {
    getrolecode();
});

function getrolecode() {
    try {
        IRSWorld.BranchSetup.Receipts.getrolecode(function (result) {
            if (result.error == null) {
                guserrolecode = result.value;
            }
            else {
                alert("Failed to get role, please reload the page");
            }
        });
    }
    catch (err) { //err.message
        spawn_errpage('Failed to get role. Please try again.');
    }
}

function getReceiptType()
{
var optReceipt = document.getElementsByName("ReceiptGroup");

for (var i = 0; i < optReceipt.length; i++) 
    {
    if(optReceipt[i].checked == true) 
        {
        return (optReceipt[i].value);
        }
     }
}
function setReceiptType(idx)
{
var optReceipt = document.getElementsByName("ReceiptGroup");


    optReceipt[idx-1].checked = true
    
}

function fixht()
{
    document.getElementById('divBody').style.height = (top.document.getElementById('divCenter').offsetHeight - 30) + 'px';
    document.getElementById('divBody').style.width = (top.document.getElementById('divCenter').offsetWidth - 3) + 'px';
}

function fixmain()
{
    var brmain = false;
    try
    {
         brmain = parent.is_branchsetup_main();
    }
    catch(e){}
    
    if (!brmain)
    {
        document.getElementById('theBody').onresize = fixht;
        document.getElementById('divBody').style.overflow = 'auto';
        
        fixht();
    }        
}

function get_dropdown()
{
    this.commons           = null;
    this.Load_Branches = function()
    {
        IRSWorld.BranchSetup.Receipts.getBranches(this.load_ddlBranches_callback);
    }
    
    this.load_ddlBranches_callback = function(result)
    {
        if (result.value != null && result.value != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddlBranches", result.value, "BranchCode", "BranchName");
        }
    }
    
    this.Load_ValidID = function(brcode)
    {
        IRSWorld.BranchSetup.Receipts.getValidID(brcode,this.load_ddlValidID_callback);
    }
    
    this.load_ddlValidID_callback = function(result)
    {
        if (result.value != null && result.value != "undefined")
        {
            //alert(result.value.Tables[0].Rows[0].SEQ_ID);
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddlValid1", result.value, "SEQ_ID", "IDDescription");
            this.commons.to_load_dropdownlist("ddlValid2", result.value, "SEQ_ID", "IDDescription");
            this.commons.to_load_dropdownlist("ddlValid3", result.value, "SEQ_ID", "IDDescription");
            this.commons.to_load_dropdownlist("ddlValid4", result.value, "SEQ_ID", "IDDescription");
        }
    }
}


function getValidID(brcode)
{
    new get_dropdown().Load_ValidID(brcode);
}

function populate_record()
{
    IRSWorld.BranchSetup.Receipts.getReceipts(document.getElementById('ddlBranches').value,get_value);
}



function set_visibility(tblname)
{
    if (tblname=='Branch')
    {
        document.getElementById('tblBranch').style.display="block";
        document.getElementById('tblReceiptHeader').style.display="none";
        document.getElementById('tblValidID').style.display="none";
        document.getElementById('tblPrinters').style.display="none";
        document.getElementById('tblButtonsMain').style.display="none";
        document.getElementById('tblReceiptType').style.display="none";
    }
    else if (tblname=='Data')
    {
        if(document.getElementById('ddlBranches').value != '' & document.getElementById('ddlBranches').value != 'null')
        {
            //clear_text();
            document.getElementById('tblBranch').style.display="none";
            document.getElementById('tblReceiptHeader').style.display="block";
            document.getElementById('tblValidID').style.display="block";
            document.getElementById('tblPrinters').style.display="block";
            document.getElementById('tblButtonsMain').style.display="block";
            document.getElementById('tblReceiptType').style.display="block";
            enabled_controls('disabled');
            populate_record();
        }
        else
        {
            alert('Please select a branch');
        }
    }
}


function enabled_button(btn_type)
{
    if (btn_type=='Save')
    {
        document.getElementById('btnSave').disabled = 'disabled';
        document.getElementById('btnEdit').disabled = '';
        document.getElementById('btnCancel').disabled = 'disabled';
        document.getElementById('btnClear').disabled = '';
        document.getElementById('btnBack').disabled = '';
        //clear_text();
        enabled_controls('disabled');
    }
    else if (btn_type=='Cancel')
    {
        document.getElementById('btnSave').disabled = 'disabled';
        document.getElementById('btnEdit').disabled = '';
        document.getElementById('btnCancel').disabled = 'disabled';
        document.getElementById('btnBack').disabled = '';
        document.getElementById('btnClear').disabled = '';
        enabled_controls('disabled');
    }
    else if (btn_type=='Edit')
    {
        document.getElementById('btnSave').disabled = '';
        document.getElementById('btnEdit').disabled = 'disabled';
        document.getElementById('btnCancel').disabled = '';
        document.getElementById('btnBack').disabled = 'disabled';
        document.getElementById('btnClear').disabled = 'disabled';
        enabled_controls('');
    }
}

function enabled_controls(x)
{
    document.getElementById('txtHeader1').disabled = x;
    document.getElementById('txtHeader2').disabled = x;
    document.getElementById('txtBranchName').disabled = x;
    document.getElementById('txtAddress').disabled = x;
    document.getElementById('txtContact').disabled = x;
    document.getElementById('txtEmail').disabled = x;
    document.getElementById('txtcorptelno').disabled = x;
    document.getElementById('txtcorpaddress').disabled = x;
    document.getElementById('ddlValid1').disabled = x;
    document.getElementById('ddlValid2').disabled = x;
    document.getElementById('ddlValid3').disabled = x;
    document.getElementById('ddlValid4').disabled = x;
    document.getElementById('optLaser').disabled = x;
    document.getElementById('optOther').disabled = x;
    document.getElementById('txtNoCopies').disabled = x;
    document.getElementById('optOriginal').disabled = x;
    document.getElementById('optRCN').disabled = x;
    document.getElementById('optGuam').disabled = x;
    document.getElementById('optTagalog').disabled = x;
    document.getElementById('optCompressed').disabled = x; //
}

function page_setup()
{
    new get_dropdown().Load_Branches();
    set_init_value();
    set_visibility('Branch');
    fixmain();
}

function check(opt_name)
{
    if(opt_name=='Laser')
    {
        document.getElementById('optLaser').checked = 'checked';
        document.getElementById('optOther').checked = '';
    }
    else if(opt_name=='Other')
    {
        document.getElementById('optLaser').checked = '';
        document.getElementById('optOther').checked = 'checked';
    }
}

function check_type(opt_name)
{
    if(opt_name=='Original')
    {
        document.getElementById('optOriginal').checked = 'checked';
        document.getElementById('optTagalog').checked = '';
        document.getElementById('optRCN').checked = '';
        document.getElementById('optGuam').checked = '';
        document.getElementById('optCompressed').checked = ''; //
    }
    else if(opt_name=='Tagalog')
    {
        document.getElementById('optOriginal').checked = '';
        document.getElementById('optTagalog').checked = 'checked';
        document.getElementById('optRCN').checked = '';
        document.getElementById('optGuam').checked = '';
        document.getElementById('optCompressed').checked = ''; //
    }
    else if(opt_name=='RCN')
    {
        document.getElementById('optOriginal').checked = '';
        document.getElementById('optTagalog').checked = '';
        document.getElementById('optRCN').checked = 'checked';
        document.getElementById('optGuam').checked = '';
        document.getElementById('optCompressed').checked = ''; //
    }
    else if(opt_name=='Guam')
    {
        document.getElementById('optOriginal').checked = '';
        document.getElementById('optTagalog').checked = '';
        document.getElementById('optRCN').checked = '';
        document.getElementById('optGuam').checked = 'checked';
        document.getElementById('optCompressed').checked = ''; //
    }
    //added by ruthy
      else if(opt_name=='Compressed')
    {
        document.getElementById('optOriginal').checked = '';
        document.getElementById('optTagalog').checked = '';
        document.getElementById('optRCN').checked = '';
        document.getElementById('optGuam').checked = '';
        document.getElementById('optCompressed').checked = 'checked'; //
      }
      //Alyssa  -RFC 22551 PNB NY update of Remittance Receipt layout and Terms and Conditions of IRSWNY TEL
      else if (opt_name == 'NewYork') {
          document.getElementById('optOriginal').checked = '';
          document.getElementById('optTagalog').checked = '';
          document.getElementById('optRCN').checked = '';
          document.getElementById('optGuam').checked = '';
          document.getElementById('optCompressed').checked = ''; 
          document.getElementById('optNewYork').checked = 'checked'; //
      }
}

function save_receipts()
{

    if (guserrolecode == '2') {
        alert('Approver is not allowed to modify current set-up.')
        return;
    }

    var brcode = document.getElementById('ddlBranches').value;
    var header1 = document.getElementById('txtHeader1').value;
    var header2 = document.getElementById('txtHeader2').value;
    var brname = document.getElementById('txtBranchName').value;
    var address = document.getElementById('txtAddress').value;
    var contact = document.getElementById('txtContact').value;
    var email = document.getElementById('txtEmail').value;
    //modified by robert habaradas for added fields on report header
    var corptelno = document.getElementById('txtcorptelno').value;
    var corpaddress = document.getElementById('txtcorpaddress').value;
    //end modification
    var valid_id1 = document.getElementById('ddlValid1').value;
    var valid_id2 = document.getElementById('ddlValid2').value;
    var valid_id3 = document.getElementById('ddlValid3').value;
    var valid_id4 = document.getElementById('ddlValid4').value;
    var receipt_form = (document.getElementById('optLaser').checked) ? '1' : '2';
    var receipt_type = (document.getElementById('optOriginal').checked) ? '1' : '2';
    receipt_type = getReceiptType();
        
    var no_copies = document.getElementById('txtNoCopies').value;
    
    if (trim(header1) == '' | trim(header2) == '' | trim(brname) == '' | trim(address) == '' | trim(contact) == '')
    {
        alert('Please fill-up necessary information');
        return;
    } 
    
    if(no_copies == '')
    {
        alert('Number of copies should not be blank');
        return;
    }
    
    if(!isInt(no_copies))
    {
        alert('Number of copies should be numeric');
        document.getElementById('txtNoCopies').value = '';
        return;
    }
    
    if(parseInt(no_copies) < 0)
    {
        alert('Negative values not allowed!');
        document.getElementById('txtNoCopies').value = '';
        return;
    }
    var value = header1 + '|' + header2 + '|' + brname + '|' + address + '|' + contact + '|' +  email  + '|' + corptelno + '|' + corpaddress +  '|' + valid_id1 + '|' + valid_id2 + '|' + valid_id3 + '|' + valid_id4 + '|' + receipt_form + '|' + no_copies + '|' + receipt_type;
    IRSWorld.BranchSetup.Receipts.SaveReceipts(brcode,value,save_receipts_callback);
}

function save_receipts_callback(result)
{
    if(result.value == null | result.value == result)
    {
        alert(result.error.Message);
        return;
    }
    enabled_button('Save');
    //clear_text();
    set_visibility('Main');
    alert('Successfully submitted for approval');
}


function isInt(myNum) 
{
     var myMod = myNum % 1;

     if (myMod == 0) {
             return true;
     } else {
             return false;
     }
}


function get_value(rs)
{
    if (rs.error != null) 
    {   
        alert(rs.error.Message);
        return;
    }
    
    getReceiptData(rs.value);
}


function check_char_inputted(controlname)
{
    var isinvalid = isSpecial(document.getElementById(controlname).value);
    if(isinvalid)
    {
        //document.getElementById(controlname).value = '';
        return;
    }
}

function getReceiptData(dt)
{
    if (dt.Rows.length > 0)
    {
        document.getElementById('txtHeader1').value = dt.Rows[0]['Header1'];
        document.getElementById('txtHeader2').value = dt.Rows[0]['Header2'];
        document.getElementById('txtBranchName').value = dt.Rows[0]['NameOfBranch'];
        document.getElementById('txtAddress').value = dt.Rows[0]['Address'];
        document.getElementById('txtContact').value = dt.Rows[0]['ContactNo'];
        document.getElementById('txtEmail').value = dt.Rows[0]['Email'];
        //modified by robert habaradas for added fields on report header
        document.getElementById('txtcorptelno').value = dt.Rows[0]['corp_telno'];
        document.getElementById('txtcorpaddress').value = dt.Rows[0]['corp_address'];
        // end modification
        this.commons = new common_method();
        this.commons.to_set_dropdownlist_index('ddlValid1',dt.Rows[0]['ValidID1']);
        this.commons.to_set_dropdownlist_index('ddlValid2',dt.Rows[0]['ValidID2']);
        this.commons.to_set_dropdownlist_index('ddlValid3',dt.Rows[0]['ValidID3']);
        this.commons.to_set_dropdownlist_index('ddlValid4',dt.Rows[0]['ValidID4']);
//        document.getElementById('ddlValid1').value = dt.Rows[0]['ValidID1'];
//        document.getElementById('ddlValid2').value = dt.Rows[0]['ValidID2'];
//        document.getElementById('ddlValid3').value = dt.Rows[0]['ValidID3'];
//        document.getElementById('ddlValid4').value = dt.Rows[0]['ValidID4'];
        
        //alert(document.getElementById('ddlValid1').value);
        var receipt_form = dt.Rows[0]['ReceiptForm'];
        (receipt_form=='1') ? document.getElementById('optLaser').checked ='checked' : document.getElementById('optOther').checked = 'checked';
        
        
        var receipt_type = dt.Rows[0]['ReceiptType'];

       setReceiptType(receipt_type);
        
        //(receipt_type=='1') ? document.getElementById('optOriginal').checked ='checked' : document.getElementById('optTagalog').checked = 'checked';
        document.getElementById('txtNoCopies').value = dt.Rows[0]['NoOfCopies'];
    }
    else
    {
        clear_text();
    }
}

function clear_text()
{
    document.getElementById('txtHeader1').value = '';
    document.getElementById('txtHeader2').value = '';
    document.getElementById('txtBranchName').value = '';
    document.getElementById('txtAddress').value = '';
    document.getElementById('txtContact').value = '';
    //modified by robert habaradas for added fields on report header
    document.getElementById('txtcorptelno').value = '';
    document.getElementById('txtcorpaddress').value = '';
    //end
    document.getElementById('optLaser').checked = '';
    document.getElementById('optOther').checked = '';
    document.getElementById('txtNoCopies').value = '';
    document.getElementById('txtEmail').value = '';
    document.getElementById('ddlValid1').value = '';
    document.getElementById('ddlValid2').value = '';
    document.getElementById('ddlValid3').value = '';
    document.getElementById('ddlValid4').value = '';
    document.getElementById('optOriginal').checked = '';
    document.getElementById('optRCN').checked = '';
    document.getElementById('optGuam').checked = '';
    document.getElementById('optTagalog').checked = '';
    document.getElementById('optCompressed').checked = ''; //
    document.getElementById('optNewYork').checked = ''; //
    //getValidID(document.getElementById('ddlBranches').value);
}

function close_session()
{
    //alert("closed");
    if (is_closing())
    {
        IRSWorld.BranchSetup.Receipts.close_session();
    }
    else
    {
        IRSWorld.BranchSetup.Receipts.drop_irsUsers();
    }
}

function isInt(myNum) 
{
     var myMod = myNum % 1;

     if (myMod == 0) {
             return true;
     } else {
             return false;
     }
}

function set_init_value()
{
    var loc = IRSWorld.BranchSetup.Receipts.getStartUpValue();
    this.commons = new common_method();
    this.commons.to_set_dropdownlist_index('ddlBranches',loc.value);
}

// Removes leading whitespaces
function LTrim( value ) {
	
	var re = /\s*((\S+\s*)*)/;
	return value.replace(re, "$1");
	
}

// Removes ending whitespaces
function RTrim( value ) {
	
	var re = /((\s*\S+)*)\s*/;
	return value.replace(re, "$1");
	
}

// Removes leading and ending whitespaces
function trim( value ) {
	
	return LTrim(RTrim(value));
	
}
