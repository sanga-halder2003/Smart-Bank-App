using System.ComponentModel.DataAnnotations;

namespace SmartBank.NotificationService.DTOs
{
    public class EmailNotificationDto
    {
        [Required, EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Recipient { get; set; }

        [Required, MinLength(5, ErrorMessage = "Subject must be at least 5 characters long"), MaxLength(100, ErrorMessage = "Subject cannot exceed 100 characters")]
        public string Subject { get; set; }

        [Required, MinLength(10, ErrorMessage = "Message must be at least 10 characters long"), MaxLength(500, ErrorMessage = "Message cannot exceed 500 characters")]
        public string Message { get; set; }
    }
}
