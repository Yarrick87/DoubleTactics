using DoubleTactics.Events;

namespace DoubleTactics.Game
{
    public class StartGameEventData : IEventData
    {
        public int CardsAmount { get; private set; }

        public StartGameEventData(int cardsAmount)
        {
            CardsAmount = cardsAmount;
        }
    }
}
