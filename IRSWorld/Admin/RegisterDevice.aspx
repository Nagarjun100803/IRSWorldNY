<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterDevice.aspx.cs" Inherits="IRSWorld.Admin.RegisterDevice" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head>

         <script src="../js/Admin/jsMultiFactor.js?v=5456" type="text/javascript"></script>
        <script src="../js/jsCommons.js" type="text/javascript"></script>
         <script src="../js/Main/jsMain.js?v=124334" type="text/javascript"></script>

        <style>
            div#divPasswd{
                       width: 50% !important;
                        /*height: expression(document.documentElement.availHeight);*/
                        display: block;
                        vertical-align: middle;
                        margin: 0 auto;
                        height: 600px;
                        margin-top: 25px;
                        background: #fff;
                        border-radius: 6px;
            }
            div#divPasswd img{
                    width: 26%;
                    padding: 7px;
            }
            div.autheader{
                    border-bottom: 1px solid #70c5c5;
            }
            div#divPasswd span{
                      color: #636161;
                    font-family: 'tahoma';
                    font-size: 15px;
                    padding-top: 10px;
                    padding-left: 10px;
                    display: block;
                    text-align: left;
                    margin-left: 150px;
            }
            div.inputcontrol{
                padding: 25px;
            }
            div.inputcontrol,div.autfooter{
                text-align:center;
            }
            div#divPasswd input.ipt-txt{
                        width: 150px;
                    border: 1px solid #7d7d7d;
                    border-radius: 6px;
                    line-height: 21px;
                    width: 175px !important;
                    text-align: center;
                    /* height: 10px; */
                    margin-top: 10px;
                    width: 46% !important;
                    padding: 3px;
                    font-size: 17px;
            }
             div#divPasswd input.btn-submit:hover{
                 background:#218e8e;
             }
            div#divPasswd input.btn-submit{
               background: #70c5c5;
                color: #fff;
                border-style: none;
                font-weight: bold;
                padding: 10px 25px;
                cursor: pointer;
                border-radius: 7px;
                margin-top: 16px;
            }
            div.autbody{
                text-align:center;
            }
            label#lblPassword{
                        font-size: 24px !important;
            }
        </style>
        <script type="text/javascript">
          

        </script>
    </head>
<body style="background-color:#171717;" onload="get_registration_data();">
    <form id="form1" runat="server">
    <div>
            <div id="divPasswd" style="width: 100%; height:expression(document.documentElement.availHeight); display:block; vertical-align:middle" class="opacified">
             <div class="autheader">
                 <img src="../images/Entrust_logo.png" />
             </div>
             <div class="autbody">
                 <span>Step 1. Download Entrust on your Android or iOS device</span>
             
                 <span>Step 2. Scan QR Code Below</span>
            
                 <img id="imgQR" style="width:250px; height:250px" />
                
                 <span>Step 3. Enter the password below on your device</span>
                 <span><label id="lblPassword" style="font-weight:bold; font-size:16px"></label></span>
               
                 <span style="display:none">Step 4. Enter the registration code shown on your device</span>
                 <input type="text" id="txtRegistrationCode" class="ipt-txt" style="display:none" />
             </div>
             <%--<div class="autfooter">
                 <input type="button" value="Validate"  onclick="complete_registration();" class="btn-submit"/>
             </div>--%>
         </div>
    </div>
    </form>
</body>
</html>