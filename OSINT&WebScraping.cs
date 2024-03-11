using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;

namespace DeadSkull_Lib
{
    public class Instagram
    {
        // This is an asynchronous method that fetches the followers of a specific user on Instagram.
        public async Task GetFollowers(string targetId, string username, string password)
        {
            // Create a new UserSessionData object with the provided username and password.
            var userSession = new UserSessionData
            {
                UserName = username,
                Password = password
            };

            // Build an instance of the Instagram API with the user session data.
            IInstaApi apiInstance = InstaApiBuilder.CreateBuilder()
                .SetUser(userSession)
                .Build();

            // Fetch the followers of the target user. The PaginationParameters.MaxPagesToLoad(1) limits the results to one page.
            var followersResult = await apiInstance.UserProcessor.GetUserFollowersAsync(targetId, PaginationParameters.MaxPagesToLoad(1));

            // If the fetching operation is successful...
            if (followersResult.Succeeded)
            {
                // ...iterate over each follower...
                foreach (var follower in followersResult.Value)
                {
                    // ...and print the username of the follower to the console.
                    Console.WriteLine($"Follower: {follower.UserName}");
                }
            }
            else
            {
                // If the fetching operation fails, throw an exception.
                throw new Exception("Unable to fetch followers");
            }
        }

        // This is an asynchronous method that fetches the followings of a specific user on Instagram.
        public async Task GetFollowings(string targetId, string username, string password)
        {
            // Create a new UserSessionData object with the provided username and password.
            var userSession = new UserSessionData
            {
                UserName = username,
                Password = password
            };

            // Build an instance of the Instagram API with the user session data.
            IInstaApi apiInstance = InstaApiBuilder.CreateBuilder()
                .SetUser(userSession)
                .Build();

            // Fetch the followings of the target user. The PaginationParameters.MaxPagesToLoad(1) limits the results to one page.
            var followingsResult = await apiInstance.UserProcessor.GetUserFollowingAsync(targetId, PaginationParameters.MaxPagesToLoad(1));

            // If the fetching operation is successful...
            if (followingsResult.Succeeded)
            {
                // ...iterate over each follower...
                foreach (var followings in followingsResult.Value)
                {
                    // ...and print the username of the follower to the console.
                    Console.WriteLine($"followings: {followings.UserName}");
                }
            }
            else
            {
                // If the fetching operation fails, throw an exception.
                throw new Exception("Unable to fetch followers");
            }
        }

        // This is an asynchronous method that fetches and prints the comments of a specific media item on Instagram.
        public async Task GetComments(string mediaId)
        {
            // Create a new UserSessionData object with the provided username and password.
            var userSession = new UserSessionData
            {
                UserName = "username",
                Password = "password"
            };

            // Build an instance of the Instagram API with the user session data.
            IInstaApi apiInstance = InstaApiBuilder.CreateBuilder()
                .SetUser(userSession)
                .Build();

            // Attempt to log in to the Instagram API.
            var loggedInResult = await apiInstance.LoginAsync();

            // If the login operation is not successful...
            if (!loggedInResult.Succeeded)
            {
                // ...print an error message to the console and return from the method.
                Console.WriteLine("Unable to login");
                return;
            }

            // Fetch the comments of the specified media item. The PaginationParameters.MaxPagesToLoad(1) limits the results to one page.
            var commentsResult = await apiInstance.CommentProcessor.GetMediaCommentsAsync(mediaId, PaginationParameters.MaxPagesToLoad(1));

            // If the fetching operation is successful...
            if (commentsResult.Succeeded)
            {
                // ...iterate over each comment...
                foreach (var comment in commentsResult.Value.Comments)
                {
                    // ...and print the username of the commenter and the comment text to the console.
                    Console.WriteLine($"User: {comment.User.UserName}, Comment: {comment.Text}");
                }
            }
            else
            {
                // If the fetching operation fails, print an error message to the console.
                Console.WriteLine($"Unable to fetch comments: {commentsResult.Info.Message}");
            }
        }

