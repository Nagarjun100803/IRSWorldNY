<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Template.aspx.cs" Inherits="IRSWorld.Controls.Template" %>
<%@ Register Src="../Controls/PageFooter.ascx" TagName="page_footer" TagPrefix="uc2" %>
<%@ Register Src="../Controls/PageHeader.ascx" TagName="page_header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Template Page</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/datagrid.js" type="text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript"></script>
    <!-- scripts to test the grid object -->
    <script type="text/javascript">
    	function Row()
    	{
    		this.id = 0;
    		this.name_first = 'Carlos';
    		this.name_last = 'Raymundo';
    		this.name_mi = 'Herrera';
    		this.Account = '123456789012345';
    		this.Status = true;
    		this.bdate = '07/25/1956';
    		this.nbr = 1;
    	}

    	function Table()
    	{

			this.Rows = new Array();
    	}

    	Table.prototype.addRow = function(row)
    	{
    		this.Rows.push(row);
    	}
    </script>

    <script type="text/javascript">

    	var test_grid;
    	var test_opt = new Array(3);

    	function test_click(e)
    	{
    		//alert ('datarow: ' + e.datarow + ' gridrow: ' + e.gridrow + ' value: ' + e.value);
    	}

    	function test_button_edit(e)
    	{
    	    var bodi = document.getElementsByTagName('BODY');
    	    
    	    //alert('window width: ' + bodi.item(0).offsetWidth);
    		//alert ('datarow: ' + e.datarow + ' gridrow: ' + e.gridrow + ' value: ' + e.value);
    	}
    	
    	function test_button_add(e)
    	{    		
    		//alert ('datarow: ' + e.datarow + ' gridrow: ' + e.gridrow + ' value: ' + e.value + ' ' + e.control);
    	}
    	
    	function test_imgbtn_calen(args)
    	{
    	    var btn = document.getElementById(args.control);
    	    var txt = document.getElementById(test_grid.id + '_txt_bdate_' + args.datarow);
    	    popUpCalendar(txt, txt, 'mm/dd/yyyy');
    	}
    	
    	function create_test_grid()
    	{
    		test_grid = new datagrid();

    		var col1 = new datagrid_column();
    		col1.header = 'ID';
    		col1.datafield = 'id';
    		col1.type = 'link';
    		col1.href = 'test_click';
    		col1.align = 'center';
    		col1.styles.cell += 'width:5%';
    		test_grid.addColumn(col1);

    		var col2 = new datagrid_column();
    		col2.header = 'First Name';
    		col2.datafield = 'name_first';
    		col2.styles.cell += 'width:20%';
    		test_grid.addColumn(col2);

    		var col3 = new datagrid_column();
    		col3.header = 'Last Name';
    		col3.datafield = 'name_last';
    		col3.styles.cell += 'width:20%';
    		test_grid.addColumn(col3);
    		
    		var col9 = new datagrid_column();
    		col9.header = 'Account';
    		col9.datafield = 'Account';
    		col9.type = 'password';
    		col9.editable = true;
    		col9.styles.cell += 'width:20%';
    		test_grid.addColumn(col9);

    		var col4 = new datagrid_column();
    		col4.header = 'Status';
    		col4.datafield = 'Status';
    		col4.type = 'check';
    		col4.align = 'center';
    		col4.styles.cell += 'width:5%';
    		test_grid.addColumn(col4);
    		
    		var col5 = new datagrid_column();
    		col5.header = 'Number';
    		col5.datafield = 'nbr';
    		col5.type = 'select';
    		col5.options = test_opt
    		col5.align = 'center';
    		col5.styles.cell += 'width:10%';
    		col5.styles.datafield += 'width: 100%';
    		test_grid.addColumn(col5);    		

    		var col7 = new datagrid_column();
    		col7.header = 'Birth date';
    		col7.datafield = 'bdate';
    		col7.styles.cell += 'width:10%';
    		test_grid.addColumn(col7);
    		
    		var col8 = new datagrid_column();
    		col8.header = '';
    		col8.datafield = '';
    		col8.type = 'image';
    		col8.value = 'Calendar';
    		col8.src = '../images/calendar/imgcalen.png';
    		col8.onclick = 'test_imgbtn_calen';
    		col8.align = 'center';
    		col8.styles.cell += 'width:5%';
    		col8.styles.datafield += 'background-color:skyblue;color:ivory;border:solid 1px navy';
    		test_grid.addColumn(col8);    		
    		
    		var col6 = new datagrid_column();
    		col6.header = '';
    		col6.datafield = '';
    		col6.type = 'button';
    		col6.value = 'Edit';
    		col6.edit_button = true;
    		col6.onclick = 'test_button_edit';
    		col6.align = 'center';
    		col6.styles.cell += 'width:5%';
    		col6.styles.datafield += 'width:100%;background-color:skyblue;color:ivory;border:solid 1px navy';
    		test_grid.addColumn(col6);
    		
            test_grid.footer.save_button.none = false;
            test_grid.footer.save_button.onclick = 'test_save';
            
            test_grid.footer.delete_button.none = false;
            test_grid.footer.delete_button.onclick = 'test_delete';
            
    	}
    	
    	function test_delete(args)
    	{
    	    alert('delete grid_row: ' + args.gridrow);
    	}
    	
    	function test_save(args)
    	{
    	    alert(mi);
    	}
    	
    	function test_add(args)
    	{
    	    alert(args.control);
    	    var row = document.getElementById(args.control);
    	    test_grid.setColumn(0, 17);
    	}

    	function testgrid()
    	{
    		var dt = new Table();
    		test_opt[0] = '0~ZERO';
    		test_opt[1] = '1~ONE';
    		test_opt[2] = '2~TWO';
    		
    		for (var i = 0; i < 16; i++)
    		{
    			var trow = new Row();
    			trow.id = i + 1;
    			dt.addRow(trow);
    		}

    		create_test_grid();
			test_grid.caption = 'Test Grid';
			//test_grid.row.onclick = 'test_click';
			test_grid.bind(dt, 'divTest', 'tblTest');
    	}
    </script>    
