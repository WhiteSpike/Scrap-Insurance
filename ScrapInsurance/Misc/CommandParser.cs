using ScrapInsurance.Misc.Util;
using System;
using System.Diagnostics;
using UnityEngine;

namespace ScrapInsurance.Misc
{
    internal static class CommandParser
    {
        static bool confirmPrompt = false;
        const string SCRAP_INSURANCE_COMMAND = ">SCRAP INSURANCE\n" +
            "Activates an insurance policy on scrap stored in the ship incase of a team wipe occurs.\n" +
            "Can only be bought while in orbit and will only apply in the next moon land after purchase.\n" +
            "Consumes {0} credits for each activation of insurance.\n\n";
        private static TerminalNode DisplayTerminalMessage(string message, bool clearPreviousText = true)
        {
            TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();
            node.displayText = message;
            node.clearPreviousText = clearPreviousText;
            return node;
        }
        public static void ParseCommands(string fullText, ref Terminal terminal, ref TerminalNode outputNode)
        {
            string[] textArray = fullText.Split();
            string firstWord = textArray[0].ToLower();
            string secondWord = textArray.Length > 1 ? textArray[1].ToLower() : "";
            string thirdWord = textArray.Length > 2 ? textArray[2].ToLower() : "";
            switch (firstWord)
            {
                case "scrap": outputNode = ExecuteScrapCommands(secondWord, thirdWord, ref terminal, ref outputNode); return;
                default: outputNode = CheckConfirmPrompt(firstWord, ref terminal, ref outputNode); return;
            }
        }

        private static TerminalNode CheckConfirmPrompt(string firstWord, ref Terminal terminal, ref TerminalNode outputNode)
        {
            if (!confirmPrompt) return outputNode;
            confirmPrompt = false;
            if (string.IsNullOrEmpty(firstWord) || !"confirm".Contains(firstWord, comparisonType: StringComparison.OrdinalIgnoreCase))
                return outputNode;

            int price = Plugin.Config.SCRAP_INSURANCE_PRICE;
            if (terminal.IsServer) terminal.SyncGroupCreditsClientRpc(terminal.groupCredits - price, terminal.numberOfItemsInDropship);
            else terminal.SyncGroupCreditsServerRpc(terminal.groupCredits - price, terminal.numberOfItemsInDropship);

            ScrapInsuranceBehaviour.TurnOnScrapInsurance();
            return DisplayTerminalMessage(Constants.SCRAP_INSURANCE_SUCCESS);
        }

        private static TerminalNode ExecuteScrapCommands(string secondWord, string thirdWord, ref Terminal terminal, ref TerminalNode outputNode)
        {
            return secondWord switch
            {
                "insurance" => ExecuteScrapInsuranceCommand(thirdWord, ref terminal, ref outputNode),
                _ => outputNode,
            };
        }
        private static TerminalNode ExecuteScrapInsuranceCommand(string thirdWord, ref Terminal terminal, ref TerminalNode outputNode)
        {
            if (!string.IsNullOrEmpty(thirdWord) && thirdWord == "help")
                return DisplayTerminalMessage(string.Format(SCRAP_INSURANCE_COMMAND, Plugin.Config.SCRAP_INSURANCE_PRICE.Value));

            if (ScrapInsuranceBehaviour.GetScrapInsuranceStatus())
                return DisplayTerminalMessage(Constants.SCRAP_INSURANCE_ALREADY_PURCHASED);

            if (!StartOfRound.Instance.inShipPhase)
                return DisplayTerminalMessage(Constants.SCRAP_INSURANCE_ONLY_IN_ORBIT);

            int price = Plugin.Config.SCRAP_INSURANCE_PRICE;
            if (terminal.groupCredits < price)
                return DisplayTerminalMessage(string.Format(Constants.SCRAP_INSURANCE_NOT_ENOUGH_CREDITS_FORMAT, price, terminal.groupCredits));

            confirmPrompt = true;
            return DisplayTerminalMessage(string.Format(Constants.SCRAP_INSURANCE_CONFIRM, price));
        }
    }
}
