using System;
using System.IO;
using System.Text;
using System.Text.Json;
using ReShadeManager.Core.Models;
using ReShadeManager.Core.Utils;

namespace ReShadeManager.Core
{
	public sealed class ReShadeManagerRuntime
	{
		readonly JsonSerializerOptions _loadOptions = new JsonSerializerOptions
		{
			AllowTrailingCommas = true,
			ReadCommentHandling = JsonCommentHandling.Skip,
			PropertyNameCaseInsensitive = true
		};

		readonly JsonSerializerOptions _saveOptions = new JsonSerializerOptions
		{
			WriteIndented = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};

		Config? _config = null;
		GitHelper? _git = null;

		public string RootPath { get; }

		public string ConfigPath
			=> $"{RootPath}/config.jsonc";

		public string RepositoriesPath
			=> $"{RootPath}/repositories";

		public Config Config
		{
			get
			{
				_config ??= Config.GetDefault();
				return _config;
			}
			set => _config = value;
		}

		public ReShadeManagerRuntime(string rootPath)
		{
			RootPath = rootPath;
		}

		/// <summary>
		/// Load the configuration file.
		/// The return value indicates whether a config file was found and
		/// loaded or the default has been used.
		/// </summary>
		/// <returns>
		/// True if a config file was found, false otherwise.
		/// </returns>
		public bool LoadConfig()
		{
			if (!File.Exists(ConfigPath))
			{
				Config = Config.GetDefault();
				return false;
			}

			var json = Encoding.UTF8.GetString(File.ReadAllBytes(ConfigPath));

			try
			{
				Config = JsonSerializer.Deserialize<Config>(json, _loadOptions);
				return true;
			}
			catch (JsonException e)
			{
				throw new Exception("Failed to load config file", e);
			}
		}

		public bool TryLoadGit()
		{
			if (!string.IsNullOrWhiteSpace(Config.GitCommandLine))
			{
				TryLoadGit(Config.GitCommandLine);
				return true;
			}

			if (!GitHelper.TryFindGitCommandline(out var cmdLine))
				return false;

			TryLoadGit(cmdLine);
			return true;
		}

		public void TryLoadGit(string cmdLine)
		{
			_git = new GitHelper(cmdLine);
			Config.GitCommandLine = cmdLine;
		}

		public void UpdateRepositories()
		{
			if (_git is null)
				throw new Exception("git has not been loaded");

			try
			{
				foreach (var (name, url, branch) in Config.Repositories)
				{
					var path = $"{RepositoriesPath}/{name}";

					if (Directory.Exists(path))
					{
						_git.Pull(path, branch);
					}
					else
					{
						_git.Clone(url, path);

						if (branch != "master")
							_git.Checkout(path, branch);
					}
				}
			}
			catch (Exception e)
			{
				throw new Exception($"Error while updating repositories", e);
			}
		}

		public void SaveConfig()
		{
			var json = JsonSerializer.Serialize(Config, _saveOptions);
			json.Replace("  ", "\t");

			File.WriteAllBytes(ConfigPath, Encoding.UTF8.GetBytes(json));
		}
	}
}
