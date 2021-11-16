using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExpoCommunityNotificationServer.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PushTicketResponse
    {
        [JsonProperty(PropertyName = "data")]
        public List<PushTicketStatus> PushTicketStatuses { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public List<Error> PushTicketErrors { get; set; }

    }

    [JsonObject(MemberSerialization.OptIn)]
    public class PushTicketStatus
    {
        [JsonProperty(PropertyName = "status")] //"error" | "ok",
        public string TicketStatus { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string TicketId { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string TicketMessage { get; set; }

        [JsonProperty(PropertyName = "details")]
        public object TicketDetails { get; set; }
    }
}