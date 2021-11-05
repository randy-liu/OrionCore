using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace Orion.API.Models
{

	/// <summary></summary>
	public class ConfigurationElementCollection<T> : ConfigurationElementCollection, IEnumerable<T> where T : ConfigurationElement, new()
	{
		private Func<ConfigurationElement, object> _keySelect = x => x;


		/// <summary></summary>
		public ConfigurationElementCollection()
		{
			var keyProp = typeof(T).GetProperties().Where(p =>
			{
				var attr = p.GetCustomAttribute<ConfigurationPropertyAttribute>();
				return attr != null && attr.IsKey;
			}).FirstOrDefault();

			if (keyProp != null) { _keySelect = x => keyProp.GetValue(x); }
		}




		/// <summary></summary>
		public T this[int index]
		{
			get { return (T)base.BaseGet(index); }
			set
			{
				if (base.BaseGet(index) != null) { base.BaseRemoveAt(index); }
				this.BaseAdd(index, value);
			}
		}


		/// <summary></summary>
		public new T this[string name]
		{
			get { return (T)base.BaseGet(name); }
		}


		/// <summary></summary>
		public int IndexOf(T element)
		{
			return base.BaseIndexOf(element);
		}


		/// <summary></summary>
		public void Add(T element)
		{
			this.BaseAdd(element);
		}


		/// <summary></summary>
		public void Remove(T element)
		{
			int index = base.BaseIndexOf(element);
			if (index >= 0) { base.BaseRemoveAt(index); }
		}


		/// <summary></summary>
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}


		/// <summary></summary>
		public void Remove(string key)
		{
			base.BaseRemove(key);
		}



		/// <summary></summary>
		protected override ConfigurationElement CreateNewElement()
		{
			return new T();
		}


		/// <summary></summary>
		protected override object GetElementKey(ConfigurationElement target)
		{
			return _keySelect(target);
		}


		/// <summary></summary>
		public void Clear()
		{
			base.BaseClear();
		}




		/// <summary></summary>
		public new IEnumerator<T> GetEnumerator()
		{
			return this.OfType<T>().GetEnumerator();
		}



	}

}
