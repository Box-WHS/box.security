using System.Threading.Tasks;
using Box.Security.Services.Types;

namespace Box.Security.Services
{
    public interface ICaptchaService
    {
        /// <summary>
        /// The client-secret for Googles reCaptcha API
        /// </summary>
        string Secret { get; }
        
        /// <summary>
        /// API-Endpoint for reCaptcha solving
        /// </summary>
        string Api { get; }

        /// <summary>
        /// Checks if the reCaptcha is solved
        /// </summary>
        /// <param name="response">Id of the reCaptcha</param>
        /// <returns>validationResult</returns>
        CaptchaResponse CaptchaSolved(string response);

        /// <summary>
        /// Checks if the reCaptcha is solved async
        /// </summary>
        /// <param name="response">Id of the reCaptcha</param>
        /// <returns>Task with validationResult</returns>
        Task<CaptchaResponse> CaptchaSolvedAsync(string response);
    }
}