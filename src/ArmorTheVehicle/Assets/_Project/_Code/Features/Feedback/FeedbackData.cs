using System;
using MoreMountains.Feedbacks;

namespace _Project._Code.Features.Feedback
{
    [Serializable]
    public class FeedbackData
    {
        public FeedbackType Type;
        public MMF_Player Player;
    }
}