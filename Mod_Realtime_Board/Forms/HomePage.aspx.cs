using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Newtonsoft.Json;
using Oracle.DataAccess.Client;

namespace Mod_Realtime_Board.Forms
{
    public partial class HomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string flag = Request.QueryString["flag"];//flag
            if (flag == "hourOutput") {
                string process = Request.QueryString["process"];//製程段
                string line = Request.QueryString["line"];
                loadHourOutput(process, line);
            }
            else if (flag == "line")
            {
                string process = Request.QueryString["process"];//製程段
                loadLineData(process);
            }else if(flag=="totalOutput"){
                string process = Request.QueryString["process"];//製程段
                string line = Request.QueryString["line"];
                loadTotalOutput(process, line);
            }
            else if (flag == "rate") {
                string process = Request.QueryString["process"];//製程段
                string line = Request.QueryString["line"];
                loadRate(process,line);
            }
            else if (flag == "pgQty") {
                string process = Request.QueryString["process"];//製程段
                string line = Request.QueryString["line"];
                loadPerson(process, line);
            }
        }

        //獲取派工人數
        private void loadPerson(string process,string line)
        {
            string shift = string.Empty;
            string OPER = string.Empty;
            string pgDate = string.Empty;

            int a = DateTime.Compare(DateTime.Parse("08:00"), DateTime.Now);
            int b = DateTime.Compare(DateTime.Parse("20:00"), DateTime.Now);

            if (a < 0 && b > 0)
            {
                shift = "白班";
                pgDate ="trunc(sysdate)";
            }
            else if (b < 0)
            {
                shift = "晚班";
                pgDate = "trunc(sysdate)";
            }
            else if (a > 0)
            {
                shift = "晚班";
                pgDate = "trunc(sysdate-1)";
            }


string sql = @"select distinct nvl(a.OLD_NEED,0)OLD_NEED,nvl(b.QTY,0)QTY from(
select WORK_DATE,FAB, replace(replace(replace(STATION_NAME,'FTP','CDKEN'),'TLAM','LAM'),'PA','PZAT') STATION_NAME,
case when STATION_NAME='ASSY' then substr(PROD_LINE,5,2) else  substr(PROD_LINE,length(PROD_LINE)-1,2) end PROD_LINE ,
 OLD_NEED, WORK_TYPE from(
SELECT 
    work_date,
    fab,
    upper(station_name)station_name,
    prod_line,
    old_need,
    WORK_TYPE
FROM
    pg_person_online
    WHERE
    fab='NHA-LCM3'
    AND work_date =" + pgDate + @"
    union all
    select   work_date,
    fab,
   upper(station_name)station_name,
    LINE,
    old_need,
    WORK_TYPE from PG_MC_ONLINE
    WHERE
    fab in( 'NHA-LCM3','NHA-LCD3')
    AND work_date =" + pgDate + @"
    )
    )a left join (
    SELECT
    work_date,
    pg_fab,
    replace(replace(replace(upper(pg_station_name),'FTP','CDKEN'),'PA','PZAT'),'TLAM','LAM') pg_station_name,
    case when upper(pg_station_name)='ASSY' then substr(PROD_LINE,5,2) else  substr(PROD_LINE,length(PROD_LINE)-1,2) end PROD_LINE ,
    PG_WORK_TYPE,
    COUNT(emp_name) qty
FROM
    hr_pg_basic
WHERE
    pg_fab in('NHA-LCM3','NHA-LCD3')
    AND work_date =" + pgDate + @"
GROUP BY
    work_date,
    pg_fab,
    pg_station_name,
    prod_line,
    PG_WORK_TYPE
    )b on a.WORK_DATE=b.WORK_DATE and a.FAB=b.PG_FAB 
    and a.STATION_NAME=b.PG_STATION_NAME
    and a.PROD_LINE=b.PROD_LINE
    and a.WORK_TYPE =b.PG_WORK_TYPE
    where a.STATION_NAME='" + process + @"' and a.PROD_LINE='" + line + @"' and a.WORK_TYPE='" + shift + @"'";

            DBConnection.DBConnection conn = new DBConnection.DBConnection("NBHRMSDA1_phaoadb");
            DataTable dt = conn.ExcuteSingleQuery(sql).Tables[0];

            string data = string.Format("{{\"success\":true,\"data\":{0}}}", JsonConvert.SerializeObject(dt));
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(data);
            Response.End();
        }

        //獲取分時良率
        private void loadRate(string process,string line)
        {

            string rn = string.Empty;
            string OPER = string.Empty;

            int a = DateTime.Compare(DateTime.Parse("08:00"), DateTime.Now);
            int b = DateTime.Compare(DateTime.Parse("20:00"), DateTime.Now);

            if (a < 0 && b > 0)
            {
                rn = " where a.rn>=9 and a.rn<=20";
            }
            else if (b < 0)
            {
                rn = " where a.rn>=21";
            }
            else if (a > 0)
            {
                rn = " where a.rn>=21";
            }

            switch (process)
            {
                case "CDKEN":
                    OPER = "1600";
                    break;
                case "ASSY":
                    OPER = "1400";
                    break;
                case "BONDING":
                    OPER = "1300";
                    break;
                case "LAM":
                    OPER = "1355";
                    break;
            }

            string sql = @"select a.TIMEHOUR ,ERRC_DESCR, nvl(MAIN_QTY,0)MAIN_QTY,nvl(TOTAL_RATE,0)TOTAL_RATE, nvl(TOTAL_TARGET_YIELD,0)TOTAL_TARGET_YIELD from(
   SELECT
    replace(replace(lpad(ROWNUM, 2), ' ', '0'), '24', '00') st_hour,
    replace(replace(lpad(ROWNUM, 2), ' ', '0'),'24','00')|| ':00' timehour,
    replace(lpad(CASE
        WHEN ROWNUM >= 9  THEN ROWNUM
        ELSE ROWNUM + 24
    END, 2), ' ', '0') rn
FROM
    dual
CONNECT BY
    ROWNUM <= 24
)a left join(
select TRANS_DATE, trim(replace(to_char(TRANS_HOUR+1,'00'),'24','00')||':00') TRANS_HOUR, EQP_LINE, PROD_NBR, PASS, REJECT_QTY, TARGET_YIELD, DFCT_CODE, ERRC_DESCR, MAIN_QTY,
round(DECODE(PASS, 0, 0, round(PASS - reject_qty) / PASS * 100), 2)rate,round(DECODE(total_pass, 0, 0, round(total_pass - totalREJECT_QTY) / total_pass * 100), 2)total_rate,total_TARGET_YIELD from(
select TRANS_DATE, TRANS_HOUR, EQP_LINE, PROD_NBR, PASS, REJECT_QTY, TARGET_YIELD, DFCT_CODE, ERRC_DESCR, MAIN_QTY,
row_number()over(partition by TRANS_DATE, TRANS_HOUR, EQP_LINE order by MAIN_QTY desc,DFCT_CODE)rl,
sum(PASS)over(partition by TRANS_DATE, TRANS_HOUR, EQP_LINE order by 1)total_pass,
sum(REJECT_QTY)over(partition by TRANS_DATE, TRANS_HOUR, EQP_LINE order by 1)totalREJECT_QTY,
min(TARGET_YIELD)over(partition by TRANS_DATE, TRANS_HOUR, EQP_LINE order by 1)total_TARGET_YIELD from(
select a.TRANS_DATE, a.TRANS_HOUR, a.EQP_LINE, a.PROD_NBR, a.PASS, a.REJECT_QTY, a.TARGET_YIELD, b.DFCT_CODE, b.ERRC_DESCR, nvl(b.MAIN_QTY,0)MAIN_QTY from(
select TRANS_DATE, TRANS_HOUR, EQP_LINE, PROD_NBR, PASS, REJECT_QTY,
    CASE
    WHEN substr(prod_nbr, 1, 4) IN (
        'GV23',
        'GM17',
        'GM19',
        'GM21'
    ) THEN '98.29'
    WHEN substr(prod_nbr, 1, 4) IN (
        '2M17',
        '2M19',
        '2V23',
        '2M21',
        '2V29'
    ) THEN '99.21'
    WHEN substr(prod_nbr, 1, 4) IN (
        '2V32',
        '2V40',
        '2V42'
    ) THEN '98.2'
    WHEN substr(prod_nbr, 1, 4) IN (
        'GV29',
        'GM31',
        'GV42',
        'GV40',
        'GV50'
    ) THEN '96.09'
    WHEN substr(prod_nbr, 1, 4) IN (
        'GV32'
    ) THEN '98.00'          -- 20190627 拉國鋒 GV320BJ8D320M  CD檢目標良率至98.00%
    WHEN substr(prod_nbr, 3, 3) IN (
        'A00',
        '850'
    ) THEN '87'
    ELSE '98.00' --20190627 wuyan  為避免其他幾種無目標良率，暫定其他為 '98.00'
END target_yield from(
select a.TRANS_DATE,a.TRANS_HOUR,c.EQP_LINE, a.PROD_NBR, sum(a.QTY)pass,sum(nvl(b.reject_qty, 0)) reject_qty from(
SELECT
    TRANS_DATE,
    TRANS_HOUR,
    rtrim(equip_nbr) AS work_name,
    a.equip_nbr   work_ctr,
    SUM(pass_qty) AS qty,
    prod_nbr
FROM
    phaamsda1.hmaspas_soutput_hour a
WHERE
    a.fac_id = 'A'
    AND trans_date >= TO_CHAR(SYSDATE - 8 / 24, 'yyyy-mm-dd')-- '2020-07-13'  
    AND trans_date
        || TRIM(TO_CHAR(trans_hour, '00')) >= TO_CHAR(SYSDATE - 8 / 24, 'yyyy-mm-dd')
                                              || CASE
        WHEN to_number(TO_CHAR(SYSDATE, 'HH24')) BETWEEN 8 AND 19 THEN '08'
        ELSE '20'
    END
    AND main_wc='" + OPER + @"'
    AND stimes_ttl <= 1
    AND pass_qty <> 0
GROUP BY
    a.equip_nbr,
    prod_nbr,
     TRANS_DATE,
    TRANS_HOUR
)a left join(
    SELECT
    substr(TRANS_DATE,0,10)TRANS_DATE,
    substr(TRANS_DATE,12,2)hour,
    equip_nbr   AS work_ctr,
    SUM(CASE
        WHEN item_nbr < 2
             AND output_fg = '-' THEN - 1
        WHEN item_nbr < 2
             AND output_fg <> '-'
             AND stimes_ttl <= 1 THEN 1
        ELSE 0
    END) reject_qty,
    prod_nbr
FROM
    phaamsda1.hmashis_defectcode a
WHERE
    a.fac_id = 'A'
    AND a.trans_type NOT IN (
        'QCMK',
        'QCCK'
    )
    AND acct_date >= TO_CHAR(SYSDATE - 8 / 24, 'yyyy-mm-dd')
    AND trans_date >= TO_CHAR(SYSDATE - 8 / 24, 'yyyy-mm-dd')
                      || ' '
                      || CASE
        WHEN to_number(TO_CHAR(SYSDATE, 'HH24')) BETWEEN 8 AND 19 THEN '08.00'
        ELSE '20.00'
    END
    AND a.main_wc='" + OPER + @"'
    AND stimes_ttl <= 1
    AND a.dfct_code NOT IN (
        'PCZE4'
    )
GROUP BY
    equip_nbr,
    prod_nbr,
    substr(TRANS_DATE,12,2),
    substr(TRANS_DATE,0,10)
)b on a.TRANS_DATE=b.TRANS_DATE and a.TRANS_HOUR=b.HOUR
and a.WORK_CTR=b.WORK_CTR and a.PROD_NBR=b.PROD_NBR
left join PHAAMWDA1.bs_eqpmain c
on a.WORK_CTR=c.EQUIP_NBR
where c.EQP_LINE='" + line + @"'
group by a.TRANS_DATE,a.TRANS_HOUR,c.EQP_LINE, a.PROD_NBR
)
)a left join(
SELECT
    acct_date,
    HOUR,
    dfct_code,
    errc_descr,
    prod_nbr,
    work_ctr,
    SUM(main_qty) main_qty
FROM
    (
        SELECT
            acct_date,
            hour,
            dfct_code   AS dfct_code,
            rtrim(coalesce(errc_descr, '')) errc_descr,
            main_qty,
            prod_nbr,
            b.eqp_line AS work_ctr,
            work_ctr    work_ctr2
        FROM
            (
                SELECT
                    a.acct_date,
                    substr(a.TRANS_DATE,12,2)hour,
                    a.dfct_code,
                    equip_nbr   AS work_ctr,
                    b.errc_descr,
                    a.prod_nbr,
                    SUM(CASE
                        WHEN a.item_nbr < 2
                             AND output_fg = '-' THEN - 1
                        WHEN a.item_nbr < 2
                             AND output_fg <> '-'
                             AND stimes_ttl <= 1 THEN 1
                        ELSE 0
                    END) main_qty
                FROM
                   phaamsda1.hmashis_defectcode a
                    LEFT JOIN phaamsda1.hmashis_errc b ON a.fac_id = b.fac_id
                     AND a.dfct_code = b.errc_nbr
                WHERE
                    a.fac_id = 'A'
                    AND a.trans_type NOT IN (
                        'QCMK',
                        'QCCK'
                    )
                    AND acct_date >= TO_CHAR(SYSDATE - 8 / 24, 'yyyy-mm-dd')
                    AND trans_date >= TO_CHAR(SYSDATE - 8 / 24, 'yyyy-mm-dd')
                                      || ' '
                                      || CASE
                        WHEN to_number(TO_CHAR(SYSDATE, 'HH24')) BETWEEN 8 AND 19 THEN '08.00'
                        ELSE '20.00'
                    END
                    AND a.main_wc = '" + OPER + @"'
                    AND stimes_ttl <= 1
                    AND a.dfct_code NOT IN (
                        'PCZE4'
                    )
                GROUP BY
                    dfct_code,
                    a.equip_nbr,
                    a.trans_type,
                    a.output_fg,
                    a.prod_nbr,
                    b.errc_descr,
                    a.acct_date,
                    substr(a.TRANS_DATE,12,2)
            ) n
            LEFT JOIN phaamwda1.bs_eqpmain b ON n.work_ctr = b.equip_nbr
    )
WHERE
    work_ctr ='" + line + @"'
GROUP BY
     acct_date,
    HOUR,
    dfct_code,
    work_ctr,
    errc_descr,
    prod_nbr
)b ON a.TRANS_DATE = b.ACCT_DATE  AND a.TRANS_HOUR = b.HOUR
and a.EQP_LINE=b.work_ctr and a.PROD_NBR=b.PROD_NBR
)
)where rl<=10
)b on a.TIMEHOUR=b.TRANS_HOUR
" + rn + @"
order by a.rn";

            DBConnection.DBConnection conn = new DBConnection.DBConnection("PHAAMSDA1_PHBUR");
            DataTable dt = conn.ExcuteSingleQuery(sql).Tables[0];

            string data = string.Format("{{\"success\":true,\"data\":{0}}}", JsonConvert.SerializeObject(dt));
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(data);
            Response.End();
        }

        //獲取by小時匯總產出
        private void loadTotalOutput(string process, string line)
        {

            string shift1 = string.Empty;
            string shift2 = string.Empty;
            string startTime = string.Empty;
            string OPER = string.Empty;

            int a = DateTime.Compare(DateTime.Parse("08:00"), DateTime.Now);
            int b = DateTime.Compare(DateTime.Parse("20:00"), DateTime.Now);

            if (a < 0 && b > 0)
            {
                startTime = DateTime.Now.ToString("yyyy-MM-dd");
                shift1 = "D";
                shift2 = "A";
            }
            else if (b < 0)
            {
                startTime = DateTime.Now.ToString("yyyy-MM-dd");
                shift1 = "N";
                shift2 = "B";
            }
            else if (a > 0)
            {
                startTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                shift1 = "N";
                shift2 = "B";
            }

            switch (process)
            {
                case "CDKEN":
                    OPER = "1600";
                    break;
                case "ASSY":
                    OPER = "1400";
                    break;
                case "BONDING":
                    OPER = "1300";
                    break;
                case "LAM":
                    OPER = "1355";
                    break;
            }

            string sql = @"select TIMEINTERVAL,sum(QTY)over(partition by EQP_LINE order by TRANS_HOUR1)QTY,sum(PLANT_QTY_BYHOUR)over(partition by EQP_LINE order by TRANS_HOUR1)PLANT_QTY_BYHOUR from(
select EQP_LINE,TRANS_HOUR1,TIMEINTERVAL, sum(CNT)qty, PLANT_QTY_BYHOUR from(
select EQP_LINE,prod_nbr,TRANS_HOUR1,CNT,replace(replace(lpad(TRANS_HOUR2+1,2),' ',0),24,'00')||':00' timeInterval,PLANT_QTY_BYHOUR from(
select  nvl(MAIN_WC2,OPER) OPER, nvl(a.eqp_line,b.eqp_line)eqp_line , nvl(b.equip_nbr,a.LINEID)equip_nbr , nvl(b.LINEID,a.LINEID)LINEID , prod_nbr,
 nvl(a.trans_hour1,b.trans_hour1)trans_hour1 , nvl(cnt,0)cnt ,OPERATOR,nvl(a.HOUR,b.trans_hour2)trans_hour2,nvl(b.color_code,'#000000') color_code,   nvl(PLANT_QTY_BYHOUR,0) PLANT_QTY_BYHOUR 
from    
(   /*   抓取DCS上的Target值  */
select fab, PROCESS,
case    when PROCESS ='BONDING' then 1300
           when PROCESS ='ASSY' then 1400
           when PROCESS ='CKEN' then 1600
            when PROCESS ='AKEN' then 1450
            when PROCESS ='DKEN' then 1700
            when PROCESS ='PACK' then 1800
            else 0  
end     MAIN_WC2,   LINEID,
case when substr(LINEID,-4,1) = 'P'  and PROCESS ='ASSY' then substr(LINEID,-4) when substr(LINEID,length(LINEID)-2,1) in( '0', '1' , '2' , '3' , '4' , '5' , '6' , '7' , '8' , '9' )   then substr(LINEID,length(LINEID)-3,2)   
       when substr(LINEID,5,2) = 'RW' then 'RW'   else substr(LINEID,length(LINEID)-1,2) 
end  EQP_LINE,
 to_number( substr( HOUR,1,2 ) )  HOUR,
case  when to_number( substr( HOUR,1,2 ) )  >7  and to_number( substr( HOUR,1,2 ) )  <20
         then  to_number( substr( HOUR,1,2 ) ) -8
         when to_number( substr( HOUR,1,2 ) ) >=20  and to_number( substr( HOUR,1,2 ) ) <25
         then  to_number( substr( HOUR,1,2 ) ) -20
          else to_number( substr( HOUR,1,2 ) ) +4
 end  trans_hour1, 
 sum(PLANT_QTY_BYHOUR) PLANT_QTY_BYHOUR
 from PHSUMDA1.DCSMES_QMS_DAILYPLAN@PHARS@WYP  A
 where acct_date= to_char(to_date('" + startTime + @"' ,'YYYY-MM-DD'),'YYYYMMDD' )                   --'20171122'
 and FAB = 'MA10'
 and PROCESS = '" + process + @"' 
 and SHIFT_ID = '" + shift1 + @"'   
 group by  fab,PROCESS,  LINEID,HOUR
) a    
 full join
(/*  分時產出等信息   */
SELECT OPER,EQP_LINE,EQUIP_NBR,PROD_NBR,TO_NUMBER(TRANS_HOUR)TRANS_HOUR2,TRANS_HOUR1,QTY CNT,OPERATORS OPERATOR,COLOR_CODE,
 case  when  substr( equip_nbr,length(equip_nbr)-2,1) between '0' and '9'  then substr(equip_nbr,1,length(equip_nbr)-2)
         when  substr( equip_nbr,length(equip_nbr)-2,1) = '-'  then  substr(equip_nbr,1, INSTR(equip_nbr,'-',-1)-1)
         else equip_nbr
 end  lineid
 FROM(
SELECT A.*,
       CASE WHEN TO_NUMBER (A.TRANS_HOUR) > 7 AND TO_NUMBER (A.TRANS_HOUR) < 20
          THEN  TO_NUMBER (A.TRANS_HOUR) - 8
          WHEN TO_NUMBER (A.TRANS_HOUR) >= 20   AND TO_NUMBER (A.TRANS_HOUR) < 25
          THEN   TO_NUMBER (A.TRANS_HOUR) - 20
          ELSE  TO_NUMBER (A.TRANS_HOUR) + 4
       END
          TRANS_HOUR1,
          NVL(TRIM(B.CUSTOMER_GROUP),'OTHERS') CUSTOMER_GROUP,
          NVL(C.COLOR_CODE,'#000000') COLOR_CODE
  FROM VW_PNL_OUTPUT_BYHOUR A LEFT JOIN WP_WOMASTE B ON A.WO_NBR = B.WO_NBR
  LEFT JOIN PHSUMDA1.RTS_COLORCODE@PHARS@WYP C ON A.PROD_NBR = C.CUSTOMER
 WHERE A.SHIFT_DATE = '" + startTime + @"' AND A.OPER = '" + OPER + @"' AND SHIFT_ID ='" + shift2 + @"'
 )
 )b
on a.EQP_LINE = b.eqp_line and  a.trans_hour1 = b.trans_hour1
)where EQP_LINE='" + line + @"'
)group by EQP_LINE,TRANS_HOUR1,TIMEINTERVAL, PLANT_QTY_BYHOUR 
)";

            DBConnection.DBConnection conn = new DBConnection.DBConnection("PHAAMWDA1_PHBUR");
            DataTable dt = conn.ExcuteSingleQuery(sql).Tables[0];

            string data = string.Format("{{\"success\":true,\"data\":{0}}}", JsonConvert.SerializeObject(dt));
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(data);
            Response.End();
        }

        //獲取線別數據
        private void loadLineData(string process)
        {
            string sql = @"select distinct EQP_LINE
            from bs_eqpmain where PROCESS_TYPE='" + process + @"'
            order by EQP_LINE";

            DBConnection.DBConnection conn = new DBConnection.DBConnection("PHAAMWDA1_PHBUR");
            DataTable dt = conn.ExcuteSingleQuery(sql).Tables[0];

            string data = string.Format("{{\"success\":true,\"data\":{0}}}", JsonConvert.SerializeObject(dt));
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(data);
            Response.End();
        }

        //獲取分時產出數據
        private void loadHourOutput(string PROCESS,string line)
        {

            string shift1 = string.Empty;
            string shift2 = string.Empty;
            string startTime = string.Empty;
            string OPER = string.Empty;

            int a = DateTime.Compare(DateTime.Parse("08:00"), DateTime.Now);
            int b = DateTime.Compare(DateTime.Parse("20:00"), DateTime.Now);

            if (a < 0 && b > 0)
            {
                startTime = DateTime.Now.ToString("yyyy-MM-dd");
                shift1 = "D";
                shift2 = "A";
            }
            else if (b < 0)
            {
                startTime = DateTime.Now.ToString("yyyy-MM-dd");
                shift1 = "N";
                shift2 = "B";
            }
            else if (a > 0)
            {
                startTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                shift1 = "N";
                shift2 = "B";
            }

            switch (PROCESS)
            {
                case "CDKEN":
                    OPER = "1600";
                    break;
                case "ASSY":
                    OPER = "1400";
                    break;
                case "BONDING":
                    OPER = "1300";
                    break;
                case "LAM":
                    OPER = "1355";
                    break;
            }

            string sql = @"select EQP_LINE,TRANS_HOUR1,wmsys.wm_concat(distinct prod_nbr) prod_nbr,TIMEINTERVAL, sum(CNT)qty, PLANT_QTY_BYHOUR,round(decode(PLANT_QTY_BYHOUR,0,0,sum(CNT)/PLANT_QTY_BYHOUR),2)rate from(
select EQP_LINE,prod_nbr,TRANS_HOUR1,CNT,replace(replace(lpad(TRANS_HOUR2+1,2),' ',0),24,'00')||':00' timeInterval,PLANT_QTY_BYHOUR from(
select  nvl(MAIN_WC2,OPER) OPER, nvl(a.eqp_line,b.eqp_line)eqp_line , nvl(b.equip_nbr,a.LINEID)equip_nbr , nvl(b.LINEID,a.LINEID)LINEID , prod_nbr,
 nvl(a.trans_hour1,b.trans_hour1)trans_hour1 , nvl(cnt,0)cnt ,OPERATOR,nvl(a.HOUR,b.trans_hour2)trans_hour2,nvl(b.color_code,'#000000') color_code,   nvl(PLANT_QTY_BYHOUR,0) PLANT_QTY_BYHOUR 
from    
(   /*   抓取DCS上的Target值  */
select fab, PROCESS,
case    when PROCESS ='BONDING' then 1300
           when PROCESS ='ASSY' then 1400
           when PROCESS ='CKEN' then 1600
            when PROCESS ='AKEN' then 1450
            when PROCESS ='DKEN' then 1700
            when PROCESS ='PACK' then 1800
            else 0  
end     MAIN_WC2,   LINEID,
case when substr(LINEID,-4,1) = 'P'  and PROCESS ='ASSY' then substr(LINEID,-4) when substr(LINEID,length(LINEID)-2,1) in( '0', '1' , '2' , '3' , '4' , '5' , '6' , '7' , '8' , '9' )   then substr(LINEID,length(LINEID)-3,2)   
       when substr(LINEID,5,2) = 'RW' then 'RW'   else substr(LINEID,length(LINEID)-1,2) 
end  EQP_LINE,
 to_number( substr( HOUR,1,2 ) )  HOUR,
case  when to_number( substr( HOUR,1,2 ) )  >7  and to_number( substr( HOUR,1,2 ) )  <20
         then  to_number( substr( HOUR,1,2 ) ) -8
         when to_number( substr( HOUR,1,2 ) ) >=20  and to_number( substr( HOUR,1,2 ) ) <25
         then  to_number( substr( HOUR,1,2 ) ) -20
          else to_number( substr( HOUR,1,2 ) ) +4
 end  trans_hour1, 
 sum(PLANT_QTY_BYHOUR) PLANT_QTY_BYHOUR
 from PHSUMDA1.DCSMES_QMS_DAILYPLAN@PHARS@WYP  A
 where acct_date= to_char(to_date('" + startTime + @"' ,'YYYY-MM-DD'),'YYYYMMDD' )                   --'20171122'
 and FAB = 'MA10'
 and PROCESS = '" + PROCESS + @"' 
 and SHIFT_ID = '" + shift1 + @"'   
 group by  fab,PROCESS,  LINEID,HOUR
) a    
 full join
(/*  分時產出等信息   */
SELECT OPER,EQP_LINE,EQUIP_NBR,PROD_NBR,TO_NUMBER(TRANS_HOUR)TRANS_HOUR2,TRANS_HOUR1,QTY CNT,OPERATORS OPERATOR,COLOR_CODE,
 case  when  substr( equip_nbr,length(equip_nbr)-2,1) between '0' and '9'  then substr(equip_nbr,1,length(equip_nbr)-2)
         when  substr( equip_nbr,length(equip_nbr)-2,1) = '-'  then  substr(equip_nbr,1, INSTR(equip_nbr,'-',-1)-1)
         else equip_nbr
 end  lineid
 FROM(
SELECT A.*,
       CASE WHEN TO_NUMBER (A.TRANS_HOUR) > 7 AND TO_NUMBER (A.TRANS_HOUR) < 20
          THEN  TO_NUMBER (A.TRANS_HOUR) - 8
          WHEN TO_NUMBER (A.TRANS_HOUR) >= 20   AND TO_NUMBER (A.TRANS_HOUR) < 25
          THEN   TO_NUMBER (A.TRANS_HOUR) - 20
          ELSE  TO_NUMBER (A.TRANS_HOUR) + 4
       END
          TRANS_HOUR1,
          NVL(TRIM(B.CUSTOMER_GROUP),'OTHERS') CUSTOMER_GROUP,
          NVL(C.COLOR_CODE,'#000000') COLOR_CODE
  FROM VW_PNL_OUTPUT_BYHOUR A LEFT JOIN WP_WOMASTE B ON A.WO_NBR = B.WO_NBR
  LEFT JOIN PHSUMDA1.RTS_COLORCODE@PHARS@WYP C ON A.PROD_NBR = C.CUSTOMER
 WHERE A.SHIFT_DATE = '" + startTime + @"' AND A.OPER = '" + OPER + @"' AND SHIFT_ID ='" + shift2 + @"'
 )
 )b
on a.EQP_LINE = b.eqp_line and  a.trans_hour1 = b.trans_hour1
)where EQP_LINE='" + line + @"'
)group by EQP_LINE,TRANS_HOUR1,TIMEINTERVAL, PLANT_QTY_BYHOUR 
order by TRANS_HOUR1";

            DBConnection.DBConnection conn = new DBConnection.DBConnection("PHAAMWDA1_PHBUR");
            DataTable dt = conn.ExcuteSingleQuery(sql).Tables[0];

            string data = string.Format("{{\"success\":true,\"data\":{0}}}", JsonConvert.SerializeObject(dt));
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(data);
            Response.End();
        }

    }
}