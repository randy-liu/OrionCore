using System;

namespace Orion.Api
{
    /// <summary>IJwLogger 介面</summary>
    public interface IOrionLoggerFactory
    {
        /// <summary></summary>
        IOrionLogger Create();

        /// <summary></summary>
        IOrionLogger Create(Type type);

        /// <summary></summary>
        IOrionLogger Create(string name);
    }
}
