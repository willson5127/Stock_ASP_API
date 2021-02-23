using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace Stock_ASP_API
{
    public class Mysql_A
    {
        private string connString = "server=127.0.0.1;port=3306;user id=root;password=1234 ;database=StockAPP;charset=utf8;";
        private MySqlConnection conn = new MySqlConnection();
        MySqlCommand cmd = new MySqlCommand();
        /// <summary>
        /// 初始化MySQL，包含連線SQL
        /// </summary>
        public Mysql_A()
        {
            conn.ConnectionString = connString;
            if (conn.State != ConnectionState.Open)
                conn.Open();
        }
        public List<string> Select(string commend)
        {
            //string result = "error";
            List<string> list = new List<string>(); 
            cmd = new MySqlCommand(@commend, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    for(int i = 0; i < dr.FieldCount; i++) list.Add(dr[i].ToString());
                }
            }
            dr.Close();
            return list;
        }
        /// <summary>
        /// SELECT讀取資料庫
        /// </summary>
        /// <param name="commend">SQL指令</param>
        /// <returns></returns>
        public string Select(string commend, string rowname)
        {
            string result = "error";
            cmd = new MySqlCommand(@commend, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    result = dr[rowname].ToString();
                }
            }
            dr.Close();
            return result;
        }
        /// <summary>
        /// SELECT讀取資料庫
        /// </summary>
        /// <param name="commend">SQL指令</param>
        /// <param name="rowname">檢視欄位</param>
        /// <param name="rn">是否跳行</param>
        /// <returns></returns>
        public string Select(string commend, string[] rowname, bool rn)
        {
            string result = "error", tmp = "";
            cmd = new MySqlCommand(@commend, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (!dr[0].Equals(DBNull.Value))
                {
                    for(int i = 0; i < rowname.Length; i++)
                    {
                        tmp += dr[rowname[i]].ToString();
                        if (i != rowname.Length - 1) tmp += ",\t";

                    }                    
                }
                if(rn) tmp += "\r\n";
            }
            result = tmp;
            dr.Close();
            return result;
        }
        /// <summary>
        /// 資料庫更新指令
        /// </summary>
        /// <param name="commend">SQL指令</param>
        /// <returns></returns>
        public int DataProcess(string commend)
        {
            cmd = new MySqlCommand(@commend, conn);
            return cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 關閉資料庫
        /// </summary>
        public void close()
        {
            conn.Close();
        }
    }
}
