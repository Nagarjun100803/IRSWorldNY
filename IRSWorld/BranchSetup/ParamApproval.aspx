<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ParamApproval.aspx.cs" Inherits="IRSWorld.BranchSetup.ParamApproval" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <title></title>
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery-3.5.1.min.js"></script>
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery-migrate-3.3.1.js"></script>

    <link href="<%=ResolveUrl("~/") %>js/BranchSetup/BS/bootstrap.min.css" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/bootstrap.bundle.min.js"></script>
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/jsParamApproval.js"></script>
    <link href="<%=ResolveUrl("~/") %>js/BranchSetup/BS/buttons.dataTables.min.css" rel="stylesheet" />
    <link href="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="<%=ResolveUrl("~/") %>js/BranchSetup/BS/responsive.dataTables.min.css" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery.dataTables.min.js"></script>
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/dataTables.responsive.min.js"></script>
    <link href="<%=ResolveUrl("~/") %>js/BranchSetup/BS/waitMe.min.css" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/waitMe.min.js"></script>
    <link href="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery-ui.css" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery-ui.min.js"></script>
    <script src="<%=ResolveUrl("~/") %>js/commons.js" type="text/javascript"></script>
    <link href="<%=ResolveUrl("~/") %>css/Default.css" rel="stylesheet" type="text/css" />
    <style>
        .nav-pills .nav-link.active, .nav-pills .show>.nav-link {
            color: #f0f8ff;
            background-color: #5697c1;
        }
        .nav-link {
            font-size: small;
        }
        table.dataTable thead th, table.dataTable thead td {
           padding: 0px 0px;
            font-size: small;
            background-color: #5697c1;
            color: #f0f8ff;
        }
        .table-bordered>:not(caption)>*>* {
            border-width: 1px 1px;
        }
        table.dataTable tbody tr {
            font-size:small;
            padding: 0px 0px;
        }
        /*#tblHitList tbody tr {
            font-size:small;
            padding: 0px 0px;
        }*/
        table.dataTable tbody th, table.dataTable tbody td {
            padding: 3px 8px;
        }
        .form-control:focus {
            box-shadow: 0 0 0 0rem rgb(13 110 253 / 25%);
        }
        .nav-link {
            color: #3f769a;
            font-weight:bold;
        }

    </style>
