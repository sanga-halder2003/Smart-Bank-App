namespace SmartBank.Frontend.Models.Transaction
{
    public class TransactionModel
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public int? DestinationAccountId { get; set; }

        public decimal Amount { get; set; }

        public string Type { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}