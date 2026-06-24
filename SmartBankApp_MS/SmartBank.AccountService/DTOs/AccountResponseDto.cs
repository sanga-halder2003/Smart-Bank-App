namespace SmartBank.AccountService.DTOs
{
    public class AccountResponseDto
    {
        public int AccountId { get; set; }

        public string AccountNumber { get; set; } = string.Empty;

        public string AccountType { get; set; } = string.Empty;

        public decimal Balance { get; set; }

        public bool IsActive { get; set; }
    }
}