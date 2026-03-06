using FeedbackApp.Models;
using System.Collections.Generic;

namespace FeedbackApp.Services
{
    public class FeedbackService

    {
        private List<Feedback> feedbackList;

        public void AddFeedback(Feedback feedback)
        {
            if (feedbackList == null)
            {
                feedbackList = new List<Feedback>();
            }
            feedbackList.Add(feedback);
        }

        public IEnumerable<Feedback> GetFeedback() => feedbackList;
    }

}