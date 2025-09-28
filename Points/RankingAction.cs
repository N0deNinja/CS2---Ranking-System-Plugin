namespace RankingSystemBySeen.Points
{
    public enum RankingActions
    {
        Kill,
        Death,
        RoundWin,
        RoundLoss,
        KillStreak,
        Message,
    }

    public class RankingAction
    {
        private const int PointsKill = 6;
        private const int PointsDeath = -2;
        private const int PointsRoundWin = 3;
        private const int PointsRoundLoss = -1;
        private const int PointsKillStreak = 5;
        private const int PointsMessage = 0;

        private static readonly Dictionary<RankingActions, (int Points, string Description)> ActionPoints = new()
        {
            { RankingActions.Kill,       (PointsKill, "You just got a kill - Adding 6 points to your rank") },
            { RankingActions.Death,      (PointsDeath, "You died - Losing 2 points") },
            { RankingActions.RoundWin,   (PointsRoundWin, "Round won - Adding 3 points to your rank") },
            { RankingActions.RoundLoss,  (PointsRoundLoss, "Round lost - Losing 1 point") },
            { RankingActions.KillStreak, (PointsKillStreak, "Kill streak achieved - Adding 5 points!") },
            { RankingActions.Message,    (PointsMessage, "You sent a message - No points added") }
        };

        public static (int Points, string Description) GetAction(RankingActions action)
        {
            return ActionPoints[action];
        }
    }
}
