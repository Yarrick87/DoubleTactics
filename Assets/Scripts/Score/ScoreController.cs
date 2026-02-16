using DoubleTactics.Events;
using DoubleTactics.Game;
using DoubleTactics.Progress;
using DoubleTactics.Settings;
using UnityEngine;

namespace DoubleTactics.Score
{
    public class ScoreController : MonoBehaviour
    {
        private int _scoreValue;
        private int _scoreMultiplier;
        private int _currentScore;
        private bool _isConsecutive;
        
        private void Start()
        {
            var settings = SettingsManager.Instance.GameSettings;
            _scoreValue = settings.ScoreValue;
            _scoreMultiplier = settings.ScoreMultiplier;
            
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventBus.Subscribe(EventTypes.StartGame, OnStartGame);
            EventBus.Subscribe(EventTypes.ProgressLoaded, OnProgressLoaded);
            EventBus.Subscribe(EventTypes.CardsCompared, OnCardsCompared);
        }
        
        private void UnsubscribeEvents()
        {
            EventBus.Unsubscribe(EventTypes.StartGame, OnStartGame);
            EventBus.Unsubscribe(EventTypes.ProgressLoaded, OnProgressLoaded);
            EventBus.Unsubscribe(EventTypes.CardsCompared, OnCardsCompared);
        }

        private void OnCardsCompared(IEventData eventData)
        {
            if (eventData?.GetType() != typeof(ComparisonEventData))
            {
                Debug.LogError("Invalid comparison event data");
                return;
            }
            
            var comparisonData = (ComparisonEventData)eventData;

            _isConsecutive = comparisonData.IsConsecutive;
            var scoreMultiplier = _isConsecutive ? _scoreMultiplier : 1;
            _currentScore += _scoreValue * scoreMultiplier;
            InvokeScoreChange();
        }
        
        private void OnStartGame(IEventData eventData)
        {
            _currentScore = 0;
            InvokeScoreChange();
        }
        
        private void OnProgressLoaded(IEventData eventData)
        {
            if (eventData?.GetType() != typeof(ProgressLoadedDataEvent))
            {
                Debug.LogError("Invalid pogress loaded event data");
                return;
            }
            
            var data = (ProgressLoadedDataEvent)eventData;

            _isConsecutive = data.GameProgressData.IsConsecutive;
            _currentScore = data.GameProgressData.CurrentScore;
            InvokeScoreChange();
        }

        private void InvokeScoreChange()
        {
            var scoreData = new ChangeScoreEventData(_currentScore, _isConsecutive);
            EventBus.Invoke(EventTypes.ChangeScore, scoreData);
        }
    }
}
