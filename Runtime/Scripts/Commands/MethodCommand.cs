using System;
using System.Globalization;
using System.Reflection;

namespace MadStark.MadShell
{
	/// <summary>
	/// Command that wraps around a <see cref="MethodInfo"/> to invoke when the command is invoked.
	/// </summary>
	public class MethodCommand : Command
	{
		/// <summary>
		/// Method that will get invoked by this command.
		/// </summary>
		public readonly MethodInfo method;


		internal MethodCommand(MethodInfo methodInfo)
		{
			method = methodInfo;
		}

		/// <summary>
		/// Create a new <see cref="MethodCommand"/> around a method.
		/// </summary>
		/// <param name="methodInfo">The method to invoke from the command.</param>
		/// <returns>The new command.</returns>
		public static MethodCommand Create(MethodInfo methodInfo)
		{
			if (methodInfo == null)
				throw new ArgumentNullException(nameof(methodInfo));
			if (!IsMethodValidForCommand(methodInfo))
				throw new ArgumentException($"Method '{methodInfo.Name}' is not suitable for a command.", nameof(methodInfo));

			return new MethodCommand(methodInfo);
		}

		/// <summary>
		/// Checks the validity of a method as a command.
		/// </summary>
		/// <param name="methodInfo">The method to analyse.</param>
		/// <returns>Whether the method can be a valid command.</returns>
		public static bool IsMethodValidForCommand(MethodInfo methodInfo)
		{
			ParameterInfo[] parameters = methodInfo.GetParameters();
			return parameters.Length == 0 || parameters.Length == 1 && parameters[0].ParameterType == typeof(string[]);
		}

		/// <inheritdoc />
		public override void Invoke(string[] args)
		{
			object[] parameters = method.GetParameters().Length == 0 ? null : new object[] { args };
			method.Invoke(null, BindingFlags.Static, null, parameters, CultureInfo.CurrentCulture);
		}
	}
}
