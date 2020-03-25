using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory
{
    class CSql
    {
        public static string GetConnectionString()
        {
            string connectionString = Properties.Settings.Default.connectionString2;
            return connectionString;
        }

        public static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(GetConnectionString());
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
                return conn;
            }
            return null;
        }

        public static int CountRecords(string table)
        {
            int count = 0;
            string sql = $"SELECT COUNT(*) FROM {table}";
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    count = (int)command.ExecuteScalar();
                }
            }
            return count;
        }

        public static DataTable GetDataTable(string sql)
        {
            SqlConnection conn = GetConnection();
            if (conn != null)
            {
                DataTable dataTable = new DataTable();
                if (dataTable.Rows.Count >= 0)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);                    
                    adapter.Fill(dataTable);
                    
                    return dataTable;
                }
                conn.Close();
            }
            return null;
        }


        public static void CloseConnection(SqlConnection conn)
        {
            if (conn.State != ConnectionState.Closed)
            {
                try
                {
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace.ToString());
                }
            }
        }

        public static void CloseConnection()
        {
            string connectionString = GetConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            if (conn.State != ConnectionState.Closed)
            {
                conn.Close();
            }
        }

        public static void ExecuteCommand(string sql)
        {
            SqlConnection conn = GetConnection();
            if (conn != null)
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand(sql, conn);
                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace.ToString());
                }
            }
        }

        public static void UpdateItemRecord(SqlConnection conn, int type, int id)
        {
            string sql = $"UPDATE tblItems SET Type='{type}',SellIn='{null}',Quality='{null}' WHERE Id = '{id}';";
            SqlCommand sqlCommand = new SqlCommand(sql, conn);
            sqlCommand.ExecuteNonQuery();
        }


        public static void UpdateItemRecord(SqlConnection conn, string item,int type, int sellin, int quality,int id)
        {
            string sql = $"UPDATE tblItems SET Item='{item}',Type='{type}',SellIn='{sellin}',Quality='{quality}' WHERE Id = '{id}';";
            SqlCommand sqlCommand = new SqlCommand(sql, conn);
            sqlCommand.ExecuteNonQuery();
        }

        public static void UpdateItemRecord(SqlConnection conn, int sellin, int quality, int id)
        {
            string sql = $"UPDATE tblItems SET SellIn='{sellin}',Quality='{quality}' WHERE Id = '{id}';";
            SqlCommand sqlCommand = new SqlCommand(sql, conn);
            sqlCommand.ExecuteNonQuery();
        }


        public static void UpdateItemRecord(SqlConnection conn, int type, int sellin, int quality, int id)
        {
            string sql = $"UPDATE tblItems SET Type='{type}',SellIn='{sellin}',Quality='{quality}' WHERE Id = '{id}';";
            SqlCommand sqlCommand = new SqlCommand(sql, conn);
            sqlCommand.ExecuteNonQuery();
        }

        public static void InsertTypeRecord(SqlConnection conn, string type,int typeValue)
        {
            string sql = $"INSERT INTO tblTypes ([Type],[TypeValue]) VALUES ('{type}','{typeValue}')";
            SqlCommand sqlCommand = new SqlCommand(sql, conn);
            sqlCommand.ExecuteNonQuery();
        }

        public static void InsertItemRecord(SqlConnection conn, string item, int type, int sellIn, int quality)
        {
            string sql = $"INSERT INTO tblItems ([Item],[Type],[SellIn],[Quality]) VALUES ('{item}','{type}','{sellIn}','{quality}')";
            SqlCommand sqlCommand = new SqlCommand(sql, conn);
            sqlCommand.ExecuteNonQuery();
        }

        public static void DeleteRecord(string table, int id)
        {
            string sql = $"DELETE FROM {table} WHERE Id = '{id}'";
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void ClearTable(SqlConnection conn, string table)
        {
            string sql = $"DELETE FROM {table}";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
        }


        public static string GetRecord(string table,string id)
        {
            string sql = $"SELECT TOP 1 FROM {table} WHERE [ID] = '{id}'";
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
            return "";
        }

    }
}
