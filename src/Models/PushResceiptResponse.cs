using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExpoCommunityNotificationServer.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PushResceiptResponse
    {
        [JsonProperty(PropertyName = "data")]
        public Dictionary<string, PushTicketDeliveryStatus> PushTicketReceipts { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public List<Error> ErrorInformations { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class PushTicketDeliveryStatus
    {
        [JsonProperty(PropertyName = "status")]
        public string DeliveryStatus { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string DeliveryMessage { get; set; }

        [JsonProperty(PropertyName = "details")]
        public object DeliveryDetails { get; set; }
    }
}
