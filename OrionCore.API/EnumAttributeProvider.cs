using System;
using System.Collections.Generic;
using System.Reflection;

namespace Orion.Api
{
    /// <summary>提供列舉值對應屬性中繼資料的存取器。</summary>
    public class EnumAttributeProvider<TEnum, TMeta>
        where TEnum : struct
        where TMeta : Attribute, new()
    {
        private TMeta _defaultMeta = new TMeta();
        private Dictionary<TEnum, TMeta> _metas = new Dictionary<TEnum, TMeta>();

        /// <summary>建立列舉屬性提供者並快取列舉欄位上的屬性。</summary>
        public EnumAttributeProvider()
        {
            Type type = typeof(TEnum);

            foreach (TEnum value in OrionUtils.GetEnumValues<TEnum>())
            {
                TMeta meta = type.GetField(value.ToString()).GetCustomAttribute<TMeta>();
                if (meta != null) { _metas[value] = meta; }
            }
        }


        /// <summary>依列舉值取得對應的屬性中繼資料。</summary>
        /// <param name="value">列舉值。</param>
        /// <returns>對應的屬性；若未標註則回傳預設 <typeparamref name="TMeta"/> 實例。</returns>
        public TMeta this[TEnum value]
        {
            get { return _metas.ContainsKey(value) ? _metas[value] : _defaultMeta; }
        }

    }

}
