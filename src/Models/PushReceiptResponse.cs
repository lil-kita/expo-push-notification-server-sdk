using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ExpoCommunityNotificationServer.Models
{
    /// <summary>
    /// Class represents response model for getting push receipts
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PushReceiptResponse : Response
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

    [Obsolete("Use PushReceiptResponse class instead")]
    [JsonObject(MemberSerialization.OptIn)]
    public class PushResceiptResponse : PushReceiptResponse { }
}
