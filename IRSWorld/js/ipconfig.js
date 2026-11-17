/******************************************
Obtain Hostname, IPAddress and MacAddress 
using ipconfig /all command.

windows only.
requires ActiveX.

IRSWorld     CHR 2011     (c) PNB 
*******************************************/

    var netattr =
    {
        HostName: '',
        IPAddress: '',
        MacAddress: ''
    }
    
    function exec_cmd(scmd)
    {
        var retval = "";
        try
        {
            var wsh = new ActiveXObject("WScript.Shell");
            var xhost = wsh.Exec(scmd);
            while (!xhost.StdOut.AtEndOfStream)
            {
                retval += xhost.StdOut.Read(1);
            }
        }
        catch (e)
        {
            retval = null;
        }
        
        return retval;    
    }
    
    function get_string(sout)
    {
        var i = sout.indexOf(" ");
        if (i > -1)
        {
            return sout.substring(0, i);
        }
        return '';
    }
    
    function parse_cmd_out(sout)
    {
        var name_const = "Host Name . . . . . . . . . . . . : ";
        var ip_const = "IP Address. . . . . . . . . . . . : ";
        var mac_const = "Physical Address. . . . . . . . . : ";
        
        var i = sout.indexOf(name_const);
        if (i > -1)
        {
            netattr.HostName = get_string(sout.substring(i + name_const.length));
        }
        i = sout.indexOf(ip_const);
        if (i > 0)
        {
            netattr.IPAddress = get_string(sout.substring(i + ip_const.length));
        }
        i = sout.indexOf(mac_const);
        if (i > 0)
        {
            netattr.MacAddress = get_string(sout.substring(i + mac_const.length));
        }
        
        return netattr;
    }
    
    function get_client_properties()
    {
        var sout = exec_cmd("ipconfig /all");
        if (sout != null)
        {
            return parse_cmd_out(sout);
        }
        return "undefined";
    }