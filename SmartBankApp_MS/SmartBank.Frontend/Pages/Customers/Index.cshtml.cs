using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBank.Frontend.Models.Customer;
using SmartBank.Frontend.Services;

namespace SmartBank.Frontend.Pages.Customers
{
    public class IndexModel : PageModel
    {
        private readonly CustomerService _customerService;

        public IndexModel(CustomerService customerService)
        {
            _customerService = customerService;
        }

        public List<CustomerModel> Customers { get; set; }
            = new();

        public async Task OnGetAsync()
        {
            Customers =
                await _customerService.GetAllCustomersAsync();
        }
    }
}