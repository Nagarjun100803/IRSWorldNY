<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="IRSWorld.OnlineHelp.Help" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>IRSWorld Online Help</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <link href="../js/jstree/dtree.css" rel="stylesheet" type="text/css" />    
    <script src="../js/jstree/dtree.js" type="text/javascript"></script>
    <script src="../js/jsCommons.js" type="text/javascript"></script> <!-- added by chr 11/21/2010 for change password to work -->
    <script>
        // added to enabled page scroll bars 
        function fix_dim()
        {
            if (top.document.getElementById('divCenter'))
            {
                var div = document.getElementById('divmain');
                if (div)
                {
                    div.style.width = (top.document.getElementById('divCenter').offsetWidth) + 'px';
                    div.style.height = (top.document.getElementById('divCenter').offsetHeight - 30) + 'px';
                }
            }
        }
    </script>
</head>
<body onload="fix_dim();" onresize="fix_dim();">
    <form id="form1" runat="server">
        <div id="divMain" style="width:100%; height:100%; padding-top:30px; overflow:auto;">
            <table style="width:100%; height:100%" cellpadding="0" cellspacing="5">
                <tr>    
                    <td align="center" valign="middle" style="width:100%; border-collapse:collapse">
                        <table id="tblHelp" style="width:90%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width:60%; vertical-align:bottom; text-align:left" align="left">
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" valign="bottom" style="width:0%;height:24px; border-collapse: collapse"> 
                                                <img src="../images/toe.GIF" /></td>
                                            <td id="capLabel" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse">
                                                IRSWorld Online Help</td>
                                            <td align="left" valign="bottom" style="width:60%; height: 24px" class="bg_body"> 
                                                <img align="absbottom" src="../images/rend.gif" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; border:solid 3px #336699;" class=table_body>
                                    <table style="width:100%; background-color:#839fcf; " cellspacing="3">
                                        <tr>
                                            <td id="tdTree" align=left valign=top style="width: 25%; height: 500px">
                                                <table style="width:100%; height:100%" cellspacing="2" cellpadding="0">                                              
                                                    <tr>
                                                        <td style="width:100%; height:100%;">
                                                        
                                                                    <div class="dtree" style="width:250px; height:500px; overflow-y:auto">                                                                            
                                                                        <div align="center"><a href="javascript: d.openAll();">open all</a> | <a href="javascript: d.closeAll();">close all</a></div>
                                                                            <script type="text/javascript">
                                                                            <!--
                                                                            d = new dTree('d');

                                                                            d.add(0,-1,'IRSWorld v1.0');
                                                                            
                                                                            d.add(1,0,'INTRODUCTION');
                                                                                d.add(2,1,'IRSWorld','pages\\intro.htm','Introduction','iframeHelp');
                                                                                d.add(3,1,'Logging In and Out','pages\\loginout.htm','How to Login','iframeHelp');                                                                               
                                                                                
                                                                            d.add(10,0,'MAINTENANCE');
                                                                                d.add(11,10,'System Administration');
                                                                                    d.add(12,11,'User Administration','pages\\users.htm','User Maintenance','iframeHelp');
                                                                                    d.add(13,11,'Menu Maintenance','pages\\menu.htm','Menu Maintenance','iframeHelp');
                                                                                    d.add(14,11,'Modules Entry','pages\\modules.htm','Module Maintenance','iframeHelp');
                                                                                    d.add(15,11,'Default Profiles','pages\\profiles.htm','Profile Maintenance','iframeHelp');
                                                                                    d.add(16,11,'Lookup Maintenance','pages\\lookup.htm','Lookup Maintenance','iframeHelp');
                                                                                    d.add(17,11,'Reports Lookup','pages\\replookup.htm','Reports Lookup','iframeHelp');
                                                                                    d.add(18,11,'Report Setup','pages\\repsetup.htm','Report Setup','iframeHelp');
                                                                                    d.add(19,11,'FCR Interface Setup','pages\\fcrintsetup.htm','FCR Interface Setup','iframeHelp');
                                                                                    d.add(20,11,'Zip Codes','pages\\zipcodes.htm','Zip Codes','iframeHelp');
                                                                                d.add(21,10,'Branch Administration');
                                                                                    d.add(22,21,'Branch Setup','pages\\branch.htm','Branch Maintenance','iframeHelp');
                                                                                d.add(23,10,'CIF Maintenance');   
                                                                                    d.add(24,23,'CIF','pages\\cif.htm','CIF Maintenance','iframeHelp'); 
                                                                                d.add(25,10,'CIF GEFU Maintenance');   
                                                                                    d.add(26,25,'CIF GEFU','pages\\cifgefu.htm','CIF GEFU Maintenance','iframeHelp'); 
                                                                                d.add(27,10,'IRS GEFU Maintenance');   
                                                                                    d.add(28,27,'IRS GEFU','pages\\irsgefu.htm','IRS GEFU Maintenance','iframeHelp');                                                                                     
