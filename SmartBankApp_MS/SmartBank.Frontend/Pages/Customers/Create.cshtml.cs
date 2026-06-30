using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBank.Frontend.Models.Customer;
using SmartBank.Frontend.Services;

namespace SmartBank.Frontend.Pages.Customers
{
    public class CreateModel : PageModel
    {
        private readonly CustomerService _customerService;

        public CreateModel(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [BindProperty]
        public CustomerModel Customer { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _customerService.CreateCustomerAsync(Customer);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}