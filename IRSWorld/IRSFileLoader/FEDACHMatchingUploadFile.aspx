<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FEDACHMatchingUploadFile.aspx.cs" Inherits="IRSWorld.IRSFileLoader.FEDACHMatchingUploadFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Upload File</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
    <script src="../js/js_sidebar.js" type="text/javascript"></script>
    <script src="../js/FileLoader/js_viewuploadedfiles.js" type="text/javascript"></script>
    
    <script src="../js/jsCommons_min.js"                type="text/javascript"></script>
    <script src="../js/js_generic_min.js"               type="text/javascript"></script>
    <script src="../js/jscommonmethod_min.js"           type="text/javascript"></script>
    <script src="../js/jshttprequest_min.js"            type="text/javascript"></script>    
    <script src="../js/jsmodalwindow_min.js"            type="text/javascript"></script>
    <script src="../js/jsgridselector.js"           type="text/javascript"></script>  
    
    <script src="../js/Fileloader/js_fedachmatchinguploadfile.js" type="text/javascript"></script>   
    <script type="text/javascript">
       
        var upf = new js_fedmatchinguploadfile();
        var vf = new js_view_uploaded_files();
        var datatable;
        var dbgrid;
        var divmask;
        
        function close_detail()
        {
            hidepanel('divpresummary');
        }
        
        function showpanel(divname)
        {   
            var div = document.getElementById(divname);
            
            divmask = new maskpanel();
            divmask.show();
            
            document.getElementById(divname).style.display = "block";
            
            div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + 'px';
            div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + 'px';
        }

        function hidepanel(divname)
        {
            document.getElementById(divname).style.display = "none";
            divmask.hide();
        }
        
        function call_post()
        {
            document.getElementById("btnPost").click();
        }
        
        function load_ftp_files()
        {
            upf.display_ftpfiles();
        }
        
        function select_ftp_files(args)
        {
            upf.selected_ftp_file(args);
        }       

    </script>
