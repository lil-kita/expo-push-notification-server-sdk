using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExpoCommunityNotificationServer.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PushResceiptResponse : Response
    {
        [JsonProperty(PropertyName = "data")]
        public Dictionary<string, PushTicketDeliveryStatus> PushTicketReceipts { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class PushTicketDeliveryStatus : Status
    {
        [JsonProperty(PropertyName = "status")]
        public string DeliveryStatus { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string DeliveryMessage { get; set; }
    }
}
