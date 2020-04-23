using System;

namespace ReShadeManager.Models
{
	struct Repository : IEquatable<Repository>
	{
		public string Name { get; }

		public string Url { get; }

		public Repository(string name, string url)
		{
			Name = name;
			Url = url;
		}

		public override string ToString()
			=> $"[{Url} {Name}]";

		public override int GetHashCode()
			=> Name.GetHashCode() ^ Url.GetHashCode();

		public override bool Equals(object? obj)
		{
			if (obj is null)
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (obj is Repository other)
				return Equals(other);

			return false;
		}

		public bool Equals(Repository other)
			=> this.Name == other.Name && this.Url == other.Url;
	}
}
