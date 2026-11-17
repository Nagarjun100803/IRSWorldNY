
        /**************************************************************/       
        function user_record()
        {}
              
        var user = user_record.prototype;
        
        user.setup = function(is_setup, xdays)
        {
            this.is_setup = is_setup;
            this.xdays = xdays;
            this.crud = 'add';
            this.creationDate = new Date();
            this.lastPwdChangeDate = new Date();
            this.dateModified = null;
            this.cifid = null;
            this.cif_class = null;
            this.userid = null;
            this.error = null;
            this.fullname = null; 
            this.location = '0';
            this.role = null;
            this.flag = null;
            this.logon = false;
            this.login_group = 1;
            this.login_orig_group = this.login_group;
            this.group_changed = false;
            this.user_exist = false; 
            this.password = null; 
            this.rowIndex = 0;
            this.email = ''; 
            this.fullname = '';  
            this.pwd = ''; 
            this.lastAccessDate = null;
            this.hasModProfile = false;   
            this.maxUserIdLength = 16;  
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
            
            if (cifid != null) //&& cifid.length > 0)
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
            
            if (this.crud == 'add' && userid.length > this.maxUserIdLength)
            {
                this.error = "UserId length cannot be greater than " + this.maxUserIdLength;
                return false;
            }
            
            this.userid = userid;
            return true;
        }
        
        user.setOrigGroup = function(gval)
        {
            this.login_group = gval;
            this.login_orig_group = gval;
            this.group_changed = false;
        }
        
        user.setLoginGroup = function(gval)
        {
            if (gval != this.login_orig_group)
            {
                this.login_group = gval;
                this.group_changed = true;
            }
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
                if (this.group_changed)
                {
                    rec     +=  "~LoginGroup=" + this.login_group;
                }
            }
            else
            {
                rec     += this.rowIndex;
            }
            
            return rec;
        }
        
        /* Structure of db_grid.table.Row as returned from the code behind
		        u.CIFID, 
				u.UserID, 
				u.location, 
				u.DateCreated, 
				u.LastModified, 
				u.LastAccessDate, 
				u.LastPasswordChange, 
				u.DaysExpired, 
				u.Role, 
				u.Flags, 
				u.Mode, 
				u.LoggedOn, 
				RTRIM(m.LastName + ISNULL('','' + m.FirstName, '' '') + ISNULL('' '' + m.MiddleName, '' '')) AS FullName,
				l.FldCode AS FlagDesc, 
				r.FldCode AS RoleDesc, 
				b.BranchName, 
				DateAdd(day, u.DaysExpired, u.LastPasswordChange) as DateExpired,
				u.LoginGroup
		*/
			         
        user.parseRow = function(idx, row)
        {
            this.setup(false, row.DaysExpired);
            this.crud = 'query';
            this.setCifId(row.CIFID);
            this.setUserId(row.UserID);
            this.location = row.location;
            this.cif_class = row.Mode;
            this.role = row.Role;
            this.creationDate = row.DateCreated;
            this.lastPwdChangeDate = row.LastPasswordChange;
            this.dateModified = row.LastModified;
            this.flag = row.Flags;
            this.logon = row.LoggedOn;
            this.user_exist = true;
            this.fullname = row.FullName;
            this.rowIndex = idx;
            this.lastAccessDate = row.LastAccessDate;
            this.setOrigGroup(parseInt(row.LoginGroup));
        }