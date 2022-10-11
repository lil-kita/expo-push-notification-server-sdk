using ExpoCommunityNotificationServer.Exceptions;
using ExpoCommunityNotificationServer.Models;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExpoCommunityNotificationServer.Client
{
    /// <summary>
    /// Provides a class for sending push notifications using Expo server
    /// </summary>
    public class PushApiClient : BaseClient
    {
        /// <summary>
        /// Do not forget to set auth token before sending request, if token was not passed to constructor.<br></br>
        /// Token from options have priority over token.<br></br>
        /// Settings object have priority over settings object from options.<br></br>
        /// </summary>
        /// <param name="token">Expo auth token.</param>
        /// <param name="httpClient">Custom HttpClient object.</param>
        /// <param name="options">Pass settings using options.<br></br> Token from options have priority over token.</param>
        /// <param name="settings">Pass expo settings<br></br> Settings object have priority over settings object from options.</param>
        public PushApiClient(string token = null, HttpClient httpClient = null, IOptions<ExpoSettings> options = null, ExpoSettings settings = null) : base(httpClient)
        {
            settings = settings ?? options?.Value ?? new ExpoSettings();
            if (string.IsNullOrWhiteSpace(settings.ExpoAuthToken))
            {
                settings.ExpoAuthToken = token;
            }

            SetSettings(settings);
        }

        [Obsolete("Please use public PushApiClient(string token = null, HttpClient httpClient = null, IOptions<ExpoSettings> options = null, ExpoSettings settings = null)")]
        public PushApiClient(HttpClient httpClient) : base(httpClient) { }

        [Obsolete("Please use public PushApiClient(string token = null, HttpClient httpClient = null, IOptions<ExpoSettings> options = null, ExpoSettings settings = null)")]
        public PushApiClient(string token) : base(token) { }

        [Obsolete("Please use public PushApiClient(string token = null, HttpClient httpClient = null, IOptions<ExpoSettings> options = null, ExpoSettings settings = null)")]
        public PushApiClient(IOptions<ExpoSettings> options) : base(options.Value?.ExpoAuthToken) { }

        [Obsolete("Please use public PushApiClient(string token = null, HttpClient httpClient = null, IOptions<ExpoSettings> options = null, ExpoSettings settings = null)")]
        public PushApiClient(string token, HttpClient httpClient) : base(token, httpClient) { }

        [Obsolete("Please use public PushApiClient(string token = null, HttpClient httpClient = null, IOptions<ExpoSettings> options = null, ExpoSettings settings = null)")]
        public PushApiClient(IOptions<ExpoSettings> options, HttpClient httpClient) : base(options.Value?.ExpoAuthToken, httpClient) { }

        /// <summary>
        /// Set new auth token or replace the old one.
        /// </summary>
        /// <param name="token">Expo auth token.</param>
        /// <exception cref="InvalidTokenException">Token is null, empty or white space.</exception>
        public override void SetToken(string token) => ValidateAndSetToken(token);

        /// <summary>
        /// Send push notification.
        /// It may either be a single message object or an array of up to 100 message objects.
        /// </summary>
        /// <param name="pushTicketRequest">Push notification object or an array of up to 100 objects.</param>
        /// <returns>Response with statuses and other info about sent push notifications.</returns>
        /// <exception cref="InvalidTokenException">Token was not set.</exception>
        /// <exception cref="InvalidRequestException">PushTicketMessages count must be between 1 and 100.</exception>
        /// <exception cref="HttpPostException">HttpRequestException or unsuccessfull status code</exception>
        public override async Task<PushTicketResponse> SendPushAsync(params PushTicketRequest[] pushTicketRequest)
        {
            Validate(pushTicketRequest);
            StringContent requestBody = Serialize(pushTicketRequest);

            return await PostAsync<PushTicketResponse>(SendPushPath, requestBody);
        }

        /// <summary>
        /// Send request to get push notification receipts.
        /// Make sure you are only sending a list of 1000 (or less) ticket ID strings.
        /// </summary>
        /// <param name="pushReceiptRequest">Request that contains list of 1000 (or less) ticket ID strings.</param>
        /// <returns>Response with requested receipts.</returns>
        /// <exception cref="InvalidTokenException">Token was not set.</exception>
        /// <exception cref="InvalidRequestException">PushTicketIds must be between 1 and 1000.</exception>
        /// <exception cref="HttpPostException">HttpRequestException or unsuccessfull status code</exception>
        public override async Task<PushReceiptResponse> GetReceiptsAsync(PushReceiptRequest pushReceiptRequest)
        {
            Validate(pushReceiptRequest);
            StringContent requestBody = Serialize(pushReceiptRequest);

            return await PostAsync<PushReceiptResponse>(GetReceiptsPath, requestBody);
        }
    }
}
