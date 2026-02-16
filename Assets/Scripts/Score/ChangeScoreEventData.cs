using DoubleTactics.Events;

namespace DoubleTactics.Score
{
    public class ChangeScoreEventData : IEventData
    {
        public int Score { get; private set; }
        public bool IsConsecutive { get; private set; }
        
        public ChangeScoreEventData(int score, bool isConsecutive)
        {
            Score = score;
            IsConsecutive = isConsecutive;
        }
    }
}
