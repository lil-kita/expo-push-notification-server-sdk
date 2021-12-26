using System;

namespace ExpoCommunityNotificationServer.Exceptions
{
    internal class InvalidRequestException : ArgumentOutOfRangeException
    {
        public InvalidRequestException(string paramName, string message)
            : base(paramName, message) { }
    }
}
