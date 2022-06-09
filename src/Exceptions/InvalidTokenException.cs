using System;

namespace ExpoCommunityNotificationServer.Exceptions
{
    /// <summary>
    /// The exception that is thrown when smth wrong with expo token
    /// </summary>
    [Serializable()]
    public class InvalidTokenException : ArgumentNullException
    {
        public InvalidTokenException(string paramName, string message) : base(paramName, message) { }

        public InvalidTokenException(string message) : base(null, message) { }
    }
}
