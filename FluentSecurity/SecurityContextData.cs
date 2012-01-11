using System;
using System.Collections.Generic;

namespace FluentSecurity
{
	public class SecurityContextData
	{
		private readonly Dictionary<Type, object> _data = new Dictionary<Type, object>();

		public T Get<T>() where T : class
		{
			var key = typeof(T);
			return _data.ContainsKey(key) ? _data[key] as T : null;
		}

		public void Set<T>(T instance, bool replaceIfExists = false) where T : class
		{
			var key = typeof(T);
			if (_data.ContainsKey(key) && replaceIfExists == false)
				throw new ArgumentException(String.Concat("An instance of {0} already exists in the data dictionary.", key.Name), "instance");

			_data.Add(key, instance);
		}	 
	}
}