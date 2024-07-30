using HarmonyLib;
using ScrapInsurance.Misc;
using ScrapInsurance.Util;
using System.Collections.Generic;
using System.Reflection;

namespace ScrapInsurance.Patches.HUD
{
    [HarmonyPatch(typeof(HUDManager))]
    internal static class HudManagerPatcher
    {

        [HarmonyPatch(nameof(HUDManager.FillEndGameStats))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> FillEndGameStatsTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo allPlayersDead = typeof(StartOfRound).GetField(nameof(StartOfRound.allPlayersDead));
            MethodInfo scrapInsuranceStatus = typeof(ScrapInsuranceBehaviour).GetMethod(nameof(ScrapInsuranceBehaviour.GetScrapInsuranceStatus));
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindField(ref index, ref codes, findField: allPlayersDead, addCode: scrapInsuranceStatus, notInstruction: true, andInstruction: true, errorMessage: "Couldn't find all players dead field");
            return codes;
        }
    }
}
