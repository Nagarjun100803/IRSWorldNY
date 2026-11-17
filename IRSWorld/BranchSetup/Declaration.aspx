<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Declaration.aspx.cs" Inherits="IRSWorld.BranchSetup.Declaration" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Declaration</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jspad/jspad.js" type="text/javascript"></script>
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/js_generic.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/BranchSetup/js_Declaration.js" type="text/javascript"></script>
    <script type="text/javascript">
            var Declaration = new Declaration();
    </script>
</head>
<body onload="javascript:Declaration.load();">
    <center>
        <form id="form1" runat="server">
            <div id="divBodyDeclaration" style="width: 98%; display: block; vertical-align: top;
                margin-top: 30px;">
                <table id="tblDeclaration" style="width: 100%; display: block">
                    <tr>
                        <td id="tdDeclaration" style="width: 85%; border-collapse: collapse;
                            vertical-align: middle;">
                            <div id="divDeclarationDefault" style="width: 80%;">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="vertical-align: bottom">
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" style="width: 0%; height: 24px; border-collapse: collapse; vertical-align: bottom;">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="tdPageTitle" class="table_head_dark" style="width: 50%; height: 24px">
                                                        RECEIPT DECLARATION</td>
                                                    <td align="left" style="width: 100%; height: 24px; vertical-align: bottom;">
                                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; border: solid 3px #336699; border-collapse: collapse" class="table_body"
                                            align="center">
                                            <br />
                                            <div id="divDeclaration" style="width: 95%; display: block">
                                                <table id="tblDeclarationDefault" cellpadding="0" cellspacing="5" style="width:95%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            Negotiating Branch :&nbsp;</td>
                                                        <td align="left" style="width: 70%; height: 24px;">
                                                            <select id="ddlNegBranch" style="width: 100%" onchange="javascript:Declaration.load_branches();" >
                                                                <option selected="selected"></option>
                                                            </select>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            Branch :&nbsp;</td>
                                                        <td align="left" style="width:  70%; height: 24px;">
                                                            <select id="ddlBranch" style="width: 100%" onchange="javascript:Declaration.load_declarations();">
                                                                <option selected="selected"></option>
                                                            </select>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            Declaration 1 :&nbsp;</td>
                                                        <td align="left" style="width:  70%; height: 24px;">
                                                            <textarea id="txtDeclaration1" rows="2" style="width: 100%"></textarea>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            Declaration 2 :&nbsp;</td>
                                                        <td align="left" style="width:  70%; height: 24px;">
                                                            <textarea id="txtDeclaration2" rows="2" style="width: 100%"></textarea>
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            Declaration 3 :&nbsp;</td>
                                                        <td align="left" style="width:  70%; height: 24px;">
                                                            <textarea id="txtDeclaration3" rows="2" style="width: 100%"></textarea>
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            Declaration 4 :&nbsp;</td>
                                                        <td align="left" style="width:  70%; height: 24px;">
                                                            <textarea id="txtDeclaration4" rows="2" style="width: 100%"></textarea>
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            Declaration 5 :&nbsp;</td>
                                                        <td align="left" style="width:  70%; height: 24px;">
                                                            <textarea id="txtDeclaration5" rows="2" style="width: 100%"></textarea>
                                                        </td>
                                                    </tr>
                                                    
                                                </table>
                                                <input id="txtHiddenSeqID" type="hidden" />
                                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%" align="center">
                                                            <table id="tblbuttons" cellpadding="0" cellspacing="5">
                                                                <tr>
                                                                    <!--td style="width: 20%">
                                                                        <input id="btnNew" class="button" onclick="javascript:Declaration.insert_record();"
                                                                            onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                            type="button" value="New" /></td-->
                                                                    <td style="width: 20%; height: 21px;">
                                                                        <input id="btnEdit" class="button" onclick="javascript:Declaration.update_record();"
                                                                            onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                            type="button" value="Edit" /></td>
                                                                    <td style="width: 20%; height: 21px;">
                                                                        <input id="btnSave" class="button" onclick="javascript:Declaration.commit_record();"
                                                                            type="button" value="Save" /></td>
                                                                    <td style="width: 20%; height: 21px;">
                                                                        <input id="btnCancel" class="button" onclick="javascript:Declaration.load();"
                                                                            onmouseout="this.className='button';" onmouseover="this.className='warning_over';"
                                                                            type="button" value="Cancel" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="div1" style="display: none">
                                            </div>
                                            <div id="div2" style="display: none">
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
    </center>
</body>
</html>
