using System;
using System.Collections.Generic;

namespace TDGachaGame
{
    public static class RedeemCodes
    {
        private sealed class CodeReward
        {
            public int Coins { get; }
            public int Gems { get; }

            public CodeReward(int coins, int gems)
            {
                Coins = coins;
                Gems = gems;
            }
        }

        private static readonly Dictionary<string, CodeReward> Codes =
            new Dictionary<string, CodeReward>(StringComparer.OrdinalIgnoreCase)
            {
                { "Flyopi", new CodeReward(10000, 500000) }
            };
        public static bool TryRedeem(PlayerProfile profile, string input, out string message)
        {
            string code = (input ?? "").Trim();

            if (code.Length == 0)
            {
                message = "Type a code first.";
                return false;
            }

            if (!Codes.TryGetValue(code, out CodeReward reward))
            {
                message = "That code isn't valid.";
                return false;
            }

            if (!profile.RedeemedCodes.Add(code))
            {
                message = "You already used that code.";
                return false;
            }

            profile.AddCoins(reward.Coins);
            profile.AddGems(reward.Gems);
            message = $"Code accepted!  +{reward.Coins} coins   +{reward.Gems} gems";
            return true;
        }
    }
}