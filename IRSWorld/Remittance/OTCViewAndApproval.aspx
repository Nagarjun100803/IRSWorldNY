<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OTCViewAndApproval.aspx.cs" Inherits="IRSWorld.Remittance.OTCViewAndApproval"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>OTC Remittance [Template]</title>
    <link href="../css/Default_min.css" rel="stylesheet" type="text/css" />
    <link href="../css/cssWeb_min.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .idtab_unselected A { width:100%; color:Gray; text-decoration: none; } 
        .idtab_unselected A:hover { width:100%;  color:Gray; text-decoration: underline; }
        .idtab_unselected A:visited { width:100%;  color: Gray; text-decoration: none; }
        .idtab_selected A { width:100%;  color:#135f94; text-decoration: none; }
        .idtab_selected A:hover { width:100%;  color:5697c1; text-decoration: underline; }
        .idtab_selected A:visited { width:100%;  color:#135f94; text-decoration: none; }

        .idtab_unselected 
        {
            width: 120px; 
            height: 100%;
            background-color: Silver;
            color: Gray;
            font-family: Arial; font-weight: bold; font-size: 12px; 
            border-collapse: collapse;
            cursor: hand; 
            border: solid 2px Gray;
            padding-left: 5px;      
        }
        .idtab_selected
        {
            width: 120px; 
            height: 100%;
            background-color: Lavender;
            color: AliceBlue; 
            font-family: Arial; font-weight: bold; font-size: 12px; 
            border-collapse: collapse;
            cursor: hand;  
            border: solid 2px #5697c1;     
            padding-left: 5px;      
        }    
    </style>
    <script src="../js/jsCommons_min.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod_min.js" type="text/javascript"></script>
    <script src="../js/jshttprequest_min.js" type="text/javascript"></script>
    <script src="../js/jsgridselector_min.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow_min.js" type="text/javascript"></script>
    <script src="../js/Template/jsOTCTemplate_min.js" type="text/javascript"></script>
    <script src="../js/Remittance/jsOTCandViewTrans_min.js" type="text/javascript"></script>
    <script src = "../js/Remittance/jsremsaving_min.js"         type = "text/javascript"></script>
    <script src = "../js/Remittance/jsRemExternals_min.js"            type = "text/javascript"></script>
    <script src = "../js/js_sidebar_min.js"            type = "text/javascript"></script>
    <script src="../js/popcalendar_min.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        var OTCView = new OTCView();
        var oreceipt = new online_recept();
        function fix_height()
        {
            if (top.document.getElementById("divCenter"))
            {
                var cent = top.document.getElementById("divCenter");
                var div = document.getElementById("divmain");
                if (div)
                {
                    div.style.width = cent.offsetWidth + "px";
                    div.style.height = (parseInt(cent.offsetHeight) - 30) + "px";
                    var tdrem = document.getElementById("td_content");
                    var divrem = document.getElementById("div_content");
                    var tdmen = document.getElementById("td_remittance");
                    if (divrem && tdmen)
                    {
                        tdrem.style.height = div.style.height
                        tdrem.style.width = (cent.offsetWidth - tdmen.offsetWidth - 10) + "px";
                        divrem.style.height = tdrem.style.height;
                        divrem.style.width = tdrem.style.width;
                    }
                }
            }
        }
        
        function show_idtab(n)
        {
            if (n==1)
            {
                document.getElementById('divsecid').style.display = 'none';
                document.getElementById('asecid').className = 'idtab_unselected';
                document.getElementById('divprid').style.display = '';
                document.getElementById('aprid').className = 'idtab_selected';
            }
            else
            {
                document.getElementById('divsecid').style.display = '';
                document.getElementById('asecid').className = 'idtab_selected';
                document.getElementById('divprid').style.display = 'none';
                document.getElementById('aprid').className = 'idtab_unselected';
            }
        }
        
        function show_idtab_obps(n)
        {
            if (n==1)
            {
                document.getElementById('divsecid_obps').style.display = 'none';
                document.getElementById('asecid_obps').className = 'idtab_unselected';
                document.getElementById('divprid_obps').style.display = '';
                document.getElementById('aprid_obps').className = 'idtab_selected';
            }
            else
            {
                document.getElementById('divsecid_obps').style.display = '';
                document.getElementById('asecid_obps').className = 'idtab_selected';
                document.getElementById('divprid_obps').style.display = 'none';
                document.getElementById('aprid_obps').className = 'idtab_unselected';
            }
        }        
    </script>
</head>
<body onload="fix_height(); javascript:OTCView.load_remittance_transaction();" onresize="fix_height();">
    <form id="form1" runat="server">
    <div id=divmain style="width:100%; height: 100%; padding-top: 30px;">
    <div id="divHelp" style="display:none;position:absolute;border-style:solid; ; background-color:white;padding:5px;width:200px; left: 928px; top: 39px;"><span id="spnHelp" class="label2"></span></div>
    <div style="width:100%">
        <table style="width:100%" cellpadding="0" cellspacing="5">
            <tr>
                <td  id="td_remittance" style=" vertical-align:top; width: 18%;">
                    <div id="divmenu" style="width:100%; display: block;"></div>
                        <a id="anchor_off" href="#" onclick="javascript:OTCView.load_unload_sidebar();">«</a>
                    
                </td>
                <td id="td_content" style="width: 82%px; overflow:auto;">
                    <div id="div_content" style="width:100%; overflow:auto;">
                    <div id="div_remittance" style="vertical-align:top; display:none; width: 97%; "  >
                        <table style="width:100%" cellpadding="0" cellspacing="5">
                            <tr>
                                <td align="center" style="vertical-align:top; width:100%; height:100%;">
                                    <div id="divTransaction" style="width:100%;display:block;">
                                        <table cellpadding="0" cellspacing="0" style="width:100%">
                                            <tr>
                                            <td style="width:60%; vertical-align:bottom" align=left>
                                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left" valign="bottom" style="width:0%; border-collapse:collapse; height: 24px;"><img src="../images/toe.GIF" /></td>    
                                                <td  id="tdDesc" class="table_head_dark" style="width: 40%; height: 24px;">
                                                    REMITTANCE INFORMATION</td>
                                                <td align="left" style="width: 60%; height: 24px;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>
                                            </tr>
                                            </table>
                                            </td>
                                            </tr>
                                            <tr>
                                                <td> <!--colspan="3">-->
                                          
                                                    <table id = "tbl_content_container" class="table_border" style="width:100%;height:210px">
                                                        <tr>
                                                            <td style="height: 70%">
                                                                <center>
                                                                    <table cellpadding = "0"; cellspacing = "1" style = "width:100%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid" id="TABLE1">
                                                                    <tr>
                                                                        <td class = "table_caption" colspan="2" style ="text-align:center">
                                                                            <span id="spnExchangeRate" class="label2">Transaction's Exchange Rate used: </span>
                                                                            <span id="spnExchangeRateValue" class="label2"> </span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: left; height: 100px;" colspan="2">
                                                                            <table cellpadding = "0"; cellspacing = "1" style = "width:100%" id="Table2">
                                                                                <tr style="height: 19px">
                                                                                    <td class = "cell_caption" style="width: 20%; text-align: right">Reference Number :</td>
                                                                                    <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "spnrefno"></label></td>
                                                                                    
                                                                                    <td class = "cell_caption" style="width: 20%; text-align: right">Transaction Status :</td>
                                                                                    <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "spnstatus"></label></td>
                                                                                </tr>
                                                                                <tr style="height: 19px">
                                                                                    <td class = "cell_caption" style="width: 20%; text-align: right">Net Remitting Amount :</td>
                                                                                    <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "spnremittingamount"></label></td>
                                                                                    
                                                                                    <td class = "cell_caption" style="width: 20%; text-align: right">Computation Type :</td>
                                                                                    <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "spncomptype"></label></td>
                                                                                </tr>
                                                                                <tr style="height: 19px">
                                                                                    <td class = "cell_caption" style="width: 20%; text-align: right">Gross Remitting Amount :</td>
                                                                                    <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "spngrossamount"></label></td>                                                                            
                                                                                    <td class = "cell_caption" style="width: 20%; text-align: right">Mode of Transaction :</td>
                                                                                    <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "spnProductCode"></label></td>
                                                                                </tr>
                                                                           
                                                                                <tr style ="height: 19px">
                                                                                    <td class = "cell_caption" style="text-align: right; width: 20%; height: 19px;">Transaction Charge :</td>
                                                                                    <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "spnCharge"></label></td>                                                                            
                                                                                    <td class = "cell_caption" style="width: 20%; text-align: right; height: 19px;">Purpose :</td>
                                                                                    <td class = "cell_desc" style="width: 30%; text-align: left; height: 19px;"><label id = "lbl_purpose"></label></td>
                                                                                </tr>
                                                                                <tr style ="height: 19px">
                                                                                    <td class = "cell_caption" style="text-align: right; width: 20%;">Amount To Be Received :</td>
                                                                                    <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "spnBenefAmount"></label></td>                                                                            
                                                                                    <td class = "cell_caption" style="width: 20%; text-align: right">Source of Fund :</td>
                                                                                    <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "lbl_source_of_fund"></label></td>
                                                                                
                                                                                </tr>
                                                                                <tr style ="height: 19px">
                                                                                    <td class = "cell_caption" style="text-align: right; width: 20%;">Peso Equivalent :</td>
                                                                                    <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "spnPHPAmount"></label></td>                                                                            
                                                                                    <td class = "cell_caption" style="width: 20%; text-align: right">Payment Method :</td>
                                                                                    <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "spnPaymentMethod"></label></td>
                                                                                </tr>
                                                                                <tr style ="height: 19px">
                                                                                    <td class = "cell_desc" style="text-align: right; width: 20%;"></td>
                                                                                    <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "Label1"></label></td>                                                                            
                                                                                    <td class = "cell_caption" style="width: 20%; text-align: right">Transaction Source :</td>
                                                                                    <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "lbl_Tranx_Source"></label></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style ="height: 19px">
                                                                        <td class = "cell_caption" style="text-align: right; width: 18%;"> Message :</td>
                                                                        <td class = "cell_desc" style="text-align: left; width: 70%;"><input  id="txtSpecialMessage" type="text" style="width: 99%"   readonly="readonly" /></td>
                                                                    </tr>
                                                           
                                                                    </table>
                                                                </center>
                                                            </td>
                                                         </tr>
                                                        <tr>
                                                            <td style="vertical-align: top">
                                                                <div>
                                                                    <table id = "tbl_left_and_right" cellpadding = "0" cellspacing = "2" style =" height: 98%; width:100%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">
                                                                        <tr>
                                                                            <td style="width: 50%; height: 100%; vertical-align:top">
                                                                                    <table cellpadding = "0"; cellspacing = "1" style = "width:100%;" id="tblRemitter">
                                                                                        <tr><td class = "table_caption" colspan="2" style ="text-align:center">REMITTER</td></tr>
                                                                                        <tr style ="height: 19px">
                                                                                            <td class = "cell_caption" style="text-align: right; width: 40%;">Remitter Name :</td>
                                                                                            <td class = "cell_desc" style="text-align: left; width: 60%;"><label id="spnRemitterName"></label></td>
                                                                                                
                                                                                        </tr>
                                                                                        <tr style ="height: 19px">
                                                                                            <td class = "cell_caption" style="text-align: right; width: 40%;">Address :</td>
                                                                                            <td class = "cell_desc" style="text-align: left; width: 60%;"><label id="spnRemitterAddress"></label></td>
                                                                                        </tr>
                                                                                        <tr style ="height: 19px">
                                                                                            <td class = "cell_caption" style="text-align: right; width: 40%; height: 16px;">Country :</td>
                                                                                            <td class = "cell_desc" style="text-align: left; width: 60%; height: 16px;"><label id="spnRemitterCountry"></label></td>
                                                                                        </tr>
                                                                                        <tr style ="height: 19px">
                                                                                            <td class = "cell_caption" style="text-align: right; width: 40%;">Zip Code :</td>
                                                                                            <td class = "cell_desc" style="text-align: left; width: 60%;"><label id="spnRemitterZipCode"></label></td>
                                                                                        </tr>
                                                                                        <tr><td colspan="2" style="width:100%; height: 10Px;"></td></tr>
                                                                                        <tr>
                                                                                            <td colspan="2" style="width:100%">
                                                                                                <table style="width:100%" cellspacing="0" cellpadding="0">
                                                                                                    <tr style="background-color:Silver;">
                                                                                                        <td id=aprid class=idtab_selected style="width:33%;"><a href="javascript:show_idtab(1);">Primary ID</a></td>
                                                                                                        <td id=asecid class=idtab_unselected style="width:33%;"><a href="javascript:show_idtab(2);">Secondary ID</a></td>
                                                                                                        <td style="width: 34%;">&nbsp;</td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td colspan=3 style="width:100%">
                                                                                                            <div id=divprid style="width:100%">
                                                                                                                <table style="width:100%" cellpadding=0 cellspacing=0>
                                                                                                                    <tr style ="height: 19px">
                                                                                                                        <td class = "cell_caption" style="text-align: right; width: 40%; height: 16px;">ID Number :</td>
                                                                                                                        <td class = "cell_desc" style="text-align: left; width: 60%; height: 16px;"><label id="spnPrimIdNo"></label></td>
                                                                                                                    </tr>
                                                                                                                    <tr style ="height: 19px">
                                                                                                                        <td class = "cell_caption" style="text-align: right; width: 40%; height: 16px;">ID Type :</td>
                                                                                                                        <td class = "cell_desc" style="text-align: left; width: 60%; height: 16px;"><label id="spnPrimIdType"></label></td>
                                                                                                                    </tr>
                                                                                                                    <tr style ="height: 19px">
                                                                                                                        <td class = "cell_caption" style="text-align: right; width: 40%; height: 16px;">ID Classification :</td>
                                                                                                                        <td class = "cell_desc" style="text-align: left; width: 60%; height: 16px;"><label id="spnPrimIdClass"></label></td>
                                                                                                                    </tr>
                                                                                                                    <tr style ="height: 19px">
                                                                                                                        <td class = "cell_caption" style="text-align: right; width: 40%; height: 16px;">Expiry Date :</td>
                                                                                                                        <td class = "cell_desc" style="text-align: left; width: 60%; height: 16px;"><label id="spnPrimExpDte"></label></td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </div>
                                                                                                            <div id=divsecid style="width:100%;display:none;">
                                                                                                                <table style="width:100%" cellpadding=0 cellspacing=0>
                                                                                                                    <tr style ="height: 19px">
                                                                                                                        <td class = "cell_caption" style="text-align: right; width: 40%; height: 16px;">ID Number :</td>
                                                                                                                        <td class = "cell_desc" style="text-align: left; width: 60%; height: 16px;"><label id="spnSecIdNo"></label></td>
                                                                                                                    </tr>
                                                                                                                    <tr style ="height: 19px">
                                                                                                                        <td class = "cell_caption" style="text-align: right; width: 40%; height: 16px;">ID Type :</td>
                                                                                                                        <td class = "cell_desc" style="text-align: left; width: 60%; height: 16px;"><label id="spnSecIdType"></label></td>
                                                                                                                    </tr>
                                                                                                                    <tr style ="height: 19px">
                                                                                                                        <td class = "cell_caption" style="text-align: right; width: 40%; height: 16px;">ID Classification :</td>
                                                                                                                        <td class = "cell_desc" style="text-align: left; width: 60%; height: 16px;"><label id="spnSecIdClass"></label></td>
                                                                                                                    </tr>
                                                                                                                    <tr style ="height: 19px">
                                                                                                                        <td class = "cell_caption" style="text-align: right; width: 40%; height: 16px;">Expiry Date :</td>
                                                                                                                        <td class = "cell_desc" style="text-align: left; width: 60%; height: 16px;"><label id="spnSecExpDte"></label></td>
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
                                                                            <td style="width: 50%; height: 100%; vertical-align:top">                                                        
                                                                                    <table cellpadding = "0"; cellspacing = "1" style = "width:100%;" id="tbl_benef">
                                                                                        <tr>
                                                                                            <td class = "table_caption" colspan="2" style="text-align:center; height: 26px;">BENEFICIARY</td>
                                                                                        </tr>
                                                                                        <tr style ="height: 19px">
                                                                                            <td class = "cell_caption" style="text-align: right; width: 40%;">Beneficiary &nbsp; :</td>
                                                                                            <td class = "cell_desc" style="text-align: left; width: 60%;"><label id = "spnBeneficiaryName"></label></td>                                                                    
                                                                                        </tr>
                                                                                        <tr style ="height: 19px">
                                                                                            <td class = "cell_caption" style="text-align: right; width: 40%;">Address :</td>
                                                                                            <td class = "cell_desc" style="text-align: left; width: 60%;"><label id = "spnBeneficiaryAddress"></label></td>
                                                                                        </tr>
                                                                                        <tr style ="height: 19px">
                                                                                            <td class = "cell_caption" style="text-align: right; width: 40%; height: 16px;">Country :</td>
                                                                                            <td class = "cell_desc" style="text-align: left; width: 60%; height: 16px;"><label id = "spnBeneficiaryCountry"></label></td>
                                                                                        </tr>
                                                                                        <tr style ="height: 19px">
                                                                                            <td class = "cell_caption" style="text-align: right; width: 40%; height: 16px;">Zip Code :</td>
                                                                                            <td class = "cell_desc" style="text-align: left; width: 60%"><label id = "spnBeneficiaryZipCode"></label></td>
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
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_obps"  style="vertical-align:top; display:none; width:97%"  >
                        <table style="width:100%" cellpadding="0" cellspacing="5" >
                        <tr>
                            <td style="vertical-align:top; height: 100%;"><!--menu-->
                            
                            </td>
                            <td align="center" style="vertical-align:top; width:100%; height:100%;">
                                <div id="div5" style="display:block;">
                                    <table cellpadding="0" cellspacing="0" style="width:750px">
                                        <tr>
                                        <td style="width:60%; vertical-align:bottom; height: 54px;" align=left>
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                            <td align="left" valign="bottom" style="width:0%; border-collapse:collapse; height: 24px;"><img src="../images/toe.GIF" /></td>    
                                            <td id="tdOBPSDesc" class="table_head_dark" style="width: 40%; height: 24px;">
                                            OBPS INFORMATION</td>
                                            <td align="left" style="width: 60%; height: 24px;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>
                                        </tr>
                                        </table>
                                        </td>
                                        </tr>
                               
                                        <tr>
                                            <td> <!--colspan="3"-->
                                          
                                                <table id = "Table5" class="table_border" style="width:100%;height:50%">
                                                    <tr>
                                                        <td style="height: 30%">
                                                            <center>
                                                            <table cellpadding="0"; cellspacing="1"  id="TABLE6" style="width:100%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">
                                                                    <tr>
                                                                        <td class = "table_caption" colspan="2" style ="text-align:center">
                                                                             <span id="Span1" class="label2">Transaction's Exchange Rate used: </span>
                                                                             <span id="spnOBPSrateused" class="label2"> </span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: left; height: 33px;" colspan="2">
                                                                            <table cellpadding = "0"; cellspacing = "1" style = "width:100%" id="Table7">
                                                                                 <tr style="height: 19px">
                                                                                    <td class = "cell_caption" style="width: 20%; text-align: right">Reference Number :</td>
                                                                                    <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "spnOBPSrefno"></label></td>
                                                                                        
                                                                                    <td class = "cell_caption" style="width: 20%; text-align: right">Transaction Status :</td>
                                                                                    <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "spnOBPStranstatus"></label></td>
                                                                                
                                                                                </tr>
                                                                                <tr style="height: 19px">
                                                                                    <td class = "cell_caption" style="width: 20%; text-align: right; height: 23px;">Net Remitting Amount :</td>
                                                                                    <td class = "cell_desc" style="width: 30%; height: 23px; text-align: left"><label id = "spnOBPSremittingamount"></></td>
                                                                                        
                                                                                    <td class = "cell_caption" style="width: 20%; text-align: right; height: 23px;">Computation Type  :</td>
                                                                                    <td class = "cell_desc" style="width: 30%; text-align: left; height: 23px;"><label id = "spnOBPScomptype"></label></td>
                                                                                </tr>
                                                                                <tr style="height: 19px">
                                                                                    <td class = "cell_caption" style="width: 20%; text-align: right">Gross Remitting Amount :</td>
                                                                                    <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "spnOBPSgrossamount"></></td>
                                                                                        
                                                                                    <td class = "cell_caption" style="width: 20%; text-align: right">Payment Method :</td>
                                                                                    <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "spnOBPSPaymentMethod"></label></td>
                                                                                </tr>
                                                                                 <tr style="height: 19px">
                                                                                    <td class = "cell_caption" style="width: 20%; text-align: right">Transaction Charge  :</td>
                                                                                    <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "spnOBPScharge"></></td>
                                                                                        
                                                                                    <td class = "cell_caption" style="width: 20%; text-align: right">Mode of Implementation:</td>
                                                                                    <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "spnOBPSmodeoftrans"></label></td>
                                                                                </tr>
                                                                              </table>
                                                                         </td>
                                                                     </tr>
                                                                      
                                                                     <tr style ="height: 19px">
                                                                                    <td class = "cell_caption" style="text-align: right; width: 18%;">Amount to be received (Merchant) :</td>
                                                                                    <td class = "cell_desc" style="text-align: left; width: 70%;">
                                                                                        <span id="spnOBPSamounttobereceived" style="width:50%;text-align:right;" class="label2"></span>
                                                                                    </td>
                                                                     </tr>
                                                            </table>
                                                            </center>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="vertical-align: top">
                                                            <div>
                                                                <table id = "tblremittermerchant" cellpadding = "0" cellspacing = "2" style =" height: 98%; width:100%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;">
                                                                    <tr>
                                                                        <td style="width: 50%; vertical-align:top">
                                                                            <table cellpadding = "0"; cellspacing = "1" style = "width:100%;" id="tblOBPSRemitter">
                                                                         
                                                                                <tr>
                                                                                    <td class = "table_caption" colspan="2" style ="text-align:center">REMITTER</td>
                                                                                </tr>
                                                                                <tr style ="height: 19px">
                                                                                    <td class = "cell_caption" style="text-align: right; width: 34%;">Remitter Name :</td>
                                                                                    <td class = "cell_desc" style="text-align: left; width: 64%;"><input type=text style="border:none; width: 100%;"; readonly="readonly"; id="spnOBPSremittername" class = "cell_desc"/></td>
                                                                                </tr>
                                                                                <tr style ="height: 19px">
                                                                                    <td class = "cell_caption" style="text-align: right; width: 34%;">Address :</td>
                                                                                    <td class = "cell_desc" style="text-align: left; width: 64%;"><input type=text style="border:none; width: 100%;"; readonly="readonly"; id = "spnOBPSremitteraddress" class = "cell_desc"/></td>
                                                                                </tr>
                                                                                <tr style ="height: 19px">
                                                                                    <td class = "cell_caption" style="text-align: right; width: 34%; height: 16px;">Country :</td>
                                                                                    <td class = "cell_desc" style="text-align: left; width: 64%; height: 16px;"><input  type=text style="border:none; width: 100%;"; readonly="readonly"; id="spbOBPSremittercountry"/></td>
                                                                                </tr>
                                                                                <tr style ="height: 19px">
                                                                                    <td class = "cell_caption" style="text-align: right; width: 34%; height: 19px;">Zip Code :</td>
                                                                                    <td class = "cell_desc" style="text-align: left; width: 64%; height: 19px;"><input type=text style="border:none; width: 100%;"; readonly="readonly"; id="spnOBPSremitterzipcode"/></td>
                                                                                </tr>
                                                                                <!-- RFS 13311-002 -->
                                                                                <tr><td colspan="2" style="width:100%; height: 10Px;"></td></tr>
                                                                                <tr>
                                                                                    <td colspan="2" style="width:100%">
                                                                                        <table style="width:100%" cellspacing="0" cellpadding="0">
                                                                                            <tr style="background-color:Silver;">
                                                                                                <td id=aprid_obps class=idtab_selected style="width:33%;"><a href="javascript:show_idtab_obps(1);">Primary ID</a></td>
                                                                                                <td id=asecid_obps class=idtab_unselected style="width:33%;"><a href="javascript:show_idtab_obps(2);">Secondary ID</a></td>
                                                                                                <td style="width: 34%;">&nbsp;</td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan=3 style="width:100%">
                                                                                                    <div id=divprid_obps style="width:100%">
                                                                                                        <table style="width:100%" cellpadding=0 cellspacing=0>
                                                                                                            <tr style ="height: 19px">
                                                                                                                <td class = "cell_caption" style="text-align: right; width: 40%; height: 16px;">ID Number :</td>
                                                                                                                <td class = "cell_desc" style="text-align: left; width: 60%; height: 16px;"><label id="spnPrimIdNo_obps"></label></td>
                                                                                                            </tr>
                                                                                                            <tr style ="height: 19px">
                                                                                                                <td class = "cell_caption" style="text-align: right; width: 40%; height: 16px;">ID Type :</td>
                                                                                                                <td class = "cell_desc" style="text-align: left; width: 60%; height: 16px;"><label id="spnPrimIdType_obps"></label></td>
                                                                                                            </tr>
                                                                                                            <tr style ="height: 19px">
                                                                                                                <td class = "cell_caption" style="text-align: right; width: 40%; height: 16px;">ID Classification :</td>
                                                                                                                <td class = "cell_desc" style="text-align: left; width: 60%; height: 16px;"><label id="spnPrimIdClass_obps"></label></td>
                                                                                                            </tr>
                                                                                                            <tr style ="height: 19px">
                                                                                                                <td class = "cell_caption" style="text-align: right; width: 40%; height: 16px;">Expiry Date :</td>
                                                                                                                <td class = "cell_desc" style="text-align: left; width: 60%; height: 16px;"><label id="spnPrimExpDte_obps"></label></td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </div>
                                                                                                    <div id=divsecid_obps style="width:100%;display:none;">
                                                                                                        <table style="width:100%" cellpadding=0 cellspacing=0>
                                                                                                            <tr style ="height: 19px">
                                                                                                                <td class = "cell_caption" style="text-align: right; width: 40%; height: 16px;">ID Number :</td>
                                                                                                                <td class = "cell_desc" style="text-align: left; width: 60%; height: 16px;"><label id="spnSecIdNo_obps"></label></td>
                                                                                                            </tr>
                                                                                                            <tr style ="height: 19px">
                                                                                                                <td class = "cell_caption" style="text-align: right; width: 40%; height: 16px;">ID Type :</td>
                                                                                                                <td class = "cell_desc" style="text-align: left; width: 60%; height: 16px;"><label id="spnSecIdType_obps"></label></td>
                                                                                                            </tr>
                                                                                                            <tr style ="height: 19px">
                                                                                                                <td class = "cell_caption" style="text-align: right; width: 40%; height: 16px;">ID Classification :</td>
                                                                                                                <td class = "cell_desc" style="text-align: left; width: 60%; height: 16px;"><label id="spnSecIdClass_obps"></label></td>
                                                                                                            </tr>
                                                                                                            <tr style ="height: 19px">
                                                                                                                <td class = "cell_caption" style="text-align: right; width: 40%; height: 16px;">Expiry Date :</td>
                                                                                                                <td class = "cell_desc" style="text-align: left; width: 60%; height: 16px;"><label id="spnSecExpDte_obps"></label></td>
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
                                                                        <td style="width: 50%; vertical-align:top">                                                        
                                                                            <table cellpadding = "0"; cellspacing = "1" style = "width:100%;" id="TrnxDetails">
                                                                                <tr>
                                                                                    <td class = "table_caption" colspan="2" style="text-align:center; height: 26px;">MERCHANT</td>
                                                                                </tr>
                                                                                <tr style ="height: 19px">
                                                                                    <td class = "cell_caption" style="text-align: right; width: 35%;">Merchant Name &nbsp; :</td>
                                                                                    <td class = "cell_desc" style="text-align: left; width: 70%;"><input type=text style="border:none; width: 100%;"; readonly="readonly"; id="lbl_merchant_name" /></td>                                                                    
                                                                                </tr>
                                                                                <tr style ="height: 19px">
                                                                                    <td class = "cell_caption" style="text-align: right; width: 35%; height: 19px;">Account Currency :</td>
                                                                                    <td class = "cell_desc" style="text-align: left; width: 70%; height: 19px;"><input type=text style="border:none; width: 100%;"; readonly="readonly"; id="lbl_merchant_currency" /></td>
                                                                                </tr>
                                                                                <tr style ="height: 19px">
                                                                                    <td class = "cell_caption" style="text-align: right; width: 35%; height: 16px;">
                                                                                        Merchant Prod. Code :</td>
                                                                                    <td class = "cell_desc" style="text-align: left; width: 70%; height: 16px;"><input style="border:none; width: 100%;"; readonly="readonly"; id="lbl_merchant_prodcode"/></td>
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
                            
                            
                            </td>
                        </tr>
                        </table>
                    </div>
                    <div id="divClearingDate"  style="width:50%; z-index:101;position:absolute;left:1009px;color:darkblue;border:solid 2px #000;padding:10px;display:none; font-family: Arial; top: 765px;width:341px; height:150px;">
                        <table cellpadding = "0"; cellspacing = "1" style = " height:99%;width:99%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid" id="Table3">
                        <tr>
                        <td>
                        
                        <table>
                            
                            <tr>
                                <td style="height: 42px">
                                    <table align="center" style="width:100%; height: 99%">
                                        <tr>
                                            <td style="width:250px;height:21px;" class="cell_caption">
                                                <a>Old Clearing Date</a>
                                            </td>
                                            <td style="width:450px; height: 21px;" align="left">
                                                <span id="txtClearingDate" style="width:50%;text-align:right;" class="label2"></span>
                                               
                                               
                                            </td>
                                        </tr>
                                         
                                        <tr>
                                            <td style="width:250px;height:21px;" class="cell_caption">
                                                <a>New Clearing Date</a>
                                            </td>
                                            <td style="width:450px; height: 21px;" align="left">
                                               <input  maxlength="10" id="txtNewClearingDate" type="text" style="width: 85%; height: 20px" disabled="disabled"  />
                                                <img id="img2" src="../images/calendar/imgcalen.png" onclick="popUpCalendar(this, document.forms[0].txtNewClearingDate, 'mm/dd/yyyy');"/>
                                            </td>
                                        </tr>
                                       
                                    </table>
                                </td>
                              
                            </tr>
                            <tr>
                                <td align="center">
                                    <input id="btn_changeclearingdate"  class="button" onclick="javascript:OTCView.apply_changes_clearingdate();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Apply Changes" style="width:137px" /> 
                                    <input id="btn_back"  class="button" onclick="javascript:OTCView.hide_clearingdate();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Back" style="width:137px" /> 
                                </td>
                            </tr>
                        </table>
                        </td>
                         </tr>
                         </table>
                    </div >
                     <div id="div_showcharges" style="width:50%; z-index:101;position:absolute;left:678px;color:darkblue;border:solid 2px #000;padding:10px;display:none; font-family: Arial; top: 1019px;width:290px;height:84px;">
                        <table >
                            <tr>
                                <td  align="center" style="width: 280px; font-weight: bold; font-size: 14px;  font-family: Arial; height: 3px;"  >
                                    <a >Charges Summary</a>
                                </td>
                            </tr>
                            <tr style=" border:border-style">
                                <td style="height: 11px; width: 280px;">
                                    <table id="tblcharges" border="0" align="center" style="width:100%; height: 11px">
                                        
                                    </table>
                                </td>
                              
                            </tr>
                            
                            <tr>
                                <td align="center" style="height: 11px; width: 280px;">
                                    <input id="Button4"  class="button" onclick="javascript:OTCView.hide_charges();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Back" style="width:112px" /> 
                                </td>
                            </tr>
                        </table>
                    </div >
                     <div id="divoverride" style="width:70%; z-index:101;left:-6px;color:darkblue;border:solid 2px #000;padding:10px;display:none; font-family: Arial; top: 830px;width:346px;height:304px;">
                           <table style="width:100%" align="center">
                                <tr>
                                    <td>
                                        <div id="divrates"> </div>
                                    </td>
                                </tr>
                                 <tr>
                                <td align="center" style="height: 11px; width: 280px;">
                                    <input id="Button1"  class="button" onclick="javascript:hidePanel('divoverride');" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Back" style="width:112px" /> 
                                </td>
                            </tr>
                           </table>
                    </div >
                    <div style="width:97%" align="center">
                        <table style="width:100%"   align="center">
                             <tr  align="center">
                             <td style="height: 24px" > </td>
                            <td align="center" valign="bottom" style="width:100% ; height: 24px;">
                                <input id="btnremit"  class="button" onclick="javascript:OTCView.remit();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Send Money Now" style="width:150px" visible="false" /> 
                               
                            </td>
                           </tr>
                        </table>
                    </div>
                    <div id="divHidden" >
                        <input id="lblBeneficiaryAcctSeqID" type="hidden" />
                        <input id="lblBeneficiaryID" type="hidden" />
                        <input id="lblCIFClass" type="hidden" />
                        <input id="lblPaymentMethod" type="hidden" />
                        <input id="lblRemitterAcctSeqID" type="hidden" />
                        <input id="lblRemitterID" type="hidden" />
                        <input id="lblSourceForex" type="hidden" />
                        <input id="lblTargetForex" type="hidden" />
                    </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </div>
    </form>
</body>
</html>
