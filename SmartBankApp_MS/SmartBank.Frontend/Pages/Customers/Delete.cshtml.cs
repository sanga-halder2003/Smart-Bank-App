using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBank.Frontend.Services;

namespace SmartBank.Frontend.Pages.Customers
{
    public class DeleteModel : PageModel
    {
        private readonly CustomerService _customerService;

        public DeleteModel(CustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await _customerService.DeleteCustomerAsync(id);

            return RedirectToPage("Index");
        }
    }
}