var gtmpseqid;
var gmenuname;
var gcommandtype;
var guserrolecode = '0';
var param_approval = this;

$(document).ready(function () {

    //initialization();
    //getrolecode();
    loadAllStatusList();
    getrolecode();

    $('#txtDateFrom').on('change', function () {
        $('#spnerror').text('');
    }); 

});

function getrolecode() {
    try {
        IRSWorld.BranchSetup.ParamApproval.getrolecode(function (result) {
            if (result.error == null) {
                guserrolecode = result.value;
                if (guserrolecode == '1') {
                    $('.spnTitle').text('Parameter Approval (Processor Page)');
                }
                else if (guserrolecode == '2') {
                    $('.spnTitle').text('Parameter Approval (Approver Page)');
                }
                //=======================================================================
                //RFC-31620 : IAMD Maker-Checker
                else if (guserrolecode == '3') {
                    $('.spnTitle').text('Administrator Approval (Processor Page)');
                }
                else if (guserrolecode == '4') {
                    $('.spnTitle').text('Administrator Approval (Approver Page)');
                }
                //=======================================================================

                else {
                    spawn_errpage('Role is neither processor or approver.');
                }
            }
            else {
                spawn_errpage('Failed to get role. Please try again.');
            }
        });
    }
    catch (err) { //err.message
        spawn_errpage('Failed to get role. Please try again.');
    }
}

function loadAllStatusList() {
    getStatusList('For Approval', '#tblForApprovalList');
    getStatusList('Approved', '#tblApprovedList');
    getStatusList('Rejected', '#tblRejectedList');
    
    BackMain();
}

function runWaitMe(title) {
    if (title == "" || title == undefined) {
        title = "Please wait . . .";
    }
    else {
        title = title + ' . . .'
    }
    $('#mainContainer').waitMe({
        effect: 'bounce',
        text: title,
        bg: 'rgba(255, 255, 255, 0.7)',
        color: '#000'
    });
}

function stopWaitMe() {
    $('#mainContainer').waitMe('hide');
}

function basicAlertMsg(title, message) {
    setText(document.getElementById('lblMessagebox'), '');
    setText(document.getElementById('lblMessagebox'), message);

    $(function () {
        $("#divMessageBox").dialog({
            title: title,
            modal: true,
            overlay: {
                draggable: false,
                position: "center",
                modal: true,
                backgroundColor: '#000',
                opacity: 0.75
            }
        });
    });
    $(".ui-widget-header").css("background", "#70c5c5");
}

function basicAlertMsgHTML(title, msg) {

    $("<div>", {
        html: msg,
        title: title
    }).dialog(
    {
        draggable: false,
        position: "top",
        modal: true,
        zIndex: 9999,
        buttons: {
            Close: function () {
                $(this).dialog("close");
            }
        }
    });
    $(".ui-widget-header").css("background", "#70c5c5");
}

