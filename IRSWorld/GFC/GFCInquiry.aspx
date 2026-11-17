<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GFCInquiry.aspx.cs" Inherits="IRSWorld.GFC.GFCInquiry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>GFC Inquiry</title>
    <link href="../css/Default.css" rel="stylesheet"   type = "text/css" />
    <link href="../css/cssmodals.css" rel="stylesheet" type = "text/css" />
    <script src="../js/jsCommons.js"                   type = "text/javascript"></script>
    <script src="../js/jscommonmethod.js"              type = "text/javascript"></script>
    <script src="../js/jshttprequest.js"               type = "text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"               type = "text/javascript"></script>
    <script src="../js/GFC/jsGFCInquiry.js"                 type = "text/javascript"></script>
    <script src="../js/jsgridselector.js"                   type="text/javascript"></script>     
    <script type="text/javascript">
        var inq = new GFCInquiry();
    </script>
</head>
<body onload = "javascript:inq.init();" onkeyup="javascript:inq.on_key_press();">
     <center>
        <form id="form2" runat="server">

           <div id="divPopProcess" style="width:250px; height:100px; background:White; z-index: 101; position: absolute; left: 454px;
            color: darkblue; padding: 10px; display: none; font-family: Arial;
            top: 688px;">
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
          
          <div id="DIVFRONTPAGE">
            <table background="../images/GFMCard.jpg" style="width: 819px; height: 463px; margin-top: 30px;">
                <tr>
                    <td style="height: 244px; width: 337px;"></td>
                    <td style="height: 244px; width: 484px;"></td>                    
                </tr> 
                <tr>
                    <td style="height: 1px; width: 337px;"></td>
                    <td style="height: 1px; width: 484px;"></td>                    
                </tr> 
                <tr>
                    <td style="width: 337px" align="right">
                        <strong>
                        GFC Number*</strong> :&nbsp;</td>
                    <td style="width: 484px" align="left"><input id="txtGFCNo" type="text" maxlength="200" style="text-align: left; width: 59%;"/></td>                    
                </tr> 
                <tr>
                    <td style="width: 337px" align="right">
                        <strong>
                        ATM / INQUIRY PIN*</strong> :&nbsp;</td>
                    <td style="width: 484px" align="left"><input id="txtPIN" type="password" maxlength="200" style="text-align: left; width: 59%;"/></td>                    
                </tr> 
                <tr>
                    <td style="width: 337px" align="right">
                        <span style="color: #ccffff"><em>Note* (16 - digit card number)</em></span></td>
                    <td style="width: 484px" align="left">
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp;<input id="btnSubmit"   class = "button" onclick = "javascript:inq.btnSubmit_click();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Submit" style="width: 18%"/>&nbsp;
                        <input id="btnReset"   class = "button" onclick = "javascript:inq.btnReset_click();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Reset" style="width: 18%"/></td>                    
                </tr>
                <tr>
                    <td style="width: 337px; height: 21px;"></td>
                    <td style="width: 484px; height: 21px;"></td>                    
                </tr>        
                <tr>
                    <td style="width: 337px"></td>
                    <td style="width: 484px"></td>                    
                </tr>   
                <tr>
                    <td style="width: 337px"></td>
                    <td style="width: 484px"></td>                    
                </tr>   
                <tr>
                    <td style="width: 337px"></td>
                    <td style="width: 484px"></td>                    
                </tr>   
                <tr>
                    <td style="width: 337px; height: 5px"></td>
                    <td style="width: 484px; height: 5px"></td>                    
                </tr>   
            </table> 
          </div>           

          <div id = "DIVMAIN">
            <table id="tbl_main" cellpadding="0" cellspacing="5" style="width:100%; height:100%;margin-top: 30px; display:block;">                
                <tr>
                    <td style="width: 79%" align="center" valign="top">
                        <div style= "display:block; height:220px;">
                            <table cellspacing="0" cellpadding="0" style="width: 49%">
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" >
                                            <tr>
                                                <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                                    <img src="../images/toe.GIF" /></td>    
                                                <td id='tdHeader' class="table_head_dark" style="width: 50%; height: 24px">
                                                    GFC INQUIRY</td>
                                                <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                                    <img align="absBottom" src="../images/rend.gif" /></td> 
                                             </tr>                                                                              
                                        </table>    
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td colspan="2" style="background:azure; height: 95px; width: 471px;">
                                    
                                    <!--Search--> 
                                    
                                        <table style="width:100%; border:solid 3px #336699;" >
                                            <tr>
                                                <td style="width: 100%; height: 136px;">
                                                    <%--<div id="DIVGFCSEARCH" style="display:none">
                                                        <table id="Table2" width="100%" cellpadding="0" cellspacing="5" style="border: solid 1px #336699; background:azure;">
                                                            <tr>
                                                                <td id='tdSearchHeader' class="table_caption" colspan="5" style="color: black; text-indent: 5px; text-align: left">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td id="Td1" colspan="4" width="70%"class="comment" align="left">
                                                                &nbsp;<label id="Label1">Note* (16 - digit card number)</label></td>
                                                            </tr>   
                                                            <tr>
                                                                <td class="cell_caption" style="width: 20%; height: 25px" >
                                                                    GFC Number* :&nbsp;</td>
                                                                <td colspan="3" align="left" style="height: 25px;">
                                                                    <input id="txtGFCNo" type="text" maxlength="200" style="text-align: left; width: 98%;"/>
                                                                    </td>
                                                            </tr>
                                                           
                                                            <tr id="tr1">
                                                                <td class="cell_caption" style="width: 20%; height: 25px" >
                                                                    ATM / INQUIRY PIN* :&nbsp;</td>
                                                                <td colspan="3" align="left" style="height: 25px;">
                                                                    <input id="txtPIN" type="text" maxlength="200" style="text-align: left; width: 98%;"/>
                                                                    </td>
                                                            </tr>
                                                           <tr>
                                                                <td width ="20%" align="right" style="height: 22px" colspan="4">
                                                                    <input id="btnSubmit"   class = "button" onclick = "javascript:inq.btnSubmit_click();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Submit" style="width: 18%"/>
                                                                    <input id="btnReset"   class = "button" onclick = "javascript:inq.btnReset_click();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Reset" style="width: 18%"/>&nbsp;</td>
                                                            </tr>                                                            
                                                        </table>
                                                    </div>--%>
                                                    <!--Details-->
                                                    <div id="DIVGFCDETAILS" style="display:none">
                                                        <table cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                            <tr>
                                                                <td id='td2' class="table_caption" colspan="5" style="color: black; text-indent: 5px; text-align: left">
                                                                    Account Balance</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 31%; height: 25px" >
                                                                    Global Filipino Card :&nbsp;</td>
                                                                <td colspan="3" align="left" style="height: 25px;">
                                                                    <input id="txtGFCNum" type="text" maxlength="200" style="text-align: left; width: 98%;"/>
                                                                    </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 31%; height: 25px" >
                                                                    Available Balance :&nbsp;</td>
                                                                <td colspan="3" align="left" style="height: 25px;">
                                                                    <input id="txtAvailableBal" type="text" maxlength="200" style="text-align: left; width: 98%;"/>
                                                                    </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 31%; height: 25px" >
                                                                    Currency Code :&nbsp;</td>
                                                                <td colspan="3" align="left" style="height: 25px;">
                                                                    <input id="txtCurr" type="text" maxlength="200" style="text-align: left; width: 98%;"/>
                                                                    </td>
                                                            </tr>                                                            
                                                           <tr>
                                                                <td width ="20%" align="right" style="height: 22px" colspan="4">
                                                                    <input id="btnShowTrxn"   class = "button" onclick = "javascript:inq.btnShowTrxn_click();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Display Transaction(s)" style="width: 35%"/>
                                                                    <input id="btnPrint"   class = "button" onclick = "javascript:inq.btnPrint_click();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Print" style="width: 18%"/>
                                                                    <input id="btnBack"   class = "button" onclick = "javascript:inq.btnBack_click();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Back" style="width: 18%"/>&nbsp;</td>
                                                            </tr>                                                            
                                                        </table>
                                                    </div>
                                                    <!--GRID-->
                                                    <div id = "DIVGRIDTRXNHISTORY" style= "display:none">
                                                        <table id="Table20" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                            <tr>
                                                                <td class="table_caption" colspan="5" style="color: black; text-indent: 5px; text-align: left">
                                                                    Transaction History</td>
                                                            </tr>
                                                            <tr>
                                                                <td id="tdTrxnHistory" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 19px;" colspan="5"></td>
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
            </table>           
          </div>

            <div id="DIVREPORTVIEW" style="display:none">                
                <table id="tbReportView" cellpadding="0" cellspacing="5" style="width:500px; background:azure; border-top-width: 1px; border-left-width: 1px; border-left-color: #336699; border-bottom-width: 1px; border-bottom-color: #336699; border-top-color: #336699; border-right-width: 1px; border-right-color: #336699;">
                    <tr>
                        <td id='td3' class="table_caption" colspan="5" style="color: black; text-indent: 5px; text-align: left; height: 26px;">
                            Account Balance</td>
                    </tr>
                    <tr>
                        <td class="cell_caption" style="width: 31%; height: 25px" >
                            Global Filipino Card :&nbsp;</td>
                        <td colspan="3" align="left" style="height: 25px;">
                            <input id="txtRepCardNo" type="text" maxlength="200" style="text-align: left; width: 98%;"/>
                            </td>
                    </tr>
                    <tr>
                        <td class="cell_caption" style="width: 31%; height: 25px" >
                            Available Balance :&nbsp;</td>
                        <td colspan="3" align="left" style="height: 25px;">
                            <input id="txtRepBal" type="text" maxlength="200" style="text-align: left; width: 98%;"/>
                            </td>
                    </tr>
                    <tr>
                        <td class="cell_caption" style="width: 31%; height: 25px" >
                            Currency Code :&nbsp;</td>
                        <td colspan="3" align="left" style="height: 25px;">
                            <input id="txtRepCurr" type="text" maxlength="200" style="text-align: left; width: 98%;"/>
                            </td>
                    </tr>                    
                    <tr>
                        <td class="table_caption" colspan="5" style="color: black; text-indent: 5px; text-align: left; height: 26px;">
                            Transaction History</td>
                    </tr>
                    <tr>
                        <td id="tdRepGrid" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 19px;" colspan="5"></td>
                    </tr>                                                            
                </table>
           </div>               
        </form>
    </center>     
</body>
</html>
