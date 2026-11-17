using System;
using System.Collections.Specialized;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using Bharosa.VCrypt.Common;
using Bharosa.VCrypt.Common.Auth;
using Bharosa.VCrypt.Common.Tracker;
using Bharosa.VCrypt.Common.Util;
using Bharosa.VCrypt.Client;
using System.Collections;

namespace IRSClassLib
{

    public enum AuthDeviceType { None, DeviceKeyPad, DeviceKeyPadGeneric, DeviceKeyPadFull, DeviceKeyPadFullGeneric, DeviceTextPad, DeviceTextPadGeneric, DeviceQuestionPad, DeviceQuestionPadGeneric, DeviceResetPad, DeviceResetPadGeneric, DevicePinPad, DevicePinPadGeneric };
    public enum PreAuthResult { None, Allow, Block };
    public enum PostAuthResult { None, Allow, Block, ChallengeUser, RegisterUser, RegisterUserOptional, RegisterQuestions };
    public enum ChallengeUserRulesResult { None, Allow, Block, ChallengeUser };
    public enum TransactionResult { None, Allow, Block, ChallengeUser };

    public class PreAuthResultInfo
    {
        public PreAuthResultInfo(VCryptRulesResult res)
        {
            if (res == null || res.Result == null)
            {
                BharosaTrace.Warn("PreAuthResultInfo(): VCryptRulesResult is null; perhaps pre-auth rules failed?");
                return;
            }

            mRuleResultString = res.Result;
            mScore = res.Score;

            if (StringUtil.EqualsIgnoreCase(res.Result, "Block"))
            {
                mResult = PreAuthResult.Block;
            }
            else if (StringUtil.EqualsIgnoreCase(res.Result, "Allow"))
            {
                mResult = PreAuthResult.Allow;
            }
            else
            {
                BharosaTrace.Error("PreAuthResultInfo(): unexpected pre-auth result - " + res.Result);
            }
        }

        public string RuleResultString
        {
            get { return mRuleResultString; }
        }

        public PreAuthResult Result
        {
            get { return mResult; }
        }

        public int Score
        {
            get { return mScore; }
        }

        string mRuleResultString = null;
        PreAuthResult mResult = PreAuthResult.None;
        int mScore = 0;
    }

    public class AuthentiPadResultInfo
    {
        public AuthentiPadResultInfo(VCryptRulesResult res)
        {
            if (res == null || res.Result == null)
            {
                BharosaTrace.Warn("AuthentiPadResultInfo(): VCryptRulesResult is null; perhaps pre-auth rules failed?");
                return;
            }

            mRuleResultString = res.Result;

            if (StringUtil.EqualsIgnoreCase(res.Result, "PasswordKeypadAlpha"))
            {
                mDevice = AuthDeviceType.DeviceKeyPad;
            }
            else if (StringUtil.EqualsIgnoreCase(res.Result, "PasswordKeypadAlphaGeneric") ||
                     StringUtil.EqualsIgnoreCase(res.Result, "PasswordKeypadAlphaGenericFirstTime"))
            {
                mDevice = AuthDeviceType.DeviceKeyPadGeneric;
            }
            else if (StringUtil.EqualsIgnoreCase(res.Result, "PasswordKeypadFull"))
            {
                mDevice = AuthDeviceType.DeviceKeyPadFull;
            }
            else if (StringUtil.EqualsIgnoreCase(res.Result, "PasswordKeypadFullGeneric") ||
                     StringUtil.EqualsIgnoreCase(res.Result, "PasswordKeypadFullGenericFirstTime"))
            {
                mDevice = AuthDeviceType.DeviceKeyPadFullGeneric;
            }
            else if (StringUtil.EqualsIgnoreCase(res.Result, "PasswordTextPad") ||
                     StringUtil.EqualsIgnoreCase(res.Result, "Password"))
            {
                mDevice = AuthDeviceType.DeviceTextPad;
            }
            else if (StringUtil.EqualsIgnoreCase(res.Result, "PasswordTextPadGeneric"))
            {
                mDevice = AuthDeviceType.DeviceTextPadGeneric;
            }
            else if (StringUtil.EqualsIgnoreCase(res.Result, "PasswordPinPad"))
            {
                mDevice = AuthDeviceType.DevicePinPad;
            }
            else if (StringUtil.EqualsIgnoreCase(res.Result, "PasswordPinPadGeneric") ||
                     StringUtil.EqualsIgnoreCase(res.Result, "PasswordPinPadGenericFirstTime"))
            {
                mDevice = AuthDeviceType.DevicePinPadGeneric;
            }
            else
            {
                BharosaTrace.Error("PreAuthResultInfo(): unexpected pre-auth result - " + res.Result);
            }

            if (res.Result.EndsWith("FirstTime"))
                mIsFirstTime = true;
        }

        public string RuleResultString
        {
            get { return mRuleResultString; }
        }

        public AuthDeviceType DeviceType
        {
            get { return mDevice; }
        }

        public bool IsDeviceFirstTime
        {
            get { return mIsFirstTime; }
        }

        string mRuleResultString = null;
        AuthDeviceType mDevice = AuthDeviceType.None;
        bool mIsFirstTime = false;
    }

    public class PostAuthResultInfo
    {
        public PostAuthResultInfo(VCryptRulesResult res)
        {
            if (res == null || res.Result == null)
            {
                BharosaTrace.Warn("PostAuthResultInfo(): VCryptRulesResult is null; perhaps post-auth rules failed?");
                return;
            }

            mRuleResultString = res.Result;
            mScore = res.Score;

            if (StringUtil.IsEmpty(res.Result))
            {
                mResult = PostAuthResult.None;
            }
            else if (StringUtil.EqualsIgnoreCase(res.Result, "Block"))
            {
                mResult = PostAuthResult.Block;
            }
            else if (StringUtil.EqualsIgnoreCase(res.Result, "Allow"))
            {
                mResult = PostAuthResult.Allow;
            }
            else if (res.Result.StartsWith("Challenge"))
            {
                mResult = PostAuthResult.ChallengeUser;
            }
            else if (res.Result.StartsWith("RegisterUserOptional"))
            {
                mResult = PostAuthResult.RegisterUserOptional;
            }
            else if (res.Result.StartsWith("RegisterUser"))
            {
                mResult = PostAuthResult.RegisterUser;
            }
            else if (res.Result.StartsWith("RegisterQuestions"))
            {
                mResult = PostAuthResult.RegisterQuestions;
            }
            else
            {
                BharosaTrace.Error("PostAuthResultInfo(): unexpected post-auth result - " + res.Result);
            }
        }

