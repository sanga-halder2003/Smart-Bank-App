using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBank.Frontend.Models.Transaction;
using SmartBank.Frontend.Services;

namespace SmartBank.Frontend.Pages.Transactions;

public class TransferModel : PageModel
{
    private readonly TransactionService _service;

    public TransferModel(TransactionService service)
    {
        _service = service;
    }

    [BindProperty]

    public TransferDto Transfer { get; set; }
        = new();

    public async Task<IActionResult> OnPostAsync()
    {
        await _service.TransferAsync(Transfer);

        return RedirectToPage("Statement");
    }
}