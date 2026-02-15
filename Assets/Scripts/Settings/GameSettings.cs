using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "DoubleTacticsSettings/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private int _minCards = 4;

    [SerializeField] private int _maxCards = 20;

    [SerializeField] private int _scoreValue = 1;

    [SerializeField] private int _scoreMultiplier = 2;

    public int MinCards => _minCards;
    public int MaxCards => _maxCards;

    public int ScoreValue => _scoreValue;
    public int ScoreMultiplier => _scoreMultiplier;
}
