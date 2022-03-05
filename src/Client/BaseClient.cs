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

        protected BaseClient()
        {
            _httpHandler = new HttpClientHandler() { MaxConnectionsPerServer = 6 };
            _httpClient = new HttpClient(_httpHandler)
            {
                BaseAddress = new Uri(_expoBackendHost)
            };
        }

        protected BaseClient(string token)
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

        public abstract Task<PushTicketResponse> SendPushAsync(params PushTicketRequest[] pushTicketRequest);

        public abstract Task<PushTicketResponse> SendPushAsync(PushTicketRequest[] pushTicketRequest, bool isTokenRequired = true);

        public abstract Task<PushReceiptResponse> GetReceiptsAsync(PushReceiptRequest pushReceiptRequest, bool isTokenRequired = true);

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

        /// <summary>
        /// Check is Access Token already set
        /// </summary>
        /// <returns>True if token was set</returns>
        public bool IsTokenSet() => _httpClient.IsTokenSet();

        protected static string SendPushPath() => _sendPushPath;

        protected static string GetReceiptsPath() => _getReceiptsPath;

        protected static StringContent Serialize<TRequestModel>(TRequestModel obj) where TRequestModel : class
        {
            string serializedRequestObj = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            return new StringContent(serializedRequestObj, System.Text.Encoding.UTF8, "application/json");
        }

        protected async Task<TResponseModel> PostAsync<TResponseModel>(string path, StringContent requestBody) where TResponseModel : class
        {
            TResponseModel responseBody = default;
            HttpResponseMessage response = default;
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

        protected void Validate<T>(T model, bool isTokenRequired = true)
        {
            if (isTokenRequired && !_httpClient.IsTokenSet())
            {
                throw new InvalidTokenException("Token was not set");
            }

            bool isValid = true;
            string error = string.Empty;
            if (model is PushReceiptRequest receiptRequest)
            {
                isValid = receiptRequest.IsReceiptRequestInValidRange();
                error = "PushTicketIds count should be >0 and <=1000";
            }
            else if (model is PushTicketRequest[] ticketRequest)
            {
                isValid = ticketRequest.IsPushMessagesInValidRange();
                error = "PushTicketMessages count should be >0 and <=100";
            }

            if (!isValid)
            {
                throw new InvalidRequestException(typeof(T).Name, error);
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