        public string RuleResultString
        {
            get { return mRuleResultString; }
        }

        public PostAuthResult Result
        {
            get { return mResult; }
        }

        public int Score
        {
            get { return mScore; }
        }

        public bool AllowAuthDeviceChange
        {
            get { return mAllowAuthDeviceChange; }
        }

        string mRuleResultString = null;
        PostAuthResult mResult = PostAuthResult.None;
        int mScore = 0;
        bool mAllowAuthDeviceChange = true;
    }

    public class TransactionResultInfo
    {
        public TransactionResultInfo(long transactionId, VCryptRulesResult res)
        {
            mTransactionId = transactionId;

            if (res == null || res.Result == null)
            {
                BharosaTrace.Warn("TransactionResultInfo(): VCryptRulesResult is null; perhaps transaction rules failed?");
                return;
            }

            mRuleResultString = res.Result;
            mScore = res.Score;

            if (StringUtil.IsEmpty(res.Result))
            {
                mResult = TransactionResult.None;
            }
            else if (StringUtil.EqualsIgnoreCase(res.Result, "Block"))
            {
                mResult = TransactionResult.Block;
            }
            else if (StringUtil.EqualsIgnoreCase(res.Result, "Allow"))
            {
                mResult = TransactionResult.Allow;
            }
            else if (res.Result.StartsWith("Challenge"))
            {
                mResult = TransactionResult.ChallengeUser;
            }
            else
            {
                BharosaTrace.Error("TransactionResultInfo(): unexpected transaction result - " + res.Result);
            }
        }

        public string RuleResultString
        {
            get { return mRuleResultString; }
        }

        public TransactionResult Result
        {
            get { return mResult; }
        }

        public int Score
        {
            get { return mScore; }
        }

        public long TransactionId
        {
            get { return mTransactionId; }
            set { mTransactionId = value; }
        }

        long mTransactionId = -1;
        string mRuleResultString = null;
        TransactionResult mResult = TransactionResult.None;
        int mScore = 0;
    }

    public class ChallengeUserRulesResultInfo
    {
        public ChallengeUserRulesResultInfo(VCryptRulesResult res)
        {
            if (res == null || res.Result == null)
            {
                BharosaTrace.Warn("ChallengeUserRulesResultInfo(): VCryptRulesResult is null; perhaps ChallengeUser rules failed?");
                return;
            }

            mRuleResultString = res.Result;
            mScore = res.Score;

            if (StringUtil.EqualsIgnoreCase(res.Result, "Allow"))
            {
                mResult = ChallengeUserRulesResult.Allow;
                mDevice = AuthDeviceType.DeviceQuestionPad;
            }
            else if (StringUtil.EqualsIgnoreCase(res.Result, "ChallengeUser"))
            {
                mResult = ChallengeUserRulesResult.ChallengeUser;
                mDevice = AuthDeviceType.DeviceQuestionPad;
            }
            else if (StringUtil.EqualsIgnoreCase(res.Result, "Block"))
            {
                mResult = ChallengeUserRulesResult.Block;
                mDevice = AuthDeviceType.None;
            }
            else
            {
                BharosaTrace.Error("ChallengeUserRulesResultInfo(): unexpected ChallengeUser rules result - " + res.Result);

                mResult = ChallengeUserRulesResult.Block;
                mDevice = AuthDeviceType.None;
            }
        }

        public string RuleResultString
        {
            get { return mRuleResultString; }
        }

        public ChallengeUserRulesResult Result
        {
            get { return mResult; }
        }

        public AuthDeviceType DeviceType
        {
            get { return mDevice; }
        }

        public int Score
        {
            get { return mScore; }
        }

        string mRuleResultString = null;
        ChallengeUserRulesResult mResult = ChallengeUserRulesResult.None;
        AuthDeviceType mDevice = AuthDeviceType.DeviceQuestionPad;
        int mScore = 0;
    }

    public class AuthentiPadData
    {
        public AuthentiPad AuthPad
        {
            get { return mAuthPad; }
            set { mAuthPad = value; }
        }

        public string PadName
        {
            get { return mAuthPad.PadName; }
        }

        public int ClientType
        {
            get { return mClientType; }
            set { mClientType = value; }
        }

        public string DataFieldName
        {
            get { return mAuthPad.PadName + "DataField"; }
        }

        public string ConfirmFieldName
        {
            get { return mAuthPad.PadName + "ConfirmField"; }
        }

        public AuthentiPad mAuthPad;
        public int mClientType;
    }

    public class AuthStatusEnum
    {
        public static int Success
        {
            get { return UserDefEnumFactory.getInstance().getElementValue("auth.status.enum", "success"); }
        }

        public static int InvalidUser
        {
            get { return UserDefEnumFactory.getInstance().getElementValue("auth.status.enum", "invalid_user"); }
        }

        public static int WrongPassword
        {
            get { return UserDefEnumFactory.getInstance().getElementValue("auth.status.enum", "wrong_password"); }
        }

        public static int WrongPin
        {
            get { return UserDefEnumFactory.getInstance().getElementValue("auth.status.enum", "wrong_pin"); }
        }

        public static int UserDisabled
        {
            get { return UserDefEnumFactory.getInstance().getElementValue("auth.status.enum", "user_disabled"); }
        }

        public static int WrongAnswer
        {
            get { return UserDefEnumFactory.getInstance().getElementValue("auth.status.enum", "wrong_answer"); }
        }

