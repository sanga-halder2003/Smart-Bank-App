namespace SmartBank.Frontend.Models.Transaction
{
    public class WithdrawDto
    {
        public int AccountId { get; set; }

        public decimal Amount { get; set; }
    }
}