using System;
using System.Threading;
using System.Web.SessionState;
using Bharosa.VCrypt.Common;
using Bharosa.VCrypt.Common.Util;
using Bharosa.VCrypt.Common.Auth;

/// <summary>
/// Summary description for AppSessionData
/// </summary>
/// 

namespace IRSWorld.bharosa
{

    public class AppSessionData : BharosaSessionData
    {
        public static AppSessionData GetInstance(HttpSessionState Session)
        {
            AppSessionData sessionData = (AppSessionData)Session["AppSessionData"];

            if (sessionData == null)
            {
                BharosaTrace.Debug("Creating new AppSessionData for session " + Session.SessionID);

                sessionData = new AppSessionData(Session);
            }

            return sessionData;
        }

        public override void Reset(HttpSessionState Session)
        {
            base.Reset(Session);

            Session.Remove("AppSessionData");
        }

        private AppSessionData(HttpSessionState Session)
            : base(Session)
        {
            Session["AppSessionData"] = this;
        }

        public PreAuthResultInfo PreAuthRes
        {
            get { return (PreAuthResultInfo)base["preAuthResult"]; }
            set { base["preAuthResult"] = value; }
        }

        public AuthentiPadResultInfo AuthentiPadRes
        {
            get { return (AuthentiPadResultInfo)base["authentiPadResult"]; }
            set { base["authentiPadResult"] = value; }
        }

        public PostAuthResultInfo PostAuthRes
        {
            get { return (PostAuthResultInfo)base["postAuthResult"]; }
            set { base["postAuthResult"] = value; }
        }

        public TransactionResultInfo PreTransactionRes
        {
            get { return (TransactionResultInfo)base["preTransactionRes"]; }
            set { base["preTransactionRes"] = value; }
        }

        public TransactionResultInfo PostTransactionRes
        {
            get { return (TransactionResultInfo)base["postTransactionRes"]; }
            set { base["postTransactionRes"] = value; }
        }

        public VCryptAuthUser CurrentUser
        {
            get { return (VCryptAuthUser)base["vcryptAuthUser"]; }
            set { base["vcryptAuthUser"] = value; }
        }

        public AuthDeviceType UserAuthDevice
        {
            get { object dev = base["userAuthDevice"]; return dev == null ? AuthDeviceType.None : (AuthDeviceType)dev; }
            set { base["userAuthDevice"] = value; }
        }

        public UserStatus InitialUserStatus
        {
            get { object status = base["userStatus"]; return status == null ? UserStatus.Invalid : (UserStatus)status; }
            set { base["userStatus"] = value; }
        }

        public int ClientType
        {
            get { object type = base["clientType"]; return type == null ? ClientTypeEnum.Normal : (int)type; }
            set { base["clientType"] = value; }
        }

        public bool IsUserLoggedIn
        {
            get { object ret = base["isUserLoggedIn"]; return ret == null ? false : (bool)ret; }
            set { base["isUserLoggedIn"] = value; }
        }
    }
}

