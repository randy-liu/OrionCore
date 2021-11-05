using System.Linq;
using CodeGenerator.Models;

namespace CodeGenerator
{
    /// <summary></summary>
    public static class MetaExtensions
    {
        public static bool Is(this ColumnMeta meta, params string[] columns)
        {
            return columns.Contains(meta.Name);
        }


        public static string EditBox(this ColumnMeta meta)
        {

            if (meta.Name == "RemarkText") { return "BsTextAreaFor"; }
            if (meta.IsEnum) { return "BsEnumDropDownListFor"; }
            if (meta.CodeType == "DateTime") { return "BsDateBoxFor"; }
            if (meta.CodeType == "TimeSpan") { return "BsTimeBoxFor"; }

            return "BsTextBoxFor";
        }


        public static string WhereBox(this ColumnMeta meta)
        {
            if (meta.IsBasic)
            {
                if (meta.CodeType == "DateTime")
                { return $"{meta.Name}"; }
                else
                { return $"{meta.Name}, Items.UserName"; }
            }

            if (meta.CodeType == "DateTime") { return $"{meta.Name}.Date"; }

            return meta.Name;
        }



        public static string DisplayBox(this ColumnMeta meta, string variable)
        {
            if (meta.IsBasic)
            {
                if (meta.CodeType == "DateTime")
                { return $"{variable}.{meta.Name}"; }
                else
                { return $"Html.ShowItem({variable}.{meta.Name}, Items.UserName)"; }
            }

            if (meta.IsEnum) { return $"Html.ShowItem({variable}.{meta.Name})"; }

            if (meta.CodeType == "DateTime") { return $"Html.ShowDate({variable}.{meta.Name})"; }

            return $"{variable}.{meta.Name}";
        }


    }
}
