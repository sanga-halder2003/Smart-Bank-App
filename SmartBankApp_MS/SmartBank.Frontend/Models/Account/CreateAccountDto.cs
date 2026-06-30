namespace SmartBank.Frontend.Models.Account
{
    public class CreateAccountDto
    {
        public int CustomerId { get; set; }

        public string AccountType { get; set; } = string.Empty;
    }
}