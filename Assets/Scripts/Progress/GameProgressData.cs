using System;
using UnityEngine;

namespace DoubleTactics.Progress
{
    [Serializable]
    public class GameProgressData
    {
        public CardProgressData[] CardsData;
        public int CurrentScore;
        public bool IsConsecutive;
        public Vector3 InitLeftTopPos;
        public Vector3 InitRightBottomPos;
    }
}
