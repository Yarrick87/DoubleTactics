using DoubleTactics.Settings;

namespace DoubleTactics.Systems
{
    public class CardsAmountValidator
    {
        private GameSettings _gameSettings;
        
        public CardsAmountValidator()
        {
            _gameSettings = SettingsManager.Instance.GameSettings;
        }

        public bool IsValid(int cardsAmount)
        {
            var isValid = cardsAmount >= _gameSettings.MinCards &&
                          cardsAmount <= _gameSettings.MaxCards;

            return isValid;
        }
    }
}
