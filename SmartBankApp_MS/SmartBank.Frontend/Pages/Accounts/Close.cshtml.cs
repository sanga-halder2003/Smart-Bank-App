using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBank.Frontend.Services;

namespace SmartBank.Frontend.Pages.Accounts;

public class CloseModel : PageModel
{
    private readonly AccountService _service;

    public CloseModel(AccountService service)
    {
        _service = service;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        await _service.CloseAsync(id);

        return RedirectToPage("Index");
    }
}