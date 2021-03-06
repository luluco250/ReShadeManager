using System;
using System.IO;
using ReShadeManager.CLI.Commands;
using ReShadeManager.CLI.Utils;
using ReShadeManager.Core;

namespace ReShadeManager.CLI
{
	static class Program
	{
		static void Main()
		{
			try
			{
				var mgr = new ReShadeManagerRuntime
				(
					rootPath: Directory.GetCurrentDirectory()
					// Does not seem to work when publish as a self-contained exe.
					//rootPath: AppDomain.CurrentDomain.SetupInformation.ApplicationBase ?? ""
				);

				Console.WriteLine($"Starting with root path: {mgr.RootPath}");

				Console.WriteLine(
					mgr.LoadConfig()
						? "Configuration loaded."
						: "No configuration found, using defaults.");

				if (!mgr.TryLoadGit())
				{
					Console.WriteLine("Failed to find git path!");
					Console.Write("Please specify (empty to quit): ");

					var gitCmdLine = Console.ReadLine();
					if (string.IsNullOrWhiteSpace(gitCmdLine))
						return;

					mgr.TryLoadGit(gitCmdLine);
				}

				Console.WriteLine("Starting interpreter...");
				var repl = new Interpreter(new Context(mgr));
				repl.Start();
			}
			catch (AggregateException e)
			{
				Console.Error.WriteLine(
					$"Fatal error: {e.Message}\n{e.InnerExceptions}");
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(
					$"Fatal error: {e.Message}\n{e.InnerException}");
			}
		}
	}
}
