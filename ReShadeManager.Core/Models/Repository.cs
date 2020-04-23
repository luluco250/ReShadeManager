namespace ReShadeManager.Core.Models
{
	public class Repository
	{
		public string Name { get; set; } = "";

		public string Url { get; set; } = "";

		public string Branch { get; set; } = "master";

		public Repository() {}

		public Repository(string name, string url, string branch = "master")
		{
			Name = name;
			Url = url;
			Branch = branch;
		}

		public override string ToString()
			=> Name;

		public void Deconstruct(out string name, out string url, out string branch)
		{
			name = Name;
			url =  Url;
			branch = Branch;
		}
	}
}
