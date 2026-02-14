using DoubleTactics.Events;
using UnityEngine;

namespace DoubleTactics.Input
{
    public class InputManager : MonoBehaviour
    {
        public void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                var data = new InputClickEventData()
                {
                    Position = UnityEngine.Input.mousePosition,
                };

                EventBus.Invoke(EventTypes.InputClick, data);
            }
        }
    }
}
