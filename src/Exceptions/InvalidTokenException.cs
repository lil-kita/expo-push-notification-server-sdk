using System;

namespace ExpoCommunityNotificationServer.Exceptions
{
    [Serializable()]
    public class InvalidTokenException : ArgumentNullException
    {
        public InvalidTokenException(string paramName, string message)
            : base(paramName, message) { }

        public InvalidTokenException(string message)
            : base(null, message) { }
    }
}
