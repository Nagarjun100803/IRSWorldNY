<%@ Page Language="C#" AutoEventWireup="true" Codebehind="GRSDataEntry.aspx.cs" Inherits="IRSWorld.GRS.Processing.GRSDataEntry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Batch Remittance Data Entry</title>
    <link href="../../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>

    <script src="../../js/jsCommons.js"                type="text/javascript"></script>
    <script src="../../js/jscommonmethod.js"           type="text/javascript"></script>
    <script src="../../js/js_sidebar.js"               type="text/javascript"></script>
    <script src="../../js/jsmodalwindow.js"            type="text/javascript"></script>
    
    <!-- <script src="../../js/jshttprequest.js" type="text/javascript"></script>-->
    <script src="../../js/jsgridselector.js" type="text/javascript"></script>    
    <script src="../../js/Internet/jsInternetPage.js" type="text/javascript"></script>
    <script src="../../js/GRS/js_GRSDataEntry.js" type="text/javascript"></script>
    
    

</head>
<body onload="javascript:ide.init_state();">
<center>
    <form id="form1" runat="server">
    
        <div style="width: 100%" id="divMainPanel">
            <table cellpadding="0" cellspacing="5" width="100%" height="100%">
                <tr>
                    <td align="center" class="" colspan="2" style="width: 100%; border-collapse: collapse;" valign="top">
                        <div style="display: block; width: 100%; height: 100%">
                            <table width="100%">
                                <tr>
                                    <td style="vertical-align: top;  padding-top: 30px" valign="top">
                                    
                                    <div id="divDataEntryMenu" style="display:block;">                                    
                                     
                         <table  cellpadding="0" cellspacing="5">
                            <tr id="trCreateBatch" style="display:none;">
                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                    <a href="javascript:ide.create_new_batch();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Create Batch</a>
                                </td>
                            </tr>
                            <tr id="trSearchBatch" style="display:none;">
                                <td style="text-align:right;border-bottom:solid 1px #5697c1;height:20px;">
                                    <a href="" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">
                                        Search Batch</a>
                                </td>
                            </tr>
                            <tr id="trSearchCIF" style="display:none;">
                                <td style="text-align:right;border-bottom:solid 1px #5697c1;height:20px;">
                                    <a href="javascript:searchCIF();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Search Client</a>
                                </td>
                            </tr>
                            <tr id="trAddCIF" style="display:none;">
                                <td style="text-align:right;border-bottom:solid 1px #5697c1;height:20px;">
                                    <a href="javascript:show_frame_dialog('../../Internet/InternetRemitter.aspx?isRemit=true', 'internetClose', '100%', '100%', '0');" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Add Client</a>
                                </td>
                            </tr>
                        </table>
                    </div>
                                    
                          
                                    
                                    </td>
                                    <td style=" height:auto; padding-top: 30px" valign="top" align="center">
                                    
                                    <div id="divicreatebatch" style="display:none" >
                                        &nbsp;<input id="txtctr" type="text" style="width: 11%" />
                                        <input id="txttotitems_ctr" type="text" style="width: 11%" />
                                        <table cellpadding="0" cellspacing="0" style="background-color: azure; width: 1%;">
                                                <tr>
                                                    <td nowrap align="left" valign="bottom" style="width:1%;height:24px;">
                                                        <img src="../../images/toe.GIF" /></td>
                                                    <td nowrap align="left" class="table_head_dark" width="20%">
                                                        Create Batch</td>
                                                    <td nowrap align="left" valign="bottom" class="bg_body" style="WIDTH: 400px">
                                                        <img align="absbottom" src="../../images/rend.gif" />&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" style="width: 100%; ">
                                                        <table class="table_border" style="width: 100%; ">
                                                            <tr>
                                                                <td style="width: 100%; height: 8px" align="center">
                                                                    <table id="Table4" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                    border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;width: 81%; ">
                                                                        <tr>
                                                                            <td class="cell_caption" style="width: 17%; height: 23px">
                                                                                File Format :</td>
                                                                            <td align="left" style="width: 29%; height: 23px">
                                                                                <select id="ddFileFormat" style="width: auto; height: 21px">
                                                                                    <option selected="selected"></option>
                                                                                </select>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="cell_caption" style="width: 17%; height: 23px">
                                                                                Rate Type :</td>
                                                                            <td align="left" style="width: auto; height: 23px">
                                                                                <select id="ddlRateType" style="width: auto; ">
                                                                                </select>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="cell_caption" style="width: 17%; height: 23px">
                                                                                No of items :</td>
                                                                            <td align="left" style="width: 29%; height: 23px">
                                                                                <input id="txtbatchitems" type="text" style="width: 59%;text-align:right;" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="cell_caption" style="width: 17%; height: 23px">
                                                                                Total Remitting Amount :</td>
                                                                            <td align="left" style="width: 29%; height: 23px">
                                                                                <input id="txttotremamt" type="text" style="width: 59%;text-align:right;" /></td>
                                                                        </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td  colspan="2" style="height: 30px">
                                                                                    <input id="btnCreateBatch" onclick="javascript:ide.prepare_new_batch();"style="width: 17%" type="button" value="Create" />
                                                                                    <input id="btnCancelCreate" style="width: 17%; height: 20px;" type="button" value="Cancel" onclick="javascript:ide.btnCancelCreate_click()" /></td>
                                                                                </tr>
                                                                            </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>                                  
                                     
                                    
                                    <div id="diviinquiry" style="display:none" >
                                            <table cellpadding="0" cellspacing="0" style="background-color: azure; width: 1%;">
                                                <tr>
                                                    <td nowrap align="left" valign="bottom" style="width:1%;height:24px;">
                                                        <img src="../../images/toe.GIF" /></td>
                                                    <td nowrap align="left" class="table_head_dark" width="20%">
                                                        Search Batch</td>
                                                    <td nowrap align="left" valign="bottom" class="bg_body" style="WIDTH: 400px">
                                                        <img align="absbottom" src="../../images/rend.gif" />&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" style="width: 100%; height: 100%">
                                                        <table class="table_border" style="width: 100%; height: 100%">
                                                            <tr>
                                                                <td style="width: 100%; height: 8px" align="center">
                                                                    <table id="Table3" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                    border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;width: 100%; ">
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 17%; height: 23px">
                                                                                        Batch No :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px">
                                                                                        <input id="Text6" type="text" style="width: 14%" />
                                                                                        <select id="Select3" style="width: 67%; height: 21px">
                                                                                            <option selected="selected"></option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td  colspan="2" style="height: 23px">
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
                                    <div id="divibatchholder" style="display:none;height:100%;overflow:auto ;" >
                                    <table cellpadding="0" cellspacing="0" style="background-color: azure" width="96%">
                                        <tr>
                                            <td nowrap align="left" valign="bottom" style="width:1%;height:24px;">
                                                <img src="../../images/toe.GIF" /></td>
                                            <td nowrap align="left" class="table_head_dark" width="20%">
                                                Batch Information</td>
                                            <td nowrap align="left" valign="bottom" class="bg_body" style="">
                                                <img align="absbottom" src="../../images/rend.gif" />&nbsp;</td>
                                        </tr>
                                        <tr>
                                        <td colspan="3" style="height: 100%">
                                                
                                    <div id="diviDebit" style="display:none" >
                                        <table cellpadding="0" cellspacing="0" style="background-color: azure" width="100%" height="100%">
                                            <tr>
                                                <td colspan="3" style="width: 100%; height: 100%">
                                                    <table class="table_border" style="width: 100%; height: 100%">
                                                        <tr>
                                                            <td style="width: 100%; height: 8px" align="right">
                                                                <table cellpadding="0" cellspacing="5" width="100%">
                                                                    <tr>
                                                                        <td style="width: 100%; height: 100%" valign="middle">
                                                                            <div id="" class="table_body" style="margin: 0px; width: 98%;
                                                                                height: 100%">
                                                                                <table id="tblAPASRemRemitter" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                    border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid"
                                                                                    width="100%">
                                                                                    <tr>
                                                                                        <td align="left" colspan="4" >
                                                                                            
                                                                                            <table style="width: 344px">
                                                                                            <tr>
                                                                                            <td class="cell_caption" style="width: 54px;text-align:center ;">&nbsp;Debit</td>
                                                                                            <td style="width: 54px;text-align:center ;" class="tab_unselected" 
                                                                                                onmouseover="javascript:ide.handle_mouseover('Credit','hoverin',this)"
                                                                                                onclick="javascript:ide.handle_mouseover('Credit','click',this)"
                                                                                                    >&nbsp;Credit</td>
                                                                                            <td style="width: 54px;text-align:center ;" class="tab_unselected" 
                                                                                                onmouseover="javascript:ide.handle_mouseover('Charges','hoverin',this)"
                                                                                                onclick="javascript:ide.handle_mouseover('Charges','click',this)"
                                                                                                    >&nbsp;Charges</td>
                                                                                            <td style="width: 54px;text-align:center ;" class="tab_unselected" 
                                                                                                onmouseover="javascript:ide.handle_mouseover('Summary','hoverin',this)"
                                                                                                onclick="javascript:ide.handle_mouseover('Summary','click',this)"
                                                                                                    >&nbsp;Summary</td>
                                                                                            </tr>
                                                                                            </table>
                                                                                            
                                                                                            </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                            Output Seq. No :</td>
                                                                                        <td align="left" style="width: 31%; height: 23px">
                                                                                            <input id="txtOutSeqno" type="text" style="width: 88%" /></td>
                                                                                        <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                            Rem. Bank :</td>
                                                                                        <td align="left" style="width: 85%; height: 23px">
                                                                                            <select id="ddRemBank" style="width: auto; height: 21px">
                                                                                                <option selected="selected"></option>
                                                                                            </select>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                            Refnum Theirs:&nbsp;</td>
                                                                                        <td align="left" style="width: 31%; height: 23px">
                                                                                            <input id="txtRefnumTheirs" type="text" style="width: 88%" /></td>
                                                                                        <td align="left" style="width: 22%; height: 23px" class="cell_caption">
                                                                                            Prot. Bank:</td>
                                                                                        <td align="left" style="width: 85%; height: 23px">
                                                                                            <select id="ddProtBank" style="width: auto; height: 21px">
                                                                                                <option selected="selected"></option>
                                                                                            </select>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr style="font-size: 13pt">
                                                                                        <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                            Date Sent :&nbsp;</td>
                                                                                        <td align="left" style="width: 31%; height: 23px">
                                                                                            <label id="lblAPASRemAddress" class="cell_desc" style="font-weight: normal; font-size: 15px">
                                                                                            <input id="txtDateSent" type="text" style="width: 88%" /></label></td>
                                                                                        <td align="left" style="width: 22%; height: 23px" class="cell_caption">
                                                                                            Currency Code: &nbsp;</td>
                                                                                        <td align="left" style="width: 85%; height: 23px">
                                                                                            <select id="ddDRRemCurr" style="width: auto; height: 21px">
                                                                                                <option selected="selected"></option>
                                                                                            </select></td>
                                                                                    </tr>
                                                                                    <tr style="font-size: 13pt">
                                                                                        <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                            Date Received :</td>
                                                                                        <td align="left" style="width: 31%; height: 23px" >
                                                                                            <input id="txtDateReceived" type="text" style="width: 88%" /></td>
                                                                                        <td align="left" style="width: 22%; height: 23px" class="cell_caption" >
                                                                                            </td>
                                                                                        <td align="left" style="width: 85%; height: 23px">
                                                                                            </td>
                                                                                    </tr>
                                                                                    <tr style="font-size: 13pt">
                                                                                        <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                            &nbsp;</td>
                                                                                        <td align="left" style="width: 31%; height: 23px">
                                                                                            &nbsp;</td>
                                                                                        <td align="left" style="width: 22%; height: 23px" class="cell_caption" >
                                                                                            &nbsp;</td>
                                                                                        <td align="left" style="width: 85%; height: 23px">
                                                                                            &nbsp;</td>
                                                                                    </tr>
                                                                                    <tr style="font-size: 13pt">
                                                                                        <td class="cell_caption" style="width: 19%; height: 24px" >
                                                                                            </td>
                                                                                        <td align="left" style="width: 31%; height: 24px">
                                                                                            &nbsp;</td>
                                                                                        <td align="left" style="width: 22%; height: 24px" class="cell_caption">
                                                                                            &nbsp;</td>
                                                                                        <td align="left" style="width: 85%; height: 24px">
                                                                                            &nbsp;</td>
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
                                    <div id="diviCredit" style="display:none ;" >
                                            <table cellpadding="0" cellspacing="0" style="background-color: azure" width="100%" height="100%">
                                                <tr>
                                                    <td colspan="3" style="width: 98%; height: 100%">
                                                        <table class="table_border" style="width: 100%; height: 100%">
                                                            <tr>
                                                                <td style="width: 103%;">
                                                                    <table cellpadding="0" cellspacing="5" width="100%">
                                                                        <tr>
                                                                            <td style="width: 100%; height: 100%" valign="middle">
                                                                            <table id="Table2" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                    border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; height: 102%;"
                                                                                    width="100%">
                                                                                <tr>
                                                                                    <td align="left" colspan="4" >
                                                                                    <table style="width: 344px">
                                                                                        <tr>
                                                                                            
                                                                                            <td style="width: 54px;text-align:center ;" class="tab_unselected" 
                                                                                                onmouseover="javascript:ide.handle_mouseover('Debit','hoverin',this)"
                                                                                                onclick="javascript:ide.handle_mouseover('Debit','click',this)"
                                                                                                    >
                                                                                                &nbsp;Debit</td>
                                                                                             <td class="cell_caption" style="width: 54px;text-align:center ;">
                                                                                                &nbsp;Credit</td>   
                                                                                            <td style="width: 54px;text-align:center ;" class="tab_unselected" 
                                                                                                onmouseover="javascript:ide.handle_mouseover('Charges','hoverin',this)"
                                                                                                onclick="javascript:ide.handle_mouseover('Charges','click',this)"
                                                                                                    >
                                                                                                &nbsp;Charges</td>
                                                                                            <td style="width: 54px;text-align:center ;" class="tab_unselected" 
                                                                                                onmouseover="javascript:ide.handle_mouseover('Summary','hoverin',this)"
                                                                                                onclick="javascript:ide.handle_mouseover('Summary','click',this)"
                                                                                                    >
                                                                                                &nbsp;Summary</td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    
                                                                                        </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                        Remitter ID :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px">
                                                                                        <input id="txtRemId" type="text" style="width: 88%" /></td>
                                                                                    <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                        Benef ID :</td>
                                                                                    <td align="left" style="width: 85%; height: 23px"><input id="txtBeneId" type="text" style="width: 88%" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                        Remitter Accountno :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px">
                                                                                        <input id="txtRemAcct" type="text" style="width: 88%" /></td>
                                                                                    <td align="left" style="width: 22%; height: 23px" class="cell_caption">
                                                                                        Benef&nbsp; AccountNo :</td>
                                                                                    <td align="left" style="width: 85%; height: 23px"><input id="txtBeneAcct" type="text" style="width: 88%" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                        Remitter Name :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px">
                                                                                        <input id="txtRemName" type="text" style="width: 89%" /></td>
                                                                                    <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                        Benef Name :</td>
                                                                                    <td align="left" style="width: 85%; height: 23px">
                                                                                        <input id="txtBeneName" type="text" style="width: 88%" /></td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td class="cell_caption" style="width: 19%;" rowspan="2" valign="top">
                                                                                        Rem. Address :</td>
                                                                                    <td align="left" style="width: 29%;" rowspan="2" valign="top">
                                                                                        <label id="Label2" class="cell_desc" style="font-weight: normal; font-size: 15px">
                                                                                            <textarea id="txtRemAdd" style="width: 176px; height: 48px"></textarea></label></td>
                                                                                    <td align="left" style="width: 22%;" class="cell_caption" rowspan="2" valign="top">
                                                                                        Benef. Address :</td>
                                                                                    <td align="left" style="width: 85%;" rowspan="2" valign="top">
                                                                                        <textarea id="txtAddress" style="width: 176px; height: 48px"></textarea></td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                        </td>
                                                                                    <td align="left" style="width: 29%; height: 23px"></td>
                                                                                    <td align="left" style="width: 22%; height: 23px" class="cell_caption" nowrap>
                                                                                        B. Worker Type :&nbsp;</td>
                                                                                    <td align="left" style="width: 85%; height: 23px"><input id="txtWType" type="text" style="width: 89%" /></td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                    </td>
                                                                                    <td align="left" style="width: 29%; height: 23px">
                                                                                    </td>
                                                                                    <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                        B. Account Status :</td>
                                                                                    <td align="left" style="width: 85%; height: 23px">
                                                                                        <input id="txtBeneAcctStat" type="text" style="width: 89%" /></td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td align="right" colspan="4" style="height:5px;text-align:left;" valign="middle" >
                                                                                        </td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px" >
                                                                                        Product Code :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px" ><select id="ddProdCode" style="width: 98%; height: 21px" >
                                                                                        <option selected="selected"></option>
                                                                                    </select>
                                                                                    </td>
                                                                                    <td align="left" style="width: 22%; height: 23px" class="cell_caption">
                                                                                        Amount :
                                                                                    </td>
                                                                                    <td align="left" style="width: 85%; height: 23px"><input id="txtAmt" type="text" style="width: 89%" /></td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px" >
                                                                                        Prod Desc :
                                                                                        </td>
                                                                                    <td align="left" style="width: 29%; height: 23px"><input id="txtProdDesc" type="text" style="width: 89%" /></td>
                                                                                    <td align="left" class="cell_caption" style="width: 22%; height: 23px" >
                                                                                        Currency :</td>
                                                                                    <td align="left" style="width: 85%; height: 23px"><select id="ddCRAcctCurr" style="width: 98%; height: 21px" >
                                                                                        <option selected="selected"></option>
                                                                                    </select>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                        Implementation :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px"><select id="ddImplem" style="width: 98%; height: 21px" >
                                                                                        <option selected="selected"></option>
                                                                                    </select>
                                                                                    </td>
                                                                                    <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                    </td>
                                                                                    <td align="left" style="width: 85%; height: 23px">
                                                                                        &nbsp;</td>
                                                                                </tr>
                                                                            </table>
                                                                                <%-- start here  --%>
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
                                    <div id="diviCharges" style="display:none ;" >
                                            <table cellpadding="0" cellspacing="0" style="background-color: azure" width="100%" height="100%">
                                                <tr>
                                                    <td colspan="3" style="width: 100%; height: 100%">
                                                        <table class="table_border" style="width: 100%; height: 100%">
                                                            <tr>
                                                                <td style="width: 100%; height: 8px">
                                                                    <table cellpadding="0" cellspacing="5" width="100%">
                                                                        <tr>
                                                                            <td style="width: 100%; height: 100%" valign="middle">
                                                                            <table id="Table1" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                    border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid"
                                                                                    width="100%">
                                                                                <tr>
                                                                                    <td align="left" colspan="4" style="height: 23px">
                                                                                        <table style="width: 344px" >
                                                                                            <tr>
                                                                                                
                                                                                                <td style="width: 54px;text-align:center ;" class="tab_unselected" 
                                                                                                onmouseover="javascript:ide.handle_mouseover('Debit','hoverin',this)"
                                                                                                onclick="javascript:ide.handle_mouseover('Debit','click',this)"
                                                                                                    >
                                                                                                    &nbsp;Debit</td>
                                                                                                <td style="width: 54px;text-align:center ;" class="tab_unselected" 
                                                                                                onmouseover="javascript:ide.handle_mouseover('Credit','hoverin',this)"                                                                                                
                                                                                                onclick="javascript:ide.handle_mouseover('Credit','click',this)"
                                                                                                    >
                                                                                                    &nbsp;Credit</td>
                                                                                                
                                                                                                    <td class="cell_caption" style="width: 54px;text-align:center ;">
                                                                                                    &nbsp;Charges</td>
                                                                                                <td style="width: 54px;text-align:center ;" class="tab_unselected" 
                                                                                                onmouseover="javascript:ide.handle_mouseover('Summary','hoverin',this)"
                                                                                                onclick="javascript:ide.handle_mouseover('Summary','click',this)"
                                                                                                    >
                                                                                                    &nbsp;Summary</td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td id="tdservicefees" align="left" colspan="4" style="height: 23px">
                                                                                        </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                        Gross Peso :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px">
                                                                                        <input id="txtGrossPeso" type="text" style="width: 88%" /></td>
                                                                                    <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                        Net Peso :</td>
                                                                                    <td align="left" style="width: 85%; height: 23px"><input id="txtNetPeso" type="text" style="width: 88%" /></td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                        Amount :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px"><input id="txtRemAmt" type="text" style="width: 89%" /></td>
                                                                                    <td align="left" style="width: 22%; height: 23px" class="cell_caption" >
                                                                                        Net Dollar :</td>
                                                                                    <td align="left" style="width: 85%; height: 23px"><input id="txtNetDollar" type="text" style="width: 89%" /></td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                        Rate Used :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px"><input id="Text4" type="text" style="width: 89%" /></td>
                                                                                    <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                        Total Charges :</td>
                                                                                    <td align="left" style="width: 85%; height: 23px">
                                                                                        <input id="txtTotCharges" type="text" style="width: 89%" /></td>
                                                                                </tr>
                                                                            </table>
                                                                                <%-- start here  --%>
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
                                    <div id="diviSummary" style="display:none;" >
                                            <table cellpadding="0" cellspacing="0" style="background-color: azure" width="100%" height="100%">
                                                <tr>
                                                    <td colspan="3" style="width: 104%; height: 100%">
                                                        <table class="table_border" style="width: 100%; height: 100%">
                                                            <tr>
                                                                <td style="width: 100%; height: 8px">
                                                                    <table cellpadding="0" cellspacing="5" width="100%">
                                                                        <tr>
                                                                            <td style="width: 100%; height: 100%" valign="middle"><table id="Table5" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                    border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid"
                                                                                    width="100%">
                                                                                <tr>
                                                                                    <td align="left" colspan="4" style="height: 23px">
                                                                                        <table style="width: 344px">
                                                                                            <tr>
                                                                                                
                                                                                                <td style="width: 54px;text-align:center ;" class="tab_unselected" 
                                                                                                onmouseover="javascript:ide.handle_mouseover('Debit','hoverin',this)"
                                                                                                onclick="javascript:ide.handle_mouseover('Debit','click',this)"
                                                                                                    >
                                                                                                    &nbsp;Debit</td>
                                                                                                <td style="width: 54px;text-align:center ;" class="tab_unselected" 
                                                                                                onmouseover="javascript:ide.handle_mouseover('Credit','hoverin',this)"
                                                                                                onclick="javascript:ide.handle_mouseover('Credit','click')"
                                                                                                    >
                                                                                                    &nbsp;Credit</td>
                                                                                                
                                                                                                    
                                                                                                <td style="width: 54px;text-align:center ;" class="tab_unselected" 
                                                                                                onmouseover="javascript:ide.handle_mouseover('Charges','hoverin',this)"
                                                                                                onclick="javascript:ide.handle_mouseover('Charges','click',this)"
                                                                                                    >
                                                                                                    &nbsp;Charges</td>
                                                                                                    <td class="cell_caption" style="width: 54px;text-align:center ;">
                                                                                                    &nbsp;Summary</td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                        </td>
                                                                                    <td align="left" style="width: 29%; height: 23px">
                                                                                        <input id="txtTe" type="text" style="width: 88%" /></td>
                                                                                    <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                        USERID :</td>
                                                                                    <td align="left" style="width: 85%; height: 23px"><input id="txtUserId" type="text" style="width: 88%" /></td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                        TOTA L ITEMS :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px"><input id="txtTotItems" type="text" style="width: 89%" /></td>
                                                                                    <td align="left" style="width: 22%; height: 23px" class="cell_caption" >
                                                                                        CURRENT ITEMS :</td>
                                                                                    <td align="left" style="width: 85%; height: 23px"><input id="txtCurrItems" type="text" style="width: 89%" /></td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                        TOTAL AMOUNT :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px"><input id="txtTotAmt" type="text" style="width: 89%" /></td>
                                                                                    <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                        CURRENT AMOUNT :</td>
                                                                                    <td align="left" style="width: 85%; height: 23px">
                                                                                        <input id="txtCurrAmt" type="text" style="width: 89%" /></td>
                                                                                </tr>
                                                                            </table>
                                                                                <%-- start here  --%>
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
                                               
                                    <div id="btnButtons">
                                        <br />
                                        <input id="btnPrevious" style="width: 11%" type="button" value="Previous" />
                                        <input id="btnNext" style="width: 11%" type="button" value="Next" />
                                        <input id="btnUpdate" style="width: 11%" type="button" value="Save" />
                                        <input id="btnBack" style="width: 11%" type="button" value="Back" onclick="javascript:ide.btnCancelCreate_click()"/>
                                        </div>  
                                               
                                                </td>
                                                </tr>
                                            </table>
                                    </div>
                                    
                                 
                                     
                                    
                                        
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfSessionid" runat="server" />
            <asp:HiddenField ID="hfGUID" runat="server" />
            
        </div>
        <div id="divSearch" style="z-index:101;position:absolute;left:8px;color:darkblue;border:solid 2px #000;padding:10px;font-family:Arial;top:1928px;width:auto;height:auto;display:none;">
                        <table cellpadding="0" cellspacing="0" style="width: 350px">
                            <tr>
                                <td style="width:802px;height:194px;">
                                    <table cellpadding="0" cellspacing="0" width="350px">
                                        <tr>
                                            <td align="left" valign="bottom" style="width:0%; border-collapse:collapse;height:24px;"><img src="../../images/toe.GIF" /></td>
                                            <td class="table_head_dark" style="width:50%;height:24px;">
                                                SEARCH</td>
                                            <td align="left" style="width:100%;height:24px;" valign="bottom"><img align="absBottom" src="../../images/rend.gif" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <table border="0" style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;border-bottom: #336699 3px solid">
                                                    <tr>
                                                        <td style="width:250px;" class="cell_caption">
                                                            Search By
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <select id="ddlSearchBy" class="ddl" onchange="javascript:internet_page.getDDL('ddlSearchBy', 'lblCIFClass', true);" style="width:99%" onkeydown="javascript:internet_page.callSearch();" name="ddSearch">
                                                                <option selected="selected" value="1">Remitter</option>
                                                                <option value="2">Beneficiary</option>
                                                            </select>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px;" class="cell_caption">
                                                            Last Name
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <input id="txtSLastName" style="width:99%;text-align:left;" type="text" value="" onkeydown="javascript:internet_page.callSearch();" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px;" class="cell_caption">
                                                            First Name
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <input id="txtSFirstName" style="width:99%;text-align:left;" type="text" value="" onkeydown="javascript:internet_page.callSearch();" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px;" class="cell_caption">
                                                            Middle Name
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <input id="txtSMiddleName" style="width:99%;text-align:left;" type="text" value="" onkeydown="javascript:internet_page.callSearch();" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 250px">
                                                            Account No</td>
                                                        <td align="left" style="width: 450px">
                                                            <input id="txtSearchAcctNo" style="width:99%;text-align:left;" type="text" value="" onkeydown="javascript:internet_page.callSearch();" /></td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td colspan="2">
                                                            &nbsp;<input id="btnSearch" class="button" onclick="javascript:" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Search" style="width:72px; height: 22px;" />
                                                            <input id="btnSearchCancel" class="button" onclick="javascript:cif.btnCancel_click();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Cancel" style="width:72px; height: 22px;" /></td>
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
