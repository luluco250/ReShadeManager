using System;

namespace ReShadeManager.CLI.Commands
{
	sealed class Interpreter
	{
		public Context Context { get; }

		public Interpreter(Context context)
		{
			Context = context;
		}

		public void Start()
		{
			while (!Context.ShouldQuit)
			{
				Console.Write("\nCommand>");
				var input = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);

				if (input.Length == 0 || string.IsNullOrWhiteSpace(input[0]))
					continue;

				if (!Context.Commands.TryGetValue(input[0], out var action))
				{
					Console.Error.WriteLine($"Command not found: {input[0]}");
					continue;
				}

				try
				{
					action(input[1..]);
				}
				catch (Exception e)
				{
					Console.Error.WriteLine($"Unexpected error: {e.Message}");
				}
			}
		}
	}
}
