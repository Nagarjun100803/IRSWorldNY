using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityGuardAdminServiceV11API;
using IdentityGuardAuthServiceV11API;
using System.IO;
using System.Configuration;
using System.Web.Services;
using System.Web;
using System.Web.Services.Protocols;
using System.Data;

namespace IRSClassLib
{
    public class Entrust
    {

        private string _uname;
        private string _pw;
        private string _activation_url;
        private string _ip_address_test;

        public enum IP_GEO_ASSESTMENT
        {
            AUTHENTICATED,
            CHALLENGE,
            REJECT
        }


        public Entrust()
        {
            _uname = new System.Configuration.AppSettingsReader().GetValue("Entrust_UID", Type.GetType("System.String")).ToString();
            _pw = new System.Configuration.AppSettingsReader().GetValue("Entrust_PW", Type.GetType("System.String")).ToString();
            _activation_url = new System.Configuration.AppSettingsReader().GetValue("Entrust_Activation_URL", Type.GetType("System.String")).ToString();
            _ip_address_test = new System.Configuration.AppSettingsReader().GetValue("Entrust_IPAddress", Type.GetType("System.String")).ToString();
        }



        public bool UserCreate(string userid)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string Url1 = new AppSettingsReader().GetValue("OTP2", Type.GetType("System.String")).ToString();
            try
            {
                string brcode = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
                String[] serviceUrls = Url1.Split(',');
                IGAdminServiceFailoverFactory factory = new IGAdminServiceFailoverFactory(serviceUrls, _uname, _pw);
                AdminService adminBinding = factory.getService();
                UserParms userParms = new UserParms();
                // set attributes
                userParms.Group = brcode;
                userParms.Aliases = null;
                userParms.ContactInfoList = null;
                userParms.AuthenticationSecrets = null;
                userParms.QaSecrets = null;
                userParms.SharedSecrets = null;
                UserCreateCallParms callParms = new UserCreateCallParms();
                callParms.userid = userid;
                callParms.parms = userParms;
                adminBinding.userCreate(callParms);
                //RegisterDevice(userid);
                return true;

            }
            catch (System.Web.Services.Protocols.SoapException soapEx)
            {
                csAuditLog audit = new csAuditLog();
                AuthenticationFault fault = AuthenticationService.getFault(soapEx);
                if (fault != null)
                {
                    audit.writeAudit("", "2FA User Registration", soapEx.Message, fault.errorMessage.ToString(), fault.ErrorCode.ToString(), csAuditLog.Category.Insert);
                }


                return false;
            }
            catch (Exception ex)
            {
                csAuditLog audit = new csAuditLog();
                audit.writeAudit("", "2FA User Registration", ex.Message, "", ex.InnerException.Message, csAuditLog.Category.Insert);
                return false;
            }
            return false;

        }

        public bool StartActivation(string userid)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                string Url1 = new AppSettingsReader().GetValue("OTP2", Type.GetType("System.String")).ToString();
                String[] serviceUrls = Url1.Split(',');
                IGAdminServiceFailoverFactory factory = new IGAdminServiceFailoverFactory(serviceUrls, _uname, _pw);

                AdminService binding = factory.getService();


                UserTokenCreateCallParms usertokencall = new UserTokenCreateCallParms();
                usertokencall.userid = userid;
                usertokencall.vendorId = "Entrust Soft Token";



                UserTokenParms usertokenparms = new UserTokenParms();
                usertokenparms.TokenSet = null;

                usertokenparms.generateQRCode = true;
                usertokencall.parms = usertokenparms;
                string tokenSerial = binding.userTokenCreate(usertokencall);


                UserTokenFilter filter = new UserTokenFilter();
                filter.VendorId = "Entrust Soft Token";
                filter.SerialNumber = tokenSerial;
                UserTokenParms usertokenparms2 = new UserTokenParms();
                usertokenparms2.TokenSet = null;
                usertokenparms2.activationAddress = _activation_url;
                usertokenparms2.generateQRCode = true;
                usertokenparms2.QRCodeSize = 250;


                UserTokenActivateCallParms callParms = new UserTokenActivateCallParms();
                callParms.userid = userid;
                callParms.filter = filter;
                callParms.parms = usertokenparms2;
                UserTokenActivateResult result = binding.userTokenActivate(callParms);
                // result contains the information the application needs to provide to the //soft token for
                // activation to proceed