        public static int Block
        {
            get { return UserDefEnumFactory.getInstance().getElementValue("auth.status.enum", "block"); }
        }

        public static int Pending
        {
            get { return UserDefEnumFactory.getInstance().getElementValue("auth.status.enum", "pending"); }
        }

        public static int PendingActivation
        {
            get { return UserDefEnumFactory.getInstance().getElementValue("auth.status.enum", "pending_activation"); }
        }

        public static int SystemError
        {
            get { return UserDefEnumFactory.getInstance().getElementValue("auth.status.enum", "system_error"); }
        }
    }

    public class TransactionStatusEnum
    {
        public static int Success
        {
            get { return UserDefEnumFactory.getInstance().getElementValue("tracker.transaction.status.enum", "success"); }
        }

        public static int Block
        {
            get { return UserDefEnumFactory.getInstance().getElementValue("tracker.transaction.status.enum", "block"); }
        }

        public static int Reject
        {
            get { return UserDefEnumFactory.getInstance().getElementValue("tracker.transaction.status.enum", "reject"); }
        }

        public static int Pending
        {
            get { return UserDefEnumFactory.getInstance().getElementValue("tracker.transaction.status.enum", "pending"); }
        }
    }

    public class ClientTypeEnum
    {
        public static int Normal
        {
            get { return UserDefEnumFactory.getInstance().getElementValue("auth.client.type.enum", "normal"); }
        }

        public static int Textpad
        {
            get { return UserDefEnumFactory.getInstance().getElementValue("auth.client.type.enum", "textpad"); }
        }

        public static int AlphaKeypad
        {
            get { return UserDefEnumFactory.getInstance().getElementValue("auth.client.type.enum", "alpha_keypad"); }
        }

        public static int FullKeypad
        {
            get { return UserDefEnumFactory.getInstance().getElementValue("auth.client.type.enum", "full_keypad"); }
        }

        public static int Question
        {
            get { return UserDefEnumFactory.getInstance().getElementValue("auth.client.type.enum", "question"); }
        }

        public static int PinPad
        {
            get { return UserDefEnumFactory.getInstance().getElementValue("auth.client.type.enum", "num_keypad"); }
        }
    }

    public class AppUtil
    {
        public static AppUtil GetInstance()
        {
            return sInstance;
        }

        public VCryptAuthUser InitUser(HttpSessionState Session)
        {
            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            IBharosaProxy proxy = BharosaClientFactory.getProxyInstance();
            string loginId = sessionData.LoginId;
            string customerId = sessionData.CustomerId;
            VCryptAuthUser user = null;

            if (StringUtil.IsEmpty(customerId))
            {
                user = proxy.getUserByLoginId(loginId);

                if (!IsValidUserObject(user))
                {
                    BharosaTrace.Warn("InitUser(): no user exists with '" + loginId + "' as loginId; treating the given loginId as customerId...");

                    user = proxy.getUser(loginId);

                    if (!IsValidUserObject(user))
                    {
                        BharosaTrace.Warn("InitUser(): no user exists with '" + loginId + "' as loginId or customerId");
                    }
                }
            }
            else
            {
                user = proxy.getUser(customerId);
            }

            // if the user does not exist, create and set status to Invalid
            if (!IsValidUserObject(user))
            {
                string customerGroupId = BharosaConfig.get("default.usergroupname", "IRS");
                string defaultPassword = BharosaConfig.get("default.userpassword", "_");

                // if customerId is not given set customerId same as the given loginId
                if (StringUtil.IsEmpty(customerId))
                    customerId = loginId;

                string[] lLocaleArr = StringUtil.SplitString(BharosaLocale.getCurrentLocale().Name, '-');
                VCryptLocale lVCryptLocale = new VCryptLocale(lLocaleArr);

                user = new VCryptAuthUser(loginId, customerId, customerGroupId, defaultPassword);

                user.setLocale(lVCryptLocale);

                user = proxy.createUser(user);

                UserStatus status = BharosaConfig.getBoolean("newuser.autoactivate", false) ? UserStatus.Active : UserStatus.Invalid;
                string defPin = BharosaConfig.get("newuser.default.pin", "");
                string pinStatus = BharosaConfig.get("newuser.default.pinstatus", "active");

                proxy.setUserStatus(customerId, (int)UserStatus.Invalid);

                if (!StringUtil.IsEmpty(defPin))
                {
                    BharosaTrace.Debug("Setting default pin " + defPin);

                    proxy.setPIN(customerId, defPin, UserDefEnumFactory.getInstance().getElementValue("vcrypt.user.password.status.enum", pinStatus));
                }

                user = proxy.getUser(customerId);
            }

            UserStatus userStatus = (UserStatus)proxy.getUserStatus(user.CustomerId);

            sessionData.CurrentUser = user;
            sessionData.LoginId = user.LoginId;
            sessionData.CustomerId = user.CustomerId;
            sessionData.InitialUserStatus = userStatus;

            return user;
        }

        public bool SetUserStatusValid(HttpSessionState Session)
        {
            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            IBharosaProxy proxy = BharosaClientFactory.getProxyInstance();

            bool ret = proxy.setUserStatus(sessionData.CustomerId, (int)UserStatus.PendingActivation);

            return ret;
        }

        public PreAuthResultInfo RunPreAuthRules(HttpRequest Request, HttpSessionState Session)
        {
            BharosaTrace.Enter("AppUtil.RunPreAuthRules()");

            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            int preAuthType = UserDefEnumFactory.getInstance().getEnum("profile.type.enum").getElementValue("preauth");
            BharosaStringList profileTypes = new BharosaStringList();
            BharosaStringTable contextList = new BharosaStringTable();
            IBharosaProxy proxy = BharosaClientFactory.getProxyInstance();
            VCryptRulesResult res = null;
            PreAuthResultInfo ret = null;

            // reset preauth fields in sessionData
            sessionData.PreAuthRes = null;

            profileTypes.Add(preAuthType.ToString());

            res = proxy.processRules(sessionData.RequestId, profileTypes, contextList);

            ret = new PreAuthResultInfo(res);

            sessionData.PreAuthRes = ret;

            BharosaTrace.Exit("AppUtil.RunPreAuthRules()==> " + ret.Result);

            return ret;
        }

