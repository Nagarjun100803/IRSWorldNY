
        /**************************************************************/       
        
        function user_record2()
        {}
              
        var user = user_record2.prototype;
        
        user.setup = function(crud, is_setup, xdays)
        {
            this.is_setup = is_setup;
            this.xdays = xdays;
            this.crud = crud;
            
            this.cifid = null;
            this.userid = null;
            this.fullname = '';
            this.flag = null;
            this.creationDate = new Date();
            this.dateModified = null;
            this.lastAccessDate = null;
            this.lastPwdChangeDate = new Date();
            this.cif_class = null;
            this.user_exist = false; 
            this.rowIndex = 0;
            this.email = ''; 
            
            this.locrole = null;
            this.locrole_changed = null; 
            
            this.password = null; 
            this.pwd = ''; 
               
            this.error = null;
            
            if (crud == 'add')
            {
                var rs = IRSWorld.Admin.UserAdmin2.getEmptyLocRole();
                
                if (rs.error != null)
                {
                    alert("Get Empty LocRole table: " + rs.error.Message);
                    this.error = rs.error.Message;
                }
                else
                    this.locrole = rs.value;            
            }
        }

        
        user.getExpirationDate = function()
        {
            var dt = (this.crud == 'add') ? this.creationDate : this.lastPwdChangeDate;
            var xdt = new Date(dt.getYear(), dt.getMonth(), dt.getDate(), dt.getHours(), dt.getMinutes(), dt.getSeconds());
            xdt.setDate(xdt.getDate() + this.xdays);
            return xdt;
        }
        
        user.setDaysExpired = function(ndays)
        {
            this.error = null;

            if (!isNumeric(ndays))
            {
                this.error = 'Days Expired must be numeric';
                return;
            }
            
            var ndd = parseInt(ndays);
            if (ndd < 1)
            {
                this.error = 'Days Expired must be positive value  > 0.';
                return;
            }
            
            this.xdays = parseInt(ndays);
        }
        
        user.setCifId = function(cifid)
        {
            this.error = null;
            
            if (cifid != null) 
            {
                this.cifid = cifid;
                return true;
            }
            
            this.error = 'CIFID: ' + cifid + ' is required';
            return false;
        }
        
        user.setDdlValue = function(fld, value)
        {
            this.error = null;
            
            if (value == '0') 
            {
                this.error = 'A value must be selected for field ' + fld;
                return false;
            }
            try
            {
                eval('this.' + fld + ' = "' + value + '"');
                return true;
            }
            catch (e)
            {
                this.error = e.description;
            }
            return false;
        }
        
        user.setUserId = function(userid)
        {
            this.error = null;
            
            if (userid == null || userid.length == 0)
            {
                this.error = "UserId cannot be null or empty";
                return false;
            }
            
            this.userid = userid;
            return true;
        }
        
        user.toString = function()
        {
            this.error = null;
            if (this.cifid == null || this.cifid.length < 1) 
            {
                this.error = "CIFID must be supplied";
                return null;
            }
            if (this.userid == null || this.userid.length < 1)
            {
                this.error = "USERID is required";
                return null;
            }
            
            var rec =   this.crud + "~";
            if (this.crud != 'add') 
            {
                rec     +=  this.userid + "~";
                rec     +=  dateToString(this.dateModified) + "~";
            }
            if (this.crud != 'delete')
            {
                rec     +=  "CIFID=" + this.cifid + "~";
                rec     +=  "MODE=" + this.cif_class + "~";
                if (this.crud == 'add')
                    rec +=  "USERID=" + this.userid + "~";
                rec     +=  "LOCATION=" + this.location + "~";
                rec     +=  "DAYSEXPIRED=" + this.xdays + "~";
                rec     +=  "ROLE=" + this.role  + "~";
                if (this.crud != 'add')
                {
                    rec +=  "DATECREATED=" + dateToString(this.creationDate) + "~";
                    rec +=  "LASTPASSWORDCHANGE=" + dateToString(this.lastPwdChangeDate) + "~";
                    rec +=  "FLAGS=" + this.flag  + "~";
                }
                rec     +=  "LoggedOn=" + ((this.logon) ? "true" : "false");
            }
            else
            {
                rec     += this.rowIndex;
            }
            
            return rec;
        }
        
        /* Structure of db_grid.table.Row as returned from the code behind
        
            - Master
            
                DataColumn("CIFID", typeof(System.String)));
                DataColumn("UserID", typeof(System.String)));
                DataColumn("FullName", typeof(System.String)));
                DataColumn("DateCreated", typeof(System.DateTime)));
                DataColumn("LastModified", typeof(System.DateTime)));
                DataColumn("LastAccessDate", typeof(System.DateTime)));
                DataColumn("LastPasswordChange", typeof(System.DateTime)));
                DataColumn("DaysExpired", typeof(System.Int16)));
                DataColumn("DateExpired", typeof(System.DateTime)));
                DataColumn("Mode", typeof(System.Int16)));

            - Detail
                DataColumn("UserID", typeof(System.String)));
                DataColumn("location", typeof(System.String)));
                DataColumn("Role", typeof(System.Int16)));
                DataColumn("LoggedOn", typeof(System.Boolean)));
                DataColumn("setPrimary", typeof(System.Boolean)));
                DataColumn("FlagDesc", typeof(System.String)));
                DataColumn("RoleDesc", typeof(System.String)));
                DataColumn("BranchName", typeof(System.String)));
                DataColumn("hasModuleProfile", typeof(System.Int32)));
		*/
			         
        user.parseRow = function(idx, row, dt_locrol)
        {
            this.setup('query', false, row.DaysExpired);
            this.user_exist = true;
            this.rowIndex = idx;
            
            this.setCifId(row.CIFID);
            this.setUserId(row.UserID);
            this.fullname = row.FullName;
            this.flag = row.Flags;
            this.creationDate = row.DateCreated;
            this.dateModified = row.LastModified;
            this.lastAccessDate = row.LastAccessDate;
            this.lastPwdChangeDate = row.LastPasswordChange;
            this.cif_class = row.Mode;
            
            this.locrole = dt_locrol;
            this.locrole_changed = new Array();
            for (var i = 0; i < dt_locrol.Rows.length; i++)
            {
                this.locrole_changed[i] = false;
            }
        }