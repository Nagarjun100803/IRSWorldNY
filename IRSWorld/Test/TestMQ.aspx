<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestMQ.aspx.cs" Inherits="IRSWorld.Test.TestMQ" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>MQ Test Page</title>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script type="text/javascript">
    
        function init_ddl(tid, dt, val, desc)
        {
            var ddl = document.getElementById(tid);
            ddl.options.length = 0;
            create_option(ddl, "Default", "Default");
            if (dt.Rows.length > 0)
            {
                for (var i = 0; i < dt.Rows.length; i++)
                {
                    create_option(ddl, dt.Rows[i][val], dt.Rows[i][desc]);
                }  
                
                ddl.selectedIndex = 0;         
            }
        }
            
        function load_ddl_settings(rs)
        {
            if (rs.error != null)
            {
                alert(rs.error);
                return;
            }

            init_ddl("ddTest", rs.value, "fldValue", "fldDesc");
            document.getElementById('txtAccount').focus();
        }
        
        function display(ht)
        {
            document.getElementById("divTest").innerText += ht[0] + "\n";;
            
            var value = ht[1];
            if (value[value.length - 1].constructor == String && value[value.length -1].indexOf('Hashtable') > - 1)
            {
                display(value);
            }
            else
            {
                for (var i = 1; i < ht.length; i++)
                {
                    document.getElementById("divTest").innerText += ht[i] + "\n";;
                }
            }
        }
        
        function hasAccountDetails(rs)
        {
            if (rs.error != null)
            {
                alert(rs.error.Message);
                return;
            }
            
            if (rs.value.length > 0)
            {
                for (var i = 0; i < rs.value.length; i++)
                {
                    var ht = rs.value[i];
                    display(ht);
                }
            }
        }
        
        function test_mq()
        {
            var txt = document.getElementById('txtAccount');
            if (txt.value.length > 0)
            {
                var setid = document.getElementById('ddTest').value;
                IRSWorld.Test.TestMQ.getAccountDetails(setid, txt.value, hasAccountDetails);
                return;
            }
            
            alert("Provide a test account number.");
            txt.focus();
        }
    
        function page_setup()
        {
            IRSWorld.Test.TestMQ.getSettingIDs(load_ddl_settings);
        }
        
    </script>
</head>
<body onload="page_setup();">
    <form id="form1" runat="server">
    <div>
        <table style="width:100%">
            <tr>
                <td style="width: 20%; font-weight: bold; color: navy; font-family: Arial; text-align: right; font-size:small">Acount No:&nbsp;</td>
                <td style="width: 80%">
                    <input type=text id="txtAccount" style="width: 300px" />
                    </td>
            </tr>
            <tr>
                <td style="width: 20%; font-weight: bold; color: navy; font-family: Arial; text-align: right; font-size:small">
                    Settings ID:&nbsp;</td>
                <td style="width: 80%">
                    <select id="ddTest" style="width: 305px">
                        <option selected="selected"></option>
                    </select>
                </td>
            </tr>
            <tr>
                <td style="width: 20%">&nbsp;</td>
                <td style="width: 80%">
                    <input id="btnTest" type="button" value="Go!" style="width: 52px" onclick="test_mq();" /></td>
            </tr>
            <tr>
                <td style="width: 100%" colspan=2><hr />
                </td>
            </tr>

            <tr>
                <td style="width: 90%"  colspan=2>
                    <div id=divTest style="width: 100%; height: 400px; overflow:auto; font-size: small; font-weight: bold; color: navy; font-family: Arial;">
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
