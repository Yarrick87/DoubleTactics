using DoubleTactics.Progress;
using UnityEngine;

namespace DoubleTactics.Game.Cards
{
    public class LoadedProgressCardsGenerator : ICardsGenerator
    {
        public Card _prefab;
        private CardProgressData[] _cardsData;
        public Transform _parent;
        
        public LoadedProgressCardsGenerator(CardProgressData[] cardsData, Card prefab, Transform parent)
        {
            _cardsData = cardsData;
            _prefab = prefab;
            _parent = parent;
        }
        
        public Card[] GenerateCards()
        {
            var cards = InstantiateCards(_cardsData, _prefab, _parent);
            
            return cards;
        }

        public void SetupCards(Card[] cards, Sprite backSprite, Sprite[] frontSprites)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].SetCard(backSprite, frontSprites[_cardsData[i].Id], _cardsData[i].Id);

                if (_cardsData[i].IsShown)
                {
                    cards[i].Show(false);
                }
            }
        }

        private Card[] InstantiateCards(CardProgressData[] cardsData, Card prefab, Transform parent)
        {
            var cards = new Card[cardsData.Length];
            
            for (int i = 0; i < cardsData.Length; i++)
            {
                var card = GameObject.Instantiate(prefab, cardsData[i].Position, Quaternion.identity, parent);
                cards[i] = card;
            }
             
            return cards;
        }
    }
}
