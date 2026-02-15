using DoubleTactics.Events;
using UnityEngine;

namespace DoubleTactics.Input
{
    public class InputManager : MonoBehaviour
    {
        public void Update()
        {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            if (UnityEngine.Input.touchCount > 0)
            {
                var data = new InputClickEventData(UnityEngine.Input.GetTouch(0).position);
                EventBus.Invoke(EventTypes.InputClick, data);
            }
#else
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                var data = new InputClickEventData(UnityEngine.Input.mousePosition);
                EventBus.Invoke(EventTypes.InputClick, data);
            }
#endif
        }
    }
}
