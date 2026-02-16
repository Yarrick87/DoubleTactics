using DoubleTactics.Events;

namespace DoubleTactics.Game
{
    public class ComparisonEventData : IEventData
    {
        public bool IsConsecutive { get; private set; }

        public ComparisonEventData(bool isConsecutive)
        {
            IsConsecutive = isConsecutive;
        }
    }
}