function getStatusList(stat, gridname) {
    runWaitMe();
    AjaxPro.timeoutPeriod = 300 * 1000;
    AjaxPro.onTimeout = function () {
        showErrorMsg("Internal Server Error", "The server has stopped responding. Please try again later.");
    }
    try {
        IRSWorld.BranchSetup.ParamApproval.getStatusList(stat, function (result) {
            stopWaitMe();
            if (result.error == null) {
                var data = result.value;
                var isview = '1';
                if (data != null) {
                    $(gridname).DataTable().clear().draw();
                    if (stat == 'For Approval') {
                        $(gridname).DataTable({
                            responsive: true,
                            "destroy": true,
                            "bSort": false,
                            dom: 'Bfrtip',
                            "pageLength": 15,
                            data: data,
                            columns: [
                                {
                                    data: 0,
                                    render: function (data, type, row, meta) {
                                        if (stat == 'For Approval') {
                                            isview = '0';
                                        }
                                        return '<center><button type="button" title="View Details" class="btn btn-sm btn-outline-primary" onclick="getStatusListDet(\'' + data + '\',\'' + row[7] + '\',\'' + isview + '\');">' +
                                            '<svg width="16" height="16" fill="currentColor" class="bi bi-zoom-in" viewBox="0 0 16 16"><path fill-rule="evenodd" d="M6.5 12a5.5 5.5 0 1 0 0-11 5.5 5.5 0 0 0 0 11zM13 6.5a6.5 6.5 0 1 1-13 0 6.5 6.5 0 0 1 13 0z"/><path d="M10.344 11.742c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1 6.538 6.538 0 0 1-1.398 1.4z"/><path fill-rule="evenodd" d="M6.5 3a.5.5 0 0 1 .5.5V6h2.5a.5.5 0 0 1 0 1H7v2.5a.5.5 0 0 1-1 0V7H3.5a.5.5 0 0 1 0-1H6V3.5a.5.5 0 0 1 .5-.5z"/></svg>' +
                                            '&nbsp;&nbsp;</button></center>'
                                    }
                                },
                                { data: 0, "visible": true },
                                { data: 1 },
                                { data: 2 },
                                { data: 5 },
                                { data: 6 },
                                { data: 7 },
                            ]
                        });
                    }
                    else {
                        $(gridname).DataTable({
                            responsive: true,
                            "destroy": true,
                            "bSort": false,
                            dom: 'Bfrtip',
                            "pageLength": 15,
                            data: data,
                            columns: [
                                {
                                    data: 0,
                                    render: function (data, type, row, meta) {
                                        if (stat == 'For Approval') {
                                            isview = '0';
                                        }
                                        return '<center><button type="button" title="View Details" class="btn btn-sm btn-outline-primary" onclick="getStatusListDet(\'' + data + '\',\'' + row[7] + '\',\'' + isview + '\');">' +
                                            '<svg width="16" height="16" fill="currentColor" class="bi bi-zoom-in" viewBox="0 0 16 16"><path fill-rule="evenodd" d="M6.5 12a5.5 5.5 0 1 0 0-11 5.5 5.5 0 0 0 0 11zM13 6.5a6.5 6.5 0 1 1-13 0 6.5 6.5 0 0 1 13 0z"/><path d="M10.344 11.742c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1 6.538 6.538 0 0 1-1.398 1.4z"/><path fill-rule="evenodd" d="M6.5 3a.5.5 0 0 1 .5.5V6h2.5a.5.5 0 0 1 0 1H7v2.5a.5.5 0 0 1-1 0V7H3.5a.5.5 0 0 1 0-1H6V3.5a.5.5 0 0 1 .5-.5z"/></svg>' +
                                            '&nbsp;&nbsp;</button></center>'
                                    }
                                },
                                { data: 0, "visible": true },
                                { data: 1 },
                                { data: 2 },
                                { data: 3 },
                                { data: 4 },
                                { data: 5 },
                                { data: 6 },
                                { data: 7 },
                            ]
                        });
                    }
                    

                    var table = $(gridname).DataTable();
                    table.responsive.rebuild();
                    table.responsive.recalc();

                }
                else {
                    spawn_errpage("[PARAMETER LIST] Failed to fetch change parameter list. Please try again later.");
                }
            }
            else {
                stopWaitMe();
                spawn_errpage("[PARAMETER LIST] Failed to fetch change parameter list. Please try again later.");
            }
        });
    }
    catch (err) { //err.message
        stopWaitMe();
        spawn_errpage("[PARAMETER LIST] " + err.message);
    }
}

function getStatusListDet(seq, menu, isview) {
    gtmpseqid = seq;
    gmenuname = menu;
    var gridname = '#tblDetailedist';
    runWaitMe();
    AjaxPro.timeoutPeriod = 300 * 1000;
    AjaxPro.onTimeout = function () {
        showErrorMsg("Internal Server Error", "The server has stopped responding. Please try again later.");
    }
    try {
        IRSWorld.BranchSetup.ParamApproval.getStatusListDet(seq, menu, isview,  function (result) {
            stopWaitMe();
            if (result.error == null) {

                ViewDetails(isview);

                var data = result.value;
                if (data != null) {
                    $(gridname).DataTable().clear().draw();
                    $(gridname).DataTable({
                        responsive: true,
                        "destroy": true,
                        "bSort": false,
                        "bFilter":false,
                        dom: 'Bfrtip',
                        "pageLength": 15,
                        data: data,
                        columns: [
                            { data: 0 },
                            { data: 1 },
                            { data: 2 },
                            { data: 3 },
                            { data: 4 },
                        ]
                    });

                    var table = $(gridname).DataTable();
                    table.responsive.rebuild();
                    table.responsive.recalc();
                }
                else {
                    spawn_errpage("[PARAMETER DETAIL] Failed to fetch details. Please try again later.");
                }
            }
            else {
                stopWaitMe();
                spawn_errpage("[PARAMETER DETAIL] Failed to fetch details. Please try again later.");
            }
        });
    }
    catch (err) { //err.message
        stopWaitMe();
        spawn_errpage("[PARAMETER DETAIL] " + err.message);
    }
}


function BackMain() {
    $('#rowList').show();
    $('#rowDetails').hide();
}

