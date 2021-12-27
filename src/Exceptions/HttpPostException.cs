using System.Net;
using System.Net.Http;

namespace ExpoCommunityNotificationServer.Exceptions
{
    internal class HttpPostException : HttpRequestException
    {
        private readonly HttpStatusCode _status;

        public HttpPostException(HttpStatusCode status) : base()
        {
            _status = status;
        }

        public new HttpStatusCode StatusCode() => _status;
    }
}
