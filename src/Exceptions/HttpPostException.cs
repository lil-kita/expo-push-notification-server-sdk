using System.Net;
using System.Net.Http;

namespace ExpoCommunityNotificationServer.Exceptions
{
    public class HttpPostException : HttpRequestException
    {
        private readonly HttpStatusCode _status;

        public HttpPostException() : base() { }

        public HttpPostException(HttpStatusCode status) : base()
        {
            _status = status;
        }

        public new HttpStatusCode StatusCode() => _status;
    }
}
