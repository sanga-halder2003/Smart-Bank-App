namespace SmartBank.TransactionService.DTOs
{
    public class DepositDto
    {
        public string AccountId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
