using CounterStrikeSharp.API.Core;
using RankingSystemBySeen.Storage;

namespace RankingSystemBySeen.Handlers;

public class KillHandler(StorageRepository storage)
{
    private readonly StorageRepository _storage = storage;

    public void HandleKill(CCSPlayerController Attacker, CCSPlayerController Victim)
    {
        _storage.UpdatePlayer(Attacker, Points.RankingActions.Kill);
        _storage.UpdatePlayer(Victim, Points.RankingActions.Death);
    }
}