namespace SmartBank.CustomerServie.Messaging
{
    public class UserRegisteredEvent
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }
}