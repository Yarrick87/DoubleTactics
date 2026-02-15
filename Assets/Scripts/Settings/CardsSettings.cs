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

    public Card CardPrefab => _cardPrefab;
    public Sprite BackSprite => _backSprite;
    public Sprite[] FrontSprites => _frontSprites;
}
