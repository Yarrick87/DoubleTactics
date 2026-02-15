using System;
using System.Collections;
using DoubleTactics.Events;
using DoubleTactics.Game.Board;
using DoubleTactics.Game.Cards;
using DoubleTactics.Input;
using DoubleTactics.Settings;
using DoubleTactics.UI.Popups;
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
        private bool _areCardsEqual;
        private GameSettings _settings;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }
        
        private void Start()
        {
            _settings = SettingsManager.Instance.GameSettings;
            
            SubscribeEvents();
            
            PopupManager.Instance.ShowPopup(PopupTypes.StartGame);
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventBus.Subscribe(EventTypes.StartGame, OnStartGame);
            EventBus.Subscribe(EventTypes.InputClick, OnInputClick);
        }
        
        private void UnsubscribeEvents()
        {
            EventBus.Unsubscribe(EventTypes.StartGame, OnStartGame);
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
            var delay = _areCardsEqual ? _settings.RemoveCardsDelay : _settings.HideCardsDelay;
            
            yield return new WaitForSeconds(delay);

            UpdateCardsState();
        }

        private void OnStartGame(IEventData eventData)
        {
            _shownCards = new Card[MAX_SHOWN_CARDS_AMOUNT];
            
            if (eventData?.GetType() != typeof(StartGameEventData))
            {
                Debug.LogError("Invalid start game event data");
                return;
            }
            
            var data = (StartGameEventData)eventData;
            _boardController.CreateBoard(data.CardsAmount);
        }
        
        private void OnInputClick(IEventData eventData)
        {
            if (eventData?.GetType() != typeof(InputClickEventData))
            {
                Debug.LogError("Invalid input click event data");
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
