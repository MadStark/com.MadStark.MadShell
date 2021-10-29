namespace MadStark.MadShell
{
	/// <summary>
	/// Base class for any command run by the <see cref="MadShell"/>.
	/// </summary>
	public abstract class Command
	{
		/// <summary>
		/// Invoke this command.
		/// </summary>
		/// <param name="args">Command arguments.</param>
		public abstract void Invoke(string[] args);
	}
}
