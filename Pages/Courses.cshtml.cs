using Microsoft.AspNetCore.Mvc.RazorPages;
using RecreaLearnAspNet.Models;
using RecreaLearnAspNet.Services;

namespace RecreaLearnAspNet.Pages;

public class CoursesModel : PageModel
{
    private readonly SupabaseCourseService _courseService;

    public CoursesModel(SupabaseCourseService courseService)
    {
        _courseService = courseService;
    }

    public List<SupabaseCourse> Courses { get; set; } = new();

    public string SelectedCategory { get; set; } = "All";

    public string Search { get; set; } = "";

    public string Level { get; set; } = "All";

    public string DatabaseMessage { get; set; } = "";

    public async Task OnGetAsync(string category = "All", string search = "", string level = "All")
    {
        SelectedCategory = string.IsNullOrWhiteSpace(category) ? "All" : category;
        Search = search ?? "";
        Level = string.IsNullOrWhiteSpace(level) ? "All" : level;

        try
        {
            var allCourses = await _courseService.GetCoursesAsync();

            Courses = allCourses
                .Where(MatchesCategory)
                .Where(MatchesSearch)
                .Where(MatchesLevel)
                .ToList();
        }
        catch (Exception ex)
        {
            DatabaseMessage = $"Could not load Supabase courses: {ex.Message}";
            Courses = new();
        }
    }

    public string GetCategoryName(long categoryId) => _courseService.GetCategoryName(categoryId);

    public int GetLessonCount(SupabaseCourse course)
    {
        return course.LessonsCount > 0 ? course.LessonsCount : course.CategoryId switch
        {
            1 => 32,
            2 => 24,
            3 => 28,
            4 => 18,
            5 => 35,
            6 => 20,
            _ => 12
        };
    }

    private bool MatchesCategory(SupabaseCourse course)
    {
        if (SelectedCategory == "All")
        {
            return true;
        }

        return GetCategoryName(course.CategoryId).Equals(SelectedCategory, StringComparison.OrdinalIgnoreCase);
    }

    private bool MatchesSearch(SupabaseCourse course)
    {
        if (string.IsNullOrWhiteSpace(Search))
        {
            return true;
        }

        return course.Title.Contains(Search, StringComparison.OrdinalIgnoreCase)
            || course.Description.Contains(Search, StringComparison.OrdinalIgnoreCase)
            || course.InstructorName.Contains(Search, StringComparison.OrdinalIgnoreCase);
    }

    private bool MatchesLevel(SupabaseCourse course)
    {
        return Level == "All" || course.Level.Equals(Level, StringComparison.OrdinalIgnoreCase);
    }
}
