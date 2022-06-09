using System.Net.Http;
using System.Net.Http.Headers;

namespace ExpoCommunityNotificationServer.Helpers
{
    internal static class HttpClientHelper
    {
        public static bool IsTokenSet(this HttpClient httpClient) =>
            httpClient != null && httpClient.DefaultRequestHeaders.Authorization.IsTokenSet();

        public static bool IsTokenSet(this AuthenticationHeaderValue authHeader) =>
            authHeader != null && authHeader.Scheme.Equals("Bearer") && !string.IsNullOrWhiteSpace(authHeader.Parameter);
    }
}
