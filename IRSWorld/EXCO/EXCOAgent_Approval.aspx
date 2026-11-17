<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EXCOAgent_Approval.aspx.cs" Inherits="IRSWorld.EXCO.EXCOAgent_Approval" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>MIDDLE EAST FOR ARRPOVAL</title>
    
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />    
    <script src="../js/jsCommons.js"                        type="text/javascript"> </script>
    <script src="../js/jscommonmethod.js"                   type="text/javascript" > </script>
    <script type="text/javascript" src="../js/Remittance/jsRemExternals.js"> </script>
    <script src="../js/js_generic.js"                       type="text/javascript"></script>
    <script src="../js/jshttprequest.js"                    type="text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"                    type="text/javascript"></script>
    <script src="../js/jsgridselector.js"                   type="text/javascript"  ></script>    
    <script src="../js/EXCO/js_EXCOAgent_Approval.js" type="text/javascript"></script>    
    <script src="../js/Remittance/jsRemExternals.js" type="text/javascript"></script>
    <script type="text/javascript">
    
        function fixdim()
        {
            if (top.document.getElementById('divCenter'))
            {
                var div = document.getElementById('divmainexco');
                
                div.style.height = (top.document.getElementById('divCenter').offsetHeight - 35) + 'px';
                div.style.width = (top.document.getElementById('divCenter').offsetWidth ) + 'px';
            }
        }

    </script>    
    
