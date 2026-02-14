using System.Collections.Generic;
using DoubleTactics.Game.Cards;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DoubleTactics.Game.Board
{
    public class BoardController : MonoBehaviour
    {
        public Card[] Cards;

        private CardsSettings _cardsSettings;

        public void CreateBoard()
        {
            _cardsSettings = SettingsManager.Instance.CardsSettings;
            PopulateBoard();
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
        }

        private void PopulateBoard()
        {
            var cardIdList = GetCardIds();
            
            var random = new System.Random();
            for (int i = cardIdList.Length - 1; i > 0; i--)
            {
                int index = random.Next(i + 1);
                
                (cardIdList[i], cardIdList[index]) = (cardIdList[index], cardIdList[i]);
            }
            
            SetCards(cardIdList);
        }

        private int[] GetCardIds()
        {
            var cardIdList = new List<int>();
            var usedCardIds = new HashSet<int>();

            while (cardIdList.Count < Cards.Length)
            {
                int randomCardId = Random.Range(0, _cardsSettings.FrontSprites.Length);
                
                if (usedCardIds.Add(randomCardId))
                {
                    cardIdList.Add(randomCardId);
                    cardIdList.Add(randomCardId);
                }
            }

            return cardIdList.ToArray();
        }

        private void SetCards(int[] ids)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                var id = ids[i];
                Cards[i].SetCard(_cardsSettings.BackSprite, _cardsSettings.FrontSprites[id], id);
            }
        }
    }
}
