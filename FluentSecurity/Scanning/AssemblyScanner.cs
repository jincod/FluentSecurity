using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace FluentSecurity.Scanning
{
	public class AssemblyScanner
	{
		private readonly List<Assembly> _assemblies = new List<Assembly>();
		private readonly CompositeFilter<Type> _typeFilters = new CompositeFilter<Type>();
		private readonly List<IScannerConvention> _conventions = new List<IScannerConvention>();

		public void Assembly(Assembly assembly)
		{
			if (assembly == null) throw new ArgumentNullException("assembly");
			_assemblies.Add(assembly);
		}

		public void TheCallingAssembly()
		{
			var callingAssembly = FindCallingAssembly();
			if (callingAssembly != null) _assemblies.Add(callingAssembly);
		}

		private static Assembly FindCallingAssembly()
		{
			var thisAssembly = System.Reflection.Assembly.GetExecutingAssembly();
			Assembly callingAssembly = null;

			var trace = new StackTrace(false);
			for (var i = 0; i < trace.FrameCount; i++)
			{
				var frame = trace.GetFrame(i);
				var assembly = frame.GetMethod().DeclaringType.Assembly;
				if (assembly != thisAssembly)
				{
					callingAssembly = assembly;
					break;
				}
			}
			return callingAssembly;
		}

		public void With(IScannerConvention convention)
		{
			_conventions.Add(convention);
		}

		public void With<TConvention>() where TConvention : IScannerConvention, new()
		{
			With(new TConvention());
		}

		public void IncludeNamespaceContainingType<T>()
		{
			Func<Type, bool> predicate = type =>
			{
				var currentNamespace = type.Namespace ?? "";
				var expectedNamespace = typeof(T).Namespace ?? "";
				return currentNamespace.StartsWith(expectedNamespace);
			};

			_typeFilters.Includes.Add(predicate);
		}

		public ScannerRegistry Scan(ScannerContext context)
		{
			var registry = new ScannerRegistry();
			
			context.Types.For(_assemblies, _typeFilters).Each(type =>
				_conventions.Each(c => c.Process(type, context, registry))
				);

			return registry;
		}
	}
}