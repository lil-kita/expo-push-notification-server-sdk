using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExpoCommunityNotificationServer.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PushReceiptRequest
    {
        [JsonProperty(PropertyName = "ids")]
        public List<string> PushTicketIds { get; set; }
    }
}
