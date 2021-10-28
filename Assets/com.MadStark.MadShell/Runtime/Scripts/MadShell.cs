using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace MadStark.MadShell
{
	/// <summary>
	/// MadStark's take on a command line interpreter.
	/// </summary>
	public static class MadShell
	{
		/// <summary>
		/// List of all commands indexed by their name.
		/// </summary>
		public static ReadOnlyDictionary<string, Command> Commands { get; }

		private static readonly Dictionary<string, Command> _commands;


		static MadShell()
		{
			_commands = new Dictionary<string, Command>(50);
			Commands = new ReadOnlyDictionary<string, Command>(_commands);
		}


		/// <summary>
		/// Parse and execute a command from a command line.
		/// Handles whitespace delimited parameters and double quotation marks for escaping.
		/// </summary>
		/// <param name="commandLine">The command line to interpret.</param>
		public static void Interpret(string commandLine)
		{
			if (commandLine == null)
				throw new ArgumentNullException(nameof(commandLine));

			string[] elements = CommandLineUtility.SplitCommandLine(commandLine);

			if (elements.Length == 0)
				return;

			string command = elements[0];
			string[] args;

			if (elements.Length == 1)
			{
				args = Array.Empty<string>();
			}
			else
			{
				args = new string[elements.Length - 1];
				Array.Copy(elements, 1, args, 0, args.Length);
			}

			InvokeCommand(command, args);
		}

		/// <summary>
		/// Try to find the command with this name.
		/// </summary>
		/// <param name="name">Name of the command.</param>
		/// <param name="command">The command.</param>
		/// <returns>Whether the command was found.</returns>
		public static bool TryGetCommand(string name, out Command command)
		{
			return _commands.TryGetValue(name, out command);
		}

		/// <summary>
		/// Check if a command with a given name has been registered.
		/// </summary>
		/// <param name="name">Name of the command.</param>
		/// <returns>Whether the command has been registered.</returns>
		public static bool HasCommand(string name)
		{
			return _commands.ContainsKey(name);
		}

		/// <summary>
		/// Invoke a command with a set of arguments.
		/// </summary>
		/// <param name="name">Name of the command.</param>
		/// <param name="args">Parameters to pass to the command.</param>
		/// <exception cref="CommandNotFoundException">The command was not found.</exception>
		public static void InvokeCommand(string name, string[] args = null)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));

			if (!TryGetCommand(name, out Command callback))
				throw new CommandNotFoundException(name);

			if (args == null)
				args = Array.Empty<string>();

			callback.Invoke(args);
		}


		#region Command Registration

		/// <summary>
		/// Register a command.
		/// </summary>
		/// <param name="name">Name of the command.</param>
		/// <param name="command">Command instance.</param>
		public static void RegisterCommand(string name, Command command)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));
			if (command == null)
				throw new ArgumentNullException(nameof(command));

			_commands[name] = command;
		}

		/// <summary>
		/// Unregister a command.
		/// If the command was not found, no exception is thrown.
		/// </summary>
		/// <param name="name">Name of the command.</param>
		public static void UnregisterCommand(string name)
		{
			if (_commands.ContainsKey(name))
				_commands.Remove(name);
		}

		/// <summary>
		/// Find all <see cref="CommandAttribute"/> in all the loaded assemblies and register them as commands.
		/// </summary>
		public static void FindAndRegisterAllCommands()
		{
			IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach (Assembly assembly in assemblies)
			{
				RegisterCommandsInAssembly(assembly);
			}
		}

		/// <summary>
		/// Find all <see cref="CommandAttribute"/> in a given and register them as commands.
		/// </summary>
		/// <param name="assembly"></param>
		public static void RegisterCommandsInAssembly(Assembly assembly)
		{
			const BindingFlags FUNCTION_BINDING_FLAGS = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

			foreach (Type type in assembly.GetTypes())
			{
				if (type.IsSubclassOf(typeof(Command)))
				{
					foreach (CommandAttribute commandAttr in type.GetCustomAttributes<CommandAttribute>(false))
					{
						Command command = (Command)Activator.CreateInstance(type);
						RegisterCommand(commandAttr.name, command);
					}
				}

				foreach (MethodInfo method in type.GetMethods(FUNCTION_BINDING_FLAGS).Where(MethodCommand.IsMethodValidForCommand))
				{
					foreach (CommandAttribute consoleCommandAttribute in method.GetCustomAttributes<CommandAttribute>(false))
					{
						MethodCommand command = new MethodCommand(method);
						RegisterCommand(consoleCommandAttribute.name, command);
					}
				}
			}
		}

		#endregion
	}
}
