using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecreaLearnAspNet.Models;
using RecreaLearnAspNet.Services;
using Supabase;

namespace RecreaLearnAspNet.Pages;

public class DashboardModel : PageModel
{
    private readonly SupabaseCourseService _courseService;
    private readonly IConfiguration _configuration;

    public DashboardModel(SupabaseCourseService courseService, IConfiguration configuration)
    {
        _courseService = courseService;
        _configuration = configuration;
    }

    public SupabaseProfile? Profile { get; set; }
    public SupabaseLearnerProgression? InstructorApplication { get; set; }

    public List<SupabaseCourse> Courses { get; set; } = new();
    public List<SupabaseProgress> Progress { get; set; } = new();

    public string DatabaseMessage { get; set; } = "";

    public int CompletedLessons => Progress.Count(x => x.IsCompleted);
    public int TotalXp => Progress.Sum(x => x.XpEarned);

    public int CompletedCourses =>
        Progress.GroupBy(x => x.CourseId).Count(g => g.Count(x => x.IsCompleted) >= 6);

    public int ParticipationPoints => CompletedLessons * 5;

    public string ProgressionRole =>
        TotalXp >= 1500 && CompletedLessons >= 20
            ? "Ready for Instructor Review"
            : "Learner";

    public string NextProgressionRole =>
        TotalXp >= 1500 && CompletedLessons >= 20
            ? "Apply as Instructor"
            : "Instructor Eligibility";

    public int ProgressionPercent
    {
        get
        {
            var xpPercent = Math.Min((TotalXp / 1500.0) * 100, 100);
            var lessonPercent = Math.Min((CompletedLessons / 20.0) * 100, 100);
            return (int)((xpPercent + lessonPercent) / 2);
        }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.Session.GetString("UserId");

        if (string.IsNullOrWhiteSpace(userId))
        {
            return RedirectToPage("/Login", new { returnUrl = HttpContext.Request.Path });
        }

        try
        {
            Profile = await _courseService.GetProfileAsync(userId);
            Progress = await _courseService.GetProgressAsync(userId);
            Courses = await _courseService.GetCoursesAsync();

            var client = await CreateSupabaseClientAsync();

            var applicationResponse = await client
                .From<SupabaseLearnerProgression>()
                .Where(x => x.UserId == userId)
                .Get();

            InstructorApplication = applicationResponse.Models.FirstOrDefault();
        }
        catch (Exception ex)
        {
            DatabaseMessage = $"Could not load Supabase dashboard data: {ex.Message}";
        }

        return Page();
    }

    public async Task<IActionResult> OnPostApplyInstructorAsync()
    {
        var userId = HttpContext.Session.GetString("UserId");

        if (string.IsNullOrWhiteSpace(userId))
        {
            return RedirectToPage("/Login");
        }

        try
        {
            Progress = await _courseService.GetProgressAsync(userId);

            if (TotalXp < 1500 || CompletedLessons < 20)
            {
                TempData["ApplyMessage"] = "You are not eligible yet. Please earn 1500 XP and complete 20 quests first.";
                return RedirectToPage();
            }

            var client = await CreateSupabaseClientAsync();

            var existing = await client
                .From<SupabaseLearnerProgression>()
                .Where(x => x.UserId == userId)
                .Get();

            var record = existing.Models.FirstOrDefault();

            if (record == null)
            {
                record = new SupabaseLearnerProgression
                {
                    UserId = userId,
                    TotalXp = TotalXp,
                    CompletedLessons = CompletedLessons,
                    CompletedCourses = CompletedCourses,
                    ParticipationPoints = ParticipationPoints,
                    EligibleForInstructor = true,
                    RequestedInstructor = true,
                    ApprovedByAdmin = false,
                    ApplicationStatus = "Pending",
                    AdminFeedback = "Your application is waiting for admin review.",
                    UpdatedAt = DateTime.UtcNow
                };

                await client.From<SupabaseLearnerProgression>().Insert(record);
            }
            else
            {
                record.TotalXp = TotalXp;
                record.CompletedLessons = CompletedLessons;
                record.CompletedCourses = CompletedCourses;
                record.ParticipationPoints = ParticipationPoints;
                record.EligibleForInstructor = true;
                record.RequestedInstructor = true;
                record.ApprovedByAdmin = false;
                record.ApplicationStatus = "Pending";
                record.AdminFeedback = "Your application is waiting for admin review.";
                record.UpdatedAt = DateTime.UtcNow;

                await client.From<SupabaseLearnerProgression>().Update(record);
            }

            TempData["ApplyMessage"] = "Your instructor application has been submitted for admin review.";
        }
        catch (Exception ex)
        {
            TempData["ApplyMessage"] = $"Could not submit application: {ex.Message}";
        }

        return RedirectToPage();
    }

    private async Task<Supabase.Client> CreateSupabaseClientAsync()
    {
        string url = _configuration["Supabase:Url"]!;
        string anonKey = _configuration["Supabase:AnonKey"]!;

        var client = new Supabase.Client(url, anonKey, new SupabaseOptions
        {
            AutoConnectRealtime = false
        });

        await client.InitializeAsync();
        return client;
    }

    public string DisplayName()
    {
        if (!string.IsNullOrWhiteSpace(Profile?.FullName))
        {
            return Profile.FullName;
        }

        var email = HttpContext.Session.GetString("UserEmail") ?? "Learner";
        return email.Split('@')[0];
    }

    public string Initials()
    {
        var name = DisplayName();

        if (string.IsNullOrWhiteSpace(name))
        {
            return "RL";
        }

        var parts = name
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Take(2)
            .Select(x => char.ToUpperInvariant(x[0]));

        var initials = string.Concat(parts);

        return string.IsNullOrWhiteSpace(initials) ? "RL" : initials;
    }
}