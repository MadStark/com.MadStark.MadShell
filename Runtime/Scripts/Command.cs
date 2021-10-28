namespace MadStark.MadShell
{
	/// <summary>
	/// Base class for any command run by the <see cref="MadShell"/>.
	/// </summary>
	public abstract class Command
	{
		public abstract void Invoke(string[] args);
	}
}
