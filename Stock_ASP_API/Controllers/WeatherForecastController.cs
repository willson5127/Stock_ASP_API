using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace Stock_ASP_API.Controllers
{
    [ApiController]
    
    public class WeatherForecastController : ControllerBase
    {
        public class DataJson
        {
            public string[] fields { get; set; }
            public string[,] data { get; set; }
        }

        public class Userpost
        {
            public string action { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public string stock { get; set; }
        }
        int i = 0;

        
        [Route("Usercommit")]
        [HttpPost]
        public string FirstSample([FromBody] Userpost d)
        {
            UserPost up = new UserPost();
            switch (d.action)
            {
                case "register":
                    return up.Register(d.username, d.password);
                case "login":
                    return up.Login(d.username, d.password);
                case "add stock":
                    return up.AddOrder(d.stock, d.username, d.password);
                case "search order":
                    return up.SearchOrder(d.username, d.password);
                default:
                    return "Error, out of case!";
            }
        }

        [Route("Webgrabber")]
        [HttpGet]
        public string webgrabber()
        {
            //避開週休二日
            string week = DateTime.Now.ToString("ddd");
            DateTime Date = DateTime.Now;
            string date = Date.ToString("yyyyMMdd");
            //if (week == "週六") date = Date.AddDays(-1).ToString("yyyyMMdd");
            //else if (week == "週日") date = Date.AddDays(-2).ToString("yyyyMMdd");

            //網頁訪問
            WebRequest HttpWReq = WebRequest.Create(@"https://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date=" + date + "&stockNo=2330");
            HttpWReq.Method = "GET";
            WebResponse myResponse = HttpWReq.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream());
            string json = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();
            
            //JSON反序列化
            DataJson result = JsonConvert.DeserializeObject<DataJson>(@json);
            string lestdate = result.data[result.data.GetLength(0) - 1, 0];     //["日期","成交股數","成交金額","開盤價","最高價","最低價","收盤價","漲跌價差","成交筆數"]

            //資料庫工作
            Models.Mysql_A sqla = new Models.Mysql_A();
            string commend;
            string[] rowname = new string[] { "Date" };
            commend = "SELECT stocks.Date FROM orders JOIN stocks ON stocks.S_id = 2330 WHERE orders.S_id = 2330;";
            string historydate = sqla.Select(commend, rowname, false);
            if (historydate != lestdate)
            {
                commend = "UPDATE stocks SET Open_price='" + result.data[result.data.GetLength(0) - 1, 3] + "', End_price='" + result.data[result.data.GetLength(0) - 1, 6] +
                    "', High_price='" + result.data[result.data.GetLength(0) - 1, 4] + "', Low_price='" + result.data[result.data.GetLength(0) - 1, 5] +
                    "', Rang='" + result.data[result.data.GetLength(0) - 1, 7] + "', Vol='" + result.data[result.data.GetLength(0) - 1, 8] +
                    "', Vol_unit='" + result.data[result.data.GetLength(0) - 1, 1] + "', Vol_price='" + result.data[result.data.GetLength(0) - 1, 2] +
                    "', Date='" + result.data[result.data.GetLength(0) - 1, 0] + "' WHERE S_id='2330';";
                int n = sqla.DataProcess(commend);
            }
            commend = @"SELECT customers.User, stocks.S_id, stocks.S_name, stocks.End_price FROM orders JOIN customers ON orders.C_id = customers.C_id JOIN stocks ON orders.S_id = stocks.S_id;";
            rowname = new string[]  { "User" , "S_id" , "S_name" , "End_price"};
            string finalresult = sqla.Select(commend, rowname, true);
            sqla.close();
            return finalresult;


            //string connString = "server=127.0.0.1;port=3306;user id=root;password=5!2751@7Qp;database=StockAPP;charset=utf8;";
            //MySqlConnection conn = new MySqlConnection();
            //conn.ConnectionString = connString;
            //if (conn.State != ConnectionState.Open)
            //    conn.Open();

            //string sql = @"SELECT stocks.Date FROM orders JOIN stocks ON stocks.S_id = 2330 WHERE orders.S_id = 2330;";
            //MySqlCommand cmd = new MySqlCommand(sql, conn);
            //MySqlDataReader dr = cmd.ExecuteReader();

            //string historydate = "";
            //while (dr.Read())
            //{
            //    if (!dr[0].Equals(DBNull.Value))
            //    {
            //        historydate = dr["Date"].ToString();
            //    }
            //}            
            //dr.Close();

            //判定資料是否過期
            //if (historydate != lestdate)
            //{
            //    sql = "UPDATE stocks SET Open_price='" + result.data[result.data.GetLength(0) - 1, 3] + "', End_price='" + result.data[result.data.GetLength(0) - 1, 6] +
            //        "', High_price='" + result.data[result.data.GetLength(0) - 1, 4] + "', Low_price='" + result.data[result.data.GetLength(0) - 1, 5] +
            //        "', Rang='" + result.data[result.data.GetLength(0) - 1, 7] + "', Vol='" + result.data[result.data.GetLength(0) - 1, 8] +
            //        "', Vol_unit='" + result.data[result.data.GetLength(0) - 1, 1] + "', Vol_price='" + result.data[result.data.GetLength(0) - 1, 2] +
            //        "', Date='" + result.data[result.data.GetLength(0) - 1, 0] + "' WHERE S_id='2330';";

            //    cmd = new MySqlCommand(sql, conn);
            //    int n = cmd.ExecuteNonQuery();
            //}

            //回傳修改後資料
            //sql = @"SELECT * FROM orders JOIN customers ON orders.C_id = customers.C_id JOIN stocks ON orders.S_id = stocks.S_id;";
            //cmd = new MySqlCommand(sql, conn);
            //MySqlDataReader dr2 = cmd.ExecuteReader();
            //string tmp2 = "";
            //while (dr2.Read())
            //{
            //    if (!dr2[0].Equals(DBNull.Value))
            //    {
            //        tmp2 += dr2["O_id"].ToString() + ",";
            //        tmp2 += dr2["C_id"].ToString() + ",";                    
            //        tmp2 += dr2["User"].ToString() + ",";
            //        tmp2 += dr2["S_id"].ToString() + ",";
            //        tmp2 += dr2["S_name"].ToString() + ",";
            //        tmp2 += dr2["Open_price"].ToString() + ",";                    
            //        tmp2 += dr2["End_price"].ToString() + ",";
            //        tmp2 += dr2["High_price"].ToString() + ",";
            //        tmp2 += dr2["Low_price"].ToString() + ",";
            //        tmp2 += dr2["Rang"].ToString() + ",";
            //        tmp2 += dr2["Vol"].ToString() + ",";
            //        tmp2 += dr2["Vol_unit"].ToString() + ",";
            //        tmp2 += dr2["Vol_price"].ToString() + ",";
            //        tmp2 += dr2["Date"].ToString();
            //    }
            //    tmp2 += "\r\n";
            //}

            //警告 執行時間可能將近10分鐘
            //string add = "";
            //for (int i = 0; i < result.fields.Length; i++) add += result.fields[i] + "    ";
            //add += "\r\n";
            //for(int i = 0; i < result.data.GetLength(0); i++)
            //{
            //    for(int j = 0; j < result.data.GetLength(1); j++) add += result.data[i,j] + "    ";
            //    add += "\r\n";
            //}
            //return tmp2;
        }

        [Route("[Controller]")]
        [HttpGet]
        public string SQL()
        {
            //MySQL登入
            string connString = "server=127.0.0.1;port=3306;user id=root;password=5!2751@7Qp;database=StockAPP;charset=utf8;";
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = connString;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            //MySQL下指令
            string sql = @"SELECT customers.User, stocks.S_id, stocks.S_name, stocks.End_price FROM orders JOIN customers ON orders.C_id = customers.C_id JOIN stocks ON orders.S_id = stocks.S_id;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            string[] tmp = new string[10];
            string tmp2 = "";
            int i = 0;
            while (dr.Read())
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    tmp2 += dr["User"].ToString() + ",";
                    tmp2 += dr["S_id"].ToString() + ",";
                    tmp2 += dr["S_name"].ToString() + ",";
                    tmp2 += dr["End_price"].ToString() + ",";
                }
                tmp2 += "\r\n";
            }

            conn.Close();
            return tmp2;

            
        }

    }
}
