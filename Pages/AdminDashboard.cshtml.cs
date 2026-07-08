using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecreaLearnAspNet.Models;
using Supabase;

namespace RecreaLearnAspNet.Pages
{
    public class AdminDashboardModel : PageModel
    {
        private readonly Supabase.Client _client;
        private readonly Task _initializeTask;

        public int TotalUsers { get; set; }
        public int TotalLearners { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalCourses { get; set; }
        public int PendingSubmissionsCount { get; set; }

        public List<SupabaseProfile> Users { get; set; } = new();
        public List<SupabaseCourse> Courses { get; set; } = new();
        public List<SupabaseCourseSubmission> PendingSubmissions { get; set; } = new();
        public List<SupabaseLearnerProgression> InstructorApplications { get; set; } = new();

        public AdminDashboardModel(IConfiguration configuration)
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

            if (role != "Admin")
            {
                return RedirectToPage("/Login");
            }

            await LoadDataAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostApproveInstructorApplicationAsync(long id, string userId)
        {
            await _initializeTask;

            var appResponse = await _client
                .From<SupabaseLearnerProgression>()
                .Where(x => x.Id == id)
                .Get();

            var application = appResponse.Models.FirstOrDefault();

            if (application != null)
            {
                application.ApprovedByAdmin = true;
                application.RequestedInstructor = false;
                application.ApplicationStatus = "Approved";
                application.AdminFeedback = "Your instructor application has been approved. You can now access Instructor Studio.";
                application.UpdatedAt = DateTime.UtcNow;

                await _client.From<SupabaseLearnerProgression>().Update(application);
            }

            var userResponse = await _client
                .From<SupabaseProfile>()
                .Where(x => x.Id == userId)
                .Get();

            var user = userResponse.Models.FirstOrDefault();

            if (user != null)
            {
                user.Role = "Instructor";
                await _client.From<SupabaseProfile>().Update(user);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDenyInstructorApplicationAsync(long id)
        {
            await _initializeTask;

            var appResponse = await _client
                .From<SupabaseLearnerProgression>()
                .Where(x => x.Id == id)
                .Get();

            var application = appResponse.Models.FirstOrDefault();

            if (application != null)
            {
                application.ApprovedByAdmin = false;
                application.RequestedInstructor = false;
                application.ApplicationStatus = "Denied";
                application.AdminFeedback = "Your instructor application was denied. Please continue completing more courses and improving your participation before applying again.";
                application.UpdatedAt = DateTime.UtcNow;

                await _client.From<SupabaseLearnerProgression>().Update(application);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostApproveSubmissionAsync(long id)
        {
            await _initializeTask;

            var response = await _client
                .From<SupabaseCourseSubmission>()
                .Where(x => x.Id == id)
                .Get();

            var submission = response.Models.FirstOrDefault();

            if (submission != null)
            {
                long categoryId = submission.Category switch
                {
                    "Arts" => 1,
                    "Cooking" => 2,
                    "Fitness" => 3,
                    "Photography" => 4,
                    "Music" => 5,
                    "Language" => 6,
                    _ => 1
                };

                var newCourse = new SupabaseCourse
                {
                    CategoryId = categoryId,
                    Title = submission.Title,
                    Description = submission.Description,
                    ImageUrl = submission.ImageUrl,
                    InstructorName = submission.InstructorName,
                    Level = submission.Level,
                    Rating = 0,
                    StudentsCount = 0,
                    LessonsCount = 6,
                    InstructorId = submission.InstructorId,
                    Status = "Approved",
                    SubmittedAt = DateTime.UtcNow
                };

                await _client.From<SupabaseCourse>().Insert(newCourse);

                submission.Status = "Approved";
                submission.AdminFeedback = "Approved and published by admin.";

                await _client.From<SupabaseCourseSubmission>().Update(submission);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRejectSubmissionAsync(long id)
        {
            await _initializeTask;

            var response = await _client
                .From<SupabaseCourseSubmission>()
                .Where(x => x.Id == id)
                .Get();

            var submission = response.Models.FirstOrDefault();

            if (submission != null)
            {
                submission.Status = "Rejected";
                submission.AdminFeedback = "Rejected by admin. Please improve the course content.";

                await _client.From<SupabaseCourseSubmission>().Update(submission);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostPromoteUserAsync(string userId)
        {
            await _initializeTask;

            var response = await _client
                .From<SupabaseProfile>()
                .Where(x => x.Id == userId)
                .Get();

            var user = response.Models.FirstOrDefault();

            if (user != null)
            {
                user.Role = "Instructor";
                await _client.From<SupabaseProfile>().Update(user);
            }

            return RedirectToPage();
        }

        private async Task LoadDataAsync()
        {
            await _initializeTask;

            var profiles = await _client.From<SupabaseProfile>().Get();
            var courses = await _client.From<SupabaseCourse>().Get();
            var submissions = await _client.From<SupabaseCourseSubmission>().Get();

            var applications = await _client
                .From<SupabaseLearnerProgression>()
                .Get();

            Users = profiles.Models;
            Courses = courses.Models;

            PendingSubmissions = submissions.Models
                .Where(x => x.Status == "Pending")
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            InstructorApplications = applications.Models
                .Where(x => x.RequestedInstructor == true &&
                            x.ApprovedByAdmin == false &&
                            x.ApplicationStatus != "Denied")
                .OrderByDescending(x => x.UpdatedAt)
                .ToList();

            TotalUsers = Users.Count;
            TotalLearners = Users.Count(x => x.Role == "Learner");
            TotalInstructors = Users.Count(x => x.Role == "Instructor");
            TotalCourses = Courses.Count;

            PendingSubmissionsCount = PendingSubmissions.Count + InstructorApplications.Count;
        }
    }
}