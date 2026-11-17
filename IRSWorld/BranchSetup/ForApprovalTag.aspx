<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForApprovalTag.aspx.cs" Inherits="IRSWorld.BranchSetup.ForApprovalTag" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>For Approval Tag</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/BranchSetup/jsForApproval.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var commons = new common_method();        
        var forapprovaltagging = new forapprovaltagging();
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 20%;
        }
        .auto-style2 {
            height: 6px;
            width: 20%;
            align-items: center;
        }
        .auto-style3 {
            width: 100%;
        }
        
    </style>
</head>
<body onresize="fixdim();" onload="page_setup();" style="height: 300px; align-content: center;">
    <div>
        <center>
            <form id="form1" runat="server">
            <div id="divBody" style="width: 30%; display: block; vertical-align: middle; margin-top: 50px;">
            <table id="tblContent" style="width: 100%; display: block">
                    <tr>
                        <td id="tdContent" style="border-collapse: collapse;" class="auto-style3" align="center">
                         <table id="divBranchSetupContent" style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                        <img src="../images/toe.GIF" /></td>
                                    <td id="tdPageTitle" class="table_head_dark" style="width: 70%; height: 24px">
                                        For Approval Tag</td>
                                    <td align="left" style="width: 30%; height: 24px" valign="bottom">
                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                </tr>
                          </table>
                          <table style="width:100%;border: solid 3px #336699; border-collapse: collapse;" class="table_body">
                            
                            <tr>
                                
                                <td class="cell_caption" style="font-weight: bold; width: 80%; text-align: left; height: 6px; padding-left: 35px;">
                                   Branch Code :</td>
                                <td class="cell_desc auto-style1" style="font-weight: bold; width: 10%; height: 6px; text-align: left">
                                                                    
                                <select id="cmbBranches"  onchange="javascript:forapprovaltagging.oncheckChanged();">
                                    <option selected="selected"></option>
                                </select>
                                </td>
                            </tr>
                            <tr>
                                <td class="cell_caption" style="font-weight: bold; width: 80%; text-align: left; height: 6px; padding-left: 35px;">
                                   Apply to Same Negotiating Branch :</td>
                                <td class="auto-style2">
                                    <input id="chkApplyToAll" type="checkbox" style="width:10%" />
                                </td>    
                            </tr>
                            <tr>
                                <td class="cell_caption" style="font-weight: bold; width: 80%; text-align: left; height: 6px; padding-left: 35px;">
                                   For Approval Tag :</td>
                                <td class="auto-style1">
                                    <input id="chkForApproval" type="checkbox" style="width:10%" />
                                </td>    
                            </tr>
                            <tr>
                                <td style="text-align:center" class="cell_desc" colspan=2>
                                    <input id="btnCommit" type="button" value="Apply Changes" onclick="javascript:forapprovaltagging.apply_changes();" style="width: 60%" />
                                </td> 
                                
                            </tr>
                          </table>
                        </td>
                    </tr>
                   
            </table>
            </div>
    </form>
        </center>
    </div>
    
</body>
</html>
