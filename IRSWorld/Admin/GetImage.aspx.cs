using System;
using System.IO;
using Bharosa.VCrypt.Common.Auth;
using Bharosa.VCrypt.Common.Util;

using IRSWorld.bharosa;

namespace IRSWorld.Admin
{
    public partial class GetImage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BharosaSessionData sessionData = BharosaSessionData.getInstance(Session);
            try
            {

                string pinPadId = Request.QueryString["pid"];
                string imageName = Request.QueryString["loadImage"];

                AuthentiPad pad = null;

                if (pinPadId != null)
                {
                    pad = (AuthentiPad)sessionData[pinPadId];
                }

                Response.CacheControl = "no-cache"; //HTTP 1.1
                Response.Expires = 0; //prevents caching at the proxy server

                if (imageName != null)
                {
                    string outputType = "png";
                    Response.ContentType = "image/" + outputType;

                    string skinDir = BharosaConfig.get("bharosa.authentipad.images.dir", "alphapad_bg");
                    string imageFilePath = skinDir + "/" + imageName;

                    FileInfo iconFile = KeyPadUtil.getFile(imageFilePath);
                    KeyPadUtil.encryptImageToStream(iconFile, outputType, Response.OutputStream);

                }
                else if (pad != null)
                {
                    Response.ContentType = "image/" + pad.OutputType;
                    KeyPadUtil.encryptImageToStream(pad, Response.OutputStream);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}