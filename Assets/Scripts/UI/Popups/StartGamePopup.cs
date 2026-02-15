using System;
using DoubleTactics.Events;
using DoubleTactics.Game;
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
        
        private CardsAmountValidator _cardsAmountValidator;

        private void Start()
        {
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

        private void OnStartClick()
        {
            var pairsAmount = Int32.Parse(_inputField.text);
            if (!_cardsAmountValidator.IsValid(pairsAmount * 2))
            {
                _errorText.SetActive(true);
                return;
            }
            
            var data = new StartGameEventData(4);
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
