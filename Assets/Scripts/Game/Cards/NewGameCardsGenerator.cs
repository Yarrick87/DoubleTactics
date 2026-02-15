using System;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleTactics.Game.Cards
{
    public class NewGameCardsGenerator : ICardsGenerator
    {
        public Card _prefab;
        public int _cardsAmount;
        public Vector3 _size;
        public float _offsetFactor;
        public Transform _parent;

        public NewGameCardsGenerator(Card prefab, int cardsAmount, Vector3 size, float offsetFactor, Transform parent)
        {
            _prefab = prefab;
            _cardsAmount = cardsAmount;
            _size = size;
            _offsetFactor = offsetFactor;
            _parent = parent;
        }
        
        public Card[] GenerateCards()
        {
            var positions = GetCardPositions(_cardsAmount, _size, _offsetFactor);

            var cards = InstantiateCards(positions, _prefab, _parent);
            
            return cards;
        }

        public void SetupCards(Card[] cards, Sprite backSprite, Sprite[] frontSprites)
        {
            var cardIdList = GetCardIds(cards, frontSprites);
            
            var random = new System.Random();
            for (int i = cardIdList.Length - 1; i > 0; i--)
            {
                int index = random.Next(i + 1);
                
                (cardIdList[i], cardIdList[index]) = (cardIdList[index], cardIdList[i]);
            }
            
            SetCards(cards, cardIdList, backSprite, frontSprites);
        }
        
        private Vector3[] GetCardPositions(int cardsAmount, Vector3 size, float offsetFactor)
        {
            var rows = 1;
            var columns = cardsAmount;
            
            for (int i = (int)Math.Sqrt(cardsAmount); i >= 1; i--)
            {
                if (cardsAmount % i == 0)
                {
                    rows = i;
                    columns = cardsAmount / i;
                    break;
                }
            }
            
            var positions = new List<Vector3>();
            
            size.x *= offsetFactor;
            size.y *= offsetFactor;
            
            var initPosition = Vector3.zero;
            initPosition.x -= (size.x) * (columns - 1) / 2.0f;
            initPosition.y += (size.y) * (rows - 1) / 2.0f;
            
            var nextPosition = Vector3.zero;

            for (int i = 0; i < rows; i++)
            {
                nextPosition.y = initPosition.y - (size.y) * i;
                
                for (int j = 0; j < columns; j++)
                {
                    nextPosition.x = initPosition.x + (size.x) * j;

                    positions.Add(nextPosition);
                }
            }
            
            return positions.ToArray();
        }

        private Card[] InstantiateCards(Vector3[] positions, Card prefab, Transform parent)
        {
            var cards = new Card[positions.Length];
            
             for (int i = 0; i < positions.Length; i++)
             {
                 var card = GameObject.Instantiate(prefab, positions[i], Quaternion.identity, parent);
                 cards[i] = card;
             }
             
             return cards;
        }
        
        private int[] GetCardIds(Card[] cards, Sprite[] frontSprites)
        {
            var cardIdList = new List<int>();
            var usedCardIds = new HashSet<int>();

            while (cardIdList.Count < cards.Length)
            {
                int randomCardId = UnityEngine.Random.Range(0, frontSprites.Length);
                
                if (usedCardIds.Add(randomCardId))
                {
                    cardIdList.Add(randomCardId);
                    cardIdList.Add(randomCardId);
                }
            }

            return cardIdList.ToArray();
        }
        
        private void SetCards(Card[] cards, int[] ids, Sprite backSprite, Sprite[] frontSprites)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                var id = ids[i];
                cards[i].SetCard(backSprite, frontSprites[id], id);
            }
        }
    }
}
