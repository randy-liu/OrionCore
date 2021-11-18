using System;
using System.Collections.Generic;
using System.Reflection;

namespace Orion.Api
{
    /// <summary></summary>
    public class EnumAttributeProvider<TEnum, TMeta>
        where TEnum : struct
        where TMeta : Attribute, new()
    {
        private TMeta _defaultMeta = new TMeta();
        private Dictionary<TEnum, TMeta> _metas = new Dictionary<TEnum, TMeta>();

        /// <summary></summary>
        public EnumAttributeProvider()
        {
            Type type = typeof(TEnum);

            foreach (TEnum value in OrionUtils.GetEnumValues<TEnum>())
            {
                TMeta meta = type.GetField(value.ToString()).GetCustomAttribute<TMeta>();
                if (meta != null) { _metas[value] = meta; }
            }
        }


        /// <summary></summary>
        public TMeta this[TEnum value]
        {
            get { return _metas.ContainsKey(value) ? _metas[value] : _defaultMeta; }
        }

    }

}
