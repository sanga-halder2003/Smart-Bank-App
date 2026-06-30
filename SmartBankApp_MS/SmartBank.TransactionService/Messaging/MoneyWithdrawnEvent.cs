namespace SmartBank.TransactionService.Messaging
{
    public class MoneyWithdrawnEvent
    {
        public string AccountId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}