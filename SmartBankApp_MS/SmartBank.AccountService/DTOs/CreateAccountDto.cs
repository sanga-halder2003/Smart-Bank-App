using System.ComponentModel.DataAnnotations;

namespace SmartBank.AccountService.DTOs
{
    public class CreateAccountDto
    {
        [Required(ErrorMessage = "CustomerId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be greater than 0")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Account Type is required")]
        [StringLength(20, ErrorMessage = "Account Type cannot exceed 20 characters")]
        public string AccountType { get; set; } = string.Empty;
    }
}