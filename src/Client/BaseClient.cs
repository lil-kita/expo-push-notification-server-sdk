using ExpoCommunityNotificationServer.Exceptions;
using ExpoCommunityNotificationServer.Helpers;
using ExpoCommunityNotificationServer.Models;
using Newtonsoft.Json;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ExpoCommunityNotificationServer.Client
{
    public abstract class BaseClient : IPushApiClient, IDisposable
    {
        private const string _expoHost = "https://exp.host";
        private const string _sendPushPath = "/--/api/v2/push/send";
        private const string _getReceiptsPath = "/--/api/v2/push/getReceipts";

        private TimeSpan _retryDelay = TimeSpan.FromMilliseconds(500);
        private int _retryCount = 3;

        private bool _disposed;
        private bool _disposeClient;

        private HttpClient _httpClient;

        protected BaseClient(string token, HttpClient httpClient) : this(httpClient)
        {
            ValidateAndSetToken(token);
        }

        protected BaseClient(string token) : this()
        {
            ValidateAndSetToken(token);
        }

        protected BaseClient(HttpClient httpClient)
        {
            ConfigureClient(httpClient);
        }

        protected BaseClient()
        {
            ConfigureClient();
        }

        public abstract Task<PushTicketResponse> SendPushAsync(params PushTicketRequest[] pushTicketRequest);
        public abstract Task<PushReceiptResponse> GetReceiptsAsync(PushReceiptRequest pushReceiptRequest);
        public abstract void SetToken(string token);

        public bool IsTokenSet() => _httpClient.IsTokenSet();

        protected HttpClient Client => _httpClient;
        protected static string Host => _expoHost;
        protected static string SendPushPath => _sendPushPath;
        protected static string GetReceiptsPath => _getReceiptsPath;

        protected void SetSettings(ExpoSettings settings)
        {
            _retryCount = settings.RetriesNumber;
            _retryDelay = TimeSpan.FromMilliseconds(settings.RetriesDelayMS);

            if (!string.IsNullOrWhiteSpace(settings.ExpoAuthToken))
            {
                SetAuthenticationToken(settings.ExpoAuthToken);
            }
        }

        protected void ValidateAndSetToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidTokenException(nameof(token), "Token is null, empty or white space");
            }

            SetAuthenticationToken(token);
        }

        protected static StringContent Serialize<TRequestModel>(TRequestModel obj) where TRequestModel : class
        {
            string serializedRequestObj = JsonConvert.SerializeObject(obj, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return new StringContent(serializedRequestObj, System.Text.Encoding.UTF8, "application/json");
        }

        protected async Task<TResponseModel> PostAsync<TResponseModel>(string path, StringContent requestBody) where TResponseModel : Response
        {
            TResponseModel responseBody;
            HttpResponseMessage response = default;
            try
            {
                response = await PostAsync(_httpClient.PostAsync(path, requestBody));
                response.EnsureSuccessStatusCode();

                string rawResponseBody = await response.Content.ReadAsStringAsync();
                responseBody = JsonConvert.DeserializeObject<TResponseModel>(rawResponseBody);
            }
            catch (Exception ex)
            {
                throw new HttpPostException(ex, response?.StatusCode);
            }

            return responseBody;
        }

        private async Task<HttpResponseMessage> PostAsync(Task<HttpResponseMessage> postAction)
        {
            IEnumerable<TimeSpan> delay = Backoff.ExponentialBackoff(_retryDelay, _retryCount);
            AsyncRetryPolicy<HttpResponseMessage> retryPolicy = GetRetryPolicy(delay);
            return await retryPolicy.ExecuteAsync(async () => await postAction);
        }

        private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy(IEnumerable<TimeSpan> delay) => HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(response => (int)response.StatusCode == 429)
            .WaitAndRetryAsync(delay);

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

        private void ConfigureClient(HttpClient client = null)
        {
            if (client is null)
            {
                _httpClient = new HttpClient() { BaseAddress = new Uri(Host) };
                _disposeClient = true;
            }
            else
            {
                _httpClient = client;
                _httpClient.BaseAddress = new Uri(Host);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            // dispose managed state (managed objects)
            if (disposing)
            {
                if (_disposeClient)
                {
                    _httpClient?.Dispose();
                }
            }

            // free unmanaged resources (unmanaged objects) and override finalizer
            // set large fields to null

            _disposed = true;
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
