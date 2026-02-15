using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "DoubleTacticsSettings/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    private int _minCards = 4;

    [SerializeField]
    private int _maxCards = 20;

    [SerializeField]
    private int _scoreValue = 1;

    [SerializeField]
    private int _scoreMultiplier = 2;

    [SerializeField]
    private float _removeCardsDelay = 1f;
    
    [SerializeField]
    private float _hideCardsDelay = 1f;

    public int MinCards => _minCards;
    public int MaxCards => _maxCards;

    public int ScoreValue => _scoreValue;
    public int ScoreMultiplier => _scoreMultiplier;
    public float RemoveCardsDelay => _removeCardsDelay;
    public float HideCardsDelay => _hideCardsDelay;
}
