using System.Collections.Generic;
using DoubleTactics.Board;
using DoubleTactics.Cards;
using DoubleTactics.Events;
using DoubleTactics.Game.Cards;
using DoubleTactics.Score;
using DoubleTactics.Systems;
using UnityEngine;
using EventBus = DoubleTactics.Events.EventBus;

namespace DoubleTactics.Progress
{
    public class ProgressManager : SingletonMonoBehaviour<ProgressManager>
    {
        private const string PROGRESS_KEY = "GameProgressData";
        
        private List<CardProgressData> _cardProgressDataList;
        private int _currentScore;
        private bool _isConsecutive;
        private bool _wasLoaded;
        
        private Vector3 _initLeftTopPos;
        private Vector3 _initRightBottomPos;
        
        private void Start()
        {
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
            
            TrySaveProgress();
        }

        public bool HasSavedData()
        {
            return PlayerPrefs.HasKey(PROGRESS_KEY);
        }

        public void LoadProgress()
        {
            _wasLoaded = true;
            
            var savedDataString =  PlayerPrefs.GetString(PROGRESS_KEY);
            
            var progressData = JsonUtility.FromJson<GameProgressData>(savedDataString);

            var data = new ProgressLoadedDataEvent(progressData);
            EventBus.Invoke(EventTypes.ProgressLoaded, data);
        }

        private void SubscribeEvents()
        {
            EventBus.Subscribe(EventTypes.BoardPopulated, OnBoardPopulated);
            EventBus.Subscribe(EventTypes.ShowCard, OnShowCard);
            EventBus.Subscribe(EventTypes.HideCard, OnHideCard);
            EventBus.Subscribe(EventTypes.RemoveCard, OnRemoveCard);
            EventBus.Subscribe(EventTypes.BoardFinished, OnBoardFinished);
            EventBus.Subscribe(EventTypes.ChangeScore, OnChangeScore);
        }

        private void UnsubscribeEvents()
        {
            EventBus.Unsubscribe(EventTypes.BoardPopulated, OnBoardPopulated);
            EventBus.Unsubscribe(EventTypes.ShowCard, OnShowCard);
            EventBus.Unsubscribe(EventTypes.HideCard, OnHideCard);
            EventBus.Unsubscribe(EventTypes.RemoveCard, OnRemoveCard);
            EventBus.Unsubscribe(EventTypes.BoardFinished, OnBoardFinished);
            EventBus.Unsubscribe(EventTypes.ChangeScore, OnChangeScore);
        }

        private void OnBoardPopulated(IEventData eventData)
        {
            if (eventData?.GetType() != typeof(BoardPopulatedEventData))
            {
                Debug.LogError("Invalid board populated event data");
                return;
            }
            
            var data = (BoardPopulatedEventData)eventData;
            CreateCardsProgressData(data.Cards);

            if (!_wasLoaded)
            {
                _initLeftTopPos = _cardProgressDataList[0].Position;
                _initRightBottomPos = _cardProgressDataList[^1].Position;
            }
        }

        private void CreateCardsProgressData(Card[] cards)
        {
            _cardProgressDataList = new List<CardProgressData>();

            for (int i = 0; i < cards.Length; i++)
            {
                var cardProgressData = new CardProgressData()
                {
                    Position = cards[i].transform.position,
                    IsShown = cards[i].IsShown,
                    Id = cards[i].Id,
                };
                
                _cardProgressDataList.Add(cardProgressData);
            }
        }

        private void TrySaveProgress()
        {
            if (_cardProgressDataList == null ||
                _cardProgressDataList.Count <= 0)
            {
                return;
            }
            
            var gameProgressData = new GameProgressData()
            {
                CardsData = _cardProgressDataList.ToArray(),
                CurrentScore = _currentScore,
                IsConsecutive = _isConsecutive,
                InitLeftTopPos = _initLeftTopPos,
                InitRightBottomPos = _initRightBottomPos,
            };

            var data = JsonUtility.ToJson(gameProgressData);
            
            PlayerPrefs.SetString(PROGRESS_KEY, data);
            PlayerPrefs.Save();
        }

        private void OnShowCard(IEventData eventData)
        {
            if (eventData?.GetType() != typeof(CardsManipulationEventData))
            {
                Debug.LogError("Invalid cards manipulation show event data");
                return;
            }
            
            var data = (CardsManipulationEventData)eventData;

            foreach (var cardProgressData in _cardProgressDataList)
            {
                if (cardProgressData.Id == data.Id &&
                    cardProgressData.Position == data.Position)
                {
                    cardProgressData.IsShown = true;
                }
            }
        }

        private void OnHideCard(IEventData eventData)
        {
            if (eventData?.GetType() != typeof(CardsManipulationEventData))
            {
                Debug.LogError("Invalid cards manipulation hide event data");
                return;
            }
            
            var data = (CardsManipulationEventData)eventData;
            
            foreach (var cardProgressData in _cardProgressDataList)
            {
                if (cardProgressData.Id == data.Id)
                {
                    cardProgressData.IsShown = false;
                }
            }
        }

        private void OnRemoveCard(IEventData eventData)
        {
            if (eventData?.GetType() != typeof(CardsManipulationEventData))
            {
                Debug.LogError("Invalid cards manipulation remove event data");
                return;
            }
            
            var data = (CardsManipulationEventData)eventData;
            
            _cardProgressDataList.RemoveAll(x => x.Id == data.Id);
        }

        private void OnBoardFinished(IEventData eventData)
        {
            _cardProgressDataList = null;
            _wasLoaded = false;
            
            PlayerPrefs.DeleteKey(PROGRESS_KEY);
            PlayerPrefs.Save();
        }
        
        private void OnChangeScore(IEventData eventData)
        {
            if (eventData?.GetType() != typeof(ChangeScoreEventData))
            {
                Debug.LogError("Invalid change score event data");
                return;
            }
            
            var data = (ChangeScoreEventData)eventData;
            _currentScore = data.Score;
            _isConsecutive = data.IsConsecutive;
        }
    }
}
