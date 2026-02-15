using System.Collections.Generic;
using DoubleTactics.Board;
using DoubleTactics.Cards;
using DoubleTactics.Events;
using DoubleTactics.Game.Cards;
using DoubleTactics.Progress;
using DoubleTactics.Settings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DoubleTactics.Game.Board
{
    public class BoardController : MonoBehaviour
    {
        [SerializeField]
        private Transform _cardsContainer;

        private int _cardsAmount;
        private CardsSettings _cardsSettings;
        private Card[] _cards;
        // private NewGameCardsGenerator _cardsGenerator;
        private ICardsGenerator _cardsGenerator;

        // private void Awake()
        // {
        //     _cardsGenerator = new NewGameCardsGenerator();
        // }

        // public void CreateBoard(int cardsAmount)
        // {
        //     // var cardPrefab = _cardsSettings.CardPrefab;
        //     // var size = _cardsSettings.BackSprite.bounds.size;
        //     //
        //     // _cards = _cardsGenerator.GenerateCards(cardPrefab, _cardsAmount, size,
        //     //     _cardsSettings.OffsetFactor, _cardsContainer);
        //     //
        //     // var data = new CardsGeneratedEventData(_cards[0].transform.position, 
        //     //     _cards[^1].transform.position, size);
        //     // EventBus.Invoke(EventTypes.CardsGenerated, data);
        //     
        //     //----
        //     
        //     // _cardsAmount = cardsAmount;
        //     // _cardsSettings = SettingsManager.Instance.CardsSettings;
        //     //
        //     // Ololo();
        //     
        //     // CreateCards();
        //     // PopulateBoard();
        //     //
        //     // var data = new BoardPopulatedEventData(_cards);
        //     // EventBus.Invoke(EventTypes.BoardPopulated, data);
        // }

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
        
        //-----
        
        // private void Ololo()
        // {
        //     var cardPrefab = _cardsSettings.CardPrefab;
        //     var size = _cardsSettings.BackSprite.bounds.size;
        //     
        //     _cards = _cardsGenerator.GenerateCards(cardPrefab, _cardsAmount, size,
        //         _cardsSettings.OffsetFactor, _cardsContainer);
        //     
        //     var data = new CardsGeneratedEventData(_cards[0].transform.position, 
        //         _cards[^1].transform.position, size);
        //     EventBus.Invoke(EventTypes.CardsGenerated, data);
        // }
        
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

        // private void CreateCards()
        // {
        //     var cardPrefab = _cardsSettings.CardPrefab;
        //     var size = _cardsSettings.BackSprite.bounds.size;
        //     
        //     _cards = _cardsGenerator.GenerateCards(cardPrefab, _cardsAmount, size,
        //         _cardsSettings.OffsetFactor, _cardsContainer);
        //     
        //     var data = new CardsGeneratedEventData(_cards[0].transform.position, 
        //         _cards[^1].transform.position, size);
        //     EventBus.Invoke(EventTypes.CardsGenerated, data);
        // }

        private void PopulateBoard(int cardsAmount, Sprite backSprite, Sprite[] frontSprites)
        {
            _cardsAmount = cardsAmount;
            
            _cards = _cardsGenerator.GenerateCards();
            _cardsGenerator.SetupCards(_cards, backSprite, frontSprites);
            
            var data = new BoardPopulatedEventData(_cards);
            EventBus.Invoke(EventTypes.BoardPopulated, data);
        }
        
        // private void PopulateBoard()
        // {
        //     var cardIdList = GetCardIds();
        //     
        //     var random = new System.Random();
        //     for (int i = cardIdList.Length - 1; i > 0; i--)
        //     {
        //         int index = random.Next(i + 1);
        //         
        //         (cardIdList[i], cardIdList[index]) = (cardIdList[index], cardIdList[i]);
        //     }
        //     
        //     SetCards(cardIdList);
        // }

        // private int[] GetCardIds()
        // {
        //     var cardIdList = new List<int>();
        //     var usedCardIds = new HashSet<int>();
        //     
        //     var frontSprites = _cardsSettings.FrontSprites;
        //
        //     while (cardIdList.Count < _cards.Length)
        //     {
        //         int randomCardId = Random.Range(0, frontSprites.Length);
        //         
        //         if (usedCardIds.Add(randomCardId))
        //         {
        //             cardIdList.Add(randomCardId);
        //             cardIdList.Add(randomCardId);
        //         }
        //     }
        //
        //     return cardIdList.ToArray();
        // }

        // private void SetCards(int[] ids)
        // {
        //     var backSprite = _cardsSettings.BackSprite;
        //     var frontSprites = _cardsSettings.FrontSprites;
        //     
        //     for (int i = 0; i < _cards.Length; i++)
        //     {
        //         var id = ids[i];
        //         _cards[i].SetCard(backSprite, frontSprites[id], id);
        //     }
        // }
    }
}
