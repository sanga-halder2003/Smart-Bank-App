namespace SmartBank.TransactionService.DTOs
{
    public class WithdrawDto
    {
        public string AccountId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}