        // This is an asynchronous method that fetches and prints the hashtags used by a specific user on Instagram.
        public async Task GetHashtags(string targetUsername)
        {
            // Create a new UserSessionData object with the provided username and password.
            var userSession = new UserSessionData
            {
                UserName = "username",
                Password = "password"
            };

            // Build an instance of the Instagram API with the user session data.
            IInstaApi apiInstance = InstaApiBuilder.CreateBuilder()
                .SetUser(userSession)
                .Build();

            // Fetch the user data for the specified username.
            var userResult = await apiInstance.UserProcessor.GetUserAsync(targetUsername);

            // If the fetching operation is successful...
            if (userResult.Succeeded)
            {
                // ...store the user data in a variable.
                var user = userResult.Value;

                // Fetch the media items posted by the user. The PaginationParameters.MaxPagesToLoad(1) limits the results to one page.
                var mediaResult = await apiInstance.UserProcessor.GetUserMediaAsync(user.UserName, PaginationParameters.MaxPagesToLoad(1));

                // If the fetching operation is successful...
                if (mediaResult.Succeeded)
                {
                    // ...iterate over each media item...
                    foreach (var media in mediaResult.Value)
                    {
                        // ...split the caption text into words, filter out the words that start with "#", and store them in a list.
                        var hashtags = media.Caption.Text.Split(' ').Where(word => word.StartsWith("#")).ToList();

                        // Print the ID of the media item and the hashtags used in its caption to the console.
                        Console.WriteLine($"Post: {media.Pk}, Hashtags: {string.Join(", ", hashtags)}");
                    }
                }
                else
                {
                    // If the fetching operation fails, print an error message to the console.
                    Console.WriteLine($"Unable to fetch media: {mediaResult.Info.Message}");
                }
            }
            else
            {
                // If the fetching operation fails, print an error message to the console.
                Console.WriteLine($"Unable to fetch user: {userResult.Info.Message}");
            }
        }

        // This is an asynchronous method that fetches and prints the geolocations of media items posted by a specific user on Instagram.
        public async Task GetGeolocations(IInstaApi api, string targetUsername)
        {
            // Fetch the user data for the specified username.
            var userResult = await api.UserProcessor.GetUserAsync(targetUsername);

            // If the fetching operation is successful...
            if (userResult.Succeeded)
            {
                // ...store the user data in a variable.
                var user = userResult.Value;

                // Fetch the media items posted by the user. The PaginationParameters.MaxPagesToLoad(1) limits the results to one page.
                var mediaResult = await api.UserProcessor.GetUserMediaAsync(user.UserName, PaginationParameters.MaxPagesToLoad(1));

                // If the fetching operation is successful...
                if (mediaResult.Succeeded)
                {
                    // ...iterate over each media item...
                    foreach (var media in mediaResult.Value)
                    {
                        // ...if the media item has a location...
                        if (media.Location != null)
                        {
                            // ...print the ID of the media item and its geolocation to the console.
                            Console.WriteLine($"Post: {media.Pk}, Geolocation: {media.Location.Lat}, {media.Location.Lng}");
                        }
                    }
                }
                else
                {
                    // If the fetching operation fails, print an error message to the console.
                    Console.WriteLine($"Unable to fetch media: {mediaResult.Info.Message}");
                }
            }
            else
            {
                // If the fetching operation fails, print an error message to the console.
                Console.WriteLine($"Unable to fetch user: {userResult.Info.Message}");
            }
        }

        // This is an asynchronous method that fetches and prints the phone numbers of the users followed by a specific user on Instagram.
        public async Task GetFollowingsPhoneNumber(string username, string password)
        {
            // Create a new UserSessionData object with the provided username and password.
            var userSession = new UserSessionData
            {
                UserName = username,
                Password = password
            };

            // Build an instance of the Instagram API with the user session data.
            IInstaApi apiInstance = InstaApiBuilder.CreateBuilder()
                .SetUser(userSession)
                .Build();

            // Attempt to log in to the Instagram API.
            var loggedInResult = await apiInstance.LoginAsync();

            // If the login operation is not successful...
            if (!loggedInResult.Succeeded)
            {
                // ...print an error message to the console and return from the method.
                Console.WriteLine("Unable to login");
                return;
            }

            // Fetch the users followed by the specified user. The PaginationParameters.MaxPagesToLoad(1) limits the results to one page.
            var followingsResult = await apiInstance.UserProcessor.GetUserFollowingAsync("targetUsername", PaginationParameters.MaxPagesToLoad(1));

            // If the fetching operation is not successful...
            if (!followingsResult.Succeeded)
            {
                // ...print an error message to the console and return from the method.
                Console.WriteLine("Unable to fetch followings");
                return;
            }

            // Store the users followed by the specified user in a variable.
            var followings = followingsResult.Value;

            // Ask the user how many phone numbers they want to retrieve.
            Console.WriteLine("How many phone numbers do you want to retrieve?");
            int numPhoneNumbers;
            // Try to parse the user's input as an integer.
            if (!int.TryParse(Console.ReadLine(), out numPhoneNumbers))
            {
                // If the parsing operation fails, print an error message to the console and return from the method.
                Console.WriteLine("Invalid input. Please enter a number.");
                return;
            }

            // Create a list to store the phone numbers.
            var phoneNumbers = new List<string>();
            // Iterate over each user followed by the specified user.
            foreach (var following in followings)
            {
                // If the list of phone numbers has reached the requested size, break the loop.
                if (phoneNumbers.Count >= numPhoneNumbers)
                    break;

                // Fetch the user data for the current user.
                var userResult = await apiInstance.UserProcessor.GetUserInfoByIdAsync(following.Pk);

                // If the fetching operation is successful and the user has a contact phone number...
                if (userResult.Succeeded && !string.IsNullOrEmpty(userResult.Value.ContactPhoneNumber))
                {
                    // ...add the phone number to the list.
                    phoneNumbers.Add(userResult.Value.ContactPhoneNumber);
                }
            }

            // Print each phone number in the list to the console.
            foreach (var phoneNumber in phoneNumbers)
            {
                Console.WriteLine(phoneNumber);
            }
        }

