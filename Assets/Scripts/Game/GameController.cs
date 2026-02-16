using System;
using System.Collections;
using DoubleTactics.Board;
using DoubleTactics.Cards;
using DoubleTactics.Events;
using DoubleTactics.Game.Board;
using DoubleTactics.Game.Cards;
using DoubleTactics.Input;
using DoubleTactics.Progress;
using DoubleTactics.Settings;
using DoubleTactics.Sound;
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
        private bool _isConsecutiveGuess;

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
            EventBus.Subscribe(EventTypes.ProgressLoaded, OnProgressLoaded);
            EventBus.Subscribe(EventTypes.InputClick, OnInputClick);
            EventBus.Subscribe(EventTypes.BoardFinished, OnBoardFinished);
            EventBus.Subscribe(EventTypes.BoardPopulated, OnBoardPopulated);
            EventBus.Subscribe(EventTypes.ExitGame, OnExitGame);
        }
        
        private void UnsubscribeEvents()
        {
            EventBus.Unsubscribe(EventTypes.StartGame, OnStartGame);
            EventBus.Unsubscribe(EventTypes.ProgressLoaded, OnProgressLoaded);
            EventBus.Unsubscribe(EventTypes.InputClick, OnInputClick);
            EventBus.Unsubscribe(EventTypes.BoardFinished, OnBoardFinished);
            EventBus.Unsubscribe(EventTypes.BoardPopulated, OnBoardPopulated);
            EventBus.Unsubscribe(EventTypes.ExitGame, OnExitGame);
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
            else
            {
                SoundManager.Instance.PlaySound(SoundTypes.Click);
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
                var data = new CardsManipulationEventData(_shownCards[i].Id);
                
                if (_areCardsEqual)
                {
                    EventBus.Invoke(EventTypes.RemoveCard, data);
                    _boardController.RemoveCard(_shownCards[i]);
                }
                else
                {
                    EventBus.Invoke(EventTypes.HideCard, data);
                    _boardController.HideCard(_shownCards[i]);
                }
            }

            if (_areCardsEqual)
            {
                var comparisonData = new ComparisonEventData(_isConsecutiveGuess);
                EventBus.Invoke(EventTypes.CardsCompared, comparisonData);
            }

            _isConsecutiveGuess = _areCardsEqual;
            
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
            
            var soundType = _areCardsEqual ? SoundTypes.Match : SoundTypes.Mismatch;
            SoundManager.Instance.PlaySound(soundType);
            
            yield return new WaitForSeconds(delay);

            UpdateCardsState();
        }

        private void OnStartGame(IEventData eventData)
        {
            _shownCards = new Card[MAX_SHOWN_CARDS_AMOUNT];
            _isConsecutiveGuess = false;
            
            if (eventData?.GetType() != typeof(StartGameEventData))
            {
                Debug.LogError("Invalid start game event data");
                return;
            }
            
            var data = (StartGameEventData)eventData;
            _boardController.CreateBoard(data.CardsAmount);
        }

        private void OnProgressLoaded(IEventData eventData)
        {
            _shownCards = new Card[MAX_SHOWN_CARDS_AMOUNT];
            
            if (eventData?.GetType() != typeof(ProgressLoadedDataEvent))
            {
                Debug.LogError("Invalid progress loaded event data");
                return;
            }
            
            var data = (ProgressLoadedDataEvent)eventData;
            _isConsecutiveGuess = data.GameProgressData.IsConsecutive;
            _boardController.CreateBoard(data);
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
                    var data = new CardsManipulationEventData(card.Id, card.transform.position);
                    EventBus.Invoke(EventTypes.ShowCard, data);
                    
                    UpdateBoardState(card);
                }
            }
        }

        private void OnBoardFinished(IEventData eventData)
        {
            PopupManager.Instance.ShowPopup(PopupTypes.StartGame);
        }

        private void OnBoardPopulated(IEventData eventData)
        {
            if (eventData?.GetType() != typeof(BoardPopulatedEventData))
            {
                Debug.LogError("Invalid board populated event data");
                return;
            }

            var data = (BoardPopulatedEventData)eventData;
            var cards = data.Cards;

            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i].IsShown)
                {
                    _shownCards[_shownCardsAmount] = cards[i];
                    _shownCardsAmount++;
                }
            }
        }

        private void OnExitGame(IEventData eventData)
        {
            Application.Quit();
        }
    }
}