        public AuthentiPadResultInfo RunAuthentiPadRules(HttpRequest Request, HttpSessionState Session)
        {
            BharosaTrace.Enter("AppUtil.RunAuthentiPadRules()");

            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            int authentiPadType = UserDefEnumFactory.getInstance().getEnum("profile.type.enum").getElementValue("authentiPad");
            BharosaStringList profileTypes = new BharosaStringList();
            BharosaStringTable contextList = new BharosaStringTable();
            IBharosaProxy proxy = BharosaClientFactory.getProxyInstance();
            VCryptRulesResult res = null;
            AuthentiPadResultInfo ret = null;

            if (authentiPadType == -1)
                authentiPadType = 10;

            // reset authentipad fields in sessionData
            sessionData.AuthentiPadRes = null;

            profileTypes.Add(authentiPadType.ToString());

            res = proxy.processRules(sessionData.RequestId, profileTypes, contextList);

            ret = new AuthentiPadResultInfo(res);

            sessionData.AuthentiPadRes = ret;
            sessionData.UserAuthDevice = ret.DeviceType;

            BharosaTrace.Exit("AppUtil.RunAuthentiPadRules()==> " + ret.DeviceType);

            return ret;
        }

        public PostAuthResultInfo RunPostAuthRules(HttpRequest Request, HttpSessionState Session)
        {
            BharosaTrace.Enter("AppUtil.RunPostAuthRules()");

            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            int postAuthType = UserDefEnumFactory.getInstance().getEnum("profile.type.enum").getElementValue("postauth");
            BharosaStringList profileTypes = new BharosaStringList();
            BharosaStringTable contextList = new BharosaStringTable();
            IBharosaProxy proxy = BharosaClientFactory.getProxyInstance();
            VCryptRulesResult res = null;
            PostAuthResultInfo ret = null;

            // reset postauth fields in sessionData
            sessionData.PostAuthRes = null;

            profileTypes.Add(postAuthType.ToString());

            res = proxy.processRules(sessionData.RequestId, profileTypes, contextList);

            ret = new PostAuthResultInfo(res);

            sessionData.PostAuthRes = ret;

            BharosaTrace.Exit("AppUtil.RunPostAuthRules()==> " + ret.Result);

            return ret;
        }

        public ChallengeUserRulesResultInfo RunChallengeUserRules(HttpRequest Request, HttpSessionState Session)
        {
            BharosaTrace.Enter("AppUtil.RunChallengeUserRules()");

            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            int profileType = UserDefEnumFactory.getInstance().getEnum("profile.type.enum").getElementValue("challengeUser");
            BharosaStringList profileTypes = new BharosaStringList();
            BharosaStringTable contextList = new BharosaStringTable();
            IBharosaProxy proxy = BharosaClientFactory.getProxyInstance();
            VCryptRulesResult res = null;
            ChallengeUserRulesResultInfo ret = null;

            if (profileType == -1)
                profileType = 3;

            profileTypes.Add(profileType.ToString());

            res = proxy.processRules(sessionData.RequestId, profileTypes, contextList);

            ret = new ChallengeUserRulesResultInfo(res);

            BharosaTrace.Exit("AppUtil.RunChallengeUserRules()==> " + ret.Result);

            return ret;
        }

        public CookieSet UpdateAuthStatus(HttpRequest Request, HttpResponse Response, HttpSessionState Session,
                                          string loginId, string customerId, string customerGroupId, int clientType, int authStatus)
        {
            BharosaTrace.Enter("AppUtil.UpdateAuthStatus()");

            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            string requestId = sessionData.RequestId;
            string remoteIPAddr = Request.UserHostAddress;
            string remoteHost = Request.UserHostName;
            string secureClientVer = "1.0";
            object[] fingerPrintInfo = HttpUtil.GetBrowserFingerPrint();
            int fingerPrintType = fingerPrintInfo == null ? 0 : (int)fingerPrintInfo[0];
            string fingerPrint = fingerPrintInfo == null ? "" : (string)fingerPrintInfo[1];
            object[] fingerPrintInfo2 = HttpUtil.GetFlashFingerPrint();
            int fingerPrintType2 = fingerPrintInfo2 == null ? 0 : (int)fingerPrintInfo2[0];
            string fingerPrint2 = fingerPrintInfo2 == null ? "" : (string)fingerPrintInfo2[1];
            bool isSecure = false;
            IBharosaProxy proxy = BharosaClientFactory.getProxyInstance();
            CookieSet cs = null;

            lock (sessionData)
            {
                cs = (CookieSet)sessionData["cookieSet"];

                string secureCookie = (cs != null) ? cs.SecureCookie : null;
                string digitalCookie = (cs != null) ? cs.FlashCookie : null;

                if (secureCookie == null && Request.Cookies["vsc"] != null)
                    secureCookie = Request.Cookies["vsc"].Value;

                if (digitalCookie == null)
                    digitalCookie = (string)sessionData["v"];

                cs = proxy.updateLog(requestId, remoteIPAddr, remoteHost,
                                     secureCookie, digitalCookie, customerGroupId, customerId, loginId,
                                     isSecure, authStatus, clientType, secureClientVer,
                                     fingerPrintType, fingerPrint, fingerPrintType2, fingerPrint2);

                if (cs != null)
                {
                    sessionData["cookieSet"] = cs;
                }
            }

            if (cs != null)
            {
                BharosaTrace.Debug("Setting secure cookie=" + cs.SecureCookie);

                HttpUtil.UpdateSecureCookie(Response, cs);
            }

            BharosaTrace.Exit("AppUtil.UpdateAuthStatus()==> " + cs);

            return cs;
        }

