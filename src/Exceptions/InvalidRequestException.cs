using System;

namespace ExpoCommunityNotificationServer.Exceptions
{
    [Serializable()]
    public class InvalidRequestException : ArgumentOutOfRangeException
    {
        public InvalidRequestException(string paramName, string message)
            : base(paramName, message) { }
    }
}