</head>
<body>
    <form id="Index" runat="server" method="post" autocomplete="off"></form>
    <div id="mainContainer" style="padding-bottom:150px">
        <div id="divMain" class="container" style="padding-top:20px">
            <div id="rowList" class="row">
                <div class="col-sm-12">
                    <div class="panel panel-primary">
                        <div class="divcontent boxshadow" style="background-color:#369!important;padding:3px 0px">
                            <h4 style="color:#fff; text-align:center;"><b><span class="spnTitle" style="font-weight:bold; font-size:large;color:#87ceeb">Parameter Approval</span></b></h4>
                        </div>
                        <div class="panel-body boxshadow" style="background-color:whitesmoke;padding-top:10px">
                            <ul class="nav nav-pills mb-3" id="pills-tab" role="tablist">
                              <li class="nav-item">
                                <a class="nav-link active" id="pills-forapproval-tab" data-toggle="pill" href="#pills-forapproval" role="tab" aria-controls="pills-home" aria-selected="true">FOR APPROVAL</a>
                              </li>
                              <li class="nav-item">
                                <a class="nav-link" id="pills-approved-tab" data-toggle="pill" href="#pills-approved" role="tab" aria-controls="pills-contact" aria-selected="false">APPROVED</a>
                              </li>
                              <li class="nav-item">
                                <a class="nav-link" id="pills-rejected-tab" data-toggle="pill" href="#pills-rejected" role="tab" aria-controls="pills-contact" aria-selected="false">REJECTED</a>
                              </li>
                              <span style="margin-left:550px">
                                  <button type="button" class="btn nav-link active" id="btnAuditLog" data-toggle="modal" data-target="#AuditLogModal">Audit Log</button>
                              </span>
                              
                            </ul>
                            <div class="tab-content" id="pills-tabContent">
                              <div class="tab-pane fade show active" id="pills-forapproval" role="tabpanel" aria-labelledby="pills-home-tab">
                                <div class="panel-bodyNone" style="background-color:whitesmoke">
                                    <table id="tblForApprovalList" class="table table-striped table-bordered" style="width:100%">
                                        <thead>
                                            <tr>
                                                <th class="text-center"></th>
                                                <th class="text-center">Ref#</th>
                                                <th class="text-center">Action Type</th>
                                                <th class="text-center">Maker</th>
                                                <th class="text-center">Status Date</th>
                                                <th class="text-center">Module</th>
                                                <th class="text-center">Menu Name</th>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>
                              </div>
                              <div class="tab-pane fade" id="pills-approved" role="tabpanel" aria-labelledby="pills-contact-tab">
                                <div class="panel-bodyNone" style="background-color:whitesmoke">
                                    <table id="tblApprovedList" class="table table-striped table-bordered" style="width:100%">
                                        <thead>
                                            <tr>
                                                <th class="text-center"></th>
                                                <th class="text-center">Ref#</th>
                                                <th class="text-center">Action Type</th>
                                                <th class="text-center">Maker</th>
                                                <th class="text-center">Date Created</th>
                                                <th class="text-center">Checker</th>
                                                <th class="text-center">Approved Date</th>
                                                <th class="text-center">Module</th>
                                                <th class="text-center">Menu Name</th>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>
                              </div>
                              <div class="tab-pane fade" id="pills-rejected" role="tabpanel" aria-labelledby="pills-contact-tab">
                                <div class="panel-bodyNone" style="background-color:whitesmoke">
                                    <table id="tblRejectedList" class="table table-striped table-bordered" style="width:100%">
                                        <thead>
                                            <tr>
                                                <th class="text-center"></th>
                                                <th class="text-center">Ref#</th>
                                                <th class="text-center">Action Type</th>
                                                <th class="text-center">Maker</th>
                                                <th class="text-center">Date Created</th>
                                                <th class="text-center">Checker</th>
                                                <th class="text-center">Reject Date</th>
                                                <th class="text-center">Module</th>
                                                <th class="text-center">Menu Name</th>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>
                              </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div> <%--end of rowlist--%>


            <div id="rowDetails" class="row" style="display:none">
                <div class="col-sm-12">
                    <div class="panel panel-primary">
                        <div class="divcontent boxshadow" style="background-color:#369!important;padding:3px 0px">
                            <h4 style="color:#fff; text-align:center;"><b><span class="spnTitleDet" style="font-weight:bold; font-size:large;color:#87ceeb"></span></b></h4>
                            <a class="nav-link active" href="#" style="float:right;font-size:medium" onclick="BackMain();">
                                <b>
                                    <svg width="16" height="16" fill="currentColor" class="bi bi-arrow-bar-left" viewBox="0 0 16 16">
                                      <path fill-rule="evenodd" d="M12.5 15a.5.5 0 0 1-.5-.5v-13a.5.5 0 0 1 1 0v13a.5.5 0 0 1-.5.5zM10 8a.5.5 0 0 1-.5.5H3.707l2.147 2.146a.5.5 0 0 1-.708.708l-3-3a.5.5 0 0 1 0-.708l3-3a.5.5 0 1 1 .708.708L3.707 7.5H9.5a.5.5 0 0 1 .5.5z"/>
                                    </svg>
                                    Back to List
                                </b>
                            </a>
                        </div>
	                    <div class="panel-body boxshadow italizeLabel" style="background-color:whitesmoke" id="divcifdetails">
                            <div class="row">
                                <div class="col-sm-2" style="padding-right:0px">
                                </div>
                            </div>
                            <div class="rowNone">
                                <div class="panel-bodyNone" style="background-color:whitesmoke">
                                    <table id="tblDetailedist" class="table table-striped table-bordered nowrap" style="width:100%">
                                        <thead>
                                            <tr>
                                                <th class="text-center">Action Type</th>
                                                <th class="text-center">Menu Name</th>
                                                <th class="text-center">Field Name</th>
                                                <th class="text-center">Before Value</th>
                                                <th class="text-center">After Value</th>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>
                            </div>
                            
                            <div id="divBtns" class="row" style="text-align:right">
                                <div class="col-sm-12" style="text-align:right;padding-top:25px">
                                    <div class="form-group" style="text-align:right">
                                        <button id="btnReject" type="submit" style="margin-right:30px" class="btn btn-outline-danger" onclick="confirmAction('Rejected');">Reject</button>
                                        <button id="btnApprove" type="submit" class="btn btn-outline-primary" onclick="confirmAction('Approved');">Approve</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                            
	                </div>
                </div>
            </div>

            <!--modal auditlog-->
            <div class="modal fade" id="AuditLogModal" tabindex="-1" role="dialog" aria-hidden="true">
              <div class="modal-dialog" role="document">
                <div class="modal-content">
                  <div class="modal-header">
                    <h5 class="modal-title" id="AuditLogModalLabel">Generate Audit Log</h5>
                  </div>
                  <div class="modal-body">
                    <div class="row">
                        <label for=""><h6 class="modal-title">Parameter Change Initiated</h6></label>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label for="">Date From</label>
                                <input id="txtDateFrom" placeholder="mm/dd/yyyy" type="date" tabindex="0"/>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label for="">Date To</label>
                                <input id="txtDateTo" placeholder="mm/dd/yyyy" type="date" tabindex="0"/>
                            </div>
                        </div>
                    </div>
                      <div class="row">
                          <span id="spnerror" style="color:red"></span>
                      </div>
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn nav-link" data-dismiss="modal">Close</button>
                    <button type="button" class="btn nav-link" onclick="genAuditLog();">Generate</button>          
                  </div>
                </div>
              </div>
            </div>

            <!--MessageBox Section-->
            <div id="divMessageBox">
                <label id="lblMessagebox"></label>
            </div>

            <!--Sending of Password-->
            <div id="divPopup" style="width: 400px;border: solid 1px midnightblue; display: none; z-index: 10; vertical-align:middle; text-align:center;">
                <table  cellpadding="0" cellspacing="0">
                    <tr>
                        <td id=td_popup align=center style="width:100%; vertical-align:middle">
                            <div style=" position: absolute; height: auto; width: 400px; top: 326.508px; left: 810px; z-index: 101;">
                            <table  cellpadding="0" cellspacing="0">
                                <tr>
                                    <td id=pop_heading style="width:400px; height: 24px; color:Ivory; font: bold 12px Arial" class="table_head_dark" align="left">&nbsp;&nbsp;Password Mailing</td>
                                </tr>
                                <tr>
                                    <td class="cell_desc" style="width: 100%; height: 25px; text-align: left; text-indent: 5px">
                                        A Password was generated for:
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%; text-align: left;">
                                        <table cellspacing=5 style="width:100%">
                                            <tr>
                                                <td class=cell_caption style="width: 30%">User ID:&nbsp;
                                                </td>
                                                <td id=popUserId class=cell_desc style="width:70%; text-align:left"></td>
                                            </tr>
                                            <tr>
                                                <td class=cell_caption style="width: 30%">User Name:&nbsp;
                                                </td>
                                                <td id=popUserName class=cell_desc style="width:70%; text-align:left"></td>
                                            </tr> 
                                            <tr>
                                                <td class=cell_caption style="width: 30%">CIF ID:&nbsp;
                                                </td>
                                                <td id=popCifId class=cell_desc style="width:70%; text-align:left"></td>
                                            </tr> 
                                            <tr>
                                                <td colspan=2 style="width:100%;" align=left>
                                                    <table id=tblemail style="width:100%; display:block" cellspacing=0>
                                                        <tr>
                                                            <td class=cell_desc style="width: 100%" colspan=2>The password will be sent through email:
                                                            </td>
                                                        <tr>
                                                        <tr>
                                                            <td class=cell_caption style="width: 30%">Email Address:&nbsp;
                                                            </td>
                                                            <td id=popTdEmail class=cell_desc style="width:70%; text-align:left; vertical-align: top; padding-left: 5px">
                                                                <select style="width:100%" id=popddemail></select>
                                                            </td>
                                                        </tr>                                                   
                                                    </table>
                                                    <table id=tblnoemail style="width:100%; display:none" cellspacing=0>
                                                        <tr>
                                                            <td class=cell_desc style="text-align:left; width:400px">
                                                                No registered email address. You may enter an email address below or view the password and personally disclose it to the client.
                                                            </td>
                                                        </tr> 
                                                        <tr>
                                                            <td class=cell_desc style="text-align:left; width:100%">
                                                                <input type=text id=popTxtEmail style="width:99%; text-align:left" onchange="pop_email_changed(this);" />
                                                            </td>
                                                        </tr>                                                   
                                                    </table>
                                                    <table id=tblviewpwd style="width:100%; display:none" cellspacing=0>
                                                        <tr>
                                                            <td class=cell_desc style="text-align:left; width:100%">The password is:
                                                            </td>
                                                        </tr>                                                 
                                                        <tr>
                                                            <td id=tdviewpwd class=cell_desc align=center style="text-align:center; width:100%; font-size:larger; font-weight:bold">Password
                                                            </td>
                                                        </tr>                                           
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr><td style="width:100%">&nbsp;</td></tr>
                                <tr>
                                    <td style="width:100%;vertical-align:middle" align="center">
                                        <div style="width:100%; vertical-align:top" align=center>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 18%">
                                                        <input id="btnSend" class="button" onclick="btn_send_click(this);" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Send" /></td>
                                                    <td style="width: 18%">
                                                        <input id="btnView" class="button" onclick="btn_view_click(this);" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="View" style="display:none" /></td>
                                                    <td style="width: 18%">
                                                        <input id="btnClose" class="button" onclick="close_popup();" onmouseover="this.className='warning_over';" onmouseout="this.className='button';" type="button" value="Close" /></td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>                  
                                            </table>
                                        </div>
                                    </td>
                                </tr> 
                            </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <!--End of Sending of Password-->

        </div>
    </div>
</body>
</html>