        public CookieSet InitTracker(HttpRequest Request, HttpResponse Response, HttpSessionState Session)
        {
            BharosaTrace.Enter("AppUtil.InitTracker()");

            AppSessionData sessionData = AppSessionData.GetInstance(Session);

            string requestId = sessionData.RequestId;
            string remoteIPAddr = Request.UserHostAddress;
            string remoteHost = Request.UserHostName;
            string secureClientVersion = "1.0";
            int clientType = UserDefEnumFactory.getInstance().getElementValue("auth.client.type.enum", "image_tracker");
            int digitalClientType = UserDefEnumFactory.getInstance().getElementValue("auth.client.type.enum", "flash_tracker");
            string digitalClientVersion = "1.0";
            bool isFlashRequest = StringUtil.EqualsIgnoreCase(Request.Params["client"], "vfc");
            object[] fingerPrintInfo = HttpUtil.GetBrowserFingerPrint();
            int fingerPrintType = fingerPrintInfo == null ? 0 : (int)fingerPrintInfo[0];
            string fingerPrint = fingerPrintInfo == null ? "" : (string)fingerPrintInfo[1];
            object[] fingerPrintInfo2 = HttpUtil.GetFlashFingerPrint();
            int fingerPrintType2 = fingerPrintInfo2 == null ? 0 : (int)fingerPrintInfo2[0];
            string fingerPrint2 = fingerPrintInfo2 == null ? "" : (string)fingerPrintInfo2[1];
            CookieSet cs = null;

            lock (sessionData)
            {
                cs = (CookieSet)sessionData["cookieSet"];

                if (cs == null || isFlashRequest)
                {
                    IBharosaProxy proxy = BharosaClientFactory.getProxyInstance();

                    string secureCookie = (Request.Cookies["vsc"] != null) ? Request.Cookies["vsc"].Value : null;
                    string digitalCookie = isFlashRequest ? Request.Params["v"] : null; ;

                    //
                    // call updateLog() if user information is available; else call handleTrackerRequest()
                    //
                    VCryptAuthUser user = sessionData.CurrentUser;

                    if (IsValidUserObject(user))
                    {
                        cs = proxy.updateLog(requestId, remoteIPAddr, remoteHost, secureCookie, digitalCookie,
                                             user.CustomerGroupId, user.CustomerId, user.LoginId,
                                             false, AuthStatusEnum.Pending, ClientTypeEnum.Normal, secureClientVersion,
                                             fingerPrintType, fingerPrint, fingerPrintType2, fingerPrint2);
                    }
                    else
                    {
                        cs = proxy.handleTrackerRequest(requestId, remoteIPAddr, remoteHost,
                                                        secureCookie, clientType, secureClientVersion,
                                                        digitalCookie, digitalClientType, digitalClientVersion,
                                                        fingerPrintType, fingerPrint,
                                                        fingerPrintType2, fingerPrint2);
                    }

                    sessionData["cookieSet"] = cs;
                }
                else
                {
                    BharosaTrace.Debug("AppUtil.InitTracker(): Tracker initialized elsewhere!");
                }
            }

            if (cs != null)
            {
                BharosaTrace.Debug("Setting secure cookie=" + cs.SecureCookie);

                HttpUtil.UpdateSecureCookie(Response, cs);
            }

            if (isFlashRequest)
            {
                if (!Request.IsSecureConnection)
                {
                    Response.AppendHeader("Cache-Control", "no-cache"); //HTTP 1.1
                    Response.AppendHeader("Pragma", "no-cache"); //HTTP 1.0
                    Response.AppendHeader("Cache-Control", "no-store");
                    Response.Expires = 0; //prevents caching at the proxy server

                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                }
            }

            return cs;
        }

        public string AssignNewImage(HttpSessionState Session)
        {
            BharosaTrace.Enter("AppUtil.AssignNewImage()");

            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            IBharosaProxy proxy = BharosaClientFactory.getProxyInstance();
            string imagePath = null;
            VCryptAuthUser user = sessionData.CurrentUser;

            if (user != null)
            {
                BharosaTrace.Debug("AppUtil.AssignNewImage(): user=" + user.CustomerId);

                proxy.setImage(user.CustomerId, null); // this will assign a random image to the user

                user = proxy.getUser(user.CustomerId);

                sessionData.CurrentUser = user;

                imagePath = user.SecurityPreferences["imagePath"];
            }
            else
            {
                BharosaTrace.Warn("AppUtil.AssignNewImage(): no current user in session");
            }

            BharosaTrace.Exit("AppUtil.AssignNewImage()==>" + imagePath);

            return imagePath;
        }

        public string AssignNewCaption(HttpSessionState Session)
        {
            BharosaTrace.Enter("AppUtil.AssignNewCaption()");

            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            IBharosaProxy proxy = BharosaClientFactory.getProxyInstance();
            string caption = null;
            VCryptAuthUser user = sessionData.CurrentUser;

            if (user != null)
            {
                BharosaTrace.Debug("AppUtil.AssignNewCaption(): user=" + user.LoginId);

                proxy.setCaption(user.CustomerId, caption); // this will assign a random caption to the user

                user = proxy.getUser(user.CustomerId);

                sessionData.CurrentUser = user;

                caption = user.SecurityPreferences["personalNote"];
            }
            else
            {
                BharosaTrace.Warn("AppUtil.AssignNewCaption(): no current user in session");
            }

            BharosaTrace.Exit("AppUtil.AssignNewCaption()==>" + caption);

            return caption;
        }

