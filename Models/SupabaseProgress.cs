using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace RecreaLearnAspNet.Models;

[Table("learner_progress")]
public class SupabaseProgress : BaseModel
{
    [PrimaryKey("id", false)]
    public long? Id { get; set; }

    [Column("user_id")]
    public string UserId { get; set; } = "";

    [Column("course_id")]
    public long CourseId { get; set; }

    [Column("lesson_id")]
    public int LessonId { get; set; }

    [Column("is_completed")]
    public bool IsCompleted { get; set; } = true;

    [Column("xp_earned")]
    public int XpEarned { get; set; }

    [Column("completed_at")]
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
}
