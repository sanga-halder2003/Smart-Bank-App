using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBank.Frontend.Models.Auth;
using SmartBank.Frontend.Services;

namespace SmartBank.Frontend.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly AuthService _authService;

        public LoginModel(AuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public LoginRequest LoginRequest { get; set; } = new();

        public string Message { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _authService.LoginAsync(LoginRequest);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Dashboard/Dashboard");
            }

            Message = "Invalid Email or Password";

            return Page();
        }
    }
}
