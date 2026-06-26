using Microsoft.AspNetCore.Mvc;
using SmartBank.NotificationService.DTOs;
using SmartBank.NotificationService.Services;

namespace SmartBank.NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IMessageService _messageService;

        public NotificationController(IEmailService emailService, IMessageService messageService)
        {
            _emailService = emailService;
            _messageService = messageService;
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromBody] EmailNotificationDto notification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _emailService.SendEmailAsync(notification);
            if (!result)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        Message = "Failed to send email."
                    });
            }
            return Ok(new
            {
                Message = "Email sent successfully."
            });
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage([FromBody] SMSNotificationDto notification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _messageService.SendMessageAsync(notification);
            if (!result)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        Message = "Failed to send message."
                    });
            }
            return Ok(new
            {
                Message = "Message sent successfully."
            });
        }
    }
}
