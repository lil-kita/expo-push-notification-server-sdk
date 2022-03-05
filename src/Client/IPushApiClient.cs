using ExpoCommunityNotificationServer.Exceptions;
using ExpoCommunityNotificationServer.Models;
using System.Threading.Tasks;

namespace ExpoCommunityNotificationServer.Client
{
    /// <summary>
    /// Provides an interface for sending push notifications using Expo server
    /// </summary>
    public interface IPushApiClient
    {
        /// <summary>
        /// Set new auth token or replace the old one.
        /// </summary>
        /// <param name="token">Expo auth token.</param>
        /// <exception cref="InvalidTokenException">Token is null, empty or white space.</exception>
        void SetToken(string token);

        /// <summary>
        /// Check is Access Token already set
        /// </summary>
        /// <returns>True if token was set</returns>
        bool IsTokenSet();

        /// <summary>
        /// Send push notification.
        /// It may either be a single message object or an array of up to 100 message objects.
        /// </summary>
        /// <param name="pushTicketRequest">Push notification object or an array of up to 100 objects.</param>
        /// <returns>Response with statuses and other info about sent push notifications.</returns>
        /// <exception cref="InvalidTokenException">Token was not set.</exception>
        /// <exception cref="InvalidRequestException">PushTicketMessages count must be between 1 and 100.</exception>
        /// <exception cref="HttpPostException">HttpRequestException or unsuccessfull status code</exception>
        Task<PushTicketResponse> SendPushAsync(params PushTicketRequest[] pushTicketRequest);

        /// <summary>
        /// Send push notification.
        /// It may either be a single message object or an array of up to 100 message objects.
        /// </summary>
        /// <param name="pushTicketRequest">Push notification object or an array of up to 100 objects.</param>
        /// <param name="isTokenRequired">Is Access Token required. Default value is true.</param>
        /// <returns>Response with statuses and other info about sent push notifications.</returns>
        /// <exception cref="InvalidTokenException">Token was not set.</exception>
        /// <exception cref="InvalidRequestException">PushTicketMessages count must be between 1 and 100.</exception>
        /// <exception cref="HttpPostException">HttpRequestException or unsuccessfull status code</exception>
        Task<PushTicketResponse> SendPushAsync(PushTicketRequest[] pushTicketRequest, bool isTokenRequired = true);

        /// <summary>
        /// Send request to get push notification receipts.
        /// Make sure you are only sending a list of 1000 (or less) ticket ID strings.
        /// </summary>
        /// <param name="pushReceiptRequest">Request that contains list of 1000 (or less) ticket ID strings.</param>
        /// <param name="isTokenRequired">Is Access Token required. Default value is true.</param>
        /// <returns>Response with requested receipts.</returns>
        /// <exception cref="InvalidTokenException">Token was not set.</exception>
        /// <exception cref="InvalidRequestException">PushTicketIds must be between 1 and 1000.</exception>
        /// <exception cref="HttpPostException">HttpRequestException or unsuccessfull status code</exception>
        Task<PushReceiptResponse> GetReceiptsAsync(PushReceiptRequest pushReceiptRequest, bool isTokenRequired = true);
    }
}
