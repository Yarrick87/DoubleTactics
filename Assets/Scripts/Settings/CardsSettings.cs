using UnityEngine;

[CreateAssetMenu(fileName = "CardSettings", menuName = "CardSettings")]
public class CardsSettings : ScriptableObject
{
    [SerializeField]
    private Sprite _backSprite;
    
    [SerializeField]
    private Sprite[] _frontSprites;

    public Sprite BackSprite => _backSprite;
    public Sprite[] FrontSprites => _frontSprites;
}
