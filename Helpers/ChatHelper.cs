using CounterStrikeSharp.API.Core;

namespace RankingSystemBySeen.Helpers;

public static class ChatHelper
{
    private static readonly string Prefix = ""; // green prefix

    public static void PrintRankMessage(CCSPlayerController Player, int Score, int Position, int TotalPlayers)
    {
        Player.PrintToChat($" \x02[RankingSystem] \x01 Your current position is \x04{Position}\x01 / \x04{TotalPlayers}!");
        Player.PrintToChat($" \x02[RankingSystem] \x01 Your current score is \x04{Score}\x01!");
        if (Position == 1)
        {
            Player.PrintToChat($"{Prefix}You are the \x04 BEST \x01 on this SERVER!");
        }

    }
}
