<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GFCCardCKUpload.aspx.cs" Inherits="IRSWorld.GFC.GFCCardCKUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../css/Default.css" rel="stylesheet"   type = "text/css" />
    <link href="../css/cssmodals.css" rel="stylesheet" type = "text/css" />
    <script src="../js/jsCommons.js"                   type = "text/javascript"></script>
    <script src="../js/jscommonmethod.js"              type = "text/javascript"></script>
    <script src="../js/jshttprequest.js"               type = "text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"               type = "text/javascript"></script>
    <script src="../js/jsgridselector.js"              type = "text/javascript"></script>    
    <script src="../js/js_sidebar.js"                  type = "text/javascript"></script>
    <script src="../js/GFC/jsGFCCardCKUpload.js"                 type = "text/javascript"></script>    
    <script type="text/javascript">        
        var cls_GFCCardCKUpload = new GFC_Card_CK_Upload();
        function setHid()
        {
            document.getElementById('hUpType').innerText = document.getElementById('ddlUploadType').value;
        } 
    </script> 
</head>
<body onload = "javascript:cls_GFCCardCKUpload.page_onload();">
    <form id="form1" runat="server">
    <div style="width: 100%" id="DIV_Search">
        <table style="width:100%">
            <tr><td colspan="3" style="height:20px"></td></tr> 
            <tr>
                <td id="tdAPASMenu" style="vertical-align: top; width: 10%; padding-top: 30px">
                    <div id="divtdAPASMenu" style="margin-top: 20px; display: block">

                        <script type="text/javascript">
                        </script>

                    </div>
                </td>
                <td style="vertical-align: top; text-align: center;">
                    <table id="tblMain" cellpadding="0" cellspacing="0" style="background-color: azure; width: 62%; height: 98px;">
                        <tr>
                            <td align="left" style="width: 1%; height: 24px" valign="bottom">
                                <img src="../images/toe.GIF" /></td>
                            <td id="td_Header" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                                height: 24px">
                                GFC CARD / CK UPLOAD</td>
                            <td align="left" class="bg_body" style="height: 24px; width: 436px;" valign="bottom">
                                <img align="absBottom" src="../images/rend.gif" /></td>
                        </tr>
                        <tr>
                            <td colspan="3" style="width: 100%; height: 100%">
                                <table class="table_border" style="width: 100%; height: 80%">                                    
                                    <tr>
                                        <td style="vertical-align: top; width: 100%; height: 100%">
                                            <!-- content starts here -->
                                            <table id="tblUploads" cellpadding="0" style="width:100%">
                                                <tr>
                                                    <td style="text-align: right; height: 152px;">
                                                        <table style="width:100%; border: solid 1px #336699;">
                                                            <tr>
                                                                <td class="table_caption" colspan="2" style="height: 23px" align="center"><label id="lblStatus"  class="cell_desc" style="font-size: medium; font-weight: bold; text-align: center; width: 100%" runat="server">
                                                                    </label></td>
                                                            </tr>
                                                            <tr>
                                                                <td id = "tdDDLCaption" class="cell_caption" style="width: 10%; height: 23px">
                                                                    Upload Type :</td>
                                                                <td style="width: 36%; height: 23px"><select id="ddlUploadType" name="cmb_trxn_stat" style="width: 97%" runat="server" onchange="setHid();">
                                                                    <option selected="selected"></option>
                                                                </select>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr id="trBatchName">
                                                                <td class="cell_caption" style="width: 10%; height: 23px;">
                                                                    Batch Name :</td>
                                                                <td class="cell_desc" style="width: 36%; height: 23px;">
                                                                    <input id="txtBatchName" name="txtRefno" type="text" style="width: 96%" runat="server"/>&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 10%">
                                                                    File Name :</td>
                                                                <td style="width: 36%">
                                                                    <input id="File1" style="width: 98%" type="file" name="File1" runat="server" />                                                                    
                                                            </tr>
                                                           
                                                        </table>
                                                        <table cellpadding="0" style="width:100%">
                                                            <tr><td style="width: 458px; height: 21px;" align="left"><label id="lblMandatory" class="comment">
                                                                &nbsp; Note* All fields are required</label>
                                                            </td></tr>
                                                            <tr>                                                                 
                                                                <td style="width: 458px">
                                                                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" style="width:1px; visibility:hidden" />
                                                                    &nbsp;
                                                                    <input id="btnCancel" style="width: 18%" type="button"
                                                                        value="Cancel" onclick="javascript:cls_GFCCardCKUpload.cancel();"/>
                                                                    <input id="btnUpload" style="width: 18%" type="button"
                                                                        value="Upload" onclick="javascript:cls_GFCCardCKUpload.uploadFile();" />
                                                                    <input id="btnPrintForm" style="width: 18%" type="button"
                                                                        value="Print Forms" onclick="javascript:cls_GFCCardCKUpload.btnPrintForm_click();"/>                                                                
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                     
                                                                </td> 
                                                            </tr>  
                                                           <tr id="trLoading">
                                                                <td class="table_caption" colspan="2" style="height: 23px" align="center">
                                                                        <img src="../images/ajaxloader.gif" id="imgAjaxIcon" />
                                                                    <asp:Label ID="Label1" runat="server" Text="uploading file, pls wait..."></asp:Label></td>
                                                            </tr> 
                                                    
                                                        </table> &nbsp;
                                                        </td>
                                                </tr>
                                                
                                            </table>
                                                <input id="hUpType" name="hUpType" type="text" style="width: 12%; display: none" runat="server" value="0"/>
                                                <input id="HMode" name="HMode" type="text" style="width: 12%; display: none" runat="server"/>
                                                <input id="HBatchNum" name="HMode" type="text" style="width: 12%; display: none" runat="server" onchange="javascript:cls_GFCCardCKUpload.HBatchNum_onchange();"/>
                                                                                                                   
                                                <table id="tbl_Forms" style="width:100%; border: solid 1px #336699;">
                                                    <tr>
                                                        <td class="table_caption" colspan="2" style="height: 23px" align="left"><label class="cell_desc" style="font-size: medium; font-weight: bold; width: 100%; text-align: left;">
                                                            &nbsp;</label></td>
                                                    </tr>
                                                    <tr id="tr1">
                                                        <td class="cell_caption" style="width: 10%; height: 23px;">
                                                            File Name :</td>
                                                        <td class="cell_desc" style="width: 36%; height: 23px;">
                                                            <input id="txtFileName" name="txtRefno" type="text" style="width: 96%" runat="server"/>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <div id = "diviGridBatchItems" style= "display:block">
                                                                <table cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                    <tr>
                                                                        <td style="width: 100%; text-align: center; text-indent: 5px; color: Black; height: 19px;" colspan="5"></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="height: 2px; text-align: center;"><div id = "CIFIDGRID" style= "display:none">  PLACE GRIDVIEW ID'S</div></td>
                                                                    </tr>
                                                                </table>
                                                           </div>                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 36%; text-align:right; height: 22px;" colspan="2">
                                                            &nbsp;<input id="btnPrintReleaseForm" style="width: 37%" type="button"
                                                                        value="Print Release Form" onclick="javascript:cls_GFCCardCKUpload.btnPrintReleaseForm_click();" />
                                                            <input id="btnPrintTermsAndConditions" style="width: 37%" type="button"
                                                                        value="Print Terms and Conditions" onclick="javascript:cls_GFCCardCKUpload.btnPrintTermsAndConditions_click();" />
                                                            <input id="btnBack" style="width: 15%" type="button"
                                                                        value="Back" onclick="javascript:cls_GFCCardCKUpload.btnBack_click();"/>&nbsp;
                                                    </tr>
                                                   
                                                </table>                                                                
                                                               
                                                               
                                      </td>
                                                                                                            
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td id="td4" style="vertical-align: top; width: 10%; padding-top: 30px">
                </td>
            </tr>
        </table>    
    </div>
    </form>
</body>
</html>
