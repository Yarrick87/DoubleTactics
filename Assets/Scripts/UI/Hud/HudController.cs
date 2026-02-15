using DoubleTactics.Events;
using DoubleTactics.Score;
using TMPro;
using UnityEngine;

namespace DoubleTactics.UI.Hud
{
    public class HudController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _hudContainer;

        [SerializeField]
        private TMP_Text _scoreValueText;

        private void Start()
        {
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventBus.Subscribe(EventTypes.BoardPopulated, OnBoardPopulated);
            EventBus.Subscribe(EventTypes.ChangeScore, OnChangeScore);
            EventBus.Subscribe(EventTypes.BoardFinished, OnBoardFinished);
        }
        
        private void UnsubscribeEvents()
        {
            EventBus.Unsubscribe(EventTypes.BoardPopulated, OnBoardPopulated);
            EventBus.Unsubscribe(EventTypes.ChangeScore, OnChangeScore);
            EventBus.Unsubscribe(EventTypes.BoardFinished, OnBoardFinished);
        }

        private void UpdateScore(int newScore)
        {
            _scoreValueText.text = newScore.ToString();
        }
        
        private void OnBoardPopulated(IEventData eventData)
        {
            _hudContainer.SetActive(true);
        }

        private void OnChangeScore(IEventData eventData)
        {
            if (eventData?.GetType() != typeof(ChangeScoreEventData))
            {
                Debug.LogError("Invalid score change event data");
                return;
            }
            
            var data = (ChangeScoreEventData)eventData;
            UpdateScore(data.Score);
        }
        
        private void OnBoardFinished(IEventData eventData)
        {
            _hudContainer.SetActive(false);
        }
    }
}