        // This is an asynchronous method that fetches and prints the phone numbers of the followers of a specific user on Instagram.
        public async Task GetFollowersPhoneNumber(string username, string password)
        {
            // Create a new UserSessionData object with the provided username and password.
            var userSession = new UserSessionData
            {
                UserName = username,
                Password = password
            };

            // Build an instance of the Instagram API with the user session data.
            IInstaApi apiInstance = InstaApiBuilder.CreateBuilder()
                .SetUser(userSession)
                .Build();

            // Attempt to log in to the Instagram API.
            var loggedInResult = await apiInstance.LoginAsync();

            // If the login operation is not successful...
            if (!loggedInResult.Succeeded)
            {
                // ...print an error message to the console and return from the method.
                Console.WriteLine("Unable to login");
                return;
            }

            // Fetch the followers of the specified user. The PaginationParameters.MaxPagesToLoad(1) limits the results to one page.
            var followersResult = await apiInstance.UserProcessor.GetUserFollowersAsync("targetUsername", PaginationParameters.MaxPagesToLoad(1));

            // If the fetching operation is not successful...
            if (!followersResult.Succeeded)
            {
                // ...print an error message to the console and return from the method.
                Console.WriteLine("Unable to fetch followers");
                return;
            }

            // Store the followers of the specified user in a variable.
            var followers = followersResult.Value;

            // Ask the user how many phone numbers they want to retrieve.
            Console.WriteLine("How many phone numbers do you want to retrieve?");
            int numPhoneNumbers;
            // Try to parse the user's input as an integer.
            if (!int.TryParse(Console.ReadLine(), out numPhoneNumbers))
            {
                // If the parsing operation fails, print an error message to the console and return from the method.
                Console.WriteLine("Invalid input. Please enter a number.");
                return;
            }

            // Create a list to store the phone numbers.
            var phoneNumbers = new List<string>();
            // Iterate over each follower of the specified user.
            foreach (var follower in followers)
            {
                // If the list of phone numbers has reached the requested size, break the loop.
                if (phoneNumbers.Count >= numPhoneNumbers)
                    break;

                // Fetch the user data for the current follower.
                var userResult = await apiInstance.UserProcessor.GetUserInfoByIdAsync(follower.Pk);

                // If the fetching operation is successful and the follower has a contact phone number...
                if (userResult.Succeeded && !string.IsNullOrEmpty(userResult.Value.ContactPhoneNumber))
                {
                    // ...add the phone number to the list.
                    phoneNumbers.Add(userResult.Value.ContactPhoneNumber);
                }
            }

            // Print each phone number in the list to the console.
            foreach (var phoneNumber in phoneNumbers)
            {
                Console.WriteLine(phoneNumber);
            }
        }

