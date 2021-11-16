using Newtonsoft.Json;

namespace ExpoCommunityNotificationServer.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Error
    {
        [JsonProperty(PropertyName = "code")]
        public string ErrorCode { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string ErrorMessage { get; set; }
    }
}
