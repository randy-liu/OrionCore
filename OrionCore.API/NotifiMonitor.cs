using System;
using System.Reflection;

namespace OrionCore.API
{
    /// <summary></summary>
    public class NotifiMonitor
    {
        /// <summary>執行對象</summary>
        public object Handle { get; private set; }

        /// <summary>執行對象類型</summary>
        public Type HandleType { get; private set; }

        /// <summary>執行方法</summary>
        public MethodInfo Method { get; private set; }

        /// <summary>執行總數</summary>
        public uint RunCount { get; private set; }

        /// <summary>最後一次的開始時間</summary>
        public DateTime LastBegin { get; private set; }

        /// <summary>最後一次的結束時間</summary>
        public DateTime LastEnd { get; private set; }

        /// <summary>最後一次的執行時間</summary>
        public double LastSeconds { get; private set; }

        /// <summary>最大的開始時間</summary>
        public DateTime MaxBegin { get; private set; }

        /// <summary>最大執行時間</summary>
        public double MaxSeconds { get; private set; }

        /// <summary>超過一秒的開始時間</summary>
        public DateTime ConsumeBegin { get; private set; }

        /// <summary>超過一秒執行平均</summary>
        public double ConsumeAvg { get; private set; }

        /// <summary>超過一秒的總數</summary>
        public uint ConsumeCount { get; private set; }

        /// <summary>超過一秒的百分比</summary>
        public double ConsumePercent { get; private set; }




        private readonly object _lockFlag = new object();

        /// <summary></summary>
        public NotifiMonitor(object handle, MethodInfo method)
        {
            Handle = handle;
            HandleType = handle.GetType();
            Method = method;
        }


        /// <summary></summary>
        public void Add(DateTime beginTime, DateTime endTime)
        {
            double duration = (endTime - beginTime).TotalSeconds;

            lock (_lockFlag)
            {
                LastBegin = beginTime;
                LastEnd = endTime;
                LastSeconds = duration;
                RunCount++;

                if (duration >= MaxSeconds)
                {
                    MaxBegin = beginTime;
                    MaxSeconds = duration;
                }

                if (duration >= 1)
                {
                    ConsumeCount++;
                    ConsumeBegin = beginTime;
                    ConsumeAvg += (duration - ConsumeAvg) / ConsumeCount;
                }

                ConsumePercent = 100.0 * ConsumeCount / RunCount;
            }
        }

    }
}
