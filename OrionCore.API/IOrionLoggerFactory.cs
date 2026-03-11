using System;

namespace Orion.Api
{
    /// <summary>Orion 記錄器工廠介面。</summary>
    public interface IOrionLoggerFactory
    {
        /// <summary>建立預設 logger。</summary>
        /// <returns>Logger 實例。</returns>
        IOrionLogger Create();

        /// <summary>依型別建立 logger。</summary>
        /// <param name="type">目標型別。</param>
        /// <returns>Logger 實例。</returns>
        IOrionLogger Create(Type type);

        /// <summary>依名稱建立 logger。</summary>
        /// <param name="name">Logger 名稱。</param>
        /// <returns>Logger 實例。</returns>
        IOrionLogger Create(string name);
    }
}
