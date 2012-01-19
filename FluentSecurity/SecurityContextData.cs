using System;
using System.Collections.Generic;

namespace FluentSecurity
{
	public class SecurityContextData
	{
		private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

		public T Get<T>(string uniqueKey = null) where T : class
		{
			var key = uniqueKey ?? "";
			
			var instance = _data.ContainsKey(key) ? _data[key] as T : null;
			if (instance == null)
			{
				key = typeof(T).Name;
				instance = _data.ContainsKey(key) ? _data[key] as T : null;
			}
			
			return instance;
		}

		public void Set<T>(T instance, string uniqueKey = null, bool replaceIfExists = false) where T : class
		{
			var key = uniqueKey ?? typeof(T).Name;
			if (_data.ContainsKey(key) && replaceIfExists == false)
				throw new ArgumentException(String.Concat("An instance of {0} with the key {1} already exists in the data dictionary.", typeof(T).Name, key), "instance");

			_data[key] = instance;
		}
	}
}