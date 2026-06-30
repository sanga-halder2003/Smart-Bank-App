using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBank.Frontend.Models.Auth;
using SmartBank.Frontend.Services;

namespace SmartBank.Frontend.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly AuthService _authService;

        public RegisterModel(AuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public RegisterRequest RegisterRequest { get; set; }
            = new();

        public string Message { get; set; } = "";

        public async Task<IActionResult> OnPostAsync()
        {
            var response =
                await _authService.RegisterAsync(RegisterRequest);

            if (response.IsSuccessStatusCode)
            {
                Message = "Registration Successful";

                return RedirectToPage("/Auth/Login");
            }

            Message = "Registration Failed";

            return Page();
        }
    }
}