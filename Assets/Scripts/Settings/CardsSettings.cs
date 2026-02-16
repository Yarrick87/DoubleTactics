using DoubleTactics.Game.Cards;
using UnityEngine;

[CreateAssetMenu(fileName = "CardSettings", menuName = "DoubleTacticsSettings/CardSettings")]
public class CardsSettings : ScriptableObject
{
    [SerializeField]
    private Card _cardPrefab;
    
    [SerializeField]
    private Sprite _backSprite;
    
    [SerializeField]
    private Sprite[] _frontSprites;

    [SerializeField]
    private float _offsetFactor = 1.5f;

    public Card CardPrefab => _cardPrefab;
    public Sprite BackSprite => _backSprite;
    public Sprite[] FrontSprites => _frontSprites;
    public float OffsetFactor => _offsetFactor;
}
