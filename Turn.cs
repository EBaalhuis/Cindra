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
