namespace SmartBank.AccountService.Messaging
{
    public class AccountCreatedEvent
    {
        public int CustomerId { get; set; }

        public string AccountNumber { get; set; } = string.Empty;

        public string AccountType { get; set; } = string.Empty;
    }
}