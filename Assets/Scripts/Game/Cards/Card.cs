using UnityEngine;

namespace DoubleTactics.Game.Cards
{
    public class Card : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        
        private Sprite _backSprite;
        private Sprite _frontSprite;
        
        // public int Id { get; private set; }
        // public bool IsShown { get; private set; }

        public int Id;
        public bool IsShown;

        public void SetCard(Sprite backSprite, Sprite frontSprite, int id)
        {
            _backSprite = backSprite;
            _frontSprite = frontSprite;
            Id = id;

            IsShown = false;
            _spriteRenderer.sprite = _backSprite;
        }

        public void Show()
        {
            IsShown = true;
            _spriteRenderer.sprite = _frontSprite;
        }
        
        public void Hide()
        {
            IsShown = false;
            _spriteRenderer.sprite = _backSprite;
        }
    }
}
