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
                await DoDemosAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.Write("\nPress any key to close console window...");
            Console.ReadKey(true);
        }

        static async Task DoDemosAsync()
        {
            IAuthorizer auth = ChooseAuthenticationStrategy();

            await auth.AuthorizeAsync();

            // This is how you access credentials after authorization.
            // The oauthToken and oauthTokenSecret do not expire.
            // You can use the userID to associate the credentials with the user.
            // You can save credentials any way you want - database, isolated storage, etc. - it's up to you.
            // You can retrieve and load all 4 credentials on subsequent queries to avoid the need to re-authorize.
            // When you've loaded all 4 credentials, LINQ to Twitter will let you make queries without re-authorizing.
            //
            //var credentials = auth.CredentialStore;
            //string oauthToken = credentials.OAuthToken;
            //string oauthTokenSecret = credentials.OAuthTokenSecret;
            //string screenName = credentials.ScreenName;
            //ulong userID = credentials.UserID;
            //

            var twitterCtx = new TwitterContext(auth);
            char key;

            do
            {
                ShowMenu();

                key = Console.ReadKey(true).KeyChar;

                switch (key)
                {
                    case 'E':
                        Console.WriteLine("\n\tRunning Search Demos...\n");
                        await SearchDemo.RunAsync(twitterCtx);
                        break;                    
                    default:
                        Console.WriteLine(key + " is unknown");
                        break;
                }

            } while (char.ToUpper(key) != 'Q');
        }

        static void ShowMenu()
        {
            Console.WriteLine("\nPlease select category:\n");

            Console.WriteLine("\t 0. Account Demos");
            Console.WriteLine("\t 1. Account Activity Demos");
            Console.WriteLine("\t 2. Block Demos");
            Console.WriteLine("\t 3. Direct Message Demos");
            Console.WriteLine("\t 4. Direct Message Event Demos");
            Console.WriteLine("\t 5. Favorite Demos");
            Console.WriteLine("\t 6. Friendship Demos");
            Console.WriteLine("\t 7. Geo Demos");
            Console.WriteLine("\t 8. Help Demos");
            Console.WriteLine("\t 9. List Demos");
            Console.WriteLine("\t A. Media Demos");
            Console.WriteLine("\t B. Mutes Demos");
            Console.WriteLine("\t C. Raw Demos");
            Console.WriteLine("\t D. Saved Search Demos");
            Console.WriteLine("\t E. Search Demos");
            Console.WriteLine("\t F. Status Demos");
            Console.WriteLine("\t G. Stream Demos");
            Console.WriteLine("\t H. Trend Demos");
            Console.WriteLine("\t I. User Demos");
            Console.WriteLine("\t J. Vine Demos");
            Console.WriteLine("\t K. Welcome Message Demos");
            Console.WriteLine();
            Console.Write("\t Q. End Program");
        }


        static IAuthorizer ChooseAuthenticationStrategy()
        {
            Console.WriteLine("Authentication Strategy:\n\n");

            Console.WriteLine("  1 - Pin (default)");
            Console.WriteLine("  2 - Application-Only");
            Console.WriteLine("  3 - Single User");
            Console.WriteLine("  4 - XAuth");

            Console.Write("\nPlease choose (1, 2, 3, or 4): ");
            ConsoleKeyInfo input = Console.ReadKey();
            Console.WriteLine("");

            IAuthorizer auth = null;

            switch (input.Key)
            {

                case ConsoleKey.D1:
                    auth = DoPinOAuth();
                    break;
                case ConsoleKey.D2:
                    auth = DoApplicationOnlyAuth();
                    break;
                case ConsoleKey.D3:
                    auth = DoSingleUserAuth();
                    break;
                case ConsoleKey.D4:
                    auth = DoXAuth();
                    break;
                default:
                    auth = DoPinOAuth();
                    break;
            }

            return auth;
        }

        static IAuthorizer DoPinOAuth()
        {
            var auth = new PinAuthorizer()
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = (string)ConfigurationManager.GetSection(OAuthKeys.TwitterConsumerKey),
                    ConsumerSecret = (string)ConfigurationManager.GetSection(OAuthKeys.TwitterConsumerSecret)
                },
                GoToTwitterAuthorization = pageLink => Process.Start(pageLink),
                GetPin = () =>
                {
                    Console.WriteLine(
                        "\nAfter authorizing this application, Twitter " +
                        "will give you a 7-digit PIN Number.\n");
                    Console.Write("Enter the PIN number here: ");
                    return Console.ReadLine();
                }
            };

            return auth;
        }

        static IAuthorizer DoApplicationOnlyAuth()
        {
            var auth = new ApplicationOnlyAuthorizer()
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = (string)ConfigurationManager.GetSection(OAuthKeys.TwitterConsumerKey),
                    ConsumerSecret = (string)ConfigurationManager.GetSection(OAuthKeys.TwitterConsumerSecret)
                },
            };

            return auth;
        }
        static IAuthorizer DoSingleUserAuth()
        {
            var auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = (string)ConfigurationManager.GetSection(OAuthKeys.TwitterConsumerKey),
                    ConsumerSecret = (string)ConfigurationManager.GetSection(OAuthKeys.TwitterConsumerSecret),
                    AccessToken = (string)ConfigurationManager.GetSection(OAuthKeys.TwitterAccessToken),
                    AccessTokenSecret = (string)ConfigurationManager.GetSection(OAuthKeys.TwitterAccessTokenSecret)
                }
            };

            return auth;
        }

        static IAuthorizer DoXAuth()
        {
            var auth = new XAuthAuthorizer
            {
                CredentialStore = new XAuthCredentials
                {
                    ConsumerKey = (string)ConfigurationManager.GetSection(OAuthKeys.TwitterConsumerKey),
                    ConsumerSecret = (string)ConfigurationManager.GetSection(OAuthKeys.TwitterConsumerSecret),
                    UserName = "YourUserName",
                    Password = "YourPassword"
                }
            };

            return auth;
        }

    }
}
