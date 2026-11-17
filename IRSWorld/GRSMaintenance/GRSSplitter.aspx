<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GRSSplitter.aspx.cs" Inherits="IRSWorld.GRSMaintenance.GRSSplitter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
       <link href="../css/Default.css" rel="stylesheet" type="text/css" />
       <script type="text/javascript"></script>
       
        <script language="javascript" >				   
    	
    	    var tobj = null;
            var input_path;
            var g_header_type;
            var g_beginofday;
            var g_prevfile;
            var g_empty;
            var g_table;
            var oldgridSelectedColor; 
            var g_pgindex;
            var g_opening_url;
         

            function file_changed()
            {
              
                show_panel_visibility(false);	        
                document.getElementById("btn_post").style.display = "none";
                document.getElementById('btn_post').click();	        
            }
            
            function file_work(input_path)        
            {                                     
                this.input_path = input_path;
                //this.do_process_msg( " PLEASE WAIT WHILE THE SYSTEM IS PROCESSING YOUR REQUEST...");            
               
                this.SplitFile.do_load_payroll(this.input_path ,file_work_callback );
             
            }   
              function file_work_spilt()        
            {                                     
                this.input_path = input_path;
                //this.do_process_msg( " PLEASE WAIT WHILE THE SYSTEM IS PROCESSING YOUR REQUEST...");            
               
                this.SplitFile.do_split_file(this.input_path);
             
            }   	    	    	   	    	     
    	    
            function file_work_callback(res)
            {	    
            
                var ajax_value;
                ajax_value  = res.value.split(';');
                document.getElementById("txtFileName").innerHTML = ajax_value[0];
                document.getElementById("txtFileSize").innerHTML = ajax_value[1];
                document.getElementById("txtNonDeducted").innerHTML = ajax_value[2];
                document.getElementById("txtDeducted").innerHTML = ajax_value[3];
                document.getElementById("txtTotalOrigRec").innerHTML = ajax_value[4];
                document.getElementById("txtTotalRecPUNLA").innerHTML = ajax_value[5];
                //Alert('me');
            }
            
            function display_filework_errors(errmsg)
            {
                this.init_tab_error(0);
                do_process_msg("File may not be intended for Payroll", 1); 
            }
            function set_combo_index(combo, combo_value)
            {	        	        
                for(var i = 0; i < combo.length; i++)
                {
                    if (parseInt(combo_value) == parseInt(combo.options[i].value))
                    {
                        combo.selectedIndex = i;	             
                    }
                }
            }
    	    
    	    function do_process_msg(msg, img)
	        {
	            var s;        	        	        
    	        
	            if (img == 1)
	            {	        
	                s = '<table><tr><td><img src="../UILibrary/Generic/information.jpg" /></td><td>' + msg + '</td></tr>';
	            }
	            else
	            {
	                s = '<table><tr><td><img src="../UILibrary/Generic/ajax-loader.gif" /></td><td>' + msg + '</td></tr>';
	            }	        
	            document.getElementById('msgticker').innerHTML = s;	        
	        }
	        
            function set_globals(res)
            {   	        
                g_header_type = res[0];
                g_beginofday  = (res[5] == '1');
                g_prevfile    = (res[6] == '1');
                g_empty       = (parseFloat(res[1]) < 1);            
            }
            
            function verify_file()
            {                                    
                this.show_panel_visibility(false);               
                this.do_process_msg(" VERIFYING FILE PLEASE WAIT ...");
                
                SplitFile.confirm_valid_file(this.input_path,verify_file_callback);          
            }                
            
            function check_rate_response()
            {            
               var rate_type="";
               
               SplitFile.get_rate_response(rate_type, check_rate_response_callback);
            }
            
            function check_rate_response_callback(res)
            {            
                if (res.value != "")
                {
                    this.do_process_msg(res.value, 1); return;
                }
                            
                SplitFile.confirm_valid_file(this.input_path,verify_file_callback);
            }
            
            function verify_file_callback(res)
            {
                if (res.value != "")
                {
                    this.do_process_msg(" INVALID PAYROLL FILE: " + res.value, 1);
                }
                else
                {
                    this.load_summary_amounts();
                }            
            }
            

            

            
            function show_panel_visibility(bool)
            {                    
                if (bool == false)
                {   
                    try
                    {             
                        document.getElementById("pnlUpload").style.display = "none";
                    }
                    catch(e){}
                }
                else
                {    
                    try
                    {               
                        document.getElementById("pnlUpload").style.display = "block"; 
                        document.getElementById("summary_area").innerHTML = "";                           
                    }
                    catch(e){}
                }            
                return;       
            }
            
            function proceed_transfer_loader()
            {
                this.do_process_msg("CREATING FILE HEADER ...");
                    
                var rate_type_var = document.getElementById('dropdownHolder').children.item(0).value;
                var file_date_var = document.getElementById('txt_file_date').value;
                var file_path_var = this.input_path;            
               
                SplitFile.construct_input_file(file_path_var, rate_type_var, file_date_var, proceed_transfer_loader_callback);
            }
            
            function proceed_transfer_loader_callback(res)
            {        
            
                if (res.value != "")
                {
                    this.do_process_msg(res.value,1);
                }
                else
                {
                    this.do_process_msg(" LOADING INPUT FILE TO PAYROLL LOADER ...",2);                                
                    this.load_payroll_to_dir();
                                  
                    document.getElementById("pnl_FileInput").style.display = "block";
                }
            }
            
            function load_payroll_to_dir()
            {
                var rate_type_var = document.getElementById('dropdownHolder').children.item(0).value;
                var file_date_var = document.getElementById('txt_file_date').value;
                var file_path_var = this.input_path;
                
                SplitFile.run_and_load_payrolldisk(file_path_var, rate_type_var, load_payroll_to_dir_callback)
            }
            
            function load_payroll_to_dir_callback(res)
            {
                
                var vmsg = res.value ;
                if (vmsg.indexOf("ERROR") > -1)
                {
                    this.do_process_msg(res.value, 1);
                }
                else
                {
                    
                    this.do_process_msg("FILE SUCCESSFULLY SENT TO LOADER", 1);
                    document.getElementById("pnl_fileinput").innerHTML = "";
                    
                    document.getElementById("summary_area").innerHTML = "<div><input type='button' id='btnLoadFile' style='BORDER-RIGHT: navy thin solid; BORDER-TOP: navy thin solid; FONT-WEIGHT: bolder; FONT-SIZE: x-small; BORDER-LEFT: navy thin solid; WIDTH: 88px; COLOR: green; BORDER-BOTTOM: navy thin solid; BACKGROUND-COLOR: lightgrey'  value='&nbsp;Load Another File&nbsp;' onclick='changeurl()'> <br><br><br></div>";            
                }
                
            }
            function changeurl(user)
            {
            var user = document.getElementById("btnUser").value ;
                location.href = "?userid=" + user;
            }
            function getfile(url)
            {
                location.href = url;
            }
            function cancel_loading()
            {                     
                SplitFile.cancel_loading(this.input_path,cancel_loading_callback);            
            }
            function cancel_loading_callback(res)
            {
                 if (res.value != "")
                 {
                    this.do_process_msg(res.value, 1);
                 }
                 else
                 {
                    this.do_process_msg(res.value, 1);                
                 }   
                                        
                //this.reset_views(true,1);
               // this.init_tab(0);
            }
            
            function cancel_summary()
            {
                SplitFile.cancel_loading(this.input_path,cancel_summary_callback);            
            }
            
            function cancel_summary_callback(res)
            {
                 if (res.value != "")
                 {
                    this.do_process_msg(res.value, 1);
                 }
                 else
                 {
                    this.do_process_msg(res.value, 1);                
                 }
                
                this.reset_views(true,2);            
            }
            
            function hide_panel()
            {
                document.getElementById("pnlUpload").style.display = "none";
                document.getElementById("pnlUpload").style.height = "0";
                document.getElementById("pnlUpload").style.width = "0";
                document.getElementById("pnlUpload").style.position = "absolute";
            }
            function reset_views(stog,stype)
            {   
                if (stype == 1) 
                {
                    if (stog)
                    {                
                        document.getElementById("pnl_FileInput").style.display = "block";                    
                        document.getElementById("pnlUpload").style.display = "none";
                    }
                    else
                    {
                        document.getElementById("pnl_FileInput").style.display = "none";                    
                        document.getElementById("pnlUpload").style.display = "block";
                    }
                 }
                 else
                 {
                    if (stog)
                    {                
                        document.getElementById("pnl_FileInput").style.display = "block";                    
                        document.getElementById("summary_area").innerHTML = "";
                    }
                    else
                    {
                        document.getElementById("pnl_FileInput").style.display = "none";                                        
                    }             
                 }
            }
            
            function goto_pg(url)
            {
                window.location.href = url;
            }
            

        </script> 

