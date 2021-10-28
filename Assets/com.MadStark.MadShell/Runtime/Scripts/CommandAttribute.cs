using System;

namespace MadStark.MadShell
{
	/// <summary>
	/// Declares a console command.
	/// Either place on a class that inherits from <see cref="Command"/> or on a static method.
	/// </summary>
	/// <remarks>When decorating a static method, the method must either take none or a single argument of type string[].</remarks>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
	public class CommandAttribute : Attribute
	{
		/// <summary>
		/// Name of this command.
		/// </summary>
		public readonly string name;

		/// <summary>
		/// Declare a new command with a given name.
		/// </summary>
		/// <param name="name"></param>
		public CommandAttribute(string name)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));

			this.name = name;
		}
	}
}
