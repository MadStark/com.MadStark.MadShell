using System;

namespace MadStark.MadShell
{
	/// <summary>
	/// Exception thrown when the invoked command has not be registered.
	/// </summary>
	public class CommandNotFoundException : Exception
	{
		public CommandNotFoundException(string name) : base($"Command {name} not found.") { }
	}
}