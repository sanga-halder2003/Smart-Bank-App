using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBank.Frontend.Models.Account;
using SmartBank.Frontend.Services;

namespace SmartBank.Frontend.Pages.Accounts;

public class CreateModel : PageModel
{
    private readonly AccountService _service;

    public CreateModel(AccountService service)
    {
        _service = service;
    }

    [BindProperty]

    public CreateAccountDto Account { get; set; }
        = new();

    public async Task<IActionResult> OnPostAsync()
    {
        await _service.CreateAsync(Account);

        return RedirectToPage("Index");
    }
}