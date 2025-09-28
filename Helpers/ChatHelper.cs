using CounterStrikeSharp.API.Core;
using RankingSystemBySeen.Storage;

namespace RankingSystemBySeen.Helpers;

public static class ChatHelper
{

    private static readonly string prefix = "[\x02RankingSystem\x01]";
    public static void PrintRankMessage(CCSPlayerController Player, int Score, int Position, int TotalPlayers)
    {
        Player.PrintToChat($"{prefix} Your current position is \x04{Position}\x01 / \x04{TotalPlayers}!");
        Player.PrintToChat($"{prefix} Your current score is \x04{Score}\x01!");
        if (Position == 1)
        {
            Player.PrintToChat($"{prefix} You are the \x04" + "BEST" + "\x01 on this SERVER!");
        }
    }

    public static void PrintTopList(CCSPlayerController Player, UserItem[] players)
    {
        Player.PrintToChat($"{prefix} \x05TOP 15 - Leaderboard");

        // Header row
        Player.PrintToChat($" \x02#   \x03Name               \x04Score");

        for (int i = 0; i < players.Length && i < 15; i++)
        {
            var currentPlayer = players[i];
            var isCurrentPlayer = Player.SteamID == currentPlayer.SteamID;

            // Colors
            var placeColor = i switch
            {
                0 => "\x07", // gold
                1 => "\x0F", // silver
                2 => "\x06", // bronze
                _ => "\x01"
            };
            var nameColor = isCurrentPlayer ? "\x04" : "\x01";


            string pos = (i + 1).ToString().PadLeft(2, ' ');
            string name = currentPlayer.Name.PadRight(16, ' ');

            Player.PrintToChat(
                $" {placeColor}{pos}. " +
                $"{nameColor}{name} \x01" +
                $"- \x05{currentPlayer.Score}"
            );
        }
    }

}
