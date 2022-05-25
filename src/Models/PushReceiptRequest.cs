using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExpoCommunityNotificationServer.Models
{
    /// <summary>
    /// Class represents request model for getting push receipts
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PushReceiptRequest
    {
        [JsonProperty(PropertyName = "ids")]
        public List<string> PushTicketIds { get; set; }
    }
}
