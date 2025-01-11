namespace Cindra
{
    public class Model
    {
        public string? Card1 { get; set; }
        public string? Card2 { get; set; }
        public string? Card3 { get; set; }
        public string? Card4 { get; set; }
        public string? CardArsenal { get; set; }

        public bool Card1Draconic { get; set; } = true;
        public bool Card2Draconic { get; set; } = true;
        public bool Card3Draconic { get; set; } = true;
        public bool Card4Draconic { get; set; } = true;
        public bool CardArsenalDraconic { get; set; } = true;

        public Result? Result { get; set; }

        public string GetEndDaggersText(int i)
        {
            if (Result?.DaggersAtEnd[i] == null) return string.Empty;
            return $"End with {Result?.DaggersAtEnd[i]} daggers";
        }

        public IEnumerable<Card> GetCards()
        {
            if (Card1 != null && Card1 != string.Empty)
            {
                yield return new Card(Card1, Card1Draconic, indexOnPage: 1);
            }
            if (Card2 != null && Card2 != string.Empty)
            {
                yield return new Card(Card2, Card2Draconic, indexOnPage: 2);
            }
            if (Card3 != null && Card3 != string.Empty)
            {
                yield return new Card(Card3, Card3Draconic, indexOnPage: 3);
            }
            if (Card4 != null && Card4 != string.Empty)
            {
                yield return new Card(Card4, Card4Draconic, indexOnPage: 4);
            }
            if (CardArsenal != null && CardArsenal != string.Empty)
            {
                yield return new Card(CardArsenal, CardArsenalDraconic, indexOnPage: 5, isInArsenal: true);
            }
        }
    }
}
