using System;
using System.Collections.Generic;
using DoubleTactics.Board;
using DoubleTactics.Events;
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
        private CardsGenerator _cardsGenerator;

        private void Awake()
        {
            _cardsGenerator = new CardsGenerator();
        }

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
            var size = _cardsSettings.BackSprite.bounds.size;
            
            _cards = _cardsGenerator.GenerateCards(cardPrefab, _cardsAmount, size,
                _cardsSettings.OffsetFactor, _cardsContainer);
            
            var data = new CardsGeneratedEventData(_cards[0].transform.position, 
                _cards[^1].transform.position, size);
            EventBus.Invoke(EventTypes.CardsGenerated, data);
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
