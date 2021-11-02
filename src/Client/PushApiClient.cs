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

        public async Task<PushTicketResponse> PushSendAsync(List<PushTicketRequest> pushTicketRequest) // It may either be a single message object or a list of up to 100 message objects
        {
            StringContent requestBody = Serialize(pushTicketRequest);
            PushTicketResponse ticketResponse = await SendRequest<PushTicketResponse>(_pushSendPath, requestBody);
            return ticketResponse;
        }

        public async Task<PushResceiptResponse> PushGetReceiptsAsync(PushReceiptRequest pushReceiptRequest) // Make sure you are only sending a list of 1000 (or less) ticket ID strings
        {
            StringContent requestBody = Serialize(pushReceiptRequest);
            PushResceiptResponse receiptResponse = await SendRequest<PushResceiptResponse>(_pushGetReceiptsPath, requestBody);
            return receiptResponse;
        }

        private StringContent Serialize<T>(T obj) where T: class
        {
            string serializedRequestObj = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            return new StringContent(serializedRequestObj, System.Text.Encoding.UTF8, "application/json");
        }

        private async Task<U> SendRequest<U>(string path, StringContent requestBody)
        {
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
