function xml_http_req()
{
	this.ajax_response_div = null;
	
	this.get_xml_http_connection = function()
	{
		var xml_reqts = null;

        try
        {    
		    if (typeof XMLHttpRequest != "undefined")
		    {
			    return new XMLHttpRequest();
		    }
		    else if (window.ActiveXObject)
		    {
			    var msxmlversion = ["MSXML2.XMLHttp.5.0", "MSXML2.XMLHttp.4.0", "MSXML2.XMLHttp.3.0", "MSXML2.XMLHttp","Microsoft.XMLHttp"];

			    for(var irows = 0; irows < msxmlversion.length; irows++)
			    {
				    try
				    {
					    xml_reqts = new ActiveXObject(msxmlversion[irows]);return xml_reqts;				    					    
				    }
				    catch(ex)
				    {
					    throw new error(ex);					
				    }
			    }
		    }
		}
		catch(error)
		{
		    alert("ERROR FROM XML HTTP REQUEST OBJECT: " + error.message);
		}
	}
}

