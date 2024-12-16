using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

using static TerminalApi.Events.Events;
using static TerminalApi.TerminalApi;
using TerminalApi;

namespace LethalASD;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("atomic.terminalapi")]
public class LethalASD : BaseUnityPlugin
{
    public static LethalASD Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;
    internal static Harmony? Harmony { get; set; }

    public static MatrixSorter sorter = new();
    public static TerminalNode runNode = CreateTerminalNode("", true);

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

        Patch();

        TerminalParsedSentence += OnTerminalCommandSend;

        runNode.maxCharactersToType = 500;
        TerminalKeyword runVerb = CreateTerminalKeyword("run", true, runNode);
        AddTerminalKeyword(runVerb, new TerminalApi.Classes.CommandInfo {
            Description = "[size] [matrix ]\nSorts a matrix",
            Category = "Other",
        });

        TerminalKeyword catVerb = CreateTerminalKeyword("cat", true);
        TerminalKeyword catNoun = CreateTerminalKeyword("main.c");
        TerminalNode catNode = CreateTerminalNode(MatrixSorter.SOURCE_CODE, true);

        catVerb = catVerb.AddCompatibleNoun(catNoun, catNode);
        AddTerminalKeyword(catVerb);
        AddTerminalKeyword(catNoun);

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }
    private void OnTerminalCommandSend(object sender, TerminalParseSentenceEventArgs e) {
        	string userInput = GetTerminalInput();

            if (userInput == "run") {
                runNode.displayText = sorter.RunFromString();
            }
			else if (userInput.StartsWith("run ") && userInput.Length >= 5)
			{
				string parsedNoun = userInput.Split(" ", 2, System.StringSplitOptions.RemoveEmptyEntries)[1];
                runNode.displayText = sorter.RunFromString(parsedNoun);
			}
    }
    internal static void Patch()
    {
        Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

        Logger.LogDebug("Patching...");

        Harmony.PatchAll();

        Logger.LogDebug("Finished patching!");
    }

    internal static void Unpatch()
    {
        Logger.LogDebug("Unpatching...");

        Harmony?.UnpatchSelf();

        Logger.LogDebug("Finished unpatching!");
    }
}