function ViewDetails(isview) {
    $('#rowList').hide();
    $('#rowDetails').show();
    if (isview == '0') {
        //=======================================================================
        //RFC-31620 : IAMD Maker-Checker
        if (guserrolecode == '2' || guserrolecode == '4') {
            //=======================================================================
            $('#divBtns').show()
        }
        else {
            $('#divBtns').hide()
        }
    }
    else {
        $('#divBtns').hide()
    }
}

function updateStatus(stat) {
    runWaitMe();
    AjaxPro.timeoutPeriod = 300 * 1000;
    AjaxPro.onTimeout = function () {
        showErrorMsg("Internal Server Error", "The server has stopped responding. Please try again later.");
    }
    try {
        IRSWorld.BranchSetup.ParamApproval.updateStatus(gtmpseqid, stat, gmenuname, function (result) {
            if (result.error == null) {
                stopWaitMe();
                var data = result.value;
                if (data == '1') {
                    //=======================================================================
                    //RFC-31620 : IAMD Maker-Checker
                    //loadAllStatusList();
                    basicAlertMsg("UPDATESTAT", "Successfully saved.");
                    
                    if (stat == 'Approved') {
                        IRSWorld.BranchSetup.ParamApproval.get_user_details(gtmpseqid, gmenuname, function (rs) {
                            if (rs.value != '') {
                                prompt_for_email(rs.value, 'Password Mailing');
                            }
                            else {
                                loadAllStatusList();
                            }
                        });
                    }
                    else {
                        loadAllStatusList();
                    }
                    //=======================================================================
                }
                else if (data == '0') {
                    loadAllStatusList();
                    basicAlertMsg("UPDATESTAT", "Failed to save, please try again.");
                }
                else {
                    loadAllStatusList();
                    basicAlertMsg("UPDATESTAT", data);
                }
            }
            else {
                stopWaitMe();
                basicAlertMsg("UPDATESTAT", "Failed to save, please try again.");
            }
        });
    }
    catch (err) { //err.message
        stopWaitMe();
        spawn_errpage("[UPDATESTAT] " + err.message);
    }

}

//=======================================================================
//RFC-31620 : IAMD Maker-Checker
function prompt_for_email(retval, msg) {
    var ss = retval.split('~');
    setText(document.getElementById('popUserId'), ss[0]);
    setText(document.getElementById('popUserName'), ss[1]);
    setText(document.getElementById('popCifId'), ss[2]);
    document.getElementById('btnClose').disabled = false;
    var emails = null;
    if (ss.length > 3 && ss[3].length > 0) {
        emails = ss[3].split(';');
    }
    if (emails != null && emails.length > 0) {
        document.getElementById('tblnoemail').style.display = 'none';
        document.getElementById('tblemail').style.display = 'block';
        document.getElementById('btnSend').disabled = false;
        //document.getElementById('btnView').disabled = false;
        emailopts = new Array();
        for (var i = 0; i < emails.length; i++) {
            if (emails[i] != '') {
                emailopts[i] = emails[i] + "~" + emails[i];
            }
        }
        set_ddl_options(document.getElementById('popddemail'), emailopts);
        document.getElementById('popddemail').selectedIndex = 1;

        if (emailopts.length == 1) {
            document.getElementById('btnSend').onclick();
        }
    }
    else {
        document.getElementById('popTxtEmail').value = '';
        document.getElementById('tblnoemail').style.display = 'block';
        document.getElementById('tblemail').style.display = 'none';
        document.getElementById('btnSend').disabled = false;
    }

    setText(document.getElementById('pop_heading'), msg); //document.getElementById('pop_heading').innerText = msg;

    var div = document.getElementById('divPopup');
    var td = document.getElementById('td_popup');
    div.style.height = top.document.getElementById('divCenter').offsetHeight + 'px';
    div.style.width = document.documentElement.clientWidth + 'px';
    div.style.display = 'block';
    if (isIE) div.className = 'div_undock';
    else div.setAttribute("class", "div_undock");
    td.style.height = div.offsetHeight + 'px';
}

function pop_email_changed(tobj) {
    document.getElementById('btnSend').disabled = false;
    emailopts = new Array();
    emailopts[0] = tobj.value + "~" + tobj.value;
    set_ddl_options(document.getElementById('popddemail'), emailopts);
    document.getElementById('popddemail').selectedIndex = 1;
    document.getElementById('tblnoemail').style.display = 'none';
    document.getElementById('tblemail').style.display = 'block';
}

function email_sent(rs) {
    if (rs.error != null) {
        alert(rs.error.Message);
        document.getElementById('btnClose').disabled = false;
        return;
    }

    alert("Password sent to: " + rs.value);
    close_popup();

    loadAllStatusList();
}

