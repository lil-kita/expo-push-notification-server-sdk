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
}
