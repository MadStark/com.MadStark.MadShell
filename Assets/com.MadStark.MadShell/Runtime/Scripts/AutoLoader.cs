using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

namespace MadStark.MadShell
{
	/// <summary>
	/// Automatically calls <see cref="MadShell.FindAndRegisterAllCommands"/> when entering play mode
	/// or in Editor after script compilation if Reload Domain is false.
	/// </summary>
	/// <remarks>Disable auto loading with the MADSHELL_DISABLE_AUTO_LOADER define.</remarks>
	internal static class AutoLoader
	{
		private static bool Loaded { get; set; }


		private static void Load()
		{
			if (!Loaded)
			{
				Loaded = true;
				MadShell.FindAndRegisterAllCommands();
			}
		}

#if !MADSHELL_DISABLE_AUTO_LOADER
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
		private static void InitializeOnLoad()
		{
			Load();
		}

#if UNITY_EDITOR && UNITY_2019_3_OR_NEWER
		[UnityEditor.Callbacks.DidReloadScripts]
		private static void DidReloadScripts()
		{
			// Save up some time when entering play mode in Editor if reload domain is disabled
			// by loading the commands right after compilation instead.

			bool domainReload = !EditorSettings.enterPlayModeOptionsEnabled ||
			                    (EditorSettings.enterPlayModeOptions & EnterPlayModeOptions.DisableDomainReload) == 0;

			if (!domainReload)
			{
				Load();
			}
		}
#endif // UNITY_EDITOR && UNITY_2019_3_OR_NEWER

#endif // MADSHELL_DISABLE_AUTO_LOADER
	}
}
