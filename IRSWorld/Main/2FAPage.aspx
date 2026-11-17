<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="2FAPage.aspx.cs" Inherits="IRSWorld.Main._2FAPage" %>

<!DOCTYPE html>

<html>
    <head>

         <script src="../js/Admin/jsMultiFactor.js" type="text/javascript"></script>
        <script src="../js/jsCommons.js" type="text/javascript"></script>
         <script src="../js/Main/jsMain.js?v=193083" type="text/javascript"></script>
 
        <style>
            div#divPasswd{
                       width: 37% !important;
                        /*height: expression(document.documentElement.availHeight);*/
                        display: block;
                        vertical-align: middle;
                        margin: 0 auto;
                        height: 258px;
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
                    font-size: 12px;
                    padding-top: 10px;
                    padding-left: 10px;
                    display: block;
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
                    text-align:center;
                    /* height: 10px; */
            }
             div#divPasswd input.btn-submit:hover{
                 background:#218e8e;
             }
            div#divPasswd input.btn-submit{
                background: #70c5c5;
                color: #fff;
                border-style: none;
                font-weight: bold;
                padding: 9px 20px;
                cursor:pointer;
                border-radius: 7px;
            }
        </style>
    </head>
<body style="background-color:#171717;">
<form id="form1" runat="server">
 <div id="divPasswd" style="width: 100%; height:expression(document.documentElement.availHeight); display:block; vertical-align:middle" class="opacified">
     <div class="autheader">
         <img src="../images/Entrust_logo.png" />
     </div>
     <div class="autbody">
         <span>Set up one-time password soft token</span>
         <span>Please enter the generated one-time password to verify the setup</span>
         <span>One-Time Password</span>
         <div class="inputcontrol">
            <input type="text" id="txtTPass" class="ipt-txt"/>
         </div>
     </div>
     <div class="autfooter">
         <input type="button" value="Submit"  onclick="validate_token();" class="btn-submit"/>
     </div>
 </div>
</form>
</body>
</html>