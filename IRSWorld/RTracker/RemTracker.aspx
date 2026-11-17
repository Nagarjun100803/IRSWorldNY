<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemTracker.aspx.cs" Inherits="IRSWorld.RTracker.RemTracker" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Remittance Tracker</title>
    <link href="../css/Default.css" rel="stylesheet"   type = "text/css" />
    <link href="../css/cssmodals.css" rel="stylesheet" type = "text/css" />
    <script src="../js/jsCommons.js"                   type = "text/javascript"></script>
    <script src="../js/jscommonmethod.js"              type = "text/javascript"></script>
    <script src="../js/jshttprequest.js"               type = "text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"               type = "text/javascript"></script>
    <script src="../js/RTracker/jsRemTracker.js"                 type = "text/javascript"></script>
    <script type="text/javascript">
        var rTracker = new remTracker();
    </script>
</head>
<body onload = "javascript:rTracker.init();" onkeyup="javascript:rTracker.on_key_press();">
     <center>
        <form id="form2" runat="server">

           <div id="divPopProcess" style="width:250px; height:100px; background:White; z-index: 101; position: absolute; left: 150px;
            color: darkblue; padding: 10px; display: none; font-family: Arial;
            top: 741px;">
            <table style="width:100%; height:100%;">
                <tr>
                    <td>
                        <label id="lblLoadingMsg" style="text-align:center; color:Chocolate ">Processing request, Please wait... </label>
                        <div id="divLoadingMsg">
                            <img src="../images/ajaxloader.gif" id="imgAjaxIcon" />
                        </div>
                    </td>
                </tr>
            </table>
           </div>          

            <div id = "DIVFRONT">
                <table background="../images/tracker.jpg" style="width: 573px; height: 169px; margin-top: 30px; color: #000000;" border="0">
                    <tr>
                        <td style="width: 224px; height: 211px">
                        </td>
                        <td style="height: 211px; width: 333px;">
                        </td>
                    </tr> 
                    <tr>
                        <td style="width: 224px; height: 8px;" align="right">
                            <span><strong>Transaction Ref. No. :</strong></span></td>
                        <td align="left" style="width: 333px; height: 8px;">
                            <input id="txtSearchTrxnNo" type="text" maxlength="200" style="text-align: left; width: 83%; "/></td>
                    </tr> 
                    <tr>
                        <td style="width: 224px; height: 7px;">
                        </td>
                        <td style="width: 333px; height: 7px;" align="right"><input id="btnSubmit"   class = "button" onclick = "javascript:rTracker.btnSubmit_click();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Submit" style="width: 18%"/>
                            &nbsp;<input id="btnReset"   class = "button" onclick = "javascript:rTracker.btnReset_click();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Reset" style="width: 18%"/>&nbsp; &nbsp; &nbsp; &nbsp;
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;</td>
                    </tr>         
                    <tr>
                        <td style="width: 224px; height: 70px;">
                        </td>
                        <td style="width: 333px; height: 70px;">
                        </td>
                    </tr>   
                    <tr>
                        <td style="width: 224px; height: 21px;">
                        </td>
                        <td style="width: 333px; height: 21px;">
                        </td>
                    </tr>                                                                           
                </table>  
            </div>
            <div id="DIVMAIN">
                <table id="tbl_main" cellpadding="0" cellspacing="5" style="margin-top: 30px; width: 54%; height: 200px; color: #000000;">                
                    <tr>
                        <td style="width: 100%; height: 100%;" align="center" valign="top">
                            <table cellpadding="0" cellspacing="0" >
                                <tr>
                                    <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                        <img src="../images/toe.GIF" /></td>    
                                    <td id='tdHeader' class="table_head_dark" style="width: 50%; height: 24px">
                                        Remittance Tracker</td>
                                    <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                        <img align="absBottom" src="../images/rend.gif" /></td> 
                                 </tr>                                                                              
                            </table>                                                        
                            <table id="Table2" width="100%" cellpadding="0" cellspacing="5" style="border: solid 1px #336699; background:azure;">
                                <tr>
                                    <td id='tdSearchHeader' class="table_caption" colspan="5" style="color: black; text-indent: 5px; text-align: left">
                                    </td>
                                </tr>
                                <tr id="trTrxnNo">
                                    <td class="cell_caption" style="width: 33%; height: 25px" >
                                        Transaction Ref. No. :&nbsp;</td>
                                    <td colspan="3" align="left" style="height: 25px;">
                                        <input id="txtTrxnNo" type="text" maxlength="200" style="text-align: left; width: 98%;"/>
                                        </td>
                                </tr>
                                <tr id="trAmount">
                                    <td class="cell_caption" style="width: 33%; height: 25px" >
                                        Amount :&nbsp;</td>
                                    <td colspan="3" align="left" style="height: 25px;">
                                        <input id="txtAmount" type="text" maxlength="200" style="text-align: left; width: 98%;"/>
                                        </td>
                                </tr>
                                <tr id="trCurr">
                                    <td class="cell_caption" style="width: 33%; height: 25px" >
                                        Currency :&nbsp;</td>
                                    <td colspan="3" align="left" style="height: 25px;">
                                        <input id="txtCurr" type="text" maxlength="200" style="text-align: left; width: 98%;"/>
                                        </td>
                                </tr>                                                            
                                <tr id="trDate">
                                    <td class="cell_caption" style="width: 33%; height: 25px" >
                                        Date :&nbsp;</td>
                                    <td colspan="3" align="left" style="height: 25px;">
                                        <input id="txtDate" type="text" maxlength="200" style="text-align: left; width: 98%;"/>
                                        </td>
                                </tr>             
                                <tr id="trStatus">
                                    <td class="cell_caption" style="width: 33%; height: 25px" >
                                        Status :&nbsp;</td>
                                    <td colspan="3" align="left" style="height: 25px;">
                                        <input id="txtStatus" type="text" maxlength="200" style="text-align: left; width: 98%;"/>
                                        </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td colspan="3" align="right" style="height: 25px;">
                                        <input id="btnReset2"   class = "button" onclick = "javascript:rTracker.btnReset_click();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Back" style="width: 32%"/>&nbsp;
                                        </td>
                                </tr>                                
                            </table>                 
                        </td> 
                    </tr>    
                </table>            
            </div>  
