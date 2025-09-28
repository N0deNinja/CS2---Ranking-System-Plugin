using System.ComponentModel;
using System.Text.Json;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using RankingSystemBySeen.Points;

namespace RankingSystemBySeen.Storage;

public class UserItem
{
    public int Score { get; set; }
    public int BiggestKS { get; set; }
    public int MessagesAmount { get; set; }
}
public class StorageRepository
{
    private Dictionary<ulong, UserItem> Leaderboard = [];
    private readonly string RankingJsonPath =
   Path.Combine(Server.GameDirectory, "csgo", "addons", "counterstrikesharp", "configs", "plugins", "RankingSystemBySeen", "RankingSystemBySeen.json");

    public StorageRepository()
    {
        Console.WriteLine("[Ranking System] - StorageRepository initialized");
        Init();
    }

    private void Init()
    {
        Console.WriteLine("[Ranking System] - Loading leaderboard from JSON...");
        if (File.Exists(RankingJsonPath))
        {
            try
            {
                var json = File.ReadAllText(RankingJsonPath);
                var data = JsonSerializer.Deserialize<Dictionary<ulong, UserItem>>(json);
                if (data != null)
                {
                    Leaderboard = data;
                    Console.WriteLine($"[Ranking System] - Loaded {Leaderboard.Count} players from {RankingJsonPath}");
                }
                else
                {
                    Console.WriteLine("[Ranking System] - No data found in JSON, starting fresh");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Ranking System] - Failed to load leaderboard: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("[Ranking System] - Missing ranking.json, starting fresh");
        }
    }

    public void InitPlayer(ulong SteamId)
    {
        if (Leaderboard.TryAdd(SteamId, new UserItem
        {
            Score = 0,
            BiggestKS = 0,
            MessagesAmount = 0
        }))
        {
            Console.WriteLine($"[Ranking System] - Initialized player {SteamId}");
        }
        else
        {
            Console.WriteLine($"[Ranking System] - Player {SteamId} already initialized");
        }
    }

    private bool UserExists(ulong SteamId)
    {

        return Leaderboard.ContainsKey(SteamId);
    }

    public (int Score, int Position, int TotalPlayers) GetPlayerInfo(ulong steamId)
    {
        if (!Leaderboard.ContainsKey(steamId))
            return (0, 0, Leaderboard.Count);

        // Sort by score descending
        var sorted = Leaderboard
            .OrderByDescending(x => x.Value.Score)
            .Select((x, i) => new { SteamId = x.Key, Score = x.Value.Score, Position = i + 1 })
            .ToList();

        var playerEntry = sorted.FirstOrDefault(x => x.SteamId == steamId);
        if (playerEntry == null)
            return (0, 0, Leaderboard.Count);

        return (playerEntry.Score, playerEntry.Position, Leaderboard.Count);
    }


    // Save after every update
    public void UpdatePlayer(CCSPlayerController User, RankingActions Action)
    {
        var SteamId = User.SteamID;

        if (SteamId == 0)
        {
            Console.WriteLine("[Ranking System] - Ignoring invalid SteamId (0)");
            return;
        }

        if (!UserExists(SteamId))
            InitPlayer(SteamId);

        var userItem = Leaderboard[SteamId];
        var (points, desc) = RankingAction.GetAction(Action);
        User.PrintToChat(desc);

        if (Action is RankingActions.Kill or RankingActions.Death or RankingActions.RoundWin or RankingActions.RoundLoss)
        {
            userItem.Score += points;
            User.Score += points;
            Console.WriteLine($"[Ranking System] - {desc} ({points} points). New Score: {userItem.Score}");

            SaveLeaderboard();
        }
    }



    public void SaveLeaderboard()
    {
        try
        {
            var directory = Path.GetDirectoryName(RankingJsonPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory!);

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(Leaderboard, options);
            File.WriteAllText(RankingJsonPath, json);

            Console.WriteLine($"[Ranking System] - Leaderboard saved. Players stored: {Leaderboard.Count}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Ranking System] - Error saving leaderboard: {ex.Message}");
        }
    }

}
