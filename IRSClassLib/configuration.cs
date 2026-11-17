using System;
using System.Collections;
using System.Configuration;
using System.Text;

namespace IRSClassLib
{
    public class configuration
    {
        public String machine_name
        {
            get { return System.Environment.MachineName; }
        }

        public String appsetting(String key)
        {
            try
            {
                return (String) new AppSettingsReader().GetValue(key, typeof(System.String));
            }
            catch
            {}

            return null;
        }
    }
}
