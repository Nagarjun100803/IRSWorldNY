function fl_page_load()
{
    var iframe = document.createElement("iframe");

        // Point the IFRAME to GenerateFile, with the
        //   desired region as a querystring argument.
        iframe.src = "FileDownloader.aspx";

        // This makes the IFRAME invisible to the user.
        iframe.style.display = "none";

        // Add the IFRAME to the page.  This will trigger
        //   a request to GenerateFile now.
        document.body.appendChild(iframe);
}

function file_downloader()
{
    this.begin_download = function()
    {
        var iframe = document.createElement("iframe");

        // Point the IFRAME to GenerateFile, with the
        //   desired region as a querystring argument.
        iframe.src = "FileDownloader.aspx";

        // This makes the IFRAME invisible to the user.
        iframe.style.display = "none";

        // Add the IFRAME to the page.  This will trigger
        //   a request to GenerateFile now.
        document.body.appendChild(iframe); 
    }
}