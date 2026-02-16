using UnityEngine;
using UnityEngine.Events;

namespace DoubleTactics.Game.Cards
{
    public class Card : MonoBehaviour
    {
        private const string ROTATE_TO_FRONT_ANIMATION_NAME = "RotateToFront";
        private const string ROTATE_TO_BACK_ANIMATION_NAME = "RotateToBack";
        private const string REMOVE_ANIMATION_NAME = "Remove";
        
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private Animation _animation;
        
        public UnityEvent<Card> OnCardRemoved;
        
        private Sprite _backSprite;
        private Sprite _frontSprite;
        
        public int Id { get; private set; }
        public bool IsShown { get; private set; }

        public void SetCard(Sprite backSprite, Sprite frontSprite, int id)
        {
            _backSprite = backSprite;
            _frontSprite = frontSprite;
            Id = id;

            IsShown = false;
            _spriteRenderer.sprite = _backSprite;
        }

        public void Show(bool hasAnimation = true)
        {
            IsShown = true;

            if (hasAnimation)
            {
                _animation.Play(ROTATE_TO_FRONT_ANIMATION_NAME);
            }
            else
            {
                _spriteRenderer.sprite = _frontSprite;
            }
        }
        
        public void Hide()
        {
            IsShown = false;
            _animation.Play(ROTATE_TO_BACK_ANIMATION_NAME);
        }

        public void Remove()
        {
            _animation.Play(REMOVE_ANIMATION_NAME);
        }

        // call from animation
        public void OnRotateToFrontMiddle()
        {
            _spriteRenderer.sprite = _frontSprite;
        }
        
        // call from animation
        public void OnRotateToBackMiddle()
        {
            _spriteRenderer.sprite = _backSprite;
        }
        
        // call from animation
        public void OnScaledToZero()
        {
            OnCardRemoved?.Invoke(this);
        }
    }
}