                System.Web.HttpContext.Current.Session["QRCode"] = result.QRCode;
                System.Web.HttpContext.Current.Session["QRCodePassword"] = result.QRCodePassword;
                System.Web.HttpContext.Current.Session["SerialNo"] = filter.SerialNumber;
                System.Web.HttpContext.Current.Session["QRLink"] = result.QRCodeLink;



                return true;
            }
            catch (System.Web.Services.Protocols.SoapException soapEx)
            {
                csAuditLog audit = new csAuditLog();
                AuthenticationFault fault = AuthenticationService.getFault(soapEx);
                audit.writeAudit("", "2FA User Activation", soapEx.Message, "", "", csAuditLog.Category.Insert);
                return false;
            }
            catch (Exception ex)
            {
                csAuditLog audit = new csAuditLog();
                audit.writeAudit("", "2FA User Activation", ex.Message, "", ex.InnerException.Message, csAuditLog.Category.Insert);
                return false;
            }
            return false;

        }

        public bool CompleteActivation(String userid, string regcode)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                string Url1 = new AppSettingsReader().GetValue("OTP2", Type.GetType("System.String")).ToString();
                String[] serviceUrls = Url1.Split(',');
                IGAdminServiceFailoverFactory factory = new IGAdminServiceFailoverFactory(serviceUrls, _uname, _pw);

                AdminService binding = factory.getService();
                string serialNumber = Convert.ToString(System.Web.HttpContext.Current.Session["SerialNo"]);
                UserTokenFilter filter = new UserTokenFilter();

                filter.SerialNumber = serialNumber;
                IdentityGuardAdminServiceV11API.NameValue regCode = new IdentityGuardAdminServiceV11API.NameValue();
                regCode.Name = "registrationCode";
                regCode.Value = regcode;
                UserTokenActivateCompleteCallParms callParms = new UserTokenActivateCompleteCallParms();
                callParms.userid = userid;
                filter.VendorId = "Entrust Soft Token";
                callParms.filter = filter;
                callParms.activationParms = new IdentityGuardAdminServiceV11API.NameValue[] { regCode
                };
                binding.userTokenActivateComplete(callParms);
                return true;
            }
            catch (System.Web.Services.Protocols.SoapException soapEx)
            {
                csAuditLog audit = new csAuditLog();
                AuthenticationFault fault = AuthenticationService.getFault(soapEx);
                audit.writeAudit("", "2FA", soapEx.Message, "", "", csAuditLog.Category.Insert);
                return false;
            }
            catch (Exception ex)
            {
                csAuditLog audit = new csAuditLog();
                audit.writeAudit("", "2FA", ex.Message, "", ex.InnerException.Message, csAuditLog.Category.Insert);
                return false;
            }
            return false;
        }

        private string GetUserIP()
        {



            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string ipList = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if(ipList == null)
            {
                ipList = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            

            if (!string.IsNullOrEmpty(ipList))
            {
                string[] iplst = ipList.Split(',');
                foreach (string s in iplst)
                {
                    csAuditLog audit = new csAuditLog();
                    audit.writeAudit("", "2FA IP List", s, "", "", csAuditLog.Category.Insert);
                }

                return ipList.Split(',')[0];
            }

            return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }



        public bool ipGeolocation(string userId)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            csAuditLog audit = new csAuditLog();
            string Url1 = new irsConfiguration().appsetting("OTP1").ToString();

            String[] serviceUrls = Url1.Split(',');
            audit.writeAudit("", "2FA IP Address", GetUserIP(), string.Empty, string.Empty, csAuditLog.Category.Insert);
            //connection to identityGuard web service
            try
            {
                IGAuthServiceFailoverFactory factory = new IGAuthServiceFailoverFactory(serviceUrls);
                AuthenticationService binding = factory.getService();

                // process the challenge
                GenericChallengeParms genChallengeParams = new GenericChallengeParms();
                //IP Geolocation

                genChallengeParams.IPAddress = GetUserIP();//_ip_address_test;//"182.18.238.191";//"202.58.236.76"//genChallengeParams.IPAddress = _ip_address_test;//"202.58.236.76";//this.GetIPAddress(new HttpRequestWrapper(HttpContext.Current.Request));
                                                                // audit.writeAudit("", "IP Address Login", genChallengeParams.IPAddress, "", "", csAuditLog.Category.Insert);

                genChallengeParams.SecurityLevel = SecurityLevel.NORMAL;
                GetGenericChallengeCallParms callParams = new GetGenericChallengeCallParms();
                callParams.userId = userId;
                callParams.parms = genChallengeParams;

                GenericChallenge genChallenge = binding.getGenericChallenge(callParams);

                ChallengeRequestResult request_result = genChallenge.challengeRequestResult;

                //if(genChallengeParams.IPAddress == "202.58.236.76")
                //{
                //    HttpContext.Current.Session["token_validity"] = true;
                //    HttpContext.Current.Session["IP_GEO_STAT"] = "AUTHENTICATED";
                //    audit.writeAudit("", "2FA IP Geolocation", "Authenticated", string.Empty, string.Empty, csAuditLog.Category.Insert);
                //    return true;
                //}
                //else if(genChallengeParams.IPAddress == "222.127.72.71")
                //{
                //    HttpContext.Current.Session["IP_GEO_STAT"] = "CHALLENGE";
                //    audit.writeAudit("", "2FA IP Geolocation", "Challenge", string.Empty, string.Empty, csAuditLog.Category.Insert);
                //    return true;
                //}
                //else if(request_result == ChallengeRequestResult.REJECT)
                //{
                //    HttpContext.Current.Session["token_validity"] = false;
                //    HttpContext.Current.Session["IP_GEO_STAT"] = "REJECT";
                //    audit.writeAudit("", "2FA IP Geolocation", "Reject",string.Empty , string.Empty, csAuditLog.Category.Insert);
                //    return false;

                //    //    HttpContext.Current.Session["token_validity"] = false;
                //    //    HttpContext.Current.Session["IP_GEO_STAT"] = "REJECT";
                //    //    return false;
                //}


                if (request_result == ChallengeRequestResult.REJECT)
                {
                    HttpContext.Current.Session["token_validity"] = false;
                    HttpContext.Current.Session["IP_GEO_STAT"] = "REJECT";
                    return false;
                }

                else if (request_result == ChallengeRequestResult.AUTHENTICATED)
                {
                    if (!((bool)genChallenge.riskScoringResult.IPAuthenticationPassed))
                    {
                        HttpContext.Current.Session["IP_GEO_STAT"] = "CHALLENGE";
                        return true;
                    }
                    else
                    {
                        HttpContext.Current.Session["token_validity"] = true;
                        HttpContext.Current.Session["IP_GEO_STAT"] = "AUTHENTICATED";
                        return true;
                    }
                }
                else if (request_result == ChallengeRequestResult.CHALLENGE)
                {

                    HttpContext.Current.Session["IP_GEO_STAT"] = "CHALLENGE";
                    return true;
                }

                return false;
            }
            catch (System.Web.Services.Protocols.SoapException soapEx)
            {

                AuthenticationFault fault = AuthenticationService.getFault(soapEx);
                if (fault != null)
                {
                    if (fault.ErrorCode == IdentityGuardAuthServiceV11API.ErrorCode.USER_DOES_NOT_EXIST)
                    {
                        HttpContext.Current.Session["IP_GEO_STAT"] = "AUTHENTICATED";
                        audit.writeAudit("", "2FA IP Geolocation", fault.InternalCode, fault.ErrorCode.ToString(), fault.errorMessage, csAuditLog.Category.Insert);
                        return true;
                    }

                    audit.writeAudit("", "2FA IP Geolocation", fault.InternalCode, fault.ErrorCode.ToString(), fault.errorMessage, csAuditLog.Category.Insert);
                }
                else
                {
                    audit.writeAudit("", "2FA IP Geolocation", soapEx.Message, "", "", csAuditLog.Category.Insert);
                }
                return false;
            }
            catch (Exception ex)
            {
                audit.writeAudit("", "2FA IP Geolocation", ex.Message, "", "", csAuditLog.Category.Insert);
                return false;
            }

        }


        public IP_GEO_ASSESTMENT ipGeoAssestment()
        {
            if (HttpContext.Current.Session["IP_GEO_STAT"] != null)
            {
                switch (HttpContext.Current.Session["IP_GEO_STAT"].ToString())
                {
                    case "AUTHENTICATED":
                        return IP_GEO_ASSESTMENT.AUTHENTICATED;
                    case "CHALLENGE":
                        return IP_GEO_ASSESTMENT.CHALLENGE;
                    case "REJECT":
                        return IP_GEO_ASSESTMENT.REJECT;
                }
                return IP_GEO_ASSESTMENT.REJECT;
            }
            return IP_GEO_ASSESTMENT.REJECT;

        }

        public bool ValidateMachine(string deviceFingerPrint, string userid)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string Url1 = new irsConfiguration().appsetting("OTP1").ToString();

            String[] serviceUrls = Url1.Split(',');

            //connection to identityGuard web service

            IGAuthServiceFailoverFactory factory = new IGAuthServiceFailoverFactory(serviceUrls);
            AuthenticationService binding = factory.getService();


            MachineSecret msecret = new MachineSecret();
            msecret.deviceFingerprint = deviceFingerPrint;
            GenericChallengeParms genChallengeParams = new GenericChallengeParms();
            GenericChallengeParms parms = new GenericChallengeParms();
            parms.machineSecret = msecret;
            GetGenericChallengeCallParms getGenericChallengeCallParms
            = new GetGenericChallengeCallParms();
            getGenericChallengeCallParms.userId = userid;
            getGenericChallengeCallParms.parms = parms;
            GenericChallenge genericChallenge =
            binding.getGenericChallenge(getGenericChallengeCallParms);
            if (genericChallenge.challengeRequestResult.Equals(
            ChallengeRequestResult.AUTHENTICATED))
            {
                // No second-factor authentication is required as
                // the machine authentication successfully passed
                // Get the updated machine secret (for sequence nonce)
                // and update the local fingerprint
                msecret = genericChallenge.machineSecret;
            }
            else if (genericChallenge.challengeRequestResult.Equals(
            ChallengeRequestResult.CHALLENGE))
            {
                // require second-factor challenge
                // add code to display challenge and retrieve user response
            }
            else if (genericChallenge.challengeRequestResult.Equals(
            ChallengeRequestResult.REJECT))
            {
                // authentication has been rejected
            }
            return true;
        }


        public bool StoreMachineInformation(string deviceFingerPrint, string userid, string token)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string Url1 = new irsConfiguration().appsetting("OTP1").ToString();

            String[] serviceUrls = Url1.Split(',');

            //connection to identityGuard web service

            IGAuthServiceFailoverFactory factory = new IGAuthServiceFailoverFactory(serviceUrls);
            AuthenticationService binding = factory.getService();


            MachineSecret msecret = new MachineSecret();
            msecret.deviceFingerprint = deviceFingerPrint;
            GenericAuthenticateParms genAuthParms = new GenericAuthenticateParms();
            genAuthParms.machineSecret = msecret;

            String[] userResponse = { token };

            Response response = new Response();
            response.response = userResponse;


            AuthenticateGenericChallengeCallParms authenticateGenericChallengeCallParms
            = new AuthenticateGenericChallengeCallParms();
            authenticateGenericChallengeCallParms.userId = userid;
            authenticateGenericChallengeCallParms.response = response;
            authenticateGenericChallengeCallParms.parms = genAuthParms;
            GenericAuthenticateResponse resp = binding.authenticateGenericChallenge(
            authenticateGenericChallengeCallParms);
            // handle the response
            // Get the updated machine secret and update the local fingerprint
            msecret = resp.machineSecret;


            return true;
        }


        public bool authenticate(string userid, IdentityGuardAuthServiceV11API.NameValue[] transactionDetails, string summary)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string Url1 = new System.Configuration.AppSettingsReader().GetValue("OTP1", Type.GetType("System.String")).ToString();

            String[] serviceUrls = Url1.Split(',');

            //connection to identityGuard web service

            IGAuthServiceFailoverFactory factory = new IGAuthServiceFailoverFactory(serviceUrls);
            AuthenticationService binding = factory.getService();

            GenericChallengeParms gParms = new GenericChallengeParms();
            gParms.AuthenticationType = IdentityGuardAuthServiceV11API.AuthenticationType.TOKENRO;
            gParms.performDeliveryAndSignature = true;
            gParms.requireDeliveryAndSignatureIfAvailable = true;
            gParms.tokenTransactionMode = IdentityGuardAuthServiceV11API.TokenTransactionMode.ONLINE;
            gParms.tokenChallengeSummary = summary;
            // if the transactionDetails are null then an authentication
            // transaction verification challenge will be performed.
            // Otherwise, a transaction verification challenge will be performed.
            gParms.transactionDetails = transactionDetails;
            GetGenericChallengeCallParms challengeCallParms = new GetGenericChallengeCallParms();
            challengeCallParms.userId = userid;
            challengeCallParms.parms = gParms;
            GenericChallenge challenge = binding.getGenericChallenge(challengeCallParms);
            GenericAuthenticateParms aParms = new GenericAuthenticateParms();
            aParms.AuthenticationType = IdentityGuardAuthServiceV11API.AuthenticationType.TOKENRO;
            aParms.transactionId = challenge.TokenChallenge.TransactionId;
            aParms.transactionDetails = transactionDetails;

            // instead of looping forever we could look at the challenge
            // lifetime in the returned challenge and quit after it has
            // expired.
            // Alternatively, Entrust IdentityGuard will return an error when
            // the challenge has expired.
            int t_ctr = 0;
            while (true)
            {
                if (HttpContext.Current.Session["LISTENERCANCELLED"] != null)
                {
                    bool iscancelled = Convert.ToBoolean(HttpContext.Current.Session["LISTENERCANCELLED"]);
                    if (iscancelled)
                    {
                        break;
                    }
                }


                try
                {
                    AuthenticateGenericChallengeCallParms genChallengeCallParms = new AuthenticateGenericChallengeCallParms();
                    genChallengeCallParms.userId = userid;
                    genChallengeCallParms.response = new Response();
                    genChallengeCallParms.parms = aParms;
                    GenericAuthenticateResponse response = binding.authenticateGenericChallenge(genChallengeCallParms);
                    HttpContext.Current.Session["TRANS_STATUS"] = "ACCEPT";
                    //return true;
                    break;
                }
                catch (SoapException soapEx)
                {
                    AuthenticationFault fault = AuthenticationService.getFault(soapEx);
                    if (fault != null && fault.ErrorCode == IdentityGuardAuthServiceV11API.ErrorCode.TRANSACTION_CANCEL)
                    {
                        HttpContext.Current.Session["TRANS_STATUS"] = "CANCEL";
                        break;
                        //return false;

                    }
                    else if (fault != null && fault.ErrorCode == IdentityGuardAuthServiceV11API.ErrorCode.TRANSACTION_CONCERN)
                    {
                        HttpContext.Current.Session["TRANS_STATUS"] = "CONCERN";
                        break;
                    }
                    else if (fault != null && fault.ErrorCode == IdentityGuardAuthServiceV11API.ErrorCode.NO_RESPONSE)
                    {
                        // continue if we received NO_RESPONSE.Entrust IdentityGuard Programming Guide for the Java Platform
                        // Page | 130 © 2017 Entrust Datacard. All rights reserved
                        // Otherwise return the error to the caller
                        //System.Console.WriteLine(fault.errorMessage);


                        System.Threading.Thread.Sleep(2000);
                        t_ctr += 2;
                        if (t_ctr >= 60)
                        {
                            HttpContext.Current.Session["TRANS_STATUS"] = "EXPIRED";
                            break;
                        }
                        continue;
                    }
                    else
                    {
                        // not IG fault, handle the generic SoapException
                        break;

                    }
                }
            }

            if (HttpContext.Current.Session["TRANS_STATUS"] != null)
            {
                if (HttpContext.Current.Session["TRANS_STATUS"].ToString() == "ACCEPT")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }




            return false;
        }



        public bool tokenactivation(string userid)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string Url1 = new System.Configuration.AppSettingsReader().GetValue("OTP1", Type.GetType("System.String")).ToString();

            String[] serviceUrls = Url1.Split(',');

            //connection to identityGuard web service

            IGAuthServiceFailoverFactory factory = new IGAuthServiceFailoverFactory(serviceUrls);
            AuthenticationService binding = factory.getService();



            // instead of looping forever we could look at the challenge
            // lifetime in the returned challenge and quit after it has
            // expired.
            // Alternatively, Entrust IdentityGuard will return an error when
            // the challenge has expired.
            int t_ctr = 0;
            while (true)
            {
                try
                {
                    GenericChallengeParms gParms = new GenericChallengeParms();
                    gParms.AuthenticationType = IdentityGuardAuthServiceV11API.AuthenticationType.TOKENRO;
                    gParms.performDeliveryAndSignature = true;
                    gParms.requireDeliveryAndSignatureIfAvailable = true;
                    gParms.tokenTransactionMode = IdentityGuardAuthServiceV11API.TokenTransactionMode.ONLINE;

                    // if the transactionDetails are null then an authentication
                    // transaction verification challenge will be performed.
                    // Otherwise, a transaction verification challenge will be performed.
                    GetGenericChallengeCallParms challengeCallParms = new GetGenericChallengeCallParms();
                    challengeCallParms.userId = userid;
                    challengeCallParms.parms = gParms;
                    GenericChallenge challenge = binding.getGenericChallenge(challengeCallParms);
                    GenericAuthenticateParms aParms = new GenericAuthenticateParms();
                    aParms.AuthenticationType = IdentityGuardAuthServiceV11API.AuthenticationType.TOKENRO;
                    aParms.transactionId = challenge.TokenChallenge.TransactionId;

                    return true;
                }

                catch (SoapException soapEx)
                {
                    AuthenticationFault fault = AuthenticationService.getFault(soapEx);
                    
                    if (fault != null && fault.ErrorCode == IdentityGuardAuthServiceV11API.ErrorCode.NO_RESPONSE)
                    {
                        // continue if we received NO_RESPONSE.Entrust IdentityGuard Programming Guide for the Java Platform
                        // Page | 130 © 2017 Entrust Datacard. All rights reserved
                        // Otherwise return the error to the caller
                        //System.Console.WriteLine(fault.errorMessage);
                        System.Threading.Thread.Sleep(2000);
                        continue;
                    }
                    else if (fault != null && fault.ErrorCode == IdentityGuardAuthServiceV11API.ErrorCode.NO_ACTIVE_TOKENS)
                    {
                        System.Threading.Thread.Sleep(2000);
                        continue;
                    }
                    else
                    {
                        // not IG fault, handle the generic SoapException
                        break;

                    }
                }
                catch(Exception ex)
                {

                }
            }
            return false;
        }


        public IdentityGuardAuthServiceV11API.TokenData[] getQRCodeChallenge(String userid,
                                        IdentityGuardAuthServiceV11API.NameValue[] transactionDetails, String transactionSummary)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string Url1 = new System.Configuration.AppSettingsReader().GetValue("OTP1", Type.GetType("System.String")).ToString();

            String[] serviceUrls = Url1.Split(',');

            //connection to identityGuard web service

            IGAuthServiceFailoverFactory factory = new IGAuthServiceFailoverFactory(serviceUrls);
            AuthenticationService binding = factory.getService();
            HttpContext.Current.Session["transaction_details"] = transactionDetails;

            GenericChallengeParms gParms = new GenericChallengeParms();
            gParms.AuthenticationType = IdentityGuardAuthServiceV11API.AuthenticationType.TOKENRO;
            gParms.tokenTransactionMode = IdentityGuardAuthServiceV11API.TokenTransactionMode.OFFLINE;
            gParms.tokenChallengeSummary = transactionSummary;
            gParms.transactionDetails = transactionDetails;
            GetGenericChallengeCallParms callParms = new GetGenericChallengeCallParms();
            callParms.userId = userid;
            callParms.parms = gParms;
            GenericChallenge challenge = binding.getGenericChallenge(callParms);
            if ((challenge.TokenChallenge.tokens == null)
            || (challenge.TokenChallenge.tokens.Length == 0))
            {
                throw new Exception("No tokens.");
            }
            // each token returned will include the QR Code information which must be Entrust IdentityGuard Programming Guide for the Java Platform © 2017 Entrust Datacard. All rights reserved Page | 133 // displayed by the application
            return challenge.TokenChallenge.tokens;
        }

        public bool ConfimrOfflineTransaction(string userid, string userResponse)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string Url1 = new System.Configuration.AppSettingsReader().GetValue("OTP1", Type.GetType("System.String")).ToString();

            String[] serviceUrls = Url1.Split(',');

            //connection to identityGuard web service

            IGAuthServiceFailoverFactory factory = new IGAuthServiceFailoverFactory(serviceUrls);
            AuthenticationService binding = factory.getService();

            try
            {

                string[] userresp = new string[] { userResponse };

                Response response = new Response();
                response.response = userresp;
                GenericAuthenticateParms authParms = new GenericAuthenticateParms();
                // The same transaction details must be included in
                // authenticateGenericChallenge
                authParms.transactionDetails = (IdentityGuardAuthServiceV11API.NameValue[])HttpContext.Current.Session["transaction_details"];
                authParms.AuthenticationType = IdentityGuardAuthServiceV11API.AuthenticationType.TOKENRO;
                AuthenticateGenericChallengeCallParms authCallParms
                = new AuthenticateGenericChallengeCallParms();
                authCallParms.userId = userid;
                authCallParms.parms = authParms;
                authCallParms.response = response;
                GenericAuthenticateResponse authResponse = binding.authenticateGenericChallenge(authCallParms);
                String name = authResponse.FullName;
                HttpContext.Current.Session["transaction_details"] = null;
                if (name == null) name = userid;
                return true;
            }
            catch (SoapException soapEx)
            {
                AuthenticationFault fault
                = AuthenticationService.getFault(soapEx);
                if (fault != null)
                {
                    // handle the IdentityGuard fault
                    System.Console.WriteLine(fault.errorMessage);
                    return false;
                }
                else
                {
                    // not IG fault, handle the generic SoapException
                    System.Console.WriteLine(soapEx.Message);
                    return false;
                }
            }

            return false;
        }


        public bool confirm_mfa(string branchcode)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            sqlHelperWs sql = new sqlHelperWs();
            /* VAPT 20191010 */
            object o = null;

            try
            {
                if (System.Web.HttpContext.Current.Session["ENTRUSTMFABRANCH"] != null)
                {
                    o = (object)System.Web.HttpContext.Current.Session["ENTRUSTMFABRANCH"];
                }
                else
                {
                    o = sql.execScalar("SELECT FldCode FROM tbl_Lookup WHERE FldGroup='MFA' AND FldDescription = '" + branchcode + "'");
                    System.Web.HttpContext.Current.Session["ENTRUSTMFABRANCH"] = o;
                }
            }
            catch
            {
                o = sql.execScalar("SELECT FldCode FROM tbl_Lookup WHERE FldGroup='MFA' AND FldDescription = '" + branchcode + "'");
                System.Web.HttpContext.Current.Session["ENTRUSTMFABRANCH"] = o;
            }
            
            if (Convert.ToInt16(o) >= 1)
            {
                return true;
            }
            return false;
        }

        public bool checktoken(string userId, string machineInformation, string stoken)
        {
            bool bContinue = false;
            string Url1 = new irsConfiguration().appsetting("OTP1").ToString();

            String[] serviceUrls = Url1.Split(',');


            try
            {
                IGAuthServiceFailoverFactory factory = new IGAuthServiceFailoverFactory(serviceUrls);

                AuthenticationService binding = factory.getService();

                // Create the generic challenge parameters and set the authentication type
                GenericChallengeParms genericChallengeParms = new GenericChallengeParms();
                genericChallengeParms.AuthenticationType = IdentityGuardAuthServiceV11API.AuthenticationType.TOKENRO;

                // Get the generic challenge via Generic challenge call parameters
                GetGenericChallengeCallParms callParms = new GetGenericChallengeCallParms();
                //Userid is the mandatory parameter to get generic challenge
                callParms.userId = userId;
                callParms.parms = genericChallengeParms;
                GenericChallenge genericChallenge = null;

                // Get the token challenge and issue the challenge

                genericChallenge = binding.getGenericChallenge(callParms);
                //               return true;
                bContinue = Authenticate(userId, stoken, machineInformation);
                System.Web.HttpContext.Current.Session["USER_OTP"] = false;
                if (bContinue == true)
                {

                    System.Web.HttpContext.Current.Session["USER_OTP"] = true;

                }
                return bContinue;

            }
            catch (SoapException soapEx)
            {
                //lblStatus.Text = "soap error occur";
                AuthenticationFault fault = AuthenticationService.getFault(soapEx);
                if (fault != null)
                {
                    fault.ErrorCode = fault.ErrorCode;
                    string error_message = fault.errorMessage;
                    string s = fault.InternalCode;
                    csAuditLog audit = new csAuditLog();
                    audit.writeAudit("", "2FA User Check Token", soapEx.Message, s, error_message, csAuditLog.Category.Insert);
                    return false;
                    // lblStatus.Text = soapEx.StackTrace;
                    return false;
                }
                else
                {
                    csAuditLog audit = new csAuditLog();
                    audit.writeAudit("", "2FA User Check Token", soapEx.Message, "", "", csAuditLog.Category.Insert);
                    // not IG fault, handle the generic SoapException
                    //lblStatus.Text = soapEx.StackTrace;
                    return false;

                }
            }
            catch (Exception ex)
            {
                csAuditLog audit = new csAuditLog();
                audit.writeAudit("", "2FA User Check Token", ex.Message, "", "", csAuditLog.Category.Insert);
                // handle non-soap exception, e.g., connection error
                //lblStatus.Text = "general error occur";
                //lblStatus.Text = ex.Message;
                return false;
            }

        }

        private bool Authenticate(string userId, string responseInput, string machineInformation)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            //new Entrust().StoreMachineInformation(machineInformation, userId, responseInput);
            csAuditLog audit = new csAuditLog();
            try
            {
                string Url1 = new irsConfiguration().appsetting("OTP1").ToString();

                String[] serviceUrls = Url1.Split(',');

                //connection to identityGuard web service

                IGAuthServiceFailoverFactory factory = new IGAuthServiceFailoverFactory(serviceUrls);
                AuthenticationService binding = factory.getService();

                // process the challenge
                Response response = new Response();
                GenericAuthenticateParms authParms = new GenericAuthenticateParms();

                //initalise array of size 1 with the text using shorthand notation
                String[] userResponse = { responseInput };
                response.response = userResponse;


                // Once the client application gets a challenge response from the user, authenticate the response

                //set the AuthenticateGenericChallengeCallParms
                authParms.AuthenticationType = IdentityGuardAuthServiceV11API.AuthenticationType.TOKENRO;
                AuthenticateGenericChallengeCallParms authCallParms = new AuthenticateGenericChallengeCallParms();
                authCallParms.userId = userId;
                authCallParms.parms = authParms;
                authCallParms.response = response;


                GenericAuthenticateResponse authResponse = binding.authenticateGenericChallenge(authCallParms);
                GenericChallengeParms genChallengeParams = new GenericChallengeParms();


                //IP Geolocation

                genChallengeParams.IPAddress = GetUserIP();//"202.58.234.100";//this.GetIPAddress(new HttpRequestWrapper(HttpContext.Current.Request));
                audit.writeAudit("", "IP Address Login", genChallengeParams.IPAddress, "", "", csAuditLog.Category.Insert);

                genChallengeParams.SecurityLevel = SecurityLevel.NORMAL;
                GetGenericChallengeCallParms callParams = new GetGenericChallengeCallParms();
                callParams.userId = userId;
                callParams.parms = genChallengeParams;

                GenericChallenge genChallenge = binding.getGenericChallenge(callParams);

                ChallengeRequestResult request_result = genChallenge.challengeRequestResult;
                if (request_result == ChallengeRequestResult.AUTHENTICATED)
                {
                    return true;
                }
                else if (request_result == ChallengeRequestResult.CHALLENGE)
                {
                    return true;
                }
                else if (request_result == ChallengeRequestResult.REJECT)
                {
                    return false;
                }
                return true;
            }
            catch (SoapException soapEx)
            {

                AuthenticationFault fault = AuthenticationService.getFault(soapEx);
                if (fault != null)
                {
                    audit.writeAudit("", "2FA Login", soapEx.Message, "", fault.errorMessage, csAuditLog.Category.Insert);
                    //lblStatus.Text = soapEx.StackTrace;
                    return false;
                }
                else
                {

                    return false;

                }
            }
            catch (Exception ex)
            {
                audit.writeAudit("", "2FA Login", ex.Message, "", ex.InnerException.Message, csAuditLog.Category.Insert);
                return false;
            }

        }

        //private string GetUserIP()
        //{

        //    string ipList = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        //    if (!string.IsNullOrEmpty(ipList))
        //    {
        //        return ipList.Split(',')[0];
        //    }

        //    return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //}



    }
}
