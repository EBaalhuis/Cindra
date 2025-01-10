namespace Cindra
{
    public class Calculator
    {
        public static Result? GetResult(IEnumerable<Card> cards)
        {
            if (!cards.Any()) return null;

            var turns = GenerateAllValidTurns(cards.ToArray()).ToList();
            return new Result(turns);
        }

        private static int MaxCindraActivations => 1;

        private static IEnumerable<Turn> GenerateAllValidTurns(Card[] cards)
        {
            var result = new List<Turn>();

            for (int cindraActivations = 0; cindraActivations <= MaxCindraActivations; cindraActivations++)
            {
                for (int playedCardsMask = 0; playedCardsMask < Math.Pow(2, cards.Length); playedCardsMask++)
                {
                    for (int daggersAtStart = 0; daggersAtStart <= 2; daggersAtStart++)
                    {
                        var maxKunaiAttacks = daggersAtStart + 2 * cindraActivations;
                        for (int kunaiAttacks = 0; kunaiAttacks <= maxKunaiAttacks; kunaiAttacks++)
                        {
                            var moves = GetMoves(cards, kunaiAttacks, cindraActivations, (PlayedCards)playedCardsMask);

                            var newTurns = GetValidTurns_Rec(cards, GetInitialValidTurnsData(cards, moves, daggersAtStart));
                            result.AddRange(newTurns);
                        }
                    }
                }
            }

            return result;
        }

        private static IEnumerable<Turn> GetValidTurns_Rec(Card[] cards, ValidTurnsData data)
        {
            for (int i = 0; i < data.MovesAvailable.Count; i++)
            {
                var move = data.MovesAvailable.ElementAt(i);

                if (move == Move.Kunai)
                {
                    if (data.MovesAvailable.IndexOf(move) < i) continue;
                }

                if (move == Move.Cindra && data.DaggersInGrave == 0) continue;

                if (!IsValid(cards, move, data))
                {
                    continue;
                }

                var updatedData = data.GetUpdatedData(cards, move, i);
                if (!updatedData.MovesAvailable.Any())
                {
                    yield return new Turn(cards, updatedData.MovesSoFar, data.DaggersAtStart, updatedData.DaggersOffChain);
                }
                else
                {
                    foreach (var turn in GetValidTurns_Rec(cards, updatedData))
                    {
                        yield return turn;
                    }
                }
            }
        }

        private static ValidTurnsData GetInitialValidTurnsData(Card[] cards, List<Move> moves, int daggersAtStart)
        {
            var pitchMoves = moves.Where(m => GetPitch(cards, m) > 0).ToList();
            var nonPitchMoves = moves.Where(m => GetPitch(cards, m) == 0).ToList();
            var initialResource = pitchMoves.Sum(m => GetPitch(cards, m));

            return new ValidTurnsData(movesAvailable: nonPitchMoves, resources: initialResource,
                movesSoFar: pitchMoves, daggersOffChain: daggersAtStart, daggersAtStart: daggersAtStart,
                daggersInGrave: 2 - daggersAtStart);
        }

        private class ValidTurnsData
        {
            public ValidTurnsData(List<Move> movesAvailable, List<Move> movesSoFar, int daggersOffChain, int daggersOnChain = 0,
                int daggersInGrave = 0, int daggersAtStart = 0, int resources = 0, int draconicLinks = 0, bool actionPoint = true)
            {
                MovesAvailable = movesAvailable;
                MovesSoFar = movesSoFar;
                DaggersOffChain = daggersOffChain;
                DaggersOnChain = daggersOnChain;
                DaggersInGrave = daggersInGrave;
                DaggersAtStart = daggersAtStart;
                Resources = resources;
                DraconicLinks = draconicLinks;
                ActionPoint = actionPoint;
            }

            public ValidTurnsData GetUpdatedData(Card[] cards, Move move, int indexToSkip)
            {
                var newMovesAvailable = new List<Move>(MovesAvailable);
                newMovesAvailable.RemoveAt(indexToSkip);
                var newMovesSoFar = new List<Move>(MovesSoFar)
                {
                    move
                };
                var newDaggersOffChain = GetNewDaggersOffChain(move, DaggersOffChain, DaggersInGrave);
                var newDaggersOnChain = GetNewDaggersOnChain(move, DaggersOnChain);
                var newDaggersInGrave = GetNewDaggersInGrave(move, DaggersInGrave, DaggersOnChain);
                var newResources = GetNewResources(cards, move, Resources, DraconicLinks);
                var newDraconicLinks = GetNewDraconicLinks(cards, move, DraconicLinks);
                var newActionPoint = GetNewActionPoint(cards, move, ActionPoint);

                return new ValidTurnsData(newMovesAvailable, newMovesSoFar, newDaggersOffChain, newDaggersOnChain,
                    newDaggersInGrave, DaggersAtStart, newResources, newDraconicLinks, newActionPoint);
            }

            public List<Move> MovesAvailable { get; }
            public List<Move> MovesSoFar { get; }
            public int DaggersOffChain { get; }
            public int DaggersOnChain { get; }
            public int DaggersInGrave { get; }
            public int DaggersAtStart { get; }
            public int Resources { get; }
            public int DraconicLinks { get; }
            public bool ActionPoint { get; }
        }

        private static bool IsValid(Card[] cards, Move move, ValidTurnsData data)
        {
            return IsValidForCardCount(cards, move)
                && IsValidForResources(cards, move, data)
                && IsValidForGoAgain(move, data)
                && IsValidForDaggerCount(move, data)
                && IsValidMomentToBreakChain(move, data);
        }

        private static bool IsValidForCardCount(Card[] cards, Move move)
        {
            return move switch
            {
                Move.PlayCard1 => cards.Length > 0,
                Move.PlayCard2 => cards.Length > 1,
                Move.PlayCard3 => cards.Length > 2,
                Move.PlayCard4 => cards.Length > 3,
                _ => true,
            };
        }

        private static bool IsValidForResources(Card[] cards, Move move, ValidTurnsData data)
        {
            return GetCost(cards, move, data.DraconicLinks) <= data.Resources;
        }

        private static bool IsValidForGoAgain(Move move, ValidTurnsData data)
        {
            return move switch
            {
                Move.Kunai => data.ActionPoint,
                Move.PlayCard1 => data.ActionPoint,
                Move.PlayCard2 => data.ActionPoint,
                Move.PlayCard3 => data.ActionPoint,
                Move.PlayCard4 => data.ActionPoint,
                _ => true,
            };
        }

        private static int GetCost(Card[] cards, Move move, int draconicLinks)
        {
            return move switch
            {
                Move.Kunai => 1,
                Move.PlayCard1 => cards[0].Cost,
                Move.PlayCard2 => cards[1].Cost,
                Move.PlayCard3 => cards[2].Cost,
                Move.PlayCard4 => cards[3].Cost,
                Move.Cindra => Math.Max(3 - draconicLinks, 0),
                _ => 0,
            };
        }

        private static int GetPitch(Card[] cards, Move move)
        {
            return move switch
            {
                Move.PitchCard1 => cards[0].Pitch,
                Move.PitchCard2 => cards[1].Pitch,
                Move.PitchCard3 => cards[2].Pitch,
                Move.PitchCard4 => cards[3].Pitch,
                _ => 0,
            };
        }

        private static bool IsValidForDaggerCount(Move move, ValidTurnsData data)
        {
            return move switch
            {
                Move.Kunai => data.DaggersOffChain > 0,
                _ => true,
            };
        }

        private static bool IsValidMomentToBreakChain(Move move, ValidTurnsData data)
        {
            return move switch
            {
                Move.BreakChain => data.DaggersOnChain > 0 || !data.MovesAvailable.Skip(1).Any(),
                _ => true,
            };
        }

        private static int GetNewDaggersOffChain(Move move, int oldDaggersOffChain, int oldDaggersInGrave)
        {
            return move switch
            {
                Move.Kunai => oldDaggersOffChain - 1,
                Move.Cindra => oldDaggersOffChain + oldDaggersInGrave,
                _ => oldDaggersOffChain,
            };
        }

        private static int GetNewDaggersOnChain(Move move, int oldDaggersOnChain)
        {
            return move switch
            {
                Move.Kunai => oldDaggersOnChain + 1,
                Move.BreakChain => 0,
                _ => oldDaggersOnChain,
            };
        }

        private static int GetNewDaggersInGrave(Move move, int oldDaggersInGrave, int oldDaggersOnChain)
        {
            return move switch
            {
                Move.BreakChain => oldDaggersInGrave + oldDaggersOnChain,
                Move.Cindra => 0,
                _ => oldDaggersInGrave,
            };
        }

        private static int GetNewResources(Card[] cards, Move move, int oldResources, int draconicLinks)
        {
            return move switch
            {
                Move.Kunai => oldResources - 1,
                Move.PlayCard1 or Move.PlayCard2 or Move.PlayCard3 or Move.PlayCard4 or Move.Cindra =>
                    oldResources - GetCost(cards, move, draconicLinks),
                Move.PitchCard1 or Move.PitchCard2 or Move.PitchCard3 or Move.PitchCard4 =>
                    oldResources + GetPitch(cards, move),
                _ => oldResources,
            };
        }

        private static int GetNewDraconicLinks(Card[] cards, Move move, int oldDraconicLinks)
        {
            return move switch
            {
                Move.Kunai => oldDraconicLinks + 1,
                Move.PlayCard1 => cards[0].Draconic ? oldDraconicLinks + 1 : oldDraconicLinks,
                Move.PlayCard2 => cards[1].Draconic ? oldDraconicLinks + 1 : oldDraconicLinks,
                Move.PlayCard3 => cards[2].Draconic ? oldDraconicLinks + 1 : oldDraconicLinks,
                Move.PlayCard4 => cards[3].Draconic ? oldDraconicLinks + 1 : oldDraconicLinks,
                Move.BreakChain => 0,
                _ => oldDraconicLinks,
            };
        }

        private static bool GetNewActionPoint(Card[] cards, Move move, bool oldActionPoint)
        {
            return move switch
            {
                Move.PlayCard1 => cards[0].GoAgain,
                Move.PlayCard2 => cards[1].GoAgain,
                Move.PlayCard3 => cards[2].GoAgain,
                Move.PlayCard4 => cards[3].GoAgain,
                _ => oldActionPoint,
            };
        }

        private static List<Move> GetMoves(Card[] cards, int kunaiAttacks, int cindraActivations, PlayedCards playedCards)
        {
            var card1Move = playedCards.HasFlag(PlayedCards.Card1) ? Move.PlayCard1 : Move.PitchCard1;
            var card2Move = playedCards.HasFlag(PlayedCards.Card2) ? Move.PlayCard2 : Move.PitchCard2;
            var card3Move = playedCards.HasFlag(PlayedCards.Card3) ? Move.PlayCard3 : Move.PitchCard3;
            var card4Move = playedCards.HasFlag(PlayedCards.Card4) ? Move.PlayCard4 : Move.PitchCard4;

            var cardMoves = new List<Move>
            {
                card1Move,
                card2Move,
                card3Move,
                card4Move,
            };

            var moves = cardMoves.Take(cards.Length).ToList();

            for (int i = 0; i < kunaiAttacks; i++)
            {
                moves.Add(Move.Kunai);
            }

            for (int i = 0; i < cindraActivations; i++)
            {
                moves.Add(Move.Cindra);
                moves.Add(Move.BreakChain);
            }

            return moves;
        }

        [Flags]
        private enum PlayedCards
        {
            None = 0,
            Card1 = 1,
            Card2 = 2,
            Card3 = 4,
            Card4 = 8
        }
    }
}
