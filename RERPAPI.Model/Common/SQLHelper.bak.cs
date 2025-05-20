using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Data.Common;
using System.Reflection;
using System.Data;
using System.Dynamic;
using System.Linq.Expressions;


namespace RERPAPI.Model.Common
{
    public static class SQLHelper<T> where T : class, new()
    {
        static string connectionString = Config.ConnectionString;
        public static int Timeout = 2000;
        public static T ProcedureToModel(string procedureName, string[] paramName, object[] paramValue)
        {
            T model = new T();
            SqlConnection mySqlConnection = new SqlConnection(connectionString);
            SqlParameter sqlParam;
            mySqlConnection.Open();

            try
            {
                SqlCommand mySqlCommand = new SqlCommand(procedureName, mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                if (paramName != null)
                {
                    for (int i = 0; i < paramName.Length; i++)
                    {
                        sqlParam = new SqlParameter(paramName[i], paramValue[i]);
                        mySqlCommand.Parameters.Add(sqlParam);
                    }
                }
                SqlDataReader reader = mySqlCommand.ExecuteReader();
                model = reader.MapToSingle<T>();
            }
            catch (SqlException e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                mySqlConnection.Close();
            }

            return model;
        }
        public static List<T> FindAll()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            List<T> lst = new List<T>();
            T model = new T();
            Type type = model.GetType();
            string tableName = type.Name.StartsWith("Model") ? type.Name : type.Name.Replace("Model", "");
            try
            {
                string sql = string.Format("SELECT * FROM [{0}] with (nolock)", tableName);
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandTimeout = Timeout;
                cmd.CommandType = CommandType.Text;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                lst = reader.MapToList<T>();
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return lst;
        }

        public static ResultQuery UpdateFields(Dictionary<string, object> fieldValues, Expression exp)
        {
            ResultQuery r = new ResultQuery();
            r.TotalRow = 1;
            Type type = typeof(T);
            //string tableName = type.Name;
            string tableName = type.Name.StartsWith("Model") ? type.Name : type.Name.Replace("Model", "");

            string setClause = string.Join(", ", fieldValues.Keys.Select(key => $"{key} = @{key}"));

            string query = $"UPDATE {tableName} SET {setClause} WHERE {exp.ToString()}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                foreach (var field in fieldValues)
                {

                    command.Parameters.AddWithValue($"@{field.Key}", field.Value == null ? DBNull.Value : field.Value);
                }

                try
                {
                    connection.Open();
                    r.TotalRow = command.ExecuteNonQuery();
                    r.IsSuccess = true;
                }
                catch (Exception ex)
                {
                    r.IsSuccess = false;
                    r.ErrorText = ex.ToString();
                }
            }
            return r;
        }
        public class ResultQuery
        {
            public int ID { get; set; }
            public int TotalRow { get; set; }
            public bool IsSuccess { get; set; }
            public string ErrorText { get; set; }
        }

        public static List<List<dynamic>> ProcedureToDynamicLists(string procedureName, string[] paramName, object[] paramValue)
        {
            List<List<dynamic>> resultLists = new List<List<dynamic>>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = Timeout;

                    if (paramName != null)
                    {
                        for (int i = 0; i < paramName.Length; i++)
                        {
                            cmd.Parameters.Add(new SqlParameter(paramName[i], paramValue[i]));
                        }
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);

                        foreach (DataTable table in ds.Tables)
                        {
                            List<dynamic> dynamicList = new List<dynamic>();

                            foreach (DataRow row in table.Rows)
                            {
                                IDictionary<string, object> expando = new ExpandoObject();
                                foreach (DataColumn col in table.Columns)
                                {
                                    expando[col.ColumnName] = row[col];
                                }
                                dynamicList.Add(expando);
                            }

                            resultLists.Add(dynamicList);
                        }
                    }
                }
            }

            return resultLists;
        }

        public static List<T> ProcedureToList(string procedureName, string[] paramName, object[] paramValue)
        {
            List<T> lst = new List<T>();
            SqlConnection mySqlConnection = new SqlConnection(connectionString);
            SqlParameter sqlParam;
            mySqlConnection.Open();

            try
            {
                SqlCommand mySqlCommand = new SqlCommand(procedureName, mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.CommandTimeout = Timeout;
                if (paramName != null)
                {
                    for (int i = 0; i < paramName.Length; i++)
                    {
                        sqlParam = new SqlParameter(paramName[i], paramValue[i]);
                        mySqlCommand.Parameters.Add(sqlParam);
                    }
                }
                SqlDataReader reader = mySqlCommand.ExecuteReader();
                lst = reader.MapToList<T>();
            }
            catch (SqlException e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                mySqlConnection.Close();
            }

            return lst;
        }
    }
}
