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
    class DB
    {
      
       SqlConnection con;
       SqlCommand cmd;
       SqlDataReader res;
        public DB()
        {
            //Data Source=WIN-I8HJI2D9UE1\SQLEXPRESS;Initial Catalog=ap_database;User ID=sa;Password=***********
            //con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=E:\Project\ApoioView_database\Ezapiya_databse.mdf;Integrated Security=True;Connect Timeout=30");
            //con = new SqlConnection(@"Data Source=WIN-I8HJI2D9UE1\SQLEXPRESS;Initial Catalog=ap_database;User ID=sa;Password=april@131211");
            con = new SqlConnection(@"Data Source=VINIT-PC\SQLEXPRESS;Initial Catalog=ap1;User ID=sa;Password=april@131211;MultipleActiveResultSets=true;");
            cmd = con.CreateCommand();
            con.Open();
        }
       public DataTable get_data_table(String sql)
        {
           
           DataTable dt = new DataTable();
           cmd.CommandText = sql;
       retry: ;
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
                    goto retry;
                }
                else
                {
                    Console.WriteLine("DB error :-" + ex.Number);
                }
                Thread.Sleep(200);
                return dt;

            }
             
               
        }
        
      
        public String get_data_table_in_string(String sql)
        {
            String result = "";

            DataTable dt = new DataTable();

            cmd.CommandText = sql;
       
            try
            {
                res = cmd.ExecuteReader();
                dt.Load(res);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        result = result + Convert.ToString(dt.Rows[i][j]) + "\t";
                    }
                    result = result + "\n";
                }
                res.Close();
            }
            catch (Exception ex)
            {
               
            }
            return result;
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
