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
    public abstract class BaseClient : IPushApiClient
    {
        private const string _expoBackendHost = "https://exp.host";
        private const string _sendPushPath = "/--/api/v2/push/send";
        private const string _getReceiptsPath = "/--/api/v2/push/getReceipts";

        private readonly HttpClientHandler _httpHandler;
        private readonly HttpClient _httpClient;

        protected BaseClient(string token)
        {
            _httpHandler = new HttpClientHandler() { MaxConnectionsPerServer = 6 };
            _httpClient = new HttpClient(_httpHandler)
            {
                BaseAddress = new Uri(_expoBackendHost)
            };
            SetToken(token);
        }

        public abstract Task<PushTicketResponse> SendPushAsync(params PushTicketRequest[] pushTicketRequest);

        public abstract Task<PushReceiptResponse> GetReceiptsAsync(PushReceiptRequest pushReceiptRequest);

        /// <summary>
        /// Set new auth token or replace the old one.
        /// </summary>
        /// <param name="token">Expo auth token.</param>
        /// <exception cref="InvalidTokenException">Token is null, empty or white space.</exception>
        public void SetToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidTokenException(nameof(token), "Token is null, empty or white space");
            }

            SetAuthenticationToken(token);
        }

        protected static string SendPushPath => _sendPushPath;

        protected static string GetReceiptsPath => _getReceiptsPath;

        protected static StringContent Serialize<TRequestModel>(TRequestModel obj) where TRequestModel : class
        {
            string serializedRequestObj = JsonConvert.SerializeObject(obj, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return new StringContent(serializedRequestObj, System.Text.Encoding.UTF8, "application/json");
        }

        protected async Task<TResponseModel> PostAsync<TResponseModel>(string path, StringContent requestBody) where TResponseModel : class
        {
            TResponseModel responseBody;
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync(path, requestBody);
            }
            catch
            {
                throw new HttpPostException();
            }

            if (response.IsSuccessStatusCode)
            {
                string rawResponseBody = await response.Content.ReadAsStringAsync();
                responseBody = JsonConvert.DeserializeObject<TResponseModel>(rawResponseBody);
            }
            else
            {
                throw new HttpPostException(response.StatusCode);
            }

            return responseBody;
        }

        protected void Validate(PushReceiptRequest receiptRequest)
        {
            Validate();

            if (!receiptRequest.IsReceiptRequestInValidRange())
            {
                throw new InvalidRequestException(typeof(PushReceiptRequest).Name, "PushTicketIds count should be >0 and <=1000");
            }
        }

        protected void Validate(PushTicketRequest[] ticketRequest)
        {
            Validate();
;
            if (!ticketRequest.IsPushMessagesInValidRange())
            {
                throw new InvalidRequestException(typeof(PushTicketRequest).Name, "PushTicketMessages count should be >0 and <=100");
            }
        }

        private void Validate()
        {
            if (!_httpClient.IsTokenSet())
            {
                throw new InvalidTokenException("Token was not set");
            }
        }

        private void SetAuthenticationToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
