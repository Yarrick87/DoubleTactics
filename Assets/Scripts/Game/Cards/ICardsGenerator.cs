using UnityEngine;

namespace DoubleTactics.Game.Cards
{
    public interface ICardsGenerator
    {
        public Card[] GenerateCards();
        public void SetupCards(Card[] cards, Sprite backSprite, Sprite[] frontSprites);
    }
}
