using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace Stock_ASP_API.Controllers
{
    public class WebGrabber
    {
        WebRequest HttpWReq;
        public class DataJson
        {
            public string[] fields { get; set; }
            public string[,] data { get; set; }
        }
        public WebGrabber()
        {
            DateTime Date = DateTime.Now;
            string date = Date.ToString("yyyyMMdd");
            HttpWReq = WebRequest.Create(@"https://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date=" + date + "&stockNo=2330");
            HttpWReq.Method = "GET";
        }

        public DataJson WebStream()
        {
            WebResponse myResponse = HttpWReq.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream());
            string json = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();
            DataJson result = JsonConvert.DeserializeObject<DataJson>(@json);
            return result;
        }
        public string GetDate(DataJson result)
        {
            string lestdate = result.data[result.data.GetLength(0) - 1, 0];
            return lestdate;
        }
    }
}
