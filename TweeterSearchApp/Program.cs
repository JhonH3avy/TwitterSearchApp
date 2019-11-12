using LinqToTwitter;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Configuration;

namespace TweeterSearchApp
{
    class Program
    {
        static async Task Main()
        {
            try
            {
                await ExtractTweetsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static async Task ExtractTweetsAsync()
        {
            IAuthorizer auth = DoSingleUserAuth();

            await auth.AuthorizeAsync();

            var twitterCtx = new TwitterContext(auth);
            await SearchDemo.RunAsync(twitterCtx);
        }

        static IAuthorizer DoSingleUserAuth()
        {
            var appSettings = ConfigurationManager.AppSettings;
            var auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = appSettings[OAuthKeys.TwitterConsumerKey],
                    ConsumerSecret = appSettings[OAuthKeys.TwitterConsumerSecret],
                    AccessToken = appSettings[OAuthKeys.TwitterAccessToken],
                    AccessTokenSecret = appSettings[OAuthKeys.TwitterAccessTokenSecret]
                }
            };

            return auth;
        }
    }
}
