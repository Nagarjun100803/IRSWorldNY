function get_registration_data() {
    ajax_handler();
    var b = IRSWorld.Admin.RegisterDevice.RegistrationData().value;
    document.getElementById('imgQR').src = b[3];;
    document.getElementById('lblPassword').innerHTML = b[1];
    validate_token_listener();

}

function validate_token_listener() {
    IRSWorld.Admin.RegisterDevice.validate_token_listener(validate_token_listener_callback)
}
function validate_token_listener_callback(result) {
    if (result.value == 'true' || result.value == true) {
        complete_registration();
    }
}

function ajax_handler() {
    AjaxPro.timeoutPeriod = 1000 * 1000;
    AjaxPro.onTimeout = function () {
        alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");

    }
}

