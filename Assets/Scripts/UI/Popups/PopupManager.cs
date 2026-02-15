using System.Collections.Generic;
using DoubleTactics.Systems;
using UnityEngine;

namespace DoubleTactics.UI.Popups
{
    public class PopupManager : SingletonMonoBehaviour<PopupManager>
    {
        [SerializeField]
        private PopupSettings _settings;
        
        [SerializeField]
        private RectTransform _uiCanvas;

        private Dictionary<PopupTypes, GameObject> _popups;

        protected override void Awake()
        {
            base.Awake();
            
            _popups = _settings.GetPopupsDictionary();
        }

        public void ShowPopup(PopupTypes type)
        {
            GameObject.Instantiate(_popups[type], _uiCanvas);
        }
    }
}
