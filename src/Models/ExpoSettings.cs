namespace ExpoCommunityNotificationServer.Models
{
    public class ExpoSettings
    {
        public string ExpoAuthToken { get; set; } = null;

        public int RetriesNumber { get; set; } = 3;

        public int RetriesDelayMS { get; set; } = 500;
    }
}
