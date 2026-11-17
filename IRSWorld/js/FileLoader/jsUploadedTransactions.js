//Side Bar
function SideBar()
{
    this.construct_side_bar = function()
    {
        document.getElementById('divUploadedTranxSideBar').innerHTML = this.get_side_bar();
    }
    
    this.get_side_bar = function()
    {
        var side_array = this.get_side_bar_list();
        var side_winder = new js_sidebar('sidemenu', side_array);
        
        return side_winder.toString();
    }
    
    this.get_side_bar_list = function()
    {
        var side_bar;
        
        side_bar = new Array
        (
            new Array(2, 0, 'Other Channels', ''),
            new Array(3, 0, 'ARS', '')
        );
        
        return side_bar;
    }
    
    this.get_side_bar_visibility = function(division_id, anchor_id)
    {
        var div = document.getElementById(division_id);
        var anc = document.getElementById(anchor_id);
        var cset = div.style.display;
        
        div.style.display = (cset == 'block') ? 'none' : 'block';
        
        anc.innerText = (cset == 'block') ? String.fromCharCode(187) : String.fromCharCode(171);
        document.getElementById('tdUploadedTranxSideBar').style.width = (cset == 'block') ? '0%' : '18%';
        this.construct_side_bar();
    }
}
