using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecreaLearnAspNet.Services;

namespace RecreaLearnAspNet.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly SupabaseAuthService _authService;

        public RegisterModel(SupabaseAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public string FullName { get; set; } = "";

        [BindProperty]
        public string Email { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";

        public string Role { get; set; } = "Learner";

        public string Message { get; set; } = "";

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _authService.RegisterAsync(
                FullName,
                Email,
                Password,
                "Learner"
            );

            if (!result.Success)
            {
                var error = result.Message.ToLower();

                if (error.Contains("already registered") ||
                    error.Contains("user already registered") ||
                    error.Contains("duplicate"))
                {
                    Message = "Email is already registered.";
                }
                else if (error.Contains("password"))
                {
                    Message = "Password is too weak. Use at least 6 characters.";
                }
                else if (error.Contains("invalid") && error.Contains("email"))
                {
                    Message = "Please enter a valid email address.";
                }
                else if (error.Contains("rate limit") ||
                         error.Contains("over_email_send_rate_limit"))
                {
                    Message = "Too many attempts. Please wait a few minutes and try again.";
                }
                else
                {
                    Message = "Registration failed: " + result.Message;
                }

                return Page();
            }

            HttpContext.Session.SetString("UserEmail", Email.Trim().ToLower());
            HttpContext.Session.SetString("UserId", result.UserId);
            HttpContext.Session.SetString("UserRole", "Learner");

            return RedirectToPage("/Index");
        }
    }
}