using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private CardsSettings _cardsSettings;
    
    public CardsSettings CardsSettings => _cardsSettings;
    
    public static SettingsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }
}
