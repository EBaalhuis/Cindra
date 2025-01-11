namespace Cindra
{
    public class Card
    {
        public int Power { get; }
        public int Cost { get; }
        public int Pitch { get; }
        public bool GoAgain { get; }
        public bool Draconic { get; }
        public bool IsInArsenal { get; }
        public int IndexOnPage { get; }

        public Card(int power, int cost, int pitch, bool goAgain, bool draconic=true, bool isInArsenal=false)
        {
            Power = power;
            Cost = cost;
            Pitch = pitch;
            GoAgain = goAgain;
            Draconic = draconic;
            IsInArsenal = isInArsenal;
        }

        public Card(string? s, bool draconic, bool isInArsenal = false, int indexOnPage = 0)
        {
            if (s != null && s != string.Empty)
            {
                var split = s.Split(" ");
                Power = int.Parse(split[0]);
                Cost = int.Parse(split[2]);
                GoAgain = s.Contains("GA");
                Pitch = s.Contains("red") ? 1 : s.Contains("yellow") ? 2 : 3;
                Draconic = draconic;
                IsInArsenal |= isInArsenal;
                IndexOnPage = indexOnPage;
            }
        }

        public override string ToString()
        {
            var goAgainText = GoAgain ? " GA" : string.Empty;
            var colorText = Pitch switch
            {
                1 => "red",
                2 => "yellow",
                _ => "blue"
            };

            return $"{Power} for {Cost}{goAgainText} ({colorText})";
        }
    }
}
