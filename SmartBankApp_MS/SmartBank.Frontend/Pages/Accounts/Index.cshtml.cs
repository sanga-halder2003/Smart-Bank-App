using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBank.Frontend.Models.Account;
using SmartBank.Frontend.Services;

namespace SmartBank.Frontend.Pages.Accounts;

public class IndexModel : PageModel
{
    private readonly AccountService _service;

    public IndexModel(AccountService service)
    {
        _service = service;
    }

    public List<AccountModel> Accounts { get; set; } = new();

    public async Task OnGetAsync()
    {
        Accounts = await _service.GetAllAsync();
    }
}