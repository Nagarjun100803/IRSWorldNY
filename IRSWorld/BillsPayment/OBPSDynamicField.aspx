<%@ Page Language="C#" AutoEventWireup="true" Codebehind="OBPSDynamicField.aspx.cs"
    Inherits="IRSWorld.BillsPayment.OBPSDynamicField" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>OBPS Dynamic Field</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>

    <script src="../js/jsCommons.js"                        type="text/javascript"></script>
    <script src="../js/jscommonmethod.js"                   type="text/javascript"></script>
    <script src="../js/js_generic.js"                       type="text/javascript"></script>
    <script src="../js/jshttprequest.js"                    type="text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"                    type="text/javascript"></script>
    <script src="../js/jsgridselector.js"                   type="text/javascript"></script>
    
    <script src="../js/BillsPayment/jsBillsPayOBPSRemitter.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var dfields = new OBPSDynamicFields();
        function keyUP(keyCode) 
	    { 
		    if(keyCode==16)
				    isShift = false;
	    } 

	    function keyDown(keyCode)
	    {
	     if ((event.keyCode >= 48 && event.keyCode <= 57) || event.keyCode == 46) 
            {                
                event.returnValue = true;
                return;
            }
            else
            {
                event.returnValue=false;
                return;            
            }
            
	    }
    </script>

</head>
<body onload="dfields.LoadFieldsPage()">
    <form id="form1" runat="server">
        <div style="width: 100%">
            <table width="100%" cellpadding="0" cellspacing="5">
                <tr>
                    <td align="center" valign="top" style="width: 100%; border-collapse: collapse; height: 600px;"
                        class="content_height" colspan="2">
                        <div id="divBillsPayOBPSRemitter" style="width: 100%; height: 100%; display: block">
                            <table width="100%">
                                <tr>
                                    <td style="width: 78%; padding-top: 30px">
                                        <table width="96%" cellspacing="0" cellpadding="0" style="background-color: Azure">
                                            <tr>
                                                <td align="left" valign="bottom" style="width: 1%; height: 24px;">
                                                    <img src="../images/toe.GIF" /></td>
                                                <td id="capResult" align="left" class="table_head_dark" style="width: 40%; height: 24px;
                                                    border-collapse: collapse;">
                                                    OBPS - Dynamic Field</td>
                                                <td align="left" valign="bottom" style="height: 24px" class="bg_body">
                                                    <img align="absbottom" src="../images/rend.gif" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="width: 100%; height: 100%">
                                                    <table class="table_border" style="width: 100%; height: 100%">
                                                        <tr>
                                                            <td style="width: 100%; height: 21px">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="vertical-align: top; width: 100%; height: 100%">
                                                                <!-- content starts here -->
                                                                <table cellpadding="0" cellspacing="5" width="100%">
                                                                    <tr>
                                                                        <td style="width: 100%; height: 100%" valign="center">
                                                                            <div id="divBillsPayOBPSRemitterInfo" class="table_body" style="margin: 0px; width: 98%;
                                                                                height: 100%">
                                                                                <table id="tblBillsPayOBPSRemitter" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                    border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid"
                                                                                    width="100%">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 10%; height: 23px;">
                                                                                            Merchant Code : &nbsp;
                                                                                        </td>
                                                                                        <td style="width: 15%; height: 23px;">
                                                                                            <select id="ddlOBPSDynamicMerchantCode" style="width: 99%" onchange = "dfields.getMerProductCode();">
                                                                                            </select>
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 10%; height: 23px;">
                                                                                            Product Code: &nbsp;
                                                                                        </td>
                                                                                        <td style="width: 15%; height: 23px;">
                                                                                            <select id="ddlOBPSDynamicProductCode" style="width: 99%" onchange = "dfields.load_grid_viewer();">
                                                                                            </select>
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 10%; height: 23px;">
                                                                                            Field Type: &nbsp;
                                                                                        </td>
                                                                                        <td style="width: 15%; height: 23px;">
                                                                                            <select id="ddlOBPSDynamicFieldName" style="width: 99%" disabled="true">
                                                                                                <option value=""></option>
                                                                                                <option value="tf">Text Field</option>
                                                                                                <option value="nf">Numeric Field</option>
                                                                                                <option value="df">Date Field</option>
                                                                                                <option value="rf">Remarks Field</option>
                                                                                                <option value="id">ID Field</option>
                                                                                            </select>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>                                                                                        
                                                                                        <td class="cell_caption" style="width: 10%; height: 23px;">
                                                                                            Field Label: &nbsp;
                                                                                        </td>
                                                                                        <td style="width: 15%; height: 23px;">
                                                                                            &nbsp;<input id="txtOBPSDynamicFieldLabel" style="width: 94%" disabled="true"/></td>
                                                                                        <td class="cell_caption" style="width: 10%; height: 23px;">
                                                                                            Validation Type: &nbsp;
                                                                                        </td>
                                                                                        <td style="width: 15%; height: 23px;">
                                                                                            <select id="ddlOBPSDynamicOBPSValidationType" style="width: 99%" disabled="true">
                                                                                                <option value=""></option>
                                                                                                <option value="NA">Not Applicable</option>
                                                                                                <option value="LI">List Item</option>
                                                                                                <option value="OF">Other field validation</option>
                                                                                                <option value="SV">Self validation</option>
                                                                                            </select>
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 10%; height: 23px;">
                                                                                            Use in ARM : &nbsp;
                                                                                        </td>
                                                                                        <td style="width: auto; height: 23px;" align="left">
                                                                                            <input id="txtOBPSDynamicDisplaySeq" type="checkbox" style="width: 17%" disabled="true"/>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 10%; height: 23px;">
                                                                                            Mandatory : &nbsp;
                                                                                        </td>
                                                                                        <td style="width: 15%; height: 23px;" align="left">
                                                                                            <input id="chbIsMandatory" type="checkbox" style="width: 17%" disabled="true"/>
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 10%; height: 23px; display:none;" id="FieldRef">
                                                                                            List Field Ref : &nbsp;
                                                                                        </td>
                                                                                        <td style="width: 15%; height: 23px; display:none;" id="crtlFieldRef">
                                                                                            <select id="ddlOBPSDynamicListFIeldRef" style="width: 97%" disabled="true"></select>
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width:10%; height:23px; display:none;" id="VOV">
                                                                                            Validate on Value: &nbsp;
                                                                                        </td>
                                                                                        <td style="width:15%; height:23px; display:none;" id="crtlVOV">
                                                                                            <input id="txtValOnVal" type="text" style="width:94%" disabled="true" onkeypress="javascript:keyDown(event.keyCode);" onkeyup="javascript:keyUP(event.keyCode);"/>
                                                                                        </td>                                                                                        
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="height: 23px; width: 100%;">
                                                                                        <div id="dvFieldGridView" style="display:none;"></div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 100%; height: 27px" valign="top">
                                                                            <input class="button"  onmouseout="this.className='button';"
                                                                                onmouseover="this.className='button_over';" style="width: 10%; height: 28px"
                                                                                type="button" value="Add" id="btnOBPSDynamicAdd" onclick="dfields.addRecord();" />
                                                                            <input id="btnOBPSDynamicEdit" class="button"
                                                                                onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                                style="width: 10%; height: 28px" type="button" value="Edit" disabled="true" onclick="dfields.editRecord();" />
                                                                            <input class="button" onmouseout="this.className='button';"
                                                                                onmouseover="this.className='button_over';" style="width: 10%; height: 28px"
                                                                                type="button" value="Save" id="btnOBPSDynamicSave" disabled="true" onclick="dfields.saveRecord();"/>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <!-- content ends here -->
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
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
