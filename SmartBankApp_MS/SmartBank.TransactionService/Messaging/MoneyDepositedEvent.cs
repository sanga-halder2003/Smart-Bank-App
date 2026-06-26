namespace SmartBank.TransactionService.Messaging
{
    public class MoneyDepositedEvent
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