</head>
<body onload="testgrid();">
    <center>
    <form id="form1" runat="server">
    <div style="width:100%">
        <table style="width:100%" cellpadding="0" cellspacing="5">
            <tr>
                <td>
                    <uc1:page_header id="Page_header1" runat="server">
                    </uc1:page_header>
                </td>
            </tr>
            <tr>
                <td align="center" valign="middle" style="width:100%; border-collapse:collapse" class="content_height">
                    <table style="width:70%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td align="left" valign="bottom" style="width:1%;height:24px; border-collapse: collapse"> 
                                    <img src="../images/toe.GIF" /></td>
                                <td id="capLabel" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse">
                                    Template Title</td>
                                <td align="left" valign="bottom" style="height: 24px" class="bg_body"> 
                                    <img align="absbottom" src="../images/rend.gif" /></td>
                            </tr>
                            <tr>
                                <td colspan="3" style="width: 100%; background: azure">
                                <table style="width:100%; border:solid 3px #336699;">
                                    <tr>
                                        <td style="width: 100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%">
                                            <!-- content starts here, if necessary increase/decrease the size of the bounding table -->
                                            <div id="div_content" style="width: 90%;height: 100px; overflow:scroll; display: block;" class="table_body">
                                                <table id="tbl_sample" style="width:100%">
                                                <caption id="sample_caption" class="table_caption" style="vertical-align: middle">SAMPLE TABLE</caption>
                                                    <tr>
                                                        <td style="width: 130px; height: 23px" class="cell_caption">Sample stub:&nbsp;</td>
                                                        <td style="width:223px; height:23px" class="cell_desc" align="left">
                                                            <input type="text" id="sample_txtbox" style="width: 80%; height: 100%" />
                                                            <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.forms[0].sample_txtbox, 'mm/dd/yyyy', document.getElementById('div_content'));" />
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                     <tr>
                                                        <td style="width: 130px; height: 23px" class="cell_caption">Sample stub:&nbsp;</td>
                                                        <td style="width:223px; height:23px" class="cell_desc" align="left">
                                                            <input type="text" id="Text1" style="width: 80%; height: 100%" />
                                                            <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.forms[0].sample_txtbox, 'mm/dd/yyyy', document.getElementById('div_content'));" />
                                                        </td>
                                                        <td></td>
                                                    </tr>  
                                                    <tr>
                                                        <td style="width: 130px; height: 23px" class="cell_caption">Sample stub:&nbsp;</td>
                                                        <td style="width:223px; height:23px" class="cell_desc" align="left">
                                                            <input type="text" id="Text2" style="width: 80%; height: 100%" />
                                                            <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.forms[0].sample_txtbox, 'mm/dd/yyyy',  document.getElementById('div_content'));" />
                                                        </td>
                                                        <td></td>
                                                    </tr>                                                    
                                                    <tr>
                                                        <td style="width: 130px; height: 23px" class="cell_caption">Sample stub:&nbsp;</td>
                                                        <td style="width:223px; height:23px" class="cell_desc" align="left">
                                                            <input type="text" id="Text3" style="width: 80%; height: 100%" />
                                                            <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.forms[0].sample_txtbox, 'mm/dd/yyyy',  document.getElementById('div_content'));" />
                                                        </td>
                                                        <td></td>
                                                    </tr> 
                                                    <tr>
                                                        <td style="width: 130px; height: 23px" class="cell_caption">Sample stub:&nbsp;</td>
                                                        <td style="width:223px; height:23px" class="cell_desc" align="left">
                                                            <input type="text" id="Text4" style="width: 80%; height: 100%" />
                                                            <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.forms[0].sample_txtbox, 'mm/dd/yyyy',  document.getElementById('div_content'));" />
                                                        </td>
                                                        <td></td>
                                                    </tr>                                                
                                                    </table>
                                            </div>
                                            <!-- content ends here -->
                                            <br />
                                            <input id="Button1" class="button" style="width: 16%" type="button" value="Sample Button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" /><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%" id="btnSample">
                                            &nbsp;
                                        </td>
                                    </tr>                                    
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td style="width:100%">
	                <div id="divTest" style="width:90%;">
	                </div>        
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <uc2:page_footer id="Page_footer1" runat="server">
                    </uc2:page_footer></td>
            </tr>            
        </table>   

    </div>
    </form>
    </center>
</body>
</html>
