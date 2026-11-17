<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Extract_3D.aspx.cs" Inherits="IRSWorld._3D.Extract_3D" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Extract 3D Transactions</title>
    <link href="../css/cssmodals.css" rel="stylesheet" type = "text/css" />
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src = "../js/jsCommons.js"                      type = "text/javascript"></script>
    <script src = "../js/jscommonmethod.js"                 type = "text/javascript"></script>
    <script src = "../js/jshttprequest.js"                  type = "text/javascript"></script>
    <script src = "../js/jsmodalwindow.js"                  type = "text/javascript"></script>
    <script src = "../js/jsgridselector.js"                 type = "text/javascript"></script>
    <script src = "../js/3D/Courier_Tran_List.js"           type = "text/javascript"></script>    
    <script src = "../js/3D/Extract_3D.js"                  type = "text/javascript"></script> 

    <script type="text/javascript">        
             
    </script>
</head>
<body onload="courier_extract.load_currency();">
    <form id="Form1" runat="server">
        <center>
        &nbsp;<table id="gridDetails" cellpadding="0" cellspacing="0" style="background-color: azure; width: 60%;">
                        <tr>
                            <td align="left" style="width: 1%; height: 24px" valign="bottom">
                                <img src="../images/toe.GIF" /></td>
                            <td id="capResult" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                                height: 24px">
                                Extract 3D Transactions </td>
                            <td align="left" class="bg_body" style="height: 24px; width: 424px;" valign="bottom">
                                <img align="absBottom" src="../images/rend.gif" /></td>
                               
                        </tr>
                       
                        <tr>
                            <td colspan="3" style="width: 100%; height: 100%">
                                <table class="table_border" style="width: 100%; height: 100%">                                    
                                                <tr>
                                                    <td style="vertical-align: top; width: 93%; height: 100%">
                                                        <!-- content starts here -->
                                            <!-- content ends here -->
                                                        <table cellpadding="0" width="100%">
                                                            <tr>
                                                    <td style="width: 35%; height: 21px;" align="left" class="cell_caption">
                                                        Extract 3D:&nbsp; &nbsp;
                                                    </td>
                                                    <td align="left" style="height: 21px; width: 366px;"><div id="divfldgroup">
                                                        <select id="ddl_3D_Currency" style="width: 92%">
                                                            <option selected="selected"></option>
                                                        </select>
                                                    </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="width: 100%; height: 100%; text-align: top">
                                                        <div id="div_courier_3d_extract" style="width: 400px; height: 100%;" visible="false">
                                                        </div><label id="" visible></label>
                                                    </td>
                                                </tr>
                                                
                                                <tr>
                                                    <td  id="trnxStatus" style="text-align: center; width:100%; height: 19px;" colspan="2">
                                                        <input id="btnExtract" style="width: 30%; " type="button" value="Pull 3D" onclick="courier_extract.extract_viewer();"  /></td>
                                                </tr>
                                                
                                            </table>
                                         </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

            <br />
        </center>
        <center>
            &nbsp;</center>
        <center>
        <div id ="divTest" style="width: 500px; height: 11px;">
        </div>
            &nbsp;</center>
    </form>
</body>
</html>