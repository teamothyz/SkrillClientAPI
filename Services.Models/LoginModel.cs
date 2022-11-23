namespace SkrillClientAPI.Services.Models
{
    public class LoginModel
    {
        public CaptchaValidationRequest captchaValidationRequest;
        public string customCaptcha = "";
        public string password;
        public string username;
    }

    public class LoginWithOTPModel : LoginModel
    {
        public string scaClientEventId;
        public string scaEventId;
    }

    public class CaptchaValidationRequest
    {
        public string response;
        public string siteKey;
        public string type = "invisible";
    }
}
