using System.Collections.Generic;
using DoubleTactics.Events;
using DoubleTactics.Game.Cards;
using UnityEngine;
using UnityEngine.UIElements;

namespace DoubleTactics.Game.Board
{
    public class BoardController : MonoBehaviour
    {
        public Card[] Cards;
        public Sprite CardBackSprite;
        public Sprite[] CardFrontSprites;
        
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
            
            PopulateBoard();

            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventBus.Subscribe(EventTypes.InputClick, OnClick);
        }
        
        private void UnsubscribeEvents()
        {
            EventBus.Unsubscribe(EventTypes.InputClick, OnClick);
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

            while (cardIdList.Count < Cards.Length)
            {
                int randomCardId = Random.Range(0, CardFrontSprites.Length);
                
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
            for (int i = 0; i < Cards.Length; i++)
            {
                var id = ids[i];
                Cards[i].SetCard(CardBackSprite, CardFrontSprites[id], id);
            }
        }

        private void OnClick(IEventData eventData)
        {
            if (eventData == null ||
                eventData.GetType() != typeof(ClickEvent))
            {
                return;
            }
            
            var data = (InputClickEventData)eventData;
            
            var hit = Physics2D.Raycast(_mainCamera.ScreenToWorldPoint(data.Position), Vector2.zero);
            if (hit.transform != null)
            {
                //
            }
        }
    }
}
