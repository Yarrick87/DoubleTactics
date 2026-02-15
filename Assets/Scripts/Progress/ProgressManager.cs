using System.Collections.Generic;
using DoubleTactics.Board;
using DoubleTactics.Cards;
using DoubleTactics.Events;
using DoubleTactics.Game.Cards;
using DoubleTactics.Systems;
using UnityEngine;
using EventBus = DoubleTactics.Events.EventBus;

namespace DoubleTactics.Progress
{
    public class ProgressManager : SingletonMonoBehaviour<ProgressManager>
    {
        private const string PROGRESS_KEY = "GameProgressData";
        
        private List<CardProgressData> _cardProgressDataList;
        
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
        }

        private void UnsubscribeEvents()
        {
            EventBus.Unsubscribe(EventTypes.BoardPopulated, OnBoardPopulated);
            EventBus.Unsubscribe(EventTypes.ShowCard, OnShowCard);
            EventBus.Unsubscribe(EventTypes.HideCard, OnHideCard);
            EventBus.Unsubscribe(EventTypes.RemoveCard, OnRemoveCard);
            EventBus.Unsubscribe(EventTypes.BoardFinished, OnBoardFinished);
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
            
            PlayerPrefs.DeleteKey(PROGRESS_KEY);
            PlayerPrefs.Save();
        }
    }
}