        public string[] AssignNewImageAndCaption(HttpSessionState Session)
        {
            BharosaTrace.Enter("AppUtil.AssignNewImageAndCaption()");

            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            IBharosaProxy proxy = BharosaClientFactory.getProxyInstance();
            string imagePath = null;
            string caption = null;
            VCryptAuthUser user = sessionData.CurrentUser;

            if (user != null)
            {
                BharosaTrace.Debug("AppUtil.AssignNewImageAndCaption(): user=" + user.CustomerId);

                proxy.setImageAndCaption(user.CustomerId, imagePath, (VCryptLocalizedString)null); // this will assign a random image and caption to the user

                user = proxy.getUser(user.CustomerId);

                sessionData.CurrentUser = user;

                imagePath = user.SecurityPreferences["imagePath"];
                caption = user.SecurityPreferences["personalNote"];
            }
            else
            {
                BharosaTrace.Warn("AppUtil.AssignNewImageAndCaption(): no current user in session");
            }

            BharosaTrace.Exit("AppUtil.AssignNewImageAndCaption()==>" + imagePath + ", " + caption);

            return new string[] { imagePath, caption };
        }

        public bool SetAuthMode(HttpSessionState Session, AuthDeviceType deviceType)
        {
            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            IBharosaProxy proxy = BharosaClientFactory.getProxyInstance();
            VCryptAuthUser user = sessionData.CurrentUser;
            int authMode = 0;

            if (!IsValidUserObject(user))
            {
                BharosaTrace.Warn("AppUtil.SetAuthMode(): no current user in session");

                return false;
            }

            if (deviceType == AuthDeviceType.DeviceTextPad || deviceType == AuthDeviceType.DeviceTextPadGeneric)
            {
                authMode = UserDefEnumFactory.getInstance().getElementValue("auth.client.type.enum", "textpad");

                if (authMode == -1)
                    authMode = 10;
            }
            else
            {
                authMode = UserDefEnumFactory.getInstance().getElementValue("auth.client.type.enum", "full_keypad");

                if (authMode == -1)
                    authMode = 12;
            }

            sessionData.UserAuthDevice = deviceType;

            return proxy.setUserAuthMode(user.CustomerId, authMode);
        }

        public bool IsValidUserObject(VCryptAuthUser user)
        {
            return (user != null && user.SecurityPreferences != null &&
                    !StringUtil.IsEmpty(user.CustomerId) && !StringUtil.IsEmpty(user.LoginId));
        }

        public AuthentiPadData CreateAuthentiPad(HttpSessionState Session)
        {
            return this.CreateAuthentiPad(Session, null, AuthDeviceType.None);
        }

        public AuthentiPadData CreateAuthentiPad(HttpSessionState Session, string authpadName)
        {
            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            AuthentiPadResultInfo authentiPadRes = sessionData.AuthentiPadRes;
            AuthDeviceType deviceType = authentiPadRes.DeviceType;

            return this.CreateAuthentiPad(Session, authpadName, deviceType);
        }

        public AuthentiPadData CreateAuthentiPad(HttpSessionState Session, AuthDeviceType deviceType)
        {
            return this.CreateAuthentiPad(Session, null, deviceType);
        }

        public AuthentiPadData CreateAuthentiPad(HttpSessionState Session, string authpadName, AuthDeviceType deviceType)
        {
            return this.CreateAuthentiPad(Session, authpadName, deviceType, false);
        }

        public AuthentiPadData CreateSampleAuthentipad(HttpSessionState Session, string authpadName)
        {
            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            AuthDeviceType deviceType = sessionData.UserAuthDevice;
            bool isSample = true;

            return this.CreateAuthentiPad(Session, authpadName, deviceType, isSample);
        }

