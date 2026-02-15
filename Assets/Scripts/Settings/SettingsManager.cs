using DoubleTactics.Systems;
using UnityEngine;

namespace DoubleTactics.Settings
{
    public class SettingsManager : SingletonMonoBehaviour<SettingsManager>
    {
        [SerializeField]
        private CardsSettings _cardsSettings;
        
        [SerializeField]
        private GameSettings _gameSettings;

        public CardsSettings CardsSettings => _cardsSettings;
        public GameSettings GameSettings => _gameSettings;
    }
}
