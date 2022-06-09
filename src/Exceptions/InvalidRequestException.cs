using System;

namespace ExpoCommunityNotificationServer.Exceptions
{
    /// <summary>
    /// The exception that is thrown when request model is out of range
    /// </summary>
    [Serializable()]
    public class InvalidRequestException : ArgumentOutOfRangeException
    {
        public InvalidRequestException(string paramName, string message) : base(paramName, message) { }
    }
}
