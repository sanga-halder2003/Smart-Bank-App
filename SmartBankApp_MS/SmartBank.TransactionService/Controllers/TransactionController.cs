using Microsoft.AspNetCore.Mvc;
using SmartBank.TransactionService.DTOs;
using SmartBank.TransactionService.Services;

namespace SmartBank.TransactionService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionManager _service;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(
            TransactionManager service,
            ILogger<TransactionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(DepositDto dto)
        {
            _logger.LogInformation("Deposit request received for AccountId {AccountId}", dto.AccountId);

            await _service.Deposit(dto);

            _logger.LogInformation("Deposit completed successfully for AccountId {AccountId}", dto.AccountId);

            return Ok("Deposit Successful");
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw(WithdrawDto dto)
        {
            _logger.LogInformation("Withdraw request received for AccountId {AccountId}", dto.AccountId);

            await _service.Withdraw(dto);

            _logger.LogInformation("Withdraw completed successfully for AccountId {AccountId}", dto.AccountId);

            return Ok("Withdraw Successful");
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(TransferDto dto)
        {
            _logger.LogInformation(
                "Transfer request received from Account {FromAccountId} to Account {ToAccountId} Amount {Amount}",
                dto.FromAccountId,
                dto.ToAccountId,
                dto.Amount);

            await _service.Transfer(dto);

            _logger.LogInformation(
                "Transfer completed successfully from Account {FromAccountId} to Account {ToAccountId}",
                dto.FromAccountId,
                dto.ToAccountId);

            return Ok("Transfer Successful");
        }

        [HttpGet("statement/{accountId}")]
        public async Task<IActionResult> GetStatement(string accountId)
        {
            _logger.LogInformation("Fetching statement for AccountId {AccountId}", accountId);

            var data = await _service.GetStatement(accountId);

            _logger.LogInformation("Statement fetched successfully for AccountId {AccountId}", accountId);

            return Ok(data);
        }
    }
}