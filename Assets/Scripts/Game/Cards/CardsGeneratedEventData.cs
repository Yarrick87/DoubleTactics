using DoubleTactics.Events;
using UnityEngine;

namespace DoubleTactics.Cards
{
    public class CardsGeneratedEventData : IEventData
    {
        public Vector3 LeftTopPosition { get; private set; }
        public Vector3 RightBottomPosition { get; private set; }
        public Vector3 Size { get; private set; }

        public CardsGeneratedEventData(Vector3 leftTop, Vector3 rightBottom, Vector3 size)
        {
            LeftTopPosition = leftTop;
            RightBottomPosition = rightBottom;
            Size = size;
        }
    }
}
