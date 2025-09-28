using CounterStrikeSharp.API.Core;
using RankingSystemBySeen.Storage;
using RankingSystemBySeen.Handlers;
using RankingSystemBySeen.Helpers;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Commands;

namespace RankingSystemBySeen;

public class RankingSystemBySeen : BasePlugin
{
    public override string ModuleName => "RankingSystem";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "Seen";
    public override string ModuleDescription => "Ranks players on server and displays numeric ranks in scoreboard + !rank";

    private KillHandler? _killHandler;
    private RoundHandler? _roundHandler;
    private StorageRepository? _storage;


    public override void OnAllPluginsLoaded(bool hotReload)
    {
        Console.WriteLine("[RankingSystem] Plugin loaded successfully!");
    }

    public override void Load(bool hotReload)
    {
        _storage = new StorageRepository();
        _killHandler = new KillHandler(_storage);
        _roundHandler = new RoundHandler(_storage);


        RegisterEventHandler<EventPlayerDeath>(OnPlayerDeath);
        RegisterEventHandler<EventRoundEnd>(OnRoundEnds);
        AddCommand("rank", "Prints the rank message to chat", OnRankCommand);
        AddCommand("!rank", "Prints the rank message to chat", OnRankCommand);
    }

    public override void Unload(bool hotReload)
    {
        _storage?.SaveLeaderboard();
        Console.WriteLine("[RankingSystem] Leaderboard saved on unload.");
    }

    private HookResult OnPlayerDeath(EventPlayerDeath ev, GameEventInfo info)
    {
        var attacker = ev.Attacker;
        var victim = ev.Userid;


        if (attacker == null || victim == null || attacker == victim)
            return HookResult.Continue;

        _killHandler?.HandleKill(attacker, victim);
        return HookResult.Continue;
    }

    private void OnRankCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null || !player.IsValid || _storage == null)
            return;

        var steamId = player.SteamID;
        var (Score, Position, TotalPlayers) = _storage.GetPlayerInfo(steamId);


        ChatHelper.PrintRankMessage(player, Score, Position, TotalPlayers);
    }
    private HookResult OnRoundEnds(EventRoundEnd ev, GameEventInfo info)
    {
        _storage?.SaveLeaderboard();
        if (ev.Winner == 0)
            return HookResult.Continue;

        var winningTeam = (CsTeam)ev.Winner;
        var winners = GameHelper.GetPlayersByTeam(winningTeam);
        winners.ForEach((winner) =>
        {
            _roundHandler?.HandleRound(winner, Points.RankingActions.RoundWin);
        });


        var losingTeam = winningTeam == CsTeam.CounterTerrorist ? CsTeam.Terrorist : CsTeam.CounterTerrorist;
        var losers = GameHelper.GetPlayersByTeam(losingTeam);

        losers.ForEach((loser) =>
        {
            _roundHandler?.HandleRound(loser, Points.RankingActions.RoundLoss);
        });

        return HookResult.Continue;
    }
}