</head>
<body onload="javascript:page_load()">
 <div  style="overflow:auto ;">
   
        <form id="form1" runat="server" method="post" autocomplete="Off">
        
        <div id="divPopProcess" style="width:400px; height:200px; background:White; z-index: 101; position: absolute; left: 150px;
            color: darkblue; padding: 10px; display: none; font-family: Arial;
            top: 741px;">
            <table style="width:100%; height:100%;">
                <tr>
                    <td>
                        <label id="lblLoadingMsg" style="text-align:center; color:Chocolate "></label>
                        <div id="divLoadingMsg">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <table cellspacing="30" cellpadding="6" >
            <tr>
            <td valign="top" nowrap> &nbsp;<div id="divFileUploadSideBar"></div></td>
            <td height="250">&nbsp;
            <div >
                <table cellspacing="0" cellpadding="0" style="width:100%; " align="center">
                    <tr>
                        <td nowrap align="left" valign="bottom" style="width:1%;height:24px;">
                            <img src="../images/toe.GIF" /></td>
                        <td nowrap align="left" class="table_head_dark" width="20%">Web File Loader<%-- - Compliance--%></td>
                        <td nowrap align="left" valign="bottom" class="bg_body" style="">
                            <img align="absbottom" src="../images/rend.gif" />&nbsp;</td>
                    </tr>
                    <tr>
                        <td nowrap colspan="3" style="" align="left">
                        <div id="tblHolder" >
                            <table style=" border:solid 3px #336699; background:Azure; " cellpadding="0" cellspacing="0">
                                <tr style="height:auto">
                                    <td  align="center" valign="top">                                  
                                        <table style="" cellpadding="4" cellspacing="4">
                                            <tr>
                                                <td style="width:100%;" align="center" valign="middle" colspan="0">
                                                    <div id="divFEDACHMatchingUploadFile" style="width:648px; vertical-align:middle; border:solid 3px midnightblue; display:none" align="center">
                                                        <table style="width:100%;" cellpadding="2" cellspacing="2">
                                                            <tr>
                                                                <td valign="middle" style="border:solid 1px #336699">
                                                                    <table style="width:100%; background:Azure" cellspacing="2" cellpadding="2">
                                                                        <tr>
                                                                            <td align="left" class="table_head_dark" width="100%" style="height: 10px" colspan="1">&nbsp;FEDACH VISUAL MATCHING Upload File</td>
                                                                        </tr>                                                                        
                                                                        <tr>
                                                                            <td nowrap style="width:18%; height:20px; text-align:center"  class="cell_caption">
                                                                                FEDACH File to be Matched</td>
                                                                        </tr>
                                                                        <%--<tr><td style="width:100%; height:5px" colspan="4" align="center"><div id="divImgLoading"></div></td></tr>--%>
                                                                        <tr style="width:100%">
                                                                            <td id="tdFileBrowse"  colspan="1" align="left">
                                                                                <div id="divfilename" style="background:Azure;font-size: smaller; width: 25%; color: #135f94; border-top-style: none;
                                                                                                font-family: Tahoma; border-right-style: none; border-left-style: none; text-align: left;
                                                                                                border-bottom-style: none">
                                                                                                <label class="cell_caption" style="font-size:xx-small; display:none; word-wrap:break-word" id="filename"></label></div>
                                                                                <div id="divfileBrowse" style="background:Azure"><input id="fileBrowse" runat="server" style="width:100%; height:24px" type="file" onchange="javascript:upf.file_browse()" /></div></td>
                                                                            
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="1" style="height: 150px">                                                                                
                                                                                <div id="divAddlField" style="width:auto; vertical-align:middle; border:solid 3px midnightblue;  display:none; background:Azure">
                                                                                    <table style="width:100%">
                                                                                        <tr>
                                                                                            <td nowrap colspan="2" style="height:6px; text-align:center; font-size:small" class="cell_caption">FILE INFO SUMMARY</td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td nowrap style="width:25%; text-align:right; font-size:smaller; border:none; font-family:Tahoma; color:#135f94">Record Count:&nbsp;
                                                                                            </td>
                                                                                            <td style="width:25%; text-align:left; font-size:smaller; border:none; font-family:Tahoma; color:red">
                                                                                                &nbsp;<a id="recordCount"></a></td>
                                                                                        </tr>
                                                                                        <tr id="trfilesize">
                                                                                            <td nowrap style="width:25%; text-align:right; font-size:smaller; border:none; font-family:Tahoma; color:#135f94">File Size:&nbsp;
                                                                                            </td>
                                                                                            <td style="width:25%; text-align:left; font-size:smaller; border:none; font-family:Tahoma; color:red">
                                                                                                &nbsp;<a id="fileSize"></a></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td nowrap style="font-size: smaller; width: 25%; color: #135f94; border-top-style: none;
                                                                                                font-family: Tahoma; border-right-style: none; border-left-style: none; text-align: right;
                                                                                                border-bottom-style: none; height: 2px;">
                                                                                            </td>
                                                                                            <td nowrap style="font-size: smaller; width: 25%; color: red; border-top-style: none; font-family: Tahoma;
                                                                                                border-right-style: none; border-left-style: none; text-align: left; border-bottom-style: none; height: 2px;">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td  nowrap colspan="2" style="font-size: smaller; color: #135f94; border-top-style: none;
                                                                                                font-family: Tahoma; border-right-style: none; border-left-style: none; text-align: center;
                                                                                                border-bottom-style: none; height: 13px;width:100%;">
                                                                                                
                                                                                                
                                                                                                <table >
                                                                                                <tr>
                                                                                                <td>
                                                                                                <input onclick="javascript:upf.cancel_click();" value="Cancel" class="button" type="button" id="Button1" style="height:24px" />
                                                                                                </td>
                                                                                                <td>
                                                                                                <input onclick="javascript:upf.btnUpload_click()" value="Upload" class="button" type="button" id="btnUpload" style=" height:24px" />
                                                                                                </td>
                                                                                                </tr>
                                                                                                </table>
                                                                                                
                                                                                                
                                                                                                </td>
                                                                                        </tr>
                                                                                       
                                                                                    </table>
                                                                                </div>
                                                                                <asp:HiddenField ID="hfAllowedView" runat="server" />
                                                                                <br />
                                                                                &nbsp;<asp:HiddenField ID="hfuserrole" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr><td style="width:100%; height:5px" colspan="1" align="right">
                                                                                <div id="divAdditionalControl" style="background-color:Azure; display:none">
                                                                                    <asp:Button ID="btnPost" runat="server" OnClick="btnPost_Click" Text="->" Height="24px" Width="30%" />&nbsp;</div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    
                                                    <div id="divShowUpload" style="  background:Azure;display:none ; ">
                                                    <table style="vertical-align:middle; border:solid 3px midnightblue;  width: 480px; height: 80px;">
                                                            <tr>
                                                                <td  nowrap colspan="2" style="height:7px; text-align:center; font-size:small" class="cell_caption">
                                                                    UPLOADED FILES</td>                                                                                            
                                                            </tr>
                                                            <tr>
                                                                <td style="font-size: smaller; color: #135f94; border-top-style: none;
                                                                    font-family: Tahoma; border-right-style: none; border-left-style: none; text-align: right;
                                                                    border-bottom-style: none; width: 123px;" nowrap> Search File:&nbsp;
                                                                </td> 
                                                                <td nowrap align="left" style="height: 12px;width:100%;">
                                                                    <input type="text" id="txtsearchupload"  />
                                                                    <input onclick="javascript:upf.btnSearch_click()" value="Search" class="button" type="button" id="Button4" style=" height:24px" /></td>
                                                            </tr>
                                                           <tr>
                                                            <td  width ="100%" colspan="2" align="center" style="text-align:center; vertical-align:middle; border:solid 3px midnightblue; " valign="top" id="tdShowUpload" >
                                                             <div id="divFTPSummary" style="display:block;"></div>
                                                            </td>
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
            </td>
            </tr>
        </table>
        
        </form>
</div>
</body>
</html>