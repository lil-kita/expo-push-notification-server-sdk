using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExpoCommunityNotificationServer.Models
{
    /// <summary>
    /// Class represents response model for sending push notifications
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PushTicketResponse : Response
    {
        [JsonProperty(PropertyName = "data")]
        public List<PushTicketStatus> PushTicketStatuses { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class PushTicketStatus : Status
    {
        /// <summary>
        /// "error" | "ok"
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string TicketStatus { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string TicketId { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string TicketMessage { get; set; }
    }
}