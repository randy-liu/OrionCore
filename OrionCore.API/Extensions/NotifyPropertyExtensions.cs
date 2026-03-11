using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Orion.Api.Extensions
{
    /// <summary>提供 `INotifyPropertyChanged` 的通知觸發擴充方法。</summary>
    public static class NotifyPropertyExtensions
    {

        /// <summary>手動觸發指定屬性的 `PropertyChanged` 事件。</summary>
        /// <typeparam name="T">目標型別。</typeparam>
        /// <typeparam name="TProp">屬性型別。</typeparam>
        /// <param name="target">目標物件。</param>
        /// <param name="property">要觸發通知的屬性表達式。</param>
        public static void TriggerChanged<T, TProp>(this T target, Expression<Func<T, TProp>> property) where T : INotifyPropertyChanged
        {
            if (target == null) { return; }

            var field = typeof(T).GetField(nameof(INotifyPropertyChanged.PropertyChanged), BindingFlags.Instance | BindingFlags.NonPublic);
            var changedEvent = (PropertyChangedEventHandler)field.GetValue(target);
            if (changedEvent == null) { return; }

            PropertyInfo info = property.GetProperty();
            changedEvent.Invoke(target, new PropertyChangedEventArgs(info.Name));
        }

    }
}
