using DoubleTactics.Events;
using UnityEngine;

namespace DoubleTactics.Cards
{
    public class CardsManipulationEventData : IEventData
    {
        public Vector3 Position { get; private set; }
        public int Id { get; private set; }

        public CardsManipulationEventData(int id, Vector3 position = default)
        {
            Position = position;
            Id = id;
        }
    }
}