</head>
<body onload="fixdim(); javascript:js_EXCO.init_state();" onresize="fixdim();">
<center>
    <form id="form1" runat="server">
    <div id="divprintexco" style="width:100%;display:none;"></div>
    <div style="width:100%;display:block; overflow: auto; vertical-align:top; padding-top: 30px" id="divmainexco">
         <table "border="0" style="width: 100%; height: 100%" align="center" >
           <!-- <tr>
                <td style=" width:100%; " align="center">&nbsp;</td>
                </tr> -->
            <tr>
                <td style=" width:100%; vertical-align:top" align="center">
                    
                    <table cellpadding="0" cellspacing="0" style="background-color: azure; width: 65%;">
                        <tr>
                        <td style="width:60%; vertical-align:bottom; text-align:left">
                        <table style="width:100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="left" style="width: 0%; height: 24px" valign="bottom">
                                <img src="../images/toe.GIF" /></td>
                            <td align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;height: 24px">
                                MIDDLE EAST (FOR APPROVAL)</td>
                            <td align="left" class="bg_body" style="width: 60%; height: 24px" valign="bottom">
                                <img  src="../images/rend.gif" /></td>
                        </tr>
                        </table>
                        </td>
                        </tr>
                        <tr>
                            <td style="width: 100%; height: 100%"> <!-- colspan="3"-->
                                <table class="table_border" style="width: 100%; height: 100%">
                                    <tr>
                                        <td style="vertical-align: top; width: 60%; height: 44%">
                                            
                                            <!-- content starts here -->
                                             <div id="divFileListing" style= "">                                           
                                            <table id="tbl1" style="width: 100%;text-align:center;" class="table_border">
                                                <tr>
                                                    <td class="cell_caption" style="width: 100%; height: 21px;text-align:center;">
                                                        &nbsp;<input type="hidden" id="hdBranch" value= "0" />
                                                        <input type="hidden" id="hdselectedpage" value= "0" />
                                                    <input type="hidden" id="hdselectedfile" value= "0" />
                                                    <input type="hidden" id="hdselectedcurr" value= "0" />
                                                    <input type="hidden" id="hdselectedfiledtls" value= "0" />
                                                    <input type="hidden" id="hdselectedfunds" value= "" />
                                                    
                                                    <input type="hidden" id="hdexcoswitch" value= "false" />
                                                    
                                                        Uploaded Files</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%; height: 21px;" >
                                                      <label class="cell_desc">Select Agent : </label>
                                                            <select id="ddlEXCOBranch" onchange="javascript:js_EXCO.display_file_listing();" style="width:42%"></select>
                                            
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td id="tdEXCOfilelisting" style="width: 100%; height: 21px;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%; height: 21px">
                                                        <input disabled id="btnView" onclick="javascript:js_EXCO.btnView_click();" style="width: 15%" type="button" value="View" />
                                                        <input disabled id="btnViewCancel" type="button" value="Cancel" style="width: 15%" onclick="javascript:js_EXCO.btnViewCancel_click();" /></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%; height: 21px" id="tdswitch">
                                                    </td>
                                                </tr>
                                            </table>
                                            </div>
                                            <div id="divForApproval" style="display:none;">
                                            <table id="tbl2" style="width: 100%;" class="table_border">
                                                <tr>
                                                    <td class="cell_caption" colspan="2" style="height: 21px; text-align: center">
                                                    For Approval
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 33%; height: 21px; text-align: right">
                                                        Agent:</td>
                                                    <td class="cell_desc" style="width: 100%; height: 21px; text-align: left">
                                                    <label id="lblAgent">&nbsp;</label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 33%; height: 21px; text-align: right">
                                                        File Name:</td>
                                                    <td class="cell_desc" style="width: 100%; height: 21px; text-align: left">
                                                    <label id="lblFilename">&nbsp;</label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 33%; height: 21px; text-align: right">
                                                        File Upload Date/Uploaded By:</td>
                                                    <td class="cell_desc" style="width: 100%; height: 21px; text-align: left">
                                                    <label id="lblupload_date_by">&nbsp;</label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td id="tdEXCOForApproval" colspan="2" style="height: 21px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="height: 21px">
                                                        <input id="btnEdit" disabled="disabled" onclick="javascript:js_EXCO.edit_click();"
                                                            style="width: 9%; height: 20px" type="button" value="EDIT" />
                                                        <input id="btnSave" disabled="disabled" onclick="javascript:js_EXCO.save_click();"
                                                            style="width: 9%; height: 20px" type="button" value="SAVE" />
                                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<input id="btnAll" onclick="javascript:js_EXCO.select_for_approval_items(0);" style="width: 15%" type="button" value="ALL CURRENCY" />
                                                        <input id="btnAllPHP" type="button" value="ALL PHP" style="width: 15%" onclick="javascript:js_EXCO.select_for_approval_items(1);" />
                                                        <input id="btnAllUSD" type="button" value="ALL USD" style="width: 15%" onclick="javascript:js_EXCO.select_for_approval_items(2);" /></td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" colspan="2" style="height: 21px;text-align:center;">
                                                    Item Summary
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="height: 21px" id="tdEXCOAmountSummary">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="height: 21px">
                                                       
                                                        <input id="btnCancelApprove" type="button" value="CANCEL" style="width: 15%" onclick="javascript:js_EXCO.btnCancelApprove_click();"/>
                                                        <input id="btnViewFunds" onclick="javascript:js_EXCO.btnViewFunds_click();" style="width: 15%" type="button" value="VIEW FUNDS" /></td>
                                                </tr>
                                            </table>
                                            </div>
                                            <div id="divFundsComputation" style="display:none">
                                                <table id="tblComputeFunds_holder" style="width: 100%;text-align:center;" class="table_border">
                                                <tr>
                                                    <td colspan="2" class="cell_caption" style="width: 100%; height: 21px;text-align:center;">                                                    
                                                        Funds Summary</td>
                                                     
                                                </tr>
                                                 <tr>
                                                    <td class="cell_caption" style="width: 34%; height: 21px;text-align:center;">                                                    
                                                        Rates Used:</td>
                                                        <td class="cell_desc" id ="tdRatesUsed" align="left"></td>
                                                     
                                                </tr>
                                                <tr>
                                                    <td colspan ="2" >
                                                    </td>
                                                </tr>
                                                </table>
                                            </div>
                                            <!-- content ends here -->
                                              <div id="divTransDetail" style="display:none">
                                                <table id="Table1" style="width: 100%;text-align:center;" class="table_border">
                                                <tr>
                                                    <td class="cell_caption" colspan="4" style="height: 21px; text-align: center">
                                                        Transaction Summary</td>
                                                     
                                                </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 197px; height: 13px;">
                                                            System Reference:</td>
                                                        <td id="td0" class="cell_desc" align="left" style="width: 140px; height: 13px">
                                                        </td>
                                                        <td align="left" class="cell_caption" style="width: 167px; height: 13px">
                                                            Transaction Reference:</td>
                                                        <td align="left" class="cell_desc" style="height: 13px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 197px; height: 18px;">
                                                            Transaction Date:</td>
                                                        <td id="td1"  class="cell_desc" align="left" style="width: 140px; height: 18px">
                                                        </td>
                                                        <td align="left" class="cell_caption" style="width: 167px; height: 18px">
                                                        </td>
                                                        <td align="left" class="cell_desc" style="height: 18px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 197px; height: 11px">
                                                            Source Account:</td>
                                                        <td id="td10" align="left" class="cell_desc" style="height: 11px; width: 140px;">
                                                        </td>
                                                        <td align="left" class="cell_caption" style="width: 167px; height: 11px">
                                                            Target Account:</td>
                                                        <td align="left" class="cell_desc" style="height: 11px">
                                                        </td>
                                                    </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 197px; height: 7px">
                                                        Currency:</td>
                                                    <td id="td3" style="height: 7px; width: 140px;" class="cell_desc" align="left">
                                                    </td>
                                                    <td align="left" class="cell_caption" style="width: 167px; height: 7px">
                                                            Amount to Debit:</td>
                                                    <td align="left" class="cell_desc" style="height: 7px">
                                                    </td>
                                                </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 197px">
                                                            Status/Remarks:</td>
                                                        <td id="td4" class="cell_desc" align="left" colspan="3">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <input id="btnPrint" type="button" value="PRINT" style="width: 15%" onclick="javascript:window.print();"/>
                                                            <input id="btnClose" type="button" value="CLOSE" style="width: 15%" onclick="javascript:js_EXCO.btnCancelApprove_click();"/></td>
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
            </tr>
        </table>
    </div>
    </form>
    </center>
</body>
</html>
