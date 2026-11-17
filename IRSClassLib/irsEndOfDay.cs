using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
namespace IRSClassLib
{
    [Serializable]
    public class irsEndOfDay : sqlHelperWs
    {
        #region variables & properties
        private string _messages;
        private csAuditLog oLog = new csAuditLog();
        private string sysid = "irsEndOfDay";
        private string _uid;
        private string _pwd;
        private string _servernm;
        private string _dbnm;
        private string rptpath;
        public string messages
        {
            get { return _messages; }
            set { _messages = value; }

        }
        #endregion variables & properties
        #region public methods
        public void update_txn(object[] ob)
        {
            object _o;

            try
            {
                _o = base.execScalar("sp_irsAgent_update_for_credit", ob);
                if (_o == null)
                { messages = base.Message; }
            }
            catch (Exception er)
            {
                oLog.writeAudit(sysid, "fccredit.update_txn", er.Message.ToString(),"" ,
                                  "", csAuditLog.Category.FlexCube);
                throw er;
            }
        }

        public ReportDocument getReportDataSet(string rptnm, DataSet ds, DateTime from, DateTime to,string branchcode)
        {
            CrystalDecisions.CrystalReports.Engine.ReportDocument repDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            
            try
            {
                string rootpath = new irsConfiguration().appsetting("ReportPath");
                this.rptpath = rootpath  + ("//IRSWorld_Setup//" + rptnm).Replace(@"/", @"\");
                repDoc.Load(this.rptpath);
                repDoc.SetDataSource(ds.Tables[0]);
                

                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();

                ds1 = this.getbenecharge(branchcode, from);
                ds2 = this.getforexrate(branchcode, from);


                repDoc.SetDatabaseLogon(this._uid, this._pwd, this._servernm, this._dbnm);

                repDoc.Subreports[0].SetDataSource(ds1.Tables[0]);

                repDoc.Subreports[0].SetDatabaseLogon(this._uid, this._pwd, this._servernm, this._dbnm);

                

                repDoc.Subreports[1].SetDataSource(ds2.Tables[0]);

                repDoc.Subreports[1].SetDatabaseLogon(this._uid, this._pwd, this._servernm, this._dbnm);

                
                
                return repDoc;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public ReportDocument generate_report(string values, string rptnm, string spnm)
        {
            string[] param = values.Split(',');
            try
            {
                DataSet ds = execStoredProc(spnm, param);
                return getReportDataSet(rptnm, ds,Convert.ToDateTime(param[2]),Convert.ToDateTime(param[3]),param[1]);
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }
        public DataSet execStoredProc(string spname, string[] param)
        {
            DataSet ds = execStoredProcedure(spname, param);
            return ds;
        }
        public bool isokforload()
        {
            csFileIo io = new csFileIo();
            try
            {
                return io.exist(this.rptpath);
            }
            catch { return false; }
            finally { io = null; }
        }
        public irsEndOfDay()
        {
        }
        public irsEndOfDay(string uid, string pwd, string servernm, string dbnm)
        {
            _uid = uid;
            _pwd = pwd;
            _servernm = servernm;
            _dbnm = dbnm;
        }

        ~irsEndOfDay()
        {
        }

        public DataSet getbenecharge(string branchcode, DateTime tranxdate )
        {
            string param = tranxdate.ToString("MM/dd/yyyy") + "," + branchcode;
            return this.execStoredProc("proc_BeneCharges", param.Split(','));

        
        }

        public DataSet getforexrate(string branchcode, DateTime tranxdate)
        {
         
            string param = tranxdate.ToString("MM/dd/yyyy") + "," + branchcode;
            return this.execStoredProc("rpt_EODForexRates", param.Split(','));

        }
        #endregion public methods

    }
}
