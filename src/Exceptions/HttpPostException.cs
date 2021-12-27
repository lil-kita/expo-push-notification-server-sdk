using System;
using System.Net;
using System.Net.Http;

namespace ExpoCommunityNotificationServer.Exceptions
{
    /// <summary>
    /// The exception that is thrown when exception is thrown by HttpClient or StatusCode is not successfull
    /// </summary>
    [Serializable()]
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
