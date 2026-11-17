function show_gfc_loading_panel(divname) {
    var div = document.getElementById(divname);

    divmask = new maskpanel();
    divmask.show();
    document.getElementById(divname).style.display = "block";

    div.align = "center";
    div.style.height = "auto";
    div.style.width = parseInt(parent.screen.availWidth) - 600;
    div.style.top = 40;
    div.style.left = (parseInt(screen.width) - parseInt(div.style.width)) / 2 - 310;
}

function hide_gfc_loading_panel(divname) {
    document.getElementById(divname).style.display = "none";
    try {
        divmask.hide();
    }
    catch (e) { }
}


function GFMC_Maintenance() {

    var icls = this;
    /*
    * pmode type
    *    avail
    *    purchase
    *    link  
    *    view
    *    replace
    *    replaceview
    */

    // ==== Global Variables =====

    this.CIFID = null;
    this.REMCIFID = null;
    this.pMode = "avail";
    this._Details = null;//new Array(); 
    this.CASASeqID = null;
    this.ActExistCard = false;

    this.isOTC = false;
    this.isOTCReview = false;

    this.CardNumber = '';
    this.CKAcct = '';
    this.IDPres = '';
    this.Mode = '';
    this.FullName = '';
    this.CurrCode = '';
    this.AccountType = '';

    this.isAgent = false;
    this.prevMode = 'avail';
    // ==== Initialize ===========
    this.CardDetails = function () {
        return this._Details;
    }

    this.getDetails = function (ID, SEQID) {
        if (this.isOTCReview == false) {
            var IDs = ID.split('~');
            this.CIFID = IDs[0];
            this.REMCIFID = IDs[1];
        }

        if (this.CIFID != null && this.CIFID != "") {
            this.CASASeqID = SEQID;
            this.populateDDL();
        }
        else {
            document.getElementById('PurchaseAvailment').style.display = 'none';
            document.getElementById('CARDDETAILS').style.display = 'block';
        }
    }

    this.getDetails_callback = function (response) {
        //        try
        //        {
        //        )
        //        catch(e)
        //        {
        //            alert(e.message);
        //        }
        if (response.error != null) {
            alert(response.error.Message.toString().replace('System.Exception ', ''));
        }
        else {
            var ds = response.value;

            if (ds.Tables[0] != null && typeof (ds.Tables[0]) == "object" && ds.Tables[0].Rows.length > 0) {
                setText('lblGFMCRemBeneName', ds.Tables[0].Rows[0].Full_Name.toString());
                setText('lblGFMCCIF', icls.CIFID);

            }
            else {
                setText('lblGFMCRemBeneName', "");
                setText('lblGFMCCIF', "");
            }

            if (icls.pMode == "excard") {
                if (ds.Tables[1] != null && typeof (ds.Tables[1]) == "object" && ds.Tables[1].Rows.length > 0) {
                    if (ds.Tables[1].Rows[0].CIFID.toString() != icls.CIFID) {
                        if (!confirm("Money Card No. " + ds.Tables[1].Rows[0].CardNo.toString() + " was linked to CIF ID : " + ds.Tables[1].Rows[0].CIFID.toString() + " " + ds.Tables[1].Rows[0].CIFName.toString() + ", do you want to proceed?")) {
                            hide_gfc_loading_panel('divPop_gfc_loading_Process');
                            return;
                        }
                        else {
                            icls._Details = new Array();
                            icls._Details[0] = ds.Tables[1].Rows[0].AccountNo.toString();       //CASA_Acct 
                            icls._Details[1] = ds.Tables[1].Rows[0].CardNo.toString();           //GFMC_Card
                            icls._Details[2] = ds.Tables[1].Rows[0].Currency.toString();        //Currency
                            icls._Details[3] = 'excard';

                            icls.popAccountDetails(ds.Tables[1]);

                            icls.pMode = "view";
                        }
                    }
                    else // Given CIF is same with card CIF
                    {
                        icls._Details = new Array();
                        icls._Details[0] = ds.Tables[1].Rows[0].AccountNo.toString();       //CASA_Acct 
                        icls._Details[1] = ds.Tables[1].Rows[0].CardNo.toString();           //GFMC_Card
                        icls._Details[2] = ds.Tables[1].Rows[0].Currency.toString();        //Currency
                        icls._Details[3] = 'excard';

                        icls.popAccountDetails(ds.Tables[1]);

                        icls.pMode = "view";
                    }
                }
                else if (ds.Tables[2] != null && typeof (ds.Tables[2]) == "object" && ds.Tables[2].Rows.length > 0) {
                    if (!confirm("Money Card No. " + ds.Tables[2].Rows[0].rec_CardNo.toString() + " was purchased outside IRSWorld and was linked to" + " " + ds.Tables[2].Rows[0].CIFName.toString() + ", do you want to proceed?")) {
                        hide_gfc_loading_panel('divPop_gfc_loading_Process');
                        return;
                    }
                    else {
                        var accttype = "";
                        var irsCurr = "";

                        if (ds.Tables[3] != null && typeof (ds.Tables[3]) == "object" && ds.Tables[3].Rows.length > 0) {
                            accttype = ds.Tables[3].Rows[0].AcctType.toString();
                            irsCurr = ds.Tables[3].Rows[0].irsCurr.toString();
                        }

                        if (accttype != null && accttype != '') {
                            icls.AccountType = accttype;
                        }
                        else {
                            if (confirm("Failed to inquire account type in FC\nClick OK to set the account type to Savings\nClick Cancel to set the account type to Current")) {
                                icls.AccountType = '1';
                            }
                            else {
                                icls.AccountType = '2';
                            }
                        }

                        // save data to server
                        var res = IRSWorld.TranClass.RemTranGFMCCommons.SaveGFMCAccount(icls.CIFID + "~" + icls.REMCIFID, ds.Tables[2].Rows[0].rec_CardNo.toString(), ds.Tables[2].Rows[0].rec_CkAcctNo.toString(), irsCurr, "", 'excard', icls.AccountType);
                        if (res.error != null) {
                            hide_gfc_loading_panel('divPop_gfc_loading_Process');
                            alert(res.error.Message);
                            return;
                        }

                        icls._Details = new Array();
                        icls._Details[0] = ds.Tables[2].Rows[0].rec_CkAcctNo.toString();       //CASA_Acct 
                        icls._Details[1] = ds.Tables[2].Rows[0].rec_CardNo.toString();           //GFMC_Card
                        icls._Details[2] = irsCurr; // Currency
                        icls._Details[3] = 'excard';

                        icls.popAccountDetails(ds.Tables[2], true);
                        icls.pMode = "view";

                    }
                }
                else   // Invalid
                {
                    hide_gfc_loading_panel('divPop_gfc_loading_Process');
                    alert("Card entered is either not valid or is not yet linked.");
                    return;
                }
            }
            else {
                if (ds.Tables[1] != null && typeof (ds.Tables[1]) == "object" && ds.Tables[1].Rows.length > 0) {
                    icls.popAccountDetails(ds.Tables[1]);

                    icls._Details = new Array();
                    icls._Details[0] = ds.Tables[1].Rows[0].AccountNo.toString();       //CASA_Acct 
                    icls._Details[1] = ds.Tables[1].Rows[0].CardNo.toString();           //GFMC_Card
                    icls._Details[2] = ds.Tables[1].Rows[0].Currency.toString();        //Currency
                    icls._Details[3] = (ds.Tables[1].Rows[0].CardNo == "") ? "avail" : "purchase";
                    document.getElementById('ddlMode').selectedIndex = (ds.Tables[1].Rows[0].CardNo == "") ? 0 : 1;

                    icls.popAccountDetails(ds.Tables[1]);

                    icls.pMode = "view";
                }
                else {
                    if (icls.isOTCReview == true) {
                        icls.forOTCReview();
                    }
                    else {
                        document.getElementById('txtGFMCRemCardRefNo').value = "";
                        document.getElementById('txtGFCCardRefNoEncrypt').value = "";

                        /* FOR CBIP */
                        icls.pMode = "purchase";
                    }
                }
            }

            icls.trxnMode(icls.pMode);
        }

        //        if(icls.isInitializing == true && document.getElementById("txtHiddenUserRole").value == '10')
        //        {   
        //            icls.pMode = 'excard'
        //            icls.trxnMode(icls.pMode);
        //            icls.prevMode = icls.pMode;
        //            icls.isInitializing = false;
        //        }

        hide_gfc_loading_panel('divPop_gfc_loading_Process');
    }

    this.popAccountDetails = function (dt, isEGFC) {
        if (isEGFC != undefined && isEGFC != null && isEGFC == true) {
            document.getElementById('txtGFMCRemCardRefNo').value = dt.Rows[0].rec_CkAcctNo.toString();
            document.getElementById('txtGFCCardRefNoEncrypt').value = dt.Rows[0].rec_CkAcctNo.toString();
            document.getElementById('ddlCurrency').selectedIndex = this.getIndex('ddlCurrency', this._Details[2]);
            document.getElementById('txtStatus').value = 'CLAIMED';

            document.getElementById('txtGFMCRemGlobalCardNo').value = dt.Rows[0].rec_CardNo.toString();
            document.getElementById('txtCardType').value = '';
            document.getElementById('txtID').value = '';
        }
        else {
            document.getElementById('txtGFMCRemCardRefNo').value = dt.Rows[0].AccountNo.toString();
            document.getElementById('txtGFCCardRefNoEncrypt').value = dt.Rows[0].AccountNo.toString();
            document.getElementById('ddlCurrency').selectedIndex = this.getIndex('ddlCurrency', dt.Rows[0].Currency.toString());
            document.getElementById('txtStatus').value = dt.Rows[0].Status.toString();

            document.getElementById('txtGFMCRemGlobalCardNo').value = dt.Rows[0].CardNo.toString();
            document.getElementById('txtCardType').value = dt.Rows[0].CardType.toString();
            document.getElementById('txtID').value = dt.Rows[0].IDPresented.toString();
        }

    }

    // ==== Button Click ======== 

    this.btnGet_Click = function () {
        show_gfc_loading_panel('divPop_gfc_loading_Process');
        this.getCKAccount();
    }

    this.btnClose_Click = function () {
        //this.btnCancel_Click();
        this.close_modal_window();

        if (this.isOTCReview != true) {
            this.throw_response();
        }
    }

    this.forOTCClose = function () {
        var msg = '';

        if (document.getElementById('txtGFMCRemCardRefNo').value == '') { msg = 'CK Account is invalid'; }
        if (this.pMode == 'purchase' && document.getElementById('txtGFMCRemGlobalCardNo').value == '') { msg = msg + ((msg != '') ? ', ' : '') + 'Card Number is invalid'; }
        if (this.pMode == 'purchase' && document.getElementById('txtID').value.toString().trim() == '') { msg = msg + ((msg != '') ? ', ' : '') + 'ID is required'; }

        if (msg != '') {
            alert(msg);
        }
        else {
            AjaxPro.timeoutPeriod = 100 * 1000;

            AjaxPro.onTimeout = function () {
                alert("SESSION TIMEOUT");
                return;
            }

            IRSWorld.TranClass.RemTranGFMCCommons.validateGFC(this.pMode, document.getElementById('txtGFMCRemGlobalCardNo').value, document.getElementById('txtGFMCRemCardRefNo').value, this.forOTCClose_Callback);
        }
    }

    this.forOTCClose_Callback = function (response) {
        if (response.error != null) {
            alert(response.error.Message.toString());
        }
        else {
            var res = response.value.toString().split('|');

            if (res[0] == "00") {
                icls.CardNumber = document.getElementById('txtGFMCRemGlobalCardNo').value;
                icls.CKAcct = document.getElementById('txtGFMCRemCardRefNo').value;
                icls.IDPres = document.getElementById('txtID').value;
                icls.FullName = getText('lblGFMCRemBeneName');
                icls.CurrCode = document.getElementById('ddlCurrency').value;
                icls.Mode = icls.pMode;

                icls._Details = new Array();
                icls._Details[0] = icls.CKAcct;
                icls._Details[1] = icls.CardNumber;
                icls._Details[2] = icls.CurrCode;
                icls._Details[3] = icls.Mode;

                icls.isOTCReview = true;
                icls.close_modal_window();
                icls.throw_response();
            }
            else if (res[0] == "99") {
                alert(res[1].toString());
            }
            else {
                alert("Unknown Error");
            }
        }
    }

    this.forOTCReview = function () {
        document.getElementById('txtGFMCRemGlobalCardNo').value = this.CardNumber;
        document.getElementById('txtGFMCRemCardRefNo').value = this.CKAcct;
        document.getElementById('txtGFCCardRefNoEncrypt').value = this.CKAcct;
        document.getElementById('txtID').value = this.IDPres;
        setText('lblGFMCRemBeneName', this.FullName);
        document.getElementById('ddlCurrency').selectedIndex = this.getIndex('ddlCurrency', this.CurrCode);

        if (this.Mode == 'avail') {
            this.pMode = 'view|' + this.Mode;
        }
        else {
            this.pMode = 'view';
        }
    }

    this.OTCCheckIfHasTransaction = function () {
        return IRSWorld.TranClass.RemTranGFMCCommons.OTCCheckIfHasTransaction(this.CKAcct).value;
    }

    this.OTCCommitTransaction = function () {
        var msg = '';

        if (this.CIFID != null && this.CIFID != '' && this.REMCIFID != null && this.REMCIFID != '') {
            AjaxPro.timeoutPeriod = 1000 * 1000;

            AjaxPro.onTimeout = function () {
                icls._Details = new Array();
                icls._Details[0] = '99';
                icls._Details[1] = 'SESSION TIMEOUT';
                icls.throw_commit_response();
                return;
            }

            IRSWorld.TranClass.RemTranGFMCCommons.SaveGFMCAccount(this.CIFID + "~" + this.REMCIFID, this.CardNumber, this.CKAcct, this.CurrCode, this.IDPres, this.Mode, this.AccountType, this.OTCCommitTransaction_callback);
        }
        else {
            icls._Details = new Array();
            icls._Details[0] = '00';
            icls._Details[1] = 'Success';
            icls.throw_commit_response();
        }

        //        if (this.CKAcct != null && this.CKAcct != '' && this.OTCCheckIfHasTransaction())
        //        {
        //            this._Details = new Array(); 
        //            this._Details[0] = '01';
        //            this._Details[1] = 'Account already has transaction(s)';        
        //            this.throw_commit_response();
        //            return;        
        //        }
        //        else
        //        {
        //            icls._Details = new Array();
        //            icls._Details[0] = '00';
        //            icls._Details[1] = 'Success';
        //            this.throw_commit_response();
        //            return;        
        //        }
        //                  
        //        if (this.CKAcct == '') { msg = 'CK Account is invalid'; }
        //        if (this.Mode == 'purchase' && this.CardNumber == '') { msg = 'Card Number is invalid'; }
        //        
        //        if (msg != '')
        //        {
        //            this._Details = new Array(); 
        //            this._Details[0] = '99';
        //            this._Details[1] = msg;
        //            this.throw_commit_response();
        //        }
        //        else
        //        {
        //            IRSWorld.TranClass.RemTranGFMCCommons.SaveGFMCAccount(this.CIFID + "~" + this.REMCIFID, this.CardNumber, this.CKAcct, this.CurrCode, this.IDPres, this.Mode, this.OTCCommitTransaction_callback);
        //        }
    }

    this.OTCCommitTransaction_callback = function (status) {
        if (status.error != null) {
            icls._Details = new Array();
            icls._Details[0] = '99';
            icls._Details[1] = status.error.Message.toString().replace('System.Exception ', '')
        }
        else {
            icls._Details = new Array();
            icls._Details[0] = '00';
            icls._Details[1] = 'Success';
        }

        icls.throw_commit_response();
    }

    this.btnCloseDetails_Click = function () {
        this.close_modal_window();
    }

    this.btnGetDetails_Click = function () {
        if (document.getElementById('txtGFMCRemGlobalCardNo_').value != null && document.getElementById('txtGFMCRemGlobalCardNo_').value != "") {
            show_gfc_loading_panel('divPop_gfc_loading_Process');

            AjaxPro.timeoutPeriod = 1000 * 1000;

            AjaxPro.onTimeout = function () {
                hide_gfc_loading_panel('divPop_gfc_loading_Process');
                alert('SESSION TIMEOUT');
                return;
            }

            IRSWorld.TranClass.RemTranGFMCCommons.GFMCDetails(icls.CIFID, "EXCARD", document.getElementById('txtGFMCRemGlobalCardNo_').value, icls.getDetails_callback);
        }
        else {
            alert("Please enter a valid GFC Number");
        }
    }

    this.throw_response = function () {
        eval("javascript:gfc_response();");
    }

    this.throw_commit_response = function () {
        eval("javascript:gfc_commit_response();");
    }


    this.btnSave_Click = function () {
        if (this.isOTC == true) {
            this.forOTCClose();
        }
        else {
            var isValidMsg = this.validate();

            if (isValidMsg == "") {
                show_gfc_loading_panel('divPop_gfc_loading_Process');

                AjaxPro.timeoutPeriod = 100 * 1000;

                AjaxPro.onTimeout = function () {
                    hidepanel('divPop_gfc_loading_Process');
                    alert("SESSION TIMEOUT");
                    return;
                }

                IRSWorld.TranClass.RemTranGFMCCommons.validateGFC(this.pMode, document.getElementById('txtGFMCRemGlobalCardNo').value, document.getElementById('txtGFMCRemCardRefNo').value, this.validateGFC_Callback);
            }
            else {
                alert(isValidMsg);
            }
        }
    }

    this.validateGFC_Callback = function (response) {
        if (response.error != null) {
            hide_gfc_loading_panel('divPop_gfc_loading_Process');
            alert(response.error.Message.toString());
        }
        else {
            var res = response.value.toString().split('|');

            if (res[0] == "00") {
                var CIFID = icls.CIFID;                                                                      //@v_CIF_ID
                var CardNo = document.getElementById('txtGFMCRemGlobalCardNo').value;   //@v_GFMC_Card            
                var AcctNo = document.getElementById('txtGFMCRemCardRefNo').value;       //@v_CASA_Acct
                var Curr = document.getElementById('ddlCurrency').value;                      //@v_Currency
                var IDPresented = document.getElementById('txtID').value;                      // ID Presented
                var Mode = icls.pMode;                                                                    //@v_Mode

                AjaxPro.timeoutPeriod = 100 * 1000;

                AjaxPro.onTimeout = function () {
                    hidepanel('divPop_gfc_loading_Process');
                    alert("SESSION TIMEOUT");
                    return;
                }

                IRSWorld.TranClass.RemTranGFMCCommons.SaveGFMCAccount(CIFID + "~" + icls.REMCIFID, CardNo, AcctNo, Curr, IDPresented, Mode, icls.AccountType, icls.btnSave_Click_Callback);
            }
            else if (res[0] == "99") {
                hide_gfc_loading_panel('divPop_gfc_loading_Process');
                alert(res[1].toString());
            }
            else {
                hide_gfc_loading_panel('divPop_gfc_loading_Process');
                alert("Unknown Error");
            }
        }
    }

    this.btnSave_Click_Callback = function (status) {
        if (status.error != null) {
            hide_gfc_loading_panel('divPop_gfc_loading_Process');
            alert(status.error.Message.toString().replace('System.Exception ', ''));
        }
        else {
            var res = status.value;

            icls._Details = new Array();
            icls._Details[0] = document.getElementById('txtGFMCRemCardRefNo').value;       //CASA_Acct 
            icls._Details[1] = document.getElementById('txtGFMCRemGlobalCardNo').value;   //GFMC_Card
            icls._Details[2] = document.getElementById('ddlCurrency').value;                     //Currency
            icls._Details[3] = icls.pMode;                                                                    //Mode

            icls.CASASeqID = res[4];

            hide_gfc_loading_panel('divPop_gfc_loading_Process');

            switch (icls.pMode) {
                case 'avail':
                    document.getElementById('txtStatus').value = "AVAILED";
                    alert('Availment is successful');
                    icls.pMode = 'view|avail';
                    break;
                case 'purchase':
                    var stat = status.value;
                    document.getElementById('txtCardType').value = stat[6];
                    document.getElementById('txtStatus').value = "CLAIMED";
                    document.getElementById('PrintFormBtn').style.display = 'block';
                    alert('Purchase is successful');
                    icls.pMode = 'view';
                    break;
            }

            icls.trxnMode(icls.pMode);

            this.value = icls;
        }

        hide_gfc_loading_panel('divPop_gfc_loading_Process');
    }

    this.btnCancel_Click = function () {
        if (this.isOTC == true) {
            //this.throw_response(); 
            this.close_modal_window();
        }
        else if (this.pMode == "avail") {
            this.unlockCK();

            document.getElementById('txtGFMCRemCardRefNo').value = "";
            document.getElementById('txtGFCCardRefNoEncrypt').value = "";

            document.getElementById('txtID').value = "";

            this.enable('ddlCurrency');
            this.enable('btnGetCK');
        }
        else if (this.pMode == "purchase") {
            this.unlockCK();

            document.getElementById('txtGFMCRemCardRefNo').value = "";
            document.getElementById('txtGFCCardRefNoEncrypt').value = "";

            document.getElementById('txtGFMCRemGlobalCardNo').value = "";
            document.getElementById('txtCardType').value = "";
            document.getElementById('txtID').value = "";

            this.enable('ddlCurrency');
            this.enable('btnGetCK');
        }
        else if (this.pMode == "link") {
            document.getElementById('txtGFMCRemGlobalCardNo').value = "";
            document.getElementById('txtCardType').value = "";
        }
    }

    this.ddlMode_OnChange = function () {
        this.trxnMode(document.getElementById('ddlMode').value);
        this.prevMode = document.getElementById('ddlMode').value;
        //this.btnCancel_Click();
    }

    // ==== Methods =========== 

    this.getCKAccount = function () {
        AjaxPro.timeoutPeriod = 100 * 1000;

        AjaxPro.onTimeout = function () {
            hidepanel('divPop_gfc_loading_Process');
            alert("SESSION TIMEOUT");
            return;
        }

        /* FOR CBIP */
        if (document.getElementById("txtGFMCRemGlobalCardNo").value == "") {
            alert("GFC Card is required for purchase. Kindly input card number and try again.");
            hide_gfc_loading_panel('divPop_gfc_loading_Process');
            return;
        }
        if (document.getElementById("txtID").value == "") {
            alert("ID is required for purchase. Kindly input id presented and try again.");
            hide_gfc_loading_panel('divPop_gfc_loading_Process');
            return;
        }

        IRSWorld.TranClass.RemTranGFMCCommons.GetCKAccount(
            icls.REMCIFID,
            document.getElementById("txtGFMCRemGlobalCardNo").value,
            document.getElementById("txtID").value,
            document.getElementById('ddlCurrency').value, this.getCKAccount_callback);
    }

    this.getCKAccount_callback = function (response) {
        if (response.error != null) {
            alert(response.error.Message.toString().replace('System.Exception ', ''));
        }
        else {
            var ck = response.value.toString().split('~')[0];
            var accttype = response.value.toString().split('~')[1];

            document.getElementById('txtGFMCRemCardRefNo').value = ck;
            document.getElementById('txtGFCCardRefNoEncrypt').value = ck;
            icls.disable('ddlCurrency');
            icls.disable('btnGetCK');

            if (accttype != null && accttype != '') {
                icls.AccountType = accttype;
            }
            else {
                if (confirm("Failed to inquire account type in FC\nClick OK to set the account type to Savings\nClick Cancel to set the account type to Current")) {
                    icls.AccountType = '1';
                }
                else {
                    icls.AccountType = '2';
                }
            }
        }

        hide_gfc_loading_panel('divPop_gfc_loading_Process');
    }

    this.unlockCK = function () {
        if (trim(document.getElementById('txtGFMCRemCardRefNo').value) != "") {
            IRSWorld.TranClass.RemTranGFMCCommons.UnlockCK(document.getElementById('txtGFMCRemCardRefNo').value, this.unlockCK_callback);
        }
    }

    this.unlockCK_callback = function (response) {
        if (response.error != null) {
            alert(response.error.Message.toString().replace('System.Exception ', ''));
        }
        else {
            if (response.value != true) {
                alert('Failed to unlock ck number ' + document.getElementById('txtGFMCRemCardRefNo').value + ' ' + msg);
            }
        }
    }

    this.trxnMode = function (mode) {
        this.pMode = mode;
        this.readOnly("txtGFMCRemCardRefNo", true);
        this.readOnly("txtGFCCardRefNoEncrypt", true);
        this.readOnly("txtCardType", true);
        this.readOnly("txtStatus", true);

        document.getElementById('trStatus').style.display = 'none';
        document.getElementById('trIDGiven').style.display = 'none';

        if (this.isAgent == true) {
            document.getElementById('trCkAcct').style.display = 'none';
            document.getElementById('trCkAcctEncrypted').style.display = 'block';
        }
        else {
            document.getElementById('trCkAcct').style.display = 'block';
            document.getElementById('trCkAcctEncrypted').style.display = 'none';
        }

        if (mode == "purchase") {
            //this.commons.setText(document.getElementById('btnSave')) = "SAVE";
            setText("btnSave", "SAVE");

            /* FOR CBIP */
            document.getElementById('ddlMode').selectedIndex = 0;
            this.readOnly("txtGFMCRemGlobalCardNo", false);
            this.readOnly("txtID", false);

            this.enable("btnGetCK");
            this.enable("btnSave");
            this.enable("btnCancel");

            this.enable('ddlMode');
            this.enable('ddlCurrency');

            document.getElementById('div_button1').style.display = 'block';
            document.getElementById('trGFMCRemGlobalCardNo').style.display = 'block';
            document.getElementById('trIDGiven').style.display = 'block';
            document.getElementById('trCardType').style.display = 'none';

            document.getElementById('PurchaseAvailment').style.display = 'block';
            document.getElementById('PurchaseAvailmentBtn').style.display = 'block';
            document.getElementById('CARDDETAILS').style.display = 'none';

            if (this.isOTC == true) {
                setText('btnSave', "OK");
                document.getElementById('btnClose').style.display = 'none';
            }
        }
        /* FOR CBIP */
        /*
        else if (mode == "avail") {
            setText('btnSave', "SAVE");

            document.getElementById('ddlMode').selectedIndex = 0;

            this.readOnly("txtGFMCRemGlobalCardNo", true);
            this.readOnly("txtID", true);

            this.enable("btnGetCK");
            this.enable("btnSave");
            this.enable("btnCancel");

            this.enable('ddlMode');
            this.enable('ddlCurrency');

            document.getElementById('div_button1').style.display = 'block';
            document.getElementById('trGFMCRemGlobalCardNo').style.display = 'none';
            document.getElementById('trCardType').style.display = 'none';
            document.getElementById('trIDGiven').style.display = 'none';

            document.getElementById('PurchaseAvailment').style.display = 'block';
            document.getElementById('PurchaseAvailmentBtn').style.display = 'block';
            document.getElementById('CARDDETAILS').style.display = 'none';

            if (this.isOTC == true) {
                setText('btnSave', "OK");
                document.getElementById('btnClose').style.display = 'none';
            }
        }
        */
        else if (mode == "link") {
            setText('btnSave', "LINK");

            this.readOnly("txtGFMCRemGlobalCardNo", false);
            this.readOnly("txtID", false);
            this.disable("btnSave");

            this.disable('ddlMode');

            this.disable("btnGetCK");
            this.enable("btnSave");
            this.enable("btnCancel");

            this.disable('ddlCurrency');

            document.getElementById('div_button1').style.display = 'block';
        }
        else if (mode == "view") {
            this.readOnly("txtGFMCRemGlobalCardNo", true);
            this.readOnly("txtID", true);

            this.disable("btnGetCK");
            this.disable("btnSave");
            this.disable("btnCancel");

            this.disable('ddlCurrency');
            this.disable('ddlMode');

            document.getElementById('trGFMCRemGlobalCardNo').style.display = 'block';
            document.getElementById('trCardType').style.display = 'none';
            document.getElementById('trStatus').style.display = 'none';

            document.getElementById('div_button1').style.display = 'none';

            document.getElementById('PurchaseAvailment').style.display = 'block';
            document.getElementById('PurchaseAvailmentBtn').style.display = 'block';
            document.getElementById('CARDDETAILS').style.display = 'none';

            //document.getElementById('ddlMode').selectedIndex = 1;    
            document.getElementById('ddlMode').value = this.prevMode;
            if (this.prevMode == 'excard') {
                document.getElementById('trIDGiven').style.display = 'none';
            }
            else {
                document.getElementById('trIDGiven').style.display = 'block';
            }
        }
        else if (mode == "view|avail") {
            this.readOnly("txtGFMCRemGlobalCardNo", true);
            this.readOnly("txtID", true);

            this.disable("btnGetCK");
            this.disable("btnSave");
            this.disable("btnCancel");

            this.disable('ddlCurrency');
            this.disable('ddlMode');

            document.getElementById('trGFMCRemGlobalCardNo').style.display = 'none';
            document.getElementById('trCardType').style.display = 'none';
            document.getElementById('trStatus').style.display = 'none';

            document.getElementById('div_button1').style.display = 'none';

            document.getElementById('PurchaseAvailment').style.display = 'block';
            document.getElementById('PurchaseAvailmentBtn').style.display = 'block';
            document.getElementById('CARDDETAILS').style.display = 'none';
        }
        else if (mode == "excard") {
            document.getElementById('PurchaseAvailment').style.display = 'none';
            document.getElementById('PurchaseAvailmentBtn').style.display = 'none';
            document.getElementById('CARDDETAILS').style.display = 'block';
        }

    }

    this.hideall = function () {
        document.getElementById('PurchaseAvailment').style.display = 'none';
        document.getElementById('PurchaseAvailmentBtn').style.display = 'none';
        document.getElementById('CARDDETAILS').style.display = 'none';
        document.getElementById('PrintFormBtn').style.display = 'none';
    }

    this.validate = function () {
        var CardNumber = document.getElementById('txtGFMCRemGlobalCardNo').value;
        var AcctNumber = document.getElementById('txtGFMCRemCardRefNo').value;
        var IDPres = document.getElementById('txtID').value;

        if (this.pMode == "avail") {
            if (trim(AcctNumber) == "") {
                return "CASA Account number should not be blank.";
            }
        }
        else if (this.pMode == "purchase") {
            if (trim(AcctNumber) == "") {
                return "CASA Account number should not be blank.";
            }
            if (IRSWorld.Internet.InternetRemitter.getLocation().value == '761') {
                if (document.getElementById('txtID').value == "") {
                    return "ID Presented should not be blank.";
                }
            }
            if (trim(CardNumber) == "") {
                return "GFC number should not be blank.";
            }
        }
        else {
            return "Invalid page Mode";
        }

        return "";
    }

    this.populateDDL = function () {
        IRSWorld.TranClass.RemTranGFMCCommons.populateDDL(this.populateDDL_callback);
    }

    this.populateDDL_callback = function (response) {
        if (response.error != null) {
            alert(response.error.Message.toString().replace('System.Exception ', ''));
        }
        else {
            var DDL = document.getElementById('ddlCurrency');
            var DDL3 = document.getElementById('ddlMode');

            DDL.length = 0;
            DDL3.length = 0;

            var ds = response.value;

            if (ds != null) {
                // For Currency 
                if (ds.Tables[0].Rows.length > 0) {
                    var dt = ds.Tables[0];
                    for (var i = 0; i < dt.Rows.length; i++) {
                        var opt = document.createElement("option");
                        opt.text = dt.Rows[i].Text.toString();
                        opt.value = dt.Rows[i].Value.toString();
                        DDL.options.add(opt);
                    }
                }
            }

            //            if (document.getElementById("txtHiddenUserRole").value != undefined && document.getElementById("txtHiddenUserRole").value != '10')
            //            {
            //                var opt3 = document.createElement("option");
            //                opt3.text = 'Availment';
            //                opt3.value = 'avail';
            //                DDL3.options.add(opt3);
            //                
            //                var opt4 = document.createElement("option");
            //                opt4.text = 'Purchase';
            //                opt4.value = 'purchase';
            //                DDL3.options.add(opt4);    
            //            }    

            /* FOR CBIP */
            /*
            var opt3 = document.createElement("option");
            opt3.text = 'Availment';
            opt3.value = 'avail';
            DDL3.options.add(opt3);
            */

            var opt4 = document.createElement("option");
            opt4.text = 'Purchase';
            opt4.value = 'purchase';
            DDL3.options.add(opt4);

            if (icls.ActExistCard == true) {
                var opt5 = document.createElement("option");
                opt5.text = 'Existing Card';
                opt5.value = 'excard';
                DDL3.options.add(opt5);
            }

            if (icls.CIFID != "") {
                var param = new Array();

                param[0] = icls.CIFID;
                param[1] = (icls.CASASeqID == "") ? " " : icls.CASASeqID;

                var params = param.join('~');

                IRSWorld.TranClass.RemTranGFMCCommons.GFMCDetails(icls.CIFID, icls.CASASeqID, " ", icls.getDetails_callback);
            }
        }
    }

    this.checkIfBeneIsCorporate = function (ID) {
        if (ID != null && ID != '' && ID != undefined) {
            var param = ID.split('~');
            return IRSWorld.TranClass.RemTranGFMCCommons.checkIfBeneIsCorporate(param[0]).value;
        }
        else {
            return false;
        }
    }

    this.btnReleaseForm_Click = function () {
        var UserRole;
        UserRole = '';
        try {
            UserRole = document.getElementById('txtHiddenUserRole').value;
        }
        catch (e) {
            UserRole = '';
        }

        IRSWorld.TranClass.RemTranGFMCCommons.set_report(document.getElementById('txtGFMCRemGlobalCardNo').value + '~' + UserRole, 'Release', this.show_report_callback);
    }

    this.btnTermsAndCondition_Click = function () {
        IRSWorld.TranClass.RemTranGFMCCommons.set_report(document.getElementById('txtGFMCRemGlobalCardNo').value, 'Terms', this.show_report_callback);
    }

    this.show_report_callback = function (response) {
        if (response.error != null) { alert('Failed to load report'); }
        else
        {
            window.open('../Reports/Pages/Report_Viewer.aspx' + response.value.toString(), 1);
        }
    }

    // ====== Common Functions =============================================

    this.enable = function (oName) {
        var id = document.getElementById(oName);
        var cset = id.disabled;

        id.disabled = cset == "false";
    }

    this.disable = function (oName) {

        var id = document.getElementById(oName);
        var cset = id.disabled;

        id.disabled = (cset == "false") ? "true" : "false";
    }

    this.readOnly = function (oName, isReadOnly) {
        if (isReadOnly == true) {
            document.getElementById(oName).readOnly = true;
        }
        else {
            document.getElementById(oName).readOnly = false;
        }
    }

    this.getIndex = function (DDL, value) {
        DDL = document.getElementById(DDL);

        for (var i = 0; i < DDL.options.length; i++) {
            if (DDL.options[i].value == value) {
                return i;
            }
            alert('currency is not in the list.');
            return null;
        }
    }

    // ====== Page Generation =============================================

    this.open_gfc_form = function (ID, SEQID, Ref, ActExistCard) {
        if (this.checkIfBeneIsCorporate(ID) == true) {
            alert('GFC product is not available for Corporate Beneficiary');
            return;
        }

        this.ActExistCard = ActExistCard;
        this._Details = new Array();
        this.open_gfc_enroll_modal_window();
        this.construct_override_gfc_enroll_window(ID, SEQID, Ref);

        /* FOR CBIP */

        try {
            if (IRSWorld.TranClass.RemTranGFMCCommons.check_userrole_is_agent(document.getElementById('txtHiddenUserRole').value).value == true || document.getElementById('txtHiddenUserRole').value == '10') {
                this.isAgent = true;
            }
        }
        catch (e) {
            this.isAgent = false;
        }
    }

    this.open_gfc_enroll_modal_window = function () {
        this.load_modal_css();
        this.outer_modal_div = document.createElement("div");
        this.outer_modal_div.id = "outer_gfc_enroll_mod_div";
        this.outer_modal_div.className = "outer_div";

        document.body.style.margin = "0px";
        document.body.appendChild(this.outer_modal_div);
        document.getElementById("outer_gfc_enroll_mod_div").style.height = screen.height;

        this.construct_div_content();
        this.commons = new common_method();
        this.commons.to_show_hide_combobox(false);
        this.commons = null;
    }

    this.construct_div_content = function () {
        var close_div = null;
        var content_div = null;

        var tdc = top.document.getElementById('divCenter');
        var aw = (tdc) ? tdc.offsetWidth : parent.screen.availWidth;

        this.inner_modal_div = document.createElement("div");
        this.inner_modal_div.align = "center";
        this.inner_modal_div.className = "inner_div";
        this.inner_modal_div.id = "inner_gfc_enroll_mod_div";
        this.inner_modal_div.style.height = "auto";
        this.inner_modal_div.style.width = (aw - 600) + "px";
        this.inner_modal_div.style.top = "150px";//10;
        this.inner_modal_div.style.left = ((aw - parseInt(this.inner_modal_div.style.width)) / 2 - 10) + "px";
        this.inner_modal_div.style.border = "medium solid white";
        this.inner_modal_div.Code = this;

        close_div = document.createElement("div");
        close_div.id = "close_gfc_enroll_mod_div";
        close_div.align = "right";
        close_div.style.background = "transparent";
        close_div.width = "100%";
        close_div.Code = this;

        content_div = document.createElement("div");
        content_div.id = "content_gfc_enroll_mod_div";
        content_div.align = "left";
        content_div.style.background = "transparent";
        content_div.style.width = "95%";
        content_div.innerHTML = "<center><img src = '../images/ajaxloader.gif'></img></center>";

        this.inner_modal_div.appendChild(close_div);
        this.inner_modal_div.appendChild(content_div);

        document.body.appendChild(this.inner_modal_div);
    }

    this.construct_cancel_button = function () {
        var closebtn = document.createElement("A");

        closebtn.setAttribute("id", "btn_close");
        closebtn.innerHTML = "CANCEL";
        closebtn.style.cursor = "hand";
        closebtn.style.cursor = "pointer";
        closebtn.style.fontWeight = "700";
        closebtn.style.fontFamily = "Arial";
        closebtn.style.fontSize = "10pt";
        closebtn.style.color = "black";
        closebtn.onclick = function () { this.parentNode.Code.close_modal_window(); }
        closebtn.onmouseover = function () { closebtn.style.color = "blue"; }
        closebtn.onmouseout = function () { closebtn.style.color = "black"; }

        return closebtn;
    }

    this.close_modal_window = function () {
        this.commons = new common_method();
        this.commons.to_show_hide_combobox(true);
        this.commons = null;

        if (this.outer_modal_div != null) { document.body.removeChild(this.outer_modal_div); this.outer_modal_div = null; }
        if (this.inner_modal_div != null) { document.body.removeChild(this.inner_modal_div); this.inner_modal_div = null; }
        if (this.content_iframe != null) { this.content_iframe = null; }

        document.body.style.margin = "";
    }

    this.load_modal_css = function () {
        var file_reference = document.createElement("link");

        file_reference.setAttribute("rel", "stylesheet");
        file_reference.setAttribute("type", "text/css");
        file_reference.setAttribute("href", "../css/cssmodals.css");

        if (typeof file_reference != "undefined") {
            document.getElementsByTagName("head")[0].appendChild(file_reference);
        }
    }

    this.construct_override_gfc_enroll_window = function (ID, SEQID, Ref) {
        if (SEQID != null) {

            var html_string = "";
            html_string += "<center>";

            html_string += "<div id='divPop_gfc_loading_Process' style='width:250px; height:100px; background:White; z-index: 101; position: absolute; left: 150px;color: darkblue; padding: 10px; display: none; font-family: Arial;top: 241px;'>";
            html_string += " <table style='width:100%; height:100%;'>";
            html_string += "    <tr>";
            html_string += "        <td>";
            html_string += "            <label id='lblLoadingMsg' style='text-align:center; color:Chocolate '>Processing request, Please wait... </label>";
            html_string += "            <div id='divLoadingMsg'>";
            html_string += "                <img src='../images/ajaxloader.gif' id='imgAjaxIcon' />";
            html_string += "            </div>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += " </table>";
            html_string += "</div>";

            html_string += "<table width='100%'  cellspacing='0' cellpadding='0' style='background-color:Azure'>";
            html_string += "    <tr>";
            html_string += "        <td colspan='3' style='width: 100%; height: 100%'>";
            html_string += "            <table style='width:100%; height: 100%;'";
            html_string += "                <tr>";
            html_string += "                    <td style='width: 100%; height: 8px;'>";
            html_string += "                    </td>";
            html_string += "                </tr>";
            html_string += "                <tr>";
            html_string += "                    <td style='width: 100%; height: 100%; vertical-align: top'>";
            html_string += "                            <table width='100%' cellpadding='0' cellspacing='1'>";
            html_string += "                                <tr>";
            html_string += "                                    <td style='width: 100%; height: 100%;'valign='center'>";
            html_string += "                                         <table id='tblGFMCRemBeneficiary' width='100%' cellpadding='0' cellspacing='3'>";
            html_string += "                                             <tr>";
            html_string += "                                                 <td class='cell_caption' style='width: 20%; height:23px'>Name :&nbsp; </td>";
            html_string += "                                                 <td colspan=3 align='left' style='width:80%'><label id= 'lblGFMCRemBeneName' class='cell_desc' style='font-size: medium; '>";
            html_string += "                                                     <span style='font-size: 13pt'></span></label></td>";
            html_string += "                                             </tr>";
            html_string += "                                             <tr>";
            html_string += "                                                 <td class='cell_caption' style='width: 20%; height:23px'>CIFID :&nbsp; </td>";
            html_string += "                                                 <td colspan=3 align='left' style='width:80%'><label id= 'lblGFMCCIF' class='cell_desc' style='font-size: medium; '>";
            html_string += "                                                     <span style='font-size: 13pt'></span></label></td>";
            html_string += "                                             </tr>";
            html_string += "                                            </table>";

            html_string += "                                            <table width='100%' cellpadding='0' cellspacing='3' >";
            html_string += "                                                 <tr>";
            html_string += "                                                     <td class='cell_caption' style='width: 20%; height:24px'>";
            html_string += "                                                         Mode* :&nbsp;</td>";
            html_string += "                                                     <td align='left' style='width:80%; height: 24px;'><label id= 'lblMode' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                                                         <select id='ddlMode' style='width: 98%' onchange='javascript:" + Ref + ".ddlMode_OnChange();'>";
            html_string += "                                                             <option selected='selected'></option>";
            html_string += "                                                         </select></label></td>";
            html_string += "                                                 </tr>";
            html_string += "                                             </table>";
            /*
                                 PurchaseAvailment
            */
            html_string += "                                            <DIV ID='PurchaseAvailment' style='background-color:Azure'>";
            html_string += "                                            <table id='tblGFMCRemBeneficiaryAvail' width='100%' cellpadding='0' cellspacing='3' >";
            html_string += "                                                 <tr>";
            html_string += "                                                     <td class='cell_caption' style='width: 20%; height:24px'>";
            html_string += "                                                         A/C Currency* :&nbsp;</td>";
            html_string += "                                                     <td align='left' style='width:80%; height: 24px;'><label id= 'lblCurrency' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                                                         <select id='ddlCurrency' style='width: 98%'>";
            html_string += "                                                             <option selected='selected'></option>";
            html_string += "                                                         </select></label></td>";
            html_string += "                                                 </tr>";
            html_string += "                                                 <tr id='trCkAcct'>";
            html_string += "                                                    <td class='cell_caption' style='width: 20%; height:24px'>CK Reference No* :&nbsp; </td>";
            html_string += "                                                    <td align='left' style='width:80%; height: 24px;'><input id='txtGFMCRemCardRefNo' type='text' style='width:98%; font-weight:normal';/></td>";
            html_string += "                                                 </tr>";

            html_string += "                                                 <tr id='trCkAcctEncrypted'>";
            html_string += "                                                    <td class='cell_caption' style='width: 20%; height:24px'>CK Reference No* :&nbsp; </td>";
            html_string += "                                                    <td align='left' style='width:80%; height: 24px;'><input id='txtGFCCardRefNoEncrypt' type='password' style='width:98%; font-weight:normal';/></td>";
            html_string += "                                                 </tr>";

            html_string += "                                                 <tr id='trCardType'>";
            html_string += "                                                     <td class='cell_caption' style='width: 20%; height:24px'>Card Type :&nbsp; </td>";
            html_string += "                                                     <td align='left' style='width:80%; height: 24px;'><input id='txtCardType' type='text' style='width:98%; font-weight:normal';/></td>";
            html_string += "                                                 </tr>";
            html_string += "                                                 <tr id='trGFMCRemGlobalCardNo'>";
            html_string += "                                                    <td class='cell_caption' style='width: 20%; height:24px'>Global Filipino Card No* :&nbsp; </td>";
            html_string += "                                                    <td align='left' style='width:80%; height: 24px;'><input id='txtGFMCRemGlobalCardNo' type='text' style='width:98%; font-weight:normal';/></td>";
            html_string += "                                                 </tr>";
            html_string += "                                                 <tr id='trIDGiven'>";
            html_string += "                                                    <td class='cell_caption' style='width: 20%; height:24px'>ID Presented* :&nbsp; </td>";
            html_string += "                                                    <td align='left' style='width:80%; height: 24px;'><input id='txtID' type='text' style='width:98%; font-weight:normal';/></td>";
            html_string += "                                                 </tr>";
            html_string += "                                                 <tr id='trStatus'>";
            html_string += "                                                    <td class='cell_caption' style='width: 20%; height:24px'>Status :&nbsp; </td>";
            html_string += "                                                    <td align='left' style='width:80%; height: 24px;'><input id='txtStatus' type='text' style='width:98%; font-weight:normal';/></td>";
            html_string += "                                                 </tr>";
            html_string += "                                             </table>";
            html_string += "                                         </DIV>";
            /*
                                 CARDDETAILS                         
            */
            html_string += "                                         <DIV ID='CARDDETAILS' style='background-color:Azure'>";
            html_string += "                                         <table width='100%' cellpadding='0' cellspacing='3'>";
            html_string += "                                                 <tr id='trGFMCRemGlobalCardNo'>";
            html_string += "                                                    <td class='cell_caption' style='width: 20%; height:24px'>Global Filipino Card No* :&nbsp; </td>";
            html_string += "                                                    <td align='left' style='width:80%; height: 24px;'><input id='txtGFMCRemGlobalCardNo_' type='text' style='width:98%; font-weight:normal';/></td>";
            html_string += "                                                 </tr>";
            html_string += "                                         </table>";
            html_string += "                                         <table id='tblButtons' width='100%' cellpadding='0' cellspacing='5'>";
            html_string += "                                             <tr>";
            html_string += "                                                 <td width='80%' align='right'>";
            html_string += "                                                   <div>";
            html_string += "                                                       <input style=' width:30%;' id='btnGetDetails' type='button' value='GET CK' onclick = 'javascript:" + Ref + ".btnGetDetails_Click();' />&nbsp&nbsp";
            html_string += "                                                   </div>";
            html_string += "                                                 </td>";
            html_string += "                                                 <td width='20%'>";
            html_string += "                                                    <input style=' width:100%;' id='btnCloseDetails' type='button' value='CLOSE' onclick='javascript:" + Ref + ".btnCloseDetails_Click();'/>";
            html_string += "                                                 </td>";
            html_string += "                                             </tr>";
            html_string += "                                        </table>";
            html_string += "                                        </DIV>";
            /*
                                  Print Forms Button
            */
            html_string += "                                         <DIV ID='PrintFormBtn' style='background-color:Azure'>";
            html_string += "                                         <table width='100%' cellpadding='0' cellspacing='5'>";
            html_string += "                                             <tr>";
            html_string += "                                                 <td width='80%' align='right'>";
            html_string += "                                                       <input style=' width:35%;' id='btnReleaseForm' type='button' value='Release Form' onclick = 'javascript:" + Ref + ".btnReleaseForm_Click();' />&nbsp&nbsp";
            html_string += "                                                       <input style=' width:35%;' id='btnTermsAndCondition' type='button' value='Terms And Condition' onclick='javascript:" + Ref + ".btnTermsAndCondition_Click();' />";
            html_string += "                                                 </td>";
            html_string += "                                             </tr>";
            html_string += "                                        </table>";
            html_string += "                                        </DIV>";
            /*
                                 PurchaseAvailmentBtn
            */
            html_string += "                                         <DIV ID='PurchaseAvailmentBtn' style='background-color:Azure'>";
            html_string += "                                         <table id='tblButtons' width='100%' cellpadding='0' cellspacing='5'>";
            html_string += "                                             <tr>";
            html_string += "                                                 <td width='80%' align='right'>";
            html_string += "                                                   <div id='div_button1' style='display:none'>";
            html_string += "                                                       <input style=' width:20%;' id='btnGetCK' type='button' value='GET' onclick = 'javascript:" + Ref + ".btnGet_Click();' />&nbsp&nbsp";
            html_string += "                                                       <input style=' width:20%;' id='btnCancel' type='button' value='CANCEL' onclick='javascript:" + Ref + ".btnCancel_Click();' />&nbsp&nbsp";
            html_string += "                                                       <input style=' width:20%;' id='btnSave' type='button' value='SAVE' onclick='javascript:" + Ref + ".btnSave_Click();'/>&nbsp&nbsp";
            html_string += "                                                   </div>";
            html_string += "                                                 </td>";
            html_string += "                                                 <td width='20%'>";
            html_string += "                                                    <input style=' width:100%;' id='btnClose' type='button' value='CLOSE' onclick='javascript:" + Ref + ".btnClose_Click();'/>";
            html_string += "                                                 </td>";
            html_string += "                                             </tr>";
            html_string += "                                        </table>";
            html_string += "                                        </DIV>";

            html_string += "                                    </td>";
            html_string += "                                 </tr>";
            html_string += "                             </table>";
            html_string += "                    </td>";
            html_string += "                </tr>";
            html_string += "            </table>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "</table>";
            html_string += "</center>";

            document.getElementById("content_gfc_enroll_mod_div").innerHTML = html_string;
            this.hideall();

            this.getDetails(ID, SEQID);
        }
    }
}

