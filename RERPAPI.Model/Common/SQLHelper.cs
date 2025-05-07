using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RERPAPI.Model.Common
{
    public static class SQLHelper<T> where T : class, new()
    {
        static string connectionString = Config.ConnectionString;
        static int commandTimeout = 2000;
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
                mySqlCommand.CommandTimeout = commandTimeout;
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
