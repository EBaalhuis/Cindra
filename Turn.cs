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
            return string.Join(", ", Moves.Select(m => ToDisplay(m)));
        }

        private static string ToDisplay(Move m)
        {
            return m switch
            {
                Move.PlayCard1 => "play card 1",
                Move.PlayCard2 => "play card 2",
                Move.PlayCard3 => "play card 3",
                Move.PlayCard4 => "play card 4",
                Move.PitchCard1 => "pitch card 1",
                Move.PitchCard2 => "pitch card 2",
                Move.PitchCard3 => "pitch card 3",
                Move.PitchCard4 => "pitch card 4",
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
                Move.Kunai => 1,
                _ => 0,
            };
        }
    }
}
