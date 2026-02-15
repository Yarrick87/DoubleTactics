using DoubleTactics.Cards;
using DoubleTactics.Events;
using UnityEngine;

namespace DoubleTactics.Systems
{
    public class ScreenAdjuster : MonoBehaviour
    {
        private Camera _mainCamera;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
        }
        
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
            EventBus.Subscribe(EventTypes.CardsGenerated, OnCardsGenerated);
        }
        
        private void UnsubscribeEvents()
        {
            EventBus.Unsubscribe(EventTypes.CardsGenerated, OnCardsGenerated);
        }

        private void OnCardsGenerated(IEventData eventData)
        {
            if (eventData?.GetType() != typeof(CardsGeneratedEventData))
            {
                Debug.LogError("Invalid cards generated event data");
                return;
            }
            
            AdjustScreenSettings((CardsGeneratedEventData)eventData);
        }

        private void AdjustScreenSettings(CardsGeneratedEventData data)
        {
            var horizontalCardsSize = data.RightBottomPosition.x - data.LeftTopPosition.x + data.Size.x;
            var verticalCardsSize = data.LeftTopPosition.y - data.RightBottomPosition.y + data.Size.y;

            var vericalScreenSize = _mainCamera.orthographicSize * 2.0f;
            var horizontalScreenSize = vericalScreenSize * Screen.width / Screen.height;

            if (vericalScreenSize < verticalCardsSize ||
                horizontalScreenSize < horizontalCardsSize)
            {
                var verticalFactor = verticalCardsSize / vericalScreenSize;
                var horizontalFactor = horizontalCardsSize / horizontalScreenSize;

                if (verticalFactor > horizontalFactor)
                {
                    _mainCamera.orthographicSize = verticalCardsSize / 2.0f;
                }
                else
                {
                    _mainCamera.orthographicSize = (horizontalCardsSize * Screen.height) / (Screen.width * 2.0f);
                }
            }
        }
    }
}