</head>
<body >
    <form id="form1" runat="server">
    <div>
      <table style="width:100%; border:solid,1,#000000;" cellspacing="0" cellpadding="0" >
            <tr>
                <td>
                    <table id="tblBrowse" style="width:100%; border:solid,1,#000000;" cellspacing="0" cellpadding="0"> 

                            <tr>
                                <td colspan="2"><br /></td>
                            </tr> 
                       
                            <tr>
                                <td style="height:26px" class="table_caption" colspan="2">GRS Splitter</td>
                            </tr>
                             <tr>
                            <td colspan="2"><br /></td>
                             </tr>  
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                <div id="divSummary">
               <span style="background-color:Transparent; font-size:x-small;color:#RRRGGBB" id="msgticker"></span>
               
            
                <table id="tblSummary" visible ="false"  style="width:100%; border:solid,1,#000000;" cellspacing="0" cellpadding="0">
                      <tr>
                            <td style="width:30%; height:21px" class="cell_caption">File &nbsp;</td>
                            <td style="height: 21px">
                            <input id="File1" runat="server"  name ="File1" type="file" onchange="file_changed()" enableviewstate="true"/>&nbsp;
                            <asp:Button ID="btn_post" runat="server" OnClick="btn_post_Click" Text="Button" style="visibility:hidden; width:1px;"/> 
   
                            </td>   
                      </tr>
                      
                      <tr>
                            <td colspan="2"><br /></td>
                      </tr> 
                          
                      <tr>
                            <td style="width:30%; height:26px" class="table_caption" colspan="2">Show Summary:&nbsp;</td>
                            
                      </tr>
                      
                      <tr>
                            <td colspan="2"><br /></td>
                      </tr>
                         
                      <tr>
                            <td style="width:30%; height:22px" class="cell_caption">File Name:&nbsp;</td>
                            <td class="cell_desc" style="height: 22px">
                            <span id="txtFileName" style="width: 72%; text-align: left; visibility:visible "></span></td>
                      </tr>
                       
                      <tr>
                               <td style="width:30%; height:22px" class="cell_caption">File Size:&nbsp;</td>
                               <td class="cell_desc" style="height: 22px"><span id="txtFileSize" style="width: 72%; text-align: right;"></span></td>
                      </tr>
                      
                      <tr>
                               <td style="width:30%; height:21px" class="cell_caption">Total Non-deducted:&nbsp;</td>
                               <td class="cell_desc"><span id="txtNonDeducted" style="width: 72%; text-align: left;"></span></td>
                      </tr>
                         
                      <tr>
                               <td style="width:30%; height:21px" class="cell_caption">Total Deducted to PUNLA:&nbsp;</td>
                               <td class="cell_desc"><span id="txtDeducted" style="width: 72%; text-align: left;"></span></td>
                      </tr>
                      
                      <tr>
                               <td style="width:30%; height:22px" class="cell_caption">Total of Original Record:&nbsp;</td>
                               <td class="cell_desc" style="height: 22px"><span id="txtTotalOrigRec" style="width: 72%; text-align: left;"></span></td>
                      </tr>
                      
                      <tr>                   
                               <td style="width:30%; height:21px" class="cell_caption">Total of Records with PUNLA:&nbsp;</td>
                               <td class="cell_desc"><span id="txtTotalRecPUNLA" style="width: 72%; text-align: left;"></span></td>
                      </tr> 
                      <tr>    <!--onclick="do_split_file('input_path')" -->
                                <td colspan ="2" align="right" > 
                          
                                <input id="btnsplitFile" class="button" type="button" value="Split File" onclick="file_work_spilt()"/>
                                    <input id="btnCancel" class="button" type="button" value="Cancel" onclick="cancel_loading()" />
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
