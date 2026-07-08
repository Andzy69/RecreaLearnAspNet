using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace RecreaLearnAspNet.Models;

[Table("learner_progression")]
public class SupabaseLearnerProgression : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("user_id")]
    public string UserId { get; set; } = "";

    [Column("total_xp")]
    public int TotalXp { get; set; }

    [Column("completed_lessons")]
    public int CompletedLessons { get; set; }

    [Column("eligible_for_instructor")]
    public bool EligibleForInstructor { get; set; }

    [Column("requested_instructor")]
    public bool RequestedInstructor { get; set; }

    [Column("approved_by_admin")]
    public bool ApprovedByAdmin { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Column("completed_courses")]
    public int CompletedCourses { get; set; }

    [Column("participation_points")]
    public int ParticipationPoints { get; set; }

    [Column("application_status")]
    public string ApplicationStatus { get; set; } = "Not Applied";

    [Column("admin_feedback")]
    public string AdminFeedback { get; set; } = "";
}