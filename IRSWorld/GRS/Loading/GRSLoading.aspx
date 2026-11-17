<%@ Page Language="C#" AutoEventWireup="true" Codebehind="GRSLoading.aspx.cs" Inherits="IRSWorld.GRS.Loading.GRSLoading" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Remittance - APAS</title>
    <link href="../../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>

    <script src="../../js/jsCommons.js"                type="text/javascript"></script>
    <script src="../../js/jscommonmethod.js"           type="text/javascript"></script>
    <script src="../../js/js_sidebar.js"               type="text/javascript"></script>
    <script src="../../js/jsmodalwindow.js"            type="text/javascript"></script>
    <script src="../../js/Remittance/jsremsaving.js"   type="text/javascript"></script>
    <script src="../../js/Remittance/jsremcommons.js"  type="text/javascript"></script>
    <script src="../../js/Remittance/jsRemTranAPAS.js" type="text/javascript"></script>

    <script type="text/javascript">    
            var RemTran_APAS = new RemTran_APAS();
    </script>

</head>
<body >
    <form id="form1" runat="server">
        <div style="width: 100%">
            <table width="100%" cellpadding="0" cellspacing="5">
                <tr>
                    <td align="center" valign="top" style="width: 100%; border-collapse: collapse; height: 380px;"
                        class="content_height" colspan="2">
                        <div style="width: 100%; height: 100%; display: block">
                            <table width="100%">
                                <tr>
                                    <td  valign="top" style="width: 20%; vertical-align: top; padding-top: 30px;">
                                        <!-- border-right: solid 1px #5697c1;"> -->
                                        <div id="divtdRemAPASMenu" style="margin-top: 20px; display: block;">
                                        <table style="width: 128px"><tr><td style="width: 7px">
                                            File Upload&nbsp;</td></tr>
                                            <tr>
                                                <td style="width: 7px">
                                                    Edit Upload</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 7px">
                                                    Search</td>
                                            </tr>
                                        </table>


                                        </div>
                                        <a id="ahider" href="#" 
                                        >«
                                        </a>
                                    </td>
                                    <td style="width: 80%; padding-top: 30px" valign="top">
                                        <table width="96%" cellspacing="0" cellpadding="0" style="background-color: Azure">
                                            <tr>
                                                <td align="left" valign="bottom" style="width: 1%; height: 24px;">
                                                    <img src="../../images/toe.GIF" />
                                                </td>
                                                <td id="capResult" align="left" class="table_head_dark" 
                                                style="width: 40%; height: 23px; border-collapse: collapse;">
                                                    File Loading&nbsp;</td>
                                                <td align="left" valign="bottom" style="height: 24px" class="bg_body">
                                                    <img align="absbottom" src="../../images/rend.gif" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="width: 100%; height: 100%">
                                                    <table style="width: 100%; height: 100%;" class="table_border">
                                                        <tr>
                                                            <td style="width: 100%; height: 8px;">
                                                                <table width="100%" cellpadding="0" cellspacing="5px">
                                                                    <tr>
                                                                        <td style="width: 100%; height: 100%;" valign="middle">
                                                                            <div style="width: 98%; height: 100%; margin: 0" class="table_body" id="divAPASRemRemitterInfo">
                                                                                <table id="tblAPASRemRemitter" width="100%" cellpadding="0" cellspacing="8" style="border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 33%; height: 23px">
                                                                                            Tran Source :&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 85%; height: 23px;">
                                                                                            <select id="Select1" style="width: 89%; height: 21px">
                                                                                                <option selected="selected"></option>
                                                                                            </select>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr style="font-size: 13pt">
                                                                                        <td class="cell_caption" style="width: 33%; height: 23px">
                                                                                            Rate Type : &nbsp;</td>
                                                                                        <td align="left" style="width: 85%; height: 23px;">
                                                                                            <label id="lblAPASRemAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;"><select id="Select2" style="width: 89%">
                                                                                                <option selected="selected"></option>
                                                                                            </select>
                                                                                            </label></td>
                                                                                    </tr>
                                                                                    <tr style="font-size: 13pt">
                                                                                        <td class="cell_caption" style="width: 33%; height: 23px">
                                                                                            File Date :&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 85%; height: 23px">
                                                                                            <input id="Text1" type="text" /></td>
                                                                                    </tr>
                                                                                    <tr style="font-size: 13pt">
                                                                                        <td class="cell_caption" style="width: 33%; height: 23px">
                                                                                            Date Received :&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 85%; height: 23px">
                                                                                            <input id="Text2" type="text" /></td>
                                                                                    </tr>
                                                                                    <tr style="font-size: 13pt">
                                                                                        <td class="cell_caption" style="width: 33%; height: 23px">
                                                                                            BOD Date : &nbsp;</td>
                                                                                        <td align="left" style="width: 85%; height: 23px">
                                                                                            <input id="Text3" type="text" /></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <%-- start here  --%>
                                                                            <div style="width: 98%; height: 100%; margin: 0" class="table_body" id="divAPASRemBeneficiaryInfo">
                                                                                <table id="tblAPASRemBeneficiary" width="100%" cellpadding="0" cellspacing="8" style="border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="table_head_dark" colspan="1" style="height: 23px; text-align: center">
                                                                                            Uploaded Files</td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 15%; height: 23px">
                                                                                            <table style="width: 616px">
                                                                                                <tr class="cell_caption">
                                                                                                    <td align="center" style="width: 97px">
                                                                                                        Date Sent</td>
                                                                                                    <td style="width: 190px" align="center">
                                                                                                        File Name</td>
                                                                                                    <td align="center" style="width: 42px">
                                                                                                        Size</td>
                                                                                                    <td align="center" style="width: 7px">
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="width: 97px">
                                                                                                    </td>
                                                                                                    <td style="width: 190px">
                                                                                                    </td>
                                                                                                    <td style="width: 42px">
                                                                                                    </td>
                                                                                                    <td style="width: 7px">
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="width: 97px">
                                                                                                    </td>
                                                                                                    <td style="width: 190px">
                                                                                                    </td>
                                                                                                    <td style="width: 42px">
                                                                                                    </td>
                                                                                                    <td style="width: 7px">
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td align="right" colspan="4">
                                                                                                        <table style="width: 200px">
                                                                                                            <tr>
                                                                                                                <td style="width: 106px" class="cell_desc">
                                                                                                                    page 1 of 2</td>
                                                                                                                <td><select id="Select3" style="width: 89%; height: 21px">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        &nbsp;</td>
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
                                                        <tr>
                                                            <td style="width: 100%; height: 100%; vertical-align: top">
                                                                <!-- content starts here -->
                                                                <!-- content ends here -->
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
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
