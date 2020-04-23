using System.Collections.Generic;

namespace ReShadeManager.Core.Models
{
	public sealed class Config
	{
		public string GitCommandLine { get; set; } = "";

		public List<Repository> Repositories { get; set; } = new List<Repository>();

		public static Config GetDefault() => new Config
		{
			Repositories =
			{
				new Repository(
					"reshade-shaders",
					"https://github.com/crosire/reshade-shaders",
					"slim"),
				new Repository(
					"FXShaders",
					"https://github.com/luluco250/FXShaders"),
				new Repository(
					"SweetFX",
					"https://github.com/CeeJayDK/SweetFX"),
				new Repository(
					"qUINT",
					"https://github.com/martymcmodding/qUINT"),
			}
		};
	}
}
