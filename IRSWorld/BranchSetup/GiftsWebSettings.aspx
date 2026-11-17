<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GiftsWebSettings.aspx.cs" Inherits="IRSWorld.BranchSetup.GiftsWebSettings" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>GiftsWeb Settings</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/js_generic.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>    
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/BranchSetup/js_giftswebsettings.js" type="text/javascript"></script>
    <script type="text/javascript">
    
        var gws = new gws();
        
        // added by caloy to implement resize and scroller 
        // added onresize in body tag.
        // Modified div (Machine_Setup) to put overflow:auto in style
       function fixht()
        {
            document.getElementById('GWS').style.height = (top.document.getElementById('divCenter').offsetHeight - 50) + 'px';
            document.getElementById('GWS').style.width = (top.document.getElementById('divCenter').offsetWidth - 3) + 'px';
        }
        
        function fixmain()
        {
            var brmain = false;
            try
            {
                 brmain = parent.is_branchsetup_main();
            }
            catch(e){}
            
            if (!brmain)
            {
                document.getElementById('theBody').onresize = fixht;
                document.getElementById('GWS').style.overflow = 'auto';
                
                fixht();
            }        
        }                
                
    </script>
    
</head>
<body id="theBody" onload = "javascript:gws.load_all_dropdownlist();gws.load_grid_viewer();"  onresize=fixmain();>
<br />
    <form id="form1" runat="server">
    <div>
    <center>
        <div id="GWS" style="display: block;padding-top: 30px; ;overflow:auto;">
             <table cellpadding="0" cellspacing="0" style="width:30%">
                <tr>
                    <td> 
                        <table cellpadding="0" cellspacing="0" >
                            <tr>
                                <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                    <img src="../images/toe.GIF" />
                                </td>    
                                <td class="table_head_dark" style="width: 50%; height: 24px">Branch Setup</td>
                                <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                    <img align="absBottom" src="../images/rend.gif" />
                                </td>   
                          </tr>
                        </table>    
                    </td>                    
                </tr>
                <tr>
                    <td colspan="2" style="width: 625px">
                        <table id="GWSFIELD" style="width: 86%;border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;
                            border-bottom: #336699 3px solid;">
                            <tr >
                                <td colspan="2" style="width: 900px; height: 100%;">
                                    <table id="tblGWS" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                        border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; text-align: center; width: 794px; height: 100%;">
                                        <tr>
                                            <td class="table_caption" colspan="5" style="width: 125px; color: black; text-indent: 5px;
                                                text-align: left; height: 25px;">Gifts Web Settings
                                            </td>
                                        </tr>
                                        <tr>
                                           <td style="height: 40px; width: 900px; text-align: center; background-color: azure;">
                                                <table id="tblGWSField" cellspacing="8" cellpadding="0" style="width:100%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; text-align: right; height: 21px;">Region :</td>
                                                        <td style="WIDTH: 25%; height: 21px;" align="left">
                                                            <select id="ddRegion" onchange="javascript:gws.reload_branches()" style="width:98%">
                                                                <option selected="selected"></option>
                                                            </select>
                                                        </td>
                                                        <td class="cell_caption" style="width: 25%; text-align: right; height: 21px;">Branch :</td>
                                                        <td align="left" style="width: 25%; height: 21px;">
                                                             <select id="ddBranch" style="width:98%">
                                                                 <option selected="selected"></option>
                                                             </select>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; text-align: right; height: 21px;">Domestic :</td>
                                                        <td align="left" style="width: 15%; color:Red">
                                                            &nbsp;<input id="chkIsDomestic" style="width: 12%" type="checkbox";/><label id="astDomestic"></label></td>
                                                    </tr>
                                               </table>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="width: 900px; height:100%">
                                    <table id="Table1" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                        border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; text-align: center; width: 794px; height: 150px;">
                                        <tr>
                                            <td class="table_caption" colspan="2" style="width: 125px; color: black; text-indent: 5px;
                                                text-align: left; height: 25px;">FTP</td>
                                        </tr>
                                        <tr>
                                            <td style="height: 70px; width: 900px; text-align: center; background-color: azure;">
                                                <table id="tbl_FTP" cellspacing="8" cellpadding="0" style="width:100%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; text-align: right;" >FTP Server Name / IP Address : </td>
                                                        <td style="WIDTH: 25%" align="left" >
                                                            <input id="txtUrl" type="text" style="width:98%"/>
                                                        </td>
                                                        <td class="cell_caption" style="width: 25%; text-align: right;" >Folder Name :</td>
                                                        <td style="width: 25%" align="left">
                                                            <input id="txtFolderName" type="text" style="width:98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; text-align: right; height: 21px;">UserName :</td>
                                                        <td style="width: 25%; text-align: center; height: 21px;">
                                                            <input id="txtUserName" type="text" style="width:98%"/>
                                                        </td>
                                                        <td class="cell_caption" align="left" style="width: 25%; text-align: right; height: 21px;">Password :</td>
                                                        <td align="left" style="width: 25%; height: 21px;">
                                                            <input id="txtPassword" type="password" style="width:98%" />
                                                        </td>
                                                    </tr>  
                                               </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                             <tr>
                                <td colspan="2" style="width: 900px; height: 280px;">
                                    <table id="Table4" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                        border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; text-align: center; width: 794px; height: 150px;">
                                        <tr>
                                            <td class="table_caption" colspan="5" style="width: 125px; color: black; text-indent: 5px;
                                                text-align: left; height: 25px;">Extract
                                            </td>
                                        </tr>
                                        <tr>
                                           <td style="height: 116px; width: 900px; text-align: center; background-color: azure;">
                                                <table id="tbl_Extract" cellspacing="8" cellpadding="0" style="width:100%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; text-align: right;">Currency :</td>
                                                        <td style="WIDTH: 25%" align="left">
                                                            <select id="ddCurrency" style="width:100%">
                                                                <option selected="selected"></option>
                                                            </select>
                                                        </td>
                                                        <td class="cell_caption" style="width: 25%; text-align: right; height: 21px;">BranchCode :</td>
                                                        <td align="left" style="width: 25%; height: 21px;">
                                                            <input id="txtBranchCode" type="text" style="width:98%"/>
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 21px; text-align: right">Branch Prefix : </td>
                                                        <td align="left" style="width: 25%;">
                                                            <input id="txtBranchPrefix" type="text" style="width:98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 25%; height: 21px; text-align: right">Account Officer :
                                                        </td>
                                                        <td align="left" style="width: 25%; ">
                                                            <input id="txtAccountOfficer" type="text" style="width:98%"/>
                                                        </td>
                                                     </tr>
                                                     <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 22px; text-align: right">Account Officer Tel. No. :</td>
                                                        <td align="left" style="width: 25%; height: 22px;">
                                                            <input id="txtAccOfficerTel" type="text" style="width:98%"/>
                                                        </td>
                                                        <td class="cell_caption" style="width: 25%; height: 21px; text-align: right">Account Officer Email :</td>
                                                        <td align="left" style="width: 25%; ">
                                                            <input id="txtAccOfficerEmail" type="text" style="width:98%"/>
                                                        </td>
                                                     </tr>
                                                     <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 21px; text-align: right">Compliance Officer :</td>
                                                        <td align="left" style="width: 25%; ">
                                                            <input id="txtComplianceOfficer" type="text" style="width:98%"/>
                                                        </td>
                                                        <td class="cell_caption" style="width: 25%; height: 21px; text-align: right">Compliance Officer Tel. No. :</td>
                                                        <td align="left" style="width: 25%; ">
                                                            <input id="txtCompOfficerTel" type="text" style="width:98%" />
                                                        </td>
                                                     </tr>
                                                     <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 21px; text-align: right">Compliance Officer Email :</td>
                                                        <td align="left" style="width: 25%; ">
                                                            <input id="txtCompOfficerEmail" type="text" style="width:98%"/>
                                                        </td>
                                                        <td class="cell_caption" style="width: 25%; height: 21px; text-align: right">Extract Runtime (HH:MM - military time) :</td>
                                                        <td align="left" style="width: 25%; ">
                                                            <input id="txtExtractRuntime" type="text" style="width:98%" onkeypress="javascript:gws.check_ExtractTime()"/>
                                                        </td>
                                                     </tr>
                                                </table>                                                
                                                <table cellpadding="0" cellspacing="5" style="width:300px" id="tblbuttons">
                                                    <tr>
                                                        <td style="width: 25%; height: 22px;">
                                                            <input id="btnAdd" class="button" onclick = "javascript:gws.insert_record();" onmouseout="this.className='button';"
                                                                onmouseover="this.className='button_over';" type="button" value="  Add  " /></td>
                                                        <td style="width: 25%; height: 22px;">
                                                            <input id="btnEdit" class="button" onclick = "javascript:gws.update_record();" onmouseout="this.className='button';"
                                                                onmouseover="this.className='button_over';" type="button" value= "  Edit  " disabled="disabled" /></td>
                                                        <td style="width: 25%; height: 22px;" ><input id="btnDelete" class="button" onclick = "javascript:gws.delete_record();" onmouseout="this.className='button';"
                                                                onmouseover="this.className='button_over';" type="button" value= "Delete" disabled="disabled" /></td>
                                                        <td style="width: 25%; height: 22px;" >
                                                            <input id="btnSave" class="button" onclick = "javascript:gws.commit_record();"  onmouseout="this.className='button';"
                                                                onmouseover="this.className='button_over';" type="button" value=" Save " disabled ="disabled"/></td>
                                                        <td style="width: 25%; height: 22px;" >
                                                            <input id="btnCancel" class="button" onclick = "javascript:gws.rollback_record();" onmouseout="this.className='button';"
                                                                onmouseover="this.className='button_over';" type="button" value="Cancel" disabled ="disabled" /></td>
                                                    </tr>
                                                </table>
                                                    <input id="txtHiddenSeqId"  type="hidden" style="width: 2%" /></td>
                                            </tr>
                                    </table>                                                
                                    <div id='GWSGRID'></div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </center>
    </div>
    </form>    
</body>
</html>