using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expo.Server.Models
{   
    [JsonObject(MemberSerialization.OptIn)]
    public class PushTicketRequest
    {
        [JsonProperty(PropertyName = "to")]
        public List<string> PushTo { get; set; }

        [JsonProperty(PropertyName = "data")]
        public object PushData { get; set; } // A JSON object delivered to your app

        [JsonProperty(PropertyName = "title")]
        public string PushTitle { get; set; }

        [JsonProperty(PropertyName = "body")]
        public string PushBody { get; set; }

        [JsonProperty(PropertyName = "ttl")]
        public int? PushTTL { get; set; } // Defaults to undefined in order to use the respective defaults of each provider (0 for iOS/APNs and 2419200 (4 weeks) for Android/FCM)

        [JsonProperty(PropertyName = "expiration")]
        public int? PushExpiration { get; set; }

        [JsonProperty(PropertyName = "priority")]  
        public string PushPriority { get; set; } //'default' | 'normal' | 'high'

        [JsonProperty(PropertyName = "subtitle")]
        public string PushSubTitle { get; set; } // iOS only

        [JsonProperty(PropertyName = "sound")] 
        public string PushSound { get; set; } // iOS only : 'default' | null	

        [JsonProperty(PropertyName = "badge")]
        public int? PushBadgeCount { get; set; } // iOS only. Number to display in the badge on the app icon

        [JsonProperty(PropertyName = "channelId")]
        public string PushChannelId { get; set; } // Android only. 

        [JsonProperty(PropertyName = "categoryId")]
        public string PushCategoryId { get; set; }

        [JsonProperty(PropertyName = "mutableContent")]
        public bool PushMutableContent { get; set; } // iOS only : defaults to false
    }
}
