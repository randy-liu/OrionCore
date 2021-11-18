using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Orion.Api.Extensions
{
    /// <summary></summary>
    public static class NotifyPropertyExtensions
    {

        /// <summary></summary>
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
