<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tran_3D_Details.aspx.cs" Inherits="IRSWorld._3D.Tran_3D_Details" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Transaction Details</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons.js"                        type="text/javascript"></script>
    <script src="../js/jscommonmethod.js"                   type="text/javascript"></script>
    <script src="../js/3D/Courier_Tran_List.js"             type="text/javascript"></script>
    <script src = "../js/jsgridselector.js"                 type = "text/javascript"></script>
    <script src = "../js/jshttprequest.js"                  type = "text/javascript"></script>
    <script src="../js/3D/Extract_3D.js"                    type="text/javascript"></script>
    <style type="text/css"></style>
    
    <script type="text/javascript">

    </script>
    
    <script type="text/javascript">
    	    	
    	var test_grid;
    	var test_opt = new Array(3);
    	
    	function create_test_grid()
    	{
    		test_grid = new datagrid();

    		var col1 = new datagrid_column();
    		col1.header = 'ID';
    		col1.datafield = 'tag';
    		col1.type = 'link';
    		col1.href = 'test_click';
    		col1.align = 'center';
    		col1.styles.cell += 'width:5%';
    		test_grid.addColumn(col1);

    		var col2 = new datagrid_column();
    		col2.header = 'REFERENCE NUMBER';
    		col2.datafield = 'REFERENCE NUMBER';
    		col2.styles.cell += 'width:30%';
    		test_grid.addColumn(col2);

    		var col3 = new datagrid_column();
    		col3.header = 'REFERENCE NUMBER (THEIRS)';
    		col3.datafield = 'REFERENCE NUMBER (THEIRS)';
    		col3.styles.cell += 'width:30%';
    		test_grid.addColumn(col3);

    		var col4 = new datagrid_column();
    		col3.header = 'REMITTING BRANCH';
    		col3.datafield = 'REMITTING BRANCH';
    		col3.styles.cell += 'width:30%';
    		test_grid.addColumn(col4);
    		
            var col5 = new datagrid_column();
    		col3.header = 'TRANSACTION STATUS';
    		col3.datafield = 'TRANSACTION STATUS';
    		col3.styles.cell += 'width:30%';
    		test_grid.addColumn(col5);
    		
    		var col6 = new datagrid_column();
    		col3.header = 'PRODUCT NAME';
    		col3.datafield = 'PRODUCT NAME';
    		col3.styles.cell += 'width:30%';
    		test_grid.addColumn(col6);
    		
    		var col7 = new datagrid_column();
    		col3.header = 'TRANSACTION DATE';
    		col3.datafield = 'TRANSACTION DATE';
    		col3.styles.cell += 'width:30%';
    		test_grid.addColumn(col7);
    		
    		var col8 = new datagrid_column();
    		col3.header = 'REMITTER NAME';
    		col3.datafield = 'REMITTER NAME';
    		col3.styles.cell += 'width:30%';
    		test_grid.addColumn(col8);    	
    		
    		var col9 = new datagrid_column();
    		col3.header = 'BENEFICIARY NAME';
    		col3.datafield = 'BENEFICIARY NAME';
    		col3.styles.cell += 'width:30%';
    		test_grid.addColumn(col9);    	
    		
/*    		var col4 = new datagrid_column();
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
    		test_grid.addColumn(col6);*/
    		
            test_grid.footer.save_button.none = false;
            test_grid.footer.save_button.onclick = 'test_save';
            
            test_grid.footer.delete_button.none = false;
            test_grid.footer.delete_button.onclick = 'test_delete';
            test_grid.rows_page = 10;
            
    	}
    	
    	function load_details()
    	{
            IRSWorld._3D.Tran_3D_Details.load_3D_details(testgrid);
    	}
    	
    	function Table()
    	{

			this.Rows = new Array();
    	}
    	
    	function Row()
    	{
    		this.id = 0;
    		this.name_first = 'Filip';
    		this.name_last = 'Raymundo';
    		this.name_mi = 'Herrera';
    		this.Status = true;
    		this.bdate = '06/26/1985';
    		this.nbr = 1;
    	}
    	
        Table.prototype.addRow = function(row)
    	{
    		this.Rows.push(row);
    	}
    	    	
    	function testgrid(data)
    	{
    		var dt = new Table();
    		test_opt[0] = '0~ZERO';
    		test_opt[1] = '1~ONE';
    		test_opt[2] = '2~TWO';
    		
    		for (var i = 0; i < 20; i++)
    		{
    			var trow = new Row();
    			trow.id = i + 1;
    			dt.addRow(trow);
    		}

    		create_test_grid();
			test_grid.caption = 'Test Grid';
			//test_grid.row.onclick = 'test_click';
			test_grid.bind(data.value, 'divTest', 'tblTest');
			
		}
    </script>
               
                
</head>

<body onload="load_details();">
    <form id="form1" runat="server">
        <div id="divTest" style="width: 90%">
        </div>
</form>

</body>
</html>
