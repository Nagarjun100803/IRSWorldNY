<%@ Page Language="C#" AutoEventWireup="true" Codebehind="OBPSTransactions.aspx.cs"
    Inherits="IRSWorld.BillsPayment.OBPSTransactions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>OBPS - Transaction Walkin</title>
    <link href="../css/Default_min.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css"> </style>

<%--    <script src="../js/jsCommons.js" type="text/javascript"></script>--%>
    <script src="../js/commons_min.js" type="text/javascript"></script> 
    <script src="../js/datagrid_min.js" type="text/javascript"></script>
    <script src="../js/jsgridselector_min.js" type="text/javascript"></script>
    <script src="../js/js_sidebar_min.js" type="text/javascript"></script>
    <script src="../js/jshttprequest_min.js" type="text/javascript"></script>
    <script src="../js/js_generic_min.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod_min.js" type="text/javascript"></script>
    <%--<script src="../js/js_sidebar.js" type="text/javascript"></script>--%>
    <script src="../js/jsmodalwindow_min.js" type="text/javascript"></script>
    <script src="../js/BillsPayment/jsOBPSTransactions_min.js" type="text/javascript"></script>
    <script src="../js/popcalendar_min.js" type="text/javascript"></script>
    <script src="../js/BillsPayment/js_OBPSSideMenu_min.js" type="text/javascript"></script>
    <script src="../js/BillsPayment/jsOBPSView_min.js" type="text/javascript"></script>
    <script src = "../js/Remittance/jsRemExternals_min.js" type = "text/javascript"></script>
    <script src = "../js/RemoteOverride/js_RemOverride.js" type = "text/javascript"></script>

    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery-3.5.1.min.js"></script>
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery-migrate-3.3.1.js"></script>
    <link href="<%=ResolveUrl("~/") %>js/BranchSetup/BS/bootstrap.min.css" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/bootstrap.bundle.min.js"></script>
    <link href="<%=ResolveUrl("~/") %>js/BranchSetup/BS/buttons.dataTables.min.css" rel="stylesheet" />
    <link href="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="<%=ResolveUrl("~/") %>js/BranchSetup/BS/responsive.dataTables.min.css" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery.dataTables.min.js"></script>
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/dataTables.responsive.min.js"></script>
    <link href="<%=ResolveUrl("~/") %>js/BranchSetup/BS/waitMe.min.css" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/waitMe.min.js"></script>
    <link href="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery-ui.css" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery-ui.min.js"></script>
    <style>
        .nav-pills .nav-link.active, .nav-pills .show>.nav-link {
            color: #f0f8ff;
            background-color: #5697c1;
        }
        .nav-link {
            font-size: small;
        }
        table.dataTable thead th, table.dataTable thead td {
            padding: 0px 0px;
            font-size: small;
            background-color: #5697c1;
            color: #f0f8ff;
            border-left: 6px solid #5697c1;
        }
        .table-bordered>:not(caption)>*>* {
            border-width: 1px 1px;
        }
        table.dataTable tbody tr {
            font-size:small;
            padding: 0px 0px;
            border-left: 6px solid lightgray;
        }
        /*#tblHitList tbody tr {
            font-size:small;
            padding: 0px 0px;
        }*/
        table.dataTable tbody th, table.dataTable tbody td {
            padding: 3px 8px;
        }
        .form-control:focus {
            box-shadow: 0 0 0 0rem rgb(13 110 253 / 25%);
        }
        .nav-link {
            color: #3f769a;
            font-weight: bold;
        }
    </style>
    <script language="javascript" type="text/javascript">
        var commons = new common_method();        

        var remteller = new bills_rem_teller();
        var bills = new frm_bills();
        var frm_validation = new frm_validation();
        var sidebar = new SideBar();
        var obpsview = new obps_view();
        var idisclaimer = new online_disclaimer();
        var oreceipt = new online_recept();
        var agentRem = new agent_remittance();
        
        function execute_ajx_command(param)
        {            
            if (remteller != null && remteller != "undefined")
            {
                remteller.do_ajax(param);                 
            }             
        }
        
	    function keyUP(keyCode) 
	    { 
		    if(keyCode==16)
				    isShift = false;
	    } 

	    function keyDown(keyCode)
	    {
	        var ev;
	        if(window.event)
	        {ev = event.keyCode}
	        else{ev = keyCode.charCode}
	        if ((ev >= 48 && ev <= 57) || ev == 46 || ev == 13) 
            {
                if(ev == 46 & (document.getElementById("txtTargetAmount").value.indexOf('.') >= 0))
                {                    
                    if(window.event)
                    {
                        event.returnValue=false;
                    }
                    else
                    {
                        return false
                    }
                    return;
                }
                if(window.event)
                {
                    event.returnValue = true;
                }
                else
                {
                    return true;
                }
                return;
            }
            else
            {
                if(window.event)
                {
                    event.returnValue=false;
                }
                else
                {
                    return false;
                }
                return;
                //alert("Enter only Numbers");
            }
            
	    }
	    
	    function load_main()
        {
            if (top == self)
            {
                document.irs_loader.submit();            
            }
        }
        function formatCurrency(num) 
        {
            num = num.toString().replace(/\$|\,/g,'');
            if(isNaN(num))
                num = "0";
            sign = (num == (num = Math.abs(num)));
            num = Math.floor(num*100+0.50000000001);
            cents = num%100;
            num = Math.floor(num/100).toString();
            if(cents<10)
                cents = "0" + cents;
            for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
                num = num.substring(0,num.length-(4*i+3))+
                num.substring(num.length-(4*i+3));
            return (((sign)?'':'-') + num + '.' + cents);
        }  
        
        function validate_input_ach(evt)
        {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if ((charCode > 47 && charCode < 58) || charCode == 127) 
            {
                return true;
            }
            return false;
        }      
        
    </script>

