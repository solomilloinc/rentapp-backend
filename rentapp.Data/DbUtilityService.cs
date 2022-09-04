using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using Microsoft.Data.SqlClient;
using rentapp.Data;

namespace rentapp.Service.Data
{
    public static class DbUtilityService
    {
        public static SqlParameter Build(string name, bool? value)
        {
            if (value.HasValue)
            {
                return new SqlParameter() { ParameterName = name, Value = value.Value };
            }
            return new SqlParameter() { ParameterName = name, Value = DBNull.Value };
        }

        public static SqlParameter Build(string name, int? value)
        {
            if (value.HasValue)
            {
                return new SqlParameter() { ParameterName = name, Value = value.Value };
            }
            return new SqlParameter() { ParameterName = name, Value = DBNull.Value };
        }

        public static SqlParameter Build(string name, double? value)
        {
            if (value.HasValue)
            {
                return new SqlParameter() { ParameterName = name, Value = value.Value };
            }
            return new SqlParameter() { ParameterName = name, Value = DBNull.Value };
        }

        public static SqlParameter Build(string name, string value)
        {
            if (value != null)
            {
                return new SqlParameter() { ParameterName = name, Value = value };
            }
            return new SqlParameter() { ParameterName = name, Value = DBNull.Value };
        }

        public static SqlParameter Build(string name, DateTime? value)
        {
            if (value != null)
            {
                return new SqlParameter { ParameterName = name, SqlDbType = SqlDbType.DateTime, Value = value };
            }
            return new SqlParameter() { ParameterName = name, Value = DBNull.Value };
        }

        public static SqlParameter Build(string name, TimeSpan? value)
        {
            if (value != null)
            {
                return new SqlParameter { ParameterName = name, SqlDbType = SqlDbType.Time, Value = value };
            }
            return new SqlParameter() { ParameterName = name, Value = DBNull.Value };
        }

        public static SqlParameter Build(string name, Guid? value)
        {
            if (value.HasValue)
            {
                return new SqlParameter { ParameterName = name, SqlDbType = SqlDbType.VarChar, Value = value };
            }
            return new SqlParameter() { ParameterName = name, Value = DBNull.Value };
        }

        /*
        public static SqlParameter Build(string name, int[] values)
        {
            SqlParameter par = new SqlParameter(name, SqlDbType.Structured);
            par.TypeName = "dbo.IntParameterList";
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            par.Value = dt;
            if (values != null)
            {
                foreach (int value in values.Where(p => p != 0))
                {
                    dt.Rows.Add(value);
                }
            }
            return par;
        }*/


        /*
        public static SqlParameter Build(string name, string[] values, VarcharParameterListEnum varcharParameterListType = VarcharParameterListEnum.Varchar50)
        {
            SqlParameter par = new SqlParameter(name, SqlDbType.Structured);
            switch (varcharParameterListType)
            {
                case VarcharParameterListEnum.Varchar2:
                    par.TypeName = "dbo.Varchar2ParameterList";
                    break;
                case VarcharParameterListEnum.Varchar15:
                    par.TypeName = "dbo.Varchar15ParameterList";
                    break;
                case VarcharParameterListEnum.Varchar50:
                    par.TypeName = "dbo.Varchar50ParameterList";
                    break;
                case VarcharParameterListEnum.Varchar100:
                    par.TypeName = "dbo.Varchar100ParameterList";
                    break;
                case VarcharParameterListEnum.Varchar255:
                    par.TypeName = "dbo.Varchar255ParameterList";
                    break;
                case VarcharParameterListEnum.Varchar510:
                    par.TypeName = "dbo.Varchar510ParameterList";
                    break;
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("textValue", typeof(string));
            par.Value = dt;
            if (values != null)
            {
                foreach (var value in values.Where(p => !string.IsNullOrWhiteSpace(p)))
                {
                    dt.Rows.Add(value);
                }
            }
            return par;
        }
        */

        public enum VarcharParameterListEnum
        {
            Varchar2,
            Varchar15,
            Varchar50,
            Varchar100,
            Varchar255,
            Varchar510
        }

        #region Async

        public static Task<List<T>> SqlQueryAsync<T>(this DatabaseFacade database, string query, params SqlParameter[] parameters)
        {
            return SqlQueryAsync<T>(database, query, null, CommandType.StoredProcedure, parameters);
        }

        public static Task<List<T>> SqlQueryAsync<T>(this DatabaseFacade database, string query, CommandType commandType, params SqlParameter[] parameters)
        {
            return SqlQueryAsync<T>(database, query, null, commandType, parameters);
        }

        public static Task<List<T>> SqlQueryAsync<T>(this DatabaseFacade database, string query, int? commandTimeout, params SqlParameter[] parameters)
        {
            return SqlQueryAsync<T>(database, query, commandTimeout, CommandType.StoredProcedure, parameters);
        }

        public static async Task<List<T>> SqlQueryAsync<T>(this DatabaseFacade database, string query, int? commandTimeout, CommandType commandType, params SqlParameter[] parameters)
        {
            using (var cmd = database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = commandType;
                if (commandTimeout.HasValue)
                {
                    cmd.CommandTimeout = commandTimeout.Value;
                }
                cmd.Parameters.AddRange(parameters);

                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }

                try
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        return reader.MapToList<T>();
                    }
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }
        }

        #endregion

        #region Sync

        public static List<T> SqlQuery<T>(this DatabaseFacade database, string query, params SqlParameter[] parameters)
        {
            return SqlQuery<T>(database, query, null, CommandType.StoredProcedure, parameters);
        }

        public static List<T> SqlQuery<T>(this DatabaseFacade database, string query, CommandType commandType, params SqlParameter[] parameters)
        {
            return SqlQuery<T>(database, query, null, commandType, parameters);
        }

        public static List<T> SqlQuery<T>(this DatabaseFacade database, string query, int? commandTimeout, params SqlParameter[] parameters)
        {
            return SqlQuery<T>(database, query, commandTimeout, CommandType.StoredProcedure, parameters);
        }

        public static List<T> SqlQuery<T>(this DatabaseFacade database, string query, int? commandTimeout, CommandType commandType, params SqlParameter[] parameters)
        {
            bool shouldCloseTransaction = true;

            using (var cmd = database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = commandType;
                if (commandTimeout.HasValue)
                {
                    cmd.CommandTimeout = commandTimeout.Value;
                }

                if (database.CurrentTransaction != null)
                {
                    cmd.Transaction = database.CurrentTransaction.GetDbTransaction();
                }
                cmd.Parameters.AddRange(parameters);

                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }
                else
                {
                    shouldCloseTransaction = false;
                }

                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.MapToList<T>();
                    }
                }
                finally
                {
                    if (shouldCloseTransaction && cmd.Connection.State != System.Data.ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
        }

        #endregion
    }
}