<%--            
            <table id="tbl_main" width="100%" cellpadding="0" cellspacing="5" style="height:100%;margin-top: 30px;">                
                <tr>
                    <td style="width: 79%" align="center" valign="top">
                        <div style= "display:block; height:220px;">
                            <table width="50%" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="height: 43px">
                                        <table cellpadding="0" cellspacing="0" >
                                            <tr>
                                                <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                                    <img src="../images/toe.GIF" /></td>    
                                                <td id='tdHeader' class="table_head_dark" style="width: 50%; height: 24px">
                                                    Remittance Tracker</td>
                                                <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                                    <img align="absBottom" src="../images/rend.gif" /></td> 
                                             </tr>                                                                              
                                        </table>    
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <%--<td colspan="2" style="background:azure; height: 90px; width: 467px;">
                                    <td colspan="2" style="background:azure; height: 90px; width: 100%;"> 
                                    
                                    <!--Search--> 
                                    
                                        <table style="width:100%; border:solid 3px #336699;" >
                                            <tr>
                                                <td style="width: 100%; height: 136px;">
                                                    <div id="DIVGFCSEARCH" style="display:none">
                                                        <table id="Table2" width="100%" cellpadding="0" cellspacing="5" style="border: solid 1px #336699; background:azure;">
                                                            <tr>
                                                                <td id='tdSearchHeader' class="table_caption" colspan="5" style="color: black; text-indent: 5px; text-align: left">
                                                                </td>
                                                            </tr>
                                                            <tr id="trTrxnNo">
                                                                <td class="cell_caption" style="width: 33%; height: 25px" >
                                                                    Transaction Ref. No. :&nbsp;</td>
                                                                <td colspan="3" align="left" style="height: 25px;">
                                                                    <input id="txtTrxnNo" type="text" maxlength="200" style="text-align: left; width: 98%;"/>
                                                                    </td>
                                                            </tr>
                                                           
                                                            <tr id="trAmount">
                                                                <td class="cell_caption" style="width: 33%; height: 25px" >
                                                                    Amount :&nbsp;</td>
                                                                <td colspan="3" align="left" style="height: 25px;">
                                                                    <input id="txtAmount" type="text" maxlength="200" style="text-align: left; width: 98%;"/>
                                                                    </td>
                                                            </tr>
                                                            <tr id="trCurr">
                                                                <td class="cell_caption" style="width: 33%; height: 25px" >
                                                                    Currency :&nbsp;</td>
                                                                <td colspan="3" align="left" style="height: 25px;">
                                                                    <input id="txtCurr" type="text" maxlength="200" style="text-align: left; width: 98%;"/>
                                                                    </td>
                                                            </tr>                                                            
                                                            <tr id="trDate">
                                                                <td class="cell_caption" style="width: 33%; height: 25px" >
                                                                    Date :&nbsp;</td>
                                                                <td colspan="3" align="left" style="height: 25px;">
                                                                    <input id="txtDate" type="text" maxlength="200" style="text-align: left; width: 98%;"/>
                                                                    </td>
                                                            </tr>             
                                                            <tr id="trStatus">
                                                                <td class="cell_caption" style="width: 33%; height: 25px" >
                                                                    Status :&nbsp;</td>
                                                                <td colspan="3" align="left" style="height: 25px;">
                                                                    <input id="txtStatus" type="text" maxlength="200" style="text-align: left; width: 98%;"/>
                                                                    </td>
                                                            </tr>                                                                                
<%--                                                           <tr>
                                                                <td width ="20%" align="right" style="height: 22px" colspan="4">
                                                                    <input id="btnSubmit"   class = "button" onclick = "javascript:rTracker.btnSubmit_click();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Submit" style="width: 18%"/>
                                                                    <input id="btnReset"   class = "button" onclick = "javascript:rTracker.btnReset_click();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Reset" style="width: 18%"/>&nbsp;</td>
                                                            </tr>                                                           
                                                        </table>
                                                    </div>                                                           
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>                            
                        </div>
                    </td> 
                </tr>    
            </table> --%>
        </form>
    </center>     
</body>
</html>
