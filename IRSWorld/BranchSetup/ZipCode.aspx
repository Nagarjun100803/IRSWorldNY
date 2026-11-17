<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ZipCode.aspx.cs" Inherits="IRSWorld.BranchSetup.ZipCode" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Zip Codes</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/commons.js" type="text/javascript"></script> 
    <script src="../js/datagrid.js" type="text/javascript"></script> 
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/BranchSetup/js_zipcode.js" type="text/javascript"></script>     
</head>
<script language="javascript">
    var zip_code = new zip_code();
    
    function fixdim()
    {
        if (top.document.getElementById('divCenter'))
        {
            div = document.getElementById('divmain');
            if (div)
            {
                div.style.width = top.document.getElementById('divCenter').offsetWidth + 'px';
                div.style.height = (top.document.getElementById('divCenter').offsetHeight) + 'px';
                
                var tab = document.getElementById('tabmain');
                if (tab)
                {
                    tab.style.width = top.document.getElementById('divCenter').offsetWidth + 'px';
                    tab.style.height = (top.document.getElementById('divCenter').offsetHeight) + 'px';
                }
            }
        }
    }
    
    function page_setup()
    {
        if (top == self)
        {
            window.open('../Main/IRSMain.aspx?load=../BranchSetup/ZipCode.aspx', '_top');
        }
        else
        {
            fixdim();
        }
    }
     
</script>


<!-- chr10072010 cross browser modifications, added onresize attribute pluse page_setup and fixdim functions -->

<body id=theBody onload="javascript:page_setup(); zip_code.load_all_dropdownlist();" onresize="fixdim();">
    <center>
    <form id="form1" runat="server">
    <div id=divmain style="width:100%; height:100%; overflow:auto;">
        <table id=tabmain style="width:82%;height:100%;" cellpadding="30" cellspacing="5">
            <tr>    
                <td align="center" style="width:100%; border-collapse:collapse; vertical-align: top;">
                    <table style="width:90%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td>
                                    <table style="width:100%;" cellpadding="0" cellspacing="0" >
                                        <tr>
                                            <td style="width: 60%; vertical-align: bottom; text-align: left">
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="left" valign="bottom" style="width:0%;height:24px; border-collapse: collapse"> 
                                                            <img src="../images/toe.GIF" /></td>
                                                        <td id="capLabel" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse">
                                                            Zip Codes</td>
                                                        <td id="tdpopup2" align="left" valign="bottom" style="width: 60%; height: 24px" class="bg_body">
                                                            <img align="absbottom" src="../images/rend.gif" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>  
                                    </table>    
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="width: 100%; border:solid 3px #336699;">
                                <table style="width:100%;">
                                    <tr>
                                        <td style="width: 18%; height: 21px;" align="left" class="cell_caption">
                                            Region:&nbsp; </td>
                                        <td align="left" style="height: 21px"><div id="divfldgroup">
                                            <select id="ddRegion" style="width: 40%" onchange="zip_code.ddregion_change();">
                                                <option selected="selected"></option>
                                               
                                            </select>
                                             </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="cell_caption" style="width: 18%; height: 21px">
                                            Province:</td>
                                        <td align="left" style="height: 21px">
                                            <select id="ddProvince" style="width: 40%" onchange="zip_code.ddprovince_change();">
                                                <option selected="selected"></option>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="cell_caption" style="width: 18%; height: 21px">
                                            Search ZipCode:</td>
                                        <td align="left" style="height: 21px">
                                            <input id="txtSearchZip" style="width: 27%" type="text" onkeyup="zip_code.search_file();"/></td>
                                    </tr>
                                    <tr><td colspan=3 style="height:8px;"></td></tr>
                                    <tr>
                                        <td id="tdZiptab" colspan="2" style="width: 100%; background: azure">
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