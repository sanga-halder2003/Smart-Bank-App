using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBank.Frontend.Models.Customer;
using SmartBank.Frontend.Services;

namespace SmartBank.Frontend.Pages.Customers
{
    public class EditModel : PageModel
    {
        private readonly CustomerService _customerService;

        public EditModel(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [BindProperty]
        public CustomerModel Customer { get; set; } = new();

        public async Task OnGetAsync(int id)
        {
            Customer = await _customerService.GetCustomerByIdAsync(id) ?? new CustomerModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _customerService.UpdateCustomerAsync(Customer);

            return RedirectToPage("Index");
        }
    }
}