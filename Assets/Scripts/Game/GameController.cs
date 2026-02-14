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
        
        private Camera _mainCamera;
        
        private Card[] _shownCards;
        private int _shownCardsAmount;

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

        private void UpdateCardsState(Card card)
        {
            _boardController.ShowCard(card);

            _shownCards[_shownCardsAmount] = card;
            _shownCardsAmount++;

            if (_shownCardsAmount >= MAX_SHOWN_CARDS_AMOUNT)
            {
                _shownCardsAmount = 0;
                CompareCards();
            }
        }

        private void CompareCards()
        {
            var areEqual = _shownCards[0].Id == _shownCards[1].Id;
            _boardController.UpdateCardsAfterComparison(_shownCards, areEqual);
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
                    UpdateCardsState(card);
                }
            }
        }
    }
}
