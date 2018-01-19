using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Box.Security.Data.Types;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Steeltoe.Discovery.Client;

namespace Box.Security.Services
{
    public class ApiService : IApiService
    {
        private readonly string ZUUL_PROXY;

        public ApiService(IDiscoveryClient discoveryClient, IConfiguration configuration)
        {
            DiscoveryClient = discoveryClient;
            Configuration = configuration;
            ZUUL_PROXY = Configuration["ZuulProxy"];
        }

        private IDiscoveryClient DiscoveryClient { get; }
        private IConfiguration Configuration { get; }

        public async Task<User> GetUserAsync(Guid userId)
        {
            User user = null;
            using (var http = new HttpClient())
            {
                try
                {
                    var response = await http.GetAsync($"{ZUUL_PROXY}/api/user/{userId}");
                    user = response.StatusCode == HttpStatusCode.OK
                        ? JsonConvert.DeserializeObject<User>(response.Content.ToString())
                        : null;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                    throw new ApiReadException($"Error while reading from {http.BaseAddress}");
                }
            }
            if (user == null)
                throw new UserNotFoundException($"User with id {userId} could not be found.");
            return user;
        }

        public User GetUser(Guid userId)
        {
            return GetUserAsync(userId).GetAwaiter().GetResult();
        }

        public async Task AddUserAsync(Guid userId)
        {
            using (var http = new HttpClient())
            {
                try
                {
                    var response = await http.PostAsync($"{ZUUL_PROXY}/api/user/{userId}", null);
                    if (!response.IsSuccessStatusCode)
                        throw new UserNotCreatedException(
                            $"The user with id {userId} is not added for backend-access. Status code: {response.StatusCode}");
                }
                catch (HttpRequestException)
                {
                    throw new ApiWriteException($"Error while writing on {http.BaseAddress}");
                }
            }
        }

        public void AddUser(Guid userId)
        {
            AddUserAsync(userId).GetAwaiter().GetResult();
        }
    }

    public class ApiWriteException : Exception
    {
        public ApiWriteException(string s) : base(s)
        {
        }
    }

    public class ApiReadException : Exception
    {
        public ApiReadException(string s) : base(s)
        {
        }
    }

    public class UserNotCreatedException : Exception
    {
        public UserNotCreatedException(string message) : base(message)
        {
        }
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string s) : base(s)
        {
        }
    }
}