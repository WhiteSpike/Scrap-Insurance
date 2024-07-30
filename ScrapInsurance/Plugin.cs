using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MoreShipUpgrades.Patches.RoundComponents;
using ScrapInsurance.Misc;
using ScrapInsurance.Patches.HUD;
using ScrapInsurance.Patches.RoundComponents;
using ScrapInsurance.Patches.TerminalComponents;
using UnityEngine;
namespace ScrapInsurance
{
    [BepInPlugin(Metadata.GUID,Metadata.NAME,Metadata.VERSION)]
    [BepInDependency("com.sigurd.csync")]
    [BepInDependency("evaisa.lethallib")]
    public class Plugin : BaseUnityPlugin
    {
        internal static readonly Harmony harmony = new(Metadata.GUID);
        internal static readonly ManualLogSource mls = BepInEx.Logging.Logger.CreateLogSource(Metadata.NAME);

        public new static PluginConfig Config;
        internal static GameObject networkPrefab;

        void Awake()
        {
            Config = new PluginConfig(base.Config);

            PatchMainVersion();
            networkPrefab = LethalLib.Modules.NetworkPrefabs.CreateNetworkPrefab(ScrapInsuranceBehaviour.COMMAND_NAME);
            networkPrefab.AddComponent<ScrapInsuranceBehaviour>();

            mls.LogInfo($"{Metadata.NAME} {Metadata.VERSION} has been loaded successfully.");
        }
        internal static void PatchMainVersion()
        {
            PatchVitalComponents();
        }
        static void PatchVitalComponents()
        {
            harmony.PatchAll(typeof(HudManagerPatcher));
            harmony.PatchAll(typeof(StartOfRoundPatcher));
            harmony.PatchAll(typeof(RoundManagerPatcher));
            harmony.PatchAll(typeof(RoundManagerTranspilerPatcher));
            harmony.PatchAll(typeof(TerminalPatcher));
            mls.LogInfo("Game managers have been patched");
        }
    }   
}
