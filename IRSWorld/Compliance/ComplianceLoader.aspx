<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComplianceLoader.aspx.cs" Inherits="IRSWorld.Compliance.ComplianceLoader" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../css/Default.css" rel ="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <table style="width:100%" cellpadding ="0" cellspacing ="5">
            <tr>
                <td align="center" valign="middle">
                   <table style="width:400px" cellpadding="0" cellspacing="0"> <!--class=-->
                        <tr>                           
                            <td class="table_head_dark" style="width:160px; height: 26px;">Compliance Loader</td>
                            <td align="left" valign="bottom" style="width:160px; height:26px;"><img align="absbottom" src="../images/cap.gif"/></td>                            
                        </tr>
                            <td colspan="2">
                                <table style="width:100%; border:solid 3px #336699">
                                    <tr>
                                        <td class="table_caption" colspan="2" style="width:388px; text-align:left; text-indent:5px; color:Black; height:26px;" valign="top">File Setup</td>
                                    </tr>                               
                                    <tr>
                                        <td class="cell_caption" style="width: 40%; height:24px">Table Name:&nbsp</td>
                                        <td align="left" style="width:70%; height:24px">
                                            <select id="ddTableName" style="width:100%"></select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="cell_caption"> Regulatory File Name:&nbsp</td>
                                        <td align="left" style="width:70%; height:24px">
                                            <input id="txtFileName" type="text" style="width:98%; text-align:left "/>                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="cell_caption">File Format:&nbsp</td>
                                        <td align="left" style="width:70%; height:24px">
                                            <select id="ddFileFormat" style="width:100%"></select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="cell_caption">Row Delimiter:&nbsp</td>
                                        <td align="left" style="width:70%; height:24px">
                                            <select id="ddRowDelimiter" style="width:100%"></select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="cell_caption">Column Delimiter:&nbsp</td>
                                        <td align="left" style="width:70%; height:24px">
                                            <select id="ddColumnDelimiter" style="width:100%"></select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="cell_caption">Text Qualifier:&nbsp</td>
                                        <td align="left" style="width:70%;height:24px">
                                            <select id="ddTextQualifier" style="width:100%"></select>
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td class="cell_caption">Records Starts at Row:&nbsp</td>
                                        <td align="left" style="width:70%; height:24px">
                                            <select id="ddRecordStarts" style="width:100%"></select>
                                        </td>
                                    </tr>                                    
                                </table>
                            </td>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                    </tr>                                    
                                </table>
                            </td>      
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table style="width:100%; border:solid 3px #336699">
                                    <tr>
                                        <td class="cell_caption" style="width: 40%; height:24px">Field Name: &nbsp</td>
                                        <td align="left" style="width:70%; height:24px">
                                            <input id="txtFieldName" type="text" style="width:98%; text-align:left" />
                                        </td>
                                    </tr>    
                                    <tr>
                                        <td class="cell_caption">Start Position: &nbsp</td>
                                        <td align="left" style="width:70%; height:24px">
                                            <input id="txtStartPosition" type="text" style="width:98%; text-align:left" />
                                        </td>
                                    </tr>   
                                        <td class="cell_caption">Data Length: &nbsp</td>                             
                                        <td align="left" style="width:70%; height:24px">
                                            <input id="txtDataLength" type="text" style="width:98%; text-align:left" />
                                        </td>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td style="width:100px; height:19px">
                                            <input id="btnAdd" type="button" class="add_button" value="Add"/>
                                        </td>
                                        <td style="width:100px; height:19px;">
                                            <input id="btnEdit" type="button" class="edit_button" value="Edit"/>
                                        </td>
                                        <td style="width:100px; height:19px;">
                                            <input id="btnDelete" type="button" class="delete_button" value="Delete"/>
                                        </td>
                                        <td style="width:100px; height:19px;">
                                            <input id="btnClose" type="button" class="" value="Close"/>
                                        </td>                                        
                                    </tr>
                                </table>
                            </td>
                        </tr>
                   </table> 
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
