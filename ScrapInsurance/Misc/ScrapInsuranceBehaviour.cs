using Unity.Netcode;

namespace ScrapInsurance.Misc
{
    internal class ScrapInsuranceBehaviour : NetworkBehaviour
    {
        internal const string COMMAND_NAME = "Scrap Insurance";
        internal const int DEFAULT_PRICE = 400;
        static bool insurance = false;
        internal static ScrapInsuranceBehaviour instance;
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            insurance = false;
            instance = this;
        }
        public static bool GetScrapInsuranceStatus()
        {
            Plugin.mls.LogDebug("Grabbing status of insurance...");
            return insurance;
        }

        [ServerRpc(RequireOwnership = false)]
        public void ToggleScrapInsuranceServerRpc(bool toggle)
        {
            ToggleInsurance(toggle);
        }
        public static void TurnOnScrapInsurance()
        {
            Plugin.mls.LogDebug("Turning on...");
            if (instance.IsServer) ToggleInsurance(true);
            else instance.ToggleScrapInsuranceServerRpc(true);
        }

        public static void TurnOffScrapInsurance()
        {
            Plugin.mls.LogDebug("Turning off...");
            if (instance.IsServer) ToggleInsurance(false);
            else instance.ToggleScrapInsuranceServerRpc(false);
        }

        static void ToggleInsurance(bool enabled)
        {
            insurance = enabled;
        }
    }
}
