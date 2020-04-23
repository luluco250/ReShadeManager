using System;
using System.Collections.Generic;
using System.IO;
using ReShadeManager.CLI.Utils;
using ReShadeManager.Core;
using ReShadeManager.Core.Utils;

namespace ReShadeManager.CLI.Commands
{
	sealed class Context
	{
		public ReShadeManagerRuntime Runtime { get; }

		public IReadOnlyDictionary<string, Action<IReadOnlyList<string>>> Commands { get; }

		public bool ShouldQuit { get; private set; } = false;

		public Context(ReShadeManagerRuntime runtime)
		{
			Runtime = runtime;
			Commands = new Dictionary<string, Action<IReadOnlyList<string>>>
			{
				{"install", Install},
				{"help", Help},
				{"quit", Quit},
				{"exit", Quit},
				{"save", Save},
				{"update", Update},
				{"list", List}
			};
		}

		void List(IReadOnlyList<string> obj)
		{
			Console.WriteLine("The following repositories are available:");

			foreach (var (name, _, _) in Runtime.Config.Repositories)
				Console.WriteLine($"  {name}");
		}

		void Help(IReadOnlyList<string> obj)
		{
			Console.WriteLine("The following commands are available:");

			foreach (var (name, _) in Commands)
				Console.WriteLine($"  {name}");
		}

		void Update(IReadOnlyList<string> obj)
		{
			Console.Write("Updating repositories...");

			Runtime.UpdateRepositories();

			Console.WriteLine("done.");
		}

		void Quit(IReadOnlyList<string> args)
		{
			Console.WriteLine("Bye.");

			ShouldQuit = true;
		}

		void Save(IReadOnlyList<string> args)
		{
			Console.Write("Saving configuration file...");

			Runtime.SaveConfig();

			Console.WriteLine("done.");
		}

		void Install(IReadOnlyList<string> args)
		{
			if (args.Count < 2)
				Console.Error.WriteLine("install command requires a name and path");

			var (name, path) = (args[0], args[1]);
			var installPath = $"{path}/ReShade/Repositories/{name}";

			if (FSUtils.GetItem(installPath, out var kind))
			{
				if (InteractUtils.RequestInput(
					$"Installation path {installPath} already exists, remove?",
					allowDefault: false,
					(ConsoleKey.Y, "Yes"),
					(ConsoleKey.N, "No")) ==  ConsoleKey.Y)
				{
					switch (kind)
					{
						case FSItemKind.File:
						case FSItemKind.Link:
							File.Delete(path);
							break;
						case FSItemKind.Directory:
							Directory.Delete(path, true);
							break;
					}
				}
			}

			FSUtils.CreateSymbolicLink(
				installPath,
				Path.GetFullPath($"{Runtime.RepositoriesPath}/{name}"));

			Console.WriteLine("Repository installed.");
		}
	}
}
