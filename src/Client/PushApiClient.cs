using ExpoCommunityNotificationServer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ExpoCommunityNotificationServer.Client
{
    public class PushApiClient
    {
        private const string _expoBackendHost = "https://exp.host";
        private const string _sendPushPath = "/--/api/v2/push/send";
        private const string _getReceiptsPath = "/--/api/v2/push/getReceipts";

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

        // It may either be a single message object or a list of up to 100 message objects
        public async Task<PushTicketResponse> SendPushAsync(params PushTicketRequest[] pushTicketRequest) 
        {
            StringContent requestBody = Serialize(pushTicketRequest);
            PushTicketResponse ticketResponse = await PostAsync<PushTicketResponse>(_sendPushPath, requestBody);
            return ticketResponse;
        }

        // Make sure you are only sending a list of 1000 (or less) ticket ID strings
        public async Task<PushResceiptResponse> GetReceiptsAsync(PushReceiptRequest pushReceiptRequest) 
        {
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
    }
}
