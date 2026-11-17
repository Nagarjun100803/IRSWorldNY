<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report_Holder.aspx.cs" Inherits="IRSWorld.Reports.Pages.Report_Holder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Report Generator</title>
    <link href="../../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jsCommons.js" type="text/javascript"></script>
    <script src="../../js/jscommonmethod.js" type="text/javascript"></script>    
    <script src="../../js/Reports/js_Reports_LookupUI.js" language="javascript"> </script>
    <script src="../../js/popcalendar.js" type="text/javascript"></script>    
</head>
<body  topmargin ="6" leftmargin="0" onload="javascript:this.page_setup('Report_Holder.aspx');">
    <form id="form1" runat="server">
    <div id="divreportgen" align="center" style="width:96%; height:53%;" class=content_height>
    <table cellpadding="30" >
    <tr>
        <td>
        <table cellpadding="0" cellspacing="0" style="width: 81%">        
            <tr>    
                <td>
                    <table style="width:100%;" cellpadding="0" cellspacing="0" >
                        <tr>
                            <td align="left" valign="bottom" style="width:1%;height:24px; border-collapse: collapse"> 
                                <img src="../../images/toe.GIF" /></td>
                            <td id="capLabel" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse">
                                REPORTS GENERATOR</td>
                            <td id="tdpopup2" align="left" valign="bottom" style="height: 24px" class="bg_body">
                                <img align="absbottom" src="../../images/rend.gif" />
                            </td>
                        </tr>  
                    </table>    
                </td>            
            </tr>
            <tr>
                <td colspan="3" style="background: azure; width: 100%">
                    <table style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;
                        width: 100%; border-bottom: #336699 3px solid">
                        <tr>
                            <td style="width: 97%; height: 21px;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 97%; height: 131px; text-align: center;">
                                <!-- content starts here, if necessary increase/decrease the size of the bounding table -->
                                <div id="div_content" class="table_body" style="display: block;">
                                    <table id="tbl_sample" style="width: 76%; ">
                                        <caption id="sample_caption" class="table_caption" style="vertical-align: middle">
                                            Report Selection</caption>
                                        <tr>
                                            <td class="cell_caption" style="width: 182px; height: 9px">
                                                Branch Code:&nbsp;
                                            </td>
                                            <td align="left" class="cell_desc" style="width: 453px; height: 9px" nowrap >
                                                <div id="divbranchcode">
                                                    <select id="ddBranchCode" style="width: auto; height: 21px">
                                                        <option selected="selected"></option>
                                                    </select>
                                                    <asp:HiddenField ID="txtbranchcode" runat="server" EnableViewState="False" /><asp:HiddenField ID="txtisadmin" runat="server" EnableViewState="False" /><asp:HiddenField ID="txtparam" runat="server" EnableViewState="False" />
                                                    <asp:HiddenField ID="txtparamval" runat="server" EnableViewState="False" /><asp:HiddenField ID="txtpre_eoddate" runat="server" EnableViewState="False" />
                                                    <asp:HiddenField ID="txtpre_sdate" runat="server" EnableViewState="False" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width: 182px; height: 9px" nowrap>
                                                Report Type:&nbsp;
                                            </td>
                                            <td align="left" class="cell_desc"  style="height: 9px; width: 453px;" >
                                                <select id="ddlRepMain" name="ddlRepMain" style="width: auto; height: 21px;" onchange="javascript:ddl_groupchange();">
                                                    <option selected="selected"></option>
                                                </select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width: 182px; height: 23px" nowrap >
                                                Report Name:&nbsp;
                                            </td>
                                            <td align="left" class="cell_desc" style="height: 23px; width: 453px;" >
                                                <select id="ddlRepName" name="ddlRepName" style="width: auto; height: 21px;" onchange="javascript:document.getElementById('ddlExport').selectedIndex = 0;">
                                                    <option selected="selected"></option>
                                                </select>
                                            </td>
                                           
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width: 182px; height: 23px" nowrap >
                                                Export To:&nbsp;
                                            </td>
                                            <td align="left" class="cell_desc" style="height: 23px">
                                                <select id='ddlExport' style="width: auto" onchange ="javascript:create_report_params();">
                                                    <option value=''>&nbsp;</option>
                                                    <option value='PDF'> PDF</option>
                                                    <option value="XLS">EXCEL</option>
                                                    <option value="RTF">RTF</option>
                                                    <option value="DOC">WORD</option>
                                                    <option value="HTML">HTML </option>
                                                </select>
                                                <input id="chkautoprint" style="width: 6%;display:none ;" type="checkbox" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="height: 1px; text-align: center">
                                                &nbsp;</td>
                                        </tr>                                   
                                       
                                    </table>                                
                                  
                                </div>
                                <!-- content ends here -->
                            </td>
                        </tr>
                        <tr>
                            <td id="btnSample" style="width: 97%">
                                &nbsp;
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
   <div id="divExplore" style="width: auto;  border: solid 1px midnightblue; display: none; position: absolute; z-index: 101; " class="ds_gray">
        <table style="width:auto;" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width:auto; height: 15px; font: bold 12px Arial; color: Ivory" class="navbar" align="left"  >&nbsp;&nbsp;Input Parameters</td>
                <td class="navbar" align="right" style="width:15px;">
                    <input type="button" value="x" onclick="close_explore();" style="width:15px; color: red" class="table_caption" />
                </td>
            </tr>
            <tr>
                <td  align="left" colspan="2" style="height:auto;">
                      <div id="pnlParameters" style="width:auto;height:auto;"></div>                     
                </td>
                
                 
            </tr>
            <tr><td  align="center" colspan="2" style="height: 28px">
                <input id="btnSearch" style="width: auto" type="button" value="Generate" onclick="javascript:load_reportpage();"/></td></tr>
        </table>
    </div>        
    </form>
    
</body>
</html>
