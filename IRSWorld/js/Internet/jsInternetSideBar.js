function Internet_Sidebar()
{
    this.get_side_bar_visibility = function(division_id, anchor_id)
    {
        var div  = document.getElementById(division_id);
        var cset = div.style.display;

        div.style.display = (cset == "block") ? "none" : "block";
        document.getElementById(anchor_id).innerHTML = (cset == "block") ? String.fromCharCode(187) : String.fromCharCode(171);
    }

    this.get_side_bar = function()
    {
        var side_array  = this.get_side_bar_list();
        var side_winder = new js_sidebar('sidemenu', side_array);
        
        return side_winder.toString();
      
    }
  
    this.get_side_bar_list = function()
    {  var cifid = document.getElementById("txtHiddenRemCIFID").value;
      // var cifid = '1072102109000001'

        var sidebar = new Array
        (   
            new Array(1,0,'Log-in',            ''),
            new Array(2,0,'CIF',''), 
            new Array(2,2,'Edit Profile','javascript:show_frame_dialog("../CIF/cifmain.aspx?function=search&cifid=' + cifid + '", "cif_detail_close", "95%", "95%", "0");'),
            new Array(2,2,'Add Beneficiary', 'javascript:show_frame_dialog("../CIF/cifmain.aspx?function=add_bene&cifid=' + cifid + '", "cif_detail_close", "95%", "95%", "0");'),
            new Array(3,0,'Remittance',      ''),
		    new Array(4,0,'OBPS', '')
//		    new Array(100,0,'Cancel Transaction', '')
        );
        return sidebar;
    }
}

function cif_detail_close()
{    
}