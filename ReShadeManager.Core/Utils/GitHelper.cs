using System;
using System.Diagnostics;
using System.IO;

namespace ReShadeManager.Core.Utils
{
	public sealed class GitHelper
	{
		public string GitCommandLine { get; }

		public GitHelper(string commandLine)
		{
			GitCommandLine = commandLine;
		}

		public void Clone(string url, string destination)
		{
			var git = Process.Start(new ProcessStartInfo
			{
				FileName = GitCommandLine,
				ArgumentList = { "clone", url, destination },
				UseShellExecute = true,
				CreateNoWindow = true,
				WindowStyle = ProcessWindowStyle.Hidden
			});
			git.WaitForExit();

			if (git.ExitCode != 0)
				throw new Exception(
					$"Failed to clone repository {url} into {destination}, " +
					$"with exit code {git.ExitCode}");
		}

		public void Checkout(string repositoryPath, string branch)
		{
			var git = Process.Start(new ProcessStartInfo
			{
				FileName = GitCommandLine,
				ArgumentList = { "checkout", "origin", branch },
				WorkingDirectory = repositoryPath,
				UseShellExecute = true,
				CreateNoWindow = true,
				WindowStyle = ProcessWindowStyle.Hidden
			});
			git.WaitForExit();

			if (git.ExitCode != 0)
				throw new Exception(
					$"Failed to checkout origin/{branch} in {repositoryPath}, " +
					$"with exit code {git.ExitCode}");
		}

		public void Pull(string repositoryPath, string branch)
		{
			var git = Process.Start(new ProcessStartInfo
			{
				FileName = GitCommandLine,
				ArgumentList = { "pull", "origin", branch },
				WorkingDirectory = repositoryPath,
				UseShellExecute = true,
				CreateNoWindow = true,
				WindowStyle = ProcessWindowStyle.Hidden
			});
			git.WaitForExit();

			if (git.ExitCode != 0)
				throw new Exception(
					$"Failed to pull from origin/{branch} in {repositoryPath}, " +
					$"with exit code {git.ExitCode}");
		}

		public static bool TryFindGitCommandline(out string commandLine)
		{
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = "git",
					UseShellExecute = true,
					CreateNoWindow = true,
					WindowStyle = ProcessWindowStyle.Hidden
				});
				commandLine = "git";
				return true;
			}
			catch { }

			foreach (var p in new[]
			{
				"C:/Program Files/Git/bin/git.exe",
				"C:/Program Files (x86)/Git/bin/git.exe",
				"C:/Program Files (x86)/Git/bin/git.exe",
				"./git.exe",
				"/usr/bin/git",
				"./git"
			})
			{
				if (File.Exists(p))
				{
					commandLine = p;
					return true;
				}
			}

			commandLine = "";
			return false;
		}
	}
}
