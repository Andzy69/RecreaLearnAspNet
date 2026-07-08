using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecreaLearnAspNet.Models;
using Supabase;

namespace RecreaLearnAspNet.Pages;

public class UploadCourseModel : PageModel
{
    private readonly Supabase.Client _client;
    private readonly Task _initializeTask;

    public UploadCourseModel(IConfiguration configuration)
    {
        string url = configuration["Supabase:Url"]!;
        string anonKey = configuration["Supabase:AnonKey"]!;

        _client = new Supabase.Client(url, anonKey, new SupabaseOptions
        {
            AutoConnectRealtime = false
        });

        _initializeTask = _client.InitializeAsync();
    }

    [BindProperty] public string Title { get; set; } = "";
    [BindProperty] public string Category { get; set; } = "Arts";
    [BindProperty] public string Level { get; set; } = "Beginner";
    [BindProperty] public string Description { get; set; } = "";
    [BindProperty] public string ImageUrl { get; set; } = "";

    [BindProperty] public string Lesson1Title { get; set; } = "";
    [BindProperty] public string Lesson1VideoUrl { get; set; } = "";

    [BindProperty] public string Lesson2Title { get; set; } = "";
    [BindProperty] public string Lesson2Reading { get; set; } = "";

    [BindProperty] public string Lesson3Title { get; set; } = "";
    [BindProperty] public string Lesson3Activity { get; set; } = "";

    [BindProperty] public string Lesson4Title { get; set; } = "";
    [BindProperty] public string Lesson4Quiz { get; set; } = "";

    [BindProperty] public string Lesson5Title { get; set; } = "";
    [BindProperty] public string Lesson5Scene { get; set; } = "";

    [BindProperty] public string Lesson6Title { get; set; } = "";
    [BindProperty] public string Lesson6Project { get; set; } = "";

    public string Message { get; set; } = "";

    public IActionResult OnGet()
    {
        var role = HttpContext.Session.GetString("UserRole");

        if (role != "Instructor")
        {
            return RedirectToPage("/Login");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var role = HttpContext.Session.GetString("UserRole");

        if (role != "Instructor")
        {
            return RedirectToPage("/Login");
        }

        if (string.IsNullOrWhiteSpace(Title) ||
            string.IsNullOrWhiteSpace(Description) ||
            string.IsNullOrWhiteSpace(ImageUrl))
        {
            Message = "Please fill in the course title, description, and image URL.";
            return Page();
        }

        await _initializeTask;

        var instructorId = HttpContext.Session.GetString("UserId") ?? "";
        var instructorName = HttpContext.Session.GetString("FullName")
                             ?? HttpContext.Session.GetString("Username")
                             ?? "Instructor";

        var submission = new SupabaseCourseSubmission
        {
            InstructorId = instructorId,
            InstructorName = instructorName,
            Title = Title,
            Category = Category,
            Level = Level,
            Description = Description,
            ImageUrl = ImageUrl,
            Status = "Pending",
            AdminFeedback = "Waiting for admin review."
        };

        await _client.From<SupabaseCourseSubmission>().Insert(submission);

        Message = "Course submitted successfully. It is now pending admin approval.";

        return Page();
    }
}