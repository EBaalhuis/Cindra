namespace Cindra
{
    public class Result
    {
        public Result(List<Turn> validTurns)
        {
            StartEnd = new int[3][];
            //for (int start = 0; start < 3; start++)
            //{
            //    StartEnd[start] = new int[3];
            //    for (int end = 0; end < 3; end++)
            //    {
            //        var relevantTurns = validTurns
            //            .Where(t => t.DaggersAtStartOfTurn == start)
            //            .Where(t => t.DaggersAtEndOfTurn == end);

            //        StartEnd[start][end] = relevantTurns.Any() ?
            //            relevantTurns.Max(t => t.GetDamage()) : 0;
            //    }
            //}

            OverallMax = validTurns.Max(t => t.GetDamage());
            BestTurn = validTurns.First(t => t.GetDamage() == OverallMax).ToString();
        }

        public int[][] StartEnd { get; }
        //public int OverallMax => StartEnd.SelectMany(rec => rec).Max();
        public int OverallMax { get; }
        public string BestTurn { get; }
    }
}
