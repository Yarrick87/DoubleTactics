using DoubleTactics.Events;
using UnityEngine;

namespace DoubleTactics.Input
{
    public class InputManager : MonoBehaviour, IInputManager
    {
        public void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                var data = new InputClickData()
                {
                    Position = UnityEngine.Input.mousePosition,
                };

                EventBus.Invoke(EventTypes.Click, data);
            }
        }
    }
}
