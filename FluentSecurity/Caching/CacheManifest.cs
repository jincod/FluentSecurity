namespace FluentSecurity.Caching
{
	public class CacheManifest
	{
		public CacheManifest(Cache level, CacheKeyType keyType)
		{
			Level = level;
			KeyType = keyType;
		}

		public Cache Level { get; set; }
		public CacheKeyType KeyType { get; set; }
	}
}