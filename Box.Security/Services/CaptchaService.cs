using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Box.Security.Services.Types;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Box.Security.Services
{
    public class CaptchaService : ICaptchaService
    {
        private IConfiguration Configuration { get; }
        
        ///<inheritdoc />
        public string Secret { get; }
        ///<inheritdoc />
        public string Api { get; }

        public CaptchaService(IConfiguration config)
        {
            Configuration = config;
            Secret = Configuration["CaptchaSecret"];
            Api = Configuration["CaptchaApi"];
        }
        
        ///<inheritdoc />
        public CaptchaResponse CaptchaSolved(string response)
        {
            return CaptchaSolvedAsync(response).GetAwaiter().GetResult();
        }

        ///<inheritdoc />
        public async Task<CaptchaResponse> CaptchaSolvedAsync(string response)
        {
            CaptchaResponse captchaResponse;
            using (var http = new HttpClient())
            {
                var payload = new Dictionary<string, string>
                {
                    {"secret", Secret},
                    {"response", response}
                };
                var payloadContent = new FormUrlEncodedContent(payload);
                captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(
                    await (await http.PostAsync(Api, payloadContent)).Content
                        .ReadAsStringAsync());
            }
            return captchaResponse;
        }
    }
}