        // This is an asynchronous method that fetches and prints the public email addresses of the users followed by a specific user on Instagram.
        public async Task GetFollowingsEmail()
        {
            // Create a new UserSessionData object with the provided username and password.
            var userSession = new UserSessionData
            {
                UserName = "username",
                Password = "password"
            };

            // Build an instance of the Instagram API with the user session data.
            IInstaApi apiInstance = InstaApiBuilder.CreateBuilder()
                .SetUser(userSession)
                .Build();

            // Attempt to log in to the Instagram API.
            var loggedInResult = await apiInstance.LoginAsync();

            // If the login operation is not successful...
            if (!loggedInResult.Succeeded)
            {
                // ...print an error message to the console and return from the method.
                Console.WriteLine("Unable to login");
                return;
            }

            // Fetch the users followed by the specified user. The PaginationParameters.MaxPagesToLoad(1) limits the results to one page.
            var followingsResult = await apiInstance.UserProcessor.GetUserFollowingAsync("targetUsername", PaginationParameters.MaxPagesToLoad(1));

            // If the fetching operation is not successful...
            if (!followingsResult.Succeeded)
            {
                // ...print an error message to the console and return from the method.
                Console.WriteLine("Unable to fetch followings");
                return;
            }

            // Store the users followed by the specified user in a variable.
            var followings = followingsResult.Value;

            // Ask the user how many email addresses they want to retrieve.
            Console.WriteLine("How many Emails do you want to retrieve?");
            int numEmail;
            // Try to parse the user's input as an integer.
            if (!int.TryParse(Console.ReadLine(), out numEmail))
            {
                // If the parsing operation fails, print an error message to the console and return from the method.
                Console.WriteLine("Invalid input. Please enter a number.");
                return;
            }

            // Create a list to store the email addresses.
            var Emails = new List<string>();
            // Iterate over each user followed by the specified user.
            foreach (var following in followings)
            {
                // If the list of email addresses has reached the requested size, break the loop.
                if (Emails.Count >= numEmail)
                    break;

                // Fetch the user data for the current user.
                var userResult = await apiInstance.UserProcessor.GetUserInfoByIdAsync(following.Pk);

                // If the fetching operation is successful and the user has a public email address...
                if (userResult.Succeeded && !string.IsNullOrEmpty(userResult.Value.PublicEmail))
                {
                    // ...add the email address to the list.
                    Emails.Add(userResult.Value.PublicEmail);
                }
            }

            // Print each email address in the list to the console.
            foreach (var emails in Emails)
            {
                Console.WriteLine(emails);
            }
        }

        // This is an asynchronous method that fetches and prints the public email addresses of the followers of a specific user on Instagram.
        public async Task GetFollowersEmail()
        {
            // Create a new UserSessionData object with the provided username and password.
            var userSession = new UserSessionData
            {
                UserName = "username",
                Password = "password"
            };

            // Build an instance of the Instagram API with the user session data.
            IInstaApi apiInstance = InstaApiBuilder.CreateBuilder()
                .SetUser(userSession)
                .Build();

            // Attempt to log in to the Instagram API.
            var loggedInResult = await apiInstance.LoginAsync();

            // If the login operation is not successful...
            if (!loggedInResult.Succeeded)
            {
                // ...print an error message to the console and return from the method.
                Console.WriteLine("Unable to login");
                return;
            }

            // Fetch the followers of the specified user. The PaginationParameters.MaxPagesToLoad(1) limits the results to one page.
            var followersResult = await apiInstance.UserProcessor.GetUserFollowersAsync("targetUsername", PaginationParameters.MaxPagesToLoad(1));

            // If the fetching operation is not successful...
            if (!followersResult.Succeeded)
            {
                // ...print an error message to the console and return from the method.
                Console.WriteLine("Unable to fetch followers");
                return;
            }

            // Store the followers of the specified user in a variable.
            var followers = followersResult.Value;

            // Ask the user how many email addresses they want to retrieve.
            Console.WriteLine("How many Emails do you want to retrieve?");
            int numEmail;
            // Try to parse the user's input as an integer.
            if (!int.TryParse(Console.ReadLine(), out numEmail))
            {
                // If the parsing operation fails, print an error message to the console and return from the method.
                Console.WriteLine("Invalid input. Please enter a number.");
                return;
            }

            // Create a list to store the email addresses.
            var Emails = new List<string>();
            // Iterate over each follower of the specified user.
            foreach (var follower in followers)
            {
                // If the list of email addresses has reached the requested size, break the loop.
                if (Emails.Count >= numEmail)
                    break;

                // Fetch the user data for the current follower.
                var userResult = await apiInstance.UserProcessor.GetUserInfoByIdAsync(follower.Pk);

                // If the fetching operation is successful and the follower has a public email address...
                if (userResult.Succeeded && !string.IsNullOrEmpty(userResult.Value.PublicEmail))
                {
                    // ...add the email address to the list.
                    Emails.Add(userResult.Value.PublicEmail);
                }
            }

            // Print each email address in the list to the console.
            foreach (var emails in Emails)
            {
                Console.WriteLine(emails);
            }
        }
    }

    public class FaceBook
    {
    }

    public class Telegram
    {
    }

    public class Twitter
    {
    }

    public class Udemy
    {
    }

    public class WebSites
    {
    }
}