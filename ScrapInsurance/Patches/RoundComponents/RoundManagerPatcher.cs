using HarmonyLib;
using ScrapInsurance.Misc;

namespace ScrapInsurance.Patches.RoundComponents
{
    [HarmonyPatch(typeof(RoundManager))]
    internal static class RoundManagerPatcher
    {

        [HarmonyPatch(nameof(RoundManager.DespawnPropsAtEndOfRound))]
        [HarmonyPostfix]
        static void DespawnPropsAtEndOfRoundPostfix()
        {
            ScrapInsuranceBehaviour.TurnOffScrapInsurance();
        }
    }
}
