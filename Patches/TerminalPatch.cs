using HarmonyLib;

namespace LethalASD.Patches;

[HarmonyPatch(typeof(Terminal))]
public class TerminalPatch
{
    [HarmonyPatch("ParsePlayerSentence")]
    [HarmonyPostfix]
    private static void HandleAllRunSubcommands(ref TerminalNode __result) {
        string userInput = TerminalApi.TerminalApi.GetTerminalInput();
        if (userInput.StartsWith("run")) {
            __result = LethalASD.runNode;
        }
    }
}
