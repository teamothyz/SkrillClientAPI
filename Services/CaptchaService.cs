using SkrillClientAPI.Services.Models;

namespace SkrillClientAPI.Services
{
    public class CaptchaService
    {
        public static async Task<string> ResolveCaptcha(ClientRequest client)
        {
            string code = string.Empty;
            var passCaptcha = false;
            while (!passCaptcha)
            {
                var id = await GetCaptchaResolveId(client);
                while (true)
                {
                    await Task.Delay(5000);
                    string codeRaw = await GetCaptchaResolvedCode(client, id);

                    if (codeRaw.Equals("ERROR_CAPTCHA_UNSOLVABLE"))
                        break;

                    if (codeRaw.Equals("CAPCHA_NOT_READY"))
                        continue;

                    if (codeRaw.ToLower().Contains("ok"))
                    {
                        code = codeRaw.Split('|')[1];
                        passCaptcha = true;
                        break;
                    }
                    else
                        throw new Exception($"Error when resolving captcha. Error: {codeRaw}");
                }
            }
            return code;
        }

        public static async Task<string> GetCaptchaResolveId(ClientRequest client)
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    { "key", client.APIKey },
                    { "method", "userrecaptcha" },
                    { "googlekey", client.SiteKey },
                    { "pageurl", "https://account.skrill.com/wallet/account/login" }
                };
                var content = new FormUrlEncodedContent(values);
                var url = "https://2captcha.com/in.php";
                var response = await client.Client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();
                return responseString.Split('|')[1];
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static async Task<string> GetCaptchaResolvedCode(ClientRequest client, string id)
        {
            try
            {
                var url = "https://2captcha.com/res.php";
                var query = $"key={client.APIKey}&action=get&id={id}";
                var response = await client.Client.GetAsync($"{url}?{query}");
                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
