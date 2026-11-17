<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Faultlog.aspx.cs" Inherits="IRSWorld.Faultlog.Faultlog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Faultlog</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />

    <script src="../js/jspad/jspad.js" type="text/javascript"></script>

    <script src="../js/jsCommons.js" type="text/javascript"></script>

    <script src="../js/js_generic.js" type="text/javascript"></script>

    <script src="../js/jscommonmethod.js" type="text/javascript"></script>

    <script src="../js/jshttprequest.js" type="text/javascript"></script>

    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>

    <script src="../js/jsgridselector.js" type="text/javascript"></script>

    <script src="../js/Faultlog/jsFaultlog.js" type="text/javascript"></script>

    <script type="text/javascript">
            var Faultlog_Status = new Faultlog_Status();
    </script>

</head>
<body onload="javascript:Faultlog_Status.load();">
    <form id="form1" runat="server">
        <div id="divFaultlog" style="width: 98%; display: none; text-align: left; vertical-align: top;
            margin-top: 20px;">
            <table id="tblFaultlog" style="width: 100%; display: block">
                <tr>
                    <td style="vertical-align: top; width: 20%">
                        <!--menu-->
                        <div id="divFaultlogMenu" style="display: block;">
                            <table style="width:100%" cellpadding="0" cellspacing="5">
                                <tr id="trFaultlogSummary">
                                    <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                        <a href="javascript:Faultlog_Status.open_division('divFaultlogSummary');Faultlog_Status.load_status_grid();"
                                            style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">
                                            Faultlog Summary</a>
                                    </td>
                                </tr>
                                <tr id="trFaultlogIssue">
                                    <td style="text-align: right; border-bottom: solid 1px #5697c1; height: 20px;">
                                        <a href="javascript:Faultlog_Status.load_issue();" style="font-weight: bold; font-size: 12px;
                                            font-style: italic; font-family: Arial;">Faultlog Issue</a>
                                    </td>
                                </tr>
                                <tr id="trFaultlogAction">
                                    <td style="text-align: right; border-bottom: solid 1px #5697c1; height: 20px;">
                                        <a href="javascript:Faultlog_Status.load_status_grid_search('ACTION');" style="font-weight: bold;
                                            font-size: 12px; font-style: italic; font-family: Arial;">Faultlog Action</a>
                                    </td>
                                </tr>
                                <tr id="trFaultlogComment">
                                    <td style="text-align: right; border-bottom: solid 1px #5697c1; height: 20px;">
                                        <a href="javascript:Faultlog_Status.load_status_grid_search('COMMENT');" style="font-weight: bold;
                                            font-size: 12px; font-style: italic; font-family: Arial;">Comment</a>
                                    </td>
                                </tr>
                                <tr id="trScreenShot">
                                    <td style="text-align: right; border-bottom: solid 1px #5697c1; height: 20px;">
                                        <a href="javascript:Faultlog_Status.load_status_grid_search();" style="font-weight: bold;
                                            font-size: 12px; font-style: italic; font-family: Arial;">ScreenShot</a>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td align="center" style="vertical-align: top; width: 80%;">
                        <div id="divFaultlogIssue" style="width: 100%;">
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                        <img src="../images/toe.GIF" /></td>
                                    <td id="tdPageTitle" class="table_head_dark" style="width: 30%; height: 24px">
                                        FAULTLOG ISSUE</td>
                                    <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="width: 100%; border: solid 3px #336699; border-collapse: collapse"
                                        class="table_body" align="center">
                                        <div id="divFaultlogMain" style="width: 98%; display: block">
                                            <table id="tblFaultlogDefault" cellpadding="0" cellspacing="5" style="width:95%">
                                                <tr>
                                                    <td class="label4" colspan="4" style="height: 32px">
                                                        System Application : IRS World
                                                        <br />
                                                        FINDINGS AND CORRECTION LOGSHEET
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td id="tdFaultlogIssueFCLSNo" class="cell_caption" style="width: 20%; height: 20px;">
                                                        FCLS No. :&nbsp;</td>
                                                    <td style="text-align: left; width: 30%">
                                                        <input id="txtFaultlogIssueFCLSNo" style="width: 98%" />
                                                    </td>
                                                    <td class="cell_caption" style="width: 20%; height: 20px;">
                                                        Date :&nbsp;</td>
                                                    <td style="text-align: left; width: 30%">
                                                        <input id="txtFaultlogIssueDate" style="width: 98%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" class="table_caption" align="center">
                                                        <span id="spnExchangeRate" class="label2">To be filled up by User Acceptance Test Team:
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 20%; height: 20px;">
                                                        Module Tested :&nbsp;</td>
                                                    <td colspan="3" style="width: 80%; text-align: left;">
                                                        <input id="txtFaultlogIssueModuleTested" style="width: 99%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                     <td class="cell_caption" style="width: 20%; height: 20px;">
                                                        Status :&nbsp;</td>
                                                    <td style="width: 30%; text-align: left;">
                                                        <select id="ddlFaultlogIssueStatus" style="width: 98%">
                                                        </select>
                                                    </td>
                                                </tr>
                                                <tr>

                                                    <td class="cell_caption" style="width: 20%; height: 20px;">
                                                        Screen Shot :&nbsp;</td>
                                                    <td colspan="2" style="text-align: left; width: 30%">
                                                       <%-- <asp:FileUpload ID="ScreenShot" Width="100%" Height="24px" runat="server" onpropertychange="javascript:Faultlog_Status.upload_scrshot()" Font-Names="Arial" ForeColor="Navy" />--%>
                                                        
                                                        <input id="ScreenShot" runat="server" style="width:100%; height:24px" type="file" onchange="javascript:Faultlog_Status.upload_scrshot()"  />
                                                    </td>
                                                    <td>
                                                    <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Height="1px" Width="1%" />
                                                    <%--<asp:Button ID="Button1" OnClick="btnUpload_Click" runat="server" Text="Button" />--%>
                                                    </td>
                                                    
                                                </tr>
                                                <%--<tr>
                                                    <td colspan="2">
                                                    <input id="imgFile2" runat="server" style="width:100%; height:24px; display:none;" type="file" onchange="javascript:Faultlog_Status.upload_img('imgFile2');"  />
                                                    </td>
                                                    <td colspan="2">
                                                    <input id="imgFile3" runat="server" style="width:100%; height:24px; display:none;" type="file" onchange="javascript:Faultlog_Status.upload_img('imgFile3');"   />
                                                    </td>
                                                
                                                </tr>
                                                 <tr>
                                                    <td colspan="2">
                                                    <input id="imgFile4" runat="server" style="width:100%; height:24px; display:none;" type="file" onchange="javascript:Faultlog_Status.upload_img('imgFile4');"  />
                                                    </td>
                                                    <td colspan="2">
                                                    <input id="imgFile5" runat="server" style="width:100%; height:24px; display:none;" type="file" onchange="javascript:Faultlog_Status.upload_img('imgFile5');"   />
                                                    </td>
                                                
                                                </tr>--%>
                                                <tr>
                                                    <td class="cell_caption" style="height: 20px;">
                                                        Issue Description :&nbsp;</td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <%--<input style="width:100%; display:none;" id="btnUpload" class="button" onclick="javascript:Faultlog_Status.show_other();"
                                                                        onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                        type="button" value="Upload More Screenshots" />--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="text-align: left;">
                                                        <textarea style="width: 100%; height: 140px;" cols="4" id="txtIssueDescription"></textarea>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 20%; height: 21px;">
                                                        Prepared By :
                                                    </td>
                                                    <td style="width: 30%; text-align: left; height: 21px;">
                                                        <input id="txtFaultlogIssuePreparedBy" style="width: 98%" />
                                                    </td>
                                                    <td class="cell_caption" style="width: 20%; height: 21px;">
                                                        Branch :
                                                    </td>
                                                    <td style="width: 30%; text-align: left; height: 21px;">
                                                        <input id="txtFaultlogIssueBranch" style="width: 98%" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <input id="txtHiddenSeqID" type="hidden" />
                                           <%-- <iframe id="frameupload" style="height:0px; width:0px" runat="server" src="Faultlog.aspx"></iframe>--%>
                                            <table cellpadding="0" cellspacing="0" style="width:100%">
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%" align="center">
                                                        <table id="tblFaultlogIssueButtons" cellpadding="0" cellspacing="5">
                                                            <tr>
                                                                <td style="width: 20%">
                                                                    <input id="btnFaultlogIssueNew" class="button" onclick="javascript:Faultlog_Status.insert_record();"
                                                                        onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                        type="button" value="New" /></td>
                                                                <td style="width: 20%">
                                                                    <input id="btnFaultlogIssueEdit" class="button" onclick="javascript:Faultlog_Status.update_record();"
                                                                        onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                        type="button" value="Edit" /></td>
                                                                <td style="width: 20%">
                                                                    <input id="btnFaultlogIssueSave" class="button" onclick="javascript:Faultlog_Status.commit_record('ISSUE');"
                                                                        type="button" value="Save" /></td>
                                                                <td style="width: 20%">
                                                                    <input id="btnFaultlogIssueCancel" class="button" onclick="javascript:Faultlog_Status.load();"
                                                                        onmouseout="this.className='button';" onmouseover="this.className='warning_over';"
                                                                        type="button" value="Cancel" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp</td>
                                </tr>
                            </table>
                        </div>
                        <div id="divFaultlogAction" style="width: 100%;">
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                        <img src="../images/toe.GIF" /></td>
                                    <td id="td5" class="table_head_dark" style="width: 30%; height: 24px">
                                        FAULTLOG ACTION</td>
                                    <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="width: 100%; border: solid 3px #336699; border-collapse: collapse"
                                        class="table_body" align="center">
                                        <div id="div6" style="width: 98%; display: block">
                                            <table id="tblFaultlogAction" cellpadding="0" cellspacing="5" style="width:95%">
                                                <tr>
                                                    <td colspan="4">
                                                        <label class="label2">
                                                            System Application : IRS World
                                                            <br />
                                                            FINDINGS AND CORRECTION LOGSHEET
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 20%; height: 20px;">
                                                        FCLS No. :&nbsp;</td>
                                                    <td style="text-align: left; width: 30%">
                                                        <input id="txtFaultlogActionFCLSNo" style="width: 98%" />
                                                    </td>
                                                    <td class="cell_caption" style="width: 20%; height: 20px;">
                                                        Date :&nbsp;</td>
                                                    <td style="text-align: left; width: 30%">
                                                        <input id="txtFaultlogActionDate" style="width: 98%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" class="table_caption" align="center">
                                                        <span id="Span1" class="label2">To be filled up by Project Development Team: </span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 20%; height: 20px;">
                                                        Module :&nbsp;</td>
                                                    <td colspan="3" style="width: 80%; text-align: left;">
                                                        <input id="txtFaultlogActionModule" style="width: 99%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 20%; height: 20px;">
                                                        Status :&nbsp;</td>
                                                    <td colspan="3" style="width: 80%; text-align: left;">
                                                        <select id="ddlFaultlogActionStatus" style="width: 37%">
                                                        </select>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 20%; height: 20px;">
                                                        Action/Resolution :&nbsp;</td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="text-align: left;">
                                                        <textarea style="width: 100%; height: 140px;" cols="4" id="txtFaultlogAction"></textarea>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 20%;">
                                                        Attended By :
                                                    </td>
                                                    <td style="width: 30%; text-align: left;">
                                                        <input id="txtFaultlogActionAttendedBy" style="width: 98%" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                            <input id="txtHiddenSeqID2" type="hidden" />
                                            <table cellpadding="0" cellspacing="0" style="width:100%">
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%" align="center">
                                                        <table id="tblFaultlogActionButtons" cellpadding="0" cellspacing="5">
                                                            <tr>
                                                                <td style="width: 20%">
                                                                    <input id="btnFaultlogActionEdit" class="button" onclick="javascript:Faultlog_Status.update_record_action();"
                                                                        onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                        type="button" value="Edit" /></td>
                                                                <td style="width: 20%">
                                                                    <input id="btnFaultlogActionSave" class="button" onclick="javascript:Faultlog_Status.commit_record();"
                                                                        type="button" value="Save" /></td>
                                                                <td style="width: 20%">
                                                                    <input id="btnFaultlogActionCancel" class="button" onclick="javascript:Faultlog_Status.load();"
                                                                        onmouseout="this.className='button';" onmouseover="this.className='warning_over';"
                                                                        type="button" value="Cancel" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp</td>
                                </tr>
                            </table>
                        </div>
                        <div id="divFaultlogSummary" style="width: 100%;">
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                        <img src="../images/toe.GIF" /></td>
                                    <td id="td3" class="table_head_dark" style="width: 30%; height: 24px">
                                        FAULTLOG SUMMARY</td>
                                    <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="width: 100%; border: solid 3px #336699; border-collapse: collapse"
                                        class="table_body" align="center">
                                        <div id="div3" style="width: 98%; display: block">
                                            <table id="tblFaultlogSummary" cellpadding="0" cellspacing="5" style="width:95%">
                                                <tr>
                                                    <td class="label4">
                                                        IRS World Issue
                                                        <br />
                                                        Faultlog Summary
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 30px; width: 100%">
                                                        <div style="text-align: center" id="DIV_STATUS" onmousemove="javascript:Faultlog_Status.active_grid = 'DIV_STATUS';">
                                                            <br />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table id="Table3" cellpadding="0" cellspacing="1" style="width:95%">
                                                <tr>
                                                    <td colspan="4" class="label4">
                                                        <br />
                                                        Faultlog Status
                                                    </td>
                                                </tr>
                                                <tr>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%">
                                                    </td>
                                                    <td class="label4" style="width: 25%; text-align: center">
                                                        Status
                                                    </td>
                                                    <td class="label4" style="width: 15%">
                                                        Total</td>
                                                    <td style="width: 30%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%">
                                                    </td>
                                                    <td class="cell_caption" style="width: 25%; text-align: left">
                                                        CLOSED :
                                                    </td>
                                                    <td style="width: 15%">
                                                        <input id="txtClosed" />
                                                        <td style="width: 30%">
                                                        </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%">
                                                    </td>
                                                    <td class="cell_caption" style="width: 25%; text-align: left">
                                                        DUPLICATE, FOR DELETION :
                                                    </td>
                                                    <td style="width: 15%">
                                                        <input id="txtDuplicate" />
                                                        <td style="width: 30%">
                                                        </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%">
                                                    </td>
                                                    <td class="cell_caption" style="width: 25%; text-align: left">
                                                        FOR DISCUSSION :
                                                    </td>
                                                    <td style="width: 15%">
                                                        <input id="txtForDiscussion" />
                                                        <td style="width: 30%">
                                                        </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%">
                                                    </td>
                                                    <td class="cell_caption" style="width: 25%; text-align: left">
                                                        FOR RETEST :
                                                    </td>
                                                    <td style="width: 15%">
                                                        <input id="txtForRetest" />
                                                        <td style="width: 30%">
                                                        </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%">
                                                    </td>
                                                    <td class="cell_caption" style="width: 25%; text-align: left">
                                                        OPEN :
                                                    </td>
                                                    <td style="width: 15%">
                                                        <input id="txtOpen" />
                                                        <td style="width: 30%">
                                                        </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%">
                                                    </td>
                                                    <td class="cell_caption" style="width: 25%; text-align: left">
                                                        GRAND TOTAL :
                                                    </td>
                                                    <td style="width: 15%">
                                                        <input id="txtGrandTotal" />
                                                        <td style="width: 30%">
                                                        </td>
                                                </tr>
                                            </table>
                                            <input id="Hidden1" type="hidden" />
                                            <table cellpadding="0" cellspacing="0" style="width:100%">
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%" align="center">
                                                        <table id="tblFaultlogSummaryButtons" cellpadding="0" cellspacing="5">
                                                            <tr>
                                                                <td style="width: 20%">
                                                                    <input id="btnFaultlogSummaryNew" class="button" onclick="javascript:Faultlog_Status.open_division('divFaultlogIssue');Faultlog_Status.insert_record();"
                                                                        onmouseout="this.className='button';" onmouseover="this.className='warning_over';"
                                                                        type="button" value="New" /></td>
                                                                <td style="width: 20%">
                                                                    <input id="btnFaultlogSummaryCancel" class="button" onclick="javascript:Faultlog_Status.load();"
                                                                        onmouseout="this.className='button';" onmouseover="this.className='warning_over';"
                                                                        type="button" value="Cancel" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp</td>
                                </tr>
                            </table>
                        </div>
                        <div id="divFaultlogComment" style="width: 100%;">
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                        <img src="../images/toe.GIF" /></td>
                                    <td id="td1" class="table_head_dark" style="width: 30%; height: 24px">
                                        FAULTLOG COMMENT</td>
                                    <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="width: 100%; border: solid 3px #336699; border-collapse: collapse"
                                        class="table_body" align="center">
                                        <div id="div2" style="width: 98%; display: block">
                                            <table id="tblFaultlogComment" cellpadding="0" cellspacing="5" style="width:95%">
                                                <tr>
                                                    <td colspan="4">
                                                        <label class="label2">
                                                            System Application : IRS World
                                                            <br />
                                                            FINDINGS AND CORRECTION LOGSHEET
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 20%; height: 20px;">
                                                        FCLS No.:&nbsp;</td>
                                                    <td style="text-align: left; width: 30%">
                                                        <input id="txtFaultlogCommentFCLSNo" style="width: 98%" />
                                                    </td>
                                                    <td class="cell_caption" style="width: 20%; height: 20px;">
                                                        Date.:&nbsp;</td>
                                                    <td style="text-align: left; width: 30%">
                                                        <input id="txtFaultlogCommentDate" style="width: 98%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 20%; height: 20px;">
                                                        Module:&nbsp;</td>
                                                    <td colspan="3" style="width: 30%; text-align: left;">
                                                        <input id="txtFaultlogCommentModule" style="width: 99%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 20%; height: 20px;">
                                                        Comment:&nbsp;</td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="text-align: left;">
                                                        <textarea style="width: 100%; height: 90px;" cols="4" id="txtFaultlogComment"></textarea>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 20%; height: 20px;">
                                                        User:&nbsp;</td>
                                                    <td style="width: 30%; text-align: left;">
                                                        <input id="txtFaultlogCommentUser" style="width: 98%" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <input id="txtHiddenSeqID3" type="hidden" />
                                            <table cellpadding="0" cellspacing="0" style="width:100%">
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%; height: 51px;" align="center">
                                                        <table id="tblFaultlogCommentButtons" cellpadding="0" cellspacing="5">
                                                            <tr>
                                                                <td style="width: 20%">
                                                                    <input id="btnFaultlogCommentNew" class="button" onclick="javascript:Faultlog_Status.insert_record_comment();"
                                                                        onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                        type="button" value="New" /></td>
                                                                <td style="width: 20%">
                                                                    <input id="btnFaultlogCommentEdit" class="button" onclick="javascript:Faultlog_Status.update_record_comment();"
                                                                        onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                        type="button" value="Edit" /></td>
                                                                <td style="width: 20%">
                                                                    <input id="btnFaultlogCommentSave" class="button" onclick="javascript:Faultlog_Status.commit_record_comment();"
                                                                        type="button" value="Save" /></td>
                                                                <td style="width: 20%">
                                                                    <input id="btnFaultlogCommentCancel" class="button" onclick="javascript:Faultlog_Status.load();"
                                                                        onmouseout="this.className='button';" onmouseover="this.className='warning_over';"
                                                                        type="button" value="Cancel" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                </tr>
                                                <tr>
                                                    <td align="center" valign="middle">
                                                        <div id="DIV_COMMENT" onmousemove="javascript:Faultlog_Status.active_grid = 'DIV_COMMENT';">
                                                            <br />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp</td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp</td>
                                </tr>
                            </table>
                        </div>
                        <div id="divFaultlogScreenShot" style="width: 100%;">
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                        <img src="../images/toe.GIF" /></td>
                                    <td id="td2" class="table_head_dark" style="width: 30%; height: 24px">
                                        FAULTLOG SCREENSHOTS</td>
                                    <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="width: 100%; border: solid 3px #336699; border-collapse: collapse"
                                        class="table_body" align="center">
                                        <div id="div4" style="width: 98%; display: block">
                                            <table id="Table1" cellpadding="0" cellspacing="5" style="width:95%">
                                                <tr>
                                                    <td colspan="4">
                                                        <label class="label2">
                                                            System Application : IRS World
                                                            <br />
                                                            FINDINGS AND CORRECTION LOGSHEET
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                </tr>
                                                <tr>
                                                    <td style="width: 75%; text-align: left;">
                                                        <input id="txtScreenShot" disabled="true" style="width: 99%" />
                                                    </td>
                                                    <td style="width: 25%">
                                                        <input style="width: 100%;" id="btnGetPath" class="button" onclick="javascript:Faultlog_Status.get_file_path();"
                                                            onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                            type="button" value="Get File Path" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <input id="txtHiddenSeqId4" type="hidden" />
                                            <table cellpadding="0" cellspacing="0" style="width:100%">
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%; height: 51px;" align="center">
                                                        <table id="Table2" cellpadding="0" cellspacing="5">
                                                            <tr>
                                                                <td style="width: 20%">
                                                                    <input style="width: 80%" id="btnDownload" class="button" onclick="javascript:Faultlog_Status.download_file();"
                                                                        onmouseout="this.className='button';" onmouseover="this.className='warning_over';"
                                                                        type="button" value="Download" /></td>
                                                                <td style="width: 20%">
                                                                    <input style="width: 70%" id="Button4" class="button" onclick="javascript:Faultlog_Status.load();"
                                                                        onmouseout="this.className='button';" onmouseover="this.className='warning_over';"
                                                                        type="button" value="Cancel" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                </tr>
                                                <tr>
                                                    <td align="center" valign="middle">
                                                        <div id="DIV5" onmousemove="javascript:Faultlog_Status.active_grid = 'DIV_COMMENT';">
                                                            <br />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp</td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp</td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
<%--    <form id="form2" runat="server">
    <div id="divAdditionalControl" style=" display:none">
                                                                                    <asp:Button ID="btnUpload2" runat="server" OnClick="btnUpload_Click" Text=">>" Height="24px" Width="30%" />
                                                                                    </div>
    </form>--%>
</body>

</html>
