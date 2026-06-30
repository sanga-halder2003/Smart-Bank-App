using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBank.Frontend.Models.Transaction;
using SmartBank.Frontend.Services;

namespace SmartBank.Frontend.Pages.Transactions;

public class DepositModel : PageModel
{
    private readonly TransactionService _service;

    public DepositModel(TransactionService service)
    {
        _service = service;
    }

    [BindProperty]

    public DepositDto Deposit { get; set; }
        = new();

    public async Task<IActionResult> OnPostAsync()
    {
        await _service.DepositAsync(Deposit);

        return RedirectToPage("Statement");
    }
}