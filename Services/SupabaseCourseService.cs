using RecreaLearnAspNet.Models;
using Supabase;
using PostgrestQueryOptions = Supabase.Postgrest.QueryOptions;

namespace RecreaLearnAspNet.Services;

public class SupabaseCourseService
{
    private readonly Supabase.Client _client;
    private readonly Task _initializeTask;

    public SupabaseCourseService(IConfiguration configuration)
    {
        string url = configuration["Supabase:Url"]
            ?? throw new InvalidOperationException("Supabase:Url is missing.");
        string anonKey = configuration["Supabase:AnonKey"]
            ?? throw new InvalidOperationException("Supabase:AnonKey is missing.");

        _client = new Supabase.Client(url, anonKey, new SupabaseOptions
        {
            AutoConnectRealtime = false
        });

        _initializeTask = _client.InitializeAsync();
    }

    public async Task<List<SupabaseCategory>> GetCategoriesAsync()
    {
        await _initializeTask;

        var response = await _client
            .From<SupabaseCategory>()
            .Order(x => x.Id, Supabase.Postgrest.Constants.Ordering.Ascending)
            .Get();

        return response.Models;
    }

    public async Task<List<SupabaseCourse>> GetCoursesAsync()
    {
        await _initializeTask;

        var response = await _client
            .From<SupabaseCourse>()
            .Order(x => x.Id, Supabase.Postgrest.Constants.Ordering.Ascending)
            .Get();

        return response.Models;
    }

    public async Task<SupabaseCourse?> GetCourseByIdAsync(long courseId)
    {
        await _initializeTask;

        var response = await _client
            .From<SupabaseCourse>()
            .Where(x => x.Id == courseId)
            .Get();

        return response.Models.FirstOrDefault();
    }

    public async Task<List<SupabaseLesson>> GetLessonsByCourseAsync(long courseId)
    {
        await _initializeTask;

        var response = await _client
            .From<SupabaseLesson>()
            .Where(x => x.CourseId == courseId)
            .Order(x => x.LessonOrder, Supabase.Postgrest.Constants.Ordering.Ascending)
            .Get();

        return response.Models;
    }

    public async Task<SupabaseProfile?> GetProfileAsync(string userId)
    {
        await _initializeTask;

        var response = await _client
            .From<SupabaseProfile>()
            .Where(x => x.Id == userId)
            .Get();

        return response.Models.FirstOrDefault();
    }

    public async Task<List<SupabaseProgress>> GetProgressAsync(string userId)
    {
        await _initializeTask;

        var response = await _client
            .From<SupabaseProgress>()
            .Where(x => x.UserId == userId)
            .Order(x => x.CompletedAt, Supabase.Postgrest.Constants.Ordering.Descending)
            .Get();

        return response.Models;
    }

    public async Task<List<SupabaseProgress>> GetProgressByCourseAsync(string userId, long courseId)
    {
        await _initializeTask;

        var response = await _client
            .From<SupabaseProgress>()
            .Where(x => x.UserId == userId)
            .Where(x => x.CourseId == courseId)
            .Order(x => x.CompletedAt, Supabase.Postgrest.Constants.Ordering.Descending)
            .Get();

        return response.Models;
    }

    public async Task SaveLessonProgressAsync(string userId, long courseId, SupabaseLesson lesson)
    {
        await _initializeTask;

        var progress = new SupabaseProgress
        {
            Id = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            UserId = userId,
            CourseId = courseId,
            LessonId = lesson.Id,
            IsCompleted = true,
            XpEarned = lesson.XpReward,
            CompletedAt = DateTime.UtcNow
        };

        await _client
            .From<SupabaseProgress>()
            .Upsert(progress, new PostgrestQueryOptions
            {
                OnConflict = "user_id,lesson_id",
                DuplicateResolution = PostgrestQueryOptions.DuplicateResolutionType.MergeDuplicates
            });
    }
    public string GetCategoryName(long categoryId)
    {
        return categoryId switch
        {
            1 => "Language",
            2 => "Photography",
            3 => "Cooking",
            4 => "Fitness",
            5 => "Arts",
            6 => "Music",
            _ => "Course"
        };
    }

    public string GetCategorySlug(long categoryId)
    {
        return GetCategoryName(categoryId).ToLowerInvariant().Replace(" ", "-");
    }
}