//                                                                                d.add(29,10,'GRS GEFU Maintenance');   
//                                                                                    d.add(30,29,'GRS GEFU','pages\\grsgefu.htm','GRS GEFU Maintenance','iframeHelp');  
                                                                                                                                                                                                                                                           
                                                                            d.add(40,0,'MY ACCOUNT');               
                                                                                    d.add(41,40,'Change Password','pages\\changepass.htm','Change Account Password','iframeHelp');
                                                                                   
                                                                            d.add(50,0,'TRANSACTION');               
                                                                                    d.add(51,50,'Bills Payment','pages\\bills.htm','Bills Payment','iframeHelp');                                                                                 
                                                                                    d.add(52,50,'OTC Remittance','pages\\otcremit.htm','OTC Remittance','iframeHelp');
                                                                                    d.add(53,50,'Approval/Amendments','pages\\appamend.htm','Approval/Amendments','iframeHelp');
                                                                                    d.add(54,50,'File Upload','pages\\fileuploadtxn.htm','File Upload - Transaction','iframeHelp');
                                                                                    d.add(55,50,'ORD Data Entry','pages\\orddataentry.htm','ORD Data Entry','iframeHelp');
                                                                                                                                                                        
                                                                            d.add(80,0,'REMIT ONLINE');
                                                                                d.add(81,80,'Internet Transaction','pages\\inettxn.htm','Internet Transaction','iframeHelp');
                                                                                d.add(82,80,'Internet Remitter','pages\\inetremit.htm','Internet Remitter','iframeHelp');
                                                                                d.add(83,80,'ARM','pages\\arm.htm','Automated Remittance Machine','iframeHelp');
                                                                                d.add(84,80,'Agent Remittance','pages\\agentremit.htm','Agent Remittance','iframeHelp');
//                                                                                d.add(83,80,'ARS File Upload','pages\\arsfileupload.htm','ARS File Upload','iframeHelp');

                                                                            d.add(90,0,'COMPLIANCE');
                                                                                d.add(91,90,'Compliance Viewer','pages\\compview.htm','Compliance Viewer','iframeHelp');
//                                                                                d.add(92,90,'Hits','pages\\hits.htm','Hits','iframeHelp');
                                                                                d.add(93,90,'File Upload','pages\\fileuploadcomp.htm','File Upload - Compliance','iframeHelp');
//                                                                                d.add(94,90,'File Loader Setup','pages\\fileloadersetup.htm','File Loader Setup','iframeHelp');
                                                                                
                                                                            d.add(100,0,'INQUIRY');
                                                                                d.add(101,100,'Inquiry','pages\\inquiry.htm','Inquiry','iframeHelp');
                                                                                
//                                                                            d.add(110,0,'APAS');
//                                                                                d.add(111,110,'APAS Transaction','pages\\apastxn.htm','APAS Transaction','iframeHelp');
//                                                                                d.add(112,110,'APAS Reactivate Transactions','pages\\apasreacttxn.htm','APAS Reactivate Transactions','iframeHelp');
//                                                                                d.add(113,110,'APAS Status','pages\\apasstat.htm','APAS Status','iframeHelp');
//                                                                                d.add(114,110,'APAS Unlock Transaction','pages\\apasunlocktxn.htm','APAS Unlock Transaction','iframeHelp');
//                                                                                d.add(115,110,'Amendment/Cancellation/Approval','pages\\amendcancelapp.htm','Amendment/Cancellation/Approval','iframeHelp');
////                                                                                d.add(116,110,'APAS Reports','pages\\apasrep.htm','APAS Reports','iframeHelp');
                                                                                
                                                                            d.add(120,0,'REPORTS');
                                                                                d.add(121,120,'Reports','pages\\reports.htm','Reports','iframeHelp');

