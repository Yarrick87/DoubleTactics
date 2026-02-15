using System;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleTactics.Game.Cards
{
    public class CardsGenerator
    {
        public Card[] GenerateCards(Card prefab, int cardsAmount, Vector3 size, float offsetFactor, Transform parent)
        {
            var positions = GetCardPositions(cardsAmount, size, offsetFactor);
            
            var cards = new Card[positions.Length];

            for (int i = 0; i < positions.Length; i++)
            {
                var card = GameObject.Instantiate(prefab, positions[i], Quaternion.identity, parent);
                cards[i] = card;
            }
            
            return cards;
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
    }
}
