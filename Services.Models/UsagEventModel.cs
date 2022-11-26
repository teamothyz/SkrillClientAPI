namespace SkrillClientAPI.Services.Models
{
    public class UsagEventModel
    {
        public string appName = "Web";
        public string deviceId;
        public EventData eventData = new EventData();
        public long sessionId;
        public string ul = "en-US";
        public long userId;
    }

    public class EventData
    {
        public string en = "Identify";
    }
}
