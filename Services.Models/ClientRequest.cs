using Newtonsoft.Json;
using SkrillClientAPI.Controllers.Models;
using System.Net;

namespace SkrillClientAPI.Services.Models
{
    public class ClientRequest
    {
        public HttpClient Client { get; set; }
        public CookieContainer Cookies { get; set; }
        public HttpClientHandler Handler { get; set; }
        public readonly Uri Uri = new("https://account.skrill.com/");
        public string EventId;
        public string ClientEventId;
        public string APIKey;
        public string SiteKey;
        public string CaptchaResolvedCode;
        public UserModel User;
        public Guid SessionKey;
        public Guid InstanceId;

        public ClientRequest()
        {
            Cookies = new CookieContainer();
            Handler = new HttpClientHandler();
            Handler.CookieContainer = Cookies;
            Client = new HttpClient(Handler);
            SessionKey = Guid.NewGuid();
            InstanceId = Guid.NewGuid();
            using (var reader = new StreamReader("appsettings.json"))
            {
                string json = reader.ReadToEnd();
                dynamic? item = JsonConvert.DeserializeObject(json);
                if (item != null)
                {
                    APIKey = item.captcha.apikey;
                    SiteKey = item.captcha.sitekey;
                }
            }
        }

        public void SetHeader()
        {
            var cookies = Cookies.GetCookies(Uri);
            var cookiesToString = cookies.Select(item => $"{item.Name}={item.Value}").ToList();
            var token = cookies.FirstOrDefault(item => item.Name.ToUpper().Equals("XSRF-TOKEN"))?.Value;

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("x-xsrf-token", token);
            Client.DefaultRequestHeaders.Add("x-tmx-session-id", SessionKey.ToString());
            Client.DefaultRequestHeaders.Add("cookie", string.Join(";", cookiesToString));
            Client.DefaultRequestHeaders.Add("clientRequest-instance-id", InstanceId.ToString());
            Client.DefaultRequestHeaders.Add("clientRequest-app-version", "Web-7.2.47");
        }
    }
}