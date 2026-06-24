namespace SmartBank.AccountService.Models
{
    public class Account
    {
        public int AccountId { get; set; }

        public int CustomerId { get; set; }

        public string AccountNumber { get; set; } = string.Empty;

        public string AccountType { get; set; } = string.Empty;

        public decimal Balance { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ClosedDate { get; set; }
    }
}