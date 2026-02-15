using UnityEngine;

[CreateAssetMenu(fileName = "SoundSettings", menuName = "DoubleTacticsSettings/SoundSettings")]
public class SoundSettings : ScriptableObject
{
    [SerializeField]
    private AudioClip _clickSound;
    
    [SerializeField]
    private AudioClip _matchSound;
    
    [SerializeField]
    private AudioClip _mismatchSound;
    
    [SerializeField]
    private AudioClip _finishSound;
    
    public AudioClip ClickSound => _clickSound;
    public AudioClip MatchSound => _matchSound;
    public AudioClip MismatchSound => _mismatchSound;
    public AudioClip FinishSound => _finishSound;
}
