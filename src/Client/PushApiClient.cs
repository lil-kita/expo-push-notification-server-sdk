using Expo.Server.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Expo.Server.Client
{
    public class PushApiClient
    {
        private const string _expoBackendHost = "https://exp.host";
        private const string _pushSendPath = "/--/api/v2/push/send";
        private const string _pushGetReceiptsPath = "/--/api/v2/push/getReceipts";

        private readonly HttpClientHandler _httpHandler;
        private readonly HttpClient _httpClient;

        public PushApiClient(string token)
        {
            _httpHandler = new HttpClientHandler() { MaxConnectionsPerServer = 6 };
            _httpClient = new HttpClient(_httpHandler)
            {
                BaseAddress = new Uri(_expoBackendHost)
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<PushTicketResponse> PushSendAsync(List<PushTicketRequest> pushTicketRequest)
        {
            PushTicketResponse ticketResponse = await PostAsync<PushTicketRequest, PushTicketResponse>(_pushSendPath, pushTicketRequest);
            return ticketResponse;
        }

        public async Task<PushResceiptResponse> PushGetReceiptsAsync(PushReceiptRequest pushReceiptRequest)
        {
            PushResceiptResponse receiptResponse = await PostAsync<PushReceiptRequest, PushResceiptResponse>(_pushGetReceiptsPath, pushReceiptRequest);
            return receiptResponse;
        }

        public async Task<U> PostAsync<T, U>(string path, List<T> requestObj) where T : class
        {
            string serializedRequestObj = JsonConvert.SerializeObject(requestObj, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            StringContent requestBody = new StringContent(serializedRequestObj, System.Text.Encoding.UTF8, "application/json");
            U responseBody = default;
            HttpResponseMessage response = await _httpClient.PostAsync(path, requestBody);

            if (response.IsSuccessStatusCode)
            {
                string rawResponseBody = await response.Content.ReadAsStringAsync();
                responseBody = JsonConvert.DeserializeObject<U>(rawResponseBody);
            }

            return responseBody;
        }

        public async Task<U> PostAsync<T, U>(string path, T requestObj) where T : class
        {
            string serializedRequestObj = JsonConvert.SerializeObject(requestObj, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            StringContent requestBody = new StringContent(serializedRequestObj, System.Text.Encoding.UTF8, "application/json");
            U responseBody = default;
            HttpResponseMessage response = await _httpClient.PostAsync(path, requestBody);

            if (response.IsSuccessStatusCode)
            {
                string rawResponseBody = await response.Content.ReadAsStringAsync();
                responseBody = JsonConvert.DeserializeObject<U>(rawResponseBody);
            }

            return responseBody;
        }
    }
}
