using System;
using DoubleTactics.Events;
using DoubleTactics.Game;
using DoubleTactics.Progress;
using DoubleTactics.Settings;
using DoubleTactics.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DoubleTactics.UI.Popups
{
    public class StartGamePopup : MonoBehaviour
    {
        [SerializeField]
        private Button _startGameButton;
        
        [SerializeField]
        private Button _loadGameButton;

        [SerializeField]
        private TMP_InputField _inputField;

        [SerializeField]
        private GameObject _errorText;
        
        private int _defaultCardPairsAmount;
        private CardsAmountValidator _cardsAmountValidator;
        private GameSettings _gameSettings;

        private void Start()
        {
            _gameSettings = SettingsManager.Instance.GameSettings;
            _cardsAmountValidator = new CardsAmountValidator();

            if (ProgressManager.Instance.HasSavedData())
            {
                _loadGameButton.gameObject.SetActive(true);
            }
            
            SetCardPairsAmount();
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _startGameButton.onClick.AddListener(OnStartClick);
            _loadGameButton.onClick.AddListener(OnLoadGameClick);
            _inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
            
            // EventBus.Subscribe(EventTypes.ProgressLoaded, OnProgressLoaded);
        }
        
        private void UnsubscribeEvents()
        {
            _startGameButton.onClick.RemoveListener(OnStartClick);
            _loadGameButton.onClick.RemoveListener(OnLoadGameClick);
            _inputField.onValueChanged.RemoveListener(OnInputFieldValueChanged);
            
            // EventBus.Unsubscribe(EventTypes.ProgressLoaded, OnProgressLoaded);
        }

        private void SetCardPairsAmount()
        {
            var amount = _gameSettings.PreviousCardPairsAmount > 0
                ? _gameSettings.PreviousCardPairsAmount
                : _gameSettings.DefaultCardPairsAmount;
            
            _defaultCardPairsAmount = amount;
            _inputField.text = _defaultCardPairsAmount.ToString();
        }

        private void OnStartClick()
        {
            var pairsAmount = Int32.Parse(_inputField.text);
            var cardsAmount = pairsAmount * 2;
            
            if (!_cardsAmountValidator.IsValid(cardsAmount))
            {
                _errorText.SetActive(true);
                return;
            }

            if (_gameSettings.PreviousCardPairsAmount != pairsAmount)
            {
                _gameSettings.PreviousCardPairsAmount = pairsAmount;
            }
            
            var data = new StartGameEventData(cardsAmount);
            EventBus.Invoke(EventTypes.StartGame, data);
            DestroyImmediate(this.gameObject);
        }

        private void OnInputFieldValueChanged(string text)
        {
            if (_errorText.activeSelf)
            {
                _errorText.SetActive(false);
            }
        }

        private void OnLoadGameClick()
        {
            ProgressManager.Instance.LoadProgress();
            DestroyImmediate(this.gameObject);
        }

        // private void OnProgressLoaded(IEventData eventData)
        // {
        //     DestroyImmediate(this.gameObject);
        // }
    }
}
