namespace Cindra
{
    public class Result
    {
        public Result(List<Turn> validTurns)
        {
            OverallMax = validTurns.Max(t => t.GetDamage());
            OverallBestTurn = validTurns.First(t => t.GetDamage() == OverallMax).ToString();

            SetProperties(0, validTurns);
            SetProperties(1, validTurns);
            SetProperties(2, validTurns);
        }

        public int OverallMax { get; }
        public string OverallBestTurn { get; }
        public int?[] MaxStartingWithNDaggers { get; } = new int?[3];
        public string?[] BestTurnStartingWithNDaggers { get; } = new string?[3];
        public int?[] DaggersAtEnd { get; } = new int?[3];

        private void SetProperties(int startingDaggers, List<Turn> validTurns)
        {
            var turns = validTurns.Where(t => t.DaggersAtStartOfTurn == startingDaggers);
            MaxStartingWithNDaggers[startingDaggers] = 
                turns.Any() ? turns.Max(t => t.GetDamage()) : null;

            var highestDamageTurns = turns.Where(t => t.GetDamage() == MaxStartingWithNDaggers[startingDaggers]);
            var maxDaggersAtEnd = highestDamageTurns.Any() ? highestDamageTurns.Max(t => t.DaggersAtEndOfTurn) : (int?)null;
            var bestTurns = highestDamageTurns.Where(t => t.DaggersAtEndOfTurn == maxDaggersAtEnd);
            BestTurnStartingWithNDaggers[startingDaggers] = bestTurns.FirstOrDefault()?.ToString();
            DaggersAtEnd[startingDaggers] = maxDaggersAtEnd;
        }
    }
}
