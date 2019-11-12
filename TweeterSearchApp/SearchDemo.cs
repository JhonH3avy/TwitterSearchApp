using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TweeterSearchApp
{
    public class SearchDemo
    {
        internal static async Task RunAsync(TwitterContext twitterCtx)
        {
            await DoSearchAsync(twitterCtx);
        }

        static async Task DoSearchAsync(TwitterContext twitterCtx)
        {
            string searchTerm = "avianca";

            var searchResponse =
                await
                (from search in twitterCtx.Search
                 where search.Type == SearchType.Search &&
                     search.Query == searchTerm &&
                     search.IncludeEntities == true &&
                     search.TweetMode == TweetMode.Extended &&
                     search.Count == 1500 
                 select search)
                 .FirstOrDefaultAsync();

            if (searchResponse?.Statuses != null)
                searchResponse.Statuses.ForEach(tweet =>
                    Console.WriteLine(
                        "\n  User: {0} ({1})\n  Tweet: {2}",
                        tweet.User.ScreenNameResponse,
                        tweet.User.UserIDResponse,
                        tweet.Text ?? tweet.FullText));
        }
    }
}
