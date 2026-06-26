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

        public AccountsController(IAccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAccountsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var account = await _service.GetAccountByIdAsync(id);

            if (account == null)
            {
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
            var accounts =
                await _service.GetAccountsByCustomerIdAsync(customerId);

            return Ok(accounts);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.CreateAccountAsync(dto);

            return Ok(new
            {
                Message = "Account Created Successfully"
            });
        }

        [HttpPut("close/{id}")]
        public async Task<IActionResult> CloseAccount(int id)
        {
            var result = await _service.CloseAccountAsync(id);

            if (!result)
            {
                return NotFound(new
                {
                    Message = "Account not found"
                });
            }

            return Ok(new
            {
                Message = "Account Closed Successfully"
            });
        }
    }
}