using CounterStrikeSharp.API.Core;
using RankingSystemBySeen.Storage;

namespace RankingSystemBySeen.Handlers;

public class RoundHandler(StorageRepository storage)
{
    private readonly StorageRepository _storage = storage;

    public void HandleRound(CCSPlayerController player, Points.RankingActions action)
    {
        _storage.UpdatePlayer(player, action);
    }
}