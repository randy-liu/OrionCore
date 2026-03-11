using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Orion.Api.Models
{

    /// <summary>分頁結果共用資訊介面。</summary>
    public interface IPagination
    {

        /// <summary>當前頁號</summary>
        int PageNumber { get; }

        /// <summary>每頁大小</summary>
        int PageSize { get; }

        /// <summary>總筆數</summary>
        int TotalItems { get; }

        /// <summary>當頁第一筆編號</summary>
        int FirstItem { get; }

        /// <summary>是否有下一頁</summary>
        bool HasNextPage { get; }

        /// <summary>是否有前一頁</summary>
        bool HasPreviousPage { get; }

        /// <summary>當頁最後一筆編號</summary>
        int LastItem { get; }

        /// <summary>總共頁數</summary>
        int TotalPages { get; }
    }




    /*=======================================================*/

    /// <summary>泛型分頁結果資料。</summary>
    [DataContract]
    public class Pagination<T> : IPagination
    {

        /// <summary>建立不含資料列的分頁結果。</summary>
        /// <returns>清單為空的分頁實例。</returns>
        public static Pagination<T> Empty()
        {
            return new Pagination<T> { List = new List<T>() };
        }

        /// <summary>資料清單</summary>
        [DataMember]
        public List<T> List { get; set; }

        /// <summary>當前頁號</summary>
        [DataMember]
        public int PageNumber { get; set; }

        /// <summary>每頁大小</summary>
        [DataMember]
        public int PageSize { get; set; }

        /// <summary>總筆數</summary>
        [DataMember]
        public int TotalItems { get; set; }


        /// <summary>當頁第一筆編號</summary>
        public int FirstItem
        {
            get { return (PageNumber - 1) * PageSize + 1; }
        }

        /// <summary>是否有下一頁</summary>
        public bool HasNextPage
        {
            get { return PageNumber < TotalPages; }
        }

        /// <summary>是否有前一頁</summary>
        public bool HasPreviousPage
        {
            get { return PageNumber > 1; }
        }

        /// <summary>當頁最後一筆編號</summary>
        public int LastItem
        {
            get { return FirstItem + List.Count - 1; }
        }

        /// <summary>總共頁數</summary>
        public int TotalPages
        {
            get 
            {
                if (TotalItems <= 0) { return 0; }
                if (PageSize <= 0) { return 1; }
                return (int)Math.Ceiling(1.0 * TotalItems / PageSize); 
            }
        }
    }




    /*=======================================================*/

    /// <summary>以 <see cref="DataTable"/> 承載資料的分頁結果。</summary>
    public class DataTablePagination : IPagination
    {

        /// <summary>建立不含資料列的分頁結果。</summary>
        /// <returns><see cref="DataTable"/> 為空白資料表的分頁實例。</returns>
        public static DataTablePagination Empty()
        {
            return new DataTablePagination { DataTable = new DataTable() };
        }


        /// <summary>資料清單</summary>
        public DataTable DataTable { get; set; }

        /// <summary>當前頁號</summary>
        public int PageNumber { get; set; }

        /// <summary>每頁大小</summary>
        public int PageSize { get; set; }

        /// <summary>總筆數</summary>
        public int TotalItems { get; set; }



        /// <summary>當頁第一筆編號</summary>
        public int FirstItem
        {
            get { return ((PageNumber - 1) * PageSize) + 1; }
        }

        /// <summary>是否有下一頁</summary>
        public bool HasNextPage
        {
            get { return PageNumber < TotalPages; }
        }

        /// <summary>是否有前一頁</summary>
        public bool HasPreviousPage
        {
            get { return PageNumber > 1; }
        }

        /// <summary>當頁最後一筆編號</summary>
        public int LastItem
        {
            get { return FirstItem + DataTable.Rows.Count - 1; }
        }

        /// <summary>總共頁數</summary>
        public int TotalPages
        {
            get { return (int)Math.Ceiling(1.0 * TotalItems / PageSize); }
        }
    }



}
