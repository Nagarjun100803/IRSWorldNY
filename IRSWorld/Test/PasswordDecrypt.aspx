<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PasswordDecrypt.aspx.cs" Inherits="IRSWorld.Test.PasswordDecrypt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Password Decryptor</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>

    <script src="../js/jsCommons.js"                        type="text/javascript"></script>
    <script src="../js/jscommonmethod.js"                   type="text/javascript"></script>
    <script src="../js/js_generic.js"                       type="text/javascript"></script>
    <script src="../js/jshttprequest.js"                    type="text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"                    type="text/javascript"></script>
    <script src="../js/jsgridselector.js"                   type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function page_load()
        {
            var dt = IRSWorld.Test.PasswordDecrypt.getBranchList().value;
            if (dt != null && dt.Rows.length > 0)
            {            
                for (var i = 0; i < dt.Rows.length; i++)
                {
                    if (i == 0)
                    {
                        var opt2 = document.createElement("option");
                        opt2.text = '';
                        opt2.value = '';
                        document.getElementById("ddlBranch").options.add(opt2);
                    }
                    var opt = document.createElement("option");                 
                    opt.text = dt.Rows[i].BranchName.toString();
                    opt.value = dt.Rows[i].BranchCode.toString();
                    document.getElementById("ddlBranch").options.add(opt);
                }      
            }
        }
        
        function getUserbyBranch(brCode)
        {
            var dt = IRSWorld.Test.PasswordDecrypt.getUserByBranch(brCode).value;
            if (dt != null && dt.Rows.length > 0)
            {            
                for (var i = 0; i < dt.Rows.length; i++)
                {
                    if (i == 0)
                    {
                        var opt2 = document.createElement("option");
                        opt2.text = '';
                        opt2.value = '';
                        document.getElementById("ddlUserName").options.add(opt2);
                    }
                    var opt = document.createElement("option");                 
                    opt.text = dt.Rows[i].UserID.toString();
                    opt.value = dt.Rows[i].Role.toString();
                    document.getElementById("ddlUserName").options.add(opt);
                }      
            }
        }
        
        function getUserRole(UserID)
        {
            var dt = IRSWorld.Test.PasswordDecrypt.getUserRole(UserID).value;
            if (dt != null && dt.Rows.length > 0)
            {
                document.getElementById("spnUserRole").value = dt.Rows[0].FldDescription.toString();
                document.getElementById("spnUserRole").innerHTML = dt.Rows[0].FldDescription.toString();
            }
        }
        
        function decryptpass(UserID)
        {
            alert(IRSWorld.Test.PasswordDecrypt.getUserPassword(UserID).value);
        }
    </script>
</head>
<body onload="page_load();">
    <form id="form1" runat="server">
    <div style="width: 100%">
        <table width="100%" cellpadding="0" cellspacing="5">
                <tr>
                    <td align="center" valign="top" style="width: 100%; border-collapse: collapse; height: 600px;"
                        class="content_height" colspan="2">
                        <div id="divBillsPayOBPSRemitter" style="width: 50%; height: 100%; display: block">
                            <table width="100%">
                                <tr>
                                    <td style="width: 78%; padding-top: 30px">
                                        <table width="96%" cellspacing="0" cellpadding="0" style="background-color: Azure">
                                            <tr>
                                                <td align="left" valign="bottom" style="width: 1%; height: 24px;">
                                                    <img src="../images/toe.GIF" /></td>
                                                <td id="capResult" align="left" class="table_head_dark" style="width: 40%; height: 24px;
                                                    border-collapse: collapse;">
                                                    Password Descrypt</td>
                                                <td align="left" valign="bottom" style="height: 24px" class="bg_body">
                                                    <img align="absbottom" src="../images/rend.gif" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="width: 100%; height: 100%">
                                                    <table class="table_border" style="width: 100%; height: 100%">
                                                        <tr>
                                                            <td style="width: 100%; height: 21px">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="vertical-align: top; width: 100%; height: 100%">
                                                                <!-- content starts here -->
                                                                <table cellpadding="0" cellspacing="5" width="100%">
                                                                    <tr>
                                                                        <td style="width: 100%; height: 100%" valign="center">
                                                                            <div id="divBillsPayOBPSRemitterInfo" class="table_body" style="margin: 0px; width: 98%;
                                                                                height: 100%">
                                                                                <table id="tblBillsPayOBPSRemitter" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                    border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid"
                                                                                    width="100%">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 10%; height: 23px;">
                                                                                            Branch : &nbsp;
                                                                                        </td>
                                                                                        <td style="width: 15%; height: 23px;">
                                                                                            <select id="ddlBranch" style="width: 99%" onchange = "getUserbyBranch(document.getElementById('ddlBranch').value);">
                                                                                            </select>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 10%; height: 23px;">
                                                                                            User Name : &nbsp;
                                                                                        </td>
                                                                                        <td style="width: 15%; height: 23px;">
                                                                                            <select id="ddlUserName" style="width: 99%" onchange = "getUserRole(document.getElementById('ddlUserName').value);">
                                                                                            </select>
                                                                                        </td>                                                                                        
                                                                                    </tr>                                                                                    
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 10%; height: 23px;">
                                                                                            User Role : &nbsp;
                                                                                        </td>
                                                                                        <td style="width: 15%; height: 23px;" align="left">
                                                                                            <span id="spnUserRole"></span>
                                                                                        </td>
                                                                                                                                                                
                                                                                    </tr>
                                                                                </table>
                                                                            </div>                                                                            
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 100%; height: 27px" valign="top">
                                                                            <input class="button"  onmouseout="this.className='button';"
                                                                                onmouseover="this.className='button_over';" style="width: 30%; height: 28px"
                                                                                type="button" value="Decrypt" id="btnPassDec" onclick="decryptpass(document.getElementById('ddlUserName')[document.getElementById('ddlUserName').selectedIndex].text);" />
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <!-- content ends here -->
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
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
