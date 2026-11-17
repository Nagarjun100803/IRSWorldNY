<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EndOfDay.aspx.cs" Inherits="IRSWorld.EOD.EndOfDay" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    
    <link href="../css/Default.css" rel="stylesheet"   type = "text/css" />
    <script src="../js/jsCommons.js"                   type = "text/javascript"></script>
    <script src="../js/jscommonmethod.js"              type = "text/javascript"></script>
    <script src="../js/jshttprequest.js"               type = "text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"               type = "text/javascript"></script>
    <script src="../js/jsgridselector.js"              type = "text/javascript"></script>
    <script src="../js/Remittance/jsRemExternals.js"   type="text/javascript"> </script>
    <script src="../js/EOD/EOD.js"                     type = "text/javascript"></script>
    
    <script type="text/javascript">
        var cls_EOD = new EOD();          
    </script>
    
    
</head>
<body onload="javascript:cls_EOD.get_transaction_date();">
<form id="form1" runat="server" method="post" action="EndOfDay.aspx">


    <table cellpadding="30" style="width:100%" align="center" >
    <tr>
        <td style="width:100%;" align="center">
        <table style="width:100%; height:100%" cellpadding="0" cellspacing="5">        
            <tr>
                <td align="center" style="vertical-align: top; text-align: center;">
                    <div align="center">
                    <table style="width:50%" cellspacing="0" cellpadding="0">
                        <tr>
                           <td colspan="3" style="width: 50%; ">
                           <table style="width:100%;" cellpadding="0" cellspacing="0" >
                           <tr>
                            <td align="left" style="width: 1%; height: 24px" valign="bottom">
                                <img src="../images/toe.GIF" /></td>
                            <td id="capResult" align="left" class="table_head_dark" style="width: 40%; height: 24px;
                                            border-collapse: collapse">
                                IRSWorld End of Day</td>
                            <td align="left" class="bg_body" style="width: 171px; height: 24px" valign="bottom">
                                <img align="absBottom" src="../images/rend.gif" /></td>
                                </tr>
                            </table>
                         </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="width: 50%; ">
                                <table class="table_border" style="width: 100%; height: 100%">
                                    <tr>
                                        <td style="vertical-align: top; width: 41%; height: 44%" valign="middle">                                            
                                            <table style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid;
                                                border-bottom: #336699 1px solid; width:100%">
                                                <tr>
                                                    <td class="cell_caption" style="width: 25%">
                                                        Transaction Date :</td>
                                                    <td class="cell_desc" style="width: 25%">
                                                        <input id="txttranDate" name="txttranDate" type="text" disabled="disabled" /></td>
                                                </tr>
                                                <tr>
                                                <td colspan="2" height="20%" align="center"><input id="btnEODclick" onclick="javascript:cls_EOD.chk_eod_items();" style="width: 32%" type="button"
                                                value="End of Day" /></td>
                                                </tr>
                                                
                                            </table>
                                                                                
                                            </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        
                    </table>
                    </div>
                  <div id="divWaitMsg" style="background-color:transparent;display:none;z-index:100;">
                                            <table border="0"  style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;width:400px;background-color: azure;" cellpadding="0" cellspacing="1" align="center">
                                                <tr>
                                                    <td align="center" class="cell_desc"> CHECKING EOD ITEMS</td>
                                                </tr>
                                                <tr>
                                                    <td align="center" class="cell_desc"><img src = ".././images/ajaxloader.gif" /></td>
                                                </tr>
                                            </table>
                                            </div>     
                </td>
            </tr>
        </table>
        </td>
    </tr>
    </table>


  <asp:HiddenField ID="hdBranchCode" runat="server" />
                    <asp:HiddenField ID="hdUserID" runat="server" />
                    <asp:HiddenField ID="hdTranxDate" runat="server" />
  </form>
     
</body>
</html>
