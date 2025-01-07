using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ExpoCommunityNotificationServer.Models
{
    /// <summary>
    /// Class represents request model for sending push notifications
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PushTicketRequest
    {
        /// <summary>
        /// Recipient list
        /// </summary>
        [JsonProperty(PropertyName = "to")]
        public List<string> PushTo { get; set; }

        /// <summary>
        /// iOS only. Cause the app to start in the background to run a background task.
        /// </summary>
        [JsonProperty(PropertyName = "_contentAvailable")]
        public bool? PushContentAvailable { get; set; }

        /// <summary>
        /// A JSON object delivered to your app
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public object PushData { get; set; }

        /// <summary>
        /// Push message title
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string PushTitle { get; set; }

        /// <summary>
        /// Push message text
        /// </summary>
        [JsonProperty(PropertyName = "body")]
        public string PushBody { get; set; }

        /// <summary>
        /// Defaults to undefined in order to use the respective defaults of each provider (0 for iOS/APNs and 2419200 (4 weeks) for Android/FCM)
        /// </summary>
        [JsonProperty(PropertyName = "ttl")]
        public int? PushTTL { get; set; }

        [Obsolete("Use PushTTL instead")]
        [JsonProperty(PropertyName = "expiration")]
        public int? PushExpiration { get; set; }

        /// <summary>
        /// 'default' | 'normal' | 'high'
        /// </summary>
        [JsonProperty(PropertyName = "priority")]
        public string PushPriority { get; set; }

        /// <summary>
        /// iOS only
        /// </summary>
        [JsonProperty(PropertyName = "subtitle")]
        public string PushSubTitle { get; set; }

        /// <summary>
        /// iOS only : 'default' | null
        /// </summary>
        [JsonProperty(PropertyName = "sound")]
        public string PushSound { get; set; }

        /// <summary>
        /// iOS only. Number to display in the badge on the app icon
        /// </summary>
        [JsonProperty(PropertyName = "badge")]
        public int? PushBadgeCount { get; set; }

        /// <summary>
        /// iOS only : 'active' | 'critical' | 'passive' | 'time-sensitive'
        /// </summary>
        [JsonProperty(PropertyName = "interruptionLevel")]
        public string PushInterruptionLevel { get; set; }

        /// <summary>
        /// Android only. Id of chanel that will receive the message.
        /// </summary>
        [JsonProperty(PropertyName = "channelId")]
        public string PushChannelId { get; set; }

        /// <summary>
        /// Push message receiver category
        /// </summary>
        [JsonProperty(PropertyName = "categoryId")]
        public string PushCategoryId { get; set; }

        /// <summary>
        /// iOS only : defaults to false
        /// </summary>
        [JsonProperty(PropertyName = "mutableContent")]
        public bool PushMutableContent { get; set; }
    }
}
