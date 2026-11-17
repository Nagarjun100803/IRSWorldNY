<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Receipts.aspx.cs" Inherits="IRSWorld.BranchSetup.Receipts" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Receipts</title>
 <link href="../css/Default.css" rel="stylesheet"       type="text/css" />
<script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery-3.5.1.min.js"></script>
 <script src="../js/commons.js" type="text/javascript"></script>
 <script src="../js/jscommonmethod.js" type="text/javascript"></script>
 <script src="../js/BranchSetup/js_receipts_min.js" type="text/javascript"> </script>
</head>
<body id="theBody"  onload="page_setup();" >
<center>
    <form id="form1" runat="server">

    <div id="divBody" style="width:98%;display:block; vertical-align:top; margin-top:30px;">        
        <table id="tblContent" style="width:100%;display:block">
            <tr>
                <td id="tdContent" style="width: 100%; height: 94%; border-collapse:collapse;">
                    <div id="divBranchSetupContent" style="width:100%">
                        <table style="width:100%" cellpadding="0" cellspacing="0" >
                            <tr>
                                <td style="width: 60%; vertical-align: bottom; text-align:left">
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                                <img src="../images/toe.GIF" /></td>    
                                            <td id="tdPageTitle" class="table_head_dark" style="width: 40%; height: 24px">
                                                Receipts</td>
                                            <td align="left" style="width: 60%; height: 24px" valign="bottom">
                                                <img align="absBottom" src="../images/rend.gif" /></td> 
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:100%; border:solid 3px #336699;border-collapse:collapse" class=table_body> <!--colspan=3-->
                                   
                                    <table style="width:100%">
                                       
                                        <tr>
                                        
                                            <td style="width:100%" colspan=2 align=left>
                                                <div>
                                                
                                                
                                                
                                                 <table id="tblBranch" style="width:100%">
                                                 <caption class=table_caption style="text-align:left">Branch</caption>
                                                         <tr>
                                                            <td class=cell_caption style="width: 200px; height: 14px;" align="right">
                                                                Select Branch</td>
                                                            <td>
                                                                <select id="ddlBranches" style="width: 277px" onchange="">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                           
                                                            
                                                         </tr>
                                                         
                                                         <tr>
                                                               <td style="width: 200px">
                                                               </td>
                                                               <td>
                                                         
                                                            <input id="btnSearch" class="cancel_button"  onmouseout="this.className='cancel_button';"
                                                                               onmouseover="this.className='cancel_button_over';" type="button" value="Search" style="width: 19%" onclick="getValidID(document.getElementById('ddlBranches').value);set_visibility('Data');" /></td>
                                                         </tr>
                                                  </table>
                                                 
                                                 <center>
                                                <table id=tblchgrid style="width:100%">
                                               
                                                         
                                                 </table>
                                                 </center>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width:100%" colspan=2 align=left>
                                                <div>
                                                
                                                    <table id = "tblReceiptHeader" style="width:100%">
                                                    <tr>
                                                    </tr>
                                                    <caption class=table_caption style="text-align:left">
                                                        Receipt Header</caption>
                                                        <tr>
                                                            <td class=cell_caption style="height: 22px; width:105px;">
                                                               Header 1:</td>
                                                            <td style="width: 405px" align=left>
                                                                <input id="txtHeader1" type="text"  style="width: 80%;   " onkeyup="check_char_inputted('txtHeader1');" maxlength="65"  />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td class=cell_caption style="height: 22px; width:105px;">
                                                               Header 2:</td>
                                                            <td style="width: 405px" align=left>
                                                                <input id="txtHeader2" type="text"  style="width: 80%;   " onkeyup="check_char_inputted('txtHeader2');" maxlength="65"  />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td class=cell_caption style="height: 22px; width:105px;">
                                                               Name of Branch:</td>
                                                            <td style="width: 405px" align=left>
                                                                <input id="txtBranchName" type="text"  style="width: 80%;   " onkeyup="check_char_inputted('txtBranchName');" maxlength="65"  />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td class=cell_caption style="height: 22px; width:105px;">
                                                                Branch
                                                               Address:</td>
                                                            <td style="width: 405px" align=left>
                                                                <input id="txtAddress" type="text" style="width: 80%;   " onkeyup="check_char_inputted('txtAddress');" maxlength="65"  />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td class=cell_caption style="height: 25px; width:105px;">
                                                               Contact Numbers:</td>
                                                            <td style="width: 405px; height: 25px;" align=left>
                                                                <input id="txtContact" type="text" style="width: 80%;   " onkeyup="check_char_inputted('txtContact');" maxlength="65"  />
                                                            </td>
                                                            <td style="height: 25px"></td>
                                                        </tr>
                                                        <tr>
                                                            <td class=cell_caption style="height: 25px; width:105px;">
                                                               Email:</td>
                                                            <td style="width: 405px; height: 25px;" align=left>
                                                                <input id="txtEmail" type="text"  style="width: 80%;   " maxlength="65"   />
                                                            </td>
                                                            <td style="height: 25px"></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 105px; height: 22px">
                                                                Others 1:</td>
                                                            <td align="left" style="width: 405px">
                                                                <input id="txtcorpaddress" type="text"  style="width: 80%;   " maxlength="65"   /></td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 105px; height: 22px">
                                                                Others 2:</td>
                                                            <td align="left" style="width: 405px">
                                                                <input id="txtcorptelno" type="text"  style="width: 80%;   " maxlength="65"   /></td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                                
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width:100%" colspan=2 align=left>
                                            <table id = "tblValidID" style="width:100%">
                                            <caption class=table_caption style="text-align:left">Valid ID</caption>
                                            <tr>
                                                <td class=cell_caption style="height: 22px; width:105px;">
                                                    Valid ID 1:</td>
                                                <td style="width: 405px" align=left>
                                                    &nbsp;<select id="ddlValid1" style="width: 277px">
                                                        <option selected="selected"></option>
                                                    </select></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td class=cell_caption style="height: 22px; width:105px;">
                                                    Valid ID 2:</td>
                                                <td style="width: 405px" align=left>
                                                    &nbsp;<select id="ddlValid2" style="width: 277px">
                                                        <option selected="selected"></option>
                                                    </select></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td class=cell_caption style="height: 22px; width:105px;">
                                                    Valid ID 3:</td>
                                                <td style="width: 405px" align=left>
                                                    &nbsp;<select id="ddlValid3" style="width: 277px">
                                                        <option selected="selected"></option>
                                                    </select></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td class=cell_caption style="height: 22px; width:105px;">
                                                    Valid ID 4:</td>
                                                <td style="width: 405px" align=left>
                                                    &nbsp;<select id="ddlValid4" style="width: 277px">
                                                        <option selected="selected"></option>
                                                    </select></td>
                                                <td></td>
                                            </tr>
                                            
                                            </table>
                     
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td style="width:100%" colspan=2 align=left>
                                                <table id = "tblPrinters" style="width:100%">
                                                <caption class=table_caption style="text-align:left">Receipt Form</caption>
                                                    <tr>
                                                        <td class=cell_caption style="height: 22px; width:110px;">
                                                            Laser Printer:</td>
                                                        <td style="width: 400px" align=left>
                                                            &nbsp;<input id="optLaser" style="width: 9%" type="radio" onclick="check('Laser');" /></td>
                                                        <td></td>
                                                        
                                                        
                                                    </tr>
                                                     <tr>
                                                        <td class=cell_caption style="height: 22px; width:110px;" >
                                                            Other Printer:</td>
                                                        <td style="width: 400px" align=left>
                                                            &nbsp;<input id="optOther" style="width: 9%" type="radio" tabindex="1" onclick="check('Other');" /></td>
                                                        <td></td>
                                                        
                                                    </tr>
                                                    <tr>
                                                        <td class=cell_caption style="height: 22px; width:110px;">
                                                            Number of Copies:</td>
                                                        <td style="width: 400px" align=left>
                                                            &nbsp;<input id="txtNoCopies" style="width: 25%" type="text" onchange="isInt(document.getElementById('txtNoCopies').value);"  /></td>
                                                        <td></td>
                                                        
                                                    </tr>
                                            
                                            
                                            
                                                </table>
                                            </td>
                                        
                                        
                                        </tr>
                                        <tr>
                                            <td style="width:100%" colspan=2 align=left>
                                                <table id = "tblReceiptType" style="width:100%">
                                                <caption class=table_caption style="text-align:left">Receipt Type</caption>
                                                    <tr>
                                                        <td class=cell_caption style="height: 22px; width:110px;">
                                                            Original Receipt:</td>
                                                        <td style="width: 400px" align=left>
                                                            &nbsp;<input id="optOriginal" style="width: 9%" type="radio" onclick="check_type('Original');" name="ReceiptGroup" value="1" /></td>
                                                        <td></td>
                                                        
                                                        
                                                    </tr>
                                                     <tr>
                                                        <td class=cell_caption style="height: 25px; width:110px;" >
                                                            Tagalog Receipt:</td>
                                                        <td style="width: 400px; height: 25px;" align=left>
                                                            &nbsp;<input id="optTagalog" style="width: 9%" type="radio" tabindex="1" onclick="check_type('Tagalog');" name="ReceiptGroup" value="2" /></td>
                                                        <td style="height: 25px"></td>
                                                        
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 110px; height: 22px">
                                                            RCN Receipt:</td>
                                                        <td align="left" style="width: 400px">
                                                            &nbsp;<input id="optRCN" style="width: 9%" type="radio" tabindex="1" onclick="check_type('RCN');" name="ReceiptGroup" value="3" /></td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 110px; height: 22px">
                                                            Guam Receipt:</td>
                                                        <td align="left" style="width: 400px">
                                                            &nbsp;<input id="optGuam" style="width: 9%" type="radio" tabindex="1" onclick="check_type('Guam');" name="ReceiptGroup" value="4" /></td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <!--added by: RUTH for single page receipt-->
                                                    <tr>
                                                        <td class="cell_caption" style="width: 110px; height: 22px">
                                                            Compressed Receipt:</td>
                                                        <td align="left" style="width: 400px">
                                                            &nbsp;<input id="optCompressed" style="width: 9%" type="radio" tabindex="1" onclick="check_type('Compressed');" name="ReceiptGroup" value="5" /></td>
                                                        <td>
                                                        </td>
                                                    <!--added by: RUTH for single page receipt-->
                                                    </tr>
                                                                                                <tr>
                                                        <td class="cell_caption" style="width: 110px; height: 22px">
                                                            New York Receipt:</td>
                                                        <td align="left" style="width: 400px">
                                                            &nbsp;<input id="optNewYork" style="width: 9%" type="radio" tabindex="1" onclick="check_type('NewYork');" name="ReceiptGroup" value="6" /></td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        
                                        
                                        </tr>
                                        
                                       
                                    </table>
                                                
                                                
                                                <center>
                                                 <table id="tblButtonsMain" cellpadding="0" cellspacing="5" style="width:100%">
                                                       <tr >
                                                           <td align="center" colspan="3" style="width: 100%">
                                                               <table cellpadding="0" cellspacing="5" style="width:80%">
                                                                   <tr>
                                                                       
                                                                       <td style="width: 20%">
                                                                           <input id="btnEdit" class="edit_button" onmouseout="this.className='edit_button';"
                                                                               onmouseover="this.className='edit_button_over';" type="button" value="Edit" onclick="enabled_button('Edit');" /></td>
                                                                       <td style="width: 20%">
                                                                           <input id="btnClear" class="delete_button" onmouseout="this.className='delete_button';"
                                                                               onmouseover="this.className='delete_button_over';" type="button" value="Clear" onclick="clear_text();" /></td>
                                                                       <td style="width: 20%">
                                                                           <input id="btnSave" class="save_button" onmouseout="this.className='save_button';"
                                                                               onmouseover="this.className='save_button_over';" type="button" value="Save" disabled="disabled" onclick="save_receipts();"  /></td>
                                                                       <td style="width: 20%">
                                                                           <input id="btnCancel" class="cancel_button"  onmouseout="this.className='cancel_button';"
                                                                               onmouseover="this.className='cancel_button_over';" type="button" value="Cancel" onclick="enabled_button('Cancel');" disabled="disabled" /></td>
                                                                      <td style="width: 20%">
                                                                           <input id="btnBack" class="cancel_button"  onmouseout="this.className='cancel_button';"
                                                                               onmouseover="this.className='cancel_button_over';" type="button" value="Back" onclick="set_visibility('Branch');" /></td>
                                                                   </tr>
                                                               </table>
                                                             </td>
                                                        </tr>
                                                        
                                                </table>
                                                </center>
                                               
                                </td>
                                
                            </tr>                            
                        </table>            
                    </div>

                    
                </td>
            </tr>  
        </table>
        </div>

        <div id="lblValues"></div>
    </form>
</center>
    <form name="irs_login" id="irs_login" method="post" action="../Main/IRSMain.aspx?load=../Admin/Login.aspx" target="_parent"></form>
    <form name="irs_loader" id="irs_loader" method="post" action="../Main/IRSMain.aspx?load=../BranchSetup/Receipts.aspx" target="_parent"></form>
</body>
</html>
