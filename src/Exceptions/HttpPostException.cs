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
#if NET6_0_OR_GREATER
        public HttpPostException(Exception ex, HttpStatusCode? status = null) 
            : base("Exception on request", ex, status) { }

#else
        private readonly HttpStatusCode? _status;

        public HttpPostException(Exception ex, HttpStatusCode? status = null): base("Exception on request", ex) 
        {
            _status = status;
        }

        public HttpStatusCode? StatusCode => _status;
#endif
    }
}
