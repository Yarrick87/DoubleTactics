using DoubleTactics.Events;
using DoubleTactics.Game.Cards;

namespace DoubleTactics.Board
{
    public class BoardPopulatedEventData : IEventData
    {
        public Card[] Cards { get; private set; }

        public BoardPopulatedEventData(Card[] cards)
        {
            Cards = cards;
        }
    }
}
