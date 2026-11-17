using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace IRSClassLib
{
    [Serializable]
    public class irsForexUpload
    {
        private String constring_suffix = "FXRatesConnectionString";
        private irsSqlCommonsWs fxcom;
        public String Message;

        public enum RateTypes
        {
            FX_PHP,
            FX_USD,
            BookingRate
        }

        private String getTablename(RateTypes rtype)
        {
            return (rtype.Equals(RateTypes.BookingRate)) ? "brate" : "USDPHP";
        }

        private ArrayList getFields(RateTypes rtype)
        {
            String sqlstring = "SELECT name FROM syscolumns WHERE [id] = OBJECT_ID('" + getTablename(rtype) + "')";
            DataSet ds = fxcom.getDataSet(sqlstring, false, new object[] { }); //(fxcom.getSqlCommand(sqlstring, false));
            if (ds != null & ds.Tables.Count > 0)
            {
                ArrayList al = new ArrayList();
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    String str = dt.Rows[i]["name"].ToString();
                    if (rtype.Equals(RateTypes.BookingRate) & str.Length == 4) str = str.Substring(0, 3);
                    else if (rtype.Equals(RateTypes.FX_USD) & (str.Length == 4 & str.Substring(0, 1).Equals("s"))) str = str.Substring(1);
                    else if (rtype.Equals(RateTypes.FX_PHP) & ((str.StartsWith("N_") | str.StartsWith("TD_") | str.StartsWith("S_")) & str.EndsWith("_PHP")))
                    {
                        if (str.StartsWith("TD_")) str = str.Substring(3, 3);
                        else str = str.Substring(2, 3);
                    }
                    else continue;

                    if (al.Contains(str)) continue;

                    al.Add(str);
                }

                if (rtype.Equals(RateTypes.FX_USD))
                {
                    if (!al.Contains("USD")) al.Add("USD");
                    if (!al.Contains("PHP")) al.Add("PHP");
                }

                return al;
            }

            return null;
        }


        private long getMaxId(String sqlstring)
        {
            DataSet ds = fxcom.getDataSet(sqlstring, false, new object[] { }); //(fxcom.getSqlCommand(sqlstring, false));
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt64((decimal)ds.Tables[0].Rows[0]["ID"]);
                }

                return -2; // for try.
            }
            Message = fxcom.Message;
            return -1; // db error
        }

        private Object[] getMaxId(RateTypes rtype, DateTime vdate)
        {
            String sqlstring = "SELECT MAX(ID) as ID, CONVERT(varchar, value_date, 101) as value_date";
            if (rtype.Equals(RateTypes.FX_USD) | rtype.Equals(RateTypes.FX_PHP))
            {
                sqlstring += ", MAX(asof) AS asof";
            }

            String tbl_name = getTablename(rtype);

            sqlstring += " FROM " + tbl_name + " WHERE value_date = '"; // +vdate.ToString("MM/dd/yyyy") + "' Group by value_date;";
            
            DateTime xvdate = vdate;
            String s = sqlstring + xvdate.ToString("MM/dd/yyyy") + "' Group by value_date;";

            long l = getMaxId(s);
            if (l > -2) return new Object[] { l, xvdate };

            for (int i = 0; i < 7; i++)
            {
                xvdate = vdate.AddDays( (i + 1) * -1 );
                s = sqlstring + xvdate.ToString("MM/dd/yyyy") + "' Group by value_date;";
                l = getMaxId(s);
                if (l > -2) break;
            }

            s = "SELECT max(value_date) FROM " + tbl_name;
            Object o = fxcom.execScalar(s);
            if (o != null && (o != DBNull.Value || o.ToString().IndexOf("DBNull") == -1))
            {
                xvdate = Convert.ToDateTime(o);
                s = sqlstring + xvdate.ToString("MM/dd/yyyy") + "' Group by value_date;";
                l = getMaxId(s);
            }

            if (l == -2)
            {
                Message = "No rates in Cybermag.";
            }

            return new Object[] { l, xvdate };
        }

        private DataRow getRates(RateTypes rtype, DateTime vdate)
        {
            Object[] oo = getMaxId(rtype, vdate);
            if (oo == null) return null;
            long maxid = (long)oo[0];
            DateTime xvdate = (DateTime)oo[1];
            if (maxid < 0) return null;

            String sqlstring = 
                "SELECT * FROM " + getTablename(rtype) + " WHERE ID = " + maxid + " " +
                "AND value_date = '" + xvdate.ToString("MM/dd/yyyy") + "'";
            DataSet ds = fxcom.getDataSet(fxcom.getSqlCommand(sqlstring, false));
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0) return ds.Tables[0].Rows[0];
            }

            return null;
        }

        private DataTable getBookingTable()
        {
            DataTable dt = new DataTable("BMRate");
            dt.Columns.Add(new DataColumn("SEQ_ID", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("VALUE_DATE", typeof(System.DateTime)));
            dt.Columns.Add(new DataColumn("CURRENCY_CODE", typeof(System.String)));
            dt.Columns.Add(new DataColumn("BOOKING_RATE", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("MID_RATE", typeof(System.Decimal)));

            return dt;
        }

        public DataTable getBookingRates(DateTime vdate)
        {
            ArrayList bnames = getFields(RateTypes.BookingRate);
            DataRow rate = getRates(RateTypes.BookingRate, vdate);
            DataTable dt = getBookingTable();

            decimal usdp = -1M;
            int i = 0;

            if (rate != null)
            {
                for (i = 0; i < bnames.Count; i++)
                {
                    String curr_cd = bnames[i].ToString();
                    DataRow uprow = dt.NewRow();
                    uprow["SEQ_ID"] = i + 1;
                    uprow["VALUE_DATE"] = rate["value_date"]; //vdt;
                    uprow["CURRENCY_CODE"] = curr_cd;

                    String currp = curr_cd + "p";

                    uprow["BOOKING_RATE"] = rate[currp]; //rate["USDp"];
                    if (currp.ToUpper().Equals("USDP"))
                    {
                        usdp = Convert.ToDecimal(rate[currp]);
                    }

                    String sparam = curr_cd + "u";
                    if (sparam.Equals("USDu"))
                    {
                        uprow["MID_RATE"] = 1.0000M;
                    }
                    else
                    {
                        uprow["MID_RATE"] = rate[sparam];
                    }
                    dt.Rows.Add(uprow);
                }
                if (usdp > -1M)
                {
                    DataRow phprow = dt.NewRow();
                    phprow["SEQ_ID"] = i;
                    phprow["VALUE_DATE"] = rate["value_date"]; //vdt;
                    phprow["CURRENCY_CODE"] = "PHP";
                    phprow["BOOKING_RATE"] = 1.0000M;
                    phprow["MID_RATE"] = Math.Round(1.0000M / usdp, 6);
                    dt.Rows.Add(phprow);
                }
            }
            else
            {
                Message += "MidRate: " + fxcom.Message + ";";
            }

            return dt;
        }

        private DataTable getForexTable(String tbname)
        {
            DataTable dt = new DataTable(tbname);
            dt.Columns.Add(new DataColumn("SEQ_ID", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("VALUE_DATE", typeof(System.DateTime)));
            dt.Columns.Add(new DataColumn("CURRENCY_CODE", typeof(System.String)));
            dt.Columns.Add(new DataColumn("BUYING_RATE", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("SELLING_RATE", typeof(System.Decimal)));
            //dt.Columns.Add(new DataColumn("GUIDING_RATE", typeof(System.Decimal)));

            return dt;
        }

        private DateTime getDate(Object odt, Object oasof, DateTime vdate)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(odt);
                String stime = oasof.ToString().Trim();
                String st = null;
                if (stime.IndexOf("AM") > -1 | stime.IndexOf("PM") > -1)
                {
                    st = stime.Substring(0, stime.Length - 2) + " " + stime.Substring(stime.Length - 2);
                }
                else
                {
                    st = stime;
                }

                return DateTime.Parse(dt.ToString("MM/dd/yyyy") + " " + st);
            }
            catch { }

            return vdate;
        }

        public DataTable getFxUsdRates(DateTime vdate)
        {
            ArrayList fnames = getFields(RateTypes.FX_USD);
            DataRow rate = getRates(RateTypes.FX_USD, vdate);
            DataTable dt = getForexTable("FXUSD");

            if (rate != null)
            {
                DateTime vdt = getDate(rate["value_date"], rate["asof"], vdate);
                for (int i = 0; i < fnames.Count; i++)
                {
                    String curr_cd = fnames[i].ToString();
                    DataRow uprow = dt.NewRow();
                    uprow["SEQ_ID"] = i + 1;
                    uprow["VALUE_DATE"] = vdt;
                    uprow["CURRENCY_CODE"] = curr_cd;
                    //uprow["GUIDING_RATE"] = rate["PDS_AVE"];

                    if (curr_cd.Equals("USD"))
                    {
                        uprow["BUYING_RATE"] = 1.0000M;
                        uprow["SELLING_RATE"] = 1.0000M;                        
                        //uprow["BUYING_RATE"] = rate["WDR_TT_DDONPNB_BUY"];
                        //uprow["SELLING_RATE"] = rate["LC_SELL"];
                    }
                    else if (curr_cd.Equals("PHP"))
                    {
                        //uprow["BUYING_RATE"] = 0.0000M;
                        //uprow["SELLING_RATE"] = 0.0000M;
                        uprow["BUYING_RATE"] = rate["WDR_TT_DDONPNB_BUY"];
                        uprow["SELLING_RATE"] = rate["LC_SELL"];
                    }
                    else
                    {
                        uprow["BUYING_RATE"] = rate["b" + curr_cd + "_US"];
                        uprow["SELLING_RATE"] = rate["s" + curr_cd + "_US"];
                    }

                    dt.Rows.Add(uprow);
                }
            }
            else
            {
                Message += "FxRate: " + fxcom.Message + ";";
            }

            return dt;
        }

        public DataTable getFxPhpRates(DateTime vdate)
        {
            ArrayList fnames = getFields(RateTypes.FX_PHP);
            DataRow rate = getRates(RateTypes.FX_PHP, vdate);
            DataTable dt = getForexTable("FXPHP");

            if (rate != null)
            {
                DateTime vdt = getDate(rate["value_date"], rate["asof"], vdate);
                for (int i = 0; i < fnames.Count; i++)
                {
                    String curr_cd = fnames[i].ToString();
                    DataRow uprow = dt.NewRow();
                    uprow["SEQ_ID"] = i + 1;
                    uprow["VALUE_DATE"] = vdt;
                    uprow["CURRENCY_CODE"] = curr_cd;

                    Object ttrate = null;
                    try
                    {
                        ttrate = rate["TD_" + curr_cd + "_PHP"];
                    }
                    catch { };
                    if (ttrate == null)
                    {
                        try
                        {
                            ttrate = rate["N_" + curr_cd + "_PHP"];
                        }
                        catch { }
                    }
                    try
                    {
                        uprow["BUYING_RATE"] = ttrate;
                        uprow["SELLING_RATE"] = rate["S_" + curr_cd + "_PHP"];
                    }
                    catch {}

                    dt.Rows.Add(uprow);
                }
            }
            else
            {
                Message += "FxRate: " + fxcom.Message + ";";
            }

            return dt;
        }

        public Object[] getUploadRates(DateTime vdate)
        {
            Message = String.Empty;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(this.getFxPhpRates(vdate));
                ds.Tables.Add(this.getFxUsdRates(vdate));
                ds.Tables.Add(this.getBookingRates(vdate));

                return new Object[] { ds, Message };
            }
            catch (Exception e)
            {
                Message = e.Message;
                Message += "\n" + fxcom.Message;
            }
            return null;
        }

        public irsForexUpload()
        {
            fxcom = new irsSqlCommonsWs(constring_suffix);
            fxcom.IsWinApp = true;
        }

        ~irsForexUpload()
        {
            fxcom = null;
        }
    }
}
