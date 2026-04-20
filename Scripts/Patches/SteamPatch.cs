using System;
using System.Reflection;
using HarmonyLib;
using Hiro.Scripts.Characters;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

[HarmonyPatch(typeof(RunManager))]
public static class RichPresencePatch
{
    private static readonly Lazy<MethodInfo?> SteamSetRichPresence = new(() =>
    {
        var t = AccessTools.TypeByName("Steamworks.SteamFriends");
        return t == null
            ? null
            : AccessTools.Method(t, "SetRichPresence", new[] { typeof(string), typeof(string) });
    });

    private static readonly Lazy<PropertyInfo?> StateProp = new(() =>
        AccessTools.DeclaredProperty(typeof(RunManager), "State"));

    [HarmonyPostfix]
    [HarmonyPatch("UpdateRichPresence")]
    public static void UpdateRichPresence_Postfix(RunManager __instance)
    {
        if (__instance == null)
            return;

        var state = StateProp.Value?.GetValue(__instance) as RunState;
        if (TestMode.IsOn || state == null)
            return;

        var player = LocalContext.GetMe(state);
        var character = player?.Character;
        if (character == null)
            return;

        if (character is HiroCharacter)
        {
            SetRichPresence("Character", "defect");
            SetRichPresence("Ascension", $"二阶堂希罗 - A{state.AscensionLevel}");
        }
    }

    private static void SetRichPresence(string key, string value)
    {
        SteamSetRichPresence.Value?.Invoke(null, new object[] { key, value });
    }
}