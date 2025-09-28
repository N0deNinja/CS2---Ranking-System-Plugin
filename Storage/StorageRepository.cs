using System.ComponentModel;
using System.Text.Json;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using RankingSystemBySeen.Points;

namespace RankingSystemBySeen.Storage;

public class UserItem
{
    public int Score;
    public int BiggestKS;
    public int MessagesAmount;
}


public class StorageRepository
{
    private Dictionary<ulong, UserItem> Leaderboard = [];
    private readonly string RankingJsonPath = "ranking.json";

    public StorageRepository()
    {
        Init();
    }

    private void Init()
    {
        if (File.Exists(RankingJsonPath))
        {
            var json = File.ReadAllText(RankingJsonPath);
            var data = JsonSerializer.Deserialize<Dictionary<ulong, UserItem>>(json);
            if (data != null)
            {
                Leaderboard = data;
            }
        }
        else
        {
            Console.WriteLine("[Ranking System] - Missing ranking.json");
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
        return Leaderboard.TryGetValue(SteamId, out var user);
    }

    public void UpdatePlayer(CCSPlayerController User, RankingActions Action)
    {
        var SteamId = User.SteamID;

        if (!Leaderboard.ContainsKey(SteamId))
            InitPlayer(SteamId);

        if (UserExists(SteamId))
        {
            var UserItem = Leaderboard[SteamId];

            var (points, _) = RankingAction.GetAction(Action);

            // apply score only for certain actions
            if (Action is RankingActions.Kill
                      or RankingActions.Death
                      or RankingActions.RoundWin
                      or RankingActions.RoundLoss)
            {
                UserItem.Score += points;
            }
        }
    }

    public void SaveLeaderboard()
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(Leaderboard, options);
            File.WriteAllText(RankingJsonPath, json);

            Console.WriteLine("[Ranking System] - Leaderboard saved to JSON");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Ranking System] - Error saving leaderboard: {ex.Message}");
        }
    }


}