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

        /// <summary>
        /// Using for the first time login to request OTP
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("login/requestotp")]
        public async Task<ActionResult<string>> RequestOTP([FromBody] UserModel user)
        {
            bool isNewSession = true;
            await apiService.AuthorizeSkrillSession(isNewSession);
            await apiService.RegisterSkrillSession();
            await apiService.AuthorizeSkrillSession();
            await apiService.Login(user);
            return await apiService.RequestOTP();
        }

        /// <summary>
        /// Send OTP to login
        /// </summary>
        /// <param name="otp"></param>
        /// <returns></returns>
        [HttpPost("login/submitotp")]
        public async Task<ActionResult<string>> SubmitOTP([FromBody] string otp)
        {
            return await apiService.SubmitOTPToLogin(otp);
        }

        /// <summary>
        /// Create a new money request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("request")]
        public async Task<ActionResult<MoneyRequestModel>> CreateMoneyRequest([FromBody] MoneyRequestCreateModel request)
        {
            return await apiService.CreateMoneyRequest(request);
        }

        /// <summary>
        /// Cancel a money request with given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("request/{id}/cancel")]
        public async Task<ActionResult<string>> DeleteMoneyRequest(string id)
        {
            return await apiService.DeleteMoneyRequest(id);
        }

        /// <summary>
        /// Checking the state of a money request
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("request/{key}/isexpired")]
        public async Task<ActionResult<string>> CheckExpiredRequest(string key)
        {
            var result = await apiService.IsExpiredMoneyRequest(key);
            return $"{{\"isexpired\": {result}}}";
        }

        /// <summary>
        /// Retrive the information of transactions history
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpGet("transactions")]
        public async Task<ActionResult<string>> GetTransactionHistory(string currency = "USD", string fromdate = "", string todate = "", int page = 1, int pagesize = 100)
        {
            page = page < 1 ? 0 : page;
            pagesize = pagesize < 1 ? 0 : 100;
            return await apiService.CheckMoneyRequest(currency, fromdate, todate, page, pagesize);
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public async Task<ActionResult<string>> Logout()
        {
            return await apiService.Logout();
        }
    }
}