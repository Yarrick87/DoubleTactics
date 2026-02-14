using System.Collections;
using System.Collections.Generic;
using DoubleTactics.Game.Cards;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DoubleTactics.Game.Board
{
    public class BoardController : MonoBehaviour
    {
        public Card[] Cards;
        public Sprite CardBackSprite;
        public Sprite[] CardFrontSprites;

        public float RemoveCardsDelay = 1f;
        public float HideCardsDelay = 1f;

        public void CreateBoard()
        {
            PopulateBoard();
        }
        
        public void ShowCard(Card card)
        {
            card.Show();
        }
        
        public void UpdateCardsAfterComparison(Card[] cards, bool areEqual)
        {
            var delay = areEqual ? HideCardsDelay : RemoveCardsDelay;

            StartCoroutine(ComparisonDelayCoroutine(cards, delay, areEqual));
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
                int randomCardId = Random.Range(0, CardFrontSprites.Length);
                
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
                Cards[i].SetCard(CardBackSprite, CardFrontSprites[id], id);
            }
        }

        private IEnumerator ComparisonDelayCoroutine(Card[] cards, float delay, bool areEqual)
        {
            yield return new WaitForSeconds(delay);
            SetCardsAfterComparison(cards, areEqual);
        }

        private void SetCardsAfterComparison(Card[] cards, bool areEqual)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                if (areEqual)
                {
                    Destroy(cards[i].gameObject);
                }
                else
                {
                    cards[i].Hide();
                }
            }
        }
    }
}
