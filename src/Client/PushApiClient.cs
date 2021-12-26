using ExpoCommunityNotificationServer.Exceptions;
using ExpoCommunityNotificationServer.Helpers;
using ExpoCommunityNotificationServer.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ExpoCommunityNotificationServer.Client
{
    public class PushApiClient : IPushApiClient
    {
        private const string _expoBackendHost = "https://exp.host";
        private const string _sendPushPath = "/--/api/v2/push/send";
        private const string _getReceiptsPath = "/--/api/v2/push/getReceipts";

        private readonly HttpClientHandler _httpHandler;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Create client without auth token
        /// Make sure to set token before using before sending push notification or getting reciepts
        /// </summary>
        public PushApiClient()
        {
            _httpHandler = new HttpClientHandler() { MaxConnectionsPerServer = 6 };
            _httpClient = new HttpClient(_httpHandler)
            {
                BaseAddress = new Uri(_expoBackendHost)
            };
        }

        /// <summary>
        /// Create client with auth token
        /// </summary>
        /// <param name="token">Expo auth token</param>
        /// <exception cref="InvalidTokenException">Token is null, empty or white space</exception>
        public PushApiClient(string token)
        {
            _httpHandler = new HttpClientHandler() { MaxConnectionsPerServer = 6 };
            _httpClient = new HttpClient(_httpHandler)
            {
                BaseAddress = new Uri(_expoBackendHost)
            };
            try
            {
                SetToken(token);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Set new auth token or replace old one
        /// </summary>
        /// <param name="token">Expo auth token</param>
        /// <exception cref="InvalidTokenException">Token is null, empty or white space</exception>
        public void SetToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidTokenException(nameof(token), "Token is null, empty or white space");
            }

            SetAuthenticationToken(token);
        }

        /// <summary>
        /// Send push notification
        /// It may either be a single message object or an array of up to 100 message objects
        /// </summary>
        /// <param name="pushTicketRequest">Push notification object or an array of up to 100 objects</param>
        /// <returns>Response with statuses and other info about sent push notifications</returns>
        /// <exception cref="InvalidTokenException">Token was not set</exception>
        /// <exception cref="InvalidRequestException">PushTicketMessages count should be >0 and <=100</exception>
        public async Task<PushTicketResponse> SendPushAsync(params PushTicketRequest[] pushTicketRequest)
        {
            if (!_httpClient.IsTokenSet())
            {
                throw new InvalidTokenException("Token was not set");
            }
            if (!pushTicketRequest.IsPushMessagesInValidRange())
            {
                throw new InvalidRequestException(nameof(pushTicketRequest), "PushTicketMessages count should be >0 and <=100");
            }

            StringContent requestBody = Serialize(pushTicketRequest);
            PushTicketResponse ticketResponse = await PostAsync<PushTicketResponse>(_sendPushPath, requestBody);
            return ticketResponse;
        }


        /// <summary>
        /// Send request to get push notification receipts
        /// Make sure you are only sending a list of 1000 (or less) ticket ID strings
        /// </summary>
        /// <param name="pushReceiptRequest">Request that contains list of 1000 (or less) ticket ID strings</param>
        /// <returns>Response with requested receipts</returns>
        /// <exception cref="InvalidTokenException">Token was not set</exception>
        /// <exception cref="InvalidRequestException">PushTicketIds count should be >0 and <=1000</exception>
        public async Task<PushResceiptResponse> GetReceiptsAsync(PushReceiptRequest pushReceiptRequest)
        {
            if (!_httpClient.IsTokenSet())
            {
                throw new InvalidTokenException("Token was not set");
            }
            if (!pushReceiptRequest.IsReceiptRequestInValidRange())
            {
                throw new InvalidRequestException(nameof(pushReceiptRequest), "PushTicketIds count should be >0 and <=1000");
            }

            StringContent requestBody = Serialize(pushReceiptRequest);
            PushResceiptResponse receiptResponse = await PostAsync<PushResceiptResponse>(_getReceiptsPath, requestBody);
            return receiptResponse;
        }

        private static StringContent Serialize<TRequestModel>(TRequestModel obj) where TRequestModel : class
        {
            string serializedRequestObj = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            return new StringContent(serializedRequestObj, System.Text.Encoding.UTF8, "application/json");
        }

        private async Task<TResponseModel> PostAsync<TResponseModel>(string path, StringContent requestBody) where TResponseModel : class
        {
            TResponseModel responseBody = default;
            HttpResponseMessage response = await _httpClient.PostAsync(path, requestBody);
            if (response.IsSuccessStatusCode)
            {
                string rawResponseBody = await response.Content.ReadAsStringAsync();
                responseBody = JsonConvert.DeserializeObject<TResponseModel>(rawResponseBody);
            }
            return responseBody;
        }

        private void SetAuthenticationToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