//                                                                            d.add(130,0,'GFC');
//                                                                                d.add(131,130,'GFC Inquiry','pages\\gfcinq.htm','GFC Inquiry','iframeHelp');
//                                                                                d.add(132,130,'Card Replacement/Cancellation','pages\\cardrepcan.htm','Card Replacement/Cancellation','iframeHelp');
//                                                                                d.add(133,130,'Card Replacement Approval','pages\\cardrepapp.htm','Card Replacement Approval','iframeHelp');
//                                                                                d.add(134,130,'Card Cancellation Approval','pages\\cardcanapp.htm','Card Cancellation Approval','iframeHelp');
//                                                                                d.add(135,130,'Purchase','pages\\purchase.htm','Purchase','iframeHelp');
//                                                                                d.add(136,130,'Availment','pages\\availment.htm','Availment','iframeHelp');
//                                                                                d.add(137,130,'Claim','pages\\claim.htm','Claim','iframeHelp');
//                                                                                d.add(138,130,'GFC Uploads','pages\\gfcuploads.htm','GFC Uploads','iframeHelp');
//                                                                                d.add(139,130,'Batch Upload','pages\\batchupload.htm','Batch Upload','iframeHelp');
//                                                                                d.add(140,130,'Batch Enrollment','pages\\batchenroll.htm','Batch Enrollment','iframeHelp');
//                                                                                d.add(141,130,'GFC Aggregation','pages\\gfcagg.htm','GFC Aggregation','iframeHelp');
//                                                                                d.add(142,130,'GFC Locked Transaction','pages\\gfclocktxn.htm','GFC Locked Transaction','iframeHelp');
//                                                                                d.add(143,130,'GFC Aggregation Inquiry','pages\\gfcagginq.htm','GFC Aggregation Inquiry','iframeHelp');

//                                                                            d.add(150,0,'PAW');
//                                                                                d.add(151,150,'PAW Transaction','pages\\pawtxn.htm','PAW Transaction','iframeHelp');
//                                                                                d.add(152,150,'PAW Reactivate Transaction','pages\\pawreactivatetxn.htm','PAW Reactivate Transaction','iframeHelp');
//                                                                                d.add(153,150,'PAW Status','pages\\pawstatus.htm','PAW Status','iframeHelp');
//                                                                                d.add(154,150,'PAW Unlock Transaction','pages\\pawunlocktxn.htm','PAW Unlock Transaction','iframeHelp');

//                                                                            d.add(160,0,'APAS/PAW Setup');
//                                                                                d.add(161,160,'Setup APAS/PAW','pages\\apaspawsetup.htm','APAS/PAW Setup','iframeHelp');
////                                                                                d.add(161,160,'Payout Currency','pages\\payoutcurr.htm','Payout Currency','iframeHelp');
////                                                                                d.add(162,160,'PNB Branches Setup','pages\\pnbbranchsetup.htm','PNB Branches Setup','iframeHelp');
////                                                                                d.add(163,160,'Payout Partner Setup','pages\\payoutpartsetup.htm','Payout Partner Setup','iframeHelp');
////                                                                                d.add(164,160,'Payout Partner Fee','pages\\payoutpartfee.htm','Payout Partner Fee','iframeHelp');
////                                                                                d.add(165,160,'Approval Setup','pages\\appsetup.htm','Approval Setup','iframeHelp');
////                                                                                d.add(166,160,'APAS Sub Payout Partner Branch','pages\\appsubpaypartbranch.htm','APAS Sub Payout Partner Branch','iframeHelp');
      
                                                                            d.add(180,0,'3D');
                                                                                d.add(181,180,'Setup Courier Account','pages\\setupcourieracct.htm','Setting up a Courier Account','iframeHelp');
                                                                                d.add(182,180,'Extract 3D','pages\\extract3d.htm','Extract 3D','iframeHelp');
                                                                                d.add(183,180,'ORD3D Monitoring List','pages\\ord3dmonlist.htm','ORD3D Monitoring List','iframeHelp');
                                                                                d.add(184,180,'Courier Transaction List','pages\\couriertxnlist.htm','Courier Transaction List','iframeHelp');

                                                                            d.add(190,0,'Migration');
                                                                                d.add(191,190,'Match CIF System Installation','pages\\matchcifinstall.htm','Match CIF System Installation','iframeHelp');
                                                                                d.add(192,190,'Migration Installation and Setup','pages\\sysmiginstallsetup.htm','System Migration Installation and Setup','iframeHelp');

                                                                        document.write(d);
                                                                            //-->
                                                                    </script>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                  
                                                </table>
                                            </td>
                                            <td class="header" valign="top" align="left" style="width:75%; height: 500px">
                                                <iframe name="iframeHelp" id="iframeHelp" src="pages\default.htm" frameborder="0" style="height:100%; width:100%; margin-bottom:0; margin-left:0; margin-right:0; margin-top:0; background-color:#EDF4F8;"></iframe>                                               
                                            </td>
                                        </tr>
                                    </table>
                                    </td>
                            </tr>
                        </table> 
                    </td>
                </tr>
            </table>    
        </div>     
    </form>
</body>
</html>

