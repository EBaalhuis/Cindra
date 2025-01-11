namespace Cindra
{
    public class Turn
    {
        public Card[] Cards { get; }
        public List<Move> Moves { get; }
        public int DaggersAtStartOfTurn { get; }
        public int DaggersAtEndOfTurn { get; }

        public Turn(Card[] cards, List<Move> moves, int daggersAtStart, int daggersAtEnd)
        {
            Cards = cards;
            Moves = moves;
            DaggersAtStartOfTurn = daggersAtStart;
            DaggersAtEndOfTurn = daggersAtEnd;
        }

        public override string ToString()
        {
            if (Moves.Last() != Move.BreakChain)
            {
                return string.Join(", ", Moves.Select(m => ToDisplay(m)));
            }
            else
            {
                return string.Join(", ", Moves.Take(Moves.Count - 1).Select(m => ToDisplay(m)));
            }
        }

        private string ToDisplay(Move m)
        {
            return m switch
            {
                Move.PlayCard1 => $"play card {Cards[0].IndexOnPage}",
                Move.PlayCard2 => $"play card {Cards[1].IndexOnPage}",
                Move.PlayCard3 => $"play card {Cards[2].IndexOnPage}",
                Move.PlayCard4 => $"play card {Cards[3].IndexOnPage}",
                Move.PlayCard5 => $"play card {Cards[4].IndexOnPage}",
                Move.PitchCard1 => $"pitch card {Cards[0].IndexOnPage}",
                Move.PitchCard2 => $"pitch card {Cards[1].IndexOnPage}",
                Move.PitchCard3 => $"pitch card {Cards[2].IndexOnPage}",
                Move.PitchCard4 => $"pitch card {Cards[3].IndexOnPage}",
                Move.PitchCard5 => $"pitch card {Cards[4].IndexOnPage}",
                Move.BreakChain => "break chain",
                _ => m.ToString(),
            };
        }

        public int GetDamage()
        {
            return Moves.Sum(a => GetDamage(a));
        }

        private int GetDamage(Move move)
        {
            return move switch
            {
                Move.PlayCard1 => Cards[0].Power,
                Move.PlayCard2 => Cards[1].Power,
                Move.PlayCard3 => Cards[2].Power,
                Move.PlayCard4 => Cards[3].Power,
                Move.PlayCard5 => Cards[4].Power,
                Move.Kunai => 1,
                _ => 0,
            };
        }
    }
}
