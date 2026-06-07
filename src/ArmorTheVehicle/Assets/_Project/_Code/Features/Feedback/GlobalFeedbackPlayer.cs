using System.Collections.Generic;
using System.Linq;
using ZLinq;

namespace _Project._Code.Features.Feedback
{
    public class GlobalFeedbackPlayer
    {
        private readonly List<FeedbackData> _feedbackData;

        public GlobalFeedbackPlayer(List<FeedbackData> feedbackData) => _feedbackData = feedbackData;

        public void PlayFeedback(FeedbackType type) =>
            _feedbackData
                .AsValueEnumerable()
                .FirstOrDefault(f => f.Type == type)
                ?.Player.PlayFeedbacks();
    }
}