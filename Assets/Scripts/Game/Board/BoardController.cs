using DoubleTactics.Board;
using DoubleTactics.Cards;
using DoubleTactics.Events;
using DoubleTactics.Game.Cards;
using DoubleTactics.Progress;
using DoubleTactics.Settings;
using UnityEngine;

namespace DoubleTactics.Game.Board
{
    public class BoardController : MonoBehaviour
    {
        [SerializeField]
        private Transform _cardsContainer;

        private int _cardsAmount;
        private CardsSettings _cardsSettings;
        private Card[] _cards;
        private ICardsGenerator _cardsGenerator;

        private void Start()
        {
            _cardsSettings = SettingsManager.Instance.CardsSettings;
        }

        public void CreateBoard(int cardsAmount)
        {
            _cardsGenerator = new NewGameCardsGenerator(_cardsSettings.CardPrefab, cardsAmount,
                _cardsSettings.BackSprite.bounds.size, _cardsSettings.OffsetFactor, _cardsContainer);
            
            PopulateBoard(cardsAmount, _cardsSettings.BackSprite, _cardsSettings.FrontSprites);
        }

        public void CreateBoard(CardProgressData[] cardsData)
        {
            _cardsGenerator = new LoadedProgressCardsGenerator(cardsData, _cardsSettings.CardPrefab, _cardsContainer);
            
            PopulateBoard(cardsData.Length, _cardsSettings.BackSprite, _cardsSettings.FrontSprites);
        }
        
        public void ShowCard(Card card)
        {
            card.Show();
        }

        public void HideCard(Card card)
        {
            card.Hide();
        }

        public void RemoveCard(Card card)
        {
            DestroyImmediate(card.gameObject);
            _cardsAmount--;

            if (_cardsAmount <= 0)
            {
                EventBus.Invoke(EventTypes.BoardFinished);
            }
        }

        private void PopulateBoard(int cardsAmount, Sprite backSprite, Sprite[] frontSprites)
        {
            _cardsAmount = cardsAmount;
            
            _cards = _cardsGenerator.GenerateCards();

            var size = backSprite.bounds.size;
            var cardsGeneratedData = new CardsGeneratedEventData(_cards[0].transform.position, 
                _cards[^1].transform.position, size);
            EventBus.Invoke(EventTypes.CardsGenerated, cardsGeneratedData);
            
            _cardsGenerator.SetupCards(_cards, backSprite, frontSprites);
            
            var boardPopulatedData = new BoardPopulatedEventData(_cards);
            EventBus.Invoke(EventTypes.BoardPopulated, boardPopulatedData);
        }
    }
}
