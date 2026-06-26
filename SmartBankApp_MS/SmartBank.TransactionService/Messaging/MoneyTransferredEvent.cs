namespace SmartBank.TransactionService.Messaging
{
    public class MoneyTransferredEvent
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}