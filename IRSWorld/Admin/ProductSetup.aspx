<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductSetup.aspx.cs" Inherits="IRSWorld.Admin.ProductSetup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Product Setup</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/js_generic.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/Admin/jsProductSetup.js" type="text/javascript"></script>
    <script type="text/javascript">
    
    var product_setup = new product_setup();
    
    function page_setup()
    {
        if (top != self)
        {
            top.frame_scroll("yes");
        }
    }
    
    function page_off()
    {
        if (top != self)
        {
            top.frame_scroll("no");
        }
    }
   
    </script>
  

</head>
<body onload="javascript:product_setup.load_all_dropdownlist();" onunload="page_off();" >
    <form id="form1" runat="server">
         <div style="padding-top: 30px;overflow:auto;height:800px">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="height: 43px; width: 700px;"> 
                        <table cellpadding="0" cellspacing="0" >
                            <tr>
                                <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                    <img src="../images/toe.GIF" /></td>    
                                <td class="table_head_dark" style="width: 50%; height: 24px">
                                    Product Setup</td>
                                <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                    <img align="absBottom" src="../images/rend.gif" /></td>   
                            </tr>
                        </table>    
                    </td>                    
                </tr>
                <tr>
                    <td colspan="2" style="width: 715px">                                    
                        <table style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;width: 81%; border-bottom: #336699 3px solid">
                            <tr>
                                <td style="width: 100%; height: 21px; background-color: azure;">                                    
                                    <div id="divProductSetup"style="text-align: center;">
                                        <table id="" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                            border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;
                                            width:900px">
                                            <tr>
                                                <td class="table_caption" colspan="5" style="width: 125px; color: black; text-indent: 5px;height: 26px; text-align: left">Setup</td>
                                            </tr>
                                            <tr>
                                                <td style="height: 190px; width: 900px; background-color: azure; text-align: center;">
                                                    <div style="text-align: center">
                                                        <table >
                                                            <tbody>
                                                                <tr>
                                                                    <td style="width: 100%">
                                                                        <table style="text-align: center">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 25%">
                                                                                        Product Code:&nbsp;</td>
                                                                                    <td style="width: 25%">
                                                                                        <select id="ddProductCode" style="width: 100%">
                                                                                            <option selected="selected"></option>
                                                                                        </select>
                                                                                    </td>
                                                                                    <td class="cell_caption" style="width: 25%">
                                                                                        Implementation Type:</td>
                                                                                    <td style="width: 25%">
                                                                                        <select id="ddImplemType" style="width: 100%">
                                                                                            <option selected="selected"></option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" >
                                                                                        Control Type:</td>
                                                                                    <td style="width: 25%;">
                                                                                        <select id="ddControlType" style="width: 100%" onchange="javascript:product_setup.enable_control_value();">
                                                                                            <option selected="selected"></option>
                                                                                        </select>
                                                                                    </td>
                                                                                    <td class="cell_caption" style="width: 25%;">
                                                                                        Control Name:</td>
                                                                                    <td style="width: 25%;">
                                                                                        <input id="txtControlName" style="width: 98%" type="text" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="4">
                                                                                        <div id="divcontrolvalue" style="display:none">
                                                                                           <table style="width: 100%;">
                                                                                                <tr>
                                                                                                    <td class="cell_caption" style="width: 25%;">
                                                                                                        Control Value:</td>
                                                                                                    <td style="width: 25%;">
                                                                                                        <input id="txtControlValue" style="width: 98%" type="text"/></td>
                                                                                                    <td class="cell_caption" style="width: 25%;">Fixed:</td>
                                                                                                    <td>
                                                                                                        <table style="width:100%">
                                                                                                            <tr>                                                                                            
                                                                                                                <td style="width:20%">
                                                                                                                    <input id="chkFixed" style="width: 100%" type="checkbox" onchange="javascript:product_setup.fixed();" />
                                                                                                                </td>
                                                                                                                <td style="width: 80%">
                                                                                                                    <input id="txtFixed" style="width: 98%" type="text" disabled="disabled"/>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 202px">
                                                                                        Source:</td>
                                                                                    <td style="width: 401px">
                                                                                        <select id="ddFieldSource" style="width: 100%" onchange="javascript:product_setup.field_source_option();">
                                                                                            <option selected="selected"></option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
    <%--                                                                                <td style="width: 202px; height: 438px">
                                                                                    </td>--%>
                                                                                    <td colspan ="4" >
                                                                                        <table style="width:100%">
                                                                                            <tr>
                                                                                                <td >
                                                                                                    <div id="divquery" style="display:none">
                                                                                                        <table style="width:100%">                                                                                            
                                                                                                            <tr>
                                                                                                                <td colspan="4" style="width: 80%; text-align: left">
                                                                                                                    <textarea id="txtQuery" style="width: 100%; height: 230px;"></textarea>
                                                                                                                </td>
                                                                                                                    <td style="width: 20%">
                                                                                                                    <input id="btnParse" type="button" value="Parse"  onclick="javascript:product_setup.exec_stmt();"/>
                                                                                                                </td>                                                                                        
                                                                                                            </tr>   
                                                                                                    </table>
                                                                                                    </div>  
                                                                                                    <div id="divtable" style="display:none">
                                                                                                        <table style="text-align: center; width:100%">
                                                                                                            <tr>
                                                                                                                <td class="cell_caption" style="width: 25%;">Table Name:</td>
                                                                                                                <td style="width: 25%">
                                                                                                                    <select id="ddTableName" style="width: 100%" onchange="javascript:product_setup.dropdown_get_columns();">
                                                                                                                        <option selected="selected"></option>
                                                                                                                    </select>
                                                                                                                </td>
                                                                                                                <td class="cell_caption" style="width: 25%;">Column Name:</td>
                                                                                                                <td style="width: 53%; ">
                                                                                                                    <select id="ddColumnName" style="width: 100%">
                                                                                                                        <option selected="selected"></option>
                                                                                                                    </select>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td class="cell_caption" style="width: 214px; height: 23px;">
                                                                                                                    No. of Criteria:</td>
                                                                                                                <td style="width: 25%; height: 23px;">
                                                                                                                    &nbsp;<input id="txtNoOfCriteria" type="text" onchange="javascript:product_setup.create_criteria();" style="width: 95%"/></td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td style="width: 100%" colspan="4">
                                                                                                                    <div style="text-align:center ">
                                                                                                                    <table style="text-align:center; width: 100%">
                                                                                                                        <tr>
                                                                                                                            <td class="cell_caption" style="text-align: center;width:20%">Column Name:</td>
                                                                                                                            <td class="cell_caption" style="text-align: center;width:20%">
                                                                                                                                Comparison Operator:</td>
                                                                                                                            <td class="cell_caption" style="text-align: center;width:20%">Value:</td>
                                                                                                                            <td class="cell_caption" style="width: 20%; text-align: center">
                                                                                                                                &nbsp;Logical Operator:</td>
                                                                                                                        </tr>                                                                                                            
                                                                                                                        <tr>
                                                                                                                            <td colspan="4">
                                                                                                                                <div id="divcriteria1" style="display:block">
                                                                                                                                </div>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                    </div>
                                                                                                                </td>
                                                                                                            </tr> 
                                                                                                           
                                                                                                            <tr>
                                                                                                                <td class="cell_caption" style="width: 214px;">
                                                                                                                    No. of Destination Table:</td>
                                                                                                                <td style="width: 25%;">
                                                                                                                    <input id="txtNoOfDestination" type="text" onchange="javascript:product_setup.create_destination();" style="width: 95%"/></td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td style="width: 100%" colspan="4">
                                                                                                                    <div style="text-align:center ">
                                                                                                                    <table style="text-align:center; width: 100%">
                                                                                                                        <tr>
                                                                                                                            <td class="cell_caption" style="text-align: center;width:33%">
                                                                                                                                Table Name:</td>
                                                                                                                            <td class="cell_caption" style="text-align: center;width:33%">
                                                                                                                                Column Name:</td>
                                                                                                                        </tr>                                                                                                            
                                                                                                                        <tr>
                                                                                                                            <td colspan="3">
                                                                                                                                <div id="divDestination1" style="display:block">
                                                                                                                                </div>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                    </div>
                                                                                                                    <table>
                                                                                                                        <tr>
                                                                                                                            <td style="width: 140px">
                                                                                                                                <input id="btnParse2" onclick="javascript:product_setup.check_table_query();" style="width: 100%"
                                                                                                                                    type="button" value="Parse" /></td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                            </tr>                                                                                                                   
                                                                                                        </table>
                                                                                                    </div>     
                                                                                                </td>  
                                                                                            </tr>                                                                        
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                        <div id="divbuttons"> 
                                                                            <table id="tblbuttons" cellpadding="0" cellspacing="5" style="width:300">
                                                                                <tr>
                                                                                    <td style="width: 25%; height: 22px">
                                                                                        <input id="btnAdd" class="button" onclick="javascript:product_setup.insert_record();" onmouseout="this.className='button';"
                                                                                            onmouseover="this.className='button_over';" type="button" value="  Add  " /></td>
                                                                                    <td style="width: 25%; height: 22px">
                                                                                        <input id="btnEdit" class="button" disabled="disabled" onclick="javascript:product_setup.update_record();"
                                                                                            onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                                            type="button" value="  Edit  " /></td>
                                                                                    <td style="width: 25%; height: 22px">
                                                                                        <input id="btnSave" class="button" onclick="javascript:product_setup.set_product_setup();"
                                                                                            onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                                            type="button" value=" Save " /></td>
                                                                                    <td style="width: 25%; height: 22px">
                                                                                        <input id="btnCancel" class="button" disabled="disabled" onclick="javascript:product_setup.rollback_record();"
                                                                                            onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                                            type="button" value="Cancel" /></td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                        <div id ="divproductgrid">
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>    
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
    </form>   
</body>
</html>
