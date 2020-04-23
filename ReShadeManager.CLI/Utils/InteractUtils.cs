using System;
using System.Linq;

namespace ReShadeManager.CLI.Utils
{
	static class InteractUtils
	{
		public static ConsoleKey RequestInput(
			string prompt,
			bool allowDefault = true,
			params (ConsoleKey? key, string action)[] options)
		{
			Console.Write($"{prompt}");

			if (options.Length == 0)
				return Console.ReadKey().Key;

			Console.Write(" (");

			foreach (var o in options)
			{
				var keyName = o.key?.ToString() ?? "Default";
				Console.Write($"{keyName}: {o.action}, ");
			}

			Console.Write("\b\b)");

			ConsoleKey choice;

			if (allowDefault)
			{
				choice = Console.ReadKey(true).Key;
			}
			else
			{
				var required =
					options
						.Where(x => !(x.key is null))
						.Select(x => x.key)
						.ToHashSet();

				do choice = Console.ReadKey(true).Key;
				while (!required.Contains(choice));
			}

			Console.WriteLine();
			return choice;
		}
	}
}
