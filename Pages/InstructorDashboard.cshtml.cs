using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecreaLearnAspNet.Models;
using Supabase;

namespace RecreaLearnAspNet.Pages
{
    public class InstructorDashboardModel : PageModel
    {
        private readonly Supabase.Client _client;
        private readonly Task _initializeTask;

        public List<SupabaseCourse> MyCourses { get; set; } = new();

        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }
        public int PendingCourses { get; set; }
        public decimal AvgRating { get; set; }

        public InstructorDashboardModel(IConfiguration configuration)
        {
            string url = configuration["Supabase:Url"]!;
            string anonKey = configuration["Supabase:AnonKey"]!;

            _client = new Supabase.Client(url, anonKey, new SupabaseOptions
            {
                AutoConnectRealtime = false
            });

            _initializeTask = _client.InitializeAsync();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Instructor")
            {
                return RedirectToPage("/Login");
            }

            await _initializeTask;

            var userId = HttpContext.Session.GetString("UserId");

            var coursesResult = await _client
                .From<SupabaseCourse>()
                .Where(x => x.InstructorId == userId)
                .Get();

            MyCourses = coursesResult.Models;

            TotalCourses = MyCourses.Count;
            PendingCourses = MyCourses.Count(x => x.Status == "Pending");
            TotalStudents = MyCourses.Sum(x => x.StudentsCount);
            AvgRating = MyCourses.Any() ? MyCourses.Average(x => x.Rating) : 0;

            return Page();
        }
    }
}