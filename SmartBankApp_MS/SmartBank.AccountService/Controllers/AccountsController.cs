using Microsoft.AspNetCore.Mvc;
using SmartBank.AccountService.DTOs;
using SmartBank.AccountService.Interfaces;

namespace SmartBank.AccountService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _service;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(
            IAccountService service,
            ILogger<AccountsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all accounts.");

            return Ok(await _service.GetAllAccountsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching account with Id: {Id}", id);

            var account = await _service.GetAccountByIdAsync(id);

            if (account == null)
            {
                _logger.LogWarning("Account not found. Id: {Id}", id);

                return NotFound(new
                {
                    Message = "Account not found"
                });
            }

            return Ok(account);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(int customerId)
        {
            _logger.LogInformation("Fetching accounts for CustomerId: {CustomerId}", customerId);

            var accounts =
                await _service.GetAccountsByCustomerIdAsync(customerId);

            return Ok(accounts);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountDto dto)
        {
            _logger.LogInformation("Create Account request received.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model received while creating account.");

                return BadRequest(ModelState);
            }

            await _service.CreateAccountAsync(dto);

            _logger.LogInformation("Account created successfully.");

            return Ok(new
            {
                Message = "Account Created Successfully"
            });
        }

        [HttpPut("close/{id}")]
        public async Task<IActionResult> CloseAccount(int id)
        {
            _logger.LogInformation("Closing account with Id: {Id}", id);

            var result = await _service.CloseAccountAsync(id);

            if (!result)
            {
                _logger.LogWarning("Account not found while closing. Id: {Id}", id);

                return NotFound(new
                {
                    Message = "Account not found"
                });
            }

            _logger.LogInformation("Account closed successfully. Id: {Id}", id);

            return Ok(new
            {
                Message = "Account Closed Successfully"
            });
        }
    }
}