using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace FluentSecurity.Scanning
{
	public class ApplySecurityProfilesConvention : IScannerConvention
	{
		public void Process(Type type, ScannerContext context, ScannerRegistry registry)
		{
			var securityProfileType = typeof(SecurityProfile);

			if (securityProfileType.IsAssignableFrom(type) &&
				securityProfileType != type &&
				context.ContainsProfile(type) == false)
			{
				var profileToApply = (SecurityProfile)Activator.CreateInstance(type);
				context.Configuration.ApplyProfile(profileToApply);
			}
		}
	}

	internal class ControllerTypeFilterConvention : IScannerConvention
	{
		public void Process(Type type, ScannerContext context, ScannerRegistry registry)
		{
			if (typeof(IController).IsAssignableFrom(type))
				registry.AddType(type);
		}
	}

	public interface IScannerConvention
	{
		void Process(Type type, ScannerContext context, ScannerRegistry registry);
	}

	public static class AssemblyScannerExtensions
	{
		public static void LookForProfiles(this AssemblyScanner scanner)
		{
			scanner.With<ApplySecurityProfilesConvention>();
		}
	}

	public class ScannerContext
	{
		private readonly List<ProfileImport> _profiles = new List<ProfileImport>();
		
		public TypePool Types { get; private set; }
		public RootConfigurationExpression Configuration { get; private set; }

		public ScannerContext(RootConfigurationExpression rootConfigurationExpression)
		{
			Configuration = rootConfigurationExpression;
			Types = new TypePool();
		}

		public IEnumerable<Type> GetImportedProfiles()
		{
			return _profiles.Where(pi => pi.ImportCompleted).Select(pi => pi.ProfileType).ToList();
		}

		public bool ContainsProfile(Type profileType)
		{
			return _profiles.Any(pi => pi.ProfileType == profileType);
		}

		public void BeginProfileImport(Type profileType)
		{
			var profileImport = new ProfileImport(profileType);
			_profiles.Add(profileImport);
		}

		public void EndProfileImport(Type profileType)
		{
			var profileImport = _profiles.Single(pi => pi.ProfileType == profileType);
			profileImport.Completed();
		}
	}

	public class ScannerRegistry
	{
		private readonly List<Type> _types = new List<Type>();

		internal void AddType(Type type)
		{
			if (_types.Contains(type) == false)
				_types.Add(type);
		}

		public IEnumerable<Type> GetRegisteredTypes()
		{
			return _types;
		}
	}

	public class ProfileImport
	{
		public Type ProfileType { get; private set; }
		public bool ImportCompleted { get; private set; }

		public ProfileImport(Type profileType)
		{
			ProfileType = profileType;
			ImportCompleted = false;
		}

		public void Completed()
		{
			ImportCompleted = true;
		}
	}

	public class TypePool
	{
		private readonly Dictionary<Assembly, Type[]> _types = new Dictionary<Assembly, Type[]>();

		public IEnumerable<Type> For(IEnumerable<Assembly> assemblies, CompositeFilter<Type> filter)
		{
			return assemblies.SelectMany(assembly =>
			{
				if (_types.ContainsKey(assembly) == false)
					AddTypesFromAssembly(assembly);

				return _types[assembly].Where(filter.Matches);
			});
		}

		private void AddTypesFromAssembly(Assembly assembly)
		{
			if (_types.Keys.Contains(assembly) == false)
				_types.Add(assembly, assembly.GetExportedTypes());
		}
	}

	public class CompositeFilter<T>
	{
		private readonly CompositePredicate<T> _excludes = new CompositePredicate<T>();
		private readonly CompositePredicate<T> _includes = new CompositePredicate<T>();

		public CompositePredicate<T> Includes { get { return _includes; } }
		public CompositePredicate<T> Excludes { get { return _excludes; } }

		public bool Matches(T target)
		{
			return Includes.MatchesAny(target) && Excludes.DoesNotMatcheAny(target);
		}
	}

	public class CompositePredicate<T>
	{
		private readonly List<Func<T, bool>> _list = new List<Func<T, bool>>();
		private Func<T, bool> _matchesAll = x => true;
		private Func<T, bool> _matchesAny = x => true;
		private Func<T, bool> _matchesNone = x => false;

		public void Add(Func<T, bool> filter)
		{
			_matchesAll = x => _list.All(predicate => predicate(x));
			_matchesAny = x => _list.Any(predicate => predicate(x));
			_matchesNone = x => !MatchesAny(x);

			_list.Add(filter);
		}

		public static CompositePredicate<T> operator +(CompositePredicate<T> invokes, Func<T, bool> filter)
		{
			invokes.Add(filter);
			return invokes;
		}

		public bool MatchesAll(T target)
		{
			return _matchesAll(target);
		}

		public bool MatchesAny(T target)
		{
			return _matchesAny(target);
		}

		public bool MatchesNone(T target)
		{
			return _matchesNone(target);
		}

		public bool DoesNotMatcheAny(T target)
		{
			return _list.Count == 0 ? true : !MatchesAny(target);
		}
	}

}