using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Orion.Api.Extensions
{
    public static class ConnectionExtensions
    {
        private static DbCommand connectCommand(DbConnection cnt)
        {
            if (!cnt.State.HasFlag(ConnectionState.Open)) { cnt.Open(); }
            return cnt.CreateCommand();
        }


        public static DbTransaction Transaction(this DbConnection cnt)
        {
            return Transaction(cnt, IsolationLevel.ReadCommitted);
        }


        public static DbTransaction Transaction(this DbConnection cnt, IsolationLevel isolationLevel)
        {
            if (!cnt.State.HasFlag(ConnectionState.Open)) { cnt.Open(); }
            return cnt.BeginTransaction(isolationLevel);
        }


        public static DbCommand CreateCommandRaw(this DbConnection cnt, string commandText)
        {
            DbCommand command = connectCommand(cnt);
            command.CommandText = commandText;
            return command;
        }


        public static DbCommand CreateCommand(this DbConnection cnt, FormattableString commandText)
        {
            DbCommand command = connectCommand(cnt);

            command.CommandText = "";
            command.AddCommand(commandText);

            return command;
        }



        public static int ExecuteCommand(this DbConnection cnt, FormattableString commandText)
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.ExecuteNonQuery();
        }


        public static bool IsDataExists(this DbConnection cnt, FormattableString commandText)
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.IsDataExists();
        }



        public static T FetchOne<T>(this DbConnection cnt, FormattableString commandText)
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.FetchOne<T>();
        }



        public static DataTable FetchDataTable(this DbConnection cnt, FormattableString commandText)
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.FetchDataTable();
        }



        public static DataRow FetchDataRow(this DbConnection cnt, FormattableString commandText)
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.FetchDataRow();
        }


        public static List<TModel> FetchList<TModel>(this DbConnection cnt, FormattableString commandText) where TModel : new()
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.FetchList<TModel>();
        }


        public static TModel FetchModel<TModel>(this DbConnection cnt, FormattableString commandText) where TModel : new()
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.FetchModel<TModel>();
        }




        /*#[Insert]###########################################################################*/

        public static int Insert(this DbConnection cnt, string tableName, object nameValues)
        {
            using DbCommand command = connectCommand(cnt);
            return command.BuildInsert(tableName, nameValues).ExecuteNonQuery();
        }



        /*#[Update]###########################################################################*/

        public static int Update(this DbConnection cnt, string tableName, object setValues, object whereValues)
        {
            using DbCommand command = connectCommand(cnt);
            return command.BuildUpdate(tableName, setValues, whereValues).ExecuteNonQuery();
        }



        /*#[Delete]###########################################################################*/

        public static int Delete(this DbConnection cnt, string tableName, object whereValues)
        {
            using DbCommand command = connectCommand(cnt);
            return command.BuildDelete(tableName, whereValues).ExecuteNonQuery();
        }



        /*#[Procedure]###########################################################################*/

        public static int Procedure(this DbConnection cnt, string procedureName, params object[] parameters)
        {
            using DbCommand command = connectCommand(cnt);
            return command.BuildProcedure(procedureName, parameters).ExecuteNonQuery();
        }






        /*#[DbParameter]###########################################################################*/

        private static DbParameter createOut(DbConnection cnt, DbType type, int size = 0)
        {
            DbParameter parame = DbProviderFactories.GetFactory(cnt).CreateParameter();
            parame.Size = size;
            parame.DbType = type;
            parame.Direction = ParameterDirection.Output;

            return parame;
        }
        public static DbParameter OutBoolean(this DbConnection cnt) { return createOut(cnt, DbType.Boolean); }
        public static DbParameter OutInt32(this DbConnection cnt) { return createOut(cnt, DbType.Int32); }
        public static DbParameter OutDecimal(this DbConnection cnt) { return createOut(cnt, DbType.Decimal); }
        public static DbParameter OutString(this DbConnection cnt) { return createOut(cnt, DbType.String, 256); }




    }

}
