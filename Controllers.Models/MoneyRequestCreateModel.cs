namespace SkrillClientAPI.Controllers.Models
{
    public class MoneyRequestCreateModel
    {
        public double amount { get; set; }
        public string currency { get; set; }
        public string message { get; set; }
    }
}