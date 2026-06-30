namespace SmartBank.TransactionService.Messaging
{
    public class MoneyTransferredEvent
    {
        public string FromAccountId { get; set; } = string.Empty;

        public string ToAccountId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}