using DoubleTactics.Events;
using UnityEngine;

namespace DoubleTactics.Input
{
    public class InputClickEventData : IEventData
    {
        public Vector3 Position { get; private set; }

        public InputClickEventData(Vector3 position)
        {
            Position = position;
        }
    }
}
