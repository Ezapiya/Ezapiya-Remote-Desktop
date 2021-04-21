using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Threading;

namespace Connection_Server
{
    class Main_db
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader res;
        public Main_db()
        {
            ///con = new SqlConnection(@"Data Source=WIN-I8HJI2D9UE1\SQLEXPRESS;Initial Catalog=ap_database;User ID=sa;Password=april@131211;MultipleActiveResultSets=true;");
            con = new SqlConnection(@"Data Source=VINIT-PC\SQLEXPRESS;Initial Catalog=main_database;User ID=sa;Password=april@131211;MultipleActiveResultSets=true;");
            cmd = con.CreateCommand();
            con.Open();
        }
        public DataTable get_data_table(String sql)
        {

            DataTable dt = new DataTable();
            cmd.CommandText = sql;
        retry:;
            try
            {

                res = cmd.ExecuteReader();
                dt.Load(res);
                res.Close();

                return dt;
            }
            catch (SqlException ex)
            {
                if (ex.Number == -2)
                {
                    Console.WriteLine("Timeout occurred");
                    Thread.Sleep(200);
                    goto retry;
                }
                else
                {
                    Console.WriteLine("DB error :-" + ex.Number);
                }
                
                return dt;

            }


        }
        public void run_sql(String sql)
        {
            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
         }

        public String get_value(string coloum_name, string table_name, string Condition)
        {
            String re = "";
            try
            {
                string temp = "";
                if (Condition == String.Empty)
                    temp = "select " + coloum_name + " as max_val from " + table_name;
                else
                    temp = "select " + coloum_name + " as max_val from " + table_name + " where " + Condition;

                cmd.CommandText = temp;
                res = cmd.ExecuteReader();
                while (res.Read())
                {
                    re = Convert.ToString(res["max_val"]);
                }
                res.Close();
                return re;
            }
            catch (Exception ex)
            {
                //re = -1;
                return re;
            }
        }
        public String get_value(string coloum_name, string table_name)
        {
            String re = "";
            try
            {
                string temp = "";

                temp = "select " + coloum_name + " as max_val from " + table_name;

                cmd.CommandText = temp;
                res = cmd.ExecuteReader();
                while (res.Read())
                {
                    re = Convert.ToString(res["max_val"]);
                }
                res.Close();
                return re;
            }
            catch (Exception ex)
            {
                //re = -1;
                return re;
            }
        }
        public int get_max_value(string coloum_name, string table_name, string Condition)
        {
            int re = -1;
            try
            {
                string temp = "";
                if (Condition == String.Empty)
                    temp = "select max(" + coloum_name + ") as max_val from " + table_name;
                else
                    temp = "select max(" + coloum_name + ") as max_val from " + table_name + " where " + Condition;

                cmd.CommandText = temp;
                res = cmd.ExecuteReader();
                while (res.Read())
                {
                    re = Convert.ToInt16(res["max_val"]);
                }
                res.Close();
                return re;
            }
            catch (Exception ex)
            {
                re = -1;
                return re;
            }
        }
        public int check_duplicate(string coloum_name, string value, string table_name, string Condition)
        {
            int re = -1;
            try
            {
                string temp = "";
                if (Condition == String.Empty)
                    temp = "select count(*) as row_count from " + table_name + " where " + coloum_name + " = '" + value + "'";
                else
                    temp = "select count(*) as row_count from " + table_name + " where " + coloum_name + " = '" + value + "' and " + Condition;

                cmd.CommandText = temp;
                res = cmd.ExecuteReader();
                while (res.Read())
                {
                    re = Convert.ToInt16(res["row_count"]);
                }
                res.Close();
                return re;
            }
            catch (Exception ex)
            {
                re = -1;
                return re;
            }


        }
    }
}
