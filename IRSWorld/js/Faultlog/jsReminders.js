function execute_ajx_command(param)
{
    if (Reminders_Message != null && Reminders_Message != "undefined")
    {
        Reminders_Message.do_gridsearch(param);
    }
}

function execute_ajaxgridview_command(param)
{
    if (Reminders_Message != null && Reminders_Message != "undefined")
    {
        Reminders_Message.do_gridview(param);
    }
}

var as_for = "" ;
var as_thru = "";
var as_new = "";
var as_arr = "";
var as_len = "";
var y = new Array();
var yy = "";

function Reminders_Message()
{
    this.commons        = null;
    this.SEQ_ID         = null;
    this.ReferenceNo    = null;
    this.ReminderFor    = null;
    this.ReminderThru   = null;
    this.ReminderFrom   = null;
    this.Date           = null;
    this.Subject        = null;
    this.ReminderMsg    = null;
    this.FileName       = "";
    this.Name           = "";
    this.Attachment     = "";
    this.grid           = null;
    this.ajax_grid      = null;
    this.grid_ajax_page      = "../AjxPage/ajaxgrid.aspx";
    this.grid_ajax_query     = null;    
    this.ajax_container      = null;
    this.generic     = new generic_method(); 
    this.error_input = null;
    this.oText = null;
    
    this.get_email = function(txt_add)
    {
        this.email = document.getElementById(txt_add).value;
        var x = "";
        var x_ar = new Array();
        var x_len = "0";
        x = this.generic.to_trim(document.getElementById(txt_add).value);
        if (x != "")
        {
            if (x.indexOf(';') != -1)
            {
                x_ar = x.split(";");
                x_len = x_ar.length;
                if(x_ar[x_len-1] != "")
                {
                    this.email = x_ar[x_len-1];
                }
            }
        }
        
        this.positionSuggest(txt_add);       
//        IRSWorld.Faultlog.Reminders.email_add(this.email, Reminders_Message.email_callback);
        IRSWorld.Faultlog.Reminders.email_add(this.email, Reminders_Message.onTextChange);
    }
   
    this.email_callback = function(result)
    {
        var ds = result.value;
        document.getElementById("suggest").innerHTML = "";
        divLeft = document.getElementById("suggest");
        var currentText = "";
        var theParag = document.createElement("div");
        for (i=0; i<=4; i++)
        {
            if(ds.Tables[0].Rows[i] != null)
            {
                currentText = ds.Tables[0].Rows[i].emailaddress;
                innerDiv = document.createElement("div");
                innerDiv.className = "br";
                var currentTextNode = document.createTextNode(currentText);
                innerDiv.appendChild(currentTextNode);
                theParag.appendChild(innerDiv);
            }
        }
        divLeft.appendChild(theParag);
    }
    
    
    this.onTextChange = function(result)
    {
        var txt = Reminders_Message.oText.value;
        this.oDiv = document.getElementById('suggest');
        var currentText = "";
        var a = "";
        yy = "";
        if(txt.length>0)
        {
            while(this.oDiv.hasChildNodes())
            this.oDiv.removeChild(this.oDiv.firstChild);
            var currentText = "";
            var ds = result.value;       
            for (i=0; i<=4; i++)       
            {
                var oNew=document.createElement('div');
                if (ds.Tables[0].Rows[i] != null)       
                {
                    currentText = ds.Tables[0].Rows[i].emailaddress;
                    this.oDiv.appendChild(oNew);
                    oNew.onmouseover=
                    oNew.onmouseout=
                    oNew.onmousedown=function(oEvent)
                    {
                        oEvent=window.event || oEvent;
                        oSrcDiv=oEvent.target || oEvent.srcElement;
                        if(oEvent.type=="mousedown")
                        {
                            if (Reminders_Message.oText == document.getElementById("txtFor"))
                            {
                                as_new = document.getElementById("txtFor").value;
                                if (as_new.indexOf(';') != -1)
                                {
                                    as_arr = as_new.split(";");
                                    as_len = as_arr.length;
                                    for(i=0; i<=as_len-2; i++) 
                                    {
                                        y[i]=as_arr[(as_len+i)-as_len];
                                        if (yy == "")
                                        {
                                            yy = y[i] + ";";
                                        }
                                        else
                                        {
                                            yy = yy + y[i] + ";";
                                        }
                                    }
                                }
                                as_for = this.innerHTML + ";";
                                Reminders_Message.oText.value= yy + as_for;
                            }
                            else    //(Reminders_Message.oText == document.getElementById("txtThru"))
                            {
                                as_new = document.getElementById("txtThru").value;
                                if (as_new.indexOf(';') != -1)
                                {
                                    as_arr = as_new.split(";");
                                    as_len = as_arr.length;
                                    for(i=0; i<=as_len-2; i++) 
                                    {
                                        y[i]=as_arr[(as_len+i)-as_len];
                                        if (yy == "")
                                        {
                                            yy = y[i] + ";";
                                        }
                                        else
                                        {
                                            yy = yy + y[i] + ";";
                                        }
                                    }
                                }
                                as_thru = this.innerHTML + ";";
                                Reminders_Message.oText.value= yy + as_thru;
                            }
                            Reminders_Message.hide_suggest();
                            Reminders_Message.oText.focus();
                        }
                        else if(oEvent.type=="mouseover")
                        {
                            this.className="over";
                        }
                        else if(oEvent.type=="mouseout")
                        {
                            this.className="";
                        }
                        else
                        {
                            this.oText.focus();
                        }
                    }
                    oNew.innerHTML=currentText;   
                    this.oDiv.style.visibility="visible";
                }
                a += currentText;
                if (a == "")
                {
                    this.oDiv.style.visibility="hidden";
                }
                if(Reminders_Message.email == currentText)
                {
                    this.oDiv.style.visibility="hidden";
                }
            }
        }
        else
        {
            this.oDiv.innerHTML="";
            this.oDiv.style.visibility="hidden";
        }
    }
 
    this.positionSuggest = function(txt_add) 
    {
        this.oText = document.getElementById(txt_add);
        var oNode=this.oText;
        var x=0,y=oNode.offsetHeight;
        while(oNode.offsetParent && oNode.offsetParent.tagName.toUpperCase() != 'BODY')
        {
           x+=oNode.offsetLeft;
           y+=oNode.offsetTop;
           oNode=oNode.offsetParent;
        }
        x+=oNode.offsetLeft;
        y+=oNode.offsetTop;
        this.oDiv = document.getElementById('suggest');
        this.oDiv.style.top=y+"px";
        this.oDiv.style.left=x+"px";
     }
    
    this.hide_suggest = function()
    {
        this.oDiv.innerHTML="";
        this.oDiv.style.visibility="hidden";
    }
    
    this.file_browse = function()
    {
        var filename = document.getElementById('file1').value;
        document.getElementById('aspbtnSend').click();        
    }
    
    this.onload = function()
    { 
        this.open_division("divReminders");
        document.getElementById("txtHiddenSeqId").value = "";
        document.getElementById("txtareaReminderMsg").readOnly = true;
        this.commons = new common_method();
        this.commons.to_clear_screen("tblReminders");
        this.commons.to_read_only("tblReminders",true);  
        this.commons.to_unlock_button("tblButton","btnNew");
        this.commons = null;
        this.load_field();
//        this.delete_record();
    }
    
    this.delete_record = function()
    {
        IRSWorld.Faultlog.Reminders.delete_record();
    }
    
    this.load_field = function()
    {
        document.getElementById("txtareaReminderMsg").value = "";
        document.getElementById("Forward_Files").value = "";
        document.getElementById("forward_hidden").value = "";
        document.getElementById("txtRefno").value = "IRSW";
        //document.getElementById("txtDate").value = this.get_date();
        document.getElementById('tdfile2').style.display = 'none';
        document.getElementById('tdfile3').style.display = 'none';
        document.getElementById('tdFiles').style.display = 'none'; 
        document.getElementById('td_forward_hidden').style.display = 'none';
        document.getElementById('tr_forward_hidden').style.display = 'none';
        document.getElementById('txtRefno').readOnly = true;
        document.getElementById('txtDate').readOnly = true;
        document.getElementById('txtFrom').readOnly = true;
        this.get_sender();
        document.getElementById('file1').value = "";
        document.getElementById('file2').value = "";
        document.getElementById('file3').value = "";
        as_for = "";
        as_thru = "";
        document.getElementById('suggest').style.visibility = "hidden";
    }
    
    this.get_sender = function()
    {
        IRSWorld.Faultlog.Reminders.user_info(this.sender_callback);
    }
    
    this.sender_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            var r = result.value.split("~");
            if (r[0] == "Error")
            {
                alert(r[1]);
            }
            else
            {
                document.getElementById("txtFrom").value = r[0];
                document.getElementById("txtDate").value = r[1];
            }
        }
    }
    
    this.check_message = function()
    {
        if(document.getElementById('txtRefno').value != "IRSW")
        {
            IRSWorld.Faultlog.Reminders.get_message(this.SEQ_ID, this.message_callback);
        }
    }
    
    this.message_callback = function(msg) 
    {
        if(msg != null && msg.value != "undefined")
        {
            document.getElementById("txtareaReminderMsg").value = msg.value;
        }
    }
   
    this.close_divisions = function()
    {
        document.getElementById("divReminders").style.display = 'none';
        document.getElementById("divRemindersList").style.display = 'none';
    }
    
    this.open_division = function(division_id)
    {
        this.close_divisions(); document.getElementById(division_id).style.display = 'block';       
    }
    
  
    this.load_RemindersList = function()
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_RemindersList~";
        this.grid.div_container  = document.getElementById("divRemindersList");
        this.grid.datarow_column = 0;
        this.grid.page_size = 10;
        this.grid.open_grid_view();
        this.grid = null;
    }
    

    this.insert_record = function()
    {
        this.add_table();
        IRSWorld.Faultlog.Reminders.insert_record(this.insert_record_callback);
    }

    this.insert_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.Faultlog.Reminders.read_message(); alert(msg.value); return;
            }
            this.commons = new common_method();
            this.commons.to_unlock_button("tblButton","btnSend~btnCancel"); //~btnAttach
            this.commons = null;
        }
    }
    this.forward_record = function()
    {
        this.update_table();
        IRSWorld.Faultlog.Reminders.insert_record(this.insert_record_callback);
    }
    
    this.update_table = function()
    {
        this.commons = new common_method();
        this.commons.to_read_only("tblReminders",false);
        this.commons.to_unlock_button("tblButton","btnSend~btnCancel");
        this.commons = null;
        document.getElementById("txtareaReminderMsg").readOnly = false;
        //document.getElementById("txtDate").value = this.get_date();
        document.getElementById("txtRefno").readOnly = true;
        document.getElementById("txtDate").readOnly = true;
        document.getElementById("txtFrom").readOnly = true;
        this.get_sender();
        document.getElementById('file1').value = "";
        document.getElementById('file2').value = "";
        document.getElementById('file3').value = "";
        this.FileName = "";
    }    
    
    this.add_table = function()
    {
        this.commons = new common_method();
        this.commons.to_clear_screen("tblReminders");
        this.commons.to_read_only("tblReminders",false);
        this.commons.to_unlock_button("tblButton","btnSend~btnCancel");
        this.commons = null;
        document.getElementById("txtareaReminderMsg").readOnly = false;
        this.load_field();
    }    
    	
	this.set_reminders_new = function()
	{
	    IRSWorld.Faultlog.Reminders.record(this.rec_callback);
	    this.commons = new common_method();
        this.show_div_sending(true);
    	this.commons.to_show_loading_message('divSending');   
	}

    this.rec_callback = function(result)
    {
	    var val = result.value.split("~");
	    if (val[0] == "" && val[1] == "")
	    {
	        Reminders_Message.show_div_sending(false);
	        alert("No Email Address provided");
	    }
	    else
	    {
	        Reminders_Message.ReminderFor = val[0];
	        Reminders_Message.ReminderThru = val[1];
	        Reminders_Message.Subject = val[2];
	        Reminders_Message.ReminderMsg = val[3];
	        Reminders_Message.ReferenceNo = val[4];
	        Reminders_Message.ReminderFrom = val[5];
	        Reminders_Message.Date = val[6];
	        Reminders_Message.FileName = val[7];
	        Reminders_Message.Name = val[8];
	        IRSWorld.Faultlog.Reminders.commit_record(Reminders_Message.push_data_to_server(), Reminders_Message.commit_record_callback);
	    }
    }
    
    this.show_div_sending = function(isVisible)
	{
	    if(isVisible == true)
	    {
	        document.getElementById('divPopSending').style.display = 'block';
	    }
	    else
	    {
	        document.getElementById('divPopSending').style.display = 'none';
	    }
	}
	

    this.commit_record = function()
    {
        if (typeof(this.validate_input) != 'undefined')
        {
            if (this.validate_input() == false)
            {
               alert(this.error_input); return;
            }                            
        } 
        //Reminders.commit_record(this.set_reminders(), this.commit_record_callback);
        
        var attach = null;
        attach = document.getElementById("file1").value;
        
        if(document.getElementById("file2").value != "")
        {
            attach += '~' + document.getElementById("file2").value;
            if (document.getElementById("file3").value != "")
            {
                attach += '~' + document.getElementById("file3").value;
            }
        }
        
        this.Attachment = attach.split("~")
                
        IRSWorld.Faultlog.Reminders.get_path(this.Attachment, this.path_callback);  
           
        this.commons = new common_method();
        this.show_div_sending(true);
    	this.commons.to_show_loading_message('divSending');          
    }
    
    this.path_callback = function(result_path)
    {
        var path_name = result_path.value.split("~");        
        Reminders_Message.FileName = path_name[0];
        Reminders_Message.Name = path_name[1];

        Reminders_Message.ReminderFor    = document.getElementById("txtFor").value;
        Reminders_Message.ReminderThru   = document.getElementById("txtThru").value;
        Reminders_Message.Subject        = document.getElementById("txtSubject").value;
        Reminders_Message.ReminderMsg    = document.getElementById("txtareaReminderMsg").value;
        Reminders_Message.ReferenceNo    = document.getElementById("txtRefno").value;
        Reminders_Message.ReminderFrom   = document.getElementById("txtFrom").value;
        Reminders_Message.Date           = document.getElementById("txtDate").value;
        
        if (document.getElementById("Forward_Files").value != "" )
        {
            if(Reminders_Message.Name != "") 
            {
                Reminders_Message.Name += ';' + document.getElementById("Forward_Files").value ;
            }
            else
            {
                Reminders_Message.Name = document.getElementById("Forward_Files").value ;
            }
        }
        
        if (document.getElementById("forward_hidden").value != "")
        {
            if(Reminders_Message.FileName != "")
            {
                Reminders_Message.FileName += ';' + document.getElementById("forward_hidden").value;
            }
            else
            {
                Reminders_Message.FileName = document.getElementById("forward_hidden").value;
            }
        }
       
        IRSWorld.Faultlog.Reminders.commit_record(Reminders_Message.push_data_to_server(), Reminders_Message.commit_record_callback);
    }


    this.push_data_to_server = function()
    {
        var data_row = new Array();
        
        data_row[data_row.length] = "ReminderFor~"  + this.ReminderFor;
        data_row[data_row.length] = "ReminderThru~" + this.ReminderThru;
        data_row[data_row.length] = "Subject~"      + this.Subject;
        data_row[data_row.length] = "ReminderMsg~"  + this.ReminderMsg;
        data_row[data_row.length] = "ReferenceNo~"  + this.ReferenceNo;
        data_row[data_row.length] = "ReminderFrom~" + this.ReminderFrom;
        data_row[data_row.length] = "Date~"         + this.Date;
        data_row[data_row.length] = "FileName~"     + this.FileName;
        data_row[data_row.length] = "Name~"         + this.Name;
        return data_row.join("|");
    }
    
    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                Reminders_Message.show_div_sending(false);
                var msg = IRSWorld.Faultlog.Reminders.read_message(); alert(msg.value); return;
            }
            this.SEQ_ID = IRSWorld.Faultlog.Reminders.REMINDERS_SEQ_ID().value;
            document.getElementById("txtHiddenSeqId").value = this.SEQ_ID;
            document.getElementById("txtRefno").value = "IRSW" + this.SEQ_ID;
            
                if (this.ReferenceNo == null || this.ReferenceNo == "undefined")
                {
                    this.ReferenceNo = "IRSW" + this.SEQ_ID;
                    IRSWorld.Faultlog.Reminders.reference_no(this.SEQ_ID, this.ReferenceNo);
                    this.ReferenceNo = null;
                }

            document.getElementById('txtareaReminderMsg').readOnly = true;
            document.getElementById('txtFor').value = Reminders_Message.ReminderFor;
            document.getElementById('txtThru').value = Reminders_Message.ReminderThru;
            document.getElementById('txtSubject').value = Reminders_Message.Subject;
            document.getElementById('txtareaReminderMsg').value = Reminders_Message.ReminderMsg;
            document.getElementById('txtDate').value = Reminders_Message.Date;
            if (Reminders_Message.Name != "")
            {
                document.getElementById('tdFiles').style.display = 'block';
                document.getElementById('Forward_Files').readOnly = true;
                document.getElementById('Forward_Files').value = Reminders_Message.Name;
                document.getElementById('forward_hidden').value = Reminders_Message.FileName;
            }
            
            Reminders_Message.show_div_sending(false);            
            alert("Your message has been sent");  
            this.commons = new common_method();
            this.commons.to_read_only("tblReminders",true);
            this.commons.to_unlock_button("tblButton","btnNew~btnForward");
            this.commons = null;
       }
    }
       
    
    this.get_date = function()
    {
        var dt = new Date();
        return dateToString(dt);
    }   
    

    this.do_gridview = function(parameter)
    {
        this.ajax_grid = new grid_selector();
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");
            if (command_param.length > 0)
            { 
                if (command_param[0] == "ajx_selected")                 
                {
                    this.clear_fields();
                    var command_value = command_param[1].split('~');
                    this.SEQ_ID = command_value[0];
                    this.ReferenceNo = command_value[1];
                    this.ReminderFor = command_value[2];
                    this.ReminderThru = command_value[3];
                    this.ReminderFrom = command_value[4];
                    this.Date = command_value[5];
                    this.Subject = command_value[6];
                    this.Name = command_value[7]; 
                    this.FileName = command_value[8];
                    
                    this.populate_reminder();
                    this.open_division('divReminders');
                }
                else
                {
                    this.process_grid_viewer(parameter);
                }
            }
        }
    }
    
    this.clear_fields = function()
    {
        this.commons = new common_method();
        this.commons.to_clear_screen("tblReminders");
        document.getElementById("txtHiddenSeqId").value = "";
        document.getElementById('txtFor').value = "";
        document.getElementById('txtThru').value = "";
        document.getElementById('txtSubject').value = "";
        document.getElementById('txtareaReminderMsg').value = "";
        document.getElementById('txtRefno').value = "";
        document.getElementById('txtDate').value = "";        
        document.getElementById('file1').value = "";
        document.getElementById('file2').value = "";
        document.getElementById('file3').value = "";       
        document.getElementById("Forward_Files").value = "";
        document.getElementById("forward_hidden").value = "";
        document.getElementById('file1').value = "";
        document.getElementById('file2').value = "";
        document.getElementById('file3').value = "";
        document.getElementById('tdfile2').style.display = 'none';
        document.getElementById('tdfile3').style.display = 'none';
    }
    
    this.populate_reminder = function()
    {
//        this.onload();
//        this.clear_fields();
        document.getElementById('txtHiddenSeqId').value = this.SEQ_ID;
        document.getElementById('txtRefno').value = this.ReferenceNo;
        document.getElementById('txtFor').value = this.ReminderFor;
        document.getElementById('txtThru').value = this.ReminderThru;
        document.getElementById('txtFrom').value = this.ReminderFrom;
        document.getElementById('txtDate').value = this.Date;
        document.getElementById('txtSubject').value = this.Subject;
        this.file_callback();  
        this.check_message();
        this.file_hidden();
        this.commons = new common_method();
        this.commons.to_read_only("tblReminders",true);
        this.commons.to_unlock_button("tblButton","btnForward~btnCancel");
        this.commons = null;
    }
   
    this.file_hidden = function()
    {
        document.getElementById("forward_hidden").value = "";
        document.getElementById("forward_hidden").value = this.FileName;
    }
    this.file_callback = function()
    {
        if(this.Name != null && this.Name != "")
        {
            document.getElementById("Forward_Files").value = "";
            document.getElementById('tdFiles').style.display = 'block';
            document.getElementById('Forward_Files').readOnly = true;
            document.getElementById('Forward_Files').value = this.Name;
        }
        else
        {
            document.getElementById('tdFiles').style.display = 'none';
        }
    }


    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        this.grid.sql_source = "sp_RemindersList~"
        this.grid.div_container = document.getElementById("divRemindersList");
        this.grid.datarow_column = 0;
        this.grid.page_size = 10;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }

    
    this.attach_files = function()
    {
        document.getElementById('tdfile2').style.display = 'block';
        document.getElementById('tdfile3').style.display = 'block';
    }
    

    this.validate_input = function()
    {
        this.error_input = "";
        if (this.generic.is_empty(document.getElementById("txtFor").value) == true ||document.getElementById("txtFor").value == "")
        {
            this.error_input += "The specified string is not in the form required for an e-mail address.";
            document.getElementById("txtFor").focus();
            return false;
        } 
                
        if (this.error_input != "")
        {
            return false;
        }        
        return true; 
    }
    
}


