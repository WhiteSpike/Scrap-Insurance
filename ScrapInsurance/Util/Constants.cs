using UnityEngine;

namespace ScrapInsurance.Misc.Util
{
    internal static class Constants
    {
        internal const string SCRAP_INSURANCE_PRICE_KEY = $"Price of {ScrapInsuranceBehaviour.COMMAND_NAME}";

        internal const string SCRAP_INSURANCE_ALREADY_PURCHASED = "You already purchased insurance to protect your scrap belongings.\n\n";
        internal const string SCRAP_INSURANCE_ONLY_IN_ORBIT = "You can only acquire insurance while in orbit.\n\n";
        internal const string SCRAP_INSURANCE_NOT_ENOUGH_CREDITS_FORMAT = "Not enough credits to purchase Scrap Insurance.\nPrice: {0}\nCurrent credits: {1}\n\n";
        internal const string SCRAP_INSURANCE_CONFIRM = "Type \"confirm\" if you wish to purchase scrap insurance for {0} Company Credits to keep your scrap items in the ship incase of a team wipe.\n\n";
        internal const string SCRAP_INSURANCE_SUCCESS = "Scrap Insurance has been activated.\nIn case of a team wipe in your next trip, your scrap will be preserved.\n\n";
    }
}
