using ExpoCommunityNotificationServer.Exceptions;
using ExpoCommunityNotificationServer.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExpoCommunityNotificationServer.Client
{
    /// <summary>
    /// Provides a class for sending push notifications using Expo server
    /// </summary>
    public sealed class PushApiClient : BaseClient
    {
        /// <summary>
        /// Client without auth token. Set auth token before sending request
        /// </summary>
        public PushApiClient() : base() { }

        /// <summary>
        /// Client without auth token. Set auth token before sending request
        /// </summary>
        /// <param name="httpClient">Custom HttpClient object</param>
        /// <exception cref="InvalidTokenException">Token is null, empty or white space.</exception>
        public PushApiClient(HttpClient httpClient) : base(httpClient) { }

        /// <summary>
        /// Client with auth token.
        /// </summary>
        /// <param name="token">Expo auth token.</param>
        /// <exception cref="InvalidTokenException">Token is null, empty or white space.</exception>
        public PushApiClient(string token) : base(token) { }

        /// <summary>
        /// Client with auth token.
        /// </summary>
        /// <param name="token">Expo auth token.</param>
        /// <param name="httpClient">Custom HttpClient object</param>
        /// <exception cref="InvalidTokenException">Token is null, empty or white space.</exception>
        public PushApiClient(string token, HttpClient httpClient) : base(token, httpClient) { }

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
