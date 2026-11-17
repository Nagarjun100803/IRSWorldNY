
function GRSGEFU()
{
    this.ActiveModule = function (divName)
    {
        GRSGEFUMaintenance.ActiveModule(divName);
    }
    
    this.create_grs_gefu = function(tranxdate)
    {
        GRSGEFUMaintenance.create_grs_gefu(tranxdate, this.create_grs_gefu_callback());
    }
    
    this.create_grs_gefu_callback = function (result)
    {
        alert ("hello");
    }
}