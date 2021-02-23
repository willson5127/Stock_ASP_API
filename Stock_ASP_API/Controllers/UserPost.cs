using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace Stock_ASP_API.Controllers
{
    public class UserPost
    {
        Mysql_A sqlb = new Mysql_A();
        WebGrabber wg = new WebGrabber();
        DateTime Date = DateTime.Now;

        public class DataJson
        {
            public string[] fields { get; set; }
            public string[,] data { get; set; }
        }
        /// <summary>
        /// 使用者註冊
        /// </summary>
        /// <param name="username">名稱</param>
        /// <param name="password">密碼</param>
        /// <returns></returns>
        public string Register(string username, string password)
        {
            try 
            {
                sqlb.DataProcess("INSERT INTO customers VALUES(0, '" + username + "');");
                sqlb.DataProcess("INSERT INTO Passwords VALUES((SELECT C_id FROM customers WHERE User = '" + username + "'), '" + password + "');");
                return "Success!";
            } catch 
            {
                return "Error!"; 
            }
        }
        /// <summary>
        /// 使用者登入
        /// </summary>
        /// <param name="username">名稱</param>
        /// <param name="password">密碼</param>
        /// <returns></returns>
        public string Login(string username, string password)
        {
            try
            {
                string search = sqlb.Select("SELECT User FROM customers WHERE User = '" + username + "';", "User");
                if (search != username) return "Error! The user name is not exist!";
                string scan = sqlb.Select("SELECT Password FROM passwords WHERE C_id = (SELECT C_id FROM customers WHERE User = '" + username + "');", "Password");
                if (scan != password) return "Error! password is wrong!";
                return "Success!";
            }
            catch
            {
                return "Error!";
            }
        }
        /// <summary>
        /// 加入自選股票
        /// </summary>
        /// <param name="stock">股票代號</param>
        /// <param name="username">使用者名稱</param>
        /// <param name="password">使用者密碼</param>
        /// <returns></returns>
        public string AddOrder(string stock, string username, string password)
        {
            try
            {
                if (Login(username, password) != "Success!") return "Login Fail!";
                string search = sqlb.Select("SELECT S_id FROM stocks WHERE S_id = '" + stock + "';", "S_id");
                if (search != stock) return "Error! The stock id is not exist!";
                sqlb.DataProcess("INSERT INTO orders(C_id, S_id) VALUES((SELECT C_id FROM customers WHERE User = '" + username + "'), " + stock + ");");
                return "Success!";
            }
            catch
            {
                return "Error!";
            }            
        }
        /// <summary>
        /// 查詢自選股票
        /// </summary>
        /// <param name="username">使用者名稱</param>
        /// <param name="password">使用者密碼</param>
        /// <returns></returns>
        public string SearchOrder(string username, string password)
        {
            //try
            //{
                if (Login(username, password) != "Success!") return "Login Fail!";
                List<string> list = sqlb.Select("SELECT stocks.S_id FROM orders JOIN customers ON orders.C_id = customers.C_id JOIN stocks ON orders.S_id = stocks.S_id WHERE User = '" + username + "';");
                string[] rowname = new string[] { "Date" };
                for (int i=0; i < list.Count; i++)
                {
                    string date = Date.ToString("yyyyMMdd");
                    WebRequest HttpWReq = WebRequest.Create(@"https://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date=" + date + "&stockNo=" + list[i] + "");
                    HttpWReq.Method = "GET";
                    WebResponse myResponse = HttpWReq.GetResponse();
                    Thread.Sleep(3000);
                    StreamReader sr = new StreamReader(myResponse.GetResponseStream());
                    string json = sr.ReadToEnd();
                    sr.Close();
                    myResponse.Close();

                    DataJson result = JsonConvert.DeserializeObject<DataJson>(@json);
                    string lestdate = result.data[result.data.GetLength(0) - 1, 0];

                    string historydate = sqlb.Select("SELECT stocks.Date FROM orders JOIN stocks ON stocks.S_id = " + list[i] + " WHERE orders.S_id = " + list[i] + ";", rowname, false);
                    if (historydate != lestdate)
                    {                        
                        int n = sqlb.DataProcess("UPDATE stocks SET Open_price='" + result.data[result.data.GetLength(0) - 1, 3] + "', End_price='" + result.data[result.data.GetLength(0) - 1, 6] +
                            "', High_price='" + result.data[result.data.GetLength(0) - 1, 4] + "', Low_price='" + result.data[result.data.GetLength(0) - 1, 5] +
                            "', Rang='" + result.data[result.data.GetLength(0) - 1, 7] + "', Vol='" + result.data[result.data.GetLength(0) - 1, 8] +
                            "', Vol_unit='" + result.data[result.data.GetLength(0) - 1, 1] + "', Vol_price='" + result.data[result.data.GetLength(0) - 1, 2] +
                            "', Date='" + result.data[result.data.GetLength(0) - 1, 0] + "' WHERE S_id='" + list[i] + "';");
                    }
                }
                string search = "代號\t名稱\t開盤價\t收盤價\t最高價\t最低價\t漲跌幅\r\n";
                rowname = new string[] { "S_id", "S_name", "Open_price", "End_price" , "High_price", "Low_price", "Rang" };
                search += sqlb.Select("SELECT stocks.S_id, stocks.S_name, stocks.Open_price, stocks.End_price, stocks.High_price, stocks.Low_price, stocks.Rang FROM orders JOIN customers ON orders.C_id = customers.C_id JOIN stocks ON orders.S_id = stocks.S_id WHERE User = '" + username + "';", rowname, true);
                return search;
            //}
            //catch
            //{
            //    return "Error!";
            //}
        }
    }
}
