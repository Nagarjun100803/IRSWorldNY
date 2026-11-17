<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InternetMain.aspx.cs" Inherits="IRSWorld.Internet.InternetMain" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Internet-Main</title>
    <link href="../css/reskin.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/js_sidebar.js"  type="text/javascript"></script>
    <script src = "../js/jsmodalwindow.js"           type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>    
    <script src = "../js/jsgridselector.js"           type="text/javascript"></script>
    <script src = "../js/Internet/jsInternetSideBar.js"  type="text/javascript"></script>
    <script src = "../js/Internet/jsInternetMain.js" type="text/javascript"></script>
    <script src = "../js/Internet/jsInternetGridView.js" type="text/javascript"></script>
    <script type="text/javascript">
            var Internet_Main = new Internet_Main();
    
    </script>
</head>
<body onload="javascript:Internet_Main.construct_side_bar('divtdInternetMainMenu');Internet_Main.load_grid_viewer();Internet_Main.fill_label()">
    <form id="form2" runat="server">
        <div style="width:100%">
            <table style="width:100%" cellpadding="0" cellspacing="5">
                <tr>
                    <td align="center" valign="top" style="width:100%; border-collapse:collapse; height: 380px;" class="content_height" colspan = "2">
                    
                        <div id="divInternetMain" style="width: 100%; height: 100%; display: block">
                            <table style="width:100%">
                                <tr>
                                    <td id = "tdInternetMainMenu" valign="top" style="width:20%; vertical-align:top; padding-top: 30px;"> <!-- border-right: solid 1px #5697c1;"> -->
                                    
                                    <div id = "divtdInternetMainMenu" style="margin-top: 20px;display:block;">
                                        <script type="text/javascript">
//                                            var sider = new js_sidebar('sidemenu', sidebar);
//                                            document.write(sider.toString());
                                        </script>
                                    </div>
                                    <a id=ahider href=# onclick="javascript:Internet_Main.set_side_bar_visibility('divtdInternetMainMenu', 'ahider');">«</a>
                                 </td>            
                                                               
                                    <td style="width: 78%; padding-top: 30px">
                                        <table cellspacing="0" cellpadding="0" style="width:96%; background-color:white">
                                            <tr>
                                                <td align="left" valign="bottom" style="width:1%;height:24px;"> 
                                                    <img src="../images/reskin/toe.GIF" /></td>
                                                <td id="capResult" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse;">Internet</td>
                                                <td align="left" valign="bottom" style="height: 24px"  class="bg_body"> 
                                                    <img align="absbottom" src="../images/reskin/rend.gif" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="width: 100%; height: 100%">
                                                    <table style="width:100%; height: 100%;" class="table_border">
                                                        <tr>
                                                            <td style="width: 100%; height: 8px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100%; height: 100%; vertical-align: top">
                                                                <!-- content starts here -->
                                                                    <table style="width:100%" cellpadding="0" cellspacing="5px">
                                                                        <tr>
                                                                            <td style="width: 100%; height: 100%;"valign="center">
                                                                
                                                                             <div style="width:98%; height: 100%; margin: 0" class="table_body" id="divInternetMainRemitterInfo" >
                                                                                <table id="tblInternetMainRemitter" cellpadding="0" cellspacing="8" style="width:100%; border: solid 1px #394398;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Remitter :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblInternetMainRemName" class="cell_desc" style="font-size: medium;" ></label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Address :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblInternetMainRemAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;" ></label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Contact No :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblInternetMainRemContactNo" class="cell_desc" style="font-size: 15px; font-weight: normal;" > </label></td>
                                                                                    </tr>
                                                                                </table>
                                                                             </div> 
                                                                              <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td> 
                                                                                </tr> 
                                                                           </table> 
                                                                             <div style ="width:98%; height:100%; margin:0; " class="table_body" id="divInternetMainBeneficiary">
                                                                                 <table id="tblInternetMainBeneficiary" cellpadding="0" cellspacing="8" style="width:100%; border:solid 1px #394398;">
                                                                                     <tr>
                                                                                         <td align="left" class="cell_caption" style="width: 15%; height:24px">Beneficiary :&nbsp; </td>
                                                                                         <td style="height: 24px"></td>
                                                                                         <td style="height: 24px"></td>
                                                                                         <td style="height: 24px"></td>
                                                                                     </tr>
                                                                                     <tr>
                                                                                         <td colspan=4 ><div id = "gridInternetMainBenefeciary"></div></td>
                                                                                     </tr>
                                                                                     <tr>
                                                                                         <td colspan=4 ><input id="txtHiddenCIFID"  type="hidden" value="1072102109000001"/></td>
                                                                                     </tr>
                                                                                 </table>
                                                                              </div>
                                                                               <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td> 
                                                                                </tr> 
                                                                              </table> 
                                                                              <div style ="width:98%; height:30%; margin:0;" class="table_body" id="divInternetMainTransaction">
                                                                                 <table id="tblInternetMainTransaction" cellpadding="0" cellspacing="8" style="width:100%; border:solid 1px #394398;">
                                                                                     <tr>
                                                                                         <td class="cell_caption" style="width: 15%; height:23px">
                                                                                             Transactions :&nbsp; </td>
                                                                                         <td></td>
                                                                                         <td></td>
                                                                                         <td></td>
                                                                                     </tr>
                                                                                     <tr>
                                                                                         <td colspan=4  align="left" style="width: 305px; height: 24px;" > &nbsp; </td>
                                                                                         
                                                                                     </tr>
                                                                                     <%--</div>--%>
                                                                                 </table>
                                                                                 </div>
                                                                              <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td> 
                                                                                </tr> 
                                                                             </table>
                                                                             
                                                                       <%--      <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td> 
                                                                                </tr> 
                                                                             </table>--%>
                                                                            </td>
                                                                        </tr>
                                                                       <tr>
                                                                            <td style="height: 28px; width: 100%" valign="top" ><input style="height: 28px; width: 10%" type="button" value="New" class="button" onclick="javascript:RemTran_GFMC.load_transaction_view_details();" onmouseover="this.className='button_over';" onmouseout="this.className='button';"/>
                                                                                                                                <input style="height: 28px; width: 10%" type="button" value="Cancel" class="button" onclick="javascript:RemTran_GFMC.load_transaction_view_details();" onmouseover="this.className='button_over';" onmouseout="this.className='button';"/></td>
                                                                        </tr>
     
                                                                    </table>

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
