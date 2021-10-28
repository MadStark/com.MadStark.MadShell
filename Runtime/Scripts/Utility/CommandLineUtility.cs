using System.Collections.Generic;
using System.Linq;

namespace MadStark.MadShell
{
	/// <summary>
	/// Utility class for dealing with command lines.
	/// </summary>
	public static class CommandLineUtility
	{
		public static IEnumerable<string> EnumerateArguments(string commandLine)
		{
			bool inQuotes = false;

			return commandLine
				.Split(c => {
					if (c == '\"') inQuotes = !inQuotes;
					return !inQuotes && char.IsWhiteSpace(c);
				})
				.Select(arg => arg.Trim().TrimMatchingQuotes('\"'))
				.Where(arg => !string.IsNullOrWhiteSpace(arg));
		}

		public static string[] SplitCommandLine(string commandLine) => EnumerateArguments(commandLine).ToArray();
	}
}