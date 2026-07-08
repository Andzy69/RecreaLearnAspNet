using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace RecreaLearnAspNet.Models;

[Table("profiles")]
public class SupabaseProfile : BaseModel
{
    [PrimaryKey("id", false)]
    public string Id { get; set; } = "";

    [Column("full_name")]
    public string FullName { get; set; } = "";

    [Column("email")]
    public string Email { get; set; } = "";

    [Column("role")]
    public string Role { get; set; } = "Learner";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}