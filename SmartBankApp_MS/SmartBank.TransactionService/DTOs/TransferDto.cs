namespace SmartBank.TransactionService.DTOs
{
    public class TransferDto
    {
        public string FromAccountId { get; set; } = string.Empty;

        public string ToAccountId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}