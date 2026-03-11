using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Orion.Api.Extensions
{
	/// <summary>提供 `DbConnection` 常用資料存取擴充方法。</summary>
    public static class ConnectionExtensions
    {
        /// <summary>取得可用的命令物件，必要時會先開啟連線。</summary>
        /// <param name="cnt">要建立命令的資料庫連線。</param>
        /// <returns>已綁定連線的 `DbCommand`。</returns>
        private static DbCommand connectCommand(DbConnection cnt)
        {
            if (!cnt.State.HasFlag(ConnectionState.Open)) { cnt.Open(); }
            return cnt.CreateCommand();
        }


		/// <summary>依指定隔離層級建立交易。</summary>
        /// <param name="cnt">要啟用交易的資料庫連線。</param>
        /// <param name="isolationLevel">交易使用的隔離層級。</param>
        /// <returns>已開始的交易物件。</returns>
        private static DbTransaction tx(DbConnection cnt, IsolationLevel isolationLevel)
        {
            if (!cnt.State.HasFlag(ConnectionState.Open)) { cnt.Open(); }
            return cnt.BeginTransaction(isolationLevel);
        }


        /// <summary>v0: 在交易期間可以讀取 Volatile (易失性)資料，但無法修改該資料，且不能加入新資料。</summary>
        /// <param name="cnt">要啟用交易的資料庫連線。</param>
        /// <returns>可序列化隔離層級交易物件。</returns>
        public static DbTransaction TxSerializable(this DbConnection cnt) { return tx(cnt, IsolationLevel.Serializable); }

        /// <summary>v1: 在交易期間可以讀取 Volatile (易失性)資料，但無法修改該資料。 在交易期間可以加入新資料。</summary>
        /// <param name="cnt">要啟用交易的資料庫連線。</param>
        /// <returns>可重複讀取隔離層級交易物件。</returns>
        public static DbTransaction TxRepeatableRead(this DbConnection cnt) { return tx(cnt, IsolationLevel.RepeatableRead); }

        /// <summary>v2: 在交易期間無法讀取 Volatile (易失性)資料，但可以修改該資料。</summary>
        /// <param name="cnt">要啟用交易的資料庫連線。</param>
        /// <returns>讀取已認可隔離層級交易物件。</returns>
        public static DbTransaction TxReadCommitted(this DbConnection cnt) { return tx(cnt, IsolationLevel.ReadCommitted); }

        /// <summary>v3: 在交易期間可以讀取和修改 Volatile (易失性)資料。[髒讀]</summary>
        /// <param name="cnt">要啟用交易的資料庫連線。</param>
        /// <returns>讀取未認可隔離層級交易物件。</returns>
        public static DbTransaction TxReadUncommitted(this DbConnection cnt) { return tx(cnt, IsolationLevel.ReadUncommitted); }






        /// <summary>建立原始 SQL 命令。</summary>
        /// <param name="cnt">用來建立命令的資料庫連線。</param>
        /// <param name="commandText">要直接執行的 SQL 命令文字。</param>
        /// <returns>建立後的 `DbCommand`。</returns>
        public static DbCommand CreateCommandRaw(this DbConnection cnt, string commandText)
        {
            DbCommand command = connectCommand(cnt);
            command.CommandText = commandText;
            return command;
        }


        /// <summary>由插值字串建立參數化 SQL 命令。</summary>
        /// <param name="cnt">用來建立命令的資料庫連線。</param>
        /// <param name="commandText">以插值字串表示的 SQL 命令，值會轉為參數。</param>
        /// <returns>建立後的 `DbCommand`。</returns>
        public static DbCommand CreateCommand(this DbConnection cnt, FormattableString commandText)
        {
            DbCommand command = connectCommand(cnt);

            command.CommandText = "";
            command.AddCommand(commandText);

            return command;
        }



        /// <summary>執行非查詢命令並回傳影響筆數。</summary>
        /// <param name="cnt">用來執行命令的資料庫連線。</param>
        /// <param name="commandText">以插值字串表示的非查詢 SQL 命令。</param>
        /// <returns>影響筆數。</returns>
        public static int ExecuteCommand(this DbConnection cnt, FormattableString commandText)
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.ExecuteNonQuery();
        }


        /// <summary>判斷查詢是否存在資料。</summary>
        /// <param name="cnt">用來執行查詢的資料庫連線。</param>
        /// <param name="commandText">以插值字串表示的查詢命令。</param>
        /// <returns>存在資料時回傳 `true`。</returns>
        public static bool IsDataExists(this DbConnection cnt, FormattableString commandText)
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.IsDataExists();
        }



        /// <summary>執行查詢並回傳第一欄值。</summary>
        /// <typeparam name="T">回傳值型別。</typeparam>
        /// <param name="cnt">用來執行查詢的資料庫連線。</param>
        /// <param name="commandText">以插值字串表示的查詢命令。</param>
        /// <returns>第一欄值。</returns>
        public static T FetchOne<T>(this DbConnection cnt, FormattableString commandText)
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.FetchOne<T>();
        }



        /// <summary>執行查詢並回傳 `DataTable`。</summary>
        /// <param name="cnt">用來執行查詢的資料庫連線。</param>
        /// <param name="commandText">以插值字串表示的查詢命令。</param>
        /// <returns>查詢結果 `DataTable`。</returns>
        public static DataTable FetchDataTable(this DbConnection cnt, FormattableString commandText)
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.FetchDataTable();
        }



        /// <summary>執行查詢並回傳第一筆 `DataRow`。</summary>
        /// <param name="cnt">用來執行查詢的資料庫連線。</param>
        /// <param name="commandText">以插值字串表示的查詢命令。</param>
        /// <returns>第一筆資料列。</returns>
        public static DataRow FetchDataRow(this DbConnection cnt, FormattableString commandText)
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.FetchDataRow();
        }


        /// <summary>執行查詢並轉為模型清單。</summary>
        /// <typeparam name="TModel">目標模型型別。</typeparam>
        /// <param name="cnt">用來執行查詢的資料庫連線。</param>
        /// <param name="commandText">以插值字串表示的查詢命令。</param>
        /// <returns>模型清單。</returns>
        public static List<TModel> FetchList<TModel>(this DbConnection cnt, FormattableString commandText) where TModel : new()
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.FetchList<TModel>();
        }


        /// <summary>執行查詢並回傳第一筆模型資料。</summary>
        /// <typeparam name="TModel">目標模型型別。</typeparam>
        /// <param name="cnt">用來執行查詢的資料庫連線。</param>
        /// <param name="commandText">以插值字串表示的查詢命令。</param>
        /// <returns>第一筆模型資料。</returns>
        public static TModel FetchModel<TModel>(this DbConnection cnt, FormattableString commandText) where TModel : new()
        {
            using DbCommand command = CreateCommand(cnt, commandText);
            return command.FetchModel<TModel>();
        }




        /*#[Insert]###########################################################################*/

        /// <summary>插入資料。</summary>
        /// <param name="cnt">用來執行新增命令的資料庫連線。</param>
        /// <param name="tableName">目標資料表名稱。</param>
        /// <param name="nameValues">要新增的欄位名稱與值。</param>
        /// <returns>影響筆數。</returns>
        public static int Insert(this DbConnection cnt, string tableName, object nameValues)
        {
            using DbCommand command = connectCommand(cnt);
            return command.BuildInsert(tableName, nameValues).ExecuteNonQuery();
        }



        /*#[Update]###########################################################################*/

        /// <summary>更新資料。</summary>
        /// <param name="cnt">用來執行更新命令的資料庫連線。</param>
        /// <param name="tableName">目標資料表名稱。</param>
        /// <param name="setValues">要更新的欄位名稱與值。</param>
        /// <param name="whereValues">篩選要更新資料的條件欄位與值。</param>
        /// <returns>影響筆數。</returns>
        public static int Update(this DbConnection cnt, string tableName, object setValues, object whereValues)
        {
            using DbCommand command = connectCommand(cnt);
            return command.BuildUpdate(tableName, setValues, whereValues).ExecuteNonQuery();
        }



        /*#[Delete]###########################################################################*/

        /// <summary>刪除資料。</summary>
        /// <param name="cnt">用來執行刪除命令的資料庫連線。</param>
        /// <param name="tableName">目標資料表名稱。</param>
        /// <param name="whereValues">篩選要刪除資料的條件欄位與值。</param>
        /// <returns>影響筆數。</returns>
        public static int Delete(this DbConnection cnt, string tableName, object whereValues)
        {
            using DbCommand command = connectCommand(cnt);
            return command.BuildDelete(tableName, whereValues).ExecuteNonQuery();
        }



        /*#[Procedure]###########################################################################*/

        /// <summary>預存程序呼叫</summary>
        /// <param name="cnt">用來執行預存程序的資料庫連線。</param>
        /// <param name="procedureName">要呼叫的預存程序名稱。</param>
        /// <param name="parameters">依序傳入預存程序的參數值。</param>
        /// <returns>影響筆數。</returns>
        public static int Procedure(this DbConnection cnt, string procedureName, params object[] parameters)
        {
            using DbCommand command = connectCommand(cnt);
            return command.BuildProcedure(procedureName, parameters).ExecuteNonQuery();
        }






        /*#[DbParameter]###########################################################################*/

        /// <summary>建立輸出參數。</summary>
        /// <param name="cnt">用來取得提供者工廠的資料庫連線。</param>
        /// <param name="type">輸出參數的資料型別。</param>
        /// <param name="size">輸出參數長度；非字串型別可使用預設值 0。</param>
        /// <returns>已設定為輸出方向的參數物件。</returns>
        private static DbParameter createOut(DbConnection cnt, DbType type, int size = 0)
        {
            DbParameter parame = DbProviderFactories.GetFactory(cnt).CreateParameter();
            parame.Size = size;
            parame.DbType = type;
            parame.Direction = ParameterDirection.Output;

            return parame;
        }
        /// <summary>建立布林型輸出參數。</summary>
        /// <param name="cnt">用來建立參數的資料庫連線。</param>
        /// <returns>輸出參數物件。</returns>
        public static DbParameter OutBoolean(this DbConnection cnt) { return createOut(cnt, DbType.Boolean); }
        /// <summary>建立 Int32 輸出參數。</summary>
        /// <param name="cnt">用來建立參數的資料庫連線。</param>
        /// <returns>輸出參數物件。</returns>
        public static DbParameter OutInt32(this DbConnection cnt) { return createOut(cnt, DbType.Int32); }
        /// <summary>建立 Decimal 輸出參數。</summary>
        /// <param name="cnt">用來建立參數的資料庫連線。</param>
        /// <returns>輸出參數物件。</returns>
        public static DbParameter OutDecimal(this DbConnection cnt) { return createOut(cnt, DbType.Decimal); }
        /// <summary>建立字串輸出參數（長度 256）。</summary>
        /// <param name="cnt">用來建立參數的資料庫連線。</param>
        /// <returns>輸出參數物件。</returns>
        public static DbParameter OutString(this DbConnection cnt) { return createOut(cnt, DbType.String, 256); }
    }

}
