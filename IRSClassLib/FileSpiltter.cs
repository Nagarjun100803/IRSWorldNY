using System;
using System.Data;
using System.Configuration;
using System.Web;

using System.Data.SqlClient;
using System.Collections;
//using GRSFileLoaderLib;


namespace IRSClassLib
{
    public class FileSpiltter 
    {
         private splitter_ui_code _do_count = null;
            string str_split = string.Empty;

        protected void load(string str_split, string path)
        {

            try
            {
                 // button of browse or split file

                if (str_split == "summary")
                {
                    this._do_count.PUNLA_summary(path);

                }
                else if (str_split == "split_file")
                {
                    this._do_count.PUNLA_split_file(path);

                }
            }

            //catch (Exception e)
            //{
            //    //load_errors.Add(e.Message.ToString());
            //}
            finally
            {
                //if (this._do != null) { this._do = null; }

            }
        }
   
    }
}
