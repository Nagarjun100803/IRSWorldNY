
var msgnum = 0;
var dtmsg;

function show_welcome() {
    var welcome_text = getText(document.getElementById('spanWelcome'));
    var last_access_user_datetime = IRSWorld.Main.frmCenter.user_last_access_datetime().value;

    if (top.isAuthenticated()) {
        if (top.mfa_enabled()) {
            if (top.validate_enrolled_user()) {
                IRSWorld.Main.frmCenter.getMessages(hasMessages);
                show_frame_dialog('../Admin/RegisterDevice.aspx?js_closer=login_close', 'login_close', '100%', '100%', 0);
            }
            else if (!top.token_response() == "CHALLENGE") {
                IRSWorld.Main.frmCenter.getMessages(hasMessages);
                show_frame_dialog('../Main/2FAPage.aspx?js_closer=login_close', 'login_close', '100%', '100%', 0);
            }
            else if (!top.isTokenAuthenticated()) {
                IRSWorld.Main.frmCenter.getMessages(hasMessages);
                show_frame_dialog('../Main/2FAPage.aspx?js_closer=login_close', 'login_close', '100%', '100%', 0);
            }
        }
        else {
            setText(document.getElementById('spanWelcome'), welcome_text + '\n' + last_access_user_datetime);
            IRSWorld.Main.frmCenter.getMessages(hasMessages);
        }
    }
    document.getElementById('divCenter').style.display = 'block';
}

function show_prev_msg() {
    if (dtmsg != null & msgnum - 1 > 0) {
        msgnum--;
        document.getElementById('divMsgView').innerHTML = dtmsg.Rows[msgnum - 1].MsgText;
        setText(document.getElementById('spanMsgNum'), '' + msgnum);
    }
}

function show_next_msg() {
    if (dtmsg != null & msgnum + 1 <= dtmsg.Rows.length) {
        msgnum++;
        document.getElementById('divMsgView').innerHTML = dtmsg.Rows[msgnum - 1].MsgText;
        setText(document.getElementById('spanMsgNum'), '' + msgnum);
    }
}

function hasMessages(rs) {
    if (rs.error != null) {
        alert(rs.error.Message);
        return;
    }

    var disp = (rs.value.Rows.length > 0) ? 'block' : 'none';

    document.getElementById('hrsep').style.display = disp;
    document.getElementById('divCountMsg').style.display = disp;
    document.getElementById('divMsgView').style.display = disp;

    if (disp) {
        setText(document.getElementById('spanTotalMsg'), '' + rs.value.Rows.length);
        dtmsg = rs.value;
        show_next_msg();
    }
}

function list_msgs() {
    show_frame_dialog('../BranchSetup/Messages.aspx?function=listview', '', '100%', '100%', '0');
}

function fix_height() {
    var center = top.document.getElementById('divCenter');
    var div = document.getElementById('divCenter');

    if (center) {
        if (div) {
            div.style.width = center.offsetWidth + 'px';
            div.style.height = center.offsetHeight + 'px';
        }
    }
}

function page_setup() {
    if (top == self) {
        window.open("./IRSMain.aspx", '_top');
    }
    else {
        fix_height();
        show_welcome();
    }
}

function close_session() {
    if (is_closing()) {
        IRSWorld.Main.frmCenter.close_session();
    }
    else {
        IRSWorld.Main.frmCenter.drop_center();
    }
}

function login_close() {

    spawn_main();
}
function spawn_main() {
    var murl = '../Main/IRSMain.aspx';
    var lurl = window.location.search;
    if (lurl != null && lurl.indexOf('Login.aspx') == -1) {
        murl += lurl;
    }
    top.window.name = "PNBIRSWorld";
    top.window.open(murl, '_self', 'status=no,toolbar=no');
}