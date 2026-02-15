using System.Collections.Generic;
using DoubleTactics.Systems;
using UnityEngine;

namespace DoubleTactics.Sound
{
    public enum SoundTypes
    {
        Click,
        Match,
        Mismatch,
        Finish,
    }
    
    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        [SerializeField]
        private SoundSettings _settings;
        
        [SerializeField]
        private AudioSource _audioSource;
        
        private Dictionary<SoundTypes, AudioClip> _sounds;
        
        private void Start()
        {
            SetupSounds();
        }
        
        public void PlaySound(SoundTypes type)
        {
            _audioSource.PlayOneShot(_sounds[type]);
        }

        private void SetupSounds()
        {
            _sounds = new Dictionary<SoundTypes, AudioClip>();
            _sounds.Add(SoundTypes.Click, _settings.ClickSound);
            _sounds.Add(SoundTypes.Match, _settings.MatchSound);
            _sounds.Add(SoundTypes.Mismatch, _settings.MismatchSound);
            _sounds.Add(SoundTypes.Finish, _settings.FinishSound);
        }
    }
}
