using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Box.Security.Services.Types
{
    public class CaptchaResponse : ICaptchaResponse
    {
        public bool Success { get; set; }
        
        [JsonProperty("challenge_ts")]
        public DateTime ChallengeTs { get; set; }
        
        public string Hostname { get; set; }
        
        [JsonProperty("error-codes")]
        public IEnumerable<string> ErrorCodes { get; set; }
    }
}