using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExpoCommunityNotificationServer.Models
{   
    public class Response
    {
        [JsonProperty(PropertyName = "errors")]
        public List<Error> Errors { get; set; }
    }

    public class Status
    {
        [JsonProperty(PropertyName = "details")]
        public PushTicketDetails Details { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Error
    {
        [JsonProperty(PropertyName = "code")]
        public string ErrorCode { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string ErrorMessage { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class PushTicketDetails
    {
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "fault")]
        public string Fault { get; set; }
    }

    public static class ResponseStatusTypes
    {
        public const string Ok = "ok";

        public const string Error = "error";
    }

    public static class ResponseErrorTypes
    {
        public const string DeviceNotRegistered = "DeviceNotRegistered";

        public const string MessageTooBig = "MessageTooBig";

        public const string MessageRateExceeded = "MessageRateExceeded";

        public const string MismatchSenderId = "MismatchSenderId";

        public const string InvalidCredentials = "InvalidCredentials";

        public const string InvalidProviderToken = "InvalidProviderToken";

    }

    public static class RequestErrorTypes
    {
        public const string PUSH_TOO_MANY_EXPERIENCE_IDS = "PUSH_TOO_MANY_EXPERIENCE_IDS";

        public const string PUSH_TOO_MANY_NOTIFICATIONS = "PUSH_TOO_MANY_NOTIFICATIONS";

        public const string PUSH_TOO_MANY_RECEIPTS = "PUSH_TOO_MANY_RECEIPTS";
    }
}