function btn_view_click(btn) {
    if (document.getElementById('tblviewpwd').style.display != 'block') {
        var div = document.getElementById('divPoppwd');
        var td = document.getElementById('tdPoppwd');
        div.style.height = top.document.getElementById('divCenter').offsetHeight + 'px';
        div.style.width = document.documentElement.clientWidth + 'px';
        div.style.display = 'block';
        if (isIE) div.className = 'div_undock';
        else div.setAttribute("class", "div_undock");
        td.style.height = div.offsetHeight + 'px';
    }
}

function btn_send_click(btn) {
    document.getElementById('tblviewpwd').style.display = 'none';

    var email = document.getElementById('popddemail').value;
    if (email == 'Select' || trim(email).length == 0) {
        document.getElementById('tblnoemail').style.display = 'block';
        document.getElementById('tblemail').style.display = 'none';
        alert("No email address provided.");
        return;
    }
    else {
        document.getElementById('tblemail').style.display = 'block';
        document.getElementById('tblnoemail').style.display = 'none';
    }

    var userid = getText(document.getElementById('popUserId'));
    var email_op = 'New';
    IRSWorld.BranchSetup.ParamApproval.send_email(userid, email, email_op, email_sent);
}

function btn_pwdok_click() {
    var pwd = document.getElementById('pwdDiscloser').value;
    if (pwd == null || pwd.length == 0) {
        alert('Please enter your password!');
        return;
    }

    var userid = getText(document.getElementById('popUserId'));
    var rs = IRSWorld.BranchSetup.ParamApproval.disclose_password(userid, pwd);
    if (rs.error != null) {
        alert(rs.error.Message);
        return;
    }

    pwd_close_popup();

    if (rs.error != null) {
        alert(rs.error.Message);
        close_popup();
        return;
    }
    document.getElementById('tblnoemail').style.display = 'none';
    document.getElementById('tblemail').style.display = 'none';
    setText(document.getElementById('tdviewpwd'), rs.value);
    document.getElementById('tblviewpwd').style.display = 'block';
    document.getElementById('btnClose').disabled = false;
    document.getElementById('btnSend').disabled = false;
}

function close_popup() {
    document.getElementById('tblnoemail').style.display = 'none';
    document.getElementById('tblemail').style.display = 'none';
    document.getElementById('popTxtEmail').value = '';
    setText(document.getElementById('tdviewpwd'), ''); //document.getElementById('tdviewpwd').innerText = '';
    document.getElementById('tblviewpwd').style.display = 'none';
    document.getElementById('divPopup').style.display = 'none';
}

function pwd_close_popup() {
    document.getElementById('pwdDiscloser').value = '';
    document.getElementById('divPoppwd').style.display = 'none';
}
//=======================================================================

function confirmAction(actionType) {
    $('<div></div>').appendTo('body')
      .html('<div><h6>' + 'Continue with this action?' + '</h6></div>')
      .dialog({
          modal: true,
          title: 'Confirm Action',
          zIndex: 10000,
          autoOpen: true,
          width: 'auto',
          resizable: false,
          closeOnEscape: false,
          buttons: {
              "YES": function () {
                  $(this).dialog("close");
                  updateStatus(actionType);
              },
              "NO": function () {
                  $(this).dialog("close");
              }
          },
          open: function (event, ui) {
              $(".ui-dialog-titlebar-close", $(this).parent()).hide();
          }
      });
    $(".ui-widget-header").css("background", "#70c5c5");
};

function genAuditLog() {
    
    AjaxPro.timeoutPeriod = 300 * 1000;
    AjaxPro.onTimeout = function () {
        showErrorMsg("Internal Server Error", "The server has stopped responding. Please try again later.");
    }

    if (Date.parse($('#txtDateFrom').val()).toString() == 'NaN' || Date.parse($('#txtDateTo').val()).toString() == 'NaN') {
        $('#spnerror').text('Invalid Date. Please check validity date');
        return;
    }

    runWaitMe();
    try {
        IRSWorld.BranchSetup.ParamApproval.GenerateAuditLog($('#txtDateFrom').val(), $('#txtDateTo').val(), function (result) {
            stopWaitMe();
            var data = result.value;
            if (data.indexOf(".pdf") > 0) {
                window.location = "ParamApprovalDL.ashx?pdffn=" + data;
            }
            else {
                $('#spnerror').text(data);
            }
        });
    }
    catch (err) { //err.message
        stopWaitMe();
        spawn_errpage("[genAuditLog] " + err.message);
    }
}