using System;
using System.Collections.Generic;
using DoubleTactics.UI.Popups;
using UnityEngine;

namespace DoubleTactics.UI.Popups
{
    public enum PopupTypes
    {
        StartGame,
    }

    [Serializable]
    public struct PopupContainer
    {
        [SerializeField]
        private PopupTypes _type;
        
        [SerializeField]
        private GameObject _popup;
        
        public PopupTypes Type => _type;
        public GameObject Popup => _popup;
    }
}

[CreateAssetMenu(fileName = "PopupSettings", menuName = "DoubleTacticsSettings/PopupSettings")]
public class PopupSettings : ScriptableObject
{
    [SerializeField]
    private PopupContainer[] _popups;
    
    public PopupContainer[] Popups => _popups;
    
    public Dictionary<PopupTypes, GameObject> GetPopupsDictionary()
    {
        var dictionary = new Dictionary<PopupTypes, GameObject>();

        for (int i = 0; i < _popups.Length; i++)
        {
            dictionary.Add(_popups[i].Type, _popups[i].Popup);
        }
        
        return dictionary;
    }
}
