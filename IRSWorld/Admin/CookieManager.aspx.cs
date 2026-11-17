/*
* Copyright (c) 2005 Bharosa, Inc.
* All rights reserved.
*
* This software is the confidential and proprietary information of 
* Bharosa, Inc. ("Confidential Information").  You shall not
* disclose such Confidential Information and shall use it only in
* accordance with the terms of the license agreement you entered into
* with Bharosa,Inc.
*/
using System;
using Bharosa.VCrypt.Common.Tracker;
using Bharosa.VCrypt.Common.Util;
using Bharosa.VCrypt.Client;

using IRSClassLib;
using IRSWorld.bharosa;

/// <summary> This classes servers the secure cookie and flash cookie request.
/// For the secure cookie request, it returns back the dummy image.
/// For flash cookie request, it returns back the new flash cookies
/// </summary>
namespace IRSWorld.Admin
{
    public partial class CookieManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BharosaTrace.Enter("CookieManager.Page_Load(): request.getQueryString()=" + Request.QueryString);

            AppSessionData sessionData = AppSessionData.GetInstance(Session);

            //Code to identify whether user has Image turned on or off
            string imgValue = Request.Params["img"];
            string flashValue = Request.Params["flash"];
            string jsValue = Request.Params["js"];

            if (!StringUtil.IsEmpty(flashValue))
            {
                sessionData["hasFlash"] = flashValue;
                Response.AppendHeader("Cache-Control", "no-cache"); //HTTP 1.1
                Response.AppendHeader("Pragma", "no-cache"); //HTTP 1.0
                Response.AppendHeader("Cache-Control", "no-store");
                Response.Expires = 0; //prevents caching at the proxy server
            }

            if (!StringUtil.IsEmpty(jsValue))
            {
                sessionData["hasJS"] = jsValue;
                Response.AppendHeader("Cache-Control", "no-cache"); //HTTP 1.1
                Response.AppendHeader("Pragma", "no-cache"); //HTTP 1.0
                Response.AppendHeader("Cache-Control", "no-store");
                Response.Expires = 0; //prevents caching at the proxy server
            }

            if (!StringUtil.IsEmpty(imgValue))
            {
                sessionData["hasImgs"] = imgValue;
                Response.AppendHeader("Cache-Control", "no-cache"); //HTTP 1.1
                Response.AppendHeader("Pragma", "no-cache"); //HTTP 1.0
                Response.AppendHeader("Cache-Control", "no-store");
                Response.Expires = 0; //prevents caching at the proxy server

                return;
            }

            AppUtil appUtil = AppUtil.GetInstance();

            CookieSet cs = appUtil.InitTracker(Request, Response, Session);

            string flashCookie = (cs == null || StringUtil.IsEmpty(cs.FlashCookie)) ? "undefined" : cs.FlashCookie;

            BharosaTrace.Debug("Request from flash:New flash cookie=" + flashCookie);

            BharosaTrace.Exit("CookieManager.Page_Load()");

            Response.Output.WriteLine("&v=" + flashCookie);
            Response.End();
        }
    }
}