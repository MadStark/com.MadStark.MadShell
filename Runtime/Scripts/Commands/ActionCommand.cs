using System;

namespace MadStark.MadShell
{
	/// <summary>
	/// Command that wraps around a <see cref="Action"/> to invoke when the command is invoked.
	/// </summary>
	public class ActionCommand : Command
	{
		/// <summary>
		/// Delegate that will be invoked by this command.
		/// </summary>
		public readonly Action<string[]> action;


		public ActionCommand(Action<string[]> action)
		{
			this.action = action;
		}

		/// <inheritdoc />
		public override void Invoke(string[] args)
		{
			action?.Invoke(args);
		}
	}
}
