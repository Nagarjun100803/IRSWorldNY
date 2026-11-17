using System;
using System.Collections;
using System.Data;
using System.Text;

using PublicDomain;

namespace IRSClassLib
{
    [Serializable]
    public class irsTimeZone
    {

        public String[] Timezones
        {
            get 
            { 
                return TzTimeZone.AllZoneNames; 
            }
        }

        public String branchTimeZone(String brcode)
        {
            Object brtzo = new irsSqlCommonsWs().execScalar("SELECT TimeZone FROM TBL_BRANCHES WHERE BranchCode = '" + brcode + "'");
            if (brtzo != null && brtzo != DBNull.Value)
            {
                return ((String)brtzo).Trim();
            }

            return String.Empty;
        }

        /// <summary>
        /// Function added by mak.
        /// To be used for non-web apps that use tz functions
        /// </summary>
        /// <param name="brcode"></param>
        /// <returns>TimeZone</returns>
        public String branchTimeZone_nWs(String brcode)
        {
            Object brtzo = new irsSqlCommons().execScalar("SELECT TimeZone FROM TBL_BRANCHES WHERE BranchCode = '" + brcode + "'");
            if (brtzo != null && brtzo != DBNull.Value)
            {
                return ((String)brtzo).Trim();
            }

            return String.Empty;
        }

        public DateTime localTime(String zone_name)
        {
            //DateTime dt1 = TzTimeZone.GetTimeZone(zone_name).Now.DateTimeUtc;
            //DateTime dt2 = TzTimeZone.GetTimeZone(zone_name).Now.DateTimeLocal;
            return TzTimeZone.GetTimeZone(zone_name).Now.DateTimeLocal;
        }

        public DateTime branchLocalTime(String brcode)
        {
            String s = branchTimeZone(brcode);
            if (s.Length > 0)
            {
                return localTime(s);
            }

            throw new Exception ("Time zone is not configured for branch: " + brcode);
        }
        
        /// <summary>
        /// Function added by mak.
        /// To be used for non-web apps that use tz functions
        /// </summary>
        /// <param name="brcode"></param>
        /// <returns>DateTime</returns>
        public DateTime branchLocalTime_nWs(String brcode)
        {
            String s = branchTimeZone_nWs(brcode);
            if (s.Length > 0)
            {
                return localTime(s);
            }

            throw new Exception("Time zone is not configured for branch: " + brcode);
        }

        public DateTime tzConvert(DateTime dt, String from_tz, String to_tz)
        {
            TzTimeZone tzFrom = TzTimeZone.GetTimeZone(from_tz);
            TzTimeZone tzTo = TzTimeZone.GetTimeZone(to_tz);
            DateTime utc = tzFrom.ToUniversalTime(dt);
            DateTime dtval = tzTo.ToLocalTime(utc);

            return dtval;
        }

        public String ServerTimezone
        {
            get
            {
                return "Asia/Taipei";
                /*
                String s = branchTimeZone("MAIN");
                if (s.Length == 0)
                {
                    s = branchTimeZone("1170"); // ORD
                }

                return s;
                 * */
            }
        }

        public DateTime ServerTime
        {
            get
            {
                return localTime(ServerTimezone);
            }
        }

    }

}
