using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBank.Frontend.Models.Transaction;
using SmartBank.Frontend.Services;

namespace SmartBank.Frontend.Pages.Transactions;

public class WithdrawModel : PageModel
{
    private readonly TransactionService _service;

    public WithdrawModel(TransactionService service)
    {
        _service = service;
    }

    [BindProperty]

    public WithdrawDto Withdraw { get; set; }
        = new();

    public async Task<IActionResult> OnPostAsync()
    {
        await _service.WithdrawAsync(Withdraw);

        return RedirectToPage("Statement");
    }
}