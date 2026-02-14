using System;
using System.Collections;
using DoubleTactics.Events;
using DoubleTactics.Game.Board;
using DoubleTactics.Game.Cards;
using DoubleTactics.Input;
using UnityEngine;

namespace DoubleTactics.Game
{
    public class GameController : MonoBehaviour
    {
        private const int MAX_SHOWN_CARDS_AMOUNT = 2;
        
        [SerializeField]
        private BoardController _boardController;
        
        public float RemoveCardsDelay = 3f;
        public float HideCardsDelay = 3f;
        
        private Camera _mainCamera;
        
        private Card[] _shownCards;
        private int _shownCardsAmount;
        private bool _areCardsEqual;

        private void Start()
        {
            _mainCamera = Camera.main;

            _shownCards = new Card[MAX_SHOWN_CARDS_AMOUNT];
            
            _boardController.CreateBoard();
            
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventBus.Subscribe(EventTypes.InputClick, OnInputClick);
        }
        
        private void UnsubscribeEvents()
        {
            EventBus.Unsubscribe(EventTypes.InputClick, OnInputClick);
        }

        private void UpdateBoardState(Card card)
        {
            if (_shownCardsAmount >= MAX_SHOWN_CARDS_AMOUNT)
            {
                ForceUpdateBoardState();
            }
            
            _boardController.ShowCard(card);

            _shownCards[_shownCardsAmount] = card;
            _shownCardsAmount++;

            if (_shownCardsAmount >= MAX_SHOWN_CARDS_AMOUNT)
            {
                CompareCards();
            }
        }

        private void ForceUpdateBoardState()
        {
            StopAllCoroutines();
            UpdateCardsState();
        }

        private void UpdateCardsState()
        {
            for (int i = 0; i < _shownCards.Length; i++)
            {
                if (_areCardsEqual)
                {
                    _boardController.RemoveCard(_shownCards[i]);
                }
                else
                {
                    _boardController.HideCard(_shownCards[i]);
                }
            }
            
            _shownCardsAmount = 0;
            Array.Clear(_shownCards, 0, _shownCards.Length);
        }

        private void CompareCards()
        {
            StartCoroutine(CompareCardsCoroutine());
        }
        
        private IEnumerator CompareCardsCoroutine()
        {
            _areCardsEqual = _shownCards[0].Id == _shownCards[1].Id;
            var delay = _areCardsEqual ? HideCardsDelay : RemoveCardsDelay;
            
            yield return new WaitForSeconds(delay);

            UpdateCardsState();
        }
        
        private void OnInputClick(IEventData eventData)
        {
            if (eventData?.GetType() != typeof(InputClickEventData))
            {
                return;
            }
            
            var inputData = (InputClickEventData)eventData;
            
            var hit = Physics2D.Raycast(_mainCamera.ScreenToWorldPoint(inputData.Position), Vector2.zero);
            if (hit.transform != null)
            {
                var card = hit.transform.GetComponent<Card>();
                if (card != null &&
                    !card.IsShown)
                {
                    UpdateBoardState(card);
                }
            }
        }
    }
}
