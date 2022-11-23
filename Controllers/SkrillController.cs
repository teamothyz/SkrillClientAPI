using Microsoft.AspNetCore.Mvc;
using SkrillClientAPI.Controllers.Models;
using SkrillClientAPI.Services;
using SkrillClientAPI.Services.Models;
using System.Net;

namespace SkrillClientAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkrillController : Controller
    {
        SkrillAPIService apiService;

        public SkrillController(SkrillAPIService apiService)
        {
            this.apiService = apiService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginWithoutOTP([FromBody] UserModel user)
        {
            await apiService.RegisterSkrillSession();
            await apiService.AuthorizeSkrillSession();
            return await apiService.Login(user, false);
        }

        [HttpPost("login/requestotp")]
        public async Task<ActionResult<string>> LoginWithOTP([FromBody] UserModel user)
        {
            await apiService.RegisterSkrillSession();
            await apiService.AuthorizeSkrillSession();
            apiService.Login(user, true);
            return "wait to send OTP";
        }

        [HttpPost("login/submitotp")]
        public async Task<ActionResult<string>> SubmitOTP([FromBody] string otp)
        {
            return await apiService.SubmitOTPToLogin(otp);
        }

        [HttpPut("request")]
        public async Task<ActionResult<MoneyRequestModel>> CreateMoneyRequest([FromBody] MoneyRequestCreateModel request)
        {
            var result = await apiService.CreateMoneyRequest(request);
            return result;
        }

        [HttpDelete("request/{id}/cancel")]
        public async Task<ActionResult<string>> DeleteMoneyRequest(string id)
        {
            return await apiService.DeleteMoneyRequest(id);
        }

        [HttpGet("request/{key}/isexpired")]
        public async Task<ActionResult<string>> CheckExpiredRequest(string key)
        {
            var result = await apiService.IsExpiredMoneyRequest(key);
            return $"Request is expired: {result}";
        }

        [HttpGet("transaction/{currency}/{fromdate}/{todate}/{page}/{pagesize}")]
        public async Task<ActionResult<string>> GetTransactionHistory(string currency = "USD", string fromdate = "", string todate = "", int page = 1, int pagesize = 100)
        {
            page = page < 1 ? 0 : page;
            pagesize = pagesize < 1 ? 0 : 100;
            return await apiService.CheckMoneyRequest(currency, fromdate, todate, page, pagesize);
        }
    }
}