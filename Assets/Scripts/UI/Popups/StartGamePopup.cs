using System;
using DoubleTactics.Events;
using DoubleTactics.Game;
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
        private TMP_InputField _inputField;

        [SerializeField]
        private GameObject _errorText;
        
        private int _defaultCardPairsAmount;
        private CardsAmountValidator _cardsAmountValidator;
        private GameSettings _gameSettings;

        private void Start()
        {
            _gameSettings = SettingsManager.Instance.GameSettings;
            
            SetCardPairsAmount();
            
            _cardsAmountValidator = new CardsAmountValidator();
            
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _startGameButton.onClick.AddListener(OnStartClick);
            _inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
        }
        
        private void UnsubscribeEvents()
        {
            _startGameButton.onClick.RemoveListener(OnStartClick);
            _inputField.onValueChanged.RemoveListener(OnInputFieldValueChanged);
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
            var cardsAmount = Int32.Parse(_inputField.text) * 2;
            if (!_cardsAmountValidator.IsValid(cardsAmount))
            {
                _errorText.SetActive(true);
                return;
            }

            _gameSettings.PreviousCardPairsAmount = cardsAmount / 2;
            
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
    }
}
