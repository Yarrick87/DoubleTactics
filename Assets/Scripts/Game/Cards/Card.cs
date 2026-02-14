using UnityEngine;

namespace DoubleTactics.Game.Cards
{
    public class Card : MonoBehaviour
    {
        private Sprite _backSprite;
        private Sprite _frontSprite;
        
        public int Id { get; private set; }

        public void SetCard(Sprite backSprite, Sprite frontSprite, int id)
        {
            _backSprite = backSprite;
            _frontSprite = frontSprite;
            Id = id;
        }
    }
}
