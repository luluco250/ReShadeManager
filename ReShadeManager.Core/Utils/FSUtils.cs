using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using static ReShadeManager.Core.Utils.NativeMethods;

namespace ReShadeManager.Core.Utils
{
	public enum FSItemKind
	{
		None = 0,
		File,
		Directory,
		Link
	}

	public static class FSUtils
	{
		static Lazy<IReadOnlyList<string>> _path = new Lazy<IReadOnlyList<string>>(() =>
			(Environment.GetEnvironmentVariable("PATH") ?? "").Split(';'));

		public static IReadOnlyList<string> EnvPath
			=> _path.Value;

		public static bool IsInPath(string programName)
		{
			foreach (var p in EnvPath)
				if (p.EndsWith(programName))
					return true;

			return false;
		}

		/// <summary>
		/// Get information about the item in a given path.
		/// </summary>
		/// <param name="path">The file system path to check.</param>
		/// <param name="kind">The kind of item located in the path.</param>
		/// <returns>Whether an item exists in the path or not.</returns>
		public static bool GetItem(string path, out FSItemKind kind)
		{
			if (File.Exists(path))
			{
				var info = new FileInfo(path);

				kind = ((info.Attributes & FileAttributes.ReparsePoint) != 0)
					? FSItemKind.Link
					: FSItemKind.File;

				return true;
			}

			if (Directory.Exists(path))
			{
				kind = FSItemKind.Directory;
				return true;
			}

			kind = FSItemKind.None;
			return false;
		}

		public static bool Exists(string path, out bool isDirectory)
		{
			isDirectory = false;

			if (!GetItem(path, out var kind))
				return false;

			isDirectory = kind == FSItemKind.Directory;
			return true;
		}

		public static void CreateSymbolicLink(string path, string target)
		{
			if (!Exists(target, out var isDirectory))
				throw new ArgumentException(
					"target path does not exist",
					nameof(target));

			var dir = new DirectoryInfo(path);
			Directory.CreateDirectory(dir.Parent.FullName);

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				var flags = SYMBOLIC_LINK_FLAG_ALLOW_UNPRIVILEGED_CREATE;

				if (isDirectory)
					flags |= SYMBOLIC_LINK_FLAG_DIRECTORY;

				if (!CreateSymbolicLinkW(path, target, flags))
				{
					var error = new Win32Exception(Marshal.GetLastWin32Error()).Message;
					throw new Exception($"Failed to create symbolic link: {error}");
				}
			}
			else
			{
				var procInfo = new ProcessStartInfo
				{
					FileName = "/bin/ln",
					ArgumentList = {"-s"},
					WindowStyle = ProcessWindowStyle.Hidden,
					CreateNoWindow = true,
					UseShellExecute = true,
				};

				if (isDirectory)
					procInfo.ArgumentList.Add("-d");

				procInfo.ArgumentList.Add(Path.GetFullPath(target));
				procInfo.ArgumentList.Add(path);

				var ln = Process.Start(procInfo);
				ln.WaitForExit();

				if (ln.ExitCode != 0)
					throw new Exception("Failed to create symbolic link");
			}
		}
	}
}
