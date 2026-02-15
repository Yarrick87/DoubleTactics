using System;
using System.Collections.Generic;
using DoubleTactics.Game.Cards;
using DoubleTactics.Settings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DoubleTactics.Game.Board
{
    public class BoardController : MonoBehaviour
    {
        [SerializeField]
        private Transform _cardsContainer;

        private int _cardsAmount;
        private CardsSettings _cardsSettings;
        private Card[] _cards;

        public void CreateBoard(int cardsAmount)
        {
            _cardsAmount = cardsAmount;
            _cardsSettings = SettingsManager.Instance.CardsSettings;
            
            CreateCards();
            PopulateBoard();
        }
        
        public void ShowCard(Card card)
        {
            card.Show();
        }

        public void HideCard(Card card)
        {
            card.Hide();
        }

        public void RemoveCard(Card card)
        {
            DestroyImmediate(card.gameObject);
        }

        private void CreateCards()
        {
            var cardPrefab = _cardsSettings.CardPrefab;
            
            var positions = GetCardPositions(_cardsAmount, _cardsSettings.BackSprite.bounds.size);
            _cards = new Card[positions.Length];

            for (int i = 0; i < positions.Length; i++)
            {
                var card = GameObject.Instantiate(cardPrefab, positions[i], Quaternion.identity, _cardsContainer);
                _cards[i] = card;
            }
        }

        private Vector3[] GetCardPositions(int cardsAmount, Vector3 size)
        {
            var rows = 1;
            var columns = cardsAmount;
            
            for (int i = (int)Math.Sqrt(_cardsAmount); i >= 1; i--)
            {
                if (_cardsAmount % i == 0)
                {
                    rows = i;
                    columns = _cardsAmount / i;
                    break;
                }
            }
            
            var positions = new List<Vector3>();
            
            size.x *= 1.5f;
            size.y *= 1.5f;
            
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

        private void PopulateBoard()
        {
            var cardIdList = GetCardIds();
            
            var random = new System.Random();
            for (int i = cardIdList.Length - 1; i > 0; i--)
            {
                int index = random.Next(i + 1);
                
                (cardIdList[i], cardIdList[index]) = (cardIdList[index], cardIdList[i]);
            }
            
            SetCards(cardIdList);
        }

        private int[] GetCardIds()
        {
            var cardIdList = new List<int>();
            var usedCardIds = new HashSet<int>();
            
            var frontSprites = _cardsSettings.FrontSprites;

            while (cardIdList.Count < _cards.Length)
            {
                int randomCardId = Random.Range(0, frontSprites.Length);
                
                if (usedCardIds.Add(randomCardId))
                {
                    cardIdList.Add(randomCardId);
                    cardIdList.Add(randomCardId);
                }
            }

            return cardIdList.ToArray();
        }

        private void SetCards(int[] ids)
        {
            var backSprite = _cardsSettings.BackSprite;
            var frontSprites = _cardsSettings.FrontSprites;
            
            for (int i = 0; i < _cards.Length; i++)
            {
                var id = ids[i];
                _cards[i].SetCard(backSprite, frontSprites[id], id);
            }
        }
    }
}
