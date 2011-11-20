using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

		public void AssembliesFromApplicationBaseDirectory()
		{
			AssembliesFromApplicationBaseDirectory(a => true);
		}

		public void AssembliesFromApplicationBaseDirectory(Predicate<Assembly> assemblyFilter)
		{
			var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

			AssembliesFromPath(baseDirectory, assemblyFilter);
			
			var binPath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
			if (Directory.Exists(binPath))
				AssembliesFromPath(binPath, assemblyFilter);
		}

		public void AssembliesFromPath(string path)
		{
			AssembliesFromPath(path, a => true);
		}

		public void AssembliesFromPath(string path, Predicate<Assembly> assemblyFilter)
		{
			var assemblyPaths = Directory.GetFiles(path).Where(file =>
			{
				var extension = Path.GetExtension(file);
				return extension != null && (
					extension.Equals(".exe", StringComparison.OrdinalIgnoreCase) ||
					extension.Equals(".dll", StringComparison.OrdinalIgnoreCase)
					);
			});

			foreach (var assemblyPath in assemblyPaths)
			{
				Assembly assembly = null;
				try
				{
					assembly = System.Reflection.Assembly.LoadFrom(assemblyPath);
				}
				catch {}
				if (assembly != null && assemblyFilter(assembly)) Assembly(assembly);
			}
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