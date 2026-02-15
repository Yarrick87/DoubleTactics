using DoubleTactics.Events;

namespace DoubleTactics.Progress
{
    public class ProgressLoadedDataEvent : IEventData
    {
        public GameProgressData GameProgressData { get; private set; }

        public ProgressLoadedDataEvent(GameProgressData gameProgressData)
        {
            GameProgressData = gameProgressData;
        }
    }
}