</head>
<%--javascript:load_main();bills.hide_panel();--%>
<body onload="page_setup();" id="btnTest" onresize="fix_dim();" onunload="javascript:remteller.close_session();"><%--onkeyup="javascript:btn_enter()"--%>
    <form id="form1" runat="server">
    <div id="divalpha" style="width:100%;height:100%">
        <center>
            <div id="divpopProcess" style="z-index: 101; position:absolute; left: 166px; color: #00008B;
                padding: 10px; display: none; font-family: Arial; top: 70px;
                width: 400px; height:200px;">
                <table style="width:100%; height:100%;">
                    <tr>
                        <td>
                            <label id="lblLoadingMsg" style="text-align:center; " class="label2"></label>
                            <div id="divLoadingMsg">
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divSearch" style="z-index: 101; left: 116px; color: #00008B;position:absolute; 
                border: solid 2px #000; padding: 10px; display: none; font-family: Arial; top: 55px;
                width: 666px;" class="table_body">
                <table id="tblSearch" cellpadding="0" cellspacing="0" style="width:70%" <%--class="table_border"--%>>
                    <tr>
                        <td align="left" style="width: 1%; border-collapse: collapse; height: 24px" valign="bottom">
                            <img alt="toe." src="../images/toe.GIF" /></td>
                        <td id="capSearch" align="left" class="table_head_dark" style="width: 32%; border-collapse: collapse;
                            height: 24px">
                            Search Remitter</td>
                        <td align="left" class="bg_body" style="width: 300px; height: 24px" valign="bottom">
                            <img alt="rend." align="absBottom" src="../images/rend.gif" /></td>
                    </tr>
                    <tr>
                        <td colspan="3" style="background: azure; width: 100%">
                            <table class="table_border" style="width: 100%">
                                <tr>
                                    <td style="width: 99%; height: 192px;">
                                        <table cellpadding="0" cellspacing="0" class="table_body" style="border-right: skyblue 1px solid;
                                            border-top: skyblue 1px solid; border-left: skyblue 1px solid; border-bottom: skyblue 1px solid;
                                            width: 99%; height: 181px;">
                                            <tr>
                                                <td class="table_caption" style="width: 50%; height: 26px;">
                                                    Search by:</td>
                                                <td align="left" class="table_caption" style="width: 50%; height: 26px;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2" style="width: 100%; height: 115px" valign="middle">
                                                    <table cellpadding="0" cellspacing="5" style="width:96%; margin-top: 2%">
                                                        <tr>
                                                            <td align="right" class="cell_desc" style="width:30%; height:25px;">&nbsp;OLD CIF ID:</td>
                                                            <td align="left" style="width:70%; height:25px;">
                                                            <input id="txtOldRemitterCIFID" style="font-size:medium; width:100%; text-align:left" type="text" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="cell_desc" style="width:30%; height:25px;">&nbsp;CIF ID:</td>
                                                            <td align="left" style="width:70%; height:25px;">
                                                            <input id="txtRemitterCIFID" style="font-size:medium; width:100%; text-align:left" type="text" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="cell_desc" style="width: 30%; height: 25px;">
                                                                &nbsp;Lastname :</td>
                                                            <td align="left" style="width: 70%; height: 25px;">
                                                                <input id="txtSearchLastname" style="font-size: medium; width: 100%; text-align: left"
                                                                    type="text" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="cell_desc" style="width: 30%">
                                                                &nbsp;Firstname :</td>
                                                            <td align="left" style="width: 70%">
                                                                <input id="txtSearchFirstname" style="font-size: medium; width: 100%; text-align: left"
                                                                    type="text" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="cell_desc" style="width: 30%">
                                                                &nbsp;Middlename :</td>
                                                            <td align="left" style="width: 70%">
                                                                <input id="txtSearchMiddlename" style="font-size: medium; width: 100%; text-align: left"
                                                                    type="text" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="height: 28px;vertical-align:top;" valign="top" align="center">
                                                    <input id="btnsearch" class="button" onclick="javascript:remteller.callLookUp('REMITTER');hidePanelOB('divSearch');"
                                                        onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                        type="button" value="Search" />
                                                    <input id="btnCancelSearch" class="button" onclick="javascript:hidePanelOB('divSearch');"
                                                        onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                        type="button" value="Cancel" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divMerchantDetails" style="z-index: 101; left: 116px; overflow:auto; height:85%; width:45%; position:absolute;
                color: #00008B; border: solid 2px #000; padding: 10px; display: none; font-family: Arial;
                top: 350px;" class="table_body">
                <table style="width:auto" cellpadding="0" cellspacing="0">
                    <tr id="trCharges" style="display: block">
                        <td id="tdlblCharges" style="height: 17px">
                            <b>Charges</b></td>
                    </tr>
                    <tr id="trChargesDetails" style="display: block">
                        <td>
                            <div id="calc" style="display: block; background-color:#F0FFFF; border:#f9f9f9; font-size:8px; font-family:Arial;">
                            </div>
                        </td>
                    </tr>
                    <tr id="trOverride" style="display:none;">
                        <td><b>Override Charges</b></td>
                    </tr>
                    <tr id="trOverrideDetails" style="display:none;">
                        <td>
                            <%--<div id="content_mod_div" style="display:block;"></div>--%>
                        </td>
                    </tr>
                    <tr id="trMerchant" style="display: none">
                        <td>
                            <b>Merchant Details</b></td>
                    </tr>
                    <tr id="trMerchantDetails">
                        <td>
                            <table id="tblMerchantDetails" style="width:80%" cellpadding="0" cellspacing="8">
                                <tr id="MerD">
                                    <td style="width: 100%;">
                                        <table id="tblBillsPayOBPSTransactionsMDetails" cellpadding="0" cellspacing="8"
                                            style="border: solid 1px #336699; vertical-align: top; width:100% ">
                                            <tr>
                                            </tr>
                                            
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="lblTranxHistory" style="display: none;">
                        <td>
                            <b>Transaction History</b></td>
                    </tr>
                    <tr>
                        <td>
                            <div id="DIV_TRAN_DETAILS" style="display: block;" class="table_body">
                                test</div>
                        </td>
                    </tr>
                    <tr valign="top" style="display: block">                        
                        <td class="label1" align="right" style="display: block">
                            <input style="display:inline; width: 150px;" id="btnOverrideCharges" type="button" value="Override" onclick="javascript:remteller.open_override_window();" />
                            <input style="display:inline; width: 150px;" id="btnWaive" type="button" value="Waive" onclick="javascript:remteller.waivecharges();" />
                            <input style="display: inline; width: 150px;" id="btnBClosePanel" type="button" value="Close"
                                onclick="hidePanelOB('divMerchantDetails');" /></td>
                    </tr>
                </table>
            </div>
        </center>
        <!--
        <div style="width: 100%;">
            <table width="100%" cellpadding="0" cellspacing="5">
                <tr>
                    <td align="center" valign="top" style="width: 100%; border-collapse: collapse; height: auto;"
                        class="content_height" colspan="2">
                        <div id="divBillsPayOBPSTransactions" style="width: 100%; height: 100%; display: block">
                            <table width="100%">
                                <tr>
                                    <td style="width: 77%; padding-top: 30px">
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        -->
        <div id="divmain" style="width: 100%; height: 100%; display: block; overflow: auto; padding-top:30px; padding-left:0px;">
            <table id="tblMain" cellspacing="5" cellpadding="0" style="display: none; width: 100%;">
                <tr>
                    <td id="tdBillsPayOBPSTransactions" style="vertical-align: top; width:18%;text-align:right; " align="right">
                        <div id="divtdBillsPayOBPSTransactionsMenu" style="width: 100%;">
                        </div>&nbsp;
                        <table id="tblForexRate" style="width:100%">
                            <tr>
                                <!--td style="background-image: url('../images/forex.jpg'); background-repeat: no-repeat; width: 100px; height:221px;"-->
                                <td>
                                    <table style="border-style:none; border-spacing:0;border-collapse:collapse;">
                                        <tr><td style="background-image:url('../images/forex_top.jpg');background-repeat: no-repeat;width:223px; height:28px;"></td></tr>
                                        <tr style="text-align:right; margin-left:auto; margin-right:auto;background-color:#26A6E1;">
                                            <td>
                                                <span id="spnForexDate" style="color:White;"></span>
                                            </td>
                                        </tr>
                                        <tr style="text-align:center; margin-left:auto; margin-right:auto;background-color:#26A6E1;">
                                            <td>
                                                <span id="spnForexValue"></span>
                                            </td>
                                        </tr>
                                        <tr><td style="background-image:url('../images/forex_bottom.jpg');background-repeat: no-repeat; width:223px; height:19px;"></td></tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <a id="btnAnchor" href="#" onclick="sidebar.get_side_bar_visibility('divtdBillsPayOBPSTransactionsMenu','btnAnchor');">« Menu</a>
                    </td> 
                    <td style="width:82%;vertical-align:top;">
                        <table style="width:100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width:98%; vertical-align:bottom" align="left">
                                    <table cellpadding="0" cellspacing="0" >
                                    <tr>
                                        <td align="left" style="width:10px; height:24px; border-collapse:collapse; vertical-align:bottom;"><img alt="toe." align="left" src="../images/toe.GIF" /></td>
                                        <td id="capResult" class="table_head_dark" style="width: 40%; height: 24px; border-collapse:collapse" align="left">
                                            Overseas Bills Payment</td>
                                        <td align="left" style="width: 59%;height: 24px; vertical-align:bottom; border-collapse:collapse">
                                            <img alt="rend." align="absBottom" src="../images/rend.gif" /></td> 
                                    </tr>
                                    </table>
                                </td>                                      
                            </tr>
                            <tr>
		                        <td style="width: 98%; height: 100%" class="table_border">
                                    <div id="divBillsPayOBPSTransactionsRemitter" style="width: 100%; vertical-align: top;"
                                        class="table_body">
                                        <table cellspacing="5" cellpadding="0" id="TABLE1" style="width:100%; vertical-align: top">
                                            <tr id="Trxn" valign="top">
                                                <td colspan="2" style="width: 100%" valign="top">
                                                    <table cellpadding="0" cellspacing="3" style="width:100%">
                                                        <tr>
                                                            <td style="width:100%">
                                                                <table style="border: solid 1px #336699;width:100%">                                                                                                        
                                                                    <tr id="trTodayExRate">
                                                                        <td colspan="4" class="table_caption" style="width: 75%; text-align: center; text-indent: 5px;
                                                                                color: Black; height: 26px; font-size: smaller" >
                                                                            <span id="spnExchangeRate" class="label2" >Today's Exchange Rate: </span><label id="lblfxRate" class="label2"></label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td id="tdProduct" colspan="1" style="width: 22%;" class="cell_caption">
                                                                            <a>Mode of Transaction:&nbsp;</a>
                                                                        </td>
                                                                        <td id="tdProductCode" colspan="1" style="width: 25%;" align="left">
                                                                            <span id="spnProductCode" style="width: 90%; text-align: right;" class="label2">Rapid Remit</span>
                                                                        </td>
                                                                        <td id="tdImplem" style="width:30%;" class="cell_caption">
                                                                            <a>Implementation Type:&nbsp;</a>
                                                                        </td>
                                                                        <td id="tdImplemType" style="width:23%" align="left">
                                                                            <span id="spnImplemType" style="width:90%; text-align:right;" class="label2">Credit To Account</span>                                                                            
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="1" class="cell_caption" style="width: 22%;">
                                                                            <a>Target Amount (PHP):</a>&nbsp;</td>
                                                                        <td colspan="1" style="width: 25%; text-align: left">
                                                                            <input id="txtTargetAmount" maxlength="16" class="cell_desc" style="width: 50%; text-align: right;
                                                                                display: none;" 
                                                                                 value=".00" /><a style="color:Red; font-size:smaller">Press enter.</a><%--onkeypress="javascript:keyDown(event.keyCode);" onkeyup="javascript:keyUP(event.keyCode);enter_targetamoumt()" onchange="populate_rec();" javascript:populate_rec();--%>
                                                                        </td>
                                                                        <td id="tdlblActualAmount" colspan="1" class="cell_caption" style="width: 30%; display:none;">
                                                                            <a>Actual Amount (PHP):</a>&nbsp;</td>
                                                                        <td id="tdActualAmount" colspan="1" align="left" style="width: 23%; display:none;" class="cell_desc">
                                                                            <input id="txtActualAmount" class="cell_desc" style="width: 90%; text-align: right;"
                                                                                onkeydown="javascript:return isNumeric_keycode(event.keyCode);" onchange="populate_rec();"
                                                                                onkeyup="javascript:keyUP(event.keyCode);javascript:populate_rec();" readonly="readonly"
                                                                                value=".00" />
                                                                        </td>       
                                                                    </tr>
                                                                    <%--<tr id ="trActualAmount" style="display:none;">
                                                                                                                                                                             
                                                                    </tr>--%>
                                                                    <tr>
                                                                        <td colspan="1" style="width: 22%;" class="cell_caption">
                                                                            <a>Remitting Currency:&nbsp;</a>
                                                                        </td>
                                                                        <td colspan="1" style="width: 25%;" align="left">
                                                                            <select id="ddlSourceCurrency" class="ddl" onchange="javascript:remteller.setremittingccy();" style="width: 100%">
                                                                            </select>
                                                                        </td>
                                                                        <td colspan="1" style="width: 30%;" class="cell_caption">
                                                                            <a>Total Amount (RMT-CCY):&nbsp;</a>
                                                                        </td>
                                                                        <td colspan="1" style="width: 23%;" align="left">
                                                                            <span id="spnNetAmount" style="width: 90%; text-align: right;" class="label2">0.00</span>
                                                                        </td>
                                                                    </tr>                                                                                                        
                                                                    <tr>
                                                                        <td colspan="1" style="width: 22%;" class="cell_caption">
                                                                            <a>Total Charge (RMT-CCY):&nbsp;</a>
                                                                        </td>
                                                                        <td colspan="1" style="width: 25%;" align="left">
                                                                            <span id="spnTotalCharges" style="width: 90%; text-align: right;" class="label2">0.00</span>
                                                                        </td>
                                                                        <td colspan="1" style="width: 30%;" class="cell_caption">
                                                                            <a>Amount to be received (Merchant):&nbsp;</a>
                                                                        </td>
                                                                        <td colspan="1" style="width: 23%;" align="left">
                                                                            <span id="spnBenefAmount" style="width: 90%; text-align: right;" class="label2">0.00</span>
                                                                        </td>
                                                                    </tr>                                                                                                        
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="Rem">
                                                <td style="width: 47%" valign="top">
                                                    <table id="tblBPRemitter" align="left" cellpadding="0" cellspacing="2" style="width:100%;border: solid 1px #336699; display: none; vertical-align: top;">
                                                        <tr>
                                                            <td class="table_caption" style="width: 75%; text-align: left; text-indent: 5px;
                                                                color: Black; height: 26px; font-size: smaller" valign="top">
                                                                <a class="label2" style="width:100%">REMITTER INFO. &nbsp;</a>
                                                            </td>
                                                            <td id="tdSearchRemitter" class="table_caption" style="width: 25%; text-align: left; 
                                                                color: Black; height: 22px; font-size: smaller" valign="top">
                                                                <input style="width: 100%;" id="Button2" onclick="javascript:search_page();" class="enable_button"
                                                                    onmouseover="this.className='mouseover_button';" onmouseout="this.className='enable_button';"
                                                                    type="button" value="Search" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" valign="top" style="width:100%; height: 200px">
                                                                <table cellpadding="0" cellspacing="2" style="width:100%; vertical-align: top;">
                                                                    <tr id ="trRemitterID">
                                                                        <td class="cell_caption" style="width: 40%; height: 22px;">
                                                                            <a>Remitter ID:&nbsp;</a></td>
                                                                        <td align="left" colspan="5" style="width: 60%; height: 22px">
                                                                            <label class="cell_desc" id="lbl_rem_id">
                                                                            </label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="cell_caption" style="font-size: 11px; width: 40%; height: 22px;">
                                                                            <a>Complete Name:&nbsp;</a></td>
                                                                        <td align="left" style="width: 60%; height: 22px;" colspan="5">
                                                                            <label class="cell_desc" id="lbl_rem_fullname">
                                                                            </label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="cell_caption" style="width: 40%; height: 22px">
                                                                            <a>Address:&nbsp;</a></td>
                                                                        <td align="left" style="width: 60%; height: 22px;" colspan="5">
                                                                            <label class="cell_desc" id="lbl_rem_address">
                                                                            </label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="cell_caption" style="font-size: 11px; width: 40%; height: 22px">
                                                                            <a>Telephone No:&nbsp;</a></td>
                                                                        <td align="left" style="width: 60%; height: 22px;" colspan="5">
                                                                            <label class="cell_desc" id="lbl_rem_telno">
                                                                            </label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="cell_caption" style="font-size: 11px; width: 40%; height: 22px">
                                                                            <a>Account No:&nbsp;</a><i>(optional)</i></td>
                                                                        <td align="left" style="width: 40%; height: 22px;" colspan="4">
                                                                            <input id="txtRemAccountNo" type="text" style="display:none;"/>
                                                                        </td>
                                                                        <td align="left" style="width: 20%; height: 22px;" >
                                                                            <input id="btnRemAccountNo" type="button" style="display:none;" class="enable_button" onmouseover="this.className='mouseover_button';" onmouseout="this.className='enable_button';" value="Search" onclick="javascript:remteller.callLookUp('REMACCOUNTNO')" />
                                                                        </td>
                                                                    </tr>                                                                    
                                                                    <tr>
                                                                        <td id="tdCheckNo" class="cell_caption" style="font-size: 11px; width: 40%; height: 22px;
                                                                            display: none">
                                                                            <a>Check No:&nbsp;</a></td>
                                                                        <td id="tdtxtCheckNo" align="left" style="width: 60%; height: 22px; display: none"
                                                                            colspan="5">
                                                                            <input id="txtchkno" onkeydown="javascript:return isNumeric_keycode(event.keyCode);" class="cell_desc" style="width: 95%;" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td id="tdClearanceDate" class="cell_caption" style="font-size: 11px; width: 40%;
                                                                            height: 22px; display: none">
                                                                            <a>Clearing Date:&nbsp;</a></td>
                                                                        <td id="tdtxtClearanceDate" align="left" style="width: 60%; height: 22px; display: none"
                                                                            colspan="5">
                                                                            <input id="txtClearanceDate" readonly="readonly" style="width: 83%; height: auto;"
                                                                                type="text" /><img alt="Pick a date." onclick="popUpCalendar(this, document.forms[0].txtClearanceDate, 'mm/dd/yyyy',document.getElementById('divmain'));"
                                                                                    src="../images/calendar/imgcalen.png" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td id="lblOthers" class="cell_caption" style="font-size: 11px; width: 40%; height: 22px;
                                                                            display: none">
                                                                            <a>Others:&nbsp;</a></td>
                                                                        <td id="tdOthers" align="left" style="width: 60%; height: 22px; display: none" colspan="5">
                                                                            <input id="txtOthers" class="cell_desc" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 50%; height: 100%; vertical-align:top"; colspan="6">
                                                                            <div id="divRemAccountDetails"; style="width:100%; height:100%; display:none">
                                                                                <table cellpadding = "0"; cellspacing = "1" style = "width:100%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid" id="Table5">
                                                                                    <tr>
                                                                                        <td class="table_caption" colspan="2" style="width: 75%; text-align: left; text-indent: 5px;color: Black; height: 26px; font-size: smaller" valign="top">
                                                                                            <a class="label2" style="width:100%">REMITTER ACCOUNT DETAILS &nbsp;</a>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr style ="height: 19px">
                                                                                        <td class = "cell_caption" style="text-align: right; width: 50%;">Remitter Account Type :</td>
                                                                                        <td class = "cell_desc" style="width: 50%; height: 19px; text-align: left">
                                                                                            <select id="ddRemAcctType" style="width: 200px">
                                                                                                        <option value="0"></option>
                                                                                                        <option value="1">Savings</option>
                                                                                                        <option value="2">Current</option>
                                                                                                    </select>                                
                                                                                            <label id="astRemAccountType"; style="color : Red">*</label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr style="height: 19px">
                                                                                        <td class="cell_caption" style="width: 50%; text-align: right">Remitter Account No :</td>
                                                                                        <td class = "cell_desc" style="width: 50%; height: 19px; text-align: left"><input type="text" id="txtRemAcctNo" style="width:200px" onkeypress="return validate_input_ach(event);" /><label id="astRemAccountNo"; style="color : Red">*</label></td>
                                                                                    </tr>
                                                                                    <tr style ="height: 19px">
                                                                                        <td class = "cell_caption" style="text-align: right; width: 50%;">Remitter Routing No :</td>
                                                                                        <td class = "cell_desc" style="width: 50%; height: 19px; text-align: left"><input type="text" id="txtRemRoutingNo" style="width:200px" onkeypress="return validate_input_ach(event);" /><label id="astRemRoutingNo"; style="color : Red">*</label></td>
                                                                                    </tr>
                                                                                    <tr style ="height: 19px">
                                                                                        <td class = "cell_caption" style="text-align: right; width: 50%;">Remitter Bank Name :</td>
                                                                                        <td class = "cell_desc" style="width: 50%; height: 19px; text-align: left"><input type="text" id="txtRemBankName" style="width:200px" /><label id="astRemBankName"; style="color : Red">*</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </td> 
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <%--<tr id="Mer">
                                                                            <td>--%>
                                                <td style="width: 53%;" valign="top">
                                                    <table id="tblBillsPayOBPSTransactionsMerchants" style="width:100%; border: solid 1px #336699;">
                                                        <tr>
                                                            <td class="table_caption" style="width: 77%; text-align: left; text-indent: 5px;
                                                                color: Black; height: 22px; font-size: smaller;" valign="top">
                                                                <a class="label2">MERCHANT INFO. &nbsp;</a>
                                                            </td>
                                                            <td class="table_caption" style="width: 23%; text-align: left; 
                                                                color: Black; height: 22px; font-size: smaller" valign="top">
                                                                <input style="width: 100%;" id="Button1" onclick="javascript:remteller.callLookUp('MERCHANT');"
                                                                    class="enable_button" onmouseover="this.className='mouseover_button';" onmouseout="this.className='enable_button';"
                                                                    type="button" value="Search" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td id="tdMerchantTable" colspan="2" style="width:100%; height: 200px" valign="top">
                                                                <table id="TrnxDetails" cellpadding="0" cellspacing="2" style="width:100%; display: none; vertical-align: top;">
                                                                    <tr>
                                                                        <td class="cell_caption" style="font-size: 11px; width: 30%; height: 23px;">
                                                                            <a>Merchant Name:&nbsp;</a></td>
                                                                        <td style="width: 50%; height: 23px; text-align: left;" class="cell_desc">
                                                                            <label class="cell_desc" id="lbl_merchant_name">
                                                                            </label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trMembID">
                                                                        <td class="cell_caption" style="width: 30%; height: 23px;">
                                                                            <label id="lbl_MID"><a></a>
                                                                            </label>
                                                                            &nbsp;</td>
                                                                        <td align="left" style="width: 50%; height: 23px;" class="cell_desc">
                                                                            <%--<label id="lbl_merchant_id">
                                                                            </label>--%>
                                                                            <input id="txtMemberID" class="cell_desc" style="width: 90%; display: none" disabled="disabled" /> <label id="lblEnter" style="color:Red; display:none"><a></a>
                                                                            </label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="cell_caption" style="font-size: 11px; width: 30%; height: 23px;">
                                                                            <a>Account Currency:&nbsp;</a></td>
                                                                        <td align="left" style="width: 50%; height: 23px;" class="cell_desc">
                                                                            <label id="lbl_merchant_currency">
                                                                            </label>
                                                                            <select id="ddMAccountCurrency" class="cell_desc" style="width: 1px; display: none">
                                                                            </select>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="cell_caption" style="font-size: 11px; width: 30%; height: 23px;">
                                                                            <a>Merchant Product Code:&nbsp;</a></td>
                                                                        <td align="left" style="width: 50%; height: 23px;" class="cell_desc">
                                                                            <label id="lbl_merchant_prodcode">
                                                                            </label>
                                                                            <select id="ddMProductCode" class="cell_desc" style="width: 1px; display: none">
                                                                            </select>
                                                                        </td>
                                                                    </tr>                                                                                                        
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="width:100%" align="center">
                                                    <table>
                                                        <tr>
                                                            <td id="tdbtnSave" style="text-align: center; display: block;" valign="top">
                                                                <input style="width: 100%" id="btnSave" onclick="javascript:remteller.SaveTransaction();"
                                                                    class="enable_button" onmouseover="this.className='mouseover_button';" onmouseout="this.className='enable_button';"
                                                                    type="button" value="Send Payment Now" />
                                                            </td>
                                                            <td id="tdbtnDetails" style="text-align: center; display: block;" valign="top">
                                                                <input style="width: 100%" id="btnTrnxDetails" onclick="javascript:verifyButton('btnTrnxDetails');"
                                                                    class="enable_button" onmouseover="this.className='mouseover_button';" onmouseout="this.className='enable_button';"
                                                                    type="button" value="Payment Details" />
                                                            </td>
                                                            <td id="tdbtnCharges" style="text-align: center; display: none;" valign="top">
                                                                <input style="width: 100%" id="btnCharges" onclick="javascript:verifyButton('btnCharges');"
                                                                    class="enable_button" onmouseover="this.className='mouseover_button';" onmouseout="this.className='enable_button';"
                                                                    type="button" value="Show Breakdown" />
                                                            </td>
                                                            <td id="tdbtnCancel" style="text-align: center; display: block;" valign="top">
                                                                <input style="width: 100%" id="btnCancel" onclick="javascript:bills.ShowSearch();"
                                                                    class="enable_button" onmouseover="this.className='mouseover_button';" onmouseout="this.className='enable_button';"
                                                                    type="button" value="Cancel Payment" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>                    
                            </tr>
                        </table>
                    </td>
                </tr>                
            </table>
        </div>
        <div id="divTransactionDetails" style="z-index: 101; position:absolute; left: 175px;
            color: #00008B; border: solid 2px #000; padding: 10px; display: none; font-family: Arial;
            top: 800px; width: 581px; height: 230px;">
            <center>
                <table style="width:98%" cellpadding="0" cellspacing="0">
                    <tr style="display: block">
                        <td colspan="4" id="td1" style="height: 17px">
                            <b>Payment Details</b></td>
                    </tr>
                </table>
                <table id="tbPaymentDet_1" cellpadding="0" cellspacing="0" style="width:98%; border: solid 1px #336699;">
                    <tr>
                        <td style="border: solid 1px #ffffff; width: 50%; text-align: center" class="cell_caption">
                            Branch</td>
                        <td style="border: solid 1px #ffffff; width: 50%; text-align: center" class="cell_caption">
                            Remitting Currency</td>
                    </tr>
                    <tr>
                        <td align="left" style="border: solid 1px #ffffff; width: 50%; height: 23px; text-align: center"
                            class="cell_desc">
                            <label id="lblBranch" class="cell_desc">
                            </label></td>
                        <td style="border: solid 1px #ffffff; width: 50%; text-align: center">
                            <label id="lblRemitCurr" class="cell_desc">
                            </label>
                        </td>
                    </tr>
                </table>
                <table id="tbSpace_1">
                    <tr>
                        <td style="height: 1px">
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" style="width:98%; border: solid 1px #336699;"
                    id="tbPaymentDet_2">
                    <tr>
                        <td style="border: solid 1px #ffffff; width: 50%; text-align: center" class="cell_caption">
                            Charge Type</td>
                        <td style="border: solid 1px #ffffff; width: 50%; text-align: center" class="cell_caption">
                            Receipt Mode</td>                        
                    </tr>
                    <tr>
                        <td align="center" style="border: solid 1px #ffffff; width: 50%; height: 23px" class="cell_desc">
                            <label id="lblChargeType" class="cell_desc">
                            <a href="#" id="lnkChargeType" class="cell_desc" style="text-decoration:underline" onclick="javascript:sidebar.get_remittance_lookup_data('ChargeType');">(Please select)</a>
                            </label>
                        </td>
                        <td style="border: solid 1px #ffffff; width: 50%; text-align: left; text-align: center">
                            <label id="lblReceiptMode" class="cell_desc">
                            <a href="#" id="lnkReceiptMode" class="cell_desc" style="text-decoration:underline" onclick="javascript:sidebar.get_remittance_lookup_data('ReceiptMode');">(Please select)</a>
                            </label>
                        </td>                        
                    </tr>
                </table>
                <table id="tbSpace_2">
                    <tr>
                        <td style="height: 1px">
                        </td>
                    </tr>
                </table>
                <table id="tbPaymentDet_3" cellpadding="0" cellspacing="0" style="width:98%; border: solid 1px #336699;">
                    <tr>
                        <td style="border: solid 1px #ffffff; width: 50%; text-align: center" class="cell_caption">
                            Payment Mode</td>
                        <td style="border: solid 1px #ffffff; width: 50%; text-align: center" class="cell_caption">
                            Source of Funds</td>
                    </tr>
                    <tr>
                        <td align="left" style="border: solid 1px #ffffff; width: 50%; height: 23px; text-align: center"
                            class="cell_desc">
                            <label id="lblPayMode" class="cell_desc">
                            <a href="#" id="lnkPaymode" class="cell_desc" style="text-decoration:underline" onclick="javascript:sidebar.get_remittance_lookup_data('PayMode');">(Please select)</a>
                            </label>
                        </td>
                        <td align="center" style="border: solid 1px #ffffff; width: 50%; height: 23px" class="cell_desc">
                            <label id="lblSourceOfFunds" class="cell_desc">                            
                            <a href="#" id="btnSourceOfFunds" class="cell_desc" style="text-decoration:underline" onclick="javascript:sidebar.get_remittance_lookup_data('SourceOfFunds');">(Please select)</a>
                            </label>
                        </td>
                        
                    </tr>
                </table>
                <table id="tbSpace_3">
                    <tr>
                        <td style="height: 1px">
                        </td>
                    </tr>
                </table>
                <table id="tbPaymentDet_4" cellpadding="0" cellspacing="0" style="width:98%; border: solid 1px #336699;">
                    <tr>
                        <td style="border: solid 1px #ffffff; width: 50%; text-align: center" class="cell_caption">
                            Transaction Source</td>
                        <td style="border: solid 1px #ffffff; width: 50%; text-align: center" class="cell_caption">
                            Transaction Purpose</td>
                    </tr>
                    <tr>
                        <td align="center" style="border: solid 1px #ffffff; width: 50%; height: 23px" class="cell_desc">
                            <label id="lblTranxSource" class="cell_desc">
                            <a href="#" id="lnkTranxSource" class="cell_desc" style="text-decoration:underline" onclick="javascript:sidebar.get_remittance_lookup_data('TranxSource');">(Please select)</a>
                            </label></td>
                            <td style="border: solid 1px #ffffff; width: 50%; text-align: center">
                                <label id="lblTranxPurpose" class="cell_desc">
                                <a href="#" id="lnkTranxPurpose" class="cell_desc"></a><!--style="text-decoration:underline" onclick="javascript:sidebar.get_remittance_lookup_data('TranxPurpose');">(Please select)-->
                                </label>
                            </td>
                    </tr>
                </table>               
                <table style="width:98%" cellpadding="0" cellspacing="0">
                    <tr valign="top" style="display: block">
                        <td class="label1" align="right" style="display: block">
                            <input style="display: block; width: 150px;" id="Button3" type="button" value="Close"
                                onclick="hidePanelOB('divTransactionDetails');" /></td>
                    </tr>
                </table>
            </center>
        </div>
        <div id="divRemitterInfo" style="z-index:75;position:absolute;left:191px;color:darkblue;border:solid 2px #000;padding:10px;display:none;font-family:tahoma;top:1700px;height:auto;">
            <table cellpadding="0" cellspacing="0" style="FONT-SIZE: 12px; width:800px;">
                <tr>
                    <td class="table_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                        Remitter CIFID:
                    </td>
                    <td class="table_caption" style="vertical-align:bottom;text-align:left;width:250px;"><span id="spnRemInfoCIFID" class="label2" style="width:85px;"></span></td>
                    <td class="table_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                        Remitter Name:
                    </td>
                    <td class="table_caption" style="vertical-align:bottom;text-align:left;width:250px;"><span id="spnRemInfoRemitterName" class="label2" style="width:85px;"></span></td>
                </tr>
                <tr>
                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                        BirthDate:
                    </td>
                    <td style="vertical-align:bottom;text-align:left;width:250px;"><input id="txtRemInfoBirthdate" type="text" style="width:200px;" maxlength="13" readonly="readonly" />
                    <img id="imgBirthDate" src="../images/calendar/imgcalen.png" alt="" align="middle" onclick="popUpCalendar(this, document.forms[0].txtRemInfoBirthdate, 'mm/dd/yyyy',document.getElementById('divRemitterInfo'));"/><label id="mnBirthDate" style="color:Red; display:none">*</label></td>
                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                        Source Of Funds:
                    </td>
                    <td style="vertical-align:bottom;text-align:left;width:250px;"><select id="ddlRemInfoSourceOfFunds" class="ddl" style="width:200px"></select><label id="mnSourceOfFunds" style="color:Red; display:none">*</label></td>                                
                </tr>
                <tr>
                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                        Occupation:
                    </td>
                    <td style="vertical-align:bottom;text-align:left;width:250px;"><select id="ddlRemInfoOccupation" class="ddl" style="width:200px"></select><label id="mnOccupation" style="color:Red; display:none">*</label></td>
                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                        Detailed Occupation:
                    </td>
                    <td style="vertical-align:bottom;text-align:left;width:250px;"><input id="txtRemInfoDetailedOccupation" type="text" style="width:200px;" maxlength="50" /></td>                                
                </tr>
               <%-- <tr>
                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                        Employer:
                    </td>
                    <td style="vertical-align:bottom;text-align:left;width:250px;"><input id="txtRemInfoEmployer" type="text" style="width:200px;" maxlength="100" /></td>
                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                        Worker Type:
                    </td>
                    <td style="vertical-align:bottom;text-align:left;width:250px;"><select id="ddlRemInfoWorkerType" class="ddl" style="width:200px"></select></td>
                    
                </tr>
                <tr>
                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                        Annual Salary:
                    </td>
                    <td style="vertical-align:bottom;text-align:left;width:250px;"><input id="txtRemInfoSalary" type="text" style="width:200px;" maxlength="8" /></td>
                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                        Salary Currency:
                    </td>
                    <td style="vertical-align:bottom;text-align:left;width:250px;"><select id="ddlRemInfoSalaryCcy" class="ddl" style="width:200px"></select></td>
                </tr>--%>
                 <%--<tr>
                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                        Beneficiary:
                    </td>
                    <td style="vertical-align:bottom;text-align:left;width:250px;"><span id="spnRemInfoRemBeneficiaryName" class="label2" style="width:85px;"></span></td>
                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                        Relationship:
                    </td>
                    <td style="vertical-align:bottom;text-align:left;width:250px;"><select id="ddlRemInfoRelationship" class="ddl" style="width:200px"></select><label id="mnRelationship" style="color:Red; display:none">*</label></td>
                </tr>--%>
                <tr>
                    <td colspan="4">
                        <table>
                            <tr>
                                <td style="vertical-align:bottom;text-align:left;width:200px;">&nbsp;</td>
                                <td style="vertical-align:bottom;text-align:center;width:200px;">                                                
                                    <input id="btnRemitterInfoCancel" type="button" value="Cancel" style="width:180px;" disabled="disabled" onclick="javascript:agentRem.editSysControl_RemitterInfo()" />
                                </td>
                                <td style="vertical-align:bottom;text-align:center;width:200px;">
                                    <input id="btnRemitterInfoSave" type="button" value="Save" style="width:180px;" disabled="disabled" onclick="javascript:agentRem.saveSysControl_RemitterInfo()" />
                                </td>
                                <td style="vertical-align:bottom;text-align:left;width:200px;">&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                    
                </tr>
                <tr>
                    <td colspan="4">
                        <table cellpadding="0" cellspacing="0" style="FONT-SIZE: 12px; width:800px;">
                            <tr>
                                <td class="table_caption" style="vertical-align:bottom;text-align:left;" colspan = "4">Remitter IDs</td>
                            </tr>
                            <tr>
                                <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                    ID type:
                                </td>
                                <td style="vertical-align:bottom;text-align:left;width:250px;"><select id="ddlRemInfoIDType" class="ddl" style="width:200px" disabled="disabled"></select></td>
                                <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                    ID No:
                                </td>
                                <td style="vertical-align:bottom;text-align:left;width:250px;"><input id="txtRemInfoIDNo" type="text" style="width:200px;" maxlength="50" disabled="disabled"/></td>
                            </tr>
                            <tr>
                                <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                    Expiry Date:
                                </td>
                                <td style="vertical-align:bottom;text-align:left;width:250px;"><input id="txtRemInfoExpiryDate" type="text" style="width:200px;" maxlength="50" readonly="readonly" />&nbsp;
                                <img id="imgIDExpiryDate" src="../images/calendar/imgcalen.png" alt="" align="middle" onclick="popUpCalendar(this, document.forms[0].txtRemInfoExpiryDate, 'mm/dd/yyyy',document.getElementById('divRemitterInfo'));" disabled="disabled"/></td>
                                <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                    ID Classification:
                                </td>
                                <td style="vertical-align:bottom;text-align:left;width:250px;"><select id="ddlRemInfoIDClass" class="ddl" style="width:200px" disabled="disabled"></select></td>
                            </tr>
                            <tr>
                                <td colspan = "4"><div id="divGridIds"></div></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table>
                            <tr>
                                <td style="vertical-align:bottom;text-align:center;width:200px;"><input id="btnRemInfoAddID" type="button" value="Add" style="width:150px;" onclick="javascript:agentRem.addSystemControl_RemitterIDs()" /></td>
                                <td style="vertical-align:bottom;text-align:center;width:200px;"><input id="btnRemInfoEditID" type="button" value="Edit" style="width:150px;" disabled="disabled" onclick="javascript:agentRem.editSystemControl_RemitterIDs()"/></td>
                                <td style="vertical-align:bottom;text-align:center;width:200px;"><input id="btnRemInfoSaveID" type="button" value="Save" style="width:150px;" disabled="disabled" onclick="javascript:agentRem.saveSystemControl_RemitterIDs()"/></td>
                                <td style="vertical-align:bottom;text-align:center;width:200px;"><input id="btnRemInfoCancelID" type="button" value="Cancel" style="width:150px;" disabled="disabled" onclick="javascript:agentRem.cancelSystemControl_RemitterIDs()"/></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form> 
</body>
</html>
