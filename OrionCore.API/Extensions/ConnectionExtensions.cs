using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Orion.Api.Extensions
{
	/// <summary></summary>
    public static class ConnectionExtensions
    {
        private static DbCommand connectCommand(DbConnection cnt)
        {
            if (!cnt.State.HasFlag(ConnectionState.Open)) { cnt.Open(); }
            return cnt.CreateCommand();
        }


        /// <summary></summary>
        private static DbTransaction tx(DbConnection cnt, IsolationLevel isolationLevel)
        {
            if (!cnt.State.HasFlag(ConnectionState.Open)) { cnt.Open(); }
            return cnt.BeginTransaction(isolationLevel);
        }


        /// <summary>v0: 在交易期間可以讀取 Volatile (易失性)資料，但無法修改該資料，且不能加入新資料。</summary>
        public static DbTransaction TxSerializable(this DbConnection cnt) { return tx(cnt, IsolationLevel.Serializable); }

        /// <summary>v1: 在交易期間可以讀取 Volatile (易失性)資料，但無法修改該資料。 在交易期間可以加入新資料。</summary>
        public static DbTransaction TxRepeatableRead(this DbConnection cnt) { return tx(cnt, IsolationLevel.RepeatableRead); }

        /// <summary>v2: 在交易期間無法讀取 Volatile (易失性)資料，但可以修改該資料。</summary>
        public static DbTransaction TxReadCommitted(this DbConnection cnt) { return tx(cnt, IsolationLevel.ReadCommitted); }

        /// <summary>v3: 在交易期間可以讀取和修改 Volatile (易失性)資料。[髒讀]</summary>
        public static DbTransaction TxReadUncommitted(this DbConnection cnt) { return tx(cnt, IsolationLevel.ReadUncommitted); }






        /// <summary></summary>
        public static DbCommand CreateCommandRaw(this DbConnection cnt, string commandText)
        {
            DbCommand command = connectCommand(cnt);
            command.CommandText = commandText;
            return command;
        }


        /// <summary></summary>
        public static DbCommand CreateCommand(this DbConnection cnt, FormattableString commandText)
        {
            DbCommand command = connectCommand(cnt);

            command.CommandText = "";
            command.AddCommand(commandText);

            return command;
        }



        /// <summary></summary>
        public static int ExecuteCommand(this DbConnection cnt, FormattableString commandText)
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.ExecuteNonQuery();
        }


        /// <summary></summary>
        public static bool IsDataExists(this DbConnection cnt, FormattableString commandText)
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.IsDataExists();
        }



        /// <summary></summary>
        public static T FetchOne<T>(this DbConnection cnt, FormattableString commandText)
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.FetchOne<T>();
        }



        /// <summary></summary>
        public static DataTable FetchDataTable(this DbConnection cnt, FormattableString commandText)
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.FetchDataTable();
        }



        /// <summary></summary>
        public static DataRow FetchDataRow(this DbConnection cnt, FormattableString commandText)
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.FetchDataRow();
        }


        /// <summary></summary>
        public static List<TModel> FetchList<TModel>(this DbConnection cnt, FormattableString commandText) where TModel : new()
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.FetchList<TModel>();
        }


        /// <summary></summary>
        public static TModel FetchModel<TModel>(this DbConnection cnt, FormattableString commandText) where TModel : new()
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.FetchModel<TModel>();
        }




        /*#[Insert]###########################################################################*/

        /// <summary></summary>
        public static int Insert(this DbConnection cnt, string tableName, object nameValues)
        {
            using DbCommand command = connectCommand(cnt);
            return command.BuildInsert(tableName, nameValues).ExecuteNonQuery();
        }



        /*#[Update]###########################################################################*/

        /// <summary></summary>
        public static int Update(this DbConnection cnt, string tableName, object setValues, object whereValues)
        {
            using DbCommand command = connectCommand(cnt);
            return command.BuildUpdate(tableName, setValues, whereValues).ExecuteNonQuery();
        }



        /*#[Delete]###########################################################################*/

        /// <summary></summary>
        public static int Delete(this DbConnection cnt, string tableName, object whereValues)
        {
            using DbCommand command = connectCommand(cnt);
            return command.BuildDelete(tableName, whereValues).ExecuteNonQuery();
        }



        /*#[Procedure]###########################################################################*/

        /// <summary></summary>
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
        /// <summary></summary>
        public static DbParameter OutBoolean(this DbConnection cnt) { return createOut(cnt, DbType.Boolean); }
        /// <summary></summary>
        public static DbParameter OutInt32(this DbConnection cnt) { return createOut(cnt, DbType.Int32); }
        /// <summary></summary>
        public static DbParameter OutDecimal(this DbConnection cnt) { return createOut(cnt, DbType.Decimal); }
        /// <summary></summary>
        public static DbParameter OutString(this DbConnection cnt) { return createOut(cnt, DbType.String, 256); }




    }

}
