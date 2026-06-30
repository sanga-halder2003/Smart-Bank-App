using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBank.Frontend.Models.Transaction;
using SmartBank.Frontend.Services;

namespace SmartBank.Frontend.Pages.Transactions;

public class StatementModel : PageModel
{
    private readonly TransactionService _service;

    public StatementModel(TransactionService service)
    {
        _service = service;
    }

    [BindProperty(SupportsGet = true)]

    public int AccountId { get; set; }

    public List<TransactionModel> Transactions
        = new();

    public async Task OnGetAsync()
    {
        if(AccountId>0)
        {
            Transactions =
                await _service.GetStatementAsync(AccountId);
        }
    }
}