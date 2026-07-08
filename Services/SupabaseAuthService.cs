using RecreaLearnAspNet.Models;
using Supabase;
using PostgrestQueryOptions = Supabase.Postgrest.QueryOptions;

namespace RecreaLearnAspNet.Services;

public class SupabaseAuthService
{
    private readonly Supabase.Client _client;
    private readonly Task _initializeTask;

    public SupabaseAuthService(IConfiguration configuration)
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

    public async Task<(bool Success, string Message, string UserId)> RegisterAsync(
        string fullName,
        string email,
        string password,
        string role)
    {
        try
        {
            await _initializeTask;

            var cleanEmail = email.Trim().ToLower();

            var session = await _client.Auth.SignUp(cleanEmail, password);
            var userId = session?.User?.Id;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return (false, "Registration failed.", "");
            }

            var profile = new SupabaseProfile
            {
                Id = userId,
                FullName = fullName,
                Email = cleanEmail,
                Role = "Learner",
                CreatedAt = DateTime.UtcNow
            };

            await _client
                .From<SupabaseProfile>()
                .Upsert(profile, new PostgrestQueryOptions
                {
                    OnConflict = "id",
                    DuplicateResolution = PostgrestQueryOptions.DuplicateResolutionType.MergeDuplicates
                });

            return (true, "Registration successful.", userId);
        }
        catch (Exception ex)
        {
            return (false, ex.Message, "");
        }
    }

    public async Task<(bool Success, string Message, string UserId, string Role)> LoginAsync(
        string email,
        string password)
    {
        try
        {
            await _initializeTask;

            var cleanEmail = email.Trim().ToLower();

            var session = await _client.Auth.SignIn(cleanEmail, password);
            var userId = session?.User?.Id;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return (false, "Invalid email or password.", "", "");
            }

            var profileByEmail = await _client
                .From<SupabaseProfile>()
                .Where(x => x.Email == cleanEmail)
                .Get();

            var profile = profileByEmail.Models.FirstOrDefault();

            if (profile == null)
            {
                var profileById = await _client
                    .From<SupabaseProfile>()
                    .Where(x => x.Id == userId)
                    .Get();

                profile = profileById.Models.FirstOrDefault();
            }

            var role = string.IsNullOrWhiteSpace(profile?.Role)
                ? "Learner"
                : profile.Role.Trim();

            return (true, "Login successful.", userId, role);
        }
        catch (Exception ex)
        {
            return (false, ex.Message, "", "");
        }
    }
}