using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecreaLearnAspNet.Services;

namespace RecreaLearnAspNet.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SupabaseAuthService _authService;

        public LoginModel(SupabaseAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public string Email { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";

        public string Message { get; set; } = "";

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; } = "";

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _authService.LoginAsync(Email, Password);

            if (!result.Success)
            {
                if (result.Message.Contains("invalid_credentials") ||
                    result.Message.Contains("Invalid login credentials"))
                {
                    Message = "Invalid email or password. Please try again.";
                }
                else if (result.Message.Contains("email"))
                {
                    Message = "Please enter a valid email address.";
                }
                else
                {
                    Message = "Login failed. Please try again.";
                }

                return Page();
            }

            var cleanEmail = Email.Trim().ToLower();

            var role = string.IsNullOrWhiteSpace(result.Role)
                ? "Learner"
                : result.Role.Trim();

            HttpContext.Session.SetString("UserEmail", cleanEmail);
            HttpContext.Session.SetString("UserId", result.UserId);
            HttpContext.Session.SetString("UserRole", role);

            if (!string.IsNullOrWhiteSpace(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return LocalRedirect(ReturnUrl);
            }

            if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToPage("/AdminDashboard");
            }

            if (role.Equals("Instructor", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToPage("/InstructorDashboard");
            }

            return RedirectToPage("/Dashboard");
        }
    }
}