        public AuthentiPadData CreateAuthentiPad(HttpSessionState Session, string authpadName, AuthDeviceType deviceType, bool isSample)
        {
            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            IBharosaClient client = BharosaClientFactory.getClientInstance();
            VCryptAuthUser user = sessionData.CurrentUser;

            if (user == null)
            {
                BharosaTrace.Warn("AppUtil.CreateAuthentiPad(): no current user in session");

                return null;
            }

            if (deviceType == AuthDeviceType.None)
            {
                if (sessionData.UserAuthDevice == AuthDeviceType.None)
                    deviceType = AuthDeviceType.DeviceTextPad;
                else
                    deviceType = sessionData.UserAuthDevice;
            }

            // set default values for TextPad
            AuthentiPadType padType = AuthentiPadType.TYPE_TEXTPAD;
            int clientType = UserDefEnumFactory.getInstance().getElementValue("auth.client.type.enum", "textpad");
            string frameFile = BharosaConfig.get("bharosa.authentipad.textpad.frame.file", "textpad_bg/TP_O_password.png");
            string bgFile = BharosaConfig.get("bharosa.textpad.defaultimage", null);
            string caption = BharosaConfig.get("bharosa.textpad.defaultcaption", null);
            bool isADACompliant = BharosaConfig.getBoolean("bharosa.authentipad.isADACompliant", false);
            bool hasJS = true;
            bool hasImgs = true;

            if (deviceType == AuthDeviceType.DeviceKeyPad || deviceType == AuthDeviceType.DeviceKeyPadGeneric)
            {
                padType = AuthentiPadType.TYPE_ALPHANUMERICPAD;
                clientType = UserDefEnumFactory.getInstance().getElementValue("auth.client.type.enum", "alpha_keypad");
                frameFile = (isSample) ? BharosaConfig.get("bharosa.authentipad.alphanumeric.sample.frame.file", "alphanumpad_bg/kp_O_preview.png")
                                        : BharosaConfig.get("bharosa.authentipad.alphanumeric.frame.file", "alphanumpad_bg/kp_frame_O.png");

                if (StringUtil.IsEmpty(authpadName))
                    authpadName = BharosaConfig.get("password.entry.keypad.name", "Bharosa_KeyPad");

                bgFile = user.SecurityPreferences["imagePath"];
                caption = user.SecurityPreferences["personalNote"];

                if (deviceType == AuthDeviceType.DeviceKeyPadGeneric)
                {
                    bgFile = BharosaConfig.get("bharosa.keypad.defaultimage");
                    caption = BharosaConfig.get("bharosa.keypad.defaultcaption");
                }
            }
            else if (deviceType == AuthDeviceType.DeviceKeyPadFull || deviceType == AuthDeviceType.DeviceKeyPadFullGeneric)
            {
                padType = AuthentiPadType.TYPE_FULLKEYPAD;
                clientType = UserDefEnumFactory.getInstance().getElementValue("auth.client.type.enum", "full_keypad");
                frameFile = (isSample) ? BharosaConfig.get("bharosa.authentipad.alphanumeric.sample.frame.file", "alphanumpad_bg/kp_O_preview.png")
                                        : BharosaConfig.get("bharosa.authentipad.alphanumeric.frame.file", "alphanumpad_bg/kp_frame_O.png");

                if (StringUtil.IsEmpty(authpadName))
                    authpadName = BharosaConfig.get("password.entry.keypad.name", "Bharosa_KeyPad");

                bgFile = user.SecurityPreferences["imagePath"];
                caption = user.SecurityPreferences["personalNote"];

                if (deviceType == AuthDeviceType.DeviceKeyPadGeneric)
                {
                    bgFile = BharosaConfig.get("bharosa.keypad.defaultimage");
                    caption = BharosaConfig.get("bharosa.keypad.defaultcaption");
                }
            }
            else if (deviceType == AuthDeviceType.DeviceTextPad || deviceType == AuthDeviceType.DeviceTextPadGeneric)
            {
                padType = AuthentiPadType.TYPE_TEXTPAD;
                clientType = UserDefEnumFactory.getInstance().getElementValue("auth.client.type.enum", "textpad");
                frameFile = (isSample) ? BharosaConfig.get("bharosa.authentipad.textpad.sample.frame.file", "textpad_bg/TP_O_preview.png")
                                        : BharosaConfig.get("bharosa.authentipad.textpad.frame.file", "textpad_bg/TP_O_password.png");

                if (StringUtil.IsEmpty(authpadName))
                    authpadName = BharosaConfig.get("password.entry.textpad.name", "Bharosa_TextPad");

                bgFile = user.SecurityPreferences["imagePath"];
                caption = user.SecurityPreferences["personalNote"];

                if (deviceType == AuthDeviceType.DeviceTextPadGeneric)
                {
                    bgFile = BharosaConfig.get("bharosa.textpad.defaultimage");
                    caption = BharosaConfig.get("bharosa.textpad.defaultcaption");
                }
            }
            else if (deviceType == AuthDeviceType.DevicePinPad || deviceType == AuthDeviceType.DevicePinPadGeneric)
            {
                padType = AuthentiPadType.TYPE_PINPAD;
                clientType = UserDefEnumFactory.getInstance().getElementValue("auth.client.type.enum", "num_keypad");
                frameFile = (isSample) ? BharosaConfig.get("bharosa.authentipad.pinpad.sample.frame.file", "pinpad_bg/PP_O_preview.png")
                                        : BharosaConfig.get("bharosa.authentipad.pinpad.frame.file", "pinpad_bg/PP_O_frame.png");

                if (StringUtil.IsEmpty(authpadName))
                    authpadName = BharosaConfig.get("password.entry.numeric.name", "Bharosa_PinPad");

                bgFile = user.SecurityPreferences["imagePath"];
                caption = user.SecurityPreferences["personalNote"];

                if (deviceType == AuthDeviceType.DevicePinPadGeneric)
                {
                    bgFile = BharosaConfig.get("bharosa.numeric.defaultimage");
                    caption = BharosaConfig.get("bharosa.numeric.defaultcaption");
                }
            }
            else if (deviceType == AuthDeviceType.DeviceQuestionPad)
            {
                padType = AuthentiPadType.TYPE_QUESTIONPAD;
                clientType = UserDefEnumFactory.getInstance().getElementValue("auth.client.type.enum", "question");
                frameFile = (isSample) ? BharosaConfig.get("bharosa.authentipad.questionpad.sample.frame.file", "questionpad_bg/QuP_frame_O.png")
                                        : BharosaConfig.get("bharosa.authentipad.questionpad.frame.file", "questionpad_bg/QuP_frame_nologo.png");

                if (StringUtil.IsEmpty(authpadName))
                    authpadName = BharosaConfig.get("password.entry.questionpad.name", "Bharosa_QuestionPad");

                bgFile = user.SecurityPreferences["imagePath"];
                caption = user.SecurityPreferences["personalNote"];
            }
            else if (deviceType == AuthDeviceType.DeviceResetPad)
            {
                padType = AuthentiPadType.TYPE_RESETTEXTPAD;
                clientType = UserDefEnumFactory.getInstance().getElementValue("auth.client.type.enum", "textpad");
                frameFile = (isSample) ? BharosaConfig.get("bharosa.authentipad.resettextpad.sample.frame.file", "textpad_bg/TP_O_preview.png")
                                        : BharosaConfig.get("bharosa.authentipad.resettextpad.frame.file", "textpad_bg/TP_O_password.png");

                if (StringUtil.IsEmpty(authpadName))
                    authpadName = BharosaConfig.get("password.entry.resettextpad.name", "Bharosa_QuestionPad");

                bgFile = user.SecurityPreferences["imagePath"];
                caption = user.SecurityPreferences["personalNote"];
            }

            //
            // if no image is set for the user, assign a random image
            //
            if (StringUtil.IsEmpty(bgFile))
                bgFile = this.AssignNewImage(Session);

            //
            // if no caption is set for the user, assign a random caption
            //
            if (StringUtil.IsEmpty(caption))
                caption = this.AssignNewCaption(Session);

            AuthentiPadData ret = new AuthentiPadData();

            ret.AuthPad = client.getAuthentiPad(padType, authpadName, frameFile, bgFile, caption, isADACompliant, hasJS, hasImgs);
            ret.ClientType = clientType;
            ret.AuthPad.TimeStampText = DateTime.Now.ToString();

            return ret;
        }

