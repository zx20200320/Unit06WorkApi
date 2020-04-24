using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace Unit06WorkApi
{
    public static class DBHelper<T> where T:class,new()
    {
        private static string connStr = "Data Source=.;Initial Catalog=Blog_DB;Integrated Security=True";

        /// <summary>
        /// 执行增删改
        /// </summary>
        /// <returns></returns>
        public static int ExecNonQuery(string sql)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                return cmd.ExecuteNonQuery();
            }                    
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DataTable GetTable(string str)
        {
            using (SqlConnection conn=new SqlConnection(connStr))
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(str,conn);
                SqlDataAdapter dpt = new SqlDataAdapter(cmd);
                dpt.Fill(dt);
                return dt;
            }
        }
        /// <summary>
        /// datatable转list
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<T> GetData(string str)
        {
            var dt = GetTable(str);
            return JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(dt));
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static int Add(T t)
        {
            var type = t.GetType();
            var tableName = type.Name;    //获取表命名
            var colName = string.Join(",",type.GetProperties().Where(p=>p.Name.ToLower()!="id").Select(p=>p.Name).ToList());
            var colValue = string.Join(",",type.GetProperties().Where(p=>p.Name.ToLower()!="id").Select(p=>"'"+p.GetValue(t)+"'").ToList());
            string sql = $"insert {tableName} ({colName}) values({colValue})";
            return ExecNonQuery(sql);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="t"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int Update(T t,int id)
        {
            var type = typeof(T);
            var tableName = type.Name;
            var colValue = string.Join(",",type.GetProperties().Where(p=>p.Name.ToLower()!="id").Select(p=>p.Name+"'"+p.GetValue(t)+"'").ToList());
            string sql = $"update {tableName} set {colValue} where Id={id}";
            return ExecNonQuery(sql);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="t"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int Delete(T t,int id)
        {
            var type = typeof(T);
            var tableName = type.Name;
            string sql = $"delete from {tableName} where Id={id}";
            return ExecNonQuery(sql);
        }
        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<T> GetAll(T t)
        {
            var type = t.GetType();
            var tableName = type.Name;
            string sql = $"select * from {tableName}";
            var dt = GetTable(sql);
            return JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(dt));
        }
    }
}