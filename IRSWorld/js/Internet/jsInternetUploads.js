var upload = new upload_Documents()

function upload_Documents()
{
   this.file1 = "";
   this.file2 = "";
   this.file3 = "";
   this.cifid = "";
   this.uid = "";
   
   this.uploadDocuments = function()
   {
        this.file1 = document.getElementById("File1").value;
        this.file2 = document.getElementById("File2").value;
        this.file3 = document.getElementById("File3").value;
        
        if(this.file1 == "" && this.file2 == "" && this.file3 == "")
        {
            alert("Please select file to upload")
        }
        else
        {
            IRSWorld.Internet.InternetUploads.upload_Documents(upload.createList(),this.uploadDocuments_call_back)
        }s
   } 
   
   
   this.uploadDocuments_call_back = function(result)
   {
        var value = result.value;
        if(value != "")
        {
            alert(value);
        }
        else
        {
            alert("Documents Succesfully Uploaded to Server.");
            upload.resetAll("File1");
            upload.resetAll("File2");
            upload.resetAll("File3");
            document.getElementById("btnUploadFile").disabled = true;
            document.getElementById("File1").disabled = true;
            document.getElementById("File2").disabled = true;
            document.getElementById("File3").disabled = true;
        }
   }
   
   this.createList = function()
   {
        var list = new Array();
        list[list.length] = this.file1;
        list[list.length] = this.file2;
        list[list.length] = this.file3;
        
        return list;
   }
   
   this.clearAll = function()
   {
        if(confirm("Are you sure you want to cancel?."))
        {
            close_frame_dialog();
        }
//        document.getElementById("File1").innerHTML  = "";
//        document.getElementById("File2").innerHTML  = "";s
//        document.getElementById("File3").innerHTML  = "";
   }
   
   this.resetAll = function(id)
   {
       var file = document.getElementById(id);
       file.select();
       k=file.createTextRange();
       k.execCommand("delete");
       //file.focus();
    }
}