        public AuthentiPad CreateQuestionPad(HttpSessionState Session, string authpadName, bool bDiffQuestionArea)
        {
            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            VCryptAuthUser user = sessionData.CurrentUser;

            // set default values for TextPad
            string frameFile = null;
            string bgFile = user.SecurityPreferences["imagePath"];
            string caption = user.SecurityPreferences["personalNote"];
            string question = BharosaConfig.get("register.question.question_text", "");
            bool isADACompliant = BharosaConfig.getBoolean("bharosa.authentipad.isADACompliant", false); ;
            bool hasJS = true;
            bool hasImgs = true;

            if (bDiffQuestionArea)
            {
                frameFile = BharosaConfig.get("bharosa.authentipad.questionpad.frame.file", "questionpad_bg/QuP_frame_O.png");
            }
            else
            {
                frameFile = BharosaConfig.get("bharosa.authentipad.questionpad.answeronly.frame.file", "questionpad_bg/QuP_frame_O.png");
            }

            IBharosaClient client = BharosaClientFactory.getClientInstance();

            AuthentiPad ret = client.getAuthentiPad(AuthentiPadType.TYPE_QUESTIONPAD, authpadName, frameFile, bgFile, caption, isADACompliant, hasJS, hasImgs);

            ret.QuestionText = question;
            ret.TimeStampText = DateTime.Now.ToString();

            return ret;
        }

        public int IsValidLogin(String pLoginId, String pPassword)
        {
            BharosaTrace.Debug("AppUtil:IsValidLogin(): "
                     + "LoginId = " + pLoginId
                    + ", Password = " + pPassword);

            int authStatus = AuthStatusEnum.Pending;

            if (pLoginId.ToLower().StartsWith("disabled_"))
            {
                authStatus = AuthStatusEnum.UserDisabled;
            }
            else if (pLoginId.ToLower().StartsWith("invalid_"))
            {
                authStatus = AuthStatusEnum.InvalidUser;
            }
            else if (this.IsValidPassword(pPassword))
            {
                authStatus = AuthStatusEnum.Success;
            }
            else
            {
                authStatus = AuthStatusEnum.WrongPassword;
            }

            BharosaTrace.Debug("AppUtil:IsValidLogin(): "
                     + "authStatus returned = " + authStatus);

            return authStatus;
        }

        public bool IsValidPassword(String pPassword)
        {
            bool ret = false;

            string lExpectedPassword = BharosaConfig.get("bharosa.default.password", "test");

            if (StringUtil.Equals(lExpectedPassword, pPassword))
                ret = true;

            BharosaTrace.Debug("AppUtil:IsValidPassword(): "
                           + "Password Entered = " + pPassword
                           + ", Expected password = " + lExpectedPassword);
            return ret;
        }

        public void SetLoginPage(AppSessionData sessionData, string loginPage)
        {
            sessionData["loginPage"] = loginPage;
        }

        public string GetLoginPage(AppSessionData sessionData)
        {
            string loginPage = (string)sessionData["loginPage"];

            if (StringUtil.IsEmpty(loginPage))
                loginPage = "LoginPage.aspx";

            return loginPage;
        }

        public VCryptResponse CreateTransaction(HttpRequest request, HttpSessionState session, String trxType, BharosaStringTable trxParams)
        {
            BharosaTrace.Enter("AppUtil.CreateTransaction()");

            AppSessionData sessionData = AppSessionData.GetInstance(session);

            IBharosaProxy proxy = BharosaClientFactory.getProxyInstance();

            VCryptResponse ret = proxy.createTransaction(sessionData.RequestId, DateTime.MinValue, null, trxType, TransactionStatusEnum.Pending, trxParams);

            BharosaTrace.Exit("AppUtil.CreateTransaction()");

            return ret;
        }

        public VCryptResponse UpdateTransaction(HttpRequest request, HttpSessionState session, long trxId, BharosaStringTable trxParams, int trxStatus, bool analyzePatterns)
        {
            BharosaTrace.Enter("AppUtil.UpdateTransaction()");

            AppSessionData sessionData = AppSessionData.GetInstance(session);
            IBharosaProxy proxy = BharosaClientFactory.getProxyInstance();

            VCryptResponse ret = proxy.updateTransaction(sessionData.RequestId, DateTime.MinValue, trxId, trxStatus, trxParams, analyzePatterns);

            BharosaTrace.Exit("AppUtil.UpdateTransaction()");

            return ret;
        }

        public TransactionResultInfo RunPreTransactionRules(HttpRequest request, HttpSessionState session, long trxId, int trxRunTime)
        {
            BharosaTrace.Enter("AppUtil.RunPreTransactionRules()");

            AppSessionData sessionData = AppSessionData.GetInstance(session);
            IBharosaProxy proxy = BharosaClientFactory.getProxyInstance();
            BharosaStringList runTimeList = new BharosaStringList();

            sessionData.PreTransactionRes = null;

            runTimeList.Add(trxRunTime.ToString());

            VCryptRulesResult res = proxy.processRules(sessionData.RequestId, trxId, null, DateTime.MinValue, runTimeList, null);

            TransactionResultInfo ret = new TransactionResultInfo(trxId, res);

            sessionData.PreTransactionRes = ret;

            BharosaTrace.Exit("AppUtil.RunPreTransactionRules()");

            return ret;
        }

        public TransactionResultInfo RunPostTransactionRules(HttpRequest request, HttpSessionState session, long trxId, int trxRunTime)
        {
            BharosaTrace.Enter("AppUtil.RunPostTransactionRules()");

            AppSessionData sessionData = AppSessionData.GetInstance(session);
            IBharosaProxy proxy = BharosaClientFactory.getProxyInstance();
            BharosaStringList runTimeList = new BharosaStringList();

            sessionData.PostTransactionRes = null;

            runTimeList.Add(trxRunTime.ToString());

            VCryptRulesResult res = proxy.processRules(sessionData.RequestId, trxId, null, DateTime.MinValue, runTimeList, null);

            TransactionResultInfo ret = new TransactionResultInfo(trxId, res);

            sessionData.PostTransactionRes = ret;

            BharosaTrace.Exit("AppUtil.RunPostTransactionRules()");

            return ret;
        }

        private AppUtil()
        {
        }

        private static AppUtil sInstance = new AppUtil();
    }
}
