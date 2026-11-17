function modal_window()
{	
    var imodal = this;

	this.outer_modal_div  = null;
	this.inner_modal_div  = null;
    this.commons          = null;

	this.construct_close_button = function()
	{
		var closebtn = document.createElement("A");

		closebtn.setAttribute("id","btn_close");
		closebtn.innerHTML        = "CLOSE";
        closebtn.style.cursor     = "hand";
		closebtn.style.cursor     = "pointer";
		closebtn.style.fontWeight = "700";
		closebtn.style.fontFamily = "Arial";
		closebtn.style.fontSize   = "10pt";
		closebtn.style.color      = "black";
       	closebtn.onclick     = function() {this.parentNode.Code.close_modal_window(); }
        closebtn.onmouseover = function() {closebtn.style.color = "blue";}
        closebtn.onmouseout  = function() {closebtn.style.color = "black";}
        
		return closebtn;
	}

	this.construct_div_content = function()
	{
	    var close_div   = null;
	    var extra_div   = null;
	    var content_div = null;        

		this.inner_modal_div              = document.createElement("div");		
		this.inner_modal_div.align        = "center";
		this.inner_modal_div.className    = "inner_div";		
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.border = "medium solid white";
		this.inner_modal_div.style.height = "auto";
		this.inner_modal_div.style.width  = "auto";
		
        close_div                    = document.createElement("div");
        close_div.id                 = "close_mod_div";
        close_div.align              = "right" ;
        close_div.style.background   = "transparent";
        close_div.style.width        = "auto";
        close_div.style.height       = "auto";
        close_div.Code               = this;
        close_div.appendChild(this.construct_close_button());
        
        extra_div                    = document.createElement("div");
        extra_div.id                 = "extra_mod_div";
        extra_div.align              = "left";
        extra_div.style.background   = "transparent";        
        extra_div.style.width        = "auto";        
        extra_div.style.height       = "auto";

		content_div                  = document.createElement("div");
		content_div.id               = "content_mod_div";
		content_div.align            = "left";
		content_div.style.background = "transparent";
		content_div.style.width      = "auto";
		content_div.style.height     = "auto";
		content_div.innerHTML        = "<center><img src = '../images/ajaxloader.gif'></img></center>";            

		this.inner_modal_div.appendChild(close_div);
		this.inner_modal_div.appendChild(extra_div);
		this.inner_modal_div.appendChild(content_div);
		
		document.body.appendChild(this.inner_modal_div);

	}
	
	this.get_div_container = function()
	{
	    var i = 0;
	    
	    if (this.inner_modal_div != null)
	    {	        
	        var inners = this.inner_modal_div.getElementsByTagName("div");
	        
	        for(i = 0; i < inners.length; i++)
	        {	            
	            if (inners.item(i).id == "content_mod_div")
	            {
	                return inners.item(i);
	            }
	        }	    
	    }	    	    
		return null;
	}
	
	this.open_modal_window = function()
	{
        this.load_modal_css();
		this.outer_modal_div           = document.createElement("div");
		this.outer_modal_div.id        = "outer_mod_div";
		this.outer_modal_div.className = "outer_div";		
		
		document.body.style.margin = "0px";
		document.body.appendChild(this.outer_modal_div);
		document.getElementById("outer_mod_div").style.height = screen.height;
				
		this.construct_div_content();

        this.commons = new common_method();
        this.commons.to_show_hide_combobox(false);
        this.commons = null;

        document.body.onkeypress = function(e)
        {
            if (imodal != null)
            {
                var e = window.event || e;
                
                if ((e.charCode || e.keyCode) == 27)
                {
                    imodal.close_modal_window();
                }
            }
        }
	}

	this.close_modal_window = function()
	{
	    this.commons = new common_method();
        this.commons.to_show_hide_combobox(true);
        this.commons = null;

		if(this.outer_modal_div != null){document.body.removeChild(this.outer_modal_div);this.outer_modal_div  = null;}
		if(this.inner_modal_div != null){document.body.removeChild(this.inner_modal_div);this.inner_modal_div  = null;}
		if(this.content_iframe  != null){this.content_iframe = null;}

		document.body.style.margin = ""; imodal = null;

	}

	this.load_modal_css = function()
	{	    
		var file_reference = document.createElement("link");
		
	  	file_reference.setAttribute("rel", "stylesheet")
	  	file_reference.setAttribute("type", "text/css")
	  	file_reference.setAttribute("href", "../css/cssmodals.css")

		if (typeof file_reference != "undefined")
		{		    
	  		document.getElementsByTagName("head")[0].appendChild(file_reference)
		}
	}
	
	window.onscroll = function ()
	{		
	    var i = 0;
	    var m = 0;
    	
    	var divelement_outer = document.documentElement.getElementsByTagName("div");
    	var divelement_inner = document.documentElement.getElementsByTagName("div");
    	    	    	
	    for(i = 0; i < divelement_outer.length;i++)
	    {
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        {
	            divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
	        }	        
	    }
	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        {
	            divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20;break;
	        }	        
	    }
	}
}