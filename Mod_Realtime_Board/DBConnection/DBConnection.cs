using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;
using System.Data;
using System.Net;

namespace Mod_Realtime_Board.DBConnection
{
    public class DBConnection
    {
        private OracleConnection Connection;
        public DBConnection(string tns)
        {
            string ds = "";
            switch (tns)
            {
                case "PHARTS_PHAMR":
                    ds = "data source =( DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST =  10.178.1.64)(PORT = 1521)) (CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = phamr) ) );user id=PHARTS;password=PHARTS2015;Pooling=false";
                    break;
                case "PHAMWDA1_PHAMR":
                    ds = "data source =( DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST =  10.178.1.64)(PORT = 1521)) (CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = phamr) ) );user id=PHAMWDA1;password=PHAMWDA123;Pooling=false";
                    break;
                case "NBHRMSDA1":
                    ds = "data source =(DESCRIPTION=    (LOAD_BALANCE=yes)    (ADDRESS_LIST=      (ADDRESS=        (PROTOCOL=TCP)        (HOST=10.189.128.65)        (PORT=1521)      )      (ADDRESS=        (PROTOCOL=TCP)        (HOST=10.189.128.66)        (PORT=1521)      )    )    (CONNECT_DATA=      (FAILOVER_MODE=        (TYPE=select)        (METHOD=basic)        (RETRIES=180)        (DELAY=5)      )      (SERVER=dedicated)      (SERVICE_NAME=phaoa)    )  );user id=NBHRMSDA1;password=NBHRMSDA123;Pooling=false";
                    break;
                case "PHBRTS_PHBMR_NEW":
                    ds = "data source =(DESCRIPTION=   (ADDRESS=     (PROTOCOL=TCP)     (HOST=10.178.3.69)     (PORT=1521)   )   (CONNECT_DATA=     (SERVER=dedicated)     (SERVICE_NAME=PHBMR)   ) );user id=PHBRTS;password=PHBRTS2015;Pooling=false";
                    break;
                case "PHBMWDA1_PHBMR_NEW":
                    ds = "data source =(DESCRIPTION=   (ADDRESS=     (PROTOCOL=TCP)     (HOST=10.178.3.69)     (PORT=1521)   )   (CONNECT_DATA=     (SERVER=dedicated)     (SERVICE_NAME=PHBMR)   ) );user id=PHBMWDA1;password=PHBMWDA2015;Pooling=false";
                    break;
                case "PHCRTS_PHBUR":
                    ds = "data source = (DESCRIPTION=  (ADDRESS=(PROTOCOL=TCP)(HOST=10.178.3.62) (PORT=1521)  ) (CONNECT_DATA=  (SERVER=dedicated) (SERVICE_NAME=phbur) ) );user id=PHCRTS;password=PHCRTS2015;Pooling=false";
                    break;
                case "PHAAMWDA1_PHBUR":
                    ds = "data source = (DESCRIPTION=  (ADDRESS=(PROTOCOL=TCP)(HOST=10.178.3.62) (PORT=1521)  ) (CONNECT_DATA=  (SERVER=dedicated) (SERVICE_NAME=phbur) ) );user id=PHAAMWDA1;password=PHAAMWDA123;Pooling=false";
                    break;
                case "PHACRTS_PHACR":
                    ds = "data source = (DESCRIPTION=  (ADDRESS=  (PROTOCOL=TCP)  (HOST=10.178.1.68) (PORT=1521)   ) (CONNECT_DATA=   (SERVER=dedicated)  (SERVICE_NAME=PHACR)  ));user id=PHACRTS;password=PHACRTS2015;Pooling=false";
                    break;
                case "BLCMESRPT_PHACR":
                    ds = "data source = (DESCRIPTION=  (ADDRESS=  (PROTOCOL=TCP)  (HOST=10.178.1.68) (PORT=1521)   ) (CONNECT_DATA=   (SERVER=dedicated)  (SERVICE_NAME=PHACR)  ));user id=BLCMESRPT;password=BLCMESRPT123;Pooling=false";
                    break;
                case "PHBCRTS_PHACR":
                    ds = "data source = (DESCRIPTION=  (ADDRESS=  (PROTOCOL=TCP)  (HOST=10.178.1.68) (PORT=1521)   ) (CONNECT_DATA=   (SERVER=dedicated)  (SERVICE_NAME=PHACR)  ));user id=PHBCRTS;password=PHBCRTS2015;Pooling=false";
                    break;
                case "FMMESRPT_PHACR":
                    ds = "data source = (DESCRIPTION=  (ADDRESS=  (PROTOCOL=TCP)  (HOST=10.178.1.68) (PORT=1521)   ) (CONNECT_DATA=   (SERVER=dedicated)  (SERVICE_NAME=PHACR)  ));user id=FMMESRPT;password=FMMESRPT123;Pooling=false";
                    break;
                case "PHCCRTS_PHACR":
                    ds = "data source = (DESCRIPTION=  (ADDRESS=  (PROTOCOL=TCP)  (HOST=10.178.1.68) (PORT=1521)   ) (CONNECT_DATA=   (SERVER=dedicated)  (SERVICE_NAME=PHACR)  ));user id=PHCCRTS;password=PHCCRTS2015;Pooling=false";
                    break;
                case "PHALWDA1_PHALR":
                    ds = "data source = (DESCRIPTION= (ADDRESS= (PROTOCOL=TCP)  (HOST=10.178.1.63) (PORT=1521) ) (CONNECT_DATA=    (SERVER=dedicated)   (SERVICE_NAME=phalr) ) );user id=PHALWDA1;password=PHALWDA123;Pooling=false";
                    break;
                case "PHALTS_PHALR":
                    ds = "data source = (DESCRIPTION= (ADDRESS= (PROTOCOL=TCP)  (HOST=10.178.1.63) (PORT=1521) ) (CONNECT_DATA=    (SERVER=dedicated)   (SERVICE_NAME=phalr) ) );user id=PHALTS;password=PHALTS2015;Pooling=false";
                    break;
                case "PHBLWDA1_PHBLR":
                    ds = "data source = (DESCRIPTION= (ADDRESS= (PROTOCOL=TCP)  (HOST=10.178.1.70) (PORT=1521) ) (CONNECT_DATA=    (SERVER=dedicated)   (SERVICE_NAME=PHBLR) ) );user id=PHBLWDA1;password=PHBLWDA123;Pooling=false";
                    break;
                case "PHBLTS_PHBLR":
                    ds = "data source = (DESCRIPTION= (ADDRESS= (PROTOCOL=TCP)  (HOST=10.178.1.70) (PORT=1521) ) (CONNECT_DATA=    (SERVER=dedicated)   (SERVICE_NAME=PHBLR) ) );user id=PHBLTS;password=PHBLTS2015;Pooling=false";
                    break;
                case "PHAALWDA1_PHBUR":
                    ds = "data source = (DESCRIPTION=  (ADDRESS=(PROTOCOL=TCP)(HOST=10.178.3.62) (PORT=1521)  ) (CONNECT_DATA=  (SERVER=dedicated) (SERVICE_NAME=phbur) ) );user id=PHAALWDA1;password=PHAALWDA123;Pooling=false";
                    break;
                case "PHAAMSDA1_PHBUR"://wuyan
                    ds = "data source = (DESCRIPTION=  (ADDRESS=(PROTOCOL=TCP)(HOST=10.178.3.62) (PORT=1521)  ) (CONNECT_DATA=  (SERVER=dedicated) (SERVICE_NAME=phbur) ) );user id=PHAAMSDA1;password=PHAAMSDA123;Pooling=false";
                    break;
                case "PHSUMDA1_PHARS":
                    ds = "data source = (DESCRIPTION=(LOAD_BALANCE=yes)(ADDRESS_LIST=(ADDRESS=  (PROTOCOL=TCP)  (HOST=10.189.128.60)  (PORT=1521))(ADDRESS=  (PROTOCOL=TCP)  (HOST=10.189.128.59)  (PORT=1521)))(CONNECT_DATA=(FAILOVER_MODE=  (TYPE=select)  (METHOD=basic)  (RETRIES=180)  (DELAY=5))(SERVER=dedicated)(SERVICE_NAME=phars)));user id=PHSUMDA1;password=PHSUMDA2016;Pooling=false";
                    break;
                case "PHBBMSDA1_PHARS"://wuyan
                    ds = "data source = (DESCRIPTION=(LOAD_BALANCE=yes)(ADDRESS_LIST=(ADDRESS=  (PROTOCOL=TCP)  (HOST=10.189.128.60)  (PORT=1521))(ADDRESS=  (PROTOCOL=TCP)  (HOST=10.189.128.59)  (PORT=1521)))(CONNECT_DATA=(FAILOVER_MODE=  (TYPE=select)  (METHOD=basic)  (RETRIES=180)  (DELAY=5))(SERVER=dedicated)(SERVICE_NAME=phars)));user id=PHBBMSDA1;password=PHBBMSDA2015;Pooling=false";
                    break;
                case "PHAMSDA1_PHARS"://wuyan
                    ds = "data source = (DESCRIPTION=(LOAD_BALANCE=yes)(ADDRESS_LIST=(ADDRESS=  (PROTOCOL=TCP)  (HOST=10.189.128.60)  (PORT=1521))(ADDRESS=  (PROTOCOL=TCP)  (HOST=10.189.128.59)  (PORT=1521)))(CONNECT_DATA=(FAILOVER_MODE=  (TYPE=select)  (METHOD=basic)  (RETRIES=180)  (DELAY=5))(SERVER=dedicated)(SERVICE_NAME=phars)));user id=PHAMSDA1;password=PHAMSDA321;Pooling=false";
                    break;
                case "PHALSAP1_PHARS"://wuyan
                    ds = "data source = (DESCRIPTION=(LOAD_BALANCE=yes)(ADDRESS_LIST=(ADDRESS=  (PROTOCOL=TCP)  (HOST=10.189.128.60)  (PORT=1521))(ADDRESS=  (PROTOCOL=TCP)  (HOST=10.189.128.59)  (PORT=1521)))(CONNECT_DATA=(FAILOVER_MODE=  (TYPE=select)  (METHOD=basic)  (RETRIES=180)  (DELAY=5))(SERVER=dedicated)(SERVICE_NAME=phars)));user id=PHALSAP1;password=lcdplsum;Pooling=false";
                    break;
                case "PHASWDA1_PHAMR":
                    ds = "data source =( DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST =  10.178.1.64)(PORT = 1521)) (CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = phamr) ) );user id=PHASWDA1;password=PHASWDA2016;Pooling=false";
                    break;
                case "PHBSWDA1_PHBMR_NEW"://LCM2 整機
                    ds = "data source =(DESCRIPTION=   (ADDRESS=     (PROTOCOL=TCP)     (HOST=10.178.3.69)     (PORT=1521)   )   (CONNECT_DATA=     (SERVER=dedicated)     (SERVICE_NAME=PHBMR)   ) );user id=PHBSWDA1;password=PHBSWDA12017;Pooling=false";
                    break;
                case "NBHRMSDA1_phaoadb"://派工DB
                    ds = @"Data Source=  (DESCRIPTION =
    (ADDRESS_LIST =
      (ADDRESS = (PROTOCOL = TCP)(HOST = 10.189.128.66)(PORT = 1521))
    )
    (CONNECT_DATA =
      (SERVER = dedicated)
      (SERVICE_NAME = phaoa)
    )
  );User ID=NBHRMSDA1;Password=NBHRMSDA123;Pooling=true;Max Pool Size=5";
                    break;
                default:
                    break;
            }
            SetConn(ds);
        }
        public void SetConn(string conn)
        {
            Connection = new OracleConnection(conn);
        }
        /*打開鏈接*/
        public void OpenConn()
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
        }
        /*關閉鏈接*/
        public void CloseConn()
        {
            if (Connection.State != ConnectionState.Closed)
                Connection.Clone();
        }
        /*返回一句select語句結果集合*/
        public DataSet ExcuteSingleQuery(string sql)
        {
            DataSet dataset = new DataSet();
            Connection.Open();
            try
            {
                OracleDataAdapter OraDA = new OracleDataAdapter(sql, Connection);
                OraDA.Fill(dataset);
                return dataset;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataset.Dispose();
                Connection.Dispose();
                Connection.Close();
            }
        }
        /*返回帶參數的sql的結果集合*/
        public DataSet ExcuteSingleQueryParam(string sql, OracleParameter[] parameter)
        {
            DataSet dataset = new DataSet();
            Connection.Open();
            try
            {
                OracleDataAdapter OraDA = new OracleDataAdapter(sql, Connection);
                OraDA.SelectCommand.BindByName = true;
                if (parameter != null)
                {
                    foreach (var param in parameter)
                    {
                        OraDA.SelectCommand.Parameters.Add(param);
                    }
                }
                OraDA.Fill(dataset);
                return dataset;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataset.Dispose();
                Connection.Dispose();
            }
        }
        /*执行SQL语句，返回影响的记录数*/
        public int ExecuteSql(string SQLString,OracleParameter[] parameter)
        {

            Connection.Open();
            try
            {
                OracleCommand cmd = new OracleCommand(SQLString, Connection);
                foreach (var param in parameter)
                {
                    cmd.Parameters.Add(param);
                }
                int rows = cmd.ExecuteNonQuery();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception("ExecuteSql" + ex.Message);
            }
            finally
            {
                Connection.Dispose();
            }


        }


        public int ExecuteSingleSql(string SQLString)
        {

            Connection.Open();
            try
            {
                OracleCommand cmd = new OracleCommand(SQLString, Connection);
                int rows = cmd.ExecuteNonQuery();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception("ExecuteSql" + ex.Message);
            }
            finally
            {
                Connection.Dispose();
            }


        } 

        /// <summary>
        /// 記錄點擊日誌
        /// </summary>
        /// <param name="fab">廠別</param>
        /// <param name="report_name">報表名稱</param>
        /// <param name="url">網址</param>
        /// <param name="auto">看板or查詢報表</param>
        /// <param name="user">賬號</param>
        public static void call_Vlog(string fab, string report_name, string url, string auto, string user)
        {
            //string user = Request.ServerVariables["LOGON_USER"].Split('\\').Count() > 1 ? Request.ServerVariables["LOGON_USER"].Split('\\')[1].ToLower() : Request.UserHostName;
            string result = "";
            using (var client = new WebClient())
            {
                string json = "上傳數據";
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                result = client.UploadString("http://10.189.128.9/LOG_Report/Home/InsertLog.aspx?FAB=" + fab + "&REPORT_NAME=" + report_name + "&URL=" + url + "&OPERATORS=" + user + "&AUTO=" + auto + "", "POST", json);
            }
        }


    }
}