using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Box.Security.Services.Types
{
    public interface ICaptchaResponse
    {
        bool Success { get; }
        DateTime ChallengeTs { get; }
        string Hostname { get; }
        IEnumerable<string> ErrorCodes { get; }
    }
}