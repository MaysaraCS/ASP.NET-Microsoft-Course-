using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using FeedbackApp.Models;
using Microsoft.JSInterop;

namespace FeedbackApp
{
    public class FeedbackState
    {
        private readonly IJSRuntime _jsRuntime;
        public FeedbackState(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }
        public async Task SaveFeedbackAsync(List<Feedback> feedbackList)
        {
            var json = JsonSerializer.Serialize(feedbackList);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "feedback", json);
        }
        

        public async Task<List<Feedback>> LoadFeedbackAsync()
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "feedback");
            if (!string.IsNullOrEmpty(json))
            {
                return new List<Feedback>();
            }
            var feedbackList =  JsonSerializer.Deserialize<List<Feedback>>(json);
            return feedbackList ?? new List<Feedback>();
        }
    }
}