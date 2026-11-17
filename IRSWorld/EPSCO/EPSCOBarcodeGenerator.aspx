<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EPSCOBarcodeGenerator.aspx.cs" Inherits="IRSWorld.EPSCO.EPSCOBarcodeGenerator" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>EPSCO Bulk Barcode Generator</title>
    <link href="../css/Default_min.css" rel="stylesheet" type="text/css" />
    <script src="../js/commons_min.js" type="text/javascript"></script>
    <script src="../js/datagrid_min.js" type="text/javascript"></script>
    <script src="../js/js_sidebar_min.js" type="text/javascript"></script>    
    <script src="../js/EPSCO/jsEPSCOBarcodeGenerator.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
</head>
<body id=theBody onload="page_setup();" onresize="fixdim();">
    <form id="form1" runat="server">
    <div id=divMain style="width:100%; margin:0 auto; display:none; padding-top:30px;">
        <table style="width:100%" cellpadding=0 cellspacing=0>
            <tr>
                <td id=tdMenu style="width:18%; vertical-align: top; text-align:right; border-collapse:collapse;">
                    <div id=divMenu style="width:100%; text-align:right; padding-right: 5px; vertical-align:top;">
                        <script type="text/javascript">
                            var sideA = new Array
                            (
                                new Array(1, 0, 'For Printing', 'javascript:show_for_print();'),
                                new Array(2, 0, 'Close', 'javascript:close_bcgen(); top.show_center();')
                            );
                            
                            var sidemenuA = new js_sidebar('sidemenuA', sideA);
                            document.write(sidemenuA.toString());
                        </script>
                    </div>
                    <a id=ahider href=# onclick='javascript:hide_menu();'>«</a>
                </td>
                <td id=tdMain style="width:82%; overflow:auto; vertical-align:top;" align="center">
                    <div id="divGen" style="width:70%;">
                        <table style="width:100%" cellpadding=0 cellspacing=0>
                            <tr>
                                <td style="width: 100%; vertical-align:top">
                                    <table style="width:100%;" cellpadding="0" cellspacing="0" >
                                        <tr>
                                            <td align="left" valign="bottom" style="vertical-align:bottom; width:0%;height:24px; border-collapse: collapse"> 
                                                <img src="../images/toe.GIF" /></td>
                                            <td class="table_head_dark" style="width:40%; height: 24px;">
                                                GENERATE BARCODES</td>
                                            <td align="left" style="width: 60%; vertical-align:bottom; height: 24px">
                                                <img align="absbottom" src="../images/rend.GIF" />
                                            </td>
                                        </tr>  
                                    </table>                                
                                </td>
                            </tr>
                            <tr>
                                <td style="width:100%" align=left class="table_border">
                                    <table style="width:100%">
                                        <tr>
                                            <td style="width:100%">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%" align=center>
                                                <table style="width: 80%" cellspacing=8>
                                                    <tr>
                                                        <td style="width:40%; text-align:right;" class="cell_desc">Indicate the number of barcodes to generate:</td>
                                                        <td style="width: 60%;" align=left>
                                                            <input id=txtNum2Gen type=text style="width:60px; font-weight:bold; color:red; text-align:right;" maxlength=3 value="0" onblur="blur_num2gen(this.value);" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%" align=center>
                                                <table style="width: 60%" cellspacing=8>
                                                    <tr>
                                                        <td style="width:50%;">
                                                            <input id="btnGenerate" type=button class="button" style="width:90%" value="Generate" onmouseover="this.className='button_over'" onmouseout="this.className='button';" onclick="start_gen();" />
                                                        </td>
                                                        <td style="width:50%;">
                                                            <input id="btnCancel" type=button class="button" style="width:90%" value="Cancel" onmouseover="this.className='warning_over'" onmouseout="this.className='button';" onclick="cancel_gen();" />
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
                
                    <div id="divGrid" style="width:100%;display:none">
                        <table style="width:100%" cellpadding=0 cellspacing=0>
                            <tr>
                                <td id=tdGrid style="width:100%; vertical-align:middle;" align=center>
                                    <table style="width: 80%; border: solit 2px navy" align=center class="table_body">
                                         <tr>
                                            <td style="width:100%;" colspan=2>
                                                <div id=divGridContainer style="width:100%;">
                                                    <!-- show grid here -->
                                                </div>
                                            </td>
                                         </tr>
                                         <tr><td style="width:100%;" colspan=2>&nbsp;
                                             <asp:Button ID="btnHidden" runat="server" Text="ExportToCSV" style="visibility:hidden" OnClick="btnHidden_Click" />
                                         </td></tr>
                                         <tr>
                                            <td style="width:50%;">
                                                <input id="btnPrintGen" type=button class="button" style="width:40%" value="Export" onmouseover="this.className='button_over'" onmouseout="this.className='button';" onclick="print_gen();" />
                                            </td>
                                            <td style="width:50%;">
                                                <input id="btnCloseGen" type=button class="button" style="width:40%" value="Close" onmouseover="this.className='warning_over'" onmouseout="this.className='button';" onclick="close_gen();" />
                                            </td>
                                        </tr> 
                                        <tr><td colspan=2 style="width:100%;">&nbsp;</td></tr>                                  
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
