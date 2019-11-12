using System;
using System.Collections.Generic;
using System.Text;

namespace TweeterSearchApp.Models
{
    public class Tweet
    {
        public int TweetId { get; set; }

        public string Text { get; set; }

        public ServiceCategory Category { get; set; }

        public float SentimentPolarization { get; set; }
    }
}
