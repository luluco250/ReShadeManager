using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace ReShadeManager.Models
{
	sealed class ConfigManager
	{
		JsonSerializerOptions _jsonLoadOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			ReadCommentHandling = JsonCommentHandling.Skip,
			AllowTrailingCommas = true
		};

		JsonSerializerOptions _jsonSaveOptions = new JsonSerializerOptions
		{
			WriteIndented = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};

		public string ConfigPath { get; } =
			Path.Combine(
				AppDomain.CurrentDomain.BaseDirectory ?? "",
				"config.jsonc");

		public Config GetDefaultConfig() => new Config
		{
			Repositories = new List<Repository>
			{
				new Repository(
					"reshade-shaders",
					"https://github.com/crosire/reshade-shaders"),
				new Repository(
					"SweetFX",
					"https://github.com/CeeJayDK/SweetFX"),
				new Repository(
					"FXShaders",
					"https://github.com/luluco250/FXShaders"),
			}
		};

		public Config LoadConfig()
		{
			using (var file = File.OpenRead(ConfigPath))
				return
					JsonSerializer
						.DeserializeAsync<Config>(file, _jsonLoadOptions)
						.Result;
		}

		public void SaveConfig(Config config)
		{
			var json = JsonSerializer.Serialize(config, _jsonSaveOptions);
			json = json.Replace("  ", "\t");

			File.WriteAllBytes(ConfigPath, Encoding.UTF8.GetBytes(json));
		}

		public Config LoadConfigOrGetDefault()
		{
			if (File.Exists(ConfigPath))
				return LoadConfig();

			return GetDefaultConfig();
		}
	}
}
